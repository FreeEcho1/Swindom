namespace Swindom;

/// <summary>
/// 位置とサイズ
/// </summary>
[DataContract]
public class PositionSize : IExtensibleDataObject
{
    /// <summary>
    /// ディスプレイ
    /// </summary>
    [DataMember]
    public string Display;
    /// <summary>
    /// ウィンドウの位置
    /// </summary>
    [DataMember]
    public PointDecimal Position;
    /// <summary>
    ///  ウィンドウのサイズ
    /// </summary>
    [DataMember]
    public SizeDecimal Size;
    /// <summary>
    /// ウィンドウのX位置指定の種類
    /// </summary>
    [DataMember]
    public WindowXType XType;
    /// <summary>
    /// ウィンドウのY位置指定の種類
    /// </summary>
    [DataMember]
    public WindowYType YType;
    /// <summary>
    /// ウィンドウの幅指定の種類
    /// </summary>
    [DataMember]
    public WindowSizeType WidthType;
    /// <summary>
    /// ウィンドウの高さ指定の種類
    /// </summary>
    [DataMember]
    public WindowSizeType HeightType;
    /// <summary>
    /// ウィンドウのX座標の値の種類
    /// </summary>
    [DataMember]
    public PositionSizeValueType XValueType;
    /// <summary>
    /// ウィンドウのY座標の値の種類
    /// </summary>
    [DataMember]
    public PositionSizeValueType YValueType;
    /// <summary>
    /// ウィンドウの幅の値の種類
    /// </summary>
    [DataMember]
    public PositionSizeValueType WidthValueType;
    /// <summary>
    /// ウィンドウの高さの値の種類
    /// </summary>
    [DataMember]
    public PositionSizeValueType HeightValueType;
    /// <summary>
    /// クライアントエリアを対象とするかの値 (いいえ「false」/はい「true」)
    /// </summary>
    [DataMember]
    public bool ClientArea;
    /// <summary>
    /// 位置とサイズを2回処理
    /// </summary>
    [DataMember]
    public bool ProcessingPositionAndSizeTwice;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public PositionSize()
    {
        SetDefaultValue();
    }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="ps">PositionSize</param>
    public PositionSize(
        PositionSize ps
        )
    {
        Position = new(ps.Position.X, ps.Position.Y);
        Size = new(ps.Size.Width, ps.Size.Height);
        XType = ps.XType;
        YType = ps.YType;
        WidthType = ps.WidthType;
        HeightType = ps.HeightType;
        XValueType = ps.XValueType;
        YValueType = ps.YValueType;
        WidthValueType = ps.WidthValueType;
        HeightValueType = ps.HeightValueType;
        Display = ps.Display;
        ClientArea = ps.ClientArea;
        ProcessingPositionAndSizeTwice = ps.ProcessingPositionAndSizeTwice;
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
        Position = new();
        Size = new();
        XType = WindowXType.DoNotChange;
        YType = WindowYType.DoNotChange;
        WidthType = WindowSizeType.DoNotChange;
        HeightType = WindowSizeType.DoNotChange;
        XValueType = PositionSizeValueType.Pixel;
        YValueType = PositionSizeValueType.Pixel;
        WidthValueType = PositionSizeValueType.Pixel;
        HeightValueType = PositionSizeValueType.Pixel;
        Display = "";
        ClientArea = false;
        ProcessingPositionAndSizeTwice = false;
    }
}
