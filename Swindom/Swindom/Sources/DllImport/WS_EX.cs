namespace Swindom;

[Flags]
public enum WS_EX : long
{
    WS_EX_APPWINDOW = 0x00040000,
    WS_EX_LAYERED = 0x00080000,
    WS_EX_TOPMOST = 0x00000008,
    WS_EX_TOOLWINDOW = 0x00000080,
    WS_EX_NOACTIVATE = 0x08000000,
}
