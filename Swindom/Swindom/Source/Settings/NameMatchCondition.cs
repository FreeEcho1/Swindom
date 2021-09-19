namespace Swindom.Source.Settings
{
    /// <summary>
    /// 名前の一致条件
    /// </summary>
    public enum NameMatchCondition : int
    {
        /// <summary>
        /// 完全一致
        /// </summary>
        [System.Xml.Serialization.XmlEnum(Name = "ExactMatch")]
        ExactMatch,
        /// <summary>
        /// 部分一致
        /// </summary>
        [System.Xml.Serialization.XmlEnum(Name = "PartialMatch")]
        PartialMatch,
        /// <summary>
        /// 前方一致
        /// </summary>
        [System.Xml.Serialization.XmlEnum(Name = "ForwardMatch")]
        ForwardMatch,
        /// <summary>
        /// 後方一致
        /// </summary>
        [System.Xml.Serialization.XmlEnum(Name = "BackwardMatch")]
        BackwardMatch,
    }
}
