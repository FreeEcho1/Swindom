using System.Threading;
using System.Threading.Tasks;

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
    private MyNotifyIcon MyNotifyIcon { get; } = new();
    /// <summary>
    /// 「システムトレイアイコン」のContextMenu
    /// </summary>
    private ContextMenu NotifyIconContextMenu { get; } = new();
    /// <summary>
    /// 「開く」MenuItem
    /// </summary>
    private MenuItem OpenMenuItem { get; } = new();
    /// <summary>
    /// 「指定ウィンドウ」MenuItem
    /// </summary>
    private MenuItem SpecifyWindowMenuItem { get; } = new()
    {
        IsCheckable = true,
    };
    /// <summary>
    /// 「全てのウィンドウ」MenuItem
    /// </summary>
    private MenuItem AllWindowMenuItem { get; } = new()
    {
        IsCheckable = true,
    };
    /// <summary>
    /// 「マグネット」MenuItem
    /// </summary>
    private MenuItem MagnetMenuItem { get; } = new()
    {
        IsCheckable = true,
    };
    /// <summary>
    /// 「ホットキー」MenuItem
    /// </summary>
    private MenuItem HotkeyMenuItem { get; } = new()
    {
        IsCheckable = true,
    };
    /// <summary>
    /// 「指定ウィンドウの一括実行」MenuItem
    /// </summary>
    private MenuItem BatchProcessingOfSpecifyWindowMenuItem { get; } = new();
    /// <summary>
    /// 「ウィンドウが画面外の場合は画面内に移動」MenuItem
    /// </summary>
    private MenuItem DoOutsideToInsideMenuItem { get; } = new();
    /// <summary>
    /// 「ウィンドウ情報取得」MenuItem
    /// </summary>
    private MenuItem GetInformationWindowMenuItem { get; } = new();
    /// <summary>
    /// 「数値計算」MenuItem
    /// </summary>
    private MenuItem NumericCalculationWindowMenuItem { get; } = new();
    /// <summary>
    /// 「ディスプレイ情報更新」MenuItem
    /// </summary>
    private MenuItem DisplayInformationUpdateMenuItem { get; } = new();
    /// <summary>
    /// 「説明」MenuItem
    /// </summary>
    private MenuItem ExplanationMenuItem { get; } = new();
    /// <summary>
    /// 「終了」MenuItem
    /// </summary>
    private MenuItem EndMenuItem { get; } = new();

    /// <summary>
    /// ディスプレイ変更の待機数
    /// </summary>
    private int CountDisplayChange = 0;
    /// <summary>
    /// ディスプレイ変更の待ち間隔 (ミリ秒)
    /// </summary>
    private static int DisplayChangeWaitInterval { get; } = 1000;
    /// <summary>
    /// ディスプレイ変更の最大待ち回数
    /// </summary>
    private static int MaxWaitCountForDisplayChange { get; } = 1200000;

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

            DisplayInformationUpdateAsync();
        }
        catch
        {
            FEMessageBox.Show(ApplicationData.Strings.ErrorOccurred, ApplicationData.Strings.Check + WindowControlValue.CopySeparateString + ApplicationValue.ApplicationName, MessageBoxButton.OK);
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
            FEMessageBox.Show(ApplicationData.Strings.ErrorOccurred, ApplicationData.Strings.Check + WindowControlValue.CopySeparateString + ApplicationValue.ApplicationName, MessageBoxButton.OK);
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
            FEMessageBox.Show(ApplicationData.Strings.ErrorOccurred, ApplicationData.Strings.Check + WindowControlValue.CopySeparateString + ApplicationValue.ApplicationName, MessageBoxButton.OK);
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
            FEMessageBox.Show(ApplicationData.Strings.ErrorOccurred, ApplicationData.Strings.Check + WindowControlValue.CopySeparateString + ApplicationValue.ApplicationName, MessageBoxButton.OK);
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
            FEMessageBox.Show(ApplicationData.Strings.ErrorOccurred, ApplicationData.Strings.Check + WindowControlValue.CopySeparateString + ApplicationValue.ApplicationName, MessageBoxButton.OK);
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
            FEMessageBox.Show(ApplicationData.Strings.ErrorOccurred, ApplicationData.Strings.Check + WindowControlValue.CopySeparateString + ApplicationValue.ApplicationName, MessageBoxButton.OK);
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
            FEMessageBox.Show(ApplicationData.Strings.ErrorOccurred, ApplicationData.Strings.Check + WindowControlValue.CopySeparateString + ApplicationValue.ApplicationName, MessageBoxButton.OK);
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
            FEMessageBox.Show(ApplicationData.Strings.ErrorOccurred, ApplicationData.Strings.Check + WindowControlValue.CopySeparateString + ApplicationValue.ApplicationName, MessageBoxButton.OK);
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
            ApplicationData.EventData.DoProcessingEvent(ProcessingEventType.DisplayInformationUpdate);
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
            FEMessageBox.Show(ApplicationData.Strings.ErrorOccurred, ApplicationData.Strings.Check + WindowControlValue.CopySeparateString + ApplicationValue.ApplicationName, MessageBoxButton.OK);
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
                case ProcessingEventType.ShowNotifyIconContextMenu:
                    MyNotifyIcon.ShowContextMenu();
                    break;
                case ProcessingEventType.DisplayInformationUpdate:
                    DisplayInformationUpdateAsync();
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
                DisplayInformationUpdateAsync();
            }
        }
        catch
        {
        }
    }

    /// <summary>
    /// ディスプレイ情報更新
    /// </summary>
    private async void DisplayInformationUpdateAsync()
    {
        await DisplayInformationUpdate();
    }

    /// <summary>
    /// ディスプレイ情報更新
    /// </summary>
    /// <returns>Task</returns>
    //private async Task DisplayInformationUpdate()
    //{
    //    try
    //    {
    //        // 必要以上の更新はしない
    //        if (CountDisplayChange > 1)
    //        {
    //            return;
    //        }

    //        CountDisplayChange++;

    //        // 待機する
    //        if (CountDisplayChange > 2)
    //        {
    //            int waitCount = 0;      // 待機回数

    //            await Task.Run(() =>
    //            {
    //                while (CountDisplayChange > 1)
    //                {
    //                    Thread.Sleep(DisplayChangeWaitInterval);
    //                    waitCount++;
    //                    if (waitCount > MaxWaitCountForDisplayChange)
    //                    {
    //                        return;
    //                    }
    //                }
    //            });
    //        }

    //        // ディスプレイの状態が変更されたかを確認
    //        // Windowsの仕様で変更されていないのに2回「WM_DISPLAYCHANGE」イベントが発生するなどの対策
    //        bool displayChange = false;     // ディスプレイの状態が変更されたかの値
    //        MonitorInformation monitorInformation = MonitorInformation.GetMonitorInformation();     // モニター情報
    //        if (monitorInformation.MonitorInfo.Count == ApplicationData.MonitorInformation.MonitorInfo.Count)
    //        {
    //            foreach (MonitorInfoEx nowNewMonitorInfo in monitorInformation.MonitorInfo)
    //            {
    //                bool match = false;     // 一致したかの値

    //                foreach (MonitorInfoEx nowOldMonitorInfo in ApplicationData.MonitorInformation.MonitorInfo)
    //                {
    //                    if (nowNewMonitorInfo.DeviceName == nowOldMonitorInfo.DeviceName
    //                        && nowNewMonitorInfo.WorkArea.Left == nowOldMonitorInfo.WorkArea.Left
    //                        && nowNewMonitorInfo.WorkArea.Top == nowOldMonitorInfo.WorkArea.Top
    //                        && nowNewMonitorInfo.WorkArea.Right == nowOldMonitorInfo.WorkArea.Right
    //                        && nowNewMonitorInfo.WorkArea.Bottom == nowOldMonitorInfo.WorkArea.Bottom
    //                        )
    //                    {
    //                        match = true;
    //                        break;
    //                    }
    //                }
    //                if (match == false)
    //                {
    //                    displayChange = true;
    //                    break;
    //                }
    //            }
    //        }
    //        else
    //        {
    //            displayChange = true;
    //        }

    //        // ディスプレイの状態が変更されている場合は処理
    //        if (displayChange)
    //        {
    //            ApplicationData.MonitorInformation = monitorInformation;
    //            WindowProcessing.ChangeDisplaySettings();
    //        }

    //        CountDisplayChange--;
    //        if (CountDisplayChange < 0)
    //        {
    //            CountDisplayChange = 0;
    //        }
    //    }
    //    catch
    //    {
    //        CountDisplayChange--;
    //    }
    //}

    /// <summary>
    /// ディスプレイ情報更新
    /// </summary>
    /// <returns>Task</returns>
    private async Task DisplayInformationUpdate()
    {
        try
        {
            MonitorInformation monitorInformation;     // モニター情報

            if (CountDisplayChange > 0)
            {
                monitorInformation = MonitorInformation.GetMonitorInformation();
                // ディスプレイの数が変わっていないなら以降の処理はしない (更新しない)
                if (monitorInformation.MonitorInfo.Count == ApplicationData.MonitorInformation.MonitorInfo.Count)
                {
                    return;
                }
                else
                {
                    if (WindowProcessing.CheckSettingDisplaysExist(monitorInformation))
                    {
                        FEMessageBox.Show(ApplicationData.Strings.MessageDisplayChangeAllValid, ApplicationData.Strings.Check + WindowControlValue.CopySeparateString + ApplicationValue.ApplicationName, MessageBoxButton.OK);
                    }
                    else
                    {
                        FEMessageBox.Show(ApplicationData.Strings.MessageDisplayChangeRequiresReset, ApplicationData.Strings.Check + WindowControlValue.CopySeparateString + ApplicationValue.ApplicationName, MessageBoxButton.OK);
                    }

                    // 必要以上の更新はしない (待機中の処理に任せる)
                    if (CountDisplayChange > 1)
                    {
                        return;
                    }
                }
            }

            CountDisplayChange++;

            try
            {
                // 待機する
                if (CountDisplayChange > 1)
                {
                    int waitCount = 0;      // 待機回数

                    await Task.Run(() =>
                    {
                        while (CountDisplayChange > 1)
                        {
                            Thread.Sleep(DisplayChangeWaitInterval);
                            waitCount++;
                            if (waitCount > MaxWaitCountForDisplayChange)
                            {
                                return;
                            }
                        }
                    });
                }

                monitorInformation = MonitorInformation.GetMonitorInformation();

                ApplicationData.MonitorInformation = monitorInformation;
                WindowProcessing.ChangeDisplaySettings();
            }
            catch
            {
            }

            CountDisplayChange--;
            if (CountDisplayChange < 0)
            {
                CountDisplayChange = 0;
            }
        }
        catch
        {
        }
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
            FEMessageBoxWindow.Ok = ApplicationData.Strings.Ok;
            FEMessageBoxWindow.Yes = ApplicationData.Strings.Yes;
            FEMessageBoxWindow.No = ApplicationData.Strings.No;
            FEMessageBoxWindow.Cancel = ApplicationData.Strings.Cancel;

            OpenMenuItem.Header = ApplicationData.Strings.Open;
            SpecifyWindowMenuItem.Header = ApplicationData.Strings.SpecifyWindow;
            AllWindowMenuItem.Header = ApplicationData.Strings.AllWindow;
            MagnetMenuItem.Header = ApplicationData.Strings.Magnet;
            HotkeyMenuItem.Header = ApplicationData.Strings.Hotkey;
            BatchProcessingOfSpecifyWindowMenuItem.Header = ApplicationData.Strings.BatchProcessingOfSpecifyWindow;
            DoOutsideToInsideMenuItem.Header = ApplicationData.Strings.WindowMoveOutsideToInside;
            GetInformationWindowMenuItem.Header = ApplicationData.Strings.GetWindowInformation;
            NumericCalculationWindowMenuItem.Header = ApplicationData.Strings.NumericCalculation;
            DisplayInformationUpdateMenuItem.Header = ApplicationData.Strings.DisplayInformationUpdate;
            ExplanationMenuItem.Header = ApplicationData.Strings.Help;
            EndMenuItem.Header = ApplicationData.Strings.End;
        }
        catch
        {
        }
    }
}
