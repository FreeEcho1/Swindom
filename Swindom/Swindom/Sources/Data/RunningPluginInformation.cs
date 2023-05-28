namespace Swindom;

/// <summary>
/// 実行中のプラグイン情報
/// </summary>
public class RunningPluginInformation
{
    /// <summary>
    /// プラグインのファイルパス
    /// </summary>
    public string Path = "";
    /// <summary>
    /// IPlugin
    /// </summary>
    public IPlugin IPlugin;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <exception cref="Exception"></exception>
    public RunningPluginInformation()
    {
        throw new Exception("Do not use. - RunningPluginInformation()");
    }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="path">プラグインのファイルパス</param>
    /// <param name="iPlugin">IPlugin</param>
    public RunningPluginInformation(
        string path,
        IPlugin iPlugin)
    {
        Path = path;
        IPlugin = iPlugin;
    }
}
