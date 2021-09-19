namespace Swindom.Source.Settings
{
    /// <summary>
    /// 「ホットキー」機能の情報
    /// </summary>
    public class HotkeyInformation
    {
        /// <summary>
        /// 処理状態 (無効「false」/有効「true」)
        /// </summary>
        public bool Enabled = false;
        /// <summary>
        /// 画面外に出る場合は位置やサイズを変更しない (無効「false」/有効「true」)
        /// </summary>
        public bool DoNotChangeOutOfScreen = true;
        /// <summary>
        /// 全画面ウィンドウがある場合は処理停止 (無効「false」/有効「true」)
        /// </summary>
        public bool StopProcessingFullScreen = false;
        /// <summary>
        /// ホットキーの追加/修正ウィンドウのサイズ
        /// </summary>
        public System.Drawing.Size AddModifyWindowSize = new();
        /// <summary>
        /// 「ホットキー」機能の項目情報
        /// </summary>
        public System.Collections.Generic.List<HotkeyItemInformation> Items = new();
    }
}
