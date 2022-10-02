namespace Swindom;

/// <summary>
/// 「貼り付ける位置をずらす」情報
/// </summary>
[DataContract]
public class ShiftPastePosition : IExtensibleDataObject
{
    /// <summary>
    /// 有効状態 (無効「false」/有効「true」)
    /// </summary>
    [DataMember]
    public bool Enabled;
    /// <summary>
    /// 左側の距離
    /// </summary>
    [DataMember]
    public int Left;
    /// <summary>
    /// 上側の距離
    /// </summary>
    [DataMember]
    public int Top;
    /// <summary>
    /// 右側の距離
    /// </summary>
    [DataMember]
    public int Right;
    /// <summary>
    /// 下側の距離
    /// </summary>
    [DataMember]
    public int Bottom;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public ShiftPastePosition()
    {
        SetDefaultValue();
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
        Enabled = false;
        Left = 0;
        Top = 0;
        Right = 0;
        Bottom = 0;
    }
}
