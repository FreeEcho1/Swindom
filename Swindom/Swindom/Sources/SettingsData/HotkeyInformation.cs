namespace Swindom;

/// <summary>
/// 「ホットキー」機能の情報
/// </summary>
public class HotkeyInformation
{
    /// <summary>
    /// 処理状態 (無効「false」/有効「true」)
    /// </summary>
    public bool IsEnabled { get; set; }
    /// <summary>
    /// 画面外に出る場合は位置やサイズを変更しない (無効「false」/有効「true」)
    /// </summary>
    public bool DoNotChangeOutOfScreen { get; set; }
    /// <summary>
    /// 全画面のウィンドウが存在する場合は処理停止 (無効「false」/有効「true」)
    /// </summary>
    public bool StopProcessingFullScreen { get; set; }
    /// <summary>
    /// ホットキーの追加/修正ウィンドウのサイズ
    /// </summary>
    public SizeInt AddModifyWindowSize { get; set; }
    /// <summary>
    /// 「ホットキー」機能の項目情報
    /// </summary>
    public List<HotkeyItemInformation> Items { get; set; }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public HotkeyInformation()
    {
        IsEnabled = false;
        DoNotChangeOutOfScreen = true;
        StopProcessingFullScreen = false;
        AddModifyWindowSize = new();
        Items = new();
    }
}
