using Swindom.Source.Settings;

namespace Swindom.Source
{
    /// <summary>
    /// 「ウィンドウを画面の中央に移動」処理
    /// </summary>
    public class MoveCenterProcessing : System.IDisposable
    {
        /// <summary>
        /// Disposeが呼ばれたかの値
        /// </summary>
        private bool Disposed;
        /// <summary>
        /// ウィンドウイベント
        /// </summary>
        private FreeEcho.FEWindowEvent.WindowEvent WindowEvent;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MoveCenterProcessing()
        {
            ProcessingSettings();
        }

        /// <summary>
        /// デストラクタ
        /// </summary>
        ~MoveCenterProcessing()
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
                DisposeWindowEvent();
            }
        }

        /// <summary>
        /// 処理設定
        /// </summary>
        public void ProcessingSettings()
        {
            if (WindowEvent == null)
            {
                WindowEvent = new();
                WindowEvent.WindowEventOccurrence += WindowEvent_WindowEventOccurrence;
                WindowEvent.Hook(FreeEcho.FEWindowEvent.HookWindowEventType.Create);
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
        /// ウィンドウのイベント
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
                MoveWindowToCenterOfScreen(e.Hwnd);
            }
            catch
            {
            }
        }

        /// <summary>
        /// ウィンドウを画面の中央に移動
        /// </summary>
        /// <param name="hwnd">ウィンドウハンドル</param>
        private void MoveWindowToCenterOfScreen(
            System.IntPtr hwnd
            )
        {
            try
            {
                hwnd = NativeMethods.GetAncestor(hwnd, GetAncestorFlags.GetRootOwner);
                WINDOWPLACEMENT windowPlacement;
                NativeMethods.GetWindowPlacement(hwnd, out windowPlacement);

                if (windowPlacement.showCmd == (int)SW.SW_SHOWNORMAL)
                {
                    // 処理しないウィンドウかを判定して、以降の処理はしない
                    WindowInformation windowInformation = WindowProcessing.GetWindowInformation(hwnd);
                    foreach (EventItemInformation nowEII in Common.ApplicationData.Settings.EventInformation.Items)
                    {
                        if (SpecifiedWindow.CheckWindowToProcessing(Common.ApplicationData.Settings.EventInformation.CaseSensitiveWindowQueries, nowEII, windowInformation, in windowPlacement))
                        {
                            return;
                        }
                    }
                    foreach (TimerItemInformation nowTII in Common.ApplicationData.Settings.TimerInformation.Items)
                    {
                        if (SpecifiedWindow.CheckWindowToProcessing(Common.ApplicationData.Settings.TimerInformation.CaseSensitiveWindowQueries, nowTII, windowInformation, in windowPlacement))
                        {
                            return;
                        }
                    }

                    MonitorInfoEx monitorInfo;
                    MonitorInformation.GetMonitorWithWindow(hwnd, out monitorInfo, null);
                    System.Drawing.Point windowPosition = new();
                    windowPosition.X = monitorInfo.WorkArea.Left + ((monitorInfo.WorkArea.Right - monitorInfo.WorkArea.Left) / 2) - ((windowPlacement.rcNormalPosition.Right - windowPlacement.rcNormalPosition.Left) / 2);
                    windowPosition.Y = monitorInfo.WorkArea.Top + ((monitorInfo.WorkArea.Bottom - monitorInfo.WorkArea.Top) / 2) - ((windowPlacement.rcNormalPosition.Bottom - windowPlacement.rcNormalPosition.Top) / 2);
                    NativeMethods.SetWindowPos(hwnd, (int)HwndInsertAfter.HWND_NOTOPMOST, windowPosition.X, windowPosition.Y, 0, 0, (int)SWP.SWP_NOZORDER | (int)SWP.SWP_NOSIZE);
                }
            }
            catch
            {
            }
        }
    }
}
