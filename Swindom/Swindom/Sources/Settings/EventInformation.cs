namespace Swindom;

/// <summary>
/// 「イベント」機能の情報
/// </summary>
[DataContract]
public class EventInformation : SpecifiedWindowBaseInformation, IExtensibleDataObject
{
    /// <summary>
    /// 「イベント」機能の種類の情報
    /// </summary>
    [DataMember]
    public EventTypeInformation EventTypeInformation;
    /// <summary>
    /// 「イベント」機能の項目情報
    /// </summary>
    [DataMember]
    public List<EventItemInformation> Items;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public EventInformation()
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
        EventTypeInformation = new();
        Items = new();
    }
}
