namespace Swindom.Sources.SettingsData;

/// <summary>
/// ウィンドウのX位置指定の種類
/// </summary>
public enum WindowXType : int
{
    /// <summary>
    /// 変更しない
    /// </summary>
    [XmlEnum(Name = "DoNotChange")]
    DoNotChange,
    /// <summary>
    /// 左端
    /// </summary>
    [XmlEnum(Name = "Left")]
    Left,
    /// <summary>
    /// 中央
    /// </summary>
    [XmlEnum(Name = "Middle")]
    Middle,
    /// <summary>
    /// 右端
    /// </summary>
    [XmlEnum(Name = "Right")]
    Right,
    /// <summary>
    /// 値
    /// </summary>
    [XmlEnum(Name = "Value")]
    Value,
}
