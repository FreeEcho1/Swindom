namespace Swindom.Source
{
    /// <summary>
    /// NativeMethodsのデリゲート
    /// </summary>
    public static class NativeMethodsDelegate
    {
        public delegate int EnumWindowCallbackDelegate(
            System.IntPtr hwnd,
            ref HwndList lParam
            );
        public delegate int EnumMonitorsDelegate(
            System.IntPtr hMonitor,
            System.IntPtr hdcMonitor,
            ref RECT lprcMonitor,
            MonitorInformation dwData);
    }
}
