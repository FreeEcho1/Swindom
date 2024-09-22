using ModernWpf.Controls;

namespace Swindom;

/// <summary>
/// 「数値計算」ウィンドウ
/// </summary>
public partial class NumericCalculationWindow : Window
{
    /// <summary>
    /// 比率の連動での変更かの値 (いいえ「false」/はい「true」)
    /// </summary>
    private bool InterlockRatioValues = false;
    /// <summary>
    /// 比率の鎖の状態 (繋がっていない「false」/繋がっている「true」)
    /// </summary>
    private bool RatioChainState;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public NumericCalculationWindow()
    {
        InitializeComponent();

        ChainImage.Source = ImageProcessing.GetImageUnattachedChain();

        Title = ApplicationData.Languages.NumericCalculation + WindowControlValue.CopySeparateString + ApplicationValue.ApplicationName;
        RatioGroupBox.Header = ApplicationData.Languages.Ratio;

        Loaded += NumericCalculationWindow_Loaded;
        Ratio1NumberBox.ValueChanged += Ratio1NumberBox_ValueChanged;
        Ratio2NumberBox.ValueChanged += Ratio2NumberBox_ValueChanged;
        ChainButton.Click += ChainButton_Click;
    }

    /// <summary>
    /// 「ContentRendered」イベント
    /// </summary>
    /// <param name="e"></param>
    protected override void OnContentRendered(
        EventArgs e
        )
    {
        base.OnContentRendered(e);
        // WPFの「SizeToContent」「WidthAndHeight」のバグ対策
        InvalidateMeasure();
    }

    /// <summary>
    /// ウィンドウの「Loaded」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void NumericCalculationWindow_Loaded(
        object sender,
        RoutedEventArgs e
        )
    {
        try
        {
            Activate();
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「比率1」NumberBoxの「ValueChanged」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void Ratio1NumberBox_ValueChanged(
        NumberBox sender,
        NumberBoxValueChangedEventArgs args
        )
    {
        try
        {
            if (RatioChainState)
            {
                ControlsProcessing.InterlockProcessing(Ratio1NumberBox, Ratio2NumberBox, args, ref InterlockRatioValues);
            }
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「比率2」NumberBoxの「ValueChanged」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void Ratio2NumberBox_ValueChanged(
        NumberBox sender,
        NumberBoxValueChangedEventArgs args
        )
    {
        try
        {
            if (RatioChainState)
            {
                ControlsProcessing.InterlockProcessing(Ratio2NumberBox, Ratio1NumberBox, args, ref InterlockRatioValues);
            }
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「比率」の「鎖」Buttonの「Click」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ChainButton_Click(
        object sender,
        RoutedEventArgs e
        )
    {
        try
        {
            RatioChainState = !RatioChainState;
            ChainImage.Source = RatioChainState ? ImageProcessing.GetImageLinkedChain() : ImageProcessing.GetImageUnattachedChain();
        }
        catch
        {
        }
    }
}
