namespace Swindom;

/// <summary>
/// プラグインのファイル情報
/// </summary>
public class PluginFileInformation
{
    /// <summary>
    /// プラグインのファイルパス
    /// </summary>
    public string Path { get; set; }
    /// <summary>
    /// プラグインの名前
    /// </summary>
    public string PluginName { get; set; }
    /// <summary>
    /// ウィンドウが存在するかの値 (存在しない「false」/存在する「true」)
    /// </summary>
    public bool IsWindowExist { get; set; }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public PluginFileInformation()
    {
        Path = "";
        PluginName = "";
    }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="path">プラグインのファイルパス</param>
    /// <param name="pluginName">プラグインの名前</param>
    public PluginFileInformation(
        string path,
        string pluginName
        )
    {
        Path = path;
        PluginName = pluginName;
    }
}
