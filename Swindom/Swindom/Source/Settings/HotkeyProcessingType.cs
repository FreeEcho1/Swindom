namespace Swindom.Source.Settings
{
    /// <summary>
    /// 「ホットキー」機能の処理の種類
    /// </summary>
    public enum HotkeyProcessingType : int
    {
        /// <summary>
        /// 位置とサイズ指定
        /// </summary>
        [System.Xml.Serialization.XmlEnum(Name = "PositionSize")]
        PositionSize,
        /// <summary>
        /// X座標を移動
        /// </summary>
        [System.Xml.Serialization.XmlEnum(Name = "MoveX")]
        MoveX,
        /// <summary>
        /// Y座標を移動
        /// </summary>
        [System.Xml.Serialization.XmlEnum(Name = "MoveY")]
        MoveY,
        /// <summary>
        /// XとY座標を移動
        /// </summary>
        [System.Xml.Serialization.XmlEnum(Name = "MoveXY")]
        MoveXY,
        /// <summary>
        /// 幅を増減
        /// </summary>
        [System.Xml.Serialization.XmlEnum(Name = "IncreaseDecreaseWidth")]
        IncreaseDecreaseWidth,
        /// <summary>
        /// 高さを増減
        /// </summary>
        [System.Xml.Serialization.XmlEnum(Name = "IncreaseDecreaseHeight")]
        IncreaseDecreaseHeight,
        /// <summary>
        /// 幅と高さを増減
        /// </summary>
        [System.Xml.Serialization.XmlEnum(Name = "IncreaseDecreaseWidthHeight")]
        IncreaseDecreaseWidthHeight,
        /// <summary>
        /// 常に最前面に表示/解除
        /// </summary>
        [System.Xml.Serialization.XmlEnum(Name = "AlwaysForefrontOrCancel")]
        AlwaysForefrontOrCancel,
        /// <summary>
        /// 透明度を指定/解除
        /// </summary>
        [System.Xml.Serialization.XmlEnum(Name = "SpecifyTransparencyOrCancel")]
        SpecifyTransparencyOrCancel,
        /// <summary>
        /// 「イベント」の処理開始/停止
        /// </summary>
        [System.Xml.Serialization.XmlEnum(Name = "StartStopEvent")]
        StartStopEvent,
        /// <summary>
        /// 「イベント」の一括処理
        /// </summary>
        [System.Xml.Serialization.XmlEnum(Name = "BatchEvent")]
        BatchEvent,
        /// <summary>
        /// 「イベント」のアクティブウィンドウのみ処理
        /// </summary>
        [System.Xml.Serialization.XmlEnum(Name = "OnlyActiveWindowEvent")]
        OnlyActiveWindowEvent,
        /// <summary>
        /// 「タイマー」の処理開始/停止
        /// </summary>
        [System.Xml.Serialization.XmlEnum(Name = "StartStopTimer")]
        StartStopTimer,
        /// <summary>
        /// 「タイマー」の一括処理
        /// </summary>
        [System.Xml.Serialization.XmlEnum(Name = "BatchTimer")]
        BatchTimer,
        /// <summary>
        /// 「タイマー」のアクティブウィンドウのみ処理
        /// </summary>
        [System.Xml.Serialization.XmlEnum(Name = "OnlyActiveWindowTimer")]
        OnlyActiveWindowTimer,
        /// <summary>
        /// 「マグネット」の処理開始/停止
        /// </summary>
        [System.Xml.Serialization.XmlEnum(Name = "StartStopMagnet")]
        StartStopMagnet,
    }
}
