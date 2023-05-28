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
    /// 次のイベントをキャンセル
    /// </summary>
    private bool CancelNextEvent;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public HotkeyPage()
    {
        InitializeComponent();

        SettingsRowDefinition.Height = new(Common.SettingsRowDefinitionMinimize);
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

        ProcessingStateToggleSwitch.IsOn = ApplicationData.Settings.HotkeyInformation.Enabled;
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
                SelectWindowTargetButton.IsEnabled = ApplicationData.Settings.HotkeyInformation.Enabled;
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
        object sender,
        RoutedEventArgs e
        )
    {
        try
        {
            if (HotkeyItemWindow == null)
            {
                HotkeyItemWindow = new(Window.GetWindow(this));
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
        object sender,
        RoutedEventArgs e
        )
    {
        try
        {
            if (HotkeyItemWindow == null)
            {
                HotkeyItemWindow = new(Window.GetWindow(this), HotkeyListBox.SelectedIndex);
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
                    int selectedIndex = HotkeyListBox.SelectedIndex;
                    RenewalHotkeyListBox();
                    HotkeyListBox.SelectedIndex = selectedIndex;
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
        object sender,
        RoutedEventArgs e
        )
    {
        try
        {
            if (FEMessageBox.Show(ApplicationData.Languages.LanguagesWindow?.AllowDelete ?? "", ApplicationData.Languages.Check, MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                ApplicationData.WindowProcessingManagement.HotkeyProcessing?.UnregisterHotkeys();
                try
                {
                    ApplicationData.Settings.HotkeyInformation.Items.RemoveAt(HotkeyListBox.SelectedIndex);
                    HotkeyListBox.Items.RemoveAt(HotkeyListBox.SelectedIndex);
                    FEMessageBox.Show(ApplicationData.Languages.LanguagesWindow?.Deleted ?? "", ApplicationData.Languages.Check, MessageBoxButton.OK);
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
        object sender,
        RoutedEventArgs e
        )
    {
        try
        {
            int selectedIndex = HotkeyListBox.SelectedIndex;      // 選択している項目のインデックス
            ApplicationData.Settings.HotkeyInformation.Items.Reverse(selectedIndex - 1, 2);
            RenewalHotkeyListBox();
            HotkeyListBox.SelectedIndex = selectedIndex - 1;
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
            int selectedIndex = HotkeyListBox.SelectedIndex;      // 選択している項目のインデックス
            ApplicationData.Settings.HotkeyInformation.Items.Reverse(selectedIndex, 2);
            RenewalHotkeyListBox();
            HotkeyListBox.SelectedIndex = selectedIndex + 1;
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
        object sender,
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
                ApplicationData.Settings.HotkeyInformation.Enabled = ProcessingStateToggleSwitch.IsOn;
                SelectWindowTargetButton.IsEnabled = (HotkeyListBox.SelectedItems.Count == 1) && ApplicationData.Settings.HotkeyInformation.Enabled;
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
                    if (CancelNextEvent)
                    {
                        CancelNextEvent = false;
                    }
                    else
                    {
                        CancelNextEvent = true;
                        ProcessingStateToggleSwitch.IsOn = ApplicationData.Settings.HotkeyInformation.Enabled;
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
            if (ApplicationData.Languages.LanguagesWindow == null)
            {
                throw new Exception("Languages value is null. - HotkeyPage.SetTextOnControls()");
            }

            RenewalHotkeyListBox();
            AddLabel.Content = ApplicationData.Languages.LanguagesWindow.Add;
            ModifyLabel.Content = ApplicationData.Languages.LanguagesWindow.Modify;
            DeleteLabel.Content = ApplicationData.Languages.LanguagesWindow.Delete;
            MoveUpLabel.Content = ApplicationData.Languages.LanguagesWindow.MoveUp;
            MoveDownLabel.Content = ApplicationData.Languages.LanguagesWindow.MoveDown;
            SelectWindowTargetLabel.Content = ApplicationData.Languages.LanguagesWindow.Select;
            SelectWindowTargetLabel.ToolTip = ApplicationData.Languages.LanguagesWindow.HoldDownMouseCursorMoveToSelectWindow;
            SettingsLabel.Content = ApplicationData.Languages.LanguagesWindow.Setting;

            ProcessingStateToggleSwitch.OffContent = ProcessingStateToggleSwitch.OnContent = ApplicationData.Languages.LanguagesWindow.ProcessingState;
            DoNotChangeOutOfScreenToggleSwitch.OffContent = DoNotChangeOutOfScreenToggleSwitch.OnContent = ApplicationData.Languages.LanguagesWindow.DoNotChangePositionSizeOutOfScreen;
            StopProcessingFullScreenToggleSwitch.OffContent = StopProcessingFullScreenToggleSwitch.OnContent = ApplicationData.Languages.LanguagesWindow.StopProcessingWhenWindowIsFullScreen;

            ExplanationTextBlock.Text = ApplicationData.Languages.LanguagesWindow.HotkeyExplanation;
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「ホットキー」ListBoxの項目を更新
    /// </summary>
    private void RenewalHotkeyListBox()
    {
        int selectedIndex = HotkeyListBox.SelectedIndex;       // 選択している項目のインデックス

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
            HotkeyListBox.SelectedIndex = selectedIndex;
        }
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
            if (ApplicationData.Languages.LanguagesWindow == null)
            {
                throw new Exception("Languages value is null. - HotkeyPage.GetStringHotkeyInformation()");
            }

            stringData += hotkeyItemInformation.RegisteredName + Environment.NewLine + "  ";
            switch (hotkeyItemInformation.ProcessingType)
            {
                case HotkeyProcessingType.PositionSize:
                    string separateString = "";        // 区切り文字列

                    stringData += ApplicationData.Languages.LanguagesWindow.SpecifyPositionAndSize + Common.ExplainAndValueSeparateString;
                    switch (hotkeyItemInformation.PositionSize.XType)
                    {
                        case WindowXType.Left:
                            stringData += ApplicationData.Languages.LanguagesWindow.X + Common.TypeAndValueSeparateString + ApplicationData.Languages.LanguagesWindow.LeftEdge;
                            separateString = Common.ValueAndValueSeparateString;
                            break;
                        case WindowXType.Middle:
                            stringData += ApplicationData.Languages.LanguagesWindow.X + Common.TypeAndValueSeparateString + ApplicationData.Languages.LanguagesWindow.Middle;
                            separateString = Common.ValueAndValueSeparateString;
                            break;
                        case WindowXType.Right:
                            stringData += ApplicationData.Languages.LanguagesWindow.X + Common.TypeAndValueSeparateString + ApplicationData.Languages.LanguagesWindow.RightEdge;
                            separateString = Common.ValueAndValueSeparateString;
                            break;
                        case WindowXType.Value:
                            stringData += ApplicationData.Languages.LanguagesWindow.X + Common.TypeAndValueSeparateString + hotkeyItemInformation.PositionSize.X + " " + ((hotkeyItemInformation.PositionSize.XValueType == PositionSizeValueType.Pixel) ? ApplicationData.Languages.LanguagesWindow.Pixel : ApplicationData.Languages.LanguagesWindow.Percent);
                            separateString = Common.ValueAndValueSeparateString;
                            break;
                    }
                    switch (hotkeyItemInformation.PositionSize.YType)
                    {
                        case WindowYType.Top:
                            stringData += separateString + ApplicationData.Languages.LanguagesWindow.Y + Common.TypeAndValueSeparateString + ApplicationData.Languages.LanguagesWindow.TopEdge;
                            separateString = Common.ValueAndValueSeparateString;
                            break;
                        case WindowYType.Middle:
                            stringData += separateString + ApplicationData.Languages.LanguagesWindow.Y + Common.TypeAndValueSeparateString + ApplicationData.Languages.LanguagesWindow.Middle;
                            separateString = Common.ValueAndValueSeparateString;
                            break;
                        case WindowYType.Bottom:
                            stringData += separateString + ApplicationData.Languages.LanguagesWindow.Y + Common.TypeAndValueSeparateString + ApplicationData.Languages.LanguagesWindow.BottomEdge;
                            separateString = Common.ValueAndValueSeparateString;
                            break;
                        case WindowYType.Value:
                            stringData += separateString + ApplicationData.Languages.LanguagesWindow.Y + Common.TypeAndValueSeparateString + hotkeyItemInformation.PositionSize.Y + " " + ((hotkeyItemInformation.PositionSize.YValueType == PositionSizeValueType.Pixel) ? ApplicationData.Languages.LanguagesWindow.Pixel : ApplicationData.Languages.LanguagesWindow.Percent);
                            separateString = Common.ValueAndValueSeparateString;
                            break;
                    }
                    switch (hotkeyItemInformation.PositionSize.WidthType)
                    {
                        case WindowSizeType.Value:
                            stringData += separateString + ApplicationData.Languages.LanguagesWindow.Width + Common.TypeAndValueSeparateString + hotkeyItemInformation.PositionSize.Width + " " + ((hotkeyItemInformation.PositionSize.WidthValueType == PositionSizeValueType.Pixel) ? ApplicationData.Languages.LanguagesWindow.Pixel : ApplicationData.Languages.LanguagesWindow.Percent);
                            separateString = Common.ValueAndValueSeparateString;
                            break;
                    }
                    switch (hotkeyItemInformation.PositionSize.HeightType)
                    {
                        case WindowSizeType.Value:
                            stringData += separateString + ApplicationData.Languages.LanguagesWindow.Height + Common.TypeAndValueSeparateString + hotkeyItemInformation.PositionSize.Height + " " + ((hotkeyItemInformation.PositionSize.HeightValueType == PositionSizeValueType.Pixel) ? ApplicationData.Languages.LanguagesWindow.Pixel : ApplicationData.Languages.LanguagesWindow.Percent);
                            break;
                    }
                    break;
                case HotkeyProcessingType.MoveX:
                    stringData += ApplicationData.Languages.LanguagesWindow.MoveXCoordinate + Common.ExplainAndValueSeparateString + hotkeyItemInformation.ProcessingValue + " " + ApplicationData.Languages.LanguagesWindow.Pixel;
                    break;
                case HotkeyProcessingType.MoveY:
                    stringData += ApplicationData.Languages.LanguagesWindow.MoveYCoordinate + Common.ExplainAndValueSeparateString + hotkeyItemInformation.ProcessingValue + " " + ApplicationData.Languages.LanguagesWindow.Pixel;
                    break;
                case HotkeyProcessingType.IncreaseDecreaseWidth:
                    stringData += ApplicationData.Languages.LanguagesWindow.IncreaseDecreaseWidth + Common.ExplainAndValueSeparateString + hotkeyItemInformation.ProcessingValue + " " +ApplicationData.Languages.LanguagesWindow.Pixel;
                    break;
                case HotkeyProcessingType.IncreaseDecreaseHeight:
                    stringData += ApplicationData.Languages.LanguagesWindow.IncreaseDecreaseHeight + Common.ExplainAndValueSeparateString + hotkeyItemInformation.ProcessingValue + " " + ApplicationData.Languages.LanguagesWindow.Pixel;
                    break;
                case HotkeyProcessingType.IncreaseDecreaseWidthHeight:
                    stringData += ApplicationData.Languages.LanguagesWindow.IncreaseDecreaseWidthAndHeight + Common.ExplainAndValueSeparateString + hotkeyItemInformation.ProcessingValue + " " + ApplicationData.Languages.LanguagesWindow.Pixel;
                    break;
                case HotkeyProcessingType.StartStopSpecifyWindow:
                    stringData += ApplicationData.Languages.LanguagesWindow.StartStopProcessingOfSpecifyWindow;
                    break;
                case HotkeyProcessingType.StartStopAllWindow:
                    stringData += ApplicationData.Languages.LanguagesWindow.StartStopProcessingOfAllWindow;
                    break;
                case HotkeyProcessingType.StartStopMagnet:
                    stringData += ApplicationData.Languages.LanguagesWindow.StartStopProcessingOfMagnet;
                    break;
                case HotkeyProcessingType.AlwaysForefrontOrCancel:
                    stringData += ApplicationData.Languages.LanguagesWindow.AlwaysShowOrCancelOnTop;
                    break;
                case HotkeyProcessingType.SpecifyTransparencyOrCancel:
                    stringData += ApplicationData.Languages.LanguagesWindow.SpecifyCancelTransparency + Common.ExplainAndValueSeparateString + hotkeyItemInformation.ProcessingValue;
                    break;
                case HotkeyProcessingType.BatchSpecifyWindow:
                    stringData += ApplicationData.Languages.LanguagesWindow.BatchProcessingOfSpecifyWindow;
                    break;
                case HotkeyProcessingType.OnlyActiveWindowSpecifyWindow:
                    stringData += ApplicationData.Languages.LanguagesWindow.OnlyActiveWindowSpecifyWindow;
                    break;
                case HotkeyProcessingType.ShowThisApplicationWindow:
                    stringData += ApplicationData.Languages.LanguagesWindow.ShowThisApplicationWindow;
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
        if (ApplicationData.Settings.DarkMode)
        {
            AddImage.Source = new BitmapImage(new("/Resources/AdditionWhite.png", UriKind.Relative));
            ModifyImage.Source = new BitmapImage(new("/Resources/ModifyWhite.png", UriKind.Relative));
            DeleteImage.Source = new BitmapImage(new("/Resources/DeleteWhite.png", UriKind.Relative));
            MoveUpImage.Source = new BitmapImage(new("/Resources/UpWhite.png", UriKind.Relative));
            MoveDownImage.Source = new BitmapImage(new("/Resources/DownWhite.png", UriKind.Relative));
            SelectWindowTargetImage.Source = new BitmapImage(new("/Resources/TargetWhite.png", UriKind.Relative));
            SettingsImage.Source = new BitmapImage(new((int)SettingsRowDefinition.Height.Value == Common.SettingsRowDefinitionMinimize ? "/Resources/SettingsWhite.png" : "/Resources/CloseSettingsWhite.png", UriKind.Relative));
        }
        else
        {
            AddImage.Source = new BitmapImage(new("/Resources/AdditionDark.png", UriKind.Relative));
            ModifyImage.Source = new BitmapImage(new("/Resources/ModifyDark.png", UriKind.Relative));
            DeleteImage.Source = new BitmapImage(new("/Resources/DeleteDark.png", UriKind.Relative));
            MoveUpImage.Source = new BitmapImage(new("/Resources/UpDark.png", UriKind.Relative));
            MoveDownImage.Source = new BitmapImage(new("/Resources/DownDark.png", UriKind.Relative));
            SelectWindowTargetImage.Source = new BitmapImage(new("/Resources/TargetDark.png", UriKind.Relative));
            SettingsImage.Source = new BitmapImage(new((int)SettingsRowDefinition.Height.Value == Common.SettingsRowDefinitionMinimize ? "/Resources/SettingsDark.png" : "/Resources/CloseSettingsDark.png", UriKind.Relative));
        }
    }
}
