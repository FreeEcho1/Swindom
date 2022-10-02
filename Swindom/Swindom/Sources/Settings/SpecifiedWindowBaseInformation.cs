namespace Swindom;

/// <summary>
/// 「指定ウィンドウ」機能の基礎情報
/// </summary>
[DataContract]
public class SpecifiedWindowBaseInformation : IExtensibleDataObject
{
    /// <summary>
    /// 処理状態 (無効「false」/有効「true」)
    /// </summary>
    [DataMember]
    public bool Enabled;
    /// <summary>
    /// ホットキーは停止させない (全画面ウィンドウがある場合) (無効「false」/有効「true」)
    /// </summary>
    [DataMember]
    public bool HotkeysDoNotStopFullScreen;
    /// <summary>
    /// 全画面ウィンドウがある場合は処理停止 (無効「false」/有効「true」)
    /// </summary>
    [DataMember]
    public bool StopProcessingFullScreen;
    /// <summary>
    /// ウィンドウ処理を複数登録 (無効「false」/有効「true」)
    /// </summary>
    [DataMember]
    public bool RegisterMultiple;
    /// <summary>
    /// ウィンドウ判定で大文字と小文字を区別する (無効「false」/有効「true」)
    /// </summary>
    [DataMember]
    public bool CaseSensitiveWindowQueries;
    /// <summary>
    /// 画面外に出る場合は位置やサイズを変更しない (無効「false」/有効「true」)
    /// </summary>
    [DataMember]
    public bool DoNotChangeOutOfScreen;
    /// <summary>
    /// 追加/修正のウィンドウが表示されている場合は処理停止 (無効「false」/有効「true」)
    /// </summary>
    [DataMember]
    public bool StopProcessingShowAddModifyWindow;
    /// <summary>
    /// 追加/修正ウィンドウのサイズ
    /// </summary>
    [DataMember]
    public System.Drawing.Size AddModifyWindowSize;
    /// <summary>
    /// ウィンドウ情報の取得項目
    /// </summary>
    [DataMember]
    public WindowInformationToBeRetrieved AcquiredItems;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public SpecifiedWindowBaseInformation()
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
        HotkeysDoNotStopFullScreen = false;
        StopProcessingFullScreen = false;
        RegisterMultiple = false;
        CaseSensitiveWindowQueries = true;
        DoNotChangeOutOfScreen = true;
        StopProcessingShowAddModifyWindow = true;
        AddModifyWindowSize = new();
        AcquiredItems = new();
    }
}
