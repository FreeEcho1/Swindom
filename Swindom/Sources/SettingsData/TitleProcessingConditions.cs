namespace Swindom;

/// <summary>
/// タイトル名の条件
/// </summary>
public enum TitleNameProcessingConditions : int
{
    /// <summary>
    /// 指定しない
    /// </summary>
    NotSpecified,
    /// <summary>
    /// タイトル名がないウィンドウ
    /// </summary>
    NotIncluded,
    /// <summary>
    /// タイトル名があるウィンドウ
    /// </summary>
    Included
}
