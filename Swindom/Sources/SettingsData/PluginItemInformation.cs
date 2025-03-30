namespace Swindom;

/// <summary>
/// プラグイン項目情報
/// </summary>
public class PluginItemInformation
{
    /// <summary>
    /// プラグインのファイル名
    /// </summary>
    public string PluginFileName { get; set; } = "";

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public PluginItemInformation()
    {
        PluginFileName = "";
    }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="item">プラグイン項目情報</param>
    public PluginItemInformation(
        PluginItemInformation item
        )
    {
        PluginFileName = item.PluginFileName;
    }
}
