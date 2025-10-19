using static Swindom.NativeMethods;

namespace Swindom;

/// <summary>
/// メインウィンドウ
/// </summary>
public partial class MainWindow : Window
{
    /// <summary>
    /// SpecifyWindowPage
    /// </summary>
    private SpecifyWindowPage? SpecifyWindowPage;
    /// <summary>
    /// AllWindowPage
    /// </summary>
    private AllWindowPage? AllWindowPage;
    /// <summary>
    /// MagnetPage
    /// </summary>
    private MagnetPage? MagnetPage;
    /// <summary>
    /// HotkeyPage
    /// </summary>
    private HotkeyPage? HotkeyPage;
    /// <summary>
    /// PluginPage
    /// </summary>
    private PluginPage? PluginPage;
    /// <summary>
    /// SettingsPage
    /// </summary>
    private SettingsPage? SettingsPage;
    /// <summary>
    /// InformationPage
    /// </summary>
    private InformationPage? InformationPage;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <exception cref="Exception"></exception>
    public MainWindow()
    {
        throw new Exception("Do not use. - " + GetType().Name + "()");
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

        ChangeImage();
        if (firstRun)
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }
        else
        {
            Left = ApplicationData.Settings.MainWindowRectangle.X;
            Top = ApplicationData.Settings.MainWindowRectangle.Y;
        }
        Width = ApplicationData.Settings.MainWindowRectangle.Width;
        Height = ApplicationData.Settings.MainWindowRectangle.Height;
        WindowState = ApplicationData.Settings.WindowStateMainWindow;
        SetTextOnControls();

        MainNavigationView.SelectionChanged += MainNavigationView_SelectionChanged;
        MainNavigationView.SelectedItem = SpecifyWindowNavigationViewItem;

        Loaded += MainWindow_Loaded;
        Closed += MainWindow_Closed;
        ApplicationData.EventData.ProcessingEvent += EventData_ProcessingEvent;
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

            SetNavigationViewOpenPaneLength();
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
            SpecifyWindowPage?.Release();
            MagnetPage?.Release();
            HotkeyPage?.Release();
            PluginPage?.Release();
            SettingsPage?.Release();
            InformationPage?.Release();
            ApplicationData.EventData.ProcessingEvent -= EventData_ProcessingEvent;

            ApplicationData.Settings.MainWindowRectangle.X = (int)Left;
            ApplicationData.Settings.MainWindowRectangle.Y = (int)Top;
            ApplicationData.Settings.MainWindowRectangle.Width = (int)Width;
            ApplicationData.Settings.MainWindowRectangle.Height = (int)Height;
            ApplicationData.Settings.WindowStateMainWindow = WindowState;
            SettingFileProcessing.WriteSettings();
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「ナビゲーション」の「SelectionChanged」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void MainNavigationView_SelectionChanged(
        ModernWpf.Controls.NavigationView sender,
        ModernWpf.Controls.NavigationViewSelectionChangedEventArgs args
        )
    {
        try
        {
            string tag = (string)((ModernWpf.Controls.NavigationViewItem)args.SelectedItem).Tag;

            if (tag == (string)SpecifyWindowNavigationViewItem.Tag)
            {
                SpecifyWindowPage ??= new();
                MainNavigationViewFrame.Navigate(SpecifyWindowPage);
            }
            else if (tag == (string)AllWindowNavigationViewItem.Tag)
            {
                AllWindowPage??= new();
                MainNavigationViewFrame.Navigate(AllWindowPage);
            }
            else if (tag == (string)MagnetNavigationViewItem.Tag)
            {
                MagnetPage ??= new();
                MainNavigationViewFrame.Navigate(MagnetPage);
            }
            else if (tag == (string)HotkeyNavigationViewItem.Tag)
            {
                HotkeyPage ??= new();
                MainNavigationViewFrame.Navigate(HotkeyPage);
            }
            else if (tag == (string)PluginNavigationViewItem.Tag)
            {
                PluginPage ??= new();
                MainNavigationViewFrame.Navigate(PluginPage);
            }
            else if (tag == (string)SettingsNavigationViewItem.Tag)
            {
                SettingsPage ??= new();
                MainNavigationViewFrame.Navigate(SettingsPage);
            }
            else if (tag == (string)InformationNavigationViewItem.Tag)
            {
                InformationPage ??= new();
                MainNavigationViewFrame.Navigate(InformationPage);
            }
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
    private void EventData_ProcessingEvent(
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
                    SetNavigationViewOpenPaneLength();
                    break;
                case ProcessingEventType.ThemeChanged:
                    ChangeImage();
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
            SpecifyWindowNavigationViewItem.Text = ApplicationData.Strings.SpecifyWindow;
            AllWindowNavigationViewItem.Text = ApplicationData.Strings.AllWindow;
            MagnetNavigationViewItem.Text = ApplicationData.Strings.Magnet;
            HotkeyNavigationViewItem.Text = ApplicationData.Strings.Hotkey;
            PluginNavigationViewItem.Text = ApplicationData.Strings.Plugin;
            SettingsNavigationViewItem.Text = ApplicationData.Strings.Setting;
            InformationNavigationViewItem.Text = ApplicationData.Strings.Information;
        }
        catch
        {
        }
    }

    /// <summary>
    /// 画像を変更
    /// </summary>
    private void ChangeImage()
    {
        SpecifyWindowNavigationViewItem.Image = ImageProcessing.GetImageSpecifyWindowProcessing();
        AllWindowNavigationViewItem.Image = ImageProcessing.GetImageAllWindowProcessing();
        MagnetNavigationViewItem.Image = ImageProcessing.GetImageMagnetProcessing();
        HotkeyNavigationViewItem.Image = ImageProcessing.GetImageHotkeyProcessing();
        PluginNavigationViewItem.Image = ImageProcessing.GetImagePluginProcessing();
        SettingsNavigationViewItem.Image = ImageProcessing.GetImageSettings();
        InformationNavigationViewItem.Image = ImageProcessing.GetImageInformation();
    }

    /// <summary>
    /// ナビゲーションビューの「OpenPaneLength」を設定
    /// </summary>
    private void SetNavigationViewOpenPaneLength()
    {
        // 更新しないと変更前の幅が取得されてしまうので更新している
        MainNavigationView.UpdateLayout();

        double width = 0;

        // 項目の最大幅を求める
        if (width < SpecifyWindowNavigationViewItem.ItemWidth())
        {
            width = SpecifyWindowNavigationViewItem.ItemWidth();
        }
        if (width < AllWindowNavigationViewItem.ItemWidth())
        {
            width = AllWindowNavigationViewItem.ItemWidth();
        }
        if (width < MagnetNavigationViewItem.ItemWidth())
        {
            width = MagnetNavigationViewItem.ItemWidth();
        }
        if (width < HotkeyNavigationViewItem.ItemWidth())
        {
            width = HotkeyNavigationViewItem.ItemWidth();
        }
        if (width < PluginNavigationViewItem.ItemWidth())
        {
            width = PluginNavigationViewItem.ItemWidth();
        }
        if (width < SettingsNavigationViewItem.ItemWidth())
        {
            width = SettingsNavigationViewItem.ItemWidth();
        }
        if (width < InformationNavigationViewItem.ItemWidth())
        {
            width = InformationNavigationViewItem.ItemWidth();
        }

        MainNavigationView.OpenPaneLength = width;
    }
}
