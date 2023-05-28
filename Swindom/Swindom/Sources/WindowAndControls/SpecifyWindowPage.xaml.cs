namespace Swindom;

/// <summary>
/// 「指定ウィンドウ」ページ
/// </summary>
public partial class SpecifyWindowPage : Page
{
    /// <summary>
    /// 「指定ウィンドウ」の「追加/修正」ウィンドウ
    /// </summary>
    private SpecifyWindowItemWindow? SpecifyWindowItemWindow;
    /// <summary>
    /// 次のイベントをキャンセル
    /// </summary>
    private bool CancelNextEvent;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public SpecifyWindowPage()
    {
        InitializeComponent();

        if (ApplicationData.Languages.LanguagesWindow == null)
        {
            throw new Exception("Languages value is null. - SpecifyWindowPage()");
        }

        SettingsRowDefinition.Height = new(Common.SettingsRowDefinitionMinimize);
        SettingsScrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Disabled;
        SettingsControlsImage();
        SetTextOnControls();
        UpdateSpecifyWindowListBoxItems();
        SettingsSideButtonEnabled();
        SpecifyWindowListBox.SelectionChanged += SpecifyWindowListBox_SelectionChanged;
        AddButton.Click += AddButton_Click;
        ModifyButton.Click += ModifyButton_Click;
        DeleteButton.Click += DeleteButton_Click;
        CopyButton.Click += CopyButton_Click;
        MoveUpButton.Click += MoveUpButton_Click;
        MoveDownButton.Click += MoveDownButton_Click;
        SettingsButton.Click += SettingsButton_Click;

        ProcessingStateToggleSwitch.IsOn = ApplicationData.Settings.SpecifyWindowInformation.Enabled;
        RegisterMultipleToggleSwitch.IsOn = ApplicationData.Settings.SpecifyWindowInformation.MultipleRegistrations;
        CaseSensitiveWindowQueriesToggleSwitch.IsOn = ApplicationData.Settings.SpecifyWindowInformation.CaseSensitive;
        DoNotChangeOutOfScreenToggleSwitch.IsOn = ApplicationData.Settings.SpecifyWindowInformation.DoNotChangeOutOfScreen;
        StopProcessingShowAddModifyToggleSwitch.IsOn = ApplicationData.Settings.SpecifyWindowInformation.StopProcessingShowAddModifyWindow;
        StopProcessingFullScreenToggleSwitch.IsOn = ApplicationData.Settings.SpecifyWindowInformation.StopProcessingFullScreen;
        HotkeysDoNotStopFullScreenToggleSwitch.IsOn = ApplicationData.Settings.SpecifyWindowInformation.HotkeysDoNotStopFullScreen;
        ProcessingIntervalNumberBox.Minimum = SpecifyWindowInformation.MinimumProcessingInterval;
        ProcessingIntervalNumberBox.Maximum = SpecifyWindowInformation.MaximumProcessingInterval;
        ProcessingIntervalNumberBox.Value = ApplicationData.Settings.SpecifyWindowInformation.ProcessingInterval;
        string stringData;
        stringData = ApplicationData.Settings.SpecifyWindowInformation.ProcessingWindowRange switch
        {
            ProcessingWindowRange.All => ApplicationData.Languages.LanguagesWindow.AllWindow,
            _ => ApplicationData.Languages.LanguagesWindow.ActiveWindowOnly
        };
        VariousProcessing.SelectComboBoxItem(ProcessingWindowRangeComboBox, stringData);
        WaitTimeToProcessingNextWindowNumberBox.Minimum = SpecifyWindowInformation.MinimumWaitTimeToProcessingNextWindow;
        WaitTimeToProcessingNextWindowNumberBox.Maximum = SpecifyWindowInformation.MaximumWaitTimeToProcessingNextWindow;
        WaitTimeToProcessingNextWindowNumberBox.Value = ApplicationData.Settings.SpecifyWindowInformation.WaitTimeToProcessingNextWindow;
        ProcessingStateToggleSwitch.Toggled += ProcessingStateToggleSwitch_Toggled;
        RegisterMultipleToggleSwitch.Toggled += RegisterMultipleToggleSwitch_Toggled;
        CaseSensitiveWindowQueriesToggleSwitch.Toggled += CaseSensitiveWindowQueriesToggleSwitch_Toggled;
        DoNotChangeOutOfScreenToggleSwitch.Toggled += DoNotChangeOutOfScreenToggleSwitch_Toggled;
        StopProcessingShowAddModifyToggleSwitch.Toggled += StopProcessingShowAddModifyToggleSwitch_Toggled;
        StopProcessingFullScreenToggleSwitch.Toggled += StopProcessingFullScreenToggleSwitch_Toggled;
        HotkeysDoNotStopFullScreenToggleSwitch.Toggled += HotkeysDoNotStopFullScreenToggleSwitch_Toggled;
        ProcessingIntervalNumberBox.ValueChanged += ProcessingIntervalNumberBox_ValueChanged;
        ProcessingWindowRangeComboBox.SelectionChanged += ProcessingWindowRangeComboBox_SelectionChanged;
        WaitTimeToProcessingNextWindowNumberBox.ValueChanged += WaitTimeToProcessingNextWindowNumberBox_ValueChanged;

        ApplicationData.EventData.ProcessingEvent += ApplicationData_ProcessingEvent;
    }

    /// <summary>
    /// 破棄
    /// </summary>
    public void Release()
    {
        ApplicationData.EventData.ProcessingEvent -= ApplicationData_ProcessingEvent;
        SpecifyWindowItemWindow?.Close();
        SpecifyWindowItemWindow = null;
    }

    /// <summary>
    /// 「指定ウィンドウ」ListBoxの「SelectionChanged」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void SpecifyWindowListBox_SelectionChanged(
        object sender,
        SelectionChangedEventArgs e
        )
    {
        try
        {
            SettingsSideButtonEnabled();
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
        RoutedEventArgs e
        )
    {
        try
        {
            if (SpecifyWindowItemWindow == null)
            {
                SpecifyWindowItemWindow = new(Window.GetWindow(this));
                SpecifyWindowItemWindow.Closed += SpecifyWindowItemWindow_Closed;
                SpecifyWindowItemWindow.Show();
            }
            else
            {
                SpecifyWindowItemWindow.Activate();
            }
        }
        catch
        {
            FEMessageBox.Show(ApplicationData.Languages.ErrorOccurred, ApplicationData.Languages.Check, MessageBoxButton.OK);
        }
    }

    /// <summary>
    /// 「修正」Buttonの「Click」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ModifyButton_Click(
        object sender,
        RoutedEventArgs e
        )
    {
        try
        {
            if (SpecifyWindowItemWindow == null)
            {
                SpecifyWindowItemWindow = new(Window.GetWindow(this), SpecifyWindowListBox.SelectedIndex);
                SpecifyWindowItemWindow.Closed += SpecifyWindowItemWindow_Closed;
                SpecifyWindowItemWindow.Show();
            }
            else
            {
                SpecifyWindowItemWindow.Activate();
            }
        }
        catch
        {
            FEMessageBox.Show(ApplicationData.Languages.ErrorOccurred, ApplicationData.Languages.Check, MessageBoxButton.OK);
        }
    }

    /// <summary>
    /// 「指定ウィンドウ」の「追加/修正」ウィンドウの「Closed」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void SpecifyWindowItemWindow_Closed(
        object? sender,
        EventArgs e
        )
    {
        try
        {
            if (SpecifyWindowItemWindow != null)
            {
                if (SpecifyWindowItemWindow.AddedOrModified)
                {
                    int selectedIndex = SpecifyWindowListBox.SelectedIndex;      // 選択している項目のインデックス
                    UpdateSpecifyWindowListBoxItems();
                    SpecifyWindowListBox.SelectedIndex = selectedIndex;
                }
                SpecifyWindowItemWindow.Owner.Activate();
                SpecifyWindowItemWindow = null;
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
        RoutedEventArgs e
        )
    {
        try
        {
            if (FEMessageBox.Show(ApplicationData.Settings.SpecifyWindowInformation.Items[SpecifyWindowListBox.SelectedIndex].RegisteredName + Environment.NewLine + ApplicationData.Languages.LanguagesWindow?.AllowDelete, ApplicationData.Languages.Check, MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                ApplicationData.WindowProcessingManagement.SpecifyWindowProcessing?.UnregisterHotkeys();
                try
                {
                    ApplicationData.Settings.SpecifyWindowInformation.Items.RemoveAt(SpecifyWindowListBox.SelectedIndex);
                    SpecifyWindowListBox.Items.RemoveAt(SpecifyWindowListBox.SelectedIndex);
                    FEMessageBox.Show(ApplicationData.Languages.LanguagesWindow?.Deleted ?? "", ApplicationData.Languages.Check, MessageBoxButton.OK);
                    SpecifyWindowListBox.Focus();
                }
                catch
                {
                    FEMessageBox.Show(ApplicationData.Languages.ErrorOccurred, ApplicationData.Languages.Check, MessageBoxButton.OK);
                }
                ApplicationData.WindowProcessingManagement.SpecifyWindowProcessing?.RegisterHotkeys();
            }
        }
        catch
        {
            FEMessageBox.Show(ApplicationData.Languages.ErrorOccurred, ApplicationData.Languages.Check, MessageBoxButton.OK);
        }
    }

    /// <summary>
    /// 「コピー」Buttonの「Click」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void CopyButton_Click(
        object sender,
        RoutedEventArgs e
        )
    {
        try
        {
            SpecifyWindowItemInformation newItem = new(ApplicationData.Settings.SpecifyWindowInformation.Items[SpecifyWindowListBox.SelectedIndex], false);     // 新しい項目
            int number = 1;     // 番号
            for (int count = 0; count < ApplicationData.Settings.SpecifyWindowInformation.Items.Count; count++)
            {
                if (ApplicationData.Settings.SpecifyWindowInformation.Items[count].RegisteredName == (newItem.RegisteredName + Common.CopySeparateString + ApplicationData.Languages.LanguagesWindow?.Copy + Common.SpaceSeparateString + number))
                {
                    // 番号を変えて最初から確認
                    count = 0;
                    number++;
                }
            }
            newItem.RegisteredName += Common.CopySeparateString + ApplicationData.Languages.LanguagesWindow?.Copy + Common.SpaceSeparateString + number;
            int selectedIindex = SpecifyWindowListBox.SelectedIndex;      // 選択している項目のインデックス
            ApplicationData.Settings.SpecifyWindowInformation.Items.Add(newItem);
            UpdateSpecifyWindowListBoxItems();
            SpecifyWindowListBox.SelectedIndex = selectedIindex;
        }
        catch
        {
            FEMessageBox.Show(ApplicationData.Languages.ErrorOccurred, ApplicationData.Languages.Check, MessageBoxButton.OK);
        }
    }

    /// <summary>
    /// 「上に移動」Buttonの「Click」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void MoveUpButton_Click(
        object sender,
        RoutedEventArgs e
        )
    {
        try
        {
            int selectedIndex = SpecifyWindowListBox.SelectedIndex;      // 選択している項目のインデックス
            ApplicationData.Settings.SpecifyWindowInformation.Items.Reverse(selectedIndex - 1, 2);
            UpdateSpecifyWindowListBoxItems();
            SpecifyWindowListBox.SelectedIndex = selectedIndex - 1;
        }
        catch
        {
            FEMessageBox.Show(ApplicationData.Languages.ErrorOccurred, ApplicationData.Languages.Check, MessageBoxButton.OK);
        }
    }

    /// <summary>
    /// 「下に移動」Buttonの「Click」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void MoveDownButton_Click(
        object sender,
        RoutedEventArgs e
        )
    {
        try
        {
            int selectedIndex = SpecifyWindowListBox.SelectedIndex;      // 選択している項目のインデックス
            ApplicationData.Settings.SpecifyWindowInformation.Items.Reverse(selectedIndex, 2);
            UpdateSpecifyWindowListBoxItems();
            SpecifyWindowListBox.SelectedIndex = selectedIndex + 1;
        }
        catch
        {
            FEMessageBox.Show(ApplicationData.Languages.ErrorOccurred, ApplicationData.Languages.Check, MessageBoxButton.OK);
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
                case ProcessingEventType.SpecifyWindowProcessingStateChanged:
                    if (CancelNextEvent)
                    {
                        CancelNextEvent = false;
                    }
                    else
                    {
                        CancelNextEvent = true;
                        ProcessingStateToggleSwitch.IsOn = ApplicationData.Settings.SpecifyWindowInformation.Enabled;
                    }
                    break;
                case ProcessingEventType.LanguageChanged:
                    SetTextOnControls();
                    break;
                case ProcessingEventType.ThemeChanged:
                    SettingsControlsImage();
                    break;
                case ProcessingEventType.SpecifyWindowUpdateListBox:
                    UpdateSpecifyWindowListBoxItems();
                    break;
            }
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「指定ウィンドウ」CheckListBoxItemの「CheckStateChanged」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void SpecifyWindowCheckListBoxItem_CheckStateChanged(
        object sender,
        CheckListBoxItemEventArgs e
        )
    {
        try
        {
            for (int count = 0; count < SpecifyWindowListBox.Items.Count; count++)
            {
                if (SpecifyWindowListBox.Items[count] == (CheckListBoxItem)sender)
                {
                    ApplicationData.Settings.SpecifyWindowInformation.Items[count].Enabled = ((CheckListBoxItem)SpecifyWindowListBox.Items[count]).IsChecked;
                    break;
                }
            }
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「処理状態」ToggleSwitchの「Toggled」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ProcessingStateToggleSwitch_Toggled(
        object sender,
        RoutedEventArgs e
        )
    {
        try
        {
            if (CancelNextEvent)
            {
                CancelNextEvent = false;
            }
            else
            {
                CancelNextEvent = true;
                ApplicationData.Settings.SpecifyWindowInformation.Enabled = ProcessingStateToggleSwitch.IsOn;
                ApplicationData.EventData.DoProcessingEvent(ProcessingEventType.SpecifyWindowProcessingStateChanged);
            }
        }
        catch
        {
            FEMessageBox.Show(ApplicationData.Languages.ErrorOccurred, ApplicationData.Languages.Check, MessageBoxButton.OK);
        }
    }

    /// <summary>
    /// 「設定」Buttonの「Click」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void SettingsButton_Click(
        object sender,
        RoutedEventArgs e
        )
    {
        try
        {
            if ((int)SettingsRowDefinition.Height.Value == Common.SettingsRowDefinitionMinimize)
            {
                ItemsRowDefinition.Height = new(0);
                SettingsRowDefinition.Height = new(1, GridUnitType.Star);
                SettingsImage.Source = new BitmapImage(new(ApplicationData.Settings.DarkMode ? "/Resources/CloseSettingsWhite.png" : "/Resources/CloseSettingsDark.png", UriKind.Relative));
                SettingsScrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            }
            else
            {
                ItemsRowDefinition.Height = new(1, GridUnitType.Star);
                SettingsRowDefinition.Height = new(Common.SettingsRowDefinitionMinimize);
                SettingsImage.Source = new BitmapImage(new(ApplicationData.Settings.DarkMode ? "/Resources/SettingsWhite.png" : "/Resources/SettingsDark.png", UriKind.Relative));
                SettingsScrollViewer.ScrollToVerticalOffset(0);
                SettingsScrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Disabled;
            }
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「ウィンドウ処理を複数登録」ToggleSwitchの「Toggled」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void RegisterMultipleToggleSwitch_Toggled(
        object sender,
        RoutedEventArgs e
        )
    {
        try
        {
            ApplicationData.Settings.SpecifyWindowInformation.MultipleRegistrations = RegisterMultipleToggleSwitch.IsOn;
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「ウィンドウ判定で大文字と小文字を区別する」ToggleSwitchの「Toggled」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void CaseSensitiveWindowQueriesToggleSwitch_Toggled(
        object sender,
        RoutedEventArgs e
        )
    {
        try
        {
            ApplicationData.Settings.SpecifyWindowInformation.CaseSensitive = CaseSensitiveWindowQueriesToggleSwitch.IsOn;
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「画面外に出る場合は位置やサイズを変更しない」ToggleSwitchの「Toggled」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void DoNotChangeOutOfScreenToggleSwitch_Toggled(
        object sender,
        RoutedEventArgs e
        )
    {
        try
        {
            ApplicationData.Settings.SpecifyWindowInformation.DoNotChangeOutOfScreen = DoNotChangeOutOfScreenToggleSwitch.IsOn;
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「追加/修正のウィンドウが表示されている場合は処理停止」ToggleSwitchの「Toggled」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void StopProcessingShowAddModifyToggleSwitch_Toggled(
        object sender,
        RoutedEventArgs e
        )
    {
        try
        {
            ApplicationData.Settings.SpecifyWindowInformation.StopProcessingShowAddModifyWindow = StopProcessingShowAddModifyToggleSwitch.IsOn;
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「ウィンドウが全画面表示の場合は処理停止」ToggleSwitchの「Toggled」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void StopProcessingFullScreenToggleSwitch_Toggled(
        object sender,
        RoutedEventArgs e
        )
    {
        try
        {
            ApplicationData.Settings.SpecifyWindowInformation.StopProcessingFullScreen = StopProcessingFullScreenToggleSwitch.IsOn;
            ApplicationData.EventData.DoProcessingEvent(ProcessingEventType.StopWhenWindowIsFullScreenChanged);
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「ホットキーは停止させない (全画面ウィンドウがある場合)」ToggleSwitchの「Toggled」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void HotkeysDoNotStopFullScreenToggleSwitch_Toggled(
        object sender,
        RoutedEventArgs e
        )
    {
        try
        {
            ApplicationData.Settings.SpecifyWindowInformation.HotkeysDoNotStopFullScreen = HotkeysDoNotStopFullScreenToggleSwitch.IsOn;
            ApplicationData.EventData.DoProcessingEvent(ProcessingEventType.HotkeysDoNotStopFullScreenChanged);
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「処理間隔」NumberBoxの「ValueChanged」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void ProcessingIntervalNumberBox_ValueChanged(
        ModernWpf.Controls.NumberBox sender,
        ModernWpf.Controls.NumberBoxValueChangedEventArgs args
        )
    {
        ApplicationData.Settings.SpecifyWindowInformation.ProcessingInterval = (int)ProcessingIntervalNumberBox.Value;
        ApplicationData.EventData.DoProcessingEvent(ProcessingEventType.SpecifyWindowChangeTimerProcessingInterval);
    }

    /// <summary>
    /// 「処理するウィンドウの範囲」ComboBoxの「SelectionChanged」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ProcessingWindowRangeComboBox_SelectionChanged(
        object sender,
        SelectionChangedEventArgs e
        )
    {
        try
        {
            string stringData = (string)((ComboBoxItem)ProcessingWindowRangeComboBox.SelectedItem).Content;        // 選択項目の文字列

            if (stringData == ApplicationData.Languages.LanguagesWindow?.ActiveWindowOnly)
            {
                ApplicationData.Settings.SpecifyWindowInformation.ProcessingWindowRange = ProcessingWindowRange.ActiveOnly;
                WaitTimeToProcessingNextWindowLabel.IsEnabled = false;
                WaitTimeToProcessingNextWindowNumberBox.IsEnabled = false;
            }
            else if (stringData == ApplicationData.Languages.LanguagesWindow?.AllWindow)
            {
                ApplicationData.Settings.SpecifyWindowInformation.ProcessingWindowRange = ProcessingWindowRange.All;
                WaitTimeToProcessingNextWindowLabel.IsEnabled = true;
                WaitTimeToProcessingNextWindowNumberBox.IsEnabled = true;
            }
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「次のウィンドウを処理する待ち時間 (ミリ秒)」NumberBoxの「ValueChanged」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void WaitTimeToProcessingNextWindowNumberBox_ValueChanged(
        ModernWpf.Controls.NumberBox sender,
        ModernWpf.Controls.NumberBoxValueChangedEventArgs args
        )
    {
        try
        {
            ApplicationData.Settings.SpecifyWindowInformation.WaitTimeToProcessingNextWindow = (int)WaitTimeToProcessingNextWindowNumberBox.Value;
        }
        catch
        {
        }
    }

    /// <summary>
    /// コントロールにテキストを設定
    /// </summary>
    private void SetTextOnControls()
    {
        try
        {
            if (ApplicationData.Languages.LanguagesWindow == null)
            {
                throw new Exception("Languages value is null. - SpecifyWindowPage.SetTextOnControls()");
            }

            AddLabel.Content = ApplicationData.Languages.LanguagesWindow.Add;
            ModifyLabel.Content = ApplicationData.Languages.LanguagesWindow.Modify;
            DeleteLabel.Content = ApplicationData.Languages.LanguagesWindow.Delete;
            CopyLabel.Content = ApplicationData.Languages.LanguagesWindow.Copy;
            MoveUpLabel.Content = ApplicationData.Languages.LanguagesWindow.MoveUp;
            MoveDownLabel.Content = ApplicationData.Languages.LanguagesWindow.MoveDown;
            SettingsLabel.Content = ApplicationData.Languages.LanguagesWindow.Setting;
            ExplanationTextBlock.Text = ApplicationData.Languages.LanguagesWindow.ExplanationOfSpecifyWindow;

            ProcessingStateToggleSwitch.OffContent = ProcessingStateToggleSwitch.OnContent = ApplicationData.Languages.LanguagesWindow.ProcessingState;
            RegisterMultipleToggleSwitch.OffContent = RegisterMultipleToggleSwitch.OnContent = ApplicationData.Languages.LanguagesWindow.RegisterMultipleWindowActions;
            CaseSensitiveWindowQueriesToggleSwitch.OffContent = CaseSensitiveWindowQueriesToggleSwitch.OnContent = ApplicationData.Languages.LanguagesWindow.CaseSensitiveWindowQueries;
            DoNotChangeOutOfScreenToggleSwitch.OffContent = DoNotChangeOutOfScreenToggleSwitch.OnContent = ApplicationData.Languages.LanguagesWindow.DoNotChangePositionSizeOutOfScreen;
            StopProcessingShowAddModifyToggleSwitch.OffContent = StopProcessingShowAddModifyToggleSwitch.OnContent = ApplicationData.Languages.LanguagesWindow.StopProcessingShowAddModify;
            StopProcessingFullScreenToggleSwitch.OffContent = StopProcessingFullScreenToggleSwitch.OnContent = ApplicationData.Languages.LanguagesWindow.StopProcessingWhenWindowIsFullScreen;
            HotkeysDoNotStopFullScreenToggleSwitch.OffContent = HotkeysDoNotStopFullScreenToggleSwitch.OnContent = ApplicationData.Languages.LanguagesWindow.HotkeysDoNotStop;
            TimerGroupBox.Header = ApplicationData.Languages.LanguagesWindow.Timer;
            ProcessingIntervalLabel.Content = ApplicationData.Languages.LanguagesWindow.ProcessingInterval;
            ProcessingWindowRangeLabel.Content = ApplicationData.Languages.LanguagesWindow.ProcessingWindowRange;
            ((ComboBoxItem)ProcessingWindowRangeComboBox.Items[0]).Content = ApplicationData.Languages.LanguagesWindow.ActiveWindowOnly;
            ((ComboBoxItem)ProcessingWindowRangeComboBox.Items[1]).Content = ApplicationData.Languages.LanguagesWindow.AllWindow;
            WaitTimeToProcessingNextWindowLabel.Content = ApplicationData.Languages.LanguagesWindow.WaitTimeToProcessingNextWindow;
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「指定ウィンドウ」ListBoxの項目を更新
    /// </summary>
    private void UpdateSpecifyWindowListBoxItems()
    {
        SpecifyWindowListBox.Items.Clear();

        if (ApplicationData.Settings.SpecifyWindowInformation.Items.Count != 0)
        {
            foreach (SpecifyWindowItemInformation nowEII in ApplicationData.Settings.SpecifyWindowInformation.Items)
            {
                CheckListBoxItem newItem = new()
                {
                    Text = nowEII.RegisteredName,
                    IsChecked = nowEII.Enabled
                };
                newItem.CheckStateChanged += SpecifyWindowCheckListBoxItem_CheckStateChanged;
                SpecifyWindowListBox.Items.Add(newItem);
            }
        }
    }

    /// <summary>
    /// サイドボタンの有効状態を設定
    /// </summary>
    private void SettingsSideButtonEnabled()
    {
        if (SpecifyWindowListBox.SelectedItems.Count == 0)
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
            MoveUpButton.IsEnabled = SpecifyWindowListBox.SelectedIndex != 0;
            MoveDownButton.IsEnabled = SpecifyWindowListBox.Items.Count != (SpecifyWindowListBox.SelectedIndex + 1);
            CopyButton.IsEnabled = true;
        }
    }

    /// <summary>
    /// コントロールの画像を設定
    /// </summary>
    private void SettingsControlsImage()
    {
        if (ApplicationData.Settings.DarkMode)
        {
            AddImage.Source = new BitmapImage(new("/Resources/AdditionWhite.png", UriKind.Relative));
            ModifyImage.Source = new BitmapImage(new("/Resources/ModifyWhite.png", UriKind.Relative));
            DeleteImage.Source = new BitmapImage(new("/Resources/DeleteWhite.png", UriKind.Relative));
            CopyImage.Source = new BitmapImage(new("/Resources/CopyWhite.png", UriKind.Relative));
            MoveUpImage.Source = new BitmapImage(new("/Resources/UpWhite.png", UriKind.Relative));
            MoveDownImage.Source = new BitmapImage(new("/Resources/DownWhite.png", UriKind.Relative));
            SettingsImage.Source = new BitmapImage(new((int)SettingsRowDefinition.Height.Value == Common.SettingsRowDefinitionMinimize ? "/Resources/SettingsWhite.png" : "/Resources/CloseSettingsWhite.png", UriKind.Relative));
        }
        else
        {
            AddImage.Source = new BitmapImage(new("/Resources/AdditionDark.png", UriKind.Relative));
            ModifyImage.Source = new BitmapImage(new("/Resources/ModifyDark.png", UriKind.Relative));
            DeleteImage.Source = new BitmapImage(new("/Resources/DeleteDark.png", UriKind.Relative));
            CopyImage.Source = new BitmapImage(new("/Resources/CopyDark.png", UriKind.Relative));
            MoveUpImage.Source = new BitmapImage(new("/Resources/UpDark.png", UriKind.Relative));
            MoveDownImage.Source = new BitmapImage(new("/Resources/DownDark.png", UriKind.Relative));
            SettingsImage.Source = new BitmapImage(new((int)SettingsRowDefinition.Height.Value == Common.SettingsRowDefinitionMinimize ? "/Resources/SettingsDark.png" : "/Resources/CloseSettingsDark.png", UriKind.Relative));
        }
    }
}
