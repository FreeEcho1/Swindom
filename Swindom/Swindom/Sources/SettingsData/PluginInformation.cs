namespace Swindom;

/// <summary>
/// プラグイン情報
/// </summary>
public class PluginInformation
{
    /// <summary>
    /// 処理状態 (無効「false」/有効「true」)
    /// </summary>
    public bool Enabled;
    /// <summary>
    /// プラグイン項目情報
    /// </summary>
    public List<PluginItemInformation> Items = new();
}
