namespace Swindom.Source.Settings
{
    /// <summary>
    /// 座標指定の種類
    /// </summary>
    public enum CoordinateType : int
    {
        /// <summary>
        /// ディスプレイ座標
        /// </summary>
        [System.Xml.Serialization.XmlEnum(Name = "Display")]
        Display,
        /// <summary>
        /// グローバル座標
        /// </summary>
        [System.Xml.Serialization.XmlEnum(Name = "Global")]
        Global
    }
}
