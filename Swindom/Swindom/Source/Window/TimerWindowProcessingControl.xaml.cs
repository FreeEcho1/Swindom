using Swindom.Source.Settings;

namespace Swindom.Source.Window
{
    /// <summary>
    /// 「タイマーでウィンドウ処理」のコントロール
    /// </summary>
    public partial class TimerWindowProcessingControl : System.Windows.Controls.UserControl, System.IDisposable
    {
        /// <summary>
        /// Disposeが呼ばれたかの値
        /// </summary>
        private bool Disposed;
        /// <summary>
        /// 「追加/修正」ウィンドウ
        /// </summary>
        private AddModifyWindowSpecifiedWindow AddModifyWindowSpecifiedWindow;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TimerWindowProcessingControl()
        {
            InitializeComponent();

            SettingsRowDefinition.Height = new(46);
            SettingsLanguage();

            // 項目部分
            UpdateTimerListBoxItems();
            SettingsControlEnabled();
            TimerListBox.SelectionChanged += TimerListBox_SelectionChanged;
            AddButton.Click += AddButton_Click;
            ModifyButton.Click += ModifyButton_Click;
            DeleteButton.Click += DeleteButton_Click;
            CopyButton.Click += CopyButton_Click;
            MoveUpButton.Click += MoveUpButton_Click;
            MoveDownButton.Click += MoveDownButton_Click;
            SettingsButton.Click += SettingsButton_Click;

            // 設定部分
            ProcessingStateToggleSwitch.IsOn = Common.ApplicationData.Settings.TimerInformation.Enabled;
            RegisterMultipleWindowActionsToggleSwitch.IsOn = Common.ApplicationData.Settings.TimerInformation.RegisterMultiple;
            ProcessingIntervalNumericUpDown.ValueInt = Common.ApplicationData.Settings.TimerInformation.ProcessingInterval;
            ProcessingWindowRangeComboBox.SelectedIndex = (int)Common.ApplicationData.Settings.TimerInformation.ProcessingWindowRange;
            if (Common.ApplicationData.Settings.TimerInformation.ProcessingWindowRange == ProcessingWindowRange.ActiveOnly)
            {
                WaitTimeToProcessingNextWindowLabel.IsEnabled = false;
                WaitTimeToProcessingNextWindowNumericUpDown.IsEnabled = false;
            }
            WaitTimeToProcessingNextWindowNumericUpDown.ValueInt = Common.ApplicationData.Settings.TimerInformation.WaitTimeToProcessingNextWindow;
            CaseSensitiveWindowQueriesToggleSwitch.IsOn = Common.ApplicationData.Settings.TimerInformation.CaseSensitiveWindowQueries;
            DoNotChangeOutOfScreenToggleSwitch.IsOn = Common.ApplicationData.Settings.TimerInformation.DoNotChangeOutOfScreen;
            StopProcessingShowAddModifyWindowToggleSwitch.IsOn = Common.ApplicationData.Settings.TimerInformation.StopProcessingShowAddModifyWindow;
            StopProcessingFullScreenToggleSwitch.IsOn = Common.ApplicationData.Settings.TimerInformation.StopProcessingFullScreen;
            HotkeysDoNotStopFullScreenToggleSwitch.IsOn = Common.ApplicationData.Settings.TimerInformation.HotkeysDoNotStopFullScreen;
            ProcessingStateToggleSwitch.IsOnChange += ProcessingStateToggleSwitch_IsOnChange;
            RegisterMultipleWindowActionsToggleSwitch.IsOnChange += RegisterMultipleWindowActionsToggleSwitch_IsOnChange;
            ProcessingIntervalNumericUpDown.ChangeValue += ProcessingIntervalNumericUpDown_ChangeValue;
            ProcessingWindowRangeComboBox.SelectionChanged += ProcessingWindowRangeComboBox_SelectionChanged;
            WaitTimeToProcessingNextWindowNumericUpDown.ChangeValue += WaitTimeToProcessingNextWindowNumericUpDown_ChangeValue;
            CaseSensitiveWindowQueriesToggleSwitch.IsOnChange += CaseSensitiveWindowQueriesToggleSwitch_IsOnChange;
            DoNotChangeOutOfScreenToggleSwitch.IsOnChange += DoNotChangeOutOfScreenToggleSwitch_IsOnChange;
            StopProcessingShowAddModifyWindowToggleSwitch.IsOnChange += StopProcessingShowAddModifyWindowToggleSwitch_IsOnChange;
            StopProcessingFullScreenToggleSwitch.IsOnChange += StopProcessingFullScreenToggleSwitch_IsOnChange;
            HotkeysDoNotStopFullScreenToggleSwitch.IsOnChange += HotkeysDoNotStopFullScreenToggleSwitch_IsOnChange;

            Common.ApplicationData.ProcessingEvent += ApplicationData_ProcessingEvent;
        }

        /// <summary>
        /// デストラクタ
        /// </summary>
        ~TimerWindowProcessingControl()
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
            }
        }

        /// <summary>
        /// 「タイマー」ListBoxの「SelectionChanged」イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TimerListBox_SelectionChanged(
            object sender,
            System.Windows.Controls.SelectionChangedEventArgs e
            )
        {
            try
            {
                SettingsControlEnabled();
            }
            catch
            {
            }
        }

        /// <summary>
        /// 「追加」Buttonの「Click」イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddButton_Click(
            object sender,
            System.Windows.RoutedEventArgs e
            )
        {
            try
            {
                if (AddModifyWindowSpecifiedWindow == null)
                {
                    AddModifyWindowSpecifiedWindow = new(System.Windows.Window.GetWindow(this), Common.ApplicationData.Settings.TimerInformation);
                    AddModifyWindowSpecifiedWindow.Closed += AddModifyWindowSpecifiedWindow_Closed;
                    AddModifyWindowSpecifiedWindow.Show();
                }
                else
                {
                    AddModifyWindowSpecifiedWindow.Activate();
                }
            }
            catch
            {
                FreeEcho.FEControls.MessageBox.Show(Common.ApplicationData.Languages.Check, Common.ApplicationData.Languages.ErrorOccurred, FreeEcho.FEControls.MessageBoxButton.Ok, System.Windows.Window.GetWindow(this));
            }
        }

        /// <summary>
        /// 「修正」Buttonの「Click」イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ModifyButton_Click(
            object sender,
            System.Windows.RoutedEventArgs e
            )
        {
            try
            {
                if (AddModifyWindowSpecifiedWindow == null)
                {
                    if (TimerListBox.SelectedItems.Count == 1)
                    {
                        AddModifyWindowSpecifiedWindow = new(System.Windows.Window.GetWindow(this), Common.ApplicationData.Settings.TimerInformation, TimerListBox.SelectedIndex);
                        AddModifyWindowSpecifiedWindow.Closed += AddModifyWindowSpecifiedWindow_Closed;
                        AddModifyWindowSpecifiedWindow.Show();
                    }
                }
                else
                {
                    AddModifyWindowSpecifiedWindow.Activate();
                }
            }
            catch
            {
                FreeEcho.FEControls.MessageBox.Show(Common.ApplicationData.Languages.Check, Common.ApplicationData.Languages.ErrorOccurred, FreeEcho.FEControls.MessageBoxButton.Ok, System.Windows.Window.GetWindow(this));
            }
        }

        /// <summary>
        /// 「追加/修正」ウィンドウの「Closed」イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddModifyWindowSpecifiedWindow_Closed(
            object sender,
            System.EventArgs e
            )
        {
            try
            {
                if (AddModifyWindowSpecifiedWindow != null)
                {
                    if (AddModifyWindowSpecifiedWindow.AddedOrModified)
                    {
                        int selectedIndex = TimerListBox.SelectedIndex;      // 選択している項目のインデックス
                        UpdateTimerListBoxItems();
                        TimerListBox.SelectedIndex = selectedIndex;
                        SettingsControlEnabled();
                    }
                    AddModifyWindowSpecifiedWindow.Owner.Activate();
                    AddModifyWindowSpecifiedWindow = null;
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// 「削除」Buttonの「Click」イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteButton_Click(
            object sender,
            System.Windows.RoutedEventArgs e
            )
        {
            try
            {
                if (TimerListBox.SelectedItems.Count == 1)
                {
                    if (FreeEcho.FEControls.MessageBox.Show(Common.ApplicationData.Languages.Check, Common.ApplicationData.Settings.TimerInformation.Items[TimerListBox.SelectedIndex].RegisteredName + "\n" + Common.ApplicationData.Languages.LanguagesWindow.AllowDelete, FreeEcho.FEControls.MessageBoxButton.YesNo, System.Windows.Window.GetWindow(this)) == FreeEcho.FEControls.MessageBoxResult.Yes)
                    {
                        Common.ApplicationData.WindowProcessingManagement.TimerWindowProcessing?.UnregisterHotkeys();
                        try
                        {
                            Common.ApplicationData.Settings.TimerInformation.Items.RemoveAt(TimerListBox.SelectedIndex);
                            TimerListBox.Items.RemoveAt(TimerListBox.SelectedIndex);
                            FreeEcho.FEControls.MessageBox.Show(Common.ApplicationData.Languages.Check, Common.ApplicationData.Languages.LanguagesWindow.Deleted, FreeEcho.FEControls.MessageBoxButton.Ok, System.Windows.Window.GetWindow(this));
                            TimerListBox.Focus();
                        }
                        catch
                        {
                            FreeEcho.FEControls.MessageBox.Show(Common.ApplicationData.Languages.Check, Common.ApplicationData.Languages.ErrorOccurred, FreeEcho.FEControls.MessageBoxButton.Ok, System.Windows.Window.GetWindow(this));
                        }
                        Common.ApplicationData.WindowProcessingManagement.TimerWindowProcessing?.RegisterHotkeys();
                    }
                }
            }
            catch
            {
                FreeEcho.FEControls.MessageBox.Show(Common.ApplicationData.Languages.Check, Common.ApplicationData.Languages.ErrorOccurred, FreeEcho.FEControls.MessageBoxButton.Ok, System.Windows.Window.GetWindow(this));
            }
        }

        /// <summary>
        /// 「コピー」Buttonの「Click」イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CopyButton_Click(
            object sender,
            System.Windows.RoutedEventArgs e
            )
        {
            try
            {
                if (TimerListBox.SelectedItems.Count == 1)
                {
                    TimerItemInformation newTII = new(Common.ApplicationData.Settings.TimerInformation.Items[TimerListBox.SelectedIndex], false);        // 新しいTimerItemInformation
                    int number = 1;     // 番号
                    for (int count = 0; count < Common.ApplicationData.Settings.TimerInformation.Items.Count; count++)
                    {
                        if (Common.ApplicationData.Settings.TimerInformation.Items[count].RegisteredName == (newTII.RegisteredName + Common.SeparateString + Common.ApplicationData.Languages.LanguagesWindow.Copy + " " + number))
                        {
                            count = 0;
                            number++;
                        }
                    }
                    newTII.RegisteredName += Common.SeparateString + Common.ApplicationData.Languages.LanguagesWindow.Copy + " " + number;
                    int selectedIndex = TimerListBox.SelectedIndex;      // 選択している項目のインデックス
                    Common.ApplicationData.Settings.TimerInformation.Items.Add(newTII);
                    UpdateTimerListBoxItems();
                    TimerListBox.SelectedIndex = selectedIndex;
                }
            }
            catch
            {
                FreeEcho.FEControls.MessageBox.Show(Common.ApplicationData.Languages.Check, Common.ApplicationData.Languages.ErrorOccurred, FreeEcho.FEControls.MessageBoxButton.Ok, System.Windows.Window.GetWindow(this));
            }
        }

        /// <summary>
        /// 「上に移動」Buttonの「Click」イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MoveUpButton_Click(
            object sender,
            System.Windows.RoutedEventArgs e
            )
        {
            try
            {
                if (TimerListBox.SelectedIndex != 0)
                {
                    int selectedIndex = TimerListBox.SelectedIndex;      // 選択している項目のインデックス
                    Common.ApplicationData.Settings.TimerInformation.Items.Reverse(selectedIndex - 1, 2);
                    UpdateTimerListBoxItems();
                    TimerListBox.SelectedIndex = selectedIndex - 1;
                }
            }
            catch
            {
                FreeEcho.FEControls.MessageBox.Show(Common.ApplicationData.Languages.Check, Common.ApplicationData.Languages.ErrorOccurred, FreeEcho.FEControls.MessageBoxButton.Ok, System.Windows.Window.GetWindow(this));
            }
        }

        /// <summary>
        /// 「下に移動」Buttonの「Click」イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MoveDownButton_Click(
            object sender,
            System.Windows.RoutedEventArgs e
            )
        {
            try
            {
                if ((TimerListBox.SelectedIndex + 1) != TimerListBox.Items.Count)
                {
                    int selectedIndex = TimerListBox.SelectedIndex;      // 選択している項目のインデックス
                    Common.ApplicationData.Settings.TimerInformation.Items.Reverse(selectedIndex, 2);
                    UpdateTimerListBoxItems();
                    TimerListBox.SelectedIndex = selectedIndex + 1;
                }
            }
            catch
            {
                FreeEcho.FEControls.MessageBox.Show(Common.ApplicationData.Languages.Check, Common.ApplicationData.Languages.ErrorOccurred, FreeEcho.FEControls.MessageBoxButton.Ok, System.Windows.Window.GetWindow(this));
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
                    case ProcessingEventType.TimerProcessingStateChanged:
                        ProcessingStateToggleSwitch.IsOnDoNotEvent = Common.ApplicationData.Settings.TimerInformation.Enabled;
                        break;
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
        /// 「タイマー」ListBoxの項目の「CheckStateChanged」イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ListBoxSpecifiedWindow_Item_CheckStateChanged(
            object sender,
            FreeEcho.FEControls.CheckListBoxItemEventArgs e
            )
        {
            try
            {
                for (int count = 0; count < TimerListBox.Items.Count; count++)
                {
                    if (TimerListBox.Items[count] == (FreeEcho.FEControls.CheckListBoxItem)sender)
                    {
                        Common.ApplicationData.Settings.TimerInformation.Items[count].Enabled = ((FreeEcho.FEControls.CheckListBoxItem)TimerListBox.Items[count]).IsChecked;
                        break;
                    }
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// 「処理状態」ToggleSwitchの「IsOnChange」イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProcessingStateToggleSwitch_IsOnChange(
            object sender,
            FreeEcho.FEControls.ToggleSwitchEventArgs e
            )
        {
            try
            {
                Common.ApplicationData.Settings.TimerInformation.Enabled = ProcessingStateToggleSwitch.IsOn;
                Common.ApplicationData.DoProcessingEvent(ProcessingEventType.TimerProcessingStateChanged);
            }
            catch
            {
                FreeEcho.FEControls.MessageBox.Show(Common.ApplicationData.Languages.Check, Common.ApplicationData.Languages.ErrorOccurred, FreeEcho.FEControls.MessageBoxButton.Ok, System.Windows.Window.GetWindow(this));
            }
        }

        /// <summary>
        /// 「設定」Buttonの「Click」イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SettingsButton_Click(
            object sender,
            System.Windows.RoutedEventArgs e
            )
        {
            try
            {
                if ((int)SettingsRowDefinition.Height.Value == Common.SettingsRowDefinitionMinimize)
                {
                    ItemsRowDefinition.Height = new(0);
                    SettingsRowDefinition.Height = new(1, System.Windows.GridUnitType.Star);
                    SettingsButton.ImageSource = new System.Windows.Media.Imaging.BitmapImage(new("pack://application:,,,/SwindomImage;component/Resources/CloseSettings.png", System.UriKind.Absolute));
                }
                else
                {
                    ItemsRowDefinition.Height = new(1, System.Windows.GridUnitType.Star);
                    SettingsRowDefinition.Height = new(Common.SettingsRowDefinitionMinimize);
                    SettingsButton.ImageSource = new System.Windows.Media.Imaging.BitmapImage(new("pack://application:,,,/SwindomImage;component/Resources/Settings.png", System.UriKind.Absolute));
                    SettingsScrollViewer.ScrollToVerticalOffset(0);
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// 「ウィンドウ処理を複数登録」ToggleSwitchの「IsOnChange」イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RegisterMultipleWindowActionsToggleSwitch_IsOnChange(
            object sender,
            FreeEcho.FEControls.ToggleSwitchEventArgs e
            )
        {
            try
            {
                Common.ApplicationData.Settings.TimerInformation.RegisterMultiple = RegisterMultipleWindowActionsToggleSwitch.IsOn;
            }
            catch
            {
            }
        }

        /// <summary>
        /// 「判定間隔 (ミリ秒)」NumericUpDownの「ChangeValue」イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProcessingIntervalNumericUpDown_ChangeValue(
            object sender,
            FreeEcho.FEControls.NumericUpDownChangeValueArgs e
            )
        {
            try
            {
                Common.ApplicationData.Settings.TimerInformation.ProcessingInterval = ProcessingIntervalNumericUpDown.ValueInt;
                Common.ApplicationData.DoProcessingEvent(ProcessingEventType.TimerProcessingInterval);
            }
            catch
            {
            }
        }

        /// <summary>
        /// 「処理するウィンドウの範囲」ComboBoxの「SelectionChanged」イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProcessingWindowRangeComboBox_SelectionChanged(
            object sender,
            System.Windows.Controls.SelectionChangedEventArgs e
            )
        {
            try
            {
                string stringData = (string)((System.Windows.Controls.ComboBoxItem)ProcessingWindowRangeComboBox.SelectedItem).Content;        // 選択項目の文字列

                if (stringData == Common.ApplicationData.Languages.LanguagesWindow.ActiveWindowOnly)
                {
                    Common.ApplicationData.Settings.TimerInformation.ProcessingWindowRange = ProcessingWindowRange.ActiveOnly;
                    WaitTimeToProcessingNextWindowLabel.IsEnabled = false;
                    WaitTimeToProcessingNextWindowNumericUpDown.IsEnabled = false;
                }
                else if (stringData == Common.ApplicationData.Languages.LanguagesWindow.AllWindow)
                {
                    Common.ApplicationData.Settings.TimerInformation.ProcessingWindowRange = ProcessingWindowRange.All;
                    WaitTimeToProcessingNextWindowLabel.IsEnabled = true;
                    WaitTimeToProcessingNextWindowNumericUpDown.IsEnabled = true;
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// 「次のウィンドウを処理する待ち時間 (ミリ秒)」NumericUpDownの「ChangeValue」イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WaitTimeToProcessingNextWindowNumericUpDown_ChangeValue(
            object sender,
            FreeEcho.FEControls.NumericUpDownChangeValueArgs e
            )
        {
            try
            {
                Common.ApplicationData.Settings.TimerInformation.WaitTimeToProcessingNextWindow = WaitTimeToProcessingNextWindowNumericUpDown.ValueInt;
            }
            catch
            {
            }
        }

        /// <summary>
        /// 「ウィンドウ判定で大文字と小文字を区別する」ToggleSwitchの「IsOnChange」イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CaseSensitiveWindowQueriesToggleSwitch_IsOnChange(
            object sender,
            FreeEcho.FEControls.ToggleSwitchEventArgs e
            )
        {
            try
            {
                Common.ApplicationData.Settings.TimerInformation.CaseSensitiveWindowQueries = CaseSensitiveWindowQueriesToggleSwitch.IsOn;
            }
            catch
            {
            }
        }

        /// <summary>
        /// 「画面外に出る場合は位置やサイズを変更しない」ToggleSwitchの「IsOnChange」イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DoNotChangeOutOfScreenToggleSwitch_IsOnChange(
            object sender,
            FreeEcho.FEControls.ToggleSwitchEventArgs e
            )
        {
            try
            {
                Common.ApplicationData.Settings.TimerInformation.DoNotChangeOutOfScreen = DoNotChangeOutOfScreenToggleSwitch.IsOn;
            }
            catch
            {
            }
        }

        /// <summary>
        /// 「追加/修正のウィンドウが表示されている場合は処理停止」ToggleSwitchの「IsOnChange」イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StopProcessingShowAddModifyWindowToggleSwitch_IsOnChange(
            object sender,
            FreeEcho.FEControls.ToggleSwitchEventArgs e
            )
        {
            try
            {
                Common.ApplicationData.Settings.TimerInformation.StopProcessingShowAddModifyWindow = StopProcessingShowAddModifyWindowToggleSwitch.IsOn;
            }
            catch
            {
            }
        }

        /// <summary>
        /// 「ウィンドウが全画面表示の場合は処理停止」ToggleSwitchの「IsOnChange」イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StopProcessingFullScreenToggleSwitch_IsOnChange(
            object sender,
            FreeEcho.FEControls.ToggleSwitchEventArgs e
            )
        {
            try
            {
                Common.ApplicationData.Settings.TimerInformation.StopProcessingFullScreen = StopProcessingFullScreenToggleSwitch.IsOn;
                Common.ApplicationData.DoProcessingEvent(ProcessingEventType.StopWhenWindowIsFullScreenChanged);
            }
            catch
            {
            }
        }

        /// <summary>
        /// 「ホットキーは停止させない (全画面ウィンドウがある場合)」ToggleSwitchの「IsOnChange」イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HotkeysDoNotStopFullScreenToggleSwitch_IsOnChange(
            object sender,
            FreeEcho.FEControls.ToggleSwitchEventArgs e
            )
        {
            try
            {
                Common.ApplicationData.Settings.TimerInformation.HotkeysDoNotStopFullScreen = HotkeysDoNotStopFullScreenToggleSwitch.IsOn;
                Common.ApplicationData.DoProcessingEvent(ProcessingEventType.HotkeysDoNotStopFullScreenChanged);
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
                AddButton.Text = Common.ApplicationData.Languages.LanguagesWindow.Add;
                ModifyButton.Text = Common.ApplicationData.Languages.LanguagesWindow.Modify;
                DeleteButton.Text = Common.ApplicationData.Languages.LanguagesWindow.Delete;
                MoveUpButton.Text = Common.ApplicationData.Languages.LanguagesWindow.MoveUp;
                MoveDownButton.Text = Common.ApplicationData.Languages.LanguagesWindow.MoveDown;
                CopyButton.Text = Common.ApplicationData.Languages.LanguagesWindow.Copy;
                SettingsButton.Text = Common.ApplicationData.Languages.LanguagesWindow.Setting;
                ExplanationTextBlock.Text = Common.ApplicationData.Languages.LanguagesWindow.ExplanationOfTheTimer;

                ProcessingStateToggleSwitch.Text = Common.ApplicationData.Languages.LanguagesWindow.ProcessingState;
                RegisterMultipleWindowActionsToggleSwitch.Text = Common.ApplicationData.Languages.LanguagesWindow.RegisterMultipleWindowActions;
                ProcessingIntervalLabel.Content = Common.ApplicationData.Languages.LanguagesWindow.ProcessingInterval;
                ProcessingWindowRangeLabel.Content = Common.ApplicationData.Languages.LanguagesWindow.ProcessingWindowRange;
                ((System.Windows.Controls.ComboBoxItem)ProcessingWindowRangeComboBox.Items[0]).Content = Common.ApplicationData.Languages.LanguagesWindow.ActiveWindowOnly;
                ((System.Windows.Controls.ComboBoxItem)ProcessingWindowRangeComboBox.Items[1]).Content = Common.ApplicationData.Languages.LanguagesWindow.AllWindow;
                WaitTimeToProcessingNextWindowLabel.Content = Common.ApplicationData.Languages.LanguagesWindow.WaitTimeToProcessingNextWindow;
                CaseSensitiveWindowQueriesToggleSwitch.Text = Common.ApplicationData.Languages.LanguagesWindow.CaseSensitiveWindowQueries;
                DoNotChangeOutOfScreenToggleSwitch.Text = Common.ApplicationData.Languages.LanguagesWindow.DoNotChangePositionSizeOutOfScreen;
                StopProcessingShowAddModifyWindowToggleSwitch.Text = Common.ApplicationData.Languages.LanguagesWindow.StopProcessingShowAddModify;
                StopProcessingFullScreenToggleSwitch.Text = Common.ApplicationData.Languages.LanguagesWindow.StopProcessingWhenWindowIsFullScreen;
                HotkeysDoNotStopFullScreenToggleSwitch.Text = Common.ApplicationData.Languages.LanguagesWindow.HotkeysDoNotStop;
            }
            catch
            {
            }
        }

        /// <summary>
        /// 「タイマー」ListBoxの項目を更新
        /// </summary>
        private void UpdateTimerListBoxItems()
        {
            TimerListBox.Items.Clear();

            if (Common.ApplicationData.Settings.TimerInformation.Items.Count != 0)
            {
                foreach (TimerItemInformation nowTII in Common.ApplicationData.Settings.TimerInformation.Items)
                {
                    FreeEcho.FEControls.CheckListBoxItem newItem = new()
                    {
                        Text = nowTII.RegisteredName,
                        Height = 30,
                        IsChecked = nowTII.Enabled
                    };
                    newItem.CheckStateChanged += ListBoxSpecifiedWindow_Item_CheckStateChanged;
                    TimerListBox.Items.Add(newItem);
                }
            }
        }

        /// <summary>
        /// コントロールの有効状態を設定
        /// </summary>
        private void SettingsControlEnabled()
        {
            if (TimerListBox.SelectedItems.Count == 0)
            {
                ModifyButton.IsEnabled = false;
                DeleteButton.IsEnabled = false;
                MoveUpButton.IsEnabled = false;
                MoveDownButton.IsEnabled = false;
                CopyButton.IsEnabled = false;
            }
            else
            {
                ModifyButton.IsEnabled = true;
                DeleteButton.IsEnabled = true;
                MoveUpButton.IsEnabled = TimerListBox.SelectedIndex != 0;
                MoveDownButton.IsEnabled = TimerListBox.Items.Count != (TimerListBox.SelectedIndex + 1);
                CopyButton.IsEnabled = true;
            }
        }
    }
}
