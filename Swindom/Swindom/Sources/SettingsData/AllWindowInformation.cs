namespace Swindom;

/// <summary>
/// 「全てのウィンドウ」機能の情報
/// </summary>
public class AllWindowInformation
{
    /// <summary>
    /// 処理状態 (無効「false」/有効「true」)
    /// </summary>
    public bool IsEnabled { get; set; }
    /// <summary>
    /// ウィンドウ判定で大文字と小文字を区別する (無効「false」/有効「true」)
    /// </summary>
    public bool CaseSensitive { get; set; }
    /// <summary>
    /// 全画面のウィンドウが存在する場合は処理停止 (無効「false」/有効「true」)
    /// </summary>
    public bool StopProcessingFullScreen { get; set; }
    /// <summary>
    /// ウィンドウイベントのデータ
    /// </summary>
    public WindowEventData WindowEvent { get; set; }
    /// <summary>
    /// 基準にするディスプレイ
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public StandardDisplay StandardDisplay { get; set; }
    /// <summary>
    /// 位置とサイズ
    /// </summary>
    public PositionSize PositionSize { get; set; }
    /// <summary>
    /// 移動しないウィンドウのウィンドウ判定情報
    /// </summary>
    public List<WindowJudgementInformation> Items { get; set; }
    /// <summary>
    /// 追加/修正ウィンドウのサイズ
    /// </summary>
    public SizeInt AddModifyWindowSize { get; set; }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public AllWindowInformation()
    {
        IsEnabled = false;
        CaseSensitive = true;
        StopProcessingFullScreen = false;
        StandardDisplay = StandardDisplay.CurrentDisplay;
        PositionSize = new();
        WindowEvent = new();
        Items = new();
        AddModifyWindowSize = new();
    }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="item">情報</param>
    public AllWindowInformation(
        AllWindowInformation item
        )
    {
        IsEnabled = item.IsEnabled;
        CaseSensitive = item.CaseSensitive;
        StopProcessingFullScreen = item.StopProcessingFullScreen;
        StandardDisplay = item.StandardDisplay;
        PositionSize = new(item.PositionSize);
        WindowEvent = new(item.WindowEvent);
        Items = new();
        foreach (WindowJudgementInformation nowInformation in item.Items)
        {
            item.Items.Add(new(nowInformation));
        }
        AddModifyWindowSize = new(item.AddModifyWindowSize.Width, item.AddModifyWindowSize.Height);
    }
}
