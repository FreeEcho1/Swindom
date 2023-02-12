namespace Swindom.Sources.SettingsData;

/// <summary>
/// 位置とサイズの値の種類
/// </summary>
public enum PositionSizeValueType : int
{
    /// <summary>
    /// Pixel
    /// </summary>
    [XmlEnum(Name = "Pixel")]
    Pixel,
    /// <summary>
    /// %
    /// </summary>
    [XmlEnum(Name = "Percent")]
    Percent
}
