namespace Swindom;

/// <summary>
/// 基準にするディスプレイ
/// </summary>
public enum StandardDisplay : int
{
    /// <summary>
    /// 現在のディスプレイ (複数アクティブ不可)
    /// </summary>
    [XmlEnum(Name = "CurrentDisplay")]
    CurrentDisplay,
    /// <summary>
    /// 指定したディスプレイ (複数アクティブ不可)
    /// </summary>
    [XmlEnum(Name = "SpecifiedDisplay")]
    SpecifiedDisplay,
    /// <summary>
    /// 指定したディスプレイ限定 (複数アクティブ可)
    /// </summary>
    [XmlEnum(Name = "ExclusiveSpecifiedDisplay")]
    ExclusiveSpecifiedDisplay
}
