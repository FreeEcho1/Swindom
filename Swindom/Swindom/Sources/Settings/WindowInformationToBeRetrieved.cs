namespace Swindom;

/// <summary>
/// 取得するウィンドウ情報
/// </summary>
[DataContract]
public class WindowInformationToBeRetrieved : IExtensibleDataObject
{
    /// <summary>
    /// タイトル名
    /// </summary>
    [DataMember]
    public bool TitleName;
    /// <summary>
    /// クラス名
    /// </summary>
    [DataMember]
    public bool ClassName;
    /// <summary>
    /// ファイル名
    /// </summary>
    [DataMember]
    public bool FileName;
    /// <summary>
    /// ウィンドウの状態
    /// </summary>
    [DataMember]
    public bool WindowState;
    /// <summary>
    /// X
    /// </summary>
    [DataMember]
    public bool X;
    /// <summary>
    /// Y
    /// </summary>
    [DataMember]
    public bool Y;
    /// <summary>
    /// 幅
    /// </summary>
    [DataMember]
    public bool Width;
    /// <summary>
    /// 高さ
    /// </summary>
    [DataMember]
    public bool Height;
    /// <summary>
    /// ディスプレイ
    /// </summary>
    [DataMember]
    public bool Display;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public WindowInformationToBeRetrieved()
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
        TitleName = true;
        ClassName = true;
        FileName = true;
        WindowState = true;
        X = true;
        Y = true;
        Width = true;
        Height = true;
        Display = true;
    }
}
