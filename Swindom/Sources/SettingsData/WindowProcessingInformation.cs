namespace Swindom;

/// <summary>
/// ウィンドウの処理情報
/// </summary>
public class WindowProcessingInformation
{
    /// <summary>
    /// アクティブ状態 (いいえ「false」/はい「true」)
    /// </summary>
    public bool Active { get; set; }
    /// <summary>
    /// 処理名
    /// </summary>
    public string ProcessingName { get; set; }
    /// <summary>
    /// 位置とサイズ
    /// </summary>
    public PositionSize PositionSize { get; set; }
    /// <summary>
    /// 「通常のウィンドウ」の時だけ処理する (無効「false」/有効「true」)
    /// </summary>
    public bool NormalWindowOnly { get; set; }
    /// <summary>
    /// 最前面の種類
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Forefront Forefront { get; set; }
    /// <summary>
    /// 透明度を指定 (無効「false」/有効「true」)
    /// </summary>
    public bool SpecifyTransparency { get; set; }
    /// <summary>
    /// 透明度の最小値
    /// </summary>
    [JsonIgnore]
    public static int MinimumTransparency { get; } = 0;
    /// <summary>
    /// 透明度の最大値
    /// </summary>
    [JsonIgnore]
    public static int MaximumTransparency { get; } = 255;
    /// <summary>
    /// 透明度
    /// </summary>
    [JsonIgnore]
    private int _transparency;
    /// <summary>
    /// 透明度
    /// </summary>
    public int Transparency
    {
        get
        {
            return _transparency;
        }
        set
        {
            if (value < MinimumTransparency)
            {
                _transparency = MinimumTransparency;
            }
            else if (MaximumTransparency < value)
            {
                _transparency = MaximumTransparency;
            }
            else
            {
                _transparency = value;
            }
        }
    }
    /// <summary>
    /// ウィンドウを閉じる (無効「false」/有効「true」)
    /// </summary>
    public bool CloseWindow { get; set; }
    /// <summary>
    /// ホットキー情報
    /// </summary>
    public FreeEcho.FEHotKeyWPF.HotKeyInformation Hotkey { get; set; }

    /// <summary>
    /// ホットキーの識別子 (ホットキー無し「-1」)
    /// </summary>
    [JsonIgnore]
    public int HotkeyId;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public WindowProcessingInformation()
    {
        Active = false;
        ProcessingName = "";
        PositionSize = new();
        NormalWindowOnly = false;
        Forefront = Forefront.DoNotChange;
        SpecifyTransparency = false;
        Transparency = 255;
        CloseWindow = false;
        Hotkey = new();
        HotkeyId = -1;
    }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="information">ウィンドウの処理情報</param>
    /// <param name="copyHotkey">ホットキーをコピーする (いいえ「false」/はい「true」)</param>
    public WindowProcessingInformation(
        WindowProcessingInformation information,
        bool copyHotkey = true
        )
    {
        Active = information.Active;
        ProcessingName = information.ProcessingName;
        PositionSize = new(information.PositionSize);
        NormalWindowOnly = information.NormalWindowOnly;
        Forefront = information.Forefront;
        SpecifyTransparency = information.SpecifyTransparency;
        Transparency = information.Transparency;
        CloseWindow = information.CloseWindow;
        Hotkey = copyHotkey ? new(information.Hotkey) : new();
        HotkeyId = -1;
    }

    /// <summary>
    /// コピー
    /// </summary>
    /// <param name="information">ウィンドウの処理情報</param>
    /// <param name="copyHotkey">ホットキーをコピーする (いいえ「false」/はい「true」)</param>
    public void Copy(
        WindowProcessingInformation information,
        bool copyHotkey = true
        )
    {
        Active = information.Active;
        ProcessingName = information.ProcessingName;
        PositionSize = new(information.PositionSize);
        NormalWindowOnly = information.NormalWindowOnly;
        Forefront = information.Forefront;
        SpecifyTransparency = information.SpecifyTransparency;
        Transparency = information.Transparency;
        CloseWindow = information.CloseWindow;
        Hotkey = copyHotkey ? new(information.Hotkey) : new();
        HotkeyId = -1;
    }
}
