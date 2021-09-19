namespace Swindom.Source.Window
{
    /// <summary>
    /// 「その他の機能」コントロール
    /// </summary>
    public partial class OtherFunctionControl : System.Windows.Controls.UserControl, System.IDisposable
    {
        /// <summary>
        /// Disposeが呼ばれたかの値
        /// </summary>
        private bool Disposed = false;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public OtherFunctionControl()
        {
            InitializeComponent();

            MonitorInformation monitorInformation = MonitorInformation.GetMonitorInformation();
            foreach (MonitorInfoEx nowMonitorInfo in monitorInformation.MonitorInfo)
            {
                System.Windows.Controls.ComboBoxItem newItem = new()
                {
                    Content = nowMonitorInfo.DeviceName
                };
                OutsideToInsideProcessingDisplayComboBox.Items.Add(newItem);
            }
            if (monitorInformation.MonitorInfo.Count == 1)
            {
                OutsideToInsideProcessingDisplayLabel.Visibility = System.Windows.Visibility.Collapsed;
                OutsideToInsideProcessingDisplayComboBox.Visibility = System.Windows.Visibility.Collapsed;
            }

            SettingsLanguage();

            OutsideToInsideEnabledToggleSwitch.IsOn = Common.ApplicationData.Settings.OutsideToInsideInformation.Enabled;
            OutsideToInsideProcessingIntervalNumericUpDown.ValueInt = Common.ApplicationData.Settings.OutsideToInsideInformation.ProcessingInterval;
            if (string.IsNullOrEmpty(Common.ApplicationData.Settings.OutsideToInsideInformation.Display))
            {
                OutsideToInsideProcessingDisplayComboBox.SelectedIndex = 0;
            }
            else
            {
                Processing.SelectComboBoxItem(OutsideToInsideProcessingDisplayComboBox, Common.ApplicationData.Settings.OutsideToInsideInformation.Display);
            }
            MoveCenterEnabledToggleSwitch.IsOn = Common.ApplicationData.Settings.MoveCenterInformation.Enabled;

            OutsideToInsideEnabledToggleSwitch.IsOnChange += OutsideToInsideEnabledToggleSwitch_IsOnChange;
            OutsideToInsideProcessingIntervalNumericUpDown.ChangeValue += OutsideToInsideProcessingIntervalNumericUpDown_ChangeValue;
            OutsideToInsideProcessingDisplayComboBox.SelectionChanged += OutsideToInsideProcessingDisplayComboBox_SelectionChanged;
            MoveCenterEnabledToggleSwitch.IsOnChange += MoveCenterEnabledToggleSwitch_IsOnChange;

            Common.ApplicationData.ProcessingEvent += ApplicationData_ProcessingEvent;
        }

        /// <summary>
        /// デストラクタ
        /// </summary>
        ~OtherFunctionControl()
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
            }
        }

        /// <summary>
        /// 「ウィンドウが画面外の場合は画面内に移動」の「処理状態」ToggleSwitchの「IsOnChange」イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OutsideToInsideEnabledToggleSwitch_IsOnChange(
            object sender,
            FreeEcho.FEControls.ToggleSwitchEventArgs e
            )
        {
            try
            {
                Common.ApplicationData.Settings.OutsideToInsideInformation.Enabled = OutsideToInsideEnabledToggleSwitch.IsOn;
                Common.ApplicationData.DoProcessingEvent(ProcessingEventType.OutsideToInsideProcessingStateChanged);
            }
            catch
            {
            }
        }

        /// <summary>
        /// 「ウィンドウが画面外の場合は画面内に移動」の「処理間隔」NumericUpDownの「ChangeValue」イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OutsideToInsideProcessingIntervalNumericUpDown_ChangeValue(
            object sender,
            FreeEcho.FEControls.NumericUpDownChangeValueArgs e
            )
        {
            try
            {
                Common.ApplicationData.Settings.OutsideToInsideInformation.ProcessingInterval = OutsideToInsideProcessingIntervalNumericUpDown.ValueInt;
                Common.ApplicationData.DoProcessingEvent(ProcessingEventType.OutsideToInsideProcessingIntervalChanged);
            }
            catch
            {
            }
        }

        /// <summary>
        /// 「ウィンドウが画面外の場合は画面内に移動」の「ディスプレイ」ComboBoxの「SelectionChanged」イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OutsideToInsideProcessingDisplayComboBox_SelectionChanged(
            object sender,
            System.Windows.Controls.SelectionChangedEventArgs e
            )
        {
            try
            {
                Common.ApplicationData.Settings.OutsideToInsideInformation.Display = (string)((System.Windows.Controls.ComboBoxItem)OutsideToInsideProcessingDisplayComboBox.SelectedItem).Content;
            }
            catch
            {
            }
        }

        /// <summary>
        /// 「ウィンドウを画面の中央に移動」の「処理状態」の「IsOnChange」イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MoveCenterEnabledToggleSwitch_IsOnChange(
            object sender,
            FreeEcho.FEControls.ToggleSwitchEventArgs e
            )
        {
            try
            {
                Common.ApplicationData.Settings.MoveCenterInformation.Enabled = MoveCenterEnabledToggleSwitch.IsOn;
                Common.ApplicationData.DoProcessingEvent(ProcessingEventType.MoveCenterProcessingStateChanged);
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
                OutsideToInsideFunctionGroupBox.Header = Common.ApplicationData.Languages.LanguagesWindow.WindowMoveOutsideToInside;
                OutsideToInsideEnabledToggleSwitch.Text = Common.ApplicationData.Languages.LanguagesWindow.ProcessingState;
                OutsideToInsideProcessingIntervalLabel.Content = Common.ApplicationData.Languages.LanguagesWindow.ProcessingInterval;
                OutsideToInsideProcessingDisplayLabel.Content = Common.ApplicationData.Languages.LanguagesWindow.Display;
                MoveCenterFunctionGroupBox.Header = Common.ApplicationData.Languages.LanguagesWindow.MoveWindowToCenterOfScreen;
                MoveCenterEnabledToggleSwitch.Text = Common.ApplicationData.Languages.LanguagesWindow.ProcessingState;
            }
            catch
            {
            }
        }
    }
}
