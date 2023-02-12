namespace Swindom.Sources.SettingsData;

/// <summary>
/// 「貼り付ける位置をずらす」情報
/// </summary>
public class ShiftPastePosition
{
    /// <summary>
    /// 有効状態 (無効「false」/有効「true」)
    /// </summary>
    public bool Enabled;
    /// <summary>
    /// 距離の最小値
    /// </summary>
    public const int MinimumValue = -200;
    /// <summary>
    /// 距離の最大値
    /// </summary>
    public const int MaximumValue = 200;
    /// <summary>
    /// 左側の距離
    /// </summary>
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
        Enabled = false;
        Left = 0;
        Top = 0;
        Right = 0;
        Bottom = 0;
    }
}
