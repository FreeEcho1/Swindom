namespace Swindom;

/// <summary>
/// ウィンドウハンドルのリスト
/// </summary>
public class HwndList
{
    /// <summary>
    /// ウィンドウハンドル
    /// </summary>
    public List<IntPtr> Hwnd { get; } = new();

    /// <summary>
    /// ウィンドウが存在するウィンドウハンドルを取得
    /// </summary>
    /// <returns>ウィンドウハンドルのリスト</returns>
    public static HwndList GetWindowHandleList()
    {
        HwndList hwndList = new();
        NativeMethods.EnumWindows(new(EnumerateHwnd), ref hwndList);
        return hwndList;
    }

    /// <summary>
    /// ウィンドウが存在するウィンドウハンドルを列挙するコールバックメソッド
    /// </summary>
    /// <param name="hwnd">ウィンドウハンドル</param>
    /// <param name="lparam">アプリケーション定義の値</param>
    /// <returns>列挙中止「0」/列挙続行「0以外」</returns>
    private static int EnumerateHwnd(
        IntPtr hwnd,
        ref HwndList lparam
        )
    {
        try
        {
            // 負荷が少なくなると思われる順番で判定。
            // 正確には判定できていない。
            if (NativeMethods.IsWindowVisible(hwnd))
            {
                // ウィンドウのないUWPアプリかを取得
                _ = NativeMethods.DwmGetWindowAttribute(hwnd, (uint)DWMWINDOWATTRIBUTE.Cloaked, out bool isInvisibleUwpApp, Marshal.SizeOf(typeof(bool)));

                if (isInvisibleUwpApp == false
                    && (NativeMethods.GetWindowLongPtr(hwnd, (int)GWL.GWL_EXSTYLE) & (int)WS_EX.WS_EX_TOOLWINDOW) == 0)
                {
                    lparam.Hwnd.Add(hwnd);
                }
            }
        }
        catch
        {
        }

        return 1;
    }
}
