namespace Swindom;

/// <summary>
/// モニター情報
/// </summary>
public class MonitorInformation
{
    /// <summary>
    /// モニター情報
    /// </summary>
    public List<MonitorInfoEx> MonitorInfo { get; } = new();

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
    /// 指定領域があるモニター情報を取得
    /// </summary>
    /// <param name="specifiedAreaRectangle">ウィンドウの上下左右の位置</param>
    /// <param name="monitorInfo">MonitorInfoEx</param>
    /// <returns>結果 (どのディスプレイにも入っていない「false」/ディスプレイに入っている「true」)</returns>
    public static bool GetMonitorInformationForSpecifiedArea(
        RectangleInt specifiedAreaRectangle,
        out MonitorInfoEx monitorInfo
        )
    {
        monitorInfo = new();

        // 指定領域の中央の位置があるモニターを調べる
        System.Drawing.Point centerPositionOfSpecifiedArea = new(specifiedAreaRectangle.Left + specifiedAreaRectangle.Width / 2, specifiedAreaRectangle.Top + specifiedAreaRectangle.Height / 2);     // 指定領域の中央の位置
        foreach (MonitorInfoEx nowMonitorInfo in ApplicationData.MonitorInformation.MonitorInfo)
        {
            if (nowMonitorInfo.Monitor.Left <= centerPositionOfSpecifiedArea.X
                && nowMonitorInfo.Monitor.Top <= centerPositionOfSpecifiedArea.Y
                && centerPositionOfSpecifiedArea.X < nowMonitorInfo.Monitor.Right
                && centerPositionOfSpecifiedArea.Y < nowMonitorInfo.Monitor.Bottom)
            {
                monitorInfo = nowMonitorInfo;
                return true;
            }
        }

        // 指定領域の中央の位置があるモニターがない場合は重なっているモニターを調べる
        foreach (MonitorInfoEx nowMonitorInfo in ApplicationData.MonitorInformation.MonitorInfo)
        {
            RectangleInt monitorRectangle = new(nowMonitorInfo.Monitor.Left, nowMonitorInfo.Monitor.Top, nowMonitorInfo.Monitor.Right - nowMonitorInfo.Monitor.Left, nowMonitorInfo.Monitor.Bottom - nowMonitorInfo.Monitor.Top);     // モニターの位置とサイズ
            RectangleInt intersectRectangle = RectangleInt.Intersect(monitorRectangle, specifiedAreaRectangle);     // 交差の値
            if (intersectRectangle.IsEmpty == false)
            {
                monitorInfo = nowMonitorInfo;
                return true;
            }
        }

        return false;
    }
}
