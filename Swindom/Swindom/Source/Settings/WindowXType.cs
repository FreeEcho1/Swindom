namespace Swindom.Source.Settings
{
    /// <summary>
    /// ウィンドウのX位置指定の種類
    /// </summary>
    public enum WindowXType : int
    {
        /// <summary>
        /// 変更しない
        /// </summary>
        [System.Xml.Serialization.XmlEnum(Name = "DoNotChange")]
        DoNotChange,
        /// <summary>
        /// 左端
        /// </summary>
        [System.Xml.Serialization.XmlEnum(Name = "Left")]
        Left,
        /// <summary>
        /// 中央
        /// </summary>
        [System.Xml.Serialization.XmlEnum(Name = "Middle")]
        Middle,
        /// <summary>
        /// 右端
        /// </summary>
        [System.Xml.Serialization.XmlEnum(Name = "Right")]
        Right,
        /// <summary>
        /// 値
        /// </summary>
        [System.Xml.Serialization.XmlEnum(Name = "Value")]
        Value,
    }
}
