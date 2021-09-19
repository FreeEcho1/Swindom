namespace Swindom.Source.Settings
{
    /// <summary>
    /// ウィンドウのY位置指定の種類
    /// </summary>
    public enum WindowYType : int
    {
        /// <summary>
        /// 変更しない
        /// </summary>
        [System.Xml.Serialization.XmlEnum(Name = "DoNotChange")]
        DoNotChange,
        /// <summary>
        /// 上端
        /// </summary>
        [System.Xml.Serialization.XmlEnum(Name = "Top")]
        Top,
        /// <summary>
        /// 中央
        /// </summary>
        [System.Xml.Serialization.XmlEnum(Name = "Middle")]
        Middle,
        /// <summary>
        /// 下端
        /// </summary>
        [System.Xml.Serialization.XmlEnum(Name = "Bottom")]
        Bottom,
        /// <summary>
        /// 値
        /// </summary>
        [System.Xml.Serialization.XmlEnum(Name = "Value")]
        Value,
    }
}
