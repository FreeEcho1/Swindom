namespace Swindom;

/// <summary>
/// ウィンドウ情報のバッファ
/// </summary>
public class WindowInformationBuffer
{
    /// <summary>
    /// 処理中かの値 (処理中ではない「false」 / 処理中「true」)
    /// </summary>
    public bool ProcessingGetWindowInformation;
    /// <summary>
    /// タイトル名
    /// </summary>
    public StringBuilder TitleName = new(Common.TitleNameMaxLength);
    /// <summary>
    /// クラス名
    /// </summary>
    public StringBuilder ClassName = new(Common.ClassNameMaxLength);
    /// <summary>
    /// ファイル名
    /// </summary>
    public StringBuilder FileName = new(Common.PathLength);
}
