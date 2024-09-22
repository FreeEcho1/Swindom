namespace Swindom;

/// <summary>
/// アプリケーションのスタートアップ処理
/// </summary>
public static class ApplicationStartup
{
    /// <summary>
    /// 登録情報の「Author」
    /// </summary>
    public static readonly string RegistrationInformationAuthor = "FreeEcho";
    /// <summary>
    /// タスクを作成 (識別する文字列)
    /// </summary>
    public static readonly string CreateTaskString = "CreateTask";
    /// <summary>
    /// タスクを削除 (識別する文字列)
    /// </summary>
    public static readonly string DeleteTaskString = "DeleteTask";

    /// <summary>
    /// スタートアップのショートカットファイルのパスを取得
    /// </summary>
    /// <returns>スタートアップのショートカットファイルのパス</returns>
    private static string GetStartupFilePath() => Environment.GetFolderPath(Environment.SpecialFolder.Startup) + Path.DirectorySeparatorChar + ApplicationValue.ApplicationName + ".lnk";

    /// <summary>
    /// スタートアップに登録
    /// </summary>
    public static void RegisterStartup()
    {
        if (CheckStartup() == false)
        {
            IWshRuntimeLibrary.IWshShell_Class shell = new();
            IWshRuntimeLibrary.IWshShortcut_Class shortcut = (IWshRuntimeLibrary.IWshShortcut_Class)shell.CreateShortcut(GetStartupFilePath());
            shortcut.TargetPath = ApplicationPath.OwnApplicationPath();
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
            File.Delete(GetStartupFilePath());
        }
    }

    /// <summary>
    /// スタートアップに登録されているか確認
    /// </summary>
    /// <returns>スタートアップに登録されているかの値 (登録されていない「false」/登録されている「true」)</returns>
    public static bool CheckStartup() => File.Exists(GetStartupFilePath());

    /// <summary>
    /// タスクスケジューラにタスクを登録
    /// </summary>
    public static void RegisterTask()
    {
        TaskScheduler.ITaskService? taskService = null;

        try
        {
            taskService = new TaskScheduler.TaskScheduler();
            taskService.Connect(null, null, null, null);

            TaskScheduler.ITaskDefinition taskDefinition = taskService.NewTask(0);

            // 全般
            TaskScheduler.IRegistrationInfo registrationInfo = taskDefinition.RegistrationInfo;
            registrationInfo.Author = RegistrationInformationAuthor;
            TaskScheduler.IPrincipal principal = taskDefinition.Principal;
            principal.LogonType = TaskScheduler._TASK_LOGON_TYPE.TASK_LOGON_INTERACTIVE_TOKEN;
            principal.RunLevel = TaskScheduler._TASK_RUNLEVEL.TASK_RUNLEVEL_HIGHEST;

            // トリガー
            TaskScheduler.ITriggerCollection triggerCollection = taskDefinition.Triggers;
            TaskScheduler.ILogonTrigger logonTrigger = (TaskScheduler.ILogonTrigger)triggerCollection.Create(TaskScheduler._TASK_TRIGGER_TYPE2.TASK_TRIGGER_LOGON);

            // 操作
            TaskScheduler.IActionCollection actionCollection = taskDefinition.Actions;
            TaskScheduler.IExecAction execAction = (TaskScheduler.IExecAction)actionCollection.Create(TaskScheduler._TASK_ACTION_TYPE.TASK_ACTION_EXEC);
            execAction.Path = ApplicationPath.OwnApplicationPath();

            // 条件
            TaskScheduler.ITaskSettings taskSettings = taskDefinition.Settings;
            taskSettings.DisallowStartIfOnBatteries = false;
            taskSettings.StopIfGoingOnBatteries = false;

            // 設定
            taskSettings.ExecutionTimeLimit = "P0D";

            TaskScheduler.ITaskFolder rootFolder = taskService.GetFolder(Path.DirectorySeparatorChar.ToString());
            try
            {
                rootFolder.RegisterTaskDefinition(ApplicationValue.ApplicationName, taskDefinition, (int)TaskScheduler._TASK_CREATION.TASK_CREATE_OR_UPDATE, null, null, TaskScheduler._TASK_LOGON_TYPE.TASK_LOGON_NONE, null);
            }
            finally
            {
                _ = Marshal.ReleaseComObject(rootFolder);
            }
        }
        finally
        {
            if (taskService != null)
            {
                _ = Marshal.ReleaseComObject(taskService);
            }
        }
    }

    /// <summary>
    /// タスクスケジューラのタスクを削除
    /// </summary>
    public static void DeleteTask()
    {
        TaskScheduler.ITaskService? taskService = null;

        try
        {
            taskService = new TaskScheduler.TaskScheduler();
            taskService.Connect(null, null, null, null);

            TaskScheduler.ITaskFolder rootFolder = taskService.GetFolder(Path.DirectorySeparatorChar.ToString());
            try
            {
                rootFolder.DeleteTask(ApplicationValue.ApplicationName, 0);
            }
            finally
            {
                Marshal.ReleaseComObject(rootFolder);
            }
        }
        finally
        {
            if (taskService != null)
            {
                Marshal.ReleaseComObject(taskService);
            }
        }
    }

    /// <summary>
    /// タスクが登録されているか確認
    /// </summary>
    /// <returns>登録されているかの値 (登録されていない「false」/登録されている「true」)</returns>
    public static bool CheckTaskRegistered()
    {
        bool result = false;        // 結果
        TaskScheduler.ITaskService? taskService = null;

        try
        {
            taskService = new TaskScheduler.TaskScheduler();
            taskService.Connect(null, null, null, null);

            TaskScheduler.ITaskFolder rootFolder = taskService.GetFolder(Path.DirectorySeparatorChar.ToString());
            try
            {
                TaskScheduler.IRegisteredTaskCollection taskFolderCollection = rootFolder.GetTasks(0);

                foreach (TaskScheduler.IRegisteredTask nowTaskFolder in taskFolderCollection)
                {
                    if (nowTaskFolder.Name == ApplicationValue.ApplicationName)
                    {
                        result = true;
                        break;
                    }
                }
            }
            finally
            {
                Marshal.ReleaseComObject(rootFolder);
            }
        }
        finally
        {
            if (taskService != null)
            {
                Marshal.ReleaseComObject(taskService);
            }
        }

        return (result);
    }
}
