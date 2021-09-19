namespace Swindom.Source
{
    /// <summary>
    /// Rectangle (decimal)
    /// </summary>
    public class RectangleDecimal
    {
        /// <summary>
        /// X
        /// </summary>
        public decimal X { get; set; }
        /// <summary>
        /// Y
        /// </summary>
        public decimal Y { get; set; }
        /// <summary>
        /// Width
        /// </summary>
        public decimal Width { get; set; }
        /// <summary>
        /// Height
        /// </summary>
        public decimal Height { get; set; }
        /// <summary>
        /// Left
        /// </summary>
        public decimal Left => X;
        /// <summary>
        /// Top
        /// </summary>
        public decimal Top => Y;
        /// <summary>
        /// Right
        /// </summary>
        public decimal Right => X + Width;
        /// <summary>
        /// Bottom
        /// </summary>
        public decimal Bottom => Y + Height;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="x">X</param>
        /// <param name="y">Y</param>
        /// <param name="width">Width</param>
        /// <param name="height">Height</param>
        public RectangleDecimal(
            decimal x = 0,
            decimal y = 0,
            decimal width = 0,
            decimal height = 0
            )
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        /// <summary>
        /// 全ての数値プロパティの値が「0」か確認
        /// <para>全ての値が「0」かの値 (いいえ「false」/はい「true」)</para>
        /// </summary>
        public bool IsEmpty => X == 0 && Y == 0 && Width == 0 && Height == 0;

        /// <summary>
        /// 交差確認
        /// </summary>
        /// <param name="r1">値1</param>
        /// <param name="r2">値2</param>
        /// <returns>重なっている領域</returns>
        public static RectangleDecimal Intersect(
            RectangleDecimal r1,
            RectangleDecimal r2
            )
        {
            decimal x1 = Max(r1.Left, r2.Left);
            decimal y1 = Max(r2.Top, r2.Top);
            decimal x2 = Min(r1.Right, r2.Right);
            decimal y2 = Min(r1.Bottom, r2.Bottom);

            decimal w = x2 - x1;
            decimal h = y2 - y1;

            if (w > 0 && h > 0)
            {
                return (new RectangleDecimal(x1, y1, w, h));
            }
            return (new RectangleDecimal(0, 0, 0, 0));
        }

        /// <summary>
        /// 小さい方の値を取得
        /// </summary>
        /// <param name="d1">値1</param>
        /// <param name="d2">値2</param>
        /// <returns>小さい方の値</returns>
        private static decimal Min(
            decimal d1,
            decimal d2
            )
        {
            return (d1 < d2 ? d1 : d2);
        }

        /// <summary>
        /// 大きい方の値を取得
        /// </summary>
        /// <param name="d1">値1</param>
        /// <param name="d2">値2</param>
        /// <returns>大きい方の値</returns>
        private static decimal Max(
            decimal d1,
            decimal d2
            )
        {
            return (d1 < d2 ? d2 : d1);
        }
    }
}
