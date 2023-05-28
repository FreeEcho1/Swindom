namespace Swindom;

/// <summary>
/// 「マグネット」処理
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
    /// 移動中のウィンドウのハンドル
    /// </summary>
    private IntPtr MovingHwnd;
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
        if (Disposed)
        {
            return;
        }
        if (disposing)
        {
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
        Disposed = true;
    }

    /// <summary>
    /// 処理が有効か確認
    /// </summary>
    /// <returns>処理が有効かの値 (無効「false」/有効「true」)</returns>
    public static bool CheckIfTheProcessingIsValid() => ApplicationData.Settings.MagnetInformation.Enabled
            && (ApplicationData.Settings.MagnetInformation.StopProcessingFullScreen == false || ApplicationData.FullScreenExists == false);

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
            HwndList hwndList = new();
            hwndList.Hwnd.Add(NativeMethods.GetForegroundWindow());
            if (ApplicationData.Settings.MagnetInformation.StopProcessingFullScreen && VariousWindowProcessing.CheckFullScreenWindow(hwndList))
            {
                return;
            }

            NativeMethods.GetCursorPos(out POINT cursorPosition);       // マウスカーソルの位置
            PreviousCursorPosition = new(cursorPosition.x, cursorPosition.y);
            MovingHwnd = NativeMethods.GetForegroundWindow();     // 移動中のウィンドウのハンドル

            if (HwndList == null && ApplicationData.Settings.MagnetInformation.PasteToAnotherWindow)
            {
                HwndList = HwndList.GetWindowHandleList();
                HwndList.Hwnd.Remove(NativeMethods.GetForegroundWindow());      // 移動中のウィンドウのハンドルは除外
            }
            if (CursorStopXTimer == null)
            {
                CursorStopXTimer = new();
                CursorStopXTimer.Tick += CursorStopXTimer_Tick;
            }
            CursorStopXTimer.Interval = new(0, 0, 0, 0, ApplicationData.Settings.MagnetInformation.StopTimeWhenPasted);
            if (CursorStopYTimer == null)
            {
                CursorStopYTimer = new();
                CursorStopYTimer.Tick += CursorStopYTimer_Tick;
            }
            CursorStopYTimer.Interval = new(0, 0, 0, 0, ApplicationData.Settings.MagnetInformation.StopTimeWhenPasted);
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
            NativeMethods.GetCursorPos(out POINT cursorPosition);       // マウスカーソルの位置
            System.Drawing.Point nowCursorPosition = new(cursorPosition.x, cursorPosition.y);        // マウスカーソルの位置

            if (ApplicationData.Settings.MagnetInformation.PasteToPressKey == false
                || (Keyboard.GetKeyStates(Key.LeftCtrl) & KeyStates.Down) == KeyStates.Down
                || (Keyboard.GetKeyStates(Key.RightCtrl) & KeyStates.Down) == KeyStates.Down)
            {
                if (ApplicationData.Settings.MagnetInformation.PasteToEdgeOfScreen)
                {
                    PasteToTheEdgeOfScreenProcessing(nowCursorPosition);
                }
                if (ApplicationData.Settings.MagnetInformation.PasteToAnotherWindow)
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
            NativeMethods.ClipCursor(IntPtr.Zero);
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
            MovingHwnd = IntPtr.Zero;
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
                if (ApplicationData.MonitorInformation.MonitorInfo.Count == 1)
                {
                    NativeMethods.ClipCursor(CursorClip);
                }
                else
                {
                    NativeMethods.ClipCursor(IntPtr.Zero);
                }
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
                if (ApplicationData.MonitorInformation.MonitorInfo.Count == 1)
                {
                    NativeMethods.ClipCursor(CursorClip);
                }
                else
                {
                    NativeMethods.ClipCursor(IntPtr.Zero);
                }
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
            NativeMethods.GetWindowPlacement(MovingHwnd, out WINDOWPLACEMENT windowPlacement);        // WINDOWPLACEMENT
            MonitorInformation.GetMonitorInformationOnWindowShown(new(windowPlacement.rcNormalPosition.Left, windowPlacement.rcNormalPosition.Top, windowPlacement.rcNormalPosition.Right, windowPlacement.rcNormalPosition.Bottom), out MonitorInfoEx monitorInfo);       // ウィンドウがあるモニター情報
            NativeMethods.GetWindowRect(MovingHwnd, out RECT windowRect);     // ウィンドウの上下左右の位置

            // 貼り付ける位置をずらす
            if (ApplicationData.Settings.ShiftPastePosition.Enabled)
            {
                windowRect.Left -= ApplicationData.Settings.ShiftPastePosition.Left;
                windowRect.Top -= ApplicationData.Settings.ShiftPastePosition.Top;
                windowRect.Right -= ApplicationData.Settings.ShiftPastePosition.Right;
                windowRect.Bottom -= ApplicationData.Settings.ShiftPastePosition.Bottom;
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
                if (monitorInfo.WorkArea.Left - ApplicationData.Settings.MagnetInformation.PastingDecisionDistance < windowRect.Left
                    && windowRect.Left < monitorInfo.WorkArea.Left + ApplicationData.Settings.MagnetInformation.PastingDecisionDistance)
                {
                    if (windowRect.Left <= monitorInfo.WorkArea.Left && 0 < cursorMovedDistance.X
                        || monitorInfo.WorkArea.Left <= windowRect.Left && cursorMovedDistance.X < 0)
                    {
                        checkCursorMoveX = 1;
                    }
                }
                // ウィンドウがディスプレイの右側の貼り付ける範囲内にある
                else if (monitorInfo.WorkArea.Right - ApplicationData.Settings.MagnetInformation.PastingDecisionDistance < windowRect.Right
                    && windowRect.Right < monitorInfo.WorkArea.Right + ApplicationData.Settings.MagnetInformation.PastingDecisionDistance)
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
                if (monitorInfo.WorkArea.Top - ApplicationData.Settings.MagnetInformation.PastingDecisionDistance < windowRect.Top
                    && windowRect.Top < monitorInfo.WorkArea.Top + ApplicationData.Settings.MagnetInformation.PastingDecisionDistance)
                {
                    if (windowRect.Top <= monitorInfo.WorkArea.Top && 0 < cursorMovedDistance.Y
                        || monitorInfo.WorkArea.Top <= windowRect.Top && cursorMovedDistance.Y < 0)
                    {
                        checkCursorMoveY = 1;
                    }
                }
                // ウィンドウがディスプレイの下側の貼り付ける範囲内にある
                else if (monitorInfo.WorkArea.Bottom - ApplicationData.Settings.MagnetInformation.PastingDecisionDistance < windowRect.Bottom
                    && windowRect.Bottom < monitorInfo.WorkArea.Bottom + ApplicationData.Settings.MagnetInformation.PastingDecisionDistance)
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
                NativeMethods.GetClipCursor(out RECT nowClipRect);      // 現在のマウスカーソルのクリッピング範囲

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
                    CursorStopXTimer.Interval = new(0, 0, 0, 0, ApplicationData.Settings.MagnetInformation.StopTimeWhenPasted);
                    CursorStopXTimer.Start();
                }
                if (checkCursorMoveY != 0 && CursorStopYTimer?.IsEnabled == false)
                {
                    CursorStopYTimer.Interval = new(0, 0, 0, 0, ApplicationData.Settings.MagnetInformation.StopTimeWhenPasted);
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
                NativeMethods.GetWindowRect(MovingHwnd, out RECT movingWindowRect);     // 移動中のウィンドウの上下左右の位置

                RectangleInt distanceToShift = new();      // ずらす距離
                if (ApplicationData.Settings.ShiftPastePosition.Enabled)
                {
                    distanceToShift.Left = ApplicationData.Settings.ShiftPastePosition.Left;
                    distanceToShift.Top = ApplicationData.Settings.ShiftPastePosition.Top;
                    distanceToShift.Right = ApplicationData.Settings.ShiftPastePosition.Right;
                    distanceToShift.Bottom = ApplicationData.Settings.ShiftPastePosition.Bottom;
                }

                movingWindowRect.Left -= distanceToShift.Left;
                movingWindowRect.Top -= distanceToShift.Top;
                movingWindowRect.Right -= distanceToShift.Right;
                movingWindowRect.Bottom -= distanceToShift.Bottom;

                foreach (IntPtr hwnd in HwndList.Hwnd)
                {
                    NativeMethods.GetWindowRect(hwnd, out RECT windowRect);     // 貼り付くウィンドウか調べるウィンドウの上下左右の位置
                    RECT stickWindowRect = new()
                    {
                        Left = windowRect.Left - distanceToShift.Left,
                        Top = windowRect.Top - distanceToShift.Top,
                        Right = windowRect.Right - distanceToShift.Right,
                        Bottom = windowRect.Bottom - distanceToShift.Bottom
                    };      // 貼り付けるウィンドウの貼り付ける位置をずらした上下左右の位置

                    // 貼り付けるか判定
                    byte checkCursorMoveX = 0;        // マウスカーソルのXの移動判定 (移動してない「0」/左「1」/右「2」)
                    if (CursorStopXTimer?.IsEnabled == false
                        && stickWindowRect.Top < movingWindowRect.Bottom
                        && movingWindowRect.Top < stickWindowRect.Bottom)
                    {
                        // 左側の貼り付ける範囲内にある
                        if (stickWindowRect.Right - ApplicationData.Settings.MagnetInformation.PastingDecisionDistance <= movingWindowRect.Left
                            && movingWindowRect.Left <= stickWindowRect.Right + ApplicationData.Settings.MagnetInformation.PastingDecisionDistance)
                        {
                            if (movingWindowRect.Left <= stickWindowRect.Right && 0 < cursorMovedDistance.X
                                || stickWindowRect.Right <= movingWindowRect.Left && cursorMovedDistance.X < 0)
                            {
                                checkCursorMoveX = 1;
                            }
                        }
                        // 右側の貼り付ける範囲内にある
                        else if (stickWindowRect.Left - ApplicationData.Settings.MagnetInformation.PastingDecisionDistance <= movingWindowRect.Right
                            && movingWindowRect.Right <= stickWindowRect.Left + ApplicationData.Settings.MagnetInformation.PastingDecisionDistance)
                        {
                            if (movingWindowRect.Right <= stickWindowRect.Left && 0 < cursorMovedDistance.X
                                || stickWindowRect.Left <= movingWindowRect.Right && cursorMovedDistance.X < 0)
                            {
                                checkCursorMoveX = 2;
                            }
                        }
                    }
                    byte checkCursorMoveY = 0;        // マウスカーソルのYの移動判定 (移動してない「0」/上「1」/下「2」)
                    if (CursorStopYTimer?.IsEnabled == false
                        && stickWindowRect.Left < movingWindowRect.Right
                        && movingWindowRect.Left < stickWindowRect.Right)
                    {
                        // 上側の貼り付ける範囲内にある
                        if (stickWindowRect.Bottom - ApplicationData.Settings.MagnetInformation.PastingDecisionDistance <= movingWindowRect.Top
                            && movingWindowRect.Top <= stickWindowRect.Bottom + ApplicationData.Settings.MagnetInformation.PastingDecisionDistance)
                        {
                            if (movingWindowRect.Top <= stickWindowRect.Bottom && 0 < cursorMovedDistance.Y
                                || stickWindowRect.Bottom <= movingWindowRect.Top && cursorMovedDistance.Y < 0)
                            {
                                checkCursorMoveY = 1;
                            }
                        }
                        // 下側の貼り付ける範囲内にある
                        else if (stickWindowRect.Top - ApplicationData.Settings.MagnetInformation.PastingDecisionDistance <= movingWindowRect.Bottom
                            && movingWindowRect.Bottom <= stickWindowRect.Top + ApplicationData.Settings.MagnetInformation.PastingDecisionDistance)
                        {
                            if (movingWindowRect.Bottom <= stickWindowRect.Top && 0 < cursorMovedDistance.Y
                                || stickWindowRect.Top <= movingWindowRect.Bottom && cursorMovedDistance.Y < 0)
                            {
                                checkCursorMoveY = 2;
                            }
                        }
                    }

                    // 貼り付ける
                    if (checkCursorMoveX != 0 || checkCursorMoveY != 0)
                    {
                        MonitorInformation.GetMonitorInformationOnWindowShown(new(windowRect.Left, windowRect.Top, windowRect.Right, windowRect.Bottom), out MonitorInfoEx monitorInfo);       // ウィンドウがあるモニター情報
                        RectClass settingClipRect = new()
                        {
                            Left = monitorInfo.WorkArea.Left,
                            Top = monitorInfo.WorkArea.Top,
                            Right = monitorInfo.WorkArea.Right,
                            Bottom = monitorInfo.WorkArea.Bottom
                        };      // 設定するマウスカーソルのクリッピング範囲
                        NativeMethods.GetClipCursor(out RECT nowClipRect);      // 現在のマウスカーソルのクリッピング範囲

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
                            settingClipRect.Left = stickWindowRect.Right + (cursorPosition.X - movingWindowRect.Left);
                            settingClipRect.Right = settingClipRect.Left + 1;
                        }
                        else if (checkCursorMoveX == 2)
                        {
                            settingClipRect.Left = stickWindowRect.Left - (movingWindowRect.Right - cursorPosition.X);
                            settingClipRect.Right = settingClipRect.Left + 1;
                        }
                        if (CursorStopYTimer?.IsEnabled == true)
                        {
                            settingClipRect.Top = nowClipRect.Top;
                            settingClipRect.Bottom = nowClipRect.Bottom;
                        }
                        else if (checkCursorMoveY == 1)
                        {
                            settingClipRect.Top = stickWindowRect.Bottom + (cursorPosition.Y - movingWindowRect.Top);
                            settingClipRect.Bottom = settingClipRect.Top + 1;
                        }
                        else if (checkCursorMoveY == 2)
                        {
                            settingClipRect.Top = stickWindowRect.Top - (movingWindowRect.Bottom - cursorPosition.Y);
                            settingClipRect.Bottom = settingClipRect.Top + 1;
                        }
                        NativeMethods.ClipCursor(settingClipRect);

                        // タイマー開始
                        if (checkCursorMoveX != 0 && CursorStopXTimer?.IsEnabled == false)
                        {
                            CursorStopXTimer.Interval = new(0, 0, 0, 0, ApplicationData.Settings.MagnetInformation.StopTimeWhenPasted);
                            CursorStopXTimer.Start();
                        }
                        if (checkCursorMoveY != 0 && CursorStopYTimer?.IsEnabled == false)
                        {
                            CursorStopYTimer.Interval = new(0, 0, 0, 0, ApplicationData.Settings.MagnetInformation.StopTimeWhenPasted);
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
