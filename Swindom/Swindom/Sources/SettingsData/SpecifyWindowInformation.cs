namespace Swindom.Sources.SettingsData;

/// <summary>
/// 「指定ウィンドウ」機能の情報
/// </summary>
public class SpecifyWindowInformation
{
    /// <summary>
    /// 処理状態 (無効「false」/有効「true」)
    /// </summary>
    public bool Enabled;
    /// <summary>
    /// ウィンドウ処理を複数登録 (無効「false」/有効「true」)
    /// </summary>
    public bool MultipleRegistrations;
    /// <summary>
    /// ウィンドウ判定で大文字と小文字を区別する (無効「false」/有効「true」)
    /// </summary>
    public bool CaseSensitive;
    /// <summary>
    /// 画面外に出る場合は位置やサイズを変更しない (無効「false」/有効「true」)
    /// </summary>
    public bool DoNotChangeOutOfScreen;
    /// <summary>
    /// 追加/修正のウィンドウが表示されている場合は処理停止 (無効「false」/有効「true」)
    /// </summary>
    public bool StopProcessingShowAddModifyWindow;
    /// <summary>
    /// ウィンドウが全画面表示の場合は処理停止 (無効「false」/有効「true」)
    /// </summary>
    public bool StopProcessingFullScreen;
    /// <summary>
    /// ホットキーは停止させない (全画面ウィンドウがある場合) (無効「false」/有効「true」)
    /// </summary>
    public bool HotkeysDoNotStopFullScreen;
    /// <summary>
    /// 処理間隔 (ミリ秒)
    /// </summary>
    private int PrivateProcessingInterval;
    /// <summary>
    /// 処理間隔の最小値
    /// </summary>
    public const int MinimumProcessingInterval = 10;
    /// <summary>
    /// 処理間隔の最大値
    /// </summary>
    public const int MaximumProcessingInterval = 100000;
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
    public ProcessingWindowRange ProcessingWindowRange;
    /// <summary>
    /// 次のウィンドウを処理する待ち時間 (ミリ秒) (待たない「0」)
    /// </summary>
    private int PrivateWaitTimeToProcessingNextWindow;
    /// <summary>
    /// 次のウィンドウを処理する待ち時間の最小値
    /// </summary>
    public const int MinimumWaitTimeToProcessingNextWindow = 0;
    /// <summary>
    /// 次のウィンドウを処理する待ち時間の最大値
    /// </summary>
    public const int MaximumWaitTimeToProcessingNextWindow = 100000;
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
    public System.Drawing.Size AddModifyWindowSize;
    /// <summary>
    /// 取得する情報
    /// </summary>
    public WindowInformationToBeRetrieved AcquiredItems;
    /// <summary>
    /// 「指定ウィンドウ」機能の項目情報
    /// </summary>
    public List<SpecifyWindowItemInformation> Items;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public SpecifyWindowInformation()
    {
        Enabled = false;
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
