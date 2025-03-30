namespace Swindom;

/// <summary>
/// 設定の値
/// </summary>
public static class SettingsValue
{
    /// <summary>
    /// 設定ディレクトリ名
    /// </summary>
    public static string SettingsDirectoryName { get; } = "Settings";
    /// <summary>
    /// 全てのユーザーの設定ファイル名
    /// </summary>
    public static string SettingFileNameForAllUsers { get; } = "All Users";
    /// <summary>
    /// 設定ファイルの拡張子
    /// </summary>
    public static string SettingFileExtension { get; } = ".ini";
    /// <summary>
    /// 設定ファイルの拡張子 (旧)
    /// </summary>
    public static string OldSettingFileExtension { get; } = ".config";

    /// <summary>
    /// 設定ファイル指定の判定用のの文字列
    /// </summary>
    public static string SettingFileSpecificationString { get; } = "SelectSettingFile:";
}
