namespace Swindom.Sources.DllImport;

/// <summary>
/// NativeMethods
/// </summary>
internal static partial class NativeMethods
{
    [DllImport("user32.dll", SetLastError = true)]
    public static extern IntPtr GetForegroundWindow();
    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool GetWindowRect(
        IntPtr hWnd,
        out RECT lpRect
        );
    [DllImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool GetWindowPlacement(
        IntPtr hWnd,
        out WINDOWPLACEMENT lpwndpl
        );

    [DllImport("shell32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool Shell_NotifyIcon(
        uint dwMessage,
        ref NOTIFYICONDATA pnid
        );

    [DllImport("user32.dll", SetLastError = true)]
    public static extern IntPtr SetWindowsHookEx(
        WH hookType,
        HookProcDelegate lpfn,
        IntPtr hMod,
        uint dwThreadId
        );
    [DllImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool UnhookWindowsHookEx(
        IntPtr hhk
        );
    [DllImport("user32.dll")]
    public static extern IntPtr CallNextHookEx(
        IntPtr hhk,
        int nCode,
        IntPtr wParam,
        IntPtr lParam
        );
    [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    public static extern IntPtr GetModuleHandle(
        [MarshalAs(UnmanagedType.LPWStr)] in string lpModuleName
        );

    [DllImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool SetForegroundWindow(
        IntPtr hWnd
        );
    [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
    public static extern int GetWindowTextLength(
        IntPtr hWnd
        );
    [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
    public static extern int GetWindowText(
        IntPtr hWnd,
        [MarshalAs(UnmanagedType.LPWStr), Out] StringBuilder lpString,
        int nMaxCount
        );
    [DllImport("user32.dll", SetLastError = true)]
    public static extern uint GetWindowThreadProcessId(
        IntPtr hWnd,
        out int lpdwProcessId
        );
    [DllImport("kernel32.dll", SetLastError = true)]
    public static extern IntPtr OpenProcess(
        ProcessAccessFlags processAccess,
        [MarshalAs(UnmanagedType.Bool)] bool bInheritHandle,
        int processId
        );
    [DllImport("psapi.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool EnumProcessModules(
        IntPtr hProcess,
        out IntPtr lphModule,
        uint cb,
        [MarshalAs(UnmanagedType.U4)] out uint lpcbNeeded
        );
    [DllImport("psapi.dll", SetLastError = true, CharSet = CharSet.Unicode)]
    public static extern uint GetModuleFileNameEx(
        IntPtr hProcess,
        IntPtr hModule,
        [MarshalAs(UnmanagedType.LPWStr), Out] StringBuilder lpBaseName,
        int nSize
        );
    [DllImport("kernel32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool CloseHandle(
        IntPtr hObject
        );
    [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
    public static extern int GetClassName(
        IntPtr hWnd,
        [MarshalAs(UnmanagedType.LPWStr), Out] StringBuilder lpClassName,
        int nMaxCount
        );
    [DllImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool SetWindowPos(
        IntPtr hWnd,
        int hWndInsertAfter,
        int X,
        int Y,
        int cx,
        int cy,
        int uFlags
        );
    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool ClientToScreen(
        IntPtr hWnd,
        ref POINT lpPoint
        );
    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool GetClientRect(
        IntPtr hWnd,
        out RECT lpRect
        );
    [DllImport("user32.dll", EntryPoint = "GetWindowLong")]
    private static extern int GetWindowLong32(
        IntPtr hWnd,
        int nIndex
        );
    [DllImport("user32.dll", EntryPoint = "GetWindowLongPtr")]
    private static extern IntPtr GetWindowLongPtr64(
        IntPtr hWnd,
        int nIndex
        );
    public static long GetWindowLongPtr(
        IntPtr hWnd,
        int nIndex
        )
    {
        if (IntPtr.Size == 8)
        {
            return (long)GetWindowLongPtr64(hWnd, nIndex);
        }
        else
        {
            return GetWindowLong32(hWnd, nIndex);
        }
    }
    [DllImport("user32.dll", EntryPoint = "SetWindowLong")]
    private static extern int SetWindowLong32(
        IntPtr hWnd,
        int nIndex,
        int dwNewLong
        );
    [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr")]
    private static extern IntPtr SetWindowLongPtr64(
        IntPtr hWnd,
        int nIndex,
        int dwNewLong
        );
    public static long SetWindowLongPtr(
        IntPtr hWnd,
        int nIndex,
        int dwNewLong
        )
    {
        if (IntPtr.Size == 8)
        {
            return (long)SetWindowLongPtr64(hWnd, nIndex, dwNewLong);
        }
        else
        {
            return SetWindowLong32(hWnd, nIndex, dwNewLong);
        }
    }
    [DllImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool SetLayeredWindowAttributes(
        IntPtr hWnd,
        uint crKey,
        byte bAlpha,
        uint dwFlags
        );
    [DllImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool GetLayeredWindowAttributes(
        IntPtr hWnd,
        out uint crKey,
        out byte bAlpha,
        out uint dwFlags
        );
    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool ShowWindow(
        IntPtr hWnd,
        int nCmdShow
        );

    [DllImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool RegisterHotKey(
        IntPtr hWnd,
        int id,
        uint fsModifiers,
        uint vk
        );
    [DllImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool UnregisterHotKey(
        IntPtr hWnd,
        int id
        );

    [DllImport("user32.dll", EntryPoint = "EnumWindows")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool EnumWindows(
        EnumWindowsProc lpEnumFunc,
        ref HwndList lParam
        );
    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool IsWindowVisible(
        IntPtr hWnd
        );
    [DllImport("dwmapi.dll")]
    public static extern int DwmGetWindowAttribute(
        IntPtr hWnd,
        uint dwAttribute,
        [MarshalAs(UnmanagedType.Bool)] out bool pvAttribute,
        int cbAttribute
        );
    [DllImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool PostMessage(
        IntPtr hWnd,
        uint Msg,
        IntPtr wParam,
        IntPtr lParam
        );
    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool IsWindow(IntPtr hWnd);
    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool IsWindowEnabled(IntPtr hWnd);

    [DllImport("user32.dll", EntryPoint = "EnumDisplayMonitors")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool EnumDisplayMonitors(
        IntPtr hdc,
        IntPtr lprcClip,
        EnumMonitorsDelegate lpfnEnum,
        MonitorInformation dwData
        );
    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool GetMonitorInfo(
        IntPtr hMonitor,
        ref MonitorInfoEx lpmi
        );

    [DllImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool GetCursorPos(
        out POINT lpPoint
        );
    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool GetClipCursor(
        out RECT lpRect
        );
    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool ClipCursor(
        RectClass lpRect
        );
    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool ClipCursor(
        IntPtr lpRect
        );

    //[DllImport("user32.dll", ExactSpelling = true)]
    //public static extern IntPtr GetParent(
    //    IntPtr hWnd
    //    );
    [DllImport("user32.dll")]
    public static extern IntPtr GetAncestor(
        IntPtr hwnd,
        GetAncestorFlags flags
        );

    public delegate int EnumWindowsProc(
        IntPtr hwnd,
        ref HwndList lParam
        );
    public delegate int EnumMonitorsDelegate(
        IntPtr hMonitor,
        IntPtr hdcMonitor,
        ref RECT lprcMonitor,
        MonitorInformation dwData);
    public delegate IntPtr HookProcDelegate(
        int nCode,
        IntPtr wParam,
        IntPtr lParam
        );
    public delegate IntPtr WndProc(
        IntPtr hWnd,
        uint msg,
        IntPtr wParam,
        IntPtr lParam
        );

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool UpdateWindow(
        IntPtr hWnd
        );
    [DllImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool DestroyWindow(
        IntPtr hwnd
        );
    [DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
    public static extern IntPtr CreateWindowEx(
        uint dwExStyle,
        string lpClassName,
        string lpWindowName,
        uint dwStyle,
        int x,
        int y,
        int nWidth,
        int nHeight,
        IntPtr hWndParent,
        IntPtr hMenu,
        IntPtr hInstance,
        IntPtr lpParam
    );
    [DllImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.U2)]
    public static extern short RegisterClassEx(
        [In] ref WNDCLASSEX lpwcx
        );
    [DllImport("kernel32.dll")]
    public static extern uint GetLastError();
    [DllImport("user32.dll")]
    public static extern IntPtr DefWindowProc(
        IntPtr hWnd,
        uint uMsg,
        IntPtr wParam,
        IntPtr lParam
        );
    [DllImport("user32.dll")]
    public static extern void PostQuitMessage(
        int nExitCode
        );
    [DllImport("user32.dll")]
    public static extern IntPtr LoadCursor(
        IntPtr hInstance,
        int lpCursorName
        );
    [DllImport("user32.dll")]
    public static extern bool TranslateMessage(
        [In] ref MSG lpMsg
        );
    [DllImport("user32.dll")]
    public static extern IntPtr DispatchMessage(
        [In] ref MSG lpmsg
        );
}
