namespace Swindom.Source.Settings
{
    /// <summary>
    /// 最前面の種類
    /// </summary>
    public enum Forefront : int
    {
        /// <summary>
        /// 変更しない
        /// </summary>
        [System.Xml.Serialization.XmlEnum(Name = "DoNotChange")]
        DoNotChange,
        /// <summary>
        /// 常に最前面解除
        /// </summary>
        [System.Xml.Serialization.XmlEnum(Name = "Cancel")]
        Cancel,
        /// <summary>
        /// 常に最前面
        /// </summary>
        [System.Xml.Serialization.XmlEnum(Name = "Always")]
        Always,
        /// <summary>
        /// 最前面
        /// </summary>
        [System.Xml.Serialization.XmlEnum(Name = "Forefront")]
        Forefront
    }
}
