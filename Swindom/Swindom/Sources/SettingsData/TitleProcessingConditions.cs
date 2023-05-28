namespace Swindom;

/// <summary>
/// タイトル名の条件
/// </summary>
public enum TitleNameProcessingConditions : int
{
    /// <summary>
    /// 指定しない
    /// </summary>
    [XmlEnum(Name = "NotSpecified")]
    NotSpecified,
    /// <summary>
    /// タイトル名がないウィンドウ
    /// </summary>
    [XmlEnum(Name = "NotIncluded")]
    NotIncluded,
    /// <summary>
    /// タイトル名があるウィンドウ
    /// </summary>
    [XmlEnum(Name = "Included")]
    Included
}
