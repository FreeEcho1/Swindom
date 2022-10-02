namespace Swindom;

/// <summary>
/// 「イベント」機能の項目情報
/// </summary>
[DataContract]
public class EventItemInformation : SpecifiedWindowBaseItemInformation, IExtensibleDataObject
{
    /// <summary>
    /// ウィンドウイベントのデータ
    /// </summary>
    [DataMember]
    public WindowEventData WindowEventData;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public EventItemInformation()
    {
        SetDefaultValue();
    }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="item">「イベント」機能の項目情報</param>
    /// <param name="copyHotkey">ホットキーをコピーするかの値 (コピーしない「false」/コピーする「true」)</param>
    public EventItemInformation(
        EventItemInformation item,
        bool copyHotkey = true
        )
        : base(item, copyHotkey)
    {
        WindowEventData = new();
        WindowEventData.Foreground = item.WindowEventData.Foreground;
        WindowEventData.MoveSizeEnd = item.WindowEventData.MoveSizeEnd;
        WindowEventData.MinimizeStart = item.WindowEventData.MinimizeStart;
        WindowEventData.MinimizeEnd = item.WindowEventData.MinimizeEnd;
        WindowEventData.Create = item.WindowEventData.Create;
        WindowEventData.Show = item.WindowEventData.Show;
        WindowEventData.NameChange = item.WindowEventData.NameChange;
    }

    [OnDeserializing]
    private new void DefaultDeserializing(
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
        WindowEventData = new();
    }
}
