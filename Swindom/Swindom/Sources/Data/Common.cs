namespace Swindom;

/// <summary>
/// 共通データ
/// </summary>
public static class Common
{
    /// <summary>
    /// アプリケーションデータ
    /// </summary>
    public static readonly ApplicationData ApplicationData = new();
    /// <summary>
    /// モニター情報
    /// </summary>
    public static MonitorInformation MonitorInformation { get; set; } = MonitorInformation.GetMonitorInformation();

    /// <summary>
    /// アプリケーション名
    /// </summary>
    public static readonly string ApplicationName = "Swindom";
    /// <summary>
    /// 設定ファイルを指定する場合の判定の為の文字列
    /// </summary>
    public static readonly string SelectSettingFile = "SelectSettingFile:";
    /// <summary>
    /// アプリケーションディレクトリ名
    /// </summary>
    public static readonly string ApplicationDirectoryName = "Swindom";
    /// <summary>
    /// 設定ディレクトリ名
    /// </summary>
    public static readonly string SettingsDirectoryName = "Settings";
    /// <summary>
    /// 全てのユーザーの設定ファイル名
    /// </summary>
    public static readonly string SettingFileNameForAllUsers = "All Users";
    /// <summary>
    /// 設定ファイルの拡張子
    /// </summary>
    public static readonly string SettingFileExtension = ".config";
    /// <summary>
    /// 言語ディレクトリ名
    /// </summary>
    public static readonly string LanguagesDirectoryName = "Languages";
    /// <summary>
    /// 言語ファイルの拡張子
    /// </summary>
    public static readonly string LanguageFileExtension = ".lang";
    /// <summary>
    /// Readmeファイル名
    /// </summary>
    public static readonly string ReadmeFileName = "Readme.txt";
    /// <summary>
    /// 更新履歴ファイル名
    /// </summary>
    public static readonly string UpdateHistoryFileName = "UpdateHistory.txt";
    /// <summary>
    /// スタートアップの登録名
    /// </summary>
    public static readonly string StartupRegistrationName = "Swindom";
    /// <summary>
    /// インストール情報のレジストリキー名
    /// </summary>
    public static readonly string RegistryKeyNameForInstallInformation = "Swindom";
    /// <summary>
    /// True
    /// </summary>
    public static readonly string True = "True";
    /// <summary>
    /// False
    /// </summary>
    public static readonly string False = "False";
    /// <summary>
    /// タスクを作成
    /// </summary>
    public const string CreateTask = "CreateTask";
    /// <summary>
    /// タスクを削除
    /// </summary>
    public const string DeleteTask = "DeleteTask";
    /// <summary>
    /// タスクが登録されているか確認の文字列
    /// </summary>
    public const string CheckRegistered = "CheckRegistered";
    /// <summary>
    /// タスクに登録されている
    /// </summary>
    public const int RegisteredTask = 1;
    /// <summary>
    /// タスクに登録されていない
    /// </summary>
    public const int NotRegisteredTask = 2;
    /// <summary>
    /// タスクスケジューラーのフォルダ名
    /// </summary>
    public const string TaskSchedulerFolderName = "Swindom";
    /// <summary>
    /// 登録情報の「Author」
    /// </summary>
    public const string RegistrationInformationAuthor = "FreeEcho";
    /// <summary>
    /// 登録するアプリケーションのファイル名
    /// </summary>
    public const string ApplicationFileNameToRegister = "Swindom.exe";
    /// <summary>
    /// タスクスケジューラーのディレクトリ名
    /// </summary>
    public static readonly string TaskSchedulerDirectoryName = "Swindom";
    /// <summary>
    /// パーセントの最小値
    /// </summary>
    public static readonly int PercentMinimize = 0;
    /// <summary>
    /// パーセントの最大値
    /// </summary>
    public static readonly int PercentMaximize = 100;
    /// <summary>
    /// 全画面ウィンドウの判定タイマーの間隔 (ミリ秒)
    /// </summary>
    public static readonly int FullScreenWindowDecisionTimerInterval = 10000;
    /// <summary>
    /// 設定コントロールの最小の高さ
    /// </summary>
    public static readonly int SettingsRowDefinitionMinimize = 46;
    /// <summary>
    /// Tab文字列
    /// </summary>
    public static readonly string TabString = "Tab";
    /// <summary>
    /// ウェブサイトのURL
    /// </summary>
    public static readonly string WebsiteURL = "https://github.com/FreeEcho1/Swindom";
    /// <summary>
    /// 更新確認のURL
    /// </summary>
    public static readonly string UpdateCheckURL = "https://raw.githubusercontent.com/FreeEcho1/Swindom/main/NewVersionInformation.txt";
    /// <summary>
    /// 報告、要望のウェブサイトのURL
    /// </summary>
    public static readonly string ReportsAndRequestsWebsiteURL = "http://kaciydiscovery.bbs.fc2.com/";
    /// <summary>
    /// 更新履歴情報のURL
    /// </summary>
    public static readonly string UpdateHistoryURL = "https://raw.githubusercontent.com/FreeEcho1/Swindom/main/UpdateHistory.txt";

    /// <summary>
    /// タイトル名の初期の最大文字数
    /// </summary>
    public static readonly int TitleNameMaxLength = 200;
    /// <summary>
    /// クラス名の最大文字数
    /// </summary>
    public static readonly int ClassNameMaxLength = 256;
    /// <summary>
    /// ファイル名の文字数
    /// </summary>
    public static int FileNameLength { get; set; } = 300;
    /// <summary>
    /// ファイル名の文字数を増やす文字数
    /// </summary>
    public static readonly int FileNameLengthAdd = 100;
    /// <summary>
    /// ファイル名の最大文字数
    /// </summary>
    public static readonly int FileNameMaxLength = 20000;
    /// <summary>
    /// システムトレイアイコンの作成や変更の最大繰り返し回数 (失敗した場合)
    /// </summary>
    public static readonly int MaxRetryNotifyIcon = 10;
    /// <summary>
    /// ウィンドウ情報取得の待ち時間
    /// </summary>
    public static readonly int WaitTimeForWindowInformationAcquisition = 5000;
    /// <summary>
    /// イベントのホットキーの開始ID
    /// </summary>
    public static readonly int EventHotkeysStartId = 0;
    /// <summary>
    /// タイマーのホットキーの開始ID
    /// </summary>
    public static readonly int TimerHotkeysStartId = 200;
    /// <summary>
    /// ホットキーの開始ID
    /// </summary>
    public static readonly int HotkeysStartId = 400;

    /// <summary>
    /// リソースの画像の「CloseSettings」のUri
    /// </summary>
    public static readonly string ImageResourcesCloseSettings = "pack://application:,,,/SwindomImage;component/Resources/CloseSettings.png";
    /// <summary>
    /// リソースの画像の「Settings」のUri
    /// </summary>
    public static readonly string ImageResourcesSettings = "pack://application:,,,/SwindomImage;component/Resources/Settings.png";
    /// <summary>
    /// リソースの画像の「LinkedChain」のUri
    /// </summary>
    public static readonly string ImageResourcesLinkedChain = "pack://application:,,,/SwindomImage;component/Resources/LinkedChain.png";
    /// <summary>
    /// リソースの画像の「UnattachedChain」のUri
    /// </summary>
    public static readonly string ImageResourcesUnattachedChain = "pack://application:,,,/SwindomImage;component/Resources/UnattachedChain.png";
    /// <summary>
    /// 「MessageBoxStyle」のUri
    /// </summary>
    public static readonly string UriMessageBoxStyle = "pack://application:,,,/ControlsStyle;component/MessageBoxStyle.xaml";

    /// <summary>
    /// ウィンドウタイトル、コピー、の区切り文字列
    /// </summary>
    public static readonly string CopySeparateString = " - ";
    /// <summary>
    /// 値の種類と値の区切り文字列 (例：「X:100px」)
    /// </summary>
    public static readonly string TypeAndValueSeparateString = ":";
    /// <summary>
    /// 値の説明と値の区切り文字列 (例：「位置とサイズを指定 | 値」)
    /// </summary>
    public static readonly string ExplainAndValueSeparateString = " | ";
    /// <summary>
    /// 値と値の区切り文字列 (例：「X:100px / Y:50px」)
    /// </summary>
    public static readonly string ValueAndValueSeparateString = " / ";
    /// <summary>
    /// スペースの区切り文字列
    /// </summary>
    public static readonly string SpaceSeparateString = " ";
}
