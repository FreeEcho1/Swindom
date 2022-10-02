namespace Swindom;

/// <summary>
/// 「タイマー」機能の情報
/// </summary>
[DataContract]
public class TimerInformation : SpecifiedWindowBaseInformation, IExtensibleDataObject
{
    /// <summary>
    /// 処理するウィンドウの範囲
    /// </summary>
    [DataMember]
    public ProcessingWindowRange ProcessingWindowRange;
    /// <summary>
    /// 処理間隔 (ミリ秒)
    /// </summary>
    [DataMember]
    public int ProcessingInterval;
    /// <summary>
    /// 次のウィンドウを処理する待ち時間 (ミリ秒) (待たない「0」)
    /// </summary>
    [DataMember]
    public int WaitTimeToProcessingNextWindow;
    /// <summary>
    /// 「タイマー」機能の項目情報
    /// </summary>
    [DataMember]
    public List<TimerItemInformation> Items;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public TimerInformation()
    {
        SetDefaultValue();
    }

    [OnDeserializing]
    public new void DefaultDeserializing(
        StreamingContext context
        )
    {
        SetDefaultValue();
    }

    public new ExtensionDataObject? ExtensionData { get; set; }

    /// <summary>
    /// デフォルト値を設定
    /// </summary>
    private void SetDefaultValue()
    {
        ProcessingWindowRange = ProcessingWindowRange.ActiveOnly;
        ProcessingInterval = 600;
        WaitTimeToProcessingNextWindow = 0;
        Items = new();
    }
}
