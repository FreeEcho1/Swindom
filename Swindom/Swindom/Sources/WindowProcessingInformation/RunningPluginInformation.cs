namespace Swindom;

/// <summary>
/// 実行中のプラグイン情報
/// </summary>
public class RunningPluginInformation
{
    /// <summary>
    /// プラグインのファイルパス
    /// </summary>
    public string Path { get; set; } = "";
    /// <summary>
    /// IPlugin
    /// </summary>
    public IPlugin? IPlugin { get; set; }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <exception cref="Exception"></exception>
    public RunningPluginInformation()
    {
        throw new Exception("Do not use. - " + GetType().Name + "()");
    }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="path">プラグインのファイルパス</param>
    /// <param name="iPlugin">IPlugin</param>
    public RunningPluginInformation(
        string path,
        IPlugin? iPlugin
        )
    {
        Path = path;
        IPlugin = iPlugin;
    }

    /// <summary>
    /// プラグインを停止
    /// </summary>
    public void StopPlugin()
    {
        IPlugin?.Destruction();
    }
}
