namespace Swindom
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// システムトレイアイコンの処理
        /// </summary>
        NotifyIconProcessing NotifyIconProcessing;
        /// <summary>
        /// 多重起動を防止する為のミューテックス
        /// </summary>
        private System.Threading.Mutex? Mutex = null;

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
                    case StartupProcessing.CreateTaskString:
                        taskCheck = true;
                        StartupProcessing.RegisterTask();
                        Shutdown();
                        Environment.Exit(0);
                        return;
                    case StartupProcessing.DeleteTaskString:
                        taskCheck = true;
                        StartupProcessing.DeleteTask();
                        Shutdown();
                        Environment.Exit(0);
                        return;
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

                NotifyIconProcessing = new();
                NotifyIconProcessing.CloseEvent += NoWindow_CloseEvent;
            }
        }

        /// <summary>
        /// 「閉じる」イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NoWindow_CloseEvent(
            object? sender,
            CloseEventArgs e
            )
        {
            Shutdown();
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
            Shutdown();
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
            }
            catch
            {
            }
        }
    }
}
