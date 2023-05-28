namespace Swindom;

/// <summary>
/// 「全てのウィンドウ」ページ
/// </summary>
public partial class AllWindowPage : Page
{
    /// <summary>
    /// 次のイベントをキャンセル
    /// </summary>
    private bool CancelNextEvent;
    /// <summary>
    /// 「全てのウィンドウを指定した位置に移動」の「移動しないウィンドウ」の追加/修正ウィンドウ
    /// </summary>
    private CancelAllWindowPositionSizeWindow? CancelAllWindowPositionSizeWindow;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public AllWindowPage()
    {
        if (ApplicationData.Languages.LanguagesWindow == null)
        {
            throw new Exception("Languages value is null. - AllWindowPage()");
        }

        InitializeComponent();

        SettingsRowDefinition.Height = new(Common.SettingsRowDefinitionMinimize);
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
        SettingsDisplayControlsEnabled();
        ModifyButton.IsEnabled = false;
        DeleteButton.IsEnabled = false;
        CopyButton.IsEnabled = false;
        SettingsControlsImage();
        SettingsTextOnControls();

        ProcessingStateToggleSwitch.IsOn = ApplicationData.Settings.AllWindowInformation.Enabled;
        CaseSensitiveWindowQueriesToggleSwitch.IsOn = ApplicationData.Settings.AllWindowInformation.CaseSensitive;
        StopProcessingFullScreenToggleSwitch.IsOn = ApplicationData.Settings.AllWindowInformation.StopProcessingFullScreen;
        string stringData;
        stringData = ApplicationData.Settings.AllWindowInformation.StandardDisplay switch
        {
            StandardDisplay.SpecifiedDisplay => ApplicationData.Languages.LanguagesWindow.SpecifiedDisplay,
            StandardDisplay.ExclusiveSpecifiedDisplay => ApplicationData.Languages.LanguagesWindow.LimitedToSpecifiedDisplay,
            _ => ApplicationData.Languages.LanguagesWindow.CurrentDisplay
        };
        VariousProcessing.SelectComboBoxItem(StandardDisplayComboBox, stringData);
        VariousProcessing.SelectComboBoxItem(DisplayComboBox, ApplicationData.Settings.AllWindowInformation.PositionSize.Display);
        SettingsEnabledDisplayControls();
        stringData = ApplicationData.Settings.AllWindowInformation.PositionSize.XType switch
        {
            WindowXType.DoNotChange => ApplicationData.Languages.LanguagesWindow.DoNotChange,
            WindowXType.Left => ApplicationData.Languages.LanguagesWindow.LeftEdge,
            WindowXType.Middle => ApplicationData.Languages.LanguagesWindow.Middle,
            WindowXType.Right => ApplicationData.Languages.LanguagesWindow.RightEdge,
            WindowXType.Value => ApplicationData.Languages.LanguagesWindow.CoordinateSpecification,
            _ => ApplicationData.Languages.LanguagesWindow.DoNotChange
        };
        VariousProcessing.SelectComboBoxItem(MoveAllWindowToSpecifiedXComboBox, stringData);
        MoveAllWindowToSpecifiedXNumberBox.Value = ApplicationData.Settings.AllWindowInformation.PositionSize.X;
        stringData = ApplicationData.Settings.AllWindowInformation.PositionSize.XValueType switch
        {
            PositionSizeValueType.Pixel => ApplicationData.Languages.LanguagesWindow.Pixel,
            PositionSizeValueType.Percent => ApplicationData.Languages.LanguagesWindow.Percent,
            _ => ApplicationData.Languages.LanguagesWindow.Pixel
        };
        VariousProcessing.SelectComboBoxItem(MoveAllWindowToSpecifiedXTypeComboBox, stringData);
        stringData = ApplicationData.Settings.AllWindowInformation.PositionSize.YType switch
        {
            WindowYType.DoNotChange => ApplicationData.Languages.LanguagesWindow.DoNotChange,
            WindowYType.Top => ApplicationData.Languages.LanguagesWindow.TopEdge,
            WindowYType.Middle => ApplicationData.Languages.LanguagesWindow.Middle,
            WindowYType.Bottom => ApplicationData.Languages.LanguagesWindow.BottomEdge,
            WindowYType.Value => ApplicationData.Languages.LanguagesWindow.CoordinateSpecification,
            _ => ApplicationData.Languages.LanguagesWindow.DoNotChange
        };
        VariousProcessing.SelectComboBoxItem(MoveAllWindowToSpecifiedYComboBox, stringData);
        MoveAllWindowToSpecifiedYNumberBox.Value = ApplicationData.Settings.AllWindowInformation.PositionSize.Y;
        stringData = ApplicationData.Settings.AllWindowInformation.PositionSize.YValueType switch
        {
            PositionSizeValueType.Pixel => ApplicationData.Languages.LanguagesWindow.Pixel,
            PositionSizeValueType.Percent => ApplicationData.Languages.LanguagesWindow.Percent,
            _ => ApplicationData.Languages.LanguagesWindow.Pixel
        };
        VariousProcessing.SelectComboBoxItem(MoveAllWindowToSpecifiedYTypeComboBox, stringData);
        SettingsXControlsEnabled();
        SettingsYControlsEnabled();
        EventMoveSizeEndToggleSwitch.IsOn = ApplicationData.Settings.AllWindowInformation.AllWindowPositionSizeWindowEventData.MoveSizeEnd;
        EventShowToggleSwitch.IsOn = ApplicationData.Settings.AllWindowInformation.AllWindowPositionSizeWindowEventData.Show;
        UpdateCancelProcessingListBoxItems();

        CancelProcessingListBox.SelectionChanged += CancelProcessingListBox_SelectionChanged;
        AddButton.Click += AddButton_Click;
        ModifyButton.Click += ModifyButton_Click;
        DeleteButton.Click += DeleteButton_Click;
        CopyButton.Click += CopyButton_Click;
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
        ApplicationData.EventData.ProcessingEvent += ApplicationData_ProcessingEvent;
    }

    /// <summary>
    /// ディスプレイのコントロールの「Enabled」を設定
    /// </summary>
    private void SettingsDisplayControlsEnabled()
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
            if (ApplicationData.Languages.LanguagesWindow == null)
            {
                throw new Exception("Languages value is null. - AllWindowPage.SettingsTextOnControls()");
            }

            AddLabel.Content = ApplicationData.Languages.LanguagesWindow.Add;
            ModifyLabel.Content = ApplicationData.Languages.LanguagesWindow.Modify;
            DeleteLabel.Content = ApplicationData.Languages.LanguagesWindow.Delete;
            CopyLabel.Content = ApplicationData.Languages.LanguagesWindow.Copy;
            SettingsLabel.Content = ApplicationData.Languages.LanguagesWindow.Setting;
            ExplanationTextBlock.Text = ApplicationData.Languages.LanguagesWindow.ExplanationOfAllWindow;

            ProcessingStateToggleSwitch.OffContent = ProcessingStateToggleSwitch.OnContent = ApplicationData.Languages.LanguagesWindow.ProcessingState;
            CaseSensitiveWindowQueriesToggleSwitch.OffContent = CaseSensitiveWindowQueriesToggleSwitch.OnContent = ApplicationData.Languages.LanguagesWindow.CaseSensitiveWindowQueries;
            StopProcessingFullScreenToggleSwitch.OffContent = StopProcessingFullScreenToggleSwitch.OnContent = ApplicationData.Languages.LanguagesWindow.StopProcessingWhenWindowIsFullScreen;
            PositionGroupBox.Header = ApplicationData.Languages.LanguagesWindow.Position;
            StandardDisplayLabel.Content = ApplicationData.Languages.LanguagesWindow.DisplayToUseAsStandard;
            ((ComboBoxItem)StandardDisplayComboBox.Items[0]).Content = ApplicationData.Languages.LanguagesWindow.CurrentDisplay;
            ((ComboBoxItem)StandardDisplayComboBox.Items[1]).Content = ApplicationData.Languages.LanguagesWindow.SpecifiedDisplay;
            DisplayLabel.Content = ApplicationData.Languages.LanguagesWindow.Display;
            MoveAllWindowToSpecifiedXLabel.Content = ApplicationData.Languages.LanguagesWindow.X;
            ((ComboBoxItem)MoveAllWindowToSpecifiedXComboBox.Items[0]).Content = ApplicationData.Languages.LanguagesWindow.DoNotChange;
            ((ComboBoxItem)MoveAllWindowToSpecifiedXComboBox.Items[1]).Content = ApplicationData.Languages.LanguagesWindow.LeftEdge;
            ((ComboBoxItem)MoveAllWindowToSpecifiedXComboBox.Items[2]).Content = ApplicationData.Languages.LanguagesWindow.Middle;
            ((ComboBoxItem)MoveAllWindowToSpecifiedXComboBox.Items[3]).Content = ApplicationData.Languages.LanguagesWindow.RightEdge;
            ((ComboBoxItem)MoveAllWindowToSpecifiedXComboBox.Items[4]).Content = ApplicationData.Languages.LanguagesWindow.CoordinateSpecification;
            ((ComboBoxItem)MoveAllWindowToSpecifiedXTypeComboBox.Items[0]).Content = ApplicationData.Languages.LanguagesWindow.Pixel;
            ((ComboBoxItem)MoveAllWindowToSpecifiedXTypeComboBox.Items[1]).Content = ApplicationData.Languages.LanguagesWindow.Percent;
            MoveAllWindowToSpecifiedYLabel.Content = ApplicationData.Languages.LanguagesWindow.Y;
            ((ComboBoxItem)MoveAllWindowToSpecifiedYComboBox.Items[0]).Content = ApplicationData.Languages.LanguagesWindow.DoNotChange;
            ((ComboBoxItem)MoveAllWindowToSpecifiedYComboBox.Items[1]).Content = ApplicationData.Languages.LanguagesWindow.TopEdge;
            ((ComboBoxItem)MoveAllWindowToSpecifiedYComboBox.Items[2]).Content = ApplicationData.Languages.LanguagesWindow.Middle;
            ((ComboBoxItem)MoveAllWindowToSpecifiedYComboBox.Items[3]).Content = ApplicationData.Languages.LanguagesWindow.BottomEdge;
            ((ComboBoxItem)MoveAllWindowToSpecifiedYComboBox.Items[4]).Content = ApplicationData.Languages.LanguagesWindow.CoordinateSpecification;
            ((ComboBoxItem)MoveAllWindowToSpecifiedYTypeComboBox.Items[0]).Content = ApplicationData.Languages.LanguagesWindow.Pixel;
            ((ComboBoxItem)MoveAllWindowToSpecifiedYTypeComboBox.Items[1]).Content = ApplicationData.Languages.LanguagesWindow.Percent;
            MoveAllWindowToSpecifiedPositionEventGroupBox.Header = ApplicationData.Languages.LanguagesWindow.Event;
            EventMoveSizeEndToggleSwitch.OffContent = EventMoveSizeEndToggleSwitch.OnContent = ApplicationData.Languages.LanguagesWindow.MoveSizeChangeEnd;
            EventShowToggleSwitch.OffContent = EventShowToggleSwitch.OnContent = ApplicationData.Languages.LanguagesWindow.Show;
        }
        catch
        {
        }
    }

    /// <summary>
    /// サイドボタンの有効状態を設定
    /// </summary>
    private void SettingsSideButtonEnabled()
    {
        if (CancelProcessingListBox.SelectedItems.Count == 0)
        {
            ModifyButton.IsEnabled = false;
            DeleteButton.IsEnabled = false;
            CopyButton.IsEnabled = false;
        }
        else
        {
            ModifyButton.IsEnabled = true;
            DeleteButton.IsEnabled = true;
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
            SettingsImage.Source = new BitmapImage(new((int)SettingsRowDefinition.Height.Value == Common.SettingsRowDefinitionMinimize ? "/Resources/SettingsWhite.png" : "/Resources/CloseSettingsWhite.png", UriKind.Relative));
        }
        else
        {
            AddImage.Source = new BitmapImage(new("/Resources/AdditionDark.png", UriKind.Relative));
            ModifyImage.Source = new BitmapImage(new("/Resources/ModifyDark.png", UriKind.Relative));
            DeleteImage.Source = new BitmapImage(new("/Resources/DeleteDark.png", UriKind.Relative));
            CopyImage.Source = new BitmapImage(new("/Resources/CopyDark.png", UriKind.Relative));
            SettingsImage.Source = new BitmapImage(new((int)SettingsRowDefinition.Height.Value == Common.SettingsRowDefinitionMinimize ? "/Resources/SettingsDark.png" : "/Resources/CloseSettingsDark.png", UriKind.Relative));
        }
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
            if (CancelAllWindowPositionSizeWindow == null)
            {
                CancelAllWindowPositionSizeWindow = new(Window.GetWindow(this));
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
                    if (CancelNextEvent)
                    {
                        CancelNextEvent = false;
                    }
                    else
                    {
                        CancelNextEvent = true;
                        ProcessingStateToggleSwitch.IsOn = ApplicationData.Settings.AllWindowInformation.Enabled;
                    }
                    break;
                case ProcessingEventType.LanguageChanged:
                    SettingsTextOnControls();
                    break;
                case ProcessingEventType.ThemeChanged:
                    SettingsControlsImage();
                    break;
                case ProcessingEventType.CoordinateChanged:
                    SettingsDisplayControlsEnabled();
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
        object sender,
        RoutedEventArgs e
        )
    {
        try
        {
            if (CancelAllWindowPositionSizeWindow == null)
            {
                CancelAllWindowPositionSizeWindow = new(Window.GetWindow(this), CancelProcessingListBox.SelectedIndex);
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
                    int selectedIndex = CancelProcessingListBox.SelectedIndex;      // 選択している項目のインデックス
                    UpdateCancelProcessingListBoxItems();
                    CancelProcessingListBox.SelectedIndex = selectedIndex;
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
        object sender,
        RoutedEventArgs e
        )
    {
        try
        {
            if (FEMessageBox.Show(ApplicationData.Settings.AllWindowInformation.CancelAllWindowPositionSize[CancelProcessingListBox.SelectedIndex].RegisteredName + Environment.NewLine + ApplicationData.Languages.LanguagesWindow?.AllowDelete, ApplicationData.Languages.Check, MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                try
                {
                    ApplicationData.Settings.AllWindowInformation.CancelAllWindowPositionSize.RemoveAt(CancelProcessingListBox.SelectedIndex);
                    CancelProcessingListBox.Items.RemoveAt(CancelProcessingListBox.SelectedIndex);
                    FEMessageBox.Show(ApplicationData.Languages.LanguagesWindow?.Deleted ?? "", ApplicationData.Languages.Check, MessageBoxButton.OK);
                    CancelProcessingListBox.Focus();
                }
                catch
                {
                    FEMessageBox.Show(ApplicationData.Languages.ErrorOccurred, ApplicationData.Languages.Check, MessageBoxButton.OK);
                }
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
            WindowJudgementInformation newItem = new(ApplicationData.Settings.AllWindowInformation.CancelAllWindowPositionSize[CancelProcessingListBox.SelectedIndex]);     // 新しい項目
            int number = 1;     // 番号
            string stringData = newItem.RegisteredName + Common.CopySeparateString + ApplicationData.Languages.LanguagesWindow?.Copy + Common.SpaceSeparateString;
            for (int count = 0; count < ApplicationData.Settings.AllWindowInformation.CancelAllWindowPositionSize.Count; count++)
            {
                if (ApplicationData.Settings.AllWindowInformation.CancelAllWindowPositionSize[count].RegisteredName == (stringData + number))
                {
                    // 番号を変えて最初から確認
                    count = 0;
                    number++;
                }
            }
            newItem.RegisteredName = stringData + number;
            int selectedIindex = CancelProcessingListBox.SelectedIndex;      // 選択している項目のインデックス
            ApplicationData.Settings.AllWindowInformation.CancelAllWindowPositionSize.Add(new(newItem));
            UpdateCancelProcessingListBoxItems();
            CancelProcessingListBox.SelectedIndex = selectedIindex;
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
                ApplicationData.Settings.AllWindowInformation.Enabled = ProcessingStateToggleSwitch.IsOn;
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
            if (ApplicationData.Languages.LanguagesWindow == null)
            {
                return;
            }

            string stringData = (string)((ComboBoxItem)StandardDisplayComboBox.SelectedItem).Content;
            if (stringData == ApplicationData.Languages.LanguagesWindow.CurrentDisplay)
            {
                ApplicationData.Settings.AllWindowInformation.StandardDisplay = StandardDisplay.CurrentDisplay;
            }
            else if (stringData == ApplicationData.Languages.LanguagesWindow.SpecifiedDisplay)
            {
                ApplicationData.Settings.AllWindowInformation.StandardDisplay = StandardDisplay.SpecifiedDisplay;
            }
            SettingsEnabledDisplayControls();
        }
        catch
        {
        }
    }

    /// <summary>
    /// ディスプレイのコントロールの「Enabled」を設定
    /// </summary>
    private void SettingsEnabledDisplayControls()
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
            if (ApplicationData.Languages.LanguagesWindow == null)
            {
                return;
            }

            SettingsXControlsEnabled();
            string stringData = (string)((ComboBoxItem)MoveAllWindowToSpecifiedXComboBox.SelectedItem).Content;
            if (stringData == ApplicationData.Languages.LanguagesWindow.DoNotChange)
            {
                ApplicationData.Settings.AllWindowInformation.PositionSize.XType = WindowXType.DoNotChange;
            }
            else if (stringData == ApplicationData.Languages.LanguagesWindow.LeftEdge)
            {
                ApplicationData.Settings.AllWindowInformation.PositionSize.XType = WindowXType.Left;
            }
            else if (stringData == ApplicationData.Languages.LanguagesWindow.Middle)
            {
                ApplicationData.Settings.AllWindowInformation.PositionSize.XType = WindowXType.Middle;
            }
            else if (stringData == ApplicationData.Languages.LanguagesWindow.RightEdge)
            {
                ApplicationData.Settings.AllWindowInformation.PositionSize.XType = WindowXType.Right;
            }
            else if (stringData == ApplicationData.Languages.LanguagesWindow.CoordinateSpecification)
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
            if ((string)((ComboBoxItem)MoveAllWindowToSpecifiedXTypeComboBox.SelectedItem).Content == ApplicationData.Languages.LanguagesWindow?.Percent)
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
            if (ApplicationData.Languages.LanguagesWindow == null)
            {
                return;
            }

            SettingsYControlsEnabled();
            string stringData = (string)((ComboBoxItem)MoveAllWindowToSpecifiedYComboBox.SelectedItem).Content;
            if (stringData == ApplicationData.Languages.LanguagesWindow.DoNotChange)
            {
                ApplicationData.Settings.AllWindowInformation.PositionSize.YType = WindowYType.DoNotChange;
            }
            else if (stringData == ApplicationData.Languages.LanguagesWindow.TopEdge)
            {
                ApplicationData.Settings.AllWindowInformation.PositionSize.YType = WindowYType.Top;
            }
            else if (stringData == ApplicationData.Languages.LanguagesWindow.Middle)
            {
                ApplicationData.Settings.AllWindowInformation.PositionSize.YType = WindowYType.Middle;
            }
            else if (stringData == ApplicationData.Languages.LanguagesWindow.BottomEdge)
            {
                ApplicationData.Settings.AllWindowInformation.PositionSize.YType = WindowYType.Bottom;
            }
            else if (stringData == ApplicationData.Languages.LanguagesWindow.CoordinateSpecification)
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
            if ((string)((ComboBoxItem)MoveAllWindowToSpecifiedYTypeComboBox.SelectedItem).Content == ApplicationData.Languages.LanguagesWindow?.Percent)
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
            ApplicationData.Settings.AllWindowInformation.AllWindowPositionSizeWindowEventData.MoveSizeEnd = EventMoveSizeEndToggleSwitch.IsOn;
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
            ApplicationData.Settings.AllWindowInformation.AllWindowPositionSizeWindowEventData.Show = EventShowToggleSwitch.IsOn;
            ApplicationData.EventData.DoProcessingEvent(ProcessingEventType.AllWindowProcessingStateChanged);
        }
        catch
        {
            FEMessageBox.Show(ApplicationData.Languages.ErrorOccurred, ApplicationData.Languages.Check, MessageBoxButton.OK);
        }
    }

    /// <summary>
    /// 「X」関係のコントロールの有効状態を設定
    /// </summary>
    private void SettingsXControlsEnabled()
    {
        if ((string)((ComboBoxItem)MoveAllWindowToSpecifiedXComboBox.SelectedItem).Content == ApplicationData.Languages.LanguagesWindow?.CoordinateSpecification)
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
    /// 「Y」関係のコントロールの有効状態を設定
    /// </summary>
    private void SettingsYControlsEnabled()
    {
        if ((string)((ComboBoxItem)MoveAllWindowToSpecifiedYComboBox.SelectedItem).Content == ApplicationData.Languages.LanguagesWindow?.CoordinateSpecification)
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
    private void UpdateCancelProcessingListBoxItems()
    {
        CancelProcessingListBox.Items.Clear();

        if (ApplicationData.Settings.AllWindowInformation.CancelAllWindowPositionSize.Count != 0)
        {
            foreach (WindowJudgementInformation nowItem in ApplicationData.Settings.AllWindowInformation.CancelAllWindowPositionSize)
            {
                ListBoxItem newItem = new()
                {
                    Content = nowItem.RegisteredName
                };
                CancelProcessingListBox.Items.Add(newItem);
            }
        }
    }
}
