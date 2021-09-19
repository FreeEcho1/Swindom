namespace Swindom.Source
{
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
        /// 「イベント」の処理状態が変更された
        /// </summary>
        EventProcessingStateChanged,
        /// <summary>
        /// 「イベント」の一括実行
        /// </summary>
        BatchProcessingOfEvent,
        /// <summary>
        /// 「イベント」のアクティブウィンドウのみ処理
        /// </summary>
        OnlyActiveWindowEvent,
        /// <summary>
        /// 「イベント」の処理を一時停止
        /// </summary>
        EventPause,
        /// <summary>
        /// 「イベント」の処理の一時停止を解除
        /// </summary>
        EventUnpause,
        /// <summary>
        /// 「イベント」の受け取るイベントの種類が変更された
        /// </summary>
        ReceiveEventChanged,

        /// <summary>
        /// 「タイマー」の処理状態が変更された
        /// </summary>
        TimerProcessingStateChanged,
        /// <summary>
        /// 「タイマー」の「処理間隔」が変更された
        /// </summary>
        TimerProcessingInterval,
        /// <summary>
        /// 「タイマー」の一括実行
        /// </summary>
        BatchProcessingOfTimer,
        /// <summary>
        /// 「タイマー」のアクティブウィンドウのみ処理
        /// </summary>
        OnlyActiveWindowTimer,
        /// <summary>
        /// 「タイマー」の処理を一時停止
        /// </summary>
        TimerPause,
        /// <summary>
        /// 「タイマー」の処理の一時停止を解除
        /// </summary>
        TimerUnpause,

        /// <summary>
        /// 「マグネット」の処理状態が変更された
        /// </summary>
        MagnetProcessingStateChanged,
        /// <summary>
        /// 「マグネット」の「貼り付いた時の停止時間」が変更された
        /// </summary>
        MagnetStopTimeWhenPastingChanged,

        /// <summary>
        /// 「ホットキー」の処理状態が変更された
        /// </summary>
        HotkeyValidState,

        /// <summary>
        /// 「ウィンドウが画面外の場合は画面内に移動」の処理状態が変更された
        /// </summary>
        OutsideToInsideProcessingStateChanged,
        /// <summary>
        /// 「ウィンドウが画面外の場合は画面内に移動」の処理間隔が変更された
        /// </summary>
        OutsideToInsideProcessingIntervalChanged,
        /// <summary>
        /// 「ウィンドウが画面外の場合は画面内に移動」を処理
        /// </summary>
        DoOutsideToInside,
        /// <summary>
        /// 「ウィンドウを画面の中央に移動」の処理状態が変更された
        /// </summary>
        MoveCenterProcessingStateChanged,
    }
}
