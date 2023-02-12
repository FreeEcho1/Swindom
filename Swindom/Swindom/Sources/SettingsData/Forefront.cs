namespace Swindom.Sources.SettingsData;

/// <summary>
/// 最前面の種類
/// </summary>
public enum Forefront : int
{
    /// <summary>
    /// 変更しない
    /// </summary>
    [XmlEnum(Name = "DoNotChange")]
    DoNotChange,
    /// <summary>
    /// 常に最前面解除
    /// </summary>
    [XmlEnum(Name = "Cancel")]
    Cancel,
    /// <summary>
    /// 常に最前面
    /// </summary>
    [XmlEnum(Name = "Always")]
    Always,
    /// <summary>
    /// 最前面
    /// </summary>
    [XmlEnum(Name = "Forefront")]
    Forefront
}
