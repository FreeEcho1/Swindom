namespace Swindom;

/// <summary>
/// 位置とサイズ
/// </summary>
public class PositionSize
{
    /// <summary>
    /// ディスプレイ
    /// </summary>
    [JsonIgnore]
    private string _display = "";
    /// <summary>
    /// ディスプレイ
    /// </summary>
    public string Display
    {
        get
        {
            return _display;
        }
        set
        {
            if (string.IsNullOrEmpty(value))
            {
                if (ApplicationData.MonitorInformation == null
                    || ApplicationData.MonitorInformation.MonitorInfo.Count <= 0)
                {
                    MonitorInformation monitorInformation = MonitorInformation.GetMonitorInformation();     // モニター情報
                    if (monitorInformation.MonitorInfo.Count > 0)
                    {
                        _display = monitorInformation.MonitorInfo[0].DeviceName;
                    }
                }
                else
                {
                    _display = ApplicationData.MonitorInformation.MonitorInfo[0].DeviceName;
                }
            }
            else
            {
                _display = value;
            }
        }
    }
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
    private double _x;
    /// <summary>
    /// X
    /// </summary>
    public double X
    {
        get
        {
            return _x;
        }
        set
        {
            if (XValueType == PositionSizeValueType.Percent)
            {
                if (double.IsNaN(value))
                {
                    _x = 0;
                }
                else if (value < PositionSizePercentMinimum)
                {
                    _x = PositionSizePercentMinimum;
                }
                else if (PositionSizePercentMaximum < value)
                {
                    _x = PositionSizePercentMaximum;
                }
                else
                {
                    _x = value;
                }
            }
            else
            {
                if (double.IsNaN(value))
                {
                    _x = 0;
                }
                else if (value < PositionPixelMinimum)
                {
                    _x = PositionPixelMinimum;
                }
                else if (PositionSizePixelMaximum < value)
                {
                    _x = PositionSizePixelMaximum;
                }
                else
                {
                    _x = value;
                }
            }
        }
    }
    /// <summary>
    /// Y
    /// </summary>
    [JsonIgnore]
    private double _y;
    /// <summary>
    /// Y
    /// </summary>
    public double Y
    {
        get
        {
            return _y;
        }
        set
        {
            if (YValueType == PositionSizeValueType.Percent)
            {
                if (double.IsNaN(value))
                {
                    _y = 0;
                }
                else if (value < PositionSizePercentMinimum)
                {
                    _y = PositionSizePercentMinimum;
                }
                else if (PositionSizePercentMaximum < value)
                {
                    _y = PositionSizePercentMaximum;
                }
                else
                {
                    _y = value;
                }
            }
            else
            {
                if (double.IsNaN(value))
                {
                    _y = 0;
                }
                else if (value < PositionPixelMinimum)
                {
                    _y = PositionPixelMinimum;
                }
                else if (PositionSizePixelMaximum < value)
                {
                    _y = PositionSizePixelMaximum;
                }
                else
                {
                    _y = value;
                }
            }
        }
    }
    /// <summary>
    /// Width
    /// </summary>
    [JsonIgnore]
    private double _width;
    /// <summary>
    /// Width
    /// </summary>
    public double Width
    {
        get
        {
            return _width;
        }
        set
        {
            if (WidthValueType == PositionSizeValueType.Percent)
            {
                if (double.IsNaN(value))
                {
                    _width = 0;
                }
                else if (value < PositionSizePercentMinimum)
                {
                    _width = PositionSizePercentMinimum;
                }
                else if (PositionSizePercentMaximum < value)
                {
                    _width = PositionSizePercentMaximum;
                }
                else
                {
                    _width = value;
                }
            }
            else
            {
                if (double.IsNaN(value))
                {
                    _width = 0;
                }
                else if (value < SizePixelMinimum)
                {
                    _width = SizePixelMinimum;
                }
                else if (PositionSizePixelMaximum < value)
                {
                    _width = PositionSizePixelMaximum;
                }
                else
                {
                    _width = value;
                }
            }
        }
    }
    /// <summary>
    /// Height
    /// </summary>
    [JsonIgnore]
    private double _height;
    /// <summary>
    /// Height
    /// </summary>
    public double Height
    {
        get
        {
            return _height;
        }
        set
        {
            if (HeightValueType == PositionSizeValueType.Percent)
            {
                if (double.IsNaN(value))
                {
                    _height = 0;
                }
                else if (value < PositionSizePercentMinimum)
                {
                    _height = PositionSizePercentMinimum;
                }
                else if (PositionSizePercentMaximum < value)
                {
                    _height = PositionSizePercentMaximum;
                }
                else
                {
                    _height = value;
                }
            }
            else
            {
                if (double.IsNaN(value))
                {
                    _height = 0;
                }
                else if (value < SizePixelMinimum)
                {
                    _height = SizePixelMinimum;
                }
                else if (PositionSizePixelMaximum < value)
                {
                    _height = PositionSizePixelMaximum;
                }
                else
                {
                    _height = value;
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
    private PositionSizeValueType _xValueType;
    /// <summary>
    /// ウィンドウのX座標の値の種類
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public PositionSizeValueType XValueType
    {
        get
        {
            return _xValueType;
        }
        set
        {
            _xValueType = value;
            X = _x;
        }
    }
    /// <summary>
    /// ウィンドウのY座標の値の種類
    /// </summary>
    [JsonIgnore]
    private PositionSizeValueType _yValueType;
    /// <summary>
    /// ウィンドウのY座標の値の種類
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public PositionSizeValueType YValueType
    {
        get
        {
            return _yValueType;
        }
        set
        {
            _yValueType = value;
            Y = _y;
        }
    }
    /// <summary>
    /// ウィンドウの幅の値の種類
    /// </summary>
    [JsonIgnore]
    private PositionSizeValueType _widthValueType;
    /// <summary>
    /// ウィンドウの幅の値の種類
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public PositionSizeValueType WidthValueType
    {
        get
        {
            return _widthValueType;
        }
        set
        {
            _widthValueType = value;
            Width = _width;
        }
    }
    /// <summary>
    /// ウィンドウの高さの値の種類
    /// </summary>
    [JsonIgnore]
    private PositionSizeValueType _heightValueType;
    /// <summary>
    /// ウィンドウの高さの値の種類
    /// </summary>
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public PositionSizeValueType HeightValueType
    {
        get
        {
            return _heightValueType;
        }
        set
        {
            _heightValueType = value;
            Height = _height;
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
