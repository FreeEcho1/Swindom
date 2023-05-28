namespace Swindom;

/// <summary>
/// 設定
/// </summary>
public class Settings
{
    /// <summary>
    /// メインウィンドウの位置とサイズ
    /// </summary>
    public Rectangle MainWindowRectangle;
    /// <summary>
    /// メインウィンドウのウィンドウ状態 (通常のウィンドウ、最大化)
    /// </summary>
    public WindowState WindowStateMainWindow;
    /// <summary>
    /// 言語
    /// </summary>
    public string Language;
    /// <summary>
    /// 座標の種類
    /// </summary>
    public CoordinateType CoordinateType;
    /// <summary>
    /// ダークモード
    /// </summary>
    public bool DarkMode;
    /// <summary>
    /// 実行時に自動で更新確認 (無効「false」/有効「true」)
    /// </summary>
    public bool AutomaticallyUpdateCheck;
    /// <summary>
    /// ベータバージョンを含めて更新確認 (無効「false」/有効「true」)
    /// </summary>
    public bool CheckBetaVersion;
    /// <summary>
    /// 長いパスを使用 (32767文字)
    /// </summary>
    public bool UseLongPath;
    /// <summary>
    /// 「貼り付ける位置をずらす」情報
    /// </summary>
    public ShiftPastePosition ShiftPastePosition;

    /// <summary>
    /// 「指定ウィンドウ」機能の設定情報
    /// </summary>
    public SpecifyWindowInformation SpecifyWindowInformation;
    /// <summary>
    /// 「全てのウィンドウ」機能の設定情報
    /// </summary>
    public AllWindowInformation AllWindowInformation;
    /// <summary>
    /// 「マグネット」機能の設定情報
    /// </summary>
    public MagnetInformation MagnetInformation;
    /// <summary>
    /// 「ホットキー」機能の設定情報
    /// </summary>
    public HotkeyInformation HotkeyInformation;
    /// <summary>
    /// プラグイン情報
    /// </summary>
    public PluginInformation PluginInformation;

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
        SpecifyWindowInformation = new();
        AllWindowInformation = new();
        MagnetInformation = new();
        HotkeyInformation = new();
        PluginInformation = new();
    }
}
