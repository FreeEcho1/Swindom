namespace Swindom;

/// <summary>
/// 「全てのウィンドウ」機能の情報
/// </summary>
public class AllWindowInformation
{
    /// <summary>
    /// 処理状態 (無効「false」/有効「true」)
    /// </summary>
    public bool Enabled;
    /// <summary>
    /// ウィンドウ判定で大文字と小文字を区別する (無効「false」/有効「true」)
    /// </summary>
    public bool CaseSensitive;
    /// <summary>
    /// ウィンドウが全画面表示の場合は処理停止 (無効「false」/有効「true」)
    /// </summary>
    public bool StopProcessingFullScreen;
    /// <summary>
    /// 基準にするディスプレイ
    /// </summary>
    public StandardDisplay StandardDisplay;
    /// <summary>
    /// 位置とサイズ
    /// </summary>
    public PositionSize PositionSize;
    /// <summary>
    /// 「全てのウィンドウを指定した位置に移動」のウィンドウイベントのデータ
    /// </summary>
    public WindowEventData AllWindowPositionSizeWindowEventData;
    /// <summary>
    /// 「全てのウィンドウを指定した位置に移動」の「移動しないウィンドウ」のウィンドウ判定情報
    /// </summary>
    public List<WindowJudgementInformation> CancelAllWindowPositionSize;
    /// <summary>
    /// 追加/修正ウィンドウのサイズ
    /// </summary>
    public System.Drawing.Size AddModifyWindowSize;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public AllWindowInformation()
    {
        Enabled = false;
        CaseSensitive = true;
        StopProcessingFullScreen = false;
        StandardDisplay = StandardDisplay.CurrentDisplay;
        PositionSize = new();
        AllWindowPositionSizeWindowEventData = new();
        CancelAllWindowPositionSize = new();
    }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="item">情報</param>
    public AllWindowInformation(
        AllWindowInformation item
        )
    {
        Enabled = item.Enabled;
        CaseSensitive = item.CaseSensitive;
        StopProcessingFullScreen = item.StopProcessingFullScreen;
        StandardDisplay = item.StandardDisplay;
        PositionSize = new(item.PositionSize);
        AllWindowPositionSizeWindowEventData = new(item.AllWindowPositionSizeWindowEventData);
        CancelAllWindowPositionSize = new();
        foreach (WindowJudgementInformation nowInformation in item.CancelAllWindowPositionSize)
        {
            item.CancelAllWindowPositionSize.Add(new(nowInformation));
        }
    }
}
