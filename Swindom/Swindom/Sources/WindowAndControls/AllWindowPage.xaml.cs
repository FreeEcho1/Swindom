namespace Swindom;

/// <summary>
/// 「全てのウィンドウ」ページ
/// </summary>
public partial class AllWindowPage : Page
{
    /// <summary>
    /// 「全てのウィンドウを指定した位置に移動」の「移動しないウィンドウ」の追加/修正ウィンドウ
    /// </summary>
    private CancelAllWindowPositionSizeWindow? CancelAllWindowPositionSizeWindow;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public AllWindowPage()
    {
        InitializeComponent();

        SettingsRowDefinition.Height = new(WindowControlValue.SettingsRowDefinitionMinimize);
        SettingsScrollViewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled;
        SettingsScrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Disabled;
        foreach (MonitorInfoEx nowMonitorInfo in ApplicationData.MonitorInformation.MonitorInfo)
        {
            ComboBoxItem newItem = new()
            {
                Content = nowMonitorInfo.DeviceName
            };
            DisplayComboBox.Items.Add(newItem);
        }
        DisplayComboBox.SelectedIndex = 0;
        SettingsDisplayControlsVisibility();
        ModifyButton.IsEnabled = false;
        DeleteButton.IsEnabled = false;
        CopyButton.IsEnabled = false;
        MoveUpButton.IsEnabled = false;
        MoveDownButton.IsEnabled = false;
        SettingsControlsImage();
        SettingsTextOnControls();

        ProcessingStateToggleSwitch.IsOn = ApplicationData.Settings.AllWindowInformation.IsEnabled;
        CaseSensitiveWindowQueriesToggleSwitch.IsOn = ApplicationData.Settings.AllWindowInformation.CaseSensitive;
        StopProcessingFullScreenToggleSwitch.IsOn = ApplicationData.Settings.AllWindowInformation.StopProcessingFullScreen;
        string stringData;
        stringData = ApplicationData.Settings.AllWindowInformation.StandardDisplay switch
        {
            StandardDisplay.SpecifiedDisplay => ApplicationData.Languages.SpecifiedDisplay,
            StandardDisplay.ExclusiveSpecifiedDisplay => ApplicationData.Languages.LimitedToSpecifiedDisplay,
            _ => ApplicationData.Languages.CurrentDisplay
        };
        ControlsProcessing.SelectComboBoxItem(StandardDisplayComboBox, stringData);
        ControlsProcessing.SelectComboBoxItem(DisplayComboBox, ApplicationData.Settings.AllWindowInformation.PositionSize.Display);
        SettingsIsEnabledDisplayControls();
        stringData = ApplicationData.Settings.AllWindowInformation.PositionSize.XType switch
        {
            WindowXType.DoNotChange => ApplicationData.Languages.DoNotChange,
            WindowXType.Left => ApplicationData.Languages.LeftEdge,
            WindowXType.Middle => ApplicationData.Languages.Middle,
            WindowXType.Right => ApplicationData.Languages.RightEdge,
            WindowXType.Value => ApplicationData.Languages.CoordinateSpecification,
            _ => ApplicationData.Languages.DoNotChange
        };
        ControlsProcessing.SelectComboBoxItem(MoveAllWindowToSpecifiedXComboBox, stringData);
        MoveAllWindowToSpecifiedXNumberBox.Value = ApplicationData.Settings.AllWindowInformation.PositionSize.X;
        stringData = ApplicationData.Settings.AllWindowInformation.PositionSize.XValueType switch
        {
            PositionSizeValueType.Pixel => ApplicationData.Languages.Pixel,
            PositionSizeValueType.Percent => ApplicationData.Languages.Percent,
            _ => ApplicationData.Languages.Pixel
        };
        ControlsProcessing.SelectComboBoxItem(MoveAllWindowToSpecifiedXTypeComboBox, stringData);
        stringData = ApplicationData.Settings.AllWindowInformation.PositionSize.YType switch
        {
            WindowYType.DoNotChange => ApplicationData.Languages.DoNotChange,
            WindowYType.Top => ApplicationData.Languages.TopEdge,
            WindowYType.Middle => ApplicationData.Languages.Middle,
            WindowYType.Bottom => ApplicationData.Languages.BottomEdge,
            WindowYType.Value => ApplicationData.Languages.CoordinateSpecification,
            _ => ApplicationData.Languages.DoNotChange
        };
        ControlsProcessing.SelectComboBoxItem(MoveAllWindowToSpecifiedYComboBox, stringData);
        MoveAllWindowToSpecifiedYNumberBox.Value = ApplicationData.Settings.AllWindowInformation.PositionSize.Y;
        stringData = ApplicationData.Settings.AllWindowInformation.PositionSize.YValueType switch
        {
            PositionSizeValueType.Pixel => ApplicationData.Languages.Pixel,
            PositionSizeValueType.Percent => ApplicationData.Languages.Percent,
            _ => ApplicationData.Languages.Pixel
        };
        ControlsProcessing.SelectComboBoxItem(MoveAllWindowToSpecifiedYTypeComboBox, stringData);
        SettingsXControlsIsEnabled();
        SettingsYControlsIsEnabled();
        EventMoveSizeEndToggleSwitch.IsOn = ApplicationData.Settings.AllWindowInformation.WindowEvent.MoveSizeEnd;
        EventShowToggleSwitch.IsOn = ApplicationData.Settings.AllWindowInformation.WindowEvent.Show;
        EventDelayTimeNumberBox.Value = ApplicationData.Settings.AllWindowInformation.WindowEvent.DelayTime;
        UpdateCancelProcessingListBoxItems();

        CancelProcessingListBox.SelectionChanged += CancelProcessingListBox_SelectionChanged;
        AddButton.Click += AddButton_Click;
        ModifyButton.Click += ModifyButton_Click;
        DeleteButton.Click += DeleteButton_Click;
        CopyButton.Click += CopyButton_Click;
        MoveUpButton.Click += MoveUpButton_Click;
        MoveDownButton.Click += MoveDownButton_Click;
        SettingsButton.Click += SettingsButton_Click;
        ProcessingStateToggleSwitch.Toggled += ProcessingStateToggleSwitch_Toggled;
        CaseSensitiveWindowQueriesToggleSwitch.Toggled += CaseSensitiveWindowQueriesToggleSwitch_Toggled;
        StopProcessingFullScreenToggleSwitch.Toggled += StopProcessingFullScreenToggleSwitch_Toggled;
        StandardDisplayComboBox.SelectionChanged += StandardDisplayComboBox_SelectionChanged;
        DisplayComboBox.SelectionChanged += DisplayComboBox_SelectionChanged;
        MoveAllWindowToSpecifiedXComboBox.SelectionChanged += MoveAllWindowToSpecifiedXComboBox_SelectionChanged;
        MoveAllWindowToSpecifiedXNumberBox.ValueChanged += MoveAllWindowToSpecifiedXNumberBox_ValueChanged;
        MoveAllWindowToSpecifiedXTypeComboBox.SelectionChanged += MoveAllWindowToSpecifiedXTypeComboBox_SelectionChanged;
        MoveAllWindowToSpecifiedYComboBox.SelectionChanged += MoveAllWindowToSpecifiedYComboBox_SelectionChanged;
        MoveAllWindowToSpecifiedYNumberBox.ValueChanged += MoveAllWindowToSpecifiedYNumberBox_ValueChanged;
        MoveAllWindowToSpecifiedYTypeComboBox.SelectionChanged += MoveAllWindowToSpecifiedYTypeComboBox_SelectionChanged;
        EventMoveSizeEndToggleSwitch.Toggled += EventMoveSizeEndToggleSwitch_Toggled;
        EventShowToggleSwitch.Toggled += EventShowToggleSwitch_Toggled;
        EventDelayTimeNumberBox.ValueChanged += EventDelayTimeNumberBox_ValueChanged;
        ApplicationData.EventData.ProcessingEvent += ApplicationData_ProcessingEvent;
    }

    /// <summary>
    /// ディスプレイのコントロールの「Visibility」を設定
    /// </summary>
    private void SettingsDisplayControlsVisibility()
    {
        if ((ApplicationData.Settings.CoordinateType == CoordinateType.PrimaryDisplay)
            || (ApplicationData.MonitorInformation.MonitorInfo.Count == 1))
        {
            StandardDisplayLabel.Visibility = Visibility.Collapsed;
            StandardDisplayComboBox.Visibility = Visibility.Collapsed;
            DisplayLabel.Visibility = Visibility.Collapsed;
            DisplayComboBox.Visibility = Visibility.Collapsed;
        }
        else
        {
            StandardDisplayLabel.Visibility = Visibility.Visible;
            StandardDisplayComboBox.Visibility = Visibility.Visible;
            DisplayLabel.Visibility = Visibility.Visible;
            DisplayComboBox.Visibility = Visibility.Visible;
        }
    }

    /// <summary>
    /// コントロールにテキストを設定
    /// </summary>
    private void SettingsTextOnControls()
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
            ExplanationTextBlock.Text = ApplicationData.Languages.ExplanationOfAllWindow;

            ProcessingStateToggleSwitch.OffContent = ProcessingStateToggleSwitch.OnContent = ApplicationData.Languages.ProcessingState;
            CaseSensitiveWindowQueriesToggleSwitch.OffContent = CaseSensitiveWindowQueriesToggleSwitch.OnContent = ApplicationData.Languages.CaseSensitiveWindowQueries;
            StopProcessingFullScreenToggleSwitch.OffContent = StopProcessingFullScreenToggleSwitch.OnContent = ApplicationData.Languages.StopProcessingWhenWindowIsFullScreen;
            PositionGroupBox.Header = ApplicationData.Languages.Position;
            StandardDisplayLabel.Content = ApplicationData.Languages.DisplayToUseAsStandard;
            StandardDisplayCurrentDisplayComboBoxItem.Content = ApplicationData.Languages.CurrentDisplay;
            StandardDisplaySpecifiedDisplayComboBoxItem.Content = ApplicationData.Languages.SpecifiedDisplay;
            DisplayLabel.Content = ApplicationData.Languages.Display;
            MoveAllWindowToSpecifiedXLabel.Content = ApplicationData.Languages.X;
            MoveAllWindowToSpecifiedXDoNotChangeComboBoxItem.Content = ApplicationData.Languages.DoNotChange;
            MoveAllWindowToSpecifiedXLeftEdgeComboBoxItem.Content = ApplicationData.Languages.LeftEdge;
            MoveAllWindowToSpecifiedXMiddleComboBoxItem.Content = ApplicationData.Languages.Middle;
            MoveAllWindowToSpecifiedXRightEdgeComboBoxItem.Content = ApplicationData.Languages.RightEdge;
            MoveAllWindowToSpecifiedXCoordinateSpecificationComboBoxItem.Content = ApplicationData.Languages.CoordinateSpecification;
            MoveAllWindowToSpecifiedXTypePixelComboBoxItem.Content = ApplicationData.Languages.Pixel;
            MoveAllWindowToSpecifiedXTypePercentComboBoxItem.Content = ApplicationData.Languages.Percent;
            MoveAllWindowToSpecifiedYLabel.Content = ApplicationData.Languages.Y;
            MoveAllWindowToSpecifiedYDoNotChangeComboBoxItem.Content = ApplicationData.Languages.DoNotChange;
            MoveAllWindowToSpecifiedYTopEdgeComboBoxItem.Content = ApplicationData.Languages.TopEdge;
            MoveAllWindowToSpecifiedYMiddleComboBoxItem.Content = ApplicationData.Languages.Middle;
            MoveAllWindowToSpecifiedYBottomEdgeComboBoxItem.Content = ApplicationData.Languages.BottomEdge;
            MoveAllWindowToSpecifiedYCoordinateSpecificationComboBoxItem.Content = ApplicationData.Languages.CoordinateSpecification;
            MoveAllWindowToSpecifiedYTypePixelComboBoxItem.Content = ApplicationData.Languages.Pixel;
            MoveAllWindowToSpecifiedYTypePercentComboBoxItem.Content = ApplicationData.Languages.Percent;
            MoveAllWindowToSpecifiedPositionEventGroupBox.Header = ApplicationData.Languages.Event;
            EventMoveSizeEndToggleSwitch.OffContent = EventMoveSizeEndToggleSwitch.OnContent = ApplicationData.Languages.MoveSizeChangeEnd;
            EventShowToggleSwitch.OffContent = EventShowToggleSwitch.OnContent = ApplicationData.Languages.Show;
            EventDelayTimeLabel.Content = ApplicationData.Languages.EventDelayTime;
        }
        catch
        {
        }
    }

    /// <summary>
    /// サイドボタンの「IsEnabled」を設定
    /// </summary>
    private void SettingsSideButtonIsEnabled()
    {
        if (CancelProcessingListBox.SelectedItems.Count == 0)
        {
            ModifyButton.IsEnabled = false;
            DeleteButton.IsEnabled = false;
            CopyButton.IsEnabled = false;
            MoveUpButton.IsEnabled = false;
            MoveDownButton.IsEnabled = false;
        }
        else
        {
            ModifyButton.IsEnabled = true;
            DeleteButton.IsEnabled = true;
            CopyButton.IsEnabled = true;
            MoveUpButton.IsEnabled = CancelProcessingListBox.SelectedIndex != 0;
            MoveDownButton.IsEnabled = CancelProcessingListBox.Items.Count != (CancelProcessingListBox.SelectedIndex + 1);
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

    /// <summary>
    /// 「移動しないウィンドウ」ListBoxの「SelectionChanged」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void CancelProcessingListBox_SelectionChanged(
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
            if (CancelAllWindowPositionSizeWindow == null)
            {
                CancelAllWindowPositionSizeWindow = new(-1);
                CancelAllWindowPositionSizeWindow.Closed += CancelAllWindowPositionSizeWindow_Closed;
                CancelAllWindowPositionSizeWindow.Show();
            }
            else
            {
                CancelAllWindowPositionSizeWindow.Activate();
            }
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
                case ProcessingEventType.AllWindowProcessingStateChanged:
                    if (ApplicationData.Settings.AllWindowInformation.IsEnabled != ProcessingStateToggleSwitch.IsOn)
                    {
                        ProcessingStateToggleSwitch.IsOn = ApplicationData.Settings.AllWindowInformation.IsEnabled;
                    }
                    break;
                case ProcessingEventType.LanguageChanged:
                    SettingsTextOnControls();
                    break;
                case ProcessingEventType.ThemeChanged:
                    SettingsControlsImage();
                    break;
                case ProcessingEventType.CoordinateChanged:
                    SettingsDisplayControlsVisibility();
                    break;
            }
        }
        catch
        {
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
            if (CancelAllWindowPositionSizeWindow == null)
            {
                CancelAllWindowPositionSizeWindow = new(CancelProcessingListBox.SelectedIndex);
                CancelAllWindowPositionSizeWindow.Closed += CancelAllWindowPositionSizeWindow_Closed;
                CancelAllWindowPositionSizeWindow.Show();
            }
            else
            {
                CancelAllWindowPositionSizeWindow.Activate();
            }
        }
        catch
        {
            FEMessageBox.Show(ApplicationData.Languages.ErrorOccurred, ApplicationData.Languages.Check, MessageBoxButton.OK);
        }
    }

    /// <summary>
    /// 「全てのウィンドウを指定した位置に移動」の「移動しないウィンドウ」の追加/修正ウィンドウ」の「Closed」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void CancelAllWindowPositionSizeWindow_Closed(
        object? sender,
        EventArgs e
        )
    {
        try
        {
            if (CancelAllWindowPositionSizeWindow != null)
            {
                if (CancelAllWindowPositionSizeWindow.AddedOrModified)
                {
                    UpdateCancelProcessingListBoxItems();
                }
                CancelAllWindowPositionSizeWindow.Owner.Activate();
                CancelAllWindowPositionSizeWindow = null;
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
            if (FEMessageBox.Show(ApplicationData.Settings.AllWindowInformation.Items[CancelProcessingListBox.SelectedIndex].RegisteredName + Environment.NewLine + ApplicationData.Languages.AllowDelete, ApplicationData.Languages.Check, MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                ApplicationData.Settings.AllWindowInformation.Items.RemoveAt(CancelProcessingListBox.SelectedIndex);
                CancelProcessingListBox.Items.RemoveAt(CancelProcessingListBox.SelectedIndex);
                FEMessageBox.Show(ApplicationData.Languages.Deleted, ApplicationData.Languages.Check, MessageBoxButton.OK);
                CancelProcessingListBox.Focus();
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
            AllWindowProcessing.AddCopyWindowJudgementInformation(ApplicationData.Settings.AllWindowInformation.Items[CancelProcessingListBox.SelectedIndex]);
            UpdateCancelProcessingListBoxItems();
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
    /// <exception cref="NotImplementedException"></exception>
    private void MoveUpButton_Click(
        object? sender,
        RoutedEventArgs e
        )
    {
        try
        {
            ApplicationData.Settings.AllWindowInformation.Items.Reverse(CancelProcessingListBox.SelectedIndex - 1, 2);
            UpdateCancelProcessingListBoxItems(-1);
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
    /// <exception cref="NotImplementedException"></exception>
    private void MoveDownButton_Click(
        object? sender,
        RoutedEventArgs e
        )
    {
        try
        {
            ApplicationData.Settings.AllWindowInformation.Items.Reverse(CancelProcessingListBox.SelectedIndex, 2);
            UpdateCancelProcessingListBoxItems(1);
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
            if (ApplicationData.Settings.AllWindowInformation.IsEnabled !=  ProcessingStateToggleSwitch.IsOn)
            {
                ApplicationData.Settings.AllWindowInformation.IsEnabled = ProcessingStateToggleSwitch.IsOn;
                ApplicationData.EventData.DoProcessingEvent(ProcessingEventType.AllWindowProcessingStateChanged);
            }
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
    /// <exception cref="NotImplementedException"></exception>
    private void CaseSensitiveWindowQueriesToggleSwitch_Toggled(
        object sender,
        RoutedEventArgs e
        )
    {
        try
        {
            ApplicationData.Settings.AllWindowInformation.CaseSensitive = CaseSensitiveWindowQueriesToggleSwitch.IsOn;
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
            ApplicationData.Settings.AllWindowInformation.StopProcessingFullScreen = StopProcessingFullScreenToggleSwitch.IsOn;
            ApplicationData.EventData.DoProcessingEvent(ProcessingEventType.StopWhenWindowIsFullScreenChanged);
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「基準にするディスプレイ」ComboBoxの「SelectionChanged」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void StandardDisplayComboBox_SelectionChanged(
        object sender,
        SelectionChangedEventArgs e
        )
    {
        try
        {
            string stringData = (string)((ComboBoxItem)StandardDisplayComboBox.SelectedItem).Content;
            if (stringData == ApplicationData.Languages.CurrentDisplay)
            {
                ApplicationData.Settings.AllWindowInformation.StandardDisplay = StandardDisplay.CurrentDisplay;
            }
            else if (stringData == ApplicationData.Languages.SpecifiedDisplay)
            {
                ApplicationData.Settings.AllWindowInformation.StandardDisplay = StandardDisplay.SpecifiedDisplay;
            }
            SettingsIsEnabledDisplayControls();
        }
        catch
        {
        }
    }

    /// <summary>
    /// ディスプレイのコントロールの「IsEnabled」を設定
    /// </summary>
    private void SettingsIsEnabledDisplayControls()
    {
        switch (ApplicationData.Settings.AllWindowInformation.StandardDisplay)
        {
            case StandardDisplay.SpecifiedDisplay:
                DisplayLabel.IsEnabled = true;
                DisplayComboBox.IsEnabled = true;
                break;
            default:
                DisplayLabel.IsEnabled = false;
                DisplayComboBox.IsEnabled = false;
                break;
        }
    }

    /// <summary>
    /// 「ディスプレイ」ComboBoxの「SelectionChanged」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void DisplayComboBox_SelectionChanged(
        object sender,
        SelectionChangedEventArgs e
        )
    {
        try
        {
            ApplicationData.Settings.AllWindowInformation.PositionSize.Display = (string)((ComboBoxItem)DisplayComboBox.SelectedItem).Content;
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「全てのウィンドウを指定した位置に移動」の「X」ComboBoxの「SelectionChanged」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void MoveAllWindowToSpecifiedXComboBox_SelectionChanged(
        object sender,
        SelectionChangedEventArgs e
        )
    {
        try
        {
            SettingsXControlsIsEnabled();
            string stringData = (string)((ComboBoxItem)MoveAllWindowToSpecifiedXComboBox.SelectedItem).Content;
            if (stringData == ApplicationData.Languages.DoNotChange)
            {
                ApplicationData.Settings.AllWindowInformation.PositionSize.XType = WindowXType.DoNotChange;
            }
            else if (stringData == ApplicationData.Languages.LeftEdge)
            {
                ApplicationData.Settings.AllWindowInformation.PositionSize.XType = WindowXType.Left;
            }
            else if (stringData == ApplicationData.Languages.Middle)
            {
                ApplicationData.Settings.AllWindowInformation.PositionSize.XType = WindowXType.Middle;
            }
            else if (stringData == ApplicationData.Languages.RightEdge)
            {
                ApplicationData.Settings.AllWindowInformation.PositionSize.XType = WindowXType.Right;
            }
            else if (stringData == ApplicationData.Languages.CoordinateSpecification)
            {
                ApplicationData.Settings.AllWindowInformation.PositionSize.XType = WindowXType.Value;
            }
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「全てのウィンドウを指定した位置に移動」の「X」NumberBoxの「ValueChanged」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void MoveAllWindowToSpecifiedXNumberBox_ValueChanged(
        ModernWpf.Controls.NumberBox sender,
        ModernWpf.Controls.NumberBoxValueChangedEventArgs args
        )
    {
        try
        {
            ApplicationData.Settings.AllWindowInformation.PositionSize.X = MoveAllWindowToSpecifiedXNumberBox.Value;
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「全てのウィンドウを指定した位置に移動」の「X」の「値の種類」ComboBoxの「SelectionChanged」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void MoveAllWindowToSpecifiedXTypeComboBox_SelectionChanged(
        object sender,
        SelectionChangedEventArgs e
        )
    {
        try
        {
            if ((string)((ComboBoxItem)MoveAllWindowToSpecifiedXTypeComboBox.SelectedItem).Content == ApplicationData.Languages.Percent)
            {
                MoveAllWindowToSpecifiedXNumberBox.Minimum = PositionSize.PositionSizePercentMinimum;
                MoveAllWindowToSpecifiedXNumberBox.Maximum = PositionSize.PositionSizePercentMaximum;
                ApplicationData.Settings.AllWindowInformation.PositionSize.XValueType = PositionSizeValueType.Percent;
            }
            else
            {
                MoveAllWindowToSpecifiedXNumberBox.Minimum = PositionSize.PositionPixelMinimum;
                MoveAllWindowToSpecifiedXNumberBox.Maximum = PositionSize.PositionSizePixelMaximum;
                ApplicationData.Settings.AllWindowInformation.PositionSize.XValueType = PositionSizeValueType.Pixel;
            }
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「全てのウィンドウを指定した位置に移動」の「Y」ComboBoxの「SelectionChange」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void MoveAllWindowToSpecifiedYComboBox_SelectionChanged(
        object sender,
        SelectionChangedEventArgs e
        )
    {
        try
        {
            SettingsYControlsIsEnabled();
            string stringData = (string)((ComboBoxItem)MoveAllWindowToSpecifiedYComboBox.SelectedItem).Content;
            if (stringData == ApplicationData.Languages.DoNotChange)
            {
                ApplicationData.Settings.AllWindowInformation.PositionSize.YType = WindowYType.DoNotChange;
            }
            else if (stringData == ApplicationData.Languages.TopEdge)
            {
                ApplicationData.Settings.AllWindowInformation.PositionSize.YType = WindowYType.Top;
            }
            else if (stringData == ApplicationData.Languages.Middle)
            {
                ApplicationData.Settings.AllWindowInformation.PositionSize.YType = WindowYType.Middle;
            }
            else if (stringData == ApplicationData.Languages.BottomEdge)
            {
                ApplicationData.Settings.AllWindowInformation.PositionSize.YType = WindowYType.Bottom;
            }
            else if (stringData == ApplicationData.Languages.CoordinateSpecification)
            {
                ApplicationData.Settings.AllWindowInformation.PositionSize.YType = WindowYType.Value;
            }
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「全てのウィンドウを指定した位置に移動」の「Y」NumberBoxの「ValueChanged」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void MoveAllWindowToSpecifiedYNumberBox_ValueChanged(
        ModernWpf.Controls.NumberBox sender,
        ModernWpf.Controls.NumberBoxValueChangedEventArgs args
        )
    {
        try
        {
            ApplicationData.Settings.AllWindowInformation.PositionSize.Y = MoveAllWindowToSpecifiedYNumberBox.Value;
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「全てのウィンドウを指定した位置に移動」の「Y」の「値の種類」ComboBoxの「SelectionChanged」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void MoveAllWindowToSpecifiedYTypeComboBox_SelectionChanged(
        object sender,
        SelectionChangedEventArgs e
        )
    {
        try
        {
            if ((string)((ComboBoxItem)MoveAllWindowToSpecifiedYTypeComboBox.SelectedItem).Content == ApplicationData.Languages.Percent)
            {
                MoveAllWindowToSpecifiedYNumberBox.Minimum = PositionSize.PositionSizePercentMinimum;
                MoveAllWindowToSpecifiedYNumberBox.Maximum = PositionSize.PositionSizePercentMaximum;
                ApplicationData.Settings.AllWindowInformation.PositionSize.YValueType = PositionSizeValueType.Percent;
            }
            else
            {
                MoveAllWindowToSpecifiedYNumberBox.Minimum = PositionSize.PositionPixelMinimum;
                MoveAllWindowToSpecifiedYNumberBox.Maximum = PositionSize.PositionSizePixelMaximum;
                ApplicationData.Settings.AllWindowInformation.PositionSize.YValueType = PositionSizeValueType.Pixel;
            }
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「全てのウィンドウを指定した位置に移動」の「イベント」の「移動及びサイズの変更が終了した」の「Toggled」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void EventMoveSizeEndToggleSwitch_Toggled(
        object sender,
        RoutedEventArgs e
        )
    {
        try
        {
            ApplicationData.Settings.AllWindowInformation.WindowEvent.MoveSizeEnd = EventMoveSizeEndToggleSwitch.IsOn;
            ApplicationData.EventData.DoProcessingEvent(ProcessingEventType.AllWindowProcessingStateChanged);
        }
        catch
        {
            FEMessageBox.Show(ApplicationData.Languages.ErrorOccurred, ApplicationData.Languages.Check, MessageBoxButton.OK);
        }
    }

    /// <summary>
    /// 「全てのウィンドウを指定した位置に移動」の「イベント」の「表示された」の「Toggled」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void EventShowToggleSwitch_Toggled(
        object sender,
        RoutedEventArgs e
        )
    {
        try
        {
            ApplicationData.Settings.AllWindowInformation.WindowEvent.Show = EventShowToggleSwitch.IsOn;
            ApplicationData.EventData.DoProcessingEvent(ProcessingEventType.AllWindowProcessingStateChanged);
        }
        catch
        {
            FEMessageBox.Show(ApplicationData.Languages.ErrorOccurred, ApplicationData.Languages.Check, MessageBoxButton.OK);
        }
    }

    /// <summary>
    /// 「処理の待ち時間 (ミリ秒) (「表示された」のみ)」の「ValueChanged」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void EventDelayTimeNumberBox_ValueChanged(
        ModernWpf.Controls.NumberBox sender,
        ModernWpf.Controls.NumberBoxValueChangedEventArgs args
        )
    {
        try
        {
            ApplicationData.Settings.AllWindowInformation.WindowEvent.DelayTime = (int)EventDelayTimeNumberBox.Value;
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「X」関係のコントロールの「IsEnabled」を設定
    /// </summary>
    private void SettingsXControlsIsEnabled()
    {
        if ((string)((ComboBoxItem)MoveAllWindowToSpecifiedXComboBox.SelectedItem).Content == ApplicationData.Languages.CoordinateSpecification)
        {
            MoveAllWindowToSpecifiedXNumberBox.IsEnabled = true;
            MoveAllWindowToSpecifiedXTypeComboBox.IsEnabled = true;
        }
        else
        {
            MoveAllWindowToSpecifiedXNumberBox.IsEnabled = false;
            MoveAllWindowToSpecifiedXTypeComboBox.IsEnabled = false;
        }
    }

    /// <summary>
    /// 「Y」関係のコントロールの「IsEnabled」を設定
    /// </summary>
    private void SettingsYControlsIsEnabled()
    {
        if ((string)((ComboBoxItem)MoveAllWindowToSpecifiedYComboBox.SelectedItem).Content == ApplicationData.Languages.CoordinateSpecification)
        {
            MoveAllWindowToSpecifiedYNumberBox.IsEnabled = true;
            MoveAllWindowToSpecifiedYTypeComboBox.IsEnabled = true;
        }
        else
        {
            MoveAllWindowToSpecifiedYNumberBox.IsEnabled = false;
            MoveAllWindowToSpecifiedYTypeComboBox.IsEnabled = false;
        }
    }

    /// <summary>
    /// 「移動しないウィンドウ」ListBoxの項目を更新
    /// </summary>
    /// <param name="addSelectedIndex">選択項目のインデックスに追加する値</param>
    private void UpdateCancelProcessingListBoxItems(
        int addSelectedIndex = 0
        )
    {
        int selectedIndex = CancelProcessingListBox.SelectedIndex;       // 選択している項目のインデックス

        CancelProcessingListBox.Items.Clear();
        foreach (WindowJudgementInformation nowItem in ApplicationData.Settings.AllWindowInformation.Items)
        {
            ListBoxItem newItem = new()
            {
                Content = nowItem.RegisteredName
            };
            CancelProcessingListBox.Items.Add(newItem);
        }
        CancelProcessingListBox.SelectedIndex = selectedIndex + addSelectedIndex;
    }
}
