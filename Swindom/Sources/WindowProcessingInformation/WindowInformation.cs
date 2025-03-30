namespace Swindom;

/// <summary>
/// ウィンドウの情報
/// </summary>
public class WindowInformation
{
    /// <summary>
    /// タイトル名
    /// </summary>
    public string TitleName { get; set; } = "";
    /// <summary>
    /// クラス名
    /// </summary>
    public string ClassName { get; set; } = "";
    /// <summary>
    /// ファイル名
    /// </summary>
    public string FileName { get; set; } = "";
    /// <summary>
    /// バージョン
    /// </summary>
    public string Version { get; set; } = "";
    /// <summary>
    /// 位置とサイズ
    /// </summary>
    public RectangleInt Rectangle { get; set; } = new();
    /// <summary>
    /// showCmd
    /// </summary>
    public int ShowCmd { get; set; }
}
