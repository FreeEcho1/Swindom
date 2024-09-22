namespace Swindom;

/// <summary>
/// 一度だけ処理
/// </summary>
public enum ProcessingOnlyOnce : int
{
    /// <summary>
    /// 指定しない
    /// </summary>
    NotSpecified,
    /// <summary>
    /// ウィンドウが開かれた時
    /// </summary>
    WindowOpen,
    /// <summary>
    /// このソフトウェアが実行されている間
    /// </summary>
    Running,
}
