namespace Swindom;

/// <summary>
/// 「イベント」機能の種類の情報
/// </summary>
[DataContract]
public class EventTypeInformation : IExtensibleDataObject
{
    /// <summary>
    /// Foreground
    /// </summary>
    [DataMember]
    public bool Foreground;
    /// <summary>
    /// MoveSizeEnd
    /// </summary>
    [DataMember]
    public bool MoveSizeEnd;
    /// <summary>
    /// MinimizeStart
    /// </summary>
    [DataMember]
    public bool MinimizeStart;
    /// <summary>
    /// MinimizeEnd
    /// </summary>
    [DataMember]
    public bool MinimizeEnd;
    /// <summary>
    /// Create
    /// </summary>
    [DataMember]
    public bool Create;
    /// <summary>
    /// Show
    /// </summary>
    [DataMember]
    public bool Show;
    /// <summary>
    /// NameChange
    /// </summary>
    [DataMember]
    public bool NameChange;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public EventTypeInformation()
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
        Foreground = true;
        MoveSizeEnd = true;
        MinimizeStart = true;
        MinimizeEnd = true;
        Create = true;
        Show = true;
        NameChange = true;
    }
}
