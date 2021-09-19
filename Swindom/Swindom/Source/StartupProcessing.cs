namespace Swindom.Source
{
    /// <summary>
    /// スタートアップ処理
    /// </summary>
    public static class StartupProcessing
    {
        /// <summary>
        /// スタートアップのショートカットファイルのパスを取得
        /// </summary>
        private static string StartupFilePath => System.Environment.GetFolderPath(System.Environment.SpecialFolder.Startup) + System.IO.Path.DirectorySeparatorChar + Common.StartupRegistrationName + ".lnk";

        /// <summary>
        /// スタートアップに登録
        /// </summary>
        public static void RegisterStartup()
        {
            if (CheckStartup() == false)
            {
                IWshRuntimeLibrary.IWshShell_Class shell = new();
                IWshRuntimeLibrary.IWshShortcut_Class shortcut = (IWshRuntimeLibrary.IWshShortcut_Class)shell.CreateShortcut(StartupFilePath);
                shortcut.TargetPath = Processing.OwnApplicationPath;
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
                System.IO.File.Delete(StartupFilePath);
            }
        }

        /// <summary>
        /// スタートアップに登録されているか確認
        /// </summary>
        /// <returns>スタートアップに登録されているかの値 (登録されていない「false」/登録されている「true」)</returns>
        public static bool CheckStartup()
        {
            return (System.IO.File.Exists(StartupFilePath));
        }

        /// <summary>
        /// タスクスケジューラのタスクが登録されているか確認
        /// </summary>
        /// <returns>登録されているかの値 (登録されてない「false」/登録されてる「true」)</returns>
        public static bool CheckTaskSchedulerRegistered()
        {
            bool result = false;        // 結果

            try
            {
                using System.Diagnostics.Process process = new();
                process.StartInfo.FileName = GetTaskSchedulerProcessExecutablePath();
                process.StartInfo.Arguments = Common.CheckRegistered;
                process.StartInfo.UseShellExecute = true;
                process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                process.Start();
                process.WaitForExit();
                result = process.ExitCode == Common.RegisteredTaskSchedulerNumber;
            }
            catch
            {
            }

            return (result);
        }

        /// <summary>
        /// タスクスケジューラ処理の実行ファイルのパスを取得
        /// </summary>
        /// <returns>パス (ファイルが無い「null」)</returns>
        public static string GetTaskSchedulerProcessExecutablePath()
        {
            string path = Processing.GetApplicationDirectoryPath() + System.IO.Path.DirectorySeparatorChar + Common.TaskSchedulerProcessExecutable;      // タスクスケジューラ処理の実行ファイルのパス
            if (System.IO.File.Exists(path) == false)
            {
                path = null;
            }
            return (path);
        }
    }
}
