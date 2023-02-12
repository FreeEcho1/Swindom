namespace Swindom.Sources.DllImport;

[StructLayout(LayoutKind.Sequential)]
public class MSLLHOOKSTRUCT
{
    public POINT pt;
    public uint mouseData;
    public uint flags;
    public uint time;
    public IntPtr dwExtraInfo;
}
