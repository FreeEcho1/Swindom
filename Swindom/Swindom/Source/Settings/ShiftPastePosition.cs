namespace Swindom.Source.Settings
{
    /// <summary>
    /// 「貼り付ける位置をずらす」情報
    /// </summary>
    public class ShiftPastePosition
    {
        /// <summary>
        /// 有効状態 (無効「false」/有効「true」)
        /// </summary>
        public bool Enabled = false;
        /// <summary>
        /// 左側の距離
        /// </summary>
        public int Left = 0;
        /// <summary>
        /// 上側の距離
        /// </summary>
        public int Top = 0;
        /// <summary>
        /// 右側の距離
        /// </summary>
        public int Right = 0;
        /// <summary>
        /// 下側の距離
        /// </summary>
        public int Bottom = 0;
    }
}
