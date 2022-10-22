namespace Swindom;

/// <summary>
/// マグネット処理
/// </summary>
public class MagnetProcessing : IDisposable
{
    /// <summary>
    /// Disposeが呼ばれたかの値
    /// </summary>
    private bool Disposed;
    /// <summary>
    /// マウスカーソルでウィンドウ移動検知
    /// </summary>
    private readonly FreeEcho.FEWindowMoveDetectionMouse.MouseMoveWindowDetection MouseMoveWindowDetection = new();
    /// <summary>
    /// マウスカーソルのX座標の停止タイマー
    /// </summary>
    private System.Windows.Threading.DispatcherTimer? CursorStopXTimer;
    /// <summary>
    /// マウスカーソルのY座標の停止タイマー
    /// </summary>
    private System.Windows.Threading.DispatcherTimer? CursorStopYTimer;
    /// <summary>
    /// 初期のマウスカーソルのクリッピング範囲
    /// </summary>
    private RectClass CursorClip = new();
    /// <summary>
    /// 前回のマウスカーソルの位置
    /// </summary>
    private System.Drawing.Point PreviousCursorPosition;
    /// <summary>
    /// ウィンドウハンドルのリスト
    /// </summary>
    private HwndList? HwndList;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public MagnetProcessing()
    {
        MouseMoveWindowDetection.StartMove += MouseMoveWindowDetection_StartMove;
        MouseMoveWindowDetection.Moved += DetectWindowMovementWithMouse_Moved;
        MouseMoveWindowDetection.StopMove += MouseMoveWindowDetection_StopMove;
        MouseMoveWindowDetection.Start();
    }

    /// <summary>
    /// デストラクタ
    /// </summary>
    ~MagnetProcessing()
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
            MouseMoveWindowDetection.Stop();
            if (CursorStopXTimer != null)
            {
                CursorStopXTimer.Stop();
                CursorStopXTimer = null;
            }
            if (CursorStopYTimer != null)
            {
                CursorStopYTimer.Stop();
                CursorStopYTimer = null;
            }
        }
    }

    /// <summary>
    /// 処理が有効か確認
    /// </summary>
    /// <returns>処理が有効かの値 (無効「false」/有効「true」)</returns>
    public static bool CheckIfTheProcessingIsValid() => Common.ApplicationData.Settings.MagnetInformation.Enabled
            && (Common.ApplicationData.Settings.MagnetInformation.StopProcessingFullScreen == false || Common.ApplicationData.FullScreenExists == false);

    /// <summary>
    /// 「マウスカーソルでウィンドウ移動検知」の「StartMove」イベント
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
            POINT cursorPosition;      // マウスカーソルの位置
            NativeMethods.GetCursorPos(out cursorPosition);
            PreviousCursorPosition = new(cursorPosition.x, cursorPosition.y);

            if (CursorStopXTimer == null)
            {
                CursorStopXTimer = new();
                CursorStopXTimer.Tick += CursorStopXTimer_Tick;
            }
            CursorStopXTimer.Interval = new(0, 0, 0, 0, Common.ApplicationData.Settings.MagnetInformation.StopTimeWhenPasted);
            if (CursorStopYTimer == null)
            {
                CursorStopYTimer = new();
                CursorStopYTimer.Tick += CursorStopYTimer_Tick;
            }
            CursorStopYTimer.Interval = new(0, 0, 0, 0, Common.ApplicationData.Settings.MagnetInformation.StopTimeWhenPasted);

            if (HwndList == null && Common.ApplicationData.Settings.MagnetInformation.PasteToAnotherWindow)
            {
                HwndList = HwndList.GetWindowHandleList();
                IntPtr movingHwnd = NativeMethods.GetForegroundWindow();     // 移動中のウィンドウのハンドル
                HwndList.Hwnd.Remove(movingHwnd);
            }
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「マウスカーソルでウィンドウ移動検知」の「Moved」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void DetectWindowMovementWithMouse_Moved(
        object sender,
        FreeEcho.FEWindowMoveDetectionMouse.MoveEventArgs e
        )
    {
        try
        {
            POINT cursorPosition;      // マウスカーソルの位置
            NativeMethods.GetCursorPos(out cursorPosition);
            System.Drawing.Point nowCursorPosition = new(cursorPosition.x, cursorPosition.y);        // 現在のマウスカーソル位置

            if (Common.ApplicationData.Settings.MagnetInformation.PressTheKeyToPaste == false
                || (Keyboard.GetKeyStates(Key.LeftCtrl) & KeyStates.Down) == KeyStates.Down
                || (Keyboard.GetKeyStates(Key.RightCtrl) & KeyStates.Down) == KeyStates.Down)
            {
                if (Common.ApplicationData.Settings.MagnetInformation.PasteToScreenEdge)
                {
                    PasteToTheEdgeOfScreenProcessing(nowCursorPosition);
                }
                if (Common.ApplicationData.Settings.MagnetInformation.PasteToAnotherWindow)
                {
                    PasteIntoAnotherWindowProcessing(nowCursorPosition);
                }
            }

            // マウスカーソルの位置を保存
            PreviousCursorPosition = nowCursorPosition;
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「マウスカーソルでウィンドウ移動検知」の「StopMove」イベント
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
            NativeMethods.ClipCursor(null);
            if (CursorStopXTimer != null)
            {
                CursorStopXTimer.Stop();
                CursorStopXTimer = null;
            }
            if (CursorStopYTimer != null)
            {
                CursorStopYTimer.Stop();
                CursorStopYTimer = null;
            }
            if (HwndList != null)
            {
                HwndList.Hwnd.Clear();
                HwndList = null;
            }
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「マウスカーソルのX座標の停止タイマー」の「Tick」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void CursorStopXTimer_Tick(
        object? sender,
        EventArgs e
        )
    {
        try
        {
            CursorStopXTimer?.Stop();
            if (CursorStopYTimer?.IsEnabled == false)
            {
                // ディスプレイの数で初期のクリッピング範囲が違うから (タスクバーの領域も含まれるか) 設定する値を分けている
                NativeMethods.ClipCursor(Common.MonitorInformation.MonitorInfo.Count == 1 ? CursorClip : null);
            }
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「マウスカーソルのY座標の停止タイマー」の「Tick」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void CursorStopYTimer_Tick(
        object? sender,
        EventArgs e
        )
    {
        try
        {
            CursorStopYTimer?.Stop();
            if (CursorStopXTimer?.IsEnabled == false)
            {
                NativeMethods.ClipCursor(Common.MonitorInformation.MonitorInfo.Count == 1 ? CursorClip : null);
            }
        }
        catch
        {
        }
    }

    /// <summary>
    /// 画面端に貼り付ける処理
    /// </summary>
    /// <param name="cursorPosition">マウスカーソル位置</param>
    private void PasteToTheEdgeOfScreenProcessing(
        System.Drawing.Point cursorPosition
        )
    {
        try
        {
            IntPtr hwnd = NativeMethods.GetForegroundWindow();     // ウィンドウハンドル
            MonitorInfoEx monitorInfo;     // モニター情報
            MonitorInformation.GetMonitorWithWindow(hwnd, out monitorInfo);
            RECT windowRect;      // ウィンドウの左上と右下の座標
            NativeMethods.GetWindowRect(hwnd, out windowRect);
            WINDOWPLACEMENT windowPlacement;       // WINDOWPLACEMENT
            NativeMethods.GetWindowPlacement(hwnd, out windowPlacement);

            // 貼り付ける位置をずらす
            if (Common.ApplicationData.Settings.ShiftPastePosition.Enabled)
            {
                windowRect.Left -= Common.ApplicationData.Settings.ShiftPastePosition.Left;
                windowRect.Top -= Common.ApplicationData.Settings.ShiftPastePosition.Top;
                windowRect.Right -= Common.ApplicationData.Settings.ShiftPastePosition.Right;
                windowRect.Bottom -= Common.ApplicationData.Settings.ShiftPastePosition.Bottom;
            }

            // 貼り付けるか判定
            System.Drawing.Point cursorMovedDistance = new()
            {
                X = cursorPosition.X - PreviousCursorPosition.X,
                Y = cursorPosition.Y - PreviousCursorPosition.Y
            };        // マウスカーソルが移動した距離
            byte checkCursorMoveX = 0;        // マウスカーソルのXの移動判定 (移動してない「0」/左「1」/右「2」)
            if (CursorStopXTimer?.IsEnabled == false)
            {
                // ウィンドウがディスプレイの左側の貼り付ける範囲内にある
                if (monitorInfo.WorkArea.Left - Common.ApplicationData.Settings.MagnetInformation.DecisionDistanceToPaste < windowRect.Left
                    && windowRect.Left < monitorInfo.WorkArea.Left + Common.ApplicationData.Settings.MagnetInformation.DecisionDistanceToPaste)
                {
                    if (windowRect.Left <= monitorInfo.WorkArea.Left && 0 < cursorMovedDistance.X
                        || monitorInfo.WorkArea.Left <= windowRect.Left && cursorMovedDistance.X < 0)
                    {
                        checkCursorMoveX = 1;
                    }
                }
                // ウィンドウがディスプレイの右側の貼り付ける範囲内にある
                else if (monitorInfo.WorkArea.Right - Common.ApplicationData.Settings.MagnetInformation.DecisionDistanceToPaste < windowRect.Right
                    && windowRect.Right < monitorInfo.WorkArea.Right + Common.ApplicationData.Settings.MagnetInformation.DecisionDistanceToPaste)
                {
                    if (windowRect.Right <= monitorInfo.WorkArea.Right && 0 < cursorMovedDistance.X
                        || monitorInfo.WorkArea.Right <= windowRect.Right && cursorMovedDistance.X < 0)
                    {
                        checkCursorMoveX = 2;
                    }
                }
            }
            byte checkCursorMoveY = 0;        // マウスカーソルのYの移動判定 (移動してない「0」/上「1」/下「2」)
            if (CursorStopYTimer?.IsEnabled == false)
            {
                // ウィンドウがディスプレイの上側の貼り付ける範囲内にある
                if (monitorInfo.WorkArea.Top - Common.ApplicationData.Settings.MagnetInformation.DecisionDistanceToPaste < windowRect.Top
                    && windowRect.Top < monitorInfo.WorkArea.Top + Common.ApplicationData.Settings.MagnetInformation.DecisionDistanceToPaste)
                {
                    if (windowRect.Top <= monitorInfo.WorkArea.Top && 0 < cursorMovedDistance.Y
                        || monitorInfo.WorkArea.Top <= windowRect.Top && cursorMovedDistance.Y < 0)
                    {
                        checkCursorMoveY = 1;
                    }
                }
                // ウィンドウがディスプレイの下側の貼り付ける範囲内にある
                else if (monitorInfo.WorkArea.Bottom - Common.ApplicationData.Settings.MagnetInformation.DecisionDistanceToPaste < windowRect.Bottom
                    && windowRect.Bottom < monitorInfo.WorkArea.Bottom + Common.ApplicationData.Settings.MagnetInformation.DecisionDistanceToPaste)
                {
                    if (windowRect.Bottom <= monitorInfo.WorkArea.Bottom && 0 < cursorMovedDistance.Y
                        || monitorInfo.WorkArea.Bottom <= windowRect.Bottom && cursorMovedDistance.Y < 0)
                    {
                        checkCursorMoveY = 2;
                    }
                }
            }

            // 貼り付ける
            if (checkCursorMoveX != 0 || checkCursorMoveY != 0)
            {
                RectClass settingClipRect = new()
                {
                    Left = monitorInfo.WorkArea.Left,
                    Top = monitorInfo.WorkArea.Top,
                    Right = monitorInfo.WorkArea.Right,
                    Bottom = monitorInfo.WorkArea.Bottom
                };      // 設定するマウスカーソルのクリッピング範囲
                RECT nowClipRect;     // 現在のマウスカーソルのクリッピング範囲
                NativeMethods.GetClipCursor(out nowClipRect);

                // 初期のマウスカーソルのクリッピング範囲を保存
                if (CursorStopXTimer?.IsEnabled == false && CursorStopYTimer?.IsEnabled == false)
                {
                    CursorClip = new()
                    {
                        Left = nowClipRect.Left,
                        Top = nowClipRect.Top,
                        Right = nowClipRect.Right,
                        Bottom = nowClipRect.Bottom
                    };
                }

                // マウスカーソルの移動制限範囲を計算して制限
                if (CursorStopXTimer?.IsEnabled == true)
                {
                    settingClipRect.Left = nowClipRect.Left;
                    settingClipRect.Right = nowClipRect.Right;
                }
                else if (checkCursorMoveX == 1)
                {
                    settingClipRect.Left = monitorInfo.WorkArea.Left + (cursorPosition.X - windowRect.Left);
                    settingClipRect.Right = settingClipRect.Left + 1;
                }
                else if (checkCursorMoveX == 2)
                {
                    settingClipRect.Left = monitorInfo.WorkArea.Right - (windowRect.Right - windowRect.Left) + (cursorPosition.X - windowRect.Left);
                    settingClipRect.Right = settingClipRect.Left + 1;
                }
                if (CursorStopYTimer?.IsEnabled == true)
                {
                    settingClipRect.Top = nowClipRect.Top;
                    settingClipRect.Bottom = nowClipRect.Bottom;
                }
                else if (checkCursorMoveY == 1)
                {
                    settingClipRect.Top = monitorInfo.WorkArea.Top + (cursorPosition.Y - windowRect.Top);
                    settingClipRect.Bottom = settingClipRect.Top + 1;
                }
                else if (checkCursorMoveY == 2)
                {
                    settingClipRect.Top = monitorInfo.WorkArea.Bottom - (windowRect.Bottom - windowRect.Top) + (cursorPosition.Y - windowRect.Top);
                    settingClipRect.Bottom = settingClipRect.Top + 1;
                }
                NativeMethods.ClipCursor(settingClipRect);

                // タイマー開始
                if (checkCursorMoveX != 0 && CursorStopXTimer?.IsEnabled == false)
                {
                    CursorStopXTimer.Interval = new(0, 0, 0, 0, Common.ApplicationData.Settings.MagnetInformation.StopTimeWhenPasted);
                    CursorStopXTimer.Start();
                }
                if (checkCursorMoveY != 0 && CursorStopYTimer?.IsEnabled == false)
                {
                    CursorStopYTimer.Interval = new(0, 0, 0, 0, Common.ApplicationData.Settings.MagnetInformation.StopTimeWhenPasted);
                    CursorStopYTimer.Start();
                }
            }
        }
        catch
        {
        }
    }

    /// <summary>
    /// 別のウィンドウに貼り付ける処理
    /// </summary>
    /// <param name="cursorPosition">マウスカーソル位置</param>
    private void PasteIntoAnotherWindowProcessing(
        System.Drawing.Point cursorPosition
        )
    {
        try
        {
            if (0 < HwndList?.Hwnd.Count)
            {
                System.Drawing.Point cursorMovedDistance = new()
                {
                    X = cursorPosition.X - PreviousCursorPosition.X,
                    Y = cursorPosition.Y - PreviousCursorPosition.Y
                };        // マウスカーソルが移動した距離
                IntPtr movingHwnd = NativeMethods.GetForegroundWindow();     // 移動中のウィンドウのハンドル
                RECT movingWindowRect;      // 移動中のウィンドウの左上と右下の座標
                NativeMethods.GetWindowRect(movingHwnd, out movingWindowRect);

                RECT distanceToShift = new();      // ずらす距離
                if (Common.ApplicationData.Settings.ShiftPastePosition.Enabled)
                {
                    distanceToShift.Left = Common.ApplicationData.Settings.ShiftPastePosition.Left;
                    distanceToShift.Top = Common.ApplicationData.Settings.ShiftPastePosition.Top;
                    distanceToShift.Right = Common.ApplicationData.Settings.ShiftPastePosition.Right;
                    distanceToShift.Bottom = Common.ApplicationData.Settings.ShiftPastePosition.Bottom;
                }

                foreach (IntPtr hwnd in HwndList.Hwnd)
                {
                    RECT windowRect;      // 貼り付くウィンドウか調べるウィンドウの左上と右下の座標
                    NativeMethods.GetWindowRect(hwnd, out windowRect);
                    windowRect.Left += distanceToShift.Right - distanceToShift.Left;
                    windowRect.Top += distanceToShift.Bottom - distanceToShift.Top;
                    windowRect.Right -= distanceToShift.Right - distanceToShift.Left;
                    windowRect.Bottom -= distanceToShift.Bottom - distanceToShift.Top;

                    // 貼り付けるか判定
                    byte checkCursorMoveX = 0;        // マウスカーソルのXの移動判定 (移動してない「0」/左「1」/右「2」)
                    if (CursorStopXTimer?.IsEnabled == false
                        && windowRect.Top < movingWindowRect.Bottom
                        && movingWindowRect.Top < windowRect.Bottom)
                    {
                        // 左側の貼り付ける範囲内にある
                        if (windowRect.Right - Common.ApplicationData.Settings.MagnetInformation.DecisionDistanceToPaste <= movingWindowRect.Left
                            && movingWindowRect.Left <= windowRect.Right + Common.ApplicationData.Settings.MagnetInformation.DecisionDistanceToPaste)
                        {
                            if (movingWindowRect.Left <= windowRect.Right && 0 < cursorMovedDistance.X
                                || windowRect.Right <= movingWindowRect.Left && cursorMovedDistance.X < 0)
                            {
                                checkCursorMoveX = 1;
                            }
                        }
                        // 右側の貼り付ける範囲内にある
                        else if (windowRect.Left - Common.ApplicationData.Settings.MagnetInformation.DecisionDistanceToPaste <= movingWindowRect.Right
                            && movingWindowRect.Right <= windowRect.Left + Common.ApplicationData.Settings.MagnetInformation.DecisionDistanceToPaste)
                        {
                            if (movingWindowRect.Right <= windowRect.Left && 0 < cursorMovedDistance.X
                                || windowRect.Left <= movingWindowRect.Right && cursorMovedDistance.X < 0)
                            {
                                checkCursorMoveX = 2;
                            }
                        }
                    }
                    byte checkCursorMoveY = 0;        // マウスカーソルのYの移動判定 (移動してない「0」/上「1」/下「2」)
                    if (CursorStopYTimer?.IsEnabled == false
                        && windowRect.Left < movingWindowRect.Right
                        && movingWindowRect.Left < windowRect.Right)
                    {
                        // 上側の貼り付ける範囲内にある
                        if (windowRect.Bottom - Common.ApplicationData.Settings.MagnetInformation.DecisionDistanceToPaste <= movingWindowRect.Top
                            && movingWindowRect.Top <= windowRect.Bottom + Common.ApplicationData.Settings.MagnetInformation.DecisionDistanceToPaste)
                        {
                            if (movingWindowRect.Top <= windowRect.Bottom && 0 < cursorMovedDistance.Y
                                || windowRect.Bottom <= movingWindowRect.Top && cursorMovedDistance.Y < 0)
                            {
                                checkCursorMoveY = 1;
                            }
                        }
                        // 下側の貼り付ける範囲内にある
                        else if (windowRect.Top - Common.ApplicationData.Settings.MagnetInformation.DecisionDistanceToPaste <= movingWindowRect.Bottom
                            && movingWindowRect.Bottom <= windowRect.Top + Common.ApplicationData.Settings.MagnetInformation.DecisionDistanceToPaste)
                        {
                            if (movingWindowRect.Bottom <= windowRect.Top && 0 < cursorMovedDistance.Y
                                || windowRect.Top <= movingWindowRect.Bottom && cursorMovedDistance.Y < 0)
                            {
                                checkCursorMoveY = 2;
                            }
                        }
                    }

                    // 貼り付ける
                    if (checkCursorMoveX != 0 || checkCursorMoveY != 0)
                    {
                        MonitorInfoEx monitorInfo;     // ウィンドウがあるモニター情報
                        MonitorInformation.GetMonitorWithWindow(hwnd, out monitorInfo);
                        RectClass settingClipRect = new()
                        {
                            Left = monitorInfo.WorkArea.Left,
                            Top = monitorInfo.WorkArea.Top,
                            Right = monitorInfo.WorkArea.Right,
                            Bottom = monitorInfo.WorkArea.Bottom
                        };      // 設定するマウスカーソルのクリッピング範囲
                        RECT nowClipRect;     // 現在のマウスカーソルのクリッピング範囲
                        NativeMethods.GetClipCursor(out nowClipRect);

                        // 初期状態のマウスカーソルのクリッピング範囲を保存
                        if (CursorStopXTimer?.IsEnabled == false && CursorStopYTimer?.IsEnabled == false)
                        {
                            CursorClip = new()
                            {
                                Left = nowClipRect.Left,
                                Top = nowClipRect.Top,
                                Right = nowClipRect.Right,
                                Bottom = nowClipRect.Bottom
                            };
                        }

                        // マウスカーソルの移動制限範囲を計算して制限
                        if (CursorStopXTimer?.IsEnabled == true)
                        {
                            settingClipRect.Left = nowClipRect.Left;
                            settingClipRect.Right = nowClipRect.Right;
                        }
                        else if (checkCursorMoveX == 1)
                        {
                            settingClipRect.Left = windowRect.Right + (cursorPosition.X - movingWindowRect.Left);
                            settingClipRect.Right = settingClipRect.Left + 1;
                        }
                        else if (checkCursorMoveX == 2)
                        {
                            settingClipRect.Left = windowRect.Left - (movingWindowRect.Right - cursorPosition.X);
                            settingClipRect.Right = settingClipRect.Left + 1;
                        }
                        if (CursorStopYTimer?.IsEnabled == true)
                        {
                            settingClipRect.Top = nowClipRect.Top;
                            settingClipRect.Bottom = nowClipRect.Bottom;
                        }
                        else if (checkCursorMoveY == 1)
                        {
                            settingClipRect.Top = windowRect.Bottom + (cursorPosition.Y - movingWindowRect.Top);
                            settingClipRect.Bottom = settingClipRect.Top + 1;
                        }
                        else if (checkCursorMoveY == 2)
                        {
                            settingClipRect.Top = windowRect.Top - (movingWindowRect.Bottom - cursorPosition.Y);
                            settingClipRect.Bottom = settingClipRect.Top + 1;
                        }
                        NativeMethods.ClipCursor(settingClipRect);

                        // タイマー開始
                        if (checkCursorMoveX != 0 && CursorStopXTimer?.IsEnabled == false)
                        {
                            CursorStopXTimer.Interval = new(0, 0, 0, 0, Common.ApplicationData.Settings.MagnetInformation.StopTimeWhenPasted);
                            CursorStopXTimer.Start();
                        }
                        if (checkCursorMoveY != 0 && CursorStopYTimer?.IsEnabled == false)
                        {
                            CursorStopYTimer.Interval = new(0, 0, 0, 0, Common.ApplicationData.Settings.MagnetInformation.StopTimeWhenPasted);
                            CursorStopYTimer.Start();
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
}
