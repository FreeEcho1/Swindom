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
    /// コンストラクタ
    /// </summary>
    public SpecifyWindowPage()
    {
        InitializeComponent();

        SettingsRowDefinition.Height = new(WindowControlValue.SettingsRowDefinitionMinimize);
        SettingsScrollViewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled;
        SettingsScrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Disabled;
        SettingsControlsImage();
        SetTextOnControls();
        UpdateSpecifyWindowListBoxItems();
        SettingsSideButtonIsEnabled();
        SpecifyWindowListBox.SelectionChanged += SpecifyWindowListBox_SelectionChanged;
        AddButton.Click += AddButton_Click;
        ModifyButton.Click += ModifyButton_Click;
        DeleteButton.Click += DeleteButton_Click;
        CopyButton.Click += CopyButton_Click;
        MoveUpButton.Click += MoveUpButton_Click;
        MoveDownButton.Click += MoveDownButton_Click;
        SettingsButton.Click += SettingsButton_Click;

        ProcessingStateToggleSwitch.IsOn = ApplicationData.Settings.SpecifyWindowInformation.IsEnabled;
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
            ProcessingWindowRange.All => ApplicationData.Languages.AllWindow,
            _ => ApplicationData.Languages.ActiveWindowOnly
        };
        ControlsProcessing.SelectComboBoxItem(ProcessingWindowRangeComboBox, stringData);
        WaitTimeToProcessingNextWindowNumberBox.Minimum = SpecifyWindowInformation.MinimumWaitTimeToProcessingNextWindow;
        WaitTimeToProcessingNextWindowNumberBox.Maximum = SpecifyWindowInformation.MaximumWaitTimeToProcessingNextWindow;
        WaitTimeToProcessingNextWindowNumberBox.Value = ApplicationData.Settings.SpecifyWindowInformation.WaitTimeToProcessingNextWindow;
        SettingsWaitTimeToProcessingNextWindowIsEnabled();
        ProcessingStateToggleSwitch.Toggled += ProcessingStateToggleSwitch_Toggled;
        RegisterMultipleToggleSwitch.Toggled += RegisterMultipleToggleSwitch_Toggled;
        CaseSensitiveWindowQueriesToggleSwitch.Toggled += CaseSensitiveWindowQueriesToggleSwitch_Toggled;
        DoNotChangeOutOfScreenToggleSwitch.Toggled += DoNotChangeOutOfScreenToggleSwitch_Toggled;
        StopProcessingShowAddModifyToggleSwitch.Toggled += StopProcessingShowAddModifyToggleSwitch_Toggled;
        StopProcessingFullScreenToggleSwitch.Toggled += StopProcessingFullScreenToggleSwitch_Toggled;
        HotkeysDoNotStopFullScreenToggleSwitch.Toggled += HotkeysDoNotStopFullScreenToggleSwitch_Toggled;
        ProcessingIntervalNumberBox.ValueChanged += ProcessingIntervalNumberBox_ValueChanged;
        ProcessingWindowRangeComboBox.SelectionChanged += ProcessingWindowRangeComboBox_SelectionChanged;
        ActiveWindowExplanationButton.Click += ActiveWindowExplanationButton_Click;
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
            SettingsSideButtonIsEnabled();
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
        object? sender,
        RoutedEventArgs e
        )
    {
        try
        {
            if (SpecifyWindowItemWindow == null)
            {
                SpecifyWindowItemWindow = new(-1);
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
        object? sender,
        RoutedEventArgs e
        )
    {
        try
        {
            if (SpecifyWindowItemWindow == null)
            {
                SpecifyWindowItemWindow = new(SpecifyWindowListBox.SelectedIndex);
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
                    UpdateSpecifyWindowListBoxItems();
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
        object? sender,
        RoutedEventArgs e
        )
    {
        try
        {
            if (FEMessageBox.Show(ApplicationData.Settings.SpecifyWindowInformation.Items[SpecifyWindowListBox.SelectedIndex].RegisteredName + Environment.NewLine + ApplicationData.Languages.AllowDelete, ApplicationData.Languages.Check, MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                ApplicationData.WindowProcessingManagement.SpecifyWindowProcessing?.UnregisterHotkeys();
                try
                {
                    ApplicationData.Settings.SpecifyWindowInformation.Items.RemoveAt(SpecifyWindowListBox.SelectedIndex);
                    SpecifyWindowListBox.Items.RemoveAt(SpecifyWindowListBox.SelectedIndex);
                    FEMessageBox.Show(ApplicationData.Languages.Deleted, ApplicationData.Languages.Check, MessageBoxButton.OK);
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
        object? sender,
        RoutedEventArgs e
        )
    {
        try
        {
            SpecifyWindowProcessing.AddCopySpecifyWindowItemInformation(ApplicationData.Settings.SpecifyWindowInformation.Items[SpecifyWindowListBox.SelectedIndex], false);
            UpdateSpecifyWindowListBoxItems();
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
        object? sender,
        RoutedEventArgs e
        )
    {
        try
        {
            ApplicationData.Settings.SpecifyWindowInformation.Items.Reverse(SpecifyWindowListBox.SelectedIndex - 1, 2);
            UpdateSpecifyWindowListBoxItems(-1);
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
        object? sender,
        RoutedEventArgs e
        )
    {
        try
        {
            ApplicationData.Settings.SpecifyWindowInformation.Items.Reverse(SpecifyWindowListBox.SelectedIndex, 2);
            UpdateSpecifyWindowListBoxItems(1);
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
                    if (ApplicationData.Settings.SpecifyWindowInformation.IsEnabled != ProcessingStateToggleSwitch.IsOn)
                    {
                        ProcessingStateToggleSwitch.IsOn = ApplicationData.Settings.SpecifyWindowInformation.IsEnabled;
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
                    ApplicationData.Settings.SpecifyWindowInformation.Items[count].IsEnabled = ((CheckListBoxItem)SpecifyWindowListBox.Items[count]).IsChecked;
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
            if (ApplicationData.Settings.SpecifyWindowInformation.IsEnabled != ProcessingStateToggleSwitch.IsOn)
            {
                ApplicationData.Settings.SpecifyWindowInformation.IsEnabled = ProcessingStateToggleSwitch.IsOn;
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
        object? sender,
        RoutedEventArgs e
        )
    {
        try
        {
            if ((int)SettingsRowDefinition.Height.Value == WindowControlValue.SettingsRowDefinitionMinimize)
            {
                ItemsRowDefinition.Height = new(0);
                SettingsRowDefinition.Height = new(1, GridUnitType.Star);
                SettingsButton.ButtonImage = ImageProcessing.GetImageCloseSettings();
                SettingsScrollViewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
                SettingsScrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            }
            else
            {
                ItemsRowDefinition.Height = new(1, GridUnitType.Star);
                SettingsRowDefinition.Height = new(WindowControlValue.SettingsRowDefinitionMinimize);
                SettingsButton.ButtonImage = ImageProcessing.GetImageSettings();
                SettingsScrollViewer.ScrollToVerticalOffset(0);
                SettingsScrollViewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled;
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
    /// 「追加/修正ウィンドウが表示されている場合は処理停止」ToggleSwitchの「Toggled」イベント
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
    /// 「全画面のウィンドウが存在する場合は処理停止」ToggleSwitchの「Toggled」イベント
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
    /// 「ホットキーは除外」ToggleSwitchの「Toggled」イベント
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
        try
        {
            ApplicationData.Settings.SpecifyWindowInformation.ProcessingInterval = (int)ProcessingIntervalNumberBox.Value;
            ApplicationData.EventData.DoProcessingEvent(ProcessingEventType.SpecifyWindowChangeTimerProcessingInterval);
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
        SelectionChangedEventArgs e
        )
    {
        try
        {
            string stringData = (string)((ComboBoxItem)ProcessingWindowRangeComboBox.SelectedItem).Content;        // 選択項目の文字列

            if (stringData == ApplicationData.Languages.ActiveWindowOnly)
            {
                ApplicationData.Settings.SpecifyWindowInformation.ProcessingWindowRange = ProcessingWindowRange.ActiveOnly;
            }
            else if (stringData == ApplicationData.Languages.AllWindow)
            {
                ApplicationData.Settings.SpecifyWindowInformation.ProcessingWindowRange = ProcessingWindowRange.All;
            }
            SettingsWaitTimeToProcessingNextWindowIsEnabled();
        }
        catch
        {
        }
    }


    /// <summary>
    /// 「次のウィンドウを処理する待ち時間」の「IsEnabled」を設定
    /// </summary>
    private void SettingsWaitTimeToProcessingNextWindowIsEnabled()
    {
        if (ApplicationData.Settings.SpecifyWindowInformation.ProcessingWindowRange == ProcessingWindowRange.ActiveOnly)
        {
            WaitTimeToProcessingNextWindowLabel.IsEnabled = false;
            WaitTimeToProcessingNextWindowNumberBox.IsEnabled = false;
        }
        else
        {
            WaitTimeToProcessingNextWindowLabel.IsEnabled = true;
            WaitTimeToProcessingNextWindowNumberBox.IsEnabled = true;
        }
    }

    /// <summary>
    /// 「アクティブウィンドウ」の「?」Buttonの「Click」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ActiveWindowExplanationButton_Click(
        object sender,
        RoutedEventArgs e
        )
    {
        try
        {
            ApplicationData.WindowManagement.ShowExplanationWindow(SelectExplanationType.ActiveWindow);
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
            AddButton.Text = ApplicationData.Languages.Add;
            ModifyButton.Text = ApplicationData.Languages.Modify;
            DeleteButton.Text = ApplicationData.Languages.Delete;
            CopyButton.Text = ApplicationData.Languages.Copy;
            MoveUpButton.Text = ApplicationData.Languages.MoveUp;
            MoveDownButton.Text = ApplicationData.Languages.MoveDown;
            SettingsButton.Text = ApplicationData.Languages.Setting;
            ExplanationTextBlock.Text = ApplicationData.Languages.ExplanationOfSpecifyWindow;

            ProcessingStateToggleSwitch.OffContent = ProcessingStateToggleSwitch.OnContent = ApplicationData.Languages.ProcessingState;
            RegisterMultipleToggleSwitch.OffContent = RegisterMultipleToggleSwitch.OnContent = ApplicationData.Languages.RegisterMultipleWindowActions;
            CaseSensitiveWindowQueriesToggleSwitch.OffContent = CaseSensitiveWindowQueriesToggleSwitch.OnContent = ApplicationData.Languages.CaseSensitiveWindowQueries;
            DoNotChangeOutOfScreenToggleSwitch.OffContent = DoNotChangeOutOfScreenToggleSwitch.OnContent = ApplicationData.Languages.DoNotChangePositionSizeOutOfScreen;
            StopProcessingShowAddModifyToggleSwitch.OffContent = StopProcessingShowAddModifyToggleSwitch.OnContent = ApplicationData.Languages.StopProcessingShowAddModify;
            StopProcessingFullScreenToggleSwitch.OffContent = StopProcessingFullScreenToggleSwitch.OnContent = ApplicationData.Languages.StopProcessingWhenWindowIsFullScreen;
            HotkeysDoNotStopFullScreenToggleSwitch.OffContent = HotkeysDoNotStopFullScreenToggleSwitch.OnContent = ApplicationData.Languages.HotkeysDoNotStop;
            TimerGroupBox.Header = ApplicationData.Languages.Timer;
            ProcessingIntervalLabel.Content = ApplicationData.Languages.ProcessingInterval;
            ProcessingWindowRangeLabel.Content = ApplicationData.Languages.ProcessingWindowRange;
            ProcessingWindowRangeActiveWindowOnlyComboBoxItem.Content = ApplicationData.Languages.ActiveWindowOnly;
            ProcessingWindowRangeAllWindowComboBoxItem.Content = ApplicationData.Languages.AllWindow;
            ActiveWindowExplanationButton.Content = ApplicationData.Languages.Question;
            ActiveWindowExplanationButton.ToolTip = ApplicationData.Languages.Help;
            WaitTimeToProcessingNextWindowLabel.Content = ApplicationData.Languages.WaitTimeToProcessingNextWindow;
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「指定ウィンドウ」ListBoxの項目を更新
    /// </summary>
    /// <param name="addSelectedIndex">選択項目のインデックスに追加する値</param>
    private void UpdateSpecifyWindowListBoxItems(
        int addSelectedIndex = 0
        )
    {
        int selectedIndex = SpecifyWindowListBox.SelectedIndex;       // 選択している項目のインデックス

        SpecifyWindowListBox.Items.Clear();
        foreach (SpecifyWindowItemInformation nowEII in ApplicationData.Settings.SpecifyWindowInformation.Items)
        {
            CheckListBoxItem newItem = new()
            {
                Text = nowEII.RegisteredName,
                IsChecked = nowEII.IsEnabled
            };
            newItem.CheckStateChanged += SpecifyWindowCheckListBoxItem_CheckStateChanged;
            SpecifyWindowListBox.Items.Add(newItem);
        }
        SpecifyWindowListBox.SelectedIndex = selectedIndex + addSelectedIndex;
    }

    /// <summary>
    /// サイドボタンの「IsEnabled」を設定
    /// </summary>
    private void SettingsSideButtonIsEnabled()
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
        AddButton.ButtonImage = ImageProcessing.GetImageAddition();
        ModifyButton.ButtonImage = ImageProcessing.GetImageModify();
        DeleteButton.ButtonImage = ImageProcessing.GetImageDelete();
        CopyButton.ButtonImage = ImageProcessing.GetImageCopy();
        MoveUpButton.ButtonImage = ImageProcessing.GetImageUp();
        MoveDownButton.ButtonImage = ImageProcessing.GetImageDown();
        SettingsButton.ButtonImage = (int)SettingsRowDefinition.Height.Value == WindowControlValue.SettingsRowDefinitionMinimize ? ImageProcessing.GetImageSettings() : ImageProcessing.GetImageCloseSettings();
    }
}
