namespace Swindom;

/// <summary>
/// タスクスケジューラーの処理
/// </summary>
internal class TaskSchedulerProcessing
{
    /// <summary>
    /// タスクを作成
    /// </summary>
    public static void CreateTask()
    {
        TaskScheduler.ITaskService? taskService = null;

        try
        {
            taskService = new TaskScheduler.TaskScheduler();
            taskService.Connect(null, null, null, null);

            TaskScheduler.ITaskDefinition taskDefinition = taskService.NewTask(0);

            // 全般
            TaskScheduler.IRegistrationInfo registrationInfo = taskDefinition.RegistrationInfo;
            registrationInfo.Author = Common.RegistrationInformationAuthor;
            TaskScheduler.IPrincipal principal = taskDefinition.Principal;
            principal.LogonType = TaskScheduler._TASK_LOGON_TYPE.TASK_LOGON_INTERACTIVE_TOKEN;
            principal.RunLevel = TaskScheduler._TASK_RUNLEVEL.TASK_RUNLEVEL_HIGHEST;

            // トリガー
            TaskScheduler.ITriggerCollection triggerCollection = taskDefinition.Triggers;
            TaskScheduler.ILogonTrigger logonTrigger = (TaskScheduler.ILogonTrigger)triggerCollection.Create(TaskScheduler._TASK_TRIGGER_TYPE2.TASK_TRIGGER_LOGON);

            // 操作
            TaskScheduler.IActionCollection actionCollection = taskDefinition.Actions;
            TaskScheduler.IExecAction execAction = (TaskScheduler.IExecAction)actionCollection.Create(TaskScheduler._TASK_ACTION_TYPE.TASK_ACTION_EXEC);
            execAction.Path = Path.GetDirectoryName(Path.GetFullPath(System.Environment.GetCommandLineArgs()[0])) + Path.DirectorySeparatorChar + Common.ApplicationFileNameToRegister;

            // 条件
            TaskScheduler.ITaskSettings taskSettings = taskDefinition.Settings;
            taskSettings.DisallowStartIfOnBatteries = false;
            taskSettings.StopIfGoingOnBatteries = false;

            // 設定
            taskSettings.ExecutionTimeLimit = "P0D";

            TaskScheduler.ITaskFolder rootFolder = taskService.GetFolder(Path.DirectorySeparatorChar.ToString());
            try
            {
                rootFolder.RegisterTaskDefinition(Common.TaskSchedulerFolderName, taskDefinition, (int)TaskScheduler._TASK_CREATION.TASK_CREATE_OR_UPDATE, null, null, TaskScheduler._TASK_LOGON_TYPE.TASK_LOGON_NONE, null);
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
    /// タスクを削除
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
                rootFolder.DeleteTask(Common.TaskSchedulerFolderName, 0);
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
    /// <returns>登録されているかの値 (登録されてない「false」/登録されてる「true」)</returns>
    public static bool CheckRegistered()
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
                    if (nowTaskFolder.Name == Common.TaskSchedulerFolderName)
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
