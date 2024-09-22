namespace Swindom;

/// <summary>
/// 「ホットキー」処理
/// </summary>
public class HotkeyProcessing : IDisposable
{
    /// <summary>
    /// Disposed
    /// </summary>
    private bool Disposed;
    /// <summary>
    /// ホットキーを処理するかの値 (いいえ「false」/はい「true」)
    /// </summary>
    private bool DoHotkeyProcessing = true;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public HotkeyProcessing()
    {
        ComponentDispatcher.ThreadPreprocessMessage += ComponentDispatcher_ThreadPreprocessMessage;
        ApplicationData.EventData.ProcessingEvent += ApplicationData_ProcessingEvent;
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
    /// <param name="disposing">disposing</param>
    protected virtual void Dispose(
        bool disposing
        )
    {
        if (Disposed)
        {
            return;
        }
        if (disposing)
        {
            ComponentDispatcher.ThreadPreprocessMessage -= ComponentDispatcher_ThreadPreprocessMessage;
            ApplicationData.EventData.ProcessingEvent -= ApplicationData_ProcessingEvent;
            UnregisterHotkeys();
        }
        Disposed = true;
    }

    /// <summary>
    /// 「ThreadPreprocessMessage」メッセージ (ホットキー処理)
    /// </summary>
    /// <param name="msg"></param>
    /// <param name="handled"></param>
    private void ComponentDispatcher_ThreadPreprocessMessage(
        ref MSG msg,
        ref bool handled
        )
    {
        try
        {
            if (msg.message == (int)WM.WM_HOTKEY && DoHotkeyProcessing)
            {
                int id = (int)msg.wParam;       // ホットキーのID
                IntPtr hwnd = NativeMethods.GetForegroundWindow();     // ウィンドウハンドル

                foreach (HotkeyItemInformation nowHII in ApplicationData.Settings.HotkeyInformation.Items)
                {
                    if (nowHII.HotkeyId == id)
                    {
                        PerformHotkeyProcessing(hwnd, nowHII);
                        break;
                    }
                }
            }
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「処理」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ApplicationData_ProcessingEvent(
        object? sender,
        ProcessingEventArgs e
        )
    {
        try
        {
            switch (e.ProcessingEventType)
            {
                case ProcessingEventType.PauseHotkeyProcessing:
                    DoHotkeyProcessing = false;
                    break;
                case ProcessingEventType.UnpauseHotkeyProcessing:
                    DoHotkeyProcessing = true;
                    break;
            }
        }
        catch
        {
        }
    }

    /// <summary>
    /// 処理が有効か確認
    /// </summary>
    /// <returns>処理が有効かの値 (無効「false」/有効「true」)</returns>
    public static bool CheckIfTheProcessingIsValid() => (ApplicationData.Settings.HotkeyInformation.IsEnabled
            && (ApplicationData.Settings.HotkeyInformation.StopProcessingFullScreen == false || ApplicationData.FullScreenExists == false));

    /// <summary>
    /// ホットキーを登録
    /// </summary>
    public void RegisterHotkeys()
    {
        UnregisterHotkeys();

        if (CheckIfTheProcessingIsValid() == false)
        {
            return;
        }

        foreach (HotkeyItemInformation nowHII in ApplicationData.Settings.HotkeyInformation.Items)
        {
            try
            {
                nowHII.HotkeyId = HotkeyControlProcessing.RegisterHotkey(nowHII.Hotkey);
            }
            catch
            {
            }
        }
    }

    /// <summary>
    /// ホットキーを解除
    /// </summary>
    public void UnregisterHotkeys()
    {
        foreach (HotkeyItemInformation nowHII in ApplicationData.Settings.HotkeyInformation.Items)
        {
            if (nowHII.HotkeyId != -1)
            {
                try
                {
                    HotkeyControlProcessing.UnregisterHotkey(nowHII.HotkeyId);
                    nowHII.HotkeyId = -1;
                }
                catch
                {
                }
            }
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
        if (ApplicationData.Settings.HotkeyInformation.StopProcessingFullScreen && CheckFullScreenProcessing.CheckFullScreenWindow(hwnd))
        {
            return;
        }

        switch (hotkeyItemInformation.ProcessingType)
        {
            case HotkeyProcessingType.StartStopSpecifyWindow:
                ApplicationData.Settings.SpecifyWindowInformation.IsEnabled = !ApplicationData.Settings.SpecifyWindowInformation.IsEnabled;
                ApplicationData.EventData.DoProcessingEvent(ProcessingEventType.SpecifyWindowProcessingStateChanged);
                break;
            case HotkeyProcessingType.StartStopAllWindow:
                ApplicationData.Settings.AllWindowInformation.IsEnabled = !ApplicationData.Settings.AllWindowInformation.IsEnabled;
                ApplicationData.EventData.DoProcessingEvent(ProcessingEventType.AllWindowProcessingStateChanged);
                break;
            case HotkeyProcessingType.StartStopMagnet:
                ApplicationData.Settings.MagnetInformation.IsEnabled = !ApplicationData.Settings.MagnetInformation.IsEnabled;
                ApplicationData.EventData.DoProcessingEvent(ProcessingEventType.MagnetProcessingStateChanged);
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
            case HotkeyProcessingType.BatchSpecifyWindow:
                ApplicationData.EventData.DoProcessingEvent(ProcessingEventType.SpecifyWindowBatchProcessing);
                break;
            case HotkeyProcessingType.OnlyActiveWindowSpecifyWindow:
                ApplicationData.EventData.DoProcessingEvent(ProcessingEventType.SpecifyWindowOnlyActiveWindow);
                break;
            case HotkeyProcessingType.ShowThisApplicationWindow:
                ApplicationData.WindowManagement.ShowMainWindow();
                break;
            case HotkeyProcessingType.ShowNotifyIconMenu:
                ApplicationData.EventData.DoProcessingEvent(ProcessingEventType.ShowNotifyIconContextMenu);
                break;
            default:
                WindowInformation windowInformation = WindowProcessing.GetWindowRectangleAndStateFromHandle(hwnd);

                if (windowInformation.ShowCmd != (int)SW.SW_SHOWNORMAL)
                {
                    NativeMethods.ShowWindow(hwnd, (int)SW.SW_SHOWNORMAL);
                }

                switch (hotkeyItemInformation.ProcessingType)
                {
                    case HotkeyProcessingType.PositionSize:
                        {
                            Rect windowRectangle = WindowProcessing.GetPositionSizeOfWindowAfterProcessing(hwnd, windowInformation, hotkeyItemInformation.PositionSize, hotkeyItemInformation.StandardDisplay, hotkeyItemInformation.PositionSize.ClientArea);       // 処理後のウィンドウの位置とサイズ
                            if (ApplicationData.Settings.HotkeyInformation.DoNotChangeOutOfScreen == false || WindowProcessing.CheckWindowIsInTheScreen(windowRectangle))
                            {
                                // 1回目の処理
                                NativeMethods.SetWindowPos(hwnd, 0, (int)windowRectangle.X, (int)windowRectangle.Y, (int)windowRectangle.Width, (int)windowRectangle.Height, (int)SWP.SWP_NOZORDER);
                                switch (hotkeyItemInformation.PositionSize.SettingsWindowState)
                                {
                                    case SettingsWindowState.Normal:
                                        {
                                            // 2回目の処理
                                            // 1回の処理では一部の条件で正しい位置やサイズに変更されないので、2回処理するようにしている。
                                            windowInformation = WindowProcessing.GetWindowRectangleAndStateFromHandle(hwnd);
                                            windowRectangle = WindowProcessing.GetPositionSizeOfWindowAfterProcessing(hwnd, windowInformation, hotkeyItemInformation.PositionSize, hotkeyItemInformation.StandardDisplay, hotkeyItemInformation.PositionSize.ClientArea);
                                            if ((int)windowRectangle.X != windowInformation.Rectangle.Left
                                                || (int)windowRectangle.Y != windowInformation.Rectangle.Top
                                                || (int)windowRectangle.Width != windowInformation.Rectangle.Width
                                                || (int)windowRectangle.Height != windowInformation.Rectangle.Height)
                                            {
                                                NativeMethods.SetWindowPos(hwnd, 0, (int)windowRectangle.X, (int)windowRectangle.Y, (int)windowRectangle.Width, (int)windowRectangle.Height, (int)SWP.SWP_NOZORDER);
                                            }
                                        }
                                        break;
                                    case SettingsWindowState.Minimize:
                                        NativeMethods.ShowWindow(hwnd, (int)SW.SW_SHOWMINIMIZED);
                                        break;
                                    case SettingsWindowState.Maximize:
                                        NativeMethods.ShowWindow(hwnd, (int)SW.SW_SHOWMAXIMIZED);
                                        break;
                                }
                            }
                        }
                        break;
                    case HotkeyProcessingType.MoveX:
                        NativeMethods.SetWindowPos(hwnd, 0, windowInformation.Rectangle.Left + hotkeyItemInformation.ProcessingValue, windowInformation.Rectangle.Top, 0, 0, (int)SWP.SWP_NOSIZE | (int)SWP.SWP_NOZORDER);
                        break;
                    case HotkeyProcessingType.MoveY:
                        NativeMethods.SetWindowPos(hwnd, 0, windowInformation.Rectangle.Left, windowInformation.Rectangle.Top + hotkeyItemInformation.ProcessingValue, 0, 0, (int)SWP.SWP_NOSIZE | (int)SWP.SWP_NOZORDER);
                        break;
                    case HotkeyProcessingType.IncreaseDecreaseWidth:
                        NativeMethods.SetWindowPos(hwnd, 0, 0, 0, windowInformation.Rectangle.Width + hotkeyItemInformation.ProcessingValue, windowInformation.Rectangle.Height, (int)SWP.SWP_NOMOVE | (int)SWP.SWP_NOZORDER);
                        break;
                    case HotkeyProcessingType.IncreaseDecreaseHeight:
                        NativeMethods.SetWindowPos(hwnd, 0, 0, 0, windowInformation.Rectangle.Width, windowInformation.Rectangle.Height + hotkeyItemInformation.ProcessingValue, (int)SWP.SWP_NOMOVE | (int)SWP.SWP_NOZORDER);
                        break;
                    case HotkeyProcessingType.IncreaseDecreaseWidthHeight:
                        NativeMethods.SetWindowPos(hwnd, 0, 0, 0, windowInformation.Rectangle.Width + hotkeyItemInformation.ProcessingValue, windowInformation.Rectangle.Height + hotkeyItemInformation.ProcessingValue, (int)SWP.SWP_NOMOVE | (int)SWP.SWP_NOZORDER);
                        break;
                }
                break;
        }
    }
}
