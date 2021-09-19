using Swindom.Source.Settings;

namespace Swindom.Source
{
    /// <summary>
    /// ウィンドウ処理
    /// </summary>
    public static class WindowProcessing
    {
        /// <summary>
        /// ウィンドウが存在するウィンドウハンドルを列挙するためのコールバックメソッド
        /// </summary>
        /// <param name="hwnd">ウィンドウハンドル</param>
        /// <param name="lparam">アプリケーション定義の値</param>
        /// <returns>列挙中止「0」/列挙続行「0以外」</returns>
        public static int EnumerateHwnd(
            System.IntPtr hwnd,
            ref HwndList lparam
            )
        {
            try
            {
                // 負荷が少なくなると思われる順番で判定
                if (NativeMethods.IsWindowVisible(hwnd))
                {
                    bool isInvisibleUwpApp;        // ウィンドウのないUWPアプリ
                    NativeMethods.DwmGetWindowAttribute(hwnd, (uint)DWMWINDOWATTRIBUTE.Cloaked, out isInvisibleUwpApp, System.Runtime.InteropServices.Marshal.SizeOf(typeof(bool)));

                    if ((isInvisibleUwpApp == false)
                        && ((NativeMethods.GetWindowLongPtr(hwnd, (int)GWL.GWL_EXSTYLE) & (int)WS_EX.WS_EX_TOOLWINDOW) == 0)
                        )
                    {
                        lparam.Hwnd.Add(hwnd);
                    }
                }
            }
            catch
            {
            }

            return (1);
        }

        /// <summary>
        /// ウィンドウ処理後のディスプレイの位置とサイズを取得
        /// </summary>
        /// <param name="coordinateType">座標指定の種類</param>
        /// <param name="hwnd">ウィンドウハンドル</param>
        /// <param name="monitorInformation">モニター情報 (「null」でも可)</param>
        /// <param name="standardDisplay">基準にするディスプレイ</param>
        /// <param name="displayName">ディスプレイ名</param>
        /// <param name="hotkey">ホットキー処理かの値 (いいえ「false」/はい「true」)</param>
        /// <returns>位置とサイズ</returns>
        public static RectangleDecimal GetDisplayPositionAndSizeAfterProcessing(
            CoordinateType coordinateType,
            System.IntPtr hwnd,
            MonitorInformation monitorInformation,
            StandardDisplay standardDisplay = StandardDisplay.DisplayWithWindow,
            string displayName = "",
            bool hotkey = false
            )
        {
            RectangleDecimal displayRectangle = new();       // ディスプレイの位置とサイズ

            if (monitorInformation == null)
            {
                monitorInformation = MonitorInformation.GetMonitorInformation();
            }

            switch (coordinateType)
            {
                case CoordinateType.Global:
                    {
                        RECT rect = new();
                        foreach (MonitorInfoEx nowMonitorInfo in monitorInformation.MonitorInfo)
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
                        displayRectangle.X = rect.Left;
                        displayRectangle.Y = rect.Top;
                        displayRectangle.Width = System.Math.Abs(rect.Left) + rect.Right;
                        displayRectangle.Height = System.Math.Abs(rect.Top) + rect.Bottom;
                    }
                    break;
                default:
                    if ((hotkey == false) && ((standardDisplay == StandardDisplay.OnlySpecifiedDisplay) || (standardDisplay == StandardDisplay.DisplayWithWindow) || string.IsNullOrEmpty(displayName)))
                    {
                        MonitorInfoEx monitorInfo;
                        MonitorInformation.GetMonitorWithWindow(hwnd, out monitorInfo, monitorInformation);
                        displayRectangle = new(monitorInfo.WorkArea.Left, monitorInfo.WorkArea.Top, monitorInfo.WorkArea.Right - monitorInfo.WorkArea.Left, monitorInfo.WorkArea.Bottom - monitorInfo.WorkArea.Top);
                    }
                    else
                    {
                        foreach (MonitorInfoEx nowMonitorInfo in monitorInformation.MonitorInfo)
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

            return (displayRectangle);
        }

        /// <summary>
        /// 全画面ウィンドウがあるか確認
        /// <para>全画面ウィンドウが表示されたり無くなった場合はイベント「ProcessingEventType.FullScreenWindowShowClose」発生。</para>
        /// </summary>
        /// <param name="hwndList">ウィンドウハンドルのリスト (ない「null」)</param>
        /// <param name="monitorInformation">モニター情報 (「null」でも可)</param>
        /// <returns>全画面ウィンドウがあるかの値 (いいえ「false」/はい「true」)</returns>
        public static bool CheckFullScreenWindow(
            HwndList hwndList,
            MonitorInformation monitorInformation
            )
        {
            bool fullScreen = false;       // 全画面ウィンドウがあるかの値

            try
            {
                HwndList checkHwndList = hwndList ?? HwndList.GetWindowHandleList();        // 確認するウィンドウハンドルのリスト
                System.IntPtr foregroundHwnd = NativeMethods.GetForegroundWindow();        // フォアグラウンドウィンドウのハンドル

                if (monitorInformation == null)
                {
                    monitorInformation = MonitorInformation.GetMonitorInformation();
                }

                foreach (System.IntPtr nowHwnd in checkHwndList.Hwnd)
                {
                    RECT windowRect;      // ウィンドウの左上と右下の座標
                    NativeMethods.GetWindowRect(nowHwnd, out windowRect);
                    MonitorInfoEx monitorInfo;     // MonitorInfoEx
                    MonitorInformation.GetMonitorWithWindow(nowHwnd, out monitorInfo, monitorInformation);

                    // ウィンドウの全画面判定
                    if ((monitorInfo.Monitor.Left == windowRect.Left)
                        && (monitorInfo.Monitor.Top == windowRect.Top)
                        && (monitorInfo.Monitor.Right == windowRect.Right)
                        && ((monitorInfo.Monitor.Bottom == windowRect.Bottom) || ((nowHwnd != foregroundHwnd) && (monitorInfo.Monitor.Bottom == windowRect.Bottom + 1))))     // 全画面ウィンドウがアクティブではない場合に高さが「-1」されているから「+1」している
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

            return (fullScreen);
        }

        /// <summary>
        /// 変更後のウィンドウの位置とサイズを計算
        /// </summary>
        /// <param name="hwnd">ウィンドウハンドル</param>
        /// <param name="windowPlacement">WINDOWPLACEMENT</param>
        /// <param name="positionSize">位置とサイズ</param>
        /// <param name="standardDisplay">基準にするディスプレイ</param>
        /// <param name="clientArea">クライアントエリアを対象とするかの値 (いいえ「false」/はい「true」)</param>
        /// <param name="monitorInformation">モニター情報 (「null」でも可)</param>
        /// <returns>変更後のウィンドウの位置とサイズ</returns>
        public static RectangleDecimal CalculatePositionAndSize(
            System.IntPtr hwnd,
            WINDOWPLACEMENT windowPlacement,
            PositionSize positionSize,
            StandardDisplay standardDisplay,
            bool clientArea,
            MonitorInformation monitorInformation
            )
        {
            if (monitorInformation == null)
            {
                monitorInformation = MonitorInformation.GetMonitorInformation();
            }

            // クライアントエリアを対象とする場合は位置やサイズを調整
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

            RectangleDecimal changeDisplayRectangle = GetDisplayPositionAndSizeAfterProcessing(Common.ApplicationData.Settings.CoordinateType, hwnd, monitorInformation, standardDisplay, positionSize.Display);     // 変更後のディスプレイの位置とサイズ

            // サイズを計算
            SizeDecimal changeWindowSize = new(windowPlacement.rcNormalPosition.Right - windowPlacement.rcNormalPosition.Left, windowPlacement.rcNormalPosition.Bottom - windowPlacement.rcNormalPosition.Top);      // 変更後のウィンドウサイズ
            if (positionSize.WidthType == WindowSizeType.Value)
            {
                switch (positionSize.WidthValueType)
                {
                    case ValueType.Pixel:
                        changeWindowSize.Width = positionSize.Size.Width + frameArea.Left + frameArea.Right;
                        break;
                    case ValueType.Percent:
                        changeWindowSize.Width = (positionSize.Size.Width + frameArea.Left + frameArea.Right) * (changeDisplayRectangle.Width / 100.0m);
                        break;
                }
            }
            if (positionSize.HeightType == WindowSizeType.Value)
            {
                switch (positionSize.HeightValueType)
                {
                    case ValueType.Pixel:
                        changeWindowSize.Height = positionSize.Size.Height + frameArea.Top + frameArea.Bottom;
                        break;
                    case ValueType.Percent:
                        changeWindowSize.Height = (positionSize.Size.Height + frameArea.Top + frameArea.Bottom) * (changeDisplayRectangle.Height / 100.0m);
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
                            changeWindowPosition.X = changeDisplayRectangle.X + (changeDisplayRectangle.Width / 2) - (changeWindowSize.Width / 2);
                            break;
                        case WindowXType.Right:
                            changeWindowPosition.X = changeDisplayRectangle.Right - changeWindowSize.Width + distanceOffsetFromScreenEdge.Right + frameArea.Right;
                            break;
                        case WindowXType.Value:
                            switch (positionSize.XValueType)
                            {
                                case ValueType.Pixel:
                                    changeWindowPosition.X = positionSize.Position.X - frameArea.Left;
                                    break;
                                case ValueType.Percent:
                                    changeWindowPosition.X = changeDisplayRectangle.X + (changeDisplayRectangle.Width / 100.0m) * positionSize.Position.X - frameArea.Left;
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
                            changeWindowPosition.Y = changeDisplayRectangle.Y + (changeDisplayRectangle.Height / 2) - (changeWindowSize.Height / 2);
                            break;
                        case WindowYType.Bottom:
                            changeWindowPosition.Y = changeDisplayRectangle.Y + changeDisplayRectangle.Height - changeWindowSize.Height + distanceOffsetFromScreenEdge.Bottom - frameArea.Bottom;
                            break;
                        case WindowYType.Value:
                            switch (positionSize.YValueType)
                            {
                                case ValueType.Pixel:
                                    changeWindowPosition.Y = positionSize.Position.Y - frameArea.Top;
                                    break;
                                case ValueType.Percent:
                                    changeWindowPosition.Y = changeDisplayRectangle.Y + (changeDisplayRectangle.Height / 100.0m) * positionSize.Position.Y - frameArea.Top;
                                    break;
                            }
                            break;
                    }
                    break;
                default:
                    {
                        MonitorInfoEx monitorInfo;     // MonitorInfoEx
                        MonitorInformation.GetMonitorWithWindow(hwnd, out monitorInfo, monitorInformation);
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
                                changeWindowPosition.X = changeDisplayRectangle.X + (changeDisplayRectangle.Width / 2) - (changeWindowSize.Width / 2);
                                break;
                            case WindowXType.Right:
                                changeWindowPosition.X = changeDisplayRectangle.X + changeDisplayRectangle.Width - changeWindowSize.Width + distanceOffsetFromScreenEdge.Right + frameArea.Right;
                                break;
                            case WindowXType.Value:
                                switch (positionSize.XValueType)
                                {
                                    case ValueType.Pixel:
                                        changeWindowPosition.X = changeDisplayRectangle.X + positionSize.Position.X - frameArea.Left;
                                        break;
                                    case ValueType.Percent:
                                        changeWindowPosition.X = changeDisplayRectangle.X + (changeDisplayRectangle.Width / 100.0m) * positionSize.Position.X - frameArea.Left;
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
                                changeWindowPosition.Y = changeDisplayRectangle.Y + (changeDisplayRectangle.Height / 2) - (changeWindowSize.Height / 2);
                                break;
                            case WindowYType.Bottom:
                                changeWindowPosition.Y = changeDisplayRectangle.Y + changeDisplayRectangle.Height - changeWindowSize.Height + distanceOffsetFromScreenEdge.Bottom + frameArea.Bottom;
                                break;
                            case WindowYType.Value:
                                switch (positionSize.YValueType)
                                {
                                    case ValueType.Pixel:
                                        changeWindowPosition.Y = changeDisplayRectangle.Y + positionSize.Position.Y - frameArea.Top;
                                        break;
                                    case ValueType.Percent:
                                        changeWindowPosition.Y = changeDisplayRectangle.Y + (changeDisplayRectangle.Height / 100.0m) * positionSize.Position.Y - frameArea.Top;
                                        break;
                                }
                                break;
                        }
                    }
                    break;
            }

            return (new((int)changeWindowPosition.X, (int)changeWindowPosition.Y, (int)changeWindowSize.Width, (int)changeWindowSize.Height));
        }

        /// <summary>
        /// ウィンドウハンドルからウィンドウの情報を取得
        /// </summary>
        /// <param name="hwnd">ウィンドウハンドル</param>
        /// <returns>ウィンドウの情報</returns>
        public static WindowInformation GetWindowInformation(
            System.IntPtr hwnd
            )
        {
            WindowInformation information = new();      // ウィンドウの情報

            // タイトル名取得
            try
            {
                int length = NativeMethods.GetWindowTextLength(hwnd) + 1;
                System.Text.StringBuilder title = new(length);
                NativeMethods.GetWindowText(hwnd, title, length);
                information.TitleName = title.ToString();
            }
            catch
            {
            }
            // クラス名取得
            try
            {
                System.Text.StringBuilder getString = new(Common.ClassNameMaxLength);
                NativeMethods.GetClassName(hwnd, getString, getString.Capacity);
                information.ClassName = getString.ToString();
            }
            catch
            {
            }
            // ファイル名取得
            try
            {
                int id = 0;     // プロセスID
                NativeMethods.GetWindowThreadProcessId(hwnd, out id);
                System.IntPtr process = NativeMethods.OpenProcess(ProcessAccessFlags.QueryInformation | ProcessAccessFlags.VirtualMemoryRead, false, id);
                if (process != System.IntPtr.Zero)
                {
                    System.IntPtr pmodules;
                    if (NativeMethods.EnumProcessModules(process, out pmodules, (uint)System.Runtime.InteropServices.Marshal.SizeOf(typeof(System.IntPtr)), out _))
                    {
                        System.Text.StringBuilder path = new(Common.FileNameMaxLength);     // ファイルパス
                        if (NativeMethods.GetModuleFileNameEx(process, pmodules, path, path.Capacity) != 0)
                        {
                            information.FileName = path.ToString();
                        }
                    }
                    NativeMethods.CloseHandle(process);
                }
            }
            catch
            {
            }

            return (information);
        }

        /// <summary>
        /// ウィンドウが画面内 (どれかのディスプレイ) にあるか確認
        /// </summary>
        /// <param name="windowRectangle">ウィンドウの位置とサイズ</param>
        /// <param name="monitorInformation">モニター情報 (「null」でも可)</param>
        /// <returns>画面内にあるかの値 (いいえ「false」/はい「true」)</returns>
        public static bool CheckWindowIsInTheScreen(
            RectangleDecimal windowRectangle,
            MonitorInformation monitorInformation
            )
        {
            bool result = false;        // 結果

            try
            {
                if (monitorInformation == null)
                {
                    monitorInformation = MonitorInformation.GetMonitorInformation();
                }

                foreach (MonitorInfoEx nowMonitorInfo in monitorInformation.MonitorInfo)
                {
                    RectangleDecimal intersectRectangle = RectangleDecimal.Intersect(new(nowMonitorInfo.Monitor.Left, nowMonitorInfo.Monitor.Top, nowMonitorInfo.Monitor.Right - nowMonitorInfo.Monitor.Left, nowMonitorInfo.Monitor.Bottom - nowMonitorInfo.Monitor.Top), windowRectangle);     // 交差の値
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

            return (result);
        }
    }
}
