namespace Swindom.Sources.WindowAndControls;

/// <summary>
/// システムトレイアイコンの処理
/// </summary>
public class NotifyIconProcessing
{
    /// <summary>
    /// 「閉じる」イベント
    /// </summary>
    public event EventHandler<CloseEventArgs> CloseEvent = delegate { };
    /// <summary>
    /// 「閉じる」イベントを実行
    /// </summary>
    public virtual void DoClose() => CloseEvent?.Invoke(this, new CloseEventArgs());

    /// <summary>
    /// ウィンドウハンドル用のウィンドウ
    /// </summary>
    private readonly HwndWindow HwndWindow = new();
    /// <summary>
    /// システムトレイアイコン
    /// </summary>
    private readonly TaskbarIcon TaskbarIcon = new();
    /// <summary>
    /// 「システムトレイアイコン」のContextMenu
    /// </summary>
    private ContextMenu TaskbarIconContextMenu = new();
    /// <summary>
    /// 「開く」MenuItem
    /// </summary>
    private readonly MenuItem OpenMenuItem = new();
    /// <summary>
    /// 「指定ウィンドウ」MenuItem
    /// </summary>
    private readonly MenuItem SpecifyWindowMenuItem = new()
    {
        IsCheckable = true,
    };
    /// <summary>
    /// 「マグネット」MenuItem
    /// </summary>
    private readonly MenuItem MagnetMenuItem = new()
    {
        IsCheckable = true,
    };
    /// <summary>
    /// 「ホットキー」MenuItem
    /// </summary>
    private readonly MenuItem HotkeyMenuItem = new()
    {
        IsCheckable = true,
    };
    /// <summary>
    /// 「指定ウィンドウの一括実行」MenuItem
    /// </summary>
    private readonly MenuItem BatchProcessingOfSpecifyWindowMenuItem = new();
    /// <summary>
    /// 「ウィンドウが画面外の場合は画面内に移動」MenuItem
    /// </summary>
    private readonly MenuItem DoOutsideToInsideMenuItem = new();
    /// <summary>
    /// 「ウィンドウ情報取得」MenuItem
    /// </summary>
    private readonly MenuItem GetInformationWindowMenuItem = new();
    /// <summary>
    /// 「数値計算」MenuItem
    /// </summary>
    private readonly MenuItem NumericCalculationWindowMenuItem = new();
    /// <summary>
    /// 「ディスプレイ情報更新」MenuItem
    /// </summary>
    private readonly MenuItem DisplayInformationUpdateMenuItem = new();
    /// <summary>
    /// 「終了」MenuItem
    /// </summary>
    private readonly MenuItem EndMenuItem = new();

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public NotifyIconProcessing()
    {
        try
        {
            // 言語ファイルを作成する処理 (必要ないときはコメントアウト)
            //LanguageFileProcessing.ReadLanguage();
            //string tempPath = VariousProcessing.GetApplicationDirectoryPath() + "Languages/ja-JP.lang";
            //LanguageFileProcessing.WriteLanguages(tempPath);

            // システムで設定されている言語を読み込む (言語ファイルが無い場合は英語。言語ファイルが無い場合は読み込まない。)
            string language = "";        // 言語
            try
            {
                string[] listOfLanguages = Directory.GetFiles(VariousProcessing.GetLanguageDirectoryPath(), "*" + Common.LanguageFileExtension);     // 存在する言語ファイルパスの一覧
                if (listOfLanguages.Length != 0)
                {
                    string currentCultureName = System.Globalization.CultureInfo.CurrentCulture.Name;       // システムで設定されている言語
                    // システムで設定されている言語のファイルがあるか確認
                    foreach (string nowLanguage in listOfLanguages)
                    {
                        if (currentCultureName == Path.GetFileNameWithoutExtension(nowLanguage))
                        {
                            language = currentCultureName;
                            break;
                        }
                    }
                    // システムで設定されている言語のファイルがない場合は英語のファイルがあるか確認
                    if (string.IsNullOrEmpty(language))
                    {
                        foreach (string nowLanguage in listOfLanguages)
                        {
                            if (Path.GetFileNameWithoutExtension(nowLanguage).Contains("en-US" + Common.LanguageFileExtension))
                            {
                                language = nowLanguage;
                                break;
                            }
                        }
                    }
                }

                if (string.IsNullOrEmpty(language) == false)
                {
                    LanguageFileProcessing.ReadLanguage(language);
                }
            }
            catch
            {
            }

            // 設定ファイルを読み込む
            bool firstRun = true;        // 初めての実行かの値
            // コマンドラインでファイルが指定されている場合はパスを取り出す
            foreach (string nowCommand in Environment.GetCommandLineArgs())
            {
                if (nowCommand.Contains(Common.SettingFileSpecificationString))
                {
                    ApplicationData.SpecifySettingsFilePath = nowCommand.Remove(0, Common.SettingFileSpecificationString.Length);
                    firstRun = false;
                    break;
                }
            }
            if (SettingFileProcessing.ReadSettings() == false)
            {
                SettingFileProcessing.WriteSettings();
            }
            else
            {
                firstRun = false;
                // 読み込んでいた言語と設定している言語が違う場合は設定している言語を読み込む
                if (language != ApplicationData.Settings.Language)
                {
                    LanguageFileProcessing.ReadLanguage();
                }
            }

            if (ApplicationData.Settings.DarkMode)
            {
                ThemeManager.Current.ApplicationTheme = ApplicationTheme.Dark;
            }
            else
            {
                ThemeManager.Current.ApplicationTheme = ApplicationTheme.Light;
            }
            Common.PathMaxLength = ApplicationData.Settings.UseLongPath ? Common.LongPathMaxLength : Common.PathMaxLength;
            SetTextOnControls();

            SpecifyWindowMenuItem.IsChecked = ApplicationData.Settings.SpecifyWindowInformation.Enabled;
            MagnetMenuItem.IsChecked = ApplicationData.Settings.MagnetInformation.Enabled;
            HotkeyMenuItem.IsChecked = ApplicationData.Settings.HotkeyInformation.Enabled;
            OpenMenuItem.Click += OpenMenuItem_Click;
            SpecifyWindowMenuItem.Click += SpecifyWindowMenuItem_Click;
            MagnetMenuItem.Click += MagnetMenuItem_Click;
            HotkeyMenuItem.Click += HotkeyMenuItem_Click;
            BatchProcessingOfSpecifyWindowMenuItem.Click += BatchProcessingOfSpecifyWindowMenuItem_Click;
            DoOutsideToInsideMenuItem.Click += DoOutsideToInsideMenuItem_Click;
            GetInformationWindowMenuItem.Click += GetInformationWindowMenuItem_Click;
            NumericCalculationWindowMenuItem.Click += NumericCalculationWindowMenuItem_Click;
            DisplayInformationUpdateMenuItem.Click += DisplayInformationUpdateMenuItem_Click;
            EndMenuItem.Click += EndMenuItem_Click;
            SettingsContextMenu();

            TaskbarIcon.ToolTipText = Common.ApplicationName;
            SetIconOnSystemTrayIcon();
            TaskbarIcon.ContextMenu = TaskbarIconContextMenu;
            TaskbarIcon.TrayMouseDoubleClick += TaskbarIcon_TrayMouseDoubleClick;

            HwndWindow.Create();
            ApplicationData.MainHwnd = HwndWindow.Hwnd;

            ApplicationData.EventData.ProcessingEvent += ApplicationData_ProcessingEvent;
            ApplicationData.WindowProcessingManagement.Initialize();

            // 更新確認
            bool checkDeleteLanguageData = true;        // 言語データを削除するかの値
            if (ApplicationData.Settings.AutomaticallyUpdateCheck)
            {
                try
                {
                    switch (FreeEcho.FECheckForUpdate.CheckForUpdate.CheckForUpdateFileURL(VariousProcessing.OwnApplicationPath(), Common.UpdateCheckURL, out FreeEcho.FECheckForUpdate.VersionInformation? versionInformation, ApplicationData.Settings.CheckBetaVersion))
                    {
                        case FreeEcho.FECheckForUpdate.CheckForUpdateResult.NotLatestVersion:
                            checkDeleteLanguageData = false;
                            ApplicationData.WindowManagement.ShowUpdateCheckWindow();
                            break;
                    }
                }
                catch
                {
                }
            }

            if (firstRun)
            {
                ApplicationData.WindowManagement.ShowMainWindow(true);
            }

            if (checkDeleteLanguageData)
            {
                ApplicationData.WindowManagement.UnnecessaryLanguageDataDelete();
            }

            // 更新確認ウィンドウの動作確認処理 (必要ないときはコメントアウト)
            //ApplicationData.WindowManagement.ShowUpdateCheckWindow();

            //FEMessageBox.Show("あいうえおあいうえおあいうえおあいうえおあいうえおあいうえおあいうえおあいうえおあいうえおあいうえおあいうえおあいうえおあいうえおあいうえおあいうえおあいうえおあいうえおあいうえおあいうえおあいうえおあいうえおあいうえおあいうえおかきくけこかきくけこかきくけこかきくけこかきくけこかきくけこかきくけこかきくけこかきくけこかきくけこかきくけこかきくけこかきくけこかきくけこかきくけこかきくけこかきくけこ", ApplicationData.Languages.Check + Common.CopySeparateString + Common.ApplicationName, MessageBoxButton.OK);
            //FEMessageBox.Show("あ", "あいうえおあいうえおあいうえおあいうえおあいうえおあいうえおあいうえおあいうえおあいうえおあいうえおあいうえおあいうえおあいうえおあいうえお" + Common.CopySeparateString + Common.ApplicationName, MessageBoxButton.OK);
            //FEMessageBox.Show("メッセージ表示 - Google", "メッセージ", MessageBoxButton.OK);
        }
        catch
        {
            FEMessageBox.Show(ApplicationData.Languages.ErrorOccurred, ApplicationData.Languages.Check + Common.CopySeparateString + Common.ApplicationName, MessageBoxButton.OK);
            DoClose();
        }
    }

    /// <summary>
    /// システムトレイアイコンの「TrayMouseDoubleClick」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void TaskbarIcon_TrayMouseDoubleClick(
        object sender,
        RoutedEventArgs e
        )
    {
        try
        {
            ApplicationData.WindowManagement.ShowMainWindow();
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「開く」MenuItemの「Click」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OpenMenuItem_Click(
        object sender,
        EventArgs e
        )
    {
        try
        {
            ApplicationData.WindowManagement.ShowMainWindow();
        }
        catch
        {
            FEMessageBox.Show(ApplicationData.Languages.ErrorOccurred, ApplicationData.Languages.Check + Common.CopySeparateString + Common.ApplicationName, MessageBoxButton.OK);
        }
    }

    /// <summary>
    /// 「指定ウィンドウ」MenuItemの「Click」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void SpecifyWindowMenuItem_Click(
        object sender,
        EventArgs e
        )
    {
        try
        {
            ApplicationData.Settings.SpecifyWindowInformation.Enabled = !ApplicationData.Settings.SpecifyWindowInformation.Enabled;
            ApplicationData.EventData.DoProcessingEvent(ProcessingEventType.SpecifyWindowProcessingStateChanged);
            SpecifyWindowMenuItem.IsChecked = ApplicationData.Settings.SpecifyWindowInformation.Enabled;
        }
        catch
        {
            FEMessageBox.Show(ApplicationData.Languages.ErrorOccurred, ApplicationData.Languages.Check + Common.CopySeparateString + Common.ApplicationName, MessageBoxButton.OK);
        }
    }

    /// <summary>
    /// 「マグネット」MenuItemの「Click」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void MagnetMenuItem_Click(
        object sender,
        EventArgs e
        )
    {
        try
        {
            ApplicationData.Settings.MagnetInformation.Enabled = !ApplicationData.Settings.MagnetInformation.Enabled;
            ApplicationData.EventData.DoProcessingEvent(ProcessingEventType.MagnetProcessingStateChanged);
            MagnetMenuItem.IsChecked = ApplicationData.Settings.MagnetInformation.Enabled;
        }
        catch
        {
            FEMessageBox.Show(ApplicationData.Languages.ErrorOccurred, ApplicationData.Languages.Check + Common.CopySeparateString + Common.ApplicationName, MessageBoxButton.OK);
        }
    }

    /// <summary>
    /// 「ホットキー」MenuItemの「Click」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void HotkeyMenuItem_Click(
        object sender,
        RoutedEventArgs e
        )
    {
        try
        {
            ApplicationData.Settings.HotkeyInformation.Enabled = !ApplicationData.Settings.HotkeyInformation.Enabled;
            ApplicationData.EventData.DoProcessingEvent(ProcessingEventType.HotkeyValidState);
            HotkeyMenuItem.IsChecked = ApplicationData.Settings.HotkeyInformation.Enabled;
        }
        catch
        {
            FEMessageBox.Show(ApplicationData.Languages.ErrorOccurred, ApplicationData.Languages.Check + Common.CopySeparateString + Common.ApplicationName, MessageBoxButton.OK);
        }
    }

    /// <summary>
    /// 「指定ウィンドウの一括実行」MenuItemの「Click」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void BatchProcessingOfSpecifyWindowMenuItem_Click(
        object sender,
        EventArgs e
        )
    {
        try
        {
            ApplicationData.EventData.DoProcessingEvent(ProcessingEventType.BatchProcessingOfSpecifyWindow);
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「ウィンドウが画面外の場合は画面内に移動」MenuItemの「Click」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void DoOutsideToInsideMenuItem_Click(
        object sender,
        RoutedEventArgs e
        )
    {
        try
        {
            VariousWindowProcessing.MoveWindowIntoScreen();
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「ウィンドウ情報の取得」MenuItemの「Click」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void GetInformationWindowMenuItem_Click(
        object sender,
        EventArgs e
        )
    {
        try
        {
            ApplicationData.WindowManagement.ShowGetInformationWindow();
        }
        catch
        {
            FEMessageBox.Show(ApplicationData.Languages.ErrorOccurred, ApplicationData.Languages.Check + Common.CopySeparateString + Common.ApplicationName, MessageBoxButton.OK);
        }
    }

    /// <summary>
    /// 「数値計算」MenuItemの「Click」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void NumericCalculationWindowMenuItem_Click(
        object sender,
        EventArgs e
        )
    {
        try
        {
            ApplicationData.WindowManagement.ShowNumericCalculationWindow();
        }
        catch
        {
            FEMessageBox.Show(ApplicationData.Languages.ErrorOccurred, ApplicationData.Languages.Check + Common.CopySeparateString + Common.ApplicationName, MessageBoxButton.OK);
        }
    }

    /// <summary>
    /// 「ディスプレイ情報更新」MenuItemの「Click」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void DisplayInformationUpdateMenuItem_Click(
        object sender,
        RoutedEventArgs e
        )
    {
        try
        {
            ApplicationData.MonitorInformation = MonitorInformation.GetMonitorInformation();
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「終了」MenuItemの「Click」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void EndMenuItem_Click(
        object sender,
        RoutedEventArgs e
        )
    {
        try
        {
            ApplicationData.Release();
            ApplicationData.WindowManagement.CloseAll();
            HwndWindow.Destroy();
            DoClose();
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
                case ProcessingEventType.SpecifyWindowProcessingStateChanged:
                    SettingFileProcessing.WriteSettings();
                    SpecifyWindowMenuItem.IsChecked = ApplicationData.Settings.SpecifyWindowInformation.Enabled;
                    SetIconOnSystemTrayIcon();
                    break;
                case ProcessingEventType.MagnetProcessingStateChanged:
                    SettingFileProcessing.WriteSettings();
                    MagnetMenuItem.IsChecked = ApplicationData.Settings.MagnetInformation.Enabled;
                    SetIconOnSystemTrayIcon();
                    break;
                case ProcessingEventType.HotkeyValidState:
                    SettingFileProcessing.WriteSettings();
                    HotkeyMenuItem.IsChecked = ApplicationData.Settings.HotkeyInformation.Enabled;
                    SetIconOnSystemTrayIcon();
                    break;
                case ProcessingEventType.LanguageChanged:
                    SetTextOnControls();
                    break;
                case ProcessingEventType.ThemeChanged:
                    SettingsContextMenu();
                    break;
            }
        }
        catch
        {
        }
    }

    /// <summary>
    /// システムトレイアイコンにアイコンを設定
    /// </summary>
    private void SetIconOnSystemTrayIcon()
    {
        TaskbarIcon.Icon = ApplicationData.Settings.SpecifyWindowInformation.Enabled
            || ApplicationData.Settings.MagnetInformation.Enabled
            || ApplicationData.Settings.HotkeyInformation.Enabled
            ? Properties.Resources.ApplicationIcon : Properties.Resources.ApplicationIconInvalid;
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
                throw new Exception("Languages value is null. - NotifyIconWindow.SetTextOnControls()");
            }

            FEMessageBoxWindow.Ok = ApplicationData.Languages.LanguagesWindow.Ok;
            FEMessageBoxWindow.Yes = ApplicationData.Languages.LanguagesWindow.Yes;
            FEMessageBoxWindow.No = ApplicationData.Languages.LanguagesWindow.No;
            FEMessageBoxWindow.Cancel = ApplicationData.Languages.LanguagesWindow.Cancel;

            OpenMenuItem.Header = ApplicationData.Languages.LanguagesWindow.Open;
            SpecifyWindowMenuItem.Header = ApplicationData.Languages.LanguagesWindow.Event;
            MagnetMenuItem.Header = ApplicationData.Languages.LanguagesWindow.Magnet;
            HotkeyMenuItem.Header = ApplicationData.Languages.LanguagesWindow.Hotkey;
            BatchProcessingOfSpecifyWindowMenuItem.Header = ApplicationData.Languages.LanguagesWindow.BatchProcessingOfSpecifyWindow;
            DoOutsideToInsideMenuItem.Header = ApplicationData.Languages.LanguagesWindow.WindowMoveOutsideToInside;
            GetInformationWindowMenuItem.Header = ApplicationData.Languages.LanguagesWindow.GetWindowInformation;
            NumericCalculationWindowMenuItem.Header = ApplicationData.Languages.LanguagesWindow.NumericCalculation;
            DisplayInformationUpdateMenuItem.Header = ApplicationData.Languages.LanguagesWindow.DisplayInformationUpdate;
            EndMenuItem.Header = ApplicationData.Languages.LanguagesWindow.End;
        }
        catch
        {
        }
    }

    /// <summary>
    /// システムトレイアイコンのContextMenuを設定
    /// </summary>
    private void SettingsContextMenu()
    {
        TaskbarIconContextMenu.Items.Clear();
        TaskbarIconContextMenu = new();
        TaskbarIcon.ContextMenu = TaskbarIconContextMenu;

        TaskbarIconContextMenu.Items.Add(OpenMenuItem);
        TaskbarIconContextMenu.Items.Add(SpecifyWindowMenuItem);
        TaskbarIconContextMenu.Items.Add(MagnetMenuItem);
        TaskbarIconContextMenu.Items.Add(HotkeyMenuItem);
        TaskbarIconContextMenu.Items.Add(BatchProcessingOfSpecifyWindowMenuItem);
        TaskbarIconContextMenu.Items.Add(DoOutsideToInsideMenuItem);
        TaskbarIconContextMenu.Items.Add(GetInformationWindowMenuItem);
        TaskbarIconContextMenu.Items.Add(NumericCalculationWindowMenuItem);
        TaskbarIconContextMenu.Items.Add(DisplayInformationUpdateMenuItem);
        TaskbarIconContextMenu.Items.Add(EndMenuItem);
    }
}
