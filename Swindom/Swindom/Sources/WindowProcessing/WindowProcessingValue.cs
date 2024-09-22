namespace Swindom;

/// <summary>
/// ウィンドウ処理の値
/// </summary>
public static class WindowProcessingValue
{
    /// <summary>
    /// タイトル名の初期の最大文字数
    /// </summary>
    public static int TitleNameMaxLength { get; } = 200;
    /// <summary>
    /// クラス名の最大文字数
    /// </summary>
    public static int ClassNameMaxLength { get; } = 256;
    /// <summary>
    /// パスの最大文字数
    /// </summary>
    public static int PathMaxLength { get; set; } = 260;
    /// <summary>
    /// パスの拡張された最大文字数
    /// </summary>
    public static int LongPathMaxLength { get; } = 32767;

    /// <summary>
    /// ウィンドウ情報取得の待ち時間
    /// </summary>
    public static int WaitTimeForWindowInformationAcquisition { get; } = 5000;
}
