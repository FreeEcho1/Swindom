namespace Swindom;

/// <summary>
/// モニター情報
/// </summary>
public class MonitorInformation
{
    /// <summary>
    /// モニター情報
    /// </summary>
    public List<MonitorInfoEx> MonitorInfo = new();

    /// <summary>
    /// モニター情報を取得
    /// </summary>
    public static MonitorInformation GetMonitorInformation()
    {
        MonitorInformation monitorInformation = new();
        NativeMethods.EnumDisplayMonitors(IntPtr.Zero, IntPtr.Zero, new(MonitorEnumProc), monitorInformation);
        return monitorInformation;
    }

    /// <summary>
    /// モニター情報の列挙プロシージャ
    /// </summary>
    /// <param name="hMonitor"></param>
    /// <param name="hdcMonitor"></param>
    /// <param name="lprcMonitor"></param>
    /// <param name="dwData"></param>
    /// <returns></returns>
    private static int MonitorEnumProc(
        IntPtr hMonitor,
        IntPtr hdcMonitor,
        ref RECT lprcMonitor,
        MonitorInformation dwData
        )
    {
        try
        {
            MonitorInfoEx monitorInfo = new();
            monitorInfo.Size = Marshal.SizeOf(monitorInfo);
            NativeMethods.GetMonitorInfo(hMonitor, ref monitorInfo);
            dwData.MonitorInfo.Add(monitorInfo);
        }
        catch
        {
            return 0;
        }

        return 1;
    }

    /// <summary>
    /// ウィンドウがあるモニター情報を取得
    /// </summary>
    /// <param name="rectangle">ウィンドウの上下左右の位置</param>
    /// <param name="monitorInfo">MonitorInfoEx</param>
    /// <returns>結果 (どのディスプレイにも入っていない「false」/ディスプレイに入っている「true」)</returns>
    public static bool GetMonitorInformationOnWindowShown(
        RectangleInt rectangle,
        out MonitorInfoEx monitorInfo
        )
    {
        monitorInfo = new();

        // ウィンドウの中央の位置があるモニターを調べる
        System.Drawing.Point centerPositionOfTheWindow = new(rectangle.Left + (rectangle.Right - rectangle.Left) / 2, rectangle.Top + (rectangle.Bottom - rectangle.Top) / 2);     // ウィンドウの中央の位置
        foreach (MonitorInfoEx nowMonitorInfo in ApplicationData.MonitorInformation.MonitorInfo)
        {
            if (nowMonitorInfo.Monitor.Left <= centerPositionOfTheWindow.X
                && nowMonitorInfo.Monitor.Top <= centerPositionOfTheWindow.Y
                && centerPositionOfTheWindow.X < nowMonitorInfo.Monitor.Right
                && centerPositionOfTheWindow.Y < nowMonitorInfo.Monitor.Bottom)
            {
                monitorInfo = nowMonitorInfo;
                return true;
            }
        }

        // ウィンドウの中央の位置があるモニターがない場合は重なっているモニターを調べる
        Rectangle windowRectangle = new(rectangle.Left, rectangle.Top, rectangle.Right - rectangle.Left, rectangle.Bottom - rectangle.Top);        // ウィンドウの位置とサイズ
        foreach (MonitorInfoEx nowMonitorInfo in ApplicationData.MonitorInformation.MonitorInfo)
        {
            Rectangle monitorRectangle = new(nowMonitorInfo.Monitor.Left, nowMonitorInfo.Monitor.Top, nowMonitorInfo.Monitor.Right - nowMonitorInfo.Monitor.Left, nowMonitorInfo.Monitor.Bottom - nowMonitorInfo.Monitor.Top);     // モニターの位置とサイズ
            Rectangle intersectRectangle = Rectangle.Intersect(monitorRectangle, windowRectangle);     // 交差の値
            if (intersectRectangle.IsEmpty == false)
            {
                monitorInfo = nowMonitorInfo;
                return true;
            }
        }

        return false;
    }
}
