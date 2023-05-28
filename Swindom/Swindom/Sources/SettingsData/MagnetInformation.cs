namespace Swindom;

/// <summary>
/// 「マグネット」機能の情報
/// </summary>
public class MagnetInformation
{
    /// <summary>
    /// 処理状態 (無効「false」/有効「true」)
    /// </summary>
    public bool Enabled;
    /// <summary>
    /// 画面端に貼り付ける (無効「false」/有効「true」)
    /// </summary>
    public bool PasteToEdgeOfScreen;
    /// <summary>
    /// 別のウィンドウに貼り付ける (無効「false」/有効「true」)
    /// </summary>
    public bool PasteToAnotherWindow;
    /// <summary>
    /// キーを押した状態で貼り付ける (無効「false」/有効「true」)
    /// </summary>
    public bool PasteToPressKey;
    /// <summary>
    /// 貼り付ける判定距離
    /// </summary>
    private int PrivatePastingDecisionDistance;
    /// <summary>
    /// 貼り付ける判定距離の最小値
    /// </summary>
    public const int MinimumPastingDecisionDistance = 1;
    /// <summary>
    /// 貼り付ける判定距離の最大値
    /// </summary>
    public const int MaximumPastingDecisionDistance = 500;
    /// <summary>
    /// 貼り付ける判定距離
    /// </summary>
    public int PastingDecisionDistance
    {
        get
        {
            return PrivatePastingDecisionDistance;
        }
        set
        {
            if (value < MinimumPastingDecisionDistance)
            {
                PrivatePastingDecisionDistance = MinimumPastingDecisionDistance;
            }
            else if (MaximumPastingDecisionDistance < value)
            {
                PrivatePastingDecisionDistance = MaximumPastingDecisionDistance;
            }
            else
            {
                PrivatePastingDecisionDistance = value;
            }
        }
    }
    /// <summary>
    /// 貼り付いた時の停止時間 (ミリ秒)
    /// </summary>
    private int PrivateStopTimeWhenPasted;
    /// <summary>
    /// 貼り付いた時の停止時間の最小値
    /// </summary>
    public const int MinimumStopTimeWhenPasted = 10;
    /// <summary>
    /// 貼り付いた時の停止時間の最大値
    /// </summary>
    public const int MaximumStopTimeWhenPasted = 10000;
    /// <summary>
    /// 貼り付いた時の停止時間 (ミリ秒)
    /// </summary>
    public int StopTimeWhenPasted
    {
        get
        {
            return PrivateStopTimeWhenPasted;
        }
        set
        {
            if (value < MinimumStopTimeWhenPasted)
            {
                PrivateStopTimeWhenPasted = MinimumStopTimeWhenPasted;
            }
            else if (MaximumStopTimeWhenPasted < value)
            {
                PrivateStopTimeWhenPasted = MaximumStopTimeWhenPasted;
            }
            else
            {
                PrivateStopTimeWhenPasted = value;
            }
        }
    }
    /// <summary>
    /// ウィンドウが全画面表示の場合は処理停止 (無効「false」/有効「true」)
    /// </summary>
    public bool StopProcessingFullScreen;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public MagnetInformation()
    {
        Enabled = false;
        PasteToEdgeOfScreen = false;
        PasteToAnotherWindow = false;
        PasteToPressKey = false;
        PastingDecisionDistance = 10;
        StopTimeWhenPasted = 400;
        StopProcessingFullScreen = false;
    }
}
