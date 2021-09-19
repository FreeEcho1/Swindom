namespace Swindom.Source.Settings
{
    /// <summary>
    /// 基準にするディスプレイ
    /// </summary>
    public enum StandardDisplay : int
    {
        /// <summary>
        /// ウィンドウがあるディスプレイ (複数アクティブ不可)
        /// </summary>
        [System.Xml.Serialization.XmlEnum(Name = "DisplayWithWindow")]
        DisplayWithWindow,
        /// <summary>
        /// 指定したディスプレイ (複数アクティブ不可)
        /// </summary>
        [System.Xml.Serialization.XmlEnum(Name = "SpecifiedDisplay")]
        SpecifiedDisplay,
        /// <summary>
        /// 指定したディスプレイにある場合のみ (複数アクティブ可)
        /// </summary>
        [System.Xml.Serialization.XmlEnum(Name = "OnlySpecifiedDisplay")]
        OnlySpecifiedDisplay
    }
}
