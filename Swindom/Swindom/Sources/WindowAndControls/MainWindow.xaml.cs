namespace Swindom;

/// <summary>
/// メインウィンドウ
/// </summary>
public partial class MainWindow : Window
{
    /// <summary>
    /// コンストラクタ (使用しない)
    /// </summary>
    public MainWindow()
    {
        throw new Exception("Do not use. - AddModifyWindowHotkey()");
    }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="firstRun">初めての実行かの値 (いいえ「false」/はい「true」)</param>
    public MainWindow(
        bool firstRun = false
        )
    {
        InitializeComponent();

        if (firstRun)
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }
        else
        {
            Left = Common.ApplicationData.Settings.MainWindowRectangle.X;
            Top = Common.ApplicationData.Settings.MainWindowRectangle.Y;
        }
        Width = Common.ApplicationData.Settings.MainWindowRectangle.Width;
        Height = Common.ApplicationData.Settings.MainWindowRectangle.Height;
        WindowState = Common.ApplicationData.Settings.WindowStateMainWindow;
        SetTextOnControls();

        Loaded += MainWindow_Loaded;
        Closed += MainWindow_Closed;
        Common.ApplicationData.ProcessingEvent += ApplicationData_ProcessingEvent;
    }

    /// <summary>
    /// ウィンドウの「Loaded」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void MainWindow_Loaded(
        object sender,
        RoutedEventArgs e
        )
    {
        try
        {
            Activate();
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
    private void MainWindow_Closed(
        object? sender,
        EventArgs e
        )
    {
        try
        {
            Common.ApplicationData.ProcessingEvent -= ApplicationData_ProcessingEvent;

            Common.ApplicationData.Settings.MainWindowRectangle.X = (int)Left;
            Common.ApplicationData.Settings.MainWindowRectangle.Y = (int)Top;
            Common.ApplicationData.Settings.MainWindowRectangle.Width = (int)Width;
            Common.ApplicationData.Settings.MainWindowRectangle.Height = (int)Height;
            Common.ApplicationData.Settings.WindowStateMainWindow = WindowState;
            SettingFileProcessing.WriteSettings();
        }
        catch
        {
        }

        try
        {
            EventWindowProcessingControl.Dispose();
            TimerWindowProcessingControl.Dispose();
            MagnetControl.Dispose();
            HotkeyControl.Dispose();
            SettingControl.Dispose();
            InformationControl.Dispose();
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
                throw new Exception("LanguagesWindow value is null. - MainWindow.SetTextOnControls()");
            }

            EventWindowProcessingImageTabItem.Text = Common.ApplicationData.Languages.LanguagesWindow.Event;
            TimerWindowProcessingImageTabItem.Text = Common.ApplicationData.Languages.LanguagesWindow.Timer;
            MagnetProcessingImageTabItem.Text = Common.ApplicationData.Languages.LanguagesWindow.Magnet;
            HotkeyImageTabItem.Text = Common.ApplicationData.Languages.LanguagesWindow.Hotkey;
            SettingImageTabItem.Text = Common.ApplicationData.Languages.LanguagesWindow.Setting;
            InformationImageTabItem.Text = Common.ApplicationData.Languages.LanguagesWindow.Information;
        }
        catch
        {
        }
    }
}
