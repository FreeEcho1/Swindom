namespace Swindom.Source.Settings
{
    /// <summary>
    /// 「タイマー」機能の情報
    /// </summary>
    public class TimerInformation : SpecifiedWindowBaseInformation
    {
        /// <summary>
        /// 処理するウィンドウの範囲
        /// </summary>
        public ProcessingWindowRange ProcessingWindowRange = ProcessingWindowRange.ActiveOnly;
        /// <summary>
        /// 処理間隔 (ミリ秒)
        /// </summary>
        public int ProcessingInterval = 600;
        /// <summary>
        /// 次のウィンドウを処理する待ち時間 (ミリ秒) (待たない「0」)
        /// </summary>
        public int WaitTimeToProcessingNextWindow = 0;
        /// <summary>
        /// 「タイマー」機能の項目情報
        /// </summary>
        public System.Collections.Generic.List<TimerItemInformation> Items = new();
    }
}
