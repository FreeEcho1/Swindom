namespace Swindom;

/// <summary>
/// 「指定ウィンドウ」機能の基礎の項目情報
/// </summary>
[DataContract]
public class SpecifiedWindowBaseItemInformation : IExtensibleDataObject
{
    /// <summary>
    /// 有効状態 (無効「false」/有効「true」)
    /// </summary>
    [DataMember]
    public bool Enabled;
    /// <summary>
    /// 登録名
    /// </summary>
    [DataMember]
    public string RegisteredName;
    /// <summary>
    /// タイトル名
    /// </summary>
    [DataMember]
    public string TitleName;
    /// <summary>
    /// タイトル名の一致条件
    /// </summary>
    [DataMember]
    public NameMatchCondition TitleNameMatchCondition;
    /// <summary>
    /// クラス名
    /// </summary>
    [DataMember]
    public string ClassName;
    /// <summary>
    /// クラス名の一致条件
    /// </summary>
    [DataMember]
    public NameMatchCondition ClassNameMatchCondition;
    /// <summary>
    /// ファイル名
    /// </summary>
    [DataMember]
    public string FileName;
    /// <summary>
    /// ファイル名の一致条件
    /// </summary>
    [DataMember]
    public FileNameMatchCondition FileNameMatchCondition;
    /// <summary>
    /// 1度だけ処理
    /// </summary>
    [DataMember]
    public ProcessingOnlyOnce ProcessingOnlyOnce;
    /// <summary>
    /// ウィンドウを閉じる (無効「false」/有効「true」)
    /// </summary>
    [DataMember]
    public bool CloseWindow;
    /// <summary>
    /// 基準にするディスプレイ
    /// </summary>
    [DataMember]
    public StandardDisplay StandardDisplay;
    /// <summary>
    /// タイトルの処理条件
    /// </summary>
    [DataMember]
    public TitleProcessingConditions TitleProcessingConditions;
    /// <summary>
    /// ウィンドウの処理情報
    /// </summary>
    [DataMember]
    public List<WindowProcessingInformation> WindowProcessingInformation;
    /// <summary>
    /// 処理しないウィンドウのサイズ
    /// </summary>
    [DataMember]
    public List<System.Drawing.Size> DoNotProcessingSize;
    /// <summary>
    /// 処理しないタイトル名の一部の文字列
    /// </summary>
    [DataMember]
    public List<string> DoNotProcessingTitleName;
    /// <summary>
    /// バージョン
    /// </summary>
    [DataMember]
    public string DifferentVersionVersion;
    /// <summary>
    /// 知らせる
    /// </summary>
    [DataMember]
    public bool DifferentVersionAnnounce;

    // ------------------ 設定ファイルに保存しないデータ ------------------ //
    /// <summary>
    /// 1度だけ処理が終わっているかの値 (終わってない「false」/終わってる「true」)
    /// </summary>
    [IgnoreDataMember]
    public bool EndedProcessingOnlyOnce;
    /// <summary>
    /// ウィンドウハンドル
    /// </summary>
    [IgnoreDataMember]
    public IntPtr Hwnd;
    // ------------------ 設定ファイルに保存しないデータ ------------------ //

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public SpecifiedWindowBaseItemInformation()
    {
        SetDefaultValue();
    }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="item">「指定ウィンドウ」機能の基礎の項目情報</param>
    /// <param name="copyHotkey">ホットキーをコピーするかの値 (いいえ「false」/はい「true」)</param>
    public SpecifiedWindowBaseItemInformation(
        SpecifiedWindowBaseItemInformation item,
        bool copyHotkey = true
        )
    {
        Enabled = item.Enabled;
        RegisteredName = item.RegisteredName;
        TitleName = item.TitleName;
        TitleNameMatchCondition = item.TitleNameMatchCondition;
        ClassName = item.ClassName;
        ClassNameMatchCondition = item.ClassNameMatchCondition;
        FileName = item.FileName;
        FileNameMatchCondition = item.FileNameMatchCondition;
        ProcessingOnlyOnce = item.ProcessingOnlyOnce;
        CloseWindow = item.CloseWindow;
        StandardDisplay = item.StandardDisplay;
        WindowProcessingInformation = new();
        foreach (WindowProcessingInformation nowWPI in item.WindowProcessingInformation)
        {
            WindowProcessingInformation.Add(new WindowProcessingInformation(nowWPI, copyHotkey));
        }
        TitleProcessingConditions = item.TitleProcessingConditions;
        DoNotProcessingSize = new();
        foreach (System.Drawing.Size nowSize in item.DoNotProcessingSize)
        {
            DoNotProcessingSize.Add(nowSize);
        }
        DoNotProcessingTitleName = new(item.DoNotProcessingTitleName);
        DifferentVersionVersion = item.DifferentVersionVersion;
        DifferentVersionAnnounce = item.DifferentVersionAnnounce;
        EndedProcessingOnlyOnce = false;
        Hwnd = IntPtr.Zero;
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
        Enabled = true;
        RegisteredName = "";
        TitleName = "";
        TitleNameMatchCondition = NameMatchCondition.ExactMatch;
        ClassName = "";
        ClassNameMatchCondition = NameMatchCondition.ExactMatch;
        FileName = "";
        FileNameMatchCondition = FileNameMatchCondition.Include;
        ProcessingOnlyOnce = ProcessingOnlyOnce.NotSpecified;
        CloseWindow = false;
        StandardDisplay = StandardDisplay.SpecifiedDisplay;
        WindowProcessingInformation = new();
        TitleProcessingConditions = TitleProcessingConditions.NotSpecified;
        DoNotProcessingSize = new();
        DoNotProcessingTitleName = new();
        DifferentVersionVersion = "";
        DifferentVersionAnnounce = false;
        EndedProcessingOnlyOnce = false;
        Hwnd = IntPtr.Zero;
    }
}
