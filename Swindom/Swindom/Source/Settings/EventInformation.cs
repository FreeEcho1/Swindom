namespace Swindom.Source.Settings
{
    /// <summary>
    /// 「イベント」機能の情報
    /// </summary>
    public class EventInformation : SpecifiedWindowBaseInformation
    {
        /// <summary>
        /// 「イベント」機能の種類の情報
        /// </summary>
        public EventTypeInformation EventTypeInformation = new();
        /// <summary>
        /// 「イベント」機能の項目情報
        /// </summary>
        public System.Collections.Generic.List<EventItemInformation> Items = new();
    }
}
