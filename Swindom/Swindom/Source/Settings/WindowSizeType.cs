namespace Swindom.Source.Settings
{
    /// <summary>
    /// ウィンドウのサイズ指定の種類
    /// </summary>
    public enum WindowSizeType : int
    {
        /// <summary>
        /// 変更しない
        /// </summary>
        [System.Xml.Serialization.XmlEnum(Name = "DoNotChange")]
        DoNotChange,
        /// <summary>
        /// 値
        /// </summary>
        [System.Xml.Serialization.XmlEnum(Name = "Value")]
        Value,
    }
}
