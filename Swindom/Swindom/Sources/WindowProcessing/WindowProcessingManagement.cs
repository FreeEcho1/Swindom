namespace Swindom.Sources.WindowProcessing;

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
    /// 指定ウィンドウでウィンドウ処理
    /// </summary>
    public SpecifyWindowProcessing? SpecifyWindowProcessing
    {
        get;
        private set;
    }
    /// <summary>
    /// マグネット
    /// </summary>
    public MagnetProcessing? Magnet
    {
        get;
        private set;
    }
    /// <summary>
    /// ホットキー
    /// </summary>
    public HotkeyProcessing? Hotkey
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
            DisposeMagnet();
            DisposeHotkey();
            DisposeTimerForFullScreenJudgement();
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
    /// 「Magnet」を破棄
    /// </summary>
    private void DisposeMagnet()
    {
        if (Magnet != null)
        {
            Magnet.Dispose();
            Magnet = null;
        }
    }

    /// <summary>
    /// 「Hotkey」を破棄
    /// </summary>
    private void DisposeHotkey()
    {
        if (Hotkey != null)
        {
            Hotkey.Dispose();
            Hotkey = null;
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
                    SettingEventWindowProcessing();
                    break;
                case ProcessingEventType.MagnetProcessingStateChanged:
                    SettingMagnet();
                    break;
                case ProcessingEventType.HotkeyValidState:
                    SettingHotkey();
                    break;
                case ProcessingEventType.BatchProcessingOfSpecifyWindow:
                    SpecifyWindowProcessing.DecisionAndWindowProcessing(false);
                    break;
                case ProcessingEventType.OnlyActiveWindowSpecifyWindow:
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
            || ApplicationData.Settings.MagnetInformation.Enabled && ApplicationData.Settings.MagnetInformation.StopProcessingFullScreen
            || ApplicationData.Settings.HotkeyInformation.Enabled && ApplicationData.Settings.HotkeyInformation.StopProcessingFullScreen)
        {
            if (TimerForFullScreenJudgement == null)
            {
                VariousWindowProcessing.CheckFullScreenWindow(null);
                TimerForFullScreenJudgement = new()
                {
                    Interval = FullScreenWindowDecisionTimerInterval
                };
                TimerForFullScreenJudgement.Elapsed += (s, e) =>
                {
                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        VariousWindowProcessing.CheckFullScreenWindow(null);
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
        SettingEventWindowProcessing();
        SettingMagnet();
        SettingHotkey();
    }

    /// <summary>
    /// 「指定ウィンドウ」の設定
    /// </summary>
    private void SettingEventWindowProcessing()
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
    /// 「マグネット」の設定
    /// </summary>
    private void SettingMagnet()
    {
        if (MagnetProcessing.CheckIfTheProcessingIsValid())
        {
            Magnet ??= new();
        }
        else
        {
            DisposeMagnet();
        }
    }

    /// <summary>
    /// 「ホットキー」の設定
    /// </summary>
    private void SettingHotkey()
    {
        if (HotkeyProcessing.CheckIfTheProcessingIsValid())
        {
            Hotkey ??= new();
        }
        else
        {
            DisposeHotkey();
        }
    }
}
