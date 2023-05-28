namespace Swindom;

/// <summary>
/// ウィンドウの処理情報
/// </summary>
public class WindowProcessingInformation
{
    /// <summary>
    /// アクティブ状態 (いいえ「false」/はい「true」)
    /// </summary>
    public bool Active;
    /// <summary>
    /// 処理名
    /// </summary>
    public string ProcessingName;
    /// <summary>
    /// 位置とサイズ
    /// </summary>
    public PositionSize PositionSize;
    /// <summary>
    /// 「通常のウィンドウ」の時だけ処理する (無効「false」/有効「true」)
    /// </summary>
    public bool NormalWindowOnly;
    /// <summary>
    /// 最前面の種類
    /// </summary>
    public Forefront Forefront;
    /// <summary>
    /// 透明度を指定 (無効「false」/有効「true」)
    /// </summary>
    public bool EnabledTransparency;
    /// <summary>
    /// 透明度
    /// </summary>
    private int PrivateTransparency;
    /// <summary>
    /// 透明度の最小値
    /// </summary>
    public const int MinimumTransparency = 0;
    /// <summary>
    /// 透明度の最大値
    /// </summary>
    public const int MaximumTransparency = 255;
    /// <summary>
    /// 透明度
    /// </summary>
    public int Transparency
    {
        get
        {
            return PrivateTransparency;
        }
        set
        {
            if (value < MinimumTransparency)
            {
                PrivateTransparency = MinimumTransparency;
            }
            else if (MaximumTransparency < value)
            {
                PrivateTransparency = MaximumTransparency;
            }
            else
            {
                PrivateTransparency = value;
            }
        }
    }
    /// <summary>
    /// ウィンドウを閉じる (無効「false」/有効「true」)
    /// </summary>
    public bool CloseWindow;
    /// <summary>
    /// ホットキー情報
    /// </summary>
    public FreeEcho.FEHotKeyWPF.HotKeyInformation Hotkey;

    // ------------------ 設定ファイルに保存しないデータ ------------------ //
    /// <summary>
    /// ホットキーの識別子 (ホットキー無し「-1」)
    /// </summary>
    [IgnoreDataMember]
    public int HotkeyId;
    // ------------------ 設定ファイルに保存しないデータ ------------------ //

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
        EnabledTransparency = false;
        Transparency = 255;
        CloseWindow = false;
        Hotkey = new();
        HotkeyId = -1;
    }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="item">項目情報</param>
    /// <param name="copyHotkey">ホットキーをコピーする (いいえ「false」/はい「true」)</param>
    public WindowProcessingInformation(
        WindowProcessingInformation item,
        bool copyHotkey = true
        )
    {
        Active = item.Active;
        ProcessingName = item.ProcessingName;
        PositionSize = new(item.PositionSize);
        NormalWindowOnly = item.NormalWindowOnly;
        Forefront = item.Forefront;
        EnabledTransparency = item.EnabledTransparency;
        Transparency = item.Transparency;
        CloseWindow = item.CloseWindow;
        Hotkey = copyHotkey ? new(item.Hotkey) : new();
        HotkeyId = -1;
    }

    /// <summary>
    /// コピー
    /// </summary>
    /// <param name="item">ウィンドウの処理情報</param>
    /// <param name="copyHotkey">ホットキーをコピーする (いいえ「false」/はい「true」)</param>
    public void Copy(
        WindowProcessingInformation item,
        bool copyHotkey = true
        )
    {
        Active = item.Active;
        ProcessingName = item.ProcessingName;
        PositionSize = new(item.PositionSize);
        NormalWindowOnly = item.NormalWindowOnly;
        Forefront = item.Forefront;
        EnabledTransparency = item.EnabledTransparency;
        Transparency = item.Transparency;
        CloseWindow = item.CloseWindow;
        Hotkey = copyHotkey ? new(item.Hotkey) : new();
        HotkeyId = -1;
    }
}
