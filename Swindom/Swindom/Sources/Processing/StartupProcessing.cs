namespace Swindom;

/// <summary>
/// スタートアップ処理
/// </summary>
public static class StartupProcessing
{
    /// <summary>
    /// スタートアップのショートカットファイルのパスを取得
    /// </summary>
    /// <returns>スタートアップのショートカットファイルのパス</returns>
    private static string StartupFilePath() => Environment.GetFolderPath(Environment.SpecialFolder.Startup) + Path.DirectorySeparatorChar + Common.StartupRegistrationName + ".lnk";

    /// <summary>
    /// スタートアップに登録
    /// </summary>
    public static void RegisterStartup()
    {
        if (CheckStartup() == false)
        {
            IWshRuntimeLibrary.IWshShell_Class shell = new();
            IWshRuntimeLibrary.IWshShortcut_Class shortcut = (IWshRuntimeLibrary.IWshShortcut_Class)shell.CreateShortcut(StartupFilePath());
            shortcut.TargetPath = Processing.OwnApplicationPath();
            shortcut.Save();
        }
    }

    /// <summary>
    /// スタートアップを削除
    /// </summary>
    public static void DeleteStartup()
    {
        if (CheckStartup())
        {
            File.Delete(StartupFilePath());
        }
    }

    /// <summary>
    /// スタートアップに登録されているか確認
    /// </summary>
    /// <returns>スタートアップに登録されているかの値 (登録されていない「false」/登録されている「true」)</returns>
    public static bool CheckStartup() => File.Exists(StartupFilePath());

    /// <summary>
    /// タスクスケジューラのタスクが登録されているか確認
    /// </summary>
    /// <returns>登録されているかの値 (登録されてない「false」/登録されてる「true」)</returns>
    public static bool CheckTaskSchedulerRegistered()
    {
        bool result = false;        // 結果

        try
        {
            using Process process = new();
            process.StartInfo.FileName = Processing.OwnApplicationPath();
            process.StartInfo.Arguments = Common.CheckRegistered;
            process.StartInfo.UseShellExecute = true;
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.Start();
            process.WaitForExit();
            result = process.ExitCode == Common.RegisteredTask;
        }
        catch
        {
        }

        return result;
    }
}
