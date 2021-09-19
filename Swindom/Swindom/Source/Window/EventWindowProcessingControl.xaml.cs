using Swindom.Source.Settings;

namespace Swindom.Source.Window
{
    /// <summary>
    /// 「イベントでウィンドウ処理」のコントロール
    /// </summary>
    public partial class EventWindowProcessingControl : System.Windows.Controls.UserControl, System.IDisposable
    {
        /// <summary>
        /// Disposeが呼ばれたかの値
        /// </summary>
        private bool Disposed;
        /// <summary>
        /// 「指定ウィンドウ」の「追加/修正」ウィンドウ
        /// </summary>
        private AddModifyWindowSpecifiedWindow AddModifyWindowSpecifiedWindow;
        /// <summary>
        /// ListBoxItemの高さ
        /// </summary>
        private const int ListBoxItemHeight = 30;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public EventWindowProcessingControl()
        {
            InitializeComponent();

            SettingsLanguage();
            SettingsRowDefinition.Height = new(46);

            UpdateEventListBoxItems();
            SettingsControlEnabled();
            EventListBox.SelectionChanged += EventListBox_SelectionChanged;
            AddButton.Click += AddButton_Click;
            ModifyButton.Click += ModifyButton_Click;
            DeleteButton.Click += DeleteButton_Click;
            CopyButton.Click += CopyButton_Click;
            MoveUpButton.Click += MoveUpButton_Click;
            MoveDownButton.Click += MoveDownButton_Click;
            SettingsButton.Click += SettingsButton_Click;

            ProcessingStateToggleSwitch.IsOn = Common.ApplicationData.Settings.EventInformation.Enabled;
            RegisterMultipleToggleSwitch.IsOn = Common.ApplicationData.Settings.EventInformation.RegisterMultiple;
            CaseSensitiveWindowQueriesToggleSwitch.IsOn = Common.ApplicationData.Settings.EventInformation.CaseSensitiveWindowQueries;
            DoNotChangeOutOfScreenToggleSwitch.IsOn = Common.ApplicationData.Settings.EventInformation.DoNotChangeOutOfScreen;
            StopProcessingShowAddModifyToggleSwitch.IsOn = Common.ApplicationData.Settings.EventInformation.StopProcessingShowAddModifyWindow;
            StopProcessingFullScreenToggleSwitch.IsOn = Common.ApplicationData.Settings.EventInformation.StopProcessingFullScreen;
            HotkeysDoNotStopFullScreenToggleSwitch.IsOn = Common.ApplicationData.Settings.EventInformation.HotkeysDoNotStopFullScreen;
            ForegroundEventToggleSwitch.IsOn = Common.ApplicationData.Settings.EventInformation.EventTypeInformation.Foreground;
            MoveSizeEndEventToggleSwitch.IsOn = Common.ApplicationData.Settings.EventInformation.EventTypeInformation.MoveSizeEnd;
            MinimizeStartEventToggleSwitch.IsOn = Common.ApplicationData.Settings.EventInformation.EventTypeInformation.MinimizeStart;
            MinimizeEndEventToggleSwitch.IsOn = Common.ApplicationData.Settings.EventInformation.EventTypeInformation.MinimizeEnd;
            CreateEventToggleSwitch.IsOn = Common.ApplicationData.Settings.EventInformation.EventTypeInformation.Create;
            ShowEventToggleSwitch.IsOn = Common.ApplicationData.Settings.EventInformation.EventTypeInformation.Show;
            NameChangeEventToggleSwitch.IsOn = Common.ApplicationData.Settings.EventInformation.EventTypeInformation.NameChange;
            ProcessingStateToggleSwitch.IsOnChange += ProcessingStateToggleSwitch_IsOnChange;
            RegisterMultipleToggleSwitch.IsOnChange += RegisterMultipleToggleSwitch_IsOnChange;
            CaseSensitiveWindowQueriesToggleSwitch.IsOnChange += CaseSensitiveWindowQueriesToggleSwitch_IsOnChange;
            DoNotChangeOutOfScreenToggleSwitch.IsOnChange += DoNotChangeOutOfScreenToggleSwitch_IsOnChange;
            StopProcessingShowAddModifyToggleSwitch.IsOnChange += StopProcessingShowAddModifyToggleSwitch_IsOnChange;
            StopProcessingFullScreenToggleSwitch.IsOnChange += StopProcessingFullScreenToggleSwitch_IsOnChange;
            HotkeysDoNotStopFullScreenToggleSwitch.IsOnChange += HotkeysDoNotStopFullScreenToggleSwitch_IsOnChange;
            ForegroundEventToggleSwitch.IsOnChange += ForegroundEventToggleSwitch_IsOnChange;
            MoveSizeEndEventToggleSwitch.IsOnChange += MoveSizeEndEventToggleSwitch_IsOnChange;
            MinimizeStartEventToggleSwitch.IsOnChange += MinimizeStartEventToggleSwitch_IsOnChange;
            MinimizeEndEventToggleSwitch.IsOnChange += MinimizeEndEventToggleSwitch_IsOnChange;
            CreateEventToggleSwitch.IsOnChange += CreateEventToggleSwitch_IsOnChange;
            ShowEventToggleSwitch.IsOnChange += ShowEventToggleSwitch_IsOnChange;
            NameChangeEventToggleSwitch.IsOnChange += NameChangeEventToggleSwitch_IsOnChange;

            Common.ApplicationData.ProcessingEvent += ApplicationData_ProcessingEvent;
        }

        /// <summary>
        /// デストラクタ
        /// </summary>
        ~EventWindowProcessingControl()
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
        /// 「イベント」ListBoxの「SelectionChanged」イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void EventListBox_SelectionChanged(
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
                    AddModifyWindowSpecifiedWindow = new(System.Windows.Window.GetWindow(this), Common.ApplicationData.Settings.EventInformation);
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
                    if (EventListBox.SelectedItems.Count == 1)
                    {
                        AddModifyWindowSpecifiedWindow = new(System.Windows.Window.GetWindow(this), Common.ApplicationData.Settings.EventInformation, EventListBox.SelectedIndex);
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
        /// 「指定ウィンドウ」の「追加/修正」ウィンドウの「Closed」イベント
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
                        int selectedIndex = EventListBox.SelectedIndex;      // 選択している項目のインデックス
                        UpdateEventListBoxItems();
                        EventListBox.SelectedIndex = selectedIndex;
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
                if (EventListBox.SelectedItems.Count == 1)
                {
                    if (FreeEcho.FEControls.MessageBox.Show(Common.ApplicationData.Languages.Check, Common.ApplicationData.Settings.EventInformation.Items[EventListBox.SelectedIndex].RegisteredName + "\n" + Common.ApplicationData.Languages.LanguagesWindow.AllowDelete, FreeEcho.FEControls.MessageBoxButton.YesNo, System.Windows.Window.GetWindow(this)) == FreeEcho.FEControls.MessageBoxResult.Yes)
                    {
                        Common.ApplicationData.WindowProcessingManagement.EventWindowProcessing?.UnregisterHotkeys();
                        try
                        {
                            Common.ApplicationData.Settings.EventInformation.Items.RemoveAt(EventListBox.SelectedIndex);
                            EventListBox.Items.RemoveAt(EventListBox.SelectedIndex);
                            FreeEcho.FEControls.MessageBox.Show(Common.ApplicationData.Languages.Check, Common.ApplicationData.Languages.LanguagesWindow.Deleted, FreeEcho.FEControls.MessageBoxButton.Ok, System.Windows.Window.GetWindow(this));
                            EventListBox.Focus();
                        }
                        catch
                        {
                            FreeEcho.FEControls.MessageBox.Show(Common.ApplicationData.Languages.Check, Common.ApplicationData.Languages.ErrorOccurred, FreeEcho.FEControls.MessageBoxButton.Ok, System.Windows.Window.GetWindow(this));
                        }
                        Common.ApplicationData.WindowProcessingManagement.EventWindowProcessing?.RegisterHotkeys();
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
                if (EventListBox.SelectedItems.Count == 1)
                {
                    EventItemInformation newItem = new(Common.ApplicationData.Settings.EventInformation.Items[EventListBox.SelectedIndex], false);        // 新しいEventItemInformation
                    int number = 1;     // 番号
                    for (int count = 0; count < Common.ApplicationData.Settings.EventInformation.Items.Count; count++)
                    {
                        if (Common.ApplicationData.Settings.EventInformation.Items[count].RegisteredName == (newItem.RegisteredName + Common.SeparateString + Common.ApplicationData.Languages.LanguagesWindow.Copy + " " + number))
                        {
                            // 番号を変えて最初から確認
                            count = 0;
                            number++;
                        }
                    }
                    newItem.RegisteredName += Common.SeparateString + Common.ApplicationData.Languages.LanguagesWindow.Copy + " " + number;
                    int selectedIindex = EventListBox.SelectedIndex;      // 選択している項目のインデックス
                    Common.ApplicationData.Settings.EventInformation.Items.Add(newItem);
                    UpdateEventListBoxItems();
                    EventListBox.SelectedIndex = selectedIindex;
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
                if (EventListBox.SelectedIndex != 0)
                {
                    int selectedIndex = EventListBox.SelectedIndex;      // 選択している項目のインデックス
                    Common.ApplicationData.Settings.EventInformation.Items.Reverse(selectedIndex - 1, 2);
                    UpdateEventListBoxItems();
                    EventListBox.SelectedIndex = selectedIndex - 1;
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
                if ((EventListBox.SelectedIndex + 1) != EventListBox.Items.Count)
                {
                    int selectedIndex = EventListBox.SelectedIndex;      // 選択している項目のインデックス
                    Common.ApplicationData.Settings.EventInformation.Items.Reverse(selectedIndex, 2);
                    UpdateEventListBoxItems();
                    EventListBox.SelectedIndex = selectedIndex + 1;
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
                    case ProcessingEventType.EventProcessingStateChanged:
                        ProcessingStateToggleSwitch.IsOnDoNotEvent = Common.ApplicationData.Settings.EventInformation.Enabled;
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
        /// 「イベント」ListBoxの「CheckStateChanged」イベント
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
                for (int count = 0; count < EventListBox.Items.Count; count++)
                {
                    if (EventListBox.Items[count] == (FreeEcho.FEControls.CheckListBoxItem)sender)
                    {
                        Common.ApplicationData.Settings.EventInformation.Items[count].Enabled = ((FreeEcho.FEControls.CheckListBoxItem)EventListBox.Items[count]).IsChecked;
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
                Common.ApplicationData.Settings.EventInformation.Enabled = ProcessingStateToggleSwitch.IsOn;
                Common.ApplicationData.DoProcessingEvent(ProcessingEventType.EventProcessingStateChanged);
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
        private void RegisterMultipleToggleSwitch_IsOnChange(
            object sender,
            FreeEcho.FEControls.ToggleSwitchEventArgs e
            )
        {
            try
            {
                Common.ApplicationData.Settings.EventInformation.RegisterMultiple = RegisterMultipleToggleSwitch.IsOn;
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
                Common.ApplicationData.Settings.EventInformation.CaseSensitiveWindowQueries = CaseSensitiveWindowQueriesToggleSwitch.IsOn;
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
                Common.ApplicationData.Settings.EventInformation.DoNotChangeOutOfScreen = DoNotChangeOutOfScreenToggleSwitch.IsOn;
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
        private void StopProcessingShowAddModifyToggleSwitch_IsOnChange(
            object sender,
            FreeEcho.FEControls.ToggleSwitchEventArgs e
            )
        {
            try
            {
                Common.ApplicationData.Settings.EventInformation.StopProcessingShowAddModifyWindow = StopProcessingShowAddModifyToggleSwitch.IsOn;
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
                Common.ApplicationData.Settings.EventInformation.StopProcessingFullScreen = StopProcessingFullScreenToggleSwitch.IsOn;
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
                Common.ApplicationData.Settings.EventInformation.HotkeysDoNotStopFullScreen = HotkeysDoNotStopFullScreenToggleSwitch.IsOn;
                Common.ApplicationData.DoProcessingEvent(ProcessingEventType.HotkeysDoNotStopFullScreenChanged);
            }
            catch
            {
            }
        }

        /// <summary>
        /// 「フォアグラウンドが変更された」ToggleSwitchの「IsOnChange」イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ForegroundEventToggleSwitch_IsOnChange(
            object sender,
            FreeEcho.FEControls.ToggleSwitchEventArgs e
            )
        {
            try
            {
                Common.ApplicationData.Settings.EventInformation.EventTypeInformation.Foreground = ForegroundEventToggleSwitch.IsOn;
                Common.ApplicationData.DoProcessingEvent(ProcessingEventType.ReceiveEventChanged);
            }
            catch
            {
            }
        }

        /// <summary>
        /// 「移動及びサイズの変更が終了された」ToggleSwitchの「IsOnChange」イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MoveSizeEndEventToggleSwitch_IsOnChange(
            object sender,
            FreeEcho.FEControls.ToggleSwitchEventArgs e
            )
        {
            try
            {
                Common.ApplicationData.Settings.EventInformation.EventTypeInformation.MoveSizeEnd = MoveSizeEndEventToggleSwitch.IsOn;
                Common.ApplicationData.DoProcessingEvent(ProcessingEventType.ReceiveEventChanged);
            }
            catch
            {
            }
        }

        /// <summary>
        /// 「最小化が開始された」ToggleSwitchの「IsOnChange」イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MinimizeStartEventToggleSwitch_IsOnChange(
            object sender,
            FreeEcho.FEControls.ToggleSwitchEventArgs e
            )
        {
            try
            {
                Common.ApplicationData.Settings.EventInformation.EventTypeInformation.MinimizeStart = MinimizeStartEventToggleSwitch.IsOn;
                Common.ApplicationData.DoProcessingEvent(ProcessingEventType.ReceiveEventChanged);
            }
            catch
            {
            }
        }

        /// <summary>
        /// 「最小化が終了された」ToggleSwitchの「IsOnChange」イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MinimizeEndEventToggleSwitch_IsOnChange(
            object sender,
            FreeEcho.FEControls.ToggleSwitchEventArgs e
            )
        {
            try
            {
                Common.ApplicationData.Settings.EventInformation.EventTypeInformation.MinimizeEnd = MinimizeEndEventToggleSwitch.IsOn;
                Common.ApplicationData.DoProcessingEvent(ProcessingEventType.ReceiveEventChanged);
            }
            catch
            {
            }
        }

        /// <summary>
        /// 「作成された」ToggleSwitchの「IsOnChange」イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateEventToggleSwitch_IsOnChange(
            object sender,
            FreeEcho.FEControls.ToggleSwitchEventArgs e
            )
        {
            try
            {
                Common.ApplicationData.Settings.EventInformation.EventTypeInformation.Create = CreateEventToggleSwitch.IsOn;
                Common.ApplicationData.DoProcessingEvent(ProcessingEventType.ReceiveEventChanged);
            }
            catch
            {
            }
        }

        /// <summary>
        /// 「表示された」ToggleSwitchの「IsOnChange」イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowEventToggleSwitch_IsOnChange(
            object sender,
            FreeEcho.FEControls.ToggleSwitchEventArgs e
            )
        {
            try
            {
                Common.ApplicationData.Settings.EventInformation.EventTypeInformation.Show = ShowEventToggleSwitch.IsOn;
                Common.ApplicationData.DoProcessingEvent(ProcessingEventType.ReceiveEventChanged);
            }
            catch
            {
            }
        }

        /// <summary>
        /// 「名前が変更された」ToggleSwitchの「IsOnChange」イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NameChangeEventToggleSwitch_IsOnChange(
            object sender,
            FreeEcho.FEControls.ToggleSwitchEventArgs e
            )
        {
            try
            {
                Common.ApplicationData.Settings.EventInformation.EventTypeInformation.NameChange = NameChangeEventToggleSwitch.IsOn;
                Common.ApplicationData.DoProcessingEvent(ProcessingEventType.ReceiveEventChanged);
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
                ExplanationTextBlock.Text = Common.ApplicationData.Languages.LanguagesWindow.ExplanationOfTheEvent;

                ProcessingStateToggleSwitch.Text = Common.ApplicationData.Languages.LanguagesWindow.ProcessingState;
                RegisterMultipleToggleSwitch.Text = Common.ApplicationData.Languages.LanguagesWindow.RegisterMultipleWindowActions;
                CaseSensitiveWindowQueriesToggleSwitch.Text = Common.ApplicationData.Languages.LanguagesWindow.CaseSensitiveWindowQueries;
                DoNotChangeOutOfScreenToggleSwitch.Text = Common.ApplicationData.Languages.LanguagesWindow.DoNotChangePositionSizeOutOfScreen;
                StopProcessingShowAddModifyToggleSwitch.Text = Common.ApplicationData.Languages.LanguagesWindow.StopProcessingShowAddModify;
                StopProcessingFullScreenToggleSwitch.Text = Common.ApplicationData.Languages.LanguagesWindow.StopProcessingWhenWindowIsFullScreen;
                HotkeysDoNotStopFullScreenToggleSwitch.Text = Common.ApplicationData.Languages.LanguagesWindow.HotkeysDoNotStop;
                TypesOfEventsToEnableGroupBox.Header = Common.ApplicationData.Languages.LanguagesWindow.TypesOfEventsToEnable;
                ForegroundEventToggleSwitch.Text = Common.ApplicationData.Languages.LanguagesWindow.TheForegroundHasBeenChanged;
                MoveSizeEndEventToggleSwitch.Text = Common.ApplicationData.Languages.LanguagesWindow.MoveSizeChangeEnd;
                MinimizeStartEventToggleSwitch.Text = Common.ApplicationData.Languages.LanguagesWindow.MinimizeStart;
                MinimizeEndEventToggleSwitch.Text = Common.ApplicationData.Languages.LanguagesWindow.MinimizeEnd;
                CreateEventToggleSwitch.Text = Common.ApplicationData.Languages.LanguagesWindow.Create;
                ShowEventToggleSwitch.Text = Common.ApplicationData.Languages.LanguagesWindow.Show;
                NameChangeEventToggleSwitch.Text = Common.ApplicationData.Languages.LanguagesWindow.NameChanged;
            }
            catch
            {
            }
        }

        /// <summary>
        /// 「イベント」ListBoxの項目を更新
        /// </summary>
        private void UpdateEventListBoxItems()
        {
            EventListBox.Items.Clear();

            if (Common.ApplicationData.Settings.EventInformation.Items.Count != 0)
            {
                foreach (EventItemInformation nowEII in Common.ApplicationData.Settings.EventInformation.Items)
                {
                    FreeEcho.FEControls.CheckListBoxItem newItem = new()
                    {
                        Text = nowEII.RegisteredName,
                        Height = ListBoxItemHeight,
                        IsChecked = nowEII.Enabled
                    };
                    newItem.CheckStateChanged += ListBoxSpecifiedWindow_Item_CheckStateChanged;
                    EventListBox.Items.Add(newItem);
                }
            }
        }

        /// <summary>
        /// コントロールの有効状態を設定
        /// </summary>
        private void SettingsControlEnabled()
        {
            if (EventListBox.SelectedItems.Count == 0)
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
                MoveUpButton.IsEnabled = EventListBox.SelectedIndex != 0;
                MoveDownButton.IsEnabled = EventListBox.Items.Count != (EventListBox.SelectedIndex + 1);
                CopyButton.IsEnabled = true;
            }
        }
    }
}
