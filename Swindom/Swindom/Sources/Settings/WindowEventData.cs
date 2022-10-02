namespace Swindom;

/// <summary>
/// ウィンドウイベントのデータ
/// </summary>
[DataContract]
public class WindowEventData : IExtensibleDataObject
{
    /// <summary>
    /// フォアグラウンドが変更された
    /// </summary>
    [DataMember]
    public bool Foreground;
    /// <summary>
    /// 移動及びサイズの変更が終了された
    /// </summary>
    [DataMember]
    public bool MoveSizeEnd;
    /// <summary>
    /// 最小化が開始された
    /// </summary>
    [DataMember]
    public bool MinimizeStart;
    /// <summary>
    /// 最小化が終了された
    /// </summary>
    [DataMember]
    public bool MinimizeEnd;
    /// <summary>
    /// 作成された
    /// </summary>
    [DataMember]
    public bool Create;
    /// <summary>
    /// 表示された
    /// </summary>
    [DataMember]
    public bool Show;
    /// <summary>
    /// 名前が変更された
    /// </summary>
    [DataMember]
    public bool NameChange;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public WindowEventData()
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
        Foreground = false;
        MoveSizeEnd = false;
        MinimizeStart = false;
        MinimizeEnd = false;
        Create = false;
        Show = false;
        NameChange = false;
    }
}
