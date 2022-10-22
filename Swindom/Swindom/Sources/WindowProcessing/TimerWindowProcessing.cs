namespace Swindom;

/// <summary>
/// タイマーでウィンドウ処理
/// </summary>
public class TimerWindowProcessing : IDisposable
{
    /// <summary>
    /// Disposeが呼ばれたかの値
    /// </summary>
    private bool Disposed;
    /// <summary>
    /// 処理のタイマー
    /// </summary>
    private readonly System.Windows.Threading.DispatcherTimer ProcessingTimer = new();
    /// <summary>
    /// マウスでウィンドウ移動検知
    /// </summary>
    private readonly FreeEcho.FEWindowMoveDetectionMouse.MouseMoveWindowDetection MouseMoveWindowDetection = new();
    /// <summary>
    /// ウィンドウ情報のバッファ
    /// </summary>
    private readonly WindowInformationBuffer WindowInformationBuffer = new();

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public TimerWindowProcessing()
    {
        ProcessingSettings();
        ProcessingTimer.Interval = new(0, 0, 0, 0, Common.ApplicationData.Settings.TimerInformation.ProcessingInterval);
        ProcessingTimer.Tick += ProcessingTimer_Tick;
        MouseMoveWindowDetection.StartMove += MouseMoveWindowDetection_StartMove;
        MouseMoveWindowDetection.StopMove += MouseMoveWindowDetection_StopMove;
        Common.ApplicationData.ProcessingEvent += ApplicationData_ProcessingEvent;
        ProcessingTimer.Start();
        MouseMoveWindowDetection.Start();
    }

    /// <summary>
    /// デストラクタ
    /// </summary>
    ~TimerWindowProcessing()
    {
        Dispose(false);
    }

    /// <summary>
    /// Dispose
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// 非公開Dispose
    /// </summary>
    /// <param name="disposing"></param>
    protected virtual void Dispose(
        bool disposing
        )
    {
        if (Disposed == false)
        {
            Disposed = true;
            Common.ApplicationData.ProcessingEvent -= ApplicationData_ProcessingEvent;
            ProcessingTimer.Stop();
            MouseMoveWindowDetection.Stop();
            UnregisterHotkeys();
        }
    }

    /// <summary>
    /// 処理が有効か確認
    /// </summary>
    /// <returns>処理が有効かの値 (無効「false」/有効「true」)</returns>
    public static bool CheckIfTheProcessingIsValid() => Common.ApplicationData.Settings.TimerInformation.Enabled
            && (Common.ApplicationData.Settings.TimerInformation.StopProcessingFullScreen && Common.ApplicationData.FullScreenExists ? Common.ApplicationData.Settings.TimerInformation.HotkeysDoNotStopFullScreen : true);

    /// <summary>
    /// 「処理」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ApplicationData_ProcessingEvent(
        object? sender,
        ProcessingEventArgs e
        )
    {
        try
        {
            switch (e.ProcessingEventType)
            {
                case ProcessingEventType.TimerProcessingInterval:
                    ProcessingTimer.Interval = new(0, 0, 0, 0, Common.ApplicationData.Settings.TimerInformation.ProcessingInterval);
                    break;
                case ProcessingEventType.TimerPause:
                    ProcessingTimer?.Stop();
                    MouseMoveWindowDetection.Stop();
                    break;
                case ProcessingEventType.TimerUnpause:
                    ProcessingTimer?.Start();
                    MouseMoveWindowDetection.Start();
                    break;
            }
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「マウスでウィンドウ移動検知」の「StartMove」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void MouseMoveWindowDetection_StartMove(
        object sender,
        FreeEcho.FEWindowMoveDetectionMouse.StartMoveEventArgs e
        )
    {
        try
        {
            ProcessingTimer.Stop();
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「マウスでウィンドウ移動検知」の「StopMove」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void MouseMoveWindowDetection_StopMove(
        object sender,
        FreeEcho.FEWindowMoveDetectionMouse.StopMoveEventArgs e
        )
    {
        try
        {
            ProcessingTimer.Start();
        }
        catch
        {
        }
    }

    /// <summary>
    /// 処理設定
    /// </summary>
    public void ProcessingSettings()
    {
        ProcessingTimer.Interval = new(0, 0, 0, 0, Common.ApplicationData.Settings.TimerInformation.ProcessingInterval);
        RegisterHotkeys();
    }

    /// <summary>
    /// 「処理のタイマー」の「Tick」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ProcessingTimer_Tick(
        object? sender,
        EventArgs e
        )
    {
        try
        {
            // ウィンドウが閉じている場合は「1度だけ処理が終わっている」を解除
            foreach (TimerItemInformation nowTII in Common.ApplicationData.Settings.TimerInformation.Items)
            {
                if (nowTII.Hwnd != IntPtr.Zero && NativeMethods.IsWindow(nowTII.Hwnd) == false)
                {
                    if (nowTII.ProcessingOnlyOnce == ProcessingOnlyOnce.WindowOpen)
                    {
                        nowTII.EndedProcessingOnlyOnce = false;
                    }
                    nowTII.Hwnd = IntPtr.Zero;
                    nowTII.CountNumberOfTimesNotToProcessingFirst = 0;
                }
            }

            DecisionAndWindowProcessing();
        }
        catch
        {
        }
    }

    /// <summary>
    /// 判定してウィンドウ処理 (タイマー)
    /// </summary>
    private void DecisionAndWindowProcessing()
    {
        try
        {
            ProcessingTimer?.Stop();

            HwndList hwndList;        // ウィンドウハンドルのリスト
            bool activeWindowOnly = true;     // アクティブウィンドウのみ処理するかの値
            switch (Common.ApplicationData.Settings.TimerInformation.ProcessingWindowRange)
            {
                case ProcessingWindowRange.ActiveOnly:
                    hwndList = new();
                    hwndList.Hwnd.Add(NativeMethods.GetForegroundWindow());
                    break;
                default:
                    hwndList = HwndList.GetWindowHandleList();
                    activeWindowOnly = false;
                    break;
            }

            if (Common.ApplicationData.Settings.TimerInformation.StopProcessingFullScreen && WindowProcessing.CheckFullScreenWindow(activeWindowOnly ? null : hwndList))
            {
                return;
            }

            int waitTime = 0;     // 待ち時間
            foreach (IntPtr nowHwnd in hwndList.Hwnd)
            {
                WINDOWPLACEMENT windowPlacement;       // WINDOWPLACEMENT
                NativeMethods.GetWindowPlacement(nowHwnd, out windowPlacement);
                WindowInformation windowInformation = WindowProcessing.GetWindowInformation(nowHwnd, WindowInformationBuffer);       // ウィンドウの情報

                foreach (TimerItemInformation nowTII in Common.ApplicationData.Settings.TimerInformation.Items)
                {
                    if (nowTII.Enabled && nowTII.EndedProcessingOnlyOnce == false
                        && SpecifiedWindow.CheckWindowToProcessing(Common.ApplicationData.Settings.TimerInformation.CaseSensitiveWindowQueries, nowTII, windowInformation, in windowPlacement))
                    {
                        if (nowTII.CountNumberOfTimesNotToProcessingFirst < nowTII.NumberOfTimesNotToProcessingFirst)
                        {
                            if (nowTII.Hwnd == IntPtr.Zero)
                            {
                                nowTII.Hwnd = nowHwnd;
                            }
                            if (nowTII.Hwnd == nowHwnd)
                            {
                                nowTII.CountNumberOfTimesNotToProcessingFirst++;
                            }
                        }
                        else
                        {
                            if (nowTII.CloseWindow)
                            {
                                if (waitTime != 0)
                                {
                                    WaitNextWindowProcessing();
                                }
                                else
                                {
                                    waitTime = Common.ApplicationData.Settings.TimerInformation.WaitTimeToProcessingNextWindow;
                                }
                                if (nowTII.ProcessingOnlyOnce != ProcessingOnlyOnce.NotSpecified)
                                {
                                    nowTII.EndedProcessingOnlyOnce = true;
                                }
                                nowTII.Hwnd = nowHwnd;
                                NativeMethods.PostMessage(nowHwnd, (uint)WM.WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
                            }
                            else
                            {
                                foreach (WindowProcessingInformation nowWPI in nowTII.WindowProcessingInformation)
                                {
                                    if (nowWPI.Active
                                        && (nowWPI.OnlyNormalWindow == false || windowPlacement.showCmd == (int)SW.SW_SHOWNORMAL)
                                        && SpecifiedWindow.CheckDisplayToProcessing(nowTII, nowWPI, nowHwnd))
                                    {
                                        if (waitTime == 0)
                                        {
                                            waitTime = Common.ApplicationData.Settings.TimerInformation.WaitTimeToProcessingNextWindow;
                                        }
                                        else
                                        {
                                            WaitNextWindowProcessing();
                                        }
                                        if (nowTII.ProcessingOnlyOnce != ProcessingOnlyOnce.NotSpecified)
                                        {
                                            nowTII.EndedProcessingOnlyOnce = true;
                                        }
                                        nowTII.Hwnd = nowHwnd;
                                        SpecifiedWindow.ProcessingWindow(nowTII, nowWPI, nowHwnd, in windowPlacement, false, Common.ApplicationData.Settings.TimerInformation.DoNotChangeOutOfScreen);
                                        break;
                                    }
                                }
                            }
                        }
                        break;
                    }
                }
            }
        }
        finally
        {
            try
            {
                ProcessingTimer?.Start();
            }
            catch
            {
            }
        }
    }

    /// <summary>
    /// 判定してウィンドウ処理 (一括実行、アクティブウィンドウのみ)
    /// </summary>
    /// <param name="processingType">指定ウィンドウの処理の種類 (一括実行「false」/アクティブウィンドウのみ「true」)</param>
    public static void DecisionAndWindowProcessing(
        bool processingType
        )
    {
        try
        {
            HwndList hwndList;
            if (processingType)
            {
                hwndList = new();
                hwndList.Hwnd.Add(NativeMethods.GetForegroundWindow());
            }
            else
            {
                hwndList = HwndList.GetWindowHandleList();
            }

            WindowInformationBuffer windowInformationBuffer = new();
            foreach (IntPtr nowHwnd in hwndList.Hwnd)
            {
                WINDOWPLACEMENT windowPlacement;
                NativeMethods.GetWindowPlacement(nowHwnd, out windowPlacement);
                WindowInformation windowInformation = WindowProcessing.GetWindowInformation(nowHwnd, windowInformationBuffer);

                foreach (TimerItemInformation nowTII in Common.ApplicationData.Settings.TimerInformation.Items)
                {
                    if (SpecifiedWindow.CheckWindowToProcessing(Common.ApplicationData.Settings.TimerInformation.CaseSensitiveWindowQueries, nowTII, windowInformation, in windowPlacement))
                    {
                        if (nowTII.CloseWindow)
                        {
                            NativeMethods.PostMessage(nowHwnd, (uint)WM.WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
                        }
                        else
                        {
                            foreach (WindowProcessingInformation nowWPI in nowTII.WindowProcessingInformation)
                            {
                                if (nowWPI.Active
                                    && (nowWPI.OnlyNormalWindow == false || windowPlacement.showCmd == (int)SW.SW_SHOWNORMAL)
                                    && SpecifiedWindow.CheckDisplayToProcessing(nowTII, nowWPI, nowHwnd))
                                {
                                    SpecifiedWindow.ProcessingWindow(nowTII, nowWPI, nowHwnd, in windowPlacement, false, Common.ApplicationData.Settings.TimerInformation.DoNotChangeOutOfScreen);
                                    break;
                                }
                            }
                        }
                        break;
                    }
                }
            }
        }
        catch
        {
        }
    }

    /// <summary>
    /// 次のウィンドウ処理を待つ
    /// </summary>
    private void WaitNextWindowProcessing()
    {
        try
        {
            using System.Threading.Tasks.Task task = System.Threading.Tasks.Task.Run(async () =>
            {
                await System.Threading.Tasks.Task.Delay(Common.ApplicationData.Settings.TimerInformation.WaitTimeToProcessingNextWindow);
            });
            task.Wait();
        }
        catch
        {
        }
    }

    /// <summary>
    /// ホットキーを登録
    /// </summary>
    public void RegisterHotkeys()
    {
        UnregisterHotkeys();
        if (CheckIfTheProcessingIsValid())
        {
            int id = Common.TimerHotkeysStartId;       // ホットキーの識別子
            foreach (TimerItemInformation nowTII in Common.ApplicationData.Settings.TimerInformation.Items)
            {
                foreach (WindowProcessingInformation nowWPI in nowTII.WindowProcessingInformation)
                {
                    if (Processing.RegisterHotkey(Common.ApplicationData.SystemTrayIconHwnd, nowWPI.Hotkey, id))
                    {
                        nowWPI.HotkeyId = id;
                        id++;
                    }
                }
            }
        }
    }

    /// <summary>
    /// ホットキーを解除
    /// </summary>
    public void UnregisterHotkeys()
    {
        foreach (TimerItemInformation nowTII in Common.ApplicationData.Settings.TimerInformation.Items)
        {
            foreach (WindowProcessingInformation nowWPI in nowTII.WindowProcessingInformation)
            {
                if (nowWPI.HotkeyId != -1)
                {
                    NativeMethods.UnregisterHotKey(Common.ApplicationData.SystemTrayIconHwnd, nowWPI.HotkeyId);
                    nowWPI.HotkeyId = -1;
                }
            }
        }
    }

    /// <summary>
    /// ホットキーを処理
    /// </summary>
    /// <param name="id">ホットキーの識別子</param>
    public void ProcessingHotkeys(
        int id
        )
    {
        try
        {
            HwndList hwndList = HwndList.GetWindowHandleList();

            foreach (IntPtr nowHwnd in hwndList.Hwnd)
            {
                WINDOWPLACEMENT windowPlacement;
                NativeMethods.GetWindowPlacement(nowHwnd, out windowPlacement);
                WindowInformation windowInformation = WindowProcessing.GetWindowInformation(nowHwnd, WindowInformationBuffer);

                foreach (TimerItemInformation nowTII in Common.ApplicationData.Settings.TimerInformation.Items)
                {
                    if (SpecifiedWindow.CheckWindowToProcessing(Common.ApplicationData.Settings.TimerInformation.CaseSensitiveWindowQueries, nowTII, windowInformation, in windowPlacement))
                    {
                        foreach (WindowProcessingInformation nowWPI in nowTII.WindowProcessingInformation)
                        {
                            if (nowWPI.HotkeyId == id && (nowTII.StandardDisplay == StandardDisplay.OnlySpecifiedDisplay ? SpecifiedWindow.CheckDisplayToProcessing(nowTII, nowWPI, nowHwnd) : true))
                            {
                                SpecifiedWindow.ProcessingWindow(nowTII, nowWPI, nowHwnd, in windowPlacement, true, Common.ApplicationData.Settings.TimerInformation.DoNotChangeOutOfScreen);
                            }
                        }
                    }
                }
            }
        }
        catch
        {
        }
    }
}
