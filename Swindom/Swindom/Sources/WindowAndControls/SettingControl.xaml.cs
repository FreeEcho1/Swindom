namespace Swindom;

/// <summary>
/// 「設定」コントロール
/// </summary>
public partial class SettingControl : UserControl, IDisposable
{
    /// <summary>
    /// Disposeが呼ばれたかの値
    /// </summary>
    private bool Disposed;
    /// <summary>
    /// 「言語」ComboBoxの「SelectionChanged」イベント処理を停止するかの値
    /// </summary>
    private bool StopEventLanguageSelectionChanged;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public SettingControl()
    {
        if (Common.ApplicationData.Languages.LanguagesWindow == null)
        {
            throw new Exception("LanguagesWindow value is null. - SettingControl()");
        }

        InitializeComponent();

        SetTextOnControls();

        // 「全般」タブのコントロール
        try
        {
            foreach (string path in Directory.GetFiles(Processing.GetApplicationDirectoryPath() + Path.DirectorySeparatorChar + Common.LanguagesDirectoryName + Path.DirectorySeparatorChar))
            {
                string languageName = LanguageFileProcessing.GetLanguageNameFromLanguageFile(path);        // 言語名
                string fileName = Path.GetFileNameWithoutExtension(path);     // ファイル名

                ComboBoxItem newItem = new()
                {
                    Content = string.IsNullOrEmpty(languageName) ? fileName : languageName
                };
                if (fileName == Common.ApplicationData.Settings.Language)
                {
                    newItem.IsSelected = true;
                }
                LanguageComboBox.Items.Add(newItem);
            }
        }
        catch
        {
        }
        _ = InitializeControlsRunAtWindowsStartup();
        CheckForUpdatesIncludingBetaVersionToggleSwitch.IsOn = Common.ApplicationData.Settings.CheckBetaVersion;
        AutomaticallyCheckForUpdatesWhenRunToggleSwitch.IsOn = Common.ApplicationData.Settings.AutomaticallyUpdateCheck;
        string stringData;      // 文字列データ
        stringData = Common.ApplicationData.Settings.CoordinateType switch
        {
            CoordinateType.Global => Common.ApplicationData.Languages.LanguagesWindow.GlobalCoordinates,
            _ => Common.ApplicationData.Languages.LanguagesWindow.DisplayCoordinates
        };
        Processing.SelectComboBoxItem(CoordinateSpecificationMethodComboBox, stringData);
        if (Common.MonitorInformation.MonitorInfo.Count == 1)
        {
            CoordinateSpecificationMethodLabel.Visibility = Visibility.Collapsed;
            CoordinateSpecificationMethodComboBox.Visibility = Visibility.Collapsed;
        }

        // 「貼り付ける位置をずらす」タブのコントロール
        MoveThePastePositionToggleSwitch.IsOn = Common.ApplicationData.Settings.ShiftPastePosition.Enabled;
        LeftEdgeNumericUpDown.ValueInt = Common.ApplicationData.Settings.ShiftPastePosition.Left;
        TopEdgeNumericUpDown.ValueInt = Common.ApplicationData.Settings.ShiftPastePosition.Top;
        RightEdgeNumericUpDown.ValueInt = Common.ApplicationData.Settings.ShiftPastePosition.Right;
        BottomEdgeNumericUpDown.ValueInt = Common.ApplicationData.Settings.ShiftPastePosition.Bottom;

        LanguageComboBox.SelectionChanged += LanguageComboBox_SelectionChanged;
        RunAtWindowsStartupToggleSwitch.IsOnChange += RunAtWindowsStartupToggleSwitch_IsOnChange;
        CheckForUpdatesIncludingBetaVersionToggleSwitch.IsOnChange += CheckForUpdatesIncludingBetaVersionToggleSwitch_IsOnChange;
        AutomaticallyCheckForUpdatesWhenRunToggleSwitch.IsOnChange += AutomaticallyCheckForUpdatesWhenRunToggleSwitch_IsOnChange;
        CoordinateSpecificationMethodComboBox.SelectionChanged += CoordinateSpecificationMethodComboBox_SelectionChanged;
        MoveThePastePositionToggleSwitch.IsOnChange += MoveThePastePositionToggleSwitch_IsOnChange;
        LeftEdgeNumericUpDown.ChangeValue += LeftEdgeNumericUpDown_ChangeValue;
        TopEdgeNumericUpDown.ChangeValue += TopEdgeNumericUpDown_ChangeValue;
        RightEdgeNumericUpDown.ChangeValue += RightEdgeNumericUpDown_ChangeValue;
        BottomEdgeNumericUpDown.ChangeValue += BottomEdgeNumericUpDown_ChangeValue;

        Common.ApplicationData.ProcessingEvent += ApplicationData_ProcessingEvent;
    }

    /// <summary>
    /// デストラクタ
    /// </summary>
    ~SettingControl()
    {
        Dispose(false);
    }

    /// <summary>
    /// Dispose
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// 非公開Dispose
    /// </summary>
    /// <param name="disposing"></param>
    protected virtual void Dispose(
        bool disposing
        )
    {
        if (Disposed == false)
        {
            Disposed = true;
            Common.ApplicationData.ProcessingEvent -= ApplicationData_ProcessingEvent;
        }
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
            if (StopEventLanguageSelectionChanged == false)
            {
                string language = (string)((ComboBoxItem)LanguageComboBox.SelectedItem).Content;        // 言語
                foreach (string path in Directory.GetFiles(Processing.GetApplicationDirectoryPath() + Path.DirectorySeparatorChar + Common.LanguagesDirectoryName + Path.DirectorySeparatorChar))
                {
                    if (language == LanguageFileProcessing.GetLanguageNameFromLanguageFile(path))
                    {
                        language = Path.GetFileNameWithoutExtension(path);
                        break;
                    }
                }

                string previousLanguage = Common.ApplicationData.Settings.Language;      // 変更前の言語
                if (LanguageFileProcessing.ReadLanguage(language, false))
                {
                    Common.ApplicationData.DoProcessingEvent(ProcessingEventType.LanguageChanged);
                }
                else
                {
                    StopEventLanguageSelectionChanged = true;
                    if (0 < e.RemovedItems.Count)
                    {
                        object? removedItem = e.RemovedItems[0];
                        LanguageComboBox.SelectedItem = removedItem as ComboBoxItem;
                    }
                    FreeEcho.FEControls.MessageBox.Show(Common.ApplicationData.Languages.Check, Common.ApplicationData.Languages.ErrorOccurred, FreeEcho.FEControls.MessageBoxButton.Ok, Window.GetWindow(this));
                }
            }
        }
        catch
        {
            FreeEcho.FEControls.MessageBox.Show(Common.ApplicationData.Languages.Check, Common.ApplicationData.Languages.ErrorOccurred, FreeEcho.FEControls.MessageBoxButton.Ok, Window.GetWindow(this));
        }
        finally
        {
            StopEventLanguageSelectionChanged = false;
        }
    }

    /// <summary>
    /// 「Windows起動時に実行」ToggleSwitchの「IsOnChange」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void RunAtWindowsStartupToggleSwitch_IsOnChange(
        object sender,
        FreeEcho.FEControls.ToggleSwitchEventArgs e
        )
    {
        try
        {
            string? path = Processing.OwnApplicationPath();      // タスクスケジューラ処理の実行ファイルのパス

            // 有効化する
            if (RunAtWindowsStartupToggleSwitch.IsOn)
            {
                if (string.IsNullOrEmpty(path))
                {
                    StartupProcessing.RegisterStartup();
                }
                else
                {
                    FreeEcho.FEControls.MessageBoxResult result = FreeEcho.FEControls.MessageBox.Show(Common.ApplicationData.Languages.Check, Common.ApplicationData.Languages.LanguagesWindow?.NormalExecution, FreeEcho.FEControls.MessageBoxButton.YesNo, Window.GetWindow(this));
                    switch (result)
                    {
                        case FreeEcho.FEControls.MessageBoxResult.Yes:
                            StartupProcessing.RegisterStartup();
                            break;
                        case FreeEcho.FEControls.MessageBoxResult.No:
                            {
                                using Process process = new();
                                process.StartInfo.FileName = path;
                                process.StartInfo.Arguments = Common.CreateTask;
                                process.StartInfo.UseShellExecute = true;
                                process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                                process.StartInfo.Verb = "RunAs";
                                process.Start();
                                process.WaitForExit();
                                if (StartupProcessing.CheckTaskSchedulerRegistered())
                                {
                                    RunAtWindowsStartupToggleSwitch.Text = Common.ApplicationData.Languages.LanguagesWindow?.RunAtWindowsStartupAdministrator;
                                }
                                else
                                {
                                    throw new();
                                }
                            }
                            break;
                        default:
                            RunAtWindowsStartupToggleSwitch.IsOnDoNotEvent = false;
                            break;
                    }
                }
            }
            // 無効化する
            else
            {
                if (StartupProcessing.CheckTaskSchedulerRegistered())
                {
                    using Process process = new();
                    process.StartInfo.FileName = path;
                    process.StartInfo.Arguments = Common.DeleteTask;
                    process.StartInfo.UseShellExecute = true;
                    process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    process.StartInfo.Verb = "RunAs";
                    process.Start();
                    process.WaitForExit();
                    if (StartupProcessing.CheckTaskSchedulerRegistered())
                    {
                        throw new();
                    }
                    else
                    {
                        RunAtWindowsStartupToggleSwitch.Text = Common.ApplicationData.Languages.LanguagesWindow?.RunAtWindowsStartup;
                    }
                }
                else
                {
                    StartupProcessing.DeleteStartup();
                }
            }
        }
        catch
        {
            RunAtWindowsStartupToggleSwitch.IsOnDoNotEvent = !RunAtWindowsStartupToggleSwitch.IsOn;
            FreeEcho.FEControls.MessageBox.Show(Common.ApplicationData.Languages.Check, Common.ApplicationData.Languages.ErrorOccurred, FreeEcho.FEControls.MessageBoxButton.Ok, Window.GetWindow(this));
        }
    }

    /// <summary>
    /// 「実行時に自動で更新確認」ToggleSwitchの「IsOnChange」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void AutomaticallyCheckForUpdatesWhenRunToggleSwitch_IsOnChange(
        object sender,
        FreeEcho.FEControls.ToggleSwitchEventArgs e
        )
    {
        try
        {
            Common.ApplicationData.Settings.AutomaticallyUpdateCheck = AutomaticallyCheckForUpdatesWhenRunToggleSwitch.IsOn;
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「ベータバージョン確認」ToggleSwitchの「IsOnChange」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void CheckForUpdatesIncludingBetaVersionToggleSwitch_IsOnChange(
        object sender,
        FreeEcho.FEControls.ToggleSwitchEventArgs e
        )
    {
        try
        {
            Common.ApplicationData.Settings.CheckBetaVersion = CheckForUpdatesIncludingBetaVersionToggleSwitch.IsOn;
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「座標指定の方法」ComboBoxの「SelectionChanged」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void CoordinateSpecificationMethodComboBox_SelectionChanged(
        object sender,
        SelectionChangedEventArgs e
        )
    {
        try
        {
            string stringData = (string)((ComboBoxItem)CoordinateSpecificationMethodComboBox.SelectedItem).Content;
            if (stringData == Common.ApplicationData.Languages.LanguagesWindow?.DisplayCoordinates)
            {
                Common.ApplicationData.Settings.CoordinateType = CoordinateType.Display;
            }
            else if (stringData == Common.ApplicationData.Languages.LanguagesWindow?.GlobalCoordinates)
            {
                Common.ApplicationData.Settings.CoordinateType = CoordinateType.Global;
            }
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「画面端に貼り付ける場合にずらす」ToggleSwitchの「IsOnChange」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void MoveThePastePositionToggleSwitch_IsOnChange(
        object sender,
        FreeEcho.FEControls.ToggleSwitchEventArgs e
        )
    {
        try
        {
            Common.ApplicationData.Settings.ShiftPastePosition.Enabled = MoveThePastePositionToggleSwitch.IsOn;
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「左端」NumericUpDownの「ChangeValue」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void LeftEdgeNumericUpDown_ChangeValue(
        object sender,
        FreeEcho.FEControls.NumericUpDownChangeValueArgs e
        )
    {
        try
        {
            Common.ApplicationData.Settings.ShiftPastePosition.Left = LeftEdgeNumericUpDown.ValueInt;
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「上端」NumericUpDownの「ChangeValue」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void TopEdgeNumericUpDown_ChangeValue(
        object sender,
        FreeEcho.FEControls.NumericUpDownChangeValueArgs e
        )
    {
        try
        {
            Common.ApplicationData.Settings.ShiftPastePosition.Top = TopEdgeNumericUpDown.ValueInt;
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「右端」NumericUpDownの「ChangeValue」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void RightEdgeNumericUpDown_ChangeValue(
        object sender,
        FreeEcho.FEControls.NumericUpDownChangeValueArgs e
        )
    {
        try
        {
            Common.ApplicationData.Settings.ShiftPastePosition.Right = RightEdgeNumericUpDown.ValueInt;
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「下端」NumericUpDownの「ChangeValue」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void BottomEdgeNumericUpDown_ChangeValue(
        object sender,
        FreeEcho.FEControls.NumericUpDownChangeValueArgs e
        )
    {
        try
        {
            Common.ApplicationData.Settings.ShiftPastePosition.Bottom = BottomEdgeNumericUpDown.ValueInt;
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
            if (Common.ApplicationData.Languages.LanguagesWindow == null)
            {
                throw new Exception("LanguagesWindow value is null. - SettingControl.SetTextOnControls()");
            }

            SettingsGeneralTabItem.Header = Common.ApplicationData.Languages.LanguagesWindow.General;
            LanguageLabel.Content = Common.ApplicationData.Languages.LanguagesWindow.Language;
            TranslatorsLabel.Content = Common.ApplicationData.Languages.LanguagesWindow.Translators;
            RunAtWindowsStartupToggleSwitch.Text = StartupProcessing.CheckTaskSchedulerRegistered() ? Common.ApplicationData.Languages.LanguagesWindow.RunAtWindowsStartupAdministrator : Common.ApplicationData.Languages.LanguagesWindow.RunAtWindowsStartup;
            CheckForUpdatesIncludingBetaVersionToggleSwitch.Text = Common.ApplicationData.Languages.LanguagesWindow.CheckBetaVersion;
            AutomaticallyCheckForUpdatesWhenRunToggleSwitch.Text = Common.ApplicationData.Languages.LanguagesWindow.AutomaticUpdateCheck;
            CoordinateSpecificationMethodLabel.Content = Common.ApplicationData.Languages.LanguagesWindow.CoordinateSpecificationMethod;
            ((ComboBoxItem)CoordinateSpecificationMethodComboBox.Items[0]).Content = Common.ApplicationData.Languages.LanguagesWindow.DisplayCoordinates;
            ((ComboBoxItem)CoordinateSpecificationMethodComboBox.Items[1]).Content = Common.ApplicationData.Languages.LanguagesWindow.GlobalCoordinates;

            SettingsMoveThePastePositionTabItem.Header = Common.ApplicationData.Languages.LanguagesWindow.MoveThePastePosition;
            MoveThePastePositionToggleSwitch.Text = Common.ApplicationData.Languages.LanguagesWindow.MoveThePastePosition;
            LeftEdgeLabel.Content = Common.ApplicationData.Languages.LanguagesWindow.LeftEdge;
            TopEdgeLabel.Content = Common.ApplicationData.Languages.LanguagesWindow.TopEdge;
            RightEdgeLabel.Content = Common.ApplicationData.Languages.LanguagesWindow.RightEdge;
            BottomEdgeLabel.Content = Common.ApplicationData.Languages.LanguagesWindow.BottomEdge;
        }
        catch
        {
        }
    }

    /// <summary>
    /// スタートアップのコントロールの初期化
    /// </summary>
    /// <returns>Task</returns>
    private async System.Threading.Tasks.Task InitializeControlsRunAtWindowsStartup()
    {
        bool result = false;        // 結果
        bool startupAdministrator = false;      // 管理者権限

        try
        {
            await System.Threading.Tasks.Task.Run(() =>
            {
                result = StartupProcessing.CheckTaskSchedulerRegistered();
                if (result)
                {
                    startupAdministrator = true;
                }
                else
                {
                    if (StartupProcessing.CheckStartup())
                    {
                        result = true;
                    }
                }
            });
        }
        catch
        {
        }

        if (result)
        {
            RunAtWindowsStartupToggleSwitch.IsOnDoNotEvent = result;
            if (startupAdministrator)
            {
                RunAtWindowsStartupToggleSwitch.Text = Common.ApplicationData.Languages.LanguagesWindow?.RunAtWindowsStartupAdministrator;
            }
        }
    }
}
