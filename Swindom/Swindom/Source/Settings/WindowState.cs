namespace Swindom.Source.Settings
{
    /// <summary>
    /// ウィンドウの状態
    /// </summary>
    public enum WindowState : int
    {
        /// <summary>
        /// 変更しない
        /// </summary>
        [System.Xml.Serialization.XmlEnum(Name = "DoNotChange")]
        DoNotChange,
        /// <summary>
        /// 通常のウィンドウ
        /// </summary>
        [System.Xml.Serialization.XmlEnum(Name = "Normal")]
        Normal,
        /// <summary>
        /// 最大化
        /// </summary>
        [System.Xml.Serialization.XmlEnum(Name = "Maximize")]
        Maximize,
        /// <summary>
        /// 最小化
        /// </summary>
        [System.Xml.Serialization.XmlEnum(Name = "Minimize")]
        Minimize
    }
}
