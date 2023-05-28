namespace Swindom;

/// <summary>
/// ウィンドウ判定情報
/// </summary>
public class WindowJudgementInformation
{
    /// <summary>
    /// 登録名
    /// </summary>
    public string RegisteredName;
    /// <summary>
    /// タイトル名
    /// </summary>
    public string TitleName;
    /// <summary>
    /// タイトル名の一致条件
    /// </summary>
    public NameMatchCondition TitleNameMatchCondition;
    /// <summary>
    /// クラス名
    /// </summary>
    public string ClassName;
    /// <summary>
    /// クラス名の一致条件
    /// </summary>
    public NameMatchCondition ClassNameMatchCondition;
    /// <summary>
    /// ファイル名
    /// </summary>
    public string FileName;
    /// <summary>
    /// ファイル名の一致条件
    /// </summary>
    public FileNameMatchCondition FileNameMatchCondition;

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
