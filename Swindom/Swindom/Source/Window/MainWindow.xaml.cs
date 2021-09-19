namespace Swindom.Source.Window
{
    /// <summary>
    /// メインウィンドウ
    /// </summary>
    public partial class MainWindow : System.Windows.Window
    {
        /// <summary>
        /// コンストラクタ
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            Left = Common.ApplicationData.Settings.MainWindowRectangle.X;
            Top = Common.ApplicationData.Settings.MainWindowRectangle.Y;
            Width = Common.ApplicationData.Settings.MainWindowRectangle.Width;
            Height = Common.ApplicationData.Settings.MainWindowRectangle.Height;
            WindowState = Common.ApplicationData.Settings.WindowStateOfTheMainWindow;
            SettingsLanguage();

            Loaded += MainWindow_Loaded;
            Closed += MainWindow_Closed;
            Common.ApplicationData.ProcessingEvent += ApplicationData_ProcessingEvent;
        }

        /// <summary>
        /// ウィンドウの「Loaded」イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindow_Loaded(
            object sender,
            System.Windows.RoutedEventArgs e
            )
        {
            try
            {
                Activate();
            }
            catch
            {
            }
        }

        /// <summary>
        /// ウィンドウの「Closed」イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainWindow_Closed(
            object sender,
            System.EventArgs e
            )
        {
            try
            {
                Common.ApplicationData.ProcessingEvent -= ApplicationData_ProcessingEvent;

                Common.ApplicationData.Settings.MainWindowRectangle.X = (int)Left;
                Common.ApplicationData.Settings.MainWindowRectangle.Y = (int)Top;
                Common.ApplicationData.Settings.MainWindowRectangle.Width = (int)Width;
                Common.ApplicationData.Settings.MainWindowRectangle.Height = (int)Height;
                Common.ApplicationData.Settings.WindowStateOfTheMainWindow = WindowState;
                Common.ApplicationData.WriteSettings();
            }
            catch
            {
            }

            try
            {
                EventWindowProcessingControl.Dispose();
                TimerWindowProcessingControl.Dispose();
                MagnetControl.Dispose();
                HotkeyControl.Dispose();
                OtherFunctionControl.Dispose();
                SettingControl.Dispose();
                InformationControl.Dispose();
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
            object sender,
            ProcessingEventArgs e
            )
        {
            try
            {
                switch (e.ProcessingEventType)
                {
                    case ProcessingEventType.LanguageChanged:
                        SettingsLanguage();
                        break;
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// 言語設定
        /// </summary>
        private void SettingsLanguage()
        {
            try
            {
                EventWindowProcessingImageTabItem.Text = Common.ApplicationData.Languages.LanguagesWindow.Event;
                TimerWindowProcessingImageTabItem.Text = Common.ApplicationData.Languages.LanguagesWindow.Timer;
                MagnetProcessingImageTabItem.Text = Common.ApplicationData.Languages.LanguagesWindow.Magnet;
                HotkeyImageTabItem.Text = Common.ApplicationData.Languages.LanguagesWindow.Hotkey;
                OtherFunctionImageTabItem.Text = Common.ApplicationData.Languages.LanguagesWindow.Other;
                SettingImageTabItem.Text = Common.ApplicationData.Languages.LanguagesWindow.Setting;
                InformationImageTabItem.Text = Common.ApplicationData.Languages.LanguagesWindow.Information;
            }
            catch
            {
            }
        }
    }
}
