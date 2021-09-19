namespace Swindom.Source
{
    /// <summary>
    /// Point (decimal)
    /// </summary>
    public struct PointDecimal
    {
        /// <summary>
        /// X
        /// </summary>
        public decimal X;
        /// <summary>
        /// Y
        /// </summary>
        public decimal Y;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="x">X</param>
        /// <param name="y">Y</param>
        public PointDecimal(
            decimal x,
            decimal y
            )
        {
            X = x;
            Y = y;
        }
    }
}
