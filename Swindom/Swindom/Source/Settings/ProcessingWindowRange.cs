namespace Swindom.Source.Settings
{
    /// <summary>
    /// 処理するウィンドウの範囲
    /// </summary>
    public enum ProcessingWindowRange
    {
        /// <summary>
        /// アクティブウィンドウのみ
        /// </summary>
        [System.Xml.Serialization.XmlEnum(Name = "ActiveOnly")]
        ActiveOnly,
        /// <summary>
        /// 全てのウィンドウ
        /// </summary>
        [System.Xml.Serialization.XmlEnum(Name = "All")]
        All
    }
}
