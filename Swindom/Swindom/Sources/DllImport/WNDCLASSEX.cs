namespace Swindom;

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
public class WNDCLASSEX
{
    [MarshalAs(UnmanagedType.U4)]
    public int cbSize;
    [MarshalAs(UnmanagedType.U4)]
    public int style;
    public IntPtr lpfnWndProc;
    public int cbClsExtra;
    public int cbWndExtra;
    public IntPtr hInstance;
    public IntPtr hIcon;
    public IntPtr hCursor;
    public IntPtr hbrBackground;
    [MarshalAs(UnmanagedType.LPStr)]
    public string lpszMenuName;
    [MarshalAs(UnmanagedType.LPStr)]
    public string lpszClassName;
    public IntPtr hIconSm;
}
