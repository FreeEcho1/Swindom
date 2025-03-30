namespace Swindom;

/// <summary>
/// ウィンドウ判定情報
/// </summary>
public class WindowJudgementInformation
{
    /// <summary>
    /// 登録名
    /// </summary>
    public string RegisteredName { get; set; }
    /// <summary>
    /// タイトル名
    /// </summary>
    public string TitleName { get; set; }
    /// <summary>
    /// タイトル名の一致条件
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public NameMatchCondition TitleNameMatchCondition { get; set; }
    /// <summary>
    /// クラス名
    /// </summary>
    public string ClassName { get; set; }
    /// <summary>
    /// クラス名の一致条件
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public NameMatchCondition ClassNameMatchCondition { get; set; }
    /// <summary>
    /// ファイル名
    /// </summary>
    public string FileName { get; set; }
    /// <summary>
    /// ファイル名の一致条件
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public FileNameMatchCondition FileNameMatchCondition { get; set; }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public WindowJudgementInformation()
    {
        RegisteredName = "";
        TitleName = "";
        TitleNameMatchCondition = NameMatchCondition.ExactMatch;
        ClassName = "";
        ClassNameMatchCondition = NameMatchCondition.ExactMatch;
        FileName = "";
        FileNameMatchCondition = FileNameMatchCondition.Include;
    }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="wji">ウィンドウ判定情報</param>
    public WindowJudgementInformation(
        WindowJudgementInformation wji
        )
    {
        RegisteredName = wji.RegisteredName;
        TitleName = wji.TitleName;
        TitleNameMatchCondition = wji.TitleNameMatchCondition;
        ClassName = wji.ClassName;
        ClassNameMatchCondition = wji.ClassNameMatchCondition;
        FileName = wji.FileName;
        FileNameMatchCondition = wji.FileNameMatchCondition;
    }
}
