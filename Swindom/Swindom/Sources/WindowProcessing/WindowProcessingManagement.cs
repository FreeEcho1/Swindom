namespace Swindom;

/// <summary>
/// ウィンドウ処理の管理
/// </summary>
public class WindowProcessingManagement : IDisposable
{
    /// <summary>
    /// Disposeが呼ばれたかの値
    /// </summary>
    private bool Disposed;
    /// <summary>
    /// 「指定ウィンドウ」処理
    /// </summary>
    public SpecifyWindowProcessing? SpecifyWindowProcessing
    {
        get;
        private set;
    }
    /// <summary>
    /// 「全てのウィンドウ」処理
    /// </summary>
    public AllWindowProcessing? AllWindowProcessing
    {
        get;
        private set;
    }
    /// <summary>
    /// 「マグネット」処理
    /// </summary>
    public MagnetProcessing? MagnetProcessing
    {
        get;
        private set;
    }
    /// <summary>
    /// 「ホットキー」処理
    /// </summary>
    public HotkeyProcessing? HotkeyProcessing
    {
        get;
        private set;
    }
    /// <summary>
    /// 「プラグイン」処理
    /// </summary>
    public PluginProcessing? PluginProcessing
    {
        get;
        private set;
    }
    /// <summary>
    /// 全画面ウィンドウ判定のタイマー
    /// </summary>
    private Timer? TimerForFullScreenJudgement;
    /// <summary>
    /// 全画面ウィンドウの判定タイマーの間隔 (ミリ秒)
    /// </summary>
    public const int FullScreenWindowDecisionTimerInterval = 10000;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public WindowProcessingManagement()
    {
        ApplicationData.EventData.ProcessingEvent += ApplicationData_ProcessingEvent;
    }

    /// <summary>
    /// デストラクタ
    /// </summary>
    ~WindowProcessingManagement()
    {
        Dispose(false);
    }

    /// <summary>
    /// Dispose
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// 非公開Dispose
    /// </summary>
    /// <param name="disposing">disposing</param>
    protected virtual void Dispose(
        bool disposing
        )
    {
        if (Disposed)
        {
            return;
        }
        if (disposing)
        {
            ApplicationData.EventData.ProcessingEvent -= ApplicationData_ProcessingEvent;
            DisposeSpecifyWindowProcessing();
            DisposeAllWindowProcessing();
            DisposeMagnetProcessing();
            DisposeHotkeyProcessing();
            DisposeTimerForFullScreenJudgement();
            DisposePluginProcessing();
        }
        Disposed = true;
    }

    /// <summary>
    /// 「SpecifyWindowProcessing」を破棄
    /// </summary>
    private void DisposeSpecifyWindowProcessing()
    {
        if (SpecifyWindowProcessing != null)
        {
            SpecifyWindowProcessing.Dispose();
            SpecifyWindowProcessing = null;
        }
    }

    /// <summary>
    /// 「AllWindowProcessing」を破棄
    /// </summary>
    private void DisposeAllWindowProcessing()
    {
        if (AllWindowProcessing != null)
        {
            AllWindowProcessing.Dispose();
            AllWindowProcessing = null;
        }
    }

    /// <summary>
    /// 「MagnetProcessing」を破棄
    /// </summary>
    private void DisposeMagnetProcessing()
    {
        if (MagnetProcessing != null)
        {
            MagnetProcessing.Dispose();
            MagnetProcessing = null;
        }
    }

    /// <summary>
    /// 「HotkeyProcessing」を破棄
    /// </summary>
    private void DisposeHotkeyProcessing()
    {
        if (HotkeyProcessing != null)
        {
            HotkeyProcessing.Dispose();
            HotkeyProcessing = null;
        }
    }

    /// <summary>
    /// 「TimerForFullScreenJudgement」を破棄
    /// </summary>
    private void DisposeTimerForFullScreenJudgement()
    {
        if (TimerForFullScreenJudgement != null)
        {
            TimerForFullScreenJudgement.Stop();
            TimerForFullScreenJudgement.Dispose();
            TimerForFullScreenJudgement = null;
        }
    }

    /// <summary>
    /// 「PluginProcessing」を破棄
    /// </summary>
    private void DisposePluginProcessing()
    {
        if (PluginProcessing != null)
        {
            PluginProcessing.Dispose();
        }
    }

    /// <summary>
    /// 初期化
    /// </summary>
    public void Initialize()
    {
        SettingsTheProcessingStateOfEachFunction();
        SettingTimerFullScreen();
    }

    /// <summary>
    /// 「処理」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ApplicationData_ProcessingEvent(
        object? sender,
        ProcessingEventArgs e
        )
    {
        try
        {
            switch (e.ProcessingEventType)
            {
                case ProcessingEventType.StopWhenWindowIsFullScreenChanged:
                    SettingTimerFullScreen();
                    SettingsTheProcessingStateOfEachFunction();
                    break;
                case ProcessingEventType.HotkeysDoNotStopFullScreenChanged:
                    SettingsTheProcessingStateOfEachFunction();
                    break;
                case ProcessingEventType.SpecifyWindowProcessingStateChanged:
                    SettingSpecifyWindowProcessing();
                    break;
                case ProcessingEventType.AllWindowProcessingStateChanged:
                    SettingAllWindowProcessing();
                    break;
                case ProcessingEventType.MagnetProcessingStateChanged:
                    SettingMagnet();
                    break;
                case ProcessingEventType.HotkeyProcessingStateChanged:
                    SettingHotkey();
                    break;
                case ProcessingEventType.PluginProcessingStateChanged:
                    SettingPlugin();
                    break;
                case ProcessingEventType.SpecifyWindowBatchProcessing:
                    SpecifyWindowProcessing.DecisionAndWindowProcessing(false);
                    break;
                case ProcessingEventType.SpecifyWindowOnlyActiveWindow:
                    SpecifyWindowProcessing.DecisionAndWindowProcessing(true);
                    break;
                case ProcessingEventType.FullScreenWindowShowClose:
                    SettingsTheProcessingStateOfEachFunction();
                    break;
            }
        }
        catch
        {
        }
    }

    /// <summary>
    /// 全画面ウィンドウ判定のタイマーを設定
    /// </summary>
    private void SettingTimerFullScreen()
    {
        if (ApplicationData.Settings.SpecifyWindowInformation.Enabled && ApplicationData.Settings.SpecifyWindowInformation.StopProcessingFullScreen
            || ApplicationData.Settings.AllWindowInformation.Enabled && ApplicationData.Settings.AllWindowInformation.StopProcessingFullScreen
            || ApplicationData.Settings.MagnetInformation.Enabled && ApplicationData.Settings.MagnetInformation.StopProcessingFullScreen
            || ApplicationData.Settings.HotkeyInformation.Enabled && ApplicationData.Settings.HotkeyInformation.StopProcessingFullScreen)
        {
            if (TimerForFullScreenJudgement == null)
            {
                VariousWindowProcessing.CheckFullScreenWindow(null, true);
                TimerForFullScreenJudgement = new()
                {
                    Interval = FullScreenWindowDecisionTimerInterval
                };
                TimerForFullScreenJudgement.Elapsed += (s, e) =>
                {
                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        VariousWindowProcessing.CheckFullScreenWindow(null, true);
                    }));
                };
            }
            TimerForFullScreenJudgement.Start();
        }
        else
        {
            DisposeTimerForFullScreenJudgement();
            ApplicationData.FullScreenExists = false;
        }
    }

    /// <summary>
    /// 各機能の処理状態を設定
    /// </summary>
    private void SettingsTheProcessingStateOfEachFunction()
    {
        SettingSpecifyWindowProcessing();
        SettingAllWindowProcessing();
        SettingMagnet();
        SettingHotkey();
        SettingPlugin();
    }

    /// <summary>
    /// 「指定ウィンドウ」の設定
    /// </summary>
    private void SettingSpecifyWindowProcessing()
    {
        if (SpecifyWindowProcessing.CheckIfTheProcessingIsValid())
        {
            if (SpecifyWindowProcessing == null)
            {
                SpecifyWindowProcessing = new();
            }
            else
            {
                SpecifyWindowProcessing.ProcessingSettings();
            }
        }
        else
        {
            DisposeSpecifyWindowProcessing();
        }
    }

    /// <summary>
    /// 「全てのウィンドウ」の設定
    /// </summary>
    private void SettingAllWindowProcessing()
    {
        if (AllWindowProcessing.CheckIfTheProcessingIsValid())
        {
            if (AllWindowProcessing == null)
            {
                AllWindowProcessing = new();
            }
            else
            {
                AllWindowProcessing.ProcessingSettings();
            }
        }
        else
        {
            DisposeAllWindowProcessing();
        }
    }

    /// <summary>
    /// 「マグネット」の設定
    /// </summary>
    private void SettingMagnet()
    {
        if (MagnetProcessing.CheckIfTheProcessingIsValid())
        {
            MagnetProcessing ??= new();
        }
        else
        {
            DisposeMagnetProcessing();
        }
    }

    /// <summary>
    /// 「ホットキー」の設定
    /// </summary>
    private void SettingHotkey()
    {
        if (HotkeyProcessing.CheckIfTheProcessingIsValid())
        {
            HotkeyProcessing ??= new();
        }
        else
        {
            DisposeHotkeyProcessing();
        }
    }

    /// <summary>
    /// 「プラグイン」の設定
    /// </summary>
    private void SettingPlugin()
    {
        if (PluginProcessing.CheckIfTheProcessingIsValid())
        {
            PluginProcessing ??= new();
        }
    }
}
