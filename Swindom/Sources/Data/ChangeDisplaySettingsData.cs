namespace Swindom;

/// <summary>
/// 設定しているディスプレイが存在しなくなった場合に設定するデータ
/// </summary>
public class ChangeDisplaySettingsData
{
    /// <summary>
    /// 修正したかの値 (「false」キャンセル/「true」修正)
    /// </summary>
    public bool IsModified { get; set; } = false;
    /// <summary>
    /// 以降も同じ設定や操作を適用するかの値
    /// </summary>
    public bool ApplySameSettingToRemaining { get; set; } = false;
    /// <summary>
    /// 自動設定するディスプレイ名
    /// </summary>
    public string AutoSettingDisplayName { get; set; } = string.Empty;
}
