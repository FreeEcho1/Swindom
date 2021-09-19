namespace Swindom.Source
{
    /// <summary>
    /// 「ウィンドウが画面外の場合は画面内に移動」処理
    /// </summary>
    public class OutsideToInsideProcessing : System.IDisposable
    {
        /// <summary>
        /// Disposeが呼ばれたかの値
        /// </summary>
        private bool Disposed;
        /// <summary>
        /// 処理のタイマー
        /// </summary>
        private System.Timers.Timer ProcessingTimer;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public OutsideToInsideProcessing()
        {
            ProcessingSettings();
            Common.ApplicationData.ProcessingEvent += ApplicationData_ProcessingEvent;
        }

        /// <summary>
        /// デストラクタ
        /// </summary>
        ~OutsideToInsideProcessing()
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
                if (ProcessingTimer != null)
                {
                    ProcessingTimer.Stop();
                    ProcessingTimer.Dispose();
                    ProcessingTimer = null;
                }
            }
        }

        /// <summary>
        /// 処理設定
        /// </summary>
        public void ProcessingSettings()
        {
            if (ProcessingTimer == null)
            {
                ProcessingTimer = new System.Timers.Timer
                {
                    Interval = Common.ApplicationData.Settings.OutsideToInsideInformation.ProcessingInterval
                };
                ProcessingTimer.Elapsed += ProcessingTimer_Elapsed;
                ProcessingTimer.Start();
            }
            else
            {
                ProcessingTimer.Interval = Common.ApplicationData.Settings.OutsideToInsideInformation.ProcessingInterval;
            }
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
                    case ProcessingEventType.OutsideToInsideProcessingIntervalChanged:
                        if (ProcessingTimer != null)
                        {
                            ProcessingTimer.Interval = Common.ApplicationData.Settings.OutsideToInsideInformation.ProcessingInterval;
                        }
                        break;
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// 「処理のタイマー」の「Elapsed」イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProcessingTimer_Elapsed(
            object sender,
            System.Timers.ElapsedEventArgs e
            )
        {
            try
            {
                MoveWindowIntoScreen();
            }
            catch
            {
            }
        }

        /// <summary>
        /// 画面外に存在するウィンドウを画面内に移動
        /// </summary>
        public static void MoveWindowIntoScreen()
        {
            MonitorInformation monitorInformation = MonitorInformation.GetMonitorInformation();

            if (monitorInformation.MonitorInfo.Count != 0)
            {
                HwndList hwndList = HwndList.GetWindowHandleList();

                foreach (System.IntPtr nowHwnd in hwndList.Hwnd)
                {
                    WINDOWPLACEMENT windowPlacement;
                    NativeMethods.GetWindowPlacement(nowHwnd, out windowPlacement);
                    RectangleDecimal windowRectangle = new(windowPlacement.rcNormalPosition.Left, windowPlacement.rcNormalPosition.Top, windowPlacement.rcNormalPosition.Right - windowPlacement.rcNormalPosition.Left, windowPlacement.rcNormalPosition.Bottom - windowPlacement.rcNormalPosition.Top);

                    if (WindowProcessing.CheckWindowIsInTheScreen(windowRectangle, monitorInformation) == false)
                    {
                        if (windowPlacement.showCmd != (int)SW.SW_SHOWNORMAL)
                        {
                            NativeMethods.ShowWindow(nowHwnd, (int)SW.SW_SHOWNORMAL);
                        }

                        MonitorInfoEx monitorInfo = monitorInformation.MonitorInfo[0];
                        foreach (MonitorInfoEx nowmonitorInfo in monitorInformation.MonitorInfo)
                        {
                            if (Common.ApplicationData.Settings.OutsideToInsideInformation.Display == nowmonitorInfo.DeviceName)
                            {
                                monitorInfo = nowmonitorInfo;
                                break;
                            }
                        }
                        NativeMethods.SetWindowPos(nowHwnd, (int)HwndInsertAfter.HWND_NOTOPMOST, monitorInfo.WorkArea.Left, monitorInfo.WorkArea.Top, 0, 0, (int)SWP.SWP_NOSIZE | (int)SWP.SWP_NOZORDER);
                    }
                }
            }
        }
    }
}
