namespace Swindom;

/// <summary>
/// 取得するウィンドウ情報
/// </summary>
public class WindowInformationToBeRetrieved
{
    /// <summary>
    /// タイトル名
    /// </summary>
    public bool TitleName;
    /// <summary>
    /// クラス名
    /// </summary>
    public bool ClassName;
    /// <summary>
    /// ファイル名
    /// </summary>
    public bool FileName;
    /// <summary>
    /// ディスプレイ
    /// </summary>
    public bool Display;
    /// <summary>
    /// ウィンドウの状態
    /// </summary>
    public bool WindowState;
    /// <summary>
    /// X
    /// </summary>
    public bool X;
    /// <summary>
    /// Y
    /// </summary>
    public bool Y;
    /// <summary>
    /// 幅
    /// </summary>
    public bool Width;
    /// <summary>
    /// 高さ
    /// </summary>
    public bool Height;
    /// <summary>
    /// バージョン
    /// </summary>
    public bool Version;

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
        Version = false;
    }
}
