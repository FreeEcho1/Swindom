namespace Swindom;

/// <summary>
/// アプリケーションデータ
/// </summary>
public static class ApplicationData
{
    /// <summary>
    /// 設定データ
    /// </summary>
    public static Settings Settings { get; set; } = new();
    /// <summary>
    /// ユーザー指定の設定ファイルのパス (指定しない「null」)
    /// </summary>
    public static string? SpecifySettingsFilePath { get; set; }
    /// <summary>
    /// 言語データ
    /// </summary>
    public static Languages Languages { get; set; } = new();
    /// <summary>
    /// 処理イベント
    /// </summary>
    public static ProcessingEventData EventData { get; set; } = new();
    /// <summary>
    /// 全画面のウィンドウがあるかの値 (いいえ「false」/はい「true」)
    /// </summary>
    public static bool FullScreenExists { get; set; }
    /// <summary>
    /// メインのウィンドウハンドル
    /// </summary>
    public static IntPtr MainHwnd { get; set; }
    /// <summary>
    /// ウィンドウの管理
    /// </summary>
    public static WindowManagement WindowManagement { get; set; } = new();
    /// <summary>
    /// モニター情報
    /// </summary>
    public static MonitorInformation MonitorInformation { get; set; } = MonitorInformation.GetMonitorInformation();
    /// <summary>
    /// ウィンドウ処理の管理
    /// </summary>
    public static readonly WindowProcessingManagement WindowProcessingManagement = new();

    /// <summary>
    /// 破棄
    /// </summary>
    public static void Release()
    {
        WindowManagement.CloseAll();
        WindowProcessingManagement.Dispose();
    }
}
