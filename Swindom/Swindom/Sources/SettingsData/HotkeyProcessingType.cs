﻿namespace Swindom;

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
    /// 「指定ウィンドウ」の処理開始/停止
    /// </summary>
    [XmlEnum(Name = "StartStopSpecifyWindow")]
    StartStopSpecifyWindow,
    /// <summary>
    /// 「指定ウィンドウ」の一括処理
    /// </summary>
    [XmlEnum(Name = "BatchSpecifyWindow")]
    BatchSpecifyWindow,
    /// <summary>
    /// 「指定ウィンドウ」のアクティブウィンドウのみ処理
    /// </summary>
    [XmlEnum(Name = "OnlyActiveWindowSpecifyWindow")]
    OnlyActiveWindowSpecifyWindow,
    /// <summary>
    /// 「全てのウィンドウ」の処理開始/停止
    /// </summary>
    [XmlEnum(Name = "StartStopAllWindow")]
    StartStopAllWindow,
    /// <summary>
    /// 「マグネット」の処理開始/停止
    /// </summary>
    [XmlEnum(Name = "StartStopMagnet")]
    StartStopMagnet,
    /// <summary>
    /// このアプリケーションのウィンドウを表示
    /// </summary>
    [XmlEnum(Name = "ShowThisApplicationWindow")]
    ShowThisApplicationWindow,
}
