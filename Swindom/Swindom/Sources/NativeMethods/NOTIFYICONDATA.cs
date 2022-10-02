namespace Swindom;

public struct NOTIFYICONDATA
{
    public int cbSize;
    public IntPtr hwnd;
    public int uID;
    public int uFlags;
    public int uCallbackMessage;
    public IntPtr hIcon;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
    public string szTip;
    public int dwState;
    public int dwStateMask;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
    public string szInfo;
    public int uVersion;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
    public string szInfoTitle;
    public int dwInfoFlags;
}
