namespace Swindom.Source.Settings
{
    /// <summary>
    /// 「マグネット」機能の情報
    /// </summary>
    public class MagnetInformation
    {
        /// <summary>
        /// 処理状態 (無効「false」/有効「true」)
        /// </summary>
        public bool Enabled = false;
        /// <summary>
        /// 画面端に貼り付ける (無効「false」/有効「true」)
        /// </summary>
        public bool PasteToScreenEdge = false;
        /// <summary>
        /// 別のウィンドウに貼り付ける (無効「false」/有効「true」)
        /// </summary>
        public bool PasteToAnotherWindow = false;
        /// <summary>
        /// キーを押した状態で貼り付ける (無効「false」/有効「true」)
        /// </summary>
        public bool PressTheKeyToPaste = false;
        /// <summary>
        /// 貼り付けた時の停止時間 (ミリ秒)
        /// </summary>
        public int StopTimeWhenPasted = 400;
        /// <summary>
        /// 貼り付ける判定距離
        /// </summary>
        public int DecisionDistanceToPaste = 10;
        /// <summary>
        /// 全画面ウィンドウがある場合は処理停止 (無効「false」/有効「true」)
        /// </summary>
        public bool StopProcessingFullScreen = false;
    }
}
