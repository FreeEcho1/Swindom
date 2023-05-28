namespace Swindom;

/// <summary>
/// 処理するウィンドウの範囲
/// </summary>
public enum ProcessingWindowRange
{
    /// <summary>
    /// アクティブウィンドウのみ
    /// </summary>
    [XmlEnum(Name = "ActiveOnly")]
    ActiveOnly,
    /// <summary>
    /// 全てのウィンドウ
    /// </summary>
    [XmlEnum(Name = "All")]
    All
}
