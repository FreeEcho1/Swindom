namespace Swindom;

/// <summary>
/// 「指定ウィンドウ」機能の項目情報
/// </summary>
public class SpecifyWindowItemInformation
{
    /// <summary>
    /// 有効状態 (無効「false」/有効「true」)
    /// </summary>
    public bool Enabled;
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
    /// 基準にするディスプレイ
    /// </summary>
    public StandardDisplay StandardDisplay;
    /// <summary>
    /// 一度だけ処理
    /// </summary>
    public ProcessingOnlyOnce ProcessingOnlyOnce;
    /// <summary>
    /// ウィンドウイベントのデータ
    /// </summary>
    public WindowEventData WindowEventData;
    /// <summary>
    /// タイマー処理
    /// </summary>
    public bool TimerProcessing;
    /// <summary>
    /// 最初に処理しない回数 (指定しない「0」)
    /// </summary>
    public int PrivateNumberOfTimesNotToProcessingFirst;
    /// <summary>
    /// 最初に処理しない回数の最小値
    /// </summary>
    public const int MinimumNumberOfTimesNotToProcessingFirst = 0;
    /// <summary>
    /// 最初に処理しない回数の最大値
    /// </summary>
    public const int MaximumNumberOfTimesNotToProcessingFirst = 1000;
    /// <summary>
    /// 最初に処理しない回数 (指定しない「0」)
    /// </summary>
    public int NumberOfTimesNotToProcessingFirst
    {
        get
        {
            return PrivateNumberOfTimesNotToProcessingFirst;
        }
        set
        {
            if (value < MinimumNumberOfTimesNotToProcessingFirst)
            {
                PrivateNumberOfTimesNotToProcessingFirst = MinimumNumberOfTimesNotToProcessingFirst;
            }
            else if (MaximumNumberOfTimesNotToProcessingFirst < value)
            {
                PrivateNumberOfTimesNotToProcessingFirst = MaximumNumberOfTimesNotToProcessingFirst;
            }
            else
            {
                PrivateNumberOfTimesNotToProcessingFirst = value;
            }
        }
    }
    /// <summary>
    /// ウィンドウの処理情報
    /// </summary>
    public List<WindowProcessingInformation> WindowProcessingInformation;
    /// <summary>
    /// 「処理しない条件」の「タイトル名の条件」
    /// </summary>
    public TitleNameProcessingConditions DoNotProcessingTitleNameConditions;
    /// <summary>
    /// 「処理しない条件」の「タイトル名に含まれる文字列」
    /// </summary>
    public List<string> DoNotProcessingStringContainedInTitleName;
    /// <summary>
    /// 「処理しない条件」の「サイズ」
    /// </summary>
    public List<System.Drawing.Size> DoNotProcessingSize;
    /// <summary>
    /// 「処理しない条件」の「指定バージョン以外」
    /// </summary>
    public string DoNotProcessingOtherThanSpecifiedVersion;
    /// <summary>
    /// 「処理しない条件」の「指定バージョン以外」の「知らせる」
    /// </summary>
    public bool DoNotProcessingOtherThanSpecifiedVersionAnnounce;

    // ------------------ 設定ファイルに保存しないデータ ------------------ //
    /// <summary>
    /// 一度だけ処理が終わっているかの値 (終わってない「false」/終わってる「true」)
    /// </summary>
    public bool EndedProcessingOnlyOnce;
    /// <summary>
    /// ウィンドウハンドル
    /// </summary>
    public IntPtr Hwnd;
    /// <summary>
    /// カウントした最初に処理しない回数
    /// </summary>
    public int CountNumberOfTimesNotToProcessingFirst;
    // ------------------ 設定ファイルに保存しないデータ ------------------ //

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public SpecifyWindowItemInformation()
    {
        Enabled = true;
        RegisteredName = "";
        TitleName = "";
        TitleNameMatchCondition = NameMatchCondition.ExactMatch;
        ClassName = "";
        ClassNameMatchCondition = NameMatchCondition.ExactMatch;
        FileName = "";
        FileNameMatchCondition = FileNameMatchCondition.Include;
        StandardDisplay = StandardDisplay.SpecifiedDisplay;
        ProcessingOnlyOnce = ProcessingOnlyOnce.NotSpecified;
        WindowEventData = new();
        TimerProcessing = false;
        NumberOfTimesNotToProcessingFirst = MinimumNumberOfTimesNotToProcessingFirst;
        WindowProcessingInformation = new();
        DoNotProcessingTitleNameConditions = TitleNameProcessingConditions.NotSpecified;
        DoNotProcessingStringContainedInTitleName = new();
        DoNotProcessingSize = new();
        DoNotProcessingOtherThanSpecifiedVersion = "";
        DoNotProcessingOtherThanSpecifiedVersionAnnounce = false;
        EndedProcessingOnlyOnce = false;
        Hwnd = IntPtr.Zero;
        CountNumberOfTimesNotToProcessingFirst = 0;
    }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="item">情報</param>
    /// <param name="copyHotkey">ホットキーをコピーするかの値 (コピーしない「false」/コピーする「true」)</param>
    public SpecifyWindowItemInformation(
        SpecifyWindowItemInformation item,
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
        StandardDisplay = item.StandardDisplay;
        ProcessingOnlyOnce = item.ProcessingOnlyOnce;
        WindowEventData = new(item.WindowEventData);
        TimerProcessing = item.TimerProcessing;
        NumberOfTimesNotToProcessingFirst = item.NumberOfTimesNotToProcessingFirst;
        WindowProcessingInformation = new();
        foreach (WindowProcessingInformation nowWPI in item.WindowProcessingInformation)
        {
            WindowProcessingInformation.Add(new WindowProcessingInformation(nowWPI, copyHotkey));
        }
        DoNotProcessingTitleNameConditions = item.DoNotProcessingTitleNameConditions;
        DoNotProcessingStringContainedInTitleName = new(item.DoNotProcessingStringContainedInTitleName);
        DoNotProcessingSize = new();
        foreach (System.Drawing.Size nowSize in item.DoNotProcessingSize)
        {
            DoNotProcessingSize.Add(nowSize);
        }
        DoNotProcessingOtherThanSpecifiedVersion = item.DoNotProcessingOtherThanSpecifiedVersion;
        DoNotProcessingOtherThanSpecifiedVersionAnnounce = item.DoNotProcessingOtherThanSpecifiedVersionAnnounce;
        EndedProcessingOnlyOnce = false;
        Hwnd = IntPtr.Zero;
        CountNumberOfTimesNotToProcessingFirst = 0;
    }


    /// <summary>
    /// コピー
    /// </summary>
    /// <param name="item">情報</param>
    /// <param name="copyHotkey">ホットキーをコピーするかの値 (コピーしない「false」/コピーする「true」)</param>
    public void Copy(
        SpecifyWindowItemInformation item,
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
        StandardDisplay = item.StandardDisplay;
        ProcessingOnlyOnce = item.ProcessingOnlyOnce;
        WindowEventData = new(item.WindowEventData);
        TimerProcessing = item.TimerProcessing;
        NumberOfTimesNotToProcessingFirst = item.NumberOfTimesNotToProcessingFirst;
        WindowProcessingInformation = new();
        foreach (WindowProcessingInformation nowWPI in item.WindowProcessingInformation)
        {
            WindowProcessingInformation.Add(new WindowProcessingInformation(nowWPI, copyHotkey));
        }
        DoNotProcessingTitleNameConditions = item.DoNotProcessingTitleNameConditions;
        DoNotProcessingStringContainedInTitleName = new(item.DoNotProcessingStringContainedInTitleName);
        DoNotProcessingSize = new();
        foreach (System.Drawing.Size nowSize in item.DoNotProcessingSize)
        {
            DoNotProcessingSize.Add(nowSize);
        }
        DoNotProcessingOtherThanSpecifiedVersion = item.DoNotProcessingOtherThanSpecifiedVersion;
        DoNotProcessingOtherThanSpecifiedVersionAnnounce = item.DoNotProcessingOtherThanSpecifiedVersionAnnounce;
        EndedProcessingOnlyOnce = false;
        Hwnd = IntPtr.Zero;
        CountNumberOfTimesNotToProcessingFirst = 0;
    }
}
