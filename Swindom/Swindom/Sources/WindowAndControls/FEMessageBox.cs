using System.Linq;

namespace Swindom.Sources.WindowAndControls;

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
        FEMessageBoxWindow messageBox = new(GetActiveWindow(), "", "", MessageBoxButton.OK);
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
        FEMessageBoxWindow messageBox = new(GetActiveWindow(), message, title, button);
        messageBox.ShowDialog();
        return messageBox.Result;
    }

    /// <summary>
    /// メッセージウィンドウの表示を指示したウィンドウを取得
    /// </summary>
    /// <returns>Window</returns>
    private static Window? GetActiveWindow()
    {
        return Application.Current.Windows.Cast<Window>().FirstOrDefault(window => window.IsActive && window.ShowActivated);
    }
}
