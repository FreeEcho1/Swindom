namespace Swindom;

/// <summary>
/// ウィンドウのサイズ指定の種類
/// </summary>
public enum WindowSizeType : int
{
    /// <summary>
    /// 変更しない
    /// </summary>
    [XmlEnum(Name = "DoNotChange")]
    DoNotChange,
    /// <summary>
    /// 値
    /// </summary>
    [XmlEnum(Name = "Value")]
    Value,
}
