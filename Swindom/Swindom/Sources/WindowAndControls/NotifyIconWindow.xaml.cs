namespace Swindom;

/// <summary>
/// NotifyIcon用のウィンドウ
/// </summary>
public partial class NotifyIconWindow : Window
{
    /// <summary>
    /// 「閉じる」イベント
    /// </summary>
    public event EventHandler<CloseEventArgs>? CloseEvent;
    /// <summary>
    /// 「閉じる」イベントを実行
    /// </summary>
    public virtual void DoClose() => CloseEvent?.Invoke(this, new CloseEventArgs());

    /// <summary>
    /// システムトレイアイコン
    /// </summary>
    private readonly NotifyIcon NotifyIcon = new();
    /// <summary>
    /// 「システムトレイアイコン」のContextMenu
    /// </summary>
    private readonly ContextMenu TaskbarIconContextMenu = new();
    /// <summary>
    /// 「開く」MenuItem
    /// </summary>
    private readonly MenuItem OpenMenuItem = new();
    /// <summary>
    /// 「イベント」MenuItem
    /// </summary>
    private readonly MenuItem EventMenuItem = new();
    /// <summary>
    /// 「タイマー」MenuItem
    /// </summary>
    private readonly MenuItem TimerMenuItem = new();
    /// <summary>
    /// 「マグネット」MenuItem
    /// </summary>
    private readonly MenuItem MagnetMenuItem = new();
    /// <summary>
    /// 「ホットキー」MenuItem
    /// </summary>
    private readonly MenuItem HotkeyMenuItem = new();
    /// <summary>
    /// 「イベントの一括実行」MenuItem
    /// </summary>
    private readonly MenuItem BatchProcessingOfEventMenuItem = new();
    /// <summary>
    /// 「タイマーの一括実行」MenuItem
    /// </summary>
    private readonly MenuItem BatchProcessingOfTimerMenuItem = new();
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
    /// マウスをフック
    /// </summary>
    private MouseHook? MouseHook;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public NotifyIconWindow()
    {
        InitializeComponent();

        Loaded += NotifyIcon_Loaded;
    }

    /// <summary>
    /// 「メイン」ウィンドウの「Loaded」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void NotifyIcon_Loaded(
        object sender,
        RoutedEventArgs e
        )
    {
        try
        {
            // 言語ファイルを作成する処理 (必要ないときはコメントアウト)
            //LanguageFileProcessing.ReadLanguage();
            //string tempPath = Processing.GetApplicationDirectoryPath() + "Languages/ja-JP.lang";
            //LanguageFileProcessing.WriteLanguages(tempPath);

            FreeEcho.FEControls.MessageBoxWindow.UseStyle = new Uri(Common.UriMessageBoxStyle);

            // システムで設定されている言語を読み込む (設定されている言語ファイルが無い場合は英語。言語ファイルが無い場合は読み込まない。)
            string language = "";        // 言語
            try
            {
                string path = Processing.GetApplicationDirectoryPath() + Path.DirectorySeparatorChar + Common.LanguagesDirectoryName + Path.DirectorySeparatorChar;     // パス
                string[] listOfLanguages = Directory.GetFiles(path, "*" + Common.LanguageFileExtension);     // 存在する言語ファイルの一覧
                if (listOfLanguages.Length != 0)
                {
                    bool checkLanguageFile = false;     // 言語ファイルの存在確認
                    language = System.Globalization.CultureInfo.CurrentCulture.Name;
                    foreach (string nowLanguage in listOfLanguages)
                    {
                        if (language == Path.GetFileNameWithoutExtension(nowLanguage))
                        {
                            checkLanguageFile = true;
                            break;
                        }
                    }
                    if (checkLanguageFile == false)
                    {
                        foreach (string nowLanguage in listOfLanguages)
                        {
                            if (Path.GetFileNameWithoutExtension(nowLanguage).Contains("en-US"))
                            {
                                language = nowLanguage;
                                checkLanguageFile = true;
                                break;
                            }
                        }
                        if (checkLanguageFile == false)
                        {
                            language = "";
                        }
                    }
                }
            }
            catch
            {
            }
            LanguageFileProcessing.ReadLanguage(language);

            // 設定ファイルを読み込む
            bool firstRun = true;        // 初めての実行かの値
            foreach (string nowCommand in Environment.GetCommandLineArgs())
            {
                if (nowCommand.Contains(Common.SelectSettingFile))
                {
                    Common.ApplicationData.SpecifySettingsFilePath = nowCommand.Remove(0, Common.SelectSettingFile.Length);
                    firstRun = false;
                    break;
                }
            }
            firstRun = SettingFileProcessing.CheckAndCreateSettingFile();
            if (firstRun == false)
            {
                SettingFileProcessing.ReadSettings();
                // 読み込んでいた言語と設定している言語が違う場合は設定している言語を読み込む
                if (language != Common.ApplicationData.Settings.Language)
                {
                    LanguageFileProcessing.ReadLanguage();
                }
            }

            Common.PathLength = Common.ApplicationData.Settings.UseLongPath ? Common.LongPathMaxLength : Common.PathMaxLength;
            SetTextOnControls();

            // システムトレイアイコン用のメニューの初期化
            EventMenuItem.IsChecked = Common.ApplicationData.Settings.EventInformation.Enabled;
            TimerMenuItem.IsChecked = Common.ApplicationData.Settings.TimerInformation.Enabled;
            MagnetMenuItem.IsChecked = Common.ApplicationData.Settings.MagnetInformation.Enabled;
            HotkeyMenuItem.IsChecked = Common.ApplicationData.Settings.HotkeyInformation.Enabled;
            OpenMenuItem.Click += OpenMenuItem_Click;
            EventMenuItem.Click += EventMenuItem_Click;
            TimerMenuItem.Click += TimerMenuItem_Click;
            MagnetMenuItem.Click += MagnetMenuItem_Click;
            HotkeyMenuItem.Click += HotkeyMenuItem_Click;
            BatchProcessingOfEventMenuItem.Click += BatchProcessingOfEventMenuItem_Click;
            BatchProcessingOfTimerMenuItem.Click += BatchProcessingOfTimerMenuItem_Click;
            DoOutsideToInsideMenuItem.Click += DoOutsideToInsideMenuItem_Click;
            GetInformationWindowMenuItem.Click += GetInformationWindowMenuItem_Click;
            NumericCalculationWindowMenuItem.Click += NumericCalculationWindowMenuItem_Click;
            DisplayInformationUpdateMenuItem.Click += DisplayInformationUpdateMenuItem_Click;
            EndMenuItem.Click += EndMenuItem_Click;
            TaskbarIconContextMenu.Items.Add(OpenMenuItem);
            TaskbarIconContextMenu.Items.Add(EventMenuItem);
            TaskbarIconContextMenu.Items.Add(TimerMenuItem);
            TaskbarIconContextMenu.Items.Add(MagnetMenuItem);
            TaskbarIconContextMenu.Items.Add(HotkeyMenuItem);
            TaskbarIconContextMenu.Items.Add(BatchProcessingOfEventMenuItem);
            TaskbarIconContextMenu.Items.Add(BatchProcessingOfTimerMenuItem);
            TaskbarIconContextMenu.Items.Add(DoOutsideToInsideMenuItem);
            TaskbarIconContextMenu.Items.Add(GetInformationWindowMenuItem);
            TaskbarIconContextMenu.Items.Add(NumericCalculationWindowMenuItem);
            TaskbarIconContextMenu.Items.Add(DisplayInformationUpdateMenuItem);
            TaskbarIconContextMenu.Items.Add(EndMenuItem);
            // システムトレイアイコンの初期化
            Common.ApplicationData.SystemTrayIconHwnd = new WindowInteropHelper(this).Handle;
            Icon icon = (Common.ApplicationData.Settings.EventInformation.Enabled || Common.ApplicationData.Settings.TimerInformation.Enabled) ? Properties.Resources.ApplicationIcon : Properties.Resources.ApplicationIconInvalid;
            NotifyIcon.LeftButtonDoubleClickEvent += NotifyIcon_LeftButtonDoubleClickEvent;
            NotifyIcon.RightButtonClickEvent += NotifyIcon_RightButtonClickEvent;
            NotifyIcon.Add(this, 1, Common.ApplicationData.SystemTrayIconHwnd, icon, Common.ApplicationName, Common.MaxRetryNotifyIcon);

            Common.ApplicationData.ProcessingEvent += ApplicationData_ProcessingEvent;
            Common.ApplicationData.WindowProcessingManagement.Initialize();
            Common.ApplicationData.WindowManagement.UnnecessaryLanguageDataDelete();

            // 更新確認
            if (Common.ApplicationData.Settings.AutomaticallyUpdateCheck)
            {
                try
                {
                    switch (FreeEcho.FECheckForUpdate.CheckForUpdate.CheckForUpdateFileURL(Processing.OwnApplicationPath(), Common.UpdateCheckURL, out FreeEcho.FECheckForUpdate.VersionInformation? versionInformation, Common.ApplicationData.Settings.CheckBetaVersion))
                    {
                        case FreeEcho.FECheckForUpdate.CheckForUpdateResult.NotLatestVersion:
                            Common.ApplicationData.WindowManagement.ShowUpdateCheckWindow();
                            break;
                    }
                }
                catch
                {
                }
            }

            if (firstRun)
            {
                Common.ApplicationData.WindowManagement.ShowMainWindow(true);
            }

            // 更新確認ウィンドウの動作確認処理 (必要ないときはコメントアウト)
            //LanguageFileProcessing.ReadLanguage();
            //Common.ApplicationData.WindowManagement.ShowUpdateCheckWindow();
        }
        catch
        {
            FreeEcho.FEControls.MessageBox.Show(Common.ApplicationData.Languages.Check + Common.CopySeparateString + Common.ApplicationName, Common.ApplicationData.Languages.ErrorOccurred, FreeEcho.FEControls.MessageBoxButton.Ok);
            Close();
            DoClose();
        }
    }

    /// <summary>
    /// システムトレイアイコンで「左ボタンがダブルクリックされた」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void NotifyIcon_LeftButtonDoubleClickEvent(
        object? sender,
        NotifyIconMouseEventArgs e
        )
    {
        try
        {
            Common.ApplicationData.WindowManagement.ShowMainWindow();
        }
        catch
        {
        }
    }

    /// <summary>
    /// システムトレイアイコンで「右ボタンがクリックされた」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void NotifyIcon_RightButtonClickEvent(
        object? sender,
        NotifyIconMouseEventArgs e
        )
    {
        try
        {
            if (MouseHook == null)
            {
                MouseHook = new();
                MouseHook.LButtonUpEvent += MouseHook_LButtonUpEvent;
                MouseHook.Hook();
            }
            TaskbarIconContextMenu.IsOpen = true;
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「マウスの左ボタンが離された」マウスフックのイベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void MouseHook_LButtonUpEvent(
        object sender,
        MouseEventArgs e
        )
    {
        try
        {
            // コンテキストメニューを非表示にする
            // メニューの外でクリックされても、メニューが閉じられるようにするため
            TaskbarIconContextMenu.IsOpen = false;
            if (MouseHook != null)
            {
                MouseHook.UnHook();
                MouseHook = null;
            }
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
            Common.ApplicationData.WindowManagement.ShowMainWindow();
        }
        catch
        {
            FreeEcho.FEControls.MessageBox.Show(Common.ApplicationData.Languages.Check + Common.CopySeparateString + Common.ApplicationName, Common.ApplicationData.Languages.ErrorOccurred, FreeEcho.FEControls.MessageBoxButton.Ok);
        }
    }

    /// <summary>
    /// 「イベント」MenuItemの「Click」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void EventMenuItem_Click(
        object sender,
        EventArgs e
        )
    {
        try
        {
            Common.ApplicationData.Settings.EventInformation.Enabled = !Common.ApplicationData.Settings.EventInformation.Enabled;
            Common.ApplicationData.DoProcessingEvent(ProcessingEventType.EventProcessingStateChanged);
        }
        catch
        {
            FreeEcho.FEControls.MessageBox.Show(Common.ApplicationData.Languages.Check + Common.CopySeparateString + Common.ApplicationName, Common.ApplicationData.Languages.ErrorOccurred, FreeEcho.FEControls.MessageBoxButton.Ok);
        }
    }

    /// <summary>
    /// 「タイマー」MenuItemの「Click」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void TimerMenuItem_Click(
        object sender,
        EventArgs e
        )
    {
        try
        {
            Common.ApplicationData.Settings.TimerInformation.Enabled = !Common.ApplicationData.Settings.TimerInformation.Enabled;
            Common.ApplicationData.DoProcessingEvent(ProcessingEventType.TimerProcessingStateChanged);
        }
        catch
        {
            FreeEcho.FEControls.MessageBox.Show(Common.ApplicationData.Languages.Check + Common.CopySeparateString + Common.ApplicationName, Common.ApplicationData.Languages.ErrorOccurred, FreeEcho.FEControls.MessageBoxButton.Ok);
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
            Common.ApplicationData.Settings.MagnetInformation.Enabled = !Common.ApplicationData.Settings.MagnetInformation.Enabled;
            Common.ApplicationData.DoProcessingEvent(ProcessingEventType.MagnetProcessingStateChanged);
        }
        catch
        {
            FreeEcho.FEControls.MessageBox.Show(Common.ApplicationData.Languages.Check + Common.CopySeparateString + Common.ApplicationName, Common.ApplicationData.Languages.ErrorOccurred, FreeEcho.FEControls.MessageBoxButton.Ok);
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
            Common.ApplicationData.Settings.HotkeyInformation.Enabled = !Common.ApplicationData.Settings.HotkeyInformation.Enabled;
            Common.ApplicationData.DoProcessingEvent(ProcessingEventType.HotkeyValidState);
        }
        catch
        {
            FreeEcho.FEControls.MessageBox.Show(Common.ApplicationData.Languages.Check + Common.CopySeparateString + Common.ApplicationName, Common.ApplicationData.Languages.ErrorOccurred, FreeEcho.FEControls.MessageBoxButton.Ok);
        }
    }

    /// <summary>
    /// 「イベントの一括実行」MenuItemの「Click」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void BatchProcessingOfEventMenuItem_Click(
        object sender,
        EventArgs e
        )
    {
        try
        {
            Common.ApplicationData.DoProcessingEvent(ProcessingEventType.BatchProcessingOfEvent);
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「タイマーの一括実行」MenuItemの「Click」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void BatchProcessingOfTimerMenuItem_Click(
        object sender,
        EventArgs e
        )
    {
        try
        {
            Common.ApplicationData.DoProcessingEvent(ProcessingEventType.BatchProcessingOfTimer);
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
            Common.ApplicationData.DoProcessingEvent(ProcessingEventType.DoOutsideToInside);
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
            Common.ApplicationData.WindowManagement.ShowGetInformationWindow();
        }
        catch
        {
            FreeEcho.FEControls.MessageBox.Show(Common.ApplicationData.Languages.Check + Common.CopySeparateString + Common.ApplicationName, Common.ApplicationData.Languages.ErrorOccurred, FreeEcho.FEControls.MessageBoxButton.Ok);
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
            Common.ApplicationData.WindowManagement.ShowNumericCalculationWindow();
        }
        catch
        {
            FreeEcho.FEControls.MessageBox.Show(Common.ApplicationData.Languages.Check + Common.CopySeparateString + Common.ApplicationName, Common.ApplicationData.Languages.ErrorOccurred, FreeEcho.FEControls.MessageBoxButton.Ok);
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
            Common.MonitorInformation = MonitorInformation.GetMonitorInformation();
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
            Common.ApplicationData.Dispose();
            NotifyIcon.Delete();
            Close();
            DoClose();
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「システムトレイアイコン」TaskbarIconの「TrayMouseDoubleClick」イベント
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
            Common.ApplicationData.WindowManagement.ShowMainWindow();
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
                case ProcessingEventType.EventProcessingStateChanged:
                    NotifyIcon.ModifyIcon((Common.ApplicationData.Settings.EventInformation.Enabled || Common.ApplicationData.Settings.TimerInformation.Enabled) ? Properties.Resources.ApplicationIcon : Properties.Resources.ApplicationIconInvalid, Common.MaxRetryNotifyIcon);
                    EventMenuItem.IsChecked = Common.ApplicationData.Settings.EventInformation.Enabled;
                    break;
                case ProcessingEventType.TimerProcessingStateChanged:
                    NotifyIcon.ModifyIcon((Common.ApplicationData.Settings.EventInformation.Enabled || Common.ApplicationData.Settings.TimerInformation.Enabled) ? Properties.Resources.ApplicationIcon : Properties.Resources.ApplicationIconInvalid, Common.MaxRetryNotifyIcon);
                    TimerMenuItem.IsChecked = Common.ApplicationData.Settings.TimerInformation.Enabled;
                    break;
                case ProcessingEventType.MagnetProcessingStateChanged:
                    MagnetMenuItem.IsChecked = Common.ApplicationData.Settings.MagnetInformation.Enabled;
                    break;
                case ProcessingEventType.HotkeyValidState:
                    HotkeyMenuItem.IsChecked = Common.ApplicationData.Settings.HotkeyInformation.Enabled;
                    break;
                case ProcessingEventType.LanguageChanged:
                    SetTextOnControls();
                    break;
            }
            SettingFileProcessing.WriteSettings();
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
                throw new Exception("LanguagesWindow value is null. - NotifyIconWindow.SetTextOnControls()");
            }

            FreeEcho.FEControls.MessageBoxWindow.Ok = Common.ApplicationData.Languages.LanguagesWindow.Ok;
            FreeEcho.FEControls.MessageBoxWindow.Yes = Common.ApplicationData.Languages.LanguagesWindow.Yes;
            FreeEcho.FEControls.MessageBoxWindow.No = Common.ApplicationData.Languages.LanguagesWindow.No;
            FreeEcho.FEControls.MessageBoxWindow.Cancel = Common.ApplicationData.Languages.LanguagesWindow.Cancel;

            OpenMenuItem.Header = Common.ApplicationData.Languages.LanguagesWindow.Open;
            EventMenuItem.Header = Common.ApplicationData.Languages.LanguagesWindow.Event;
            TimerMenuItem.Header = Common.ApplicationData.Languages.LanguagesWindow.Timer;
            MagnetMenuItem.Header = Common.ApplicationData.Languages.LanguagesWindow.Magnet;
            HotkeyMenuItem.Header = Common.ApplicationData.Languages.LanguagesWindow.Hotkey;
            BatchProcessingOfEventMenuItem.Header = Common.ApplicationData.Languages.LanguagesWindow.BatchProcessingOfEvent;
            BatchProcessingOfTimerMenuItem.Header = Common.ApplicationData.Languages.LanguagesWindow.BatchProcessingOfTimer;
            DoOutsideToInsideMenuItem.Header = Common.ApplicationData.Languages.LanguagesWindow.WindowMoveOutsideToInside;
            GetInformationWindowMenuItem.Header = Common.ApplicationData.Languages.LanguagesWindow.GetWindowInformation;
            NumericCalculationWindowMenuItem.Header = Common.ApplicationData.Languages.LanguagesWindow.NumericCalculation;
            DisplayInformationUpdateMenuItem.Header = Common.ApplicationData.Languages.LanguagesWindow.DisplayInformationUpdate;
            EndMenuItem.Header = Common.ApplicationData.Languages.LanguagesWindow.End;
        }
        catch
        {
        }
    }
}
