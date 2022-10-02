namespace Swindom;

/// <summary>
/// 「ホットキー」機能の項目情報
/// </summary>
[DataContract]
public class HotkeyItemInformation : IExtensibleDataObject
{
    /// <summary>
    /// 登録名
    /// </summary>
    [DataMember]
    public string RegisteredName;
    /// <summary>
    /// 処理の種類
    /// </summary>
    [DataMember]
    public HotkeyProcessingType ProcessingType;
    /// <summary>
    /// 位置とサイズ
    /// </summary>
    [DataMember]
    public PositionSize PositionSize;
    /// <summary>
    /// 基準にするディスプレイ
    /// </summary>
    [DataMember]
    public StandardDisplay StandardDisplay;
    /// <summary>
    /// 処理値 (移動量、サイズ変更量、透明度)
    /// </summary>
    [DataMember]
    public int ProcessingValue;
    /// <summary>
    /// ホットキー情報
    /// </summary>
    [DataMember]
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
    public HotkeyItemInformation()
    {
        SetDefaultValue();
    }

    /// <summary>
    /// コピー
    /// </summary>
    /// <param name="item">「ホットキー」機能の項目情報</param>
    public void Copy(
        HotkeyItemInformation item
        )
    {
        RegisteredName = item.RegisteredName;
        ProcessingType = item.ProcessingType;
        PositionSize.Position = new(item.PositionSize.Position.X, item.PositionSize.Position.Y);
        PositionSize.Size = new(item.PositionSize.Size.Width, item.PositionSize.Size.Height);
        PositionSize.XType = item.PositionSize.XType;
        PositionSize.YType = item.PositionSize.YType;
        PositionSize.WidthType = item.PositionSize.WidthType;
        PositionSize.HeightType = item.PositionSize.HeightType;
        PositionSize.XValueType = item.PositionSize.XValueType;
        PositionSize.YValueType = item.PositionSize.YValueType;
        PositionSize.WidthValueType = item.PositionSize.WidthValueType;
        PositionSize.HeightValueType = item.PositionSize.HeightValueType;
        PositionSize.Display = item.PositionSize.Display;
        PositionSize.ClientArea = item.PositionSize.ClientArea;
        PositionSize.ProcessingPositionAndSizeTwice = item.PositionSize.ProcessingPositionAndSizeTwice;
        StandardDisplay = item.StandardDisplay;
        ProcessingValue = item.ProcessingValue;
        Hotkey.Copy(item.Hotkey);
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
        RegisteredName = "";
        ProcessingType = HotkeyProcessingType.PositionSize;
        PositionSize = new();
        StandardDisplay = StandardDisplay.SpecifiedDisplay;
        ProcessingValue = 0;
        Hotkey = new();
        HotkeyId = -1;
    }
}
