namespace Swindom;

/// <summary>
/// 一度だけ処理
/// </summary>
public enum ProcessingOnlyOnce : int
{
    /// <summary>
    /// 指定しない
    /// </summary>
    [XmlEnum(Name = "NotSpecified")]
    NotSpecified,
    /// <summary>
    /// ウィンドウが開かれた時
    /// </summary>
    [XmlEnum(Name = "WindowOpen")]
    WindowOpen,
    /// <summary>
    /// このソフトウェアが実行されている間
    /// </summary>
    [XmlEnum(Name = "Running")]
    Running,
}
