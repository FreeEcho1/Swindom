namespace Swindom;

/// <summary>
/// 設定するウィンドウの状態
/// </summary>
public enum SettingsWindowState : int
{
    /// <summary>
    /// 変更しない
    /// </summary>
    [XmlEnum(Name = "DoNotChange")]
    DoNotChange,
    /// <summary>
    /// 通常のウィンドウ
    /// </summary>
    [XmlEnum(Name = "Normal")]
    Normal,
    /// <summary>
    /// 最大化
    /// </summary>
    [XmlEnum(Name = "Maximize")]
    Maximize,
    /// <summary>
    /// 最小化
    /// </summary>
    [XmlEnum(Name = "Minimize")]
    Minimize
}
