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
        throw new Exception("Do not use. - MainWindow()");
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

            SettingMainNavigationViewOpenPaleLength();
        }
        catch
        {
        }
    }

    /// <summary>
    /// メインのNavigationViewの「OpenPaleLength」を設定
    /// </summary>
    private void SettingMainNavigationViewOpenPaleLength()
    {
        double width = 0;

        if (width < SpecifyWindowLabel.ActualWidth)
        {
            width = SpecifyWindowLabel.ActualWidth;
        }
        if (width < AllWindowLabel.ActualWidth)
        {
            width = AllWindowLabel.ActualWidth;
        }
        if (width < MagnetLabel.ActualWidth)
        {
            width = MagnetLabel.ActualWidth;
        }
        if (width < HotkeyLabel.ActualWidth)
        {
            width = HotkeyLabel.ActualWidth;
        }
        if (width < PluginLabel.ActualWidth)
        {
            width = PluginLabel.ActualWidth;
        }
        if (width < SettingsLabel.ActualWidth)
        {
            width = SettingsLabel.ActualWidth;
        }
        if (width < InformationLabel.ActualWidth)
        {
            width = InformationLabel.ActualWidth;
        }

        MainNavigationView.OpenPaneLength = SpecifyWindowImage.Width + width + 50;
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
            if (ApplicationData.Languages.LanguagesWindow == null)
            {
                throw new Exception("Languages value is null. - MainWindow.SetTextOnControls()");
            }

            SpecifyWindowLabel.Content = ApplicationData.Languages.LanguagesWindow.SpecifyWindow;
            AllWindowLabel.Content = ApplicationData.Languages.LanguagesWindow.AllWindow;
            MagnetLabel.Content = ApplicationData.Languages.LanguagesWindow.Magnet;
            HotkeyLabel.Content = ApplicationData.Languages.LanguagesWindow.Hotkey;
            PluginLabel.Content = ApplicationData.Languages.LanguagesWindow.Plugin;
            SettingsLabel.Content = ApplicationData.Languages.LanguagesWindow.Setting;
            InformationLabel.Content = ApplicationData.Languages.LanguagesWindow.Information;
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
        if (ApplicationData.Settings.DarkMode)
        {
            SpecifyWindowImage.Source = new BitmapImage(new("/Resources/SpecifyWindowProcessingWhite.png", UriKind.Relative));
            AllWindowImage.Source = new BitmapImage(new("/Resources/AllWindowProcessingWhite.png", UriKind.Relative));
            MagnetImage.Source = new BitmapImage(new("/Resources/MagnetProcessingWhite.png", UriKind.Relative));
            HotkeyImage.Source = new BitmapImage(new("/Resources/HotkeyProcessingWhite.png", UriKind.Relative));
            PluginImage.Source = new BitmapImage(new("/Resources/PluginProcessingWhite.png", UriKind.Relative));
            SettingsImage.Source = new BitmapImage(new("/Resources/SettingsWhite.png", UriKind.Relative));
            InformationImage.Source = new BitmapImage(new("/Resources/InformationWhite.png", UriKind.Relative));
        }
        else
        {
            SpecifyWindowImage.Source = new BitmapImage(new("/Resources/SpecifyWindowProcessingDark.png", UriKind.Relative));
            AllWindowImage.Source = new BitmapImage(new("/Resources/AllWindowProcessingDark.png", UriKind.Relative));
            MagnetImage.Source = new BitmapImage(new("/Resources/MagnetProcessingDark.png", UriKind.Relative));
            HotkeyImage.Source = new BitmapImage(new("/Resources/HotkeyProcessingDark.png", UriKind.Relative));
            PluginImage.Source = new BitmapImage(new("/Resources/PluginProcessingDark.png", UriKind.Relative));
            SettingsImage.Source = new BitmapImage(new("/Resources/SettingsDark.png", UriKind.Relative));
            InformationImage.Source = new BitmapImage(new("/Resources/InformationDark.png", UriKind.Relative));
        }
    }
}
