namespace Swindom;

/// <summary>
/// 「貼り付ける位置をずらす」情報
/// </summary>
public class ShiftPastePosition
{
    /// <summary>
    /// 有効状態 (無効「false」/有効「true」)
    /// </summary>
    public bool IsEnabled { get; set; }
    /// <summary>
    /// 距離の最小値
    /// </summary>
    [JsonIgnore]
    public static int MinimumValue { get; } = -200;
    /// <summary>
    /// 距離の最大値
    /// </summary>
    [JsonIgnore]
    public static int MaximumValue { get; } = 200;
    /// <summary>
    /// 左側の距離
    /// </summary>
    [JsonIgnore]
    private int PrivateLeft;
    /// <summary>
    /// 左側の距離
    /// </summary>
    public int Left
    {
        get
        {
            return PrivateLeft;
        }
        set
        {
            if (value < MinimumValue)
            {
                PrivateLeft = MinimumValue;
            }
            else if (MaximumValue < value)
            {
                PrivateLeft = MaximumValue;
            }
            else
            {
                PrivateLeft = value;
            }
        }
    }
    /// <summary>
    /// 上側の距離
    /// </summary>
    [JsonIgnore]
    private int PrivateTop;
    /// <summary>
    /// 上側の距離
    /// </summary>
    public int Top
    {
        get
        {
            return PrivateTop;
        }
        set
        {
            if (value < MinimumValue)
            {
                PrivateTop = MinimumValue;
            }
            else if (MaximumValue < value)
            {
                PrivateTop = MaximumValue;
            }
            else
            {
                PrivateTop = value;
            }
        }
    }
    /// <summary>
    /// 右側の距離
    /// </summary>
    [JsonIgnore]
    private int PrivateRight;
    /// <summary>
    /// 右側の距離
    /// </summary>
    public int Right
    {
        get
        {
            return PrivateRight;
        }
        set
        {
            if (value < MinimumValue)
            {
                PrivateRight = MinimumValue;
            }
            else if (MaximumValue < value)
            {
                PrivateRight = MaximumValue;
            }
            else
            {
                PrivateRight = value;
            }
        }
    }
    /// <summary>
    /// 下側の距離
    /// </summary>
    [JsonIgnore]
    private int PrivateBottom;
    /// <summary>
    /// 下側の距離
    /// </summary>
    public int Bottom
    {
        get
        {
            return PrivateBottom;
        }
        set
        {
            if (value < MinimumValue)
            {
                PrivateBottom = MinimumValue;
            }
            else if (MaximumValue < value)
            {
                PrivateBottom = MaximumValue;
            }
            else
            {
                PrivateBottom = value;
            }
        }
    }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public ShiftPastePosition()
    {
        IsEnabled = false;
        Left = 0;
        Top = 0;
        Right = 0;
        Bottom = 0;
    }
}
