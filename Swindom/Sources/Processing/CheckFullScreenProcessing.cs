namespace Swindom;

/// <summary>
/// 全画面確認処理
/// </summary>
public class CheckFullScreenProcessing
{
    /// <summary>
    /// ウィンドウイベント
    /// </summary>
    private FreeEcho.FEWindowEvent.WindowEvent? WindowEvent;
    /// <summary>
    /// 全画面ウィンドウ判定のタイマー
    /// </summary>
    private System.Windows.Threading.DispatcherTimer? TimerForFullScreenJudgement;
    /// <summary>
    /// 全画面ウィンドウの判定タイマーの間隔 (ミリ秒)
    /// </summary>
    public static int FullScreenWindowDecisionTimerInterval { get; } = 10000;

    /// <summary>
    /// 開始
    /// <para>全画面ウィンドウの追加判定の変更時もこのメソッドを呼び出す。</para>
    /// </summary>
    public void Start()
    {
        if (WindowEvent == null)
        {
            WindowEvent = new();
            WindowEvent.WindowEventOccurrence += WindowEvent_WindowEventOccurrence;
            // 「MoveSizeEnd」では一部のソフトウェアでイベントが送られてこなかったので、「Show」を入れている。
            // 「Show」があれば「MoveSizeEnd」は必要なさそうだったけど、検証不足なので「MoveSizeEnd」も入れている。
            FreeEcho.FEWindowEvent.HookWindowEventType type =
                FreeEcho.FEWindowEvent.HookWindowEventType.Show
                | FreeEcho.FEWindowEvent.HookWindowEventType.MoveSizeEnd
                | FreeEcho.FEWindowEvent.HookWindowEventType.Destroy;        // イベントの種類
            WindowEvent.Hook(type);
        }

        if (ApplicationData.Settings.FullScreenWindowAdditionDecision)
        {
            if (TimerForFullScreenJudgement == null)
            {
                TimerForFullScreenJudgement = new()
                {
                    Interval = new(0, 0, 0, 0, FullScreenWindowDecisionTimerInterval)
                };
                TimerForFullScreenJudgement.Tick += (s, e) =>
                {
                    Application.Current.Dispatcher.Invoke(new Action(() =>
                    {
                        CheckExistsFullScreenWindow(null);
                    }));
                };
                TimerForFullScreenJudgement.Start();
            }
        }
        else
        {
            StopTimerForFullScreenJudgement();
        }

        CheckExistsFullScreenWindow(null);
    }

    /// <summary>
    /// 停止
    /// </summary>
    public void Stop()
    {
        if (WindowEvent != null)
        {
            WindowEvent.Dispose();
            WindowEvent = null;
        }

        StopTimerForFullScreenJudgement();

        ApplicationData.FullScreenExists = false;
    }

    /// <summary>
    /// 「全画面ウィンドウ判定のタイマー」の停止
    /// </summary>
    private void StopTimerForFullScreenJudgement()
    {
        if (TimerForFullScreenJudgement != null)
        {
            TimerForFullScreenJudgement.Stop();
            TimerForFullScreenJudgement = null;
        }
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
            CheckExistsFullScreenWindow(null);
        }
        catch
        {
        }
    }

    /// <summary>
    /// 全画面ウィンドウが存在するか確認
    /// <para>全画面ウィンドウが表示されたり無くなった場合はイベント「ProcessingEventType.FullScreenWindowShowClose」発生。</para>
    /// </summary>
    /// <param name="hwndList">ウィンドウハンドルのリスト (リストを用意しない場合「null」)</param>
    /// <returns>全画面ウィンドウが存在するかの値 (いいえ「false」/はい「true」)</returns>
    public static bool CheckExistsFullScreenWindow(
        HwndList? hwndList
        )
    {
        bool fullScreen = false;       // 全画面ウィンドウが存在するかの値

        try
        {
            HwndList checkHwndList = hwndList ?? HwndList.GetWindowHandleList();        // 確認するウィンドウハンドルのリスト
            IntPtr foregroundHwnd = NativeMethods.GetForegroundWindow();        // フォアグラウンドウィンドウのハンドル

            foreach (IntPtr nowHwnd in checkHwndList.Hwnd)
            {
                NativeMethods.GetWindowRect(nowHwnd, out RECT windowRect);      // ウィンドウの上下左右の位置
                MonitorInformation.GetMonitorInformationForSpecifiedArea(new(windowRect.Left, windowRect.Top, windowRect.Right, windowRect.Bottom), out MonitorInfoEx monitorInfo);        // MonitorInfoEx

                // ウィンドウの全画面判定
                if (monitorInfo.Monitor.Left == windowRect.Left
                    && monitorInfo.Monitor.Top == windowRect.Top
                    && monitorInfo.Monitor.Right == windowRect.Right
                    && (monitorInfo.Monitor.Bottom == windowRect.Bottom || nowHwnd != foregroundHwnd && monitorInfo.Monitor.Bottom == windowRect.Bottom + 1))     // 全画面ウィンドウがアクティブではない場合に高さが「-1」されているので「+1」している
                {
                    fullScreen = true;
                    break;
                }
            }

            if (fullScreen)
            {
                if (ApplicationData.FullScreenExists == false)
                {
                    ApplicationData.FullScreenExists = true;
                    ApplicationData.EventData.DoProcessingEvent(ProcessingEventType.FullScreenWindowShowClose);
                }
            }
            else
            {
                if (ApplicationData.FullScreenExists)
                {
                    ApplicationData.FullScreenExists = false;
                    ApplicationData.EventData.DoProcessingEvent(ProcessingEventType.FullScreenWindowShowClose);
                }
            }
        }
        catch
        {
        }

        return fullScreen;
    }

    /// <summary>
    /// ウィンドウが全画面か確認
    /// </summary>
    /// <param name="hwnd">確認するウィンドウのハンドル</param>
    /// <returns>全画面かの値 (全画面ではない「false」/全画面「true」)</returns>
    public static bool CheckFullScreenWindow(
        IntPtr hwnd
        )
    {
        bool fullScreen = false;       // 全画面かの値
        NativeMethods.GetWindowRect(hwnd, out RECT windowRect);      // ウィンドウの上下左右の位置
        MonitorInformation.GetMonitorInformationForSpecifiedArea(new(windowRect.Left, windowRect.Top, windowRect.Right, windowRect.Bottom), out MonitorInfoEx monitorInfo);        // MonitorInfoEx
        IntPtr foregroundHwnd = NativeMethods.GetForegroundWindow();        // フォアグラウンドウィンドウのハンドル

        // ウィンドウの全画面判定
        if (monitorInfo.Monitor.Left == windowRect.Left
            && monitorInfo.Monitor.Top == windowRect.Top
            && monitorInfo.Monitor.Right == windowRect.Right
            // 全画面ウィンドウがアクティブではない場合に高さが「-1」されているので「+1」している
            && (monitorInfo.Monitor.Bottom == windowRect.Bottom || hwnd != foregroundHwnd && monitorInfo.Monitor.Bottom == windowRect.Bottom + 1))
        {
            fullScreen = true;
        }

        return fullScreen;
    }
}
