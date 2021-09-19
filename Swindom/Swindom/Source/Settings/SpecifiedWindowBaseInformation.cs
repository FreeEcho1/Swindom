namespace Swindom.Source.Settings
{
    /// <summary>
    /// 「指定ウィンドウ」機能の基礎情報
    /// </summary>
    public class SpecifiedWindowBaseInformation
    {
        /// <summary>
        /// 処理状態 (無効「false」/有効「true」)
        /// </summary>
        public bool Enabled = false;
        /// <summary>
        /// 全画面ウィンドウがあってもホットキーは停止させない (無効「false」/有効「true」)
        /// </summary>
        public bool HotkeysDoNotStopFullScreen = false;
        /// <summary>
        /// 全画面ウィンドウがある場合は処理停止 (無効「false」/有効「true」)
        /// </summary>
        public bool StopProcessingFullScreen = false;
        /// <summary>
        /// ウィンドウ処理を複数登録 (無効「false」/有効「true」)
        /// </summary>
        public bool RegisterMultiple = false;
        /// <summary>
        /// ウィンドウ判定で大文字と小文字を区別する (無効「false」/有効「true」)
        /// </summary>
        public bool CaseSensitiveWindowQueries = true;
        /// <summary>
        /// 画面外に出る場合は位置やサイズを変更しない (無効「false」/有効「true」)
        /// </summary>
        public bool DoNotChangeOutOfScreen = true;
        /// <summary>
        /// 追加/修正のウィンドウが表示されている場合は処理停止 (無効「false」/有効「true」)
        /// </summary>
        public bool StopProcessingShowAddModifyWindow = true;
        /// <summary>
        /// 追加/修正ウィンドウのサイズ
        /// </summary>
        public System.Drawing.Size AddModifyWindowSize = new();
        /// <summary>
        /// ウィンドウ情報の取得項目
        /// </summary>
        public WindowInformationToBeRetrieved AcquiredItems = new();
    }
}
