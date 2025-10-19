using System.Threading.Tasks;

namespace Swindom;

/// <summary>
/// 「指定ウィンドウ」処理
/// </summary>
public class SpecifyWindowProcessing : IDisposable
{
    /// <summary>
    /// Disposed
    /// </summary>
    private bool Disposed;
    /// <summary>
    /// 修正中の項目のインデックス
    /// </summary>
    private static int IndexOfItemBeingProcessed = -1;

    /// <summary>
    /// マウスでウィンドウ移動検知
    /// </summary>
    private FreeEcho.FEWindowMoveDetectionMouse.MouseMoveWindowDetection? MouseMoveWindowDetection;
    /// <summary>
    /// ウィンドウイベント
    /// </summary>
    private FreeEcho.FEWindowEvent.WindowEvent? WindowEvent;
    /// <summary>
    /// 処理のタイマー
    /// </summary>
    private System.Windows.Threading.DispatcherTimer? ProcessingTimer;
    /// <summary>
    /// ウィンドウが移動中かの値 (移動中ではない「false」/移動中「true」)
    /// </summary>
    private bool MovingWindow;
    /// <summary>
    /// 追加/修正ウィンドウが表示されている場合は処理停止 (無効「false」/有効「true」)
    /// </summary>
    private bool PauseProcessingShowAddModifyWindow;
    /// <summary>
    /// ホットキーを処理するかの値 (いいえ「false」/はい「true」)
    /// </summary>
    private bool DoHotkeyProcessing = true;
    /// <summary>
    /// 「指定ウィンドウ」の「追加/修正」ウィンドウ
    /// </summary>
    private static SpecifyWindowItemWindow? SpecifyWindowItemWindow;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public SpecifyWindowProcessing()
    {
        ProcessingSettings();
        ComponentDispatcher.ThreadPreprocessMessage += ComponentDispatcher_ThreadPreprocessMessage;
        ApplicationData.EventData.ProcessingEvent += ApplicationData_ProcessingEvent;
    }

    /// <summary>
    /// デストラクタ
    /// </summary>
    ~SpecifyWindowProcessing()
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
            DisposeWindowEvent();
            DisposeProcessingTimer();
            DisposeMouseMoveWindowDetection();
            UnregisterHotkeys();
            SpecifyWindowItemWindow?.Close();
            SpecifyWindowItemWindow = null;
        }
        Disposed = true;
    }

    /// <summary>
    /// ウィンドウイベントを破棄
    /// </summary>
    private void DisposeWindowEvent()
    {
        if (WindowEvent != null)
        {
            WindowEvent.Dispose();
            WindowEvent = null;
        }
    }

    /// <summary>
    /// 処理のタイマーを破棄
    /// </summary>
    private void DisposeProcessingTimer()
    {
        if (ProcessingTimer != null)
        {
            ProcessingTimer?.Stop();
            ProcessingTimer = null;
        }
    }

    /// <summary>
    /// マウスでウィンドウ移動検知を破棄
    /// </summary>
    private void DisposeMouseMoveWindowDetection()
    {
        if (MouseMoveWindowDetection != null)
        {
            MouseMoveWindowDetection.Stop();
            MouseMoveWindowDetection = null;
        }
    }

    /// <summary>
    /// 処理が有効か確認
    /// </summary>
    /// <returns>処理が有効かの値 (無効「false」/有効「true」)</returns>
    public static bool CheckIfTheProcessingIsValid() => (ApplicationData.Settings.SpecifyWindowInformation.IsEnabled
            && ((ApplicationData.Settings.SpecifyWindowInformation.StopProcessingFullScreen && ApplicationData.FullScreenExists) ? ApplicationData.Settings.SpecifyWindowInformation.HotkeysDoNotStopFullScreen : true));

    /// <summary>
    /// 処理設定
    /// </summary>
    public void ProcessingSettings()
    {
        if (ApplicationData.Settings.SpecifyWindowInformation.IsEnabled
            && (ApplicationData.Settings.SpecifyWindowInformation.StopProcessingFullScreen == false || ApplicationData.FullScreenExists == false))
        {
            // タイマーを設定
            bool checkTimerProcessing = false;      // タイマー処理が有効かの値
            foreach (SpecifyWindowItemInformation nowItem in ApplicationData.Settings.SpecifyWindowInformation.Items)
            {
                if (nowItem.TimerProcessing)
                {
                    checkTimerProcessing = true;
                    break;
                }
            }
            if (checkTimerProcessing)
            {
                if (ProcessingTimer == null)
                {
                    ProcessingTimer = new()
                    {
                        Interval = new(0, 0, 0, 0, ApplicationData.Settings.SpecifyWindowInformation.ProcessingInterval)
                    };
                    ProcessingTimer.Tick += ProcessingTimer_Tick;
                }
                else
                {
                    ProcessingTimer.Interval = new(0, 0, 0, 0, ApplicationData.Settings.SpecifyWindowInformation.ProcessingInterval);
                }
                ProcessingTimer?.Start();
            }
            else
            {
                DisposeProcessingTimer();
            }

            // イベントを設定
            FreeEcho.FEWindowEvent.HookWindowEventType type = 0;        // イベントの種類 (イベントなし「0」)
            foreach (SpecifyWindowItemInformation nowItem in ApplicationData.Settings.SpecifyWindowInformation.Items)
            {
                if (nowItem.WindowEventData.Foreground)
                {
                    type |= FreeEcho.FEWindowEvent.HookWindowEventType.Foreground;
                }
                if (nowItem.WindowEventData.MoveSizeEnd)
                {
                    type |= FreeEcho.FEWindowEvent.HookWindowEventType.MoveSizeEnd;
                }
                if (nowItem.WindowEventData.MinimizeStart)
                {
                    type |= FreeEcho.FEWindowEvent.HookWindowEventType.MinimizeStart;
                }
                if (nowItem.WindowEventData.MinimizeEnd)
                {
                    type |= FreeEcho.FEWindowEvent.HookWindowEventType.MinimizeEnd;
                }
                if (nowItem.WindowEventData.Show)
                {
                    type |= FreeEcho.FEWindowEvent.HookWindowEventType.Show;
                }
                if (nowItem.WindowEventData.NameChange)
                {
                    type |= FreeEcho.FEWindowEvent.HookWindowEventType.NameChange;
                }
            }
            // 「1度だけ処理が終わっている」が有効の場合、「Destroy」イベントを受け取れるようにする
            if (type != 0 || checkTimerProcessing)
            {
                foreach (SpecifyWindowItemInformation nowItem in ApplicationData.Settings.SpecifyWindowInformation.Items)
                {
                    if (nowItem.ProcessingOnlyOnce == ProcessingOnlyOnce.WindowOpen)
                    {
                        type |= FreeEcho.FEWindowEvent.HookWindowEventType.Destroy;
                        break;
                    }
                }
            }
            if (type == 0)
            {
                DisposeWindowEvent();
            }
            else
            {
                if (WindowEvent == null)
                {
                    WindowEvent = new();
                    WindowEvent.WindowEventOccurrence += WindowEvent_WindowEventOccurrence;
                }
                else
                {
                    WindowEvent.Unhook();
                }
                WindowEvent.Hook(type);
            }

            if (type != 0 || checkTimerProcessing)
            {
                if (MouseMoveWindowDetection == null)
                {
                    MouseMoveWindowDetection = new();
                    MouseMoveWindowDetection.StartMove += MouseMoveWindowDetection_StartMove;
                    MouseMoveWindowDetection.StopMove += MouseMoveWindowDetection_StopMove;
                }
                MouseMoveWindowDetection.Start();
            }
            else
            {
                DisposeMouseMoveWindowDetection();
            }
        }
        else
        {
            DisposeWindowEvent();
            DisposeProcessingTimer();
            DisposeMouseMoveWindowDetection();
        }
        RegisterHotkeys();
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
                HwndList hwndList = HwndList.GetWindowHandleList();        // ウィンドウハンドルのリスト

                if (ApplicationData.Settings.SpecifyWindowInformation.StopProcessingFullScreen
                    && ApplicationData.Settings.SpecifyWindowInformation.HotkeysDoNotStopFullScreen == false
                    && CheckFullScreenProcessing.CheckExistsFullScreenWindow(hwndList))
                {
                    return;
                }

                int id = (int)msg.wParam;       // ホットキーの識別子

                foreach (IntPtr nowHwnd in hwndList.Hwnd)
                {
                    WindowInformation windowInformation = WindowProcessing.GetWindowInformationFromHandle(nowHwnd);

                    foreach (SpecifyWindowItemInformation nowEII in ApplicationData.Settings.SpecifyWindowInformation.Items)
                    {
                        if (nowEII.IsEnabled
                            && CheckWindowToProcessing(nowEII, windowInformation)
                            && ExclusionDecision(nowEII, windowInformation, nowHwnd)
                            && NotificationCheck(nowEII, windowInformation))
                        {
                            foreach (WindowProcessingInformation nowWPI in nowEII.WindowProcessingInformation)
                            {
                                if (nowWPI.HotkeyId == id
                                    && (nowEII.StandardDisplay != StandardDisplay.ExclusiveSpecifiedDisplay || CheckDisplayToProcessing(nowEII, nowWPI, windowInformation)))
                                {
                                    if (nowWPI.CloseWindow)
                                    {
                                        NativeMethods.PostMessage(nowHwnd, (uint)WM.WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
                                        break;
                                    }
                                    else
                                    {
                                        ProcessingWindow(nowEII, nowWPI, nowHwnd, windowInformation);
                                        break;
                                    }
                                }
                            }
                        }
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
                case ProcessingEventType.SpecifyWindowShowWindowPause:
                    PauseProcessingShowAddModifyWindow = true;
                    break;
                case ProcessingEventType.SpecifyWindowShowWindowUnpause:
                    PauseProcessingShowAddModifyWindow = false;
                    break;
                case ProcessingEventType.SpecifyWindowChangeTimerProcessingInterval:
                    if (ProcessingTimer != null)
                    {
                        ProcessingTimer.Interval = new(0, 0, 0, 0, ApplicationData.Settings.SpecifyWindowInformation.ProcessingInterval);
                    }
                    break;
                case ProcessingEventType.PauseHotkeyProcessing:
                    DoHotkeyProcessing = false;
                    break;
                case ProcessingEventType.UnpauseHotkeyProcessing:
                    DoHotkeyProcessing = true;
                    break;
                case ProcessingEventType.SpecifyWindowCloseItemWindow:
                    if (IndexOfItemBeingProcessed != -1)
                    {
                        ApplicationData.Settings.SpecifyWindowInformation.Items[IndexOfItemBeingProcessed].IsEnabled = true;
                        IndexOfItemBeingProcessed = -1;
                        ApplicationData.EventData.DoProcessingEvent(ProcessingEventType.SpecifyWindowUpdateListBox);
                    }
                    break;
            }
        }
        catch
        {
        }
    }

    /// <summary>
    /// ディスプレイの設定を変更
    /// </summary>
    /// <param name="changeDisplaySettingsData">設定しているディスプレイが存在しなくなった場合に設定するデータ</param>
    /// <returns>設定を変更したかの値 (「false」変更していない/「true」変更した)</returns>
    public static bool ChangeDisplaySettings(
        ChangeDisplaySettingsData changeDisplaySettingsData
        )
    {
        bool result = false;        // 設定を変更したかの値

        try
        {
            foreach (SpecifyWindowItemInformation nowSWII in ApplicationData.Settings.SpecifyWindowInformation.Items)
            {
                foreach (WindowProcessingInformation nowWPI in nowSWII.WindowProcessingInformation)
                {
                    bool checkMatch = false;        // 一致確認

                    foreach (MonitorInfoEx nowMIE in ApplicationData.MonitorInformation.MonitorInfo)
                    {
                        if (nowMIE.DeviceName == nowWPI.PositionSize.Display)
                        {
                            checkMatch = true;
                            break;
                        }
                    }

                    if (checkMatch == false)
                    {
                        if (changeDisplaySettingsData.ApplySameSettingToRemaining)
                        {
                            if (changeDisplaySettingsData.IsModified)
                            {
                                nowWPI.PositionSize.Display = changeDisplaySettingsData.AutoSettingDisplayName;
                                result = true;
                            }
                        }
                        else
                        {
                            SelectDisplayWindow window = new(ApplicationData.Strings.SpecifyWindow, nowSWII.RegisteredName + WindowControlValue.CopySeparateString + nowWPI.ProcessingName);

                            window.ShowDialog();
                            changeDisplaySettingsData.ApplySameSettingToRemaining = window.ApplySameSettingToRemaining;
                            if (window.ApplySameSettingToRemaining)
                            {
                                changeDisplaySettingsData.AutoSettingDisplayName = window.SelectedDisplay;
                            }
                            if (window.IsModified)
                            {
                                changeDisplaySettingsData.IsModified = true;
                                nowWPI.PositionSize.Display = window.SelectedDisplay;
                                result = true;
                            }
                        }
                    }
                }
            }
        }
        catch
        {
        }

        return result;
    }

    /// <summary>
    /// 登録された全項目で、設定されているディスプレイが存在するかを確認
    /// </summary>
    /// <param name="newMonitorInformation">新しいモニター情報</param>
    /// <returns>全て存在するかの値 (「false」存在しない項目がある/「true」全て存在する)</returns>
    public static bool CheckSettingDisplaysExist(
        MonitorInformation newMonitorInformation
        )
    {
        try
        {
            foreach (SpecifyWindowItemInformation nowSWII in ApplicationData.Settings.SpecifyWindowInformation.Items)
            {
                foreach (WindowProcessingInformation nowWPI in nowSWII.WindowProcessingInformation)
                {
                    bool checkMatch = false;        // 一致確認

                    foreach (MonitorInfoEx nowMIE in newMonitorInformation.MonitorInfo)
                    {
                        if (nowMIE.DeviceName == nowWPI.PositionSize.Display)
                        {
                            checkMatch = true;
                            break;
                        }
                    }

                    if (checkMatch == false)
                    {
                        return false;
                    }
                }
            }
        }
        catch
        {
        }

        return true;
    }

    /// <summary>
    /// 「ウィンドウイベント」の「WindowEventOccurrence」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void WindowEvent_WindowEventOccurrence(
        object? sender,
        FreeEcho.FEWindowEvent.WindowEventArgs e
        )
    {
        try
        {
            // EVENT_OBJECT_DESTROYのイベント
            if (e.EventType == FreeEcho.FEWindowEvent.EVENT_CONSTANTS.EVENT_OBJECT_DESTROY)
            {
                // ウィンドウが閉じている場合は「1度だけ処理が終わっている」を解除
                foreach (SpecifyWindowItemInformation nowEII in ApplicationData.Settings.SpecifyWindowInformation.Items)
                {
                    if (nowEII.Hwnd == e.Hwnd && nowEII.ProcessingOnlyOnce == ProcessingOnlyOnce.WindowOpen)
                    {
                        nowEII.EndedProcessingOnlyOnce = false;
                        nowEII.CountNumberOfTimesNotToProcessingFirst = 0;
                        nowEII.Hwnd = IntPtr.Zero;
                        break;
                    }
                }
                return;
            }
            // EVENT_OBJECT_DESTROY以外のイベント
            else
            {
                DecisionAndWindowProcessing(e);
            }
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「処理のタイマー」の「Tick」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    /// <exception cref="NotImplementedException"></exception>
    private void ProcessingTimer_Tick(
        object? sender,
        EventArgs e
        )
    {
        try
        {
            DecisionAndWindowProcessing();
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「マウスでウィンドウ移動検知」の「StartMove」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void MouseMoveWindowDetection_StartMove(
        object sender,
        FreeEcho.FEWindowMoveDetectionMouse.StartMoveEventArgs e
        )
    {
        try
        {
            MovingWindow = true;
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「マウスでウィンドウ移動検知」の「StopMove」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void MouseMoveWindowDetection_StopMove(
        object sender,
        FreeEcho.FEWindowMoveDetectionMouse.StopMoveEventArgs e
        )
    {
        try
        {
            MovingWindow = false;
        }
        catch
        {
        }
    }

    /// <summary>
    /// ウィンドウが表示されているか確認
    /// ＊動作確認用
    /// </summary>
    /// <param name="hwnd">ウィンドウハンドル</param>
    /// <param name="eventType">イベント定数</param>
    /// <returns>ウィンドウが表示されているかの値 (表示されていない「false」/表示されている「true」)</returns>
    public static bool ConfirmWindowVisible(
        IntPtr hwnd,
        uint eventType
        )
    {
        switch (eventType)
        {
            case EVENT_CONSTANTS.EVENT_OBJECT_CREATE:
            case EVENT_CONSTANTS.EVENT_OBJECT_DESTROY:
                break;
            case EVENT_CONSTANTS.EVENT_OBJECT_SHOW:
                if (NativeMethods.IsWindowVisible(hwnd) == false
                    && NativeMethods.IsWindow(hwnd) == false)
                {
                    return false;
                }
                if ((NativeMethods.GetWindowLongPtr(hwnd, (int)GWL.GWL_EXSTYLE) & (int)WS_EX.WS_EX_TOOLWINDOW) == (int)WS_EX.WS_EX_TOOLWINDOW)
                {
                    return false;
                }
                break;
            case EVENT_CONSTANTS.EVENT_SYSTEM_MINIMIZESTART:
                {
                    if (NativeMethods.IsWindowVisible(hwnd) == false
                        && NativeMethods.IsWindow(hwnd) == false
                        && NativeMethods.IsWindowEnabled(hwnd) == false)
                    {
                        return false;
                    }

                    long exStyle = NativeMethods.GetWindowLongPtr(hwnd, (int)GWL.GWL_EXSTYLE);
                    long style = NativeMethods.GetWindowLongPtr(hwnd, (int)GWL.GWL_STYLE);
                    if ((exStyle & (long)WS_EX.WS_EX_TOOLWINDOW) == (long)WS_EX.WS_EX_TOOLWINDOW
                        || (style & (long)WS.WS_VISIBLE) != (long)WS.WS_VISIBLE)
                    {
                        return false;
                    }
                    IntPtr tempHwnd = NativeMethods.GetAncestor(hwnd, GetAncestorFlags.GetRoot);
                    if (tempHwnd != hwnd)
                    {
                        return false;
                    }

                    //if (NativeMethods.GetClassLongPtr(hwnd, -26) != 0)        // GCL_STYLE = -26
                    //{
                    //    return false;
                    //}

                    //if ((NativeMethods.GetWindowLongPtr(hwnd, (int)GWL.GWL_EXSTYLE) & (int)WS_EX.WS_EX_TOOLWINDOW) == (int)WS_EX.WS_EX_TOOLWINDOW)
                    //{
                    //    return false;
                    //}

                    // ウィンドウのないUWPアプリかを判定
                    bool isInvisibleUwpApp;
                    _ = NativeMethods.DwmGetWindowAttribute(hwnd, (uint)DWMWINDOWATTRIBUTE.Cloaked, out isInvisibleUwpApp, System.Runtime.InteropServices.Marshal.SizeOf(typeof(bool)));
                    if (isInvisibleUwpApp)
                    {
                        return false;
                    }
                }
                break;
            default:
                {
                    if (NativeMethods.IsWindowVisible(hwnd) == false
                        && NativeMethods.IsWindow(hwnd) == false)
                    {
                        return false;
                    }

                    if ((NativeMethods.GetWindowLongPtr(hwnd, (int)GWL.GWL_EXSTYLE) & (long)WS_EX.WS_EX_TOOLWINDOW) == (long)WS_EX.WS_EX_TOOLWINDOW)
                    {
                        return false;
                    }
                    IntPtr ancestorHwnd = NativeMethods.GetAncestor(hwnd, GetAncestorFlags.GetRoot);
                    if (ancestorHwnd != hwnd)
                    {
                        return false;
                    }

                    //if ((NativeMethods.GetWindowLongPtr(hwnd, (int)GWL.GWL_EXSTYLE) & (int)WS_EX.WS_EX_TOOLWINDOW) == (int)WS_EX.WS_EX_TOOLWINDOW)
                    //{
                    //    return false;
                    //}

                    // ウィンドウのないUWPアプリかを判定
                    bool isInvisibleUwpApp;
                    _ = NativeMethods.DwmGetWindowAttribute(hwnd, (uint)DWMWINDOWATTRIBUTE.Cloaked, out isInvisibleUwpApp, System.Runtime.InteropServices.Marshal.SizeOf(typeof(bool)));
                    if (isInvisibleUwpApp)
                    {
                        return false;
                    }
                }
                break;
        }

        return true;
    }

    /// <summary>
    /// 判定してウィンドウ処理 (イベント)
    /// </summary>
    /// <param name="e">WindowEventArgs</param>
    private async void DecisionAndWindowProcessing(
        FreeEcho.FEWindowEvent.WindowEventArgs e
        )
    {
        try
        {
            if (MovingWindow || PauseProcessingShowAddModifyWindow)
            {
                return;
            }

            IntPtr hwnd = e.Hwnd;        // ウィンドウハンドル

            if (FreeEcho.FEWindowEvent.WindowEvent.ConfirmWindowVisible(hwnd, e.EventType) == false)
            {
                return;
            }
            //if (ConfirmWindowVisible(hwnd, e.EventType) == false)
            //{
            //    return;
            //}
            hwnd = FreeEcho.FEWindowEvent.WindowEvent.GetAncestorHwnd(e.Hwnd, e.EventType);

            WindowInformation windowInformation = WindowProcessing.GetWindowInformationFromHandle(hwnd);      // ウィンドウの情報

            foreach (SpecifyWindowItemInformation nowEII in ApplicationData.Settings.SpecifyWindowInformation.Items)
            {
                if (nowEII.IsEnabled
                    && (nowEII.ProcessingOnlyOnce == ProcessingOnlyOnce.NotSpecified || nowEII.EndedProcessingOnlyOnce == false))
                {
                    if (CheckWindowToProcessing(nowEII, windowInformation)
                        && NotificationCheck(nowEII, windowInformation))
                    {
                        // 処理待ち
                        if (e.EventType == FreeEcho.FEWindowEvent.EVENT_CONSTANTS.EVENT_OBJECT_SHOW
                            && 0 < nowEII.WindowEventData.DelayTime)
                        {
                            await Task.Delay(nowEII.WindowEventData.DelayTime);
                            WindowInformation tempWindowInformation = WindowProcessing.GetWindowRectangleAndStateFromHandle(hwnd);      // ウィンドウの情報
                            windowInformation.Rectangle = tempWindowInformation.Rectangle;
                            windowInformation.ShowCmd = tempWindowInformation.ShowCmd;
                        }

                        if (ExclusionDecision(nowEII, windowInformation, hwnd))
                        {
                            bool checkEvent = e.EventType switch
                            {
                                FreeEcho.FEWindowEvent.EVENT_CONSTANTS.EVENT_SYSTEM_FOREGROUND => nowEII.WindowEventData.Foreground,
                                FreeEcho.FEWindowEvent.EVENT_CONSTANTS.EVENT_SYSTEM_MOVESIZEEND => nowEII.WindowEventData.MoveSizeEnd,
                                FreeEcho.FEWindowEvent.EVENT_CONSTANTS.EVENT_SYSTEM_MINIMIZESTART => nowEII.WindowEventData.MinimizeStart,
                                FreeEcho.FEWindowEvent.EVENT_CONSTANTS.EVENT_SYSTEM_MINIMIZEEND => nowEII.WindowEventData.MinimizeEnd,
                                FreeEcho.FEWindowEvent.EVENT_CONSTANTS.EVENT_OBJECT_SHOW => nowEII.WindowEventData.Show,
                                FreeEcho.FEWindowEvent.EVENT_CONSTANTS.EVENT_OBJECT_NAMECHANGE => nowEII.WindowEventData.NameChange,
                                _ => false
                            };      // イベントの確認

                            if (checkEvent)
                            {
                                foreach (WindowProcessingInformation nowWPI in nowEII.WindowProcessingInformation)
                                {
                                    if (nowWPI.Active
                                        && CheckDisplayToProcessing(nowEII, nowWPI, windowInformation))
                                    {
                                        if (ApplicationData.Settings.SpecifyWindowInformation.StopProcessingFullScreen && CheckFullScreenProcessing.CheckFullScreenWindow(hwnd))
                                        {
                                            return;
                                        }

                                        if (nowWPI.CloseWindow)
                                        {
                                            if (nowEII.ProcessingOnlyOnce != ProcessingOnlyOnce.NotSpecified)
                                            {
                                                nowEII.EndedProcessingOnlyOnce = true;
                                            }
                                            nowEII.Hwnd = hwnd;
                                            NativeMethods.PostMessage(hwnd, (uint)WM.WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
                                            break;
                                        }
                                        else if (nowWPI.NormalWindowOnly == false || windowInformation.ShowCmd == (int)SW.SW_SHOWNORMAL)
                                        {
                                            if (nowEII.ProcessingOnlyOnce != ProcessingOnlyOnce.NotSpecified)
                                            {
                                                nowEII.EndedProcessingOnlyOnce = true;
                                            }
                                            nowEII.Hwnd = hwnd;
                                            ProcessingWindow(nowEII, nowWPI, hwnd, windowInformation);
                                            break;
                                        }
                                    }
                                }
                            }
                        }
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
    /// 判定してウィンドウ処理 (タイマー)
    /// </summary>
    private async void DecisionAndWindowProcessing()
    {
        try
        {
            if (MovingWindow || PauseProcessingShowAddModifyWindow)
            {
                return;
            }

            ProcessingTimer?.Stop();

            HwndList hwndList;        // ウィンドウハンドルのリスト
            switch (ApplicationData.Settings.SpecifyWindowInformation.ProcessingWindowRange)
            {
                case ProcessingWindowRange.ActiveOnly:
                    hwndList = new();
                    hwndList.Hwnd.Add(NativeMethods.GetForegroundWindow());
                    break;
                default:
                    hwndList = HwndList.GetWindowHandleList();
                    break;
            }

            int waitTime = 0;     // 待ち時間
            foreach (IntPtr nowHwnd in hwndList.Hwnd)
            {
                WindowInformation windowInformation = WindowProcessing.GetWindowInformationFromHandle(nowHwnd);

                foreach (SpecifyWindowItemInformation nowEII in ApplicationData.Settings.SpecifyWindowInformation.Items)
                {
                    if (nowEII.IsEnabled
                        && nowEII.TimerProcessing
                        && (nowEII.ProcessingOnlyOnce == ProcessingOnlyOnce.NotSpecified || nowEII.EndedProcessingOnlyOnce == false))
                    {
                        if (CheckWindowToProcessing(nowEII, windowInformation)
                            && ExclusionDecision(nowEII, windowInformation, nowHwnd)
                            && NotificationCheck(nowEII, windowInformation))
                        {
                            if (nowEII.CountNumberOfTimesNotToProcessingFirst < nowEII.NumberOfTimesNotToProcessingFirst)
                            {
                                if (nowEII.Hwnd == IntPtr.Zero)
                                {
                                    nowEII.Hwnd = nowHwnd;
                                }
                                if (nowEII.Hwnd == nowHwnd)
                                {
                                    nowEII.CountNumberOfTimesNotToProcessingFirst++;
                                }
                            }
                            else
                            {
                                foreach (WindowProcessingInformation nowWPI in nowEII.WindowProcessingInformation)
                                {
                                    if (nowWPI.Active
                                        && CheckDisplayToProcessing(nowEII, nowWPI, windowInformation))
                                    {
                                        if (ApplicationData.Settings.SpecifyWindowInformation.StopProcessingFullScreen && CheckFullScreenProcessing.CheckFullScreenWindow(nowHwnd))
                                        {
                                            return;
                                        }

                                        if (waitTime == 0)
                                        {
                                            waitTime = ApplicationData.Settings.SpecifyWindowInformation.WaitTimeToProcessingNextWindow;
                                        }
                                        else
                                        {
                                            await Task.Delay(ApplicationData.Settings.SpecifyWindowInformation.WaitTimeToProcessingNextWindow);
                                        }
                                        if (nowWPI.CloseWindow)
                                        {
                                            if (nowEII.ProcessingOnlyOnce != ProcessingOnlyOnce.NotSpecified)
                                            {
                                                nowEII.EndedProcessingOnlyOnce = true;
                                            }
                                            nowEII.Hwnd = nowHwnd;
                                            NativeMethods.PostMessage(nowHwnd, (uint)WM.WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
                                            break;
                                        }
                                        else if (nowWPI.NormalWindowOnly == false || windowInformation.ShowCmd == (int)SW.SW_SHOWNORMAL)
                                        {
                                            if (nowEII.ProcessingOnlyOnce != ProcessingOnlyOnce.NotSpecified)
                                            {
                                                nowEII.EndedProcessingOnlyOnce = true;
                                            }
                                            nowEII.Hwnd = nowHwnd;
                                            ProcessingWindow(nowEII, nowWPI, nowHwnd, windowInformation);
                                            break;
                                        }
                                    }
                                }
                            }
                            break;
                        }
                    }
                }
            }
        }
        finally
        {
            try
            {
                ProcessingTimer?.Start();
            }
            catch
            {
            }
        }
    }

    /// <summary>
    /// 判定してウィンドウ処理 (一括実行、アクティブウィンドウのみ)
    /// </summary>
    /// <param name="processingType">処理の種類 (一括実行「false」/アクティブウィンドウのみ「true」)</param>
    public static void DecisionAndWindowProcessing(
        bool processingType
        )
    {
        try
        {
            HwndList hwndList;        // ウィンドウハンドルのリスト
            bool partHwndList = true;       // ウィンドウハンドルのリストが一部かの値
            if (processingType)
            {
                hwndList = new();
                hwndList.Hwnd.Add(NativeMethods.GetForegroundWindow());
            }
            else
            {
                hwndList = HwndList.GetWindowHandleList();
                partHwndList = false;
            }

            if (ApplicationData.Settings.SpecifyWindowInformation.StopProcessingFullScreen && CheckFullScreenProcessing.CheckExistsFullScreenWindow(partHwndList ? null : hwndList))
            {
                return;
            }

            foreach (IntPtr nowHwnd in hwndList.Hwnd)
            {
                WindowInformation windowInformation = WindowProcessing.GetWindowInformationFromHandle(nowHwnd);

                foreach (SpecifyWindowItemInformation nowEII in ApplicationData.Settings.SpecifyWindowInformation.Items)
                {
                    if (nowEII.IsEnabled
                        && CheckWindowToProcessing(nowEII, windowInformation)
                        && ExclusionDecision(nowEII, windowInformation, nowHwnd)
                        && NotificationCheck(nowEII, windowInformation))
                    {
                        foreach (WindowProcessingInformation nowWPI in nowEII.WindowProcessingInformation)
                        {
                            if (nowWPI.Active
                                && (nowEII.StandardDisplay != StandardDisplay.ExclusiveSpecifiedDisplay || CheckDisplayToProcessing(nowEII, nowWPI, windowInformation)))
                            {
                                if (nowWPI.CloseWindow)
                                {
                                    NativeMethods.PostMessage(nowHwnd, (uint)WM.WM_CLOSE, IntPtr.Zero, IntPtr.Zero);
                                    break;
                                }
                                else if (nowWPI.NormalWindowOnly == false || windowInformation.ShowCmd == (int)SW.SW_SHOWNORMAL)
                                {
                                    ProcessingWindow(nowEII, nowWPI, nowHwnd, windowInformation);
                                    break;
                                }
                            }
                        }
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
    /// ホットキーを登録
    /// </summary>
    public void RegisterHotkeys()
    {
        UnregisterHotkeys();
        if (CheckIfTheProcessingIsValid())
        {
            foreach (SpecifyWindowItemInformation nowEII in ApplicationData.Settings.SpecifyWindowInformation.Items)
            {
                foreach (WindowProcessingInformation nowWPI in nowEII.WindowProcessingInformation)
                {
                    try
                    {
                        nowWPI.HotkeyId = HotkeyControlProcessing.RegisterHotkey(nowWPI.Hotkey);
                    }
                    catch
                    {
                    }
                }
            }
        }
    }

    /// <summary>
    /// ホットキーを解除
    /// </summary>
    public void UnregisterHotkeys()
    {
        foreach (SpecifyWindowItemInformation nowEII in ApplicationData.Settings.SpecifyWindowInformation.Items)
        {
            foreach (WindowProcessingInformation nowWPI in nowEII.WindowProcessingInformation)
            {
                if (nowWPI.HotkeyId != -1)
                {
                    try
                    {
                        HotkeyControlProcessing.UnregisterHotkey(nowWPI.HotkeyId);
                        nowWPI.HotkeyId = -1;
                    }
                    catch
                    {
                    }
                }
            }
        }
    }

    /// <summary>
    /// 処理するウィンドウか確認
    /// </summary>
    /// <param name="specifyWindowItemInformation">「指定ウィンドウ」機能の項目情報</param>
    /// <param name="windowInformation">ウィンドウの情報</param>
    /// <returns>処理するウィンドウかの値 (いいえ「false」/はい「true」)</returns>
    public static bool CheckWindowToProcessing(
        SpecifyWindowItemInformation specifyWindowItemInformation,
        WindowInformation windowInformation
        )
    {
        try
        {
            // 除外するウィンドウの判定
            if (WindowProcessing.CheckExclusionProcessing(windowInformation))
            {
                return false;
            }

            string checkString;      // 確認する文字列
            string windowString;       // ウィンドウ文字列

            // タイトル名判定
            if (string.IsNullOrEmpty(windowInformation.TitleName) == false)
            {
                if (ApplicationData.Settings.SpecifyWindowInformation.CaseSensitive)
                {
                    checkString = specifyWindowItemInformation.TitleName;
                    windowString = windowInformation.TitleName;
                }
                else
                {
                    checkString = specifyWindowItemInformation.TitleName.ToLower();
                    windowString = windowInformation.TitleName.ToLower();
                }

                if (string.IsNullOrEmpty(checkString) == false)
                {
                    switch (specifyWindowItemInformation.TitleNameMatchCondition)
                    {
                        case NameMatchCondition.PartialMatch:
                            if (windowString.Contains(checkString) == false)
                            {
                                return false;
                            }
                            break;
                        case NameMatchCondition.ForwardMatch:
                            if (windowString.Length < checkString.Length || windowString.StartsWith(checkString) == false)
                            {
                                return false;
                            }
                            break;
                        case NameMatchCondition.BackwardMatch:
                            if (windowString.Length < checkString.Length || windowString.EndsWith(checkString) == false)
                            {
                                return false;
                            }
                            break;
                        default:
                            if (windowString != checkString)
                            {
                                return false;
                            }
                            break;
                    }
                }
            }
            else if (string.IsNullOrEmpty(specifyWindowItemInformation.TitleName) == false)
            {
                return false;
            }

            // クラス名判定
            if (string.IsNullOrEmpty(specifyWindowItemInformation.ClassName) == false)
            {
                // 取得に失敗した場合は処理しない
                if (string.IsNullOrEmpty(windowInformation.ClassName))
                {
                    return false;
                }

                if (ApplicationData.Settings.SpecifyWindowInformation.CaseSensitive)
                {
                    checkString = specifyWindowItemInformation.ClassName;
                    windowString = windowInformation.ClassName;
                }
                else
                {
                    checkString = specifyWindowItemInformation.ClassName.ToLower();
                    windowString = windowInformation.ClassName.ToLower();
                }

                switch (specifyWindowItemInformation.ClassNameMatchCondition)
                {
                    case NameMatchCondition.PartialMatch:
                        if (windowString.Contains(checkString) == false)
                        {
                            return false;
                        }
                        break;
                    case NameMatchCondition.ForwardMatch:
                        if (windowString.Length < checkString.Length || windowString.StartsWith(checkString) == false)
                        {
                            return false;
                        }
                        break;
                    case NameMatchCondition.BackwardMatch:
                        if (windowString.Length < checkString.Length || windowString.EndsWith(checkString) == false)
                        {
                            return false;
                        }
                        break;
                    default:
                        if (checkString != windowString)
                        {
                            return false;
                        }
                        break;
                }
            }

            // ファイル名判定
            if (string.IsNullOrEmpty(specifyWindowItemInformation.FileName) == false)
            {
                // 取得に失敗した場合は処理しない
                if (string.IsNullOrEmpty(windowInformation.FileName))
                {
                    return false;
                }

                if (ApplicationData.Settings.SpecifyWindowInformation.CaseSensitive)
                {
                    checkString = specifyWindowItemInformation.FileName;
                    windowString = windowInformation.FileName;
                }
                else
                {
                    checkString = specifyWindowItemInformation.FileName.ToLower();
                    windowString = windowInformation.FileName.ToLower();
                }

                switch (specifyWindowItemInformation.FileNameMatchCondition)
                {
                    case FileNameMatchCondition.NotInclude:
                        if (checkString != Path.GetFileNameWithoutExtension(windowString)
                            && checkString != Path.GetFileName(windowString))
                        {
                            return false;
                        }
                        break;
                    default:
                        if (checkString != windowString)
                        {
                            return false;
                        }
                        break;
                }
            }
        }
        catch
        {
            return false;
        }

        return true;
    }

    /// <summary>
    /// 処理しない条件の判定
    /// </summary>
    /// <param name="specifyWindowItemInformation">「指定ウィンドウ」機能の項目情報</param>
    /// <param name="windowInformation">ウィンドウの情報</param>
    /// <param name="hwnd">ウィンドウハンドル</param>
    /// <returns>処理するかの値 (いいえ「false」/はい「true」)</returns>
    private static bool ExclusionDecision(
        SpecifyWindowItemInformation specifyWindowItemInformation,
        WindowInformation windowInformation,
        IntPtr hwnd
        )
    {
        // 子ウィンドウ
        if (specifyWindowItemInformation.DoNotProcessingChildWindow ? NativeMethods.GetWindow(hwnd, GetWindowType.GW_OWNER) != 0 : false)
        {
            return false;
        }

        // タイトル名の条件
        switch (specifyWindowItemInformation.DoNotProcessingTitleNameConditions)
        {
            case TitleNameProcessingConditions.NotIncluded:
                if (string.IsNullOrEmpty(windowInformation.TitleName))
                {
                    return false;
                }
                break;
            case TitleNameProcessingConditions.Included:
                if (string.IsNullOrEmpty(windowInformation.TitleName) == false)
                {
                    return false;
                }
                break;
        }

        // タイトル名に含まれる文字列
        string windowString = ApplicationData.Settings.SpecifyWindowInformation.CaseSensitive ? windowInformation.TitleName : windowInformation.TitleName.ToLower();       // ウィンドウ文字列
        foreach (string nowTitleName in specifyWindowItemInformation.DoNotProcessingStringContainedInTitleName)
        {
            if (windowString.Contains(ApplicationData.Settings.SpecifyWindowInformation.CaseSensitive ? nowTitleName : nowTitleName.ToLower()))
            {
                return false;
            }
        }

        // サイズ
        if (specifyWindowItemInformation.DoNotProcessingSize.Count != 0)
        {
            foreach (SizeInt nowSize in specifyWindowItemInformation.DoNotProcessingSize)
            {
                if (windowInformation.Rectangle.Width == nowSize.Width && windowInformation.Rectangle.Height == nowSize.Height)
                {
                    return false;
                }
            }
        }

        // 指定したサイズ以外
        if (specifyWindowItemInformation.DoNotProcessingOtherThanSpecifiedSize.Count != 0)
        {
            bool checkProcess = false;      // 処理するかの値
            foreach (SizeInt nowSize in specifyWindowItemInformation.DoNotProcessingOtherThanSpecifiedSize)
            {
                if (windowInformation.Rectangle.Width == nowSize.Width && windowInformation.Rectangle.Height == nowSize.Height)
                {
                    checkProcess = true;
                    break;
                }
            }
            if (checkProcess == false)
            {
                return false;
            }
        }

        // 指定バージョン以外
        if (string.IsNullOrEmpty(specifyWindowItemInformation.DoNotProcessingOtherThanSpecifiedVersion) == false)
        {
            VersionNumber targetWindowVersionNumber = VersionNumber.SplitVersionNumber(windowInformation.Version);
            VersionNumber checkVersionNumber = VersionNumber.SplitVersionNumber(specifyWindowItemInformation.DoNotProcessingOtherThanSpecifiedVersion);
            if (VersionNumber.VersionComparison(targetWindowVersionNumber, checkVersionNumber) == false)
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// 通知の確認と表示
    /// </summary>
    /// <param name="specifyWindowItemInformation">「指定ウィンドウ」機能の項目情報</param>
    /// <param name="windowInformation">ウィンドウの情報</param>
    /// <returns>処理を続けるかの値 (続けない「false」/続ける「true」)</returns>
    private static bool NotificationCheck(
        SpecifyWindowItemInformation specifyWindowItemInformation,
        WindowInformation windowInformation
        )
    {
        bool result = true;     // 結果

        if (specifyWindowItemInformation.Notification)
        {
            if (string.IsNullOrEmpty(specifyWindowItemInformation.NotificationOtherThanSpecifiedVersion) == false)
            {
                VersionNumber targetWindowVersionNumber = VersionNumber.SplitVersionNumber(windowInformation.Version);
                VersionNumber checkVersionNumber = VersionNumber.SplitVersionNumber(specifyWindowItemInformation.NotificationOtherThanSpecifiedVersion);
                if (VersionNumber.VersionComparison(targetWindowVersionNumber, checkVersionNumber) == false)
                {
                    string message = "";        // メッセージ

                    message += specifyWindowItemInformation.RegisteredName + Environment.NewLine;
                    if (specifyWindowItemInformation.WindowProcessingInformation.Count == 1)
                    {
                        WindowProcessingInformation information = specifyWindowItemInformation.WindowProcessingInformation[0];
                        message += ApplicationData.Strings.Width + WindowControlValue.TypeAndValueSeparateString + information.PositionSize.Width + WindowControlValue.ValueAndValueSeparateString + ApplicationData.Strings.Height + WindowControlValue.TypeAndValueSeparateString + information.PositionSize.Height + Environment.NewLine;
                    }
                    message += ApplicationData.Strings.Version + WindowControlValue.TypeAndValueSeparateString + specifyWindowItemInformation.NotificationOtherThanSpecifiedVersion + Environment.NewLine;
                    message += Environment.NewLine;
                    message += ApplicationData.Strings.WindowInformation + Environment.NewLine;
                    message += ApplicationData.Strings.Width + WindowControlValue.TypeAndValueSeparateString + windowInformation.Rectangle.Width + WindowControlValue.ValueAndValueSeparateString + ApplicationData.Strings.Height + WindowControlValue.TypeAndValueSeparateString + windowInformation.Rectangle.Height + Environment.NewLine;
                    message += ApplicationData.Strings.Version + WindowControlValue.TypeAndValueSeparateString + windowInformation.Version + Environment.NewLine;
                    message += Environment.NewLine + ApplicationData.Strings.ChangeSettingToNewVersion;

                    // メッセージ表示中は処理を無効化する
                    specifyWindowItemInformation.IsEnabled = false;

                    switch (FEMessageBox.Show(message, ApplicationData.Strings.ChangeVersion + WindowControlValue.CopySeparateString + ApplicationValue.ApplicationName, MessageBoxButton.YesNoCancel))
                    {
                        case MessageBoxResult.Yes:
                            {
                                string setVersionString = "";       // バージョン文字列
                                if (checkVersionNumber.MajorNumber != -1)
                                {
                                    setVersionString += targetWindowVersionNumber.MajorNumber.ToString();
                                }
                                if (checkVersionNumber.MinorNumber != -1)
                                {
                                    setVersionString += "." + targetWindowVersionNumber.MinorNumber.ToString();
                                }
                                if (checkVersionNumber.BuildNumber != -1)
                                {
                                    setVersionString += "." + targetWindowVersionNumber.BuildNumber.ToString();
                                }
                                if (checkVersionNumber.PrivateNumber != -1)
                                {
                                    setVersionString += "." + targetWindowVersionNumber.PrivateNumber.ToString();
                                }
                                if (string.IsNullOrEmpty(setVersionString) == false)
                                {
                                    specifyWindowItemInformation.NotificationOtherThanSpecifiedVersion = setVersionString;
                                    if (string.IsNullOrEmpty(specifyWindowItemInformation.DoNotProcessingOtherThanSpecifiedVersion) == false
                                        && specifyWindowItemInformation.NotificationSynchronizationVersion)
                                    {
                                        specifyWindowItemInformation.DoNotProcessingOtherThanSpecifiedVersion = setVersionString;
                                    }
                                }
                            }

                            // メッセージが閉じられたら処理を有効化する
                            specifyWindowItemInformation.IsEnabled = true;

                            ApplicationData.EventData.DoProcessingEvent(ProcessingEventType.SpecifyWindowUpdateListBox);
                            break;
                        case MessageBoxResult.No:
                            IndexOfItemBeingProcessed = ApplicationData.Settings.SpecifyWindowInformation.Items.IndexOf(specifyWindowItemInformation);
                            ShowSpecifyWindowItemWindow(IndexOfItemBeingProcessed);
                            result = false;
                            break;
                        case MessageBoxResult.Cancel:
                            specifyWindowItemInformation.IsEnabled = false;
                            ApplicationData.EventData.DoProcessingEvent(ProcessingEventType.SpecifyWindowUpdateListBox);
                            result = false;
                            break;
                        default:
                            result = false;
                            break;
                    }

                    SettingFileProcessing.WriteSettings();
                }
            }
        }

        return result;
    }

    /// <summary>
    /// 処理するディスプレイか確認
    /// </summary>
    /// <param name="specifyWindowItemInformation">「指定ウィンドウ」機能の項目情報</param>
    /// <param name="windowProcessingInformation">ウィンドウの処理情報</param>
    /// <param name="windowInformation">ウィンドウの情報</param>
    /// <returns>処理するディスプレイかの値 (処理しない「false」/処理する「true」)</returns>
    private static bool CheckDisplayToProcessing(
        SpecifyWindowItemInformation specifyWindowItemInformation,
        WindowProcessingInformation windowProcessingInformation,
        WindowInformation windowInformation
        )
    {
        if (ApplicationData.Settings.CoordinateType == CoordinateType.EachDisplay
            && specifyWindowItemInformation.StandardDisplay == StandardDisplay.ExclusiveSpecifiedDisplay)
        {
            MonitorInformation.GetMonitorInformationForSpecifiedArea(windowInformation.Rectangle, out MonitorInfoEx monitorInfo);
            if (windowProcessingInformation.PositionSize.Display != monitorInfo.DeviceName)
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// ウィンドウを処理
    /// </summary>
    /// <param name="specifyWindowItemInformation">「指定ウィンドウ」機能の項目情報</param>
    /// <param name="windowProcessingInformation">ウィンドウの処理情報</param>
    /// <param name="hwnd">ウィンドウハンドル</param>
    /// <param name="windowInformation">ウィンドウの情報</param>
    private static void ProcessingWindow(
        SpecifyWindowItemInformation specifyWindowItemInformation,
        WindowProcessingInformation windowProcessingInformation,
        IntPtr hwnd,
        WindowInformation windowInformation
        )
    {
        try
        {
            ProcessingWindowPositionAndSize(specifyWindowItemInformation, windowProcessingInformation, hwnd, windowInformation);

            if (windowProcessingInformation.SpecifyTransparency)
            {
                if ((NativeMethods.GetWindowLongPtr(hwnd, (int)GWL.GWL_EXSTYLE) & (int)WS_EX.WS_EX_LAYERED) != (int)WS_EX.WS_EX_LAYERED)
                {
                    NativeMethods.SetWindowLongPtr(hwnd, (int)GWL.GWL_EXSTYLE, (NativeMethods.GetWindowLongPtr(hwnd, (int)GWL.GWL_EXSTYLE) ^ (int)WS_EX.WS_EX_LAYERED));
                }
                uint lwa = (uint)LWA.LWA_ALPHA;
                NativeMethods.GetLayeredWindowAttributes(hwnd, out uint getKey, out byte getAlpha, out lwa);
                if (getAlpha != windowProcessingInformation.Transparency)
                {
                    NativeMethods.SetLayeredWindowAttributes(hwnd, 0, (byte)windowProcessingInformation.Transparency, (uint)LWA.LWA_ALPHA);
                }
            }
        }
        catch
        {
        }
    }

    /// <summary>
    /// ウィンドウの位置とサイズと最前面を処理
    /// </summary>
    /// <param name="specifyWindowItemInformation">「指定ウィンドウ」機能の項目情報</param>
    /// <param name="windowProcessingInformation">ウィンドウの処理情報</param>
    /// <param name="hwnd">ウィンドウハンドル</param>
    /// <param name="windowInformation">ウィンドウの情報</param>
    private static void ProcessingWindowPositionAndSize(
        SpecifyWindowItemInformation specifyWindowItemInformation,
        WindowProcessingInformation windowProcessingInformation,
        IntPtr hwnd,
        WindowInformation windowInformation
        )
    {
        try
        {
            bool doNormal = false;     // 「通常のウィンドウ」にするかの値
            MonitorInformation.GetMonitorInformationForSpecifiedArea(windowInformation.Rectangle, out MonitorInfoEx monitorInfo);       // モニター情報
            RectangleInt changedDisplayRectangle = WindowProcessing.GetDisplayPositionAndSizeAfterProcessing(windowInformation.Rectangle, specifyWindowItemInformation.StandardDisplay, windowProcessingInformation.PositionSize.Display);        // 処理後のディスプレイの位置とサイズ

            // 位置やサイズを計算
            Rect windowRectangle = new(windowInformation.Rectangle.Left, windowInformation.Rectangle.Top, windowInformation.Rectangle.Width, windowInformation.Rectangle.Height);     // 処理後のウィンドウの位置とサイズ
            switch (windowProcessingInformation.PositionSize.SettingsWindowState)
            {
                case SettingsWindowState.DoNotChange:
                    if (ApplicationData.Settings.CoordinateType == CoordinateType.EachDisplay
                        && specifyWindowItemInformation.StandardDisplay == StandardDisplay.SpecifiedDisplay
                        && windowProcessingInformation.PositionSize.Display != monitorInfo.DeviceName)
                    {
                        windowRectangle.X = changedDisplayRectangle.Left + windowInformation.Rectangle.Right - windowInformation.Rectangle.Left;
                        windowRectangle.Y = changedDisplayRectangle.Top + windowInformation.Rectangle.Bottom - windowInformation.Rectangle.Top;
                    }
                    break;
                case SettingsWindowState.Normal:
                    windowRectangle = WindowProcessing.GetPositionSizeOfWindowAfterProcessing(hwnd, windowInformation, windowProcessingInformation.PositionSize, specifyWindowItemInformation.StandardDisplay, windowProcessingInformation.PositionSize.ClientArea);
                    doNormal = true;
                    break;
                case SettingsWindowState.Maximize:
                case SettingsWindowState.Minimize:
                    if (ApplicationData.Settings.CoordinateType == CoordinateType.EachDisplay
                        && specifyWindowItemInformation.StandardDisplay == StandardDisplay.SpecifiedDisplay
                        && windowProcessingInformation.PositionSize.Display != monitorInfo.DeviceName)
                    {
                        windowRectangle.X = changedDisplayRectangle.Left;
                        windowRectangle.Y = changedDisplayRectangle.Top;
                    }
                    break;
            }

            bool doSetWindowPos = false;        // SetWindowPosを処理するかの値

            // 位置やサイズを変更する場合は処理を有効にする
            if (windowRectangle.X != windowInformation.Rectangle.Left
                || windowRectangle.Y != windowInformation.Rectangle.Top
                || windowRectangle.Width != windowInformation.Rectangle.Width
                || windowRectangle.Height != windowInformation.Rectangle.Height)
            {
                doSetWindowPos = true;
                doNormal = true;
            }

            int flag = (int)SWP.SWP_NOACTIVATE;     // フラグ
            int hwndInsertAfter = (int)HwndInsertAfter.HWND_NOTOPMOST;     // ウィンドウの順番

            // 最前面の処理を決める
            switch (windowProcessingInformation.Forefront)
            {
                case Forefront.Cancel:
                    if ((NativeMethods.GetWindowLongPtr(hwnd, (int)GWL.GWL_EXSTYLE) & (int)WS_EX.WS_EX_TOPMOST) == (int)WS_EX.WS_EX_TOPMOST)
                    {
                        hwndInsertAfter = (int)HwndInsertAfter.HWND_NOTOPMOST;
                        doSetWindowPos = true;
                    }
                    else
                    {
                        flag |= (int)SWP.SWP_NOZORDER;
                    }
                    break;
                case Forefront.Always:
                    if ((NativeMethods.GetWindowLongPtr(hwnd, (int)GWL.GWL_EXSTYLE) & (int)WS_EX.WS_EX_TOPMOST) != (int)WS_EX.WS_EX_TOPMOST)
                    {
                        hwndInsertAfter = (int)HwndInsertAfter.HWND_TOPMOST;
                        doSetWindowPos = true;
                    }
                    else
                    {
                        flag |= (int)SWP.SWP_NOZORDER;
                    }
                    break;
                case Forefront.Forefront:
                    flag |= (int)SWP.SWP_NOZORDER;
                    NativeMethods.SetForegroundWindow(hwnd);
                    break;
                default:
                    flag |= (int)SWP.SWP_NOZORDER;
                    break;
            }

            // ウィンドウを処理
            // ウィンドウが最大化されていると位置やサイズを変更できないなどがあるので、ウィンドウの状態を「通常のウィンドウ」にする。
            if (doNormal && windowInformation.ShowCmd != (int)SW.SW_SHOWNORMAL)
            {
                // ウィンドウが最小化されている場合は、先に「SendMessage()」で最小化される前の状態に戻さないと表示が乱れるウィンドウが存在する。
                // 「SendMessage()」以外だと表示が乱れる。
                // 「ShowWindow()」などの問題？
                if (windowInformation.ShowCmd == (int)SW.SW_SHOWMINIMIZED
                    || windowInformation.ShowCmd == (int)SW.SW_MINIMIZE)
                {
                    NativeMethods.SendMessage(hwnd, (int)WM.WM_SYSCOMMAND, (int)SC.SC_RESTORE, 0);
                }
                NativeMethods.ShowWindow(hwnd, (int)SW.SW_SHOWNORMAL);
            }
            if (doSetWindowPos)
            {
                if (ApplicationData.Settings.SpecifyWindowInformation.DoNotChangeOutOfScreen && WindowProcessing.CheckWindowIsInTheScreen(windowRectangle) == false)
                {
                    flag |= (int)SWP.SWP_NOMOVE | (int)SWP.SWP_NOSIZE;
                }
                else
                {
                    if (windowProcessingInformation.PositionSize.XType == WindowXType.DoNotChange
                        && windowProcessingInformation.PositionSize.YType == WindowYType.DoNotChange)
                    {
                        flag |= (int)SWP.SWP_NOMOVE;
                    }
                    if (windowProcessingInformation.PositionSize.WidthType == WindowSizeType.DoNotChange
                        && windowProcessingInformation.PositionSize.HeightType == WindowSizeType.DoNotChange)
                    {
                        flag |= (int)SWP.SWP_NOSIZE;
                    }
                }

                // 1回目の処理
                NativeMethods.SetWindowPos(hwnd, hwndInsertAfter, (int)windowRectangle.X, (int)windowRectangle.Y, (int)windowRectangle.Width, (int)windowRectangle.Height, flag);
                // 2回目の処理
                if (windowProcessingInformation.PositionSize.SettingsWindowState == SettingsWindowState.Normal)
                {
                    windowInformation = WindowProcessing.GetWindowRectangleAndStateFromHandle(hwnd);
                    windowRectangle = WindowProcessing.GetPositionSizeOfWindowAfterProcessing(hwnd, windowInformation, windowProcessingInformation.PositionSize, specifyWindowItemInformation.StandardDisplay, windowProcessingInformation.PositionSize.ClientArea);
                    if ((int)windowRectangle.X != windowInformation.Rectangle.Left
                        || (int)windowRectangle.Y != windowInformation.Rectangle.Top
                        || (int)windowRectangle.Width != windowInformation.Rectangle.Width
                        || (int)windowRectangle.Height != windowInformation.Rectangle.Height)
                    {
                        NativeMethods.SetWindowPos(hwnd, hwndInsertAfter, (int)windowRectangle.X, (int)windowRectangle.Y, (int)windowRectangle.Width, (int)windowRectangle.Height, flag);
                    }
                }
            }
            switch (windowProcessingInformation.PositionSize.SettingsWindowState)
            {
                case SettingsWindowState.Maximize:
                    if (doNormal || windowInformation.ShowCmd != (int)SW.SW_SHOWMAXIMIZED)
                    {
                        NativeMethods.ShowWindow(hwnd, (int)SW.SW_SHOWMAXIMIZED);
                    }
                    break;
                case SettingsWindowState.Minimize:
                    if (doNormal || windowInformation.ShowCmd != (int)SW.SW_SHOWMINIMIZED)
                    {
                        NativeMethods.ShowWindow(hwnd, (int)SW.SW_SHOWMINIMIZED);
                    }
                    break;
            }
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「指定ウィンドウ」の「追加/修正」ウィンドウを表示
    /// </summary>
    /// <param name="indexOfItemToBeModified">修正する項目のインデックス (追加「-1」)</param>
    private static void ShowSpecifyWindowItemWindow(
        int indexOfItemToBeModified = -1
        )
    {
        if (SpecifyWindowItemWindow == null)
        {
            SpecifyWindowItemWindow = new(indexOfItemToBeModified);
            SpecifyWindowItemWindow.Closed += SpecifyWindowItemWindow_Closed;
            SpecifyWindowItemWindow.Show();
        }
        else
        {
            SpecifyWindowItemWindow.Activate();
        }
    }

    /// <summary>
    /// 「指定ウィンドウ」の「追加/修正」ウィンドウの「Closed」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private static void SpecifyWindowItemWindow_Closed(
        object? sender,
        EventArgs e
        )
    {
        try
        {
            ApplicationData.EventData.DoProcessingEvent(ProcessingEventType.SpecifyWindowCloseItemWindow);
            SpecifyWindowItemWindow = null;
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「SpecifyWindowItemInformation」のコピーを追加
    /// </summary>
    /// <param name="copyData">コピーするデータ</param>
    /// <param name="copyHotkey">ホットキーをコピーするかの値 (コピーしない「false」/コピーする「true」)</param>
    public static void AddCopySpecifyWindowItemInformation(
        SpecifyWindowItemInformation copyData,
        bool copyHotkey
        )
    {
        SpecifyWindowItemInformation newItem = new(copyData, copyHotkey);     // 新しい項目
        int number = 1;     // 番号
        for (int count = 0; count < ApplicationData.Settings.SpecifyWindowInformation.Items.Count; count++)
        {
            if (ApplicationData.Settings.SpecifyWindowInformation.Items[count].RegisteredName == (newItem.RegisteredName + WindowControlValue.CopySeparateString + ApplicationData.Strings.Copy + WindowControlValue.SpaceSeparateString + number))
            {
                // 番号を変えて最初から確認
                count = 0;
                number++;
            }
        }
        newItem.RegisteredName += WindowControlValue.CopySeparateString + ApplicationData.Strings.Copy + WindowControlValue.SpaceSeparateString + number;
        ApplicationData.Settings.SpecifyWindowInformation.Items.Add(newItem);
    }
}
