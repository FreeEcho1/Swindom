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
    /// 全画面確認処理
    /// </summary>
    private CheckFullScreenProcessing? CheckFullScreenProcessing;

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
            DisposeCheckFullScreenProcessing();
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
    /// 「CheckFullScreenProcessing」を破棄
    /// </summary>
    private void DisposeCheckFullScreenProcessing()
    {
        if (CheckFullScreenProcessing != null)
        {
            CheckFullScreenProcessing.Stop();
            CheckFullScreenProcessing = null;
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
            PluginProcessing = null;
        }
    }

    /// <summary>
    /// 初期化
    /// </summary>
    public void Initialize()
    {
        SettingsTheProcessingStateOfEachFunction();
        SettingsCheckFullScreenProcessing();
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
                    SettingsCheckFullScreenProcessing();
                    SettingsTheProcessingStateOfEachFunction();
                    break;
                case ProcessingEventType.HotkeysDoNotStopFullScreenChanged:
                    SettingsTheProcessingStateOfEachFunction();
                    break;
                case ProcessingEventType.SpecifyWindowProcessingStateChanged:
                    SettingsCheckFullScreenProcessing();
                    SettingSpecifyWindowProcessing();
                    break;
                case ProcessingEventType.AllWindowProcessingStateChanged:
                    SettingsCheckFullScreenProcessing();
                    SettingAllWindowProcessing();
                    break;
                case ProcessingEventType.MagnetProcessingStateChanged:
                    SettingsCheckFullScreenProcessing();
                    SettingMagnet();
                    break;
                case ProcessingEventType.HotkeyProcessingStateChanged:
                    SettingsCheckFullScreenProcessing();
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
                case ProcessingEventType.AllProcessingChangeEnabled:
                    AllProcessingChangeEnabled();
                    break;
                case ProcessingEventType.FullScreenWindowAdditionDecisionChanged:
                    CheckFullScreenProcessing?.Start();
                    break;
            }
        }
        catch
        {
        }
    }

    /// <summary>
    /// 全ての処理状態を無効にする、無効にした処理を有効にする
    /// </summary>
    private void AllProcessingChangeEnabled()
    {
        if (ApplicationData.Settings.AllStopProcessingInformation.IsEnabled)
        {
            // 無効にした機能がある場合は有効にする
            if (ApplicationData.Settings.AllStopProcessingInformation.SpecifyWindowIsEnabled
                || ApplicationData.Settings.AllStopProcessingInformation.AllWindowIsEnabled
                || ApplicationData.Settings.AllStopProcessingInformation.MagnetIsEnabled
                || ApplicationData.Settings.AllStopProcessingInformation.HotkeyIsEnabled)
            {
                ApplicationData.Settings.AllStopProcessingInformation.IsEnabled = false;

                ApplicationData.Settings.SpecifyWindowInformation.IsEnabled = ApplicationData.Settings.AllStopProcessingInformation.SpecifyWindowIsEnabled;
                ApplicationData.Settings.AllWindowInformation.IsEnabled = ApplicationData.Settings.AllStopProcessingInformation.AllWindowIsEnabled;
                ApplicationData.Settings.MagnetInformation.IsEnabled = ApplicationData.Settings.AllStopProcessingInformation.MagnetIsEnabled;
                ApplicationData.Settings.HotkeyInformation.IsEnabled = ApplicationData.Settings.AllStopProcessingInformation.HotkeyIsEnabled;

                if (ApplicationData.Settings.SpecifyWindowInformation.IsEnabled)
                {
                    ApplicationData.EventData.DoProcessingEvent(ProcessingEventType.SpecifyWindowProcessingStateChanged);
                }
                if (ApplicationData.Settings.AllWindowInformation.IsEnabled)
                {
                    ApplicationData.EventData.DoProcessingEvent(ProcessingEventType.AllWindowProcessingStateChanged);
                }
                if (ApplicationData.Settings.MagnetInformation.IsEnabled)
                {
                    ApplicationData.EventData.DoProcessingEvent(ProcessingEventType.MagnetProcessingStateChanged);
                }
                if (ApplicationData.Settings.HotkeyInformation.IsEnabled)
                {
                    ApplicationData.EventData.DoProcessingEvent(ProcessingEventType.HotkeyProcessingStateChanged);
                }

                ApplicationData.Settings.AllStopProcessingInformation.SpecifyWindowIsEnabled = false;
                ApplicationData.Settings.AllStopProcessingInformation.AllWindowIsEnabled = false;
                ApplicationData.Settings.AllStopProcessingInformation.MagnetIsEnabled = false;
                ApplicationData.Settings.AllStopProcessingInformation.HotkeyIsEnabled = false;
            }
        }
        else
        {
            // 処理が有効なら無効にする
            if (ApplicationData.Settings.SpecifyWindowInformation.IsEnabled
                || ApplicationData.Settings.AllWindowInformation.IsEnabled
                || ApplicationData.Settings.MagnetInformation.IsEnabled
                || ApplicationData.Settings.HotkeyInformation.IsEnabled)
            {
                ApplicationData.Settings.AllStopProcessingInformation.IsEnabled = true;

                ApplicationData.Settings.AllStopProcessingInformation.SpecifyWindowIsEnabled = ApplicationData.Settings.SpecifyWindowInformation.IsEnabled;
                ApplicationData.Settings.AllStopProcessingInformation.AllWindowIsEnabled = ApplicationData.Settings.AllWindowInformation.IsEnabled;
                ApplicationData.Settings.AllStopProcessingInformation.MagnetIsEnabled = ApplicationData.Settings.MagnetInformation.IsEnabled;
                ApplicationData.Settings.AllStopProcessingInformation.HotkeyIsEnabled = ApplicationData.Settings.HotkeyInformation.IsEnabled;

                ApplicationData.Settings.SpecifyWindowInformation.IsEnabled = false;
                ApplicationData.Settings.AllWindowInformation.IsEnabled = false;
                ApplicationData.Settings.MagnetInformation.IsEnabled = false;
                ApplicationData.Settings.HotkeyInformation.IsEnabled = false;

                if (ApplicationData.Settings.AllStopProcessingInformation.SpecifyWindowIsEnabled)
                {
                    ApplicationData.EventData.DoProcessingEvent(ProcessingEventType.SpecifyWindowProcessingStateChanged);
                }
                if (ApplicationData.Settings.AllStopProcessingInformation.AllWindowIsEnabled)
                {
                    ApplicationData.EventData.DoProcessingEvent(ProcessingEventType.AllWindowProcessingStateChanged);
                }
                if (ApplicationData.Settings.AllStopProcessingInformation.MagnetIsEnabled)
                {
                    ApplicationData.EventData.DoProcessingEvent(ProcessingEventType.MagnetProcessingStateChanged);
                }
                if (ApplicationData.Settings.AllStopProcessingInformation.HotkeyIsEnabled)
                {
                    ApplicationData.EventData.DoProcessingEvent(ProcessingEventType.HotkeyProcessingStateChanged);
                }
            }
        }
    }

    /// <summary>
    /// 全ての処理停止の情報を初期化
    /// </summary>
    private void InitializeAllStopProcessingInformation()
    {
        ApplicationData.Settings.AllStopProcessingInformation.IsEnabled = false;
        ApplicationData.Settings.AllStopProcessingInformation.SpecifyWindowIsEnabled = false;
        ApplicationData.Settings.AllStopProcessingInformation.AllWindowIsEnabled= false;
        ApplicationData.Settings.AllStopProcessingInformation.MagnetIsEnabled= false;
        ApplicationData.Settings.AllStopProcessingInformation.HotkeyIsEnabled= false;
    }

    /// <summary>
    /// 全画面確認処理を設定
    /// </summary>
    private void SettingsCheckFullScreenProcessing()
    {
        if (ApplicationData.Settings.SpecifyWindowInformation.IsEnabled && ApplicationData.Settings.SpecifyWindowInformation.StopProcessingFullScreen
            || ApplicationData.Settings.AllWindowInformation.IsEnabled && ApplicationData.Settings.AllWindowInformation.StopProcessingFullScreen
            || ApplicationData.Settings.MagnetInformation.IsEnabled && ApplicationData.Settings.MagnetInformation.StopProcessingFullScreen
            || ApplicationData.Settings.HotkeyInformation.IsEnabled && ApplicationData.Settings.HotkeyInformation.StopProcessingFullScreen)
        {
            CheckFullScreenProcessing ??= new();
            CheckFullScreenProcessing.Start();
        }
        else
        {
            DisposeCheckFullScreenProcessing();
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
                InitializeAllStopProcessingInformation();
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
                InitializeAllStopProcessingInformation();
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
            InitializeAllStopProcessingInformation();
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
            InitializeAllStopProcessingInformation();
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
            InitializeAllStopProcessingInformation();
            PluginProcessing ??= new();
        }
        else
        {
            DisposePluginProcessing();
        }
    }
}
