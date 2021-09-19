namespace Swindom
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {
        /// <summary>
        /// NotifyIconWindow
        /// </summary>
        Source.Window.NotifyIconWindow NotifyIconWindow;
        /// <summary>
        /// 多重起動を防止する為のミューテックス
        /// </summary>
        private System.Threading.Mutex Mutex;

        /// <summary>
        /// /System.Windows.Application.Startupイベント
        /// </summary>
        /// <param name="e"></param>
        protected override void OnStartup(
            System.Windows.StartupEventArgs e
            )
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
            ShutdownMode = System.Windows.ShutdownMode.OnExplicitShutdown;
            NotifyIconWindow = new Source.Window.NotifyIconWindow();
            NotifyIconWindow.CloseEvent += NotifyIconWindow_CloseEvent;
            NotifyIconWindow.Show();
        }

        /// <summary>
        /// NotifyIconWindowが閉じられた
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NotifyIconWindow_CloseEvent(
            object sender,
            Source.CloseEventArgs e
            )
        {
            System.Windows.Application.Current.Shutdown();
        }

        /// <summary>
        /// System.Windows.Application.Exitイベント
        /// </summary>
        /// <param name="e"></param>
        protected override void OnExit(
            System.Windows.ExitEventArgs e
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
