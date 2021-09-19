namespace Swindom.Source.Window
{
    /// <summary>
    /// 「更新確認」ウィンドウ
    /// </summary>
    public partial class UpdateCheckWindow : System.Windows.Window
    {
        /// <summary>
        /// コンストラクタ (使用しない)
        /// </summary>
        public UpdateCheckWindow()
        {
            throw new System.Exception("Do Not Use. - UpdateCheckWindow()");
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="ownerWindow">オーナーウィンドウ (なし「null」)</param>
        public UpdateCheckWindow(
            System.Windows.Window ownerWindow
            )
        {
            InitializeComponent();

            if (ownerWindow != null)
            {
                Owner = ownerWindow;
                WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
            }
            Title = Common.ApplicationData.Languages.Check + Common.SeparateString + Common.ApplicationName;
            ThereIsTheLatestVersionLabel.Content = Common.ApplicationData.Languages.LanguagesWindow.ThereIsTheLatestVersion;
            try
            {
                using System.Net.WebClient webClient = new();
                System.Text.Encoding beforeEncoding = System.Text.Encoding.UTF8;
                System.Text.Encoding afterEncoding = System.Text.Encoding.Unicode;
                byte[] beforeByte = webClient.DownloadData(Common.UpdateHistoryURL);
                byte[] afterByte = System.Text.Encoding.Convert(beforeEncoding, afterEncoding, beforeByte);
                UpdateHistoryTextBox.Text = System.Text.Encoding.Unicode.GetString(afterByte);
            }
            catch
            {
                UpdateHistoryTextBox.Text = Common.ApplicationData.Languages.ErrorOccurred;
            }
            OpenWebsiteButton.Content = Common.ApplicationData.Languages.LanguagesWindow.Website;

            OpenWebsiteButton.Click += ButtonOpenWebsite_Click;
        }

        /// <summary>
        /// 「ウェブサイト」Buttonの「Click」イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonOpenWebsite_Click(
            object sender,
            System.Windows.RoutedEventArgs e
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
}
