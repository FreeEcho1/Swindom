using Swindom.Source.Settings;

namespace Swindom.Source.Window
{
    /// <summary>
    /// 「指定ウィンドウ」 (「イベント」、「タイマー」) の「追加/修正」ウィンドウ
    /// </summary>
    public partial class AddModifyWindowSpecifiedWindow : System.Windows.Window
    {
        /// <summary>
        /// 修正する項目のインデックス (追加「-1」)
        /// </summary>
        private readonly int IndexOfItemToBeModified = -1;
        /// <summary>
        /// 追加/修正したかの値 (いいえ「false」/はい「true」)
        /// </summary>
        public bool AddedOrModified;
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
        private System.Timers.Timer WindowInformationAcquisitionTimer;
        /// <summary>
        /// ウィンドウ選択枠
        /// </summary>
        private FreeEcho.FEWindowSelectionMouse.WindowSelectionFrame WindowSelectionMouse;
        /// <summary>
        /// 最小化前のオーナーウィンドウの状態
        /// </summary>
        private System.Windows.WindowState PreviousOwnerWindowState = System.Windows.WindowState.Normal;

        /// <summary>
        /// ウィンドウ情報取得の待ち時間
        /// </summary>
        private const int WaitTimeForWindowInformationAcquisition = 5000;
        /// <summary>
        /// ListBoxItemの高さ
        /// </summary>
        private const int ListBoxItemHeight = 30;

        /// <summary>
        /// コンストラクタ (使用しない)
        /// </summary>
        public AddModifyWindowSpecifiedWindow()
        {
            throw new System.Exception("Do not use. - AddModifyWindowSpecifiedWindow()");
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="ownerWindow">オーナーウィンドウ</param>
        /// <param name="specifiedWindowBaseInformation">「指定ウィンドウ」機能の基礎情報</param>
        /// <param name="indexOfItemToBeModified">修正する項目のインデックス (追加「-1」)</param>
        public AddModifyWindowSpecifiedWindow(
            System.Windows.Window ownerWindow,
            SpecifiedWindowBaseInformation specifiedWindowBaseInformation,
            int indexOfItemToBeModified = -1
            )
        {
            InitializeComponent();

            SpecifiedWindowBaseInformation = specifiedWindowBaseInformation;
            IndexOfItemToBeModified = indexOfItemToBeModified;
            Owner = ownerWindow;
            if (SpecifiedWindowBaseInformation is EventInformation isEventInformation)
            {
                SpecifiedWindowBaseItemInformation = (IndexOfItemToBeModified == -1) ? new EventItemInformation() : new EventItemInformation(isEventInformation.Items[IndexOfItemToBeModified]);
                TimerStackPanel.Visibility = System.Windows.Visibility.Collapsed;
            }
            else if (SpecifiedWindowBaseInformation is TimerInformation isTimerInformation)
            {
                SpecifiedWindowBaseItemInformation = (IndexOfItemToBeModified == -1) ? new TimerItemInformation() : new TimerItemInformation(isTimerInformation.Items[IndexOfItemToBeModified]);
                EventGroupBox.Visibility = System.Windows.Visibility.Collapsed;
            }
            if (SpecifiedWindowBaseInformation.RegisterMultiple == false)
            {
                AddAndModifyProcessingGrid.Visibility = System.Windows.Visibility.Collapsed;
                ProcessingNameLabel.Visibility = System.Windows.Visibility.Collapsed;
                ProcessingNameTextBox.Visibility = System.Windows.Visibility.Collapsed;
                MinWidth -= ProcessingListColumnDefinition.MinWidth;
                MaxWidth -= ProcessingListColumnDefinition.MinWidth;
                ProcessingListGrid.Visibility = System.Windows.Visibility.Collapsed;
                ProcessingListColumnDefinition.MinWidth = 0;
                ProcessingListColumnDefinition.Width = new System.Windows.GridLength(0);
            }
            MonitorInformation monitorInformation = MonitorInformation.GetMonitorInformation();        // モニター情報
            if ((Common.ApplicationData.Settings.CoordinateType == CoordinateType.Global)
                || (monitorInformation.MonitorInfo.Count == 1))
            {
                DisplayCheckBox.Visibility = System.Windows.Visibility.Collapsed;
                StandardDisplayLabel.Visibility = System.Windows.Visibility.Collapsed;
                ProcessOnlyOnceLabel.Margin = new(0);
                StandardDisplayComboBox.Visibility = System.Windows.Visibility.Collapsed;
                DisplayLabel.Visibility = System.Windows.Visibility.Collapsed;
                DisplayComboBox.Visibility = System.Windows.Visibility.Collapsed;
            }
            SizeToContent = System.Windows.SizeToContent.Manual;
            Width = (SpecifiedWindowBaseInformation.AddModifyWindowSize.Width < MinWidth) ? MinWidth : SpecifiedWindowBaseInformation.AddModifyWindowSize.Width;
            Height = (SpecifiedWindowBaseInformation.AddModifyWindowSize.Height < 500) ? 500 : SpecifiedWindowBaseInformation.AddModifyWindowSize.Height;
            InformationToRetrieveExpander.IsExpanded = false;
            foreach (MonitorInfoEx nowMonitorInfo in monitorInformation.MonitorInfo)
            {
                System.Windows.Controls.ComboBoxItem newItem = new()
                {
                    Content = nowMonitorInfo.DeviceName
                };
                DisplayComboBox.Items.Add(newItem);
            }

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
            ((System.Windows.Controls.ComboBoxItem)TitleNameMatchingConditionComboBox.Items[0]).Content = Common.ApplicationData.Languages.LanguagesWindow.ExactMatch;
            ((System.Windows.Controls.ComboBoxItem)TitleNameMatchingConditionComboBox.Items[1]).Content = Common.ApplicationData.Languages.LanguagesWindow.PartialMatch;
            ((System.Windows.Controls.ComboBoxItem)TitleNameMatchingConditionComboBox.Items[2]).Content = Common.ApplicationData.Languages.LanguagesWindow.ForwardMatch;
            ((System.Windows.Controls.ComboBoxItem)TitleNameMatchingConditionComboBox.Items[3]).Content = Common.ApplicationData.Languages.LanguagesWindow.BackwardMatch;
            ClassNameLabel.Content = Common.ApplicationData.Languages.LanguagesWindow.ClassName;
            ((System.Windows.Controls.ComboBoxItem)ClassNameMatchingConditionComboBox.Items[0]).Content = Common.ApplicationData.Languages.LanguagesWindow.ExactMatch;
            ((System.Windows.Controls.ComboBoxItem)ClassNameMatchingConditionComboBox.Items[1]).Content = Common.ApplicationData.Languages.LanguagesWindow.PartialMatch;
            ((System.Windows.Controls.ComboBoxItem)ClassNameMatchingConditionComboBox.Items[2]).Content = Common.ApplicationData.Languages.LanguagesWindow.ForwardMatch;
            ((System.Windows.Controls.ComboBoxItem)ClassNameMatchingConditionComboBox.Items[3]).Content = Common.ApplicationData.Languages.LanguagesWindow.BackwardMatch;
            FileNameLabel.Content = Common.ApplicationData.Languages.LanguagesWindow.FileName;
            FileNameFileSelectionButton.ToolTip = Common.ApplicationData.Languages.LanguagesWindow.FileSelection;
            ((System.Windows.Controls.ComboBoxItem)FileNameMatchingConditionComboBox.Items[0]).Content = Common.ApplicationData.Languages.LanguagesWindow.IncludePath;
            ((System.Windows.Controls.ComboBoxItem)FileNameMatchingConditionComboBox.Items[1]).Content = Common.ApplicationData.Languages.LanguagesWindow.DoNotIncludePath;
            UntitledWindowConditionsLabel.Content = Common.ApplicationData.Languages.LanguagesWindow.TitleProcessingConditions;
            ((System.Windows.Controls.ComboBoxItem)UntitledWindowConditionsComboBox.Items[0]).Content = Common.ApplicationData.Languages.LanguagesWindow.DoNotSpecify;
            ((System.Windows.Controls.ComboBoxItem)UntitledWindowConditionsComboBox.Items[1]).Content = Common.ApplicationData.Languages.LanguagesWindow.DoNotProcessingUntitledWindow;
            ((System.Windows.Controls.ComboBoxItem)UntitledWindowConditionsComboBox.Items[2]).Content = Common.ApplicationData.Languages.LanguagesWindow.DoNotProcessingWindowWithTitle;
            ProcessOnlyOnceLabel.Content = Common.ApplicationData.Languages.LanguagesWindow.ProcessOnlyOnce;
            ((System.Windows.Controls.ComboBoxItem)ProcessOnlyOnceComboBox.Items[0]).Content = Common.ApplicationData.Languages.LanguagesWindow.DoNotSpecify;
            ((System.Windows.Controls.ComboBoxItem)ProcessOnlyOnceComboBox.Items[1]).Content = Common.ApplicationData.Languages.LanguagesWindow.OnceWindowOpen;
            ((System.Windows.Controls.ComboBoxItem)ProcessOnlyOnceComboBox.Items[2]).Content = Common.ApplicationData.Languages.LanguagesWindow.OnceWhileItIsRunning;
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
            ((System.Windows.Controls.ComboBoxItem)StandardDisplayComboBox.Items[0]).Content = Common.ApplicationData.Languages.LanguagesWindow.TheSpecifiedDisplay;
            ((System.Windows.Controls.ComboBoxItem)StandardDisplayComboBox.Items[1]).Content = Common.ApplicationData.Languages.LanguagesWindow.DisplayWithWindow;
            ((System.Windows.Controls.ComboBoxItem)StandardDisplayComboBox.Items[2]).Content = Common.ApplicationData.Languages.LanguagesWindow.OnlyIfItIsOnTheSpecifiedDisplay;
            ProcessingGroupBox.Header = Common.ApplicationData.Languages.LanguagesWindow.Processing;
            ProcessingNameLabel.Content = Common.ApplicationData.Languages.LanguagesWindow.ProcessingName;
            PositionAndSizeGroupBox.Header = Common.ApplicationData.Languages.LanguagesWindow.PositionAndSize;
            WindowStateLabel.Content = Common.ApplicationData.Languages.LanguagesWindow.WindowState;
            ((System.Windows.Controls.ComboBoxItem)WindowStateComboBox.Items[0]).Content = Common.ApplicationData.Languages.LanguagesWindow.DoNotChange;
            ((System.Windows.Controls.ComboBoxItem)WindowStateComboBox.Items[1]).Content = Common.ApplicationData.Languages.LanguagesWindow.NormalWindow;
            ((System.Windows.Controls.ComboBoxItem)WindowStateComboBox.Items[2]).Content = Common.ApplicationData.Languages.LanguagesWindow.Maximize;
            ((System.Windows.Controls.ComboBoxItem)WindowStateComboBox.Items[3]).Content = Common.ApplicationData.Languages.LanguagesWindow.Minimize;
            ProcessOnlyWhenNormalWindowToggleSwitch.Text = Common.ApplicationData.Languages.LanguagesWindow.ProcessOnlyWhenNormalWindow;
            XLabel.Content = Common.ApplicationData.Languages.LanguagesWindow.X;
            ((System.Windows.Controls.ComboBoxItem)XComboBox.Items[0]).Content = Common.ApplicationData.Languages.LanguagesWindow.DoNotChange;
            ((System.Windows.Controls.ComboBoxItem)XComboBox.Items[1]).Content = Common.ApplicationData.Languages.LanguagesWindow.LeftEdge;
            ((System.Windows.Controls.ComboBoxItem)XComboBox.Items[2]).Content = Common.ApplicationData.Languages.LanguagesWindow.Middle;
            ((System.Windows.Controls.ComboBoxItem)XComboBox.Items[3]).Content = Common.ApplicationData.Languages.LanguagesWindow.RightEdge;
            ((System.Windows.Controls.ComboBoxItem)XComboBox.Items[4]).Content = Common.ApplicationData.Languages.LanguagesWindow.CoordinateSpecification;
            ((System.Windows.Controls.ComboBoxItem)XTypeComboBox.Items[0]).Content = Common.ApplicationData.Languages.LanguagesWindow.Pixel;
            ((System.Windows.Controls.ComboBoxItem)XTypeComboBox.Items[1]).Content = Common.ApplicationData.Languages.LanguagesWindow.Percent;
            YLabel.Content = Common.ApplicationData.Languages.LanguagesWindow.Y;
            ((System.Windows.Controls.ComboBoxItem)YComboBox.Items[0]).Content = Common.ApplicationData.Languages.LanguagesWindow.DoNotChange;
            ((System.Windows.Controls.ComboBoxItem)YComboBox.Items[1]).Content = Common.ApplicationData.Languages.LanguagesWindow.TopEdge;
            ((System.Windows.Controls.ComboBoxItem)YComboBox.Items[2]).Content = Common.ApplicationData.Languages.LanguagesWindow.Middle;
            ((System.Windows.Controls.ComboBoxItem)YComboBox.Items[3]).Content = Common.ApplicationData.Languages.LanguagesWindow.BottomEdge;
            ((System.Windows.Controls.ComboBoxItem)YComboBox.Items[4]).Content = Common.ApplicationData.Languages.LanguagesWindow.CoordinateSpecification;
            ((System.Windows.Controls.ComboBoxItem)YTypeComboBox.Items[0]).Content = Common.ApplicationData.Languages.LanguagesWindow.Pixel;
            ((System.Windows.Controls.ComboBoxItem)YTypeComboBox.Items[1]).Content = Common.ApplicationData.Languages.LanguagesWindow.Percent;
            WidthLabel.Content = Common.ApplicationData.Languages.LanguagesWindow.Width;
            ((System.Windows.Controls.ComboBoxItem)WidthComboBox.Items[0]).Content = Common.ApplicationData.Languages.LanguagesWindow.DoNotChange;
            ((System.Windows.Controls.ComboBoxItem)WidthComboBox.Items[1]).Content = Common.ApplicationData.Languages.LanguagesWindow.WidthSpecification;
            ((System.Windows.Controls.ComboBoxItem)WidthTypeComboBox.Items[0]).Content = Common.ApplicationData.Languages.LanguagesWindow.Pixel;
            ((System.Windows.Controls.ComboBoxItem)WidthTypeComboBox.Items[1]).Content = Common.ApplicationData.Languages.LanguagesWindow.Percent;
            HeightLabel.Content = Common.ApplicationData.Languages.LanguagesWindow.Height;
            ((System.Windows.Controls.ComboBoxItem)HeightComboBox.Items[0]).Content = Common.ApplicationData.Languages.LanguagesWindow.DoNotChange;
            ((System.Windows.Controls.ComboBoxItem)HeightComboBox.Items[1]).Content = Common.ApplicationData.Languages.LanguagesWindow.HeightSpecification;
            ((System.Windows.Controls.ComboBoxItem)HeightTypeComboBox.Items[0]).Content = Common.ApplicationData.Languages.LanguagesWindow.Pixel;
            ((System.Windows.Controls.ComboBoxItem)HeightTypeComboBox.Items[1]).Content = Common.ApplicationData.Languages.LanguagesWindow.Percent;
            ClientAreaToggleSwitch.Text = Common.ApplicationData.Languages.LanguagesWindow.ClientArea;
            ProcessingPositionAndSizeTwiceToggleSwitch.Text = Common.ApplicationData.Languages.LanguagesWindow.ProcessingPositionAndSizeTwice;
            DisplayLabel.Content = Common.ApplicationData.Languages.LanguagesWindow.Display;
            ForefrontLabel.Content = Common.ApplicationData.Languages.LanguagesWindow.Forefront;
            ((System.Windows.Controls.ComboBoxItem)ForefrontComboBox.Items[0]).Content = Common.ApplicationData.Languages.LanguagesWindow.DoNotChange;
            ((System.Windows.Controls.ComboBoxItem)ForefrontComboBox.Items[1]).Content = Common.ApplicationData.Languages.LanguagesWindow.AlwaysForefront;
            ((System.Windows.Controls.ComboBoxItem)ForefrontComboBox.Items[2]).Content = Common.ApplicationData.Languages.LanguagesWindow.AlwaysCancelForefront;
            ((System.Windows.Controls.ComboBoxItem)ForefrontComboBox.Items[3]).Content = Common.ApplicationData.Languages.LanguagesWindow.Forefront;
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
                EventItemInformation information = SpecifiedWindowBaseItemInformation as EventItemInformation;
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
                TimerItemInformation information = SpecifiedWindowBaseItemInformation as TimerItemInformation;
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
                System.Windows.Controls.ListBoxItem newItem = new()
                {
                    Content = nowTitleName
                };
                TitleNameExclusionStringListBox.Items.Add(newItem);
            }
            foreach (System.Drawing.Size nowSize in SpecifiedWindowBaseItemInformation.DoNotProcessingSize)
            {
                System.Windows.Controls.ListBoxItem newItem = new()
                {
                    Content = nowSize.Width + Common.SeparateString + nowSize.Height
                };
                DoNotProcessingSizeListBox.Items.Add(newItem);
            }
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
        /// ウィンドウの「Loaded」イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddModifyWindowSpecifiedWindow_Loaded(
            object sender,
            System.Windows.RoutedEventArgs e
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
            object sender,
            System.EventArgs e
            )
        {
            try
            {
                if (WindowInformationAcquisitionTimer != null)
                {
                    WindowInformationAcquisitionTimer.Stop();
                    WindowInformationAcquisitionTimer.Dispose();
                    WindowInformationAcquisitionTimer = null;
                }

                SpecifiedWindowBaseInformation.AcquiredItems.TitleName = (bool)TitleNameCheckBox.IsChecked;
                SpecifiedWindowBaseInformation.AcquiredItems.ClassName = (bool)ClassNameCheckBox.IsChecked;
                SpecifiedWindowBaseInformation.AcquiredItems.FileName = (bool)FileNameCheckBox.IsChecked;
                SpecifiedWindowBaseInformation.AcquiredItems.WindowState = (bool)WindowStateCheckBox.IsChecked;
                SpecifiedWindowBaseInformation.AcquiredItems.X = (bool)XCheckBox.IsChecked;
                SpecifiedWindowBaseInformation.AcquiredItems.Y = (bool)YCheckBox.IsChecked;
                SpecifiedWindowBaseInformation.AcquiredItems.Width = (bool)WidthCheckBox.IsChecked;
                SpecifiedWindowBaseInformation.AcquiredItems.Height = (bool)HeightCheckBox.IsChecked;
                SpecifiedWindowBaseInformation.AcquiredItems.Display = (bool)DisplayCheckBox.IsChecked;
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
                Common.ApplicationData.WriteSettings();
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
            System.Windows.RoutedEventArgs e
            )
        {
            try
            {
                if (FreeEcho.FEControls.MessageBox.Show(Common.ApplicationData.Languages.Check, Common.ApplicationData.Languages.LanguagesWindow.RetrievedAfterFiveSeconds, FreeEcho.FEControls.MessageBoxButton.Ok, this) == FreeEcho.FEControls.MessageBoxResult.Ok)
                {
                    WindowInformationAcquisitionTimer = new()
                    {
                        Interval = WaitTimeForWindowInformationAcquisition
                    };
                    WindowInformationAcquisitionTimer.Elapsed += (s, e) =>
                    {
                        Dispatcher.Invoke(new System.Action(() =>
                        {
                            WindowInformationAcquisitionTimerAfterProcessing();
                        }));
                    };
                    GetWindowInformationStackPanel.IsEnabled = false;
                    WindowInformationAcquisitionTimer.Start();
                }
            }
            catch
            {
                if (WindowInformationAcquisitionTimer != null)
                {
                    WindowInformationAcquisitionTimer.Dispose();
                    WindowInformationAcquisitionTimer = null;
                }
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
            System.Windows.Input.MouseButtonEventArgs e
            )
        {
            try
            {
                WindowSelectionMouse = new()
                {
                    MouseLeftUpStop = true
                };
                WindowSelectionMouse.MouseLeftButtonUp += WindowSelectionMouse_MouseLeftButtonUp;
                PreviousOwnerWindowState = Owner.WindowState;
                WindowState = System.Windows.WindowState.Minimized;
                Owner.WindowState = System.Windows.WindowState.Minimized;
                GetWindowInformationStackPanel.IsEnabled = false;
                WindowSelectionMouse.StartWindowSelection();
            }
            catch
            {
                WindowSelectionMouse = null;
                Owner.WindowState = PreviousOwnerWindowState;
                WindowState = System.Windows.WindowState.Normal;
                GetWindowInformationStackPanel.IsEnabled = true;
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
                WindowInformationAcquisitionTimer.Dispose();
                if (WindowProcessingTabItem.IsSelected)
                {
                    GetWindowProcessingInformationFromHandle(NativeMethods.GetForegroundWindow());
                }
                else
                {
                    GetInformationFromWindowHandle(NativeMethods.GetForegroundWindow());
                }
                Activate();
                FreeEcho.FEControls.MessageBox.Show(Common.ApplicationData.Languages.Check, Common.ApplicationData.Languages.LanguagesWindow.SelectionComplete, FreeEcho.FEControls.MessageBoxButton.Ok, this);
            }
            catch
            {
                Activate();
                FreeEcho.FEControls.MessageBox.Show(Common.ApplicationData.Languages.Check, Common.ApplicationData.Languages.ErrorOccurred, FreeEcho.FEControls.MessageBoxButton.Ok, this);
            }
            finally
            {
                GetWindowInformationStackPanel.IsEnabled = true;
                WindowInformationAcquisitionTimer = null;
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
                    GetWindowProcessingInformationFromHandle(WindowSelectionMouse.SelectedHwnd);
                }
                else
                {
                    GetInformationFromWindowHandle(WindowSelectionMouse.SelectedHwnd);
                }
                Owner.WindowState = PreviousOwnerWindowState;
                WindowState = System.Windows.WindowState.Normal;
                Activate();
                FreeEcho.FEControls.MessageBox.Show(Common.ApplicationData.Languages.Check, Common.ApplicationData.Languages.LanguagesWindow.SelectionComplete, FreeEcho.FEControls.MessageBoxButton.Ok, this);
            }
            catch
            {
                Owner.WindowState = PreviousOwnerWindowState;
                WindowState = System.Windows.WindowState.Normal;
                Activate();
                FreeEcho.FEControls.MessageBox.Show(Common.ApplicationData.Languages.Check, Common.ApplicationData.Languages.ErrorOccurred, FreeEcho.FEControls.MessageBoxButton.Ok, this);
            }
            finally
            {
                GetWindowInformationStackPanel.IsEnabled = true;
                WindowSelectionMouse = null;
            }
        }

        /// <summary>
        /// 「タイトル名」TextBoxの「TextChanged」イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TitleNameTextBox_TextChanged(
            object sender,
            System.Windows.Controls.TextChangedEventArgs e
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
            System.Windows.Controls.TextChangedEventArgs e
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
            System.Windows.Controls.TextChangedEventArgs e
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
            System.Windows.RoutedEventArgs e
            )
        {
            try
            {
                Microsoft.Win32.OpenFileDialog openFileDialog = new()
                {
                    Title = Common.ApplicationData.Languages.LanguagesWindow.FileSelection,
                    Filter = ".exe|*.exe*",
                    Multiselect = false
                };

                if (openFileDialog.ShowDialog() == true)
                {
                    FileNameTextBox.Text = (string)((System.Windows.Controls.ComboBoxItem)FileNameMatchingConditionComboBox.SelectedItem).Content == Common.ApplicationData.Languages.LanguagesWindow.DoNotIncludePath
                        ? System.IO.Path.GetFileNameWithoutExtension(openFileDialog.FileName)
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
            System.Windows.Controls.TextChangedEventArgs e
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
            System.Windows.Controls.SelectionChangedEventArgs e
            )
        {
            try
            {
                if ((string)((System.Windows.Controls.ComboBoxItem)WindowStateComboBox.SelectedItem).Content == Common.ApplicationData.Languages.LanguagesWindow.NormalWindow)
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
            System.Windows.Controls.SelectionChangedEventArgs e
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
            System.Windows.Controls.SelectionChangedEventArgs e
            )
        {
            try
            {
                if ((string)((System.Windows.Controls.ComboBoxItem)XTypeComboBox.SelectedItem).Content == Common.ApplicationData.Languages.LanguagesWindow.Percent)
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
            System.Windows.Controls.SelectionChangedEventArgs e
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
            System.Windows.Controls.SelectionChangedEventArgs e
            )
        {
            try
            {
                if ((string)((System.Windows.Controls.ComboBoxItem)YTypeComboBox.SelectedItem).Content == Common.ApplicationData.Languages.LanguagesWindow.Percent)
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
            System.Windows.Controls.SelectionChangedEventArgs e
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
            System.Windows.Controls.SelectionChangedEventArgs e
            )
        {
            try
            {
                if ((string)((System.Windows.Controls.ComboBoxItem)WidthTypeComboBox.SelectedItem).Content == Common.ApplicationData.Languages.LanguagesWindow.Percent)
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
            System.Windows.Controls.SelectionChangedEventArgs e
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
            System.Windows.Controls.SelectionChangedEventArgs e
            )
        {
            try
            {
                if ((string)((System.Windows.Controls.ComboBoxItem)HeightTypeComboBox.SelectedItem).Content == Common.ApplicationData.Languages.LanguagesWindow.Percent)
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
            System.Windows.Controls.SelectionChangedEventArgs e
            )
        {
            try
            {
                // 項目のアクティブ状態を変更
                if (SpecifiedWindowBaseItemInformation.WindowProcessingInformation.Count != 0)
                {
                    bool activeCheck = false;      // アクティブ項目があるかの値

                    // 複数アクティブにできる場合 (複数アクティブにできない場合は後ろの項目を無効にする)
                    if (CheckMultipleActivation())
                    {
                        for (int count1 = 0; count1 < SpecifiedWindowBaseItemInformation.WindowProcessingInformation.Count - 1; count1++)
                        {
                            for (int count2 = count1 + 1; count2 < SpecifiedWindowBaseItemInformation.WindowProcessingInformation.Count; count2++)
                            {
                                if ((SpecifiedWindowBaseItemInformation.WindowProcessingInformation[count1].PositionSize.Display == SpecifiedWindowBaseItemInformation.WindowProcessingInformation[count2].PositionSize.Display)
                                    && SpecifiedWindowBaseItemInformation.WindowProcessingInformation[count1].Active
                                    && SpecifiedWindowBaseItemInformation.WindowProcessingInformation[count2].Active)
                                {
                                    SpecifiedWindowBaseItemInformation.WindowProcessingInformation[count2].Active = false;
                                }
                            }
                        }
                    }
                    // 1つのみアクティブにできる場合 (最初に見つかった有効な項目以外は無効にする)
                    else
                    {
                        foreach (WindowProcessingInformation nowWPI in SpecifiedWindowBaseItemInformation.WindowProcessingInformation)
                        {
                            if (nowWPI.Active && (activeCheck == false))
                            {
                                activeCheck = true;
                            }
                            else
                            {
                                nowWPI.Active = false;
                            }
                        }
                    }

                    // アクティブな項目がない場合は最初の項目をアクティブにする
                    activeCheck = false;
                    foreach (WindowProcessingInformation nowWPI in SpecifiedWindowBaseItemInformation.WindowProcessingInformation)
                    {
                        if (nowWPI.Active)
                        {
                            activeCheck = true;
                            break;
                        }
                    }
                    if (activeCheck == false)
                    {
                        SpecifiedWindowBaseItemInformation.WindowProcessingInformation[0].Active = true;
                    }

                    SettingsTheActiveStateOfTheItemsListBox();
                }

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
            System.Windows.Input.KeyEventArgs e
            )
        {
            try
            {
                FreeEcho.FEHotKeyWPF.HotKeyInformation hotkeyInformation = FreeEcho.FEHotKeyWPF.HotKeyWPF.GetHotKey(e, true);        // ホットキー情報
                string hotkeyString = FreeEcho.FEHotKeyWPF.HotKeyWPF.GetHotKeyString(hotkeyInformation);        // ホットキーの文字列
                if (hotkeyString != "Tab")
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
            System.Windows.RoutedEventArgs e
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
            System.Windows.RoutedEventArgs e
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
            System.Windows.Controls.SelectionChangedEventArgs e
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
            System.Windows.Controls.TextChangedEventArgs e
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
            System.Windows.RoutedEventArgs e
            )
        {
            try
            {
                // 重複確認
                foreach (string nowTitleName in SpecifiedWindowBaseItemInformation.DoNotProcessingTitleName)
                {
                    if (nowTitleName == TitleNameExclusionStringTextBox.Text)
                    {
                        FreeEcho.FEControls.MessageBox.Show(Common.ApplicationData.Languages.Check, Common.ApplicationData.Languages.LanguagesWindow.ThereAreDuplicateItems, FreeEcho.FEControls.MessageBoxButton.Ok, this);
                        return;
                    }
                }

                SpecifiedWindowBaseItemInformation.DoNotProcessingTitleName.Add(TitleNameExclusionStringTextBox.Text);
                System.Windows.Controls.ListBoxItem newItem = new()
                {
                    Content = TitleNameExclusionStringTextBox.Text
                };      // 新しいListBoxItem
                TitleNameExclusionStringListBox.Items.Add(newItem);
                FreeEcho.FEControls.MessageBox.Show(Common.ApplicationData.Languages.Check, Common.ApplicationData.Languages.LanguagesWindow.Added, FreeEcho.FEControls.MessageBoxButton.Ok, this);
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
            System.Windows.RoutedEventArgs e
            )
        {
            try
            {
                if (FreeEcho.FEControls.MessageBox.Show(Common.ApplicationData.Languages.Check, Common.ApplicationData.Languages.LanguagesWindow.AllowDelete, FreeEcho.FEControls.MessageBoxButton.YesCancel, this) == FreeEcho.FEControls.MessageBoxResult.Yes)
                {
                    SpecifiedWindowBaseItemInformation.DoNotProcessingTitleName.RemoveAt(TitleNameExclusionStringListBox.SelectedIndex);
                    TitleNameExclusionStringListBox.Items.RemoveAt(TitleNameExclusionStringListBox.SelectedIndex);
                    FreeEcho.FEControls.MessageBox.Show(Common.ApplicationData.Languages.Check, Common.ApplicationData.Languages.LanguagesWindow.Deleted, FreeEcho.FEControls.MessageBoxButton.Ok, this);
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
            System.Windows.Controls.SelectionChangedEventArgs e
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
            System.Windows.RoutedEventArgs e
            )
        {
            try
            {
                // 重複確認
                foreach (System.Drawing.Size nowSize in SpecifiedWindowBaseItemInformation.DoNotProcessingSize)
                {
                    if ((nowSize.Width == DoNotProcessingSizeWidthNumericUpDown.ValueInt)
                        && (nowSize.Height == DoNotProcessingSizeHeightNumericUpDown.ValueInt))
                    {
                        FreeEcho.FEControls.MessageBox.Show(Common.ApplicationData.Languages.Check, Common.ApplicationData.Languages.LanguagesWindow.ThereAreDuplicateItems, FreeEcho.FEControls.MessageBoxButton.Ok, this);
                        return;
                    }
                }

                SpecifiedWindowBaseItemInformation.DoNotProcessingSize.Add(new(DoNotProcessingSizeWidthNumericUpDown.ValueInt, DoNotProcessingSizeHeightNumericUpDown.ValueInt));
                System.Windows.Controls.ListBoxItem newItem = new()
                {
                    Content = DoNotProcessingSizeWidthNumericUpDown.ValueInt + Common.SeparateString + DoNotProcessingSizeHeightNumericUpDown.ValueInt
                };      // 新しいListBoxItem
                DoNotProcessingSizeListBox.Items.Add(newItem);
                FreeEcho.FEControls.MessageBox.Show(Common.ApplicationData.Languages.Check, Common.ApplicationData.Languages.LanguagesWindow.Added, FreeEcho.FEControls.MessageBoxButton.Ok, this);
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
            System.Windows.RoutedEventArgs e
            )
        {
            try
            {
                if (FreeEcho.FEControls.MessageBox.Show(Common.ApplicationData.Languages.Check, Common.ApplicationData.Languages.LanguagesWindow.AllowDelete, FreeEcho.FEControls.MessageBoxButton.YesCancel, this) == FreeEcho.FEControls.MessageBoxResult.Yes)
                {
                    SpecifiedWindowBaseItemInformation.DoNotProcessingSize.RemoveAt(DoNotProcessingSizeListBox.SelectedIndex);
                    DoNotProcessingSizeListBox.Items.RemoveAt(DoNotProcessingSizeListBox.SelectedIndex);
                    FreeEcho.FEControls.MessageBox.Show(Common.ApplicationData.Languages.Check, Common.ApplicationData.Languages.LanguagesWindow.Deleted, FreeEcho.FEControls.MessageBoxButton.Ok, this);
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
            System.Windows.RoutedEventArgs e
            )
        {
            try
            {
                if (AddWindowProcessingItem())
                {
                    SettingsTheActiveStateOfTheItemsListBox();
                    FreeEcho.FEControls.MessageBox.Show(Common.ApplicationData.Languages.Check, Common.ApplicationData.Languages.LanguagesWindow.Added, FreeEcho.FEControls.MessageBoxButton.Ok, this);
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
            System.Windows.RoutedEventArgs e
            )
        {
            try
            {
                if (ModifyItem())
                {
                    SettingsTheActiveStateOfTheItemsListBox();
                    FreeEcho.FEControls.MessageBox.Show(Common.ApplicationData.Languages.Check, Common.ApplicationData.Languages.LanguagesWindow.Modified, FreeEcho.FEControls.MessageBoxButton.Ok, this);
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
            System.Windows.Controls.SelectionChangedEventArgs e
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
            System.Windows.RoutedEventArgs e
            )
        {
            try
            {
                SpecifiedWindowBaseItemInformation.WindowProcessingInformation[WindowProcessingListBox.SelectedIndex].Active = !SpecifiedWindowBaseItemInformation.WindowProcessingInformation[WindowProcessingListBox.SelectedIndex].Active;
                // アクティブになった場合、他の項目のアクティブ状態を調べてアクティブを解除する
                if (SpecifiedWindowBaseItemInformation.WindowProcessingInformation[WindowProcessingListBox.SelectedIndex].Active)
                {
                    int count = 0;      // カウント

                    if (CheckMultipleActivation())
                    {
                        foreach (WindowProcessingInformation nowWPI in SpecifiedWindowBaseItemInformation.WindowProcessingInformation)
                        {
                            if ((nowWPI.PositionSize.Display == SpecifiedWindowBaseItemInformation.WindowProcessingInformation[WindowProcessingListBox.SelectedIndex].PositionSize.Display)
                                && (count != WindowProcessingListBox.SelectedIndex))
                            {
                                nowWPI.Active = false;
                            }
                            count++;
                        }
                    }
                    else
                    {
                        foreach (WindowProcessingInformation nowWPI in SpecifiedWindowBaseItemInformation.WindowProcessingInformation)
                        {
                            if (nowWPI.Active && (count != WindowProcessingListBox.SelectedIndex))
                            {
                                nowWPI.Active = false;
                            }
                            count++;
                        }
                    }
                }
                // アクティブではなくなった場合はアクティブ項目を調べて、無い場合は最初の項目をアクティブにする
                else
                {
                    bool activeCheck = false;      // アクティブ項目があるかの値
                    foreach (WindowProcessingInformation nowWPI in SpecifiedWindowBaseItemInformation.WindowProcessingInformation)
                    {
                        if (nowWPI.Active)
                        {
                            activeCheck = true;
                            break;
                        }
                    }
                    if (activeCheck == false)
                    {
                        SpecifiedWindowBaseItemInformation.WindowProcessingInformation[0].Active = true;
                    }
                }
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
            System.Windows.RoutedEventArgs e
            )
        {
            try
            {
                if (WindowProcessingListBox.SelectedItems.Count == 1)
                {
                    SpecifiedWindowBaseItemInformation.WindowProcessingInformation.RemoveAt(WindowProcessingListBox.SelectedIndex);
                    WindowProcessingListBox.Items.RemoveAt(WindowProcessingListBox.SelectedIndex);

                    // アクティブ状態の項目がない場合は最初の項目をアクティブにする
                    if (SpecifiedWindowBaseItemInformation.WindowProcessingInformation.Count != 0)
                    {
                        bool activeCheck = false;      // アクティブ項目があるかの値
                        foreach (WindowProcessingInformation nowWPI in SpecifiedWindowBaseItemInformation.WindowProcessingInformation)
                        {
                            if (nowWPI.Active)
                            {
                                activeCheck = true;
                                break;
                            }
                        }
                        if (activeCheck == false)
                        {
                            SpecifiedWindowBaseItemInformation.WindowProcessingInformation[0].Active = true;
                        }
                    }
                    SettingsTheActiveStateOfTheItemsListBox();
                    FreeEcho.FEControls.MessageBox.Show(Common.ApplicationData.Languages.Check, Common.ApplicationData.Languages.LanguagesWindow.Deleted, FreeEcho.FEControls.MessageBoxButton.Ok, this);
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
            System.Windows.RoutedEventArgs e
            )
        {
            try
            {
                if (WindowProcessingListBox.SelectedItems.Count == 1)
                {
                    WindowProcessingInformation newWPI = new(SpecifiedWindowBaseItemInformation.WindowProcessingInformation[WindowProcessingListBox.SelectedIndex], false);
                    int number = 1;     // 名前の後ろに付ける番号
                    foreach (WindowProcessingInformation nowWPI in SpecifiedWindowBaseItemInformation.WindowProcessingInformation)
                    {
                        if (nowWPI.ProcessingName == newWPI.ProcessingName + Common.SeparateString + Common.ApplicationData.Languages.LanguagesWindow.Copy + " " + number)
                        {
                            number++;
                        }
                    }
                    newWPI.ProcessingName += Common.SeparateString + Common.ApplicationData.Languages.LanguagesWindow.Copy + " " + number;
                    SpecifiedWindowBaseItemInformation.WindowProcessingInformation.Add(newWPI);
                    FreeEcho.FEControls.ListBoxItemValidState newItem = new()
                    {
                        Text = newWPI.ProcessingName,
                        Height = ListBoxItemHeight,
                        IsValidState = false
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
            System.Windows.RoutedEventArgs e
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

                if (CheckTheValueExceptItem())
                {
                    GetTheValueExceptItem();

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
                            result = AddWindowProcessingItem();
                        }
                        if (result)
                        {
                            if (SpecifiedWindowBaseInformation is EventInformation eventInformation)
                            {
                                Common.ApplicationData.WindowProcessingManagement.EventWindowProcessing?.UnregisterHotkeys();
                                eventInformation.Items.Add(SpecifiedWindowBaseItemInformation as EventItemInformation);
                                Common.ApplicationData.WindowProcessingManagement.EventWindowProcessing?.RegisterHotkeys();
                            }
                            else if (SpecifiedWindowBaseInformation is TimerInformation timerInformation)
                            {
                                Common.ApplicationData.WindowProcessingManagement.TimerWindowProcessing?.UnregisterHotkeys();
                                timerInformation.Items.Add(SpecifiedWindowBaseItemInformation as TimerItemInformation);
                                Common.ApplicationData.WindowProcessingManagement.TimerWindowProcessing?.RegisterHotkeys();
                            }
                            FreeEcho.FEControls.MessageBox.Show(Common.ApplicationData.Languages.Check, Common.ApplicationData.Languages.LanguagesWindow.Added, FreeEcho.FEControls.MessageBoxButton.Ok, this);
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
                            result = ModifyItem();
                        }
                        if (result)
                        {
                            if (SpecifiedWindowBaseInformation is EventInformation eventInformation)
                            {
                                Common.ApplicationData.WindowProcessingManagement.EventWindowProcessing?.UnregisterHotkeys();
                                eventInformation.Items[IndexOfItemToBeModified] = SpecifiedWindowBaseItemInformation as EventItemInformation;
                                Common.ApplicationData.WindowProcessingManagement.EventWindowProcessing?.RegisterHotkeys();
                            }
                            else if (SpecifiedWindowBaseInformation is TimerInformation timerInformation)
                            {
                                Common.ApplicationData.WindowProcessingManagement.TimerWindowProcessing?.UnregisterHotkeys();
                                timerInformation.Items[IndexOfItemToBeModified] = SpecifiedWindowBaseItemInformation as TimerItemInformation;
                                Common.ApplicationData.WindowProcessingManagement.TimerWindowProcessing?.RegisterHotkeys();
                            }
                            FreeEcho.FEControls.MessageBox.Show(Common.ApplicationData.Languages.Check, Common.ApplicationData.Languages.LanguagesWindow.Modified, FreeEcho.FEControls.MessageBoxButton.Ok, this);
                            AddedOrModified = true;
                            Close();
                        }
                    }
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
            System.Windows.RoutedEventArgs e
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
            if ((string)((System.Windows.Controls.ComboBoxItem)WindowStateComboBox.SelectedItem).Content == Common.ApplicationData.Languages.LanguagesWindow.NormalWindow)
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
        /// 「ウィンドウ処理」以外の値を確認
        /// </summary>
        /// <returns>値に問題ない (問題がある「false」/問題ない「true」)</returns>
        private bool CheckTheValueExceptItem()
        {
            bool result = true;     // 結果

            // 登録名の重複確認
            if (SpecifiedWindowBaseInformation is EventInformation eventInformation)
            {
                if (IndexOfItemToBeModified == -1)
                {
                    foreach (EventItemInformation nowItem in eventInformation.Items)
                    {
                        if (RegisteredNameTextBox.Text == nowItem.RegisteredName)
                        {
                            FreeEcho.FEControls.MessageBox.Show(Common.ApplicationData.Languages.Check, Common.ApplicationData.Languages.LanguagesWindow.ThereIsADuplicateRegistrationName, FreeEcho.FEControls.MessageBoxButton.Ok, this);
                            result = false;
                            break;
                        }
                    }
                }
                else
                {
                    int count = 0;
                    foreach (EventItemInformation nowItem in eventInformation.Items)
                    {
                        if (count != IndexOfItemToBeModified)
                        {
                            if (RegisteredNameTextBox.Text == nowItem.RegisteredName)
                            {
                                FreeEcho.FEControls.MessageBox.Show(Common.ApplicationData.Languages.Check, Common.ApplicationData.Languages.LanguagesWindow.ThereIsADuplicateRegistrationName, FreeEcho.FEControls.MessageBoxButton.Ok, this);
                                result = false;
                                break;
                            }
                        }
                        count++;
                    }
                }
            }
            else if (SpecifiedWindowBaseInformation is TimerInformation timerInformation)
            {
                if (IndexOfItemToBeModified == -1)
                {
                    foreach (TimerItemInformation nowItem in timerInformation.Items)
                    {
                        if (RegisteredNameTextBox.Text == nowItem.RegisteredName)
                        {
                            FreeEcho.FEControls.MessageBox.Show(Common.ApplicationData.Languages.Check, Common.ApplicationData.Languages.LanguagesWindow.ThereIsADuplicateRegistrationName, FreeEcho.FEControls.MessageBoxButton.Ok, this);
                            result = false;
                            break;
                        }
                    }
                }
                else
                {
                    int count = 0;
                    foreach (TimerItemInformation nowItem in timerInformation.Items)
                    {
                        if (count != IndexOfItemToBeModified)
                        {
                            if (RegisteredNameTextBox.Text == nowItem.RegisteredName)
                            {
                                FreeEcho.FEControls.MessageBox.Show(Common.ApplicationData.Languages.Check, Common.ApplicationData.Languages.LanguagesWindow.ThereIsADuplicateRegistrationName, FreeEcho.FEControls.MessageBoxButton.Ok, this);
                                result = false;
                                break;
                            }
                        }
                        count++;
                    }
                }
            }

            return (result);
        }

        /// <summary>
        /// 項目以外の値を取得
        /// </summary>
        private void GetTheValueExceptItem()
        {
            string stringData;     // 文字列

            SpecifiedWindowBaseItemInformation.RegisteredName = RegisteredNameTextBox.Text;
            SpecifiedWindowBaseItemInformation.TitleName = TitleNameTextBox.Text;
            stringData = (string)((System.Windows.Controls.ComboBoxItem)TitleNameMatchingConditionComboBox.SelectedItem).Content;
            if (stringData == Common.ApplicationData.Languages.LanguagesWindow.ExactMatch)
            {
                SpecifiedWindowBaseItemInformation.TitleNameMatchCondition = NameMatchCondition.ExactMatch;
            }
            else if (stringData == Common.ApplicationData.Languages.LanguagesWindow.PartialMatch)
            {
                SpecifiedWindowBaseItemInformation.TitleNameMatchCondition = NameMatchCondition.PartialMatch;
            }
            else if (stringData == Common.ApplicationData.Languages.LanguagesWindow.ForwardMatch)
            {
                SpecifiedWindowBaseItemInformation.TitleNameMatchCondition = NameMatchCondition.ForwardMatch;
            }
            else if (stringData == Common.ApplicationData.Languages.LanguagesWindow.BackwardMatch)
            {
                SpecifiedWindowBaseItemInformation.TitleNameMatchCondition = NameMatchCondition.BackwardMatch;
            }
            SpecifiedWindowBaseItemInformation.ClassName = ClassNameTextBox.Text;
            stringData = (string)((System.Windows.Controls.ComboBoxItem)ClassNameMatchingConditionComboBox.SelectedItem).Content;
            if (stringData == Common.ApplicationData.Languages.LanguagesWindow.ExactMatch)
            {
                SpecifiedWindowBaseItemInformation.ClassNameMatchCondition = NameMatchCondition.ExactMatch;
            }
            else if (stringData == Common.ApplicationData.Languages.LanguagesWindow.PartialMatch)
            {
                SpecifiedWindowBaseItemInformation.ClassNameMatchCondition = NameMatchCondition.PartialMatch;
            }
            else if (stringData == Common.ApplicationData.Languages.LanguagesWindow.ForwardMatch)
            {
                SpecifiedWindowBaseItemInformation.ClassNameMatchCondition = NameMatchCondition.ForwardMatch;
            }
            else if (stringData == Common.ApplicationData.Languages.LanguagesWindow.BackwardMatch)
            {
                SpecifiedWindowBaseItemInformation.ClassNameMatchCondition = NameMatchCondition.BackwardMatch;
            }
            SpecifiedWindowBaseItemInformation.FileName = FileNameTextBox.Text;
            stringData = (string)((System.Windows.Controls.ComboBoxItem)FileNameMatchingConditionComboBox.SelectedItem).Content;
            if (stringData == Common.ApplicationData.Languages.LanguagesWindow.IncludePath)
            {
                SpecifiedWindowBaseItemInformation.FileNameMatchCondition = FileNameMatchCondition.Include;
            }
            else if (stringData == Common.ApplicationData.Languages.LanguagesWindow.DoNotIncludePath)
            {
                SpecifiedWindowBaseItemInformation.FileNameMatchCondition = FileNameMatchCondition.NotInclude;
            }
            stringData = (string)((System.Windows.Controls.ComboBoxItem)UntitledWindowConditionsComboBox.SelectedItem).Content;
            if (stringData == Common.ApplicationData.Languages.LanguagesWindow.DoNotSpecify)
            {
                SpecifiedWindowBaseItemInformation.TitleProcessingConditions = TitleProcessingConditions.NotSpecified;
            }
            else if (stringData == Common.ApplicationData.Languages.LanguagesWindow.DoNotProcessingUntitledWindow)
            {
                SpecifiedWindowBaseItemInformation.TitleProcessingConditions = TitleProcessingConditions.DoNotProcessingUntitledWindow;
            }
            else if (stringData == Common.ApplicationData.Languages.LanguagesWindow.DoNotProcessingWindowWithTitle)
            {
                SpecifiedWindowBaseItemInformation.TitleProcessingConditions = TitleProcessingConditions.DoNotProcessingWindowWithTitle;
            }
            stringData = (string)((System.Windows.Controls.ComboBoxItem)ProcessOnlyOnceComboBox.SelectedItem).Content;
            if (stringData == Common.ApplicationData.Languages.LanguagesWindow.DoNotSpecify)
            {
                SpecifiedWindowBaseItemInformation.ProcessingOnlyOnce = ProcessingOnlyOnce.NotSpecified;
            }
            else if (stringData == Common.ApplicationData.Languages.LanguagesWindow.OnceWindowOpen)
            {
                SpecifiedWindowBaseItemInformation.ProcessingOnlyOnce = ProcessingOnlyOnce.WindowOpen;
            }
            else if (stringData == Common.ApplicationData.Languages.LanguagesWindow.OnceWhileItIsRunning)
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
        }

        /// <summary>
        /// 「ウィンドウ処理」の項目追加
        /// </summary>
        /// <returns>追加に成功したかの値 (失敗「false」/成功「true」)</returns>
        private bool AddWindowProcessingItem()
        {
            bool result = false;        // 結果

            if (CheckWindowProcessingInformation(false))
            {
                WindowProcessingInformation newWPI = GetTheValueOfAnItem();

                // アクティブ状態を設定
                if (CheckMultipleActivation())
                {
                    foreach (WindowProcessingInformation nowWPI in SpecifiedWindowBaseItemInformation.WindowProcessingInformation)
                    {
                        if (nowWPI.PositionSize.Display == newWPI.PositionSize.Display)
                        {
                            newWPI.Active = false;
                            break;
                        }
                    }
                }
                else
                {
                    foreach (WindowProcessingInformation nowWPI in SpecifiedWindowBaseItemInformation.WindowProcessingInformation)
                    {
                        if (nowWPI.Active)
                        {
                            newWPI.Active = false;
                            break;
                        }
                    }
                }

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

            return (result);
        }

        /// <summary>
        /// 項目修正
        /// </summary>
        /// <returns>修正に成功したかの値 (失敗「false」/成功「true」)</returns>
        private bool ModifyItem()
        {
            bool result = false;        // 結果

            if (WindowProcessingListBox.SelectedItems.Count == 1)
            {
                if (CheckWindowProcessingInformation(true))
                {
                    WindowProcessingInformation modifyWPI = GetTheValueOfAnItem();      // 修正した処理項目
                    modifyWPI.Active = SpecifiedWindowBaseItemInformation.WindowProcessingInformation[WindowProcessingListBox.SelectedIndex].Active;
                    int count = 0;

                    // アクティブ状態を設定
                    if (CheckMultipleActivation())
                    {
                        foreach (WindowProcessingInformation nowWPI in SpecifiedWindowBaseItemInformation.WindowProcessingInformation)
                        {
                            if ((nowWPI.PositionSize.Display == modifyWPI.PositionSize.Display)
                                && nowWPI.Active
                                && (count != WindowProcessingListBox.SelectedIndex))
                            {
                                modifyWPI.Active = false;
                                break;
                            }
                            count++;
                        }
                    }
                    else
                    {
                        foreach (WindowProcessingInformation nowWPI in SpecifiedWindowBaseItemInformation.WindowProcessingInformation)
                        {
                            if (nowWPI.Active
                                && (count != WindowProcessingListBox.SelectedIndex))
                            {
                                modifyWPI.Active = false;
                                break;
                            }
                            count++;
                        }
                    }

                    SpecifiedWindowBaseItemInformation.WindowProcessingInformation[WindowProcessingListBox.SelectedIndex].Copy(modifyWPI);
                    FreeEcho.FEControls.ListBoxItemValidState modifyItem = WindowProcessingListBox.Items[WindowProcessingListBox.SelectedIndex] as FreeEcho.FEControls.ListBoxItemValidState;
                    modifyItem.Text = modifyWPI.ProcessingName;
                    modifyItem.IsValidState = modifyWPI.Active;

                    result = true;
                }
            }

            return (result);
        }

        /// <summary>
        /// 「ウィンドウ処理情報」の値を確認
        /// </summary>
        /// <param name="additionOrModify">追加か修正 (追加「false」/修正「true」)</param>
        /// <returns>値に問題ないかの値 (問題がある「false」/問題ない「true」)</returns>
        private bool CheckWindowProcessingInformation(
            bool additionOrModify
            )
        {
            if (string.IsNullOrEmpty(ProcessingNameTextBox.Text))
            {
                FreeEcho.FEControls.MessageBox.Show(Common.ApplicationData.Languages.Check, Common.ApplicationData.Languages.ErrorOccurred, FreeEcho.FEControls.MessageBoxButton.Ok, this);
                return (false);
            }

            // 項目名の重複確認
            if (additionOrModify)
            {
                int count = 0;
                foreach (WindowProcessingInformation nowWPI in SpecifiedWindowBaseItemInformation.WindowProcessingInformation)
                {
                    if ((count != WindowProcessingListBox.SelectedIndex) && (nowWPI.ProcessingName == ProcessingNameTextBox.Text))
                    {
                        FreeEcho.FEControls.MessageBox.Show(Common.ApplicationData.Languages.Check, Common.ApplicationData.Languages.LanguagesWindow.ThereIsADuplicateProcessingName, FreeEcho.FEControls.MessageBoxButton.Ok, this);
                        return (false);
                    }
                    count++;
                }
            }
            else
            {
                foreach (WindowProcessingInformation nowWPI in SpecifiedWindowBaseItemInformation.WindowProcessingInformation)
                {
                    if (nowWPI.ProcessingName == ProcessingNameTextBox.Text)
                    {
                        FreeEcho.FEControls.MessageBox.Show(Common.ApplicationData.Languages.Check, Common.ApplicationData.Languages.LanguagesWindow.ThereIsADuplicateProcessingName, FreeEcho.FEControls.MessageBoxButton.Ok, this);
                        return (false);
                    }
                }
            }

            return (true);
        }

        /// <summary>
        /// 「ウィンドウ処理情報」の値を取得
        /// </summary>
        /// <returns>取得したウィンドウの処理情報</returns>
        private WindowProcessingInformation GetTheValueOfAnItem()
        {
            WindowProcessingInformation newWPI = new()
            {
                Active = true
            };      // 新しいWindowProcessingInformation

            newWPI.ProcessingName = ProcessingNameTextBox.Text;
            newWPI.PositionSize.Display = DisplayComboBox.Text;
            string stringData = (string)((System.Windows.Controls.ComboBoxItem)WindowStateComboBox.SelectedItem).Content;
            if (stringData == Common.ApplicationData.Languages.LanguagesWindow.DoNotChange)
            {
                newWPI.WindowState = Settings.WindowState.DoNotChange;
            }
            else if (stringData == Common.ApplicationData.Languages.LanguagesWindow.NormalWindow)
            {
                newWPI.WindowState = Settings.WindowState.Normal;
            }
            else if (stringData == Common.ApplicationData.Languages.LanguagesWindow.Maximize)
            {
                newWPI.WindowState = Settings.WindowState.Maximize;
            }
            else if (stringData == Common.ApplicationData.Languages.LanguagesWindow.Minimize)
            {
                newWPI.WindowState = Settings.WindowState.Minimize;
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
            stringData = (string)((System.Windows.Controls.ComboBoxItem)XTypeComboBox.SelectedItem).Content;
            if (stringData == Common.ApplicationData.Languages.LanguagesWindow.Pixel)
            {
                XNumericUpDown.IsUseDecimal = false;
                newWPI.PositionSize.XValueType = ValueType.Pixel;
            }
            else if (stringData == Common.ApplicationData.Languages.LanguagesWindow.Percent)
            {
                XNumericUpDown.IsUseDecimal = true;
                newWPI.PositionSize.XValueType = ValueType.Percent;
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
            stringData = (string)((System.Windows.Controls.ComboBoxItem)YTypeComboBox.SelectedItem).Content;
            if (stringData == Common.ApplicationData.Languages.LanguagesWindow.Pixel)
            {
                YNumericUpDown.IsUseDecimal = false;
                newWPI.PositionSize.YValueType = ValueType.Pixel;
            }
            else if (stringData == Common.ApplicationData.Languages.LanguagesWindow.Percent)
            {
                YNumericUpDown.IsUseDecimal = true;
                newWPI.PositionSize.YValueType = ValueType.Percent;
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
            stringData = (string)((System.Windows.Controls.ComboBoxItem)WidthTypeComboBox.SelectedItem).Content;
            if (stringData == Common.ApplicationData.Languages.LanguagesWindow.Pixel)
            {
                WidthNumericUpDown.IsUseDecimal = false;
                newWPI.PositionSize.WidthValueType = ValueType.Pixel;
            }
            else if (stringData == Common.ApplicationData.Languages.LanguagesWindow.Percent)
            {
                WidthNumericUpDown.IsUseDecimal = true;
                newWPI.PositionSize.WidthValueType = ValueType.Percent;
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
            stringData = (string)((System.Windows.Controls.ComboBoxItem)HeightTypeComboBox.SelectedItem).Content;
            if (stringData == Common.ApplicationData.Languages.LanguagesWindow.Pixel)
            {
                HeightNumericUpDown.IsUseDecimal = false;
                newWPI.PositionSize.HeightValueType = ValueType.Pixel;
            }
            else if (stringData == Common.ApplicationData.Languages.LanguagesWindow.Percent)
            {
                HeightNumericUpDown.IsUseDecimal = true;
                newWPI.PositionSize.HeightValueType = ValueType.Percent;
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

            return (newWPI);
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
            stringData = settingsWPI.WindowState switch
            {
                Settings.WindowState.Normal => Common.ApplicationData.Languages.LanguagesWindow.NormalWindow,
                Settings.WindowState.Maximize => Common.ApplicationData.Languages.LanguagesWindow.Maximize,
                Settings.WindowState.Minimize => Common.ApplicationData.Languages.LanguagesWindow.Minimize,
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
                case ValueType.Percent:
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
                case ValueType.Percent:
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
                case ValueType.Percent:
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
                case ValueType.Percent:
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
            System.IntPtr hwnd
            )
        {
            WindowInformation windowInformation = WindowProcessing.GetWindowInformation(hwnd);
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
                FileNameTextBox.Text = (string)((System.Windows.Controls.ComboBoxItem)FileNameMatchingConditionComboBox.SelectedItem).Content == Common.ApplicationData.Languages.LanguagesWindow.IncludePath ? windowInformation.FileName : System.IO.Path.GetFileNameWithoutExtension(windowInformation.FileName);
            }
            GetWindowProcessingInformationFromHandle(hwnd);
        }

        /// <summary>
        /// ウィンドウハンドルから「ウィンドウ処理」の情報を取得してコントロールに値を設定
        /// </summary>
        /// <param name="hwnd">ウィンドウハンドル</param>
        private void GetWindowProcessingInformationFromHandle(
            System.IntPtr hwnd
            )
        {
            try
            {
                MonitorInfoEx monitorInfo;
                MonitorInformation.GetMonitorWithWindow(hwnd, out monitorInfo, null);

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
            if ((string)((System.Windows.Controls.ComboBoxItem)XComboBox.SelectedItem).Content == Common.ApplicationData.Languages.LanguagesWindow.CoordinateSpecification)
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
            if ((string)((System.Windows.Controls.ComboBoxItem)YComboBox.SelectedItem).Content == Common.ApplicationData.Languages.LanguagesWindow.CoordinateSpecification)
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
            if ((string)((System.Windows.Controls.ComboBoxItem)WidthComboBox.SelectedItem).Content == Common.ApplicationData.Languages.LanguagesWindow.WidthSpecification)
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
            if ((string)((System.Windows.Controls.ComboBoxItem)HeightComboBox.SelectedItem).Content == Common.ApplicationData.Languages.LanguagesWindow.HeightSpecification)
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
        /// 選択している「基準にするディスプレイ」を取得
        /// </summary>
        /// <returns>基準にするディスプレイ</returns>
        private StandardDisplay GetSelectedDisplayToUseAsStandard()
        {
            StandardDisplay selected = StandardDisplay.DisplayWithWindow;
            string stringData;

            stringData = (string)((System.Windows.Controls.ComboBoxItem)StandardDisplayComboBox.SelectedItem).Content;
            if (stringData == Common.ApplicationData.Languages.LanguagesWindow.DisplayWithWindow)
            {
                selected = StandardDisplay.DisplayWithWindow;
            }
            else if (stringData == Common.ApplicationData.Languages.LanguagesWindow.TheSpecifiedDisplay)
            {
                selected = StandardDisplay.SpecifiedDisplay;
            }
            else if (stringData == Common.ApplicationData.Languages.LanguagesWindow.OnlyIfItIsOnTheSpecifiedDisplay)
            {
                selected = StandardDisplay.OnlySpecifiedDisplay;
            }

            return (selected);
        }

        /// <summary>
        /// 複数アクティブにできるか確認
        /// </summary>
        /// <returns>複数アクティブにできるかの値 (いいえ「false」/はい「true」)</returns>
        private bool CheckMultipleActivation()
        {
            bool result = false;
            if ((GetSelectedDisplayToUseAsStandard() == StandardDisplay.OnlySpecifiedDisplay)
                && (Common.ApplicationData.Settings.CoordinateType == CoordinateType.Display))
            {
                result = true;
            }
            return (result);
        }
    }
}
