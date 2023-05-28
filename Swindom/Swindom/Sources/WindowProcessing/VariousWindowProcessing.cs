namespace Swindom;

/// <summary>
/// 様々なウィンドウ処理
/// </summary>
public class VariousWindowProcessing
{
    /// <summary>
    /// ウィンドウ処理後のディスプレイの位置とサイズを取得
    /// </summary>
    /// <param name="rectangle">ウィンドウの上下左右の位置</param>
    /// <param name="standardDisplay">基準にするディスプレイ</param>
    /// <param name="displayName">ディスプレイ名 (指定しない「null or ""」)</param>
    /// <returns>位置とサイズ</returns>
    public static RectangleInt GetDisplayPositionAndSizeAfterProcessing(
        RectangleInt rectangle,
        StandardDisplay standardDisplay = StandardDisplay.CurrentDisplay,
        string displayName = ""
        )
    {
        if (ApplicationData.Settings.CoordinateType == CoordinateType.EachDisplay)
        {
            if (standardDisplay == StandardDisplay.SpecifiedDisplay)
            {
                foreach (MonitorInfoEx nowMonitorInfo in ApplicationData.MonitorInformation.MonitorInfo)
                {
                    if (nowMonitorInfo.DeviceName == displayName)
                    {
                        return new(nowMonitorInfo.WorkArea.Left, nowMonitorInfo.WorkArea.Top, nowMonitorInfo.WorkArea.Right, nowMonitorInfo.WorkArea.Bottom);
                    }
                }
            }
            else
            {
                MonitorInformation.GetMonitorInformationOnWindowShown(rectangle, out MonitorInfoEx monitorInfo);
                return new(monitorInfo.WorkArea.Left, monitorInfo.WorkArea.Top, monitorInfo.WorkArea.Right, monitorInfo.WorkArea.Bottom);
            }
        }
        else
        {
            return GetAllDisplayPositionSize();
        }

        return new();
    }

    /// <summary>
    /// 全てのディスプレイを合わせた上下左右の位置を取得
    /// </summary>
    /// <returns>全てのディスプレイを合わせた上下左右の位置</returns>
    private static RectangleInt GetAllDisplayPositionSize()
    {
        RectangleInt rect = new();

        foreach (MonitorInfoEx nowMonitorInfo in ApplicationData.MonitorInformation.MonitorInfo)
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
    /// <param name="doEventFullScreenWindowClose">全画面ウィンドウが閉じられた時はイベントを発生させる (発生させない「false」/発生させる「true」)</param>
    /// <returns>全画面ウィンドウがあるかの値 (いいえ「false」/はい「true」)</returns>
    public static bool CheckFullScreenWindow(
        HwndList? hwndList,
        bool doEventFullScreenWindowClose = false
        )
    {
        bool fullScreen = false;       // 全画面ウィンドウがあるかの値

        try
        {
            HwndList checkHwndList = hwndList ?? HwndList.GetWindowHandleList();        // 確認するウィンドウハンドルのリスト
            IntPtr foregroundHwnd = NativeMethods.GetForegroundWindow();        // フォアグラウンドウィンドウのハンドル

            foreach (IntPtr nowHwnd in checkHwndList.Hwnd)
            {
                NativeMethods.GetWindowRect(nowHwnd, out RECT windowRect);      // ウィンドウの上下左右の位置
                MonitorInformation.GetMonitorInformationOnWindowShown(new(windowRect.Left, windowRect.Top, windowRect.Right, windowRect.Bottom), out MonitorInfoEx monitorInfo);        // MonitorInfoEx

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
                if (ApplicationData.FullScreenExists == false)
                {
                    ApplicationData.FullScreenExists = true;
                    ApplicationData.EventData.DoProcessingEvent(ProcessingEventType.FullScreenWindowShowClose);
                }
            }
            else
            {
                if (ApplicationData.FullScreenExists && doEventFullScreenWindowClose)
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
    /// ウィンドウ処理後のウィンドウの位置とサイズを取得
    /// </summary>
    /// <param name="hwnd">ウィンドウハンドル</param>
    /// <param name="windowInformation">ウィンドウの情報</param>
    /// <param name="positionSize">位置とサイズ</param>
    /// <param name="standardDisplay">基準にするディスプレイ</param>
    /// <param name="clientArea">クライアントエリアを対象とするかの値 (いいえ「false」/はい「true」)</param>
    /// <returns>変更後のウィンドウの位置とサイズ</returns>
    public static Rect GetPositionSizeOfWindowAfterProcessing(
        IntPtr hwnd,
        WindowInformation windowInformation,
        PositionSize positionSize,
        StandardDisplay standardDisplay,
        bool clientArea
        )
    {
        // クライアントエリアを対象とする場合はウィンドウの枠のエリアのサイズを取得
        RectangleInt frameArea = new();      // ウィンドウの枠のエリア
        if (clientArea)
        {
            POINT clientAreaPosition = new();       // クライアントエリアの左上のスクリーン座標
            NativeMethods.ClientToScreen(hwnd, ref clientAreaPosition);     // クライアントエリアの位置
            NativeMethods.GetClientRect(hwnd, out RECT clientAreaRect);     // クライアントエリアの幅と高さ

            frameArea.Left = clientAreaPosition.x - windowInformation.Rectangle.Left;
            frameArea.Top = clientAreaPosition.y - windowInformation.Rectangle.Top;
            frameArea.Right = windowInformation.Rectangle.Width - clientAreaRect.Right - frameArea.Left;
            frameArea.Bottom = windowInformation.Rectangle.Height - clientAreaRect.Bottom - frameArea.Top;
        }

        RectangleInt afterProcessingDisplayRectangle = GetDisplayPositionAndSizeAfterProcessing(windowInformation.Rectangle, standardDisplay, positionSize.Display);     // 処理後のディスプレイの位置とサイズ

        // サイズを計算
        System.Windows.Size afterProcessingWindowSize = new(windowInformation.Rectangle.Width, windowInformation.Rectangle.Height);      // 処理後のウィンドウサイズ
        if (positionSize.WidthType == WindowSizeType.Value)
        {
            switch (positionSize.WidthValueType)
            {
                case PositionSizeValueType.Pixel:
                    afterProcessingWindowSize.Width = positionSize.Width + frameArea.Left + frameArea.Right;
                    break;
                case PositionSizeValueType.Percent:
                    afterProcessingWindowSize.Width = (positionSize.Width * (afterProcessingDisplayRectangle.Width / 100.0)) + frameArea.Left + frameArea.Right;
                    break;
            }
        }
        if (positionSize.HeightType == WindowSizeType.Value)
        {
            switch (positionSize.HeightValueType)
            {
                case PositionSizeValueType.Pixel:
                    afterProcessingWindowSize.Height = positionSize.Height + frameArea.Top + frameArea.Bottom;
                    break;
                case PositionSizeValueType.Percent:
                    afterProcessingWindowSize.Height = (positionSize.Height * (afterProcessingDisplayRectangle.Height / 100.0)) + frameArea.Top + frameArea.Bottom;
                    break;
            }
        }

        // 位置を計算
        RectangleInt distanceOffsetFromScreenEdge = new();      // 画面端からずらす距離
        if (ApplicationData.Settings.ShiftPastePosition.Enabled && clientArea == false)
        {
            distanceOffsetFromScreenEdge.Left = ApplicationData.Settings.ShiftPastePosition.Left;
            distanceOffsetFromScreenEdge.Top = ApplicationData.Settings.ShiftPastePosition.Top;
            distanceOffsetFromScreenEdge.Right = ApplicationData.Settings.ShiftPastePosition.Right;
            distanceOffsetFromScreenEdge.Bottom = ApplicationData.Settings.ShiftPastePosition.Bottom;
        }
        System.Windows.Point afterProcessingWindowPosition = new(windowInformation.Rectangle.Left, windowInformation.Rectangle.Top);      // 処理後のウィンドウ位置
        if (ApplicationData.Settings.CoordinateType == CoordinateType.EachDisplay)
        {
            MonitorInformation.GetMonitorInformationOnWindowShown(windowInformation.Rectangle, out MonitorInfoEx monitorInfo);       // MonitorInfoEx
            Rectangle windowIsDisplayedRectangle = new(monitorInfo.Monitor.Left, monitorInfo.Monitor.Top, monitorInfo.Monitor.Right - monitorInfo.Monitor.Left, monitorInfo.Monitor.Bottom - monitorInfo.Monitor.Top);      // ウィンドウがあるディスプレイの位置とサイズ
            switch (positionSize.XType)
            {
                case WindowXType.DoNotChange:
                    afterProcessingWindowPosition.X = afterProcessingDisplayRectangle.Left + windowInformation.Rectangle.Left - windowIsDisplayedRectangle.Left;
                    break;
                case WindowXType.Left:
                    afterProcessingWindowPosition.X = afterProcessingDisplayRectangle.Left + distanceOffsetFromScreenEdge.Left - frameArea.Left;
                    break;
                case WindowXType.Middle:
                    afterProcessingWindowPosition.X = afterProcessingDisplayRectangle.Left + afterProcessingDisplayRectangle.Width / 2 - afterProcessingWindowSize.Width / 2;
                    break;
                case WindowXType.Right:
                    afterProcessingWindowPosition.X = afterProcessingDisplayRectangle.Left + afterProcessingDisplayRectangle.Width - afterProcessingWindowSize.Width + distanceOffsetFromScreenEdge.Right + frameArea.Right;
                    break;
                case WindowXType.Value:
                    switch (positionSize.XValueType)
                    {
                        case PositionSizeValueType.Pixel:
                            afterProcessingWindowPosition.X = afterProcessingDisplayRectangle.Left + positionSize.X - frameArea.Left;
                            break;
                        case PositionSizeValueType.Percent:
                            afterProcessingWindowPosition.X = afterProcessingDisplayRectangle.Left + afterProcessingDisplayRectangle.Width / 100.0 * positionSize.X - frameArea.Left;
                            break;
                    }
                    break;
            }
            switch (positionSize.YType)
            {
                case WindowYType.DoNotChange:
                    afterProcessingWindowPosition.Y = afterProcessingDisplayRectangle.Top + windowInformation.Rectangle.Top - windowIsDisplayedRectangle.Top;
                    break;
                case WindowYType.Top:
                    afterProcessingWindowPosition.Y = afterProcessingDisplayRectangle.Top + distanceOffsetFromScreenEdge.Top - frameArea.Top;
                    break;
                case WindowYType.Middle:
                    afterProcessingWindowPosition.Y = afterProcessingDisplayRectangle.Top + afterProcessingDisplayRectangle.Height / 2 - afterProcessingWindowSize.Height / 2;
                    break;
                case WindowYType.Bottom:
                    afterProcessingWindowPosition.Y = afterProcessingDisplayRectangle.Top + afterProcessingDisplayRectangle.Height - afterProcessingWindowSize.Height + distanceOffsetFromScreenEdge.Bottom + frameArea.Bottom;
                    break;
                case WindowYType.Value:
                    switch (positionSize.YValueType)
                    {
                        case PositionSizeValueType.Pixel:
                            afterProcessingWindowPosition.Y = afterProcessingDisplayRectangle.Top + positionSize.Y - frameArea.Top;
                            break;
                        case PositionSizeValueType.Percent:
                            afterProcessingWindowPosition.Y = afterProcessingDisplayRectangle.Top + afterProcessingDisplayRectangle.Height / 100.0 * positionSize.Y - frameArea.Top;
                            break;
                    }
                    break;
            }
        }
        else
        {
            switch (positionSize.XType)
            {
                case WindowXType.DoNotChange:
                    afterProcessingWindowPosition.X = windowInformation.Rectangle.Left;
                    break;
                case WindowXType.Left:
                    afterProcessingWindowPosition.X = afterProcessingDisplayRectangle.Left - frameArea.Left;
                    break;
                case WindowXType.Middle:
                    afterProcessingWindowPosition.X = afterProcessingDisplayRectangle.Left + afterProcessingDisplayRectangle.Width / 2 - afterProcessingWindowSize.Width / 2;
                    break;
                case WindowXType.Right:
                    afterProcessingWindowPosition.X = afterProcessingDisplayRectangle.Right - afterProcessingWindowSize.Width + distanceOffsetFromScreenEdge.Right + frameArea.Right;
                    break;
                case WindowXType.Value:
                    switch (positionSize.XValueType)
                    {
                        case PositionSizeValueType.Pixel:
                            afterProcessingWindowPosition.X = positionSize.X - frameArea.Left;
                            break;
                        case PositionSizeValueType.Percent:
                            afterProcessingWindowPosition.X = afterProcessingDisplayRectangle.Left + afterProcessingDisplayRectangle.Width / 100.0 * positionSize.X - frameArea.Left;
                            break;
                    }
                    break;
            }
            switch (positionSize.YType)
            {
                case WindowYType.DoNotChange:
                    afterProcessingWindowPosition.Y = windowInformation.Rectangle.Top;
                    break;
                case WindowYType.Top:
                    afterProcessingWindowPosition.Y = afterProcessingDisplayRectangle.Top - frameArea.Top;
                    break;
                case WindowYType.Middle:
                    afterProcessingWindowPosition.Y = afterProcessingDisplayRectangle.Top + afterProcessingDisplayRectangle.Height / 2 - afterProcessingWindowSize.Height / 2;
                    break;
                case WindowYType.Bottom:
                    afterProcessingWindowPosition.Y = afterProcessingDisplayRectangle.Top + afterProcessingDisplayRectangle.Height - afterProcessingWindowSize.Height + distanceOffsetFromScreenEdge.Bottom - frameArea.Bottom;
                    break;
                case WindowYType.Value:
                    switch (positionSize.YValueType)
                    {
                        case PositionSizeValueType.Pixel:
                            afterProcessingWindowPosition.Y = positionSize.Y - frameArea.Top;
                            break;
                        case PositionSizeValueType.Percent:
                            afterProcessingWindowPosition.Y = afterProcessingDisplayRectangle.Top + afterProcessingDisplayRectangle.Height / 100.0 * positionSize.Y - frameArea.Top;
                            break;
                    }
                    break;
            }
        }

        return new((int)afterProcessingWindowPosition.X, (int)afterProcessingWindowPosition.Y, (int)afterProcessingWindowSize.Width, (int)afterProcessingWindowSize.Height);
    }

    /// <summary>
    /// ウィンドウハンドルからウィンドウの情報を取得
    /// </summary>
    /// <param name="hwnd">ウィンドウハンドル</param>
    /// <param name="wib">ウィンドウ情報のバッファ (使用中の場合は使用されない)</param>
    /// <returns>ウィンドウの情報</returns>
    public static WindowInformation GetWindowInformationFromHandle(
        IntPtr hwnd,
        WindowInformationBuffer wib
        )
    {
        WindowInformation information = new();      // ウィンドウの情報
        bool checkProcessing = wib.InProcess;

        wib.InProcess = true;

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
                if (wib.TitleName.Capacity < length)
                {
                    wib.TitleName = new(length);
                }
                else
                {
                    wib.TitleName.Clear();
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
            _ = NativeMethods.GetWindowThreadProcessId(hwnd, out int id);       // プロセスID
            IntPtr process = NativeMethods.OpenProcess(ProcessAccessFlags.QueryInformation | ProcessAccessFlags.VirtualMemoryRead, false, id);
            if (process != IntPtr.Zero)
            {
                if (NativeMethods.EnumProcessModules(process, out IntPtr pmodules, (uint)Marshal.SizeOf(typeof(IntPtr)), out _))
                {
                    StringBuilder getString;
                    if (checkProcessing)
                    {
                        getString = new(Common.PathMaxLength);
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

        // ウィンドウの上下左右の位置とウィンドウの状態を取得
        try
        {
            NativeMethods.GetWindowPlacement(hwnd, out WINDOWPLACEMENT windowPlacement);
            information.Rectangle.Left = windowPlacement.rcNormalPosition.Left;
            information.Rectangle.Top = windowPlacement.rcNormalPosition.Top;
            information.Rectangle.Right = windowPlacement.rcNormalPosition.Right;
            information.Rectangle.Bottom = windowPlacement.rcNormalPosition.Bottom;
            information.ShowCmd = windowPlacement.showCmd;
        }
        catch
        {
        }

        // バージョンを取得
        try
        {
            if (string.IsNullOrEmpty(information.FileName) == false)
            {
                FileVersionInfo info = FileVersionInfo.GetVersionInfo(information.FileName);
                if (info.ProductVersion != null)
                {
                    information.Version = info.ProductVersion;
                }
            }
        }
        catch
        {
        }

        wib.InProcess = false;

        return information;
    }

    /// <summary>
    /// ウィンドウハンドルからウィンドウの上下左右の位置とウィンドウの状態の情報を取得
    /// </summary>
    /// <param name="hwnd">ウィンドウハンドル</param>
    /// <returns>ウィンドウの情報</returns>
    public static WindowInformation GetWindowInformationPositionSizeFromHandle(
        IntPtr hwnd
        )
    {
        WindowInformation information = new();      // ウィンドウの情報

        try
        {
            NativeMethods.GetWindowPlacement(hwnd, out WINDOWPLACEMENT windowPlacement);
            information.Rectangle.Left = windowPlacement.rcNormalPosition.Left;
            information.Rectangle.Top = windowPlacement.rcNormalPosition.Top;
            information.Rectangle.Right = windowPlacement.rcNormalPosition.Right;
            information.Rectangle.Bottom = windowPlacement.rcNormalPosition.Bottom;
            information.ShowCmd = windowPlacement.showCmd;
        }
        catch
        {
        }

        return information;
    }

    /// <summary>
    /// ウィンドウが画面内 (どれかのディスプレイ) にあるか確認
    /// </summary>
    /// <param name="windowRectangle">ウィンドウの位置とサイズ</param>
    /// <returns>画面内にあるかの値 (いいえ「false」/はい「true」)</returns>
    public static bool CheckWindowIsInTheScreen(
        Rect windowRectangle
        )
    {
        try
        {
            foreach (MonitorInfoEx nowMonitorInfo in ApplicationData.MonitorInformation.MonitorInfo)
            {
                Rect intersectRectangle = Rect.Intersect(new(nowMonitorInfo.WorkArea.Left, nowMonitorInfo.WorkArea.Top, nowMonitorInfo.WorkArea.Right - nowMonitorInfo.WorkArea.Left, nowMonitorInfo.WorkArea.Bottom - nowMonitorInfo.WorkArea.Top), windowRectangle);     // 交差の値
                if (intersectRectangle.IsEmpty == false)
                {
                    return true;
                }
            }
        }
        catch
        {
        }

        return false;
    }

    /// <summary>
    /// 画面外に存在するウィンドウを画面内に移動
    /// </summary>
    public static void MoveWindowIntoScreen()
    {
        if (ApplicationData.MonitorInformation.MonitorInfo.Count != 0)
        {
            HwndList hwndList = HwndList.GetWindowHandleList();

            foreach (IntPtr nowHwnd in hwndList.Hwnd)
            {
                NativeMethods.GetWindowPlacement(nowHwnd, out WINDOWPLACEMENT windowPlacement);
                Rect windowRectangle = new(windowPlacement.rcNormalPosition.Left, windowPlacement.rcNormalPosition.Top, windowPlacement.rcNormalPosition.Right - windowPlacement.rcNormalPosition.Left, windowPlacement.rcNormalPosition.Bottom - windowPlacement.rcNormalPosition.Top);

                if (CheckWindowIsInTheScreen(windowRectangle) == false)
                {
                    if (windowPlacement.showCmd != (int)SW.SW_SHOWNORMAL)
                    {
                        NativeMethods.ShowWindow(nowHwnd, (int)SW.SW_SHOWNORMAL);
                    }

                    NativeMethods.SetWindowPos(nowHwnd, (int)HwndInsertAfter.HWND_NOTOPMOST, ApplicationData.MonitorInformation.MonitorInfo[0].WorkArea.Left, ApplicationData.MonitorInformation.MonitorInfo[0].WorkArea.Top, 0, 0, (int)SWP.SWP_NOSIZE | (int)SWP.SWP_NOZORDER);
                }
            }
        }
    }
}
