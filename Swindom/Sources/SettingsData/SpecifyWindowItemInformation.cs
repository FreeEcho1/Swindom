namespace Swindom;

/// <summary>
/// 「指定ウィンドウ」機能の項目情報
/// </summary>
public class SpecifyWindowItemInformation
{
    /// <summary>
    /// 有効状態 (無効「false」/有効「true」)
    /// </summary>
    public bool IsEnabled { get; set; }
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
    /// 基準にするディスプレイ
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public StandardDisplay StandardDisplay { get; set; }
    /// <summary>
    /// 一度だけ処理
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ProcessingOnlyOnce ProcessingOnlyOnce { get; set; }
    /// <summary>
    /// ウィンドウイベントのデータ
    /// </summary>
    public WindowEventData WindowEventData { get; set; }
    /// <summary>
    /// タイマー処理
    /// </summary>
    public bool TimerProcessing { get; set; }
    /// <summary>
    /// 最初に処理しない回数の最小値
    /// </summary>
    [JsonIgnore]
    public static int MinimumNumberOfTimesNotToProcessingFirst { get; } = 0;
    /// <summary>
    /// 最初に処理しない回数の最大値
    /// </summary>
    [JsonIgnore]
    public static int MaximumNumberOfTimesNotToProcessingFirst { get; } = 1000;
    /// <summary>
    /// 最初に処理しない回数 (指定しない「0」)
    /// </summary>
    [JsonIgnore]
    private int _numberOfTimesNotToProcessingFirst;
    /// <summary>
    /// 最初に処理しない回数 (指定しない「0」)
    /// </summary>
    public int NumberOfTimesNotToProcessingFirst
    {
        get
        {
            return _numberOfTimesNotToProcessingFirst;
        }
        set
        {
            if (value < MinimumNumberOfTimesNotToProcessingFirst)
            {
                _numberOfTimesNotToProcessingFirst = MinimumNumberOfTimesNotToProcessingFirst;
            }
            else if (MaximumNumberOfTimesNotToProcessingFirst < value)
            {
                _numberOfTimesNotToProcessingFirst = MaximumNumberOfTimesNotToProcessingFirst;
            }
            else
            {
                _numberOfTimesNotToProcessingFirst = value;
            }
        }
    }
    /// <summary>
    /// ウィンドウの処理情報
    /// </summary>
    public List<WindowProcessingInformation> WindowProcessingInformation { get; set; }
    /// <summary>
    /// 「処理しない条件」の「子ウィンドウ」
    /// </summary>
    public bool DoNotProcessingChildWindow { get; set; }
    /// <summary>
    /// 「処理しない条件」の「タイトル名の条件」
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public TitleNameProcessingConditions DoNotProcessingTitleNameConditions { get; set; }
    /// <summary>
    /// 「処理しない条件」の「タイトル名に含まれる文字列」
    /// </summary>
    public List<string> DoNotProcessingStringContainedInTitleName { get; set; }
    /// <summary>
    /// 「処理しない条件」の「サイズ」
    /// </summary>
    public List<SizeInt> DoNotProcessingSize { get; set; }
    /// <summary>
    /// 「処理しない条件」の「指定したサイズ以外」
    /// </summary>
    public List<SizeInt> DoNotProcessingOtherThanSpecifiedSize { get; set; }
    /// <summary>
    /// 「処理しない条件」の「指定バージョン以外」
    /// </summary>
    public string DoNotProcessingOtherThanSpecifiedVersion { get; set; }
    /// <summary>
    /// 「通知」の「通知」
    /// </summary>
    public bool Notification { get; set; }
    /// <summary>
    /// 「通知」の「指定バージョン以外」
    /// </summary>
    public string NotificationOtherThanSpecifiedVersion { get; set; }
    /// <summary>
    /// 「処理しない条件」の「指定したバージョン以外」と同期
    /// </summary>
    public bool NotificationSynchronizationVersion { get; set; } = false;

    /// <summary>
    /// 一度だけ処理が終わっているかの値 (終わってない「false」/終わってる「true」)
    /// </summary>
    [JsonIgnore]
    public bool EndedProcessingOnlyOnce { get; set; }
    /// <summary>
    /// ウィンドウハンドル
    /// </summary>
    [JsonIgnore]
    public IntPtr Hwnd {  get; set; }
    /// <summary>
    /// カウントした最初に処理しない回数
    /// </summary>
    [JsonIgnore]
    public int CountNumberOfTimesNotToProcessingFirst { get; set; }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public SpecifyWindowItemInformation()
    {
        IsEnabled = true;
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
        WindowProcessingInformation = [];
        DoNotProcessingChildWindow = false;
        DoNotProcessingTitleNameConditions = TitleNameProcessingConditions.NotSpecified;
        DoNotProcessingStringContainedInTitleName = [];
        DoNotProcessingSize = [];
        DoNotProcessingOtherThanSpecifiedSize = [];
        DoNotProcessingOtherThanSpecifiedVersion = "";
        Notification = false;
        NotificationOtherThanSpecifiedVersion = "";
        NotificationSynchronizationVersion = false;
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
        IsEnabled = item.IsEnabled;
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
        WindowProcessingInformation = [];
        foreach (WindowProcessingInformation nowWPI in item.WindowProcessingInformation)
        {
            WindowProcessingInformation.Add(new WindowProcessingInformation(nowWPI, copyHotkey));
        }
        DoNotProcessingChildWindow = item.DoNotProcessingChildWindow;
        DoNotProcessingTitleNameConditions = item.DoNotProcessingTitleNameConditions;
        DoNotProcessingStringContainedInTitleName = [..item.DoNotProcessingStringContainedInTitleName];
        DoNotProcessingSize = [];
        foreach (SizeInt nowSize in item.DoNotProcessingSize)
        {
            DoNotProcessingSize.Add(nowSize);
        }
        DoNotProcessingOtherThanSpecifiedSize = [];
        foreach (SizeInt nowSize in item.DoNotProcessingOtherThanSpecifiedSize)
        {
            DoNotProcessingOtherThanSpecifiedSize.Add(nowSize);
        }
        DoNotProcessingOtherThanSpecifiedVersion = item.DoNotProcessingOtherThanSpecifiedVersion;
        Notification = item.Notification;
        NotificationOtherThanSpecifiedVersion = item.NotificationOtherThanSpecifiedVersion;
        NotificationSynchronizationVersion = item.NotificationSynchronizationVersion;
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
        IsEnabled = item.IsEnabled;
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
        WindowProcessingInformation = [];
        foreach (WindowProcessingInformation nowWPI in item.WindowProcessingInformation)
        {
            WindowProcessingInformation.Add(new WindowProcessingInformation(nowWPI, copyHotkey));
        }
        DoNotProcessingChildWindow = item.DoNotProcessingChildWindow;
        DoNotProcessingTitleNameConditions = item.DoNotProcessingTitleNameConditions;
        DoNotProcessingStringContainedInTitleName = [..item.DoNotProcessingStringContainedInTitleName];
        DoNotProcessingSize = [];
        foreach (SizeInt nowSize in item.DoNotProcessingSize)
        {
            DoNotProcessingSize.Add(nowSize);
        }
        DoNotProcessingOtherThanSpecifiedSize = [];
        foreach (SizeInt nowSize in item.DoNotProcessingOtherThanSpecifiedSize)
        {
            DoNotProcessingOtherThanSpecifiedSize.Add(nowSize);
        }
        DoNotProcessingOtherThanSpecifiedVersion = item.DoNotProcessingOtherThanSpecifiedVersion;
        Notification = item.Notification;
        NotificationOtherThanSpecifiedVersion = item.NotificationOtherThanSpecifiedVersion;
        NotificationSynchronizationVersion = item.NotificationSynchronizationVersion;
        EndedProcessingOnlyOnce = false;
        Hwnd = IntPtr.Zero;
        CountNumberOfTimesNotToProcessingFirst = 0;
    }
}
