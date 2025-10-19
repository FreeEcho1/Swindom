namespace Swindom;

/// <summary>
/// 「ホットキー」機能の処理の種類
/// </summary>
public enum HotkeyProcessingType : int
{
    /// <summary>
    /// 位置とサイズ指定
    /// </summary>
    PositionSize,
    /// <summary>
    /// X座標を移動
    /// </summary>
    MoveX,
    /// <summary>
    /// Y座標を移動
    /// </summary>
    MoveY,
    /// <summary>
    /// 幅を増減
    /// </summary>
    IncreaseDecreaseWidth,
    /// <summary>
    /// 高さを増減
    /// </summary>
    IncreaseDecreaseHeight,
    /// <summary>
    /// 幅と高さを増減
    /// </summary>
    IncreaseDecreaseWidthHeight,
    /// <summary>
    /// 常に最前面に表示/解除
    /// </summary>
    AlwaysForefrontOrCancel,
    /// <summary>
    /// 透明度を指定/解除
    /// </summary>
    SpecifyTransparencyOrCancel,
    /// <summary>
    /// タイトルバーと枠を表示/非表示
    /// </summary>
    TitleBarAndBorderShowAndHidden,
    /// <summary>
    /// 「指定ウィンドウ」の処理開始/停止
    /// </summary>
    StartStopSpecifyWindow,
    /// <summary>
    /// 「指定ウィンドウ」の一括処理
    /// </summary>
    BatchSpecifyWindow,
    /// <summary>
    /// 「指定ウィンドウ」のアクティブウィンドウのみ処理
    /// </summary>
    OnlyActiveWindowSpecifyWindow,
    /// <summary>
    /// 「全てのウィンドウ」の処理開始/停止
    /// </summary>
    StartStopAllWindow,
    /// <summary>
    /// 「マグネット」の処理開始/停止
    /// </summary>
    StartStopMagnet,
    /// <summary>
    /// このアプリケーションのウィンドウを表示
    /// </summary>
    ShowThisApplicationWindow,
    /// <summary>
    /// システムトレイアイコンのコンテキストメニューを表示
    /// </summary>
    ShowNotifyIconMenu,
}
