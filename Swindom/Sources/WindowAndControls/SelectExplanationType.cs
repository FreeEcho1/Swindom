namespace Swindom;

/// <summary>
/// 「説明」ウィンドウの選択する説明の種類
/// </summary>
public enum SelectExplanationType
{
    /// <summary>
    /// 座標
    /// </summary>
    Coordinate,
    /// <summary>
    /// ウィンドウ判定
    /// </summary>
    WindowDecision,
    /// <summary>
    /// イベント、タイマー
    /// </summary>
    EventTimer,
    /// <summary>
    /// クライアントエリア
    /// </summary>
    ClientArea,
    /// <summary>
    /// アクティブウィンドウ
    /// </summary>
    ActiveWindow,
    /// <summary>
    /// プラグイン
    /// </summary>
    Plugin,
    /// <summary>
    /// 子ウィンドウ
    /// </summary>
    ChildWindow,
}
