namespace Swindom.Source
{
    /// <summary>
    /// ウィンドウ処理の管理
    /// </summary>
    public class WindowProcessingManagement : System.IDisposable
    {
        /// <summary>
        /// Disposeが呼ばれたかの値
        /// </summary>
        private bool Disposed = false;
        /// <summary>
        /// イベントでウィンドウ処理
        /// </summary>
        public EventWindowProcessing EventWindowProcessing
        {
            get;
            private set;
        }
        /// <summary>
        /// タイマーでウィンドウ処理
        /// </summary>
        public TimerWindowProcessing TimerWindowProcessing
        {
            get;
            private set;
        }
        /// <summary>
        /// マグネット
        /// </summary>
        public MagnetProcessing Magnet
        {
            get;
            private set;
        }
        /// <summary>
        /// ホットキー
        /// </summary>
        public HotkeyProcessing Hotkey
        {
            get;
            private set;
        }
        /// <summary>
        /// ウィンドウが画面外の場合は画面内に移動
        /// </summary>
        public OutsideToInsideProcessing OutsideToInsideProcessing
        {
            get;
            private set;
        }
        /// <summary>
        /// ウィンドウを画面の中央に移動
        /// </summary>
        public MoveCenterProcessing MoveCenterProcessing
        {
            get;
            private set;
        }
        /// <summary>
        /// 全画面ウィンドウ判定のタイマー
        /// </summary>
        private System.Timers.Timer TimerForFullScreenJudgement;
        /// <summary>
        /// ホットキーを処理するかの値 (いいえ「false」/はい「true」)
        /// </summary>
        private bool DoHotkeyProcessing = true;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public WindowProcessingManagement()
        {
            SettingsTheProcessingStateOfEachFunction();
            SettingTimerFullScreen();
            SettingOutsideToInside();
            SettingMoveCenter();
            System.Windows.Interop.ComponentDispatcher.ThreadPreprocessMessage += ComponentDispatcher_ThreadPreprocessMessage;
            Common.ApplicationData.ProcessingEvent += ApplicationData_ProcessingEvent;
        }

        /// <summary>
        /// デストラクタ
        /// </summary>
        ~WindowProcessingManagement()
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
                System.Windows.Interop.ComponentDispatcher.ThreadPreprocessMessage -= ComponentDispatcher_ThreadPreprocessMessage;
                if (EventWindowProcessing != null)
                {
                    EventWindowProcessing.Dispose();
                    EventWindowProcessing = null;
                }
                if (TimerWindowProcessing != null)
                {
                    TimerWindowProcessing.Dispose();
                    TimerWindowProcessing = null;
                }
                if (Magnet != null)
                {
                    Magnet.Dispose();
                    Magnet = null;
                }
                if (Hotkey != null)
                {
                    Hotkey.Dispose();
                    Hotkey = null;
                }
                if (OutsideToInsideProcessing != null)
                {
                    OutsideToInsideProcessing.Dispose();
                    OutsideToInsideProcessing = null;
                }
                if (MoveCenterProcessing != null)
                {
                    MoveCenterProcessing.Dispose();
                    MoveCenterProcessing = null;
                }
                if (TimerForFullScreenJudgement != null)
                {
                    TimerForFullScreenJudgement.Stop();
                    TimerForFullScreenJudgement.Dispose();
                    TimerForFullScreenJudgement = null;
                }
            }
        }

        /// <summary>
        /// 「ThreadPreprocessMessage」メッセージ (ホットキー処理)
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="handled"></param>
        private void ComponentDispatcher_ThreadPreprocessMessage(
            ref System.Windows.Interop.MSG msg,
            ref bool handled
            )
        {
            if ((msg.message == (int)WindowMessage.WM_HOTKEY) && DoHotkeyProcessing)
            {
                MonitorInformation monitorInformation = MonitorInformation.GetMonitorInformation();
                EventWindowProcessing?.ProcessingHotkeys((int)msg.wParam, monitorInformation);
                TimerWindowProcessing?.ProcessingHotkeys((int)msg.wParam, monitorInformation);
                Hotkey?.ProcessingHotkeys((int)msg.wParam, monitorInformation);
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
                    case ProcessingEventType.StopWhenWindowIsFullScreenChanged:
                        SettingTimerFullScreen();
                        SettingsTheProcessingStateOfEachFunction();
                        break;
                    case ProcessingEventType.HotkeysDoNotStopFullScreenChanged:
                        SettingsTheProcessingStateOfEachFunction();
                        break;
                    case ProcessingEventType.EventProcessingStateChanged:
                        SettingEventWindowProcessing();
                        break;
                    case ProcessingEventType.TimerProcessingStateChanged:
                        SettingTimerWindowProcessing();
                        break;
                    case ProcessingEventType.MagnetProcessingStateChanged:
                        SettingMagnet();
                        break;
                    case ProcessingEventType.HotkeyValidState:
                        SettingHotkey();
                        break;
                    case ProcessingEventType.OutsideToInsideProcessingStateChanged:
                        SettingOutsideToInside();
                        break;
                    case ProcessingEventType.BatchProcessingOfEvent:
                        EventWindowProcessing.DecisionAndWindowProcessing(false);
                        break;
                    case ProcessingEventType.OnlyActiveWindowEvent:
                        EventWindowProcessing.DecisionAndWindowProcessing(true);
                        break;
                    case ProcessingEventType.BatchProcessingOfTimer:
                        TimerWindowProcessing.DecisionAndWindowProcessing(false);
                        break;
                    case ProcessingEventType.OnlyActiveWindowTimer:
                        TimerWindowProcessing.DecisionAndWindowProcessing(true);
                        break;
                    case ProcessingEventType.OutsideToInsideProcessingIntervalChanged:
                        SettingOutsideToInside();
                        break;
                    case ProcessingEventType.FullScreenWindowShowClose:
                        SettingsTheProcessingStateOfEachFunction();
                        break;
                    case ProcessingEventType.PauseHotkeyProcessing:
                        DoHotkeyProcessing = false;
                        break;
                    case ProcessingEventType.UnpauseHotkeyProcessing:
                        DoHotkeyProcessing = true;
                        break;
                    case ProcessingEventType.DoOutsideToInside:
                        OutsideToInsideProcessing.MoveWindowIntoScreen();
                        break;
                    case ProcessingEventType.MoveCenterProcessingStateChanged:
                        SettingMoveCenter();
                        break;
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// 全画面ウィンドウ判定のタイマーを設定
        /// </summary>
        private void SettingTimerFullScreen()
        {
            if ((Common.ApplicationData.Settings.EventInformation.Enabled && Common.ApplicationData.Settings.EventInformation.StopProcessingFullScreen)
                || (Common.ApplicationData.Settings.TimerInformation.Enabled && Common.ApplicationData.Settings.TimerInformation.StopProcessingFullScreen)
                || (Common.ApplicationData.Settings.MagnetInformation.Enabled && Common.ApplicationData.Settings.MagnetInformation.StopProcessingFullScreen)
                || (Common.ApplicationData.Settings.HotkeyInformation.Enabled && Common.ApplicationData.Settings.HotkeyInformation.StopProcessingFullScreen))
            {
                if (TimerForFullScreenJudgement == null)
                {
                    TimerForFullScreenJudgement = new()
                    {
                        Interval = Common.FullScreenWindowDecisionTimerInterval
                    };
                    TimerForFullScreenJudgement.Elapsed += (s, e) =>
                    {
                        System.Windows.Application.Current.Dispatcher.Invoke(new System.Action(() =>
                        {
                            WindowProcessing.CheckFullScreenWindow(null, null);
                        }));
                    };
                }
                TimerForFullScreenJudgement.Start();
            }
            else
            {
                if (TimerForFullScreenJudgement != null)
                {
                    TimerForFullScreenJudgement.Stop();
                    TimerForFullScreenJudgement.Dispose();
                    TimerForFullScreenJudgement = null;
                }
                Common.ApplicationData.FullScreenExists = false;
            }
        }

        /// <summary>
        /// 各機能の処理状態を設定
        /// </summary>
        private void SettingsTheProcessingStateOfEachFunction()
        {
            SettingEventWindowProcessing();
            SettingTimerWindowProcessing();
            SettingMagnet();
            SettingHotkey();
            SettingOutsideToInside();
        }

        /// <summary>
        /// 「イベント」の設定
        /// </summary>
        private void SettingEventWindowProcessing()
        {
            if (EventWindowProcessing.CheckIfTheProcessingIsValid())
            {
                if (EventWindowProcessing == null)
                {
                    EventWindowProcessing = new EventWindowProcessing();
                }
                else
                {
                    EventWindowProcessing.ProcessingSettings();
                }
            }
            else
            {
                if (EventWindowProcessing != null)
                {
                    EventWindowProcessing.Dispose();
                    EventWindowProcessing = null;
                }
            }
        }

        /// <summary>
        /// 「タイマー」の設定
        /// </summary>
        private void SettingTimerWindowProcessing()
        {
            if (TimerWindowProcessing.CheckIfTheProcessingIsValid())
            {
                if (TimerWindowProcessing == null)
                {
                    TimerWindowProcessing = new TimerWindowProcessing();
                }
                else
                {
                    TimerWindowProcessing.ProcessingSettings();
                }
            }
            else
            {
                if (TimerWindowProcessing != null)
                {
                    TimerWindowProcessing.Dispose();
                    TimerWindowProcessing = null;
                }
            }
        }

        /// <summary>
        /// 「マグネット」の設定
        /// </summary>
        private void SettingMagnet()
        {
            if (MagnetProcessing.CheckIfTheProcessingIsValid())
            {
                if (Magnet == null)
                {
                    Magnet = new MagnetProcessing();
                }
            }
            else
            {
                if (Magnet != null)
                {
                    Magnet.Dispose();
                    Magnet = null;
                }
            }
        }

        /// <summary>
        /// 「ホットキー」の設定
        /// </summary>
        private void SettingHotkey()
        {
            if (HotkeyProcessing.CheckIfTheProcessingIsValid())
            {
                if (Hotkey == null)
                {
                    Hotkey = new();
                }
            }
            else
            {
                if (Hotkey != null)
                {
                    Hotkey.Dispose();
                    Hotkey = null;
                }
            }
        }

        /// <summary>
        /// 「ウィンドウが画面外の場合は画面内に移動」の設定
        /// </summary>
        private void SettingOutsideToInside()
        {
            if (Common.ApplicationData.Settings.OutsideToInsideInformation.Enabled)
            {
                if (OutsideToInsideProcessing == null)
                {
                    OutsideToInsideProcessing = new();
                }
                else
                {
                    OutsideToInsideProcessing.ProcessingSettings();
                }
            }
            else
            {
                if (OutsideToInsideProcessing != null)
                {
                    OutsideToInsideProcessing.Dispose();
                    OutsideToInsideProcessing = null;
                }
            }
        }

        /// <summary>
        /// 「ウィンドウを画面の中央に移動」の設定
        /// </summary>
        private void SettingMoveCenter()
        {
            if (Common.ApplicationData.Settings.MoveCenterInformation.Enabled)
            {
                if (MoveCenterProcessing == null)
                {
                    MoveCenterProcessing = new();
                }
                else
                {
                    MoveCenterProcessing.ProcessingSettings();
                }
            }
            else
            {
                if (MoveCenterProcessing != null)
                {
                    MoveCenterProcessing.Dispose();
                    MoveCenterProcessing = null;
                }
            }
        }
    }
}
