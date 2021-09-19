namespace Swindom.Source.Settings
{
    /// <summary>
    /// タイトルの処理条件
    /// </summary>
    public enum TitleProcessingConditions : int
    {
        /// <summary>
        /// 指定しない
        /// </summary>
        [System.Xml.Serialization.XmlEnum(Name = "NotSpecified")]
        NotSpecified,
        /// <summary>
        /// タイトルがないウィンドウは処理しない
        /// </summary>
        [System.Xml.Serialization.XmlEnum(Name = "DoNotProcessingUntitledWindow")]
        DoNotProcessingUntitledWindow,
        /// <summary>
        /// タイトルがあるウィンドウは処理しない
        /// </summary>
        [System.Xml.Serialization.XmlEnum(Name = "DoNotProcessingWindowWithTitle")]
        DoNotProcessingWindowWithTitle
    }
}
