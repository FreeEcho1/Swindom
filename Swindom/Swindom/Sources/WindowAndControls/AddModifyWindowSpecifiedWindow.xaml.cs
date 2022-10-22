namespace Swindom;

/// <summary>
/// 「指定ウィンドウ」 (「イベント」、「タイマー」) の「追加/修正」ウィンドウ
/// </summary>
public partial class AddModifyWindowSpecifiedWindow : Window
{
    /// <summary>
    /// 修正する項目のインデックス (追加「-1」)
    /// </summary>
    private readonly int IndexOfItemToBeModified = -1;
    /// <summary>
    /// 追加/修正したかの値 (いいえ「false」/はい「true」)
    /// </summary>
    public bool AddedOrModified { get; private set; }
    /// <summary>
    /// 「指定ウィンドウ」機能の基礎情報
    /// </summary>
    private readonly SpecifiedWindowBaseInformation SpecifiedWindowBaseInformation;
    /// <summary>
    /// 「指定ウィンドウ」機能の基礎の項目情報
    /// </summary>
    private readonly SpecifiedWindowBaseItemInformation SpecifiedWindowBaseItemInformation;
    /// <summary>
    /// ホットキー情報
    /// </summary>
    private readonly FreeEcho.FEHotKeyWPF.HotKeyInformation HotkeyInformation = new();
    /// <summary>
    /// ウィンドウ情報取得タイマー
    /// </summary>
    private readonly System.Windows.Threading.DispatcherTimer WindowInformationAcquisitionTimer;
    /// <summary>
    /// ウィンドウ選択枠
    /// </summary>
    private readonly FreeEcho.FEWindowSelectionMouse.WindowSelectionFrame WindowSelectionMouse;
    /// <summary>
    /// 最小化前のオーナーウィンドウの状態
    /// </summary>
    private WindowState PreviousOwnerWindowState = WindowState.Normal;
    /// <summary>
    /// ウィンドウ情報のバッファ
    /// </summary>
    private readonly WindowInformationBuffer WindowInformationBuffer = new();

    /// <summary>
    /// ウィンドウの最小の高さ
    /// </summary>
    private const int WindowHeightMin = 500;
    /// <summary>
    /// ListBoxItemの高さ
    /// </summary>
    private const int ListBoxItemHeight = 30;

    /// <summary>
    /// コンストラクタ (使用しない)
    /// </summary>
    public AddModifyWindowSpecifiedWindow()
    {
        throw new Exception("Do not use. - AddModifyWindowSpecifiedWindow()");
    }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="ownerWindow">オーナーウィンドウ</param>
    /// <param name="specifiedWindowBaseInformation">「指定ウィンドウ」機能の基礎情報</param>
    /// <param name="indexOfItemToBeModified">修正する項目のインデックス (追加「-1」)</param>
    public AddModifyWindowSpecifiedWindow(
        Window ownerWindow,
        SpecifiedWindowBaseInformation specifiedWindowBaseInformation,
        int indexOfItemToBeModified = -1
        )
    {
        if (Common.ApplicationData.Languages.LanguagesWindow == null)
        {
            throw new Exception("LanguagesWindow value is null. - AddModifyWindowSpecifiedWindow()");
        }

        InitializeComponent();

        SpecifiedWindowBaseInformation = specifiedWindowBaseInformation;
        IndexOfItemToBeModified = indexOfItemToBeModified;
        if (SpecifiedWindowBaseInformation is EventInformation isEventInformation)
        {
            SpecifiedWindowBaseItemInformation = (IndexOfItemToBeModified == -1) ? new EventItemInformation() : new EventItemInformation(isEventInformation.Items[IndexOfItemToBeModified]);
            TimerStackPanel.Visibility = Visibility.Collapsed;
        }
        else if (SpecifiedWindowBaseInformation is TimerInformation isTimerInformation)
        {
            SpecifiedWindowBaseItemInformation = (IndexOfItemToBeModified == -1) ? new TimerItemInformation() : new TimerItemInformation(isTimerInformation.Items[IndexOfItemToBeModified]);
            EventGroupBox.Visibility = Visibility.Collapsed;
        }
        else
        {
            throw new Exception("「EventInformation」、「TimerInformation」ではない。 - AddModifyWindowSpecifiedWindow()");
        }

        Owner = ownerWindow;
        if (SpecifiedWindowBaseInformation.RegisterMultiple == false)
        {
            AddAndModifyProcessingGrid.Visibility = Visibility.Collapsed;
            ProcessingNameLabel.Visibility = Visibility.Collapsed;
            ProcessingNameTextBox.Visibility = Visibility.Collapsed;
            MinWidth -= ProcessingListColumnDefinition.MinWidth;
            MaxWidth -= ProcessingListColumnDefinition.MinWidth;
            ProcessingListGrid.Visibility = Visibility.Collapsed;
            ProcessingListColumnDefinition.MinWidth = 0;
            ProcessingListColumnDefinition.Width = new GridLength(0);
        }
        if ((Common.ApplicationData.Settings.CoordinateType == CoordinateType.Global)
            || (Common.MonitorInformation.MonitorInfo.Count == 1))
        {
            DisplayCheckBox.Visibility = Visibility.Collapsed;
            StandardDisplayLabel.Visibility = Visibility.Collapsed;
            ProcessOnlyOnceLabel.Margin = new(0);
            StandardDisplayComboBox.Visibility = Visibility.Collapsed;
            DisplayLabel.Visibility = Visibility.Collapsed;
            DisplayComboBox.Visibility = Visibility.Collapsed;
        }
        SizeToContent = SizeToContent.Manual;
        Width = (SpecifiedWindowBaseInformation.AddModifyWindowSize.Width < MinWidth) ? MinWidth : SpecifiedWindowBaseInformation.AddModifyWindowSize.Width;
        Height = (SpecifiedWindowBaseInformation.AddModifyWindowSize.Height < WindowHeightMin) ? WindowHeightMin : SpecifiedWindowBaseInformation.AddModifyWindowSize.Height;
        InformationToRetrieveExpander.IsExpanded = false;
        foreach (MonitorInfoEx nowMonitorInfo in Common.MonitorInformation.MonitorInfo)
        {
            ComboBoxItem newItem = new()
            {
                Content = nowMonitorInfo.DeviceName
            };
            DisplayComboBox.Items.Add(newItem);
        }

        WindowInformationAcquisitionTimer = new()
        {
            Interval = new(0, 0, 0, 0, Common.WaitTimeForWindowInformationAcquisition)
        };
        WindowInformationAcquisitionTimer.Tick += WindowInformationAcquisitionTimer_Tick;
        WindowSelectionMouse = new()
        {
            MouseLeftUpStop = true
        };
        WindowSelectionMouse.MouseLeftButtonUp += WindowSelectionMouse_MouseLeftButtonUp;

        if (IndexOfItemToBeModified == -1)
        {
            Title = Common.ApplicationData.Languages.LanguagesWindow.Add;
            AddOrModifyButton.Content = Common.ApplicationData.Languages.LanguagesWindow.Add;
        }
        else
        {
            Title = Common.ApplicationData.Languages.LanguagesWindow.Modify;
            AddOrModifyButton.Content = Common.ApplicationData.Languages.LanguagesWindow.Modify;
        }
        GetWindowInformationButton.Content = Common.ApplicationData.Languages.LanguagesWindow.GetWindowInformation;
        HoldDownMouseCursorMoveToSelectWindowButton.ToolTip = Common.ApplicationData.Languages.LanguagesWindow.HoldDownMouseCursorMoveToSelectWindow;
        InformationToRetrieveExpander.Header = Common.ApplicationData.Languages.LanguagesWindow.WindowInformationToGet;
        TitleNameCheckBox.Content = Common.ApplicationData.Languages.LanguagesWindow.TitleName;
        ClassNameCheckBox.Content = Common.ApplicationData.Languages.LanguagesWindow.ClassName;
        FileNameCheckBox.Content = Common.ApplicationData.Languages.LanguagesWindow.FileName;
        WindowStateCheckBox.Content = Common.ApplicationData.Languages.LanguagesWindow.WindowState;
        XCheckBox.Content = Common.ApplicationData.Languages.LanguagesWindow.X;
        YCheckBox.Content = Common.ApplicationData.Languages.LanguagesWindow.Y;
        WidthCheckBox.Content = Common.ApplicationData.Languages.LanguagesWindow.Width;
        HeightCheckBox.Content = Common.ApplicationData.Languages.LanguagesWindow.Height;
        DisplayCheckBox.Content = Common.ApplicationData.Languages.LanguagesWindow.Display;
        RegisteredNameLabel.Content = Common.ApplicationData.Languages.LanguagesWindow.RegisteredName;
        WindowDecideTabItem.Header = Common.ApplicationData.Languages.LanguagesWindow.WindowDecide;
        ProcessingSettingsTabItem.Header = Common.ApplicationData.Languages.LanguagesWindow.ProcessingSettings;
        WindowProcessingTabItem.Header = Common.ApplicationData.Languages.LanguagesWindow.WindowProcessing;
        ConditionsThatDoNotProcessingTabItem.Header = Common.ApplicationData.Languages.LanguagesWindow.ConditionsThatDoNotProcess;
        TitleNameLabel.Content = Common.ApplicationData.Languages.LanguagesWindow.TitleName;
        ((ComboBoxItem)TitleNameMatchingConditionComboBox.Items[0]).Content = Common.ApplicationData.Languages.LanguagesWindow.ExactMatch;
        ((ComboBoxItem)TitleNameMatchingConditionComboBox.Items[1]).Content = Common.ApplicationData.Languages.LanguagesWindow.PartialMatch;
        ((ComboBoxItem)TitleNameMatchingConditionComboBox.Items[2]).Content = Common.ApplicationData.Languages.LanguagesWindow.ForwardMatch;
        ((ComboBoxItem)TitleNameMatchingConditionComboBox.Items[3]).Content = Common.ApplicationData.Languages.LanguagesWindow.BackwardMatch;
        ClassNameLabel.Content = Common.ApplicationData.Languages.LanguagesWindow.ClassName;
        ((ComboBoxItem)ClassNameMatchingConditionComboBox.Items[0]).Content = Common.ApplicationData.Languages.LanguagesWindow.ExactMatch;
        ((ComboBoxItem)ClassNameMatchingConditionComboBox.Items[1]).Content = Common.ApplicationData.Languages.LanguagesWindow.PartialMatch;
        ((ComboBoxItem)ClassNameMatchingConditionComboBox.Items[2]).Content = Common.ApplicationData.Languages.LanguagesWindow.ForwardMatch;
        ((ComboBoxItem)ClassNameMatchingConditionComboBox.Items[3]).Content = Common.ApplicationData.Languages.LanguagesWindow.BackwardMatch;
        FileNameLabel.Content = Common.ApplicationData.Languages.LanguagesWindow.FileName;
        FileNameFileSelectionButton.ToolTip = Common.ApplicationData.Languages.LanguagesWindow.FileSelection;
        ((ComboBoxItem)FileNameMatchingConditionComboBox.Items[0]).Content = Common.ApplicationData.Languages.LanguagesWindow.IncludePath;
        ((ComboBoxItem)FileNameMatchingConditionComboBox.Items[1]).Content = Common.ApplicationData.Languages.LanguagesWindow.DoNotIncludePath;
        UntitledWindowConditionsLabel.Content = Common.ApplicationData.Languages.LanguagesWindow.TitleProcessingConditions;
        ((ComboBoxItem)UntitledWindowConditionsComboBox.Items[0]).Content = Common.ApplicationData.Languages.LanguagesWindow.DoNotSpecify;
        ((ComboBoxItem)UntitledWindowConditionsComboBox.Items[1]).Content = Common.ApplicationData.Languages.LanguagesWindow.DoNotProcessingUntitledWindow;
        ((ComboBoxItem)UntitledWindowConditionsComboBox.Items[2]).Content = Common.ApplicationData.Languages.LanguagesWindow.DoNotProcessingWindowWithTitle;
        ProcessOnlyOnceLabel.Content = Common.ApplicationData.Languages.LanguagesWindow.ProcessOnlyOnce;
        ((ComboBoxItem)ProcessOnlyOnceComboBox.Items[0]).Content = Common.ApplicationData.Languages.LanguagesWindow.DoNotSpecify;
        ((ComboBoxItem)ProcessOnlyOnceComboBox.Items[1]).Content = Common.ApplicationData.Languages.LanguagesWindow.OnceWindowOpen;
        ((ComboBoxItem)ProcessOnlyOnceComboBox.Items[2]).Content = Common.ApplicationData.Languages.LanguagesWindow.OnceWhileItIsRunning;
        EventGroupBox.Header = Common.ApplicationData.Languages.LanguagesWindow.WhenToProcessing;
        ForegroundToggleSwitch.Text = Common.ApplicationData.Languages.LanguagesWindow.TheForegroundHasBeenChanged;
        MoveSizeEndToggleSwitch.Text = Common.ApplicationData.Languages.LanguagesWindow.MoveSizeChangeEnd;
        MinimizeStartToggleSwitch.Text = Common.ApplicationData.Languages.LanguagesWindow.MinimizeStart;
        MinimizeEndToggleSwitch.Text = Common.ApplicationData.Languages.LanguagesWindow.MinimizeEnd;
        CreateToggleSwitch.Text = Common.ApplicationData.Languages.LanguagesWindow.Create;
        ShowToggleSwitch.Text = Common.ApplicationData.Languages.LanguagesWindow.Show;
        NameChangeToggleSwitch.Text = Common.ApplicationData.Languages.LanguagesWindow.NameChanged;
        CloseWindowToggleSwitch.Text = Common.ApplicationData.Languages.LanguagesWindow.CloseWindow;
        DelayLabel.Content = Common.ApplicationData.Languages.LanguagesWindow.NumberOfTimesNotToProcessingFirst;
        StandardDisplayLabel.Content = Common.ApplicationData.Languages.LanguagesWindow.DisplayToUseAsStandard;
        ((ComboBoxItem)StandardDisplayComboBox.Items[0]).Content = Common.ApplicationData.Languages.LanguagesWindow.TheSpecifiedDisplay;
        ((ComboBoxItem)StandardDisplayComboBox.Items[1]).Content = Common.ApplicationData.Languages.LanguagesWindow.DisplayWithWindow;
        ((ComboBoxItem)StandardDisplayComboBox.Items[2]).Content = Common.ApplicationData.Languages.LanguagesWindow.OnlyIfItIsOnTheSpecifiedDisplay;
        ProcessingGroupBox.Header = Common.ApplicationData.Languages.LanguagesWindow.Processing;
        ProcessingNameLabel.Content = Common.ApplicationData.Languages.LanguagesWindow.ProcessingName;
        PositionAndSizeGroupBox.Header = Common.ApplicationData.Languages.LanguagesWindow.PositionAndSize;
        WindowStateLabel.Content = Common.ApplicationData.Languages.LanguagesWindow.WindowState;
        ((ComboBoxItem)WindowStateComboBox.Items[0]).Content = Common.ApplicationData.Languages.LanguagesWindow.DoNotChange;
        ((ComboBoxItem)WindowStateComboBox.Items[1]).Content = Common.ApplicationData.Languages.LanguagesWindow.NormalWindow;
        ((ComboBoxItem)WindowStateComboBox.Items[2]).Content = Common.ApplicationData.Languages.LanguagesWindow.Maximize;
        ((ComboBoxItem)WindowStateComboBox.Items[3]).Content = Common.ApplicationData.Languages.LanguagesWindow.Minimize;
        ProcessOnlyWhenNormalWindowToggleSwitch.Text = Common.ApplicationData.Languages.LanguagesWindow.ProcessOnlyWhenNormalWindow;
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
        DisplayLabel.Content = Common.ApplicationData.Languages.LanguagesWindow.Display;
        ForefrontLabel.Content = Common.ApplicationData.Languages.LanguagesWindow.Forefront;
        ((ComboBoxItem)ForefrontComboBox.Items[0]).Content = Common.ApplicationData.Languages.LanguagesWindow.DoNotChange;
        ((ComboBoxItem)ForefrontComboBox.Items[1]).Content = Common.ApplicationData.Languages.LanguagesWindow.AlwaysForefront;
        ((ComboBoxItem)ForefrontComboBox.Items[2]).Content = Common.ApplicationData.Languages.LanguagesWindow.AlwaysCancelForefront;
        ((ComboBoxItem)ForefrontComboBox.Items[3]).Content = Common.ApplicationData.Languages.LanguagesWindow.Forefront;
        SpecifyTransparencyToggleSwitch.Text = Common.ApplicationData.Languages.LanguagesWindow.SpecifyTransparency;
        HotkeyLabel.Content = Common.ApplicationData.Languages.LanguagesWindow.Hotkey;
        TitleNameExclusionStringGroupBox.Header = Common.ApplicationData.Languages.LanguagesWindow.TitleNameExclusionString;
        AddTitleNameExclusionStringButton.Content = Common.ApplicationData.Languages.LanguagesWindow.Add;
        DeleteTitleNameExclusionStringButton.Content = Common.ApplicationData.Languages.LanguagesWindow.Delete;
        DoNotProcessingSizeGroupBox.Header = Common.ApplicationData.Languages.LanguagesWindow.Size;
        DoNotProcessingSizeWidthLabel.Content = Common.ApplicationData.Languages.LanguagesWindow.Width;
        DoNotProcessingSizeHeightLabel.Content = Common.ApplicationData.Languages.LanguagesWindow.Height;
        DoNotProcessingSizeAddButton.Content = Common.ApplicationData.Languages.LanguagesWindow.Add;
        DoNotProcessingSizeDeleteButton.Content = Common.ApplicationData.Languages.LanguagesWindow.Delete;
        DifferentVersionGroupBox.Header = Common.ApplicationData.Languages.LanguagesWindow.DifferentVersion;
        DifferentVersionLabel.Content = Common.ApplicationData.Languages.LanguagesWindow.Version;
        DifferentVersionAnnounceToggleSwitch.Text = Common.ApplicationData.Languages.LanguagesWindow.Announce;
        AddProcessingButton.Content = Common.ApplicationData.Languages.LanguagesWindow.Add;
        ModifyProcessingButton.Content = Common.ApplicationData.Languages.LanguagesWindow.Modify;
        ActiveProcessingButton.Content = Common.ApplicationData.Languages.LanguagesWindow.Active;
        DeleteProcessingButton.Content = Common.ApplicationData.Languages.LanguagesWindow.Delete;
        CopyProcessingButton.Content = Common.ApplicationData.Languages.LanguagesWindow.Copy;
        CancelButton.Content = Common.ApplicationData.Languages.LanguagesWindow.Cancel;

        string stringData;     // 文字列データ
        TitleNameCheckBox.IsChecked = SpecifiedWindowBaseInformation.AcquiredItems.TitleName;
        ClassNameCheckBox.IsChecked = SpecifiedWindowBaseInformation.AcquiredItems.ClassName;
        FileNameCheckBox.IsChecked = SpecifiedWindowBaseInformation.AcquiredItems.FileName;
        WindowStateCheckBox.IsChecked = SpecifiedWindowBaseInformation.AcquiredItems.WindowState;
        XCheckBox.IsChecked = SpecifiedWindowBaseInformation.AcquiredItems.X;
        YCheckBox.IsChecked = SpecifiedWindowBaseInformation.AcquiredItems.Y;
        WidthCheckBox.IsChecked = SpecifiedWindowBaseInformation.AcquiredItems.Width;
        HeightCheckBox.IsChecked = SpecifiedWindowBaseInformation.AcquiredItems.Height;
        DisplayCheckBox.IsChecked = SpecifiedWindowBaseInformation.AcquiredItems.Display;
        RegisteredNameTextBox.Text = SpecifiedWindowBaseItemInformation.RegisteredName;
        TitleNameTextBox.Text = SpecifiedWindowBaseItemInformation.TitleName;
        stringData = SpecifiedWindowBaseItemInformation.TitleNameMatchCondition switch
        {
            NameMatchCondition.PartialMatch => Common.ApplicationData.Languages.LanguagesWindow.PartialMatch,
            NameMatchCondition.ForwardMatch => Common.ApplicationData.Languages.LanguagesWindow.ForwardMatch,
            NameMatchCondition.BackwardMatch => Common.ApplicationData.Languages.LanguagesWindow.BackwardMatch,
            _ => Common.ApplicationData.Languages.LanguagesWindow.ExactMatch
        };
        Processing.SelectComboBoxItem(TitleNameMatchingConditionComboBox, stringData);
        ClassNameTextBox.Text = SpecifiedWindowBaseItemInformation.ClassName;
        stringData = SpecifiedWindowBaseItemInformation.ClassNameMatchCondition switch
        {
            NameMatchCondition.PartialMatch => Common.ApplicationData.Languages.LanguagesWindow.PartialMatch,
            NameMatchCondition.ForwardMatch => Common.ApplicationData.Languages.LanguagesWindow.ForwardMatch,
            NameMatchCondition.BackwardMatch => Common.ApplicationData.Languages.LanguagesWindow.BackwardMatch,
            _ => Common.ApplicationData.Languages.LanguagesWindow.ExactMatch
        };
        Processing.SelectComboBoxItem(ClassNameMatchingConditionComboBox, stringData);
        FileNameTextBox.Text = SpecifiedWindowBaseItemInformation.FileName;
        stringData = SpecifiedWindowBaseItemInformation.FileNameMatchCondition switch
        {
            FileNameMatchCondition.NotInclude => Common.ApplicationData.Languages.LanguagesWindow.DoNotIncludePath,
            _ => Common.ApplicationData.Languages.LanguagesWindow.IncludePath
        };
        Processing.SelectComboBoxItem(FileNameMatchingConditionComboBox, stringData);
        stringData = SpecifiedWindowBaseItemInformation.TitleProcessingConditions switch
        {
            TitleProcessingConditions.DoNotProcessingUntitledWindow => Common.ApplicationData.Languages.LanguagesWindow.DoNotProcessingUntitledWindow,
            TitleProcessingConditions.DoNotProcessingWindowWithTitle => Common.ApplicationData.Languages.LanguagesWindow.DoNotProcessingWindowWithTitle,
            _ => Common.ApplicationData.Languages.LanguagesWindow.DoNotSpecify
        };
        Processing.SelectComboBoxItem(UntitledWindowConditionsComboBox, stringData);
        stringData = SpecifiedWindowBaseItemInformation.ProcessingOnlyOnce switch
        {
            ProcessingOnlyOnce.WindowOpen => Common.ApplicationData.Languages.LanguagesWindow.OnceWindowOpen,
            ProcessingOnlyOnce.Running => Common.ApplicationData.Languages.LanguagesWindow.OnceWhileItIsRunning,
            _ => Common.ApplicationData.Languages.LanguagesWindow.DoNotSpecify
        };
        Processing.SelectComboBoxItem(ProcessOnlyOnceComboBox, stringData);
        CloseWindowToggleSwitch.IsOn = SpecifiedWindowBaseItemInformation.CloseWindow;
        if (SpecifiedWindowBaseInformation is EventInformation)
        {
            EventItemInformation information = (EventItemInformation)SpecifiedWindowBaseItemInformation;
            ForegroundToggleSwitch.IsOn = information.WindowEventData.Foreground;
            MoveSizeEndToggleSwitch.IsOn = information.WindowEventData.MoveSizeEnd;
            MinimizeStartToggleSwitch.IsOn = information.WindowEventData.MinimizeStart;
            MinimizeEndToggleSwitch.IsOn = information.WindowEventData.MinimizeEnd;
            CreateToggleSwitch.IsOn = information.WindowEventData.Create;
            ShowToggleSwitch.IsOn = information.WindowEventData.Show;
            NameChangeToggleSwitch.IsOn = information.WindowEventData.NameChange;
        }
        else if (SpecifiedWindowBaseInformation is TimerInformation)
        {
            TimerItemInformation information = (TimerItemInformation)SpecifiedWindowBaseItemInformation;
            DelayNumericUpDown.ValueInt = information.NumberOfTimesNotToProcessingFirst;
        }
        stringData = SpecifiedWindowBaseItemInformation.StandardDisplay switch
        {
            StandardDisplay.OnlySpecifiedDisplay => Common.ApplicationData.Languages.LanguagesWindow.OnlyIfItIsOnTheSpecifiedDisplay,
            StandardDisplay.DisplayWithWindow => Common.ApplicationData.Languages.LanguagesWindow.DisplayWithWindow,
            _ => Common.ApplicationData.Languages.LanguagesWindow.TheSpecifiedDisplay
        };
        Processing.SelectComboBoxItem(StandardDisplayComboBox, stringData);
        DisplayComboBox.SelectedIndex = 0;
        foreach (WindowProcessingInformation nowWPI in SpecifiedWindowBaseItemInformation.WindowProcessingInformation)
        {
            FreeEcho.FEControls.ListBoxItemValidState newItem = new()
            {
                Text = nowWPI.ProcessingName,
                Height = ListBoxItemHeight,
                IsValidState = nowWPI.Active
            };
            WindowProcessingListBox.Items.Add(newItem);
        }
        int count = 0;
        foreach (WindowProcessingInformation nowWPI in SpecifiedWindowBaseItemInformation.WindowProcessingInformation)
        {
            if (nowWPI.Active)
            {
                WindowProcessingListBox.SelectedIndex = count;
                break;
            }
            count++;
        }
        foreach (string nowTitleName in SpecifiedWindowBaseItemInformation.DoNotProcessingTitleName)
        {
            ListBoxItem newItem = new()
            {
                Content = nowTitleName
            };
            TitleNameExclusionStringListBox.Items.Add(newItem);
        }
        foreach (System.Drawing.Size nowSize in SpecifiedWindowBaseItemInformation.DoNotProcessingSize)
        {
            ListBoxItem newItem = new()
            {
                Content = nowSize.Width + Common.CopySeparateString + nowSize.Height
            };
            DoNotProcessingSizeListBox.Items.Add(newItem);
        }
        DifferentVersionTextBox.Text = SpecifiedWindowBaseItemInformation.DifferentVersionVersion;
        DifferentVersionAnnounceToggleSwitch.IsOn = SpecifiedWindowBaseItemInformation.DifferentVersionAnnounce;
        SettingsValueToControls();
        SettingsPositionSizeControlEnabled();
        TransparencyNumericUpDown.IsEnabled = SpecifyTransparencyToggleSwitch.IsOn;
        switch (SpecifiedWindowBaseItemInformation.StandardDisplay)
        {
            case StandardDisplay.DisplayWithWindow:
                DisplayLabel.IsEnabled = false;
                DisplayComboBox.IsEnabled = false;
                break;
        }
        AddTitleNameExclusionStringButton.IsEnabled = false;
        DeleteTitleNameExclusionStringButton.IsEnabled = false;
        DoNotProcessingSizeDeleteButton.IsEnabled = false;
        if (string.IsNullOrEmpty(ProcessingNameTextBox.Text))
        {
            AddProcessingButton.IsEnabled = false;
            ModifyProcessingButton.IsEnabled = false;
        }
        SettingsAddAndModifyButtonEnableState();

        Loaded += AddModifyWindowSpecifiedWindow_Loaded;
        Closed += SpecifiedWindowAddModifyWindow_Closed;
        GetWindowInformationButton.Click += GetWindowInformationButton_Click;
        HoldDownMouseCursorMoveToSelectWindowButton.PreviewMouseDown += HoldDownMouseCursorMoveToSelectWindowButton_PreviewMouseDown;
        TitleNameTextBox.TextChanged += TitleNameTextBox_TextChanged;
        ClassNameTextBox.TextChanged += ClassNameTextBox_TextChanged;
        FileNameTextBox.TextChanged += FileNameTextBox_TextChanged;
        FileNameFileSelectionButton.Click += FileNameFileSelectionButton_Click;
        ProcessingNameTextBox.TextChanged += ProcessingNameTextBox_TextChanged;
        CloseWindowToggleSwitch.IsOnChange += CloseWindowToggleSwitch_IsOnChange;
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
        SpecifyTransparencyToggleSwitch.IsOnChange += SpecifyTransparencyToggleSwitch_IsOnChange;
        HotkeyTextBox.PreviewKeyDown += HotkeyTextBox_PreviewKeyDown;
        HotkeyTextBox.GotFocus += HotkeyTextBox_GotFocus;
        HotkeyTextBox.LostFocus += HotkeyTextBox_LostFocus;
        TitleNameExclusionStringListBox.SelectionChanged += TitleNameExclusionStringListBox_SelectionChanged;
        TitleNameExclusionStringTextBox.TextChanged += TitleNameExclusionStringTextBox_TextChanged;
        AddTitleNameExclusionStringButton.Click += AddTitleNameExclusionStringButton_Click;
        DeleteTitleNameExclusionStringButton.Click += DeleteTitleNameExclusionStringButton_Click;
        DoNotProcessingSizeListBox.SelectionChanged += DoNotProcessingSizeListBox_SelectionChanged;
        DoNotProcessingSizeAddButton.Click += DoNotProcessingSizeAddButton_Click;
        DoNotProcessingSizeDeleteButton.Click += DoNotProcessingSizeDeleteButton_Click;
        AddProcessingButton.Click += AddProcessingButton_Click;
        ModifyProcessingButton.Click += ModifyProcessingButton_Click;
        WindowProcessingListBox.SelectionChanged += WindowProcessingListBox_SelectionChanged;
        ActiveProcessingButton.Click += ActiveProcessingButton_Click;
        DeleteProcessingButton.Click += DeleteProcessingButton_Click;
        CopyProcessingButton.Click += CopyProcessingButton_Click;
        AddOrModifyButton.Click += AddOrModifyButton_Click;
        CancelButton.Click += CancelButton_Click;
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
            WindowInformationAcquisitionTimerAfterProcessing();
        }
        catch
        {
        }
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
            if (SpecifiedWindowBaseInformation.StopProcessingShowAddModifyWindow)
            {
                if (SpecifiedWindowBaseInformation is EventInformation)
                {
                    Common.ApplicationData.DoProcessingEvent(ProcessingEventType.EventPause);
                }
                else if (SpecifiedWindowBaseInformation is TimerInformation)
                {
                    Common.ApplicationData.DoProcessingEvent(ProcessingEventType.TimerPause);
                }
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

            SpecifiedWindowBaseInformation.AcquiredItems.TitleName = TitleNameCheckBox.IsChecked ?? false;
            SpecifiedWindowBaseInformation.AcquiredItems.ClassName = ClassNameCheckBox.IsChecked ?? false;
            SpecifiedWindowBaseInformation.AcquiredItems.FileName = FileNameCheckBox.IsChecked ?? false;
            SpecifiedWindowBaseInformation.AcquiredItems.WindowState = WindowStateCheckBox.IsChecked ?? false;
            SpecifiedWindowBaseInformation.AcquiredItems.X = XCheckBox.IsChecked ?? false;
            SpecifiedWindowBaseInformation.AcquiredItems.Y = YCheckBox.IsChecked ?? false;
            SpecifiedWindowBaseInformation.AcquiredItems.Width = WidthCheckBox.IsChecked ?? false;
            SpecifiedWindowBaseInformation.AcquiredItems.Height = HeightCheckBox.IsChecked ?? false;
            SpecifiedWindowBaseInformation.AcquiredItems.Display = DisplayCheckBox.IsChecked ?? false;
            SpecifiedWindowBaseInformation.AddModifyWindowSize.Width = (int)Width;
            SpecifiedWindowBaseInformation.AddModifyWindowSize.Height = (int)Height;

            if (SpecifiedWindowBaseInformation.StopProcessingShowAddModifyWindow)
            {
                if (SpecifiedWindowBaseInformation is EventInformation)
                {
                    Common.ApplicationData.DoProcessingEvent(ProcessingEventType.EventUnpause);
                }
                else if (SpecifiedWindowBaseInformation is TimerInformation)
                {
                    Common.ApplicationData.DoProcessingEvent(ProcessingEventType.TimerUnpause);
                }
            }
            SettingFileProcessing.WriteSettings();
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
            if (FreeEcho.FEControls.MessageBox.Show(Common.ApplicationData.Languages.Check, Common.ApplicationData.Languages.LanguagesWindow?.RetrievedAfterFiveSeconds, FreeEcho.FEControls.MessageBoxButton.Ok, this) == FreeEcho.FEControls.MessageBoxResult.Ok)
            {
                GetWindowInformationStackPanel.IsEnabled = false;
                WindowInformationAcquisitionTimer.Start();
            }
        }
        catch
        {
            GetWindowInformationStackPanel.IsEnabled = true;
            FreeEcho.FEControls.MessageBox.Show(Common.ApplicationData.Languages.Check, Common.ApplicationData.Languages.ErrorOccurred, FreeEcho.FEControls.MessageBoxButton.Ok, this);
        }
    }

    /// <summary>
    /// 「マウスでウィンドウ情報取得」Buttonの「PreviewMouseDown」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void HoldDownMouseCursorMoveToSelectWindowButton_PreviewMouseDown(
        object sender,
        MouseButtonEventArgs e
        )
    {
        try
        {
            PreviousOwnerWindowState = Owner.WindowState;
            WindowState = WindowState.Minimized;
            Owner.WindowState = WindowState.Minimized;
            GetWindowInformationStackPanel.IsEnabled = false;
            WindowSelectionMouse.StartWindowSelection();
        }
        catch
        {
            FreeEcho.FEControls.MessageBox.Show(Common.ApplicationData.Languages.Check, Common.ApplicationData.Languages.ErrorOccurred, FreeEcho.FEControls.MessageBoxButton.Ok, this);
        }
    }

    /// <summary>
    /// 「ウィンドウ情報取得」タイマー経過後の処理
    /// </summary>
    private void WindowInformationAcquisitionTimerAfterProcessing()
    {
        try
        {
            WindowInformationAcquisitionTimer.Stop();
            if (WindowProcessingTabItem.IsSelected)
            {
                GetPositionSizeFromHandle(NativeMethods.GetForegroundWindow());
            }
            else
            {
                GetInformationFromWindowHandle(NativeMethods.GetForegroundWindow());
            }
            Activate();
            FreeEcho.FEControls.MessageBox.Show(Common.ApplicationData.Languages.Check, Common.ApplicationData.Languages.LanguagesWindow?.SelectionComplete, FreeEcho.FEControls.MessageBoxButton.Ok, this);
        }
        catch
        {
            Activate();
            FreeEcho.FEControls.MessageBox.Show(Common.ApplicationData.Languages.Check, Common.ApplicationData.Languages.ErrorOccurred, FreeEcho.FEControls.MessageBoxButton.Ok, this);
        }
        finally
        {
            GetWindowInformationStackPanel.IsEnabled = true;
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
            if (WindowProcessingTabItem.IsSelected)
            {
                GetPositionSizeFromHandle(WindowSelectionMouse.SelectedHwnd);
            }
            else
            {
                GetInformationFromWindowHandle(WindowSelectionMouse.SelectedHwnd);
            }
            Owner.WindowState = PreviousOwnerWindowState;
            WindowState = WindowState.Normal;
            Activate();
            FreeEcho.FEControls.MessageBox.Show(Common.ApplicationData.Languages.Check, Common.ApplicationData.Languages.LanguagesWindow?.SelectionComplete, FreeEcho.FEControls.MessageBoxButton.Ok, this);
        }
        catch
        {
            FreeEcho.FEControls.MessageBox.Show(Common.ApplicationData.Languages.Check, Common.ApplicationData.Languages.ErrorOccurred, FreeEcho.FEControls.MessageBoxButton.Ok, this);
        }
        finally
        {
            GetWindowInformationStackPanel.IsEnabled = true;
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
                Title = Common.ApplicationData.Languages.LanguagesWindow?.FileSelection,
                Filter = ".exe|*.exe*",
                Multiselect = false
            };

            if (openFileDialog.ShowDialog() == true)
            {
                FileNameTextBox.Text = (string)((ComboBoxItem)FileNameMatchingConditionComboBox.SelectedItem).Content == Common.ApplicationData.Languages.LanguagesWindow?.DoNotIncludePath
                    ? Path.GetFileNameWithoutExtension(openFileDialog.FileName)
                    : openFileDialog.FileName;
            }
        }
        catch
        {
            FreeEcho.FEControls.MessageBox.Show(Common.ApplicationData.Languages.Check, Common.ApplicationData.Languages.ErrorOccurred, FreeEcho.FEControls.MessageBoxButton.Ok, this);
        }
    }

    /// <summary>
    /// 「処理名」TextBoxの「TextChanged」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ProcessingNameTextBox_TextChanged(
        object sender,
        TextChangedEventArgs e
        )
    {
        try
        {
            if (string.IsNullOrEmpty(ProcessingNameTextBox.Text))
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
        catch
        {
        }
    }

    /// <summary>
    /// 「ウィンドウを閉じる」ToggleSwitchの「IsOnChange」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void CloseWindowToggleSwitch_IsOnChange(
        object sender,
        FreeEcho.FEControls.ToggleSwitchEventArgs e
        )
    {
        try
        {
            if (CloseWindowToggleSwitch.IsOn)
            {
                StandardDisplayLabel.IsEnabled = false;
                StandardDisplayComboBox.IsEnabled = false;
                ProcessingGroupBox.IsEnabled = false;
                ProcessingListGrid.IsEnabled = false;
            }
            else
            {
                StandardDisplayLabel.IsEnabled = true;
                StandardDisplayComboBox.IsEnabled = true;
                ProcessingGroupBox.IsEnabled = true;
                ProcessingListGrid.IsEnabled = true;
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
            if ((string)((ComboBoxItem)WindowStateComboBox.SelectedItem).Content == Common.ApplicationData.Languages.LanguagesWindow?.NormalWindow)
            {
                SettingsTheEnabledStateOfTheXControls();
                SettingsTheEnabledStateOfTheYControls();
                SettingsTheEnabledStateOfTheWidthControls();
                SettingsTheEnabledStateOfTheHeightControls();
            }
            SettingsPositionSizeControlEnabled();
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
            SettingsTheEnabledStateOfTheXControls();
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
            if ((string)((ComboBoxItem)XTypeComboBox.SelectedItem).Content == Common.ApplicationData.Languages.LanguagesWindow?.Percent)
            {
                XNumericUpDown.IsUseDecimal = true;
                XNumericUpDown.MinimumValue = Common.PercentMinimize;
                XNumericUpDown.MaximumValue = Common.PercentMaximize;
            }
            else
            {
                XNumericUpDown.IsUseDecimal = false;
                XNumericUpDown.MinimumValue = decimal.MinValue;
                XNumericUpDown.MaximumValue = decimal.MaxValue;
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
            SettingsTheEnabledStateOfTheYControls();
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
                YNumericUpDown.MinimumValue = decimal.MinValue;
                YNumericUpDown.MaximumValue = decimal.MaxValue;
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
            SettingsTheEnabledStateOfTheWidthControls();
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
                WidthNumericUpDown.MinimumValue = Common.PercentMinimize;
                WidthNumericUpDown.MaximumValue = Common.PercentMaximize;
            }
            else
            {
                WidthNumericUpDown.IsUseDecimal = false;
                WidthNumericUpDown.MinimumValue = decimal.MinValue;
                WidthNumericUpDown.MaximumValue = decimal.MaxValue;
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
            SettingsTheEnabledStateOfTheHeightControls();
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
                HeightNumericUpDown.MinimumValue = Common.PercentMinimize;
                HeightNumericUpDown.MaximumValue = Common.PercentMaximize;
            }
            else
            {
                HeightNumericUpDown.IsUseDecimal = false;
                HeightNumericUpDown.MinimumValue = decimal.MinValue;
                HeightNumericUpDown.MaximumValue = decimal.MaxValue;
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
            CheckAndSettingsActiveStateWPI(SpecifiedWindowBaseItemInformation, WindowProcessingListBox.SelectedIndex, GetSelectedDisplayToUseAsStandard(), -1);
            SettingsTheActiveStateOfTheItemsListBox();

            if (GetSelectedDisplayToUseAsStandard() == StandardDisplay.DisplayWithWindow)
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
    /// 「透明度指定」ToggleSwitchの「IsOnChange」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void SpecifyTransparencyToggleSwitch_IsOnChange(
        object sender,
        FreeEcho.FEControls.ToggleSwitchEventArgs e
        )
    {
        try
        {
            TransparencyNumericUpDown.IsEnabled = SpecifyTransparencyToggleSwitch.IsOn;
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
    /// 「処理しない条件」の「タイトル名の除外文字列」ListBoxの「SelectionChanged」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void TitleNameExclusionStringListBox_SelectionChanged(
        object sender,
        SelectionChangedEventArgs e
        )
    {
        try
        {
            DeleteTitleNameExclusionStringButton.IsEnabled = TitleNameExclusionStringListBox.SelectedItems.Count != 0;
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「処理しない条件」の「タイトル名の除外文字列」TextBoxの「TextChanged」イベント
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
    /// 「処理しない条件」の「タイトル名の除外文字列」の「追加」Buttonの「Click」イベント
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
            string? resultString = CheckDoNotProcessingTitleName(SpecifiedWindowBaseItemInformation, TitleNameExclusionStringTextBox.Text);
            if (string.IsNullOrEmpty(resultString) == false)
            {
                FreeEcho.FEControls.MessageBox.Show(Common.ApplicationData.Languages.Check, resultString, FreeEcho.FEControls.MessageBoxButton.Ok, this);
                return;
            }

            SpecifiedWindowBaseItemInformation.DoNotProcessingTitleName.Add(TitleNameExclusionStringTextBox.Text);
            ListBoxItem newItem = new()
            {
                Content = TitleNameExclusionStringTextBox.Text
            };      // 新しいListBoxItem
            TitleNameExclusionStringListBox.Items.Add(newItem);
            FreeEcho.FEControls.MessageBox.Show(Common.ApplicationData.Languages.Check, Common.ApplicationData.Languages.LanguagesWindow?.Added, FreeEcho.FEControls.MessageBoxButton.Ok, this);
        }
        catch
        {
            FreeEcho.FEControls.MessageBox.Show(Common.ApplicationData.Languages.Check, Common.ApplicationData.Languages.ErrorOccurred, FreeEcho.FEControls.MessageBoxButton.Ok, this);
        }
    }

    /// <summary>
    /// 「処理しない条件」の「タイトル名の除外文字列」の「削除」Buttonの「Click」イベント
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
            if (FreeEcho.FEControls.MessageBox.Show(Common.ApplicationData.Languages.Check, Common.ApplicationData.Languages.LanguagesWindow?.AllowDelete, FreeEcho.FEControls.MessageBoxButton.YesCancel, this) == FreeEcho.FEControls.MessageBoxResult.Yes)
            {
                SpecifiedWindowBaseItemInformation.DoNotProcessingTitleName.RemoveAt(TitleNameExclusionStringListBox.SelectedIndex);
                TitleNameExclusionStringListBox.Items.RemoveAt(TitleNameExclusionStringListBox.SelectedIndex);
                FreeEcho.FEControls.MessageBox.Show(Common.ApplicationData.Languages.Check, Common.ApplicationData.Languages.LanguagesWindow?.Deleted, FreeEcho.FEControls.MessageBoxButton.Ok, this);
            }
        }
        catch
        {
            FreeEcho.FEControls.MessageBox.Show(Common.ApplicationData.Languages.Check, Common.ApplicationData.Languages.ErrorOccurred, FreeEcho.FEControls.MessageBoxButton.Ok, this);
        }
    }

    /// <summary>
    /// 「処理しない条件」の「サイズ」ListBoxの「SelectionChanged」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void DoNotProcessingSizeListBox_SelectionChanged(
        object sender,
        SelectionChangedEventArgs e
        )
    {
        try
        {
            DoNotProcessingSizeDeleteButton.IsEnabled = DoNotProcessingSizeListBox.SelectedItems.Count != 0;
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
            System.Drawing.Size size = new(DoNotProcessingSizeWidthNumericUpDown.ValueInt, DoNotProcessingSizeHeightNumericUpDown.ValueInt);
            string? resultString = CheckDoNotProcessingSize(SpecifiedWindowBaseItemInformation, size);
            if (string.IsNullOrEmpty(resultString) == false)
            {
                FreeEcho.FEControls.MessageBox.Show(Common.ApplicationData.Languages.Check, resultString, FreeEcho.FEControls.MessageBoxButton.Ok, this);
                return;
            }

            SpecifiedWindowBaseItemInformation.DoNotProcessingSize.Add(new(DoNotProcessingSizeWidthNumericUpDown.ValueInt, DoNotProcessingSizeHeightNumericUpDown.ValueInt));
            ListBoxItem newItem = new()
            {
                Content = DoNotProcessingSizeWidthNumericUpDown.ValueInt + Common.CopySeparateString + DoNotProcessingSizeHeightNumericUpDown.ValueInt
            };      // 新しいListBoxItem
            DoNotProcessingSizeListBox.Items.Add(newItem);
            FreeEcho.FEControls.MessageBox.Show(Common.ApplicationData.Languages.Check, Common.ApplicationData.Languages.LanguagesWindow?.Added, FreeEcho.FEControls.MessageBoxButton.Ok, this);
        }
        catch
        {
            FreeEcho.FEControls.MessageBox.Show(Common.ApplicationData.Languages.Check, Common.ApplicationData.Languages.ErrorOccurred, FreeEcho.FEControls.MessageBoxButton.Ok, this);
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
            if (FreeEcho.FEControls.MessageBox.Show(Common.ApplicationData.Languages.Check, Common.ApplicationData.Languages.LanguagesWindow?.AllowDelete, FreeEcho.FEControls.MessageBoxButton.YesCancel, this) == FreeEcho.FEControls.MessageBoxResult.Yes)
            {
                SpecifiedWindowBaseItemInformation.DoNotProcessingSize.RemoveAt(DoNotProcessingSizeListBox.SelectedIndex);
                DoNotProcessingSizeListBox.Items.RemoveAt(DoNotProcessingSizeListBox.SelectedIndex);
                FreeEcho.FEControls.MessageBox.Show(Common.ApplicationData.Languages.Check, Common.ApplicationData.Languages.LanguagesWindow?.Deleted, FreeEcho.FEControls.MessageBoxButton.Ok, this);
            }
        }
        catch
        {
            FreeEcho.FEControls.MessageBox.Show(Common.ApplicationData.Languages.Check, Common.ApplicationData.Languages.ErrorOccurred, FreeEcho.FEControls.MessageBoxButton.Ok, this);
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
                SettingsTheActiveStateOfTheItemsListBox();
                FreeEcho.FEControls.MessageBox.Show(Common.ApplicationData.Languages.Check, Common.ApplicationData.Languages.LanguagesWindow?.Added, FreeEcho.FEControls.MessageBoxButton.Ok, this);
            }
        }
        catch
        {
            FreeEcho.FEControls.MessageBox.Show(Common.ApplicationData.Languages.Check, Common.ApplicationData.Languages.ErrorOccurred, FreeEcho.FEControls.MessageBoxButton.Ok, this);
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
                FreeEcho.FEControls.MessageBox.Show(Common.ApplicationData.Languages.Check, Common.ApplicationData.Languages.LanguagesWindow?.Modified, FreeEcho.FEControls.MessageBoxButton.Ok, this);
            }
        }
        catch
        {
            FreeEcho.FEControls.MessageBox.Show(Common.ApplicationData.Languages.Check, Common.ApplicationData.Languages.ErrorOccurred, FreeEcho.FEControls.MessageBoxButton.Ok, this);
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
            SettingsValueToControls();
        }
        catch
        {
            FreeEcho.FEControls.MessageBox.Show(Common.ApplicationData.Languages.Check, Common.ApplicationData.Languages.ErrorOccurred, FreeEcho.FEControls.MessageBoxButton.Ok, this);
        }
    }

    /// <summary>
    /// 「ウィンドウ処理」の「アクティブ」Buttonの「Click」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ActiveProcessingButton_Click(
        object sender,
        RoutedEventArgs e
        )
    {
        try
        {
            SpecifiedWindowBaseItemInformation.WindowProcessingInformation[WindowProcessingListBox.SelectedIndex].Active = !SpecifiedWindowBaseItemInformation.WindowProcessingInformation[WindowProcessingListBox.SelectedIndex].Active;
            CheckAndSettingsActiveStateWPI(SpecifiedWindowBaseItemInformation, WindowProcessingListBox.SelectedIndex, GetSelectedDisplayToUseAsStandard(), SpecifiedWindowBaseItemInformation.WindowProcessingInformation[WindowProcessingListBox.SelectedIndex].Active ? WindowProcessingListBox.SelectedIndex : -1);
            SettingsTheActiveStateOfTheItemsListBox();
        }
        catch
        {
            FreeEcho.FEControls.MessageBox.Show(Common.ApplicationData.Languages.Check, Common.ApplicationData.Languages.ErrorOccurred, FreeEcho.FEControls.MessageBoxButton.Ok, this);
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
            if (WindowProcessingListBox.SelectedItems.Count == 1)
            {
                SpecifiedWindowBaseItemInformation.WindowProcessingInformation.RemoveAt(WindowProcessingListBox.SelectedIndex);
                WindowProcessingListBox.Items.RemoveAt(WindowProcessingListBox.SelectedIndex);

                CheckAndSettingsActiveStateWPI(SpecifiedWindowBaseItemInformation, WindowProcessingListBox.SelectedIndex, GetSelectedDisplayToUseAsStandard(), -1);
                SettingsTheActiveStateOfTheItemsListBox();
                FreeEcho.FEControls.MessageBox.Show(Common.ApplicationData.Languages.Check, Common.ApplicationData.Languages.LanguagesWindow?.Deleted, FreeEcho.FEControls.MessageBoxButton.Ok, this);
            }
        }
        catch
        {
            FreeEcho.FEControls.MessageBox.Show(Common.ApplicationData.Languages.Check, Common.ApplicationData.Languages.ErrorOccurred, FreeEcho.FEControls.MessageBoxButton.Ok, this);
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
            if (WindowProcessingListBox.SelectedItems.Count == 1)
            {
                WindowProcessingInformation newWPI = new(SpecifiedWindowBaseItemInformation.WindowProcessingInformation[WindowProcessingListBox.SelectedIndex], false)
                {
                    Active = false
                };
                int number = 1;     // 名前の後ろに付ける番号
                foreach (WindowProcessingInformation nowWPI in SpecifiedWindowBaseItemInformation.WindowProcessingInformation)
                {
                    if (nowWPI.ProcessingName == newWPI.ProcessingName + Common.CopySeparateString + Common.ApplicationData.Languages.LanguagesWindow?.Copy + Common.SpaceSeparateString + number)
                    {
                        number++;
                    }
                }
                newWPI.ProcessingName += Common.CopySeparateString + Common.ApplicationData.Languages.LanguagesWindow?.Copy + Common.SpaceSeparateString + number;
                SpecifiedWindowBaseItemInformation.WindowProcessingInformation.Add(newWPI);
                FreeEcho.FEControls.ListBoxItemValidState newItem = new()
                {
                    Text = newWPI.ProcessingName,
                    Height = ListBoxItemHeight,
                    IsValidState = newWPI.Active
                };
                WindowProcessingListBox.Items.Add(newItem);
                SettingsValueToControls();
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
                if (string.IsNullOrEmpty(TitleNameTextBox.Text))
                {
                    if (string.IsNullOrEmpty(ClassNameTextBox.Text))
                    {
                        if (string.IsNullOrEmpty(FileNameTextBox.Text) == false)
                        {
                            RegisteredNameTextBox.Text = FileNameTextBox.Text;
                        }
                    }
                    else
                    {
                        RegisteredNameTextBox.Text = ClassNameTextBox.Text;
                    }
                }
                else
                {
                    RegisteredNameTextBox.Text = TitleNameTextBox.Text;
                }
            }

            GetTheValueExceptItem();
            string? check = CheckSpecifiedWindowBaseItemInformation(SpecifiedWindowBaseInformation, IndexOfItemToBeModified, SpecifiedWindowBaseItemInformation);
            if (string.IsNullOrEmpty(check))
            {
                bool result = true;     // 結果

                // 追加
                if (IndexOfItemToBeModified == -1)
                {
                    // 項目がない場合は追加する
                    if (SpecifiedWindowBaseItemInformation.WindowProcessingInformation.Count == 0)
                    {
                        if (string.IsNullOrEmpty(ProcessingNameTextBox.Text))
                        {
                            ProcessingNameTextBox.Text = RegisteredNameTextBox.Text;
                        }
                        result = AddWindowProcessingInformation();
                    }
                    if (result)
                    {
                        if (SpecifiedWindowBaseInformation is EventInformation eventInformation)
                        {
                            Common.ApplicationData.WindowProcessingManagement.EventWindowProcessing?.UnregisterHotkeys();
                            eventInformation.Items.Add((EventItemInformation)SpecifiedWindowBaseItemInformation);
                            Common.ApplicationData.WindowProcessingManagement.EventWindowProcessing?.RegisterHotkeys();
                        }
                        else if (SpecifiedWindowBaseInformation is TimerInformation timerInformation)
                        {
                            Common.ApplicationData.WindowProcessingManagement.TimerWindowProcessing?.UnregisterHotkeys();
                            timerInformation.Items.Add((TimerItemInformation)SpecifiedWindowBaseItemInformation);
                            Common.ApplicationData.WindowProcessingManagement.TimerWindowProcessing?.RegisterHotkeys();
                        }
                        FreeEcho.FEControls.MessageBox.Show(Common.ApplicationData.Languages.Check, Common.ApplicationData.Languages.LanguagesWindow?.Added, FreeEcho.FEControls.MessageBoxButton.Ok, this);
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
                        if (string.IsNullOrEmpty(ProcessingNameTextBox.Text))
                        {
                            ProcessingNameTextBox.Text = RegisteredNameTextBox.Text;
                        }
                        result = ModifyWindowProcessingInformation();
                    }
                    if (result)
                    {
                        if (SpecifiedWindowBaseInformation is EventInformation eventInformation)
                        {
                            Common.ApplicationData.WindowProcessingManagement.EventWindowProcessing?.UnregisterHotkeys();
                            eventInformation.Items[IndexOfItemToBeModified] = (EventItemInformation)SpecifiedWindowBaseItemInformation;
                            Common.ApplicationData.WindowProcessingManagement.EventWindowProcessing?.RegisterHotkeys();
                        }
                        else if (SpecifiedWindowBaseInformation is TimerInformation timerInformation)
                        {
                            Common.ApplicationData.WindowProcessingManagement.TimerWindowProcessing?.UnregisterHotkeys();
                            timerInformation.Items[IndexOfItemToBeModified] = (TimerItemInformation)SpecifiedWindowBaseItemInformation;
                            Common.ApplicationData.WindowProcessingManagement.TimerWindowProcessing?.RegisterHotkeys();
                        }
                        FreeEcho.FEControls.MessageBox.Show(Common.ApplicationData.Languages.Check, Common.ApplicationData.Languages.LanguagesWindow?.Modified, FreeEcho.FEControls.MessageBoxButton.Ok, this);
                        AddedOrModified = true;
                        Close();
                    }
                }
            }
            else
            {
                FreeEcho.FEControls.MessageBox.Show(Common.ApplicationData.Languages.Check, check, FreeEcho.FEControls.MessageBoxButton.Ok, this);
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
    /// 「追加/修正」ボタンの有効状態を設定
    /// </summary>
    private void SettingsAddAndModifyButtonEnableState()
    {
        AddOrModifyButton.IsEnabled = !string.IsNullOrEmpty(TitleNameTextBox.Text) || !string.IsNullOrEmpty(ClassNameTextBox.Text) || !string.IsNullOrEmpty(FileNameTextBox.Text);
    }

    /// <summary>
    /// 位置とサイズのコントロールの有効状態を設定
    /// </summary>
    private void SettingsPositionSizeControlEnabled()
    {
        if ((string)((ComboBoxItem)WindowStateComboBox.SelectedItem).Content == Common.ApplicationData.Languages.LanguagesWindow?.NormalWindow)
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
            ProcessingPositionAndSizeTwiceToggleSwitch.IsEnabled = true;
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
            ProcessingPositionAndSizeTwiceToggleSwitch.IsEnabled = false;
        }
    }

    /// <summary>
    /// コントロールから項目以外の値を取得
    /// </summary>
    private void GetTheValueExceptItem()
    {
        string stringData;     // 文字列

        SpecifiedWindowBaseItemInformation.RegisteredName = RegisteredNameTextBox.Text;
        SpecifiedWindowBaseItemInformation.TitleName = TitleNameTextBox.Text;
        stringData = (string)((ComboBoxItem)TitleNameMatchingConditionComboBox.SelectedItem).Content;
        if (stringData == Common.ApplicationData.Languages.LanguagesWindow?.ExactMatch)
        {
            SpecifiedWindowBaseItemInformation.TitleNameMatchCondition = NameMatchCondition.ExactMatch;
        }
        else if (stringData == Common.ApplicationData.Languages.LanguagesWindow?.PartialMatch)
        {
            SpecifiedWindowBaseItemInformation.TitleNameMatchCondition = NameMatchCondition.PartialMatch;
        }
        else if (stringData == Common.ApplicationData.Languages.LanguagesWindow?.ForwardMatch)
        {
            SpecifiedWindowBaseItemInformation.TitleNameMatchCondition = NameMatchCondition.ForwardMatch;
        }
        else if (stringData == Common.ApplicationData.Languages.LanguagesWindow?.BackwardMatch)
        {
            SpecifiedWindowBaseItemInformation.TitleNameMatchCondition = NameMatchCondition.BackwardMatch;
        }
        SpecifiedWindowBaseItemInformation.ClassName = ClassNameTextBox.Text;
        stringData = (string)((ComboBoxItem)ClassNameMatchingConditionComboBox.SelectedItem).Content;
        if (stringData == Common.ApplicationData.Languages.LanguagesWindow?.ExactMatch)
        {
            SpecifiedWindowBaseItemInformation.ClassNameMatchCondition = NameMatchCondition.ExactMatch;
        }
        else if (stringData == Common.ApplicationData.Languages.LanguagesWindow?.PartialMatch)
        {
            SpecifiedWindowBaseItemInformation.ClassNameMatchCondition = NameMatchCondition.PartialMatch;
        }
        else if (stringData == Common.ApplicationData.Languages.LanguagesWindow?.ForwardMatch)
        {
            SpecifiedWindowBaseItemInformation.ClassNameMatchCondition = NameMatchCondition.ForwardMatch;
        }
        else if (stringData == Common.ApplicationData.Languages.LanguagesWindow?.BackwardMatch)
        {
            SpecifiedWindowBaseItemInformation.ClassNameMatchCondition = NameMatchCondition.BackwardMatch;
        }
        SpecifiedWindowBaseItemInformation.FileName = FileNameTextBox.Text;
        stringData = (string)((ComboBoxItem)FileNameMatchingConditionComboBox.SelectedItem).Content;
        if (stringData == Common.ApplicationData.Languages.LanguagesWindow?.IncludePath)
        {
            SpecifiedWindowBaseItemInformation.FileNameMatchCondition = FileNameMatchCondition.Include;
        }
        else if (stringData == Common.ApplicationData.Languages.LanguagesWindow?.DoNotIncludePath)
        {
            SpecifiedWindowBaseItemInformation.FileNameMatchCondition = FileNameMatchCondition.NotInclude;
        }
        stringData = (string)((ComboBoxItem)UntitledWindowConditionsComboBox.SelectedItem).Content;
        if (stringData == Common.ApplicationData.Languages.LanguagesWindow?.DoNotSpecify)
        {
            SpecifiedWindowBaseItemInformation.TitleProcessingConditions = TitleProcessingConditions.NotSpecified;
        }
        else if (stringData == Common.ApplicationData.Languages.LanguagesWindow?.DoNotProcessingUntitledWindow)
        {
            SpecifiedWindowBaseItemInformation.TitleProcessingConditions = TitleProcessingConditions.DoNotProcessingUntitledWindow;
        }
        else if (stringData == Common.ApplicationData.Languages.LanguagesWindow?.DoNotProcessingWindowWithTitle)
        {
            SpecifiedWindowBaseItemInformation.TitleProcessingConditions = TitleProcessingConditions.DoNotProcessingWindowWithTitle;
        }
        stringData = (string)((ComboBoxItem)ProcessOnlyOnceComboBox.SelectedItem).Content;
        if (stringData == Common.ApplicationData.Languages.LanguagesWindow?.DoNotSpecify)
        {
            SpecifiedWindowBaseItemInformation.ProcessingOnlyOnce = ProcessingOnlyOnce.NotSpecified;
        }
        else if (stringData == Common.ApplicationData.Languages.LanguagesWindow?.OnceWindowOpen)
        {
            SpecifiedWindowBaseItemInformation.ProcessingOnlyOnce = ProcessingOnlyOnce.WindowOpen;
        }
        else if (stringData == Common.ApplicationData.Languages.LanguagesWindow?.OnceWhileItIsRunning)
        {
            SpecifiedWindowBaseItemInformation.ProcessingOnlyOnce = ProcessingOnlyOnce.Running;
        }
        SpecifiedWindowBaseItemInformation.StandardDisplay = GetSelectedDisplayToUseAsStandard();
        if (SpecifiedWindowBaseItemInformation is EventItemInformation eventItemInformation)
        {
            eventItemInformation.WindowEventData.Foreground = ForegroundToggleSwitch.IsOn;
            eventItemInformation.WindowEventData.MoveSizeEnd = MoveSizeEndToggleSwitch.IsOn;
            eventItemInformation.WindowEventData.MinimizeStart = MinimizeStartToggleSwitch.IsOn;
            eventItemInformation.WindowEventData.MinimizeEnd = MinimizeEndToggleSwitch.IsOn;
            eventItemInformation.WindowEventData.Create = CreateToggleSwitch.IsOn;
            eventItemInformation.WindowEventData.Show = ShowToggleSwitch.IsOn;
            eventItemInformation.WindowEventData.NameChange = NameChangeToggleSwitch.IsOn;
        }
        else if (SpecifiedWindowBaseItemInformation is TimerItemInformation timerItemInformation)
        {
            timerItemInformation.NumberOfTimesNotToProcessingFirst = DelayNumericUpDown.ValueInt;
        }
        SpecifiedWindowBaseItemInformation.CloseWindow = CloseWindowToggleSwitch.IsOn;
        SpecifiedWindowBaseItemInformation.DifferentVersionVersion = DifferentVersionTextBox.Text;
        SpecifiedWindowBaseItemInformation.DifferentVersionAnnounce = DifferentVersionAnnounceToggleSwitch.IsOn;
    }

    /// <summary>
    /// 「WindowProcessingInformation」を追加
    /// </summary>
    /// <returns>追加に成功したかの値 (失敗「false」/成功「true」)</returns>
    private bool AddWindowProcessingInformation()
    {
        bool result = false;        // 結果
        WindowProcessingInformation newWPI = GetWindowProcessingInformationFromControls();

        string? check = CheckValueWindowProcessingInformation(newWPI, false, SpecifiedWindowBaseItemInformation);
        if (string.IsNullOrEmpty(check))
        {
            CheckAndSettingsInactiveWhenActiveStateChangedWPI(newWPI, SpecifiedWindowBaseItemInformation, GetSelectedDisplayToUseAsStandard());
            SpecifiedWindowBaseItemInformation.WindowProcessingInformation.Add(newWPI);
            FreeEcho.FEControls.ListBoxItemValidState newItem = new()
            {
                Text = newWPI.ProcessingName,
                Height = ListBoxItemHeight,
                IsValidState = newWPI.Active
            };      // 新しいListBoxItemValidState
            WindowProcessingListBox.Items.Add(newItem);
            WindowProcessingListBox.SelectedIndex = WindowProcessingListBox.Items.Count - 1;

            result = true;
        }
        else
        {
            FreeEcho.FEControls.MessageBox.Show(Common.ApplicationData.Languages.Check, check, FreeEcho.FEControls.MessageBoxButton.Ok, this);
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
            WindowProcessingInformation modifyWPI = GetWindowProcessingInformationFromControls();      // 修正した処理項目
            modifyWPI.Active = SpecifiedWindowBaseItemInformation.WindowProcessingInformation[WindowProcessingListBox.SelectedIndex].Active;

            string? check = CheckValueWindowProcessingInformation(modifyWPI, true, SpecifiedWindowBaseItemInformation, WindowProcessingListBox.SelectedIndex);
            if (string.IsNullOrEmpty(check))
            {
                CheckAndSettingsInactiveWhenActiveStateChangedWPI(modifyWPI, SpecifiedWindowBaseItemInformation, GetSelectedDisplayToUseAsStandard(), WindowProcessingListBox.SelectedIndex);
                SpecifiedWindowBaseItemInformation.WindowProcessingInformation[WindowProcessingListBox.SelectedIndex].Copy(modifyWPI);
                FreeEcho.FEControls.ListBoxItemValidState modifyItem = (FreeEcho.FEControls.ListBoxItemValidState)WindowProcessingListBox.Items[WindowProcessingListBox.SelectedIndex];
                modifyItem.Text = modifyWPI.ProcessingName;
                modifyItem.IsValidState = modifyWPI.Active;

                result = true;
            }
            else
            {
                FreeEcho.FEControls.MessageBox.Show(Common.ApplicationData.Languages.Check, check, FreeEcho.FEControls.MessageBoxButton.Ok, this);
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
        if (Common.ApplicationData.Languages.LanguagesWindow == null)
        {
            throw new Exception("LanguagesWindow value is null. - AddModifyWindowSpecifiedWindow.GetWindowProcessingInformationFromControls()");
        }

        WindowProcessingInformation newWPI = new()
        {
            Active = true,
            ProcessingName = ProcessingNameTextBox.Text
        };      // 新しいWindowProcessingInformation

        newWPI.PositionSize.Display = DisplayComboBox.Text;
        string stringData = (string)((ComboBoxItem)WindowStateComboBox.SelectedItem).Content;
        if (stringData == Common.ApplicationData.Languages.LanguagesWindow.DoNotChange)
        {
            newWPI.SettingsWindowState = SettingsWindowState.DoNotChange;
        }
        else if (stringData == Common.ApplicationData.Languages.LanguagesWindow.NormalWindow)
        {
            newWPI.SettingsWindowState = SettingsWindowState.Normal;
        }
        else if (stringData == Common.ApplicationData.Languages.LanguagesWindow.Maximize)
        {
            newWPI.SettingsWindowState = SettingsWindowState.Maximize;
        }
        else if (stringData == Common.ApplicationData.Languages.LanguagesWindow.Minimize)
        {
            newWPI.SettingsWindowState = SettingsWindowState.Minimize;
        }
        newWPI.OnlyNormalWindow = ProcessOnlyWhenNormalWindowToggleSwitch.IsOn;
        if (XComboBox.Text == Common.ApplicationData.Languages.LanguagesWindow.DoNotChange)
        {
            newWPI.PositionSize.XType = WindowXType.DoNotChange;
        }
        else if (XComboBox.Text == Common.ApplicationData.Languages.LanguagesWindow.LeftEdge)
        {
            newWPI.PositionSize.XType = WindowXType.Left;
        }
        else if (XComboBox.Text == Common.ApplicationData.Languages.LanguagesWindow.Middle)
        {
            newWPI.PositionSize.XType = WindowXType.Middle;
        }
        else if (XComboBox.Text == Common.ApplicationData.Languages.LanguagesWindow.RightEdge)
        {
            newWPI.PositionSize.XType = WindowXType.Right;
        }
        else if (XComboBox.Text == Common.ApplicationData.Languages.LanguagesWindow.CoordinateSpecification)
        {
            newWPI.PositionSize.XType = WindowXType.Value;
        }
        newWPI.PositionSize.Position.X = XNumericUpDown.Value;
        stringData = (string)((ComboBoxItem)XTypeComboBox.SelectedItem).Content;
        if (stringData == Common.ApplicationData.Languages.LanguagesWindow.Pixel)
        {
            XNumericUpDown.IsUseDecimal = false;
            newWPI.PositionSize.XValueType = PositionSizeValueType.Pixel;
        }
        else if (stringData == Common.ApplicationData.Languages.LanguagesWindow.Percent)
        {
            XNumericUpDown.IsUseDecimal = true;
            newWPI.PositionSize.XValueType = PositionSizeValueType.Percent;
        }
        if (YComboBox.Text == Common.ApplicationData.Languages.LanguagesWindow.DoNotChange)
        {
            newWPI.PositionSize.YType = WindowYType.DoNotChange;
        }
        else if (YComboBox.Text == Common.ApplicationData.Languages.LanguagesWindow.TopEdge)
        {
            newWPI.PositionSize.YType = WindowYType.Top;
        }
        else if (YComboBox.Text == Common.ApplicationData.Languages.LanguagesWindow.Middle)
        {
            newWPI.PositionSize.YType = WindowYType.Middle;
        }
        else if (YComboBox.Text == Common.ApplicationData.Languages.LanguagesWindow.BottomEdge)
        {
            newWPI.PositionSize.YType = WindowYType.Bottom;
        }
        else if (YComboBox.Text == Common.ApplicationData.Languages.LanguagesWindow.CoordinateSpecification)
        {
            newWPI.PositionSize.YType = WindowYType.Value;
        }
        newWPI.PositionSize.Position.Y = YNumericUpDown.Value;
        stringData = (string)((ComboBoxItem)YTypeComboBox.SelectedItem).Content;
        if (stringData == Common.ApplicationData.Languages.LanguagesWindow.Pixel)
        {
            YNumericUpDown.IsUseDecimal = false;
            newWPI.PositionSize.YValueType = PositionSizeValueType.Pixel;
        }
        else if (stringData == Common.ApplicationData.Languages.LanguagesWindow.Percent)
        {
            YNumericUpDown.IsUseDecimal = true;
            newWPI.PositionSize.YValueType = PositionSizeValueType.Percent;
        }
        if (WidthComboBox.Text == Common.ApplicationData.Languages.LanguagesWindow.DoNotChange)
        {
            newWPI.PositionSize.WidthType = WindowSizeType.DoNotChange;
        }
        else if (WidthComboBox.Text == Common.ApplicationData.Languages.LanguagesWindow.WidthSpecification)
        {
            newWPI.PositionSize.WidthType = WindowSizeType.Value;
        }
        newWPI.PositionSize.Size.Width = WidthNumericUpDown.Value;
        stringData = (string)((ComboBoxItem)WidthTypeComboBox.SelectedItem).Content;
        if (stringData == Common.ApplicationData.Languages.LanguagesWindow.Pixel)
        {
            WidthNumericUpDown.IsUseDecimal = false;
            newWPI.PositionSize.WidthValueType = PositionSizeValueType.Pixel;
        }
        else if (stringData == Common.ApplicationData.Languages.LanguagesWindow.Percent)
        {
            WidthNumericUpDown.IsUseDecimal = true;
            newWPI.PositionSize.WidthValueType = PositionSizeValueType.Percent;
        }
        if (HeightComboBox.Text == Common.ApplicationData.Languages.LanguagesWindow.DoNotChange)
        {
            newWPI.PositionSize.HeightType = WindowSizeType.DoNotChange;
        }
        else if (HeightComboBox.Text == Common.ApplicationData.Languages.LanguagesWindow.HeightSpecification)
        {
            newWPI.PositionSize.HeightType = WindowSizeType.Value;
        }
        newWPI.PositionSize.Size.Height = HeightNumericUpDown.Value;
        stringData = (string)((ComboBoxItem)HeightTypeComboBox.SelectedItem).Content;
        if (stringData == Common.ApplicationData.Languages.LanguagesWindow.Pixel)
        {
            HeightNumericUpDown.IsUseDecimal = false;
            newWPI.PositionSize.HeightValueType = PositionSizeValueType.Pixel;
        }
        else if (stringData == Common.ApplicationData.Languages.LanguagesWindow.Percent)
        {
            HeightNumericUpDown.IsUseDecimal = true;
            newWPI.PositionSize.HeightValueType = PositionSizeValueType.Percent;
        }
        newWPI.PositionSize.ClientArea = ClientAreaToggleSwitch.IsOn;
        newWPI.PositionSize.ProcessingPositionAndSizeTwice = ProcessingPositionAndSizeTwiceToggleSwitch.IsOn;
        if (ForefrontComboBox.Text == Common.ApplicationData.Languages.LanguagesWindow.DoNotChange)
        {
            newWPI.Forefront = Forefront.DoNotChange;
        }
        else if (ForefrontComboBox.Text == Common.ApplicationData.Languages.LanguagesWindow.AlwaysForefront)
        {
            newWPI.Forefront = Forefront.Always;
        }
        else if (ForefrontComboBox.Text == Common.ApplicationData.Languages.LanguagesWindow.AlwaysCancelForefront)
        {
            newWPI.Forefront = Forefront.Cancel;
        }
        else if (ForefrontComboBox.Text == Common.ApplicationData.Languages.LanguagesWindow.Forefront)
        {
            newWPI.Forefront = Forefront.Forefront;
        }
        newWPI.EnabledTransparency = SpecifyTransparencyToggleSwitch.IsOn;
        newWPI.Transparency = (int)TransparencyNumericUpDown.Value;
        newWPI.Hotkey.Copy(HotkeyInformation);

        return newWPI;
    }

    /// <summary>
    /// 「ウィンドウ処理」ListBoxの項目の「IsValidState」を設定
    /// </summary>
    private void SettingsTheActiveStateOfTheItemsListBox()
    {
        for (int count = 0; count < WindowProcessingListBox.Items.Count; count++)
        {
            ((FreeEcho.FEControls.ListBoxItemValidState)(WindowProcessingListBox.Items[count])).IsValidState = SpecifiedWindowBaseItemInformation.WindowProcessingInformation[count].Active;
        }
    }

    /// <summary>
    /// 「ウィンドウ処理」のコントロールに値を設定
    /// </summary>
    private void SettingsValueToControls()
    {
        if (Common.ApplicationData.Languages.LanguagesWindow == null)
        {
            throw new Exception("LanguagesWindow value is null. - AddModifyWindowSpecifiedWindow.SettingsValueToControls()");
        }

        WindowProcessingInformation settingsWPI = (WindowProcessingListBox.SelectedItems.Count == 0) ? new() : SpecifiedWindowBaseItemInformation.WindowProcessingInformation[WindowProcessingListBox.SelectedIndex];        // 設定するウィンドウの処理情報

        if (WindowProcessingListBox.SelectedItems.Count == 1)
        {
            ActiveProcessingButton.IsEnabled = true;
            DeleteProcessingButton.IsEnabled = true;
            ModifyProcessingButton.IsEnabled = true;
            CopyProcessingButton.IsEnabled = true;
            HotkeyInformation.Copy(SpecifiedWindowBaseItemInformation.WindowProcessingInformation[WindowProcessingListBox.SelectedIndex].Hotkey);
        }
        else
        {
            ActiveProcessingButton.IsEnabled = false;
            DeleteProcessingButton.IsEnabled = false;
            ModifyProcessingButton.IsEnabled = false;
            CopyProcessingButton.IsEnabled = false;
            HotkeyInformation.Copy(new());
        }
        ProcessingNameTextBox.Text = settingsWPI.ProcessingName;
        Processing.SelectComboBoxItem(DisplayComboBox, settingsWPI.PositionSize.Display);
        string stringData;
        stringData = settingsWPI.SettingsWindowState switch
        {
            SettingsWindowState.Normal => Common.ApplicationData.Languages.LanguagesWindow.NormalWindow,
            SettingsWindowState.Maximize => Common.ApplicationData.Languages.LanguagesWindow.Maximize,
            SettingsWindowState.Minimize => Common.ApplicationData.Languages.LanguagesWindow.Minimize,
            _ => Common.ApplicationData.Languages.LanguagesWindow.DoNotChange
        };
        Processing.SelectComboBoxItem(WindowStateComboBox, stringData);
        ProcessOnlyWhenNormalWindowToggleSwitch.IsOn = settingsWPI.OnlyNormalWindow;
        stringData = settingsWPI.PositionSize.XType switch
        {
            WindowXType.Left => Common.ApplicationData.Languages.LanguagesWindow.LeftEdge,
            WindowXType.Middle => Common.ApplicationData.Languages.LanguagesWindow.Middle,
            WindowXType.Right => Common.ApplicationData.Languages.LanguagesWindow.RightEdge,
            WindowXType.Value => Common.ApplicationData.Languages.LanguagesWindow.CoordinateSpecification,
            _ => Common.ApplicationData.Languages.LanguagesWindow.DoNotChange
        };
        Processing.SelectComboBoxItem(XComboBox, stringData);
        switch (settingsWPI.PositionSize.XType)
        {
            case WindowXType.Value:
                XNumericUpDown.IsEnabled = true;
                XTypeComboBox.IsEnabled = true;
                break;
            default:
                XNumericUpDown.IsEnabled = false;
                XTypeComboBox.IsEnabled = false;
                break;
        }
        switch (settingsWPI.PositionSize.XValueType)
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
        XNumericUpDown.Value = settingsWPI.PositionSize.Position.X;
        stringData = settingsWPI.PositionSize.YType switch
        {
            WindowYType.Top => Common.ApplicationData.Languages.LanguagesWindow.TopEdge,
            WindowYType.Middle => Common.ApplicationData.Languages.LanguagesWindow.Middle,
            WindowYType.Bottom => Common.ApplicationData.Languages.LanguagesWindow.BottomEdge,
            WindowYType.Value => Common.ApplicationData.Languages.LanguagesWindow.CoordinateSpecification,
            _ => Common.ApplicationData.Languages.LanguagesWindow.DoNotChange
        };
        Processing.SelectComboBoxItem(YComboBox, stringData);
        switch (settingsWPI.PositionSize.YType)
        {
            case WindowYType.Value:
                YNumericUpDown.IsEnabled = true;
                YTypeComboBox.IsEnabled = true;
                break;
            default:
                YNumericUpDown.IsEnabled = false;
                YTypeComboBox.IsEnabled = false;
                break;
        }
        switch (settingsWPI.PositionSize.YValueType)
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
        YNumericUpDown.Value = settingsWPI.PositionSize.Position.Y;
        switch (settingsWPI.PositionSize.WidthType)
        {
            case WindowSizeType.Value:
                stringData = Common.ApplicationData.Languages.LanguagesWindow.WidthSpecification;
                WidthNumericUpDown.IsEnabled = true;
                WidthTypeComboBox.IsEnabled = true;
                break;
            default:
                stringData = Common.ApplicationData.Languages.LanguagesWindow.DoNotChange;
                WidthNumericUpDown.IsEnabled = false;
                WidthTypeComboBox.IsEnabled = false;
                break;
        }
        Processing.SelectComboBoxItem(WidthComboBox, stringData);
        switch (settingsWPI.PositionSize.WidthValueType)
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
        WidthNumericUpDown.Value = settingsWPI.PositionSize.Size.Width;
        switch (settingsWPI.PositionSize.HeightType)
        {
            case WindowSizeType.Value:
                stringData = Common.ApplicationData.Languages.LanguagesWindow.HeightSpecification;
                HeightNumericUpDown.IsEnabled = true;
                HeightTypeComboBox.IsEnabled = true;
                break;
            default:
                stringData = Common.ApplicationData.Languages.LanguagesWindow.DoNotChange;
                HeightNumericUpDown.IsEnabled = false;
                HeightTypeComboBox.IsEnabled = false;
                break;
        }
        Processing.SelectComboBoxItem(HeightComboBox, stringData);
        switch (settingsWPI.PositionSize.HeightValueType)
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
        HeightNumericUpDown.Value = settingsWPI.PositionSize.Size.Height;
        SettingsPositionSizeControlEnabled();
        ClientAreaToggleSwitch.IsOn = settingsWPI.PositionSize.ClientArea;
        ProcessingPositionAndSizeTwiceToggleSwitch.IsOn = settingsWPI.PositionSize.ProcessingPositionAndSizeTwice;
        stringData = settingsWPI.Forefront switch
        {
            Forefront.Always => Common.ApplicationData.Languages.LanguagesWindow.AlwaysForefront,
            Forefront.Cancel => Common.ApplicationData.Languages.LanguagesWindow.AlwaysCancelForefront,
            Forefront.Forefront => Common.ApplicationData.Languages.LanguagesWindow.Forefront,
            _ => Common.ApplicationData.Languages.LanguagesWindow.DoNotChange
        };
        Processing.SelectComboBoxItem(ForefrontComboBox, stringData);
        SpecifyTransparencyToggleSwitch.IsOn = settingsWPI.EnabledTransparency;
        TransparencyNumericUpDown.IsEnabled = settingsWPI.EnabledTransparency;
        TransparencyNumericUpDown.ValueInt = settingsWPI.Transparency;
        HotkeyTextBox.Text = FreeEcho.FEHotKeyWPF.HotKeyWPF.GetHotKeyString(settingsWPI.Hotkey);
    }

    /// <summary>
    /// ウィンドウハンドルから情報を取得してコントロールに値を設定
    /// </summary>
    /// <param name="hwnd">ウィンドウハンドル</param>
    private void GetInformationFromWindowHandle(
        IntPtr hwnd
        )
    {
        WindowInformation windowInformation = WindowProcessing.GetWindowInformation(hwnd, WindowInformationBuffer);
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
            FileNameTextBox.Text = (string)((ComboBoxItem)FileNameMatchingConditionComboBox.SelectedItem).Content == Common.ApplicationData.Languages.LanguagesWindow?.IncludePath ? windowInformation.FileName : Path.GetFileNameWithoutExtension(windowInformation.FileName);
        }
        try
        {
            FileVersionInfo info = FileVersionInfo.GetVersionInfo(windowInformation.FileName);
            DifferentVersionTextBox.Text = info.ProductVersion;
        }
        catch
        {
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
            if (Common.ApplicationData.Languages.LanguagesWindow == null)
            {
                throw new Exception("LanguagesWindow value is null. - AddModifyWindowSpecifiedWindow.GetPositionSizeFromHandle()");
            }

            MonitorInfoEx monitorInfo;
            MonitorInformation.GetMonitorWithWindow(hwnd, out monitorInfo);

            if (DisplayCheckBox.IsChecked == true)
            {
                Processing.SelectComboBoxItem(DisplayComboBox, monitorInfo.DeviceName);
            }
            if (WindowStateCheckBox.IsChecked == true)
            {
                WINDOWPLACEMENT windowPlacement;
                NativeMethods.GetWindowPlacement(hwnd, out windowPlacement);
                string stringData = windowPlacement.showCmd switch
                {
                    (int)SW.SW_SHOWMAXIMIZED => Common.ApplicationData.Languages.LanguagesWindow.Maximize,
                    (int)SW.SW_SHOWMINIMIZED => Common.ApplicationData.Languages.LanguagesWindow.Minimize,
                    _ => Common.ApplicationData.Languages.LanguagesWindow.NormalWindow,
                };
                Processing.SelectComboBoxItem(WindowStateComboBox, stringData);
            }
            SettingsPositionSizeControlEnabled();
            System.Drawing.Point point = new(0, 0);
            switch (Common.ApplicationData.Settings.CoordinateType)
            {
                case CoordinateType.Display:
                    point.X = monitorInfo.WorkArea.Left;
                    point.Y = monitorInfo.WorkArea.Top;
                    break;
            }
            RECT rect;
            NativeMethods.GetWindowRect(hwnd, out rect);
            if (XCheckBox.IsChecked == true)
            {
                Processing.SelectComboBoxItem(XComboBox, Common.ApplicationData.Languages.LanguagesWindow.CoordinateSpecification);
                XTypeComboBox.SelectedIndex = 0;
                XNumericUpDown.IsUseDecimal = false;
                XNumericUpDown.Value = rect.Left - point.X;
            }
            if (YCheckBox.IsChecked == true)
            {
                Processing.SelectComboBoxItem(YComboBox, Common.ApplicationData.Languages.LanguagesWindow.CoordinateSpecification);
                YTypeComboBox.SelectedIndex = 0;
                YNumericUpDown.IsUseDecimal = false;
                YNumericUpDown.Value = rect.Top - point.Y;
            }
            if (WidthCheckBox.IsChecked == true)
            {
                Processing.SelectComboBoxItem(WidthComboBox, Common.ApplicationData.Languages.LanguagesWindow.WidthSpecification);
                WidthTypeComboBox.SelectedIndex = 0;
                WidthNumericUpDown.IsUseDecimal = false;
                WidthNumericUpDown.Value = rect.Right - rect.Left;
            }
            if (HeightCheckBox.IsChecked == true)
            {
                Processing.SelectComboBoxItem(HeightComboBox, Common.ApplicationData.Languages.LanguagesWindow.HeightSpecification);
                HeightTypeComboBox.SelectedIndex = 0;
                HeightNumericUpDown.IsUseDecimal = false;
                HeightNumericUpDown.Value = rect.Bottom - rect.Top;
            }
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「X」コントロールの「IsEnabled」を設定
    /// </summary>
    private void SettingsTheEnabledStateOfTheXControls()
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
    /// 「Y」コントロールの「IsEnabled」を設定
    /// </summary>
    private void SettingsTheEnabledStateOfTheYControls()
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
    /// 「幅」コントロールの「IsEnabled」を設定
    /// </summary>
    private void SettingsTheEnabledStateOfTheWidthControls()
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
    /// 「高さ」のコントロールの「IsEnabled」を設定
    /// </summary>
    private void SettingsTheEnabledStateOfTheHeightControls()
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
    /// 選択している「基準にするディスプレイ」をコントロールから取得
    /// </summary>
    /// <returns>基準にするディスプレイ</returns>
    private StandardDisplay GetSelectedDisplayToUseAsStandard()
    {
        StandardDisplay selected = StandardDisplay.DisplayWithWindow;
        string stringData = (string)((ComboBoxItem)StandardDisplayComboBox.SelectedItem).Content;

        if (stringData == Common.ApplicationData.Languages.LanguagesWindow?.DisplayWithWindow)
        {
            selected = StandardDisplay.DisplayWithWindow;
        }
        else if (stringData == Common.ApplicationData.Languages.LanguagesWindow?.TheSpecifiedDisplay)
        {
            selected = StandardDisplay.SpecifiedDisplay;
        }
        else if (stringData == Common.ApplicationData.Languages.LanguagesWindow?.OnlyIfItIsOnTheSpecifiedDisplay)
        {
            selected = StandardDisplay.OnlySpecifiedDisplay;
        }

        return selected;
    }

    /// <summary>
    /// 「WindowProcessingInformation」が複数アクティブにできるか確認
    /// </summary>
    /// <param name="standardDisplay">選択している「StandardDisplay」</param>
    /// <returns>複数アクティブにできるかの値 (いいえ「false」/はい「true」)</returns>
    private bool CheckMultipleActivationWindowProcessingInformation(
        StandardDisplay standardDisplay
        )
    {
        bool result = false;
        if (standardDisplay == StandardDisplay.OnlySpecifiedDisplay
            && Common.ApplicationData.Settings.CoordinateType == CoordinateType.Display)
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
    /// <param name="specifiedWindowBaseItemInformation">SpecifiedWindowBaseItemInformation</param>
    /// <param name="itemSelectedIndex">選択している項目のインデックス (追加「-1」)</param>
    /// <returns>値に問題ないかの値 (問題がある「メッセージ内容」/問題ない「null」)</returns>
    private string? CheckValueWindowProcessingInformation(
        WindowProcessingInformation wpi,
        bool additionOrModify,
        SpecifiedWindowBaseItemInformation specifiedWindowBaseItemInformation,
        int itemSelectedIndex = -1
        )
    {
        string? result = null;     // 結果

        if (string.IsNullOrEmpty(wpi.ProcessingName))
        {
            result = Common.ApplicationData.Languages.ErrorOccurred;
        }
        else
        {
            // 項目名の重複確認
            if (additionOrModify)
            {
                int count = 0;
                foreach (WindowProcessingInformation nowWPI in specifiedWindowBaseItemInformation.WindowProcessingInformation)
                {
                    if (count != itemSelectedIndex && nowWPI.ProcessingName == wpi.ProcessingName)
                    {
                        result = Common.ApplicationData.Languages.LanguagesWindow?.ThereIsADuplicateProcessingName;
                        break;
                    }
                    count++;
                }
            }
            else
            {
                foreach (WindowProcessingInformation nowWPI in specifiedWindowBaseItemInformation.WindowProcessingInformation)
                {
                    if (nowWPI.ProcessingName == wpi.ProcessingName)
                    {
                        result = Common.ApplicationData.Languages.LanguagesWindow?.ThereIsADuplicateProcessingName;
                        break;
                    }
                }
            }
        }

        return result;
    }

    /// <summary>
    /// 「SpecifiedWindowBaseItemInformation」の値を確認
    /// </summary>
    /// <param name="specifiedWindowBaseInformation">SpecifiedWindowBaseInformation</param>
    /// <param name="indexOfItemToBeModify">修正する項目のインデックス (追加「-1」)</param>
    /// <param name="specifiedWindowBaseItemInformation">SpecifiedWindowBaseItemInformation</param>
    /// <returns>結果の文字列 (問題ない「null」/問題がある「問題がある理由の文字列」)</returns>
    private string? CheckSpecifiedWindowBaseItemInformation(
        SpecifiedWindowBaseInformation specifiedWindowBaseInformation,
        int indexOfItemToBeModify,
        SpecifiedWindowBaseItemInformation specifiedWindowBaseItemInformation
        )
    {
        string? result = null;     // 結果

        // 登録名の重複確認
        if (specifiedWindowBaseInformation is EventInformation eventInformation)
        {
            if (indexOfItemToBeModify == -1)
            {
                foreach (EventItemInformation nowItem in eventInformation.Items)
                {
                    if (specifiedWindowBaseItemInformation.RegisteredName == nowItem.RegisteredName)
                    {
                        result = Common.ApplicationData.Languages.LanguagesWindow?.ThereIsADuplicateRegistrationName;
                        break;
                    }
                }
            }
            else
            {
                int count = 0;
                foreach (EventItemInformation nowItem in eventInformation.Items)
                {
                    if (count != indexOfItemToBeModify
                        && specifiedWindowBaseItemInformation.RegisteredName == nowItem.RegisteredName)
                    {
                        result = Common.ApplicationData.Languages.LanguagesWindow?.ThereIsADuplicateRegistrationName;
                        break;
                    }
                    count++;
                }
            }
        }
        else if (specifiedWindowBaseInformation is TimerInformation timerInformation)
        {
            if (indexOfItemToBeModify == -1)
            {
                foreach (TimerItemInformation nowItem in timerInformation.Items)
                {
                    if (specifiedWindowBaseItemInformation.RegisteredName == nowItem.RegisteredName)
                    {
                        result = Common.ApplicationData.Languages.LanguagesWindow?.ThereIsADuplicateRegistrationName;
                        break;
                    }
                }
            }
            else
            {
                int count = 0;
                foreach (TimerItemInformation nowItem in timerInformation.Items)
                {
                    if (count != indexOfItemToBeModify
                        && specifiedWindowBaseItemInformation.RegisteredName == nowItem.RegisteredName)
                    {
                        result = Common.ApplicationData.Languages.LanguagesWindow?.ThereIsADuplicateRegistrationName;
                        break;
                    }
                    count++;
                }
            }
        }

        return result;
    }

    /// <summary>
    /// 「WindowProcessingInformation」のアクティブ状態の確認と設定
    /// <para>アクティブ項目がない場合は最初の項目を有効化、複数アクティブにできない条件の場合はどちらかの項目を無効化。</para>
    /// </summary>
    /// <param name="specifiedWindowBaseItemInformation">SpecifiedWindowBaseItemInformation</param>
    /// <param name="windowProcessingItemSelectedIndex">ウィンドウ処理の選択している項目のインデックス</param>
    /// <param name="standardDisplay">選択している「StandardDisplay」</param>
    /// <param name="activeChangeIndex">アクティブにした項目のインデックス (アクティブにしてない「-1」)</param>
    private void CheckAndSettingsActiveStateWPI(
        SpecifiedWindowBaseItemInformation specifiedWindowBaseItemInformation,
        int windowProcessingItemSelectedIndex,
        StandardDisplay standardDisplay,
        int activeChangeIndex = -1
        )
    {
        // 項目のアクティブ状態を変更
        if (specifiedWindowBaseItemInformation.WindowProcessingInformation.Count != 0)
        {
            bool activeCheck = false;      // アクティブ項目があるかの値

            // アクティブ項目があるか調べる
            foreach (WindowProcessingInformation nowWPI in specifiedWindowBaseItemInformation.WindowProcessingInformation)
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
                        for (int count1 = 0; count1 < specifiedWindowBaseItemInformation.WindowProcessingInformation.Count - 1; count1++)
                        {
                            for (int count2 = count1 + 1; count2 < specifiedWindowBaseItemInformation.WindowProcessingInformation.Count; count2++)
                            {
                                if (specifiedWindowBaseItemInformation.WindowProcessingInformation[count1].PositionSize.Display == specifiedWindowBaseItemInformation.WindowProcessingInformation[count2].PositionSize.Display
                                    && specifiedWindowBaseItemInformation.WindowProcessingInformation[count1].Active
                                    && specifiedWindowBaseItemInformation.WindowProcessingInformation[count2].Active)
                                {
                                    specifiedWindowBaseItemInformation.WindowProcessingInformation[count2].Active = false;
                                }
                            }
                        }
                    }
                    else
                    {
                        int count = 0;      // カウント

                        foreach (WindowProcessingInformation nowWPI in specifiedWindowBaseItemInformation.WindowProcessingInformation)
                        {
                            if (nowWPI.PositionSize.Display == specifiedWindowBaseItemInformation.WindowProcessingInformation[windowProcessingItemSelectedIndex].PositionSize.Display
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
                        for (int count1 = 0; count1 < specifiedWindowBaseItemInformation.WindowProcessingInformation.Count; count1++)
                        {
                            if (specifiedWindowBaseItemInformation.WindowProcessingInformation[count1].Active)
                            {
                                if (check)
                                {
                                    specifiedWindowBaseItemInformation.WindowProcessingInformation[count1].Active = false;
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
                        for (int count1 = 0; count1 < specifiedWindowBaseItemInformation.WindowProcessingInformation.Count; count1++)
                        {
                            if (specifiedWindowBaseItemInformation.WindowProcessingInformation[count1].Active && activeChangeIndex != count1)
                            {
                                specifiedWindowBaseItemInformation.WindowProcessingInformation[count1].Active = false;
                            }
                        }
                    }
                }
            }
            // アクティブ項目がない場合は最初の項目をアクティブにする
            else
            {
                specifiedWindowBaseItemInformation.WindowProcessingInformation[0].Active = true;
            }
        }
    }

    /// <summary>
    /// 「WindowProcessingInformation」のアクティブ状態が変更された場合に他の項目を非アクティブにするかの確認と設定
    /// </summary>
    /// <param name="wpi">WindowProcessingInformation</param>
    /// <param name="specifiedWindowBaseItemInformation">SpecifiedWindowBaseItemInformation</param>
    /// <param name="standardDisplay">選択している「StandardDisplay」</param>
    /// <param name="modifyItemIndex">修正した項目のインデックス (追加「-1」)</param>
    private void CheckAndSettingsInactiveWhenActiveStateChangedWPI(
        WindowProcessingInformation wpi,
        SpecifiedWindowBaseItemInformation specifiedWindowBaseItemInformation,
        StandardDisplay standardDisplay,
        int modifyItemIndex = -1
        )
    {
        if (CheckMultipleActivationWindowProcessingInformation(standardDisplay))
        {
            for (int count = 0; count < specifiedWindowBaseItemInformation.WindowProcessingInformation.Count; count++)
            {
                if (specifiedWindowBaseItemInformation.WindowProcessingInformation[count].PositionSize.Display == wpi.PositionSize.Display
                    && (modifyItemIndex == -1 || (specifiedWindowBaseItemInformation.WindowProcessingInformation[count].Active && count != modifyItemIndex)))
                {
                    wpi.Active = false;
                    break;
                }
            }
        }
        else
        {
            for (int count = 0; count < specifiedWindowBaseItemInformation.WindowProcessingInformation.Count; count++)
            {
                if (specifiedWindowBaseItemInformation.WindowProcessingInformation[count].Active
                    && (modifyItemIndex == -1 || count != modifyItemIndex))
                {
                    wpi.Active = false;
                    break;
                }
            }
        }
    }

    /// <summary>
    /// 「処理しない条件」の「タイトル名の除外文字列」の値を確認
    /// </summary>
    /// <param name="specifiedWindowBaseItemInformation">SpecifiedWindowBaseItemInformation</param>
    /// <param name="titleName">確認するタイトル名の文字列</param>
    /// <returns>結果の文字列 (問題ない「null」/問題がある「理由の文字列 (メッセージボックスに表示する文字列)」)</returns>
    private string? CheckDoNotProcessingTitleName(
        SpecifiedWindowBaseItemInformation specifiedWindowBaseItemInformation,
        string titleName
        )
    {
        string? result = null;

        foreach (string nowTitleName in specifiedWindowBaseItemInformation.DoNotProcessingTitleName)
        {
            if (nowTitleName == titleName)
            {
                result = Common.ApplicationData.Languages.LanguagesWindow?.ThereAreDuplicateItems;
                break;
            }
        }

        return result;
    }

    /// <summary>
    /// 「処理しない条件」の「サイズ」の値を確認
    /// </summary>
    /// <param name="specifiedWindowBaseItemInformation">SpecifiedWindowBaseItemInformation</param>
    /// <param name="size">確認するサイズ</param>
    /// <returns>結果の文字列 (問題ない「null」/問題がある「理由の文字列 (メッセージボックスに表示する文字列)」)</returns>
    private string? CheckDoNotProcessingSize(
        SpecifiedWindowBaseItemInformation specifiedWindowBaseItemInformation,
        System.Drawing.Size size
        )
    {
        string? result = null;

        foreach (System.Drawing.Size nowSize in specifiedWindowBaseItemInformation.DoNotProcessingSize)
        {
            if (nowSize.Width == size.Width
                && nowSize.Height == size.Height)
            {
                result = Common.ApplicationData.Languages.LanguagesWindow?.ThereAreDuplicateItems;
                break;
            }
        }

        return result;
    }
}
