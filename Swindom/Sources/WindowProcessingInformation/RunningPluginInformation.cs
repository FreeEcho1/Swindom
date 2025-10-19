using System.IO.Pipes;
using System.Threading;
using System.Threading.Tasks;

namespace Swindom;

/// <summary>
/// 実行中のプラグイン情報
/// </summary>
public class RunningPluginInformation
{
    /// <summary>
    /// Disposeが呼ばれたかの値
    /// </summary>
    private bool Disposed;
    /// <summary>
    /// プラグインのファイルパス
    /// </summary>
    public string Path { get; set; } = "";
    /// <summary>
    /// プラグイン名
    /// </summary>
    public string PluginName { get; set; } = "";
    /// <summary>
    /// プラグインのプロセス
    /// </summary>
    public Process? PluginProcess { get; set; }
    /// <summary>
    /// パイプ
    /// </summary>
    private NamedPipeServerStream? PipeServer;
    /// <summary>
    /// パイプ用のStreamWriter
    /// </summary>
    private StreamWriter? StreamWriterPipe;
    /// <summary>
    /// パイプ名の前方部分
    /// </summary>
    private static string ForwardPipeName { get; } = "MyPipe";
    /// <summary>
    /// プラグインを実行するファイル名
    /// </summary>
    private static string RunPluginFileName { get; } = "SwindomRunPlugin.exe";
    /// <summary>
    /// ウィンドウ表示を指示する文字列
    /// </summary>
    private static string ShowWindowDirectionsString { get; } = "ShowWindow";
    /// <summary>
    /// プロセス終了を指示する文字列
    /// </summary>
    private static string ExitProcessDirectionsString { get; } = "ExitProcess";
    /// <summary>
    /// プロセス終了を待機する時間 (ミリ秒)
    /// </summary>
    private static int WaitForExitProcessMillisecond { get; } = 2000;
    /// <summary>
    /// パイプ接続の待機最大時間
    /// </summary>
    private static int PipeConnectionMaxWaitTime { get; } = 10;

    /// <summary>
    /// 「プラグインプロセスが閉じた」イベント
    /// </summary>
    public event EventHandler<EventArgs> PluginProcessExitedEvent = delegate { };
    /// <summary>
    /// 「プラグインプロセスが閉じた」イベントを実行
    /// </summary>
    public virtual void DoPluginProcessExited() => PluginProcessExitedEvent?.Invoke(this, new());

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public RunningPluginInformation()
    {
    }

    /// <summary>
    /// デストラクタ
    /// </summary>
    ~RunningPluginInformation()
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
            StopPlugin();
        }
        Disposed = true;
    }

    /// <summary>
    /// プラグインを実行
    /// </summary>
    /// <param name="pluginFilePath">プラグインのファイルパス</param>
    /// <param name="pluginName">プラグイン名</param>
    public async Task RunPlugin(
        string pluginFilePath,
        string pluginName
        )
    {
        // パイプ名はファイル名を使用
        string pipeName = ForwardPipeName + System.IO.Path.GetFileNameWithoutExtension(pluginFilePath);      // パイプ名

        Process ownProcess = Process.GetCurrentProcess();
        Process pluginProcess = new();
        pluginProcess.StartInfo.FileName = ApplicationPath.GetApplicationDirectory() + System.IO.Path.DirectorySeparatorChar + RunPluginFileName;
        pluginProcess.StartInfo.Arguments = "\"" + pluginFilePath + "\" \"" + SettingFileProcessing.GetSettingsDirectory() + "\" \"" + ApplicationData.Settings.Language + "\" \"" + pipeName + "\" \"" + ownProcess.Id + "\"";
        pluginProcess.Exited += (sender, e) =>
        {
            if (pluginProcess.ExitCode != 0)
            {
                DoPluginProcessExited();
            }
        };
        NamedPipeServerStream pipeServer = new(pipeName, PipeDirection.Out, 1, PipeTransmissionMode.Byte, PipeOptions.Asynchronous);

        pluginProcess.Start();

        try
        {
            // 接続が終わるまで待機
            using CancellationTokenSource cts = new(TimeSpan.FromSeconds(PipeConnectionMaxWaitTime));
            Task waitConnectionTask = pipeServer.WaitForConnectionAsync(cts.Token);
            Task connectionTask = Task.Run(async () =>
            {
                while (pluginProcess.HasExited == false)
                {
                    await Task.Delay(100);
                }
                // プロセスが終了したらキャンセル
                if (waitConnectionTask.IsCompleted == false)
                {
                    cts.Cancel();
                }
            });
            await waitConnectionTask;

            Path = pluginFilePath;
            PluginName = pluginName;
            PipeServer = pipeServer;
            StreamWriterPipe = new(PipeServer)
            {
                AutoFlush = true
            };
            PluginProcess = pluginProcess;
        }
        catch
        {
            pipeServer?.Dispose();
        }
    }

    /// <summary>
    /// ウィンドウを表示
    /// </summary>
    public void ShowWindow()
    {
        try
        {
            if (PipeServer != null)
            {
                // ウィンドウを表示する指示を出す
                //using StreamWriter writer = new(PipeServer)
                //{
                //    AutoFlush = true
                //};
                StreamWriterPipe?.WriteLine(ShowWindowDirectionsString);
            }
        }
        catch
        {
        }
    }

    /// <summary>
    /// プラグインを停止
    /// </summary>
    public void StopPlugin()
    {
        try
        {
            if (PipeServer != null)
            {
                // プロセス終了の指示を出す
                //using StreamWriter writer = new(PipeServer)
                //{
                //    AutoFlush = true
                //};
                StreamWriterPipe?.WriteLine(ExitProcessDirectionsString);
            }
            if (PluginProcess?.WaitForExit(WaitForExitProcessMillisecond) == false)
            {
                PluginProcess?.Kill();
            }
            StreamWriterPipe?.Dispose();
            PipeServer?.Dispose();
        }
        catch
        {
        }
    }
}
