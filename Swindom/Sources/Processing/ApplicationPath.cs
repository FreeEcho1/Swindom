namespace Swindom;

/// <summary>
/// アプリケーションのパス処理
/// </summary>
public static class ApplicationPath
{
    /// <summary>
    /// アプリケーションのパスを取得
    /// </summary>
    /// <returns>アプリケーションのパス</returns>
    public static string OwnApplicationPath() => Path.GetFullPath(Process.GetCurrentProcess().MainModule?.FileName ?? ApplicationValue.ApplicationFileName);

    /// <summary>
    /// アプリケーションのディレクトリを取得
    /// </summary>
    /// <param name="getProjectFileDirectory">プロジェクトファイルのディレクトリを取得するかの値 (デバッグのみ) (いいえ「false」/はい「true」)</param>
    /// <returns>アプリケーションのディレクトリ</returns>
    public static string GetApplicationDirectory(
        bool getProjectFileDirectory = false
        )
    {
        string path = Path.GetDirectoryName(OwnApplicationPath()) ?? "";
#if DEBUG
        if (getProjectFileDirectory)
        {
            path += Path.DirectorySeparatorChar + ".." + Path.DirectorySeparatorChar + ".." + Path.DirectorySeparatorChar + ".." + Path.DirectorySeparatorChar;
        }
#endif
        return path;
    }

    /// <summary>
    /// アプリケーションのファイル名を取得
    /// </summary>
    /// <returns>アプリケーションのファイル名</returns>
    public static string GetApplicationFileName() => Path.GetFileName(OwnApplicationPath());

    /// <summary>
    /// 言語ディレクトリを取得
    /// </summary>
    /// <returns>言語ディレクトリのパス</returns>
    public static string GetLanguageDirectory() => GetApplicationDirectory(true) + Path.DirectorySeparatorChar + LanguageValue.LanguagesDirectoryName + Path.DirectorySeparatorChar;

    /// <summary>
    /// プラグインのディレクトリを取得
    /// </summary>
    /// <returns>プラグインのディレクトリ</returns>
    public static string GetPluginDirectory() => GetApplicationDirectory() + Path.DirectorySeparatorChar + PluginValue.PluginsDirectoryName;

    /// <summary>
    /// インストールされているか確認
    /// </summary>
    /// <returns>インストール状態 (インストールされていない「false」/インストールされている「true」)</returns>
    public static bool CheckInstalled()
    {
        bool result = false;        // インストール状態

        try
        {
            using Microsoft.Win32.RegistryKey? registryKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE" + Path.DirectorySeparatorChar + "Microsoft" + Path.DirectorySeparatorChar  + "Windows" + Path.DirectorySeparatorChar  + "CurrentVersion" + Path.DirectorySeparatorChar  + "Uninstall" + Path.DirectorySeparatorChar + ApplicationValue.ApplicationName);
            if (registryKey != null)
            {
                result = true;
            }
        }
        catch
        {
        }

        return result;
    }
}
