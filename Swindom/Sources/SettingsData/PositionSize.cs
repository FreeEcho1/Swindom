﻿namespace Swindom;

/// <summary>
/// 位置とサイズ
/// </summary>
public class PositionSize
{
    /// <summary>
    /// ディスプレイ
    /// </summary>
    public string Display { get; set; }
    /// <summary>
    /// ウィンドウの状態
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public SettingsWindowState SettingsWindowState { get; set; }
    /// <summary>
    /// 位置のpxの最小値
    /// </summary>
    [JsonIgnore]
    public static double PositionPixelMinimum { get; } = double.MinValue;
    /// <summary>
    /// サイズのpxの最小値
    /// </summary>
    [JsonIgnore]
    public static double SizePixelMinimum { get; } = 0;
    /// <summary>
    /// 位置とサイズのpxの最大値
    /// </summary>
    [JsonIgnore]
    public static double PositionSizePixelMaximum { get; } = double.MaxValue;
    /// <summary>
    /// 位置とサイズのパーセントの最小値
    /// </summary>
    [JsonIgnore]
    public static double PositionSizePercentMinimum { get; } = 0;
    /// <summary>
    /// 位置とサイズのパーセントの最大値
    /// </summary>
    [JsonIgnore]
    public static double PositionSizePercentMaximum { get; } = 100;
    /// <summary>
    /// X
    /// </summary>
    [JsonIgnore]
    private double PrivateX;
    /// <summary>
    /// X
    /// </summary>
    public double X
    {
        get
        {
            return PrivateX;
        }
        set
        {
            if (XValueType == PositionSizeValueType.Percent)
            {
                if (value < PositionSizePercentMinimum)
                {
                    PrivateX = PositionSizePercentMinimum;
                }
                else if (PositionSizePercentMaximum < value)
                {
                    PrivateX = PositionSizePercentMaximum;
                }
                else
                {
                    PrivateX = value;
                }
            }
            else
            {
                if (value < PositionPixelMinimum)
                {
                    PrivateX = PositionPixelMinimum;
                }
                else if (PositionSizePixelMaximum < value)
                {
                    PrivateX = PositionSizePixelMaximum;
                }
                else
                {
                    PrivateX = value;
                }
            }
        }
    }
    /// <summary>
    /// Y
    /// </summary>
    [JsonIgnore]
    private double PrivateY;
    /// <summary>
    /// Y
    /// </summary>
    public double Y
    {
        get
        {
            return PrivateY;
        }
        set
        {
            if (YValueType == PositionSizeValueType.Percent)
            {
                if (value < PositionSizePercentMinimum)
                {
                    PrivateY = PositionSizePercentMinimum;
                }
                else if (PositionSizePercentMaximum < value)
                {
                    PrivateY = PositionSizePercentMaximum;
                }
                else
                {
                    PrivateY = value;
                }
            }
            else
            {
                if (value < PositionPixelMinimum)
                {
                    PrivateY = PositionPixelMinimum;
                }
                else if (PositionSizePixelMaximum < value)
                {
                    PrivateY = PositionSizePixelMaximum;
                }
                else
                {
                    PrivateY = value;
                }
            }
        }
    }
    /// <summary>
    /// Width
    /// </summary>
    [JsonIgnore]
    private double PrivateWidth;
    /// <summary>
    /// Width
    /// </summary>
    public double Width
    {
        get
        {
            return PrivateWidth;
        }
        set
        {
            if (WidthValueType == PositionSizeValueType.Percent)
            {
                if (value < PositionSizePercentMinimum)
                {
                    PrivateWidth = PositionSizePercentMinimum;
                }
                else if (PositionSizePercentMaximum < value)
                {
                    PrivateWidth = PositionSizePercentMaximum;
                }
                else
                {
                    PrivateWidth = value;
                }
            }
            else
            {
                if (value < SizePixelMinimum)
                {
                    PrivateWidth = SizePixelMinimum;
                }
                else if (PositionSizePixelMaximum < value)
                {
                    PrivateWidth = PositionSizePixelMaximum;
                }
                else
                {
                    PrivateWidth = value;
                }
            }
        }
    }
    /// <summary>
    /// Height
    /// </summary>
    [JsonIgnore]
    private double PrivateHeight;
    /// <summary>
    /// Height
    /// </summary>
    public double Height
    {
        get
        {
            return PrivateHeight;
        }
        set
        {
            if (HeightValueType == PositionSizeValueType.Percent)
            {
                if (value < PositionSizePercentMinimum)
                {
                    PrivateHeight = PositionSizePercentMinimum;
                }
                else if (PositionSizePercentMaximum < value)
                {
                    PrivateHeight = PositionSizePercentMaximum;
                }
                else
                {
                    PrivateHeight = value;
                }
            }
            else
            {
                if (value < SizePixelMinimum)
                {
                    PrivateHeight = SizePixelMinimum;
                }
                else if (PositionSizePixelMaximum < value)
                {
                    PrivateHeight = PositionSizePixelMaximum;
                }
                else
                {
                    PrivateHeight = value;
                }
            }
        }
    }
    /// <summary>
    /// ウィンドウのX位置指定の種類
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public WindowXType XType { get; set; }
    /// <summary>
    /// ウィンドウのY位置指定の種類
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public WindowYType YType { get; set; }
    /// <summary>
    /// ウィンドウの幅指定の種類
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public WindowSizeType WidthType { get; set; }
    /// <summary>
    /// ウィンドウの高さ指定の種類
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public WindowSizeType HeightType { get; set; }
    /// <summary>
    /// ウィンドウのX座標の値の種類
    /// </summary>
    [JsonIgnore]
    private PositionSizeValueType PrivateXValueType;
    /// <summary>
    /// ウィンドウのX座標の値の種類
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public PositionSizeValueType XValueType
    {
        get
        {
            return PrivateXValueType;
        }
        set
        {
            PrivateXValueType = value;
            X = PrivateX;
        }
    }
    /// <summary>
    /// ウィンドウのY座標の値の種類
    /// </summary>
    [JsonIgnore]
    private PositionSizeValueType PrivateYValueType;
    /// <summary>
    /// ウィンドウのY座標の値の種類
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public PositionSizeValueType YValueType
    {
        get
        {
            return PrivateYValueType;
        }
        set
        {
            PrivateYValueType = value;
            Y = PrivateY;
        }
    }
    /// <summary>
    /// ウィンドウの幅の値の種類
    /// </summary>
    [JsonIgnore]
    private PositionSizeValueType PrivateWidthValueType;
    /// <summary>
    /// ウィンドウの幅の値の種類
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public PositionSizeValueType WidthValueType
    {
        get
        {
            return PrivateWidthValueType;
        }
        set
        {
            PrivateWidthValueType = value;
            Width = PrivateWidth;
        }
    }
    /// <summary>
    /// ウィンドウの高さの値の種類
    /// </summary>
    [JsonIgnore]
    private PositionSizeValueType PrivateHeightValueType;
    /// <summary>
    /// ウィンドウの高さの値の種類
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public PositionSizeValueType HeightValueType
    {
        get
        {
            return PrivateHeightValueType;
        }
        set
        {
            PrivateHeightValueType = value;
            Height = PrivateHeight;
        }
    }
    /// <summary>
    /// クライアントエリアを対象とするかの値 (いいえ「false」/はい「true」)
    /// </summary>
    public bool ClientArea { get; set; }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public PositionSize()
    {
        Display = "";
        SettingsWindowState = SettingsWindowState.Normal;
        X = 0;
        Y = 0;
        Width = 0;
        Height = 0;
        XType = WindowXType.DoNotChange;
        YType = WindowYType.DoNotChange;
        WidthType = WindowSizeType.DoNotChange;
        HeightType = WindowSizeType.DoNotChange;
        XValueType = PositionSizeValueType.Pixel;
        YValueType = PositionSizeValueType.Pixel;
        WidthValueType = PositionSizeValueType.Pixel;
        HeightValueType = PositionSizeValueType.Pixel;
        ClientArea = false;
    }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="positionSize">PositionSize</param>
    public PositionSize(
        PositionSize positionSize
        )
    {
        Display = positionSize.Display;
        SettingsWindowState = positionSize.SettingsWindowState;
        X = positionSize.X;
        Y = positionSize.Y;
        Width = positionSize.Width;
        Height = positionSize.Height;
        XType = positionSize.XType;
        YType = positionSize.YType;
        WidthType = positionSize.WidthType;
        HeightType = positionSize.HeightType;
        XValueType = positionSize.XValueType;
        YValueType = positionSize.YValueType;
        WidthValueType = positionSize.WidthValueType;
        HeightValueType = positionSize.HeightValueType;
        ClientArea = positionSize.ClientArea;
    }
}
