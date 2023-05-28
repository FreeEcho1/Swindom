namespace Swindom;

/// <summary>
/// 「プラグイン」処理
/// </summary>
public class PluginProcessing
{
    /// <summary>
    /// Disposeが呼ばれたかの値
    /// </summary>
    private bool Disposed;
    /// <summary>
    /// 実行中のプラグイン情報
    /// </summary>
    public List<RunningPluginInformation> RunningPluginInformation = new();
    /// <summary>
    /// ウィンドウイベント
    /// </summary>
    private FreeEcho.FEWindowEvent.WindowEvent? WindowEvent;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public PluginProcessing()
    {
        RunPlugins();
    }

    /// <summary>
    /// デストラクタ
    /// </summary>
    ~PluginProcessing()
    {
        Dispose(false);
    }

    /// <summary>
    /// Dispose
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
    }

    /// <summary>
    /// 非公開Dispose
    /// </summary>
    /// <param name="disposing"></param>
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
            DisposeWindowEvent();
            StopAllPlugins();
        }
    }

    /// <summary>
    /// ウィンドウイベントを破棄
    /// </summary>
    private void DisposeWindowEvent()
    {
        if (WindowEvent != null)
        {
            WindowEvent.Dispose();
            WindowEvent = null;
        }
    }

    /// <summary>
    /// 処理が有効か確認
    /// </summary>
    /// <returns>処理が有効かの値 (無効「false」/有効「true」)</returns>
    public static bool CheckIfTheProcessingIsValid() => ApplicationData.Settings.PluginInformation.Enabled;

    /// <summary>
    /// プラグインを実行
    /// </summary>
    public void RunPlugins()
    {
        string settingDirectory = SettingFileProcessing.GetSettingsDirectory();     // 設定ディレクトリ
        if (string.IsNullOrEmpty(settingDirectory))
        {
            return;
        }
        List<PluginFileInformation> pluginFileInformation = GetPluginsInformation();     // プラグインのファイル情報
        string? ipluginTypeName = typeof(IPlugin).FullName;       // プラグインインターフェースの型名
        if (ipluginTypeName == null)
        {
            return;
        }

        foreach (PluginFileInformation nowPFI in pluginFileInformation)
        {
            bool checkRunning = false;      // プラグインが実行中かの値
            // プラグインが実行中か確認
            foreach (RunningPluginInformation nowRPI in RunningPluginInformation)
            {
                if (nowRPI.Path == nowPFI.Path)
                {
                    checkRunning = true;
                    break;
                }
            }

            if (checkRunning == false)
            {
                foreach (PluginItemInformation nowPII in ApplicationData.Settings.PluginInformation.Items)
                {
                    if (nowPII.PluginFileName == Path.GetFileNameWithoutExtension(nowPFI.Path))
                    {
                        try
                        {
                            System.Reflection.Assembly assembly = System.Reflection.Assembly.LoadFrom(nowPFI.Path);

                            foreach (Type nowType in assembly.GetTypes())
                            {
                                // プラグインかの確認
                                if (nowType.IsClass && nowType.IsPublic
                                    && nowType.IsAbstract == false
                                    && nowType.GetInterface(ipluginTypeName) != null
                                    && string.IsNullOrEmpty(nowType.FullName) == false)
                                {
                                    object? objectInterface = assembly.CreateInstance(nowType.FullName);
                                    if (objectInterface == null)
                                    {
                                        break;
                                    }
                                    RunningPluginInformation newItem = new(nowPFI.Path, (IPlugin)objectInterface);
                                    if (newItem.IPlugin == null)
                                    {
                                        break;
                                    }
                                    newItem.IPlugin.Initialize(settingDirectory);
                                    newItem.IPlugin.ChangeGetWindowEventTypeData.ChangeEventType += ChangeGetWindowEventTypeData_ChangeEventType;
                                    RunningPluginInformation.Add(newItem);
                                    break;
                                }
                            }
                        }
                        catch
                        {
                        }
                        break;
                    }
                }
            }
        }

        SettingsWindowEvent();
    }

    /// <summary>
    /// ウィンドウイベントを設定
    /// </summary>
    private void SettingsWindowEvent()
    {
        FreeEcho.FEWindowEvent.HookWindowEventType eventType = 0;       // イベントの種類

        foreach (RunningPluginInformation nowItem in RunningPluginInformation)
        {
            eventType |= (FreeEcho.FEWindowEvent.HookWindowEventType)nowItem.IPlugin.GetWindowEventType;
        }

        if (eventType == 0)
        {
            DisposeWindowEvent();
        }
        else
        {
            if (WindowEvent == null)
            {
                WindowEvent = new();
                WindowEvent.WindowEventOccurrence += WindowEvent_WindowEventOccurrence;
            }
            else
            {
                WindowEvent.Unhook();
            }
            WindowEvent.Hook(eventType);
        }
    }

    /// <summary>
    /// 「取得するウィンドウイベントの種類」の「ChangeEventType」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ChangeGetWindowEventTypeData_ChangeEventType(
        object? sender,
        ChangeGetWindowEventTypeArgs e
        )
    {
        try
        {
            SettingsWindowEvent();
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「ウィンドウイベント」の「WindowEventOccurrence」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void WindowEvent_WindowEventOccurrence(
        object? sender,
        FreeEcho.FEWindowEvent.WindowEventArgs e
        )
    {
        try
        {
            IntPtr windowHwnd = FreeEcho.FEWindowEvent.WindowEvent.GetWindowHwnd(e.Hwnd, e.EventType);        // ウィンドウのハンドル
            if (FreeEcho.FEWindowEvent.WindowEvent.ConfirmWindowVisible(windowHwnd, e.EventType) == false)
            {
                windowHwnd = IntPtr.Zero;
            }

            foreach (RunningPluginInformation nowItem in RunningPluginInformation)
            {
                if (CheckPluginRegisterEvent(e, nowItem))
                {
                    IntPtr hwnd = nowItem.IPlugin.IsWindowOnlyEventProcessing ? windowHwnd : e.Hwnd;        // ウィンドウハンドル

                    if (hwnd != IntPtr.Zero)
                    {
                        nowItem.IPlugin.EventProcessingData.DoEventProcessing(hwnd, e.EventType);
                    }
                }
            }
        }
        catch
        {
        }
    }

    /// <summary>
    /// プラグインが登録しているイベントか確認
    /// </summary>
    /// <param name="e">ウィンドウイベントのデータ</param>
    /// <param name="runningPluginInformation">実行中のプラグイン情報</param>
    /// <returns></returns>
    private bool CheckPluginRegisterEvent(
        FreeEcho.FEWindowEvent.WindowEventArgs e,
        RunningPluginInformation runningPluginInformation
        )
    {
        bool result = false;        // 結果
        FreeEcho.FEWindowEvent.HookWindowEventType type = (FreeEcho.FEWindowEvent.HookWindowEventType)runningPluginInformation.IPlugin.GetWindowEventType;      // イベントの種類

        switch (e.EventType)
        {
            case FreeEcho.FEWindowEvent.EVENT_CONSTANTS.EVENT_OBJECT_CREATE:
                if ((type & FreeEcho.FEWindowEvent.HookWindowEventType.Create) == FreeEcho.FEWindowEvent.HookWindowEventType.Create)
                {
                    result = true;
                }
                break;
            case FreeEcho.FEWindowEvent.EVENT_CONSTANTS.EVENT_OBJECT_DESTROY:
                if ((type & FreeEcho.FEWindowEvent.HookWindowEventType.Destroy) == FreeEcho.FEWindowEvent.HookWindowEventType.Destroy)
                {
                    result = true;
                }
                break;
            case FreeEcho.FEWindowEvent.EVENT_CONSTANTS.EVENT_SYSTEM_FOREGROUND:
                if ((type & FreeEcho.FEWindowEvent.HookWindowEventType.Foreground) == FreeEcho.FEWindowEvent.HookWindowEventType.Foreground)
                {
                    result = true;
                }
                break;
            case FreeEcho.FEWindowEvent.EVENT_CONSTANTS.EVENT_OBJECT_HIDE:
                if ((type & FreeEcho.FEWindowEvent.HookWindowEventType.Hide) == FreeEcho.FEWindowEvent.HookWindowEventType.Hide)
                {
                    result = true;
                }
                break;
            case FreeEcho.FEWindowEvent.EVENT_CONSTANTS.EVENT_OBJECT_LOCATIONCHANGE:
                if ((type & FreeEcho.FEWindowEvent.HookWindowEventType.LocationChange) == FreeEcho.FEWindowEvent.HookWindowEventType.LocationChange)
                {
                    result = true;
                }
                break;
            case FreeEcho.FEWindowEvent.EVENT_CONSTANTS.EVENT_SYSTEM_MINIMIZEEND:
                if ((type & FreeEcho.FEWindowEvent.HookWindowEventType.MinimizeEnd) == FreeEcho.FEWindowEvent.HookWindowEventType.MinimizeEnd)
                {
                    result = true;
                }
                break;
            case FreeEcho.FEWindowEvent.EVENT_CONSTANTS.EVENT_SYSTEM_MINIMIZESTART:
                if ((type & FreeEcho.FEWindowEvent.HookWindowEventType.MinimizeStart) == FreeEcho.FEWindowEvent.HookWindowEventType.MinimizeStart)
                {
                    result = true;
                }
                break;
            case FreeEcho.FEWindowEvent.EVENT_CONSTANTS.EVENT_SYSTEM_MOVESIZEEND:
                if ((type & FreeEcho.FEWindowEvent.HookWindowEventType.MoveSizeEnd) == FreeEcho.FEWindowEvent.HookWindowEventType.MoveSizeEnd)
                {
                    result = true;
                }
                break;
            case FreeEcho.FEWindowEvent.EVENT_CONSTANTS.EVENT_SYSTEM_MOVESIZESTART:
                if ((type & FreeEcho.FEWindowEvent.HookWindowEventType.MoveSizeStart) == FreeEcho.FEWindowEvent.HookWindowEventType.MoveSizeStart)
                {
                    result = true;
                }
                break;
            case FreeEcho.FEWindowEvent.EVENT_CONSTANTS.EVENT_OBJECT_NAMECHANGE:
                if ((type & FreeEcho.FEWindowEvent.HookWindowEventType.NameChange) == FreeEcho.FEWindowEvent.HookWindowEventType.NameChange)
                {
                    result = true;
                }
                break;
            case FreeEcho.FEWindowEvent.EVENT_CONSTANTS.EVENT_OBJECT_SHOW:
                if ((type & FreeEcho.FEWindowEvent.HookWindowEventType.Show) == FreeEcho.FEWindowEvent.HookWindowEventType.Show)
                {
                    result = true;
                }
                break;
        }

        return result;
    }

    /// <summary>
    /// 全てのプラグインに停止を指示
    /// </summary>
    public void StopAllPlugins()
    {
        foreach (RunningPluginInformation nowInformation in RunningPluginInformation)
        {
            nowInformation.IPlugin?.Destruction();
        }
    }

    /// <summary>
    /// プラグインのファイル情報を取得
    /// </summary>
    /// <returns>プラグインのファイル情報</returns>
    public static List<PluginFileInformation> GetPluginsInformation()
    {
        List<PluginFileInformation> information = new();

        try
        {
            using Process process = new();
            process.StartInfo.FileName = VariousProcessing.GetApplicationDirectory(false) + Path.DirectorySeparatorChar + Common.GetPluginPathsFileName;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardInput = false;
            process.StartInfo.UseShellExecute = false;
            process.Start();
            string getString = process.StandardOutput.ReadToEnd();
            string[] splitString = getString.Split(Environment.NewLine);
            for (int index = 0; index + 1 < splitString.Length; index += 2)
            {
                PluginFileInformation newInformation = new()
                {
                    Path = splitString[index],
                    PluginName = splitString[index + 1]
                };
                information.Add(newInformation);
            }
            process.WaitForExit();
        }
        catch
        {
        }

        return information;
    }
}
