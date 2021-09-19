namespace Swindom.Source.Settings
{
    /// <summary>
    /// 「イベント」機能の項目情報
    /// </summary>
    public class EventItemInformation : SpecifiedWindowBaseItemInformation
    {
        /// <summary>
        /// ウィンドウイベントのデータ
        /// </summary>
        public WindowEventData WindowEventData = new();

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public EventItemInformation()
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="item">「イベント」機能の項目情報</param>
        /// <param name="copyHotkey">ホットキーをコピーするかの値 (コピーしない「false」/コピーする「true」)</param>
        public EventItemInformation(
            EventItemInformation item,
            bool copyHotkey = true
            )
            : base(item, copyHotkey)
        {
            WindowEventData.Foreground = item.WindowEventData.Foreground;
            WindowEventData.MoveSizeEnd = item.WindowEventData.MoveSizeEnd;
            WindowEventData.MinimizeStart = item.WindowEventData.MinimizeStart;
            WindowEventData.MinimizeEnd = item.WindowEventData.MinimizeEnd;
            WindowEventData.Create = item.WindowEventData.Create;
            WindowEventData.Show = item.WindowEventData.Show;
            WindowEventData.NameChange = item.WindowEventData.NameChange;
        }
    }
}
