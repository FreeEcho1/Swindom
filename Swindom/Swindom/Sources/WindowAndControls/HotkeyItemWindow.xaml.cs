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
    private readonly int IndexOfItemToBeModified = -1;
    /// <summary>
    /// 「ホットキー」機能の項目情報
    /// </summary>
    private readonly HotkeyItemInformation HotkeyItemInformation = new();

    /// <summary>
    /// コンストラクタ (使用しない)
    /// </summary>
    public HotkeyItemWindow()
    {
        throw new Exception("Do not use. - HotkeyItemWindow()");
    }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="ownerWindow">オーナーウィンドウ</param>
    /// <param name="indexOfItemToBeModified">修正する項目のインデックス (追加「-1」)</param>
    public HotkeyItemWindow(
        Window ownerWindow,
        int indexOfItemToBeModified = -1
        )
    {
        if (ApplicationData.Languages.LanguagesWindow == null)
        {
            throw new Exception("Languages value is null. - HotkeyItemWindow()");
        }

        InitializeComponent();

        IndexOfItemToBeModified = indexOfItemToBeModified;
        if (IndexOfItemToBeModified != -1)
        {
            HotkeyItemInformation.Copy(ApplicationData.Settings.HotkeyInformation.Items[IndexOfItemToBeModified]);
        }

        Owner = ownerWindow;
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
            Title = ApplicationData.Languages.LanguagesWindow.Add;
            AddModifyButton.Content = ApplicationData.Languages.LanguagesWindow.Add;
        }
        else
        {
            Title = ApplicationData.Languages.LanguagesWindow.Modify;
            AddModifyButton.Content = ApplicationData.Languages.LanguagesWindow.Modify;
        }
        RegisteredNameLabel.Content = ApplicationData.Languages.LanguagesWindow.RegisteredName;
        TypeOfProcessingLabel.Content = ApplicationData.Languages.LanguagesWindow.ProcessingType;
        ((ComboBoxItem)TypeOfProcessingComboBox.Items[0]).Content = ApplicationData.Languages.LanguagesWindow.SpecifyPositionAndSize;
        ((ComboBoxItem)TypeOfProcessingComboBox.Items[1]).Content = ApplicationData.Languages.LanguagesWindow.MoveXCoordinate;
        ((ComboBoxItem)TypeOfProcessingComboBox.Items[2]).Content = ApplicationData.Languages.LanguagesWindow.MoveYCoordinate;
        ((ComboBoxItem)TypeOfProcessingComboBox.Items[3]).Content = ApplicationData.Languages.LanguagesWindow.IncreaseDecreaseWidth;
        ((ComboBoxItem)TypeOfProcessingComboBox.Items[4]).Content = ApplicationData.Languages.LanguagesWindow.IncreaseDecreaseHeight;
        ((ComboBoxItem)TypeOfProcessingComboBox.Items[5]).Content = ApplicationData.Languages.LanguagesWindow.IncreaseDecreaseWidthAndHeight;
        ((ComboBoxItem)TypeOfProcessingComboBox.Items[6]).Content = ApplicationData.Languages.LanguagesWindow.AlwaysShowOrCancelOnTop;
        ((ComboBoxItem)TypeOfProcessingComboBox.Items[7]).Content = ApplicationData.Languages.LanguagesWindow.SpecifyCancelTransparency;
        ((ComboBoxItem)TypeOfProcessingComboBox.Items[8]).Content = ApplicationData.Languages.LanguagesWindow.StartStopProcessingOfSpecifyWindow;
        ((ComboBoxItem)TypeOfProcessingComboBox.Items[9]).Content = ApplicationData.Languages.LanguagesWindow.BatchProcessingOfSpecifyWindow;
        ((ComboBoxItem)TypeOfProcessingComboBox.Items[10]).Content = ApplicationData.Languages.LanguagesWindow.OnlyActiveWindowSpecifyWindow;
        ((ComboBoxItem)TypeOfProcessingComboBox.Items[11]).Content = ApplicationData.Languages.LanguagesWindow.StartStopProcessingOfAllWindow;
        ((ComboBoxItem)TypeOfProcessingComboBox.Items[12]).Content = ApplicationData.Languages.LanguagesWindow.StartStopProcessingOfMagnet;
        ((ComboBoxItem)TypeOfProcessingComboBox.Items[13]).Content = ApplicationData.Languages.LanguagesWindow.ShowThisApplicationWindow;
        StandardDisplayLabel.Content = ApplicationData.Languages.LanguagesWindow.DisplayToUseAsStandard;
        ((ComboBoxItem)StandardDisplayComboBox.Items[0]).Content = ApplicationData.Languages.LanguagesWindow.CurrentDisplay;
        ((ComboBoxItem)StandardDisplayComboBox.Items[1]).Content = ApplicationData.Languages.LanguagesWindow.SpecifiedDisplay;
        DisplayLabel.Content = ApplicationData.Languages.LanguagesWindow.Display;
        WindowStateLabel.Content = ApplicationData.Languages.LanguagesWindow.WindowState;
        ((ComboBoxItem)WindowStateComboBox.Items[0]).Content = ApplicationData.Languages.LanguagesWindow.DoNotChange;
        ((ComboBoxItem)WindowStateComboBox.Items[1]).Content = ApplicationData.Languages.LanguagesWindow.NormalWindow;
        ((ComboBoxItem)WindowStateComboBox.Items[2]).Content = ApplicationData.Languages.LanguagesWindow.Maximize;
        ((ComboBoxItem)WindowStateComboBox.Items[3]).Content = ApplicationData.Languages.LanguagesWindow.Minimize;
        XLabel.Content = ApplicationData.Languages.LanguagesWindow.X;
        ((ComboBoxItem)XComboBox.Items[0]).Content = ApplicationData.Languages.LanguagesWindow.DoNotChange;
        ((ComboBoxItem)XComboBox.Items[1]).Content = ApplicationData.Languages.LanguagesWindow.LeftEdge;
        ((ComboBoxItem)XComboBox.Items[2]).Content = ApplicationData.Languages.LanguagesWindow.Middle;
        ((ComboBoxItem)XComboBox.Items[3]).Content = ApplicationData.Languages.LanguagesWindow.RightEdge;
        ((ComboBoxItem)XComboBox.Items[4]).Content = ApplicationData.Languages.LanguagesWindow.CoordinateSpecification;
        ((ComboBoxItem)XTypeComboBox.Items[0]).Content = ApplicationData.Languages.LanguagesWindow.Pixel;
        ((ComboBoxItem)XTypeComboBox.Items[1]).Content = ApplicationData.Languages.LanguagesWindow.Percent;
        YLabel.Content = ApplicationData.Languages.LanguagesWindow.Y;
        ((ComboBoxItem)YComboBox.Items[0]).Content = ApplicationData.Languages.LanguagesWindow.DoNotChange;
        ((ComboBoxItem)YComboBox.Items[1]).Content = ApplicationData.Languages.LanguagesWindow.TopEdge;
        ((ComboBoxItem)YComboBox.Items[2]).Content = ApplicationData.Languages.LanguagesWindow.Middle;
        ((ComboBoxItem)YComboBox.Items[3]).Content = ApplicationData.Languages.LanguagesWindow.BottomEdge;
        ((ComboBoxItem)YComboBox.Items[4]).Content = ApplicationData.Languages.LanguagesWindow.CoordinateSpecification;
        ((ComboBoxItem)YTypeComboBox.Items[0]).Content = ApplicationData.Languages.LanguagesWindow.Pixel;
        ((ComboBoxItem)YTypeComboBox.Items[1]).Content = ApplicationData.Languages.LanguagesWindow.Percent;
        WidthLabel.Content = ApplicationData.Languages.LanguagesWindow.Width;
        ((ComboBoxItem)WidthComboBox.Items[0]).Content = ApplicationData.Languages.LanguagesWindow.DoNotChange;
        ((ComboBoxItem)WidthComboBox.Items[1]).Content = ApplicationData.Languages.LanguagesWindow.WidthSpecification;
        ((ComboBoxItem)WidthTypeComboBox.Items[0]).Content = ApplicationData.Languages.LanguagesWindow.Pixel;
        ((ComboBoxItem)WidthTypeComboBox.Items[1]).Content = ApplicationData.Languages.LanguagesWindow.Percent;
        HeightLabel.Content = ApplicationData.Languages.LanguagesWindow.Height;
        ((ComboBoxItem)HeightComboBox.Items[0]).Content = ApplicationData.Languages.LanguagesWindow.DoNotChange;
        ((ComboBoxItem)HeightComboBox.Items[1]).Content = ApplicationData.Languages.LanguagesWindow.HeightSpecification;
        ((ComboBoxItem)HeightTypeComboBox.Items[0]).Content = ApplicationData.Languages.LanguagesWindow.Pixel;
        ((ComboBoxItem)HeightTypeComboBox.Items[1]).Content = ApplicationData.Languages.LanguagesWindow.Percent;
        ClientAreaToggleSwitch.OffContent = ClientAreaToggleSwitch.OnContent = ApplicationData.Languages.LanguagesWindow.ClientArea;
        AmountOfMovementLabel.Content = ApplicationData.Languages.LanguagesWindow.AmountOfMovement;
        AmountOfMovementPixelLabel.Content = ApplicationData.Languages.LanguagesWindow.Pixel;
        SizeChangeAmountLabel.Content = ApplicationData.Languages.LanguagesWindow.SizeChangeAmount;
        SizeChangeAmountPixelLabel.Content = ApplicationData.Languages.LanguagesWindow.Pixel;
        TransparencyLabel.Content = ApplicationData.Languages.LanguagesWindow.Transparency;
        HotkeyLabel.Content = ApplicationData.Languages.LanguagesWindow.Hotkey;
        CancelButton.Content = ApplicationData.Languages.LanguagesWindow.Cancel;

        string stringData;
        RegisteredNameTextBox.Text = HotkeyItemInformation.RegisteredName;
        stringData = HotkeyItemInformation.ProcessingType switch
        {
            HotkeyProcessingType.PositionSize => ApplicationData.Languages.LanguagesWindow.SpecifyPositionAndSize,
            HotkeyProcessingType.MoveX => ApplicationData.Languages.LanguagesWindow.MoveXCoordinate,
            HotkeyProcessingType.MoveY => ApplicationData.Languages.LanguagesWindow.MoveYCoordinate,
            HotkeyProcessingType.IncreaseDecreaseWidth => ApplicationData.Languages.LanguagesWindow.IncreaseDecreaseWidth,
            HotkeyProcessingType.IncreaseDecreaseHeight => ApplicationData.Languages.LanguagesWindow.IncreaseDecreaseHeight,
            HotkeyProcessingType.IncreaseDecreaseWidthHeight => ApplicationData.Languages.LanguagesWindow.IncreaseDecreaseWidthAndHeight,
            HotkeyProcessingType.AlwaysForefrontOrCancel => ApplicationData.Languages.LanguagesWindow.AlwaysShowOrCancelOnTop,
            HotkeyProcessingType.SpecifyTransparencyOrCancel => ApplicationData.Languages.LanguagesWindow.SpecifyCancelTransparency,
            HotkeyProcessingType.StartStopSpecifyWindow => ApplicationData.Languages.LanguagesWindow.StartStopProcessingOfSpecifyWindow,
            HotkeyProcessingType.BatchSpecifyWindow => ApplicationData.Languages.LanguagesWindow.BatchProcessingOfSpecifyWindow,
            HotkeyProcessingType.OnlyActiveWindowSpecifyWindow => ApplicationData.Languages.LanguagesWindow.OnlyActiveWindowSpecifyWindow,
            HotkeyProcessingType.StartStopAllWindow => ApplicationData.Languages.LanguagesWindow.StartStopProcessingOfAllWindow,
            HotkeyProcessingType.StartStopMagnet => ApplicationData.Languages.LanguagesWindow.StartStopProcessingOfMagnet,
            HotkeyProcessingType.ShowThisApplicationWindow => ApplicationData.Languages.LanguagesWindow.ShowThisApplicationWindow,
            _ => ApplicationData.Languages.LanguagesWindow.SpecifyPositionAndSize
        };
        VariousProcessing.SelectComboBoxItem(TypeOfProcessingComboBox, stringData);
        switch (HotkeyItemInformation.StandardDisplay)
        {
            case StandardDisplay.SpecifiedDisplay:
                stringData = ApplicationData.Languages.LanguagesWindow.SpecifiedDisplay;
                break;
            default:
                stringData = ApplicationData.Languages.LanguagesWindow.CurrentDisplay;
                DisplayLabel.IsEnabled = false;
                DisplayComboBox.IsEnabled = false;
                break;
        }
        VariousProcessing.SelectComboBoxItem(StandardDisplayComboBox, stringData);
        VariousProcessing.SelectComboBoxItem(DisplayComboBox, HotkeyItemInformation.PositionSize.Display);
        if (DisplayComboBox.SelectedIndex == -1)
        {
            DisplayComboBox.SelectedIndex = 0;
        }
        stringData = HotkeyItemInformation.PositionSize.SettingsWindowState switch
        {
            SettingsWindowState.Normal => ApplicationData.Languages.LanguagesWindow.NormalWindow,
            SettingsWindowState.Maximize => ApplicationData.Languages.LanguagesWindow.Maximize,
            SettingsWindowState.Minimize => ApplicationData.Languages.LanguagesWindow.Minimize,
            _ => ApplicationData.Languages.LanguagesWindow.DoNotChange
        };
        VariousProcessing.SelectComboBoxItem(WindowStateComboBox, stringData);
        stringData = HotkeyItemInformation.PositionSize.XType switch
        {
            WindowXType.DoNotChange => ApplicationData.Languages.LanguagesWindow.DoNotChange,
            WindowXType.Left => ApplicationData.Languages.LanguagesWindow.LeftEdge,
            WindowXType.Middle => ApplicationData.Languages.LanguagesWindow.Middle,
            WindowXType.Right => ApplicationData.Languages.LanguagesWindow.RightEdge,
            WindowXType.Value => ApplicationData.Languages.LanguagesWindow.CoordinateSpecification,
            _ => ApplicationData.Languages.LanguagesWindow.DoNotChange,
        };
        VariousProcessing.SelectComboBoxItem(XComboBox, stringData);
        stringData = HotkeyItemInformation.PositionSize.XValueType switch
        {
            PositionSizeValueType.Percent => ApplicationData.Languages.LanguagesWindow.Percent,
            _ => ApplicationData.Languages.LanguagesWindow.Pixel
        };
        VariousProcessing.SelectComboBoxItem(XTypeComboBox, stringData);
        XNumberBox.Value = HotkeyItemInformation.PositionSize.X;
        stringData = HotkeyItemInformation.PositionSize.YType switch
        {
            WindowYType.DoNotChange => ApplicationData.Languages.LanguagesWindow.DoNotChange,
            WindowYType.Top => ApplicationData.Languages.LanguagesWindow.TopEdge,
            WindowYType.Middle => ApplicationData.Languages.LanguagesWindow.Middle,
            WindowYType.Bottom => ApplicationData.Languages.LanguagesWindow.BottomEdge,
            WindowYType.Value => ApplicationData.Languages.LanguagesWindow.CoordinateSpecification,
            _ => ApplicationData.Languages.LanguagesWindow.DoNotChange,
        };
        VariousProcessing.SelectComboBoxItem(YComboBox, stringData);
        stringData = HotkeyItemInformation.PositionSize.YValueType switch
        {
            PositionSizeValueType.Percent => ApplicationData.Languages.LanguagesWindow.Percent,
            _ => ApplicationData.Languages.LanguagesWindow.Pixel
        };
        VariousProcessing.SelectComboBoxItem(YTypeComboBox, stringData);
        YNumberBox.Value = HotkeyItemInformation.PositionSize.Y;
        stringData = HotkeyItemInformation.PositionSize.WidthType switch
        {
            WindowSizeType.DoNotChange => ApplicationData.Languages.LanguagesWindow.DoNotChange,
            WindowSizeType.Value => ApplicationData.Languages.LanguagesWindow.WidthSpecification,
            _ => ApplicationData.Languages.LanguagesWindow.DoNotChange,
        };
        VariousProcessing.SelectComboBoxItem(WidthComboBox, stringData);
        stringData = HotkeyItemInformation.PositionSize.WidthValueType switch
        {
            PositionSizeValueType.Percent => ApplicationData.Languages.LanguagesWindow.Percent,
            _ => ApplicationData.Languages.LanguagesWindow.Pixel
        };
        VariousProcessing.SelectComboBoxItem(WidthTypeComboBox, stringData);
        WidthNumberBox.Value = HotkeyItemInformation.PositionSize.Width;
        stringData = HotkeyItemInformation.PositionSize.HeightType switch
        {
            WindowSizeType.DoNotChange => ApplicationData.Languages.LanguagesWindow.DoNotChange,
            WindowSizeType.Value => ApplicationData.Languages.LanguagesWindow.HeightSpecification,
            _ => ApplicationData.Languages.LanguagesWindow.DoNotChange,
        };
        VariousProcessing.SelectComboBoxItem(HeightComboBox, stringData);
        stringData = HotkeyItemInformation.PositionSize.HeightValueType switch
        {
            PositionSizeValueType.Percent => ApplicationData.Languages.LanguagesWindow.Percent,
            _ => ApplicationData.Languages.LanguagesWindow.Pixel
        };
        VariousProcessing.SelectComboBoxItem(HeightTypeComboBox, stringData);
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
        SettingsControlEnabled();
        SettingsAddModifyButtonEnabled();

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
            SettingsAddModifyButtonEnabled();
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
            SettingsControlEnabled();
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
            if ((string)((ComboBoxItem)StandardDisplayComboBox.SelectedItem).Content == ApplicationData.Languages.LanguagesWindow?.CurrentDisplay)
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
            SettingsControlEnabled();
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
            SettingsXControlsEnabled();
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
            if ((string)((ComboBoxItem)XTypeComboBox.SelectedItem).Content == ApplicationData.Languages.LanguagesWindow?.Percent)
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
            SettingsYControlsEnabled();
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
            if ((string)((ComboBoxItem)YTypeComboBox.SelectedItem).Content == ApplicationData.Languages.LanguagesWindow?.Percent)
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
            SettingsWidthControlsEnabled();
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
            if ((string)((ComboBoxItem)WidthTypeComboBox.SelectedItem).Content == ApplicationData.Languages.LanguagesWindow?.Percent)
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
            SettingsHeightControlsEnabled();
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
            if ((string)((ComboBoxItem)HeightTypeComboBox.SelectedItem).Content == ApplicationData.Languages.LanguagesWindow?.Percent)
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

            if (hotkeyString != Common.TabString)
            {
                HotkeyItemInformation.Hotkey.Copy(hotkeyInformation);
                HotkeyTextBox.Text = hotkeyString;
                e.Handled = true;
            }
        }
        catch
        {
            FEMessageBox.Show(ApplicationData.Languages.ErrorOccurred, ApplicationData.Languages.Check, MessageBoxButton.OK);
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
            if (resultString == null)
            {
                ApplicationData.WindowProcessingManagement.HotkeyProcessing?.UnregisterHotkeys();
                try
                {
                    if (IndexOfItemToBeModified == -1)
                    {
                        ApplicationData.Settings.HotkeyInformation.Items.Add(HotkeyItemInformation);
                        FEMessageBox.Show(ApplicationData.Languages.LanguagesWindow?.Added ?? "", ApplicationData.Languages.Check, MessageBoxButton.OK);
                    }
                    else
                    {
                        ApplicationData.Settings.HotkeyInformation.Items[IndexOfItemToBeModified] = HotkeyItemInformation;
                        FEMessageBox.Show(ApplicationData.Languages.LanguagesWindow?.Modified ?? "", ApplicationData.Languages.Check, MessageBoxButton.OK);
                    }
                    AddedModified = true;
                }
                catch
                {
                    FEMessageBox.Show(ApplicationData.Languages.ErrorOccurred, ApplicationData.Languages.Check, MessageBoxButton.OK);
                }
                ApplicationData.WindowProcessingManagement.HotkeyProcessing?.RegisterHotkeys();
                Close();
            }
            else
            {
                FEMessageBox.Show(resultString, ApplicationData.Languages.Check, MessageBoxButton.OK);
            }
        }
        catch
        {
            FEMessageBox.Show(ApplicationData.Languages.ErrorOccurred, ApplicationData.Languages.Check, MessageBoxButton.OK);
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
    /// 「X」関係のコントロールの有効状態を設定
    /// </summary>
    private void SettingsXControlsEnabled()
    {
        if ((string)((ComboBoxItem)XComboBox.SelectedItem).Content == ApplicationData.Languages.LanguagesWindow?.CoordinateSpecification)
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
    /// 「Y」関係のコントロールの有効状態を設定
    /// </summary>
    private void SettingsYControlsEnabled()
    {
        if ((string)((ComboBoxItem)YComboBox.SelectedItem).Content == ApplicationData.Languages.LanguagesWindow?.CoordinateSpecification)
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
    /// 「幅」関係のコントロールの有効状態を設定
    /// </summary>
    private void SettingsWidthControlsEnabled()
    {
        if ((string)((ComboBoxItem)WidthComboBox.SelectedItem).Content == ApplicationData.Languages.LanguagesWindow?.WidthSpecification)
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
    /// 「高さ」関係のコントロールの有効状態を設定
    /// </summary>
    private void SettingsHeightControlsEnabled()
    {
        if ((string)((ComboBoxItem)HeightComboBox.SelectedItem).Content == ApplicationData.Languages.LanguagesWindow?.HeightSpecification)
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
    /// コントロールの有効状態を設定
    /// </summary>
    private void SettingsControlEnabled()
    {
        string selectedItemString = (string)((ComboBoxItem)TypeOfProcessingComboBox.SelectedItem).Content;
        if (selectedItemString == ApplicationData.Languages.LanguagesWindow?.SpecifyPositionAndSize)
        {
            StandardDisplayLabel.IsEnabled = true;
            StandardDisplayComboBox.IsEnabled = true;
            DisplayLabel.IsEnabled = true;
            DisplayComboBox.IsEnabled = true;
            WindowStateLabel.IsEnabled = true;
            WindowStateComboBox.IsEnabled = true;
            string stringData = (string)((ComboBoxItem)WindowStateComboBox.SelectedItem).Content;
            if (stringData == ApplicationData.Languages.LanguagesWindow?.NormalWindow)
            {
                XLabel.IsEnabled = true;
                XGrid.IsEnabled = true;
                SettingsXControlsEnabled();
                YLabel.IsEnabled = true;
                YGrid.IsEnabled = true;
                SettingsYControlsEnabled();
                WidthLabel.IsEnabled = true;
                WidthGrid.IsEnabled = true;
                SettingsWidthControlsEnabled();
                HeightLabel.IsEnabled = true;
                HeightGrid.IsEnabled = true;
                SettingsHeightControlsEnabled();
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

        if ((selectedItemString == ApplicationData.Languages.LanguagesWindow?.MoveXCoordinate)
            || (selectedItemString == ApplicationData.Languages.LanguagesWindow?.MoveYCoordinate))
        {
            AmountOfMovementGrid.IsEnabled = true;
            SizeChangeAmountGrid.IsEnabled = false;
            TransparencyGrid.IsEnabled = false;
        }
        else if ((selectedItemString == ApplicationData.Languages.LanguagesWindow?.IncreaseDecreaseWidth)
            || (selectedItemString == ApplicationData.Languages.LanguagesWindow?.IncreaseDecreaseHeight)
            || (selectedItemString == ApplicationData.Languages.LanguagesWindow?.IncreaseDecreaseWidthAndHeight))
        {
            AmountOfMovementGrid.IsEnabled = false;
            SizeChangeAmountGrid.IsEnabled = true;
            TransparencyGrid.IsEnabled = false;
        }
        else if (selectedItemString == ApplicationData.Languages.LanguagesWindow?.SpecifyCancelTransparency)
        {
            AmountOfMovementGrid.IsEnabled = false;
            SizeChangeAmountGrid.IsEnabled = false;
            TransparencyGrid.IsEnabled = true;
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
    private void SettingsAddModifyButtonEnabled()
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
        if (stringData == ApplicationData.Languages.LanguagesWindow?.SpecifyPositionAndSize)
        {
            HotkeyItemInformation.ProcessingType = HotkeyProcessingType.PositionSize;
        }
        else if (stringData == ApplicationData.Languages.LanguagesWindow?.MoveXCoordinate)
        {
            HotkeyItemInformation.ProcessingType = HotkeyProcessingType.MoveX;
        }
        else if (stringData == ApplicationData.Languages.LanguagesWindow?.MoveYCoordinate)
        {
            HotkeyItemInformation.ProcessingType = HotkeyProcessingType.MoveY;
        }
        else if (stringData == ApplicationData.Languages.LanguagesWindow?.IncreaseDecreaseWidth)
        {
            HotkeyItemInformation.ProcessingType = HotkeyProcessingType.IncreaseDecreaseWidth;
        }
        else if (stringData == ApplicationData.Languages.LanguagesWindow?.IncreaseDecreaseHeight)
        {
            HotkeyItemInformation.ProcessingType = HotkeyProcessingType.IncreaseDecreaseHeight;
        }
        else if (stringData == ApplicationData.Languages.LanguagesWindow?.IncreaseDecreaseWidthAndHeight)
        {
            HotkeyItemInformation.ProcessingType = HotkeyProcessingType.IncreaseDecreaseWidthHeight;
        }
        else if (stringData == ApplicationData.Languages.LanguagesWindow?.AlwaysShowOrCancelOnTop)
        {
            HotkeyItemInformation.ProcessingType = HotkeyProcessingType.AlwaysForefrontOrCancel;
        }
        else if (stringData == ApplicationData.Languages.LanguagesWindow?.SpecifyCancelTransparency)
        {
            HotkeyItemInformation.ProcessingType = HotkeyProcessingType.SpecifyTransparencyOrCancel;
        }
        else if (stringData == ApplicationData.Languages.LanguagesWindow?.StartStopProcessingOfSpecifyWindow)
        {
            HotkeyItemInformation.ProcessingType = HotkeyProcessingType.StartStopSpecifyWindow;
        }
        else if (stringData == ApplicationData.Languages.LanguagesWindow?.BatchProcessingOfSpecifyWindow)
        {
            HotkeyItemInformation.ProcessingType = HotkeyProcessingType.BatchSpecifyWindow;
        }
        else if (stringData == ApplicationData.Languages.LanguagesWindow?.OnlyActiveWindowSpecifyWindow)
        {
            HotkeyItemInformation.ProcessingType = HotkeyProcessingType.OnlyActiveWindowSpecifyWindow;
        }
        else if (stringData == ApplicationData.Languages.LanguagesWindow?.StartStopProcessingOfAllWindow)
        {
            HotkeyItemInformation.ProcessingType = HotkeyProcessingType.StartStopAllWindow;
        }
        else if (stringData == ApplicationData.Languages.LanguagesWindow?.StartStopProcessingOfMagnet)
        {
            HotkeyItemInformation.ProcessingType = HotkeyProcessingType.StartStopMagnet;
        }
        else if (stringData == ApplicationData.Languages.LanguagesWindow?.ShowThisApplicationWindow)
        {
            HotkeyItemInformation.ProcessingType = HotkeyProcessingType.ShowThisApplicationWindow;
        }
        switch (HotkeyItemInformation.ProcessingType)
        {
            case HotkeyProcessingType.PositionSize:
                stringData = (string)((ComboBoxItem)StandardDisplayComboBox.SelectedItem).Content;
                if (stringData == ApplicationData.Languages.LanguagesWindow?.SpecifiedDisplay)
                {
                    HotkeyItemInformation.StandardDisplay = StandardDisplay.SpecifiedDisplay;
                }
                else if (stringData == ApplicationData.Languages.LanguagesWindow?.CurrentDisplay)
                {
                    HotkeyItemInformation.StandardDisplay = StandardDisplay.CurrentDisplay;
                }
                HotkeyItemInformation.PositionSize.Display = (string)((ComboBoxItem)DisplayComboBox.SelectedItem).Content;
                stringData = (string)((ComboBoxItem)WindowStateComboBox.SelectedItem).Content;
                if (stringData == ApplicationData.Languages.LanguagesWindow?.NormalWindow)
                {
                    HotkeyItemInformation.PositionSize.SettingsWindowState = SettingsWindowState.Normal;
                }
                else if (stringData == ApplicationData.Languages.LanguagesWindow?.Maximize)
                {
                    HotkeyItemInformation.PositionSize.SettingsWindowState = SettingsWindowState.Maximize;
                }
                else if (stringData == ApplicationData.Languages.LanguagesWindow?.Minimize)
                {
                    HotkeyItemInformation.PositionSize.SettingsWindowState = SettingsWindowState.Minimize;
                }
                else
                {
                    HotkeyItemInformation.PositionSize.SettingsWindowState = SettingsWindowState.DoNotChange;
                }
                stringData = (string)((ComboBoxItem)XComboBox.SelectedItem).Content;
                if (stringData == ApplicationData.Languages.LanguagesWindow?.DoNotChange)
                {
                    HotkeyItemInformation.PositionSize.XType = WindowXType.DoNotChange;
                }
                else if (stringData == ApplicationData.Languages.LanguagesWindow?.LeftEdge)
                {
                    HotkeyItemInformation.PositionSize.XType = WindowXType.Left;
                }
                else if (stringData == ApplicationData.Languages.LanguagesWindow?.Middle)
                {
                    HotkeyItemInformation.PositionSize.XType = WindowXType.Middle;
                }
                else if (stringData == ApplicationData.Languages.LanguagesWindow?.RightEdge)
                {
                    HotkeyItemInformation.PositionSize.XType = WindowXType.Right;
                }
                else if (stringData == ApplicationData.Languages.LanguagesWindow?.CoordinateSpecification)
                {
                    HotkeyItemInformation.PositionSize.XType = WindowXType.Value;
                    HotkeyItemInformation.PositionSize.X = XNumberBox.Value;
                    if ((string)((ComboBoxItem)XTypeComboBox.SelectedItem).Content == ApplicationData.Languages.LanguagesWindow.Percent)
                    {
                        HotkeyItemInformation.PositionSize.XValueType = PositionSizeValueType.Percent;
                    }
                    else
                    {
                        HotkeyItemInformation.PositionSize.XValueType = PositionSizeValueType.Pixel;
                    }
                }
                stringData = (string)((ComboBoxItem)YComboBox.SelectedItem).Content;
                if (stringData == ApplicationData.Languages.LanguagesWindow?.DoNotChange)
                {
                    HotkeyItemInformation.PositionSize.YType = WindowYType.DoNotChange;
                }
                else if (stringData == ApplicationData.Languages.LanguagesWindow?.TopEdge)
                {
                    HotkeyItemInformation.PositionSize.YType = WindowYType.Top;
                }
                else if (stringData == ApplicationData.Languages.LanguagesWindow?.Middle)
                {
                    HotkeyItemInformation.PositionSize.YType = WindowYType.Middle;
                }
                else if (stringData == ApplicationData.Languages.LanguagesWindow?.BottomEdge)
                {
                    HotkeyItemInformation.PositionSize.YType = WindowYType.Bottom;
                }
                else if (stringData == ApplicationData.Languages.LanguagesWindow?.CoordinateSpecification)
                {
                    HotkeyItemInformation.PositionSize.YType = WindowYType.Value;
                    HotkeyItemInformation.PositionSize.Y = YNumberBox.Value;
                    if ((string)((ComboBoxItem)YTypeComboBox.SelectedItem).Content == ApplicationData.Languages.LanguagesWindow.Percent)
                    {
                        HotkeyItemInformation.PositionSize.YValueType = PositionSizeValueType.Percent;
                    }
                    else
                    {
                        HotkeyItemInformation.PositionSize.YValueType = PositionSizeValueType.Pixel;
                    }
                }
                stringData = (string)((ComboBoxItem)WidthComboBox.SelectedItem).Content;
                if (stringData == ApplicationData.Languages.LanguagesWindow?.DoNotChange)
                {
                    HotkeyItemInformation.PositionSize.WidthType = WindowSizeType.DoNotChange;
                }
                else if (stringData == ApplicationData.Languages.LanguagesWindow?.WidthSpecification)
                {
                    HotkeyItemInformation.PositionSize.WidthType = WindowSizeType.Value;
                    HotkeyItemInformation.PositionSize.Width = WidthNumberBox.Value;
                    if ((string)((ComboBoxItem)WidthTypeComboBox.SelectedItem).Content == ApplicationData.Languages.LanguagesWindow.Percent)
                    {
                        HotkeyItemInformation.PositionSize.WidthValueType = PositionSizeValueType.Percent;
                    }
                    else
                    {
                        HotkeyItemInformation.PositionSize.WidthValueType = PositionSizeValueType.Pixel;
                    }
                }
                stringData = (string)((ComboBoxItem)HeightComboBox.SelectedItem).Content;
                if (stringData == ApplicationData.Languages.LanguagesWindow?.DoNotChange)
                {
                    HotkeyItemInformation.PositionSize.HeightType = WindowSizeType.DoNotChange;
                }
                else if (stringData == ApplicationData.Languages.LanguagesWindow?.HeightSpecification)
                {
                    HotkeyItemInformation.PositionSize.HeightType = WindowSizeType.Value;
                    HotkeyItemInformation.PositionSize.Height = HeightNumberBox.Value;
                    if ((string)((ComboBoxItem)HeightTypeComboBox.SelectedItem).Content == ApplicationData.Languages.LanguagesWindow.Percent)
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
                HotkeyItemInformation.ProcessingValue = (int)AmountOfMovementNumberBox.Value;
                break;
            case HotkeyProcessingType.IncreaseDecreaseWidth:
            case HotkeyProcessingType.IncreaseDecreaseHeight:
            case HotkeyProcessingType.IncreaseDecreaseWidthHeight:
                HotkeyItemInformation.ProcessingValue = (int)SizeChangeAmountNumberBox.Value;
                break;
            case HotkeyProcessingType.SpecifyTransparencyOrCancel:
                HotkeyItemInformation.ProcessingValue = (int)TransparencyNumberBox.Value;
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
                return ApplicationData.Languages.LanguagesWindow?.ThereIsADuplicateRegistrationName;
            }
            count++;
        }

        return null;
    }
}
