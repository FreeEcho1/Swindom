namespace Swindom.Source.Settings
{
    /// <summary>
    /// 1度だけ処理
    /// </summary>
    public enum ProcessingOnlyOnce : int
    {
        /// <summary>
        /// 指定しない
        /// </summary>
        [System.Xml.Serialization.XmlEnum(Name = "NotSpecified")]
        NotSpecified,
        /// <summary>
        /// ウィンドウが開かれた時
        /// </summary>
        [System.Xml.Serialization.XmlEnum(Name = "WindowOpen")]
        WindowOpen,
        /// <summary>
        /// このソフトウェアが実行されている間
        /// </summary>
        [System.Xml.Serialization.XmlEnum(Name = "Running")]
        Running,
    }
}
