namespace Swindom;

/// <summary>
/// ウィンドウイベントのデータ
/// </summary>
public class WindowEventData
{
    /// <summary>
    /// フォアグラウンドが変更された
    /// </summary>
    public bool Foreground { get; set; }
    /// <summary>
    /// 移動及びサイズの変更が終了した
    /// </summary>
    public bool MoveSizeEnd { get; set; }
    /// <summary>
    /// 最小化が開始された
    /// </summary>
    public bool MinimizeStart { get; set; }
    /// <summary>
    /// 最小化が終了した
    /// </summary>
    public bool MinimizeEnd { get; set; }
    /// <summary>
    /// 表示された
    /// </summary>
    public bool Show { get; set; }
    /// <summary>
    /// 名前が変更された
    /// </summary>
    public bool NameChange { get; set; }
    /// <summary>
    /// 処理の待ち時間 (ミリ秒) (「表示された」のみ)
    /// </summary>
    public int DelayTime { get; set; }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public WindowEventData()
    {
        Foreground = false;
        MoveSizeEnd = false;
        MinimizeStart = false;
        MinimizeEnd = false;
        Show = false;
        NameChange = false;
        DelayTime = 0;
    }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="windowEventData">ウィンドウイベントデータ</param>
    public WindowEventData(
        WindowEventData windowEventData
        )
    {
        Foreground = windowEventData.Foreground;
        MoveSizeEnd = windowEventData.MoveSizeEnd;
        MinimizeStart = windowEventData.MinimizeStart;
        MinimizeEnd = windowEventData.MinimizeEnd;
        Show = windowEventData.Show;
        NameChange = windowEventData.NameChange;
        DelayTime = windowEventData.DelayTime;
    }
}
