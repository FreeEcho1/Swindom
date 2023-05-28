namespace Swindom;

/// <summary>
/// 処理イベントの種類
/// </summary>
public enum ProcessingEventType : int
{
    /// <summary>
    /// 言語が変更された
    /// </summary>
    LanguageChanged,
    /// <summary>
    /// テーマが変更された
    /// </summary>
    ThemeChanged,
    /// <summary>
    /// 座標が変更された
    /// </summary>
    CoordinateChanged,
    /// <summary>
    /// 「全画面ウィンドウがある場合は処理停止」の有効状態が変更された
    /// </summary>
    StopWhenWindowIsFullScreenChanged,
    /// <summary>
    /// 「ホットキーは停止させない」の有効状態が変更された
    /// </summary>
    HotkeysDoNotStopFullScreenChanged,
    /// <summary>
    /// 全画面ウィンドウが表示された/閉じられた
    /// </summary>
    FullScreenWindowShowClose,
    /// <summary>
    /// ホットキー処理を一時停止
    /// </summary>
    PauseHotkeyProcessing,
    /// <summary>
    /// ホットキー処理の一時停止を解除
    /// </summary>
    UnpauseHotkeyProcessing,

    /// <summary>
    /// 「指定ウィンドウ」の処理状態が変更された
    /// </summary>
    SpecifyWindowProcessingStateChanged,
    /// <summary>
    /// 「指定ウィンドウ」の一括実行
    /// </summary>
    SpecifyWindowBatchProcessing,
    /// <summary>
    /// 「指定ウィンドウ」のアクティブウィンドウのみ処理
    /// </summary>
    SpecifyWindowOnlyActiveWindow,
    /// <summary>
    /// 「指定ウィンドウ」の追加/修正ウィンドウ表示時の処理停止
    /// </summary>
    SpecifyWindowShowWindowPause,
    /// <summary>
    /// 「指定ウィンドウ」の追加/修正ウィンドウ表示時の処理停止を解除
    /// </summary>
    SpecifyWindowShowWindowUnpause,
    /// <summary>
    /// 「指定ウィンドウ」の「タイマー」の「処理間隔」が変更された
    /// </summary>
    SpecifyWindowChangeTimerProcessingInterval,
    /// <summary>
    /// 「指定ウィンドウ」の「追加/修正」ウィンドウが閉じられた
    /// </summary>
    SpecifyWindowCloseItemWindow,
    /// <summary>
    /// 「指定ウィンドウ」のリストボックスを更新
    /// </summary>
    SpecifyWindowUpdateListBox,

    /// <summary>
    /// 「全てのウィンドウ」の処理状態が変更された
    /// </summary>
    AllWindowProcessingStateChanged,

    /// <summary>
    /// 「マグネット」の処理状態が変更された
    /// </summary>
    MagnetProcessingStateChanged,

    /// <summary>
    /// 「プラグイン」の処理状態が変更された
    /// </summary>
    PluginProcessingStateChanged,

    /// <summary>
    /// 「ホットキー」の処理状態が変更された
    /// </summary>
    HotkeyProcessingStateChanged,

    /// <summary>
    /// このアプリケーションのウィンドウを表示
    /// </summary>
    ShowThisApplicationWindow,
    /// <summary>
    /// 再起動処理
    /// </summary>
    RestartProcessing,
    /// <summary>
    /// アプリケーションを閉じる
    /// </summary>
    CloseApplication,
}
