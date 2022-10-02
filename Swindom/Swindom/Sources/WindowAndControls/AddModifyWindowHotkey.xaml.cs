namespace Swindom;

/// <summary>
/// 「ホットキー」の「追加/修正」ウィンドウ
/// </summary>
public partial class AddModifyWindowHotkey : Window
{
    /// <summary>
    /// 修正する項目のインデックス (追加「-1」)
    /// </summary>
    private readonly int IndexOfItemToBeModified = -1;
    /// <summary>
    /// 追加/修正したかの値 (いいえ「false」/はい「true」)
    /// </summary>
    public bool AddedModified { get; private set; }
    /// <summary>
    /// 「ホットキー」機能の項目情報
    /// </summary>
    private readonly HotkeyItemInformation HotkeyItemInformation = new();
    /// <summary>
    /// ディスプレイが1つの場合のウィンドウの最大の高さ
    /// </summary>
    private const int OneDisplayWindowMaxHeight = 678;

    /// <summary>
    /// コンストラクタ (使用しない)
    /// </summary>
    public AddModifyWindowHotkey()
    {
        throw new Exception("Do not use. - AddModifyWindowHotkey()");
    }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="ownerWindow">オーナーウィンドウ</param>
    /// <param name="indexOfItemToBeModified">修正する項目のインデックス (追加「-1」)</param>
    public AddModifyWindowHotkey(
        Window ownerWindow,
        int indexOfItemToBeModified = -1
        )
    {
        if (Common.ApplicationData.Languages.LanguagesWindow == null)
        {
            throw new Exception("LanguagesWindow value is null. - AddModifyWindowHotkey()");
        }

        InitializeComponent();

        IndexOfItemToBeModified = indexOfItemToBeModified;
        if (IndexOfItemToBeModified != -1)
        {
            HotkeyItemInformation.Copy(Common.ApplicationData.Settings.HotkeyInformation.Items[IndexOfItemToBeModified]);
        }

        Owner = ownerWindow;
        SizeToContent = SizeToContent.Manual;
        if (Common.MonitorInformation.MonitorInfo.Count == 1)
        {
            MaxHeight = OneDisplayWindowMaxHeight;
        }
        Width = (Common.ApplicationData.Settings.HotkeyInformation.AddModifyWindowSize.Width < MinWidth) ? MinWidth : Common.ApplicationData.Settings.HotkeyInformation.AddModifyWindowSize.Width;
        Height = (Common.ApplicationData.Settings.HotkeyInformation.AddModifyWindowSize.Height < MinHeight) ? MinHeight : Common.ApplicationData.Settings.HotkeyInformation.AddModifyWindowSize.Height;
        if ((Common.ApplicationData.Settings.CoordinateType == CoordinateType.Global) || (Common.MonitorInformation.MonitorInfo.Count == 1))
        {
            DisplayGroupBox.Visibility = Visibility.Collapsed;
        }
        foreach (MonitorInfoEx nowMonitorInfo in Common.MonitorInformation.MonitorInfo)
        {
            ComboBoxItem newItem = new()
            {
                Content = nowMonitorInfo.DeviceName
            };
            DisplayComboBox.Items.Add(newItem);
        }

        if (IndexOfItemToBeModified == -1)
        {
            Title = Common.ApplicationData.Languages.LanguagesWindow.Add;
            AddModifyButton.Content = Common.ApplicationData.Languages.LanguagesWindow.Add;
        }
        else
        {
            Title = Common.ApplicationData.Languages.LanguagesWindow.Modify;
            AddModifyButton.Content = Common.ApplicationData.Languages.LanguagesWindow.Modify;
        }
        RegisteredNameLabel.Content = Common.ApplicationData.Languages.LanguagesWindow.RegisteredName;
        ProcessingTypeLabel.Content = Common.ApplicationData.Languages.LanguagesWindow.ProcessingType;
        ((ComboBoxItem)ProcessingTypeComboBox.Items[0]).Content = Common.ApplicationData.Languages.LanguagesWindow.SpecifyPositionAndSize;
        ((ComboBoxItem)ProcessingTypeComboBox.Items[1]).Content = Common.ApplicationData.Languages.LanguagesWindow.MoveXCoordinate;
        ((ComboBoxItem)ProcessingTypeComboBox.Items[2]).Content = Common.ApplicationData.Languages.LanguagesWindow.MoveYCoordinate;
        ((ComboBoxItem)ProcessingTypeComboBox.Items[3]).Content = Common.ApplicationData.Languages.LanguagesWindow.IncreaseDecreaseWidth;
        ((ComboBoxItem)ProcessingTypeComboBox.Items[4]).Content = Common.ApplicationData.Languages.LanguagesWindow.IncreaseDecreaseHeight;
        ((ComboBoxItem)ProcessingTypeComboBox.Items[5]).Content = Common.ApplicationData.Languages.LanguagesWindow.IncreaseDecreaseWidthAndHeight;
        ((ComboBoxItem)ProcessingTypeComboBox.Items[6]).Content = Common.ApplicationData.Languages.LanguagesWindow.AlwaysShowOrCancelOnTop;
        ((ComboBoxItem)ProcessingTypeComboBox.Items[7]).Content = Common.ApplicationData.Languages.LanguagesWindow.SpecifyCancelTransparency;
        ((ComboBoxItem)ProcessingTypeComboBox.Items[8]).Content = Common.ApplicationData.Languages.LanguagesWindow.StartStopProcessingOfEvent;
        ((ComboBoxItem)ProcessingTypeComboBox.Items[9]).Content = Common.ApplicationData.Languages.LanguagesWindow.BatchProcessingOfEvent;
        ((ComboBoxItem)ProcessingTypeComboBox.Items[10]).Content = Common.ApplicationData.Languages.LanguagesWindow.OnlyActiveWindowEvent;
        ((ComboBoxItem)ProcessingTypeComboBox.Items[11]).Content = Common.ApplicationData.Languages.LanguagesWindow.StartStopProcessingOfTimer;
        ((ComboBoxItem)ProcessingTypeComboBox.Items[12]).Content = Common.ApplicationData.Languages.LanguagesWindow.BatchProcessingOfTimer;
        ((ComboBoxItem)ProcessingTypeComboBox.Items[13]).Content = Common.ApplicationData.Languages.LanguagesWindow.OnlyActiveWindowTimer;
        ((ComboBoxItem)ProcessingTypeComboBox.Items[14]).Content = Common.ApplicationData.Languages.LanguagesWindow.StartStopProcessingOfMagnet;
        DisplayGroupBox.Header = Common.ApplicationData.Languages.LanguagesWindow.Display;
        StandardDisplayLabel.Content = Common.ApplicationData.Languages.LanguagesWindow.DisplayToUseAsStandard;
        ((ComboBoxItem)StandardDisplayComboBox.Items[0]).Content = Common.ApplicationData.Languages.LanguagesWindow.DisplayWithWindow;
        ((ComboBoxItem)StandardDisplayComboBox.Items[1]).Content = Common.ApplicationData.Languages.LanguagesWindow.TheSpecifiedDisplay;
        DisplayLabel.Content = Common.ApplicationData.Languages.LanguagesWindow.Display;
        PositionSizeGroupBox.Header = Common.ApplicationData.Languages.LanguagesWindow.PositionAndSize;
        XLabel.Content = Common.ApplicationData.Languages.LanguagesWindow.X;
        ((ComboBoxItem)XComboBox.Items[0]).Content = Common.ApplicationData.Languages.LanguagesWindow.DoNotChange;
        ((ComboBoxItem)XComboBox.Items[1]).Content = Common.ApplicationData.Languages.LanguagesWindow.LeftEdge;
        ((ComboBoxItem)XComboBox.Items[2]).Content = Common.ApplicationData.Languages.LanguagesWindow.Middle;
        ((ComboBoxItem)XComboBox.Items[3]).Content = Common.ApplicationData.Languages.LanguagesWindow.RightEdge;
        ((ComboBoxItem)XComboBox.Items[4]).Content = Common.ApplicationData.Languages.LanguagesWindow.CoordinateSpecification;
        ((ComboBoxItem)XTypeComboBox.Items[0]).Content = Common.ApplicationData.Languages.LanguagesWindow.Pixel;
        ((ComboBoxItem)XTypeComboBox.Items[1]).Content = Common.ApplicationData.Languages.LanguagesWindow.Percent;
        YLabel.Content = Common.ApplicationData.Languages.LanguagesWindow.Y;
        ((ComboBoxItem)YComboBox.Items[0]).Content = Common.ApplicationData.Languages.LanguagesWindow.DoNotChange;
        ((ComboBoxItem)YComboBox.Items[1]).Content = Common.ApplicationData.Languages.LanguagesWindow.TopEdge;
        ((ComboBoxItem)YComboBox.Items[2]).Content = Common.ApplicationData.Languages.LanguagesWindow.Middle;
        ((ComboBoxItem)YComboBox.Items[3]).Content = Common.ApplicationData.Languages.LanguagesWindow.BottomEdge;
        ((ComboBoxItem)YComboBox.Items[4]).Content = Common.ApplicationData.Languages.LanguagesWindow.CoordinateSpecification;
        ((ComboBoxItem)YTypeComboBox.Items[0]).Content = Common.ApplicationData.Languages.LanguagesWindow.Pixel;
        ((ComboBoxItem)YTypeComboBox.Items[1]).Content = Common.ApplicationData.Languages.LanguagesWindow.Percent;
        WidthLabel.Content = Common.ApplicationData.Languages.LanguagesWindow.Width;
        ((ComboBoxItem)WidthComboBox.Items[0]).Content = Common.ApplicationData.Languages.LanguagesWindow.DoNotChange;
        ((ComboBoxItem)WidthComboBox.Items[1]).Content = Common.ApplicationData.Languages.LanguagesWindow.WidthSpecification;
        ((ComboBoxItem)WidthTypeComboBox.Items[0]).Content = Common.ApplicationData.Languages.LanguagesWindow.Pixel;
        ((ComboBoxItem)WidthTypeComboBox.Items[1]).Content = Common.ApplicationData.Languages.LanguagesWindow.Percent;
        HeightLabel.Content = Common.ApplicationData.Languages.LanguagesWindow.Height;
        ((ComboBoxItem)HeightComboBox.Items[0]).Content = Common.ApplicationData.Languages.LanguagesWindow.DoNotChange;
        ((ComboBoxItem)HeightComboBox.Items[1]).Content = Common.ApplicationData.Languages.LanguagesWindow.HeightSpecification;
        ((ComboBoxItem)HeightTypeComboBox.Items[0]).Content = Common.ApplicationData.Languages.LanguagesWindow.Pixel;
        ((ComboBoxItem)HeightTypeComboBox.Items[1]).Content = Common.ApplicationData.Languages.LanguagesWindow.Percent;
        ClientAreaToggleSwitch.Text = Common.ApplicationData.Languages.LanguagesWindow.ClientArea;
        ProcessingPositionAndSizeTwiceToggleSwitch.Text = Common.ApplicationData.Languages.LanguagesWindow.ProcessingPositionAndSizeTwice;
        AmountOfMovementLabel.Content = Common.ApplicationData.Languages.LanguagesWindow.AmountOfMovement;
        AmountOfMovementPixelLabel.Content = Common.ApplicationData.Languages.LanguagesWindow.Pixel;
        SizeChangeAmountLabel.Content = Common.ApplicationData.Languages.LanguagesWindow.SizeChangeAmount;
        SizeChangeAmountPixelLabel.Content = Common.ApplicationData.Languages.LanguagesWindow.Pixel;
        TransparencyLabel.Content = Common.ApplicationData.Languages.LanguagesWindow.Transparency;
        HotkeyLabel.Content = Common.ApplicationData.Languages.LanguagesWindow.Hotkey;
        CancelButton.Content = Common.ApplicationData.Languages.LanguagesWindow.Cancel;

        string stringData;
        RegisteredNameTextBox.Text = HotkeyItemInformation.RegisteredName;
        stringData = HotkeyItemInformation.ProcessingType switch
        {
            HotkeyProcessingType.PositionSize => Common.ApplicationData.Languages.LanguagesWindow.SpecifyPositionAndSize,
            HotkeyProcessingType.MoveX => Common.ApplicationData.Languages.LanguagesWindow.MoveXCoordinate,
            HotkeyProcessingType.MoveY => Common.ApplicationData.Languages.LanguagesWindow.MoveYCoordinate,
            HotkeyProcessingType.IncreaseDecreaseWidth => Common.ApplicationData.Languages.LanguagesWindow.IncreaseDecreaseWidth,
            HotkeyProcessingType.IncreaseDecreaseHeight => Common.ApplicationData.Languages.LanguagesWindow.IncreaseDecreaseHeight,
            HotkeyProcessingType.IncreaseDecreaseWidthHeight => Common.ApplicationData.Languages.LanguagesWindow.IncreaseDecreaseWidthAndHeight,
            HotkeyProcessingType.AlwaysForefrontOrCancel => Common.ApplicationData.Languages.LanguagesWindow.AlwaysShowOrCancelOnTop,
            HotkeyProcessingType.SpecifyTransparencyOrCancel => Common.ApplicationData.Languages.LanguagesWindow.SpecifyCancelTransparency,
            HotkeyProcessingType.StartStopEvent => Common.ApplicationData.Languages.LanguagesWindow.StartStopProcessingOfEvent,
            HotkeyProcessingType.BatchEvent => Common.ApplicationData.Languages.LanguagesWindow.BatchProcessingOfEvent,
            HotkeyProcessingType.OnlyActiveWindowEvent => Common.ApplicationData.Languages.LanguagesWindow.OnlyActiveWindowEvent,
            HotkeyProcessingType.StartStopTimer => Common.ApplicationData.Languages.LanguagesWindow.StartStopProcessingOfTimer,
            HotkeyProcessingType.BatchTimer => Common.ApplicationData.Languages.LanguagesWindow.BatchProcessingOfTimer,
            HotkeyProcessingType.OnlyActiveWindowTimer => Common.ApplicationData.Languages.LanguagesWindow.OnlyActiveWindowTimer,
            HotkeyProcessingType.StartStopMagnet => Common.ApplicationData.Languages.LanguagesWindow.StartStopProcessingOfMagnet,
            _ => Common.ApplicationData.Languages.LanguagesWindow.SpecifyPositionAndSize
        };
        Processing.SelectComboBoxItem(ProcessingTypeComboBox, stringData);
        switch (HotkeyItemInformation.StandardDisplay)
        {
            case StandardDisplay.DisplayWithWindow:
                stringData = Common.ApplicationData.Languages.LanguagesWindow.DisplayWithWindow;
                DisplayLabel.IsEnabled = false;
                DisplayComboBox.IsEnabled = false;
                break;
            default:
                stringData = Common.ApplicationData.Languages.LanguagesWindow.TheSpecifiedDisplay;
                break;
        }
        Processing.SelectComboBoxItem(StandardDisplayComboBox, stringData);
        Processing.SelectComboBoxItem(DisplayComboBox, HotkeyItemInformation.PositionSize.Display);
        if (DisplayComboBox.SelectedIndex == -1)
        {
            DisplayComboBox.SelectedIndex = 0;
        }
        stringData = HotkeyItemInformation.PositionSize.XType switch
        {
            WindowXType.DoNotChange => Common.ApplicationData.Languages.LanguagesWindow.DoNotChange,
            WindowXType.Left => Common.ApplicationData.Languages.LanguagesWindow.LeftEdge,
            WindowXType.Middle => Common.ApplicationData.Languages.LanguagesWindow.Middle,
            WindowXType.Right => Common.ApplicationData.Languages.LanguagesWindow.RightEdge,
            WindowXType.Value => Common.ApplicationData.Languages.LanguagesWindow.CoordinateSpecification,
            _ => Common.ApplicationData.Languages.LanguagesWindow.DoNotChange,
        };
        Processing.SelectComboBoxItem(XComboBox, stringData);
        switch (HotkeyItemInformation.PositionSize.XValueType)
        {
            case PositionSizeValueType.Percent:
                stringData = Common.ApplicationData.Languages.LanguagesWindow.Percent;
                XNumericUpDown.IsUseDecimal = true;
                break;
            default:
                stringData = Common.ApplicationData.Languages.LanguagesWindow.Pixel;
                XNumericUpDown.IsUseDecimal = false;
                break;
        }
        Processing.SelectComboBoxItem(XTypeComboBox, stringData);
        XNumericUpDown.Value = HotkeyItemInformation.PositionSize.Position.X;
        stringData = HotkeyItemInformation.PositionSize.YType switch
        {
            WindowYType.DoNotChange => Common.ApplicationData.Languages.LanguagesWindow.DoNotChange,
            WindowYType.Top => Common.ApplicationData.Languages.LanguagesWindow.TopEdge,
            WindowYType.Middle => Common.ApplicationData.Languages.LanguagesWindow.Middle,
            WindowYType.Bottom => Common.ApplicationData.Languages.LanguagesWindow.BottomEdge,
            WindowYType.Value => Common.ApplicationData.Languages.LanguagesWindow.CoordinateSpecification,
            _ => Common.ApplicationData.Languages.LanguagesWindow.DoNotChange,
        };
        Processing.SelectComboBoxItem(YComboBox, stringData);
        switch (HotkeyItemInformation.PositionSize.YValueType)
        {
            case PositionSizeValueType.Percent:
                stringData = Common.ApplicationData.Languages.LanguagesWindow.Percent;
                YNumericUpDown.IsUseDecimal = true;
                break;
            default:
                stringData = Common.ApplicationData.Languages.LanguagesWindow.Pixel;
                YNumericUpDown.IsUseDecimal = false;
                break;
        }
        Processing.SelectComboBoxItem(YTypeComboBox, stringData);
        YNumericUpDown.Value = HotkeyItemInformation.PositionSize.Position.Y;
        stringData = HotkeyItemInformation.PositionSize.WidthType switch
        {
            WindowSizeType.DoNotChange => Common.ApplicationData.Languages.LanguagesWindow.DoNotChange,
            WindowSizeType.Value => Common.ApplicationData.Languages.LanguagesWindow.WidthSpecification,
            _ => Common.ApplicationData.Languages.LanguagesWindow.DoNotChange,
        };
        Processing.SelectComboBoxItem(WidthComboBox, stringData);
        switch (HotkeyItemInformation.PositionSize.WidthValueType)
        {
            case PositionSizeValueType.Percent:
                stringData = Common.ApplicationData.Languages.LanguagesWindow.Percent;
                WidthNumericUpDown.IsUseDecimal = true;
                break;
            default:
                stringData = Common.ApplicationData.Languages.LanguagesWindow.Pixel;
                WidthNumericUpDown.IsUseDecimal = false;
                break;
        }
        Processing.SelectComboBoxItem(WidthTypeComboBox, stringData);
        WidthNumericUpDown.Value = HotkeyItemInformation.PositionSize.Size.Width;
        stringData = HotkeyItemInformation.PositionSize.HeightType switch
        {
            WindowSizeType.DoNotChange => Common.ApplicationData.Languages.LanguagesWindow.DoNotChange,
            WindowSizeType.Value => Common.ApplicationData.Languages.LanguagesWindow.HeightSpecification,
            _ => Common.ApplicationData.Languages.LanguagesWindow.DoNotChange,
        };
        Processing.SelectComboBoxItem(HeightComboBox, stringData);
        switch (HotkeyItemInformation.PositionSize.HeightValueType)
        {
            case PositionSizeValueType.Percent:
                stringData = Common.ApplicationData.Languages.LanguagesWindow.Percent;
                HeightNumericUpDown.IsUseDecimal = true;
                break;
            default:
                stringData = Common.ApplicationData.Languages.LanguagesWindow.Pixel;
                HeightNumericUpDown.IsUseDecimal = false;
                break;
        }
        Processing.SelectComboBoxItem(HeightTypeComboBox, stringData);
        HeightNumericUpDown.Value = HotkeyItemInformation.PositionSize.Size.Height;
        ClientAreaToggleSwitch.IsOn = HotkeyItemInformation.PositionSize.ClientArea;
        ProcessingPositionAndSizeTwiceToggleSwitch.IsOn = HotkeyItemInformation.PositionSize.ProcessingPositionAndSizeTwice;
        switch (HotkeyItemInformation.ProcessingType)
        {
            case HotkeyProcessingType.MoveX:
            case HotkeyProcessingType.MoveY:
                AmountOfMovementNumericUpDown.Value = HotkeyItemInformation.ProcessingValue;
                break;
            case HotkeyProcessingType.IncreaseDecreaseWidth:
            case HotkeyProcessingType.IncreaseDecreaseHeight:
            case HotkeyProcessingType.IncreaseDecreaseWidthHeight:
                SizeChangeAmountNumericUpDown.Value = HotkeyItemInformation.ProcessingValue;
                break;
            case HotkeyProcessingType.SpecifyTransparencyOrCancel:
                TransparencyNumericUpDown.Value = HotkeyItemInformation.ProcessingValue;
                break;
        }
        HotkeyTextBox.Text = FreeEcho.FEHotKeyWPF.HotKeyWPF.GetHotKeyString(HotkeyItemInformation.Hotkey);
        SettingsControlEnabled();
        SettingsAddModifyButtonEnabled();

        Closed += HotkeyWindowProcessingAddModifyWindow_Closed;
        RegisteredNameTextBox.TextChanged += RegisteredNameTextBox_TextChanged;
        ProcessingTypeComboBox.SelectionChanged += ProcessingTypeComboBox_SelectionChanged;
        StandardDisplayComboBox.SelectionChanged += StandardDisplayComboBox_SelectionChanged;
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
            Common.ApplicationData.Settings.HotkeyInformation.AddModifyWindowSize.Width = (int)Width;
            Common.ApplicationData.Settings.HotkeyInformation.AddModifyWindowSize.Height = (int)Height;
            Common.ApplicationData.DoProcessingEvent(ProcessingEventType.UnpauseHotkeyProcessing);
            SettingFileProcessing.WriteSettings();
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
    private void ProcessingTypeComboBox_SelectionChanged(
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
            if ((string)((ComboBoxItem)StandardDisplayComboBox.SelectedItem).Content == Common.ApplicationData.Languages.LanguagesWindow?.DisplayWithWindow)
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
            SettingsXControlEnabled();
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
            if ((string)((ComboBoxItem)XTypeComboBox.SelectedItem).Content == Common.ApplicationData.Languages.LanguagesWindow?.Percent)
            {
                XNumericUpDown.IsUseDecimal = true;
                XNumericUpDown.MinimumValue = Common.PercentMinimize;
                XNumericUpDown.MaximumValue = Common.PercentMaximize;
            }
            else
            {
                XNumericUpDown.IsUseDecimal = false;
                XNumericUpDown.MinimumValue = int.MinValue;
                XNumericUpDown.MaximumValue = int.MaxValue;
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
            SettingsYControlEnabled();
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
            if ((string)((ComboBoxItem)YTypeComboBox.SelectedItem).Content == Common.ApplicationData.Languages.LanguagesWindow?.Percent)
            {
                YNumericUpDown.IsUseDecimal = true;
                YNumericUpDown.MinimumValue = Common.PercentMinimize;
                YNumericUpDown.MaximumValue = Common.PercentMaximize;
            }
            else
            {
                YNumericUpDown.IsUseDecimal = false;
                YNumericUpDown.MinimumValue = int.MinValue;
                YNumericUpDown.MaximumValue = int.MaxValue;
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
            SettingsWidthControlEnabled();
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
            if ((string)((ComboBoxItem)WidthTypeComboBox.SelectedItem).Content == Common.ApplicationData.Languages.LanguagesWindow?.Percent)
            {
                WidthNumericUpDown.IsUseDecimal = true;
                WidthNumericUpDown.MaximumValue = Common.PercentMaximize;
            }
            else
            {
                WidthNumericUpDown.IsUseDecimal = false;
                WidthNumericUpDown.MaximumValue = int.MaxValue;
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
            SettingsHeightControlEnabled();
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
            if ((string)((ComboBoxItem)HeightTypeComboBox.SelectedItem).Content == Common.ApplicationData.Languages.LanguagesWindow?.Percent)
            {
                HeightNumericUpDown.IsUseDecimal = true;
                HeightNumericUpDown.MaximumValue = Common.PercentMaximize;
            }
            else
            {
                HeightNumericUpDown.IsUseDecimal = false;
                HeightNumericUpDown.MaximumValue = int.MaxValue;
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
            Common.ApplicationData.DoProcessingEvent(ProcessingEventType.PauseHotkeyProcessing);
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
            Common.ApplicationData.DoProcessingEvent(ProcessingEventType.UnpauseHotkeyProcessing);
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
            FreeEcho.FEControls.MessageBox.Show(Common.ApplicationData.Languages.Check, Common.ApplicationData.Languages.ErrorOccurred, FreeEcho.FEControls.MessageBoxButton.Ok, this);
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
            GetHotkeyInformation();
            if (CheckValueHotkeyItemInformation(HotkeyItemInformation, IndexOfItemToBeModified))
            {
                Common.ApplicationData.WindowProcessingManagement.Hotkey?.UnregisterHotkeys();
                try
                {
                    if (IndexOfItemToBeModified == -1)
                    {
                        Common.ApplicationData.Settings.HotkeyInformation.Items.Add(HotkeyItemInformation);
                        FreeEcho.FEControls.MessageBox.Show(Common.ApplicationData.Languages.Check, Common.ApplicationData.Languages.LanguagesWindow?.Added, FreeEcho.FEControls.MessageBoxButton.Ok, this);
                    }
                    else
                    {
                        Common.ApplicationData.Settings.HotkeyInformation.Items[IndexOfItemToBeModified] = HotkeyItemInformation;
                        FreeEcho.FEControls.MessageBox.Show(Common.ApplicationData.Languages.Check, Common.ApplicationData.Languages.LanguagesWindow?.Modified, FreeEcho.FEControls.MessageBoxButton.Ok, this);
                    }
                    AddedModified = true;
                }
                catch
                {
                    FreeEcho.FEControls.MessageBox.Show(Common.ApplicationData.Languages.Check, Common.ApplicationData.Languages.ErrorOccurred, FreeEcho.FEControls.MessageBoxButton.Ok, this);
                }
                Common.ApplicationData.WindowProcessingManagement.Hotkey?.RegisterHotkeys();
                Close();
            }
            else
            {
                FreeEcho.FEControls.MessageBox.Show(Common.ApplicationData.Languages.Check, Common.ApplicationData.Languages.LanguagesWindow?.ThereIsADuplicateRegistrationName, FreeEcho.FEControls.MessageBoxButton.Ok, this);
            }
        }
        catch
        {
            FreeEcho.FEControls.MessageBox.Show(Common.ApplicationData.Languages.Check, Common.ApplicationData.Languages.ErrorOccurred, FreeEcho.FEControls.MessageBoxButton.Ok, this);
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
    private void SettingsXControlEnabled()
    {
        if ((string)((ComboBoxItem)XComboBox.SelectedItem).Content == Common.ApplicationData.Languages.LanguagesWindow?.CoordinateSpecification)
        {
            XNumericUpDown.IsEnabled = true;
            XTypeComboBox.IsEnabled = true;
        }
        else
        {
            XNumericUpDown.IsEnabled = false;
            XTypeComboBox.IsEnabled = false;
        }
    }

    /// <summary>
    /// 「Y」関係のコントロールの有効状態を設定
    /// </summary>
    private void SettingsYControlEnabled()
    {
        if ((string)((ComboBoxItem)YComboBox.SelectedItem).Content == Common.ApplicationData.Languages.LanguagesWindow?.CoordinateSpecification)
        {
            YNumericUpDown.IsEnabled = true;
            YTypeComboBox.IsEnabled = true;
        }
        else
        {
            YNumericUpDown.IsEnabled = false;
            YTypeComboBox.IsEnabled = false;
        }
    }

    /// <summary>
    /// 「幅」関係のコントロールの有効状態を設定
    /// </summary>
    private void SettingsWidthControlEnabled()
    {
        if ((string)((ComboBoxItem)WidthComboBox.SelectedItem).Content == Common.ApplicationData.Languages.LanguagesWindow?.WidthSpecification)
        {
            WidthNumericUpDown.IsEnabled = true;
            WidthTypeComboBox.IsEnabled = true;
        }
        else
        {
            WidthNumericUpDown.IsEnabled = false;
            WidthTypeComboBox.IsEnabled = false;
        }
    }

    /// <summary>
    /// 「高さ」関係のコントロールの有効状態を設定
    /// </summary>
    private void SettingsHeightControlEnabled()
    {
        if ((string)((ComboBoxItem)HeightComboBox.SelectedItem).Content == Common.ApplicationData.Languages.LanguagesWindow?.HeightSpecification)
        {
            HeightNumericUpDown.IsEnabled = true;
            HeightTypeComboBox.IsEnabled = true;
        }
        else
        {
            HeightNumericUpDown.IsEnabled = false;
            HeightTypeComboBox.IsEnabled = false;
        }
    }

    /// <summary>
    /// コントロールの有効状態を設定
    /// </summary>
    private void SettingsControlEnabled()
    {
        if ((string)((ComboBoxItem)ProcessingTypeComboBox.SelectedItem).Content == Common.ApplicationData.Languages.LanguagesWindow?.SpecifyPositionAndSize)
        {
            DisplayGroupBox.IsEnabled = true;
            PositionSizeGroupBox.IsEnabled = true;
            SettingsXControlEnabled();
            SettingsYControlEnabled();
            SettingsWidthControlEnabled();
            SettingsHeightControlEnabled();
        }
        else
        {
            DisplayGroupBox.IsEnabled = false;
            PositionSizeGroupBox.IsEnabled = false;
        }

        string selectedItemString = (string)((ComboBoxItem)ProcessingTypeComboBox.SelectedItem).Content;
        if ((selectedItemString == Common.ApplicationData.Languages.LanguagesWindow?.MoveXCoordinate)
            || (selectedItemString == Common.ApplicationData.Languages.LanguagesWindow?.MoveYCoordinate))
        {
            AmountOfMovementGrid.IsEnabled = true;
            SizeChangeAmountGrid.IsEnabled = false;
            TransparencyGrid.IsEnabled = false;
        }
        else if ((selectedItemString == Common.ApplicationData.Languages.LanguagesWindow?.IncreaseDecreaseWidth)
            || (selectedItemString == Common.ApplicationData.Languages.LanguagesWindow?.IncreaseDecreaseHeight)
            || (selectedItemString == Common.ApplicationData.Languages.LanguagesWindow?.IncreaseDecreaseWidthAndHeight))
        {
            AmountOfMovementGrid.IsEnabled = false;
            SizeChangeAmountGrid.IsEnabled = true;
            TransparencyGrid.IsEnabled = false;
        }
        else if (selectedItemString == Common.ApplicationData.Languages.LanguagesWindow?.SpecifyCancelTransparency)
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
    /// ホットキー情報取得
    /// </summary>
    private void GetHotkeyInformation()
    {
        string stringData;     // 文字列データ

        HotkeyItemInformation.RegisteredName = RegisteredNameTextBox.Text;
        stringData = (string)((ComboBoxItem)ProcessingTypeComboBox.SelectedItem).Content;
        if (stringData == Common.ApplicationData.Languages.LanguagesWindow?.SpecifyPositionAndSize)
        {
            HotkeyItemInformation.ProcessingType = HotkeyProcessingType.PositionSize;
        }
        else if (stringData == Common.ApplicationData.Languages.LanguagesWindow?.MoveXCoordinate)
        {
            HotkeyItemInformation.ProcessingType = HotkeyProcessingType.MoveX;
        }
        else if (stringData == Common.ApplicationData.Languages.LanguagesWindow?.MoveYCoordinate)
        {
            HotkeyItemInformation.ProcessingType = HotkeyProcessingType.MoveY;
        }
        else if (stringData == Common.ApplicationData.Languages.LanguagesWindow?.IncreaseDecreaseWidth)
        {
            HotkeyItemInformation.ProcessingType = HotkeyProcessingType.IncreaseDecreaseWidth;
        }
        else if (stringData == Common.ApplicationData.Languages.LanguagesWindow?.IncreaseDecreaseHeight)
        {
            HotkeyItemInformation.ProcessingType = HotkeyProcessingType.IncreaseDecreaseHeight;
        }
        else if (stringData == Common.ApplicationData.Languages.LanguagesWindow?.IncreaseDecreaseWidthAndHeight)
        {
            HotkeyItemInformation.ProcessingType = HotkeyProcessingType.IncreaseDecreaseWidthHeight;
        }
        else if (stringData == Common.ApplicationData.Languages.LanguagesWindow?.AlwaysShowOrCancelOnTop)
        {
            HotkeyItemInformation.ProcessingType = HotkeyProcessingType.AlwaysForefrontOrCancel;
        }
        else if (stringData == Common.ApplicationData.Languages.LanguagesWindow?.SpecifyCancelTransparency)
        {
            HotkeyItemInformation.ProcessingType = HotkeyProcessingType.SpecifyTransparencyOrCancel;
        }
        else if (stringData == Common.ApplicationData.Languages.LanguagesWindow?.StartStopProcessingOfEvent)
        {
            HotkeyItemInformation.ProcessingType = HotkeyProcessingType.StartStopEvent;
        }
        else if (stringData == Common.ApplicationData.Languages.LanguagesWindow?.BatchProcessingOfEvent)
        {
            HotkeyItemInformation.ProcessingType = HotkeyProcessingType.BatchEvent;
        }
        else if (stringData == Common.ApplicationData.Languages.LanguagesWindow?.OnlyActiveWindowEvent)
        {
            HotkeyItemInformation.ProcessingType = HotkeyProcessingType.OnlyActiveWindowEvent;
        }
        else if (stringData == Common.ApplicationData.Languages.LanguagesWindow?.StartStopProcessingOfTimer)
        {
            HotkeyItemInformation.ProcessingType = HotkeyProcessingType.StartStopTimer;
        }
        else if (stringData == Common.ApplicationData.Languages.LanguagesWindow?.BatchProcessingOfTimer)
        {
            HotkeyItemInformation.ProcessingType = HotkeyProcessingType.BatchTimer;
        }
        else if (stringData == Common.ApplicationData.Languages.LanguagesWindow?.OnlyActiveWindowTimer)
        {
            HotkeyItemInformation.ProcessingType = HotkeyProcessingType.OnlyActiveWindowTimer;
        }
        else if (stringData == Common.ApplicationData.Languages.LanguagesWindow?.StartStopProcessingOfMagnet)
        {
            HotkeyItemInformation.ProcessingType = HotkeyProcessingType.StartStopMagnet;
        }
        switch (HotkeyItemInformation.ProcessingType)
        {
            case HotkeyProcessingType.PositionSize:
                if (StandardDisplayComboBox.Text == Common.ApplicationData.Languages.LanguagesWindow?.TheSpecifiedDisplay)
                {
                    HotkeyItemInformation.StandardDisplay = StandardDisplay.SpecifiedDisplay;
                }
                else if (StandardDisplayComboBox.Text == Common.ApplicationData.Languages.LanguagesWindow?.DisplayWithWindow)
                {
                    HotkeyItemInformation.StandardDisplay = StandardDisplay.DisplayWithWindow;
                }
                HotkeyItemInformation.PositionSize.Display = (string)((ComboBoxItem)DisplayComboBox.SelectedItem).Content;
                stringData = (string)((ComboBoxItem)XComboBox.SelectedItem).Content;
                if (stringData == Common.ApplicationData.Languages.LanguagesWindow?.DoNotChange)
                {
                    HotkeyItemInformation.PositionSize.XType = WindowXType.DoNotChange;
                }
                else if (stringData == Common.ApplicationData.Languages.LanguagesWindow?.LeftEdge)
                {
                    HotkeyItemInformation.PositionSize.XType = WindowXType.Left;
                }
                else if (stringData == Common.ApplicationData.Languages.LanguagesWindow?.Middle)
                {
                    HotkeyItemInformation.PositionSize.XType = WindowXType.Middle;
                }
                else if (stringData == Common.ApplicationData.Languages.LanguagesWindow?.RightEdge)
                {
                    HotkeyItemInformation.PositionSize.XType = WindowXType.Right;
                }
                else if (stringData == Common.ApplicationData.Languages.LanguagesWindow?.CoordinateSpecification)
                {
                    HotkeyItemInformation.PositionSize.XType = WindowXType.Value;
                    HotkeyItemInformation.PositionSize.Position.X = XNumericUpDown.Value;
                    if ((string)((ComboBoxItem)XTypeComboBox.SelectedItem).Content == Common.ApplicationData.Languages.LanguagesWindow.Percent)
                    {
                        HotkeyItemInformation.PositionSize.XValueType = PositionSizeValueType.Percent;
                    }
                    else
                    {
                        HotkeyItemInformation.PositionSize.XValueType = PositionSizeValueType.Pixel;
                    }
                }
                stringData = (string)((ComboBoxItem)YComboBox.SelectedItem).Content;
                if (stringData == Common.ApplicationData.Languages.LanguagesWindow?.DoNotChange)
                {
                    HotkeyItemInformation.PositionSize.YType = WindowYType.DoNotChange;
                }
                else if (stringData == Common.ApplicationData.Languages.LanguagesWindow?.TopEdge)
                {
                    HotkeyItemInformation.PositionSize.YType = WindowYType.Top;
                }
                else if (stringData == Common.ApplicationData.Languages.LanguagesWindow?.Middle)
                {
                    HotkeyItemInformation.PositionSize.YType = WindowYType.Middle;
                }
                else if (stringData == Common.ApplicationData.Languages.LanguagesWindow?.BottomEdge)
                {
                    HotkeyItemInformation.PositionSize.YType = WindowYType.Bottom;
                }
                else if (stringData == Common.ApplicationData.Languages.LanguagesWindow?.CoordinateSpecification)
                {
                    HotkeyItemInformation.PositionSize.YType = WindowYType.Value;
                    HotkeyItemInformation.PositionSize.Position.Y = YNumericUpDown.Value;
                    if ((string)((ComboBoxItem)YTypeComboBox.SelectedItem).Content == Common.ApplicationData.Languages.LanguagesWindow.Percent)
                    {
                        HotkeyItemInformation.PositionSize.YValueType = PositionSizeValueType.Percent;
                    }
                    else
                    {
                        HotkeyItemInformation.PositionSize.YValueType = PositionSizeValueType.Pixel;
                    }
                }
                stringData = (string)((ComboBoxItem)WidthComboBox.SelectedItem).Content;
                if (stringData == Common.ApplicationData.Languages.LanguagesWindow?.DoNotChange)
                {
                    HotkeyItemInformation.PositionSize.WidthType = WindowSizeType.DoNotChange;
                }
                else if (stringData == Common.ApplicationData.Languages.LanguagesWindow?.WidthSpecification)
                {
                    HotkeyItemInformation.PositionSize.WidthType = WindowSizeType.Value;
                    HotkeyItemInformation.PositionSize.Size.Width = WidthNumericUpDown.Value;
                    if ((string)((ComboBoxItem)WidthTypeComboBox.SelectedItem).Content == Common.ApplicationData.Languages.LanguagesWindow.Percent)
                    {
                        HotkeyItemInformation.PositionSize.WidthValueType = PositionSizeValueType.Percent;
                    }
                    else
                    {
                        HotkeyItemInformation.PositionSize.WidthValueType = PositionSizeValueType.Pixel;
                    }
                }
                stringData = (string)((ComboBoxItem)HeightComboBox.SelectedItem).Content;
                if (stringData == Common.ApplicationData.Languages.LanguagesWindow?.DoNotChange)
                {
                    HotkeyItemInformation.PositionSize.HeightType = WindowSizeType.DoNotChange;
                }
                else if (stringData == Common.ApplicationData.Languages.LanguagesWindow?.HeightSpecification)
                {
                    HotkeyItemInformation.PositionSize.HeightType = WindowSizeType.Value;
                    HotkeyItemInformation.PositionSize.Size.Height = HeightNumericUpDown.Value;
                    if ((string)((ComboBoxItem)HeightTypeComboBox.SelectedItem).Content == Common.ApplicationData.Languages.LanguagesWindow.Percent)
                    {
                        HotkeyItemInformation.PositionSize.HeightValueType = PositionSizeValueType.Percent;
                    }
                    else
                    {
                        HotkeyItemInformation.PositionSize.HeightValueType = PositionSizeValueType.Pixel;
                    }
                }
                HotkeyItemInformation.PositionSize.ClientArea = ClientAreaToggleSwitch.IsOn;
                HotkeyItemInformation.PositionSize.ProcessingPositionAndSizeTwice = ProcessingPositionAndSizeTwiceToggleSwitch.IsOn;
                break;
            case HotkeyProcessingType.MoveX:
            case HotkeyProcessingType.MoveY:
                HotkeyItemInformation.ProcessingValue = AmountOfMovementNumericUpDown.ValueInt;
                break;
            case HotkeyProcessingType.IncreaseDecreaseWidth:
            case HotkeyProcessingType.IncreaseDecreaseHeight:
            case HotkeyProcessingType.IncreaseDecreaseWidthHeight:
                HotkeyItemInformation.ProcessingValue = SizeChangeAmountNumericUpDown.ValueInt;
                break;
            case HotkeyProcessingType.SpecifyTransparencyOrCancel:
                HotkeyItemInformation.ProcessingValue = TransparencyNumericUpDown.ValueInt;
                break;
        }
    }

    /// <summary>
    /// 「HotkeyItemInformation」の値の確認
    /// </summary>
    /// <param name="hotkeyItemInformation">HotkeyItemInformation</param>
    /// <param name="indexOfItemToBeModified">修正する項目のインデックス (追加「-1」)</param>
    /// <returns>値に問題があるかの値 (問題がある「false」/問題ない「true」)</returns>
    private bool CheckValueHotkeyItemInformation(
        HotkeyItemInformation hotkeyItemInformation,
        int indexOfItemToBeModified
        )
    {
        bool result = true;     // 結果
        int count = 0;      // カウント

        // 重複確認
        foreach (HotkeyItemInformation nowItem in Common.ApplicationData.Settings.HotkeyInformation.Items)
        {
            if (nowItem.RegisteredName == hotkeyItemInformation.RegisteredName
                && (indexOfItemToBeModified == -1 || indexOfItemToBeModified != count))
            {
                result = false;
                break;
            }
            count++;
        }

        return result;
    }
}
