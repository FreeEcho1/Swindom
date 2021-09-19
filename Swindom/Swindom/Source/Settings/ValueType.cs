namespace Swindom.Source.Settings
{
    /// <summary>
    /// 値の種類
    /// </summary>
    public enum ValueType : int
    {
        /// <summary>
        /// Pixel
        /// </summary>
        [System.Xml.Serialization.XmlEnum(Name = "Pixel")]
        Pixel,
        /// <summary>
        /// %
        /// </summary>
        [System.Xml.Serialization.XmlEnum(Name = "Percent")]
        Percent
    }
}
