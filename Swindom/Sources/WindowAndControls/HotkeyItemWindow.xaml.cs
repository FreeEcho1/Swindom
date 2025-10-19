namespace Swindom;

/// <summary>
/// 「ホットキー」の「追加/修正」ウィンドウ
/// </summary>
public partial class HotkeyItemWindow : Window
{
    /// <summary>
    /// 追加/修正したかの値 (いいえ「false」/はい「true」)
    /// </summary>
    public bool AddedModified { get; private set; }
    /// <summary>
    /// 修正する項目のインデックス (追加「-1」)
    /// </summary>
    private int IndexOfItemToBeModified { get; } = -1;
    /// <summary>
    /// 「ホットキー」機能の項目情報
    /// </summary>
    private HotkeyItemInformation HotkeyItemInformation { get; } = new();

    /// <summary>
    /// コンストラクタ (使用しない)
    /// </summary>
    public HotkeyItemWindow()
    {
        throw new Exception("Do not use. - " + GetType().Name + "()");
    }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="indexOfItemToBeModified">修正する項目のインデックス (追加「-1」)</param>
    public HotkeyItemWindow(
        int indexOfItemToBeModified
        )
    {
        InitializeComponent();

        IndexOfItemToBeModified = indexOfItemToBeModified;
        if (IndexOfItemToBeModified != -1)
        {
            HotkeyItemInformation.Copy(ApplicationData.Settings.HotkeyInformation.Items[IndexOfItemToBeModified]);
        }

        Owner = WindowManagement.GetWindowToSetOwner();
        SizeToContent = SizeToContent.Manual;
        Width = (ApplicationData.Settings.HotkeyInformation.AddModifyWindowSize.Width < MinWidth) ? MinWidth : ApplicationData.Settings.HotkeyInformation.AddModifyWindowSize.Width;
        Height = (ApplicationData.Settings.HotkeyInformation.AddModifyWindowSize.Height < MinHeight) ? MinHeight : ApplicationData.Settings.HotkeyInformation.AddModifyWindowSize.Height;
        if ((ApplicationData.Settings.CoordinateType == CoordinateType.PrimaryDisplay) || (ApplicationData.MonitorInformation.MonitorInfo.Count == 1))
        {
            StandardDisplayLabel.Visibility = Visibility.Collapsed;
            StandardDisplayComboBox.Visibility = Visibility.Collapsed;
            DisplayLabel.Visibility = Visibility.Collapsed;
            DisplayComboBox.Visibility = Visibility.Collapsed;
        }
        foreach (MonitorInfoEx nowMonitorInfo in ApplicationData.MonitorInformation.MonitorInfo)
        {
            ComboBoxItem newItem = new()
            {
                Content = nowMonitorInfo.DeviceName
            };
            DisplayComboBox.Items.Add(newItem);
        }
        AmountOfMovementNumberBox.Minimum = HotkeyItemInformation.MinimumAmountOfMovement;
        AmountOfMovementNumberBox.Maximum = HotkeyItemInformation.MaximumAmountOfMovement;
        SizeChangeAmountNumberBox.Minimum = HotkeyItemInformation.MinimumSizeChangeAmount;
        SizeChangeAmountNumberBox.Maximum = HotkeyItemInformation.MaximumSizeChangeAmount;
        TransparencyNumberBox.Minimum = HotkeyItemInformation.MinimumTransparency;
        TransparencyNumberBox.Maximum = HotkeyItemInformation.MaximumTransparency;

        if (IndexOfItemToBeModified == -1)
        {
            Title = ApplicationData.Strings.Add;
            AddModifyButton.Content = ApplicationData.Strings.Add;
        }
        else
        {
            Title = ApplicationData.Strings.Modify;
            AddModifyButton.Content = ApplicationData.Strings.Modify;
        }
        RegisteredNameLabel.Content = ApplicationData.Strings.RegisteredName;
        TypeOfProcessingLabel.Content = ApplicationData.Strings.ProcessingType;
        TypeOfProcessingSpecifyPositionAndSizeComboBoxItem.Content = ApplicationData.Strings.SpecifyPositionAndSize;
        TypeOfProcessingMoveXCoordinateComboBoxItem.Content = ApplicationData.Strings.MoveXCoordinate;
        TypeOfProcessingMoveYCoordinateComboBoxItem.Content = ApplicationData.Strings.MoveYCoordinate;
        TypeOfProcessingIncreaseDecreaseWidthComboBoxItem.Content = ApplicationData.Strings.IncreaseDecreaseWidth;
        TypeOfProcessingIncreaseDecreaseHeightComboBoxItem.Content = ApplicationData.Strings.IncreaseDecreaseHeight;
        TypeOfProcessingIncreaseDecreaseWidthAndHeightComboBoxItem.Content = ApplicationData.Strings.IncreaseDecreaseWidthAndHeight;
        TypeOfProcessingAlwaysShowOrCancelOnTopComboBoxItem.Content = ApplicationData.Strings.AlwaysShowOrCancelOnTop;
        TypeOfProcessingSpecifyCancelTransparencyComboBoxItem.Content = ApplicationData.Strings.SpecifyCancelTransparency;
        TypeOfProcessingTitleBarAndBorderShowAndHiddenComboBoxItem.Content = ApplicationData.Strings.TitleBarAndBorderShowAndHidden;
        TypeOfProcessingStartStopProcessingOfSpecifyWindowComboBoxItem.Content = ApplicationData.Strings.StartStopProcessingOfSpecifyWindow;
        TypeOfProcessingBatchProcessingOfSpecifyWindowComboBoxItem.Content = ApplicationData.Strings.BatchProcessingOfSpecifyWindow;
        TypeOfProcessingOnlyActiveWindowSpecifyWindowComboBoxItem.Content = ApplicationData.Strings.OnlyActiveWindowSpecifyWindow;
        TypeOfProcessingStartStopProcessingOfAllWindowComboBoxItem.Content = ApplicationData.Strings.StartStopProcessingOfAllWindow;
        TypeOfProcessingStartStopProcessingOfMagnetComboBoxItem.Content = ApplicationData.Strings.StartStopProcessingOfMagnet;
        TypeOfProcessingShowThisApplicationWindowComboBoxItem.Content = ApplicationData.Strings.ShowThisApplicationWindow;
        TypeOfProcessingShowNotifyIconContextMenuComboBoxItem.Content = ApplicationData.Strings.ShowSystemTrayIconMenu;
        StandardDisplayLabel.Content = ApplicationData.Strings.DisplayToUseAsStandard;
        StandardDisplayCurrentDisplayComboBoxItem.Content = ApplicationData.Strings.CurrentDisplay;
        StandardDisplaySpecifiedDisplayComboBoxItem.Content = ApplicationData.Strings.SpecifiedDisplay;
        DisplayLabel.Content = ApplicationData.Strings.Display;
        WindowStateLabel.Content = ApplicationData.Strings.WindowState;
        WindowStateDoNotChangeComboBoxItem.Content = ApplicationData.Strings.DoNotChange;
        WindowStateNormalWindowComboBoxItem.Content = ApplicationData.Strings.NormalWindow;
        WindowStateMaximizeComboBoxItem.Content = ApplicationData.Strings.Maximize;
        WindowStateMinimizeComboBoxItem.Content = ApplicationData.Strings.Minimize;
        XLabel.Content = ApplicationData.Strings.X;
        XDoNotChangeComboBoxItem.Content = ApplicationData.Strings.DoNotChange;
        XLeftEdgeComboBoxItem.Content = ApplicationData.Strings.LeftEdge;
        XMiddleComboBoxItem.Content = ApplicationData.Strings.Middle;
        XRightEdgeComboBoxItem.Content = ApplicationData.Strings.RightEdge;
        XCoordinateSpecificationComboBoxItem.Content = ApplicationData.Strings.CoordinateSpecification;
        XTypePixelComboBoxItem.Content = ApplicationData.Strings.Pixel;
        XTypePercentComboBoxItem.Content = ApplicationData.Strings.Percent;
        YLabel.Content = ApplicationData.Strings.Y;
        YDoNotChangeComboBoxItem.Content = ApplicationData.Strings.DoNotChange;
        YTopEdgeComboBoxItem.Content = ApplicationData.Strings.TopEdge;
        YMiddleComboBoxItem.Content = ApplicationData.Strings.Middle;
        YBottomEdgeComboBoxItem.Content = ApplicationData.Strings.BottomEdge;
        YCoordinateSpecificationComboBoxItem.Content = ApplicationData.Strings.CoordinateSpecification;
        YTypePixelComboBoxItem.Content = ApplicationData.Strings.Pixel;
        YTypePercentComboBoxItem.Content = ApplicationData.Strings.Percent;
        WidthLabel.Content = ApplicationData.Strings.Width;
        WidthDoNotChangeComboBoxItem.Content = ApplicationData.Strings.DoNotChange;
        WidthWidthSpecificationComboBoxItem.Content = ApplicationData.Strings.WidthSpecification;
        WidthTypePixelComboBoxItem.Content = ApplicationData.Strings.Pixel;
        WidthTypePercentComboBoxItem.Content = ApplicationData.Strings.Percent;
        HeightLabel.Content = ApplicationData.Strings.Height;
        HeightDoNotChangeComboBoxItem.Content = ApplicationData.Strings.DoNotChange;
        HeightHeightSpecificationComboBoxItem.Content = ApplicationData.Strings.HeightSpecification;
        HeightTypePixelComboBoxItem.Content = ApplicationData.Strings.Pixel;
        HeightTypePercentComboBoxItem.Content = ApplicationData.Strings.Percent;
        ClientAreaToggleSwitch.OffContent = ClientAreaToggleSwitch.OnContent = ApplicationData.Strings.ClientArea;
        AmountOfMovementLabel.Content = ApplicationData.Strings.AmountOfMovement;
        AmountOfMovementPixelLabel.Content = ApplicationData.Strings.Pixel;
        SizeChangeAmountLabel.Content = ApplicationData.Strings.SizeChangeAmount;
        SizeChangeAmountPixelLabel.Content = ApplicationData.Strings.Pixel;
        TransparencyLabel.Content = ApplicationData.Strings.Transparency;
        HotkeyLabel.Content = ApplicationData.Strings.Hotkey;
        CancelButton.Content = ApplicationData.Strings.Cancel;

        string stringData;
        RegisteredNameTextBox.Text = HotkeyItemInformation.RegisteredName;
        stringData = HotkeyItemInformation.ProcessingType switch
        {
            HotkeyProcessingType.PositionSize => ApplicationData.Strings.SpecifyPositionAndSize,
            HotkeyProcessingType.MoveX => ApplicationData.Strings.MoveXCoordinate,
            HotkeyProcessingType.MoveY => ApplicationData.Strings.MoveYCoordinate,
            HotkeyProcessingType.IncreaseDecreaseWidth => ApplicationData.Strings.IncreaseDecreaseWidth,
            HotkeyProcessingType.IncreaseDecreaseHeight => ApplicationData.Strings.IncreaseDecreaseHeight,
            HotkeyProcessingType.IncreaseDecreaseWidthHeight => ApplicationData.Strings.IncreaseDecreaseWidthAndHeight,
            HotkeyProcessingType.AlwaysForefrontOrCancel => ApplicationData.Strings.AlwaysShowOrCancelOnTop,
            HotkeyProcessingType.SpecifyTransparencyOrCancel => ApplicationData.Strings.SpecifyCancelTransparency,
            HotkeyProcessingType.TitleBarAndBorderShowAndHidden => ApplicationData.Strings.TitleBarAndBorderShowAndHidden,
            HotkeyProcessingType.StartStopSpecifyWindow => ApplicationData.Strings.StartStopProcessingOfSpecifyWindow,
            HotkeyProcessingType.BatchSpecifyWindow => ApplicationData.Strings.BatchProcessingOfSpecifyWindow,
            HotkeyProcessingType.OnlyActiveWindowSpecifyWindow => ApplicationData.Strings.OnlyActiveWindowSpecifyWindow,
            HotkeyProcessingType.StartStopAllWindow => ApplicationData.Strings.StartStopProcessingOfAllWindow,
            HotkeyProcessingType.StartStopMagnet => ApplicationData.Strings.StartStopProcessingOfMagnet,
            HotkeyProcessingType.ShowThisApplicationWindow => ApplicationData.Strings.ShowThisApplicationWindow,
            HotkeyProcessingType.ShowNotifyIconMenu => ApplicationData.Strings.ShowSystemTrayIconMenu,
            _ => ApplicationData.Strings.SpecifyPositionAndSize
        };
        ControlsProcessing.SelectComboBoxItem(TypeOfProcessingComboBox, stringData);
        switch (HotkeyItemInformation.StandardDisplay)
        {
            case StandardDisplay.SpecifiedDisplay:
                stringData = ApplicationData.Strings.SpecifiedDisplay;
                break;
            default:
                stringData = ApplicationData.Strings.CurrentDisplay;
                DisplayLabel.IsEnabled = false;
                DisplayComboBox.IsEnabled = false;
                break;
        }
        ControlsProcessing.SelectComboBoxItem(StandardDisplayComboBox, stringData);
        ControlsProcessing.SelectComboBoxItem(DisplayComboBox, HotkeyItemInformation.PositionSize.Display);
        if (DisplayComboBox.SelectedIndex == -1)
        {
            DisplayComboBox.SelectedIndex = 0;
        }
        stringData = HotkeyItemInformation.PositionSize.SettingsWindowState switch
        {
            SettingsWindowState.Normal => ApplicationData.Strings.NormalWindow,
            SettingsWindowState.Maximize => ApplicationData.Strings.Maximize,
            SettingsWindowState.Minimize => ApplicationData.Strings.Minimize,
            _ => ApplicationData.Strings.DoNotChange
        };
        ControlsProcessing.SelectComboBoxItem(WindowStateComboBox, stringData);
        stringData = HotkeyItemInformation.PositionSize.XType switch
        {
            WindowXType.DoNotChange => ApplicationData.Strings.DoNotChange,
            WindowXType.Left => ApplicationData.Strings.LeftEdge,
            WindowXType.Middle => ApplicationData.Strings.Middle,
            WindowXType.Right => ApplicationData.Strings.RightEdge,
            WindowXType.Value => ApplicationData.Strings.CoordinateSpecification,
            _ => ApplicationData.Strings.DoNotChange,
        };
        ControlsProcessing.SelectComboBoxItem(XComboBox, stringData);
        stringData = HotkeyItemInformation.PositionSize.XValueType switch
        {
            PositionSizeValueType.Percent => ApplicationData.Strings.Percent,
            _ => ApplicationData.Strings.Pixel
        };
        ControlsProcessing.SelectComboBoxItem(XTypeComboBox, stringData);
        XNumberBox.Value = HotkeyItemInformation.PositionSize.X;
        stringData = HotkeyItemInformation.PositionSize.YType switch
        {
            WindowYType.DoNotChange => ApplicationData.Strings.DoNotChange,
            WindowYType.Top => ApplicationData.Strings.TopEdge,
            WindowYType.Middle => ApplicationData.Strings.Middle,
            WindowYType.Bottom => ApplicationData.Strings.BottomEdge,
            WindowYType.Value => ApplicationData.Strings.CoordinateSpecification,
            _ => ApplicationData.Strings.DoNotChange,
        };
        ControlsProcessing.SelectComboBoxItem(YComboBox, stringData);
        stringData = HotkeyItemInformation.PositionSize.YValueType switch
        {
            PositionSizeValueType.Percent => ApplicationData.Strings.Percent,
            _ => ApplicationData.Strings.Pixel
        };
        ControlsProcessing.SelectComboBoxItem(YTypeComboBox, stringData);
        YNumberBox.Value = HotkeyItemInformation.PositionSize.Y;
        stringData = HotkeyItemInformation.PositionSize.WidthType switch
        {
            WindowSizeType.DoNotChange => ApplicationData.Strings.DoNotChange,
            WindowSizeType.Value => ApplicationData.Strings.WidthSpecification,
            _ => ApplicationData.Strings.DoNotChange,
        };
        ControlsProcessing.SelectComboBoxItem(WidthComboBox, stringData);
        stringData = HotkeyItemInformation.PositionSize.WidthValueType switch
        {
            PositionSizeValueType.Percent => ApplicationData.Strings.Percent,
            _ => ApplicationData.Strings.Pixel
        };
        ControlsProcessing.SelectComboBoxItem(WidthTypeComboBox, stringData);
        WidthNumberBox.Value = HotkeyItemInformation.PositionSize.Width;
        stringData = HotkeyItemInformation.PositionSize.HeightType switch
        {
            WindowSizeType.DoNotChange => ApplicationData.Strings.DoNotChange,
            WindowSizeType.Value => ApplicationData.Strings.HeightSpecification,
            _ => ApplicationData.Strings.DoNotChange,
        };
        ControlsProcessing.SelectComboBoxItem(HeightComboBox, stringData);
        stringData = HotkeyItemInformation.PositionSize.HeightValueType switch
        {
            PositionSizeValueType.Percent => ApplicationData.Strings.Percent,
            _ => ApplicationData.Strings.Pixel
        };
        ControlsProcessing.SelectComboBoxItem(HeightTypeComboBox, stringData);
        HeightNumberBox.Value = HotkeyItemInformation.PositionSize.Height;
        ClientAreaToggleSwitch.IsOn = HotkeyItemInformation.PositionSize.ClientArea;
        switch (HotkeyItemInformation.ProcessingType)
        {
            case HotkeyProcessingType.MoveX:
            case HotkeyProcessingType.MoveY:
                AmountOfMovementNumberBox.Value = HotkeyItemInformation.ProcessingValue;
                break;
            case HotkeyProcessingType.IncreaseDecreaseWidth:
            case HotkeyProcessingType.IncreaseDecreaseHeight:
            case HotkeyProcessingType.IncreaseDecreaseWidthHeight:
                SizeChangeAmountNumberBox.Value = HotkeyItemInformation.ProcessingValue;
                break;
            case HotkeyProcessingType.SpecifyTransparencyOrCancel:
                TransparencyNumberBox.Value = HotkeyItemInformation.ProcessingValue;
                break;
        }
        HotkeyTextBox.Text = FreeEcho.FEHotKeyWPF.HotKeyWPF.GetHotKeyString(HotkeyItemInformation.Hotkey);
        SettingsControlsIsEnabled();
        SettingsAddModifyButtonIsEnabled();

        Closed += HotkeyWindowProcessingAddModifyWindow_Closed;
        RegisteredNameTextBox.TextChanged += RegisteredNameTextBox_TextChanged;
        TypeOfProcessingComboBox.SelectionChanged += TypeOfProcessingComboBox_SelectionChanged;
        StandardDisplayComboBox.SelectionChanged += StandardDisplayComboBox_SelectionChanged;
        WindowStateComboBox.SelectionChanged += WindowStateComboBox_SelectionChanged;
        XComboBox.SelectionChanged += XComboBox_SelectionChanged;
        XTypeComboBox.SelectionChanged += ComboBoxXType_SelectionChanged;
        YComboBox.SelectionChanged += ComboBoxY_SelectionChanged;
        YTypeComboBox.SelectionChanged += YTypeComboBox_SelectionChanged;
        WidthComboBox.SelectionChanged += WidthComboBox_SelectionChanged;
        WidthTypeComboBox.SelectionChanged += WidthTypeComboBox_SelectionChanged;
        HeightComboBox.SelectionChanged += HeightComboBox_SelectionChanged;
        HeightTypeComboBox.SelectionChanged += HeightTypeComboBox_SelectionChanged;
        HotkeyTextBox.GotFocus += HotkeyTextBox_GotFocus;
        HotkeyTextBox.LostFocus += HotkeyTextBox_LostFocus;
        HotkeyTextBox.PreviewKeyDown += HotkeyTextBox_PreviewKeyDown;
        AddModifyButton.Click += AddModifyButton_Click;
        CancelButton.Click += CancelButton_Click;
    }

    /// <summary>
    /// ウィンドウの「Closed」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void HotkeyWindowProcessingAddModifyWindow_Closed(
        object? sender,
        EventArgs e
        )
    {
        try
        {
            ApplicationData.Settings.HotkeyInformation.AddModifyWindowSize.Width = (int)Width;
            ApplicationData.Settings.HotkeyInformation.AddModifyWindowSize.Height = (int)Height;
            ApplicationData.EventData.DoProcessingEvent(ProcessingEventType.UnpauseHotkeyProcessing);
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「登録名」TextBoxの「TextChanged」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void RegisteredNameTextBox_TextChanged(
        object sender,
        TextChangedEventArgs e
        )
    {
        try
        {
            SettingsAddModifyButtonIsEnabled();
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「処理の種類」ComboBoxの「SelectionChanged」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void TypeOfProcessingComboBox_SelectionChanged(
        object sender,
        SelectionChangedEventArgs e
        )
    {
        try
        {
            SettingsControlsIsEnabled();
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
            if ((string)((ComboBoxItem)StandardDisplayComboBox.SelectedItem).Content == ApplicationData.Strings.CurrentDisplay)
            {
                DisplayLabel.IsEnabled = false;
                DisplayComboBox.IsEnabled = false;
            }
            else
            {
                DisplayLabel.IsEnabled = true;
                DisplayComboBox.IsEnabled = true;
            }
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「ウィンドウの状態」ComboBoxの「SelectionChanged」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void WindowStateComboBox_SelectionChanged(
        object sender,
        SelectionChangedEventArgs e
        )
    {
        try
        {
            SettingsControlsIsEnabled();
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「X」ComboBoxの「SelectionChanged」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void XComboBox_SelectionChanged(
        object sender,
        SelectionChangedEventArgs e
        )
    {
        try
        {
            SettingsXControlsIsEnabled();
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「X」の「値の種類」ComboBoxの「SelectionChanged」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ComboBoxXType_SelectionChanged(
        object sender,
        SelectionChangedEventArgs e
        )
    {
        try
        {
            if ((string)((ComboBoxItem)XTypeComboBox.SelectedItem).Content == ApplicationData.Strings.Percent)
            {
                XNumberBox.Minimum = PositionSize.PositionSizePercentMinimum;
                XNumberBox.Maximum = PositionSize.PositionSizePercentMaximum;
            }
            else
            {
                XNumberBox.Minimum = PositionSize.PositionPixelMinimum;
                XNumberBox.Maximum = PositionSize.PositionSizePixelMaximum;
            }
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「Y」ComboBoxの「SelectionChange」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ComboBoxY_SelectionChanged(
        object sender,
        SelectionChangedEventArgs e
        )
    {
        try
        {
            SettingsYControlsIsEnabled();
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「Y」の「値の種類」ComboBoxの「SelectionChanged」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void YTypeComboBox_SelectionChanged(
        object sender,
        SelectionChangedEventArgs e
        )
    {
        try
        {
            if ((string)((ComboBoxItem)YTypeComboBox.SelectedItem).Content == ApplicationData.Strings.Percent)
            {
                YNumberBox.Minimum = PositionSize.PositionSizePercentMinimum;
                YNumberBox.Maximum= PositionSize.PositionSizePercentMaximum;
            }
            else
            {
                YNumberBox.Minimum = PositionSize.PositionPixelMinimum;
                YNumberBox.Maximum = PositionSize.PositionSizePixelMaximum;
            }
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「幅」ComboBoxの「SelectionChanged」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void WidthComboBox_SelectionChanged(
        object sender,
        SelectionChangedEventArgs e
        )
    {
        try
        {
            SettingsWidthControlsIsEnabled();
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「幅」の「値の種類」ComboBoxの「SelectionChanged」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void WidthTypeComboBox_SelectionChanged(
        object sender,
        SelectionChangedEventArgs e
        )
    {
        try
        {
            if ((string)((ComboBoxItem)WidthTypeComboBox.SelectedItem).Content == ApplicationData.Strings.Percent)
            {
                WidthNumberBox.Minimum = PositionSize.PositionSizePercentMinimum;
                WidthNumberBox.Maximum = PositionSize.PositionSizePercentMaximum;
            }
            else
            {
                WidthNumberBox.Minimum = PositionSize.SizePixelMinimum;
                WidthNumberBox.Maximum = PositionSize.PositionSizePixelMaximum;
            }
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「高さ」ComboBoxの「SelectionChanged」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void HeightComboBox_SelectionChanged(
        object sender,
        SelectionChangedEventArgs e
        )
    {
        try
        {
            SettingsHeightControlsIsEnabled();
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「高さ」の「値の種類」ComboBoxの「SelectionChanged」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void HeightTypeComboBox_SelectionChanged(
        object sender,
        SelectionChangedEventArgs e
        )
    {
        try
        {
            if ((string)((ComboBoxItem)HeightTypeComboBox.SelectedItem).Content == ApplicationData.Strings.Percent)
            {
                HeightNumberBox.Minimum = PositionSize.PositionSizePercentMinimum;
                HeightNumberBox.Maximum = PositionSize.PositionSizePercentMaximum;
            }
            else
            {
                HeightNumberBox.Minimum = PositionSize.SizePixelMinimum;
                HeightNumberBox.Maximum = PositionSize.PositionSizePixelMaximum;
            }
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「ホットキー」TextBoxの「GotFocus」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void HotkeyTextBox_GotFocus(
        object sender,
        RoutedEventArgs e
        )
    {
        try
        {
            ApplicationData.EventData.DoProcessingEvent(ProcessingEventType.PauseHotkeyProcessing);
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「ホットキー」TextBoxの「LostFocus」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void HotkeyTextBox_LostFocus(
        object sender,
        RoutedEventArgs e
        )
    {
        try
        {
            ApplicationData.EventData.DoProcessingEvent(ProcessingEventType.UnpauseHotkeyProcessing);
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「ホットキー」TextBoxの「PreviewKeyDown」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void HotkeyTextBox_PreviewKeyDown(
        object sender,
        KeyEventArgs e
        )
    {
        try
        {
            FreeEcho.FEHotKeyWPF.HotKeyInformation hotkeyInformation = FreeEcho.FEHotKeyWPF.HotKeyWPF.GetHotKey(e, true);
            string hotkeyString = FreeEcho.FEHotKeyWPF.HotKeyWPF.GetHotKeyString(hotkeyInformation);

            if (hotkeyString != WindowControlValue.TabString)
            {
                HotkeyItemInformation.Hotkey.Copy(hotkeyInformation);
                HotkeyTextBox.Text = hotkeyString;
                e.Handled = true;
            }
        }
        catch
        {
            FEMessageBox.Show(ApplicationData.Strings.ErrorOccurred, ApplicationData.Strings.Check, MessageBoxButton.OK);
        }
    }

    /// <summary>
    /// 「追加/修正」Buttonの「Click」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void AddModifyButton_Click(
        object sender,
        RoutedEventArgs e
        )
    {
        try
        {
            GetHotkeyInformationFromControls();
            string? resultString = CheckValueHotkeyItemInformation();
            if (string.IsNullOrEmpty(resultString))
            {
                ApplicationData.WindowProcessingManagement.HotkeyProcessing?.UnregisterHotkeys();
                try
                {
                    if (IndexOfItemToBeModified == -1)
                    {
                        ApplicationData.Settings.HotkeyInformation.Items.Add(HotkeyItemInformation);
                        FEMessageBox.Show(ApplicationData.Strings.Added, ApplicationData.Strings.Check, MessageBoxButton.OK);
                    }
                    else
                    {
                        ApplicationData.Settings.HotkeyInformation.Items[IndexOfItemToBeModified] = HotkeyItemInformation;
                        FEMessageBox.Show(ApplicationData.Strings.Modified, ApplicationData.Strings.Check, MessageBoxButton.OK);
                    }
                    AddedModified = true;
                }
                catch
                {
                    FEMessageBox.Show(ApplicationData.Strings.ErrorOccurred, ApplicationData.Strings.Check, MessageBoxButton.OK);
                }
                ApplicationData.WindowProcessingManagement.HotkeyProcessing?.RegisterHotkeys();
                Close();
            }
            else
            {
                FEMessageBox.Show(resultString, ApplicationData.Strings.Check, MessageBoxButton.OK);
            }
        }
        catch
        {
            FEMessageBox.Show(ApplicationData.Strings.ErrorOccurred, ApplicationData.Strings.Check, MessageBoxButton.OK);
        }
    }

    /// <summary>
    /// 「キャンセル」Buttonの「Click」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void CancelButton_Click(
        object sender,
        RoutedEventArgs e
        )
    {
        try
        {
            Close();
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
        if ((string)((ComboBoxItem)XComboBox.SelectedItem).Content == ApplicationData.Strings.CoordinateSpecification)
        {
            XNumberBox.IsEnabled = true;
            XTypeComboBox.IsEnabled = true;
        }
        else
        {
            XNumberBox.IsEnabled = false;
            XTypeComboBox.IsEnabled = false;
        }
    }

    /// <summary>
    /// 「Y」関係のコントロールの「IsEnabled」を設定
    /// </summary>
    private void SettingsYControlsIsEnabled()
    {
        if ((string)((ComboBoxItem)YComboBox.SelectedItem).Content == ApplicationData.Strings.CoordinateSpecification)
        {
            YNumberBox.IsEnabled = true;
            YTypeComboBox.IsEnabled = true;
        }
        else
        {
            YNumberBox.IsEnabled = false;
            YTypeComboBox.IsEnabled = false;
        }
    }

    /// <summary>
    /// 「幅」関係のコントロールの「IsEnabled」を設定
    /// </summary>
    private void SettingsWidthControlsIsEnabled()
    {
        if ((string)((ComboBoxItem)WidthComboBox.SelectedItem).Content == ApplicationData.Strings.WidthSpecification)
        {
            WidthNumberBox.IsEnabled = true;
            WidthTypeComboBox.IsEnabled = true;
        }
        else
        {
            WidthNumberBox.IsEnabled = false;
            WidthTypeComboBox.IsEnabled = false;
        }
    }

    /// <summary>
    /// 「高さ」関係のコントロールの「IsEnabled」を設定
    /// </summary>
    private void SettingsHeightControlsIsEnabled()
    {
        if ((string)((ComboBoxItem)HeightComboBox.SelectedItem).Content == ApplicationData.Strings.HeightSpecification)
        {
            HeightNumberBox.IsEnabled = true;
            HeightTypeComboBox.IsEnabled = true;
        }
        else
        {
            HeightNumberBox.IsEnabled = false;
            HeightTypeComboBox.IsEnabled = false;
        }
    }

    /// <summary>
    /// コントロールの「IsEnabled」を設定
    /// </summary>
    private void SettingsControlsIsEnabled()
    {
        string selectedItemString = (string)((ComboBoxItem)TypeOfProcessingComboBox.SelectedItem).Content;
        if (selectedItemString == ApplicationData.Strings.SpecifyPositionAndSize)
        {
            StandardDisplayLabel.IsEnabled = true;
            StandardDisplayComboBox.IsEnabled = true;
            DisplayLabel.IsEnabled = true;
            DisplayComboBox.IsEnabled = true;
            WindowStateLabel.IsEnabled = true;
            WindowStateComboBox.IsEnabled = true;
            string stringData = (string)((ComboBoxItem)WindowStateComboBox.SelectedItem).Content;
            if (stringData == ApplicationData.Strings.NormalWindow)
            {
                XLabel.IsEnabled = true;
                XGrid.IsEnabled = true;
                SettingsXControlsIsEnabled();
                YLabel.IsEnabled = true;
                YGrid.IsEnabled = true;
                SettingsYControlsIsEnabled();
                WidthLabel.IsEnabled = true;
                WidthGrid.IsEnabled = true;
                SettingsWidthControlsIsEnabled();
                HeightLabel.IsEnabled = true;
                HeightGrid.IsEnabled = true;
                SettingsHeightControlsIsEnabled();
                ClientAreaToggleSwitch.IsEnabled = true;
            }
            else
            {
                XLabel.IsEnabled = false;
                XGrid.IsEnabled = false;
                YLabel.IsEnabled = false;
                YGrid.IsEnabled = false;
                WidthLabel.IsEnabled = false;
                WidthGrid.IsEnabled = false;
                HeightLabel.IsEnabled = false;
                HeightGrid.IsEnabled = false;
                ClientAreaToggleSwitch.IsEnabled = false;
            }
        }
        else
        {
            StandardDisplayLabel.IsEnabled = false;
            StandardDisplayComboBox.IsEnabled = false;
            DisplayLabel.IsEnabled = false;
            DisplayComboBox.IsEnabled = false;
            WindowStateLabel.IsEnabled = false;
            WindowStateComboBox.IsEnabled = false;
            XLabel.IsEnabled = false;
            XGrid.IsEnabled = false;
            YLabel.IsEnabled = false;
            YGrid.IsEnabled = false;
            WidthLabel.IsEnabled = false;
            WidthGrid.IsEnabled = false;
            HeightLabel.IsEnabled = false;
            HeightGrid.IsEnabled = false;
            ClientAreaToggleSwitch.IsEnabled = false;
        }

        if ((selectedItemString == ApplicationData.Strings.MoveXCoordinate)
            || (selectedItemString == ApplicationData.Strings.MoveYCoordinate))
        {
            AmountOfMovementGrid.IsEnabled = true;
            SizeChangeAmountGrid.IsEnabled = false;
            TransparencyGrid.IsEnabled = false;
        }
        else if ((selectedItemString == ApplicationData.Strings.IncreaseDecreaseWidth)
            || (selectedItemString == ApplicationData.Strings.IncreaseDecreaseHeight)
            || (selectedItemString == ApplicationData.Strings.IncreaseDecreaseWidthAndHeight))
        {
            AmountOfMovementGrid.IsEnabled = false;
            SizeChangeAmountGrid.IsEnabled = true;
            TransparencyGrid.IsEnabled = false;
        }
        else if (selectedItemString == ApplicationData.Strings.SpecifyCancelTransparency)
        {
            AmountOfMovementGrid.IsEnabled = false;
            SizeChangeAmountGrid.IsEnabled = false;
            TransparencyGrid.IsEnabled = true;
        }
        else if (selectedItemString == ApplicationData.Strings.TitleBarAndBorderShowAndHidden)
        {
            AmountOfMovementGrid.IsEnabled = false;
            SizeChangeAmountGrid.IsEnabled = false;
            TransparencyGrid.IsEnabled = false;
        }
        else
        {
            AmountOfMovementGrid.IsEnabled = false;
            SizeChangeAmountGrid.IsEnabled = false;
            TransparencyGrid.IsEnabled = false;
        }
    }

    /// <summary>
    /// 「追加/修正」ボタンの有効状態を設定
    /// </summary>
    private void SettingsAddModifyButtonIsEnabled()
    {
        AddModifyButton.IsEnabled = !string.IsNullOrEmpty(RegisteredNameTextBox.Text);
    }

    /// <summary>
    /// コントロールからホットキー情報取得
    /// </summary>
    private void GetHotkeyInformationFromControls()
    {
        string stringData;     // 文字列データ

        HotkeyItemInformation.RegisteredName = RegisteredNameTextBox.Text;
        stringData = (string)((ComboBoxItem)TypeOfProcessingComboBox.SelectedItem).Content;
        if (stringData == ApplicationData.Strings.SpecifyPositionAndSize)
        {
            HotkeyItemInformation.ProcessingType = HotkeyProcessingType.PositionSize;
        }
        else if (stringData == ApplicationData.Strings.MoveXCoordinate)
        {
            HotkeyItemInformation.ProcessingType = HotkeyProcessingType.MoveX;
        }
        else if (stringData == ApplicationData.Strings.MoveYCoordinate)
        {
            HotkeyItemInformation.ProcessingType = HotkeyProcessingType.MoveY;
        }
        else if (stringData == ApplicationData.Strings.IncreaseDecreaseWidth)
        {
            HotkeyItemInformation.ProcessingType = HotkeyProcessingType.IncreaseDecreaseWidth;
        }
        else if (stringData == ApplicationData.Strings.IncreaseDecreaseHeight)
        {
            HotkeyItemInformation.ProcessingType = HotkeyProcessingType.IncreaseDecreaseHeight;
        }
        else if (stringData == ApplicationData.Strings.IncreaseDecreaseWidthAndHeight)
        {
            HotkeyItemInformation.ProcessingType = HotkeyProcessingType.IncreaseDecreaseWidthHeight;
        }
        else if (stringData == ApplicationData.Strings.AlwaysShowOrCancelOnTop)
        {
            HotkeyItemInformation.ProcessingType = HotkeyProcessingType.AlwaysForefrontOrCancel;
        }
        else if (stringData == ApplicationData.Strings.SpecifyCancelTransparency)
        {
            HotkeyItemInformation.ProcessingType = HotkeyProcessingType.SpecifyTransparencyOrCancel;
        }
        else if (stringData == ApplicationData.Strings.TitleBarAndBorderShowAndHidden)
        {
            HotkeyItemInformation.ProcessingType = HotkeyProcessingType.TitleBarAndBorderShowAndHidden;
        }
        else if (stringData == ApplicationData.Strings.StartStopProcessingOfSpecifyWindow)
        {
            HotkeyItemInformation.ProcessingType = HotkeyProcessingType.StartStopSpecifyWindow;
        }
        else if (stringData == ApplicationData.Strings.BatchProcessingOfSpecifyWindow)
        {
            HotkeyItemInformation.ProcessingType = HotkeyProcessingType.BatchSpecifyWindow;
        }
        else if (stringData == ApplicationData.Strings.OnlyActiveWindowSpecifyWindow)
        {
            HotkeyItemInformation.ProcessingType = HotkeyProcessingType.OnlyActiveWindowSpecifyWindow;
        }
        else if (stringData == ApplicationData.Strings.StartStopProcessingOfAllWindow)
        {
            HotkeyItemInformation.ProcessingType = HotkeyProcessingType.StartStopAllWindow;
        }
        else if (stringData == ApplicationData.Strings.StartStopProcessingOfMagnet)
        {
            HotkeyItemInformation.ProcessingType = HotkeyProcessingType.StartStopMagnet;
        }
        else if (stringData == ApplicationData.Strings.ShowThisApplicationWindow)
        {
            HotkeyItemInformation.ProcessingType = HotkeyProcessingType.ShowThisApplicationWindow;
        }
        else if (stringData == ApplicationData.Strings.ShowSystemTrayIconMenu)
        {
            HotkeyItemInformation.ProcessingType = HotkeyProcessingType.ShowNotifyIconMenu;
        }
        switch (HotkeyItemInformation.ProcessingType)
        {
            case HotkeyProcessingType.PositionSize:
                stringData = (string)((ComboBoxItem)StandardDisplayComboBox.SelectedItem).Content;
                if (stringData == ApplicationData.Strings.SpecifiedDisplay)
                {
                    HotkeyItemInformation.StandardDisplay = StandardDisplay.SpecifiedDisplay;
                }
                else if (stringData == ApplicationData.Strings.CurrentDisplay)
                {
                    HotkeyItemInformation.StandardDisplay = StandardDisplay.CurrentDisplay;
                }
                HotkeyItemInformation.PositionSize.Display = (string)((ComboBoxItem)DisplayComboBox.SelectedItem).Content;
                stringData = (string)((ComboBoxItem)WindowStateComboBox.SelectedItem).Content;
                if (stringData == ApplicationData.Strings.NormalWindow)
                {
                    HotkeyItemInformation.PositionSize.SettingsWindowState = SettingsWindowState.Normal;
                }
                else if (stringData == ApplicationData.Strings.Maximize)
                {
                    HotkeyItemInformation.PositionSize.SettingsWindowState = SettingsWindowState.Maximize;
                }
                else if (stringData == ApplicationData.Strings.Minimize)
                {
                    HotkeyItemInformation.PositionSize.SettingsWindowState = SettingsWindowState.Minimize;
                }
                else
                {
                    HotkeyItemInformation.PositionSize.SettingsWindowState = SettingsWindowState.DoNotChange;
                }
                stringData = (string)((ComboBoxItem)XComboBox.SelectedItem).Content;
                if (stringData == ApplicationData.Strings.DoNotChange)
                {
                    HotkeyItemInformation.PositionSize.XType = WindowXType.DoNotChange;
                }
                else if (stringData == ApplicationData.Strings.LeftEdge)
                {
                    HotkeyItemInformation.PositionSize.XType = WindowXType.Left;
                }
                else if (stringData == ApplicationData.Strings.Middle)
                {
                    HotkeyItemInformation.PositionSize.XType = WindowXType.Middle;
                }
                else if (stringData == ApplicationData.Strings.RightEdge)
                {
                    HotkeyItemInformation.PositionSize.XType = WindowXType.Right;
                }
                else if (stringData == ApplicationData.Strings.CoordinateSpecification)
                {
                    HotkeyItemInformation.PositionSize.XType = WindowXType.Value;
                    HotkeyItemInformation.PositionSize.X = XNumberBox.Value;
                    if ((string)((ComboBoxItem)XTypeComboBox.SelectedItem).Content == ApplicationData.Strings.Percent)
                    {
                        HotkeyItemInformation.PositionSize.XValueType = PositionSizeValueType.Percent;
                    }
                    else
                    {
                        HotkeyItemInformation.PositionSize.XValueType = PositionSizeValueType.Pixel;
                    }
                }
                stringData = (string)((ComboBoxItem)YComboBox.SelectedItem).Content;
                if (stringData == ApplicationData.Strings.DoNotChange)
                {
                    HotkeyItemInformation.PositionSize.YType = WindowYType.DoNotChange;
                }
                else if (stringData == ApplicationData.Strings.TopEdge)
                {
                    HotkeyItemInformation.PositionSize.YType = WindowYType.Top;
                }
                else if (stringData == ApplicationData.Strings.Middle)
                {
                    HotkeyItemInformation.PositionSize.YType = WindowYType.Middle;
                }
                else if (stringData == ApplicationData.Strings.BottomEdge)
                {
                    HotkeyItemInformation.PositionSize.YType = WindowYType.Bottom;
                }
                else if (stringData == ApplicationData.Strings.CoordinateSpecification)
                {
                    HotkeyItemInformation.PositionSize.YType = WindowYType.Value;
                    HotkeyItemInformation.PositionSize.Y = YNumberBox.Value;
                    if ((string)((ComboBoxItem)YTypeComboBox.SelectedItem).Content == ApplicationData.Strings.Percent)
                    {
                        HotkeyItemInformation.PositionSize.YValueType = PositionSizeValueType.Percent;
                    }
                    else
                    {
                        HotkeyItemInformation.PositionSize.YValueType = PositionSizeValueType.Pixel;
                    }
                }
                stringData = (string)((ComboBoxItem)WidthComboBox.SelectedItem).Content;
                if (stringData == ApplicationData.Strings.DoNotChange)
                {
                    HotkeyItemInformation.PositionSize.WidthType = WindowSizeType.DoNotChange;
                }
                else if (stringData == ApplicationData.Strings.WidthSpecification)
                {
                    HotkeyItemInformation.PositionSize.WidthType = WindowSizeType.Value;
                    HotkeyItemInformation.PositionSize.Width = WidthNumberBox.Value;
                    if ((string)((ComboBoxItem)WidthTypeComboBox.SelectedItem).Content == ApplicationData.Strings.Percent)
                    {
                        HotkeyItemInformation.PositionSize.WidthValueType = PositionSizeValueType.Percent;
                    }
                    else
                    {
                        HotkeyItemInformation.PositionSize.WidthValueType = PositionSizeValueType.Pixel;
                    }
                }
                stringData = (string)((ComboBoxItem)HeightComboBox.SelectedItem).Content;
                if (stringData == ApplicationData.Strings.DoNotChange)
                {
                    HotkeyItemInformation.PositionSize.HeightType = WindowSizeType.DoNotChange;
                }
                else if (stringData == ApplicationData.Strings.HeightSpecification)
                {
                    HotkeyItemInformation.PositionSize.HeightType = WindowSizeType.Value;
                    HotkeyItemInformation.PositionSize.Height = HeightNumberBox.Value;
                    if ((string)((ComboBoxItem)HeightTypeComboBox.SelectedItem).Content == ApplicationData.Strings.Percent)
                    {
                        HotkeyItemInformation.PositionSize.HeightValueType = PositionSizeValueType.Percent;
                    }
                    else
                    {
                        HotkeyItemInformation.PositionSize.HeightValueType = PositionSizeValueType.Pixel;
                    }
                }
                HotkeyItemInformation.PositionSize.ClientArea = ClientAreaToggleSwitch.IsOn;
                break;
            case HotkeyProcessingType.MoveX:
            case HotkeyProcessingType.MoveY:
                HotkeyItemInformation.ProcessingValue = double.IsNaN(AmountOfMovementNumberBox.Value) ? 0 : (int)AmountOfMovementNumberBox.Value;
                break;
            case HotkeyProcessingType.IncreaseDecreaseWidth:
            case HotkeyProcessingType.IncreaseDecreaseHeight:
            case HotkeyProcessingType.IncreaseDecreaseWidthHeight:
                HotkeyItemInformation.ProcessingValue = double.IsNaN(SizeChangeAmountNumberBox.Value) ? 0 : (int)SizeChangeAmountNumberBox.Value;
                break;
            case HotkeyProcessingType.SpecifyTransparencyOrCancel:
                HotkeyItemInformation.ProcessingValue = double.IsNaN(TransparencyNumberBox.Value) ? HotkeyItemInformation.MaximumTransparency : (int)TransparencyNumberBox.Value;
                break;
        }
    }

    /// <summary>
    /// 「HotkeyItemInformation」の値の確認
    /// </summary>
    /// <returns>値に問題ないかの値 (問題ない「null」/問題がある「理由の文字列」)</returns>
    private string? CheckValueHotkeyItemInformation()
    {
        int count = 0;      // カウント

        // 重複確認
        foreach (HotkeyItemInformation nowItem in ApplicationData.Settings.HotkeyInformation.Items)
        {
            if (nowItem.RegisteredName == HotkeyItemInformation.RegisteredName
                && (IndexOfItemToBeModified == -1 || IndexOfItemToBeModified != count))
            {
                return ApplicationData.Strings.ThereIsADuplicateRegistrationName;
            }
            count++;
        }

        return null;
    }
}
