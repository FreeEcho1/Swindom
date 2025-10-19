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
    /// プラグインの取得する情報数
    /// </summary>
    private const int GetNumberOfInformation = 3;

    /// <summary>
    /// プラグインのファイル情報
    /// </summary>
    public static List<PluginFileInformation> PluginFileInformation { get; private set; } = new();

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public PluginProcessing()
    {
        RunStopPlugins();
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
            StopAllPlugins();
        }
        Disposed = true;
    }

    /// <summary>
    /// 処理が有効か確認
    /// </summary>
    /// <returns>処理が有効かの値 (無効「false」/有効「true」)</returns>
    public static bool CheckIfTheProcessingIsValid() => ApplicationData.Settings.PluginInformation.IsEnabled;

    /// <summary>
    /// プラグインを実行と停止
    /// </summary>
    public async void RunStopPlugins()
    {
        // 処理状態が無効の場合は全てのプラグインを停止
        if (ApplicationData.Settings.PluginInformation.IsEnabled == false)
        {
            StopAllPlugins();
            return;
        }

        string settingDirectory = SettingFileProcessing.GetSettingsDirectory();     // 設定ディレクトリ
        if (string.IsNullOrEmpty(settingDirectory))
        {
            return;
        }
        string? ipluginTypeName = typeof(IPlugin).FullName;       // プラグインインターフェースの型名
        if (ipluginTypeName == null)
        {
            return;
        }

        // 動作しているプラグインが無効になっていたら動作を停止
        for (int index = RunningPluginInformation.Count - 1; index >= 0; index--)
        {
            bool isEnabled = false;     // 有効状態
            foreach (PluginItemInformation nowPII in ApplicationData.Settings.PluginInformation.Items)
            {
                if (nowPII.PluginFileName == Path.GetFileNameWithoutExtension(RunningPluginInformation[index].Path))
                {
                    isEnabled = true;
                    break;
                }
            }
            if (isEnabled == false)
            {
                RunningPluginInformation[index].StopPlugin();
                RunningPluginInformation.RemoveAt(index);
            }
        }

        // プラグインが有効になっていたら動作させる
        foreach (PluginItemInformation nowPII in ApplicationData.Settings.PluginInformation.Items)
        {
            bool checkRunning = false;      // 動作しているかの値
            foreach (RunningPluginInformation nowRPI in RunningPluginInformation)
            {
                if (nowPII.PluginFileName == Path.GetFileNameWithoutExtension(nowRPI.Path))
                {
                    checkRunning = true;
                    break;
                }
            }

            if (checkRunning == false)
            {
                foreach (PluginFileInformation nowPFI in PluginFileInformation)
                {
                    if (nowPII.PluginFileName == Path.GetFileNameWithoutExtension(nowPFI.Path))
                    {
                        try
                        {
                            RunningPluginInformation newItem = new();
                            await newItem.RunPlugin(nowPFI.Path, nowPFI.PluginName);
                            newItem.PluginProcessExitedEvent += NewItem_PluginProcessExitedEvent;
                            RunningPluginInformation.Add(newItem);
                        }
                        catch
                        {
                            FEMessageBox.Show(ApplicationData.Strings.ErrorOccurred + Environment.NewLine + nowPFI.PluginName, ApplicationData.Strings.Check + WindowControlValue.CopySeparateString + ApplicationValue.ApplicationName + WindowControlValue.CopySeparateString + ApplicationData.Strings.Plugin, MessageBoxButton.OK);
                        }
                        break;
                    }
                }
            }
        }
    }

    /// <summary>
    /// 「プラグインプロセスが閉じた」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <exception cref="NotImplementedException"></exception>
    private void NewItem_PluginProcessExitedEvent(
        object? sender,
        EventArgs e
        )
    {
        try
        {
            if (sender == null)
            {
                return;
            }

            RunningPluginInformation rpi = (RunningPluginInformation)sender;

            foreach (RunningPluginInformation nowRPI in RunningPluginInformation)
            {
                if (nowRPI.PluginName == rpi.PluginName)
                {
                    nowRPI.Dispose();
                    RunningPluginInformation.Remove(nowRPI);
                    break;
                }
            }
        }
        catch
        {
        }
    }

    /// <summary>
    /// 全てのプラグインに停止を指示
    /// </summary>
    public void StopAllPlugins()
    {
        foreach (RunningPluginInformation nowRPI in RunningPluginInformation)
        {
            nowRPI.StopPlugin();
        }

        RunningPluginInformation.Clear();
    }

    /// <summary>
    /// プラグインのファイル情報を更新
    /// </summary>
    public static void UpdatePluginsInformation()
    {
        List<PluginFileInformation> information = new();

        try
        {
            using Process process = new();
            process.StartInfo.FileName = ApplicationPath.GetApplicationDirectory() + Path.DirectorySeparatorChar + PluginValue.GetPluginPathsFileName;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardInput = false;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.Arguments = "\"" + ApplicationPath.GetPluginDirectory() + "\"";
            if (Directory.Exists(ApplicationData.Settings.PluginInformation.PluginFolder))
            {
                process.StartInfo.Arguments += " \"" + ApplicationData.Settings.PluginInformation.PluginFolder + "\"";
            }
            process.Start();
            string getString = process.StandardOutput.ReadToEnd();
            string[] splitString = getString.Split(Environment.NewLine);
            for (int index = 0; index + GetNumberOfInformation < splitString.Length; index += GetNumberOfInformation)
            {
                PluginFileInformation newInformation = new()
                {
                    Path = splitString[index],
                    PluginName = splitString[index + 1],
                    IsWindowExist = bool.Parse(splitString[index + 2])
                };
                information.Add(newInformation);
            }
            process.WaitForExit();

            PluginFileInformation = information;
        }
        catch
        {
        }
    }

    /// <summary>
    /// 有効にしているプラグインが存在しない場合は、プラグインのファイル情報を削除
    /// </summary>
    public static void DeletePluginFileInformation()
    {
        try
        {
            if (PluginFileInformation.Count == 0)
            {
                UpdatePluginsInformation();
            }

            for (int index = ApplicationData.Settings.PluginInformation.Items.Count - 1; index >= 0; index--)
            {
                bool isExists = false;

                foreach (PluginFileInformation nowPFI in PluginFileInformation)
                {
                    if (ApplicationData.Settings.PluginInformation.Items[index].PluginFileName == Path.GetFileNameWithoutExtension(nowPFI.Path))
                    {
                        isExists = true;
                        break;
                    }
                }

                if (isExists == false)
                {
                    ApplicationData.Settings.PluginInformation.Items.RemoveAt(index);
                }
            }
        }
        catch
        {
        }
    }
}
