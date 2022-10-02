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
    /// <param name="hwnd">ウィンドウハンドル</param>
    /// <param name="monitorInfo">MonitorInfoEx</param>
    /// <param name="monitorInformation">モニター情報 (「null」でも可)</param>
    /// <returns>結果 (失敗「false」/成功「true」)</returns>
    public static bool GetMonitorWithWindow(
        IntPtr hwnd,
        out MonitorInfoEx monitorInfo,
        MonitorInformation? monitorInformation = null
        )
    {
        monitorInfo = new();
        if (monitorInformation == null)
        {
            monitorInformation = GetMonitorInformation();
        }
        bool result = false;        // 結果
        WINDOWPLACEMENT windowPlacement;       // WINDOWPLACEMENT
        NativeMethods.GetWindowPlacement(hwnd, out windowPlacement);
        System.Drawing.Point centerPositionOfTheWindow = new(windowPlacement.rcNormalPosition.Left + (windowPlacement.rcNormalPosition.Right - windowPlacement.rcNormalPosition.Left) / 2, windowPlacement.rcNormalPosition.Top + (windowPlacement.rcNormalPosition.Bottom - windowPlacement.rcNormalPosition.Top) / 2);     // ウィンドウの中央の位置

        // ウィンドウの中央の位置があるモニターを調べる
        foreach (MonitorInfoEx nowMonitorInfo in monitorInformation.MonitorInfo)
        {
            if (nowMonitorInfo.Monitor.Left < centerPositionOfTheWindow.X
                && nowMonitorInfo.Monitor.Top < centerPositionOfTheWindow.Y
                && centerPositionOfTheWindow.X < nowMonitorInfo.Monitor.Right
                && centerPositionOfTheWindow.Y < nowMonitorInfo.Monitor.Bottom)
            {
                monitorInfo = nowMonitorInfo;
                result = true;
                break;
            }
        }
        // ウィンドウの中央の位置があるモニターがない場合は重なっているモニターを調べる
        if (result == false)
        {
            System.Drawing.Rectangle windowRectangle = new(windowPlacement.rcNormalPosition.Left, windowPlacement.rcNormalPosition.Top, windowPlacement.rcNormalPosition.Right - windowPlacement.rcNormalPosition.Left, windowPlacement.rcNormalPosition.Bottom - windowPlacement.rcNormalPosition.Top);        // ウィンドウの位置とサイズ
            foreach (MonitorInfoEx nowMonitorInfo in monitorInformation.MonitorInfo)
            {
                System.Drawing.Rectangle monitorRectangle = new(nowMonitorInfo.Monitor.Left, nowMonitorInfo.Monitor.Top, nowMonitorInfo.Monitor.Right - nowMonitorInfo.Monitor.Left, nowMonitorInfo.Monitor.Bottom - nowMonitorInfo.Monitor.Top);     // モニターの位置とサイズ
                System.Drawing.Rectangle intersectRectangle = System.Drawing.Rectangle.Intersect(monitorRectangle, windowRectangle);     // 交差の値
                if (intersectRectangle.IsEmpty == false)
                {
                    monitorInfo = nowMonitorInfo;
                    result = true;
                    break;
                }
            }
        }

        return result;
    }
}
