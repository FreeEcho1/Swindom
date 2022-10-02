namespace Swindom;

/// <summary>
/// タイトルの処理条件
/// </summary>
public enum TitleProcessingConditions : int
{
    /// <summary>
    /// 指定しない
    /// </summary>
    [XmlEnum(Name = "NotSpecified")]
    NotSpecified,
    /// <summary>
    /// タイトルがないウィンドウは処理しない
    /// </summary>
    [XmlEnum(Name = "DoNotProcessingUntitledWindow")]
    DoNotProcessingUntitledWindow,
    /// <summary>
    /// タイトルがあるウィンドウは処理しない
    /// </summary>
    [XmlEnum(Name = "DoNotProcessingWindowWithTitle")]
    DoNotProcessingWindowWithTitle
}
