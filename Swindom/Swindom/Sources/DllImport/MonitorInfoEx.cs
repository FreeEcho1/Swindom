namespace Swindom.Sources.DllImport;

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
public struct MonitorInfoEx
{
    public int Size;
    public RECT Monitor;
    public RECT WorkArea;
    public uint Flags;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
    public string DeviceName;
}
