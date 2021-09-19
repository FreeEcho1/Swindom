namespace Swindom.Source.Settings
{
    /// <summary>
    /// 「タイマー」機能の項目情報
    /// </summary>
    public class TimerItemInformation : SpecifiedWindowBaseItemInformation
    {
        /// <summary>
        /// 最初に処理しない回数 (指定しない「0」)
        /// </summary>
        public int NumberOfTimesNotToProcessingFirst = 0;

        // ------------------ 設定ファイルに保存しないデータ ------------------ //
        /// <summary>
        /// カウントした最初に処理しない回数
        /// </summary>
        public int CountNumberOfTimesNotToProcessingFirst = 0;
        // ------------------ 設定ファイルに保存しないデータ ------------------ //

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public TimerItemInformation()
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="item">「タイマー」機能の項目情報</param>
        /// <param name="copyHotkey">ホットキーをコピーするかの値 (コピーしない「false」/コピーする「true」)</param>
        public TimerItemInformation(
            TimerItemInformation item,
            bool copyHotkey = true
            )
            : base(item, copyHotkey)
        {
            NumberOfTimesNotToProcessingFirst = item.NumberOfTimesNotToProcessingFirst;
        }
    }
}
