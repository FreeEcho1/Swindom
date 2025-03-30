namespace Swindom;

/// <summary>
/// 「更新情報」ウィンドウ
/// </summary>
public partial class UpdateInformationWindow : Window
{
    /// <summary>
    /// コンストラクタ (使用しない)
    /// </summary>
    /// <exception cref="Exception"></exception>
    public UpdateInformationWindow()
    {
        throw new Exception("Do not use. - " + GetType().Name + "()");
    }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="settingParentWindow">親ウィンドウを設定するかの値 (設定しない「false」/設定する「true」)</param>
    public UpdateInformationWindow(
        bool settingParentWindow
        )
    {
        InitializeComponent();

        if (settingParentWindow)
        {
            Owner = WindowManagement.GetWindowToSetOwner();
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
        }

        Title = ApplicationData.Languages.Check + WindowControlValue.CopySeparateString + ApplicationValue.ApplicationName;
        ThereIsTheLatestVersionLabel.Content = ApplicationData.Languages.ThereIsLatestVersion;
        OpenWebsiteButton.Content = ApplicationData.Languages.Website;
        try
        {
            _ = GetHistory();
        }
        catch
        {
            UpdateHistoryTextBox.Text = ApplicationData.Languages.ErrorOccurred;
        }

        OpenWebsiteButton.Click += OpenWebsiteButton_Click;
    }

    /// <summary>
    /// 「ContentRendered」イベント
    /// </summary>
    /// <param name="e"></param>
    protected override void OnContentRendered(
        EventArgs e
        )
    {
        base.OnContentRendered(e);
        // WPFの「SizeToContent」「WidthAndHeight」のバグ対策
        InvalidateMeasure();
    }

    /// <summary>
    /// 更新履歴を取得
    /// </summary>
    /// <returns>Task</returns>
    private async System.Threading.Tasks.Task GetHistory()
    {
        using System.Net.Http.HttpClient client = new();
        UpdateHistoryTextBox.Text = await client.GetStringAsync(ApplicationValue.UpdateHistoryURL);
    }

    /// <summary>
    /// 「ウェブサイト」Buttonの「Click」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OpenWebsiteButton_Click(
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
            FEMessageBox.Show(ApplicationData.Languages.Check, ApplicationData.Languages.ErrorOccurred, MessageBoxButton.OK);
        }
    }
}
