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
        Interval = new (0, 0, 0, 0, WindowProcessingValue.WaitTimeForWindowInformationAcquisition)
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
    /// コンストラクタ (使用しない)
    /// </summary>
    public SpecifyWindowItemWindow()
    {
        throw new Exception("Do not use. - " + GetType().Name + "()");
    }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="indexOfItemToBeModified">修正する項目のインデックス (追加「-1」)</param>
    public SpecifyWindowItemWindow(
        int indexOfItemToBeModified
        )
    {
        InitializeComponent();

        IndexOfItemToBeModified = indexOfItemToBeModified;
        SpecifyWindowItemInformation = (IndexOfItemToBeModified == -1) ? new() : new(ApplicationData.Settings.SpecifyWindowInformation.Items[IndexOfItemToBeModified]);

        Owner = WindowManagement.GetWindowToSetOwner();
        if (ApplicationData.Settings.SpecifyWindowInformation.MultipleRegistrations == false)
        {
            WindowProcessingProcessNameLabel.Visibility = Visibility.Collapsed;
            WindowProcessingProcessNameTextBox.Visibility = Visibility.Collapsed;
            WindowProcessingAddModifyStackPanel.Visibility = Visibility.Collapsed;
            WindowProcessingListBox.Visibility = Visibility.Collapsed;
            WindowProcessingCopyDeleteStackPanel.Visibility = Visibility.Collapsed;
        }
        if ((ApplicationData.Settings.CoordinateType == CoordinateType.PrimaryDisplay)
            || (ApplicationData.MonitorInformation.MonitorInfo.Count == 1))
        {
            GetInformationDisplayCheckBox.Visibility = Visibility.Collapsed;
            ProcessingSettingsStandardDisplayLabel.Visibility = Visibility.Collapsed;
            ProcessingSettingsStandardDisplayComboBox.Visibility = Visibility.Collapsed;
            WindowProcessingDisplayLabel.Visibility = Visibility.Collapsed;
            WindowProcessingDisplayComboBox.Visibility = Visibility.Collapsed;
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
            WindowProcessingDisplayComboBox.Items.Add(newItem);
        }
        WindowProcessingDisplayComboBox.SelectedIndex = 0;
        TargetImage.Source = ImageProcessing.GetImageTarget();
        WindowProcessingTransparencyNumberBox.Minimum = WindowProcessingInformation.MinimumTransparency;
        WindowProcessingTransparencyNumberBox.Maximum = WindowProcessingInformation.MaximumTransparency;

        if (IndexOfItemToBeModified == -1)
        {
            Title = ApplicationData.Languages.Add;
            AddOrModifyButton.Content = ApplicationData.Languages.Add;
        }
        else
        {
            Title = ApplicationData.Languages.Modify;
            AddOrModifyButton.Content = ApplicationData.Languages.Modify;
        }
        RegisteredNameLabel.Content = ApplicationData.Languages.RegisteredName;
        InformationToBeObtainedTabItem.Header = ApplicationData.Languages.InformationToBeObtained;
        GetInformationTitleNameCheckBox.Content = ApplicationData.Languages.TitleName;
        GetInformationClassNameCheckBox.Content = ApplicationData.Languages.ClassName;
        GetInformationFileNameCheckBox.Content = ApplicationData.Languages.FileName;
        GetInformationDisplayCheckBox.Content = ApplicationData.Languages.Display;
        GetInformationWindowStateCheckBox.Content = ApplicationData.Languages.WindowState;
        GetInformationXCheckBox.Content = ApplicationData.Languages.X;
        GetInformationYCheckBox.Content = ApplicationData.Languages.Y;
        GetInformationWidthCheckBox.Content = ApplicationData.Languages.Width;
        GetInformationHeightCheckBox.Content = ApplicationData.Languages.Height;
        GetInformationVersionCheckBox.Content = ApplicationData.Languages.Version;
        GetInformationVersionLabel.Content = ApplicationData.Languages.Version;
        GetWindowInformationButton.Content = ApplicationData.Languages.GetWindowInformation;
        TargetButton.ToolTip = ApplicationData.Languages.HoldDownMousePointerMoveToSelectWindow;
        WindowDecisionTabItem.Header = ApplicationData.Languages.WindowDecide;
        WindowDecisionExplanationButton.Content = ApplicationData.Languages.Question;
        WindowDecisionExplanationButton.ToolTip = ApplicationData.Languages.Help;
        DecisionTitleNameLabel.Content = ApplicationData.Languages.TitleName;
        DecisionTitleNameMatchingConditionExactMatchComboBoxItem.Content = ApplicationData.Languages.ExactMatch;
        DecisionTitleNameMatchingConditionPartialMatchComboBoxItem.Content = ApplicationData.Languages.PartialMatch;
        DecisionTitleNameMatchingConditionForwardMatchComboBoxItem.Content = ApplicationData.Languages.ForwardMatch;
        DecisionTitleNameMatchingConditionBackwardMatchComboBoxItem.Content = ApplicationData.Languages.BackwardMatch;
        DecisionClassNameLabel.Content = ApplicationData.Languages.ClassName;
        DecisionClassNameMatchingConditionExactMatchComboBoxItem.Content = ApplicationData.Languages.ExactMatch;
        DecisionClassNameMatchingConditionPartialMatchComboBoxItem.Content = ApplicationData.Languages.PartialMatch;
        DecisionClassNameMatchingConditionForwardMatchComboBoxItem.Content = ApplicationData.Languages.ForwardMatch;
        DecisionClassNameMatchingConditionBackwardMatchComboBoxItem.Content = ApplicationData.Languages.BackwardMatch;
        DecisionFileNameLabel.Content = ApplicationData.Languages.FileName;
        DecisionFileNameFileSelectionButton.ToolTip = ApplicationData.Languages.FileSelection;
        DecisionFileNameMatchingConditionIncludePathComboBoxItem.Content = ApplicationData.Languages.IncludePath;
        DecisionFileNameMatchingConditionNotIncludePathComboBoxItem.Content = ApplicationData.Languages.NotIncludePath;
        ProcessingSettingsTabItem.Header = ApplicationData.Languages.ProcessingSettings;
        ProcessingSettingsStandardDisplayLabel.Content = ApplicationData.Languages.DisplayToUseAsStandard;
        ProcessingSettingsStandardDisplayCurrentDisplayComboBoxItem.Content = ApplicationData.Languages.CurrentDisplay;
        ProcessingSettingsStandardDisplaySpecifiedDisplayComboBoxItem.Content = ApplicationData.Languages.SpecifiedDisplay;
        ProcessingSettingsStandardDisplayLimitedToSpecifiedDisplayComboBoxItem.Content = ApplicationData.Languages.LimitedToSpecifiedDisplay;
        ProcessingSettingsOneTimeProcessingLabel.Content = ApplicationData.Languages.ProcessOnlyOnce;
        ProcessingSettingsOneTimeProcessingDoNotSpecifyComboBoxItem.Content = ApplicationData.Languages.DoNotSpecify;
        ProcessingSettingsOneTimeProcessingOnceWindowOpenComboBoxItem.Content = ApplicationData.Languages.OnceWindowOpen;
        ProcessingSettingsOneTimeProcessingOnceWhileItIsRunningComboBoxItem.Content = ApplicationData.Languages.OnceWhileItIsRunning;
        ProcessingSettingsEventGroupBox.Header = ApplicationData.Languages.Event;
        EventExplanationButton.Content = ApplicationData.Languages.Question;
        EventExplanationButton.ToolTip = ApplicationData.Languages.Help;
        ProcessingSettingsForegroundToggleSwitch.OffContent = ProcessingSettingsForegroundToggleSwitch.OnContent = ApplicationData.Languages.Foregrounded;
        ProcessingSettingsMoveSizeEndToggleSwitch.OffContent = ProcessingSettingsMoveSizeEndToggleSwitch.OnContent = ApplicationData.Languages.MoveSizeChangeEnd;
        ProcessingSettingsMinimizeStartToggleSwitch.OffContent = ProcessingSettingsMinimizeStartToggleSwitch.OnContent = ApplicationData.Languages.MinimizeStart;
        ProcessingSettingsMinimizeEndToggleSwitch.OffContent = ProcessingSettingsMinimizeEndToggleSwitch.OnContent = ApplicationData.Languages.MinimizeEnd;
        ProcessingSettingsShowToggleSwitch.OffContent = ProcessingSettingsShowToggleSwitch.OnContent = ApplicationData.Languages.Show;
        ProcessingSettingsNameChangeToggleSwitch.OffContent = ProcessingSettingsNameChangeToggleSwitch.OnContent = ApplicationData.Languages.TitleNameChanged;
        ProcessingSettingsEventDelayTimeLabel.Content = ApplicationData.Languages.EventDelayTime;
        ProcessingSettingsTimerGroupBox.Header = ApplicationData.Languages.Timer;
        TimerExplanationButton.Content = ApplicationData.Languages.Question;
        TimerExplanationButton.ToolTip = ApplicationData.Languages.Help;
        ProcessingSettingsTimerProcessingToggleSwitch.OffContent = ProcessingSettingsTimerProcessingToggleSwitch.OnContent = ApplicationData.Languages.TimerProcessing;
        ProcessingSettingsDelayLabel.Content = ApplicationData.Languages.NumberOfTimesNotToProcessingFirst;
        WindowProcessingTabItem.Header = ApplicationData.Languages.SpecifyWindow;
        WindowProcessingProcessNameLabel.Content = ApplicationData.Languages.ProcessingName;
        WindowProcessingDisplayLabel.Content = ApplicationData.Languages.Display;
        WindowProcessingWindowStateLabel.Content = ApplicationData.Languages.WindowState;
        WindowProcessingWindowStateDoNotChangeComboBoxItem.Content = ApplicationData.Languages.DoNotChange;
        WindowProcessingWindowStateNormalWindowComboBoxItem.Content = ApplicationData.Languages.NormalWindow;
        WindowProcessingWindowStateMaximizeComboBoxItem.Content = ApplicationData.Languages.Maximize;
        WindowProcessingWindowStateMinimizeComboBoxItem.Content = ApplicationData.Languages.Minimize;
        WindowProcessingXLabel.Content = ApplicationData.Languages.X;
        WindowProcessingXDoNotChangeComboBoxItem.Content = ApplicationData.Languages.DoNotChange;
        WindowProcessingXLeftEdgeComboBoxItem.Content = ApplicationData.Languages.LeftEdge;
        WindowProcessingXMiddleComboBoxItem.Content = ApplicationData.Languages.Middle;
        WindowProcessingXRightEdgeComboBoxItem.Content = ApplicationData.Languages.RightEdge;
        WindowProcessingXCoordinateSpecificationComboBoxItem.Content = ApplicationData.Languages.CoordinateSpecification;
        WindowProcessingXTypePixelComboBoxItem.Content = ApplicationData.Languages.Pixel;
        WindowProcessingXTypePercentComboBoxItem.Content = ApplicationData.Languages.Percent;
        WindowProcessingYLabel.Content = ApplicationData.Languages.Y;
        WindowProcessingYDoNotChangeComboBoxItem.Content = ApplicationData.Languages.DoNotChange;
        WindowProcessingYTopEdgeComboBoxItem.Content = ApplicationData.Languages.TopEdge;
        WindowProcessingYMiddleComboBoxItem.Content = ApplicationData.Languages.Middle;
        WindowProcessingYBottomEdgeComboBoxItem.Content = ApplicationData.Languages.BottomEdge;
        WindowProcessingYCoordinateSpecificationComboBoxItem.Content = ApplicationData.Languages.CoordinateSpecification;
        WindowProcessingYTypePixelComboBoxItem.Content = ApplicationData.Languages.Pixel;
        WindowProcessingYTypePercentComboBoxItem.Content = ApplicationData.Languages.Percent;
        WindowProcessingWidthLabel.Content = ApplicationData.Languages.Width;
        WindowProcessingWidthDoNotChangeComboBoxItem.Content = ApplicationData.Languages.DoNotChange;
        WindowProcessingWidthWidthSpecificationComboBoxItem.Content = ApplicationData.Languages.WidthSpecification;
        WindowProcessingWidthTypePixelComboBoxItem.Content = ApplicationData.Languages.Pixel;
        WindowProcessingWidthTypePercentComboBoxItem.Content = ApplicationData.Languages.Percent;
        WindowProcessingHeightLabel.Content = ApplicationData.Languages.Height;
        WindowProcessingHeightDoNotChangeComboBoxItem.Content = ApplicationData.Languages.DoNotChange;
        WindowProcessingHeightHeightSpecificationComboBoxItem.Content = ApplicationData.Languages.HeightSpecification;
        WindowProcessingHeightTypePixelComboBoxItem.Content = ApplicationData.Languages.Pixel;
        WindowProcessingHeightTypePercentComboBoxItem.Content = ApplicationData.Languages.Percent;
        WindowProcessingNormalWindowOnlyToggleSwitch.OffContent = WindowProcessingNormalWindowOnlyToggleSwitch.OnContent = ApplicationData.Languages.ProcessOnlyWhenNormalWindow;
        WindowProcessingClientAreaToggleSwitch.OffContent = WindowProcessingClientAreaToggleSwitch.OnContent = ApplicationData.Languages.ClientArea;
        WindowProcessingForefrontLabel.Content = ApplicationData.Languages.Forefront;
        WindowProcessingForefrontDoNotChangeComboBoxItem.Content = ApplicationData.Languages.DoNotChange;
        WindowProcessingForefrontAlwaysForefrontComboBoxItem.Content = ApplicationData.Languages.AlwaysForefront;
        WindowProcessingForefrontAlwaysCancelForefrontComboBoxItem.Content = ApplicationData.Languages.AlwaysCancelForefront;
        WindowProcessingForefrontForefrontComboBoxItem.Content = ApplicationData.Languages.Forefront;
        WindowProcessingSpecifyTransparencyToggleSwitch.OffContent = WindowProcessingSpecifyTransparencyToggleSwitch.OnContent = ApplicationData.Languages.SpecifyTransparency;
        WindowProcessingCloseWindowToggleSwitch.OffContent = WindowProcessingCloseWindowToggleSwitch.OnContent = ApplicationData.Languages.CloseWindow;
        WindowProcessingHotkeyLabel.Content = ApplicationData.Languages.Hotkey;
        WindowProcessingAddProcessingButton.Content = ApplicationData.Languages.Add;
        WindowProcessingModifyProcessingButton.Content = ApplicationData.Languages.Modify;
        WindowProcessingCopyProcessingButton.Content = ApplicationData.Languages.Copy;
        WindowProcessingDeleteProcessingButton.Content = ApplicationData.Languages.Delete;
        ConditionsNotProcessTabItem.Header = ApplicationData.Languages.ConditionsNotProcess;
        ConditionsNotProcessChildWindowToggleSwitch.OffContent = ConditionsNotProcessChildWindowToggleSwitch.OnContent = ApplicationData.Languages.ChildWindow;
        ChildWindowExplanationButton.Content = ApplicationData.Languages.Question;
        ChildWindowExplanationButton.ToolTip = ApplicationData.Languages.Help;
        ConditionsNotProcessTitleNameRequirementsLabel.Content = ApplicationData.Languages.TitleNameRequirements;
        ConditionsNotProcessTitleNameRequirementsDoNotSpecifyComboBoxItem.Content = ApplicationData.Languages.DoNotSpecify;
        ConditionsNotProcessTitleNameRequirementsWindowWithoutTitleNameComboBoxItem.Content = ApplicationData.Languages.WindowWithoutTitleName;
        ConditionsNotProcessTitleNameRequirementsWindowWithTitleNameComboBoxItem.Content = ApplicationData.Languages.WindowWithTitleName;
        ConditionsNotProcessOtherThanSpecifiedVersionLabel.Content = ApplicationData.Languages.OtherThanSpecifiedVersion;
        ConditionsNotProcessTitleNameExclusionStringGroupBox.Header = ApplicationData.Languages.TitleNameExclusionString;
        ConditionsNotProcessAddTitleNameExclusionStringButton.Content = ApplicationData.Languages.Add;
        ConditionsNotProcessDeleteTitleNameExclusionStringButton.Content = ApplicationData.Languages.Delete;
        ConditionsNotProcessSizeGroupBox.Header = ApplicationData.Languages.Size;
        ConditionsNotProcessSizeWidthLabel.Content = ApplicationData.Languages.Width;
        ConditionsNotProcessSizeHeightLabel.Content = ApplicationData.Languages.Height;
        ConditionsNotProcessSizeAddButton.Content = ApplicationData.Languages.Add;
        ConditionsNotProcessSizeDeleteButton.Content = ApplicationData.Languages.Delete;
        ConditionsNotProcessOtherThanSpecifiedSizeGroupBox.Header = ApplicationData.Languages.OtherThanSpecifiedSize;
        ConditionsNotProcessOtherThanSpecifiedSizeWidthLabel.Content = ApplicationData.Languages.Width;
        ConditionsNotProcessOtherThanSpecifiedSizeHeightLabel.Content = ApplicationData.Languages.Height;
        ConditionsNotProcessOtherThanSpecifiedSizeAddButton.Content = ApplicationData.Languages.Add;
        ConditionsNotProcessOtherThanSpecifiedSizeDeleteButton.Content = ApplicationData.Languages.Delete;
        NotificationTabItem.Header = ApplicationData.Languages.Notification;
        NotificationVersionAnnounceToggleSwitch.OffContent = NotificationVersionAnnounceToggleSwitch.OnContent = ApplicationData.Languages.Notification;
        NotificationOtherThanSpecifiedVersionLabel.Content = ApplicationData.Languages.OtherThanSpecifiedVersion;
        NotificationSynchronizationVersionToggleSwitch.OffContent = NotificationSynchronizationVersionToggleSwitch.OnContent = ApplicationData.Languages.SynchronizationOtherThanSpecifiedVersion;
        CancelButton.Content = ApplicationData.Languages.Cancel;

        string stringData;     // 文字列データ
        RegisteredNameTextBox.Text = SpecifyWindowItemInformation.RegisteredName;
        GetInformationTitleNameCheckBox.IsChecked = ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.TitleName;
        GetInformationClassNameCheckBox.IsChecked = ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.ClassName;
        GetInformationFileNameCheckBox.IsChecked = ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.FileName;
        GetInformationDisplayCheckBox.IsChecked = ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.Display;
        GetInformationWindowStateCheckBox.IsChecked = ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.WindowState;
        GetInformationXCheckBox.IsChecked = ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.X;
        GetInformationYCheckBox.IsChecked = ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.Y;
        GetInformationWidthCheckBox.IsChecked = ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.Width;
        GetInformationHeightCheckBox.IsChecked = ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.Height;
        GetInformationVersionCheckBox.IsChecked= ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.Version;
        DecisionTitleNameTextBox.Text = SpecifyWindowItemInformation.TitleName;
        stringData = SpecifyWindowItemInformation.TitleNameMatchCondition switch
        {
            NameMatchCondition.PartialMatch => ApplicationData.Languages.PartialMatch,
            NameMatchCondition.ForwardMatch => ApplicationData.Languages.ForwardMatch,
            NameMatchCondition.BackwardMatch => ApplicationData.Languages.BackwardMatch,
            _ => ApplicationData.Languages.ExactMatch
        };
        ControlsProcessing.SelectComboBoxItem(DecisionTitleNameMatchingConditionComboBox, stringData);
        DecisionClassNameTextBox.Text = SpecifyWindowItemInformation.ClassName;
        stringData = SpecifyWindowItemInformation.ClassNameMatchCondition switch
        {
            NameMatchCondition.PartialMatch => ApplicationData.Languages.PartialMatch,
            NameMatchCondition.ForwardMatch => ApplicationData.Languages.ForwardMatch,
            NameMatchCondition.BackwardMatch => ApplicationData.Languages.BackwardMatch,
            _ => ApplicationData.Languages.ExactMatch
        };
        ControlsProcessing.SelectComboBoxItem(DecisionClassNameMatchingConditionComboBox, stringData);
        DecisionFileNameTextBox.Text = SpecifyWindowItemInformation.FileName;
        stringData = SpecifyWindowItemInformation.FileNameMatchCondition switch
        {
            FileNameMatchCondition.NotInclude => ApplicationData.Languages.NotIncludePath,
            _ => ApplicationData.Languages.IncludePath
        };
        ControlsProcessing.SelectComboBoxItem(DecisionFileNameMatchingConditionComboBox, stringData);
        stringData = SpecifyWindowItemInformation.StandardDisplay switch
        {
            StandardDisplay.SpecifiedDisplay => ApplicationData.Languages.SpecifiedDisplay,
            StandardDisplay.ExclusiveSpecifiedDisplay => ApplicationData.Languages.LimitedToSpecifiedDisplay,
            _ => ApplicationData.Languages.CurrentDisplay
        };
        ControlsProcessing.SelectComboBoxItem(ProcessingSettingsStandardDisplayComboBox, stringData);
        stringData = SpecifyWindowItemInformation.ProcessingOnlyOnce switch
        {
            ProcessingOnlyOnce.WindowOpen => ApplicationData.Languages.OnceWindowOpen,
            ProcessingOnlyOnce.Running => ApplicationData.Languages.OnceWhileItIsRunning,
            _ => ApplicationData.Languages.DoNotSpecify
        };
        ControlsProcessing.SelectComboBoxItem(ProcessingSettingsOneTimeProcessingComboBox, stringData);
        ProcessingSettingsForegroundToggleSwitch.IsOn = SpecifyWindowItemInformation.WindowEventData.Foreground;
        ProcessingSettingsMoveSizeEndToggleSwitch.IsOn = SpecifyWindowItemInformation.WindowEventData.MoveSizeEnd;
        ProcessingSettingsMinimizeStartToggleSwitch.IsOn = SpecifyWindowItemInformation.WindowEventData.MinimizeStart;
        ProcessingSettingsMinimizeEndToggleSwitch.IsOn = SpecifyWindowItemInformation.WindowEventData.MinimizeEnd;
        ProcessingSettingsShowToggleSwitch.IsOn = SpecifyWindowItemInformation.WindowEventData.Show;
        ProcessingSettingsNameChangeToggleSwitch.IsOn = SpecifyWindowItemInformation.WindowEventData.NameChange;
        ProcessingSettingsEventDelayTimeNumberBox.Value = SpecifyWindowItemInformation.WindowEventData.DelayTime;
        ProcessingSettingsTimerProcessingToggleSwitch.IsOn = SpecifyWindowItemInformation.TimerProcessing;
        ProcessingSettingsDelayNumberBox.Minimum = SpecifyWindowItemInformation.MinimumNumberOfTimesNotToProcessingFirst;
        ProcessingSettingsDelayNumberBox.Maximum = SpecifyWindowItemInformation.MaximumNumberOfTimesNotToProcessingFirst;
        ProcessingSettingsDelayNumberBox.Value = SpecifyWindowItemInformation.NumberOfTimesNotToProcessingFirst;
        ConditionsNotProcessChildWindowToggleSwitch.IsOn = SpecifyWindowItemInformation.DoNotProcessingChildWindow;
        stringData = SpecifyWindowItemInformation.DoNotProcessingTitleNameConditions switch
        {
            TitleNameProcessingConditions.NotIncluded => ApplicationData.Languages.WindowWithoutTitleName,
            TitleNameProcessingConditions.Included => ApplicationData.Languages.WindowWithTitleName,
            _ => ApplicationData.Languages.DoNotSpecify
        };
        ControlsProcessing.SelectComboBoxItem(ConditionsNotProcessTitleNameRequirementsComboBox, stringData);
        ConditionsNotProcessOtherThanSpecifiedVersionTextBox.Text = SpecifyWindowItemInformation.DoNotProcessingOtherThanSpecifiedVersion;
        foreach (string nowTitleName in SpecifyWindowItemInformation.DoNotProcessingStringContainedInTitleName)
        {
            ListBoxItem newItem = new()
            {
                Content = nowTitleName
            };
            ConditionsNotProcessExclusionTitleNameStringListBox.Items.Add(newItem);
        }
        foreach (SizeInt nowSize in SpecifyWindowItemInformation.DoNotProcessingSize)
        {
            ListBoxItem newItem = new()
            {
                Content = nowSize.Width + WindowControlValue.CopySeparateString + nowSize.Height
            };
            ConditionsNotProcessSizeListBox.Items.Add(newItem);
        }
        foreach (SizeInt nowSize in SpecifyWindowItemInformation.DoNotProcessingOtherThanSpecifiedSize)
        {
            ListBoxItem newItem = new()
            {
                Content = nowSize.Width + WindowControlValue.CopySeparateString + nowSize.Height
            };
            ConditionsNotProcessOtherThanSpecifiedSizeListBox.Items.Add(newItem);
        }
        NotificationVersionAnnounceToggleSwitch.IsOn = SpecifyWindowItemInformation.Notification;
        NotificationOtherThanSpecifiedVersionTextBox.Text = SpecifyWindowItemInformation.NotificationOtherThanSpecifiedVersion;
        NotificationSynchronizationVersionToggleSwitch.IsOn = SpecifyWindowItemInformation.NotificationSynchronizationVersion;
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
        ConditionsNotProcessAddTitleNameExclusionStringButton.IsEnabled = false;
        ConditionsNotProcessDeleteTitleNameExclusionStringButton.IsEnabled = false;
        ConditionsNotProcessSizeDeleteButton.IsEnabled = false;
        ConditionsNotProcessOtherThanSpecifiedSizeDeleteButton.IsEnabled = false;
        SettingsWindowProcessingDisableIfCloseIsEnabled();
        SettingsAddAndModifyButtonIsEnabled();

        Loaded += AddModifyWindowSpecifiedWindow_Loaded;
        Closed += SpecifiedWindowAddModifyWindow_Closed;
        GetWindowInformationButton.Click += GetWindowInformationButton_Click;
        TargetButton.PreviewMouseDown += TargetButton_PreviewMouseDown;
        WindowDecisionExplanationButton.Click += WindowDecisionExplanationButton_Click;
        DecisionTitleNameTextBox.TextChanged += DecisionTitleNameTextBox_TextChanged;
        DecisionClassNameTextBox.TextChanged += DecisionClassNameTextBox_TextChanged;
        DecisionFileNameTextBox.TextChanged += DecisionFileNameTextBox_TextChanged;
        DecisionFileNameFileSelectionButton.Click += DecisionFileNameFileSelectionButton_Click;
        EventExplanationButton.Click += EventExplanationButton_Click;
        TimerExplanationButton.Click += TimerExplanationButton_Click;
        WindowProcessingProcessNameTextBox.TextChanged += WindowProcessingProcessNameTextBox_TextChanged;
        WindowProcessingCloseWindowToggleSwitch.Toggled += WindowProcessingCloseWindowToggleSwitch_Toggled;
        WindowProcessingWindowStateComboBox.SelectionChanged += WindowProcessingWindowStateComboBox_SelectionChanged;
        WindowProcessingXComboBox.SelectionChanged += WindowProcessingXComboBox_SelectionChanged;
        WindowProcessingXTypeComboBox.SelectionChanged += WindowProcessingXTypeComboBox_SelectionChanged;
        WindowProcessingYComboBox.SelectionChanged += WindowProcessingYComboBox_SelectionChanged;
        WindowProcessingYTypeComboBox.SelectionChanged += WindowProcessingYTypeComboBox_SelectionChanged;
        WindowProcessingWidthComboBox.SelectionChanged += WindowProcessingWidthComboBox_SelectionChanged;
        WindowProcessingWidthTypeComboBox.SelectionChanged += WindowProcessingWidthTypeComboBox_SelectionChanged;
        WindowProcessingHeightComboBox.SelectionChanged += WindowProcessingHeightComboBox_SelectionChanged;
        WindowProcessingHeightTypeComboBox.SelectionChanged += WindowProcessingHeightTypeComboBox_SelectionChanged;
        ProcessingSettingsStandardDisplayComboBox.SelectionChanged += ProcessingSettingsStandardDisplayComboBox_SelectionChanged;
        WindowProcessingSpecifyTransparencyToggleSwitch.Toggled += WindowProcessingSpecifyTransparencyToggleSwitch_Toggled;
        WindowProcessingHotkeyTextBox.PreviewKeyDown += WindowProcessingHotkeyTextBox_PreviewKeyDown;
        WindowProcessingHotkeyTextBox.GotFocus += WindowProcessingHotkeyTextBox_GotFocus;
        WindowProcessingHotkeyTextBox.LostFocus += WindowProcessingHotkeyTextBox_LostFocus;
        WindowProcessingHotkeyTextBox.TextChanged += WindowProcessingHotkeyTextBox_TextChanged;
        ChildWindowExplanationButton.Click += ChildWindowExplanationButton_Click;
        ConditionsNotProcessExclusionTitleNameStringListBox.SelectionChanged += ConditionsNotProcessExclusionTitleNameStringListBox_SelectionChanged;
        ConditionsNotProcessTitleNameExclusionStringTextBox.TextChanged += ConditionsNotProcessTitleNameExclusionStringTextBox_TextChanged;
        ConditionsNotProcessAddTitleNameExclusionStringButton.Click += ConditionsNotProcessAddTitleNameExclusionStringButton_Click;
        ConditionsNotProcessDeleteTitleNameExclusionStringButton.Click += ConditionsNotProcessDeleteTitleNameExclusionStringButton_Click;
        ConditionsNotProcessSizeListBox.SelectionChanged += ConditionsNotProcessSizeListBox_SelectionChanged;
        ConditionsNotProcessSizeAddButton.Click += ConditionsNotProcessSizeAddButton_Click;
        ConditionsNotProcessSizeDeleteButton.Click += ConditionsNotProcessSizeDeleteButton_Click;
        ConditionsNotProcessOtherThanSpecifiedSizeListBox.SelectionChanged += ConditionsNotProcessOtherThanSpecifiedSizeListBox_SelectionChanged;
        ConditionsNotProcessOtherThanSpecifiedSizeAddButton.Click += ConditionsNotProcessOtherThanSpecifiedSizeAddButton_Click;
        ConditionsNotProcessOtherThanSpecifiedSizeDeleteButton.Click += ConditionsNotProcessOtherThanSpecifiedSizeDeleteButton_Click;
        WindowProcessingAddProcessingButton.Click += WindowProcessingAddProcessingButton_Click;
        WindowProcessingModifyProcessingButton.Click += WindowProcessingModifyProcessingButton_Click;
        WindowProcessingListBox.SelectionChanged += WindowProcessingListBox_SelectionChanged;
        WindowProcessingDeleteProcessingButton.Click += WindowProcessingDeleteProcessingButton_Click;
        WindowProcessingCopyProcessingButton.Click += WindowProcessingCopyProcessingButton_Click;
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

            ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.TitleName = GetInformationTitleNameCheckBox.IsChecked ?? false;
            ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.ClassName = GetInformationClassNameCheckBox.IsChecked ?? false;
            ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.FileName = GetInformationFileNameCheckBox.IsChecked ?? false;
            ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.Display = GetInformationDisplayCheckBox.IsChecked ?? false;
            ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.WindowState = GetInformationWindowStateCheckBox.IsChecked ?? false;
            ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.X = GetInformationXCheckBox.IsChecked ?? false;
            ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.Y = GetInformationYCheckBox.IsChecked ?? false;
            ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.Width = GetInformationWidthCheckBox.IsChecked ?? false;
            ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.Height = GetInformationHeightCheckBox.IsChecked ?? false;
            ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.Version = GetInformationVersionCheckBox.IsChecked ?? false;
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
            if (FEMessageBox.Show(ApplicationData.Languages.RetrievedAfterFiveSeconds, ApplicationData.Languages.Check, MessageBoxButton.OK) == MessageBoxResult.OK)
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
            FEMessageBox.Show(ApplicationData.Languages.Obtained, ApplicationData.Languages.Check, MessageBoxButton.OK);
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
            FEMessageBox.Show(ApplicationData.Languages.Obtained, ApplicationData.Languages.Check, MessageBoxButton.OK);
        }
        catch
        {
            FEMessageBox.Show(ApplicationData.Languages.ErrorOccurred, ApplicationData.Languages.Check, MessageBoxButton.OK);
        }
    }

    /// <summary>
    /// 「ウィンドウ判定」の「?」Buttonの「Click」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void WindowDecisionExplanationButton_Click(
        object sender,
        RoutedEventArgs e
        )
    {
        try
        {
            ApplicationData.WindowManagement.ShowExplanationWindow(SelectExplanationType.WindowDecision);
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
    private void DecisionTitleNameTextBox_TextChanged(
        object sender,
        TextChangedEventArgs e
        )
    {
        try
        {
            SettingsAddAndModifyButtonIsEnabled();
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
    private void DecisionClassNameTextBox_TextChanged(
        object sender,
        TextChangedEventArgs e
        )
    {
        try
        {
            SettingsAddAndModifyButtonIsEnabled();
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
    private void DecisionFileNameTextBox_TextChanged(
        object sender,
        TextChangedEventArgs e
        )
    {
        try
        {
            SettingsAddAndModifyButtonIsEnabled();
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
    private void DecisionFileNameFileSelectionButton_Click(
        object sender,
        RoutedEventArgs e
        )
    {
        try
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new()
            {
                Title = ApplicationData.Languages.FileSelection,
                Filter = ".exe|*.exe*",
                Multiselect = false
            };

            if (openFileDialog.ShowDialog() == true)
            {
                DecisionFileNameTextBox.Text = (string)((ComboBoxItem)DecisionFileNameMatchingConditionComboBox.SelectedItem).Content == ApplicationData.Languages.NotIncludePath
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
    /// 「処理設定」-「イベント」の「?」Buttonの「Click」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void EventExplanationButton_Click(
        object sender,
        RoutedEventArgs e
        )
    {
        try
        {
            ApplicationData.WindowManagement.ShowExplanationWindow(SelectExplanationType.EventTimer);
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「処理設定」-「タイマー」の「?」Buttonの「Click」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void TimerExplanationButton_Click(
        object sender,
        RoutedEventArgs e
        )
    {
        try
        {
            ApplicationData.WindowManagement.ShowExplanationWindow(SelectExplanationType.EventTimer);
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「処理名」TextBoxの「TextChanged」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void WindowProcessingProcessNameTextBox_TextChanged(
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
    private void WindowProcessingCloseWindowToggleSwitch_Toggled(
        object sender,
        RoutedEventArgs e
        )
    {
        try
        {
            SettingsWindowProcessingDisableIfCloseIsEnabled();
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「WindowProcessingDisableIfClose」の「IsEnabled」を設定
    /// </summary>
    private void SettingsWindowProcessingDisableIfCloseIsEnabled()
    {
        WindowProcessingDisableIfCloseIsEnabledStackPanel.IsEnabled = WindowProcessingCloseWindowToggleSwitch.IsOn == false;
    }

    /// <summary>
    /// 「ウィンドウの状態」ComboBoxの「SelectionChanged」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void WindowProcessingWindowStateComboBox_SelectionChanged(
        object sender,
        SelectionChangedEventArgs e
        )
    {
        try
        {
            if ((string)((ComboBoxItem)WindowProcessingWindowStateComboBox.SelectedItem).Content == ApplicationData.Languages.NormalWindow)
            {
                SettingsIsEnabledStateOfXControls();
                SettingsEnabledStateOfYControls();
                SettingsIsEnabledStateOfWidthControls();
                SettingsIsEnabledStateOfHeightControls();
            }
            SettingsPositionSizeControlsIsEnabled();
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
    private void WindowProcessingXComboBox_SelectionChanged(
        object sender,
        SelectionChangedEventArgs e
        )
    {
        try
        {
            SettingsIsEnabledStateOfXControls();
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
    private void WindowProcessingXTypeComboBox_SelectionChanged(
        object sender,
        SelectionChangedEventArgs e
        )
    {
        try
        {
            if ((string)((ComboBoxItem)WindowProcessingXTypeComboBox.SelectedItem).Content == ApplicationData.Languages.Percent)
            {
                WindowProcessingXNumberBox.Minimum = PositionSize.PositionSizePercentMinimum;
                WindowProcessingXNumberBox.Maximum = PositionSize.PositionSizePercentMaximum;
            }
            else
            {
                WindowProcessingXNumberBox.Minimum = PositionSize.PositionPixelMinimum;
                WindowProcessingXNumberBox.Maximum = PositionSize.PositionSizePixelMaximum;
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
    private void WindowProcessingYComboBox_SelectionChanged(
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
    private void WindowProcessingYTypeComboBox_SelectionChanged(
        object sender,
        SelectionChangedEventArgs e
        )
    {
        try
        {
            if ((string)((ComboBoxItem)WindowProcessingYTypeComboBox.SelectedItem).Content == ApplicationData.Languages.Percent)
            {
                WindowProcessingYNumberBox.Minimum = PositionSize.PositionSizePercentMinimum;
                WindowProcessingYNumberBox.Maximum = PositionSize.PositionSizePercentMaximum;
            }
            else
            {
                WindowProcessingYNumberBox.Minimum = PositionSize.PositionPixelMinimum;
                WindowProcessingYNumberBox.Maximum = PositionSize.PositionSizePixelMaximum;
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
    private void WindowProcessingWidthComboBox_SelectionChanged(
        object sender,
        SelectionChangedEventArgs e
        )
    {
        try
        {
            SettingsIsEnabledStateOfWidthControls();
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
    private void WindowProcessingWidthTypeComboBox_SelectionChanged(
        object sender,
        SelectionChangedEventArgs e
        )
    {
        try
        {
            if ((string)((ComboBoxItem)WindowProcessingWidthTypeComboBox.SelectedItem).Content == ApplicationData.Languages.Percent)
            {
                WindowProcessingWidthNumberBox.Minimum = PositionSize.PositionSizePercentMinimum;
                WindowProcessingWidthNumberBox.Maximum = PositionSize.PositionSizePercentMaximum;
            }
            else
            {
                WindowProcessingWidthNumberBox.Minimum = PositionSize.SizePixelMinimum;
                WindowProcessingWidthNumberBox.Maximum = PositionSize.PositionSizePixelMaximum;
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
    private void WindowProcessingHeightComboBox_SelectionChanged(
        object sender,
        SelectionChangedEventArgs e
        )
    {
        try
        {
            SettingsIsEnabledStateOfHeightControls();
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
    private void WindowProcessingHeightTypeComboBox_SelectionChanged(
        object sender,
        SelectionChangedEventArgs e
        )
    {
        try
        {
            if ((string)((ComboBoxItem)WindowProcessingHeightTypeComboBox.SelectedItem).Content == ApplicationData.Languages.Percent)
            {
                WindowProcessingHeightNumberBox.Minimum = PositionSize.PositionSizePercentMinimum;
                WindowProcessingHeightNumberBox.Maximum = PositionSize.PositionSizePercentMaximum;
            }
            else
            {
                WindowProcessingHeightNumberBox.Minimum = PositionSize.SizePixelMinimum;
                WindowProcessingHeightNumberBox.Maximum = PositionSize.PositionSizePixelMaximum;
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
    private void ProcessingSettingsStandardDisplayComboBox_SelectionChanged(
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
                WindowProcessingDisplayLabel.IsEnabled = false;
                WindowProcessingDisplayComboBox.IsEnabled = false;
            }
            else
            {
                WindowProcessingDisplayLabel.IsEnabled = true;
                WindowProcessingDisplayComboBox.IsEnabled = true;
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
    private void WindowProcessingSpecifyTransparencyToggleSwitch_Toggled(
        object sender,
        RoutedEventArgs e
        )
    {
        try
        {
            WindowProcessingTransparencyNumberBox.IsEnabled = WindowProcessingSpecifyTransparencyToggleSwitch.IsOn;
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
    private void WindowProcessingHotkeyTextBox_PreviewKeyDown(
        object sender,
        KeyEventArgs e
        )
    {
        try
        {
            FreeEcho.FEHotKeyWPF.HotKeyInformation hotkeyInformation = FreeEcho.FEHotKeyWPF.HotKeyWPF.GetHotKey(e, true);        // ホットキー情報
            string hotkeyString = FreeEcho.FEHotKeyWPF.HotKeyWPF.GetHotKeyString(hotkeyInformation);        // ホットキーの文字列
            if (hotkeyString != WindowControlValue.TabString)
            {
                HotkeyInformation.Copy(hotkeyInformation);
                WindowProcessingHotkeyTextBox.Text = hotkeyString;
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
    private void WindowProcessingHotkeyTextBox_GotFocus(
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
    private void WindowProcessingHotkeyTextBox_LostFocus(
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
    private void WindowProcessingHotkeyTextBox_TextChanged(
        object sender,
        TextChangedEventArgs e
        )
    {
        try
        {
            if (string.IsNullOrEmpty(WindowProcessingHotkeyTextBox.Text))
            {
                HotkeyInformation.Copy(new());
            }
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「処理しない条件」-「子ウィンドウ」の「?」Buttonの「Click」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ChildWindowExplanationButton_Click(
        object sender,
        RoutedEventArgs e
        )
    {
        try
        {
            ApplicationData.WindowManagement.ShowExplanationWindow(SelectExplanationType.ChildWindow);
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
    private void ConditionsNotProcessExclusionTitleNameStringListBox_SelectionChanged(
        object sender,
        SelectionChangedEventArgs e
        )
    {
        try
        {
            ConditionsNotProcessDeleteTitleNameExclusionStringButton.IsEnabled = ConditionsNotProcessExclusionTitleNameStringListBox.SelectedItems.Count != 0;
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
    private void ConditionsNotProcessTitleNameExclusionStringTextBox_TextChanged(
        object sender,
        TextChangedEventArgs e
        )
    {
        try
        {
            ConditionsNotProcessAddTitleNameExclusionStringButton.IsEnabled = !string.IsNullOrEmpty(ConditionsNotProcessTitleNameExclusionStringTextBox.Text);
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
    private void ConditionsNotProcessAddTitleNameExclusionStringButton_Click(
        object sender,
        RoutedEventArgs e
        )
    {
        try
        {
            // 値を確認
            string? resultString = CheckTitleNameExclusionString(ConditionsNotProcessTitleNameExclusionStringTextBox.Text);
            if (string.IsNullOrEmpty(resultString) == false)
            {
                FEMessageBox.Show(resultString, ApplicationData.Languages.Check, MessageBoxButton.OK);
                return;
            }

            SpecifyWindowItemInformation.DoNotProcessingStringContainedInTitleName.Add(ConditionsNotProcessTitleNameExclusionStringTextBox.Text);
            ListBoxItem newItem = new()
            {
                Content = ConditionsNotProcessTitleNameExclusionStringTextBox.Text
            };      // 新しいListBoxの項目
            ConditionsNotProcessExclusionTitleNameStringListBox.Items.Add(newItem);
            FEMessageBox.Show(ApplicationData.Languages.Added, ApplicationData.Languages.Check, MessageBoxButton.OK);
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
    private void ConditionsNotProcessDeleteTitleNameExclusionStringButton_Click(
        object sender,
        RoutedEventArgs e
        )
    {
        try
        {
            if (FEMessageBox.Show(SpecifyWindowItemInformation.DoNotProcessingStringContainedInTitleName[ConditionsNotProcessExclusionTitleNameStringListBox.SelectedIndex] + Environment.NewLine + ApplicationData.Languages.AllowDelete, ApplicationData.Languages.Check, MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                SpecifyWindowItemInformation.DoNotProcessingStringContainedInTitleName.RemoveAt(ConditionsNotProcessExclusionTitleNameStringListBox.SelectedIndex);
                ConditionsNotProcessExclusionTitleNameStringListBox.Items.RemoveAt(ConditionsNotProcessExclusionTitleNameStringListBox.SelectedIndex);
                FEMessageBox.Show(ApplicationData.Languages.Deleted, ApplicationData.Languages.Check, MessageBoxButton.OK);
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
    private void ConditionsNotProcessSizeListBox_SelectionChanged(
        object sender,
        SelectionChangedEventArgs e
        )
    {
        try
        {
            ConditionsNotProcessSizeDeleteButton.IsEnabled = ConditionsNotProcessSizeListBox.SelectedItems.Count != 0;
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
    private void ConditionsNotProcessSizeAddButton_Click(
        object sender,
        RoutedEventArgs e
        )
    {
        try
        {
            // 値を確認
            SizeInt size = new((int)ConditionsNotProcessSizeWidthNumberBox.Value, (int)ConditionsNotProcessSizeHeightNumberBox.Value);
            string? resultString = CheckForSize(size, SpecifyWindowItemInformation.DoNotProcessingSize);
            if (string.IsNullOrEmpty(resultString) == false)
            {
                FEMessageBox.Show(resultString, ApplicationData.Languages.Check, MessageBoxButton.OK);
                return;
            }

            SpecifyWindowItemInformation.DoNotProcessingSize.Add(new((int)ConditionsNotProcessSizeWidthNumberBox.Value, (int)ConditionsNotProcessSizeHeightNumberBox.Value));
            ListBoxItem newItem = new()
            {
                Content = ConditionsNotProcessSizeWidthNumberBox.Value + WindowControlValue.CopySeparateString + ConditionsNotProcessSizeHeightNumberBox.Value
            };      // 新しいListBoxの項目
            ConditionsNotProcessSizeListBox.Items.Add(newItem);
            FEMessageBox.Show(ApplicationData.Languages.Added, ApplicationData.Languages.Check, MessageBoxButton.OK);
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
    private void ConditionsNotProcessSizeDeleteButton_Click(
        object sender,
        RoutedEventArgs e
        )
    {
        try
        {
            if (FEMessageBox.Show(ApplicationData.Languages.AllowDelete, ApplicationData.Languages.Check, MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                SpecifyWindowItemInformation.DoNotProcessingSize.RemoveAt(ConditionsNotProcessSizeListBox.SelectedIndex);
                ConditionsNotProcessSizeListBox.Items.RemoveAt(ConditionsNotProcessSizeListBox.SelectedIndex);
                FEMessageBox.Show(ApplicationData.Languages.Deleted, ApplicationData.Languages.Check, MessageBoxButton.OK);
            }
        }
        catch
        {
            FEMessageBox.Show(ApplicationData.Languages.ErrorOccurred, ApplicationData.Languages.Check, MessageBoxButton.OK);
        }
    }

    /// <summary>
    /// 「処理しない条件」の「指定したサイズ以外」ListBoxの「SelectionChanged」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <exception cref="NotImplementedException"></exception>
    private void ConditionsNotProcessOtherThanSpecifiedSizeListBox_SelectionChanged(
        object sender,
        SelectionChangedEventArgs e
        )
    {
        try
        {
            ConditionsNotProcessOtherThanSpecifiedSizeDeleteButton.IsEnabled = ConditionsNotProcessOtherThanSpecifiedSizeListBox.SelectedItems.Count != 0;
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「処理しない条件」の「指定したサイズ以外」の「追加」Buttonの「Click」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <exception cref="NotImplementedException"></exception>
    private void ConditionsNotProcessOtherThanSpecifiedSizeAddButton_Click(
        object sender,
        RoutedEventArgs e
        )
    {
        try
        {
            // 値を確認
            SizeInt size = new((int)ConditionsNotProcessOtherThanSpecifiedSizeWidthNumberBox.Value, (int)ConditionsNotProcessOtherThanSpecifiedSizeHeightNumberBox.Value);
            string? resultString = CheckForSize(size, SpecifyWindowItemInformation.DoNotProcessingOtherThanSpecifiedSize);
            if (string.IsNullOrEmpty(resultString) == false)
            {
                FEMessageBox.Show(resultString, ApplicationData.Languages.Check, MessageBoxButton.OK);
                return;
            }

            SpecifyWindowItemInformation.DoNotProcessingOtherThanSpecifiedSize.Add(new((int)ConditionsNotProcessOtherThanSpecifiedSizeWidthNumberBox.Value, (int)ConditionsNotProcessOtherThanSpecifiedSizeHeightNumberBox.Value));
            ListBoxItem newItem = new()
            {
                Content = ConditionsNotProcessOtherThanSpecifiedSizeWidthNumberBox.Value + WindowControlValue.CopySeparateString + ConditionsNotProcessOtherThanSpecifiedSizeHeightNumberBox.Value
            };      // 新しいListBoxの項目
            ConditionsNotProcessOtherThanSpecifiedSizeListBox.Items.Add(newItem);
            FEMessageBox.Show(ApplicationData.Languages.Added, ApplicationData.Languages.Check, MessageBoxButton.OK);
        }
        catch
        {
            FEMessageBox.Show(ApplicationData.Languages.ErrorOccurred, ApplicationData.Languages.Check, MessageBoxButton.OK);
        }
    }

    /// <summary>
    /// 「処理しない条件」の「指定したサイズ以外」の「削除」Buttonの「Click」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <exception cref="NotImplementedException"></exception>
    private void ConditionsNotProcessOtherThanSpecifiedSizeDeleteButton_Click(
        object sender,
        RoutedEventArgs e
        )
    {
        try
        {
            if (FEMessageBox.Show(ApplicationData.Languages.AllowDelete, ApplicationData.Languages.Check, MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                SpecifyWindowItemInformation.DoNotProcessingOtherThanSpecifiedSize.RemoveAt(ConditionsNotProcessOtherThanSpecifiedSizeListBox.SelectedIndex);
                ConditionsNotProcessOtherThanSpecifiedSizeListBox.Items.RemoveAt(ConditionsNotProcessOtherThanSpecifiedSizeListBox.SelectedIndex);
                FEMessageBox.Show(ApplicationData.Languages.Deleted, ApplicationData.Languages.Check, MessageBoxButton.OK);
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
    private void WindowProcessingAddProcessingButton_Click(
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
                FEMessageBox.Show(ApplicationData.Languages.Added, ApplicationData.Languages.Check, MessageBoxButton.OK);
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
    private void WindowProcessingModifyProcessingButton_Click(
        object sender,
        RoutedEventArgs e
        )
    {
        try
        {
            if (ModifyWindowProcessingInformation())
            {
                SettingsTheActiveStateOfTheItemsListBox();
                FEMessageBox.Show(ApplicationData.Languages.Modified, ApplicationData.Languages.Check, MessageBoxButton.OK);
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
    private void WindowProcessingDeleteProcessingButton_Click(
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
            FEMessageBox.Show(ApplicationData.Languages.Deleted, ApplicationData.Languages.Check, MessageBoxButton.OK);
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
    private void WindowProcessingCopyProcessingButton_Click(
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
                if (nowWPI.ProcessingName == newWPI.ProcessingName + WindowControlValue.CopySeparateString + ApplicationData.Languages.Copy + WindowControlValue.SpaceSeparateString + number)
                {
                    number++;
                }
            }
            newWPI.ProcessingName += WindowControlValue.CopySeparateString + ApplicationData.Languages.Copy + WindowControlValue.SpaceSeparateString + number;
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
                if (string.IsNullOrEmpty(DecisionTitleNameTextBox.Text) == false)
                {
                    RegisteredNameTextBox.Text = DecisionTitleNameTextBox.Text;
                }
                else if (string.IsNullOrEmpty(DecisionClassNameTextBox.Text) == false)
                {
                    RegisteredNameTextBox.Text = DecisionClassNameTextBox.Text;
                }
                else if (string.IsNullOrEmpty(DecisionFileNameTextBox.Text) == false)
                {
                    RegisteredNameTextBox.Text = DecisionFileNameTextBox.Text;
                }
            }

            // 「処理しない条件」の「指定したバージョン以外」と同期
            if (NotificationSynchronizationVersionToggleSwitch.IsOn)
            {
                ConditionsNotProcessOtherThanSpecifiedVersionTextBox.Text = NotificationOtherThanSpecifiedVersionTextBox.Text;
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
                        if (string.IsNullOrEmpty(WindowProcessingProcessNameTextBox.Text))
                        {
                            WindowProcessingProcessNameTextBox.Text = RegisteredNameTextBox.Text;
                        }
                        result = AddWindowProcessingInformation();
                    }
                    if (result)
                    {
                        ApplicationData.Settings.SpecifyWindowInformation.Items.Add(SpecifyWindowItemInformation);
                        ApplicationData.WindowProcessingManagement.SpecifyWindowProcessing?.ProcessingSettings();
                        FEMessageBox.Show(ApplicationData.Languages.Added, ApplicationData.Languages.Check, MessageBoxButton.OK);
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
                        if (string.IsNullOrEmpty(WindowProcessingProcessNameTextBox.Text))
                        {
                            WindowProcessingProcessNameTextBox.Text = RegisteredNameTextBox.Text;
                        }
                        result = ModifyWindowProcessingInformation();
                    }
                    if (result)
                    {
                        ApplicationData.WindowProcessingManagement.SpecifyWindowProcessing?.UnregisterHotkeys();
                        ApplicationData.Settings.SpecifyWindowInformation.Items[IndexOfItemToBeModified] = SpecifyWindowItemInformation;
                        ApplicationData.WindowProcessingManagement.SpecifyWindowProcessing?.ProcessingSettings();
                        FEMessageBox.Show(ApplicationData.Languages.Modified, ApplicationData.Languages.Check, MessageBoxButton.OK);
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
    /// 「追加/修正」ボタンの「IsEnabled」を設定
    /// </summary>
    private void SettingsAddAndModifyButtonIsEnabled()
    {
        AddOrModifyButton.IsEnabled = !string.IsNullOrEmpty(DecisionTitleNameTextBox.Text) || !string.IsNullOrEmpty(DecisionClassNameTextBox.Text) || !string.IsNullOrEmpty(DecisionFileNameTextBox.Text);
    }

    /// <summary>
    /// 位置とサイズのコントロールの「IsEnabled」を設定
    /// </summary>
    private void SettingsPositionSizeControlsIsEnabled()
    {
        WindowProcessingTransparencyNumberBox.IsEnabled = WindowProcessingSpecifyTransparencyToggleSwitch.IsOn;
        switch (GetSelectedDisplayToUseAsStandard())
        {
            case StandardDisplay.CurrentDisplay:
                WindowProcessingDisplayLabel.IsEnabled = false;
                WindowProcessingDisplayComboBox.IsEnabled = false;
                break;
            default:
                WindowProcessingDisplayLabel.IsEnabled = true;
                WindowProcessingDisplayComboBox.IsEnabled = true;
                break;
        }
        if ((string)((ComboBoxItem)WindowProcessingWindowStateComboBox.SelectedItem).Content == ApplicationData.Languages.NormalWindow)
        {
            WindowProcessingXLabel.IsEnabled = true;
            WindowProcessingXStackPanel.IsEnabled = true;
            WindowProcessingYLabel.IsEnabled = true;
            WindowProcessingYStackPanel.IsEnabled = true;
            WindowProcessingWidthLabel.IsEnabled = true;
            WindowProcessingWidthStackPanel.IsEnabled = true;
            WindowProcessingHeightLabel.IsEnabled = true;
            WindowProcessingHeightStackPanel.IsEnabled = true;
            WindowProcessingClientAreaToggleSwitch.IsEnabled = true;
        }
        else
        {
            WindowProcessingXLabel.IsEnabled = false;
            WindowProcessingXStackPanel.IsEnabled = false;
            WindowProcessingYLabel.IsEnabled = false;
            WindowProcessingYStackPanel.IsEnabled = false;
            WindowProcessingWidthLabel.IsEnabled = false;
            WindowProcessingWidthStackPanel.IsEnabled = false;
            WindowProcessingHeightLabel.IsEnabled = false;
            WindowProcessingHeightStackPanel.IsEnabled = false;
            WindowProcessingClientAreaToggleSwitch.IsEnabled = false;
        }
    }

    /// <summary>
    /// 「ウィンドウ処理」の「追加/修正」ボタンの「IsEnabled」を設定
    /// </summary>
    private void WindowProcessingAddModifyButtonEnabled()
    {
        if (string.IsNullOrEmpty(WindowProcessingProcessNameTextBox.Text))
        {
            WindowProcessingAddProcessingButton.IsEnabled = false;
            WindowProcessingModifyProcessingButton.IsEnabled = false;
        }
        else
        {
            WindowProcessingAddProcessingButton.IsEnabled = true;
            if (WindowProcessingListBox.SelectedItems.Count != 0)
            {
                WindowProcessingModifyProcessingButton.IsEnabled = true;
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
        SpecifyWindowItemInformation.TitleName = DecisionTitleNameTextBox.Text;
        stringData = (string)((ComboBoxItem)DecisionTitleNameMatchingConditionComboBox.SelectedItem).Content;
        if (stringData == ApplicationData.Languages.ExactMatch)
        {
            SpecifyWindowItemInformation.TitleNameMatchCondition = NameMatchCondition.ExactMatch;
        }
        else if (stringData == ApplicationData.Languages.PartialMatch)
        {
            SpecifyWindowItemInformation.TitleNameMatchCondition = NameMatchCondition.PartialMatch;
        }
        else if (stringData == ApplicationData.Languages.ForwardMatch)
        {
            SpecifyWindowItemInformation.TitleNameMatchCondition = NameMatchCondition.ForwardMatch;
        }
        else if (stringData == ApplicationData.Languages.BackwardMatch)
        {
            SpecifyWindowItemInformation.TitleNameMatchCondition = NameMatchCondition.BackwardMatch;
        }
        SpecifyWindowItemInformation.ClassName = DecisionClassNameTextBox.Text;
        stringData = (string)((ComboBoxItem)DecisionClassNameMatchingConditionComboBox.SelectedItem).Content;
        if (stringData == ApplicationData.Languages.ExactMatch)
        {
            SpecifyWindowItemInformation.ClassNameMatchCondition = NameMatchCondition.ExactMatch;
        }
        else if (stringData == ApplicationData.Languages.PartialMatch)
        {
            SpecifyWindowItemInformation.ClassNameMatchCondition = NameMatchCondition.PartialMatch;
        }
        else if (stringData == ApplicationData.Languages.ForwardMatch)
        {
            SpecifyWindowItemInformation.ClassNameMatchCondition = NameMatchCondition.ForwardMatch;
        }
        else if (stringData == ApplicationData.Languages.BackwardMatch)
        {
            SpecifyWindowItemInformation.ClassNameMatchCondition = NameMatchCondition.BackwardMatch;
        }
        SpecifyWindowItemInformation.FileName = DecisionFileNameTextBox.Text;
        stringData = (string)((ComboBoxItem)DecisionFileNameMatchingConditionComboBox.SelectedItem).Content;
        if (stringData == ApplicationData.Languages.IncludePath)
        {
            SpecifyWindowItemInformation.FileNameMatchCondition = FileNameMatchCondition.Include;
        }
        else if (stringData == ApplicationData.Languages.NotIncludePath)
        {
            SpecifyWindowItemInformation.FileNameMatchCondition = FileNameMatchCondition.NotInclude;
        }
        SpecifyWindowItemInformation.StandardDisplay = GetSelectedDisplayToUseAsStandard();
        stringData = (string)((ComboBoxItem)ProcessingSettingsOneTimeProcessingComboBox.SelectedItem).Content;
        if (stringData == ApplicationData.Languages.DoNotSpecify)
        {
            SpecifyWindowItemInformation.ProcessingOnlyOnce = ProcessingOnlyOnce.NotSpecified;
        }
        else if (stringData == ApplicationData.Languages.OnceWindowOpen)
        {
            SpecifyWindowItemInformation.ProcessingOnlyOnce = ProcessingOnlyOnce.WindowOpen;
        }
        else if (stringData == ApplicationData.Languages.OnceWhileItIsRunning)
        {
            SpecifyWindowItemInformation.ProcessingOnlyOnce = ProcessingOnlyOnce.Running;
        }
        SpecifyWindowItemInformation.WindowEventData.Foreground = ProcessingSettingsForegroundToggleSwitch.IsOn;
        SpecifyWindowItemInformation.WindowEventData.MoveSizeEnd = ProcessingSettingsMoveSizeEndToggleSwitch.IsOn;
        SpecifyWindowItemInformation.WindowEventData.MinimizeStart = ProcessingSettingsMinimizeStartToggleSwitch.IsOn;
        SpecifyWindowItemInformation.WindowEventData.MinimizeEnd = ProcessingSettingsMinimizeEndToggleSwitch.IsOn;
        SpecifyWindowItemInformation.WindowEventData.Show = ProcessingSettingsShowToggleSwitch.IsOn;
        SpecifyWindowItemInformation.WindowEventData.NameChange = ProcessingSettingsNameChangeToggleSwitch.IsOn;
        SpecifyWindowItemInformation.WindowEventData.DelayTime = (int)ProcessingSettingsEventDelayTimeNumberBox.Value;
        SpecifyWindowItemInformation.TimerProcessing = ProcessingSettingsTimerProcessingToggleSwitch.IsOn;
        SpecifyWindowItemInformation.NumberOfTimesNotToProcessingFirst = (int)ProcessingSettingsDelayNumberBox.Value;
        SpecifyWindowItemInformation.DoNotProcessingChildWindow = ConditionsNotProcessChildWindowToggleSwitch.IsOn;
        stringData = (string)((ComboBoxItem)ConditionsNotProcessTitleNameRequirementsComboBox.SelectedItem).Content;
        if (stringData == ApplicationData.Languages.DoNotSpecify)
        {
            SpecifyWindowItemInformation.DoNotProcessingTitleNameConditions = TitleNameProcessingConditions.NotSpecified;
        }
        else if (stringData == ApplicationData.Languages.WindowWithoutTitleName)
        {
            SpecifyWindowItemInformation.DoNotProcessingTitleNameConditions = TitleNameProcessingConditions.NotIncluded;
        }
        else if (stringData == ApplicationData.Languages.WindowWithTitleName)
        {
            SpecifyWindowItemInformation.DoNotProcessingTitleNameConditions = TitleNameProcessingConditions.Included;
        }
        SpecifyWindowItemInformation.DoNotProcessingOtherThanSpecifiedVersion = ConditionsNotProcessOtherThanSpecifiedVersionTextBox.Text;
        SpecifyWindowItemInformation.Notification = NotificationVersionAnnounceToggleSwitch.IsOn;
        SpecifyWindowItemInformation.NotificationOtherThanSpecifiedVersion = NotificationOtherThanSpecifiedVersionTextBox.Text;
        SpecifyWindowItemInformation.NotificationSynchronizationVersion = NotificationSynchronizationVersionToggleSwitch.IsOn;
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
        WindowProcessingInformation newWPI = new()
        {
            Active = true,
            ProcessingName = WindowProcessingProcessNameTextBox.Text
        };      // 新しい情報

        newWPI.PositionSize.Display = WindowProcessingDisplayComboBox.Text;
        string stringData = (string)((ComboBoxItem)WindowProcessingWindowStateComboBox.SelectedItem).Content;
        if (stringData == ApplicationData.Languages.DoNotChange)
        {
            newWPI.PositionSize.SettingsWindowState = SettingsWindowState.DoNotChange;
        }
        else if (stringData == ApplicationData.Languages.NormalWindow)
        {
            newWPI.PositionSize.SettingsWindowState = SettingsWindowState.Normal;
        }
        else if (stringData == ApplicationData.Languages.Maximize)
        {
            newWPI.PositionSize.SettingsWindowState = SettingsWindowState.Maximize;
        }
        else if (stringData == ApplicationData.Languages.Minimize)
        {
            newWPI.PositionSize.SettingsWindowState = SettingsWindowState.Minimize;
        }
        stringData = (string)((ComboBoxItem)WindowProcessingXComboBox.SelectedItem).Content;
        if (stringData == ApplicationData.Languages.DoNotChange)
        {
            newWPI.PositionSize.XType = WindowXType.DoNotChange;
        }
        else if (stringData == ApplicationData.Languages.LeftEdge)
        {
            newWPI.PositionSize.XType = WindowXType.Left;
        }
        else if (stringData == ApplicationData.Languages.Middle)
        {
            newWPI.PositionSize.XType = WindowXType.Middle;
        }
        else if (stringData == ApplicationData.Languages.RightEdge)
        {
            newWPI.PositionSize.XType = WindowXType.Right;
        }
        else if (stringData == ApplicationData.Languages.CoordinateSpecification)
        {
            newWPI.PositionSize.XType = WindowXType.Value;
        }
        newWPI.PositionSize.X = WindowProcessingXNumberBox.Value;
        stringData = (string)((ComboBoxItem)WindowProcessingXTypeComboBox.SelectedItem).Content;
        if (stringData == ApplicationData.Languages.Pixel)
        {
            newWPI.PositionSize.XValueType = PositionSizeValueType.Pixel;
        }
        else if (stringData == ApplicationData.Languages.Percent)
        {
            newWPI.PositionSize.XValueType = PositionSizeValueType.Percent;
        }
        stringData = (string)((ComboBoxItem)WindowProcessingYComboBox.SelectedItem).Content;
        if (stringData == ApplicationData.Languages.DoNotChange)
        {
            newWPI.PositionSize.YType = WindowYType.DoNotChange;
        }
        else if (stringData == ApplicationData.Languages.TopEdge)
        {
            newWPI.PositionSize.YType = WindowYType.Top;
        }
        else if (stringData == ApplicationData.Languages.Middle)
        {
            newWPI.PositionSize.YType = WindowYType.Middle;
        }
        else if (stringData == ApplicationData.Languages.BottomEdge)
        {
            newWPI.PositionSize.YType = WindowYType.Bottom;
        }
        else if (stringData == ApplicationData.Languages.CoordinateSpecification)
        {
            newWPI.PositionSize.YType = WindowYType.Value;
        }
        newWPI.PositionSize.Y = WindowProcessingYNumberBox.Value;
        stringData = (string)((ComboBoxItem)WindowProcessingYTypeComboBox.SelectedItem).Content;
        if (stringData == ApplicationData.Languages.Pixel)
        {
            newWPI.PositionSize.YValueType = PositionSizeValueType.Pixel;
        }
        else if (stringData == ApplicationData.Languages.Percent)
        {
            newWPI.PositionSize.YValueType = PositionSizeValueType.Percent;
        }
        stringData = (string)((ComboBoxItem)WindowProcessingWidthComboBox.SelectedItem).Content;
        if (stringData == ApplicationData.Languages.DoNotChange)
        {
            newWPI.PositionSize.WidthType = WindowSizeType.DoNotChange;
        }
        else if (stringData == ApplicationData.Languages.WidthSpecification)
        {
            newWPI.PositionSize.WidthType = WindowSizeType.Value;
        }
        newWPI.PositionSize.Width = WindowProcessingWidthNumberBox.Value;
        stringData = (string)((ComboBoxItem)WindowProcessingWidthTypeComboBox.SelectedItem).Content;
        if (stringData == ApplicationData.Languages.Pixel)
        {
            newWPI.PositionSize.WidthValueType = PositionSizeValueType.Pixel;
        }
        else if (stringData == ApplicationData.Languages.Percent)
        {
            newWPI.PositionSize.WidthValueType = PositionSizeValueType.Percent;
        }
        stringData = (string)((ComboBoxItem)WindowProcessingHeightComboBox.SelectedItem).Content;
        if (stringData == ApplicationData.Languages.DoNotChange)
        {
            newWPI.PositionSize.HeightType = WindowSizeType.DoNotChange;
        }
        else if (stringData == ApplicationData.Languages.HeightSpecification)
        {
            newWPI.PositionSize.HeightType = WindowSizeType.Value;
        }
        newWPI.PositionSize.Height = WindowProcessingHeightNumberBox.Value;
        stringData = (string)((ComboBoxItem)WindowProcessingHeightTypeComboBox.SelectedItem).Content;
        if (stringData == ApplicationData.Languages.Pixel)
        {
            newWPI.PositionSize.HeightValueType = PositionSizeValueType.Pixel;
        }
        else if (stringData == ApplicationData.Languages.Percent)
        {
            newWPI.PositionSize.HeightValueType = PositionSizeValueType.Percent;
        }
        newWPI.NormalWindowOnly = WindowProcessingNormalWindowOnlyToggleSwitch.IsOn;
        newWPI.PositionSize.ClientArea = WindowProcessingClientAreaToggleSwitch.IsOn;
        stringData = (string)((ComboBoxItem)WindowProcessingForefrontComboBox.SelectedItem).Content;
        if (stringData == ApplicationData.Languages.DoNotChange)
        {
            newWPI.Forefront = Forefront.DoNotChange;
        }
        else if (stringData == ApplicationData.Languages.AlwaysForefront)
        {
            newWPI.Forefront = Forefront.Always;
        }
        else if (stringData == ApplicationData.Languages.AlwaysCancelForefront)
        {
            newWPI.Forefront = Forefront.Cancel;
        }
        else if (stringData == ApplicationData.Languages.Forefront)
        {
            newWPI.Forefront = Forefront.Forefront;
        }
        newWPI.SpecifyTransparency = WindowProcessingSpecifyTransparencyToggleSwitch.IsOn;
        newWPI.Transparency = (int)WindowProcessingTransparencyNumberBox.Value;
        newWPI.CloseWindow = WindowProcessingCloseWindowToggleSwitch.IsOn;
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
        WindowProcessingInformation settingsWPI = (WindowProcessingListBox.SelectedItems.Count == 0) ? new() : SpecifyWindowItemInformation.WindowProcessingInformation[WindowProcessingListBox.SelectedIndex];        // 設定する情報

        WindowProcessingProcessNameTextBox.Text = settingsWPI.ProcessingName;
        ControlsProcessing.SelectComboBoxItem(WindowProcessingDisplayComboBox, settingsWPI.PositionSize.Display);
        string stringData;
        stringData = settingsWPI.PositionSize.SettingsWindowState switch
        {
            SettingsWindowState.Normal => ApplicationData.Languages.NormalWindow,
            SettingsWindowState.Maximize => ApplicationData.Languages.Maximize,
            SettingsWindowState.Minimize => ApplicationData.Languages.Minimize,
            _ => ApplicationData.Languages.DoNotChange
        };
        ControlsProcessing.SelectComboBoxItem(WindowProcessingWindowStateComboBox, stringData);
        stringData = settingsWPI.PositionSize.XType switch
        {
            WindowXType.Left => ApplicationData.Languages.LeftEdge,
            WindowXType.Middle => ApplicationData.Languages.Middle,
            WindowXType.Right => ApplicationData.Languages.RightEdge,
            WindowXType.Value => ApplicationData.Languages.CoordinateSpecification,
            _ => ApplicationData.Languages.DoNotChange
        };
        ControlsProcessing.SelectComboBoxItem(WindowProcessingXComboBox, stringData);
        stringData = settingsWPI.PositionSize.XValueType switch
        {
            PositionSizeValueType.Percent => ApplicationData.Languages.Percent,
            _ => ApplicationData.Languages.Pixel
        };
        ControlsProcessing.SelectComboBoxItem(WindowProcessingXTypeComboBox, stringData);
        WindowProcessingXNumberBox.Value = (double)settingsWPI.PositionSize.X;
        SettingsIsEnabledStateOfXControls();
        stringData = settingsWPI.PositionSize.YType switch
        {
            WindowYType.Top => ApplicationData.Languages.TopEdge,
            WindowYType.Middle => ApplicationData.Languages.Middle,
            WindowYType.Bottom => ApplicationData.Languages.BottomEdge,
            WindowYType.Value => ApplicationData.Languages.CoordinateSpecification,
            _ => ApplicationData.Languages.DoNotChange
        };
        ControlsProcessing.SelectComboBoxItem(WindowProcessingYComboBox, stringData);
        stringData = settingsWPI.PositionSize.YValueType switch
        {
            PositionSizeValueType.Percent => ApplicationData.Languages.Percent,
            _ => ApplicationData.Languages.Pixel
        };
        ControlsProcessing.SelectComboBoxItem(WindowProcessingYTypeComboBox, stringData);
        WindowProcessingYNumberBox.Value = (double)settingsWPI.PositionSize.Y;
        SettingsEnabledStateOfYControls();
        stringData = settingsWPI.PositionSize.WidthType switch
        {
            WindowSizeType.Value => ApplicationData.Languages.WidthSpecification,
            _ => ApplicationData.Languages.DoNotChange
        };
        ControlsProcessing.SelectComboBoxItem(WindowProcessingWidthComboBox, stringData);
        stringData = settingsWPI.PositionSize.WidthValueType switch
        {
            PositionSizeValueType.Percent => ApplicationData.Languages.Percent,
            _ => ApplicationData.Languages.Pixel
        };
        ControlsProcessing.SelectComboBoxItem(WindowProcessingWidthTypeComboBox, stringData);
        WindowProcessingWidthNumberBox.Value = settingsWPI.PositionSize.Width;
        SettingsIsEnabledStateOfWidthControls();
        stringData = settingsWPI.PositionSize.HeightType switch
        {
            WindowSizeType.Value => ApplicationData.Languages.HeightSpecification,
            _ => ApplicationData.Languages.DoNotChange
        };
        ControlsProcessing.SelectComboBoxItem(WindowProcessingHeightComboBox, stringData);
        stringData = settingsWPI.PositionSize.HeightValueType switch
        {
            PositionSizeValueType.Percent => ApplicationData.Languages.Percent,
            _ => ApplicationData.Languages.Pixel
        };
        ControlsProcessing.SelectComboBoxItem(WindowProcessingHeightTypeComboBox, stringData);
        WindowProcessingHeightNumberBox.Value = settingsWPI.PositionSize.Height;
        SettingsIsEnabledStateOfHeightControls();
        SettingsPositionSizeControlsIsEnabled();
        WindowProcessingNormalWindowOnlyToggleSwitch.IsOn = settingsWPI.NormalWindowOnly;
        WindowProcessingClientAreaToggleSwitch.IsOn = settingsWPI.PositionSize.ClientArea;
        stringData = settingsWPI.Forefront switch
        {
            Forefront.Always => ApplicationData.Languages.AlwaysForefront,
            Forefront.Cancel => ApplicationData.Languages.AlwaysCancelForefront,
            Forefront.Forefront => ApplicationData.Languages.Forefront,
            _ => ApplicationData.Languages.DoNotChange
        };
        ControlsProcessing.SelectComboBoxItem(WindowProcessingForefrontComboBox, stringData);
        WindowProcessingSpecifyTransparencyToggleSwitch.IsOn = settingsWPI.SpecifyTransparency;
        WindowProcessingTransparencyNumberBox.IsEnabled = settingsWPI.SpecifyTransparency;
        WindowProcessingTransparencyNumberBox.Value = settingsWPI.Transparency;
        WindowProcessingCloseWindowToggleSwitch.IsOn = settingsWPI.CloseWindow;
        WindowProcessingHotkeyTextBox.Text = FreeEcho.FEHotKeyWPF.HotKeyWPF.GetHotKeyString(settingsWPI.Hotkey);

        WindowProcessingAddModifyButtonEnabled();
        if (WindowProcessingListBox.SelectedItems.Count == 1)
        {
            WindowProcessingCopyProcessingButton.IsEnabled = true;
            WindowProcessingDeleteProcessingButton.IsEnabled = true;
            HotkeyInformation.Copy(SpecifyWindowItemInformation.WindowProcessingInformation[WindowProcessingListBox.SelectedIndex].Hotkey);
        }
        else
        {
            WindowProcessingCopyProcessingButton.IsEnabled = false;
            WindowProcessingDeleteProcessingButton.IsEnabled = false;
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
        WindowInformation windowInformation = WindowProcessing.GetWindowInformationFromHandle(hwnd);
        if (GetInformationTitleNameCheckBox.IsChecked == true)
        {
            DecisionTitleNameTextBox.Text = windowInformation.TitleName;
        }
        if (GetInformationClassNameCheckBox.IsChecked == true)
        {
            DecisionClassNameTextBox.Text = windowInformation.ClassName;
        }
        if (GetInformationFileNameCheckBox.IsChecked == true)
        {
            DecisionFileNameTextBox.Text = (string)((ComboBoxItem)DecisionFileNameMatchingConditionComboBox.SelectedItem).Content == ApplicationData.Languages.IncludePath ? windowInformation.FileName : Path.GetFileNameWithoutExtension(windowInformation.FileName);
        }
        if (GetInformationVersionCheckBox.IsChecked == true)
        {
            GetInformationVersionTextBox.Text = windowInformation.Version;
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
            NativeMethods.GetWindowPlacement(hwnd, out WINDOWPLACEMENT windowPlacement);
            MonitorInformation.GetMonitorInformationForSpecifiedArea(new(windowPlacement.rcNormalPosition.Left, windowPlacement.rcNormalPosition.Top, windowPlacement.rcNormalPosition.Right - windowPlacement.rcNormalPosition.Left, windowPlacement.rcNormalPosition.Bottom - windowPlacement.rcNormalPosition.Top), out MonitorInfoEx monitorInfo);

            if (GetInformationDisplayCheckBox.IsChecked == true)
            {
                ControlsProcessing.SelectComboBoxItem(WindowProcessingDisplayComboBox, monitorInfo.DeviceName);
            }
            if (GetInformationWindowStateCheckBox.IsChecked == true)
            {
                string stringData = windowPlacement.showCmd switch
                {
                    (int)SW.SW_SHOWMAXIMIZED => ApplicationData.Languages.Maximize,
                    (int)SW.SW_SHOWMINIMIZED => ApplicationData.Languages.Minimize,
                    _ => ApplicationData.Languages.NormalWindow,
                };
                ControlsProcessing.SelectComboBoxItem(WindowProcessingWindowStateComboBox, stringData);
            }
            System.Drawing.Point displayPoint = new(0, 0);     // 基準にするディスプレイの座標
            if (ApplicationData.Settings.CoordinateType == CoordinateType.EachDisplay)
            {
                displayPoint.X = monitorInfo.WorkArea.Left;
                displayPoint.Y = monitorInfo.WorkArea.Top;
            }
            NativeMethods.GetWindowRect(hwnd, out RECT windowRect);       // ウィンドウの上下左右の位置
            if (GetInformationXCheckBox.IsChecked == true)
            {
                ControlsProcessing.SelectComboBoxItem(WindowProcessingXComboBox, ApplicationData.Languages.CoordinateSpecification);
                WindowProcessingXTypeComboBox.SelectedIndex = 0;
                WindowProcessingXNumberBox.Value = windowRect.Left - displayPoint.X;
            }
            if (GetInformationYCheckBox.IsChecked == true)
            {
                ControlsProcessing.SelectComboBoxItem(WindowProcessingYComboBox, ApplicationData.Languages.CoordinateSpecification);
                WindowProcessingYTypeComboBox.SelectedIndex = 0;
                WindowProcessingYNumberBox.Value = windowRect.Top - displayPoint.Y;
            }
            if (GetInformationWidthCheckBox.IsChecked == true)
            {
                ControlsProcessing.SelectComboBoxItem(WindowProcessingWidthComboBox, ApplicationData.Languages.WidthSpecification);
                WindowProcessingWidthTypeComboBox.SelectedIndex = 0;
                WindowProcessingWidthNumberBox.Value = windowRect.Right - windowRect.Left;
            }
            if (GetInformationHeightCheckBox.IsChecked == true)
            {
                ControlsProcessing.SelectComboBoxItem(WindowProcessingHeightComboBox, ApplicationData.Languages.HeightSpecification);
                WindowProcessingHeightTypeComboBox.SelectedIndex = 0;
                WindowProcessingHeightNumberBox.Value = windowRect.Bottom - windowRect.Top;
            }
            SettingsPositionSizeControlsIsEnabled();
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「X」コントロールの「IsEnabled」を設定
    /// </summary>
    private void SettingsIsEnabledStateOfXControls()
    {
        if ((string)((ComboBoxItem)WindowProcessingXComboBox.SelectedItem).Content == ApplicationData.Languages.CoordinateSpecification)
        {
            WindowProcessingXNumberBox.IsEnabled = true;
            WindowProcessingXTypeComboBox.IsEnabled = true;
        }
        else
        {
            WindowProcessingXNumberBox.IsEnabled = false;
            WindowProcessingXTypeComboBox.IsEnabled = false;
        }
    }

    /// <summary>
    /// 「Y」コントロールの「IsEnabled」を設定
    /// </summary>
    private void SettingsEnabledStateOfYControls()
    {
        if ((string)((ComboBoxItem)WindowProcessingYComboBox.SelectedItem).Content == ApplicationData.Languages.CoordinateSpecification)
        {
            WindowProcessingYNumberBox.IsEnabled = true;
            WindowProcessingYTypeComboBox.IsEnabled = true;
        }
        else
        {
            WindowProcessingYNumberBox.IsEnabled = false;
            WindowProcessingYTypeComboBox.IsEnabled = false;
        }
    }

    /// <summary>
    /// 「幅」コントロールの「IsEnabled」を設定
    /// </summary>
    private void SettingsIsEnabledStateOfWidthControls()
    {
        if ((string)((ComboBoxItem)WindowProcessingWidthComboBox.SelectedItem).Content == ApplicationData.Languages.WidthSpecification)
        {
            WindowProcessingWidthNumberBox.IsEnabled = true;
            WindowProcessingWidthTypeComboBox.IsEnabled = true;
        }
        else
        {
            WindowProcessingWidthNumberBox.IsEnabled = false;
            WindowProcessingWidthTypeComboBox.IsEnabled = false;
        }
    }

    /// <summary>
    /// 「高さ」のコントロールの「IsEnabled」を設定
    /// </summary>
    private void SettingsIsEnabledStateOfHeightControls()
    {
        if ((string)((ComboBoxItem)WindowProcessingHeightComboBox.SelectedItem).Content == ApplicationData.Languages.HeightSpecification)
        {
            WindowProcessingHeightNumberBox.IsEnabled = true;
            WindowProcessingHeightTypeComboBox.IsEnabled = true;
        }
        else
        {
            WindowProcessingHeightNumberBox.IsEnabled = false;
            WindowProcessingHeightTypeComboBox.IsEnabled = false;
        }
    }

    /// <summary>
    /// 選択している「基準にするディスプレイ」をコントロールから取得
    /// </summary>
    /// <returns>基準にするディスプレイ</returns>
    private StandardDisplay GetSelectedDisplayToUseAsStandard()
    {
        StandardDisplay selected = StandardDisplay.CurrentDisplay;
        string stringData = (string)((ComboBoxItem)ProcessingSettingsStandardDisplayComboBox.SelectedItem).Content;

        if (stringData == ApplicationData.Languages.SpecifiedDisplay)
        {
            selected = StandardDisplay.SpecifiedDisplay;
        }
        else if (stringData == ApplicationData.Languages.LimitedToSpecifiedDisplay)
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
                    if (count != itemSelectedIndex
                        && nowWPI.ProcessingName == wpi.ProcessingName)
                    {
                        result = ApplicationData.Languages.ThereIsADuplicateProcessingName;
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
                        result = ApplicationData.Languages.ThereIsADuplicateProcessingName;
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
                    result = ApplicationData.Languages.ThereIsADuplicateRegistrationName;
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
                    result = ApplicationData.Languages.ThereIsADuplicateRegistrationName;
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
                return ApplicationData.Languages.ThereAreDuplicateItems;
            }
        }

        return null;
    }

    /// <summary>
    /// サイズの値を確認
    /// </summary>
    /// <param name="size">確認するサイズ</param>
    /// <param name="listSize">サイズのリスト</param>
    /// <returns>結果の文字列 (問題ない「null」/問題がある「理由の文字列」)</returns>
    private string? CheckForSize(
        SizeInt size,
        List<SizeInt> listSize
        )
    {
        foreach (SizeInt nowSize in listSize)
        {
            if (nowSize.Width == size.Width
                && nowSize.Height == size.Height)
            {
                return ApplicationData.Languages.ThereAreDuplicateItems;
            }
        }

        return null;
    }
}
