﻿namespace Swindom;

/// <summary>
/// 指定ウィンドウ (イベント、タイマー)
/// </summary>
public static class SpecifiedWindow
{
    /// <summary>
    /// 処理するウィンドウか確認
    /// </summary>
    /// <param name="caseSensitiveWindowQueries">ウィンドウ判定で大文字と小文字を区別するかの値</param>
    /// <param name="specifiedWindowBaseItemInformation">「指定ウィンドウ」機能の基礎の項目情報</param>
    /// <param name="windowInformation">ウィンドウの情報</param>
    /// <param name="windowPlacement">WINDOWPLACEMENT</param>
    /// <returns>処理するウィンドウかの値 (いいえ「false」/はい「true」)</returns>
    public static bool CheckWindowToProcessing(
        bool caseSensitiveWindowQueries,
        SpecifiedWindowBaseItemInformation specifiedWindowBaseItemInformation,
        WindowInformation windowInformation,
        in WINDOWPLACEMENT windowPlacement
        )
    {
        bool result = false;     // 結果

        try
        {
            // 処理しないタイトル名判定
            switch (specifiedWindowBaseItemInformation.TitleProcessingConditions)
            {
                case TitleProcessingConditions.DoNotProcessingUntitledWindow:
                    if (string.IsNullOrEmpty(windowInformation.TitleName))
                    {
                        return false;
                    }
                    break;
                case TitleProcessingConditions.DoNotProcessingWindowWithTitle:
                    if (string.IsNullOrEmpty(windowInformation.TitleName) == false)
                    {
                        return false;
                    }
                    break;
            }

            string checkString;      // 確認する文字列
            string windowString;       // ウィンドウ文字列

            // タイトル名判定
            if (string.IsNullOrEmpty(windowInformation.TitleName) == false)
            {
                if (caseSensitiveWindowQueries)
                {
                    checkString = specifiedWindowBaseItemInformation.TitleName;
                    windowString = windowInformation.TitleName;
                }
                else
                {
                    checkString = specifiedWindowBaseItemInformation.TitleName.ToLower();
                    windowString = windowInformation.TitleName.ToLower();
                }

                if (string.IsNullOrEmpty(checkString) == false)
                {
                    switch (specifiedWindowBaseItemInformation.TitleNameMatchCondition)
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

                // 指定の文字列を含んでいる場合は処理しない
                foreach (string nowTitleName in specifiedWindowBaseItemInformation.DoNotProcessingTitleName)
                {
                    if (windowString.Contains(nowTitleName))
                    {
                        return false;
                    }
                }
            }

            // クラス名判定
            if (string.IsNullOrEmpty(specifiedWindowBaseItemInformation.ClassName) == false)
            {
                // 取得に失敗した場合は処理しない
                if (string.IsNullOrEmpty(windowInformation.ClassName))
                {
                    return false;
                }

                if (caseSensitiveWindowQueries)
                {
                    checkString = specifiedWindowBaseItemInformation.ClassName;
                    windowString = windowInformation.ClassName;
                }
                else
                {
                    checkString = specifiedWindowBaseItemInformation.ClassName.ToLower();
                    windowString = windowInformation.ClassName.ToLower();
                }

                switch (specifiedWindowBaseItemInformation.ClassNameMatchCondition)
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
            if (string.IsNullOrEmpty(specifiedWindowBaseItemInformation.FileName) == false)
            {
                // 取得に失敗した場合は処理しない
                if (string.IsNullOrEmpty(windowInformation.FileName))
                {
                    return false;
                }

                if (caseSensitiveWindowQueries)
                {
                    checkString = specifiedWindowBaseItemInformation.FileName;
                    windowString = windowInformation.FileName;
                }
                else
                {
                    checkString = specifiedWindowBaseItemInformation.FileName.ToLower();
                    windowString = windowInformation.FileName.ToLower();
                }

                switch (specifiedWindowBaseItemInformation.FileNameMatchCondition)
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

            // 処理しない条件のサイズ判定
            if (specifiedWindowBaseItemInformation.DoNotProcessingSize.Count != 0)
            {
                System.Windows.Size windowSize = new()
                {
                    Width = windowPlacement.rcNormalPosition.Right - windowPlacement.rcNormalPosition.Left,
                    Height = windowPlacement.rcNormalPosition.Bottom - windowPlacement.rcNormalPosition.Top
                };      // ウィンドウサイズ
                foreach (System.Drawing.Size nowSize in specifiedWindowBaseItemInformation.DoNotProcessingSize)
                {
                    if (windowSize.Width == nowSize.Width && windowSize.Height == nowSize.Height)
                    {
                        return false;
                    }
                }
            }

            // バージョン判定
            if (string.IsNullOrEmpty(specifiedWindowBaseItemInformation.DifferentVersionVersion) == false)
            {
                if (specifiedWindowBaseItemInformation.DifferentVersionVersion != windowInformation.Version)
                {
                    if (specifiedWindowBaseItemInformation.DifferentVersionAnnounce)
                    {
                        FreeEcho.FEControls.MessageBoxResult messageBoxResult = FreeEcho.FEControls.MessageBox.Show(Common.ApplicationData.Languages.ChangeVersion + Common.CopySeparateString + Common.ApplicationName, specifiedWindowBaseItemInformation.RegisteredName + Environment.NewLine + Common.ApplicationData.Languages.DifferentVersionSettingMessage, FreeEcho.FEControls.MessageBoxButton.YesNo);
                        if (messageBoxResult == FreeEcho.FEControls.MessageBoxResult.Yes)
                        {
                            specifiedWindowBaseItemInformation.DifferentVersionVersion = windowInformation.Version;
                        }
                        else
                        {
                            specifiedWindowBaseItemInformation.Enabled = false;
                        }
                        SettingFileProcessing.WriteSettings();
                    }
                    return false;
                }
            }

            result = true;
        }
        catch
        {
        }

        return result;
    }

    /// <summary>
    /// 処理するディスプレイか確認
    /// </summary>
    /// <param name="specifiedWindowBaseItemInformation">「指定ウィンドウ」機能の基礎の項目情報</param>
    /// <param name="windowProcessingInformation">ウィンドウの処理情報</param>
    /// <param name="hwnd">ウィンドウハンドル</param>
    /// <returns>処理するディスプレイかの値 (処理しない「false」/処理する「true」)</returns>
    public static bool CheckDisplayToProcessing(
        SpecifiedWindowBaseItemInformation specifiedWindowBaseItemInformation,
        WindowProcessingInformation windowProcessingInformation,
        IntPtr hwnd
        )
    {
        bool result = true;        // 結果

        if (Common.ApplicationData.Settings.CoordinateType == CoordinateType.Display
            && specifiedWindowBaseItemInformation.StandardDisplay == StandardDisplay.OnlySpecifiedDisplay)
        {
            MonitorInfoEx monitorInfo;
            MonitorInformation.GetMonitorWithWindow(hwnd, out monitorInfo);
            if (windowProcessingInformation.PositionSize.Display != monitorInfo.DeviceName)
            {
                result = false;
            }
        }

        return result;
    }

    /// <summary>
    /// ウィンドウを処理
    /// </summary>
    /// <param name="specifiedWindowBaseItemInformation">「指定ウィンドウ」機能の基礎の項目情報</param>
    /// <param name="windowProcessingInformation">ウィンドウの処理情報</param>
    /// <param name="hwnd">ウィンドウハンドル</param>
    /// <param name="windowPlacement">WINDOWPLACEMENT</param>
    /// <param name="hotkey">ホットキー処理かの値 (いいえ「false」/はい「true」)</param>
    /// <param name="doNotChangeOutOfScreen">画面外に出る場合は位置やサイズを変更しない (無効「false」/有効「true」)</param>
    public static void ProcessingWindow(
        SpecifiedWindowBaseItemInformation specifiedWindowBaseItemInformation,
        WindowProcessingInformation windowProcessingInformation,
        IntPtr hwnd,
        in WINDOWPLACEMENT windowPlacement,
        bool hotkey,
        bool doNotChangeOutOfScreen
        )
    {
        try
        {
            // 1回目の処理
            MonitorInfoEx monitorInfo1;
            MonitorInformation.GetMonitorWithWindow(hwnd, out monitorInfo1);
            ProcessingWindowPositionAndSize(specifiedWindowBaseItemInformation, windowProcessingInformation, hwnd, windowPlacement, hotkey, doNotChangeOutOfScreen);
            // 2回目の処理
            MonitorInfoEx monitorInfo2;
            MonitorInformation.GetMonitorWithWindow(hwnd, out monitorInfo2);
            if (windowProcessingInformation.PositionSize.ProcessingPositionAndSizeTwice
                || monitorInfo1.DeviceName != monitorInfo2.DeviceName
                || windowProcessingInformation.PositionSize.ClientArea)
            {
                WINDOWPLACEMENT secondWindowPlacement;       // WINDOWPLACEMENT
                NativeMethods.GetWindowPlacement(hwnd, out secondWindowPlacement);
                ProcessingWindowPositionAndSize(specifiedWindowBaseItemInformation, windowProcessingInformation, hwnd, secondWindowPlacement, hotkey, doNotChangeOutOfScreen, true);
            }

            if (windowProcessingInformation.EnabledTransparency)
            {
                if ((NativeMethods.GetWindowLongPtr(hwnd, (int)GWL.GWL_EXSTYLE) & (int)WS_EX.WS_EX_LAYERED) != (int)WS_EX.WS_EX_LAYERED)
                {
                    NativeMethods.SetWindowLongPtr(hwnd, (int)GWL.GWL_EXSTYLE, (int)(NativeMethods.GetWindowLongPtr(hwnd, (int)GWL.GWL_EXSTYLE) ^ (int)WS_EX.WS_EX_LAYERED));
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
    /// <param name="specifiedWindowBaseItemInformation">「指定ウィンドウ」機能の基礎の項目情報</param>
    /// <param name="windowProcessingInformation">ウィンドウの処理情報</param>
    /// <param name="hwnd">ウィンドウハンドル</param>
    /// <param name="windowPlacement">WINDOWPLACEMENT</param>
    /// <param name="hotkey">ホットキー処理かの値 (いいえ「false」/はい「true」)</param>
    /// <param name="doNotChangeOutOfScreen">画面外に出る場合は位置やサイズを変更しない (無効「false」/有効「true」)</param>
    /// <param name="second">2回目の処理かの値 (いいえ「false」/はい「true」)</param>
    public static void ProcessingWindowPositionAndSize(
        SpecifiedWindowBaseItemInformation specifiedWindowBaseItemInformation,
        WindowProcessingInformation windowProcessingInformation,
        IntPtr hwnd,
        in WINDOWPLACEMENT windowPlacement,
        bool hotkey,
        bool doNotChangeOutOfScreen,
        bool second = false
        )
    {
        try
        {
            // 2回目は処理しない
            if (second)
            {
                switch (windowProcessingInformation.SettingsWindowState)
                {
                    case SettingsWindowState.Maximize:
                    case SettingsWindowState.Minimize:
                        return;
                }
            }

            bool doNormal = false;     // 「通常のウィンドウ」にするかの値
            MonitorInfoEx monitorInfo;     // モニター情報
            MonitorInformation.GetMonitorWithWindow(hwnd, out monitorInfo, Common.MonitorInformation);
            RectangleDecimal changedDisplayRectangle = WindowProcessing.GetDisplayPositionAndSizeAfterProcessing(Common.ApplicationData.Settings.CoordinateType, hwnd, specifiedWindowBaseItemInformation.StandardDisplay, windowProcessingInformation.PositionSize.Display, hotkey);        // 処理後のディスプレイの位置とサイズ

            // 位置やサイズを計算
            RectangleDecimal windowRectangle = new(windowPlacement.rcNormalPosition.Left, windowPlacement.rcNormalPosition.Top, windowPlacement.rcNormalPosition.Right - windowPlacement.rcNormalPosition.Left, windowPlacement.rcNormalPosition.Bottom - windowPlacement.rcNormalPosition.Top);     // ウィンドウの位置とサイズ
            switch (windowProcessingInformation.SettingsWindowState)
            {
                case SettingsWindowState.DoNotChange:
                    if (Common.ApplicationData.Settings.CoordinateType == CoordinateType.Display
                        && specifiedWindowBaseItemInformation.StandardDisplay == StandardDisplay.SpecifiedDisplay
                        && windowProcessingInformation.PositionSize.Display != monitorInfo.DeviceName)
                    {
                        windowRectangle.X = changedDisplayRectangle.X + windowPlacement.rcNormalPosition.Right - windowPlacement.rcNormalPosition.Left;
                        windowRectangle.Y = changedDisplayRectangle.Y + windowPlacement.rcNormalPosition.Bottom - windowPlacement.rcNormalPosition.Top;
                    }
                    break;
                case SettingsWindowState.Normal:
                    windowRectangle = WindowProcessing.CalculatePositionAndSize(hwnd, windowPlacement, windowProcessingInformation.PositionSize, specifiedWindowBaseItemInformation.StandardDisplay, windowProcessingInformation.PositionSize.ClientArea);
                    doNormal = true;
                    break;
                case SettingsWindowState.Maximize:
                case SettingsWindowState.Minimize:
                    if (Common.ApplicationData.Settings.CoordinateType == CoordinateType.Display
                        && specifiedWindowBaseItemInformation.StandardDisplay == StandardDisplay.SpecifiedDisplay
                        && windowProcessingInformation.PositionSize.Display != monitorInfo.DeviceName)
                    {
                        windowRectangle.X = changedDisplayRectangle.X;
                        windowRectangle.Y = changedDisplayRectangle.Y;
                    }
                    break;
            }

            bool doSetWindowPos = false;        // SetWindowPosを処理するかの値

            // 位置やサイズを変更する場合は処理を有効にする
            if (windowRectangle.X != windowPlacement.rcNormalPosition.Left
                || windowRectangle.Y != windowPlacement.rcNormalPosition.Top
                || windowRectangle.Width != windowPlacement.rcNormalPosition.Right - windowPlacement.rcNormalPosition.Left
                || windowRectangle.Height != windowPlacement.rcNormalPosition.Bottom - windowPlacement.rcNormalPosition.Top)
            {
                doSetWindowPos = true;
                doNormal = true;
            }

            int flag = (int)SWP.SWP_NOACTIVATE;     // フラグ
            int hwndInsertAfter = (int)HwndInsertAfter.HWND_NOTOPMOST;     // ウィンドウの順番

            // 最前面の処理を決める
            if (second == false)
            {
                switch (windowProcessingInformation.Forefront)
                {
                    case Forefront.Cancel:
                        if ((NativeMethods.GetWindowLongPtr(hwnd, (int)GWL.GWL_EXSTYLE) & (int)WS_EX.WS_EX_TOPMOST) == (int)WS_EX.WS_EX_TOPMOST)
                        {
                            hwndInsertAfter = (int)HwndInsertAfter.HWND_NOTOPMOST;
                            doSetWindowPos = true;
                        }
                        break;
                    case Forefront.Always:
                        if ((NativeMethods.GetWindowLongPtr(hwnd, (int)GWL.GWL_EXSTYLE) & (int)WS_EX.WS_EX_TOPMOST) != (int)WS_EX.WS_EX_TOPMOST)
                        {
                            hwndInsertAfter = (int)HwndInsertAfter.HWND_TOPMOST;
                            doSetWindowPos = true;
                        }
                        break;
                    case Forefront.Forefront:
                        NativeMethods.SetForegroundWindow(hwnd);
                        break;
                    default:
                        flag |= (int)SWP.SWP_NOZORDER;
                        break;
                }
            }

            // ウィンドウを処理
            // 位置やサイズを変更できるようにウィンドウの状態を「通常のウィンドウ」にする
            if (doNormal && windowPlacement.showCmd != (int)SW.SW_SHOWNORMAL)
            {
                NativeMethods.ShowWindow(hwnd, (int)SW.SW_SHOWNORMAL);
            }
            if (doSetWindowPos)
            {
                if (doNotChangeOutOfScreen && WindowProcessing.CheckWindowIsInTheScreen(windowRectangle) == false)
                {
                    flag |= (int)SWP.SWP_NOMOVE | (int)SWP.SWP_NOSIZE;
                }
                NativeMethods.SetWindowPos(hwnd, hwndInsertAfter, (int)windowRectangle.X, (int)windowRectangle.Y, (int)windowRectangle.Width, (int)windowRectangle.Height, flag);
            }
            switch (windowProcessingInformation.SettingsWindowState)
            {
                case SettingsWindowState.Maximize:
                    if (doNormal || windowPlacement.showCmd != (int)SW.SW_SHOWMAXIMIZED)
                    {
                        NativeMethods.ShowWindow(hwnd, (int)SW.SW_SHOWMAXIMIZED);
                    }
                    break;
                case SettingsWindowState.Minimize:
                    if (doNormal || windowPlacement.showCmd != (int)SW.SW_MINIMIZE)
                    {
                        NativeMethods.ShowWindow(hwnd, (int)SW.SW_MINIMIZE);
                    }
                    break;
            }
        }
        catch
        {
        }
    }
}
