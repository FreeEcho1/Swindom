namespace Swindom;

/// <summary>
/// 「情報」コントロール
/// </summary>
public partial class InformationControl : UserControl, IDisposable
{
    /// <summary>
    /// Disposeが呼ばれたかの値
    /// </summary>
    private bool Disposed;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public InformationControl()
    {
        InitializeComponent();

        SetTextOnControls();

        UpdateCheckButton.Click += UpdateCheckButton_Click;
        ManualButton.Click += ManualButton_Click;
        UpdateHistoryButton.Click += UpdateHistoryButton_Click;
        WebsiteButton.Click += WebsiteButton_Click;
        ReportsRequestsButton.Click += ReportsRequestsButton_Click;
        DotNetHyperlink.RequestNavigate += Hyperlink_RequestNavigate;
        FreeEchoControlsHyperlink.RequestNavigate += Hyperlink_RequestNavigate;
        FreeEchoWindowEventHyperlink.RequestNavigate += Hyperlink_RequestNavigate;
        FreeEchoHotKeyWPFHyperlink.RequestNavigate += Hyperlink_RequestNavigate;
        FreeEchoWindowSelectionMouseHyperlink.RequestNavigate += Hyperlink_RequestNavigate;
        FreeEchoWindowMoveDetectionMouseHyperlink.RequestNavigate += Hyperlink_RequestNavigate;
        FreeEchoCheckForUpdateHyperlink.RequestNavigate += Hyperlink_RequestNavigate;
        Common.ApplicationData.ProcessingEvent += ApplicationData_ProcessingEvent;
    }

    /// <summary>
    /// デストラクタ
    /// </summary>
    ~InformationControl()
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
            switch (FreeEcho.FECheckForUpdate.CheckForUpdate.CheckForUpdateFileURL(Processing.OwnApplicationPath(), Common.UpdateCheckURL, Common.ApplicationData.Settings.CheckBetaVersion))
            {
                case FreeEcho.FECheckForUpdate.CheckForUpdateResult.LatestVersion:
                    FreeEcho.FEControls.MessageBox.Show(Common.ApplicationData.Languages.Check, Common.ApplicationData.Languages.LanguagesWindow?.LatestVersion, FreeEcho.FEControls.MessageBoxButton.Ok, Window.GetWindow(this));
                    break;
                case FreeEcho.FECheckForUpdate.CheckForUpdateResult.NotLatestVersion:
                    Common.ApplicationData.WindowManagement.ShowUpdateCheckWindow(Window.GetWindow(this));
                    break;
                default:
                    FreeEcho.FEControls.MessageBox.Show(Common.ApplicationData.Languages.Check, Common.ApplicationData.Languages.LanguagesWindow?.UpdateCheckFailed, FreeEcho.FEControls.MessageBoxButton.Ok, Window.GetWindow(this));
                    break;
            }
        }
        catch
        {
            FreeEcho.FEControls.MessageBox.Show(Common.ApplicationData.Languages.Check, Common.ApplicationData.Languages.ErrorOccurred, FreeEcho.FEControls.MessageBoxButton.Ok, Window.GetWindow(this));
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
            Processing.OpenFileAndUrl(Processing.GetApplicationDirectoryPath() + Path.DirectorySeparatorChar + Common.ReadmeFileName);
        }
        catch
        {
            FreeEcho.FEControls.MessageBox.Show(Common.ApplicationData.Languages.Check, Common.ApplicationData.Languages.ErrorOccurred, FreeEcho.FEControls.MessageBoxButton.Ok, Window.GetWindow(this));
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
            Processing.OpenFileAndUrl(Processing.GetApplicationDirectoryPath() + Path.DirectorySeparatorChar + Common.UpdateHistoryFileName);
        }
        catch
        {
            FreeEcho.FEControls.MessageBox.Show(Common.ApplicationData.Languages.Check, Common.ApplicationData.Languages.ErrorOccurred, FreeEcho.FEControls.MessageBoxButton.Ok, Window.GetWindow(this));
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
            Processing.OpenFileAndUrl(Common.WebsiteURL);
        }
        catch
        {
            FreeEcho.FEControls.MessageBox.Show(Common.ApplicationData.Languages.Check, Common.ApplicationData.Languages.ErrorOccurred, FreeEcho.FEControls.MessageBoxButton.Ok, Window.GetWindow(this));
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
            Processing.OpenFileAndUrl(Common.ReportsAndRequestsWebsiteURL);
        }
        catch
        {
            FreeEcho.FEControls.MessageBox.Show(Common.ApplicationData.Languages.Check, Common.ApplicationData.Languages.ErrorOccurred, FreeEcho.FEControls.MessageBoxButton.Ok, Window.GetWindow(this));
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
            Processing.OpenFileAndUrl(e.Uri.AbsoluteUri);
        }
        catch
        {
            FreeEcho.FEControls.MessageBox.Show(Common.ApplicationData.Languages.Check, Common.ApplicationData.Languages.ErrorOccurred, FreeEcho.FEControls.MessageBoxButton.Ok, Window.GetWindow(this));
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
                throw new Exception("LanguagesWindow value is null. - InformationControl.SetTextOnControls()");
            }

            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(Processing.OwnApplicationPath() ?? Common.ApplicationName + ".exe");     // ファイルバージョン情報

            ApplicationNameLabel.Content = Common.ApplicationData.Languages.LanguagesWindow.SoftwareName + " : " + Common.ApplicationName;
            VersionLabel.Content = Common.ApplicationData.Languages.LanguagesWindow.Version + " : " + fvi.ProductVersion;
            CopyrightLabel.Content = fvi.LegalCopyright;
            UpdateCheckButton.Content = Common.ApplicationData.Languages.LanguagesWindow.UpdateCheck;
            ManualButton.Content = Common.ApplicationData.Languages.LanguagesWindow.Manual;
            UpdateHistoryButton.Content = Common.ApplicationData.Languages.LanguagesWindow.UpdateHistory;
            WebsiteButton.Content = Common.ApplicationData.Languages.LanguagesWindow.Website;
            ReportsRequestsButton.Content = Common.ApplicationData.Languages.LanguagesWindow.ReportsAndRequests;
            LicenseLabel.Content = Common.ApplicationData.Languages.LanguagesWindow.Library;
        }
        catch
        {
        }
    }
}
