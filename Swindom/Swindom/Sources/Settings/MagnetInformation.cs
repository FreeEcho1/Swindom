namespace Swindom;

/// <summary>
/// 「マグネット」機能の情報
/// </summary>
[DataContract]
public class MagnetInformation : IExtensibleDataObject
{
    /// <summary>
    /// 処理状態 (無効「false」/有効「true」)
    /// </summary>
    [DataMember]
    public bool Enabled;
    /// <summary>
    /// 画面端に貼り付ける (無効「false」/有効「true」)
    /// </summary>
    [DataMember]
    public bool PasteToScreenEdge;
    /// <summary>
    /// 別のウィンドウに貼り付ける (無効「false」/有効「true」)
    /// </summary>
    [DataMember]
    public bool PasteToAnotherWindow;
    /// <summary>
    /// キーを押した状態で貼り付ける (無効「false」/有効「true」)
    /// </summary>
    [DataMember]
    public bool PressTheKeyToPaste;
    /// <summary>
    /// 貼り付けた時の停止時間 (ミリ秒)
    /// </summary>
    [DataMember]
    public int StopTimeWhenPasted;
    /// <summary>
    /// 貼り付ける判定距離
    /// </summary>
    [DataMember]
    public int DecisionDistanceToPaste;
    /// <summary>
    /// 全画面ウィンドウがある場合は処理停止 (無効「false」/有効「true」)
    /// </summary>
    [DataMember]
    public bool StopProcessingFullScreen;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public MagnetInformation()
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
        PasteToScreenEdge = false;
        PasteToAnotherWindow = false;
        PressTheKeyToPaste = false;
        StopTimeWhenPasted = 400;
        DecisionDistanceToPaste = 10;
        StopProcessingFullScreen = false;
    }
}
