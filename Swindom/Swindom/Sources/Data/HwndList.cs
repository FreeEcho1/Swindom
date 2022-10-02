namespace Swindom;

/// <summary>
/// ウィンドウハンドルのリスト
/// </summary>
public class HwndList
{
    /// <summary>
    /// ウィンドウハンドル
    /// </summary>
    public List<IntPtr> Hwnd = new();

    /// <summary>
    /// ウィンドウが存在するウィンドウハンドルを取得
    /// </summary>
    /// <returns>ウィンドウハンドルのリスト</returns>
    public static HwndList GetWindowHandleList()
    {
        HwndList hwndList = new();
        NativeMethods.EnumWindows(new(WindowProcessing.EnumerateHwnd), ref hwndList);
        return hwndList;
    }
}
