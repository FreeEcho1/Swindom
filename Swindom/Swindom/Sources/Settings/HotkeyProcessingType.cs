namespace Swindom;

/// <summary>
/// 「ホットキー」機能の処理の種類
/// </summary>
public enum HotkeyProcessingType : int
{
    /// <summary>
    /// 位置とサイズ指定
    /// </summary>
    [XmlEnum(Name = "PositionSize")]
    PositionSize,
    /// <summary>
    /// X座標を移動
    /// </summary>
    [XmlEnum(Name = "MoveX")]
    MoveX,
    /// <summary>
    /// Y座標を移動
    /// </summary>
    [XmlEnum(Name = "MoveY")]
    MoveY,
    /// <summary>
    /// 幅を増減
    /// </summary>
    [XmlEnum(Name = "IncreaseDecreaseWidth")]
    IncreaseDecreaseWidth,
    /// <summary>
    /// 高さを増減
    /// </summary>
    [XmlEnum(Name = "IncreaseDecreaseHeight")]
    IncreaseDecreaseHeight,
    /// <summary>
    /// 幅と高さを増減
    /// </summary>
    [XmlEnum(Name = "IncreaseDecreaseWidthHeight")]
    IncreaseDecreaseWidthHeight,
    /// <summary>
    /// 常に最前面に表示/解除
    /// </summary>
    [XmlEnum(Name = "AlwaysForefrontOrCancel")]
    AlwaysForefrontOrCancel,
    /// <summary>
    /// 透明度を指定/解除
    /// </summary>
    [XmlEnum(Name = "SpecifyTransparencyOrCancel")]
    SpecifyTransparencyOrCancel,
    /// <summary>
    /// 「イベント」の処理開始/停止
    /// </summary>
    [XmlEnum(Name = "StartStopEvent")]
    StartStopEvent,
    /// <summary>
    /// 「イベント」の一括処理
    /// </summary>
    [XmlEnum(Name = "BatchEvent")]
    BatchEvent,
    /// <summary>
    /// 「イベント」のアクティブウィンドウのみ処理
    /// </summary>
    [XmlEnum(Name = "OnlyActiveWindowEvent")]
    OnlyActiveWindowEvent,
    /// <summary>
    /// 「タイマー」の処理開始/停止
    /// </summary>
    [XmlEnum(Name = "StartStopTimer")]
    StartStopTimer,
    /// <summary>
    /// 「タイマー」の一括処理
    /// </summary>
    [XmlEnum(Name = "BatchTimer")]
    BatchTimer,
    /// <summary>
    /// 「タイマー」のアクティブウィンドウのみ処理
    /// </summary>
    [XmlEnum(Name = "OnlyActiveWindowTimer")]
    OnlyActiveWindowTimer,
    /// <summary>
    /// 「マグネット」の処理開始/停止
    /// </summary>
    [XmlEnum(Name = "StartStopMagnet")]
    StartStopMagnet,
}
