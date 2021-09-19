namespace Swindom.Source.Window
{
    /// <summary>
    /// 「数値計算」ウィンドウ
    /// </summary>
    public partial class NumericCalculationWindow : System.Windows.Window
    {
        /// <summary>
        /// 変更前の比率1の値
        /// </summary>
        private decimal PreviousRatio1Value = 0;
        /// <summary>
        /// 変更前の比率2の値
        /// </summary>
        private decimal PreviousRatio2Value = 0;
        /// <summary>
        /// 比率の値を連動させる (連動させない「false」/連動させる「true」)
        /// </summary>
        private bool LinkingRatioValues = true;
        /// <summary>
        /// 比率の鎖の状態 (繋がっていない「false」/繋がっている「true」)
        /// </summary>
        private bool RatioChainStatus = false;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public NumericCalculationWindow()
        {
            InitializeComponent();

            Title = Common.ApplicationData.Languages.LanguagesWindow.NumericCalculation;
            RatioGroupBox.Header = Common.ApplicationData.Languages.LanguagesWindow.Ratio;

            Ratio1NumericUpDown.ChangeValue += Ratio1NumericUpDown_ChangeValue;
            Ratio2NumericUpDown.ChangeValue += Ratio2NumericUpDown_ChangeValue;
            ChainImageButton.Click += ChainImageButton_Click;
        }

        /// <summary>
        /// 「比率1」NumericUpDownの「ChangeValue」イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Ratio1NumericUpDown_ChangeValue(
            object sender,
            FreeEcho.FEControls.NumericUpDownChangeValueArgs e
            )
        {
            try
            {
                if (LinkingRatioValues)
                {
                    if (RatioChainStatus)
                    {
                        LinkingRatioValues = false;
                        Ratio2NumericUpDown.Value = Ratio1NumericUpDown.Value / PreviousRatio1Value * Ratio2NumericUpDown.Value;
                    }
                }
                else
                {
                    LinkingRatioValues = true;
                }
                PreviousRatio1Value = Ratio1NumericUpDown.Value;
            }
            catch
            {
            }
        }

        /// <summary>
        /// 「比率2」NumericUpDownの「ChangeValue」イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Ratio2NumericUpDown_ChangeValue(
            object sender,
            FreeEcho.FEControls.NumericUpDownChangeValueArgs e
            )
        {
            try
            {
                if (LinkingRatioValues)
                {
                    if (RatioChainStatus)
                    {
                        LinkingRatioValues = false;
                        Ratio1NumericUpDown.Value = Ratio2NumericUpDown.Value / PreviousRatio2Value * Ratio1NumericUpDown.Value;
                    }
                }
                else
                {
                    LinkingRatioValues = true;
                }
                PreviousRatio2Value = Ratio2NumericUpDown.Value;
            }
            catch
            {
            }
        }

        /// <summary>
        /// 比率の「鎖」Buttonの「Click」イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChainImageButton_Click(
            object sender,
            System.Windows.RoutedEventArgs e
            )
        {
            try
            {
                RatioChainStatus = !RatioChainStatus;
                ChainImageButton.ImageSource = new System.Windows.Media.Imaging.BitmapImage(new(RatioChainStatus ? "/SwindomImage;component/Resources/LinkedChain.png" : "/SwindomImage;component/Resources/UnattachedChain.png", System.UriKind.Relative));
            }
            catch
            {
            }
        }
    }
}
