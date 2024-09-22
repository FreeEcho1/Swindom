namespace Swindom;

/// <summary>
/// メインの処理
/// </summary>
public class MainProcessing
{
    /// <summary>
    /// 「閉じる」イベント
    /// </summary>
    public event EventHandler<EventArgs> CloseEvent = delegate { };
    /// <summary>
    /// 「閉じる」イベントを実行
    /// </summary>
    public virtual void DoClose() => CloseEvent?.Invoke(this, new());

    /// <summary>
    /// システムトレイアイコン
    /// </summary>
    private readonly MyNotifyIcon MyNotifyIcon = new();
    /// <summary>
    /// 「システムトレイアイコン」のContextMenu
    /// </summary>
    private readonly ContextMenu NotifyIconContextMenu = new();
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
    /// 「全てのウィンドウ」MenuItem
    /// </summary>
    private readonly MenuItem AllWindowMenuItem = new()
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
    /// 「説明」MenuItem
    /// </summary>
    private readonly MenuItem ExplanationMenuItem = new();
    /// <summary>
    /// 「終了」MenuItem
    /// </summary>
    private readonly MenuItem EndMenuItem = new();

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public MainProcessing()
    {
        try
        {
            bool firstRun = ApplicationInitialize.Initialize();        // 初めての実行かの値

            ApplicationData.MainHwnd = MyNotifyIcon.Handle;

            SetTextOnControls();

            SpecifyWindowMenuItem.IsChecked = ApplicationData.Settings.SpecifyWindowInformation.IsEnabled;
            AllWindowMenuItem.IsChecked = ApplicationData.Settings.AllWindowInformation.IsEnabled;
            MagnetMenuItem.IsChecked = ApplicationData.Settings.MagnetInformation.IsEnabled;
            HotkeyMenuItem.IsChecked = ApplicationData.Settings.HotkeyInformation.IsEnabled;
            OpenMenuItem.Click += OpenMenuItem_Click;
            SpecifyWindowMenuItem.Click += SpecifyWindowMenuItem_Click;
            AllWindowMenuItem.Click += AllWindowMenuItem_Click;
            MagnetMenuItem.Click += MagnetMenuItem_Click;
            HotkeyMenuItem.Click += HotkeyMenuItem_Click;
            BatchProcessingOfSpecifyWindowMenuItem.Click += BatchProcessingOfSpecifyWindowMenuItem_Click;
            DoOutsideToInsideMenuItem.Click += DoOutsideToInsideMenuItem_Click;
            GetInformationWindowMenuItem.Click += GetInformationWindowMenuItem_Click;
            NumericCalculationWindowMenuItem.Click += NumericCalculationWindowMenuItem_Click;
            DisplayInformationUpdateMenuItem.Click += DisplayInformationUpdateMenuItem_Click;
            ExplanationMenuItem.Click += ExplanationMenuItem_Click;
            EndMenuItem.Click += EndMenuItem_Click;
            NotifyIconContextMenu.Items.Add(OpenMenuItem);
            NotifyIconContextMenu.Items.Add(SpecifyWindowMenuItem);
            NotifyIconContextMenu.Items.Add(AllWindowMenuItem);
            NotifyIconContextMenu.Items.Add(MagnetMenuItem);
            NotifyIconContextMenu.Items.Add(HotkeyMenuItem);
            NotifyIconContextMenu.Items.Add(BatchProcessingOfSpecifyWindowMenuItem);
            NotifyIconContextMenu.Items.Add(DoOutsideToInsideMenuItem);
            NotifyIconContextMenu.Items.Add(GetInformationWindowMenuItem);
            NotifyIconContextMenu.Items.Add(NumericCalculationWindowMenuItem);
            NotifyIconContextMenu.Items.Add(DisplayInformationUpdateMenuItem);
            NotifyIconContextMenu.Items.Add(ExplanationMenuItem);
            NotifyIconContextMenu.Items.Add(EndMenuItem);

            MyNotifyIcon.NotifyIconContextMenu = NotifyIconContextMenu;
            MyNotifyIcon.Add(Properties.Resources.ApplicationIconInvalid, ApplicationValue.ApplicationName);
            MyNotifyIcon.NotifyIconLeftButtonClick += MyNotifyIcon_NotifyIconLeftButtonClick;
            MyNotifyIcon.NotifyIconLeftButtonDoubleClick += MyNotifyIcon_NotifyIconLeftButtonDoubleClick;
            SetIconOnNotifyIcon();

            ApplicationData.EventData.ProcessingEvent += ApplicationData_ProcessingEvent;
            ApplicationData.WindowProcessingManagement.Initialize();

            MyNotifyIcon.WndProcMessage += MyNotifyIcon_WndProcMessage;

            if (firstRun)
            {
                ApplicationData.WindowManagement.ShowMainWindow(true);
            }
        }
        catch
        {
            FEMessageBox.Show(ApplicationData.Languages.ErrorOccurred, ApplicationData.Languages.Check + WindowControlValue.CopySeparateString + ApplicationValue.ApplicationName, MessageBoxButton.OK);
            DoClose();
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
            FEMessageBox.Show(ApplicationData.Languages.ErrorOccurred, ApplicationData.Languages.Check + WindowControlValue.CopySeparateString + ApplicationValue.ApplicationName, MessageBoxButton.OK);
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
            ApplicationData.Settings.SpecifyWindowInformation.IsEnabled = !ApplicationData.Settings.SpecifyWindowInformation.IsEnabled;
            ApplicationData.EventData.DoProcessingEvent(ProcessingEventType.SpecifyWindowProcessingStateChanged);
            SpecifyWindowMenuItem.IsChecked = ApplicationData.Settings.SpecifyWindowInformation.IsEnabled;
            SettingFileProcessing.WriteSettings();
        }
        catch
        {
            FEMessageBox.Show(ApplicationData.Languages.ErrorOccurred, ApplicationData.Languages.Check + WindowControlValue.CopySeparateString + ApplicationValue.ApplicationName, MessageBoxButton.OK);
        }
    }

    /// <summary>
    /// 「全てのウィンドウ」MenuItemの「Click」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void AllWindowMenuItem_Click(
        object sender,
        RoutedEventArgs e
        )
    {
        try
        {
            ApplicationData.Settings.AllWindowInformation.IsEnabled = !ApplicationData.Settings.AllWindowInformation.IsEnabled;
            ApplicationData.EventData.DoProcessingEvent(ProcessingEventType.AllWindowProcessingStateChanged);
            AllWindowMenuItem.IsChecked = ApplicationData.Settings.AllWindowInformation.IsEnabled;
            SettingFileProcessing.WriteSettings();
        }
        catch
        {
            FEMessageBox.Show(ApplicationData.Languages.ErrorOccurred, ApplicationData.Languages.Check + WindowControlValue.CopySeparateString + ApplicationValue.ApplicationName, MessageBoxButton.OK);
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
            ApplicationData.Settings.MagnetInformation.IsEnabled = !ApplicationData.Settings.MagnetInformation.IsEnabled;
            ApplicationData.EventData.DoProcessingEvent(ProcessingEventType.MagnetProcessingStateChanged);
            MagnetMenuItem.IsChecked = ApplicationData.Settings.MagnetInformation.IsEnabled;
            SettingFileProcessing.WriteSettings();
        }
        catch
        {
            FEMessageBox.Show(ApplicationData.Languages.ErrorOccurred, ApplicationData.Languages.Check + WindowControlValue.CopySeparateString + ApplicationValue.ApplicationName, MessageBoxButton.OK);
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
            ApplicationData.Settings.HotkeyInformation.IsEnabled = !ApplicationData.Settings.HotkeyInformation.IsEnabled;
            ApplicationData.EventData.DoProcessingEvent(ProcessingEventType.HotkeyProcessingStateChanged);
            HotkeyMenuItem.IsChecked = ApplicationData.Settings.HotkeyInformation.IsEnabled;
            SettingFileProcessing.WriteSettings();
        }
        catch
        {
            FEMessageBox.Show(ApplicationData.Languages.ErrorOccurred, ApplicationData.Languages.Check + WindowControlValue.CopySeparateString + ApplicationValue.ApplicationName, MessageBoxButton.OK);
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
            ApplicationData.EventData.DoProcessingEvent(ProcessingEventType.SpecifyWindowBatchProcessing);
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
            WindowProcessing.MoveWindowIntoScreen();
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「ウィンドウ情報取得」MenuItemの「Click」イベント
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
            FEMessageBox.Show(ApplicationData.Languages.ErrorOccurred, ApplicationData.Languages.Check + WindowControlValue.CopySeparateString + ApplicationValue.ApplicationName, MessageBoxButton.OK);
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
            FEMessageBox.Show(ApplicationData.Languages.ErrorOccurred, ApplicationData.Languages.Check + WindowControlValue.CopySeparateString + ApplicationValue.ApplicationName, MessageBoxButton.OK);
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
    /// 「説明」MenuItemの「Click」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ExplanationMenuItem_Click(
        object sender,
        RoutedEventArgs e
        )
    {
        try
        {
            ApplicationData.WindowManagement.ShowExplanationWindow();
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
            MyNotifyIcon.Dispose();
            DoClose();
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「システムトレイアイコン」の「NotifyIconLeftButtonClick」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <exception cref="NotImplementedException"></exception>
    private void MyNotifyIcon_NotifyIconLeftButtonClick(
        object? sender,
        EventArgs e
        )
    {
        try
        {
            ApplicationData.EventData.DoProcessingEvent(ProcessingEventType.AllProcessingChangeEnabled);
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「システムトレイアイコン」の「NotifyIconLeftButtonDoubleClick」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void MyNotifyIcon_NotifyIconLeftButtonDoubleClick(
        object? sender,
        EventArgs e
        )
    {
        try
        {
            ApplicationData.WindowManagement.ShowMainWindow();
        }
        catch
        {
            FEMessageBox.Show(ApplicationData.Languages.ErrorOccurred, ApplicationData.Languages.Check + WindowControlValue.CopySeparateString + ApplicationValue.ApplicationName, MessageBoxButton.OK);
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
                    SpecifyWindowMenuItem.IsChecked = ApplicationData.Settings.SpecifyWindowInformation.IsEnabled;
                    SetIconOnNotifyIcon();
                    break;
                case ProcessingEventType.AllWindowProcessingStateChanged:
                    AllWindowMenuItem.IsChecked = ApplicationData.Settings.AllWindowInformation.IsEnabled;
                    SetIconOnNotifyIcon();
                    break;
                case ProcessingEventType.MagnetProcessingStateChanged:
                    MagnetMenuItem.IsChecked = ApplicationData.Settings.MagnetInformation.IsEnabled;
                    SetIconOnNotifyIcon();
                    break;
                case ProcessingEventType.HotkeyProcessingStateChanged:
                    HotkeyMenuItem.IsChecked = ApplicationData.Settings.HotkeyInformation.IsEnabled;
                    SetIconOnNotifyIcon();
                    break;
                case ProcessingEventType.PluginProcessingStateChanged:
                    SetIconOnNotifyIcon();
                    break;
                case ProcessingEventType.LanguageChanged:
                    SetTextOnControls();
                    break;
                case ProcessingEventType.ThemeChanged:
                    NotifyIconContextMenu.UpdateDefaultStyle();
                    break;
                case ProcessingEventType.RestartProcessing:
                    RestartProcessing();
                    break;
                case ProcessingEventType.ShowNotifyIconContextMenu:
                    MyNotifyIcon.ShowContextMenu();
                    break;
            }
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「WndProcMessage」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void MyNotifyIcon_WndProcMessage(
        object? sender,
        WndProcEventArgs e
        )
    {
        try
        {
            // ディスプレイの解像度が変更された場合はディスプレイの情報を更新
            if (e.Message.Msg == (int)WM.WM_DISPLAYCHANGE)
            {
                ApplicationData.MonitorInformation = MonitorInformation.GetMonitorInformation();
            }
        }
        catch
        {
        }
    }

    /// <summary>
    /// 再起動処理
    /// </summary>
    private static void RestartProcessing()
    {
        string batFilePath = ApplicationPath.GetApplicationDirectory() + Path.DirectorySeparatorChar + PluginValue.RestartBatchFileName;       // バッチファイルのパス

        string batString = "";
        batString += "taskkill /f /im \"" + ApplicationPath.GetApplicationFileName() + "\"" + Environment.NewLine;       // プロセスを停止
        batString += "timeout /t 2" + Environment.NewLine;      // 待機時間
        batString += "pushd \"" + ApplicationPath.GetApplicationDirectory() + "\"" + Environment.NewLine;        // ディレクトリを移動
        batString += "start \"\" \"" + ApplicationPath.GetApplicationFileName() + "\"" + Environment.NewLine;        // アプリケーションを実行
        batString += "del /f \"" + PluginValue.RestartBatchFileName + "\"" + Environment.NewLine;      // バッチファイルを削除
        batString += "exit /b" + Environment.NewLine;
        File.WriteAllText(batFilePath, batString);

        Process process = new();
        process.StartInfo.FileName = batFilePath;
        process.StartInfo.Verb = "open";
        process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
        process.StartInfo.CreateNoWindow = true;
        process.StartInfo.UseShellExecute = false;
        process.Start();
    }

    /// <summary>
    /// システムトレイにアイコンを設定
    /// </summary>
    private void SetIconOnNotifyIcon()
    {
        MyNotifyIcon.Icon = ApplicationData.Settings.SpecifyWindowInformation.IsEnabled
            || ApplicationData.Settings.AllWindowInformation.IsEnabled
            || ApplicationData.Settings.MagnetInformation.IsEnabled
            || ApplicationData.Settings.HotkeyInformation.IsEnabled
            || ApplicationData.Settings.PluginInformation.IsEnabled
            ? Properties.Resources.ApplicationIcon : Properties.Resources.ApplicationIconInvalid;
    }

    /// <summary>
    /// コントロールにテキストを設定
    /// </summary>
    private void SetTextOnControls()
    {
        try
        {
            FEMessageBoxWindow.Ok = ApplicationData.Languages.Ok;
            FEMessageBoxWindow.Yes = ApplicationData.Languages.Yes;
            FEMessageBoxWindow.No = ApplicationData.Languages.No;
            FEMessageBoxWindow.Cancel = ApplicationData.Languages.Cancel;

            OpenMenuItem.Header = ApplicationData.Languages.Open;
            SpecifyWindowMenuItem.Header = ApplicationData.Languages.SpecifyWindow;
            AllWindowMenuItem.Header = ApplicationData.Languages.AllWindow;
            MagnetMenuItem.Header = ApplicationData.Languages.Magnet;
            HotkeyMenuItem.Header = ApplicationData.Languages.Hotkey;
            BatchProcessingOfSpecifyWindowMenuItem.Header = ApplicationData.Languages.BatchProcessingOfSpecifyWindow;
            DoOutsideToInsideMenuItem.Header = ApplicationData.Languages.WindowMoveOutsideToInside;
            GetInformationWindowMenuItem.Header = ApplicationData.Languages.GetWindowInformation;
            NumericCalculationWindowMenuItem.Header = ApplicationData.Languages.NumericCalculation;
            DisplayInformationUpdateMenuItem.Header = ApplicationData.Languages.DisplayInformationUpdate;
            ExplanationMenuItem.Header = ApplicationData.Languages.Help;
            EndMenuItem.Header = ApplicationData.Languages.End;
        }
        catch
        {
        }
    }
}
