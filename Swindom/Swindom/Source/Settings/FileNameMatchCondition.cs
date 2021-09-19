namespace Swindom.Source.Settings
{
    /// <summary>
    /// ファイル名の一致条件
    /// </summary>
    public enum FileNameMatchCondition : int
    {
        /// <summary>
        /// パスを含む
        /// </summary>
        [System.Xml.Serialization.XmlEnum(Name = "Include")]
        Include,
        /// <summary>
        /// パスを含まない
        /// </summary>
        [System.Xml.Serialization.XmlEnum(Name = "NotInclude")]
        NotInclude
    }
}
