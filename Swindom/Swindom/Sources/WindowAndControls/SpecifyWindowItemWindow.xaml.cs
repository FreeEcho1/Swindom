namespace Swindom;

/// <summary>
/// 「指定ウィンドウ」の「追加/修正」ウィンドウ
/// </summary>
public partial class SpecifyWindowItemWindow : Window
{
    /// <summary>
    /// 追加/修正したかの値 (いいえ「false」/はい「true」)
    /// </summary>
    public bool AddedOrModified { get; private set; }
    /// <summary>
    /// 修正する項目のインデックス (追加「-1」)
    /// </summary>
    private readonly int IndexOfItemToBeModified = -1;
    /// <summary>
    /// ウィンドウ情報取得の待ち時間
    /// </summary>
    public const int WaitTimeForWindowInformationAcquisition = 5000;
    /// <summary>
    /// 「指定ウィンドウ」機能の項目情報
    /// </summary>
    private readonly SpecifyWindowItemInformation SpecifyWindowItemInformation;
    /// <summary>
    /// ホットキー情報
    /// </summary>
    private readonly FreeEcho.FEHotKeyWPF.HotKeyInformation HotkeyInformation = new();
    /// <summary>
    /// ウィンドウ情報取得タイマー
    /// </summary>
    private readonly System.Windows.Threading.DispatcherTimer WindowInformationAcquisitionTimer = new()
    {
        Interval = new (0, 0, 0, 0, WaitTimeForWindowInformationAcquisition)
    };
    /// <summary>
    /// ウィンドウ選択枠
    /// </summary>
    private readonly FreeEcho.FEWindowSelectionMouse.WindowSelectionFrame WindowSelectionMouse = new()
    {
        MouseLeftUpStop = true
    };
    /// <summary>
    /// 最小化前のオーナーウィンドウの状態
    /// </summary>
    private WindowState PreviousOwnerWindowState = WindowState.Normal;
    /// <summary>
    /// ウィンドウ情報のバッファ
    /// </summary>
    private readonly WindowInformationBuffer WindowInformationBuffer = new();

    /// <summary>
    /// コンストラクタ (使用しない)
    /// </summary>
    public SpecifyWindowItemWindow()
    {
        throw new Exception("Do not use. - SpecifyWindowItemWindow()");
    }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="ownerWindow">オーナーウィンドウ</param>
    /// <param name="indexOfItemToBeModified">修正する項目のインデックス (追加「-1」)</param>
    public SpecifyWindowItemWindow(
        Window? ownerWindow,
        int indexOfItemToBeModified = -1
        )
    {
        if (ApplicationData.Languages.LanguagesWindow == null)
        {
            throw new Exception("Languages value is null. - SpecifyWindowItemWindow()");
        }

        InitializeComponent();

        IndexOfItemToBeModified = indexOfItemToBeModified;
        SpecifyWindowItemInformation = (IndexOfItemToBeModified == -1) ? new() : new(ApplicationData.Settings.SpecifyWindowInformation.Items[IndexOfItemToBeModified]);

        if (ownerWindow != null)
        {
            Owner = ownerWindow;
        }
        if (ApplicationData.Settings.SpecifyWindowInformation.MultipleRegistrations == false)
        {
            ProcessNameLabel.Visibility = Visibility.Collapsed;
            ProcessNameTextBox.Visibility = Visibility.Collapsed;
            WindowProcessingAddModifyStackPanel.Visibility = Visibility.Collapsed;
            WindowProcessingListBox.Visibility = Visibility.Collapsed;
            WindowProcessingCopyDeleteStackPanel.Visibility = Visibility.Collapsed;
        }
        if ((ApplicationData.Settings.CoordinateType == CoordinateType.PrimaryDisplay)
            || (ApplicationData.MonitorInformation.MonitorInfo.Count == 1))
        {
            DisplayCheckBox.Visibility = Visibility.Collapsed;
            StandardDisplayLabel.Visibility = Visibility.Collapsed;
            StandardDisplayComboBox.Visibility = Visibility.Collapsed;
            DisplayLabel.Visibility = Visibility.Collapsed;
            DisplayComboBox.Visibility = Visibility.Collapsed;
        }
        SizeToContent = SizeToContent.Manual;
        Width = (ApplicationData.Settings.SpecifyWindowInformation.AddModifyWindowSize.Width < MinWidth) ? MinWidth + 50 : ApplicationData.Settings.SpecifyWindowInformation.AddModifyWindowSize.Width;
        Height = (ApplicationData.Settings.SpecifyWindowInformation.AddModifyWindowSize.Height < MinHeight) ? MinHeight + 200 : ApplicationData.Settings.SpecifyWindowInformation.AddModifyWindowSize.Height;
        foreach (MonitorInfoEx nowMonitorInfo in ApplicationData.MonitorInformation.MonitorInfo)
        {
            ComboBoxItem newItem = new()
            {
                Content = nowMonitorInfo.DeviceName
            };
            DisplayComboBox.Items.Add(newItem);
        }
        DisplayComboBox.SelectedIndex = 0;
        if (ApplicationData.Settings.DarkMode)
        {
            TargetImage.Source = new BitmapImage(new("/Resources/TargetWhite.png", UriKind.Relative));
        }
        TransparencyNumberBox.Minimum = WindowProcessingInformation.MinimumTransparency;
        TransparencyNumberBox.Maximum = WindowProcessingInformation.MaximumTransparency;

        if (IndexOfItemToBeModified == -1)
        {
            Title = ApplicationData.Languages.LanguagesWindow.Add;
            AddOrModifyButton.Content = ApplicationData.Languages.LanguagesWindow.Add;
        }
        else
        {
            Title = ApplicationData.Languages.LanguagesWindow.Modify;
            AddOrModifyButton.Content = ApplicationData.Languages.LanguagesWindow.Modify;
        }
        RegisteredNameLabel.Content = ApplicationData.Languages.LanguagesWindow.RegisteredName;
        InformationToBeObtainedTabItem.Header = ApplicationData.Languages.LanguagesWindow.InformationToBeObtained;
        TitleNameCheckBox.Content = ApplicationData.Languages.LanguagesWindow.TitleName;
        ClassNameCheckBox.Content = ApplicationData.Languages.LanguagesWindow.ClassName;
        FileNameCheckBox.Content = ApplicationData.Languages.LanguagesWindow.FileName;
        DisplayCheckBox.Content = ApplicationData.Languages.LanguagesWindow.Display;
        WindowStateCheckBox.Content = ApplicationData.Languages.LanguagesWindow.WindowState;
        XCheckBox.Content = ApplicationData.Languages.LanguagesWindow.X;
        YCheckBox.Content = ApplicationData.Languages.LanguagesWindow.Y;
        WidthCheckBox.Content = ApplicationData.Languages.LanguagesWindow.Width;
        HeightCheckBox.Content = ApplicationData.Languages.LanguagesWindow.Height;
        VersionCheckBox.Content = ApplicationData.Languages.LanguagesWindow.Version;
        GetWindowInformationButton.Content = ApplicationData.Languages.LanguagesWindow.GetWindowInformation;
        TargetButton.ToolTip = ApplicationData.Languages.LanguagesWindow.HoldDownMouseCursorMoveToSelectWindow;
        WindowJudgmentTabItem.Header = ApplicationData.Languages.LanguagesWindow.WindowDecide;
        WindowDesignateMethodButton.Content = ApplicationData.Languages.LanguagesWindow.WindowDesignateMethod;
        TitleNameLabel.Content = ApplicationData.Languages.LanguagesWindow.TitleName;
        ((ComboBoxItem)TitleNameMatchingConditionComboBox.Items[0]).Content = ApplicationData.Languages.LanguagesWindow.ExactMatch;
        ((ComboBoxItem)TitleNameMatchingConditionComboBox.Items[1]).Content = ApplicationData.Languages.LanguagesWindow.PartialMatch;
        ((ComboBoxItem)TitleNameMatchingConditionComboBox.Items[2]).Content = ApplicationData.Languages.LanguagesWindow.ForwardMatch;
        ((ComboBoxItem)TitleNameMatchingConditionComboBox.Items[3]).Content = ApplicationData.Languages.LanguagesWindow.BackwardMatch;
        ClassNameLabel.Content = ApplicationData.Languages.LanguagesWindow.ClassName;
        ((ComboBoxItem)ClassNameMatchingConditionComboBox.Items[0]).Content = ApplicationData.Languages.LanguagesWindow.ExactMatch;
        ((ComboBoxItem)ClassNameMatchingConditionComboBox.Items[1]).Content = ApplicationData.Languages.LanguagesWindow.PartialMatch;
        ((ComboBoxItem)ClassNameMatchingConditionComboBox.Items[2]).Content = ApplicationData.Languages.LanguagesWindow.ForwardMatch;
        ((ComboBoxItem)ClassNameMatchingConditionComboBox.Items[3]).Content = ApplicationData.Languages.LanguagesWindow.BackwardMatch;
        FileNameLabel.Content = ApplicationData.Languages.LanguagesWindow.FileName;
        FileNameFileSelectionButton.ToolTip = ApplicationData.Languages.LanguagesWindow.FileSelection;
        ((ComboBoxItem)FileNameMatchingConditionComboBox.Items[0]).Content = ApplicationData.Languages.LanguagesWindow.IncludePath;
        ((ComboBoxItem)FileNameMatchingConditionComboBox.Items[1]).Content = ApplicationData.Languages.LanguagesWindow.NotIncludePath;
        ProcessingJudgmentTabItem.Header = ApplicationData.Languages.LanguagesWindow.ProcessingSettings;
        StandardDisplayLabel.Content = ApplicationData.Languages.LanguagesWindow.DisplayToUseAsStandard;
        ((ComboBoxItem)StandardDisplayComboBox.Items[0]).Content = ApplicationData.Languages.LanguagesWindow.CurrentDisplay;
        ((ComboBoxItem)StandardDisplayComboBox.Items[1]).Content = ApplicationData.Languages.LanguagesWindow.SpecifiedDisplay;
        ((ComboBoxItem)StandardDisplayComboBox.Items[2]).Content = ApplicationData.Languages.LanguagesWindow.LimitedToSpecifiedDisplay;
        OneTimeProcessingLabel.Content = ApplicationData.Languages.LanguagesWindow.ProcessOnlyOnce;
        ((ComboBoxItem)OneTimeProcessingComboBox.Items[0]).Content = ApplicationData.Languages.LanguagesWindow.DoNotSpecify;
        ((ComboBoxItem)OneTimeProcessingComboBox.Items[1]).Content = ApplicationData.Languages.LanguagesWindow.OnceWindowOpen;
        ((ComboBoxItem)OneTimeProcessingComboBox.Items[2]).Content = ApplicationData.Languages.LanguagesWindow.OnceWhileItIsRunning;
        ProcessingJudgmentEventGroupBox.Header = ApplicationData.Languages.LanguagesWindow.Event;
        ForegroundToggleSwitch.OffContent = ForegroundToggleSwitch.OnContent = ApplicationData.Languages.LanguagesWindow.Foregrounded;
        MoveSizeEndToggleSwitch.OffContent = MoveSizeEndToggleSwitch.OnContent = ApplicationData.Languages.LanguagesWindow.MoveSizeChangeEnd;
        MinimizeStartToggleSwitch.OffContent = MinimizeStartToggleSwitch.OnContent = ApplicationData.Languages.LanguagesWindow.MinimizeStart;
        MinimizeEndToggleSwitch.OffContent = MinimizeEndToggleSwitch.OnContent = ApplicationData.Languages.LanguagesWindow.MinimizeEnd;
        ShowToggleSwitch.OffContent = ShowToggleSwitch.OnContent = ApplicationData.Languages.LanguagesWindow.Show;
        NameChangeToggleSwitch.OffContent = NameChangeToggleSwitch.OnContent = ApplicationData.Languages.LanguagesWindow.TitleNameChanged;
        ProcessingJudgmentTimerGroupBox.Header = ApplicationData.Languages.LanguagesWindow.Timer;
        TimerProcessingToggleSwitch.OffContent = TimerProcessingToggleSwitch.OnContent = ApplicationData.Languages.LanguagesWindow.TimerProcessing;
        DelayLabel.Content = ApplicationData.Languages.LanguagesWindow.NumberOfTimesNotToProcessingFirst;
        WindowProcessingTabItem.Header = ApplicationData.Languages.LanguagesWindow.SpecifyWindow;
        ProcessNameLabel.Content = ApplicationData.Languages.LanguagesWindow.ProcessingName;
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
        NormalWindowOnlyToggleSwitch.OffContent = NormalWindowOnlyToggleSwitch.OnContent = ApplicationData.Languages.LanguagesWindow.ProcessOnlyWhenNormalWindow;
        ClientAreaToggleSwitch.OffContent = ClientAreaToggleSwitch.OnContent = ApplicationData.Languages.LanguagesWindow.ClientArea;
        ForefrontLabel.Content = ApplicationData.Languages.LanguagesWindow.Forefront;
        ((ComboBoxItem)ForefrontComboBox.Items[0]).Content = ApplicationData.Languages.LanguagesWindow.DoNotChange;
        ((ComboBoxItem)ForefrontComboBox.Items[1]).Content = ApplicationData.Languages.LanguagesWindow.AlwaysForefront;
        ((ComboBoxItem)ForefrontComboBox.Items[2]).Content = ApplicationData.Languages.LanguagesWindow.AlwaysCancelForefront;
        ((ComboBoxItem)ForefrontComboBox.Items[3]).Content = ApplicationData.Languages.LanguagesWindow.Forefront;
        SpecifyTransparencyToggleSwitch.OffContent = SpecifyTransparencyToggleSwitch.OnContent = ApplicationData.Languages.LanguagesWindow.SpecifyTransparency;
        CloseWindowToggleSwitch.OffContent = CloseWindowToggleSwitch.OnContent = ApplicationData.Languages.LanguagesWindow.CloseWindow;
        HotkeyLabel.Content = ApplicationData.Languages.LanguagesWindow.Hotkey;
        AddProcessingButton.Content = ApplicationData.Languages.LanguagesWindow.Add;
        ModifyProcessingButton.Content = ApplicationData.Languages.LanguagesWindow.Modify;
        ConditionNotToProcessTabItem.Header = ApplicationData.Languages.LanguagesWindow.ConditionsThatDoNotProcess;
        TitleNameRequirementsLabel.Content = ApplicationData.Languages.LanguagesWindow.TitleNameRequirements;
        ((ComboBoxItem)TitleNameRequirementsComboBox.Items[0]).Content = ApplicationData.Languages.LanguagesWindow.DoNotSpecify;
        ((ComboBoxItem)TitleNameRequirementsComboBox.Items[1]).Content = ApplicationData.Languages.LanguagesWindow.WindowWithoutTitleName;
        ((ComboBoxItem)TitleNameRequirementsComboBox.Items[2]).Content = ApplicationData.Languages.LanguagesWindow.WindowWithTitleName;
        TitleNameExclusionStringGroupBox.Header = ApplicationData.Languages.LanguagesWindow.TitleNameExclusionString;
        AddTitleNameExclusionStringButton.Content = ApplicationData.Languages.LanguagesWindow.Add;
        DeleteTitleNameExclusionStringButton.Content = ApplicationData.Languages.LanguagesWindow.Delete;
        ExclusionSizeGroupBox.Header = ApplicationData.Languages.LanguagesWindow.Size;
        DoNotProcessingSizeWidthLabel.Content = ApplicationData.Languages.LanguagesWindow.Width;
        DoNotProcessingSizeHeightLabel.Content = ApplicationData.Languages.LanguagesWindow.Height;
        DoNotProcessingSizeAddButton.Content = ApplicationData.Languages.LanguagesWindow.Add;
        DoNotProcessingSizeDeleteButton.Content = ApplicationData.Languages.LanguagesWindow.Delete;
        VersionGroupBox.Header = ApplicationData.Languages.LanguagesWindow.Version;
        OtherThanSpecifiedVersionLabel.Content = ApplicationData.Languages.LanguagesWindow.OtherThanSpecifiedVersion;
        VersionAnnounceToggleSwitch.OffContent = VersionAnnounceToggleSwitch.OnContent = ApplicationData.Languages.LanguagesWindow.Announce;
        CopyProcessingButton.Content = ApplicationData.Languages.LanguagesWindow.Copy;
        DeleteProcessingButton.Content = ApplicationData.Languages.LanguagesWindow.Delete;
        CancelButton.Content = ApplicationData.Languages.LanguagesWindow.Cancel;

        string stringData;     // 文字列データ
        RegisteredNameTextBox.Text = SpecifyWindowItemInformation.RegisteredName;
        TitleNameCheckBox.IsChecked = ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.TitleName;
        ClassNameCheckBox.IsChecked = ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.ClassName;
        FileNameCheckBox.IsChecked = ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.FileName;
        DisplayCheckBox.IsChecked = ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.Display;
        WindowStateCheckBox.IsChecked = ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.WindowState;
        XCheckBox.IsChecked = ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.X;
        YCheckBox.IsChecked = ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.Y;
        WidthCheckBox.IsChecked = ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.Width;
        HeightCheckBox.IsChecked = ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.Height;
        VersionCheckBox.IsChecked= ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.Version;
        TitleNameTextBox.Text = SpecifyWindowItemInformation.TitleName;
        stringData = SpecifyWindowItemInformation.TitleNameMatchCondition switch
        {
            NameMatchCondition.PartialMatch => ApplicationData.Languages.LanguagesWindow.PartialMatch,
            NameMatchCondition.ForwardMatch => ApplicationData.Languages.LanguagesWindow.ForwardMatch,
            NameMatchCondition.BackwardMatch => ApplicationData.Languages.LanguagesWindow.BackwardMatch,
            _ => ApplicationData.Languages.LanguagesWindow.ExactMatch
        };
        VariousProcessing.SelectComboBoxItem(TitleNameMatchingConditionComboBox, stringData);
        ClassNameTextBox.Text = SpecifyWindowItemInformation.ClassName;
        stringData = SpecifyWindowItemInformation.ClassNameMatchCondition switch
        {
            NameMatchCondition.PartialMatch => ApplicationData.Languages.LanguagesWindow.PartialMatch,
            NameMatchCondition.ForwardMatch => ApplicationData.Languages.LanguagesWindow.ForwardMatch,
            NameMatchCondition.BackwardMatch => ApplicationData.Languages.LanguagesWindow.BackwardMatch,
            _ => ApplicationData.Languages.LanguagesWindow.ExactMatch
        };
        VariousProcessing.SelectComboBoxItem(ClassNameMatchingConditionComboBox, stringData);
        FileNameTextBox.Text = SpecifyWindowItemInformation.FileName;
        stringData = SpecifyWindowItemInformation.FileNameMatchCondition switch
        {
            FileNameMatchCondition.NotInclude => ApplicationData.Languages.LanguagesWindow.NotIncludePath,
            _ => ApplicationData.Languages.LanguagesWindow.IncludePath
        };
        VariousProcessing.SelectComboBoxItem(FileNameMatchingConditionComboBox, stringData);
        stringData = SpecifyWindowItemInformation.StandardDisplay switch
        {
            StandardDisplay.SpecifiedDisplay => ApplicationData.Languages.LanguagesWindow.SpecifiedDisplay,
            StandardDisplay.ExclusiveSpecifiedDisplay => ApplicationData.Languages.LanguagesWindow.LimitedToSpecifiedDisplay,
            _ => ApplicationData.Languages.LanguagesWindow.CurrentDisplay
        };
        VariousProcessing.SelectComboBoxItem(StandardDisplayComboBox, stringData);
        stringData = SpecifyWindowItemInformation.ProcessingOnlyOnce switch
        {
            ProcessingOnlyOnce.WindowOpen => ApplicationData.Languages.LanguagesWindow.OnceWindowOpen,
            ProcessingOnlyOnce.Running => ApplicationData.Languages.LanguagesWindow.OnceWhileItIsRunning,
            _ => ApplicationData.Languages.LanguagesWindow.DoNotSpecify
        };
        VariousProcessing.SelectComboBoxItem(OneTimeProcessingComboBox, stringData);
        ForegroundToggleSwitch.IsOn = SpecifyWindowItemInformation.WindowEventData.Foreground;
        MoveSizeEndToggleSwitch.IsOn = SpecifyWindowItemInformation.WindowEventData.MoveSizeEnd;
        MinimizeStartToggleSwitch.IsOn = SpecifyWindowItemInformation.WindowEventData.MinimizeStart;
        MinimizeEndToggleSwitch.IsOn = SpecifyWindowItemInformation.WindowEventData.MinimizeEnd;
        ShowToggleSwitch.IsOn = SpecifyWindowItemInformation.WindowEventData.Show;
        NameChangeToggleSwitch.IsOn = SpecifyWindowItemInformation.WindowEventData.NameChange;
        TimerProcessingToggleSwitch.IsOn = SpecifyWindowItemInformation.TimerProcessing;
        DelayNumberBox.Minimum = SpecifyWindowItemInformation.MinimumNumberOfTimesNotToProcessingFirst;
        DelayNumberBox.Maximum = SpecifyWindowItemInformation.MaximumNumberOfTimesNotToProcessingFirst;
        DelayNumberBox.Value = SpecifyWindowItemInformation.NumberOfTimesNotToProcessingFirst;
        stringData = SpecifyWindowItemInformation.DoNotProcessingTitleNameConditions switch
        {
            TitleNameProcessingConditions.NotIncluded => ApplicationData.Languages.LanguagesWindow.WindowWithoutTitleName,
            TitleNameProcessingConditions.Included => ApplicationData.Languages.LanguagesWindow.WindowWithTitleName,
            _ => ApplicationData.Languages.LanguagesWindow.DoNotSpecify
        };
        VariousProcessing.SelectComboBoxItem(TitleNameRequirementsComboBox, stringData);
        foreach (string nowTitleName in SpecifyWindowItemInformation.DoNotProcessingStringContainedInTitleName)
        {
            ListBoxItem newItem = new()
            {
                Content = nowTitleName
            };
            ExclusionTitleNameStringListBox.Items.Add(newItem);
        }
        foreach (System.Drawing.Size nowSize in SpecifyWindowItemInformation.DoNotProcessingSize)
        {
            ListBoxItem newItem = new()
            {
                Content = nowSize.Width + Common.CopySeparateString + nowSize.Height
            };
            ExclusionSizeListBox.Items.Add(newItem);
        }
        OtherThanSpecifiedVersionTextBox.Text = SpecifyWindowItemInformation.DoNotProcessingOtherThanSpecifiedVersion;
        VersionAnnounceToggleSwitch.IsOn = SpecifyWindowItemInformation.DoNotProcessingOtherThanSpecifiedVersionAnnounce;
        foreach (WindowProcessingInformation nowWPI in SpecifyWindowItemInformation.WindowProcessingInformation)
        {
            CheckListBoxItem newItem = new()
            {
                Text = nowWPI.ProcessingName,
                IsChecked = nowWPI.Active
            };
            newItem.CheckStateChanged += WindowProcessingListBox_CheckStateChanged;
            WindowProcessingListBox.Items.Add(newItem);
        }
        int count = 0;
        foreach (WindowProcessingInformation nowWPI in SpecifyWindowItemInformation.WindowProcessingInformation)
        {
            // 最初に見つかったアクティブ状態の項目を選択
            if (nowWPI.Active)
            {
                WindowProcessingListBox.SelectedIndex = count;
                break;
            }
            count++;
        }
        SettingsValueOfSelectedItem();
        AddTitleNameExclusionStringButton.IsEnabled = false;
        DeleteTitleNameExclusionStringButton.IsEnabled = false;
        DoNotProcessingSizeDeleteButton.IsEnabled = false;
        SettingsAddAndModifyButtonEnableState();

        Loaded += AddModifyWindowSpecifiedWindow_Loaded;
        Closed += SpecifiedWindowAddModifyWindow_Closed;
        GetWindowInformationButton.Click += GetWindowInformationButton_Click;
        TargetButton.PreviewMouseDown += TargetButton_PreviewMouseDown;
        WindowDesignateMethodButton.Click += WindowDesignateMethodButton_Click;
        TitleNameTextBox.TextChanged += TitleNameTextBox_TextChanged;
        ClassNameTextBox.TextChanged += ClassNameTextBox_TextChanged;
        FileNameTextBox.TextChanged += FileNameTextBox_TextChanged;
        FileNameFileSelectionButton.Click += FileNameFileSelectionButton_Click;
        ProcessNameTextBox.TextChanged += ProcessNameTextBox_TextChanged;
        CloseWindowToggleSwitch.Toggled += CloseWindowToggleSwitch_Toggled;
        WindowStateComboBox.SelectionChanged += WindowStateComboBox_SelectionChanged;
        XComboBox.SelectionChanged += XComboBox_SelectionChanged;
        XTypeComboBox.SelectionChanged += XTypeComboBox_SelectionChanged;
        YComboBox.SelectionChanged += YComboBox_SelectionChanged;
        YTypeComboBox.SelectionChanged += YTypeComboBox_SelectionChanged;
        WidthComboBox.SelectionChanged += WidthComboBox_SelectionChanged;
        WidthTypeComboBox.SelectionChanged += WidthTypeComboBox_SelectionChanged;
        HeightComboBox.SelectionChanged += HeightComboBox_SelectionChanged;
        HeightTypeComboBox.SelectionChanged += HeightTypeComboBox_SelectionChanged;
        StandardDisplayComboBox.SelectionChanged += DisplayToUseAsStandardComboBox_SelectionChanged;
        SpecifyTransparencyToggleSwitch.Toggled += SpecifyTransparencyToggleSwitch_Toggled;
        HotkeyTextBox.PreviewKeyDown += HotkeyTextBox_PreviewKeyDown;
        HotkeyTextBox.GotFocus += HotkeyTextBox_GotFocus;
        HotkeyTextBox.LostFocus += HotkeyTextBox_LostFocus;
        HotkeyTextBox.TextChanged += HotkeyTextBox_TextChanged;
        ExclusionTitleNameStringListBox.SelectionChanged += ExclusionTitleNameStringListBox_SelectionChanged;
        TitleNameExclusionStringTextBox.TextChanged += TitleNameExclusionStringTextBox_TextChanged;
        AddTitleNameExclusionStringButton.Click += AddTitleNameExclusionStringButton_Click;
        DeleteTitleNameExclusionStringButton.Click += DeleteTitleNameExclusionStringButton_Click;
        ExclusionSizeListBox.SelectionChanged += ExclusionSizeListBox_SelectionChanged;
        DoNotProcessingSizeAddButton.Click += DoNotProcessingSizeAddButton_Click;
        DoNotProcessingSizeDeleteButton.Click += DoNotProcessingSizeDeleteButton_Click;
        AddProcessingButton.Click += AddProcessingButton_Click;
        ModifyProcessingButton.Click += ModifyProcessingButton_Click;
        WindowProcessingListBox.SelectionChanged += WindowProcessingListBox_SelectionChanged;
        DeleteProcessingButton.Click += DeleteProcessingButton_Click;
        CopyProcessingButton.Click += CopyProcessingButton_Click;
        AddOrModifyButton.Click += AddOrModifyButton_Click;
        CancelButton.Click += CancelButton_Click;

        WindowInformationAcquisitionTimer.Tick += WindowInformationAcquisitionTimer_Tick;
        WindowSelectionMouse.MouseLeftButtonUp += WindowSelectionMouse_MouseLeftButtonUp;
    }

    /// <summary>
    /// ウィンドウの「Loaded」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void AddModifyWindowSpecifiedWindow_Loaded(
        object sender,
        RoutedEventArgs e
        )
    {
        try
        {
            if (ApplicationData.Settings.SpecifyWindowInformation.StopProcessingShowAddModifyWindow)
            {
                ApplicationData.EventData.DoProcessingEvent(ProcessingEventType.SpecifyWindowShowWindowPause);
            }
        }
        catch
        {
        }
    }

    /// <summary>
    /// ウィンドウの「Closed」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void SpecifiedWindowAddModifyWindow_Closed(
        object? sender,
        EventArgs e
        )
    {
        try
        {
            WindowInformationAcquisitionTimer.Stop();
            WindowSelectionMouse.Dispose();

            ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.TitleName = TitleNameCheckBox.IsChecked ?? false;
            ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.ClassName = ClassNameCheckBox.IsChecked ?? false;
            ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.FileName = FileNameCheckBox.IsChecked ?? false;
            ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.Display = DisplayCheckBox.IsChecked ?? false;
            ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.WindowState = WindowStateCheckBox.IsChecked ?? false;
            ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.X = XCheckBox.IsChecked ?? false;
            ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.Y = YCheckBox.IsChecked ?? false;
            ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.Width = WidthCheckBox.IsChecked ?? false;
            ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.Height = HeightCheckBox.IsChecked ?? false;
            ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.Version = VersionCheckBox.IsChecked ?? false;
            ApplicationData.Settings.SpecifyWindowInformation.AddModifyWindowSize.Width = (int)Width;
            ApplicationData.Settings.SpecifyWindowInformation.AddModifyWindowSize.Height = (int)Height;

            if (ApplicationData.Settings.SpecifyWindowInformation.StopProcessingShowAddModifyWindow)
            {
                ApplicationData.EventData.DoProcessingEvent(ProcessingEventType.SpecifyWindowShowWindowUnpause);
            }
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「ウィンドウ情報取得」Buttonの「Click」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void GetWindowInformationButton_Click(
        object sender,
        RoutedEventArgs e
        )
    {
        try
        {
            if (FEMessageBox.Show(ApplicationData.Languages.LanguagesWindow?.RetrievedAfterFiveSeconds ?? "", ApplicationData.Languages.Check, MessageBoxButton.OK) == MessageBoxResult.OK)
            {
                GetWindowInformationStackPanel.IsEnabled = false;
                WindowInformationAcquisitionTimer.Start();
            }
        }
        catch
        {
            GetWindowInformationStackPanel.IsEnabled = true;
            FEMessageBox.Show(ApplicationData.Languages.ErrorOccurred, ApplicationData.Languages.Check, MessageBoxButton.OK);
        }
    }

    /// <summary>
    /// 「ウィンドウ情報取得タイマー」の「Tick」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void WindowInformationAcquisitionTimer_Tick(
        object? sender,
        EventArgs e
        )
    {
        try
        {
            WindowInformationAcquisitionTimer.Stop();
            GetInformationFromWindowHandle(NativeMethods.GetForegroundWindow());
            Activate();
            FEMessageBox.Show(ApplicationData.Languages.LanguagesWindow?.SelectionComplete ?? "", ApplicationData.Languages.Check, MessageBoxButton.OK);
        }
        catch
        {
            Activate();
            FEMessageBox.Show(ApplicationData.Languages.ErrorOccurred, ApplicationData.Languages.Check, MessageBoxButton.OK);
        }
        finally
        {
            GetWindowInformationStackPanel.IsEnabled = true;
        }
    }

    /// <summary>
    /// 「マウスでウィンドウ情報取得」Buttonの「PreviewMouseDown」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void TargetButton_PreviewMouseDown(
        object sender,
        MouseButtonEventArgs e
        )
    {
        try
        {
            PreviousOwnerWindowState = Owner.WindowState;
            WindowState = WindowState.Minimized;
            Owner.WindowState = WindowState.Minimized;
            WindowSelectionMouse.StartWindowSelection();
        }
        catch
        {
            FEMessageBox.Show(ApplicationData.Languages.ErrorOccurred, ApplicationData.Languages.Check, MessageBoxButton.OK);
        }
    }

    /// <summary>
    /// 「マウスでウィンドウ情報取得」Buttonの「MouseLeftButtonUp」イベント
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
            GetInformationFromWindowHandle(WindowSelectionMouse.SelectedHwnd);
            Owner.WindowState = PreviousOwnerWindowState;
            WindowState = WindowState.Normal;
            Activate();
            FEMessageBox.Show(ApplicationData.Languages.LanguagesWindow?.SelectionComplete ?? "", ApplicationData.Languages.Check, MessageBoxButton.OK);
        }
        catch
        {
            FEMessageBox.Show(ApplicationData.Languages.ErrorOccurred, ApplicationData.Languages.Check, MessageBoxButton.OK);
        }
    }

    /// <summary>
    /// 「ウィンドウ判定の指定方法」Buttonの「Click」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void WindowDesignateMethodButton_Click(
        object sender,
        RoutedEventArgs e
        )
    {
        try
        {
            ApplicationData.WindowManagement.ShowDesignateMethodWindow(this);
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「タイトル名」TextBoxの「TextChanged」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void TitleNameTextBox_TextChanged(
        object sender,
        TextChangedEventArgs e
        )
    {
        try
        {
            SettingsAddAndModifyButtonEnableState();
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「クラス名」TextBoxの「TextChanged」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ClassNameTextBox_TextChanged(
        object sender,
        TextChangedEventArgs e
        )
    {
        try
        {
            SettingsAddAndModifyButtonEnableState();
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「ファイル名」TextBoxの「TextChanged」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void FileNameTextBox_TextChanged(
        object sender,
        TextChangedEventArgs e
        )
    {
        try
        {
            SettingsAddAndModifyButtonEnableState();
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「ファイル名選択」Buttonの「Click」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void FileNameFileSelectionButton_Click(
        object sender,
        RoutedEventArgs e
        )
    {
        try
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new()
            {
                Title = ApplicationData.Languages.LanguagesWindow?.FileSelection,
                Filter = ".exe|*.exe*",
                Multiselect = false
            };

            if (openFileDialog.ShowDialog() == true)
            {
                FileNameTextBox.Text = (string)((ComboBoxItem)FileNameMatchingConditionComboBox.SelectedItem).Content == ApplicationData.Languages.LanguagesWindow?.NotIncludePath
                    ? Path.GetFileNameWithoutExtension(openFileDialog.FileName)
                    : openFileDialog.FileName;
            }
        }
        catch
        {
            FEMessageBox.Show(ApplicationData.Languages.ErrorOccurred, ApplicationData.Languages.Check, MessageBoxButton.OK);
        }
    }

    /// <summary>
    /// 「処理名」TextBoxの「TextChanged」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ProcessNameTextBox_TextChanged(
        object sender,
        TextChangedEventArgs e
        )
    {
        try
        {
            WindowProcessingAddModifyButtonEnabled();
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「ウィンドウを閉じる」ToggleSwitchの「Toggled」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void CloseWindowToggleSwitch_Toggled(
        object sender,
        RoutedEventArgs e
        )
    {
        try
        {
            DisableIfCloseIsEnabledStackPanel.IsEnabled = CloseWindowToggleSwitch.IsOn == false;
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
            if ((string)((ComboBoxItem)WindowStateComboBox.SelectedItem).Content == ApplicationData.Languages.LanguagesWindow?.NormalWindow)
            {
                SettingsEnabledStateOfXControls();
                SettingsEnabledStateOfYControls();
                SettingsEnabledStateOfWidthControls();
                SettingsEnabledStateOfHeightControls();
            }
            SettingsPositionSizeControlsEnabled();
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
            SettingsEnabledStateOfXControls();
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
    private void XTypeComboBox_SelectionChanged(
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
    /// 「Y」ComboBoxの「SelectionChanged」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void YComboBox_SelectionChanged(
        object sender,
        SelectionChangedEventArgs e
        )
    {
        try
        {
            SettingsEnabledStateOfYControls();
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
                YNumberBox.Maximum = PositionSize.PositionSizePercentMaximum;
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
            SettingsEnabledStateOfWidthControls();
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
            SettingsEnabledStateOfHeightControls();
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
    /// 「基準にするディスプレイ」ComboBoxの「SelectionChanged」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void DisplayToUseAsStandardComboBox_SelectionChanged(
        object sender,
        SelectionChangedEventArgs e
        )
    {
        try
        {
            StandardDisplay standardDisplay = GetSelectedDisplayToUseAsStandard();

            SettingsActiveStateAllWPI(standardDisplay, -1);
            SettingsTheActiveStateOfTheItemsListBox();

            if (standardDisplay == StandardDisplay.CurrentDisplay)
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
    /// 「透明度指定」ToggleSwitchの「Toggled」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void SpecifyTransparencyToggleSwitch_Toggled(
        object sender,
        RoutedEventArgs e
        )
    {
        try
        {
            TransparencyNumberBox.IsEnabled = SpecifyTransparencyToggleSwitch.IsOn;
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
            FreeEcho.FEHotKeyWPF.HotKeyInformation hotkeyInformation = FreeEcho.FEHotKeyWPF.HotKeyWPF.GetHotKey(e, true);        // ホットキー情報
            string hotkeyString = FreeEcho.FEHotKeyWPF.HotKeyWPF.GetHotKeyString(hotkeyInformation);        // ホットキーの文字列
            if (hotkeyString != Common.TabString)
            {
                HotkeyInformation.Copy(hotkeyInformation);
                HotkeyTextBox.Text = hotkeyString;
            }
            e.Handled = true;
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
    /// 「ホットキー」TextBoxの「TextChanged」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <exception cref="NotImplementedException"></exception>
    private void HotkeyTextBox_TextChanged(
        object sender,
        TextChangedEventArgs e
        )
    {
        try
        {
            if (string.IsNullOrEmpty(HotkeyTextBox.Text))
            {
                HotkeyInformation.Copy(new());
            }
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「処理しない条件」の「タイトル名に含まれる文字列」ListBoxの「SelectionChanged」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ExclusionTitleNameStringListBox_SelectionChanged(
        object sender,
        SelectionChangedEventArgs e
        )
    {
        try
        {
            DeleteTitleNameExclusionStringButton.IsEnabled = ExclusionTitleNameStringListBox.SelectedItems.Count != 0;
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「処理しない条件」の「タイトル名に含まれる文字列」TextBoxの「TextChanged」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void TitleNameExclusionStringTextBox_TextChanged(
        object sender,
        TextChangedEventArgs e
        )
    {
        try
        {
            AddTitleNameExclusionStringButton.IsEnabled = !string.IsNullOrEmpty(TitleNameExclusionStringTextBox.Text);
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「処理しない条件」の「タイトル名に含まれる文字列」の「追加」Buttonの「Click」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void AddTitleNameExclusionStringButton_Click(
        object sender,
        RoutedEventArgs e
        )
    {
        try
        {
            // 値を確認
            string? resultString = CheckTitleNameExclusionString(TitleNameExclusionStringTextBox.Text);
            if (string.IsNullOrEmpty(resultString) == false)
            {
                FEMessageBox.Show(resultString, ApplicationData.Languages.Check, MessageBoxButton.OK);
                return;
            }

            SpecifyWindowItemInformation.DoNotProcessingStringContainedInTitleName.Add(TitleNameExclusionStringTextBox.Text);
            ListBoxItem newItem = new()
            {
                Content = TitleNameExclusionStringTextBox.Text
            };      // 新しいListBoxの項目
            ExclusionTitleNameStringListBox.Items.Add(newItem);
            FEMessageBox.Show(ApplicationData.Languages.LanguagesWindow?.Added ?? "", ApplicationData.Languages.Check, MessageBoxButton.OK);
        }
        catch
        {
            FEMessageBox.Show(ApplicationData.Languages.ErrorOccurred, ApplicationData.Languages.Check, MessageBoxButton.OK);
        }
    }

    /// <summary>
    /// 「処理しない条件」の「タイトル名に含まれる文字列」の「削除」Buttonの「Click」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void DeleteTitleNameExclusionStringButton_Click(
        object sender,
        RoutedEventArgs e
        )
    {
        try
        {
            if (FEMessageBox.Show(SpecifyWindowItemInformation.DoNotProcessingStringContainedInTitleName[ExclusionTitleNameStringListBox.SelectedIndex] + Environment.NewLine + ApplicationData.Languages.LanguagesWindow?.AllowDelete, ApplicationData.Languages.Check, MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                SpecifyWindowItemInformation.DoNotProcessingStringContainedInTitleName.RemoveAt(ExclusionTitleNameStringListBox.SelectedIndex);
                ExclusionTitleNameStringListBox.Items.RemoveAt(ExclusionTitleNameStringListBox.SelectedIndex);
                FEMessageBox.Show(ApplicationData.Languages.LanguagesWindow?.Deleted ?? "", ApplicationData.Languages.Check, MessageBoxButton.OK);
            }
        }
        catch
        {
            FEMessageBox.Show(ApplicationData.Languages.ErrorOccurred, ApplicationData.Languages.Check, MessageBoxButton.OK);
        }
    }

    /// <summary>
    /// 「処理しない条件」の「サイズ」ListBoxの「SelectionChanged」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ExclusionSizeListBox_SelectionChanged(
        object sender,
        SelectionChangedEventArgs e
        )
    {
        try
        {
            DoNotProcessingSizeDeleteButton.IsEnabled = ExclusionSizeListBox.SelectedItems.Count != 0;
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「処理しない条件」の「サイズ」の「追加」Buttonの「Click」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void DoNotProcessingSizeAddButton_Click(
        object sender,
        RoutedEventArgs e
        )
    {
        try
        {
            // 値を確認
            System.Drawing.Size size = new((int)DoNotProcessingSizeWidthNumberBox.Value, (int)DoNotProcessingSizeHeightNumberBox.Value);
            string? resultString = CheckDoNotProcessingSize(size);
            if (string.IsNullOrEmpty(resultString) == false)
            {
                FEMessageBox.Show(resultString, ApplicationData.Languages.Check, MessageBoxButton.OK);
                return;
            }

            SpecifyWindowItemInformation.DoNotProcessingSize.Add(new((int)DoNotProcessingSizeWidthNumberBox.Value, (int)DoNotProcessingSizeHeightNumberBox.Value));
            ListBoxItem newItem = new()
            {
                Content = DoNotProcessingSizeWidthNumberBox.Value + Common.CopySeparateString + DoNotProcessingSizeHeightNumberBox.Value
            };      // 新しいListBoxの項目
            ExclusionSizeListBox.Items.Add(newItem);
            FEMessageBox.Show(ApplicationData.Languages.LanguagesWindow?.Added ?? "", ApplicationData.Languages.Check, MessageBoxButton.OK);
        }
        catch
        {
            FEMessageBox.Show(ApplicationData.Languages.ErrorOccurred, ApplicationData.Languages.Check, MessageBoxButton.OK);
        }
    }

    /// <summary>
    /// 「処理しない条件」の「サイズ」の「削除」Buttonの「Click」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void DoNotProcessingSizeDeleteButton_Click(
        object sender,
        RoutedEventArgs e
        )
    {
        try
        {
            if (FEMessageBox.Show(ApplicationData.Languages.LanguagesWindow?.AllowDelete ?? "", ApplicationData.Languages.Check, MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                SpecifyWindowItemInformation.DoNotProcessingSize.RemoveAt(ExclusionSizeListBox.SelectedIndex);
                ExclusionSizeListBox.Items.RemoveAt(ExclusionSizeListBox.SelectedIndex);
                FEMessageBox.Show(ApplicationData.Languages.LanguagesWindow?.Deleted ?? "", ApplicationData.Languages.Check, MessageBoxButton.OK);
            }
        }
        catch
        {
            FEMessageBox.Show(ApplicationData.Languages.ErrorOccurred, ApplicationData.Languages.Check, MessageBoxButton.OK);
        }
    }

    /// <summary>
    /// 「ウィンドウ処理」の「追加」Buttonの「Click」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void AddProcessingButton_Click(
        object sender,
        RoutedEventArgs e
        )
    {
        try
        {
            if (AddWindowProcessingInformation())
            {
                WindowProcessingListBox.SelectedIndex = WindowProcessingListBox.Items.Count - 1;
                SettingsTheActiveStateOfTheItemsListBox();
                FEMessageBox.Show(ApplicationData.Languages.LanguagesWindow?.Added ?? "", ApplicationData.Languages.Check, MessageBoxButton.OK);
            }
        }
        catch
        {
            FEMessageBox.Show(ApplicationData.Languages.ErrorOccurred, ApplicationData.Languages.Check, MessageBoxButton.OK);
        }
    }

    /// <summary>
    /// ウィンドウ処理の「修正」Buttonの「Click」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ModifyProcessingButton_Click(
        object sender,
        RoutedEventArgs e
        )
    {
        try
        {
            if (ModifyWindowProcessingInformation())
            {
                SettingsTheActiveStateOfTheItemsListBox();
                FEMessageBox.Show(ApplicationData.Languages.LanguagesWindow?.Modified ?? "", ApplicationData.Languages.Check, MessageBoxButton.OK);
            }
        }
        catch
        {
            FEMessageBox.Show(ApplicationData.Languages.ErrorOccurred, ApplicationData.Languages.Check, MessageBoxButton.OK);
        }
    }

    /// <summary>
    /// 「ウィンドウ処理」ListBoxの「SelectionChanged」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void WindowProcessingListBox_SelectionChanged(
        object sender,
        SelectionChangedEventArgs e
        )
    {
        try
        {
            SettingsValueOfSelectedItem();
        }
        catch
        {
            FEMessageBox.Show(ApplicationData.Languages.ErrorOccurred, ApplicationData.Languages.Check, MessageBoxButton.OK);
        }
    }

    /// <summary>
    /// 「ウィンドウ処理」ListBoxの項目の「CheckStateChanged」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void WindowProcessingListBox_CheckStateChanged(
        object sender,
        CheckListBoxItemEventArgs e
        )
    {
        try
        {
            int count = 0;
            foreach (CheckListBoxItem nowItem in WindowProcessingListBox.Items)
            {
                if (e.Item == nowItem)
                {
                    SpecifyWindowItemInformation.WindowProcessingInformation[count].Active = ((CheckListBoxItem)WindowProcessingListBox.Items[count]).IsChecked;
                    break;
                }
                count++;
            }
            SettingsActiveStateAllWPI(GetSelectedDisplayToUseAsStandard(), SpecifyWindowItemInformation.WindowProcessingInformation[count].Active ? count : -1);
            SettingsTheActiveStateOfTheItemsListBox();
        }
        catch
        {
            FEMessageBox.Show(ApplicationData.Languages.ErrorOccurred, ApplicationData.Languages.Check, MessageBoxButton.OK);
        }
    }

    /// <summary>
    /// 「ウィンドウ処理」の「削除」Buttonの「Click」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void DeleteProcessingButton_Click(
        object sender,
        RoutedEventArgs e
        )
    {
        try
        {
            SpecifyWindowItemInformation.WindowProcessingInformation.RemoveAt(WindowProcessingListBox.SelectedIndex);
            WindowProcessingListBox.Items.RemoveAt(WindowProcessingListBox.SelectedIndex);

            SettingsActiveStateAllWPI(GetSelectedDisplayToUseAsStandard(), -1);
            SettingsTheActiveStateOfTheItemsListBox();
            WindowProcessingListBox.SelectedIndex = -1;
            FEMessageBox.Show(ApplicationData.Languages.LanguagesWindow?.Deleted ?? "", ApplicationData.Languages.Check, MessageBoxButton.OK);
        }
        catch
        {
            FEMessageBox.Show(ApplicationData.Languages.ErrorOccurred, ApplicationData.Languages.Check, MessageBoxButton.OK);
        }
    }

    /// <summary>
    /// 「ウィンドウ処理」の「コピー」Buttonの「Click」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void CopyProcessingButton_Click(
        object sender,
        RoutedEventArgs e
        )
    {
        try
        {
            WindowProcessingInformation newWPI = new(SpecifyWindowItemInformation.WindowProcessingInformation[WindowProcessingListBox.SelectedIndex], false)
            {
                Active = false
            };
            int number = 1;     // 名前の後ろに付ける番号
            foreach (WindowProcessingInformation nowWPI in SpecifyWindowItemInformation.WindowProcessingInformation)
            {
                if (nowWPI.ProcessingName == newWPI.ProcessingName + Common.CopySeparateString + ApplicationData.Languages.LanguagesWindow?.Copy + Common.SpaceSeparateString + number)
                {
                    number++;
                }
            }
            newWPI.ProcessingName += Common.CopySeparateString + ApplicationData.Languages.LanguagesWindow?.Copy + Common.SpaceSeparateString + number;
            SpecifyWindowItemInformation.WindowProcessingInformation.Add(newWPI);
            CheckListBoxItem newItem = new()
            {
                Text = newWPI.ProcessingName,
                IsChecked = newWPI.Active
            };
            newItem.CheckStateChanged += WindowProcessingListBox_CheckStateChanged;
            WindowProcessingListBox.Items.Add(newItem);
            WindowProcessingListBox.SelectedIndex = WindowProcessingListBox.Items.Count - 1;
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
    private void AddOrModifyButton_Click(
        object sender,
        RoutedEventArgs e
        )
    {
        try
        {
            // 登録名が入力されていない場合は名前を決める
            if (string.IsNullOrEmpty(RegisteredNameTextBox.Text))
            {
                if (string.IsNullOrEmpty(TitleNameTextBox.Text) == false)
                {
                    RegisteredNameTextBox.Text = TitleNameTextBox.Text;
                }
                else if (string.IsNullOrEmpty(ClassNameTextBox.Text) == false)
                {
                    RegisteredNameTextBox.Text = ClassNameTextBox.Text;
                }
                else if (string.IsNullOrEmpty(FileNameTextBox.Text) == false)
                {
                    RegisteredNameTextBox.Text = FileNameTextBox.Text;
                }
            }

            GetValueExceptWindowProcessingInformation();
            string? check = CheckSpecifyWindowItemInformation(IndexOfItemToBeModified);
            if (string.IsNullOrEmpty(check))
            {
                bool result = true;     // 結果

                // 追加
                if (IndexOfItemToBeModified == -1)
                {
                    // 項目がない場合は追加する
                    if (SpecifyWindowItemInformation.WindowProcessingInformation.Count == 0)
                    {
                        if (string.IsNullOrEmpty(ProcessNameTextBox.Text))
                        {
                            ProcessNameTextBox.Text = RegisteredNameTextBox.Text;
                        }
                        result = AddWindowProcessingInformation();
                    }
                    if (result)
                    {
                        ApplicationData.Settings.SpecifyWindowInformation.Items.Add(SpecifyWindowItemInformation);
                        ApplicationData.WindowProcessingManagement.SpecifyWindowProcessing?.ProcessingSettings();
                        FEMessageBox.Show(ApplicationData.Languages.LanguagesWindow?.Added ?? "", ApplicationData.Languages.Check, MessageBoxButton.OK);
                        AddedOrModified = true;
                        Close();
                    }
                }
                // 修正
                else
                {
                    // 項目が選択されている場合は修正する
                    if (WindowProcessingListBox.SelectedItems.Count == 1)
                    {
                        if (string.IsNullOrEmpty(ProcessNameTextBox.Text))
                        {
                            ProcessNameTextBox.Text = RegisteredNameTextBox.Text;
                        }
                        result = ModifyWindowProcessingInformation();
                    }
                    if (result)
                    {
                        ApplicationData.WindowProcessingManagement.SpecifyWindowProcessing?.UnregisterHotkeys();
                        ApplicationData.Settings.SpecifyWindowInformation.Items[IndexOfItemToBeModified] = SpecifyWindowItemInformation;
                        ApplicationData.WindowProcessingManagement.SpecifyWindowProcessing?.ProcessingSettings();
                        FEMessageBox.Show(ApplicationData.Languages.LanguagesWindow?.Modified ?? "", ApplicationData.Languages.Check, MessageBoxButton.OK);
                        AddedOrModified = true;
                        Close();
                    }
                }
            }
            else
            {
                FEMessageBox.Show(check, ApplicationData.Languages.Check, MessageBoxButton.OK);
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
    /// 「追加/修正」ボタンの有効状態を設定
    /// </summary>
    private void SettingsAddAndModifyButtonEnableState()
    {
        AddOrModifyButton.IsEnabled = !string.IsNullOrEmpty(TitleNameTextBox.Text) || !string.IsNullOrEmpty(ClassNameTextBox.Text) || !string.IsNullOrEmpty(FileNameTextBox.Text);
    }

    /// <summary>
    /// 位置とサイズのコントロールの有効状態を設定
    /// </summary>
    private void SettingsPositionSizeControlsEnabled()
    {
        TransparencyNumberBox.IsEnabled = SpecifyTransparencyToggleSwitch.IsOn;
        switch (GetSelectedDisplayToUseAsStandard())
        {
            case StandardDisplay.CurrentDisplay:
                DisplayLabel.IsEnabled = false;
                DisplayComboBox.IsEnabled = false;
                break;
            default:
                DisplayLabel.IsEnabled = true;
                DisplayComboBox.IsEnabled = true;
                break;
        }
        if ((string)((ComboBoxItem)WindowStateComboBox.SelectedItem).Content == ApplicationData.Languages.LanguagesWindow?.NormalWindow)
        {
            XLabel.IsEnabled = true;
            XStackPanel.IsEnabled = true;
            YLabel.IsEnabled = true;
            YStackPanel.IsEnabled = true;
            WidthLabel.IsEnabled = true;
            WidthStackPanel.IsEnabled = true;
            HeightLabel.IsEnabled = true;
            HeightStackPanel.IsEnabled = true;
            ClientAreaToggleSwitch.IsEnabled = true;
        }
        else
        {
            XLabel.IsEnabled = false;
            XStackPanel.IsEnabled = false;
            YLabel.IsEnabled = false;
            YStackPanel.IsEnabled = false;
            WidthLabel.IsEnabled = false;
            WidthStackPanel.IsEnabled = false;
            HeightLabel.IsEnabled = false;
            HeightStackPanel.IsEnabled = false;
            ClientAreaToggleSwitch.IsEnabled = false;
        }
    }

    /// <summary>
    /// 「ウィンドウ処理」の「追加/修正」ボタンの有効状態を設定
    /// </summary>
    private void WindowProcessingAddModifyButtonEnabled()
    {
        if (string.IsNullOrEmpty(ProcessNameTextBox.Text))
        {
            AddProcessingButton.IsEnabled = false;
            ModifyProcessingButton.IsEnabled = false;
        }
        else
        {
            AddProcessingButton.IsEnabled = true;
            if (WindowProcessingListBox.SelectedItems.Count != 0)
            {
                ModifyProcessingButton.IsEnabled = true;
            }
        }
    }

    /// <summary>
    /// コントロールから「WindowProcessingInformation」以外の値を取得
    /// </summary>
    private void GetValueExceptWindowProcessingInformation()
    {
        string stringData;     // 文字列

        SpecifyWindowItemInformation.RegisteredName = RegisteredNameTextBox.Text;
        SpecifyWindowItemInformation.TitleName = TitleNameTextBox.Text;
        stringData = (string)((ComboBoxItem)TitleNameMatchingConditionComboBox.SelectedItem).Content;
        if (stringData == ApplicationData.Languages.LanguagesWindow?.ExactMatch)
        {
            SpecifyWindowItemInformation.TitleNameMatchCondition = NameMatchCondition.ExactMatch;
        }
        else if (stringData == ApplicationData.Languages.LanguagesWindow?.PartialMatch)
        {
            SpecifyWindowItemInformation.TitleNameMatchCondition = NameMatchCondition.PartialMatch;
        }
        else if (stringData == ApplicationData.Languages.LanguagesWindow?.ForwardMatch)
        {
            SpecifyWindowItemInformation.TitleNameMatchCondition = NameMatchCondition.ForwardMatch;
        }
        else if (stringData == ApplicationData.Languages.LanguagesWindow?.BackwardMatch)
        {
            SpecifyWindowItemInformation.TitleNameMatchCondition = NameMatchCondition.BackwardMatch;
        }
        SpecifyWindowItemInformation.ClassName = ClassNameTextBox.Text;
        stringData = (string)((ComboBoxItem)ClassNameMatchingConditionComboBox.SelectedItem).Content;
        if (stringData == ApplicationData.Languages.LanguagesWindow?.ExactMatch)
        {
            SpecifyWindowItemInformation.ClassNameMatchCondition = NameMatchCondition.ExactMatch;
        }
        else if (stringData == ApplicationData.Languages.LanguagesWindow?.PartialMatch)
        {
            SpecifyWindowItemInformation.ClassNameMatchCondition = NameMatchCondition.PartialMatch;
        }
        else if (stringData == ApplicationData.Languages.LanguagesWindow?.ForwardMatch)
        {
            SpecifyWindowItemInformation.ClassNameMatchCondition = NameMatchCondition.ForwardMatch;
        }
        else if (stringData == ApplicationData.Languages.LanguagesWindow?.BackwardMatch)
        {
            SpecifyWindowItemInformation.ClassNameMatchCondition = NameMatchCondition.BackwardMatch;
        }
        SpecifyWindowItemInformation.FileName = FileNameTextBox.Text;
        stringData = (string)((ComboBoxItem)FileNameMatchingConditionComboBox.SelectedItem).Content;
        if (stringData == ApplicationData.Languages.LanguagesWindow?.IncludePath)
        {
            SpecifyWindowItemInformation.FileNameMatchCondition = FileNameMatchCondition.Include;
        }
        else if (stringData == ApplicationData.Languages.LanguagesWindow?.NotIncludePath)
        {
            SpecifyWindowItemInformation.FileNameMatchCondition = FileNameMatchCondition.NotInclude;
        }
        SpecifyWindowItemInformation.StandardDisplay = GetSelectedDisplayToUseAsStandard();
        stringData = (string)((ComboBoxItem)OneTimeProcessingComboBox.SelectedItem).Content;
        if (stringData == ApplicationData.Languages.LanguagesWindow?.DoNotSpecify)
        {
            SpecifyWindowItemInformation.ProcessingOnlyOnce = ProcessingOnlyOnce.NotSpecified;
        }
        else if (stringData == ApplicationData.Languages.LanguagesWindow?.OnceWindowOpen)
        {
            SpecifyWindowItemInformation.ProcessingOnlyOnce = ProcessingOnlyOnce.WindowOpen;
        }
        else if (stringData == ApplicationData.Languages.LanguagesWindow?.OnceWhileItIsRunning)
        {
            SpecifyWindowItemInformation.ProcessingOnlyOnce = ProcessingOnlyOnce.Running;
        }
        SpecifyWindowItemInformation.WindowEventData.Foreground = ForegroundToggleSwitch.IsOn;
        SpecifyWindowItemInformation.WindowEventData.MoveSizeEnd = MoveSizeEndToggleSwitch.IsOn;
        SpecifyWindowItemInformation.WindowEventData.MinimizeStart = MinimizeStartToggleSwitch.IsOn;
        SpecifyWindowItemInformation.WindowEventData.MinimizeEnd = MinimizeEndToggleSwitch.IsOn;
        SpecifyWindowItemInformation.WindowEventData.Show = ShowToggleSwitch.IsOn;
        SpecifyWindowItemInformation.WindowEventData.NameChange = NameChangeToggleSwitch.IsOn;
        SpecifyWindowItemInformation.TimerProcessing = TimerProcessingToggleSwitch.IsOn;
        SpecifyWindowItemInformation.NumberOfTimesNotToProcessingFirst = (int)DelayNumberBox.Value;
        stringData = (string)((ComboBoxItem)TitleNameRequirementsComboBox.SelectedItem).Content;
        if (stringData == ApplicationData.Languages.LanguagesWindow?.DoNotSpecify)
        {
            SpecifyWindowItemInformation.DoNotProcessingTitleNameConditions = TitleNameProcessingConditions.NotSpecified;
        }
        else if (stringData == ApplicationData.Languages.LanguagesWindow?.WindowWithoutTitleName)
        {
            SpecifyWindowItemInformation.DoNotProcessingTitleNameConditions = TitleNameProcessingConditions.NotIncluded;
        }
        else if (stringData == ApplicationData.Languages.LanguagesWindow?.WindowWithTitleName)
        {
            SpecifyWindowItemInformation.DoNotProcessingTitleNameConditions = TitleNameProcessingConditions.Included;
        }
        SpecifyWindowItemInformation.DoNotProcessingOtherThanSpecifiedVersion = OtherThanSpecifiedVersionTextBox.Text;
        SpecifyWindowItemInformation.DoNotProcessingOtherThanSpecifiedVersionAnnounce = VersionAnnounceToggleSwitch.IsOn;
    }

    /// <summary>
    /// 「WindowProcessingInformation」を追加
    /// </summary>
    /// <returns>追加に成功したかの値 (失敗「false」/成功「true」)</returns>
    private bool AddWindowProcessingInformation()
    {
        bool result = false;        // 結果
        WindowProcessingInformation newWPI = GetWindowProcessingInformationFromControls();

        string? check = CheckValueWindowProcessingInformation(newWPI, false);
        if (string.IsNullOrEmpty(check))
        {
            SettingsActiveStateWPI(newWPI);
            SpecifyWindowItemInformation.WindowProcessingInformation.Add(newWPI);
            CheckListBoxItem newItem = new()
            {
                Text = newWPI.ProcessingName,
                IsChecked = newWPI.Active
            };      // 新しいListBoxの項目
            newItem.CheckStateChanged += WindowProcessingListBox_CheckStateChanged;
            WindowProcessingListBox.Items.Add(newItem);

            result = true;
        }
        else
        {
            FEMessageBox.Show(check, ApplicationData.Languages.Check, MessageBoxButton.OK);
        }

        return result;
    }

    /// <summary>
    /// 「WindowProcessingInformation」を修正
    /// </summary>
    /// <returns>修正に成功したかの値 (失敗「false」/成功「true」)</returns>
    private bool ModifyWindowProcessingInformation()
    {
        bool result = false;        // 結果

        if (WindowProcessingListBox.SelectedItems.Count == 1)
        {
            WindowProcessingInformation modifyWPI = GetWindowProcessingInformationFromControls();
            modifyWPI.Active = SpecifyWindowItemInformation.WindowProcessingInformation[WindowProcessingListBox.SelectedIndex].Active;

            string? check = CheckValueWindowProcessingInformation(modifyWPI, true, WindowProcessingListBox.SelectedIndex);
            if (string.IsNullOrEmpty(check))
            {
                SettingsActiveStateWPI(modifyWPI, WindowProcessingListBox.SelectedIndex);
                SpecifyWindowItemInformation.WindowProcessingInformation[WindowProcessingListBox.SelectedIndex].Copy(modifyWPI);
                CheckListBoxItem modifyItem = (CheckListBoxItem)WindowProcessingListBox.Items[WindowProcessingListBox.SelectedIndex];
                modifyItem.Text = modifyWPI.ProcessingName;
                modifyItem.IsChecked = modifyWPI.Active;

                result = true;
            }
            else
            {
                FEMessageBox.Show(check, ApplicationData.Languages.Check, MessageBoxButton.OK);
            }
        }

        return result;
    }

    /// <summary>
    /// コントロールから「WindowProcessingInformation」の値を取得
    /// </summary>
    /// <returns>取得したウィンドウの処理情報</returns>
    private WindowProcessingInformation GetWindowProcessingInformationFromControls()
    {
        if (ApplicationData.Languages.LanguagesWindow == null)
        {
            throw new Exception("Languages value is null. - SpecifyWindowItemWindow.GetWindowProcessingInformationFromControls()");
        }

        WindowProcessingInformation newWPI = new()
        {
            Active = true,
            ProcessingName = ProcessNameTextBox.Text
        };      // 新しい情報

        newWPI.PositionSize.Display = DisplayComboBox.Text;
        string stringData = (string)((ComboBoxItem)WindowStateComboBox.SelectedItem).Content;
        if (stringData == ApplicationData.Languages.LanguagesWindow.DoNotChange)
        {
            newWPI.PositionSize.SettingsWindowState = SettingsWindowState.DoNotChange;
        }
        else if (stringData == ApplicationData.Languages.LanguagesWindow.NormalWindow)
        {
            newWPI.PositionSize.SettingsWindowState = SettingsWindowState.Normal;
        }
        else if (stringData == ApplicationData.Languages.LanguagesWindow.Maximize)
        {
            newWPI.PositionSize.SettingsWindowState = SettingsWindowState.Maximize;
        }
        else if (stringData == ApplicationData.Languages.LanguagesWindow.Minimize)
        {
            newWPI.PositionSize.SettingsWindowState = SettingsWindowState.Minimize;
        }
        stringData = (string)((ComboBoxItem)XComboBox.SelectedItem).Content;
        if (stringData == ApplicationData.Languages.LanguagesWindow.DoNotChange)
        {
            newWPI.PositionSize.XType = WindowXType.DoNotChange;
        }
        else if (stringData == ApplicationData.Languages.LanguagesWindow.LeftEdge)
        {
            newWPI.PositionSize.XType = WindowXType.Left;
        }
        else if (stringData == ApplicationData.Languages.LanguagesWindow.Middle)
        {
            newWPI.PositionSize.XType = WindowXType.Middle;
        }
        else if (stringData == ApplicationData.Languages.LanguagesWindow.RightEdge)
        {
            newWPI.PositionSize.XType = WindowXType.Right;
        }
        else if (stringData == ApplicationData.Languages.LanguagesWindow.CoordinateSpecification)
        {
            newWPI.PositionSize.XType = WindowXType.Value;
        }
        newWPI.PositionSize.X = XNumberBox.Value;
        stringData = (string)((ComboBoxItem)XTypeComboBox.SelectedItem).Content;
        if (stringData == ApplicationData.Languages.LanguagesWindow.Pixel)
        {
            newWPI.PositionSize.XValueType = PositionSizeValueType.Pixel;
        }
        else if (stringData == ApplicationData.Languages.LanguagesWindow.Percent)
        {
            newWPI.PositionSize.XValueType = PositionSizeValueType.Percent;
        }
        stringData = (string)((ComboBoxItem)YComboBox.SelectedItem).Content;
        if (stringData == ApplicationData.Languages.LanguagesWindow.DoNotChange)
        {
            newWPI.PositionSize.YType = WindowYType.DoNotChange;
        }
        else if (stringData == ApplicationData.Languages.LanguagesWindow.TopEdge)
        {
            newWPI.PositionSize.YType = WindowYType.Top;
        }
        else if (stringData == ApplicationData.Languages.LanguagesWindow.Middle)
        {
            newWPI.PositionSize.YType = WindowYType.Middle;
        }
        else if (stringData == ApplicationData.Languages.LanguagesWindow.BottomEdge)
        {
            newWPI.PositionSize.YType = WindowYType.Bottom;
        }
        else if (stringData == ApplicationData.Languages.LanguagesWindow.CoordinateSpecification)
        {
            newWPI.PositionSize.YType = WindowYType.Value;
        }
        newWPI.PositionSize.Y = YNumberBox.Value;
        stringData = (string)((ComboBoxItem)YTypeComboBox.SelectedItem).Content;
        if (stringData == ApplicationData.Languages.LanguagesWindow.Pixel)
        {
            newWPI.PositionSize.YValueType = PositionSizeValueType.Pixel;
        }
        else if (stringData == ApplicationData.Languages.LanguagesWindow.Percent)
        {
            newWPI.PositionSize.YValueType = PositionSizeValueType.Percent;
        }
        stringData = (string)((ComboBoxItem)WidthComboBox.SelectedItem).Content;
        if (stringData == ApplicationData.Languages.LanguagesWindow.DoNotChange)
        {
            newWPI.PositionSize.WidthType = WindowSizeType.DoNotChange;
        }
        else if (stringData == ApplicationData.Languages.LanguagesWindow.WidthSpecification)
        {
            newWPI.PositionSize.WidthType = WindowSizeType.Value;
        }
        newWPI.PositionSize.Width = WidthNumberBox.Value;
        stringData = (string)((ComboBoxItem)WidthTypeComboBox.SelectedItem).Content;
        if (stringData == ApplicationData.Languages.LanguagesWindow.Pixel)
        {
            newWPI.PositionSize.WidthValueType = PositionSizeValueType.Pixel;
        }
        else if (stringData == ApplicationData.Languages.LanguagesWindow.Percent)
        {
            newWPI.PositionSize.WidthValueType = PositionSizeValueType.Percent;
        }
        stringData = (string)((ComboBoxItem)HeightComboBox.SelectedItem).Content;
        if (stringData == ApplicationData.Languages.LanguagesWindow.DoNotChange)
        {
            newWPI.PositionSize.HeightType = WindowSizeType.DoNotChange;
        }
        else if (stringData == ApplicationData.Languages.LanguagesWindow.HeightSpecification)
        {
            newWPI.PositionSize.HeightType = WindowSizeType.Value;
        }
        newWPI.PositionSize.Height = HeightNumberBox.Value;
        stringData = (string)((ComboBoxItem)HeightTypeComboBox.SelectedItem).Content;
        if (stringData == ApplicationData.Languages.LanguagesWindow.Pixel)
        {
            newWPI.PositionSize.HeightValueType = PositionSizeValueType.Pixel;
        }
        else if (stringData == ApplicationData.Languages.LanguagesWindow.Percent)
        {
            newWPI.PositionSize.HeightValueType = PositionSizeValueType.Percent;
        }
        newWPI.NormalWindowOnly = NormalWindowOnlyToggleSwitch.IsOn;
        newWPI.PositionSize.ClientArea = ClientAreaToggleSwitch.IsOn;
        stringData = (string)((ComboBoxItem)ForefrontComboBox.SelectedItem).Content;
        if (stringData == ApplicationData.Languages.LanguagesWindow.DoNotChange)
        {
            newWPI.Forefront = Forefront.DoNotChange;
        }
        else if (stringData == ApplicationData.Languages.LanguagesWindow.AlwaysForefront)
        {
            newWPI.Forefront = Forefront.Always;
        }
        else if (stringData == ApplicationData.Languages.LanguagesWindow.AlwaysCancelForefront)
        {
            newWPI.Forefront = Forefront.Cancel;
        }
        else if (stringData == ApplicationData.Languages.LanguagesWindow.Forefront)
        {
            newWPI.Forefront = Forefront.Forefront;
        }
        newWPI.EnabledTransparency = SpecifyTransparencyToggleSwitch.IsOn;
        newWPI.Transparency = (int)TransparencyNumberBox.Value;
        newWPI.CloseWindow = CloseWindowToggleSwitch.IsOn;
        newWPI.Hotkey.Copy(HotkeyInformation);

        return newWPI;
    }

    /// <summary>
    /// 「ウィンドウ処理」ListBoxの項目のアクティブ状態を設定
    /// </summary>
    private void SettingsTheActiveStateOfTheItemsListBox()
    {
        for (int count = 0; count < WindowProcessingListBox.Items.Count; count++)
        {
            ((CheckListBoxItem)(WindowProcessingListBox.Items[count])).IsChecked = SpecifyWindowItemInformation.WindowProcessingInformation[count].Active;
        }
    }

    /// <summary>
    /// 「ウィンドウ処理」のコントロールにリストボックスで選択している項目の値を設定 (選択していない場合は既定値)
    /// </summary>
    private void SettingsValueOfSelectedItem()
    {
        if (ApplicationData.Languages.LanguagesWindow == null)
        {
            throw new Exception("Languages value is null. - SpecifyWindowItemWindow.SettingsValueOfSelectedItem()");
        }

        WindowProcessingInformation settingsWPI = (WindowProcessingListBox.SelectedItems.Count == 0) ? new() : SpecifyWindowItemInformation.WindowProcessingInformation[WindowProcessingListBox.SelectedIndex];        // 設定する情報

        ProcessNameTextBox.Text = settingsWPI.ProcessingName;
        VariousProcessing.SelectComboBoxItem(DisplayComboBox, settingsWPI.PositionSize.Display);
        string stringData;
        stringData = settingsWPI.PositionSize.SettingsWindowState switch
        {
            SettingsWindowState.Normal => ApplicationData.Languages.LanguagesWindow.NormalWindow,
            SettingsWindowState.Maximize => ApplicationData.Languages.LanguagesWindow.Maximize,
            SettingsWindowState.Minimize => ApplicationData.Languages.LanguagesWindow.Minimize,
            _ => ApplicationData.Languages.LanguagesWindow.DoNotChange
        };
        VariousProcessing.SelectComboBoxItem(WindowStateComboBox, stringData);
        stringData = settingsWPI.PositionSize.XType switch
        {
            WindowXType.Left => ApplicationData.Languages.LanguagesWindow.LeftEdge,
            WindowXType.Middle => ApplicationData.Languages.LanguagesWindow.Middle,
            WindowXType.Right => ApplicationData.Languages.LanguagesWindow.RightEdge,
            WindowXType.Value => ApplicationData.Languages.LanguagesWindow.CoordinateSpecification,
            _ => ApplicationData.Languages.LanguagesWindow.DoNotChange
        };
        VariousProcessing.SelectComboBoxItem(XComboBox, stringData);
        stringData = settingsWPI.PositionSize.XValueType switch
        {
            PositionSizeValueType.Percent => ApplicationData.Languages.LanguagesWindow.Percent,
            _ => ApplicationData.Languages.LanguagesWindow.Pixel
        };
        VariousProcessing.SelectComboBoxItem(XTypeComboBox, stringData);
        XNumberBox.Value = (double)settingsWPI.PositionSize.X;
        SettingsEnabledStateOfXControls();
        stringData = settingsWPI.PositionSize.YType switch
        {
            WindowYType.Top => ApplicationData.Languages.LanguagesWindow.TopEdge,
            WindowYType.Middle => ApplicationData.Languages.LanguagesWindow.Middle,
            WindowYType.Bottom => ApplicationData.Languages.LanguagesWindow.BottomEdge,
            WindowYType.Value => ApplicationData.Languages.LanguagesWindow.CoordinateSpecification,
            _ => ApplicationData.Languages.LanguagesWindow.DoNotChange
        };
        VariousProcessing.SelectComboBoxItem(YComboBox, stringData);
        stringData = settingsWPI.PositionSize.YValueType switch
        {
            PositionSizeValueType.Percent => ApplicationData.Languages.LanguagesWindow.Percent,
            _ => ApplicationData.Languages.LanguagesWindow.Pixel
        };
        VariousProcessing.SelectComboBoxItem(YTypeComboBox, stringData);
        YNumberBox.Value = (double)settingsWPI.PositionSize.Y;
        SettingsEnabledStateOfYControls();
        stringData = settingsWPI.PositionSize.WidthType switch
        {
            WindowSizeType.Value => ApplicationData.Languages.LanguagesWindow.WidthSpecification,
            _ => ApplicationData.Languages.LanguagesWindow.DoNotChange
        };
        VariousProcessing.SelectComboBoxItem(WidthComboBox, stringData);
        stringData = settingsWPI.PositionSize.WidthValueType switch
        {
            PositionSizeValueType.Percent => ApplicationData.Languages.LanguagesWindow.Percent,
            _ => ApplicationData.Languages.LanguagesWindow.Pixel
        };
        VariousProcessing.SelectComboBoxItem(WidthTypeComboBox, stringData);
        WidthNumberBox.Value = settingsWPI.PositionSize.Width;
        SettingsEnabledStateOfWidthControls();
        stringData = settingsWPI.PositionSize.HeightType switch
        {
            WindowSizeType.Value => ApplicationData.Languages.LanguagesWindow.HeightSpecification,
            _ => ApplicationData.Languages.LanguagesWindow.DoNotChange
        };
        VariousProcessing.SelectComboBoxItem(HeightComboBox, stringData);
        stringData = settingsWPI.PositionSize.HeightValueType switch
        {
            PositionSizeValueType.Percent => ApplicationData.Languages.LanguagesWindow.Percent,
            _ => ApplicationData.Languages.LanguagesWindow.Pixel
        };
        VariousProcessing.SelectComboBoxItem(HeightTypeComboBox, stringData);
        HeightNumberBox.Value = settingsWPI.PositionSize.Height;
        SettingsEnabledStateOfHeightControls();
        SettingsPositionSizeControlsEnabled();
        NormalWindowOnlyToggleSwitch.IsOn = settingsWPI.NormalWindowOnly;
        ClientAreaToggleSwitch.IsOn = settingsWPI.PositionSize.ClientArea;
        stringData = settingsWPI.Forefront switch
        {
            Forefront.Always => ApplicationData.Languages.LanguagesWindow.AlwaysForefront,
            Forefront.Cancel => ApplicationData.Languages.LanguagesWindow.AlwaysCancelForefront,
            Forefront.Forefront => ApplicationData.Languages.LanguagesWindow.Forefront,
            _ => ApplicationData.Languages.LanguagesWindow.DoNotChange
        };
        VariousProcessing.SelectComboBoxItem(ForefrontComboBox, stringData);
        SpecifyTransparencyToggleSwitch.IsOn = settingsWPI.EnabledTransparency;
        TransparencyNumberBox.IsEnabled = settingsWPI.EnabledTransparency;
        TransparencyNumberBox.Value = settingsWPI.Transparency;
        CloseWindowToggleSwitch.IsOn = settingsWPI.CloseWindow;
        HotkeyTextBox.Text = FreeEcho.FEHotKeyWPF.HotKeyWPF.GetHotKeyString(settingsWPI.Hotkey);

        WindowProcessingAddModifyButtonEnabled();
        if (WindowProcessingListBox.SelectedItems.Count == 1)
        {
            CopyProcessingButton.IsEnabled = true;
            DeleteProcessingButton.IsEnabled = true;
            HotkeyInformation.Copy(SpecifyWindowItemInformation.WindowProcessingInformation[WindowProcessingListBox.SelectedIndex].Hotkey);
        }
        else
        {
            CopyProcessingButton.IsEnabled = false;
            DeleteProcessingButton.IsEnabled = false;
            HotkeyInformation.Copy(new());
        }
    }

    /// <summary>
    /// ウィンドウハンドルから情報を取得してコントロールに値を設定
    /// </summary>
    /// <param name="hwnd">ウィンドウハンドル</param>
    private void GetInformationFromWindowHandle(
        IntPtr hwnd
        )
    {
        WindowInformation windowInformation = VariousWindowProcessing.GetWindowInformationFromHandle(hwnd, WindowInformationBuffer);
        if (TitleNameCheckBox.IsChecked == true)
        {
            TitleNameTextBox.Text = windowInformation.TitleName;
        }
        if (ClassNameCheckBox.IsChecked == true)
        {
            ClassNameTextBox.Text = windowInformation.ClassName;
        }
        if (FileNameCheckBox.IsChecked == true)
        {
            FileNameTextBox.Text = (string)((ComboBoxItem)FileNameMatchingConditionComboBox.SelectedItem).Content == ApplicationData.Languages.LanguagesWindow?.IncludePath ? windowInformation.FileName : Path.GetFileNameWithoutExtension(windowInformation.FileName);
        }
        if (VersionCheckBox.IsChecked == true)
        {
            OtherThanSpecifiedVersionTextBox.Text = windowInformation.Version;
        }
        GetPositionSizeFromHandle(hwnd);
    }

    /// <summary>
    /// ウィンドウハンドルから位置とサイズを取得してコントロールに値を設定
    /// </summary>
    /// <param name="hwnd">ウィンドウハンドル</param>
    private void GetPositionSizeFromHandle(
        IntPtr hwnd
        )
    {
        try
        {
            if (ApplicationData.Languages.LanguagesWindow == null)
            {
                throw new Exception("Languages value is null. - SpecifyWindowItemWindow.GetPositionSizeFromHandle()");
            }

            NativeMethods.GetWindowPlacement(hwnd, out WINDOWPLACEMENT windowPlacement);
            MonitorInformation.GetMonitorInformationOnWindowShown(new(windowPlacement.rcNormalPosition.Left, windowPlacement.rcNormalPosition.Top, windowPlacement.rcNormalPosition.Right, windowPlacement.rcNormalPosition.Bottom), out MonitorInfoEx monitorInfo);

            if (DisplayCheckBox.IsChecked == true)
            {
                VariousProcessing.SelectComboBoxItem(DisplayComboBox, monitorInfo.DeviceName);
            }
            if (WindowStateCheckBox.IsChecked == true)
            {
                string stringData = windowPlacement.showCmd switch
                {
                    (int)SW.SW_SHOWMAXIMIZED => ApplicationData.Languages.LanguagesWindow.Maximize,
                    (int)SW.SW_SHOWMINIMIZED => ApplicationData.Languages.LanguagesWindow.Minimize,
                    _ => ApplicationData.Languages.LanguagesWindow.NormalWindow,
                };
                VariousProcessing.SelectComboBoxItem(WindowStateComboBox, stringData);
            }
            System.Drawing.Point displayPoint = new(0, 0);     // 基準にするディスプレイの座標
            if (ApplicationData.Settings.CoordinateType == CoordinateType.EachDisplay)
            {
                displayPoint.X = monitorInfo.WorkArea.Left;
                displayPoint.Y = monitorInfo.WorkArea.Top;
            }
            NativeMethods.GetWindowRect(hwnd, out RECT windowRect);       // ウィンドウの上下左右の位置
            if (XCheckBox.IsChecked == true)
            {
                VariousProcessing.SelectComboBoxItem(XComboBox, ApplicationData.Languages.LanguagesWindow.CoordinateSpecification);
                XTypeComboBox.SelectedIndex = 0;
                XNumberBox.Value = windowRect.Left - displayPoint.X;
            }
            if (YCheckBox.IsChecked == true)
            {
                VariousProcessing.SelectComboBoxItem(YComboBox, ApplicationData.Languages.LanguagesWindow.CoordinateSpecification);
                YTypeComboBox.SelectedIndex = 0;
                YNumberBox.Value = windowRect.Top - displayPoint.Y;
            }
            if (WidthCheckBox.IsChecked == true)
            {
                VariousProcessing.SelectComboBoxItem(WidthComboBox, ApplicationData.Languages.LanguagesWindow.WidthSpecification);
                WidthTypeComboBox.SelectedIndex = 0;
                WidthNumberBox.Value = windowRect.Right - windowRect.Left;
            }
            if (HeightCheckBox.IsChecked == true)
            {
                VariousProcessing.SelectComboBoxItem(HeightComboBox, ApplicationData.Languages.LanguagesWindow.HeightSpecification);
                HeightTypeComboBox.SelectedIndex = 0;
                HeightNumberBox.Value = windowRect.Bottom - windowRect.Top;
            }
            SettingsPositionSizeControlsEnabled();
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「X」コントロールの「IsEnabled」を設定
    /// </summary>
    private void SettingsEnabledStateOfXControls()
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
    /// 「Y」コントロールの「IsEnabled」を設定
    /// </summary>
    private void SettingsEnabledStateOfYControls()
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
    /// 「幅」コントロールの「IsEnabled」を設定
    /// </summary>
    private void SettingsEnabledStateOfWidthControls()
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
    /// 「高さ」のコントロールの「IsEnabled」を設定
    /// </summary>
    private void SettingsEnabledStateOfHeightControls()
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
    /// 選択している「基準にするディスプレイ」をコントロールから取得
    /// </summary>
    /// <returns>基準にするディスプレイ</returns>
    private StandardDisplay GetSelectedDisplayToUseAsStandard()
    {
        StandardDisplay selected = StandardDisplay.CurrentDisplay;
        string stringData = (string)((ComboBoxItem)StandardDisplayComboBox.SelectedItem).Content;

        if (stringData == ApplicationData.Languages.LanguagesWindow?.SpecifiedDisplay)
        {
            selected = StandardDisplay.SpecifiedDisplay;
        }
        else if (stringData == ApplicationData.Languages.LanguagesWindow?.LimitedToSpecifiedDisplay)
        {
            selected = StandardDisplay.ExclusiveSpecifiedDisplay;
        }

        return selected;
    }

    /// <summary>
    /// 「WindowProcessingInformation」が複数アクティブにできるか確認
    /// </summary>
    /// <param name="standardDisplay">選択している「StandardDisplay」</param>
    /// <returns>複数アクティブにできるかの値 (いいえ「false」/はい「true」)</returns>
    private static bool CheckMultipleActivationWindowProcessingInformation(
        StandardDisplay standardDisplay
        )
    {
        bool result = false;
        if (standardDisplay == StandardDisplay.ExclusiveSpecifiedDisplay
            && ApplicationData.Settings.CoordinateType == CoordinateType.EachDisplay)
        {
            result = true;
        }
        return result;
    }

    /// <summary>
    /// 「WindowProcessingInformation」の値を確認
    /// </summary>
    /// <param name="wpi">WindowProcessingInformation</param>
    /// <param name="additionOrModify">追加か修正 (追加「false」/修正「true」)</param>
    /// <param name="itemSelectedIndex">選択している項目のインデックス (追加「-1」)</param>
    /// <returns>値に問題ないかの値 (問題ない「null」/問題がある「理由の文字列」)</returns>
    private string? CheckValueWindowProcessingInformation(
        WindowProcessingInformation wpi,
        bool additionOrModify,
        int itemSelectedIndex = -1
        )
    {
        string? result = null;     // 結果

        if (string.IsNullOrEmpty(wpi.ProcessingName))
        {
            result = ApplicationData.Languages.ErrorOccurred;
        }
        else
        {
            // 処理名の重複確認
            if (additionOrModify)
            {
                int count = 0;
                foreach (WindowProcessingInformation nowWPI in SpecifyWindowItemInformation.WindowProcessingInformation)
                {
                    if (count != itemSelectedIndex && nowWPI.ProcessingName == wpi.ProcessingName)
                    {
                        result = ApplicationData.Languages.LanguagesWindow?.ThereIsADuplicateProcessingName;
                        break;
                    }
                    count++;
                }
            }
            else
            {
                foreach (WindowProcessingInformation nowWPI in SpecifyWindowItemInformation.WindowProcessingInformation)
                {
                    if (nowWPI.ProcessingName == wpi.ProcessingName)
                    {
                        result = ApplicationData.Languages.LanguagesWindow?.ThereIsADuplicateProcessingName;
                        break;
                    }
                }
            }
        }

        return result;
    }

    /// <summary>
    /// 「SpecifyWindowItemInformation」の値を確認
    /// </summary>
    /// <param name="indexOfItemToBeModify">修正する項目のインデックス (追加「-1」)</param>
    /// <returns>結果の文字列 (問題ない「null」/問題がある「理由の文字列」)</returns>
    private string? CheckSpecifyWindowItemInformation(
        int indexOfItemToBeModify
        )
    {
        string? result = null;     // 結果

        // 登録名の重複確認
        if (indexOfItemToBeModify == -1)
        {
            foreach (SpecifyWindowItemInformation nowItem in ApplicationData.Settings.SpecifyWindowInformation.Items)
            {
                if (SpecifyWindowItemInformation.RegisteredName == nowItem.RegisteredName)
                {
                    result = ApplicationData.Languages.LanguagesWindow?.ThereIsADuplicateRegistrationName;
                    break;
                }
            }
        }
        else
        {
            int count = 0;
            foreach (SpecifyWindowItemInformation nowItem in ApplicationData.Settings.SpecifyWindowInformation.Items)
            {
                if (count != indexOfItemToBeModify
                    && SpecifyWindowItemInformation.RegisteredName == nowItem.RegisteredName)
                {
                    result = ApplicationData.Languages.LanguagesWindow?.ThereIsADuplicateRegistrationName;
                    break;
                }
                count++;
            }
        }

        return result;
    }

    /// <summary>
    /// 全ての「WindowProcessingInformation」のアクティブ状態を設定
    /// <para>アクティブ項目がない場合は最初の項目を有効化、複数アクティブにできない場合はどちらかの項目を無効化。</para>
    /// </summary>
    /// <param name="standardDisplay">選択している「StandardDisplay」</param>
    /// <param name="activeChangeIndex">アクティブにした項目のインデックス (アクティブにしてない「-1」)</param>
    private void SettingsActiveStateAllWPI(
        StandardDisplay standardDisplay,
        int activeChangeIndex = -1
        )
    {
        // 項目のアクティブ状態を変更
        if (SpecifyWindowItemInformation.WindowProcessingInformation.Count != 0)
        {
            bool activeCheck = false;      // アクティブ項目があるかの値

            // アクティブ項目があるか調べる
            foreach (WindowProcessingInformation nowWPI in SpecifyWindowItemInformation.WindowProcessingInformation)
            {
                if (nowWPI.Active)
                {
                    activeCheck = true;
                    break;
                }
            }

            // アクティブ項目がある場合は重複確認をする
            if (activeCheck)
            {
                // 複数アクティブにできる場合
                if (CheckMultipleActivationWindowProcessingInformation(standardDisplay))
                {
                    if (activeChangeIndex == -1)
                    {
                        for (int count1 = 0; count1 < SpecifyWindowItemInformation.WindowProcessingInformation.Count - 1; count1++)
                        {
                            for (int count2 = count1 + 1; count2 < SpecifyWindowItemInformation.WindowProcessingInformation.Count; count2++)
                            {
                                if (SpecifyWindowItemInformation.WindowProcessingInformation[count1].PositionSize.Display == SpecifyWindowItemInformation.WindowProcessingInformation[count2].PositionSize.Display
                                    && SpecifyWindowItemInformation.WindowProcessingInformation[count1].Active
                                    && SpecifyWindowItemInformation.WindowProcessingInformation[count2].Active)
                                {
                                    SpecifyWindowItemInformation.WindowProcessingInformation[count2].Active = false;
                                }
                            }
                        }
                    }
                    else
                    {
                        int count = 0;      // カウント

                        foreach (WindowProcessingInformation nowWPI in SpecifyWindowItemInformation.WindowProcessingInformation)
                        {
                            if (nowWPI.PositionSize.Display == SpecifyWindowItemInformation.WindowProcessingInformation[activeChangeIndex].PositionSize.Display
                                && count != activeChangeIndex)
                            {
                                nowWPI.Active = false;
                            }
                            count++;
                        }
                    }
                }
                // 1つのみアクティブにできる場合
                else
                {
                    bool check = false;

                    if (activeChangeIndex == -1)
                    {
                        foreach (WindowProcessingInformation nowWPI in SpecifyWindowItemInformation.WindowProcessingInformation)
                        {
                            if (nowWPI.Active)
                            {
                                if (check)
                                {
                                    nowWPI.Active = false;
                                }
                                else
                                {
                                    check = true;
                                }
                            }
                        }
                    }
                    else
                    {
                        for (int count1 = 0; count1 < SpecifyWindowItemInformation.WindowProcessingInformation.Count; count1++)
                        {
                            if (SpecifyWindowItemInformation.WindowProcessingInformation[count1].Active
                                && activeChangeIndex != count1)
                            {
                                SpecifyWindowItemInformation.WindowProcessingInformation[count1].Active = false;
                            }
                        }
                    }
                }
            }
            // アクティブ項目がない場合は最初の項目をアクティブにする
            else
            {
                SpecifyWindowItemInformation.WindowProcessingInformation[0].Active = true;
            }
        }
    }

    /// <summary>
    /// 「WindowProcessingInformation」のアクティブ状態を設定
    /// </summary>
    /// <param name="wpi">WindowProcessingInformation</param>
    /// <param name="modifyItemIndex">修正した項目のインデックス (追加「-1」)</param>
    private void SettingsActiveStateWPI(
        WindowProcessingInformation wpi,
        int modifyItemIndex = -1
        )
    {
        if (CheckMultipleActivationWindowProcessingInformation(GetSelectedDisplayToUseAsStandard()))
        {
            for (int count = 0; count < SpecifyWindowItemInformation.WindowProcessingInformation.Count; count++)
            {
                if (SpecifyWindowItemInformation.WindowProcessingInformation[count].PositionSize.Display == wpi.PositionSize.Display
                    && (modifyItemIndex == -1 || (SpecifyWindowItemInformation.WindowProcessingInformation[count].Active && count != modifyItemIndex)))
                {
                    wpi.Active = false;
                    break;
                }
            }
        }
        else
        {
            for (int count = 0; count < SpecifyWindowItemInformation.WindowProcessingInformation.Count; count++)
            {
                if (SpecifyWindowItemInformation.WindowProcessingInformation[count].Active
                    && (modifyItemIndex == -1 || count != modifyItemIndex))
                {
                    wpi.Active = false;
                    break;
                }
            }
        }
    }

    /// <summary>
    /// 「処理しない条件」の「タイトル名に含まれる文字列」の値を確認
    /// </summary>
    /// <param name="titleName">確認するタイトル名の文字列</param>
    /// <returns>結果の文字列 (問題ない「null」/問題がある「理由の文字列」)</returns>
    private string? CheckTitleNameExclusionString(
        string titleName
        )
    {
        foreach (string nowTitleName in SpecifyWindowItemInformation.DoNotProcessingStringContainedInTitleName)
        {
            if (nowTitleName == titleName)
            {
                return ApplicationData.Languages.LanguagesWindow?.ThereAreDuplicateItems;
            }
        }

        return null;
    }

    /// <summary>
    /// 「処理しない条件」の「サイズ」の値を確認
    /// </summary>
    /// <param name="size">確認するサイズ</param>
    /// <returns>結果の文字列 (問題ない「null」/問題がある「理由の文字列」)</returns>
    private string? CheckDoNotProcessingSize(
        System.Drawing.Size size
        )
    {
        foreach (System.Drawing.Size nowSize in SpecifyWindowItemInformation.DoNotProcessingSize)
        {
            if (nowSize.Width == size.Width
                && nowSize.Height == size.Height)
            {
                return ApplicationData.Languages.LanguagesWindow?.ThereAreDuplicateItems;
            }
        }

        return null;
    }
}
