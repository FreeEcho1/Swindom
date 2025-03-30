namespace Swindom;

/// <summary>
/// 設定
/// </summary>
public class Settings
{
    /// <summary>
    /// メインウィンドウの位置とサイズ
    /// </summary>
    public RectangleInt MainWindowRectangle { get; set; }
    /// <summary>
    /// メインウィンドウのウィンドウ状態 (通常のウィンドウ、最大化)
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public WindowState WindowStateMainWindow { get; set; }
    /// <summary>
    /// 言語
    /// </summary>
    public string Language { get; set; }
    /// <summary>
    /// 座標の種類
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public CoordinateType CoordinateType { get; set; }
    /// <summary>
    /// ダークモード
    /// </summary>
    public bool DarkMode { get; set; }
    /// <summary>
    /// 実行時に自動で更新確認 (無効「false」/有効「true」)
    /// </summary>
    public bool AutomaticallyUpdateCheck { get; set; }
    /// <summary>
    /// ベータバージョンを含めて更新確認 (無効「false」/有効「true」)
    /// </summary>
    public bool CheckBetaVersion { get; set; }
    /// <summary>
    /// 全画面ウィンドウの追加判定 (正しく判定されない場合のみ)
    /// </summary>
    public bool FullScreenWindowAdditionDecision { get; set; }
    /// <summary>
    /// 長いパスを使用 (32767文字)
    /// </summary>
    public bool UseLongPath { get; set; }
    /// <summary>
    /// 「貼り付ける位置をずらす」情報
    /// </summary>
    public ShiftPastePosition ShiftPastePosition { get; set; }
    /// <summary>
    /// 「説明」ウィンドウのサイズ
    /// </summary>
    public SizeInt ExplanationWindowSize { get; set; }

    /// <summary>
    /// 全て処理停止の情報
    /// </summary>
    public AllStopProcessingInformation AllStopProcessingInformation { get; set; }
    /// <summary>
    /// 「指定ウィンドウ」機能の設定情報
    /// </summary>
    public SpecifyWindowInformation SpecifyWindowInformation { get; set; }
    /// <summary>
    /// 「全てのウィンドウ」機能の設定情報
    /// </summary>
    public AllWindowInformation AllWindowInformation { get; set; }
    /// <summary>
    /// 「マグネット」機能の設定情報
    /// </summary>
    public MagnetInformation MagnetInformation { get; set; }
    /// <summary>
    /// 「ホットキー」機能の設定情報
    /// </summary>
    public HotkeyInformation HotkeyInformation { get; set; }
    /// <summary>
    /// プラグイン情報
    /// </summary>
    public PluginInformation PluginInformation { get; set; }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public Settings()
    {
        MainWindowRectangle = new(0, 0, 600, 400);
        WindowStateMainWindow = WindowState.Normal;
        Language = "";
        CoordinateType = CoordinateType.EachDisplay;
        DarkMode = false;
        AutomaticallyUpdateCheck = false;
        CheckBetaVersion = false;
        UseLongPath = false;
        ShiftPastePosition = new();
        ExplanationWindowSize = new(500, 400);
        AllStopProcessingInformation = new();
        SpecifyWindowInformation = new();
        AllWindowInformation = new();
        MagnetInformation = new();
        HotkeyInformation = new();
        PluginInformation = new();
    }
}
