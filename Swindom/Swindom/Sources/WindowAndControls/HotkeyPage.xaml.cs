namespace Swindom;

/// <summary>
/// 「ホットキー」ページ
/// </summary>
public partial class HotkeyPage : Page
{
    /// <summary>
    /// 「ホットキー」ウィンドウ処理の追加/修正ウィンドウ
    /// </summary>
    private HotkeyItemWindow? HotkeyItemWindow;
    /// <summary>
    /// ウィンドウ選択枠
    /// </summary>
    private readonly FreeEcho.FEWindowSelectionMouse.WindowSelectionFrame WindowSelectionMouse = new()
    {
        MouseLeftUpStop = true
    };
    /// <summary>
    /// 以前の「WindowState」
    /// </summary>
    private WindowState BeforeWindowState = WindowState.Normal;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public HotkeyPage()
    {
        InitializeComponent();

        SettingsRowDefinition.Height = new(WindowControlValue.SettingsRowDefinitionMinimize);
        SettingsScrollViewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled;
        SettingsScrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Disabled;
        ModifyButton.IsEnabled = false;
        DeleteButton.IsEnabled = false;
        MoveUpButton.IsEnabled = false;
        MoveDownButton.IsEnabled = false;
        SelectWindowTargetButton.IsEnabled = false;
        SettingsControlsImage();
        SetTextOnControls();

        HotkeyListBox.SelectionChanged += HotkeyListBox_SelectionChanged;
        AddButton.Click += AddButton_Click;
        ModifyButton.Click += ModifyButton_Click;
        DeleteButton.Click += DeleteButton_Click;
        MoveUpButton.Click += MoveUpButton_Click;
        MoveDownButton.Click += MoveDownButton_Click;
        SelectWindowTargetButton.PreviewMouseDown += SelectWindowTargetButton_PreviewMouseDown;
        SettingsButton.Click += SettingsButton_Click;

        ProcessingStateToggleSwitch.IsOn = ApplicationData.Settings.HotkeyInformation.IsEnabled;
        DoNotChangeOutOfScreenToggleSwitch.IsOn = ApplicationData.Settings.HotkeyInformation.DoNotChangeOutOfScreen;
        StopProcessingFullScreenToggleSwitch.IsOn = ApplicationData.Settings.HotkeyInformation.StopProcessingFullScreen;
        ProcessingStateToggleSwitch.Toggled += ProcessingStateToggleSwitch_Toggled;
        DoNotChangeOutOfScreenToggleSwitch.Toggled += DoNotChangeOutOfScreenToggleSwitch_Toggled;
        StopProcessingFullScreenToggleSwitch.Toggled += StopProcessingFullScreenToggleSwitch_Toggled;

        WindowSelectionMouse.MouseLeftButtonUp += WindowSelectionMouse_MouseLeftButtonUp;
        ApplicationData.EventData.ProcessingEvent += ApplicationData_ProcessingEvent;
    }

    /// <summary>
    /// 破棄
    /// </summary>
    public void Release()
    {
        ApplicationData.EventData.ProcessingEvent -= ApplicationData_ProcessingEvent;
        WindowSelectionMouse.Dispose();
        HotkeyItemWindow?.Close();
    }

    /// <summary>
    /// 「ホットキー」ListBoxの「SelectionChanged」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void HotkeyListBox_SelectionChanged(
        object sender,
        SelectionChangedEventArgs e
        )
    {
        try
        {
            if (HotkeyListBox.SelectedItems.Count == 0)
            {
                ModifyButton.IsEnabled = false;
                DeleteButton.IsEnabled = false;
                MoveUpButton.IsEnabled = false;
                MoveDownButton.IsEnabled = false;
                SelectWindowTargetButton.IsEnabled = false;
            }
            else
            {
                ModifyButton.IsEnabled = true;
                DeleteButton.IsEnabled = true;
                MoveUpButton.IsEnabled = HotkeyListBox.SelectedIndex != 0;
                MoveDownButton.IsEnabled = (HotkeyListBox.SelectedIndex + 1) != HotkeyListBox.Items.Count;
                SelectWindowTargetButton.IsEnabled = ApplicationData.Settings.HotkeyInformation.IsEnabled;
            }
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
            if (HotkeyItemWindow == null)
            {
                HotkeyItemWindow = new(-1);
                HotkeyItemWindow.Closed += AddModifyWindowHotkey_Closed;
                HotkeyItemWindow.Show();
            }
            else
            {
                HotkeyItemWindow.Activate();
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
            if (HotkeyItemWindow == null)
            {
                HotkeyItemWindow = new(HotkeyListBox.SelectedIndex);
                HotkeyItemWindow.Closed += AddModifyWindowHotkey_Closed;
                HotkeyItemWindow.Show();
            }
            else
            {
                HotkeyItemWindow.Activate();
            }
        }
        catch
        {
            FEMessageBox.Show(ApplicationData.Languages.ErrorOccurred, ApplicationData.Languages.Check, MessageBoxButton.OK);
        }
    }

    /// <summary>
    /// 「追加/修正」ウィンドウの「Closed」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void AddModifyWindowHotkey_Closed(
        object? sender,
        EventArgs e
        )
    {
        try
        {
            if (HotkeyItemWindow != null)
            {
                if (HotkeyItemWindow.AddedModified)
                {
                    UpdateHotkeyListBox();
                }
                HotkeyItemWindow.Owner.Activate();
                HotkeyItemWindow = null;
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
            if (FEMessageBox.Show(ApplicationData.Settings.HotkeyInformation.Items[HotkeyListBox.SelectedIndex].RegisteredName + Environment.NewLine + ApplicationData.Languages.AllowDelete, ApplicationData.Languages.Check, MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                ApplicationData.WindowProcessingManagement.HotkeyProcessing?.UnregisterHotkeys();
                try
                {
                    ApplicationData.Settings.HotkeyInformation.Items.RemoveAt(HotkeyListBox.SelectedIndex);
                    HotkeyListBox.Items.RemoveAt(HotkeyListBox.SelectedIndex);
                    FEMessageBox.Show(ApplicationData.Languages.Deleted, ApplicationData.Languages.Check, MessageBoxButton.OK);
                }
                catch
                {
                    FEMessageBox.Show(ApplicationData.Languages.ErrorOccurred, ApplicationData.Languages.Check, MessageBoxButton.OK);
                }
                ApplicationData.WindowProcessingManagement.HotkeyProcessing?.RegisterHotkeys();
            }
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
            ApplicationData.Settings.HotkeyInformation.Items.Reverse(HotkeyListBox.SelectedIndex - 1, 2);
            UpdateHotkeyListBox(-1);
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
            ApplicationData.Settings.HotkeyInformation.Items.Reverse(HotkeyListBox.SelectedIndex, 2);
            UpdateHotkeyListBox(1);
        }
        catch
        {
            FEMessageBox.Show(ApplicationData.Languages.ErrorOccurred, ApplicationData.Languages.Check, MessageBoxButton.OK);
        }
    }

    /// <summary>
    /// 「ウィンドウ選択」Buttonの「PreviewMouseDown」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void SelectWindowTargetButton_PreviewMouseDown(
        object? sender,
        MouseButtonEventArgs e
        )
    {
        try
        {
            BeforeWindowState = Window.GetWindow(this).WindowState;
            Window.GetWindow(this).WindowState = WindowState.Minimized;
            WindowSelectionMouse.StartWindowSelection();
        }
        catch
        {
            Window.GetWindow(this).WindowState = BeforeWindowState;
            FEMessageBox.Show(ApplicationData.Languages.ErrorOccurred, ApplicationData.Languages.Check, MessageBoxButton.OK);
        }
    }

    /// <summary>
    /// 「ウィンドウ選択」Buttonの「MouseLeftButtonUp」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void WindowSelectionMouse_MouseLeftButtonUp(
        object sender,
        FreeEcho.FEWindowSelectionMouse.MouseLeftButtonUpEventArgs e
        )
    {
        try
        {
            WindowSelectionMouse.StopWindowSelection();
            ApplicationData.WindowProcessingManagement.HotkeyProcessing?.PerformHotkeyProcessing(WindowSelectionMouse.SelectedHwnd, ApplicationData.Settings.HotkeyInformation.Items[HotkeyListBox.SelectedIndex]);
            Window.GetWindow(this).WindowState = BeforeWindowState;
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
            if (ApplicationData.Settings.HotkeyInformation.IsEnabled !=  ProcessingStateToggleSwitch.IsOn)
            {
                ApplicationData.Settings.HotkeyInformation.IsEnabled = ProcessingStateToggleSwitch.IsOn;
                SelectWindowTargetButton.IsEnabled = (HotkeyListBox.SelectedItems.Count == 1) && ApplicationData.Settings.HotkeyInformation.IsEnabled;
                ApplicationData.EventData.DoProcessingEvent(ProcessingEventType.HotkeyProcessingStateChanged);
            }
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
            ApplicationData.Settings.HotkeyInformation.DoNotChangeOutOfScreen = DoNotChangeOutOfScreenToggleSwitch.IsOn;
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「全画面ウィンドウがある場合は処理停止」ToggleSwitchの「Toggled」イベント
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
            ApplicationData.Settings.HotkeyInformation.StopProcessingFullScreen = StopProcessingFullScreenToggleSwitch.IsOn;
            ApplicationData.EventData.DoProcessingEvent(ProcessingEventType.StopWhenWindowIsFullScreenChanged);
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
        object? sender,
        ProcessingEventArgs e
        )
    {
        try
        {
            switch (e.ProcessingEventType)
            {
                case ProcessingEventType.HotkeyProcessingStateChanged:
                    if (ApplicationData.Settings.HotkeyInformation.IsEnabled != ProcessingStateToggleSwitch.IsOn)
                    {
                        ProcessingStateToggleSwitch.IsOn = ApplicationData.Settings.HotkeyInformation.IsEnabled;
                        SelectWindowTargetButton.IsEnabled = (HotkeyListBox.SelectedItems.Count == 1) && ApplicationData.Settings.HotkeyInformation.IsEnabled;
                    }
                    break;
                case ProcessingEventType.LanguageChanged:
                    SetTextOnControls();
                    break;
                case ProcessingEventType.ThemeChanged:
                    SettingsControlsImage();
                    break;
            }
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
            UpdateHotkeyListBox();
            AddButton.Text = ApplicationData.Languages.Add;
            ModifyButton.Text = ApplicationData.Languages.Modify;
            DeleteButton.Text = ApplicationData.Languages.Delete;
            MoveUpButton.Text = ApplicationData.Languages.MoveUp;
            MoveDownButton.Text = ApplicationData.Languages.MoveDown;
            SelectWindowTargetButton.Text = ApplicationData.Languages.Select;
            SelectWindowTargetButton.ToolTip = ApplicationData.Languages.HoldDownMousePointerMoveToSelectWindow;
            SettingsButton.Text = ApplicationData.Languages.Setting;

            ProcessingStateToggleSwitch.OffContent = ProcessingStateToggleSwitch.OnContent = ApplicationData.Languages.ProcessingState;
            DoNotChangeOutOfScreenToggleSwitch.OffContent = DoNotChangeOutOfScreenToggleSwitch.OnContent = ApplicationData.Languages.DoNotChangePositionSizeOutOfScreen;
            StopProcessingFullScreenToggleSwitch.OffContent = StopProcessingFullScreenToggleSwitch.OnContent = ApplicationData.Languages.StopProcessingWhenWindowIsFullScreen;

            ExplanationTextBlock.Text = ApplicationData.Languages.HotkeyExplanation;
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「ホットキー」ListBoxの項目を更新
    /// </summary>
    /// <param name="indexMoveValue">選択項目のインデックスを移動する数値</param>
    private void UpdateHotkeyListBox(
        int indexMoveValue = 0
        )
    {
        int selectedIndex = HotkeyListBox.SelectedIndex;      // 選択している項目のインデックス

        HotkeyListBox.Items.Clear();
        if (ApplicationData.Settings.HotkeyInformation.Items.Count != 0)
        {
            foreach (HotkeyItemInformation nowHII in ApplicationData.Settings.HotkeyInformation.Items)
            {
                ListBoxItem newItem = new()
                {
                    Content = GetStringHotkeyInformation(nowHII)
                };
                HotkeyListBox.Items.Add(newItem);
            }
        }
        HotkeyListBox.SelectedIndex = selectedIndex + indexMoveValue;
    }

    /// <summary>
    /// 「ホットキー」ListBoxの項目に表示する文字列取得
    /// </summary>
    /// <param name="hotkeyItemInformation">「ホットキー」機能の項目情報</param>
    /// <returns>ホットキーのListBoxの項目に表示する文字列</returns>
    private static string GetStringHotkeyInformation(
        HotkeyItemInformation hotkeyItemInformation
        )
    {
        string stringData = "";        // 文字列

        try
        {
            stringData += hotkeyItemInformation.RegisteredName + Environment.NewLine + "  ";
            switch (hotkeyItemInformation.ProcessingType)
            {
                case HotkeyProcessingType.PositionSize:
                    string separateString = "";        // 区切り文字列

                    stringData += ApplicationData.Languages.SpecifyPositionAndSize + WindowControlValue.ExplainAndValueSeparateString;
                    switch (hotkeyItemInformation.PositionSize.XType)
                    {
                        case WindowXType.Left:
                            stringData += separateString + ApplicationData.Languages.X + WindowControlValue.TypeAndValueSeparateString + ApplicationData.Languages.LeftEdge;
                            separateString = WindowControlValue.ValueAndValueSeparateString;
                            break;
                        case WindowXType.Middle:
                            stringData += separateString + ApplicationData.Languages.X + WindowControlValue.TypeAndValueSeparateString + ApplicationData.Languages.Middle;
                            separateString = WindowControlValue.ValueAndValueSeparateString;
                            break;
                        case WindowXType.Right:
                            stringData += separateString + ApplicationData.Languages.X + WindowControlValue.TypeAndValueSeparateString + ApplicationData.Languages.RightEdge;
                            separateString = WindowControlValue.ValueAndValueSeparateString;
                            break;
                        case WindowXType.Value:
                            stringData += separateString + ApplicationData.Languages.X + WindowControlValue.TypeAndValueSeparateString + hotkeyItemInformation.PositionSize.X + " " + ((hotkeyItemInformation.PositionSize.XValueType == PositionSizeValueType.Pixel) ? ApplicationData.Languages.Pixel : ApplicationData.Languages.Percent);
                            separateString = WindowControlValue.ValueAndValueSeparateString;
                            break;
                    }
                    switch (hotkeyItemInformation.PositionSize.YType)
                    {
                        case WindowYType.Top:
                            stringData += separateString + ApplicationData.Languages.Y + WindowControlValue.TypeAndValueSeparateString + ApplicationData.Languages.TopEdge;
                            separateString = WindowControlValue.ValueAndValueSeparateString;
                            break;
                        case WindowYType.Middle:
                            stringData += separateString + ApplicationData.Languages.Y + WindowControlValue.TypeAndValueSeparateString + ApplicationData.Languages.Middle;
                            separateString = WindowControlValue.ValueAndValueSeparateString;
                            break;
                        case WindowYType.Bottom:
                            stringData += separateString + ApplicationData.Languages.Y + WindowControlValue.TypeAndValueSeparateString + ApplicationData.Languages.BottomEdge;
                            separateString = WindowControlValue.ValueAndValueSeparateString;
                            break;
                        case WindowYType.Value:
                            stringData += separateString + ApplicationData.Languages.Y + WindowControlValue.TypeAndValueSeparateString + hotkeyItemInformation.PositionSize.Y + " " + ((hotkeyItemInformation.PositionSize.YValueType == PositionSizeValueType.Pixel) ? ApplicationData.Languages.Pixel : ApplicationData.Languages.Percent);
                            separateString = WindowControlValue.ValueAndValueSeparateString;
                            break;
                    }
                    switch (hotkeyItemInformation.PositionSize.WidthType)
                    {
                        case WindowSizeType.Value:
                            stringData += separateString + ApplicationData.Languages.Width + WindowControlValue.TypeAndValueSeparateString + hotkeyItemInformation.PositionSize.Width + " " + ((hotkeyItemInformation.PositionSize.WidthValueType == PositionSizeValueType.Pixel) ? ApplicationData.Languages.Pixel : ApplicationData.Languages.Percent);
                            separateString = WindowControlValue.ValueAndValueSeparateString;
                            break;
                    }
                    switch (hotkeyItemInformation.PositionSize.HeightType)
                    {
                        case WindowSizeType.Value:
                            stringData += separateString + ApplicationData.Languages.Height + WindowControlValue.TypeAndValueSeparateString + hotkeyItemInformation.PositionSize.Height + " " + ((hotkeyItemInformation.PositionSize.HeightValueType == PositionSizeValueType.Pixel) ? ApplicationData.Languages.Pixel : ApplicationData.Languages.Percent);
                            break;
                    }
                    break;
                case HotkeyProcessingType.MoveX:
                    stringData += ApplicationData.Languages.MoveXCoordinate + WindowControlValue.ExplainAndValueSeparateString + hotkeyItemInformation.ProcessingValue + " " + ApplicationData.Languages.Pixel;
                    break;
                case HotkeyProcessingType.MoveY:
                    stringData += ApplicationData.Languages.MoveYCoordinate + WindowControlValue.ExplainAndValueSeparateString + hotkeyItemInformation.ProcessingValue + " " + ApplicationData.Languages.Pixel;
                    break;
                case HotkeyProcessingType.IncreaseDecreaseWidth:
                    stringData += ApplicationData.Languages.IncreaseDecreaseWidth + WindowControlValue.ExplainAndValueSeparateString + hotkeyItemInformation.ProcessingValue + " " +ApplicationData.Languages.Pixel;
                    break;
                case HotkeyProcessingType.IncreaseDecreaseHeight:
                    stringData += ApplicationData.Languages.IncreaseDecreaseHeight + WindowControlValue.ExplainAndValueSeparateString + hotkeyItemInformation.ProcessingValue + " " + ApplicationData.Languages.Pixel;
                    break;
                case HotkeyProcessingType.IncreaseDecreaseWidthHeight:
                    stringData += ApplicationData.Languages.IncreaseDecreaseWidthAndHeight + WindowControlValue.ExplainAndValueSeparateString + hotkeyItemInformation.ProcessingValue + " " + ApplicationData.Languages.Pixel;
                    break;
                case HotkeyProcessingType.StartStopSpecifyWindow:
                    stringData += ApplicationData.Languages.StartStopProcessingOfSpecifyWindow;
                    break;
                case HotkeyProcessingType.StartStopAllWindow:
                    stringData += ApplicationData.Languages.StartStopProcessingOfAllWindow;
                    break;
                case HotkeyProcessingType.StartStopMagnet:
                    stringData += ApplicationData.Languages.StartStopProcessingOfMagnet;
                    break;
                case HotkeyProcessingType.AlwaysForefrontOrCancel:
                    stringData += ApplicationData.Languages.AlwaysShowOrCancelOnTop;
                    break;
                case HotkeyProcessingType.SpecifyTransparencyOrCancel:
                    stringData += ApplicationData.Languages.SpecifyCancelTransparency + WindowControlValue.ExplainAndValueSeparateString + hotkeyItemInformation.ProcessingValue;
                    break;
                case HotkeyProcessingType.BatchSpecifyWindow:
                    stringData += ApplicationData.Languages.BatchProcessingOfSpecifyWindow;
                    break;
                case HotkeyProcessingType.OnlyActiveWindowSpecifyWindow:
                    stringData += ApplicationData.Languages.OnlyActiveWindowSpecifyWindow;
                    break;
                case HotkeyProcessingType.ShowThisApplicationWindow:
                    stringData += ApplicationData.Languages.ShowThisApplicationWindow;
                    break;
                case HotkeyProcessingType.ShowNotifyIconMenu:
                    stringData += ApplicationData.Languages.ShowSystemTrayIconMenu;
                    break;
            }
            stringData += Environment.NewLine + "  " + FreeEcho.FEHotKeyWPF.HotKeyWPF.GetHotKeyString(hotkeyItemInformation.Hotkey);
        }
        catch
        {
        }

        return stringData;
    }

    /// <summary>
    /// コントロールの画像を設定
    /// </summary>
    private void SettingsControlsImage()
    {
        AddButton.ButtonImage = ImageProcessing.GetImageAddition();
        ModifyButton.ButtonImage = ImageProcessing.GetImageModify();
        DeleteButton.ButtonImage = ImageProcessing.GetImageDelete();
        MoveUpButton.ButtonImage = ImageProcessing.GetImageUp();
        MoveDownButton.ButtonImage = ImageProcessing.GetImageDown();
        SelectWindowTargetButton.ButtonImage = ImageProcessing.GetImageTarget();
        SettingsButton.ButtonImage = (int)SettingsRowDefinition.Height.Value == WindowControlValue.SettingsRowDefinitionMinimize ? ImageProcessing.GetImageSettings() : ImageProcessing.GetImageCloseSettings();
    }
}
