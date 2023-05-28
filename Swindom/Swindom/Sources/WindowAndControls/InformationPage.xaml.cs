namespace Swindom;

/// <summary>
/// 「情報」ページ
/// </summary>
public partial class InformationPage : Page
{
    /// <summary>
    /// コンストラクタ
    /// </summary>
    public InformationPage()
    {
        InitializeComponent();

        SetTextOnControls();

        UpdateCheckButton.Click += UpdateCheckButton_Click;
        ManualButton.Click += ManualButton_Click;
        UpdateHistoryButton.Click += UpdateHistoryButton_Click;
        WebsiteButton.Click += WebsiteButton_Click;
        ReportsRequestsButton.Click += ReportsRequestsButton_Click;
        DotNetHyperlink.RequestNavigate += Hyperlink_RequestNavigate;
        ModernWpfHyperlink.RequestNavigate += Hyperlink_RequestNavigate;
        HardcodetNotifyIconForWPFHyperlink.RequestNavigate += Hyperlink_RequestNavigate;
        FreeEchoWindowEventHyperlink.RequestNavigate += Hyperlink_RequestNavigate;
        FreeEchoHotKeyWPFHyperlink.RequestNavigate += Hyperlink_RequestNavigate;
        FreeEchoWindowSelectionMouseHyperlink.RequestNavigate += Hyperlink_RequestNavigate;
        FreeEchoWindowMoveDetectionMouseHyperlink.RequestNavigate += Hyperlink_RequestNavigate;
        FreeEchoCheckForUpdateHyperlink.RequestNavigate += Hyperlink_RequestNavigate;
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
    /// 「更新確認」Buttonの「Click」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void UpdateCheckButton_Click(
        object sender,
        RoutedEventArgs e
        )
    {
        try
        {
            switch (FreeEcho.FECheckForUpdate.CheckForUpdate.CheckForUpdateFileURL(VariousProcessing.OwnApplicationPath(), Common.UpdateCheckURL, ApplicationData.Settings.CheckBetaVersion))
            {
                case FreeEcho.FECheckForUpdate.CheckForUpdateResult.LatestVersion:
                    FEMessageBox.Show(ApplicationData.Languages.LanguagesWindow?.LatestVersion ?? "", ApplicationData.Languages.Check, MessageBoxButton.OK);
                    break;
                case FreeEcho.FECheckForUpdate.CheckForUpdateResult.NotLatestVersion:
                    ApplicationData.WindowManagement.ShowUpdateCheckWindow(Window.GetWindow(this));
                    break;
                default:
                    FEMessageBox.Show(ApplicationData.Languages.LanguagesWindow?.UpdateCheckFailed ?? "", ApplicationData.Languages.Check, MessageBoxButton.OK);
                    break;
            }
        }
        catch
        {
            FEMessageBox.Show(ApplicationData.Languages.ErrorOccurred, ApplicationData.Languages.Check, MessageBoxButton.OK);
        }
    }

    /// <summary>
    /// 「説明書」Buttonの「Click」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ManualButton_Click(
        object sender,
        RoutedEventArgs e
        )
    {
        try
        {
            VariousProcessing.OpenFileAndWebPage(VariousProcessing.GetApplicationDirectory() + Path.DirectorySeparatorChar + Common.ReadmeFileName);
        }
        catch
        {
            FEMessageBox.Show(ApplicationData.Languages.ErrorOccurred, ApplicationData.Languages.Check, MessageBoxButton.OK);
        }
    }

    /// <summary>
    /// 「更新履歴」Buttonの「Click」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void UpdateHistoryButton_Click(
        object sender,
        RoutedEventArgs e
        )
    {
        try
        {
            VariousProcessing.OpenFileAndWebPage(VariousProcessing.GetApplicationDirectory() + Path.DirectorySeparatorChar + Common.UpdateHistoryFileName);
        }
        catch
        {
            FEMessageBox.Show(ApplicationData.Languages.ErrorOccurred, ApplicationData.Languages.Check, MessageBoxButton.OK);
        }
    }

    /// <summary>
    /// 「ウェブサイト」Buttonの「Click」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void WebsiteButton_Click(
        object sender,
        RoutedEventArgs e
        )
    {
        try
        {
            VariousProcessing.OpenFileAndWebPage(Common.WebsiteURL);
        }
        catch
        {
            FEMessageBox.Show(ApplicationData.Languages.ErrorOccurred, ApplicationData.Languages.Check, MessageBoxButton.OK);
        }
    }

    /// <summary>
    /// 「報告、要望」Buttonの「Click」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ReportsRequestsButton_Click(
        object sender,
        RoutedEventArgs e
        )
    {
        try
        {
            VariousProcessing.OpenFileAndWebPage(Common.ReportsAndRequestsWebsiteURL);
        }
        catch
        {
            FEMessageBox.Show(ApplicationData.Languages.ErrorOccurred, ApplicationData.Languages.Check, MessageBoxButton.OK);
        }
    }

    /// <summary>
    /// Hyperlinkの「Click」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void Hyperlink_RequestNavigate(
        object sender,
        RequestNavigateEventArgs e
        )
    {
        try
        {
            VariousProcessing.OpenFileAndWebPage(e.Uri.AbsoluteUri);
            e.Handled= true;
        }
        catch
        {
            FEMessageBox.Show(ApplicationData.Languages.ErrorOccurred, ApplicationData.Languages.Check, MessageBoxButton.OK);
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
            if (ApplicationData.Languages.LanguagesWindow == null)
            {
                throw new Exception("Languages value is null. - InformationPage.SetTextOnControls()");
            }

            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(VariousProcessing.OwnApplicationPath() ?? Common.ApplicationName + ".exe");     // ファイルバージョン情報

            ApplicationNameLabel.Content = ApplicationData.Languages.LanguagesWindow.SoftwareName + " : " + Common.ApplicationName;
            VersionLabel.Content = ApplicationData.Languages.LanguagesWindow.Version + " : " + fvi.ProductVersion;
            CopyrightLabel.Content = fvi.LegalCopyright;
            UpdateCheckButton.Content = ApplicationData.Languages.LanguagesWindow.UpdateCheck;
            ManualButton.Content = ApplicationData.Languages.LanguagesWindow.Manual;
            UpdateHistoryButton.Content = ApplicationData.Languages.LanguagesWindow.UpdateHistory;
            WebsiteButton.Content = ApplicationData.Languages.LanguagesWindow.Website;
            ReportsRequestsButton.Content = ApplicationData.Languages.LanguagesWindow.ReportsAndRequests;
            LicenseGroupBox.Header = ApplicationData.Languages.LanguagesWindow.Library;
        }
        catch
        {
        }
    }
}
