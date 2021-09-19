namespace Swindom.Source.Settings
{
    /// <summary>
    /// 位置とサイズ
    /// </summary>
    public class PositionSize
    {
        /// <summary>
        /// ウィンドウの位置
        /// </summary>
        public PointDecimal Position = new();
        /// <summary>
        ///  ウィンドウのサイズ
        /// </summary>
        public SizeDecimal Size = new();
        /// <summary>
        /// ウィンドウのX位置指定の種類
        /// </summary>
        public WindowXType XType = WindowXType.DoNotChange;
        /// <summary>
        /// ウィンドウのY位置指定の種類
        /// </summary>
        public WindowYType YType = WindowYType.DoNotChange;
        /// <summary>
        /// ウィンドウの幅指定の種類
        /// </summary>
        public WindowSizeType WidthType = WindowSizeType.DoNotChange;
        /// <summary>
        /// ウィンドウの高さ指定の種類
        /// </summary>
        public WindowSizeType HeightType = WindowSizeType.DoNotChange;
        /// <summary>
        /// ウィンドウのX座標の値の種類
        /// </summary>
        public ValueType XValueType = ValueType.Pixel;
        /// <summary>
        /// ウィンドウのY座標の値の種類
        /// </summary>
        public ValueType YValueType = ValueType.Pixel;
        /// <summary>
        /// ウィンドウの幅の値の種類
        /// </summary>
        public ValueType WidthValueType = ValueType.Pixel;
        /// <summary>
        /// ウィンドウの高さの値の種類
        /// </summary>
        public ValueType HeightValueType = ValueType.Pixel;
        /// <summary>
        /// ディスプレイ
        /// </summary>
        public string Display = "";
        /// <summary>
        /// クライアントエリアを対象とするかの値 (いいえ「false」/はい「true」)
        /// </summary>
        public bool ClientArea = false;
        /// <summary>
        /// 位置とサイズを2回処理
        /// </summary>
        public bool ProcessingPositionAndSizeTwice = false;
    }
}
