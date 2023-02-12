namespace Swindom.Sources.DllImport;

[Flags]
public enum WS_EX : int
{
    WS_EX_LAYERED = 0x00080000,
    WS_EX_TOPMOST = 0x00000008,
    WS_EX_TOOLWINDOW = 0x00000080
}
