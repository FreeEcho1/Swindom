namespace Swindom;

/// <summary>
/// 「指定ウィンドウ」機能の情報
/// </summary>
public class SpecifyWindowInformation
{
    /// <summary>
    /// 処理状態 (無効「false」/有効「true」)
    /// </summary>
    public bool IsEnabled { get; set; }
    /// <summary>
    /// ウィンドウ処理を複数登録 (無効「false」/有効「true」)
    /// </summary>
    public bool MultipleRegistrations { get; set; }
    /// <summary>
    /// ウィンドウ判定で大文字と小文字を区別する (無効「false」/有効「true」)
    /// </summary>
    public bool CaseSensitive { get; set; }
    /// <summary>
    /// 画面外に出る場合は位置やサイズを変更しない (無効「false」/有効「true」)
    /// </summary>
    public bool DoNotChangeOutOfScreen { get; set; }
    /// <summary>
    /// 追加/修正ウィンドウが表示されている場合は処理停止 (無効「false」/有効「true」)
    /// </summary>
    public bool StopProcessingShowAddModifyWindow { get; set; }
    /// <summary>
    /// 全画面のウィンドウが存在する場合は処理停止 (無効「false」/有効「true」)
    /// </summary>
    public bool StopProcessingFullScreen { get; set; }
    /// <summary>
    /// ホットキーは除外 (無効「false」/有効「true」)
    /// </summary>
    public bool HotkeysDoNotStopFullScreen { get; set; }
    /// <summary>
    /// 処理間隔の最小値
    /// </summary>
    [JsonIgnore]
    public static int MinimumProcessingInterval { get; } = 10;
    /// <summary>
    /// 処理間隔の最大値
    /// </summary>
    [JsonIgnore]
    public static int MaximumProcessingInterval { get; } = 100000;
    /// <summary>
    /// 処理間隔 (ミリ秒)
    /// </summary>
    [JsonIgnore]
    private int PrivateProcessingInterval;
    /// <summary>
    /// 処理間隔 (ミリ秒)
    /// </summary>
    public int ProcessingInterval
    {
        get
        {
            return PrivateProcessingInterval;
        }
        set
        {
            if (value < MinimumProcessingInterval)
            {
                PrivateProcessingInterval = MinimumProcessingInterval;
            }
            if (MaximumProcessingInterval < value)
            {
                PrivateProcessingInterval = MaximumProcessingInterval;
            }
            else
            {
                PrivateProcessingInterval = value;
            }
        }
    }
    /// <summary>
    /// 処理するウィンドウの範囲
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public ProcessingWindowRange ProcessingWindowRange { get; set; }
    /// <summary>
    /// 次のウィンドウを処理する待ち時間の最小値
    /// </summary>
    [JsonIgnore]
    public static int MinimumWaitTimeToProcessingNextWindow { get; } = 0;
    /// <summary>
    /// 次のウィンドウを処理する待ち時間の最大値
    /// </summary>
    [JsonIgnore]
    public static int MaximumWaitTimeToProcessingNextWindow { get; } = 100000;
    /// <summary>
    /// 次のウィンドウを処理する待ち時間 (ミリ秒) (待たない「0」)
    /// </summary>
    [JsonIgnore]
    private int PrivateWaitTimeToProcessingNextWindow;
    /// <summary>
    /// 次のウィンドウを処理する待ち時間 (ミリ秒) (待たない「0」)
    /// </summary>
    public int WaitTimeToProcessingNextWindow
    {
        get
        {
            return PrivateWaitTimeToProcessingNextWindow;
        }
        set
        {
            if (value < MinimumWaitTimeToProcessingNextWindow)
            {
                PrivateWaitTimeToProcessingNextWindow = MinimumWaitTimeToProcessingNextWindow;
            }
            else if (MaximumWaitTimeToProcessingNextWindow < value)
            {
                PrivateWaitTimeToProcessingNextWindow = MaximumWaitTimeToProcessingNextWindow;
            }
            else
            {
                PrivateWaitTimeToProcessingNextWindow = value;
            }
        }
    }
    /// <summary>
    /// 追加/修正ウィンドウのサイズ
    /// </summary>
    public SizeInt AddModifyWindowSize { get; set; }
    /// <summary>
    /// 取得する情報
    /// </summary>
    public WindowInformationToBeRetrieved AcquiredItems { get; set; }
    /// <summary>
    /// 「指定ウィンドウ」機能の項目情報
    /// </summary>
    public List<SpecifyWindowItemInformation> Items { get; set; }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public SpecifyWindowInformation()
    {
        IsEnabled = false;
        MultipleRegistrations = false;
        CaseSensitive = true;
        DoNotChangeOutOfScreen = true;
        StopProcessingShowAddModifyWindow = true;
        StopProcessingFullScreen = false;
        HotkeysDoNotStopFullScreen = false;
        ProcessingInterval = 600;
        ProcessingWindowRange = ProcessingWindowRange.ActiveOnly;
        WaitTimeToProcessingNextWindow = MinimumWaitTimeToProcessingNextWindow;
        AddModifyWindowSize = new();
        AcquiredItems = new();
        Items = new();
    }
}
