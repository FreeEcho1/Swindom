﻿namespace Swindom;

/// <summary>
/// 処理イベントハンドラー
/// </summary>
public class ProcessingEventHandler
{
    /// <summary>
    /// 処理イベント
    /// </summary>
    public event EventHandler<ProcessingEventArgs> ProcessingEvent = delegate { };
    /// <summary>
    /// 処理イベントを実行
    /// </summary>
    /// <param name="processingEventType">処理イベントの種類</param>
    public virtual void DoProcessingEvent(
        ProcessingEventType processingEventType
        )
    {
        ProcessingEvent?.Invoke(this, new(processingEventType));
    }
}
