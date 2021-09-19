using Swindom.Source.Settings;

namespace Swindom.Source
{
    /// <summary>
    /// イベントでウィンドウ処理
    /// </summary>
    public class EventWindowProcessing : System.IDisposable
    {
        /// <summary>
        /// Disposeが呼ばれたかの値
        /// </summary>
        private bool Disposed;
        /// <summary>
        /// マウスでウィンドウ移動検知
        /// </summary>
        private FreeEcho.FEWindowMoveDetectionMouse.MouseMoveWindowDetection MouseMoveWindowDetection;
        /// <summary>
        /// ウィンドウイベント
        /// </summary>
        private FreeEcho.FEWindowEvent.WindowEvent WindowEvent;
        /// <summary>
        /// 処理が一時停止しているかの値 (停止していない「false」/停止している「true」)
        /// </summary>
        private bool PauseProcessing;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public EventWindowProcessing()
        {
            ProcessingSettings();
            Common.ApplicationData.ProcessingEvent += ApplicationData_ProcessingEvent;
        }

        /// <summary>
        /// デストラクタ
        /// </summary>
        ~EventWindowProcessing()
        {
            Dispose(false);
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            System.GC.SuppressFinalize(this);
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
                DisposeWindowEvent();
                DisposeMouseMoveWindowDetection();
                UnregisterHotkeys();
            }
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
        public static bool CheckIfTheProcessingIsValid()
        {
            return (Common.ApplicationData.Settings.EventInformation.Enabled
                && ((Common.ApplicationData.Settings.EventInformation.StopProcessingFullScreen && Common.ApplicationData.FullScreenExists) ? Common.ApplicationData.Settings.EventInformation.HotkeysDoNotStopFullScreen : true));
        }

        /// <summary>
        /// 処理設定
        /// </summary>
        public void ProcessingSettings()
        {
            if (Common.ApplicationData.Settings.EventInformation.Enabled
                && (Common.ApplicationData.Settings.EventInformation.StopProcessingFullScreen == false || Common.ApplicationData.FullScreenExists == false))
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
                FreeEcho.FEWindowEvent.HookWindowEventType type = FreeEcho.FEWindowEvent.HookWindowEventType.Destroy;       // 「Destroy」は「1度だけ処理が終わっている」の判定で使用するから
                if (Common.ApplicationData.Settings.EventInformation.EventTypeInformation.Foreground)
                {
                    type |= FreeEcho.FEWindowEvent.HookWindowEventType.Foreground;
                }
                if (Common.ApplicationData.Settings.EventInformation.EventTypeInformation.MoveSizeEnd)
                {
                    type |= FreeEcho.FEWindowEvent.HookWindowEventType.MoveSizeEnd;
                }
                if (Common.ApplicationData.Settings.EventInformation.EventTypeInformation.MinimizeStart)
                {
                    type |= FreeEcho.FEWindowEvent.HookWindowEventType.MinimizeStart;
                }
                if (Common.ApplicationData.Settings.EventInformation.EventTypeInformation.MinimizeEnd)
                {
                    type |= FreeEcho.FEWindowEvent.HookWindowEventType.MinimizeEnd;
                }
                if (Common.ApplicationData.Settings.EventInformation.EventTypeInformation.Create)
                {
                    type |= FreeEcho.FEWindowEvent.HookWindowEventType.Create;
                }
                if (Common.ApplicationData.Settings.EventInformation.EventTypeInformation.Show)
                {
                    type |= FreeEcho.FEWindowEvent.HookWindowEventType.Show;
                }
                if (Common.ApplicationData.Settings.EventInformation.EventTypeInformation.NameChange)
                {
                    type |= FreeEcho.FEWindowEvent.HookWindowEventType.NameChange;
                }
                WindowEvent.Hook(type);

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
                DisposeMouseMoveWindowDetection();
            }
            RegisterHotkeys();
        }

        /// <summary>
        /// 「処理」イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ApplicationData_ProcessingEvent(
            object sender,
            ProcessingEventArgs e
            )
        {
            try
            {
                switch (e.ProcessingEventType)
                {
                    case ProcessingEventType.EventPause:
                        PauseProcessing = true;
                        MouseMoveWindowDetection?.Stop();
                        break;
                    case ProcessingEventType.EventUnpause:
                        PauseProcessing = false;
                        MouseMoveWindowDetection?.Start();
                        break;
                    case ProcessingEventType.ReceiveEventChanged:
                        ProcessingSettings();
                        break;
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// 「WindowEventOccurrence」イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WindowEvent_WindowEventOccurrence(
            object sender,
            FreeEcho.FEWindowEvent.WindowEventArgs e
            )
        {
            try
            {
                if (PauseProcessing == false)
                {
                    // ウィンドウが閉じている場合は「1度だけ処理が終わっている」を解除
                    if (e.WindowEventType == FreeEcho.FEWindowEvent.WindowEventType.Destroy)
                    {
                        foreach (EventItemInformation nowEII in Common.ApplicationData.Settings.EventInformation.Items)
                        {
                            if ((nowEII.Hwnd != System.IntPtr.Zero) && (NativeMethods.IsWindow(nowEII.Hwnd) == false) && (nowEII.ProcessingOnlyOnce != ProcessingOnlyOnce.Running))
                            {
                                nowEII.EndedProcessingOnlyOnce = false;
                                nowEII.Hwnd = System.IntPtr.Zero;
                                break;
                            }
                        }
                    }

                    DecisionAndWindowProcessing(e);
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
        /// <param name="e">ウィンドウイベントのデータ</param>
        private void DecisionAndWindowProcessing(
            FreeEcho.FEWindowEvent.WindowEventArgs e
            )
        {
            try
            {
                if (Common.ApplicationData.Settings.EventInformation.StopProcessingFullScreen && WindowProcessing.CheckFullScreenWindow(null, null))
                {
                    return;
                }

                System.IntPtr hwnd = NativeMethods.GetAncestor(e.Hwnd, GetAncestorFlags.GetRootOwner);
                if (hwnd == System.IntPtr.Zero)
                {
                    return;
                }

                WINDOWPLACEMENT windowPlacement;
                NativeMethods.GetWindowPlacement(hwnd, out windowPlacement);
                WindowInformation windowInformation = WindowProcessing.GetWindowInformation(hwnd);
                MonitorInformation monitorInformation = MonitorInformation.GetMonitorInformation();

                foreach (EventItemInformation nowEII in Common.ApplicationData.Settings.EventInformation.Items)
                {
                    if (nowEII.Enabled
                        && (nowEII.ProcessingOnlyOnce == ProcessingOnlyOnce.NotSpecified || nowEII.EndedProcessingOnlyOnce == false))
                    {
                        bool checkEvent = false;       // イベントの確認

                        switch (e.WindowEventType)
                        {
                            case FreeEcho.FEWindowEvent.WindowEventType.Foreground:
                                checkEvent = nowEII.WindowEventData.Foreground;
                                break;
                            case FreeEcho.FEWindowEvent.WindowEventType.MoveSizeEnd:
                                checkEvent = nowEII.WindowEventData.MoveSizeEnd;
                                break;
                            case FreeEcho.FEWindowEvent.WindowEventType.MinimizeStart:
                                checkEvent = nowEII.WindowEventData.MinimizeStart;
                                break;
                            case FreeEcho.FEWindowEvent.WindowEventType.MinimizeEnd:
                                checkEvent = nowEII.WindowEventData.MinimizeEnd;
                                break;
                            case FreeEcho.FEWindowEvent.WindowEventType.Create:
                                checkEvent = nowEII.WindowEventData.Create;
                                break;
                            case FreeEcho.FEWindowEvent.WindowEventType.Show:
                                checkEvent = nowEII.WindowEventData.Show;
                                break;
                            case FreeEcho.FEWindowEvent.WindowEventType.NameChange:
                                checkEvent = nowEII.WindowEventData.NameChange;
                                break;
                        }

                        if (checkEvent)
                        {
                            if (SpecifiedWindow.CheckWindowToProcessing(Common.ApplicationData.Settings.EventInformation.CaseSensitiveWindowQueries, nowEII, windowInformation, in windowPlacement))
                            {
                                if (nowEII.CloseWindow)
                                {
                                    if (nowEII.ProcessingOnlyOnce != ProcessingOnlyOnce.NotSpecified)
                                    {
                                        nowEII.EndedProcessingOnlyOnce = true;
                                    }
                                    nowEII.Hwnd = hwnd;
                                    NativeMethods.PostMessage(hwnd, (uint)WindowMessage.WM_CLOSE, System.IntPtr.Zero, System.IntPtr.Zero);
                                }
                                else
                                {
                                    foreach (WindowProcessingInformation nowWPI in nowEII.WindowProcessingInformation)
                                    {
                                        if (nowWPI.Active
                                            && (nowWPI.OnlyNormalWindow == false || windowPlacement.showCmd == (int)SW.SW_SHOWNORMAL)
                                            && SpecifiedWindow.CheckDisplayToProcessing(nowEII, nowWPI, hwnd, monitorInformation))
                                        {
                                            if (nowEII.ProcessingOnlyOnce != ProcessingOnlyOnce.NotSpecified)
                                            {
                                                nowEII.EndedProcessingOnlyOnce = true;
                                            }
                                            nowEII.Hwnd = hwnd;
                                            SpecifiedWindow.ProcessingWindow(nowEII, nowWPI, hwnd, in windowPlacement, false, Common.ApplicationData.Settings.EventInformation.DoNotChangeOutOfScreen, monitorInformation);
                                            break;
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
                MonitorInformation monitorInformation = MonitorInformation.GetMonitorInformation();     // モニター情報

                foreach (System.IntPtr nowHwnd in hwndList.Hwnd)
                {
                    WINDOWPLACEMENT windowPlacement;
                    NativeMethods.GetWindowPlacement(nowHwnd, out windowPlacement);
                    WindowInformation windowInformation = WindowProcessing.GetWindowInformation(nowHwnd);

                    foreach (EventItemInformation nowEII in Common.ApplicationData.Settings.EventInformation.Items)
                    {
                        if (SpecifiedWindow.CheckWindowToProcessing(Common.ApplicationData.Settings.EventInformation.CaseSensitiveWindowQueries, nowEII, windowInformation, in windowPlacement))
                        {
                            if (nowEII.CloseWindow)
                            {
                                NativeMethods.PostMessage(nowHwnd, (uint)WindowMessage.WM_CLOSE, System.IntPtr.Zero, System.IntPtr.Zero);
                            }
                            else
                            {
                                foreach (WindowProcessingInformation nowWPI in nowEII.WindowProcessingInformation)
                                {
                                    if (nowWPI.Active
                                        && (nowWPI.OnlyNormalWindow == false || windowPlacement.showCmd == (int)SW.SW_SHOWNORMAL)
                                        && SpecifiedWindow.CheckDisplayToProcessing(nowEII, nowWPI, nowHwnd, monitorInformation))
                                    {
                                        SpecifiedWindow.ProcessingWindow(nowEII, nowWPI, nowHwnd, in windowPlacement, true, Common.ApplicationData.Settings.EventInformation.DoNotChangeOutOfScreen, monitorInformation);
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
        /// ホットキーを登録
        /// </summary>
        public void RegisterHotkeys()
        {
            UnregisterHotkeys();
            if (CheckIfTheProcessingIsValid())
            {
                int id = Common.EventHotkeysStartId;       // ホットキーの識別子
                foreach (EventItemInformation nowEII in Common.ApplicationData.Settings.EventInformation.Items)
                {
                    foreach (WindowProcessingInformation nowWPI in nowEII.WindowProcessingInformation)
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
            foreach (EventItemInformation nowEII in Common.ApplicationData.Settings.EventInformation.Items)
            {
                foreach (WindowProcessingInformation nowWPI in nowEII.WindowProcessingInformation)
                {
                    if (nowWPI.HotkeyId != -1)
                    {
                        try
                        {
                            NativeMethods.UnregisterHotKey(Common.ApplicationData.SystemTrayIconHwnd, nowWPI.HotkeyId);
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
        /// ホットキーを処理
        /// </summary>
        /// <param name="id">ホットキーの識別子</param>
        /// <param name="monitorInformation">モニター情報 (「null」でも可)</param>
        public void ProcessingHotkeys(
            int id,
            MonitorInformation monitorInformation
            )
        {
            try
            {
                HwndList hwndList = HwndList.GetWindowHandleList();        // ウィンドウハンドルのリスト

                foreach (System.IntPtr nowHwnd in hwndList.Hwnd)
                {
                    WINDOWPLACEMENT windowPlacement;
                    NativeMethods.GetWindowPlacement(nowHwnd, out windowPlacement);
                    WindowInformation windowInformation = WindowProcessing.GetWindowInformation(nowHwnd);

                    foreach (EventItemInformation nowEII in Common.ApplicationData.Settings.EventInformation.Items)
                    {
                        if (SpecifiedWindow.CheckWindowToProcessing(Common.ApplicationData.Settings.EventInformation.CaseSensitiveWindowQueries, nowEII, windowInformation, in windowPlacement))
                        {
                            foreach (WindowProcessingInformation nowWPI in nowEII.WindowProcessingInformation)
                            {
                                if (nowWPI.HotkeyId == id && ((nowEII.StandardDisplay == StandardDisplay.OnlySpecifiedDisplay) ? SpecifiedWindow.CheckDisplayToProcessing(nowEII, nowWPI, nowHwnd, monitorInformation) : true))
                                {
                                    SpecifiedWindow.ProcessingWindow(nowEII, nowWPI, nowHwnd, in windowPlacement, true, Common.ApplicationData.Settings.EventInformation.DoNotChangeOutOfScreen, monitorInformation);
                                    return;
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
}
