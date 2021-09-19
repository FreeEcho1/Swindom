namespace Swindom.Source.Settings
{
    /// <summary>
    /// 「ウィンドウを画面外から画面内に移動」機能の情報
    /// </summary>
    public class OutsideToInsideInformation
    {
        /// <summary>
        /// 処理状態
        /// </summary>
        public bool Enabled = false;
        /// <summary>
        /// 処理間隔 (ミリ秒)
        /// </summary>
        public int ProcessingInterval = 10000;
        /// <summary>
        /// ディスプレイ
        /// </summary>
        public string Display = "";
    }
}
