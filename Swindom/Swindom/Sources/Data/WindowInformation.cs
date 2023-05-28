namespace Swindom;

/// <summary>
/// ウィンドウの情報
/// </summary>
public class WindowInformation
{
    /// <summary>
    /// タイトル名
    /// </summary>
    public string TitleName = "";
    /// <summary>
    /// クラス名
    /// </summary>
    public string ClassName = "";
    /// <summary>
    /// ファイル名
    /// </summary>
    public string FileName = "";
    /// <summary>
    /// バージョン
    /// </summary>
    public string Version = "";
    /// <summary>
    /// 位置とサイズ
    /// </summary>
    public RectangleInt Rectangle = new();
    /// <summary>
    /// showCmd
    /// </summary>
    public int ShowCmd;
}
