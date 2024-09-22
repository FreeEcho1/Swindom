namespace Swindom;

/// <summary>
/// 全て処理停止の情報
/// </summary>
public class AllStopProcessingInformation
{
    /// <summary>
    /// 処理状態 (無効「false」/有効「true」)
    /// </summary>
    public bool IsEnabled { get; set; }
    /// <summary>
    /// 「指定ウィンドウ」機能の状態 (無効「false」/有効「true」)
    /// </summary>
    public bool SpecifyWindowIsEnabled { get; set; }
    /// <summary>
    /// 「全てのウィンドウ」機能の状態 (無効「false」/有効「true」)
    /// </summary>
    public bool AllWindowIsEnabled { get; set; }
    /// <summary>
    /// 「マグネット」機能の状態 (無効「false」/有効「true」)
    /// </summary>
    public bool MagnetIsEnabled { get; set; }
    /// <summary>
    /// 「ホットキー」機能の状態 (無効「false」/有効「true」)
    /// </summary>
    public bool HotkeyIsEnabled { get; set; }
}
