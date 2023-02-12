namespace Swindom.Sources.Data;

/// <summary>
/// 共通データ
/// </summary>
public class Common
{
    /// <summary>
    /// アプリケーション名
    /// </summary>
    public const string ApplicationName = "Swindom";
    /// <summary>
    /// 設定ファイル指定の判定用のの文字列
    /// </summary>
    public const string SettingFileSpecificationString = "SelectSettingFile:";
    /// <summary>
    /// 設定ディレクトリ名
    /// </summary>
    public const string SettingsDirectoryName = "Settings";
    /// <summary>
    /// 全てのユーザーの設定ファイル名
    /// </summary>
    public const string SettingFileNameForAllUsers = "All Users";
    /// <summary>
    /// 設定ファイルの拡張子
    /// </summary>
    public const string SettingFileExtension = ".config";
    /// <summary>
    /// 言語ディレクトリ名
    /// </summary>
    public const string LanguagesDirectoryName = "Languages";
    /// <summary>
    /// 言語ファイルの拡張子
    /// </summary>
    public const string LanguageFileExtension = ".lang";
    /// <summary>
    /// Readmeファイル名
    /// </summary>
    public const string ReadmeFileName = "Readme.txt";
    /// <summary>
    /// 更新履歴ファイル名
    /// </summary>
    public const string UpdateHistoryFileName = "UpdateHistory.txt";
    /// <summary>
    /// 設定コントロールの最小の高さ
    /// </summary>
    public const int SettingsRowDefinitionMinimize = 46;
    /// <summary>
    /// Tab文字列
    /// </summary>
    public const string TabString = "Tab";
    /// <summary>
    /// ウェブサイトのURL
    /// </summary>
    public const string WebsiteURL = "https://github.com/FreeEcho1/Swindom";
    /// <summary>
    /// 更新確認のURL
    /// </summary>
    public const string UpdateCheckURL = "https://raw.githubusercontent.com/FreeEcho1/Swindom/main/NewVersionInformation.txt";
    /// <summary>
    /// 報告、要望のウェブサイトのURL
    /// </summary>
    public const string ReportsAndRequestsWebsiteURL = "https://kaciydiscovery.bbs.fc2.com/";
    /// <summary>
    /// 更新履歴情報のURL
    /// </summary>
    public const string UpdateHistoryURL = "https://raw.githubusercontent.com/FreeEcho1/Swindom/main/UpdateHistory.txt";

    /// <summary>
    /// タイトル名の初期の最大文字数
    /// </summary>
    public const int TitleNameMaxLength = 200;
    /// <summary>
    /// クラス名の最大文字数
    /// </summary>
    public const int ClassNameMaxLength = 256;
    /// <summary>
    /// パスの最大文字数
    /// </summary>
    public static int PathMaxLength { get; set; } = 260;
    /// <summary>
    /// パスの拡張された最大文字数
    /// </summary>
    public const int LongPathMaxLength = 32767;
    /// <summary>
    /// イベントのホットキーの開始ID
    /// </summary>
    public const int EventHotkeysStartId = 0;
    /// <summary>
    /// ホットキーの開始ID
    /// </summary>
    public const int HotkeysStartId = 200;

    /// <summary>
    /// ウィンドウタイトル、コピー、の区切り文字列
    /// </summary>
    public const string CopySeparateString = " - ";
    /// <summary>
    /// 値の種類と値の区切り文字列 (例：「X:100px」)
    /// </summary>
    public const string TypeAndValueSeparateString = " : ";
    /// <summary>
    /// 値の説明と値の区切り文字列 (例：「位置とサイズを指定 | 値」)
    /// </summary>
    public const string ExplainAndValueSeparateString = " | ";
    /// <summary>
    /// 値と値の区切り文字列 (例：「X:100px / Y:50px」)
    /// </summary>
    public const string ValueAndValueSeparateString = " / ";
    /// <summary>
    /// スペースの区切り文字列
    /// </summary>
    public const string SpaceSeparateString = " ";
}
