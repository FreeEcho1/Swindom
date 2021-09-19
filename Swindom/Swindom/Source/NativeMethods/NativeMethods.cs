namespace Swindom.Source
{
    /// <summary>
    /// NativeMethods
    /// </summary>
    internal static class NativeMethods
    {
        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        [return: System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.Bool)]
        public static extern bool SetForegroundWindow(
            System.IntPtr hWnd
            );
        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        public static extern System.IntPtr GetForegroundWindow();
        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Unicode)]
        public static extern int GetWindowTextLength(
            System.IntPtr hWnd
            );
        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true, CharSet = System.Runtime.InteropServices.CharSet.Unicode)]
        public static extern int GetWindowText(
            System.IntPtr hWnd,
            [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.LPWStr), System.Runtime.InteropServices.Out] System.Text.StringBuilder lpString,
            int nMaxCount
            );
        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        public static extern uint GetWindowThreadProcessId(
            System.IntPtr hWnd,
            out int lpdwProcessId
            );
        [System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError = true)]
        public static extern System.IntPtr OpenProcess(
            ProcessAccessFlags processAccess,
            [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.Bool)] bool bInheritHandle,
            int processId
            );
        [System.Runtime.InteropServices.DllImport("psapi.dll", SetLastError = true)]
        [return: System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.Bool)]
        public static extern bool EnumProcessModules(
            System.IntPtr hProcess,
            out System.IntPtr lphModule,
            uint cb,
            [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.U4)] out uint lpcbNeeded
            );
        [System.Runtime.InteropServices.DllImport("psapi.dll", SetLastError = true, CharSet = System.Runtime.InteropServices.CharSet.Unicode)]
        public static extern uint GetModuleFileNameEx(
            System.IntPtr hProcess,
            System.IntPtr hModule,
            [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.LPWStr), System.Runtime.InteropServices.Out] System.Text.StringBuilder lpBaseName,
            int nSize
            );
        [System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError = true)]
        [return: System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.Bool)]
        public static extern bool CloseHandle(
            System.IntPtr hObject
            );
        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true, CharSet = System.Runtime.InteropServices.CharSet.Unicode)]
        public static extern int GetClassName(
            System.IntPtr hWnd,
            [System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.LPWStr), System.Runtime.InteropServices.Out] System.Text.StringBuilder lpClassName,
            int nMaxCount
            );
        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        [return: System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.Bool)]
        public static extern bool SetWindowPos(
            System.IntPtr hWnd,
            int hWndInsertAfter,
            int X,
            int Y,
            int cx,
            int cy,
            int uFlags
            );
        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Unicode)]
        [return: System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.Bool)]
        public static extern bool GetWindowRect(
            System.IntPtr hWnd,
            out RECT lpRect
            );
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool ClientToScreen(
            System.IntPtr hWnd,
            ref POINT lpPoint
            );
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool GetClientRect(
            System.IntPtr hWnd,
            out RECT lpRect
            );
        [System.Runtime.InteropServices.DllImport("user32.dll", EntryPoint = "GetWindowLong")]
        private static extern int GetWindowLong32(
            System.IntPtr hWnd,
            int nIndex
            );
        [System.Runtime.InteropServices.DllImport("user32.dll", EntryPoint = "GetWindowLongPtr")]
        private static extern System.IntPtr GetWindowLongPtr64(
            System.IntPtr hWnd,
            int nIndex
            );
        public static long GetWindowLongPtr(
            System.IntPtr hWnd,
            int nIndex
            )
        {
            if (System.IntPtr.Size == 8)
            {
                return ((long)GetWindowLongPtr64(hWnd, nIndex));
            }
            else
            {
                return (GetWindowLong32(hWnd, nIndex));
            }
        }
        [System.Runtime.InteropServices.DllImport("user32.dll", EntryPoint = "SetWindowLong")]
        private static extern int SetWindowLong32(
            System.IntPtr hWnd,
            int nIndex,
            int dwNewLong
            );
        [System.Runtime.InteropServices.DllImport("user32.dll", EntryPoint = "SetWindowLongPtr")]
        private static extern System.IntPtr SetWindowLongPtr64(
            System.IntPtr hWnd,
            int nIndex,
            int dwNewLong
            );
        public static long SetWindowLongPtr(
            System.IntPtr hWnd,
            int nIndex,
            int dwNewLong
            )
        {
            if (System.IntPtr.Size == 8)
            {
                return ((long)SetWindowLongPtr64(hWnd, nIndex, dwNewLong));
            }
            else
            {
                return (SetWindowLong32(hWnd, nIndex, dwNewLong));
            }
        }
        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true, CharSet = System.Runtime.InteropServices.CharSet.Unicode)]
        [return: System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.Bool)]
        public static extern bool SetLayeredWindowAttributes(
            System.IntPtr hWnd,
            uint crKey,
            byte bAlpha,
            uint dwFlags
            );
        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true, CharSet = System.Runtime.InteropServices.CharSet.Unicode)]
        [return: System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.Bool)]
        public static extern bool GetLayeredWindowAttributes(
            System.IntPtr hWnd,
            out uint crKey,
            out byte bAlpha,
            out uint dwFlags
            );
        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true, CharSet = System.Runtime.InteropServices.CharSet.Unicode)]
        [return: System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.Bool)]
        public static extern bool GetWindowPlacement(
            System.IntPtr hWnd,
            out WINDOWPLACEMENT lpwndpl
            );
        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Unicode)]
        [return: System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.Bool)]
        public static extern bool ShowWindow(
            System.IntPtr hWnd,
            int nCmdShow
            );

        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true, CharSet = System.Runtime.InteropServices.CharSet.Unicode)]
        [return: System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.Bool)]
        public extern static bool RegisterHotKey(
            System.IntPtr hWnd,
            int id,
            uint fsModifiers,
            uint vk
            );
        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true, CharSet = System.Runtime.InteropServices.CharSet.Unicode)]
        [return: System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.Bool)]
        public extern static bool UnregisterHotKey(
            System.IntPtr hWnd,
            int id
            );

        [System.Runtime.InteropServices.DllImport("user32.dll", EntryPoint = "EnumWindows")]
        [return: System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.Bool)]
        public static extern bool EnumWindows(
            NativeMethodsDelegate.EnumWindowCallbackDelegate lpEnumFunc,
            ref HwndList lParam
            );
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        [return: System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.Bool)]
        public static extern bool IsWindowVisible(
            System.IntPtr hWnd
            );
        [System.Runtime.InteropServices.DllImport("dwmapi.dll")]
        public static extern int DwmGetWindowAttribute(
            System.IntPtr hWnd,
            uint dwAttribute,
            out bool pvAttribute,
            int cbAttribute
            );
        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true, CharSet = System.Runtime.InteropServices.CharSet.Unicode)]
        [return: System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.Bool)]
        public static extern bool PostMessage(
            System.IntPtr hWnd,
            uint Msg,
            System.IntPtr wParam,
            System.IntPtr lParam
            );
        //[System.Runtime.InteropServices.DllImport("user32.dll")]
        //public static extern uint GetDoubleClickTime();
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        [return: System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.Bool)]
        public static extern bool IsWindow(System.IntPtr hWnd);

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool EnumDisplayMonitors(
            System.IntPtr hdc,
            System.IntPtr lprcClip,
            NativeMethodsDelegate.EnumMonitorsDelegate lpfnEnum,
            MonitorInformation dwData
            );
        [System.Runtime.InteropServices.DllImport("user32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        public static extern bool GetMonitorInfo(
            System.IntPtr hMonitor,
            ref MonitorInfoEx lpmi
            );

        [System.Runtime.InteropServices.DllImport("user32.dll", SetLastError = true)]
        [return: System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.Bool)]
        public static extern bool GetCursorPos(
            out POINT lpPoint
            );
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool GetClipCursor(
            out RECT lpRect
            );
        [System.Runtime.InteropServices.DllImport("user32.dll")]
        public static extern bool ClipCursor(
            RectClass lpRect
            );

        //[System.Runtime.InteropServices.DllImport("user32.dll", ExactSpelling = true, CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        //public static extern System.IntPtr GetParent(
        //    System.IntPtr hWnd
        //    );
        [System.Runtime.InteropServices.DllImport("user32.dll", ExactSpelling = true)]
        public static extern System.IntPtr GetAncestor(
            System.IntPtr hwnd,
            GetAncestorFlags flags
            );

        [System.Runtime.InteropServices.DllImport("shell32.dll")]
        public static extern bool Shell_NotifyIcon(
            uint dwMessage,
            [System.Runtime.InteropServices.In] ref NOTIFYICONDATA pnid
            );
    }
}
