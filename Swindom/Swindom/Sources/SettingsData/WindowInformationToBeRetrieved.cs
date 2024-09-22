namespace Swindom;

/// <summary>
/// 取得するウィンドウ情報
/// </summary>
public class WindowInformationToBeRetrieved
{
    /// <summary>
    /// タイトル名
    /// </summary>
    public bool TitleName { get; set; }
    /// <summary>
    /// クラス名
    /// </summary>
    public bool ClassName { get; set; }
    /// <summary>
    /// ファイル名
    /// </summary>
    public bool FileName { get; set; }
    /// <summary>
    /// ディスプレイ
    /// </summary>
    public bool Display { get; set; }
    /// <summary>
    /// ウィンドウの状態
    /// </summary>
    public bool WindowState { get; set; }
    /// <summary>
    /// X
    /// </summary>
    public bool X { get; set; }
    /// <summary>
    /// Y
    /// </summary>
    public bool Y { get; set; }
    /// <summary>
    /// 幅
    /// </summary>
    public bool Width { get; set; }
    /// <summary>
    /// 高さ
    /// </summary>
    public bool Height { get; set; }
    /// <summary>
    /// バージョン
    /// </summary>
    public bool Version { get; set; }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public WindowInformationToBeRetrieved()
    {
        TitleName = true;
        ClassName = true;
        FileName = true;
        Display = true;
        WindowState = true;
        X = true;
        Y = true;
        Width = true;
        Height = true;
        Version = true;
    }
}
