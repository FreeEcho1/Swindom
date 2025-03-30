namespace Swindom;

/// <summary>
/// プラグインの値
/// </summary>
public static class PluginValue
{
    /// <summary>
    /// プラグインディレクトリ名
    /// </summary>
    public static string PluginsDirectoryName { get; } = "Plugins";
    /// <summary>
    /// プラグインパスを取得するexeファイル名
    /// </summary>
    public static string GetPluginPathsFileName { get; } = "GetPluginInformation.exe";
    /// <summary>
    /// プラグインを実行するexeファイル名
    /// </summary>
    public static string RunPluginFileName { get; } = "RunPlugin.exe";
    /// <summary>
    /// リスタート用のバッチファイル名
    /// </summary>
    public static string RestartBatchFileName { get; } = "RestartBat.bat";
}
