namespace Swindom;

/// <summary>
/// 「更新確認」ウィンドウ
/// </summary>
public partial class UpdateCheckWindow : Window
{
    /// <summary>
    /// コンストラクタ (使用しない)
    /// </summary>
    public UpdateCheckWindow()
    {
        throw new Exception("Do Not Use. - UpdateCheckWindow()");
    }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="ownerWindow">オーナーウィンドウ (なし「null」)</param>
    public UpdateCheckWindow(
        Window? ownerWindow
        )
    {
        if (Common.ApplicationData.Languages.LanguagesWindow == null)
        {
            throw new Exception("LanguagesWindow value is null. - UpdateCheckWindow()");
        }

        InitializeComponent();

        if (ownerWindow != null)
        {
            Owner = ownerWindow;
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
        }
        Title = Common.ApplicationData.Languages.Check + Common.CopySeparateString + Common.ApplicationName;
        ThereIsTheLatestVersionLabel.Content = Common.ApplicationData.Languages.LanguagesWindow.ThereIsTheLatestVersion;
        try
        {
            _ = GetHistory();
        }
        catch
        {
            UpdateHistoryTextBox.Text = Common.ApplicationData.Languages.ErrorOccurred;
        }
        OpenWebsiteButton.Content = Common.ApplicationData.Languages.LanguagesWindow.Website;

        OpenWebsiteButton.Click += ButtonOpenWebsite_Click;
    }

    /// <summary>
    /// 更新履歴を取得
    /// </summary>
    /// <returns>Task</returns>
    private async System.Threading.Tasks.Task GetHistory()
    {
        using System.Net.Http.HttpClient client = new();
        UpdateHistoryTextBox.Text = await client.GetStringAsync(Common.UpdateHistoryURL);
    }

    /// <summary>
    /// 「ウェブサイト」Buttonの「Click」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ButtonOpenWebsite_Click(
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
            FreeEcho.FEControls.MessageBox.Show(Common.ApplicationData.Languages.Check, Common.ApplicationData.Languages.ErrorOccurred, FreeEcho.FEControls.MessageBoxButton.Ok, this);
        }
    }
}
