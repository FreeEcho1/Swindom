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
    private int _left;
    /// <summary>
    /// 左側の距離
    /// </summary>
    public int Left
    {
        get
        {
            return _left;
        }
        set
        {
            if (value < MinimumValue)
            {
                _left = MinimumValue;
            }
            else if (MaximumValue < value)
            {
                _left = MaximumValue;
            }
            else
            {
                _left = value;
            }
        }
    }
    /// <summary>
    /// 上側の距離
    /// </summary>
    [JsonIgnore]
    private int _top;
    /// <summary>
    /// 上側の距離
    /// </summary>
    public int Top
    {
        get
        {
            return _top;
        }
        set
        {
            if (value < MinimumValue)
            {
                _top = MinimumValue;
            }
            else if (MaximumValue < value)
            {
                _top = MaximumValue;
            }
            else
            {
                _top = value;
            }
        }
    }
    /// <summary>
    /// 右側の距離
    /// </summary>
    [JsonIgnore]
    private int _right;
    /// <summary>
    /// 右側の距離
    /// </summary>
    public int Right
    {
        get
        {
            return _right;
        }
        set
        {
            if (value < MinimumValue)
            {
                _right = MinimumValue;
            }
            else if (MaximumValue < value)
            {
                _right = MaximumValue;
            }
            else
            {
                _right = value;
            }
        }
    }
    /// <summary>
    /// 下側の距離
    /// </summary>
    [JsonIgnore]
    private int _bottom;
    /// <summary>
    /// 下側の距離
    /// </summary>
    public int Bottom
    {
        get
        {
            return _bottom;
        }
        set
        {
            if (value < MinimumValue)
            {
                _bottom = MinimumValue;
            }
            else if (MaximumValue < value)
            {
                _bottom = MaximumValue;
            }
            else
            {
                _bottom = value;
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
