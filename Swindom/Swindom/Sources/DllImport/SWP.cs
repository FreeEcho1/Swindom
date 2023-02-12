namespace Swindom.Sources.DllImport;

[Flags]
public enum SWP : int
{
    SWP_NOSIZE = 0x0001,
    SWP_NOMOVE = 0x0002,
    SWP_NOZORDER = 0x0004,
    SWP_NOACTIVATE = 0x0010,
    SWP_NOOWNERZORDER = 0x0200,
}
