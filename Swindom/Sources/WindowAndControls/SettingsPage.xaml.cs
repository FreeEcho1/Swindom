namespace Swindom;

/// <summary>
/// 「設定」ページ
/// </summary>
public partial class SettingsPage : Page
{
    /// <summary>
    /// 次のイベントをキャンセル
    /// </summary>
    private bool CancelNextEvent;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public SettingsPage()
    {
        InitializeComponent();

        SetTextOnControls();

        string stringData = "";

        // 「全般」タブ
        try
        {
            foreach (string path in Directory.GetFiles(ApplicationPath.GetLanguageDirectory()))
            {
                string languageName = LanguageFileProcessing.GetLanguageNameFromFile(path);        // 言語名
                string fileName = Path.GetFileNameWithoutExtension(path);     // ファイル名
                languageName = string.IsNullOrEmpty(languageName) ? fileName : languageName;

                ComboBoxItem newItem = new()
                {
                    Content = languageName
                };
                if (fileName == ApplicationData.Settings.Language)
                {
                    newItem.IsSelected = true;
                }
                LanguageComboBox.Items.Add(newItem);
            }
        }
        catch
        {
        }
        // 言語ファイルがない場合は非表示
        if (LanguageComboBox.Items.Count == 0)
        {
            LanguageLabel.Visibility = Visibility.Collapsed;
            LanguageStackPanel.Visibility = Visibility.Collapsed;
            DarkModeToggleSwitch.Margin = new();
        }
        if (ApplicationData.Settings.DarkMode)
        {
            DarkModeToggleSwitch.IsOn = true;
        }
        if (ApplicationStartup.CheckTaskRegistered())
        {
            StartupToggleSwitch.IsOn = true;
            StartupToggleSwitch.OffContent = StartupToggleSwitch.OnContent = ApplicationData.Strings.RunAtWindowsStartupAdministrator;
        }
        else if (ApplicationStartup.CheckStartup())
        {
            StartupToggleSwitch.IsOn = true;
        }
        CheckForUpdatesIncludingBetaVersionToggleSwitch.IsOn = ApplicationData.Settings.CheckBetaVersion;
        AutomaticallyCheckForUpdatesWhenRunToggleSwitch.IsOn = ApplicationData.Settings.AutomaticallyUpdateCheck;
        stringData = ApplicationData.Settings.CoordinateType switch
        {
            CoordinateType.PrimaryDisplay => ApplicationData.Strings.PrimaryDisplay,
            _ => ApplicationData.Strings.EachDisplay
        };
        ControlsProcessing.SelectComboBoxItem(CoordinateComboBox, stringData);
        if (ApplicationData.MonitorInformation.MonitorInfo.Count == 1)
        {
            CoordinateLabel.Visibility = Visibility.Collapsed;
            CoordinateStackPanel.Visibility = Visibility.Collapsed;
        }
        FullScreenWindowAdditionDecisionToggleSwitch.IsOn = ApplicationData.Settings.FullScreenWindowAdditionDecision;
        DoNotChangeDisplayToggleSwitch.IsOn = ApplicationData.Settings.DoNotChangeDisplaySettings;
        stringData = ApplicationData.Settings.DisplayChangeMode switch
        {
            DisplayChangeMode.Auto => ApplicationData.Strings.Auto,
            DisplayChangeMode.AutoOrManual => ApplicationData.Strings.OneAutoTwoOrMoreManual,
            DisplayChangeMode.Manual => ApplicationData.Strings.Manual,
            _=> ApplicationData.Strings.Auto
        };
        ControlsProcessing.SelectComboBoxItem(HowToChangeDisplayComboBox, stringData);
        LanguageComboBox.SelectionChanged += LanguageComboBox_SelectionChanged;
        StartupToggleSwitch.Toggled += StartupToggleSwitch_Toggled;
        DarkModeToggleSwitch.Toggled += DarkModeToggleSwitch_Toggled;
        CheckForUpdatesIncludingBetaVersionToggleSwitch.Toggled += CheckForUpdatesIncludingBetaVersionToggleSwitch_Toggled;
        AutomaticallyCheckForUpdatesWhenRunToggleSwitch.Toggled += AutomaticallyCheckForUpdatesWhenRunToggleSwitch_Toggled;
        CoordinateComboBox.SelectionChanged += CoordinateComboBox_SelectionChanged;
        CoordinateExplanationButton.Click += CoordinateExplanationButton_Click;
        FullScreenWindowAdditionDecisionToggleSwitch.Toggled += FullScreenWindowAdditionDecisionToggleSwitch_Toggled;
        DoNotChangeDisplayToggleSwitch.Toggled += DoNotChangeDisplayToggleSwitch_Toggled;
        HowToChangeDisplayComboBox.SelectionChanged += HowToChangeDisplayComboBox_SelectionChanged;

        // 「貼り付ける位置をずらす」タブ
        LeftEdgeNumberBox.Minimum = ShiftPastePosition.MinimumValue;
        LeftEdgeNumberBox.Maximum = ShiftPastePosition.MaximumValue;
        TopEdgeNumberBox.Minimum = ShiftPastePosition.MinimumValue;
        TopEdgeNumberBox.Maximum = ShiftPastePosition.MaximumValue;
        RightEdgeNumberBox.Minimum = ShiftPastePosition.MinimumValue;
        RightEdgeNumberBox.Maximum = ShiftPastePosition.MaximumValue;
        BottomEdgeNumberBox.Minimum = ShiftPastePosition.MinimumValue;
        BottomEdgeNumberBox.Maximum = ShiftPastePosition.MaximumValue;
        MoveThePastePositionToggleSwitch.IsOn = ApplicationData.Settings.ShiftPastePosition.IsEnabled;
        LeftEdgeNumberBox.Value = ApplicationData.Settings.ShiftPastePosition.Left;
        TopEdgeNumberBox.Value = ApplicationData.Settings.ShiftPastePosition.Top;
        RightEdgeNumberBox.Value = ApplicationData.Settings.ShiftPastePosition.Right;
        BottomEdgeNumberBox.Value = ApplicationData.Settings.ShiftPastePosition.Bottom;
        MoveThePastePositionToggleSwitch.Toggled += MoveThePastePositionToggleSwitch_Toggled;
        LeftEdgeNumberBox.ValueChanged += LeftEdgeNumberBox_ValueChanged;
        TopEdgeNumberBox.ValueChanged += TopEdgeNumberBox_ValueChanged;
        RightEdgeNumberBox.ValueChanged += RightEdgeNumberBox_ValueChanged;
        BottomEdgeNumberBox.ValueChanged += BottomEdgeNumberBox_ValueChanged;

        ApplicationData.EventData.ProcessingEvent += ApplicationData_ProcessingEvent;
    }

    /// <summary>
    /// 破棄
    /// </summary>
    public void Release()
    {
        ApplicationData.EventData.ProcessingEvent -= ApplicationData_ProcessingEvent;
    }

    /// <summary>
    /// 「言語」ComboBoxの「SelectionChanged」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void LanguageComboBox_SelectionChanged(
        object sender,
        SelectionChangedEventArgs e
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
                string selectedLanguage = (string)((ComboBoxItem)LanguageComboBox.SelectedItem).Content;        // 選択している言語
                foreach (string path in Directory.GetFiles(ApplicationPath.GetLanguageDirectory()))
                {
                    string languageName = LanguageFileProcessing.GetLanguageNameFromFile(path);
                    if (string.IsNullOrEmpty(languageName))
                    {
                        languageName = Path.GetFileNameWithoutExtension(path);
                    }
                    if (selectedLanguage == languageName)
                    {
                        selectedLanguage = Path.GetFileNameWithoutExtension(path);
                        break;
                    }
                }

                if (LanguageFileProcessing.ReadLanguage(selectedLanguage, false))
                {
                    ApplicationData.Settings.Language = selectedLanguage;
                    ApplicationData.EventData.DoProcessingEvent(ProcessingEventType.LanguageChanged);
                }
                // 言語ファイルを読み込めなかった場合は以前選択していた項目に戻す
                else
                {
                    if (0 < e.RemovedItems.Count)
                    {
                        object? removedItem = e.RemovedItems[0];
                        CancelNextEvent = true;
                        LanguageComboBox.SelectedItem = removedItem as ComboBoxItem;
                    }
                    FEMessageBox.Show(ApplicationData.Strings.ErrorOccurred, ApplicationData.Strings.Check, MessageBoxButton.OK);
                }
            }
        }
        catch
        {
            FEMessageBox.Show(ApplicationData.Strings.ErrorOccurred, ApplicationData.Strings.Check, MessageBoxButton.OK);
        }
    }

    /// <summary>
    /// 「Windows起動時に実行」ToggleSwitchの「Toggled」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void StartupToggleSwitch_Toggled(
        object sender,
        RoutedEventArgs e
        )
    {
        if (CancelNextEvent)
        {
            CancelNextEvent = false;
        }
        else
        {
            try
            {
                string? path = ApplicationPath.OwnApplicationPath();      // タスクスケジューラ処理の実行ファイルのパス

                // 有効化する
                if (StartupToggleSwitch.IsOn)
                {
                    switch (FEMessageBox.Show(ApplicationData.Strings.NormalExecution, ApplicationData.Strings.Check, MessageBoxButton.YesNo))
                    {
                        case MessageBoxResult.Yes:
                            ApplicationStartup.RegisterStartup();
                            break;
                        case MessageBoxResult.No:
                            {
                                using Process process = new();
                                process.StartInfo.FileName = path;
                                process.StartInfo.Arguments = ApplicationStartup.CreateTaskString;
                                process.StartInfo.UseShellExecute = true;
                                process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                                process.StartInfo.Verb = "RunAs";
                                process.Start();
                                process.WaitForExit();
                                if (ApplicationStartup.CheckTaskRegistered())
                                {
                                    StartupToggleSwitch.OffContent = StartupToggleSwitch.OnContent = ApplicationData.Strings.RunAtWindowsStartupAdministrator;
                                }
                                else
                                {
                                    throw new();
                                }
                            }
                            break;
                        default:
                            CancelNextEvent = true;
                            StartupToggleSwitch.IsOn = false;
                            break;
                    }
                }
                // 無効化する
                else
                {
                    if (ApplicationStartup.CheckTaskRegistered())
                    {
                        using Process process = new();
                        process.StartInfo.FileName = path;
                        process.StartInfo.Arguments = ApplicationStartup.DeleteTaskString;
                        process.StartInfo.UseShellExecute = true;
                        process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                        process.StartInfo.Verb = "RunAs";
                        process.Start();
                        process.WaitForExit();
                        if (ApplicationStartup.CheckTaskRegistered())
                        {
                            throw new();
                        }
                        else
                        {
                            StartupToggleSwitch.OffContent = StartupToggleSwitch.OnContent = ApplicationData.Strings.RunAtWindowsStartup;
                        }
                    }
                    else
                    {
                        ApplicationStartup.DeleteStartup();
                    }
                }
            }
            catch
            {
                StartupToggleSwitch.IsOn = !StartupToggleSwitch.IsOn;
                FEMessageBox.Show(ApplicationData.Strings.ErrorOccurred, ApplicationData.Strings.Check, MessageBoxButton.OK);
            }
        }
    }

    /// <summary>
    /// 「ダークモード」ToggleSwitchの「Toggled」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void DarkModeToggleSwitch_Toggled(
        object sender,
        RoutedEventArgs e
        )
    {
        try
        {
            ApplicationData.Settings.DarkMode = DarkModeToggleSwitch.IsOn;
            ThemeManager.Current.ApplicationTheme = DarkModeToggleSwitch.IsOn ? ApplicationTheme.Dark : ApplicationTheme.Light;
            ApplicationData.EventData.DoProcessingEvent(ProcessingEventType.ThemeChanged);
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「ベータバージョン確認」ToggleSwitchの「Toggled」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void CheckForUpdatesIncludingBetaVersionToggleSwitch_Toggled(
        object sender,
        RoutedEventArgs e
        )
    {
        try
        {
            ApplicationData.Settings.CheckBetaVersion = CheckForUpdatesIncludingBetaVersionToggleSwitch.IsOn;
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「実行時に自動で更新確認」ToggleSwitchの「Toggled」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void AutomaticallyCheckForUpdatesWhenRunToggleSwitch_Toggled(
        object sender,
        RoutedEventArgs e
        )
    {
        try
        {
            ApplicationData.Settings.AutomaticallyUpdateCheck = AutomaticallyCheckForUpdatesWhenRunToggleSwitch.IsOn;
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「座標」ComboBoxの「SelectionChanged」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <exception cref="NotImplementedException"></exception>
    private void CoordinateComboBox_SelectionChanged(
        object sender,
        SelectionChangedEventArgs e
        )
    {
        try
        {
            string stringData = (string)((ComboBoxItem)CoordinateComboBox.SelectedItem).Content;

            if (stringData == ApplicationData.Strings.PrimaryDisplay)
            {
                ApplicationData.Settings.CoordinateType = CoordinateType.PrimaryDisplay;
            }
            else
            {
                ApplicationData.Settings.CoordinateType = CoordinateType.EachDisplay;
            }
            ApplicationData.EventData.DoProcessingEvent(ProcessingEventType.CoordinateChanged);
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「全画面ウィンドウの追加判定 (正しく判定されない場合のみ)」ToggleSwitchの「Toggled」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void FullScreenWindowAdditionDecisionToggleSwitch_Toggled(
        object sender,
        RoutedEventArgs e
        )
    {
        try
        {
            ApplicationData.Settings.FullScreenWindowAdditionDecision = FullScreenWindowAdditionDecisionToggleSwitch.IsOn;
            ApplicationData.EventData.DoProcessingEvent(ProcessingEventType.FullScreenWindowAdditionDecisionChanged);
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「存在するディスプレイに設定を変更」-「変更しない」ToggleSwitchの「Toggled」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void DoNotChangeDisplayToggleSwitch_Toggled(
        object sender,
        RoutedEventArgs e
        )
    {
        try
        {
            ApplicationData.Settings.DoNotChangeDisplaySettings = DoNotChangeDisplayToggleSwitch.IsOn;
            ApplicationData.EventData.DoProcessingEvent(ProcessingEventType.DisplayInformationUpdate);
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「存在するディスプレイに設定を変更」-「変更する方法 (自動/手動)」ComboBoxの「SelectionChanged」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void HowToChangeDisplayComboBox_SelectionChanged(
        object sender,
        SelectionChangedEventArgs e
        )
    {
        try
        {
            string stringData = (string)((ComboBoxItem)HowToChangeDisplayComboBox.SelectedItem).Content;

            if (stringData == ApplicationData.Strings.Auto)
            {
                ApplicationData.Settings.DisplayChangeMode = DisplayChangeMode.Auto;
            }
            else if (stringData == ApplicationData.Strings.OneAutoTwoOrMoreManual)
            {
                ApplicationData.Settings.DisplayChangeMode = DisplayChangeMode.AutoOrManual;
            }
            else if (stringData == ApplicationData.Strings.Manual)
            {
                ApplicationData.Settings.DisplayChangeMode = DisplayChangeMode.Manual;
            }
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「座標」の「?」Buttonの「Click」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void CoordinateExplanationButton_Click(
        object sender,
        RoutedEventArgs e
        )
    {
        try
        {
            ApplicationData.WindowManagement.ShowExplanationWindow(SelectExplanationType.Coordinate);
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「貼り付ける位置をずらす」ToggleSwitchの「Toggled」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void MoveThePastePositionToggleSwitch_Toggled(
        object sender,
        RoutedEventArgs e
        )
    {
        try
        {
            ApplicationData.Settings.ShiftPastePosition.IsEnabled = MoveThePastePositionToggleSwitch.IsOn;
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「左端」NumberBoxの「ValueChanged」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void LeftEdgeNumberBox_ValueChanged(
        ModernWpf.Controls.NumberBox sender,
        ModernWpf.Controls.NumberBoxValueChangedEventArgs args
        )
    {
        try
        {
            ApplicationData.Settings.ShiftPastePosition.Left = double.IsNaN(LeftEdgeNumberBox.Value) ? 0 : (int)LeftEdgeNumberBox.Value;
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「上端」NumberBoxの「ValueChanged」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void TopEdgeNumberBox_ValueChanged(
        ModernWpf.Controls.NumberBox sender,
        ModernWpf.Controls.NumberBoxValueChangedEventArgs args
        )
    {
        try
        {
            ApplicationData.Settings.ShiftPastePosition.Top = double.IsNaN(TopEdgeNumberBox.Value) ? 0 : (int)TopEdgeNumberBox.Value;
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「右端」NumberBoxの「ValueChanged」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void RightEdgeNumberBox_ValueChanged(
        ModernWpf.Controls.NumberBox sender,
        ModernWpf.Controls.NumberBoxValueChangedEventArgs args
        )
    {
        try
        {
            ApplicationData.Settings.ShiftPastePosition.Right = double.IsNaN(RightEdgeNumberBox.Value) ? 0 : (int)RightEdgeNumberBox.Value;
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「下端」NumberBoxの「ValueChanged」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void BottomEdgeNumberBox_ValueChanged(
        ModernWpf.Controls.NumberBox sender,
        ModernWpf.Controls.NumberBoxValueChangedEventArgs args
        )
    {
        try
        {
            ApplicationData.Settings.ShiftPastePosition.Bottom = double.IsNaN(BottomEdgeNumberBox.Value) ? 0 : (int)BottomEdgeNumberBox.Value;
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
                case ProcessingEventType.LanguageChanged:
                    SetTextOnControls();
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
            GeneralTabItem.Header = ApplicationData.Strings.General;
            LanguageLabel.Content = ApplicationData.Strings.Language;
            TranslatorsLabel.Content = ApplicationData.Strings.Translators;
            DarkModeToggleSwitch.OffContent = DarkModeToggleSwitch.OnContent = ApplicationData.Strings.DarkMode;
            StartupToggleSwitch.OffContent = StartupToggleSwitch.OnContent = ApplicationStartup.CheckTaskRegistered() ? ApplicationData.Strings.RunAtWindowsStartupAdministrator : ApplicationData.Strings.RunAtWindowsStartup;
            CheckForUpdatesIncludingBetaVersionToggleSwitch.OffContent = CheckForUpdatesIncludingBetaVersionToggleSwitch.OnContent = ApplicationData.Strings.CheckBetaVersion;
            AutomaticallyCheckForUpdatesWhenRunToggleSwitch.OffContent = AutomaticallyCheckForUpdatesWhenRunToggleSwitch.OnContent = ApplicationData.Strings.AutomaticUpdateCheck;
            CoordinateLabel.Content = ApplicationData.Strings.Coordinate;
            CoordinateEachDisplayComboBoxItem.Content = ApplicationData.Strings.EachDisplay;
            CoordinatePrimaryDisplayComboBoxItem.Content = ApplicationData.Strings.PrimaryDisplay;
            CoordinateExplanationButton.Content = ApplicationData.Strings.Question;
            CoordinateExplanationButton.ToolTip = ApplicationData.Strings.Help;
            FullScreenWindowAdditionDecisionToggleSwitch.OffContent = FullScreenWindowAdditionDecisionToggleSwitch.OnContent = ApplicationData.Strings.FullScreenWindowAdditionDecision;
            ChangeSettingsExistDisplayGroupBox.Header = ApplicationData.Strings.ChangeSettingsExistDisplay;
            DoNotChangeDisplayToggleSwitch.OffContent = DoNotChangeDisplayToggleSwitch.OnContent = ApplicationData.Strings.DoNotChange;
            HowToChangeDisplayLabel.Content = ApplicationData.Strings.HowToChange;
            HowToChangeDisplayFirstDisplayComboBoxItem.Content = ApplicationData.Strings.Auto;
            HowToChangeDisplayAutoOrManualComboBoxItem.Content = ApplicationData.Strings.OneAutoTwoOrMoreManual;
            HowToChangeDisplayManualComboBoxItem.Content = ApplicationData.Strings.Manual;

            MoveThePastePositionTabItem.Header = ApplicationData.Strings.MovePastePosition;
            MoveThePastePositionToggleSwitch.OffContent = MoveThePastePositionToggleSwitch.OnContent = ApplicationData.Strings.MovePastePosition;
            LeftEdgeLabel.Content = ApplicationData.Strings.LeftEdge;
            TopEdgeLabel.Content = ApplicationData.Strings.TopEdge;
            RightEdgeLabel.Content = ApplicationData.Strings.RightEdge;
            BottomEdgeLabel.Content = ApplicationData.Strings.BottomEdge;
        }
        catch
        {
        }
    }
}
