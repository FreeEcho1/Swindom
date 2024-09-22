namespace Swindom;

/// <summary>
/// ウィンドウとコントロールの値
/// </summary>
public static class WindowControlValue
{
    /// <summary>
    /// 設定コントロールの最小の高さ
    /// </summary>
    public static int SettingsRowDefinitionMinimize { get; } = 46;

    /// <summary>
    /// Tab文字列
    /// </summary>
    public static string TabString { get; } = "Tab";

    /// <summary>
    /// ウィンドウタイトル、コピー、の区切り文字列
    /// </summary>
    public static string CopySeparateString { get; } = " - ";
    /// <summary>
    /// 値の種類と値の区切り文字列 (例：「X:100px」)
    /// </summary>
    public static string TypeAndValueSeparateString { get; } = " : ";
    /// <summary>
    /// 値の説明と値の区切り文字列 (例：「位置とサイズを指定 | 値」)
    /// </summary>
    public static string ExplainAndValueSeparateString { get; } = " | ";
    /// <summary>
    /// 値と値の区切り文字列 (例：「X:100px / Y:50px」)
    /// </summary>
    public static string ValueAndValueSeparateString { get; } = " / ";
    /// <summary>
    /// スペースの区切り文字列
    /// </summary>
    public static string SpaceSeparateString { get; } = " ";
}
