namespace Swindom;

/// <summary>
/// 処理イベントのデータ
/// </summary>
public class ProcessingEventArgs
{
    /// <summary>
    /// 処理イベントの種類
    /// </summary>
    public ProcessingEventType ProcessingEventType;

    /// <summary>
    /// コンストラクタ (使用しない)
    /// </summary>
    public ProcessingEventArgs()
    {
        throw new Exception("Do not use. - ProcessingEventArgs()");
    }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="processingEventType">処理イベントの種類</param>
    public ProcessingEventArgs(
        ProcessingEventType processingEventType
        )
    {
        ProcessingEventType = processingEventType;
    }
}
