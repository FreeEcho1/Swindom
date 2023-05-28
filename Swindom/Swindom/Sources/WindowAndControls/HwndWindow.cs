namespace Swindom;

/// <summary>
/// ウィンドウハンドル用のウィンドウ
/// </summary>
public class HwndWindow
{
    /// <summary>
    /// ウィンドウハンドル
    /// </summary>
    public IntPtr Hwnd;
    /// <summary>
    /// DelegateWndProc
    /// </summary>
    private readonly NativeMethods.WndProc DelegateWndProc = MyWndProc;

    /// <summary>
    /// ウィンドウ作成
    /// </summary>
    /// <returns></returns>
    public bool Create()
    {
        if (Hwnd != IntPtr.Zero)
        {
            return true;
        }

        WNDCLASSEX wndclass = new()
        {
            cbSize = Marshal.SizeOf(typeof(WNDCLASSEX)),
            style = (int)CS.CS_DBLCLKS,
            cbClsExtra = 0,
            cbWndExtra = 0,
            hInstance = Marshal.GetHINSTANCE(GetType().Module),
            lpszClassName = "SwindomNotifyIconWindow",
            lpfnWndProc = Marshal.GetFunctionPointerForDelegate(DelegateWndProc),
        };
        if (NativeMethods.RegisterClassEx(ref wndclass) == 0)
        {
            return false;
        }

        IntPtr hwnd = NativeMethods.CreateWindowEx(0, wndclass.lpszClassName, "", (uint)WS.WS_CAPTION | (uint)WS.WS_VISIBLE, 0, 0, 0, 0, IntPtr.Zero, IntPtr.Zero, wndclass.hInstance, IntPtr.Zero);
        if (hwnd == IntPtr.Zero)
        {
            return false;
        }
        Hwnd = hwnd;

        NativeMethods.ShowWindow(hwnd, (int)SW.SW_SHOWNORMAL);
        NativeMethods.UpdateWindow(hwnd);

        return true;
    }

    /// <summary>
    /// ウィンドウ破棄
    /// </summary>
    public void Destroy()
    {
        if (Hwnd != IntPtr.Zero)
        {
            NativeMethods.DestroyWindow(Hwnd);
            Hwnd = IntPtr.Zero;
        }
    }

    /// <summary>
    /// WndProc
    /// </summary>
    /// <param name="hWnd"></param>
    /// <param name="msg"></param>
    /// <param name="wParam"></param>
    /// <param name="lParam"></param>
    /// <returns></returns>
    private static IntPtr MyWndProc(
        IntPtr hWnd,
        uint msg,
        IntPtr wParam,
        IntPtr lParam
        )
    {
        switch (msg)
        {
            case (uint)WM.WM_DESTROY:
                NativeMethods.DestroyWindow(hWnd);
                break;
        }

        return NativeMethods.DefWindowProc(hWnd, msg, wParam, lParam);
    }
}
