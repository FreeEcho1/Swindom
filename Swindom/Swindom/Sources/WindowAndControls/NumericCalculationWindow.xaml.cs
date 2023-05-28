namespace Swindom;

/// <summary>
/// 「数値計算」ウィンドウ
/// </summary>
public partial class NumericCalculationWindow : Window
{
    /// <summary>
    /// 変更前の比率1の値
    /// </summary>
    private double PreviousRatio1Value;
    /// <summary>
    /// 変更前の比率2の値
    /// </summary>
    private double PreviousRatio2Value;
    /// <summary>
    /// 比率の値を連動させる (連動させない「false」/連動させる「true」)
    /// </summary>
    private bool LinkingRatioValues = true;
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

        if (ApplicationData.Languages.LanguagesWindow == null)
        {
            throw new Exception("Languages value is null. - NumericCalculationWindow()");
        }

        if (ApplicationData.Settings.DarkMode)
        {
            ChainImage.Source = new BitmapImage(new("/Resources/UnattachedChainWhite.png", UriKind.Relative));
        }

        Title = ApplicationData.Languages.LanguagesWindow.NumericCalculation;
        RatioGroupBox.Header = ApplicationData.Languages.LanguagesWindow.Ratio;

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
        ModernWpf.Controls.NumberBox sender,
        ModernWpf.Controls.NumberBoxValueChangedEventArgs args
        )
    {
        try
        {
            if (Ratio1NumberBox.Value < 0)
            {
                Ratio1NumberBox.Value = 0;
            }
            else if (LinkingRatioValues == false)
            {
                LinkingRatioValues = true;
            }
            else if (RatioChainState)
            {
                if (0 < PreviousRatio1Value)
                {
                    LinkingRatioValues = false;
                    double setValue = Ratio1NumberBox.Value / PreviousRatio1Value * Ratio2NumberBox.Value;
                    if (setValue < 0)
                    {
                        setValue = 0;
                    }
                    Ratio2NumberBox.Value = setValue;
                }
            }
            PreviousRatio1Value = Ratio1NumberBox.Value;
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
        ModernWpf.Controls.NumberBox sender,
        ModernWpf.Controls.NumberBoxValueChangedEventArgs args
        )
    {
        try
        {
            if (Ratio2NumberBox.Value < 0)
            {
                Ratio2NumberBox.Value = 0;
            }
            else if (LinkingRatioValues == false)
            {
                LinkingRatioValues = true;
            }
            else if (RatioChainState)
            {
                if (0 < PreviousRatio2Value)
                {
                    LinkingRatioValues = false;
                    double setValue = Ratio2NumberBox.Value / PreviousRatio2Value * Ratio1NumberBox.Value;
                    if (setValue < 0)
                    {
                        setValue = 0;
                    }
                    Ratio1NumberBox.Value = setValue;
                }
            }
            PreviousRatio2Value = Ratio2NumberBox.Value;
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
            ChainImage.Source = ApplicationData.Settings.DarkMode
                ? new BitmapImage(new(RatioChainState ? "/Resources/LinkedChainWhite.png" : "/Resources/UnattachedChainWhite.png", UriKind.Relative))
                : ChainImage.Source = new BitmapImage(new(RatioChainState ? "/Resources/LinkedChainDark.png" : "/Resources/UnattachedChainDark.png", UriKind.Relative));
        }
        catch
        {
        }
    }
}
