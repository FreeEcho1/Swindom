namespace Swindom;

/// <summary>
/// ウィンドウ処理
/// </summary>
public static class WindowProcessing
{
    /// <summary>
    /// ウィンドウが存在するウィンドウハンドルを列挙するコールバックメソッド
    /// </summary>
    /// <param name="hwnd">ウィンドウハンドル</param>
    /// <param name="lparam">アプリケーション定義の値</param>
    /// <returns>列挙中止「0」/列挙続行「0以外」</returns>
    public static int EnumerateHwnd(
        IntPtr hwnd,
        ref HwndList lparam
        )
    {
        try
        {
            // 負荷が少なくなると思われる順番で判定
            if (NativeMethods.IsWindowVisible(hwnd))
            {
                bool isInvisibleUwpApp;        // ウィンドウのないUWPアプリ
                _ = NativeMethods.DwmGetWindowAttribute(hwnd, (uint)DWMWINDOWATTRIBUTE.Cloaked, out isInvisibleUwpApp, Marshal.SizeOf(typeof(bool)));

                if (isInvisibleUwpApp == false
                    && (NativeMethods.GetWindowLongPtr(hwnd, (int)GWL.GWL_EXSTYLE) & (int)WS_EX.WS_EX_TOOLWINDOW) == 0)
                {
                    lparam.Hwnd.Add(hwnd);
                }
            }
        }
        catch
        {
        }

        return 1;
    }

    /// <summary>
    /// ウィンドウ処理後のディスプレイの位置とサイズを取得
    /// </summary>
    /// <param name="coordinateType">座標指定の種類</param>
    /// <param name="hwnd">ウィンドウハンドル</param>
    /// <param name="standardDisplay">基準にするディスプレイ</param>
    /// <param name="displayName">ディスプレイ名</param>
    /// <param name="hotkey">ホットキー処理かの値 (いいえ「false」/はい「true」)</param>
    /// <returns>位置とサイズ</returns>
    public static RectangleDecimal GetDisplayPositionAndSizeAfterProcessing(
        CoordinateType coordinateType,
        IntPtr hwnd,
        StandardDisplay standardDisplay = StandardDisplay.DisplayWithWindow,
        string displayName = "",
        bool hotkey = false
        )
    {
        RectangleDecimal displayRectangle = new();       // ディスプレイの位置とサイズ

        switch (coordinateType)
        {
            case CoordinateType.Global:
                {
                    RECT rect = GetAllDisplayPositionSize();
                    displayRectangle.X = rect.Left;
                    displayRectangle.Y = rect.Top;
                    displayRectangle.Width = Math.Abs(rect.Left) + rect.Right;
                    displayRectangle.Height = Math.Abs(rect.Top) + rect.Bottom;
                }
                break;
            default:
                if (hotkey == false && (standardDisplay == StandardDisplay.OnlySpecifiedDisplay || standardDisplay == StandardDisplay.DisplayWithWindow || string.IsNullOrEmpty(displayName)))
                {
                    MonitorInfoEx monitorInfo;
                    MonitorInformation.GetMonitorWithWindow(hwnd, out monitorInfo, Common.MonitorInformation);
                    displayRectangle = new(monitorInfo.WorkArea.Left, monitorInfo.WorkArea.Top, monitorInfo.WorkArea.Right - monitorInfo.WorkArea.Left, monitorInfo.WorkArea.Bottom - monitorInfo.WorkArea.Top);
                }
                else
                {
                    foreach (MonitorInfoEx nowMonitorInfo in Common.MonitorInformation.MonitorInfo)
                    {
                        if (nowMonitorInfo.DeviceName == displayName)
                        {
                            displayRectangle = new(nowMonitorInfo.WorkArea.Left, nowMonitorInfo.WorkArea.Top, nowMonitorInfo.WorkArea.Right - nowMonitorInfo.WorkArea.Left, nowMonitorInfo.WorkArea.Bottom - nowMonitorInfo.WorkArea.Top);
                            break;
                        }
                    }
                }
                break;
        }

        return displayRectangle;
    }

    /// <summary>
    /// 全てのディスプレイを合わせた上下左右の位置を取得
    /// </summary>
    /// <returns>全てのディスプレイを合わせた位置とサイズ</returns>
    public static RECT GetAllDisplayPositionSize()
    {
        RECT rect = new();

        foreach (MonitorInfoEx nowMonitorInfo in Common.MonitorInformation.MonitorInfo)
        {
            if (nowMonitorInfo.WorkArea.Left < rect.Left)
            {
                rect.Left = nowMonitorInfo.WorkArea.Left;
            }
            if (nowMonitorInfo.WorkArea.Top < rect.Top)
            {
                rect.Top = nowMonitorInfo.WorkArea.Top;
            }
            if (rect.Right < nowMonitorInfo.WorkArea.Right)
            {
                rect.Right = nowMonitorInfo.WorkArea.Right;
            }
            if (rect.Bottom < nowMonitorInfo.WorkArea.Bottom)
            {
                rect.Bottom = nowMonitorInfo.WorkArea.Bottom;
            }
        }

        return rect;
    }

    /// <summary>
    /// 全画面ウィンドウがあるか確認
    /// <para>全画面ウィンドウが表示されたり無くなった場合はイベント「ProcessingEventType.FullScreenWindowShowClose」発生。</para>
    /// </summary>
    /// <param name="hwndList">ウィンドウハンドルのリスト (ない「null」)</param>
    /// <returns>全画面ウィンドウがあるかの値 (いいえ「false」/はい「true」)</returns>
    public static bool CheckFullScreenWindow(
        HwndList? hwndList
        )
    {
        bool fullScreen = false;       // 全画面ウィンドウがあるかの値

        try
        {
            HwndList checkHwndList = hwndList ?? HwndList.GetWindowHandleList();        // 確認するウィンドウハンドルのリスト
            IntPtr foregroundHwnd = NativeMethods.GetForegroundWindow();        // フォアグラウンドウィンドウのハンドル

            foreach (IntPtr nowHwnd in checkHwndList.Hwnd)
            {
                RECT windowRect;      // ウィンドウの左上と右下の座標
                NativeMethods.GetWindowRect(nowHwnd, out windowRect);
                MonitorInfoEx monitorInfo;     // MonitorInfoEx
                MonitorInformation.GetMonitorWithWindow(nowHwnd, out monitorInfo, Common.MonitorInformation);

                // ウィンドウの全画面判定
                if (monitorInfo.Monitor.Left == windowRect.Left
                    && monitorInfo.Monitor.Top == windowRect.Top
                    && monitorInfo.Monitor.Right == windowRect.Right
                    && (monitorInfo.Monitor.Bottom == windowRect.Bottom || nowHwnd != foregroundHwnd && monitorInfo.Monitor.Bottom == windowRect.Bottom + 1))     // 全画面ウィンドウがアクティブではない場合に高さが「-1」されているから「+1」している
                {
                    fullScreen = true;
                    break;
                }
            }

            if (fullScreen)
            {
                if (Common.ApplicationData.FullScreenExists == false)
                {
                    Common.ApplicationData.FullScreenExists = true;
                    Common.ApplicationData.DoProcessingEvent(ProcessingEventType.FullScreenWindowShowClose);
                }
            }
            else
            {
                if (Common.ApplicationData.FullScreenExists)
                {
                    Common.ApplicationData.FullScreenExists = false;
                    Common.ApplicationData.DoProcessingEvent(ProcessingEventType.FullScreenWindowShowClose);
                }
            }
        }
        catch
        {
        }

        return fullScreen;
    }

    /// <summary>
    /// 変更後のウィンドウの位置とサイズを計算
    /// </summary>
    /// <param name="hwnd">ウィンドウハンドル</param>
    /// <param name="windowPlacement">WINDOWPLACEMENT</param>
    /// <param name="positionSize">位置とサイズ</param>
    /// <param name="standardDisplay">基準にするディスプレイ</param>
    /// <param name="clientArea">クライアントエリアを対象とするかの値 (いいえ「false」/はい「true」)</param>
    /// <returns>変更後のウィンドウの位置とサイズ</returns>
    public static RectangleDecimal CalculatePositionAndSize(
        IntPtr hwnd,
        WINDOWPLACEMENT windowPlacement,
        PositionSize positionSize,
        StandardDisplay standardDisplay,
        bool clientArea
        )
    {
        // クライアントエリアを対象とする場合は枠のエリアのサイズを取得
        RectClass frameArea = new();      // 枠のエリア
        if (clientArea)
        {
            POINT clientAreaPosition = new();       // クライアントエリアの位置
            NativeMethods.ClientToScreen(hwnd, ref clientAreaPosition);
            RECT clientAreaRect;      // クライアントエリアの左上と右下の座標
            NativeMethods.GetClientRect(hwnd, out clientAreaRect);

            frameArea.Left = clientAreaPosition.x - windowPlacement.rcNormalPosition.Left;
            frameArea.Top = clientAreaPosition.y - windowPlacement.rcNormalPosition.Top;
            frameArea.Right = windowPlacement.rcNormalPosition.Right - windowPlacement.rcNormalPosition.Left - clientAreaRect.Right - frameArea.Left;
            frameArea.Bottom = windowPlacement.rcNormalPosition.Bottom - windowPlacement.rcNormalPosition.Top - clientAreaRect.Bottom - frameArea.Top;
        }

        RectangleDecimal changeDisplayRectangle = GetDisplayPositionAndSizeAfterProcessing(Common.ApplicationData.Settings.CoordinateType, hwnd, standardDisplay, positionSize.Display);     // 変更後のディスプレイの位置とサイズ

        // サイズを計算
        SizeDecimal changeWindowSize = new(windowPlacement.rcNormalPosition.Right - windowPlacement.rcNormalPosition.Left, windowPlacement.rcNormalPosition.Bottom - windowPlacement.rcNormalPosition.Top);      // 変更後のウィンドウサイズ
        if (positionSize.WidthType == WindowSizeType.Value)
        {
            switch (positionSize.WidthValueType)
            {
                case PositionSizeValueType.Pixel:
                    changeWindowSize.Width = positionSize.Size.Width + frameArea.Left + frameArea.Right;
                    break;
                case PositionSizeValueType.Percent:
                    changeWindowSize.Width = (positionSize.Size.Width * (changeDisplayRectangle.Width / 100.0m)) + frameArea.Left + frameArea.Right;
                    break;
            }
        }
        if (positionSize.HeightType == WindowSizeType.Value)
        {
            switch (positionSize.HeightValueType)
            {
                case PositionSizeValueType.Pixel:
                    changeWindowSize.Height = positionSize.Size.Height + frameArea.Top + frameArea.Bottom;
                    break;
                case PositionSizeValueType.Percent:
                    changeWindowSize.Height = (positionSize.Size.Height * (changeDisplayRectangle.Height / 100.0m)) + frameArea.Top + frameArea.Bottom;
                    break;
            }
        }

        // 位置を計算
        RECT distanceOffsetFromScreenEdge = new();      // 画面端からずらす距離
        if (Common.ApplicationData.Settings.ShiftPastePosition.Enabled && clientArea == false)
        {
            distanceOffsetFromScreenEdge.Left = Common.ApplicationData.Settings.ShiftPastePosition.Left;
            distanceOffsetFromScreenEdge.Top = Common.ApplicationData.Settings.ShiftPastePosition.Top;
            distanceOffsetFromScreenEdge.Right = Common.ApplicationData.Settings.ShiftPastePosition.Right;
            distanceOffsetFromScreenEdge.Bottom = Common.ApplicationData.Settings.ShiftPastePosition.Bottom;
        }
        PointDecimal changeWindowPosition = new(windowPlacement.rcNormalPosition.Left, windowPlacement.rcNormalPosition.Top);      // 変更後のウィンドウ位置
        switch (Common.ApplicationData.Settings.CoordinateType)
        {
            case CoordinateType.Global:
                switch (positionSize.XType)
                {
                    case WindowXType.DoNotChange:
                        changeWindowPosition.X = windowPlacement.rcNormalPosition.Left;
                        break;
                    case WindowXType.Left:
                        changeWindowPosition.X = changeDisplayRectangle.X - frameArea.Left;
                        break;
                    case WindowXType.Middle:
                        changeWindowPosition.X = changeDisplayRectangle.X + changeDisplayRectangle.Width / 2 - changeWindowSize.Width / 2;
                        break;
                    case WindowXType.Right:
                        changeWindowPosition.X = changeDisplayRectangle.Right - changeWindowSize.Width + distanceOffsetFromScreenEdge.Right + frameArea.Right;
                        break;
                    case WindowXType.Value:
                        switch (positionSize.XValueType)
                        {
                            case PositionSizeValueType.Pixel:
                                changeWindowPosition.X = positionSize.Position.X - frameArea.Left;
                                break;
                            case PositionSizeValueType.Percent:
                                changeWindowPosition.X = changeDisplayRectangle.X + changeDisplayRectangle.Width / 100.0m * positionSize.Position.X - frameArea.Left;
                                break;
                        }
                        break;
                }
                switch (positionSize.YType)
                {
                    case WindowYType.DoNotChange:
                        changeWindowPosition.Y = windowPlacement.rcNormalPosition.Top;
                        break;
                    case WindowYType.Top:
                        changeWindowPosition.Y = changeDisplayRectangle.Y - frameArea.Top;
                        break;
                    case WindowYType.Middle:
                        changeWindowPosition.Y = changeDisplayRectangle.Y + changeDisplayRectangle.Height / 2 - changeWindowSize.Height / 2;
                        break;
                    case WindowYType.Bottom:
                        changeWindowPosition.Y = changeDisplayRectangle.Y + changeDisplayRectangle.Height - changeWindowSize.Height + distanceOffsetFromScreenEdge.Bottom - frameArea.Bottom;
                        break;
                    case WindowYType.Value:
                        switch (positionSize.YValueType)
                        {
                            case PositionSizeValueType.Pixel:
                                changeWindowPosition.Y = positionSize.Position.Y - frameArea.Top;
                                break;
                            case PositionSizeValueType.Percent:
                                changeWindowPosition.Y = changeDisplayRectangle.Y + changeDisplayRectangle.Height / 100.0m * positionSize.Position.Y - frameArea.Top;
                                break;
                        }
                        break;
                }
                break;
            default:
                {
                    MonitorInfoEx monitorInfo;     // MonitorInfoEx
                    MonitorInformation.GetMonitorWithWindow(hwnd, out monitorInfo);
                    System.Drawing.Rectangle displayRectangle = new(monitorInfo.Monitor.Left, monitorInfo.Monitor.Top, monitorInfo.Monitor.Right - monitorInfo.Monitor.Left, monitorInfo.Monitor.Bottom - monitorInfo.Monitor.Top);      // ウィンドウがあるディスプレイの位置とサイズ
                    switch (positionSize.XType)
                    {
                        case WindowXType.DoNotChange:
                            changeWindowPosition.X = changeDisplayRectangle.X + windowPlacement.rcNormalPosition.Left - displayRectangle.Left;
                            break;
                        case WindowXType.Left:
                            changeWindowPosition.X = changeDisplayRectangle.X + distanceOffsetFromScreenEdge.Left - frameArea.Left;
                            break;
                        case WindowXType.Middle:
                            changeWindowPosition.X = changeDisplayRectangle.X + changeDisplayRectangle.Width / 2 - changeWindowSize.Width / 2;
                            break;
                        case WindowXType.Right:
                            changeWindowPosition.X = changeDisplayRectangle.X + changeDisplayRectangle.Width - changeWindowSize.Width + distanceOffsetFromScreenEdge.Right + frameArea.Right;
                            break;
                        case WindowXType.Value:
                            switch (positionSize.XValueType)
                            {
                                case PositionSizeValueType.Pixel:
                                    changeWindowPosition.X = changeDisplayRectangle.X + positionSize.Position.X - frameArea.Left;
                                    break;
                                case PositionSizeValueType.Percent:
                                    changeWindowPosition.X = changeDisplayRectangle.X + changeDisplayRectangle.Width / 100.0m * positionSize.Position.X - frameArea.Left;
                                    break;
                            }
                            break;
                    }
                    switch (positionSize.YType)
                    {
                        case WindowYType.DoNotChange:
                            changeWindowPosition.Y = changeDisplayRectangle.Y + windowPlacement.rcNormalPosition.Top - displayRectangle.Top;
                            break;
                        case WindowYType.Top:
                            changeWindowPosition.Y = changeDisplayRectangle.Y + distanceOffsetFromScreenEdge.Top - frameArea.Top;
                            break;
                        case WindowYType.Middle:
                            changeWindowPosition.Y = changeDisplayRectangle.Y + changeDisplayRectangle.Height / 2 - changeWindowSize.Height / 2;
                            break;
                        case WindowYType.Bottom:
                            changeWindowPosition.Y = changeDisplayRectangle.Y + changeDisplayRectangle.Height - changeWindowSize.Height + distanceOffsetFromScreenEdge.Bottom + frameArea.Bottom;
                            break;
                        case WindowYType.Value:
                            switch (positionSize.YValueType)
                            {
                                case PositionSizeValueType.Pixel:
                                    changeWindowPosition.Y = changeDisplayRectangle.Y + positionSize.Position.Y - frameArea.Top;
                                    break;
                                case PositionSizeValueType.Percent:
                                    changeWindowPosition.Y = changeDisplayRectangle.Y + changeDisplayRectangle.Height / 100.0m * positionSize.Position.Y - frameArea.Top;
                                    break;
                            }
                            break;
                    }
                }
                break;
        }

        return new((int)changeWindowPosition.X, (int)changeWindowPosition.Y, (int)changeWindowSize.Width, (int)changeWindowSize.Height);
    }

    /// <summary>
    /// ウィンドウハンドルからウィンドウの情報を取得
    /// </summary>
    /// <param name="hwnd">ウィンドウハンドル</param>
    /// <param name="wib">ウィンドウ情報のバッファ (使用中の場合は使用しない)</param>
    /// <returns>ウィンドウの情報</returns>
    public static WindowInformation GetWindowInformation(
        IntPtr hwnd,
        WindowInformationBuffer wib
        )
    {
        WindowInformation information = new();      // ウィンドウの情報
        bool checkProcessing = wib.ProcessingGetWindowInformation;

        wib.ProcessingGetWindowInformation = true;

        // タイトル名取得
        try
        {
            StringBuilder getString;
            int length = NativeMethods.GetWindowTextLength(hwnd) + 1;
            if (checkProcessing)
            {
                getString = new(length);
            }
            else
            {
                wib.TitleName.Clear();
                if (wib.TitleName.Capacity < length)
                {
                    wib.TitleName = new(length);
                }
                getString = wib.TitleName;
            }
            _ = NativeMethods.GetWindowText(hwnd, getString, getString.Capacity);
            information.TitleName = getString.ToString();
        }
        catch
        {
        }
        // クラス名取得
        try
        {
            StringBuilder getString;
            if (checkProcessing)
            {
                getString = new(Common.ClassNameMaxLength);
            }
            else
            {
                wib.ClassName.Clear();
                getString = wib.ClassName;
            }
            _ = NativeMethods.GetClassName(hwnd, getString, getString.Capacity);
            information.ClassName = getString.ToString();
        }
        catch
        {
        }
        // ファイル名取得
        try
        {
            int id = 0;     // プロセスID
            _ = NativeMethods.GetWindowThreadProcessId(hwnd, out id);
            IntPtr process = NativeMethods.OpenProcess(ProcessAccessFlags.QueryInformation | ProcessAccessFlags.VirtualMemoryRead, false, id);
            if (process != IntPtr.Zero)
            {
                IntPtr pmodules;
                if (NativeMethods.EnumProcessModules(process, out pmodules, (uint)Marshal.SizeOf(typeof(IntPtr)), out _))
                {
                    StringBuilder getString;
                    if (checkProcessing)
                    {
                        getString = new(Common.PathLength);
                    }
                    else
                    {
                        wib.FileName.Clear();
                        getString = wib.FileName;
                    }
                    _ = NativeMethods.GetModuleFileNameEx(process, pmodules, getString, getString.Capacity);
                    information.FileName = getString.ToString();
                }
                NativeMethods.CloseHandle(process);
            }
        }
        catch
        {
        }
        // バージョン取得
        try
        {
            FileVersionInfo info = FileVersionInfo.GetVersionInfo(information.FileName);
            if (info.ProductVersion != null)
            {
                information.Version = info.ProductVersion;
            }
        }
        catch
        {
        }

        wib.ProcessingGetWindowInformation = false;

        return information;
    }

    /// <summary>
    /// ウィンドウが画面内 (どれかのディスプレイ) にあるか確認
    /// </summary>
    /// <param name="windowRectangle">ウィンドウの位置とサイズ</param>
    /// <returns>画面内にあるかの値 (いいえ「false」/はい「true」)</returns>
    public static bool CheckWindowIsInTheScreen(
        RectangleDecimal windowRectangle
        )
    {
        bool result = false;        // 結果

        try
        {
            foreach (MonitorInfoEx nowMonitorInfo in Common.MonitorInformation.MonitorInfo)
            {
                RectangleDecimal intersectRectangle = RectangleDecimal.Intersect(new(nowMonitorInfo.WorkArea.Left, nowMonitorInfo.WorkArea.Top, nowMonitorInfo.WorkArea.Right - nowMonitorInfo.WorkArea.Left, nowMonitorInfo.WorkArea.Bottom - nowMonitorInfo.WorkArea.Top), windowRectangle);     // 交差の値
                if (intersectRectangle.IsEmpty == false)
                {
                    result = true;
                    break;
                }
            }
        }
        catch
        {
        }

        return result;
    }

    /// <summary>
    /// 画面外に存在するウィンドウを画面内に移動
    /// </summary>
    public static void MoveWindowIntoScreen()
    {
        if (Common.MonitorInformation.MonitorInfo.Count != 0)
        {
            HwndList hwndList = HwndList.GetWindowHandleList();

            foreach (IntPtr nowHwnd in hwndList.Hwnd)
            {
                WINDOWPLACEMENT windowPlacement;
                NativeMethods.GetWindowPlacement(nowHwnd, out windowPlacement);
                RectangleDecimal windowRectangle = new(windowPlacement.rcNormalPosition.Left, windowPlacement.rcNormalPosition.Top, windowPlacement.rcNormalPosition.Right - windowPlacement.rcNormalPosition.Left, windowPlacement.rcNormalPosition.Bottom - windowPlacement.rcNormalPosition.Top);

                if (CheckWindowIsInTheScreen(windowRectangle) == false)
                {
                    if (windowPlacement.showCmd != (int)SW.SW_SHOWNORMAL)
                    {
                        NativeMethods.ShowWindow(nowHwnd, (int)SW.SW_SHOWNORMAL);
                    }

                    MonitorInfoEx monitorInfo = Common.MonitorInformation.MonitorInfo[0];
                    NativeMethods.SetWindowPos(nowHwnd, (int)HwndInsertAfter.HWND_NOTOPMOST, monitorInfo.WorkArea.Left, monitorInfo.WorkArea.Top, 0, 0, (int)SWP.SWP_NOSIZE | (int)SWP.SWP_NOZORDER);
                }
            }
        }
    }
}
