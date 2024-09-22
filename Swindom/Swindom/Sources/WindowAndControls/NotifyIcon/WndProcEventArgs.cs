namespace Swindom;

/// <summary>
/// 「WndProc」のEventArgs
/// </summary>
public class WndProcEventArgs : EventArgs
{
    /// <summary>
    /// Windows message
    /// </summary>
    public System.Windows.Forms.Message Message { get; private set; }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="message">Windows message</param>
    public WndProcEventArgs(
        System.Windows.Forms.Message message
        )
    {
        Message = message;
    }
}
