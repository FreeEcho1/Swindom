namespace Swindom;

/// <summary>
/// MessageBox
/// </summary>
public static class FEMessageBox
{
    /// <summary>
    /// 表示
    /// </summary>
    /// <returns>押されたボタン</returns>
    public static MessageBoxResult Show()
    {
        FEMessageBoxWindow messageBox = new(WindowManagement.GetWindowToSetOwner(), "", "", MessageBoxButton.OK);
        messageBox.ShowDialog();
        return messageBox.Result;
    }

    /// <summary>
    /// 表示
    /// </summary>
    /// <param name="message">メッセージ</param>
    /// <param name="title">タイトル</param>
    /// <param name="button">表示するボタン</param>
    /// <returns>押されたボタン</returns>
    public static MessageBoxResult Show(
        string message,
        string title,
        MessageBoxButton button
        )
    {
        FEMessageBoxWindow messageBox = new(WindowManagement.GetWindowToSetOwner(), message, title, button);
        messageBox.ShowDialog();
        return messageBox.Result;
    }
}
