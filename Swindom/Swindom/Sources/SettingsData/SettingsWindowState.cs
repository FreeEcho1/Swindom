namespace Swindom;

/// <summary>
/// 設定するウィンドウの状態
/// </summary>
public enum SettingsWindowState : int
{
    /// <summary>
    /// 変更しない
    /// </summary>
    DoNotChange,
    /// <summary>
    /// 通常のウィンドウ
    /// </summary>
    Normal,
    /// <summary>
    /// 最大化
    /// </summary>
    Maximize,
    /// <summary>
    /// 最小化
    /// </summary>
    Minimize
}
