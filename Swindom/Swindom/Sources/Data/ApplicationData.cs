using Swindom;

/// <summary>
/// アプリケーションデータ
/// </summary>
public class ApplicationData : IDisposable
{
    /// <summary>
    /// Disposeが呼ばれたかの値
    /// </summary>
    private bool Disposed;
    /// <summary>
    /// ウィンドウ処理の管理
    /// </summary>
    public readonly WindowProcessingManagement WindowProcessingManagement = new();
    /// <summary>
    /// 設定データ
    /// </summary>
    public Settings Settings = new();
    /// <summary>
    /// ユーザー指定の設定ファイルのパス (指定しない「null」)
    /// </summary>
    public string? SpecifySettingsFilePath;
    /// <summary>
    /// 言語データ
    /// </summary>
    public Languages Languages = new();
    /// <summary>
    /// 処理イベント
    /// </summary>
    public event EventHandler<ProcessingEventArgs>? ProcessingEvent;
    /// <summary>
    /// 処理イベントを実行
    /// </summary>
    /// <param name="processingEventType">処理イベントの種類</param>
    public virtual void DoProcessingEvent(
        ProcessingEventType processingEventType
        )
    {
        try
        {
            ProcessingEvent?.Invoke(this, new(processingEventType));
        }
        catch
        {
        }
    }
    /// <summary>
    /// 全画面のウィンドウがあるかの値 (いいえ「false」/はい「true」)
    /// </summary>
    public bool FullScreenExists;
    /// <summary>
    /// システムトレイアイコン用のウィンドウのウィンドウハンドル
    /// </summary>
    public IntPtr SystemTrayIconHwnd;
    /// <summary>
    /// ウィンドウの管理
    /// </summary>
    public WindowManagement WindowManagement = new();

    /// <summary>
    /// デストラクタ
    /// </summary>
    ~ApplicationData()
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
    /// <param name="disposing"></param>
    protected virtual void Dispose(
        bool disposing
        )
    {
        if (Disposed == false)
        {
            Disposed = true;
            WindowProcessingManagement.Dispose();
            WindowManagement.Dispose();
        }
    }
}
