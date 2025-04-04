﻿namespace Swindom;

/// <summary>
/// 「マグネット」機能の情報
/// </summary>
public class MagnetInformation
{
    /// <summary>
    /// 処理状態 (無効「false」/有効「true」)
    /// </summary>
    public bool IsEnabled { get; set; }
    /// <summary>
    /// 画面端に貼り付ける (無効「false」/有効「true」)
    /// </summary>
    public bool PasteToEdgeOfScreen { get; set; }
    /// <summary>
    /// 別のウィンドウに貼り付ける (無効「false」/有効「true」)
    /// </summary>
    public bool PasteToAnotherWindow { get; set; }
    /// <summary>
    /// キーを押した状態で貼り付ける (無効「false」/有効「true」)
    /// </summary>
    public bool PasteToPressKey { get; set; }
    /// <summary>
    /// キーを離すまで貼り付いた状態にする (無効「false」/有効「true」)
    /// </summary>
    public bool KeepPasteUntilKeyUp { get; set; }
    /// <summary>
    /// 貼り付ける判定距離の最小値
    /// </summary>
    [JsonIgnore]
    public static int MinimumPastingDecisionDistance { get; } = 1;
    /// <summary>
    /// 貼り付ける判定距離の最大値
    /// </summary>
    [JsonIgnore]
    public static int MaximumPastingDecisionDistance { get; } = 500;
    /// <summary>
    /// 貼り付ける判定距離
    /// </summary>
    [JsonIgnore]
    private int PrivatePastingDecisionDistance;
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
    /// 貼り付く時間の最小値
    /// </summary>
    [JsonIgnore]
    public static int MinimumPastingTime { get; } = 10;
    /// <summary>
    /// 貼り付く時間の最大値
    /// </summary>
    [JsonIgnore]
    public static int MaximumPastingTime { get; } = 10000;
    /// <summary>
    /// 貼り付く時間 (ミリ秒)
    /// </summary>
    [JsonIgnore]
    private int PrivatePastingTime;
    /// <summary>
    /// 貼り付く時間 (ミリ秒)
    /// </summary>
    public int PastingTime
    {
        get
        {
            return PrivatePastingTime;
        }
        set
        {
            if (value < MinimumPastingTime)
            {
                PrivatePastingTime = MinimumPastingTime;
            }
            else if (MaximumPastingTime < value)
            {
                PrivatePastingTime = MaximumPastingTime;
            }
            else
            {
                PrivatePastingTime = value;
            }
        }
    }
    /// <summary>
    /// 全画面のウィンドウが存在する場合は処理停止 (無効「false」/有効「true」)
    /// </summary>
    public bool StopProcessingFullScreen { get; set; }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public MagnetInformation()
    {
        IsEnabled = false;
        PasteToEdgeOfScreen = false;
        PasteToAnotherWindow = false;
        PasteToPressKey = false;
        PastingDecisionDistance = 10;
        PastingTime = 400;
        StopProcessingFullScreen = false;
    }
}
