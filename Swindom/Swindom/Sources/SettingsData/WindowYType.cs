namespace Swindom.Sources.SettingsData;

/// <summary>
/// ウィンドウのY位置指定の種類
/// </summary>
public enum WindowYType : int
{
    /// <summary>
    /// 変更しない
    /// </summary>
    [XmlEnum(Name = "DoNotChange")]
    DoNotChange,
    /// <summary>
    /// 上端
    /// </summary>
    [XmlEnum(Name = "Top")]
    Top,
    /// <summary>
    /// 中央
    /// </summary>
    [XmlEnum(Name = "Middle")]
    Middle,
    /// <summary>
    /// 下端
    /// </summary>
    [XmlEnum(Name = "Bottom")]
    Bottom,
    /// <summary>
    /// 値
    /// </summary>
    [XmlEnum(Name = "Value")]
    Value,
}
