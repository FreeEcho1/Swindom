namespace Swindom.Sources.WindowProcessing;

/// <summary>
/// 「指定ウィンドウ」処理
/// </summary>
public class SpecifyWindowProcessing : IDisposable
{
    /// <summary>
    /// Disposed
    /// </summary>
    private bool Disposed;
    /// <summary>
    /// 修正中の項目のインデックス
    /// </summary>
    private static int IndexOfItemBeingProcessed = -1;
    /// <summary>
    /// 「処理しない条件」の「指定したバージョン以外」のメッセージを表示するかの値 (いいえ「false」/はい「true」)
    /// </summary>
    private static bool CheckShowMessageVersion = false;

    /// <summary>
    /// マウスでウィンドウ移動検知
    /// </summary>
    private FreeEcho.FEWindowMoveDetectionMouse.MouseMoveWindowDetection? MouseMoveWindowDetection;
    /// <summary>
    /// ウィンドウイベント
    /// </summary>
    private FreeEcho.FEWindowEvent.WindowEvent? WindowEvent;
    /// <summary>
    /// 処理のタイマー
    /// </summary>
    private System.Windows.Threading.DispatcherTimer? ProcessingTimer;
    /// <summary>
    /// 処理が一時停止しているかの値 (停止していない「false」/停止している「true」)
    /// </summary>
    private bool PauseProcessing;
    /// <summary>
    /// ウィンドウ情報のバッファ
    /// </summary>
    private readonly WindowInformationBuffer WindowInformationBuffer = new();
    /// <summary>
    /// ホットキーを処理するかの値 (いいえ「false」/はい「true」)
    /// </summary>
    private bool DoHotkeyProcessing = true;
    /// <summary>
    /// 「指定ウィンドウ」の「追加/修正」ウィンドウ
    /// </summary>
    private static SpecifyWindowItemWindow? SpecifyWindowItemWindow;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public SpecifyWindowProcessing()
    {
        ProcessingSettings();
        ComponentDispatcher.ThreadPreprocessMessage += ComponentDispatcher_ThreadPreprocessMessage;
        ApplicationData.EventData.ProcessingEvent += ApplicationData_ProcessingEvent;
    }

    /// <summary>
    /// デストラクタ
    /// </summary>
    ~SpecifyWindowProcessing()
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
    /// <param name="disposing">disposing</param>
    protected virtual void Dispose(
        bool disposing
        )
    {
        if (Disposed)
        {
            return;
        }
        if (disposing)
        {
            ComponentDispatcher.ThreadPreprocessMessage -= ComponentDispatcher_ThreadPreprocessMessage;
            ApplicationData.EventData.ProcessingEvent -= ApplicationData_ProcessingEvent;
            DisposeWindowEvent();
            DisposeProcessingTimer();
            DisposeMouseMoveWindowDetection();
            UnregisterHotkeys();
            SpecifyWindowItemWindow?.Close();
            SpecifyWindowItemWindow = null;
        }
        Disposed = true;
    }

    /// <summary>
    /// ウィンドウイベントを破棄
    /// </summary>
    private void DisposeWindowEvent()
    {
        if (WindowEvent != null)
        {
            WindowEvent.Dispose();
            WindowEvent = null;
        }
    }

    /// <summary>
    /// 処理のタイマーを破棄
    /// </summary>
    private void DisposeProcessingTimer()
    {
        if (ProcessingTimer != null)
        {
            ProcessingTimer?.Stop();
            ProcessingTimer = null;
        }
    }

    /// <summary>
    /// マウスでウィンドウ移動検知を破棄
    /// </summary>
    private void DisposeMouseMoveWindowDetection()
    {
        if (MouseMoveWindowDetection != null)
        {
            MouseMoveWindowDetection.Stop();
            MouseMoveWindowDetection = null;
        }
    }

    /// <summary>
    /// 処理が有効か確認
    /// </summary>
    /// <returns>処理が有効かの値 (無効「false」/有効「true」)</returns>
    public static bool CheckIfTheProcessingIsValid() => (ApplicationData.Settings.SpecifyWindowInformation.Enabled
            && ((ApplicationData.Settings.SpecifyWindowInformation.StopProcessingFullScreen && ApplicationData.FullScreenExists) ? ApplicationData.Settings.SpecifyWindowInformation.HotkeysDoNotStopFullScreen : true));

    /// <summary>
    /// 処理設定
    /// </summary>
    public void ProcessingSettings()
    {
        if (ApplicationData.Settings.SpecifyWindowInformation.Enabled
            && (ApplicationData.Settings.SpecifyWindowInformation.StopProcessingFullScreen == false || ApplicationData.FullScreenExists == false))
        {
            if (WindowEvent == null)
            {
                WindowEvent = new();
                WindowEvent.WindowEventOccurrence += WindowEvent_WindowEventOccurrence;
            }
            else
            {
                WindowEvent.Unhook();
            }
            FreeEcho.FEWindowEvent.HookWindowEventType type = 0;
            // 「1度だけ処理が終わっている」を終わってないにするために「Destroy」イベントを受け取れるようにする
            foreach (SpecifyWindowItemInformation nowItem in ApplicationData.Settings.SpecifyWindowInformation.Items)
            {
                if (nowItem.ProcessingOnlyOnce == ProcessingOnlyOnce.WindowOpen)
                {
                    type |= FreeEcho.FEWindowEvent.HookWindowEventType.Destroy;
                    break;
                }
            }
            foreach (SpecifyWindowItemInformation nowItem in ApplicationData.Settings.SpecifyWindowInformation.Items)
            {
                if (nowItem.WindowEventData.Foreground)
                {
                    type |= FreeEcho.FEWindowEvent.HookWindowEventType.Foreground;
                }
                if (nowItem.WindowEventData.MoveSizeEnd)
                {
                    type |= FreeEcho.FEWindowEvent.HookWindowEventType.MoveSizeEnd;
                }
                if (nowItem.WindowEventData.MinimizeStart)
                {
                    type |= FreeEcho.FEWindowEvent.HookWindowEventType.MinimizeStart;
                }
                if (nowItem.WindowEventData.MinimizeEnd)
                {
                    type |= FreeEcho.FEWindowEvent.HookWindowEventType.MinimizeEnd;
                }
                if (nowItem.WindowEventData.Create)
                {
                    type |= FreeEcho.FEWindowEvent.HookWindowEventType.Create;
                }
                if (nowItem.WindowEventData.Show)
                {
                    type |= FreeEcho.FEWindowEvent.HookWindowEventType.Show;
                }
                if (nowItem.WindowEventData.NameChange)
                {
                    type |= FreeEcho.FEWindowEvent.HookWindowEventType.NameChange;
                }
            }
            if (type != 0)
            {
                WindowEvent.Hook(type);
            }

            bool checkTimerProcessing = false;      // タイマー処理が有効かの値
            foreach (SpecifyWindowItemInformation nowItem in ApplicationData.Settings.SpecifyWindowInformation.Items)
            {
                if (nowItem.TimerProcessing)
                {
                    checkTimerProcessing = true;
                    break;
                }
            }
            if (checkTimerProcessing)
            {
                if (ProcessingTimer == null)
                {
                    ProcessingTimer = new()
                    {
                        Interval = new(0, 0, 0, 0, ApplicationData.Settings.SpecifyWindowInformation.ProcessingInterval)
                    };
                    ProcessingTimer.Tick += ProcessingTimer_Tick;
                }
                else
                {
                    ProcessingTimer.Interval = new(0, 0, 0, 0, ApplicationData.Settings.SpecifyWindowInformation.ProcessingInterval);
                }
                ProcessingTimer?.Start();
            }
            else
            {
                DisposeProcessingTimer();
            }

            if (MouseMoveWindowDetection == null)
            {
                MouseMoveWindowDetection = new();
                MouseMoveWindowDetection.StartMove += MouseMoveWindowDetection_StartMove;
                MouseMoveWindowDetection.StopMove += MouseMoveWindowDetection_StopMove;
            }
            MouseMoveWindowDetection.Start();
        }
        else
        {
            DisposeWindowEvent();
            DisposeProcessingTimer();
            DisposeMouseMoveWindowDetection();
        }
        RegisterHotkeys();
    }

    /// <summary>
    /// 「ThreadPreprocessMessage」メッセージ (ホットキー処理)
    /// </summary>
    /// <param name="msg"></param>
    /// <param name="handled"></param>
    private void ComponentDispatcher_ThreadPreprocessMessage(
        ref MSG msg,
        ref bool handled
        )
    {
        try
        {
            if (msg.message == (int)WM.WM_HOTKEY && DoHotkeyProcessing)
            {
                ProcessingHotkeys((int)msg.wParam);
            }
        }
        catch
        {
        }
    }

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
                case ProcessingEventType.SpecifyWindowPause:
                    PauseProcessing = true;
                    ProcessingTimer?.Stop();
                    MouseMoveWindowDetection?.Stop();
                    break;
                case ProcessingEventType.SpecifyWindowUnpause:
                    PauseProcessing = false;
                    ProcessingTimer?.Start();
                    MouseMoveWindowDetection?.Start();
                    break;
                case ProcessingEventType.TimerProcessingInterval:
                    if (ProcessingTimer != null)
                    {
                        ProcessingTimer.Interval = new(0, 0, 0, 0, ApplicationData.Settings.SpecifyWindowInformation.ProcessingInterval);
                    }
                    break;
                case ProcessingEventType.PauseHotkeyProcessing:
                    DoHotkeyProcessing = false;
                    break;
                case ProcessingEventType.UnpauseHotkeyProcessing:
                    DoHotkeyProcessing = true;
                    break;
                case ProcessingEventType.CloseSpecifyWindowItemWindow:
                    if (IndexOfItemBeingProcessed != -1)
                    {
                        ApplicationData.Settings.SpecifyWindowInformation.Items[IndexOfItemBeingProcessed].Enabled = true;
                        IndexOfItemBeingProcessed = -1;
                        ApplicationData.EventData.DoProcessingEvent(ProcessingEventType.UpdateListBoxSpecifyWindow);
                    }
                    break;
            }
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「ウィンドウイベント」の「WindowEventOccurrence」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void WindowEvent_WindowEventOccurrence(
        object? sender,
        FreeEcho.FEWindowEvent.WindowEventArgs e
        )
    {
        try
        {
            // ウィンドウが閉じている場合は「1度だけ処理が終わっている」を解除
            if (e.WindowEventType == FreeEcho.FEWindowEvent.WindowEventType.Destroy)
            {
                foreach (SpecifyWindowItemInformation nowEII in ApplicationData.Settings.SpecifyWindowInformation.Items)
                {
                    if (nowEII.Hwnd == e.Hwnd && nowEII.ProcessingOnlyOnce == ProcessingOnlyOnce.WindowOpen)
                    {
                        nowEII.EndedProcessingOnlyOnce = false;
                        nowEII.Hwnd = IntPtr.Zero;
                        break;
                    }
                }
                return;
            }

            if (PauseProcessing == false)
            {
                DecisionAndWindowProcessing(e);
            }
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「処理のタイマー」の「Tick」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <exception cref="NotImplementedException"></exception>
    private void ProcessingTimer_Tick(
        object? sender,
        EventArgs e
        )
    {
        try
        {
            if (PauseProcessing == false)
            {
                DecisionAndWindowProcessing();
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
            PauseProcessing = true;
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
            PauseProcessing = false;
        }
        catch
        {
        }
    }

    /// <summary>
    /// 判定してウィンドウ処理 (イベント)
    /// </summary>
    /// <param name="e">WindowEventArgs</param>
    private void DecisionAndWindowProcessing(
        FreeEcho.FEWindowEvent.WindowEventArgs e
        )
    {
        try
        {
            if (ApplicationData.Settings.SpecifyWindowInformation.StopProcessingFullScreen && VariousWindowProcessing.CheckFullScreenWindow(null))
            {
                return;
            }

            //IntPtr hwnd = NativeMethods.GetAncestor(e.Hwnd, GetAncestorFlags.GetRootOwner);
            IntPtr hwnd = e.Hwnd;
            //if (hwnd == IntPtr.Zero)
            //{
            //    return;
            //}

            WindowInformation windowInformation = VariousWindowProcessing.GetWindowInformationFromHandle(hwnd, WindowInformationBuffer);

            foreach (SpecifyWindowItemInformation nowEII in ApplicationData.Settings.SpecifyWindowInformation.Items)
            {
                if (nowEII.Enabled
                    && (nowEII.ProcessingOnlyOnce == ProcessingOnlyOnce.NotSpecified || nowEII.EndedProcessingOnlyOnce == false))
                {
                    bool checkEvent = false;       // イベントの有効確認

                    checkEvent = e.WindowEventType switch
                    {
                        FreeEcho.FEWindowEvent.WindowEventType.Foreground => nowEII.WindowEventData.Foreground,
                        FreeEcho.FEWindowEvent.WindowEventType.MoveSizeEnd => nowEII.WindowEventData.MoveSizeEnd,
                        FreeEcho.FEWindowEvent.WindowEventType.MinimizeStart => nowEII.WindowEventData.MinimizeStart,
                        FreeEcho.FEWindowEvent.WindowEventType.MinimizeEnd => nowEII.WindowEventData.MinimizeEnd,
                        FreeEcho.FEWindowEvent.WindowEventType.Create => nowEII.WindowEventData.Create,
                        FreeEcho.FEWindowEvent.WindowEventType.Show => nowEII.WindowEventData.Show,
                        FreeEcho.FEWindowEvent.WindowEventType.NameChange => nowEII.WindowEventData.NameChange,
                        _ => false
                    };

                    if (checkEvent
                        && CheckWindowToProcessing(nowEII, windowInformation))
                    {
                        foreach (WindowProcessingInformation nowWPI in nowEII.WindowProcessingInformation)
                        {
                            if (VersionChangeShowMessage(nowEII, nowWPI, windowInformation)
                                && nowWPI.Active
                                && CheckDisplayToProcessing(nowEII, nowWPI, windowInformation))
                            {
                                if (nowWPI.CloseWindow)
                                {
                                    if (nowEII.ProcessingOnlyOnce != ProcessingOnlyOnce.NotSpecified)
                                    {
                                        nowEII.EndedProcessingOnlyOnce = true;
                                    }
                                    nowEII.Hwnd = hwnd;
                                    NativeMethods.PostMessage(hwnd, (uint)WM.WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
                                    break;
                                }
                                else if (nowWPI.NormalWindowOnly == false || windowInformation.ShowCmd == (int)SW.SW_SHOWNORMAL)
                                {
                                    if (nowEII.ProcessingOnlyOnce != ProcessingOnlyOnce.NotSpecified)
                                    {
                                        nowEII.EndedProcessingOnlyOnce = true;
                                    }
                                    nowEII.Hwnd = hwnd;
                                    ProcessingWindow(nowEII, nowWPI, hwnd, windowInformation);
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
    /// 判定してウィンドウ処理 (タイマー)
    /// </summary>
    private void DecisionAndWindowProcessing()
    {
        try
        {
            ProcessingTimer?.Stop();

            HwndList hwndList;        // ウィンドウハンドルのリスト
            bool activeWindowOnly = true;     // アクティブウィンドウのみ処理するかの値
            switch (ApplicationData.Settings.SpecifyWindowInformation.ProcessingWindowRange)
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

            if (ApplicationData.Settings.SpecifyWindowInformation.StopProcessingFullScreen && VariousWindowProcessing.CheckFullScreenWindow(activeWindowOnly ? null : hwndList))
            {
                return;
            }

            int waitTime = 0;     // 待ち時間
            foreach (IntPtr nowHwnd in hwndList.Hwnd)
            {
                WindowInformation windowInformation = VariousWindowProcessing.GetWindowInformationFromHandle(nowHwnd, WindowInformationBuffer);

                foreach (SpecifyWindowItemInformation nowEII in ApplicationData.Settings.SpecifyWindowInformation.Items)
                {
                    if (nowEII.Enabled
                        && nowEII.TimerProcessing
                        && (nowEII.ProcessingOnlyOnce == ProcessingOnlyOnce.NotSpecified || nowEII.EndedProcessingOnlyOnce == false))
                    {
                        if (CheckWindowToProcessing(nowEII, windowInformation))
                        {
                            if (nowEII.CountNumberOfTimesNotToProcessingFirst < nowEII.NumberOfTimesNotToProcessingFirst)
                            {
                                if (nowEII.Hwnd == IntPtr.Zero)
                                {
                                    nowEII.Hwnd = nowHwnd;
                                }
                                if (nowEII.Hwnd == nowHwnd)
                                {
                                    nowEII.CountNumberOfTimesNotToProcessingFirst++;
                                }
                            }
                            else
                            {
                                foreach (WindowProcessingInformation nowWPI in nowEII.WindowProcessingInformation)
                                {
                                    if (VersionChangeShowMessage(nowEII, nowWPI, windowInformation)
                                        && nowWPI.Active
                                        && CheckDisplayToProcessing(nowEII, nowWPI, windowInformation))
                                    {
                                        if (waitTime == 0)
                                        {
                                            waitTime = ApplicationData.Settings.SpecifyWindowInformation.WaitTimeToProcessingNextWindow;
                                        }
                                        else
                                        {
                                            WaitNextWindowProcessing();
                                        }
                                        if (nowWPI.CloseWindow)
                                        {
                                            if (nowEII.ProcessingOnlyOnce != ProcessingOnlyOnce.NotSpecified)
                                            {
                                                nowEII.EndedProcessingOnlyOnce = true;
                                            }
                                            nowEII.Hwnd = nowHwnd;
                                            NativeMethods.PostMessage(nowHwnd, (uint)WM.WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
                                            break;
                                        }
                                        else if (nowWPI.NormalWindowOnly == false || windowInformation.ShowCmd == (int)SW.SW_SHOWNORMAL)
                                        {
                                            if (nowEII.ProcessingOnlyOnce != ProcessingOnlyOnce.NotSpecified)
                                            {
                                                nowEII.EndedProcessingOnlyOnce = true;
                                            }
                                            nowEII.Hwnd = nowHwnd;
                                            ProcessingWindow(nowEII, nowWPI, nowHwnd, windowInformation);
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
    /// <param name="processingType">処理の種類 (一括実行「false」/アクティブウィンドウのみ「true」)</param>
    public static void DecisionAndWindowProcessing(
        bool processingType
        )
    {
        try
        {
            HwndList hwndList;        // ウィンドウハンドルのリスト
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

            if (ApplicationData.Settings.SpecifyWindowInformation.StopProcessingFullScreen && VariousWindowProcessing.CheckFullScreenWindow(processingType ? null : hwndList))
            {
                return;
            }

            foreach (IntPtr nowHwnd in hwndList.Hwnd)
            {
                WindowInformation windowInformation = VariousWindowProcessing.GetWindowInformationFromHandle(nowHwnd, windowInformationBuffer);

                foreach (SpecifyWindowItemInformation nowEII in ApplicationData.Settings.SpecifyWindowInformation.Items)
                {
                    if (CheckWindowToProcessing(nowEII, windowInformation))
                    {
                        foreach (WindowProcessingInformation nowWPI in nowEII.WindowProcessingInformation)
                        {
                            if (VersionChangeShowMessage(nowEII, nowWPI, windowInformation)
                                && nowWPI.Active
                                && CheckDisplayToProcessing(nowEII, nowWPI, windowInformation))
                            {
                                if (nowWPI.CloseWindow)
                                {
                                    NativeMethods.PostMessage(nowHwnd, (uint)WM.WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
                                    break;
                                }
                                else if (nowWPI.NormalWindowOnly == false || windowInformation.ShowCmd == (int)SW.SW_SHOWNORMAL)
                                {
                                    ProcessingWindow(nowEII, nowWPI, nowHwnd, windowInformation);
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
    private static void WaitNextWindowProcessing()
    {
        try
        {
            using System.Threading.Tasks.Task task = System.Threading.Tasks.Task.Run(async () =>
            {
                await System.Threading.Tasks.Task.Delay(ApplicationData.Settings.SpecifyWindowInformation.WaitTimeToProcessingNextWindow);
            });
            task.Wait();
        }
        catch
        {
        }
    }

    /// <summary>
    /// ホットキーを処理
    /// </summary>
    /// <param name="id">ホットキーの識別子</param>
    private void ProcessingHotkeys(
        int id
        )
    {
        try
        {
            HwndList hwndList = HwndList.GetWindowHandleList();        // ウィンドウハンドルのリスト

            foreach (IntPtr nowHwnd in hwndList.Hwnd)
            {
                WindowInformation windowInformation = VariousWindowProcessing.GetWindowInformationFromHandle(nowHwnd, WindowInformationBuffer);

                foreach (SpecifyWindowItemInformation nowEII in ApplicationData.Settings.SpecifyWindowInformation.Items)
                {
                    if (CheckWindowToProcessing(nowEII, windowInformation))
                    {
                        foreach (WindowProcessingInformation nowWPI in nowEII.WindowProcessingInformation)
                        {
                            if (VersionChangeShowMessage(nowEII, nowWPI, windowInformation)
                                && (nowEII.StandardDisplay != StandardDisplay.ExclusiveSpecifiedDisplay || CheckDisplayToProcessing(nowEII, nowWPI, windowInformation)))
                            {
                                if (nowWPI.CloseWindow)
                                {
                                    NativeMethods.PostMessage(nowHwnd, (uint)WM.WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
                                    break;
                                }
                                else if (nowWPI.HotkeyId == id)
                                {
                                    ProcessingWindow(nowEII, nowWPI, nowHwnd, windowInformation);
                                    break;
                                }
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

    /// <summary>
    /// ホットキーを登録
    /// </summary>
    public void RegisterHotkeys()
    {
        UnregisterHotkeys();
        if (CheckIfTheProcessingIsValid())
        {
            int id = Common.EventHotkeysStartId;       // ホットキーの識別子
            foreach (SpecifyWindowItemInformation nowEII in ApplicationData.Settings.SpecifyWindowInformation.Items)
            {
                foreach (WindowProcessingInformation nowWPI in nowEII.WindowProcessingInformation)
                {
                    if (VariousProcessing.RegisterHotkey(nowWPI.Hotkey, id))
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
        foreach (SpecifyWindowItemInformation nowEII in ApplicationData.Settings.SpecifyWindowInformation.Items)
        {
            foreach (WindowProcessingInformation nowWPI in nowEII.WindowProcessingInformation)
            {
                if (nowWPI.HotkeyId != -1)
                {
                    try
                    {
                        NativeMethods.UnregisterHotKey(ApplicationData.MainHwnd, nowWPI.HotkeyId);
                    }
                    catch
                    {
                    }
                    nowWPI.HotkeyId = -1;
                }
            }
        }
    }

    /// <summary>
    /// 処理するウィンドウか確認
    /// </summary>
    /// <param name="specifyWindowItemInformation">「指定ウィンドウ」機能の項目情報</param>
    /// <param name="windowInformation">ウィンドウの情報</param>
    /// <returns>処理するウィンドウかの値 (いいえ「false」/はい「true」)</returns>
    private static bool CheckWindowToProcessing(
        SpecifyWindowItemInformation specifyWindowItemInformation,
        WindowInformation windowInformation
        )
    {
        try
        {
            // 処理しないタイトル名の条件
            switch (specifyWindowItemInformation.DoNotProcessingTitleNameConditions)
            {
                case TitleNameProcessingConditions.NotIncluded:
                    if (string.IsNullOrEmpty(windowInformation.TitleName))
                    {
                        return false;
                    }
                    break;
                case TitleNameProcessingConditions.Included:
                    if (string.IsNullOrEmpty(windowInformation.TitleName) == false)
                    {
                        return false;
                    }
                    break;
            }

            string checkString;      // 確認する文字列
            string windowString;       // ウィンドウ文字列

            // タイトル名判定
            if (string.IsNullOrEmpty(windowInformation.TitleName) == false)
            {
                if (ApplicationData.Settings.SpecifyWindowInformation.CaseSensitive)
                {
                    checkString = specifyWindowItemInformation.TitleName;
                    windowString = windowInformation.TitleName;
                }
                else
                {
                    checkString = specifyWindowItemInformation.TitleName.ToLower();
                    windowString = windowInformation.TitleName.ToLower();
                }

                if (string.IsNullOrEmpty(checkString) == false)
                {
                    switch (specifyWindowItemInformation.TitleNameMatchCondition)
                    {
                        case NameMatchCondition.PartialMatch:
                            if (windowString.Contains(checkString) == false)
                            {
                                return false;
                            }
                            break;
                        case NameMatchCondition.ForwardMatch:
                            if (windowString.Length < checkString.Length || windowString.StartsWith(checkString) == false)
                            {
                                return false;
                            }
                            break;
                        case NameMatchCondition.BackwardMatch:
                            if (windowString.Length < checkString.Length || windowString.EndsWith(checkString) == false)
                            {
                                return false;
                            }
                            break;
                        default:
                            if (windowString != checkString)
                            {
                                return false;
                            }
                            break;
                    }
                }

                // 処理しないタイトル名に含まれる文字列
                foreach (string nowTitleName in specifyWindowItemInformation.DoNotProcessingStringContainedInTitleName)
                {
                    if (windowString.Contains(ApplicationData.Settings.SpecifyWindowInformation.CaseSensitive ? nowTitleName : nowTitleName.ToLower()))
                    {
                        return false;
                    }
                }
            }

            // クラス名判定
            if (string.IsNullOrEmpty(specifyWindowItemInformation.ClassName) == false)
            {
                // 取得に失敗した場合は処理しない
                if (string.IsNullOrEmpty(windowInformation.ClassName))
                {
                    return false;
                }

                if (ApplicationData.Settings.SpecifyWindowInformation.CaseSensitive)
                {
                    checkString = specifyWindowItemInformation.ClassName;
                    windowString = windowInformation.ClassName;
                }
                else
                {
                    checkString = specifyWindowItemInformation.ClassName.ToLower();
                    windowString = windowInformation.ClassName.ToLower();
                }

                switch (specifyWindowItemInformation.ClassNameMatchCondition)
                {
                    case NameMatchCondition.PartialMatch:
                        if (windowString.Contains(checkString) == false)
                        {
                            return false;
                        }
                        break;
                    case NameMatchCondition.ForwardMatch:
                        if (windowString.Length < checkString.Length || windowString.StartsWith(checkString) == false)
                        {
                            return false;
                        }
                        break;
                    case NameMatchCondition.BackwardMatch:
                        if (windowString.Length < checkString.Length || windowString.EndsWith(checkString) == false)
                        {
                            return false;
                        }
                        break;
                    default:
                        if (checkString != windowString)
                        {
                            return false;
                        }
                        break;
                }
            }

            // ファイル名判定
            if (string.IsNullOrEmpty(specifyWindowItemInformation.FileName) == false)
            {
                // 取得に失敗した場合は処理しない
                if (string.IsNullOrEmpty(windowInformation.FileName))
                {
                    return false;
                }

                if (ApplicationData.Settings.SpecifyWindowInformation.CaseSensitive)
                {
                    checkString = specifyWindowItemInformation.FileName;
                    windowString = windowInformation.FileName;
                }
                else
                {
                    checkString = specifyWindowItemInformation.FileName.ToLower();
                    windowString = windowInformation.FileName.ToLower();
                }

                switch (specifyWindowItemInformation.FileNameMatchCondition)
                {
                    case FileNameMatchCondition.NotInclude:
                        if (checkString != Path.GetFileNameWithoutExtension(windowString)
                            && checkString != Path.GetFileName(windowString))
                        {
                            return false;
                        }
                        break;
                    default:
                        if (checkString != windowString)
                        {
                            return false;
                        }
                        break;
                }
            }

            // 処理しない条件のサイズ判定
            if (specifyWindowItemInformation.DoNotProcessingSize.Count != 0)
            {
                System.Windows.Size windowSize = new()
                {
                    Width = windowInformation.Rectangle.Right - windowInformation.Rectangle.Left,
                    Height = windowInformation.Rectangle.Bottom - windowInformation.Rectangle.Top
                };      // ウィンドウサイズ
                foreach (System.Drawing.Size nowSize in specifyWindowItemInformation.DoNotProcessingSize)
                {
                    if (windowSize.Width == nowSize.Width && windowSize.Height == nowSize.Height)
                    {
                        return false;
                    }
                }
            }

            // 指定バージョン以外は処理しない
            if (string.IsNullOrEmpty(specifyWindowItemInformation.DoNotProcessingOtherThanSpecifiedVersion) == false)
            {
                VersionNumber targetWindowVersionNumber = VariousProcessing.SplitVersionNumber(windowInformation.Version);
                VersionNumber checkVersionNumber = VariousProcessing.SplitVersionNumber(specifyWindowItemInformation.DoNotProcessingOtherThanSpecifiedVersion);
                if (VariousProcessing.VersionComparison(targetWindowVersionNumber, checkVersionNumber) == false)
                {
                    if (specifyWindowItemInformation.DoNotProcessingOtherThanSpecifiedVersionAnnounce)
                    {
                        CheckShowMessageVersion = true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
        }
        catch
        {
        }

        return true;
    }

    /// <summary>
    /// バージョンが変更された時のメッセージ表示
    /// </summary>
    /// <param name="specifyWindowItemInformation">「指定ウィンドウ」機能の項目情報</param>
    /// <param name="windowProcessingInformation">ウィンドウの処理情報</param>
    /// <param name="windowInformation">ウィンドウの情報</param>
    /// <returns>処理するかの値 (処理しない「false」/処理する「true」)</returns>
    private static bool VersionChangeShowMessage(
        SpecifyWindowItemInformation specifyWindowItemInformation,
        WindowProcessingInformation windowProcessingInformation,
        WindowInformation windowInformation
        )
    {
        if (CheckShowMessageVersion)
        {
            CheckShowMessageVersion = false;
            LanguageFileProcessing.ReadLanguage();
            string message = "";
            message += specifyWindowItemInformation.RegisteredName + Environment.NewLine;
            message += ApplicationData.Languages.LanguagesWindow?.WindowInformation + Environment.NewLine;
            message += ApplicationData.Languages.LanguagesWindow?.Width + Common.TypeAndValueSeparateString + windowInformation.Rectangle.Width + Common.ValueAndValueSeparateString + ApplicationData.Languages.LanguagesWindow?.Height + Common.TypeAndValueSeparateString + windowInformation.Rectangle.Height + Environment.NewLine;
            message += ApplicationData.Languages.LanguagesWindow?.Version + Common.TypeAndValueSeparateString + windowInformation.Version + Environment.NewLine;
            if (windowProcessingInformation.PositionSize.WidthType == WindowSizeType.Value
                || windowProcessingInformation.PositionSize.HeightType == WindowSizeType.Value
                || string.IsNullOrEmpty(specifyWindowItemInformation.DoNotProcessingOtherThanSpecifiedVersion) == false)
            {
                message += ApplicationData.Languages.LanguagesWindow?.SettingsValue + Environment.NewLine;
                string separateString = "";
                if (windowProcessingInformation.PositionSize.WidthType == WindowSizeType.Value)
                {
                    message += ApplicationData.Languages.LanguagesWindow?.Width + Common.TypeAndValueSeparateString + windowProcessingInformation.PositionSize.Width;
                    separateString = Common.ValueAndValueSeparateString;
                }
                if (windowProcessingInformation.PositionSize.HeightType == WindowSizeType.Value)
                {
                    message += separateString + ApplicationData.Languages.LanguagesWindow?.Height + Common.TypeAndValueSeparateString + windowProcessingInformation.PositionSize.Height;
                    separateString = Common.ValueAndValueSeparateString;
                }
                if (string.IsNullOrEmpty(separateString) == false)
                {
                    message += Environment.NewLine;
                }
                if (string.IsNullOrEmpty(specifyWindowItemInformation.DoNotProcessingOtherThanSpecifiedVersion) == false)
                {
                    message += ApplicationData.Languages.LanguagesWindow?.Version + Common.TypeAndValueSeparateString + specifyWindowItemInformation.DoNotProcessingOtherThanSpecifiedVersion + Environment.NewLine;
                }
            }
            message += Environment.NewLine + ApplicationData.Languages.ChangeSettingToNewVersion;
            switch (FEMessageBox.Show(message, ApplicationData.Languages.ChangeVersion + Common.CopySeparateString + Common.ApplicationName, MessageBoxButton.YesNoCancel))
            {
                case MessageBoxResult.Yes:
                    {
                        VersionNumber targetWindowVersionNumber = VariousProcessing.SplitVersionNumber(windowInformation.Version);
                        VersionNumber checkVersionNumber = VariousProcessing.SplitVersionNumber(specifyWindowItemInformation.DoNotProcessingOtherThanSpecifiedVersion);
                        string setVersionString = "";
                        if (checkVersionNumber.MajorNumber != -1)
                        {
                            setVersionString += targetWindowVersionNumber.MajorNumber.ToString();
                        }
                        if (checkVersionNumber.MinorNumber != -1)
                        {
                            setVersionString += "." + targetWindowVersionNumber.MinorNumber.ToString();
                        }
                        if (checkVersionNumber.BuildNumber != -1)
                        {
                            setVersionString += "." + targetWindowVersionNumber.BuildNumber.ToString();
                        }
                        if (checkVersionNumber.PrivateNumber != -1)
                        {
                            setVersionString += "." + targetWindowVersionNumber.PrivateNumber.ToString();
                        }
                        if (string.IsNullOrEmpty(setVersionString) == false)
                        {
                            specifyWindowItemInformation.DoNotProcessingOtherThanSpecifiedVersion = setVersionString;
                        }
                    }
                    ApplicationData.Languages.LanguagesWindow = null;
                    break;
                case MessageBoxResult.No:
                    specifyWindowItemInformation.Enabled = false;
                    IndexOfItemBeingProcessed = ApplicationData.Settings.SpecifyWindowInformation.Items.IndexOf(specifyWindowItemInformation);
                    ShowSpecifyWindowItemWindow(IndexOfItemBeingProcessed);
                    return false;
                case MessageBoxResult.Cancel:
                    specifyWindowItemInformation.Enabled = false;
                    ApplicationData.Languages.LanguagesWindow = null;
                    break;
                default:
                    specifyWindowItemInformation.Enabled = false;
                    ApplicationData.Languages.LanguagesWindow = null;
                    break;
            }

            SettingFileProcessing.WriteSettings();
            return false;
        }

        return true;
    }

    /// <summary>
    /// 処理するディスプレイか確認
    /// </summary>
    /// <param name="specifyWindowItemInformation">「指定ウィンドウ」機能の項目情報</param>
    /// <param name="windowProcessingInformation">ウィンドウの処理情報</param>
    /// <param name="windowInformation">ウィンドウの情報</param>
    /// <returns>処理するディスプレイかの値 (処理しない「false」/処理する「true」)</returns>
    private static bool CheckDisplayToProcessing(
        SpecifyWindowItemInformation specifyWindowItemInformation,
        WindowProcessingInformation windowProcessingInformation,
        WindowInformation windowInformation
        )
    {
        if (ApplicationData.Settings.CoordinateType == CoordinateType.EachDisplay
            && specifyWindowItemInformation.StandardDisplay == StandardDisplay.ExclusiveSpecifiedDisplay)
        {
            MonitorInformation.GetMonitorInformationOnWindowShown(windowInformation.Rectangle, out MonitorInfoEx monitorInfo);
            if (windowProcessingInformation.PositionSize.Display != monitorInfo.DeviceName)
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// ウィンドウを処理
    /// </summary>
    /// <param name="specifyWindowItemInformation">「指定ウィンドウ」機能の項目情報</param>
    /// <param name="windowProcessingInformation">ウィンドウの処理情報</param>
    /// <param name="hwnd">ウィンドウハンドル</param>
    /// <param name="windowInformation">ウィンドウの情報</param>
    private static void ProcessingWindow(
        SpecifyWindowItemInformation specifyWindowItemInformation,
        WindowProcessingInformation windowProcessingInformation,
        IntPtr hwnd,
        WindowInformation windowInformation
        )
    {
        try
        {
            ProcessingWindowPositionAndSize(specifyWindowItemInformation, windowProcessingInformation, hwnd, windowInformation);

            if (windowProcessingInformation.EnabledTransparency)
            {
                if ((NativeMethods.GetWindowLongPtr(hwnd, (int)GWL.GWL_EXSTYLE) & (int)WS_EX.WS_EX_LAYERED) != (int)WS_EX.WS_EX_LAYERED)
                {
                    NativeMethods.SetWindowLongPtr(hwnd, (int)GWL.GWL_EXSTYLE, (int)(NativeMethods.GetWindowLongPtr(hwnd, (int)GWL.GWL_EXSTYLE) ^ (int)WS_EX.WS_EX_LAYERED));
                }
                uint lwa = (uint)LWA.LWA_ALPHA;
                NativeMethods.GetLayeredWindowAttributes(hwnd, out uint getKey, out byte getAlpha, out lwa);
                if (getAlpha != windowProcessingInformation.Transparency)
                {
                    NativeMethods.SetLayeredWindowAttributes(hwnd, 0, (byte)windowProcessingInformation.Transparency, (uint)LWA.LWA_ALPHA);
                }
            }
        }
        catch
        {
        }
    }

    /// <summary>
    /// ウィンドウの位置とサイズと最前面を処理
    /// </summary>
    /// <param name="specifyWindowItemInformation">「指定ウィンドウ」機能の項目情報</param>
    /// <param name="windowProcessingInformation">ウィンドウの処理情報</param>
    /// <param name="hwnd">ウィンドウハンドル</param>
    /// <param name="windowInformation">ウィンドウの情報</param>
    private static void ProcessingWindowPositionAndSize(
        SpecifyWindowItemInformation specifyWindowItemInformation,
        WindowProcessingInformation windowProcessingInformation,
        IntPtr hwnd,
        WindowInformation windowInformation
        )
    {
        try
        {
            bool doNormal = false;     // 「通常のウィンドウ」にするかの値
            MonitorInformation.GetMonitorInformationOnWindowShown(windowInformation.Rectangle, out MonitorInfoEx monitorInfo);       // モニター情報
            RectangleInt changedDisplayRectangle = VariousWindowProcessing.GetDisplayPositionAndSizeAfterProcessing(windowInformation.Rectangle, specifyWindowItemInformation.StandardDisplay, windowProcessingInformation.PositionSize.Display);        // 処理後のディスプレイの位置とサイズ

            // 位置やサイズを計算
            Rect windowRectangle = new(windowInformation.Rectangle.Left, windowInformation.Rectangle.Top, windowInformation.Rectangle.Width, windowInformation.Rectangle.Height);     // 処理後のウィンドウの位置とサイズ
            switch (windowProcessingInformation.PositionSize.SettingsWindowState)
            {
                case SettingsWindowState.DoNotChange:
                    if (ApplicationData.Settings.CoordinateType == CoordinateType.EachDisplay
                        && specifyWindowItemInformation.StandardDisplay == StandardDisplay.SpecifiedDisplay
                        && windowProcessingInformation.PositionSize.Display != monitorInfo.DeviceName)
                    {
                        windowRectangle.X = changedDisplayRectangle.Left + windowInformation.Rectangle.Right - windowInformation.Rectangle.Left;
                        windowRectangle.Y = changedDisplayRectangle.Top + windowInformation.Rectangle.Bottom - windowInformation.Rectangle.Top;
                    }
                    break;
                case SettingsWindowState.Normal:
                    windowRectangle = VariousWindowProcessing.GetPositionSizeOfWindowAfterProcessing(hwnd, windowInformation, windowProcessingInformation.PositionSize, specifyWindowItemInformation.StandardDisplay, windowProcessingInformation.PositionSize.ClientArea);
                    doNormal = true;
                    break;
                case SettingsWindowState.Maximize:
                case SettingsWindowState.Minimize:
                    if (ApplicationData.Settings.CoordinateType == CoordinateType.EachDisplay
                        && specifyWindowItemInformation.StandardDisplay == StandardDisplay.SpecifiedDisplay
                        && windowProcessingInformation.PositionSize.Display != monitorInfo.DeviceName)
                    {
                        windowRectangle.X = changedDisplayRectangle.Left;
                        windowRectangle.Y = changedDisplayRectangle.Top;
                    }
                    break;
            }

            bool doSetWindowPos = false;        // SetWindowPosを処理するかの値

            // 位置やサイズを変更する場合は処理を有効にする
            if (windowRectangle.X != windowInformation.Rectangle.Left
                || windowRectangle.Y != windowInformation.Rectangle.Top
                || windowRectangle.Width != windowInformation.Rectangle.Width
                || windowRectangle.Height != windowInformation.Rectangle.Height)
            {
                doSetWindowPos = true;
                doNormal = true;
            }

            int flag = (int)SWP.SWP_NOACTIVATE;     // フラグ
            int hwndInsertAfter = (int)HwndInsertAfter.HWND_NOTOPMOST;     // ウィンドウの順番

            // 最前面の処理を決める
            switch (windowProcessingInformation.Forefront)
            {
                case Forefront.Cancel:
                    if ((NativeMethods.GetWindowLongPtr(hwnd, (int)GWL.GWL_EXSTYLE) & (int)WS_EX.WS_EX_TOPMOST) == (int)WS_EX.WS_EX_TOPMOST)
                    {
                        hwndInsertAfter = (int)HwndInsertAfter.HWND_NOTOPMOST;
                        doSetWindowPos = true;
                    }
                    break;
                case Forefront.Always:
                    if ((NativeMethods.GetWindowLongPtr(hwnd, (int)GWL.GWL_EXSTYLE) & (int)WS_EX.WS_EX_TOPMOST) != (int)WS_EX.WS_EX_TOPMOST)
                    {
                        hwndInsertAfter = (int)HwndInsertAfter.HWND_TOPMOST;
                        doSetWindowPos = true;
                    }
                    break;
                case Forefront.Forefront:
                    NativeMethods.SetForegroundWindow(hwnd);
                    break;
                default:
                    flag |= (int)SWP.SWP_NOZORDER;
                    break;
            }

            // ウィンドウを処理
            // 位置やサイズを変更できるようにウィンドウの状態を「通常のウィンドウ」にする
            if (doNormal && windowInformation.ShowCmd != (int)SW.SW_SHOWNORMAL)
            {
                NativeMethods.ShowWindow(hwnd, (int)SW.SW_SHOWNORMAL);
            }
            if (doSetWindowPos)
            {
                if (ApplicationData.Settings.SpecifyWindowInformation.DoNotChangeOutOfScreen && VariousWindowProcessing.CheckWindowIsInTheScreen(windowRectangle) == false)
                {
                    flag |= (int)SWP.SWP_NOMOVE | (int)SWP.SWP_NOSIZE;
                }
                // 1回目の処理
                NativeMethods.SetWindowPos(hwnd, hwndInsertAfter, (int)windowRectangle.X, (int)windowRectangle.Y, (int)windowRectangle.Width, (int)windowRectangle.Height, flag);
                // 2回目の処理
                if (windowProcessingInformation.PositionSize.SettingsWindowState == SettingsWindowState.Normal)
                {
                    windowInformation = VariousWindowProcessing.GetWindowInformationPositionSizeFromHandle(hwnd);
                    windowRectangle = VariousWindowProcessing.GetPositionSizeOfWindowAfterProcessing(hwnd, windowInformation, windowProcessingInformation.PositionSize, specifyWindowItemInformation.StandardDisplay, windowProcessingInformation.PositionSize.ClientArea);
                    if ((int)windowRectangle.X != windowInformation.Rectangle.Left
                        || (int)windowRectangle.Y != windowInformation.Rectangle.Top
                        || (int)windowRectangle.Width != windowInformation.Rectangle.Width
                        || (int)windowRectangle.Height != windowInformation.Rectangle.Height)
                    {
                        NativeMethods.SetWindowPos(hwnd, hwndInsertAfter, (int)windowRectangle.X, (int)windowRectangle.Y, (int)windowRectangle.Width, (int)windowRectangle.Height, flag);
                    }
                }
            }
            switch (windowProcessingInformation.PositionSize.SettingsWindowState)
            {
                case SettingsWindowState.Maximize:
                    if (doNormal || windowInformation.ShowCmd != (int)SW.SW_SHOWMAXIMIZED)
                    {
                        NativeMethods.ShowWindow(hwnd, (int)SW.SW_SHOWMAXIMIZED);
                    }
                    break;
                case SettingsWindowState.Minimize:
                    if (doNormal || windowInformation.ShowCmd != (int)SW.SW_SHOWMINIMIZED)
                    {
                        NativeMethods.ShowWindow(hwnd, (int)SW.SW_SHOWMINIMIZED);
                    }
                    break;
            }
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「指定ウィンドウ」の「追加/修正」ウィンドウを表示
    /// </summary>
    /// <param name="indexOfItemToBeModified">修正する項目のインデックス (追加「-1」)</param>
    private static void ShowSpecifyWindowItemWindow(
        int indexOfItemToBeModified = -1
        )
    {
        if (SpecifyWindowItemWindow == null)
        {
            if (ApplicationData.Languages.LanguagesWindow == null)
            {
                LanguageFileProcessing.ReadLanguage();
            }
            SpecifyWindowItemWindow = new(null, indexOfItemToBeModified);
            SpecifyWindowItemWindow.Closed += SpecifyWindowItemWindow_Closed;
            SpecifyWindowItemWindow.Show();
        }
        else
        {
            SpecifyWindowItemWindow.Activate();
        }
    }

    /// <summary>
    /// 「指定ウィンドウ」の「追加/修正」ウィンドウの「Closed」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private static void SpecifyWindowItemWindow_Closed(
        object? sender,
        EventArgs e
        )
    {
        try
        {
            if (SpecifyWindowItemWindow?.Owner == null)
            {
                ApplicationData.EventData.DoProcessingEvent(ProcessingEventType.CloseSpecifyWindowItemWindow);
            }
            SpecifyWindowItemWindow = null;
            ApplicationData.WindowManagement.UnnecessaryLanguageDataDelete();
        }
        catch
        {
        }
    }
}
