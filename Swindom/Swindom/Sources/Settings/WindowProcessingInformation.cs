namespace Swindom;

/// <summary>
/// ウィンドウの処理情報
/// </summary>
[DataContract]
public class WindowProcessingInformation : IExtensibleDataObject
{
    /// <summary>
    /// 処理名
    /// </summary>
    [DataMember]
    public string ProcessingName;
    /// <summary>
    /// 最前面の種類
    /// </summary>
    [DataMember]
    public Forefront Forefront;
    /// <summary>
    /// 透明度を指定 (無効「false」/有効「true」)
    /// </summary>
    [DataMember]
    public bool EnabledTransparency;
    /// <summary>
    /// 透明度
    /// </summary>
    [DataMember]
    public int Transparency;
    /// <summary>
    /// ホットキー
    /// </summary>
    [DataMember]
    public FreeEcho.FEHotKeyWPF.HotKeyInformation Hotkey;
    /// <summary>
    /// ウィンドウの状態
    /// </summary>
    [DataMember]
    public SettingsWindowState SettingsWindowState;
    /// <summary>
    /// 「通常のウィンドウ」の時だけ処理する (無効「false」/有効「true」)
    /// </summary>
    [DataMember]
    public bool OnlyNormalWindow;
    /// <summary>
    /// 位置とサイズ
    /// </summary>
    [DataMember]
    public PositionSize PositionSize;
    /// <summary>
    /// アクティブ状態 (いいえ「false」/はい「true」)
    /// </summary>
    [DataMember]
    public bool Active;

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
        SetDefaultValue();
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
        Copy(item, copyHotkey);
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
        ProcessingName = item.ProcessingName;
        Forefront = item.Forefront;
        EnabledTransparency = item.EnabledTransparency;
        Transparency = item.Transparency;
        Hotkey = new();
        if (copyHotkey)
        {
            Hotkey.Copy(item.Hotkey);
        }
        SettingsWindowState = item.SettingsWindowState;
        OnlyNormalWindow = item.OnlyNormalWindow;
        PositionSize = new(item.PositionSize);
        Active = item.Active;
        HotkeyId = -1;
    }

    [OnDeserializing]
    public void DefaultDeserializing(
        StreamingContext context
        )
    {
        SetDefaultValue();
    }

    public ExtensionDataObject? ExtensionData { get; set; }

    /// <summary>
    /// デフォルト値を設定
    /// </summary>
    private void SetDefaultValue()
    {
        ProcessingName = "";
        Forefront = Forefront.DoNotChange;
        EnabledTransparency = false;
        Transparency = 255;
        Hotkey = new();
        SettingsWindowState = SettingsWindowState.DoNotChange;
        OnlyNormalWindow = false;
        PositionSize = new();
        Active = false;
        HotkeyId = -1;
    }
}
