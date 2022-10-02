namespace Swindom;

/// <summary>
/// ホットキー処理
/// </summary>
public class HotkeyProcessing : IDisposable
{
    /// <summary>
    /// Disposeが呼ばれたかの値
    /// </summary>
    private bool Disposed;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public HotkeyProcessing()
    {
        RegisterHotkeys();
    }

    /// <summary>
    /// デストラクタ
    /// </summary>
    ~HotkeyProcessing()
    {
        Dispose(false);
    }

    /// <summary>
    /// Dispose
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// 非公開Dispose
    /// </summary>
    /// <param name="disposing"></param>
    protected virtual void Dispose(
        bool disposing
        )
    {
        if (Disposed == false)
        {
            Disposed = true;
            UnregisterHotkeys();
        }
    }

    /// <summary>
    /// 処理が有効か確認
    /// </summary>
    /// <returns>処理が有効かの値 (無効「false」/有効「true」)</returns>
    public static bool CheckIfTheProcessingIsValid() => (Common.ApplicationData.Settings.HotkeyInformation.Enabled
            && (Common.ApplicationData.Settings.HotkeyInformation.StopProcessingFullScreen == false || Common.ApplicationData.FullScreenExists == false));

    /// <summary>
    /// ホットキーを登録
    /// </summary>
    public void RegisterHotkeys()
    {
        UnregisterHotkeys();
        int id = Common.HotkeysStartId;       // ホットキーの識別子
        foreach (HotkeyItemInformation nowHII in Common.ApplicationData.Settings.HotkeyInformation.Items)
        {
            if (Processing.RegisterHotkey(Common.ApplicationData.SystemTrayIconHwnd, nowHII.Hotkey, id))
            {
                nowHII.HotkeyId = id;
                id++;
            }
        }
    }

    /// <summary>
    /// ホットキーを解除
    /// </summary>
    public void UnregisterHotkeys()
    {
        foreach (HotkeyItemInformation nowHII in Common.ApplicationData.Settings.HotkeyInformation.Items)
        {
            if (nowHII.HotkeyId != -1)
            {
                NativeMethods.UnregisterHotKey(Common.ApplicationData.SystemTrayIconHwnd, nowHII.HotkeyId);
                nowHII.HotkeyId = -1;
            }
        }
    }

    /// <summary>
    /// ホットキーを処理
    /// </summary>
    /// <param name="id">ホットキーの識別子</param>
    public void ProcessingHotkeys(
        int id
        )
    {
        try
        {
            IntPtr hwnd = NativeMethods.GetForegroundWindow();     // ウィンドウハンドル

            foreach (HotkeyItemInformation nowHII in Common.ApplicationData.Settings.HotkeyInformation.Items)
            {
                if (nowHII.HotkeyId == id)
                {
                    PerformHotkeyProcessing(hwnd, nowHII);
                    break;
                }
            }
        }
        catch
        {
        }
    }

    /// <summary>
    /// ホットキーの処理を実行
    /// </summary>
    /// <param name="hwnd">処理するウィンドウハンドル</param>
    /// <param name="hotkeyItemInformation">ホットキー情報</param>
    public void PerformHotkeyProcessing(
        IntPtr hwnd,
        HotkeyItemInformation hotkeyItemInformation
        )
    {
        switch (hotkeyItemInformation.ProcessingType)
        {
            case HotkeyProcessingType.StartStopEvent:
                Common.ApplicationData.Settings.EventInformation.Enabled = !Common.ApplicationData.Settings.EventInformation.Enabled;
                Common.ApplicationData.DoProcessingEvent(ProcessingEventType.EventProcessingStateChanged);
                break;
            case HotkeyProcessingType.StartStopTimer:
                Common.ApplicationData.Settings.TimerInformation.Enabled = !Common.ApplicationData.Settings.TimerInformation.Enabled;
                Common.ApplicationData.DoProcessingEvent(ProcessingEventType.TimerProcessingStateChanged);
                break;
            case HotkeyProcessingType.StartStopMagnet:
                Common.ApplicationData.Settings.MagnetInformation.Enabled = !Common.ApplicationData.Settings.MagnetInformation.Enabled;
                Common.ApplicationData.DoProcessingEvent(ProcessingEventType.MagnetProcessingStateChanged);
                break;
            case HotkeyProcessingType.AlwaysForefrontOrCancel:
                if ((NativeMethods.GetWindowLongPtr(hwnd, (int)GWL.GWL_EXSTYLE) & (int)WS_EX.WS_EX_TOPMOST) == (int)WS_EX.WS_EX_TOPMOST)
                {
                    NativeMethods.SetWindowPos(hwnd, (int)HwndInsertAfter.HWND_NOTOPMOST, 0, 0, 0, 0, (int)SWP.SWP_NOMOVE | (int)SWP.SWP_NOSIZE);
                }
                else
                {
                    NativeMethods.SetWindowPos(hwnd, (int)HwndInsertAfter.HWND_TOPMOST, 0, 0, 0, 0, (int)SWP.SWP_NOMOVE | (int)SWP.SWP_NOSIZE);
                }
                break;
            case HotkeyProcessingType.SpecifyTransparencyOrCancel:
                if ((NativeMethods.GetWindowLongPtr(hwnd, (int)GWL.GWL_EXSTYLE) & (int)WS_EX.WS_EX_LAYERED) == (int)WS_EX.WS_EX_LAYERED)
                {
                    NativeMethods.SetLayeredWindowAttributes(hwnd, 0, 255, (uint)LWA.LWA_ALPHA);
                    NativeMethods.SetWindowLongPtr(hwnd, (int)GWL.GWL_EXSTYLE, (int)(NativeMethods.GetWindowLongPtr(hwnd, (int)GWL.GWL_EXSTYLE) ^ (int)WS_EX.WS_EX_LAYERED));
                }
                else
                {
                    NativeMethods.SetWindowLongPtr(hwnd, (int)GWL.GWL_EXSTYLE, (int)(NativeMethods.GetWindowLongPtr(hwnd, (int)GWL.GWL_EXSTYLE) ^ (int)WS_EX.WS_EX_LAYERED));
                    NativeMethods.SetLayeredWindowAttributes(hwnd, 0, (byte)hotkeyItemInformation.ProcessingValue, (uint)LWA.LWA_ALPHA);
                }
                break;
            case HotkeyProcessingType.BatchEvent:
                Common.ApplicationData.DoProcessingEvent(ProcessingEventType.BatchProcessingOfEvent);
                break;
            case HotkeyProcessingType.OnlyActiveWindowEvent:
                Common.ApplicationData.DoProcessingEvent(ProcessingEventType.OnlyActiveWindowEvent);
                break;
            case HotkeyProcessingType.BatchTimer:
                Common.ApplicationData.DoProcessingEvent(ProcessingEventType.BatchProcessingOfTimer);
                break;
            case HotkeyProcessingType.OnlyActiveWindowTimer:
                Common.ApplicationData.DoProcessingEvent(ProcessingEventType.OnlyActiveWindowTimer);
                break;
            default:
                WINDOWPLACEMENT windowPlacement;
                NativeMethods.GetWindowPlacement(hwnd, out windowPlacement);

                if (windowPlacement.showCmd != (int)SW.SW_SHOWNORMAL)
                {
                    NativeMethods.ShowWindow(hwnd, (int)SW.SW_SHOWNORMAL);
                }

                switch (hotkeyItemInformation.ProcessingType)
                {
                    case HotkeyProcessingType.PositionSize:
                        {
                            RectangleDecimal windowRectangle = WindowProcessing.CalculatePositionAndSize(hwnd, windowPlacement, hotkeyItemInformation.PositionSize, hotkeyItemInformation.StandardDisplay, hotkeyItemInformation.PositionSize.ClientArea);
                            if (Common.ApplicationData.Settings.HotkeyInformation.DoNotChangeOutOfScreen == false || WindowProcessing.CheckWindowIsInTheScreen(windowRectangle))
                            {
                                // 1回目の処理
                                MonitorInfoEx monitorInfo1;
                                MonitorInformation.GetMonitorWithWindow(hwnd, out monitorInfo1);
                                NativeMethods.SetWindowPos(hwnd, 0, (int)windowRectangle.X, (int)windowRectangle.Y, (int)windowRectangle.Width, (int)windowRectangle.Height, (int)SWP.SWP_NOZORDER);
                                // 2回目の処理
                                MonitorInfoEx monitorInfo2;
                                MonitorInformation.GetMonitorWithWindow(hwnd, out monitorInfo2);
                                if (hotkeyItemInformation.PositionSize.ProcessingPositionAndSizeTwice
                                    || monitorInfo1.DeviceName != monitorInfo2.DeviceName
                                    || hotkeyItemInformation.PositionSize.ClientArea)
                                {
                                    NativeMethods.GetWindowPlacement(hwnd, out windowPlacement);
                                    windowRectangle = WindowProcessing.CalculatePositionAndSize(hwnd, windowPlacement, hotkeyItemInformation.PositionSize, hotkeyItemInformation.StandardDisplay, hotkeyItemInformation.PositionSize.ClientArea);
                                    NativeMethods.SetWindowPos(hwnd, 0, (int)windowRectangle.X, (int)windowRectangle.Y, (int)windowRectangle.Width, (int)windowRectangle.Height, (int)SWP.SWP_NOZORDER);
                                }
                            }
                        }
                        break;
                    case HotkeyProcessingType.MoveX:
                        NativeMethods.SetWindowPos(hwnd, 0, windowPlacement.rcNormalPosition.Left + hotkeyItemInformation.ProcessingValue, windowPlacement.rcNormalPosition.Top, 0, 0, (int)SWP.SWP_NOSIZE | (int)SWP.SWP_NOZORDER);
                        break;
                    case HotkeyProcessingType.MoveY:
                        NativeMethods.SetWindowPos(hwnd, 0, windowPlacement.rcNormalPosition.Left, windowPlacement.rcNormalPosition.Top + hotkeyItemInformation.ProcessingValue, 0, 0, (int)SWP.SWP_NOSIZE | (int)SWP.SWP_NOZORDER);
                        break;
                    case HotkeyProcessingType.IncreaseDecreaseWidth:
                        NativeMethods.SetWindowPos(hwnd, 0, 0, 0, windowPlacement.rcNormalPosition.Right - windowPlacement.rcNormalPosition.Left + hotkeyItemInformation.ProcessingValue, windowPlacement.rcNormalPosition.Bottom - windowPlacement.rcNormalPosition.Top, (int)SWP.SWP_NOMOVE | (int)SWP.SWP_NOZORDER);
                        break;
                    case HotkeyProcessingType.IncreaseDecreaseHeight:
                        NativeMethods.SetWindowPos(hwnd, 0, 0, 0, windowPlacement.rcNormalPosition.Right - windowPlacement.rcNormalPosition.Left, windowPlacement.rcNormalPosition.Bottom - windowPlacement.rcNormalPosition.Top + hotkeyItemInformation.ProcessingValue, (int)SWP.SWP_NOMOVE | (int)SWP.SWP_NOZORDER);
                        break;
                    case HotkeyProcessingType.IncreaseDecreaseWidthHeight:
                        NativeMethods.SetWindowPos(hwnd, 0, 0, 0, windowPlacement.rcNormalPosition.Right - windowPlacement.rcNormalPosition.Left + hotkeyItemInformation.ProcessingValue, windowPlacement.rcNormalPosition.Bottom - windowPlacement.rcNormalPosition.Top + hotkeyItemInformation.ProcessingValue, (int)SWP.SWP_NOMOVE | (int)SWP.SWP_NOZORDER);
                        break;
                }
                break;
        }
    }
}
