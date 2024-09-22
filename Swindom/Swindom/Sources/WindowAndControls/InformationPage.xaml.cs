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

        WebsiteHyperlink.RequestNavigate += Hyperlink_RequestNavigate;
        ReportsRequestsHyperlink.RequestNavigate += Hyperlink_RequestNavigate;
        UpdateCheckButton.Click += UpdateCheckButton_Click;
        ExplanationButton.Click += ExplanationButton_Click;
        ReadmeButton.Click += ReadmeButton_Click;
        UpdateHistoryButton.Click += UpdateHistoryButton_Click;
        ModernWpfHyperlink.RequestNavigate += Hyperlink_RequestNavigate;
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
            switch (FreeEcho.FECheckForUpdate.CheckForUpdate.CheckForUpdateFileURL(ApplicationPath.OwnApplicationPath(), ApplicationValue.UpdateCheckURL, ApplicationData.Settings.CheckBetaVersion))
            {
                case FreeEcho.FECheckForUpdate.CheckForUpdateResult.LatestVersion:
                    FEMessageBox.Show(ApplicationData.Languages.LatestVersion, ApplicationData.Languages.Check, MessageBoxButton.OK);
                    break;
                case FreeEcho.FECheckForUpdate.CheckForUpdateResult.NotLatestVersion:
                    ApplicationData.WindowManagement.ShowUpdateCheckWindow(true);
                    break;
                default:
                    FEMessageBox.Show(ApplicationData.Languages.UpdateCheckFailed, ApplicationData.Languages.Check, MessageBoxButton.OK);
                    break;
            }
        }
        catch
        {
            FEMessageBox.Show(ApplicationData.Languages.ErrorOccurred, ApplicationData.Languages.Check, MessageBoxButton.OK);
        }
    }

    /// <summary>
    /// 「説明」Buttonの「Click」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ExplanationButton_Click(
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
    /// 「説明書」Buttonの「Click」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ReadmeButton_Click(
        object sender,
        RoutedEventArgs e
        )
    {
        try
        {
            FileProcessing.OpenFileAndWebPage(ApplicationPath.GetApplicationDirectory() + Path.DirectorySeparatorChar + ApplicationValue.ReadmeFileName);
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
            FileProcessing.OpenFileAndWebPage(ApplicationPath.GetApplicationDirectory() + Path.DirectorySeparatorChar + ApplicationValue.UpdateHistoryFileName);
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
            FileProcessing.OpenFileAndWebPage(ApplicationValue.WebsiteURL);
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
            FileProcessing.OpenFileAndWebPage(ApplicationValue.ReportsAndRequestsWebsiteURL);
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
            FileProcessing.OpenFileAndWebPage(e.Uri.AbsoluteUri);
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
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(ApplicationPath.OwnApplicationPath() ?? ApplicationValue.ApplicationName + ".exe");     // ファイルバージョン情報

            ApplicationInformationTextBlock.Inlines.Clear();
            ApplicationInformationTextBlock.Inlines.Add(ApplicationValue.ApplicationName + WindowControlValue.SpaceSeparateString + fvi.ProductVersion + Environment.NewLine
                + fvi.LegalCopyright + Environment.NewLine
                + ApplicationData.Languages.Website + WindowControlValue.TypeAndValueSeparateString);
            ApplicationInformationTextBlock.Inlines.Add(WebsiteHyperlink);
            ApplicationInformationTextBlock.Inlines.Add(Environment.NewLine + ApplicationData.Languages.ReportsAndRequests + WindowControlValue.TypeAndValueSeparateString);
            ApplicationInformationTextBlock.Inlines.Add(ReportsRequestsHyperlink);
            UpdateCheckButton.Content = ApplicationData.Languages.UpdateCheck;
            ExplanationButton.Content = ApplicationData.Languages.Help;
            ReadmeButton.Content = ApplicationData.Languages.Readme;
            UpdateHistoryButton.Content = ApplicationData.Languages.UpdateHistory;
            LicenseGroupBox.Header = ApplicationData.Languages.Library;
        }
        catch
        {
        }
    }
}
