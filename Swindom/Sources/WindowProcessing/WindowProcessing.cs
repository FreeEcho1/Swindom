namespace Swindom;

/// <summary>
/// ウィンドウ処理
/// </summary>
public class WindowProcessing
{
    /// <summary>
    /// ウィンドウ処理後のウィンドウが表示されるディスプレイの位置とサイズを取得
    /// </summary>
    /// <param name="windowRectangle">ウィンドウの位置とサイズ</param>
    /// <param name="standardDisplay">基準にするディスプレイ</param>
    /// <param name="displayName">ディスプレイ名 (指定なし「null or ""」)</param>
    /// <returns>ウィンドウ処理後のディスプレイの位置とサイズ</returns>
    public static RectangleInt GetDisplayPositionAndSizeAfterProcessing(
        RectangleInt windowRectangle,
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
                        return new(nowMonitorInfo.WorkArea.Left, nowMonitorInfo.WorkArea.Top, nowMonitorInfo.WorkArea.Right - nowMonitorInfo.WorkArea.Left, nowMonitorInfo.WorkArea.Bottom - nowMonitorInfo.WorkArea.Top);
                    }
                }
            }
            else
            {
                MonitorInformation.GetMonitorInformationForSpecifiedArea(windowRectangle, out MonitorInfoEx monitorInfo);
                return new(monitorInfo.WorkArea.Left, monitorInfo.WorkArea.Top, monitorInfo.WorkArea.Right - monitorInfo.WorkArea.Left, monitorInfo.WorkArea.Bottom - monitorInfo.WorkArea.Top);
            }
        }
        else
        {
            return GetAllDisplayPositionSize();
        }

        return new();
    }

    /// <summary>
    /// 全てのディスプレイを合わせた位置とサイズを取得
    /// </summary>
    /// <returns>全てのディスプレイを合わせた位置とサイズ</returns>
    private static RectangleInt GetAllDisplayPositionSize()
    {
        int left = 0, top = 0, right = 0, bottom = 0;

        foreach (MonitorInfoEx nowMonitorInfo in ApplicationData.MonitorInformation.MonitorInfo)
        {
            if (nowMonitorInfo.WorkArea.Left < left)
            {
                left = nowMonitorInfo.WorkArea.Left;
            }
            if (nowMonitorInfo.WorkArea.Top < top)
            {
                top = nowMonitorInfo.WorkArea.Top;
            }
            if (right < nowMonitorInfo.WorkArea.Right)
            {
                right = nowMonitorInfo.WorkArea.Right;
            }
            if (bottom < nowMonitorInfo.WorkArea.Bottom)
            {
                bottom = nowMonitorInfo.WorkArea.Bottom;
            }
        }

        return new(left, top, right - left, bottom - top);
    }

    /// <summary>
    /// ウィンドウ処理後の位置とサイズを取得
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

        RectangleInt afterProcessingDisplayRectangle = GetDisplayPositionAndSizeAfterProcessing(windowInformation.Rectangle, standardDisplay, positionSize.Display);     // ウィンドウ処理後のウィンドウがあるディスプレイの位置とサイズ
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
        if (ApplicationData.Settings.ShiftPastePosition.IsEnabled && clientArea == false)
        {
            distanceOffsetFromScreenEdge.Left = ApplicationData.Settings.ShiftPastePosition.Left;
            distanceOffsetFromScreenEdge.Top = ApplicationData.Settings.ShiftPastePosition.Top;
            distanceOffsetFromScreenEdge.Right = ApplicationData.Settings.ShiftPastePosition.Right;
            distanceOffsetFromScreenEdge.Bottom = ApplicationData.Settings.ShiftPastePosition.Bottom;
        }
        System.Windows.Point afterProcessingWindowPosition = new(windowInformation.Rectangle.Left, windowInformation.Rectangle.Top);      // 処理後のウィンドウ位置
        if (ApplicationData.Settings.CoordinateType == CoordinateType.EachDisplay)
        {
            MonitorInformation.GetMonitorInformationForSpecifiedArea(windowInformation.Rectangle, out MonitorInfoEx monitorInfo);       // MonitorInfoEx
            RectangleInt windowIsDisplayedRectangle = new(monitorInfo.Monitor.Left, monitorInfo.Monitor.Top, monitorInfo.Monitor.Right - monitorInfo.Monitor.Left, monitorInfo.Monitor.Bottom - monitorInfo.Monitor.Top);      // ウィンドウがあるディスプレイの位置とサイズ
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
    /// <returns>ウィンドウの情報</returns>
    public static WindowInformation GetWindowInformationFromHandle(
        IntPtr hwnd
        )
    {
        WindowInformation information = GetWindowRectangleAndStateFromHandle(hwnd);      // ウィンドウの情報

        // タイトル名取得
        try
        {
            int length = NativeMethods.GetWindowTextLength(hwnd) + 1;
            StringBuilder getString = new(length);

            _ = NativeMethods.GetWindowText(hwnd, getString, getString.Capacity);
            information.TitleName = getString.ToString();
        }
        catch
        {
        }

        // クラス名取得
        try
        {
            StringBuilder getString = new(WindowProcessingValue.ClassNameMaxLength);

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
                    StringBuilder getString = new(WindowProcessingValue.PathMaxLength);

                    _ = NativeMethods.GetModuleFileNameEx(process, pmodules, getString, getString.Capacity);
                    information.FileName = getString.ToString();
                }
                NativeMethods.CloseHandle(process);
            }
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

        return information;
    }

    /// <summary>
    /// 除外するウィンドウかを確認
    /// </summary>
    /// <param name="windowInformation">ウィンドウの情報</param>
    /// <returns>除外するウィンドウかの値 (除外しないウィンドウ「false」/除外するウィンドウ「true」)</returns>
    public static bool CheckExclusionProcessing(
        WindowInformation windowInformation
        )
    {
        // 「explorer.exe」のウィンドウ以外は除外
        if (Path.GetFileNameWithoutExtension(windowInformation.FileName).ToLower() == "explorer"
            && (windowInformation.ClassName != "CabinetWClass" && windowInformation.ClassName != "ExploreWClass"))
        {
            return true;
        }

        // ポップアップなどは除外
        switch (windowInformation.ClassName)
        {
            case "Microsoft.UI.Content.PopupWindowSiteBridge":
                return true;
            case "#32768":
                return true;
            case "XamlExplorerHostIslandWindow":
                return true;
            case "Message":
                return true;
        }

        return false;
    }

    /// <summary>
    /// ウィンドウハンドルからウィンドウの位置とサイズと状態を取得
    /// </summary>
    /// <param name="hwnd">ウィンドウハンドル</param>
    /// <returns>ウィンドウの情報</returns>
    public static WindowInformation GetWindowRectangleAndStateFromHandle(
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
    /// ウィンドウが画面内に入っているか確認
    /// </summary>
    /// <param name="windowRectangle">ウィンドウの位置とサイズ</param>
    /// <returns>画面内に入っているかの値 (いいえ「false」/はい「true」)</returns>
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

                if (CheckWindowIsInTheScreen(new(windowPlacement.rcNormalPosition.Left, windowPlacement.rcNormalPosition.Top, windowPlacement.rcNormalPosition.Right - windowPlacement.rcNormalPosition.Left, windowPlacement.rcNormalPosition.Bottom - windowPlacement.rcNormalPosition.Top)) == false)
                {
                    // 不要だけど念のため
                    if (windowPlacement.showCmd != (int)SW.SW_SHOWNORMAL)
                    {
                        NativeMethods.ShowWindow(nowHwnd, (int)SW.SW_SHOWNORMAL);
                    }

                    NativeMethods.SetWindowPos(nowHwnd, (int)HwndInsertAfter.HWND_NOTOPMOST, ApplicationData.MonitorInformation.MonitorInfo[0].WorkArea.Left, ApplicationData.MonitorInformation.MonitorInfo[0].WorkArea.Top, 0, 0, (int)SWP.SWP_NOSIZE | (int)SWP.SWP_NOZORDER);
                }
            }
        }
    }

    /// <summary>
    /// 全ての設定で、設定されているディスプレイが存在するかを確認
    /// </summary>
    /// <param name="newMonitorInformation">新しいモニター情報</param>
    /// <returns>全て存在するかの値 (「false」存在しない設定がある/「true」全て存在する)</returns>
    public static bool CheckSettingDisplaysExist(
        MonitorInformation newMonitorInformation
        )
    {
        bool check = true;       // 全ての設定でディスプレイが存在するかの値

        if (SpecifyWindowProcessing.CheckSettingDisplaysExist(newMonitorInformation) == false)
        {
            check = false;
        }
        if (AllWindowProcessing.CheckSettingDisplaysExist(newMonitorInformation) == false)
        {
            check = false;
        }
        if (HotkeyProcessing.CheckSettingDisplaysExist(newMonitorInformation) == false)
        {
            check = false;
        }

        return check;
    }

    /// <summary>
    /// ディスプレイの設定を変更
    /// </summary>
    public static void ChangeDisplaySettings()
    {
        try
        {
            // ディスプレイの設定を変更しない
            if (ApplicationData.Settings.DoNotChangeDisplaySettings)
            {
                return;
            }
            if (ApplicationData.MonitorInformation.MonitorInfo.Count <= 0)
            {
                return;
            }

            bool changed = false;       // 設定が変更されたかの値
            ChangeDisplaySettingsData changeDisplaySettingsData = new();

            if (ApplicationData.Settings.DisplayChangeMode == DisplayChangeMode.Auto
                || (ApplicationData.Settings.DisplayChangeMode == DisplayChangeMode.AutoOrManual
                && ApplicationData.MonitorInformation.MonitorInfo.Count == 1))
            {
                changeDisplaySettingsData.IsModified = true;
                changeDisplaySettingsData.ApplySameSettingToRemaining = true;
                changeDisplaySettingsData.AutoSettingDisplayName = ApplicationData.MonitorInformation.MonitorInfo[0].DeviceName;
            }

            if (SpecifyWindowProcessing.ChangeDisplaySettings(changeDisplaySettingsData))
            {
                changed = true;
            }
            if (AllWindowProcessing.ChangeDisplaySettings(changeDisplaySettingsData))
            {
                changed = true;
            }
            if (HotkeyProcessing.ChangeDisplaySettings(changeDisplaySettingsData))
            {
                changed = true;
            }

            // 設定が変更されたら保存する
            if (changed)
            {
                SettingFileProcessing.WriteSettings();
            }
        }
        catch
        {
        }
    }
}
