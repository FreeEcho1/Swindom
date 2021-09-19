namespace Swindom.Source.Settings
{
    /// <summary>
    /// ウィンドウイベントのデータ
    /// </summary>
    public class WindowEventData
    {
        /// <summary>
        /// フォアグラウンドが変更された
        /// </summary>
        public bool Foreground;
        /// <summary>
        /// 移動及びサイズの変更が終了された
        /// </summary>
        public bool MoveSizeEnd;
        /// <summary>
        /// 最小化が開始された
        /// </summary>
        public bool MinimizeStart;
        /// <summary>
        /// 最小化が終了された
        /// </summary>
        public bool MinimizeEnd;
        /// <summary>
        /// 作成された
        /// </summary>
        public bool Create;
        /// <summary>
        /// 表示された
        /// </summary>
        public bool Show;
        /// <summary>
        /// 名前が変更された
        /// </summary>
        public bool NameChange;
    }
}
