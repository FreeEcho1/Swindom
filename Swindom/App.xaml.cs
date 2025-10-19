namespace Swindom
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// システムトレイアイコンのウィンドウ
        /// </summary>
        private MainProcessing MainProcessing;
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
            try
            {
                foreach (string nowCommand in Environment.GetCommandLineArgs())
                {
                    if (nowCommand == ApplicationStartup.CreateTaskString)
                    {
                        ApplicationStartup.RegisterTask();
                        Environment.Exit(0);
                        return;
                    }
                    else if (nowCommand == ApplicationStartup.DeleteTaskString)
                    {
                        ApplicationStartup.DeleteTask();
                        Environment.Exit(0);
                        return;
                    }
                }

                // 多重実行の場合は終了
#if !DEBUG
                Mutex = new System.Threading.Mutex(false, "SwindomMutex");
                if (Mutex.WaitOne(0, false) == false)
                {
                    Mutex.Close();
                    Mutex.Dispose();
                    Mutex = null;
                    Environment.Exit(0);
                    return;
                }
#endif

                // Windows Forms用の高DPIの設定
                System.Windows.Forms.Application.SetHighDpiMode(System.Windows.Forms.HighDpiMode.PerMonitorV2);
            }
            catch
            {
                Environment.Exit(0);
                return;
            }

            try
            {
                base.OnStartup(e);
                ShutdownMode = ShutdownMode.OnExplicitShutdown;

                MainProcessing = new();
                MainProcessing.CloseEvent += NotifyIconWindow_CloseEvent;
            }
            catch
            {
                Shutdown();
            }
        }

        /// <summary>
        /// 「閉じる」イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NotifyIconWindow_CloseEvent(
            object? sender,
            EventArgs e
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
