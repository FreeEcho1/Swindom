namespace Swindom.Sources.SettingsData;

/// <summary>
/// ファイル名の一致条件
/// </summary>
public enum FileNameMatchCondition : int
{
    /// <summary>
    /// パスを含む
    /// </summary>
    [XmlEnum(Name = "Include")]
    Include,
    /// <summary>
    /// パスを含まない
    /// </summary>
    [XmlEnum(Name = "NotInclude")]
    NotInclude
}
