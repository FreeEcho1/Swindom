namespace Swindom;

/// <summary>
/// 「タイマー」機能の項目情報
/// </summary>
[DataContract]
public class TimerItemInformation : SpecifiedWindowBaseItemInformation, IExtensibleDataObject
{
    /// <summary>
    /// 最初に処理しない回数 (指定しない「0」)
    /// </summary>
    [DataMember]
    public int NumberOfTimesNotToProcessingFirst;

    // ------------------ 設定ファイルに保存しないデータ ------------------ //
    /// <summary>
    /// カウントした最初に処理しない回数
    /// </summary>
    [IgnoreDataMember]
    public int CountNumberOfTimesNotToProcessingFirst;
    // ------------------ 設定ファイルに保存しないデータ ------------------ //

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public TimerItemInformation()
        : base()
    {
        SetDefaultValue();
    }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="item">「タイマー」機能の項目情報</param>
    /// <param name="copyHotkey">ホットキーをコピーするかの値 (コピーしない「false」/コピーする「true」)</param>
    public TimerItemInformation(
        TimerItemInformation item,
        bool copyHotkey = true
        )
        : base(item, copyHotkey)
    {
        NumberOfTimesNotToProcessingFirst = item.NumberOfTimesNotToProcessingFirst;
        CountNumberOfTimesNotToProcessingFirst = 0;
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
        NumberOfTimesNotToProcessingFirst = 0;
        CountNumberOfTimesNotToProcessingFirst = 0;
    }
}
