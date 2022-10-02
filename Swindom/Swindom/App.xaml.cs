using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Swindom
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// NotifyIconWindow
        /// </summary>
        NotifyIconWindow? NotifyIconWindow;
        /// <summary>
        /// 多重起動を防止する為のミューテックス
        /// </summary>
        private System.Threading.Mutex? Mutex = null;
        /// <summary>
        /// タスク登録確認の「ExitCode」 (確認ではない「0」/登録されている「Common.RegisteredTask」/登録されていない「Common.NotRegisteredTask」)
        /// </summary>
        private int CheckTaskRegisteredExitCode = 0;

        /// <summary>
        /// /Application.Startupイベント
        /// </summary>
        /// <param name="e"></param>
        protected override void OnStartup(
            StartupEventArgs e
            )
        {
            bool taskCheck = false;     // タスクスケジューラ処理かの値
            foreach (string nowCommand in Environment.GetCommandLineArgs())
            {
                switch (nowCommand)
                {
                    case Common.CreateTask:
                        taskCheck = true;
                        TaskSchedulerProcessing.CreateTask();
                        Current.Shutdown();
                        break;
                    case Common.DeleteTask:
                        taskCheck = true;
                        TaskSchedulerProcessing.DeleteTask();
                        Current.Shutdown();
                        break;
                    case Common.CheckRegistered:
                        taskCheck = true;
                        CheckTaskRegisteredExitCode = TaskSchedulerProcessing.CheckRegistered() ? Common.RegisteredTask : Common.NotRegisteredTask;
                        Current.Shutdown();
                        break;
                }
            }

            if (taskCheck == false)
            {
                // 多重実行の場合は終了
                try
                {
#if !DEBUG
                Mutex = new System.Threading.Mutex(false, "SwindomMutex");
                if (Mutex.WaitOne(0, false) == false)
                {
                    Mutex.Close();
                    Mutex.Dispose();
                    Mutex = null;
                    Shutdown();
                    return;
                }
#endif
                }
                catch
                {
                }

                base.OnStartup(e);
                ShutdownMode = ShutdownMode.OnExplicitShutdown;
                NotifyIconWindow = new NotifyIconWindow();
                NotifyIconWindow.CloseEvent += NotifyIconWindow_CloseEvent;
                NotifyIconWindow.Show();
            }
        }

        /// <summary>
        /// NotifyIconWindowが閉じられた
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NotifyIconWindow_CloseEvent(
            object? sender,
            CloseEventArgs e
            )
        {
            Current.Shutdown();
        }

        /// <summary>
        /// Application.Exitイベント
        /// </summary>
        /// <param name="e"></param>
        protected override void OnExit(
            ExitEventArgs e
            )
        {
            try
            {
                base.OnExit(e);
                if (Mutex != null)
                {
                    Mutex.ReleaseMutex();
                    Mutex.Close();
                    Mutex = null;
                }

                if (CheckTaskRegisteredExitCode != 0)
                {
                    Environment.ExitCode = CheckTaskRegisteredExitCode;
                }
            }
            catch
            {
            }
        }
    }
}
