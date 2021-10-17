namespace Swindom.Source
{
    /// <summary>
    /// 共通データ
    /// </summary>
    public class Common
    {
        /// <summary>
        /// アプリケーションデータ
        /// </summary>
        public static readonly ApplicationData ApplicationData = new();

        /// <summary>
        /// アプリケーション名
        /// </summary>
        public static readonly string ApplicationName = "Swindom";
        /// <summary>
        /// 設定ファイルを指定する場合の判定の為の文字列
        /// </summary>
        public static readonly string SelectSettingFile = "SelectSettingFile:";
        /// <summary>
        /// アプリケーションフォルダ名
        /// </summary>
        public static readonly string ApplicationFolderName = "Swindom";
        /// <summary>
        /// 設定フォルダ名
        /// </summary>
        public static readonly string SettingsFolderName = "Settings";
        /// <summary>
        /// 全てのユーザーの設定ファイル名
        /// </summary>
        public static readonly string SettingFileNameForAllUsers = "All Users";
        /// <summary>
        /// 設定ファイルの拡張子
        /// </summary>
        public static readonly string SettingFileExtension = ".config";
        /// <summary>
        /// 言語フォルダ名
        /// </summary>
        public static readonly string LanguagesFolderName = "Languages";
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
        /// タスクスケジューラ処理の実行ファイル
        /// </summary>
        public static readonly string TaskSchedulerProcessExecutable = "SwindomTaskScheduler.exe";
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
        public static readonly string CreateTask = "CreateTask";
        /// <summary>
        /// タスクを削除
        /// </summary>
        public static readonly string DeleteTask = "DeleteTask";
        /// <summary>
        /// タスクが登録されているか確認の文字列
        /// </summary>
        public const string CheckRegistered = "CheckRegistered";
        /// <summary>
        /// タスクスケジューラに登録されている場合の有効状態の値 (「ExitCode」)
        /// </summary>
        public const int RegisteredTaskSchedulerNumber = 1;
        /// <summary>
        /// タスクスケジューラーのフォルダ名
        /// </summary>
        public const string TaskSchedulerFolderName = "Swindom";

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
        /// クラス名の最大文字数
        /// </summary>
        public static readonly int ClassNameMaxLength = 256;
        /// <summary>
        /// ファイル名の最大文字数
        /// </summary>
        public static readonly int FileNameMaxLength = 1024;
        /// <summary>
        /// ウィンドウタイトル、コピー、の区切り文字列
        /// </summary>
        public static readonly string SeparateString = " - ";
        /// <summary>
        /// システムトレイアイコンの作成や変更の最大繰り返し回数 (失敗した場合)
        /// </summary>
        public static readonly int MaxRetryNotifyIcon = 10;

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
    }
}
