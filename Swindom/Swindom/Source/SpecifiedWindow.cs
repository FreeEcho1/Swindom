using Swindom.Source.Settings;

namespace Swindom.Source
{
    /// <summary>
    /// 指定ウィンドウ
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
                // タイトル名の状態によっては処理しない
                switch (specifiedWindowBaseItemInformation.TitleProcessingConditions)
                {
                    case TitleProcessingConditions.DoNotProcessingUntitledWindow:
                        if (string.IsNullOrEmpty(windowInformation.TitleName))
                        {
                            return (false);
                        }
                        break;
                    case TitleProcessingConditions.DoNotProcessingWindowWithTitle:
                        if (string.IsNullOrEmpty(windowInformation.TitleName) == false)
                        {
                            return (false);
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
                                    return (false);
                                }
                                break;
                            case NameMatchCondition.ForwardMatch:
                                if ((windowString.Length < checkString.Length) || (windowString.StartsWith(checkString) == false))
                                {
                                    return (false);
                                }
                                break;
                            case NameMatchCondition.BackwardMatch:
                                if ((windowString.Length < checkString.Length) || (windowString.EndsWith(checkString) == false))
                                {
                                    return (false);
                                }
                                break;
                            default:
                                if (windowString != checkString)
                                {
                                    return (false);
                                }
                                break;
                        }
                    }

                    // 指定の文字列を含んでいる場合は処理しない
                    foreach (string nowTitleName in specifiedWindowBaseItemInformation.DoNotProcessingTitleName)
                    {
                        if (windowString.Contains(nowTitleName))
                        {
                            return (false);
                        }
                    }
                }

                // クラス名判定
                if (string.IsNullOrEmpty(specifiedWindowBaseItemInformation.ClassName) == false)
                {
                    // 取得に失敗した場合は処理しない
                    if (string.IsNullOrEmpty(windowInformation.ClassName))
                    {
                        return (false);
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
                                return (false);
                            }
                            break;
                        case NameMatchCondition.ForwardMatch:
                            if ((windowString.Length < checkString.Length) || (windowString.StartsWith(checkString) == false))
                            {
                                return (false);
                            }
                            break;
                        case NameMatchCondition.BackwardMatch:
                            if ((windowString.Length < checkString.Length) || (windowString.EndsWith(checkString) == false))
                            {
                                return (false);
                            }
                            break;
                        default:
                            if (checkString != windowString)
                            {
                                return (false);
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
                        return (false);
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
                            if ((checkString != System.IO.Path.GetFileNameWithoutExtension(windowString))
                                && (checkString != System.IO.Path.GetFileName(windowString)))
                            {
                                return (false);
                            }
                            break;
                        default:
                            if (checkString != windowString)
                            {
                                return (false);
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
                        if ((windowSize.Width == nowSize.Width) && (windowSize.Height == nowSize.Height))
                        {
                            return (false);
                        }
                    }
                }

                result = true;
            }
            catch
            {
            }

            return (result);
        }

        /// <summary>
        /// 処理するディスプレイか確認
        /// </summary>
        /// <param name="specifiedWindowBaseItemInformation">「指定ウィンドウ」機能の基礎の項目情報</param>
        /// <param name="windowProcessingInformation">ウィンドウの処理情報</param>
        /// <param name="hwnd">ウィンドウハンドル</param>
        /// <param name="monitorInformation">モニター情報 (「null」でも可)</param>
        /// <returns>処理するディスプレイかの値 (処理しない「false」/処理する「true」)</returns>
        public static bool CheckDisplayToProcessing(
            SpecifiedWindowBaseItemInformation specifiedWindowBaseItemInformation,
            WindowProcessingInformation windowProcessingInformation,
            System.IntPtr hwnd,
            MonitorInformation monitorInformation
            )
        {
            bool result = true;        // 結果

            if ((Common.ApplicationData.Settings.CoordinateType == CoordinateType.Display)
                && (specifiedWindowBaseItemInformation.StandardDisplay == StandardDisplay.OnlySpecifiedDisplay))
            {
                MonitorInfoEx monitorInfo;
                if (monitorInformation == null)
                {
                    monitorInformation = MonitorInformation.GetMonitorInformation();
                }
                MonitorInformation.GetMonitorWithWindow(hwnd, out monitorInfo, monitorInformation);
                if (windowProcessingInformation.PositionSize.Display != monitorInfo.DeviceName)
                {
                    result = false;
                }
            }

            return (result);
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
        /// <param name="monitorInformation">モニター情報 (「null」でも可)</param>
        public static void ProcessingWindow(
            SpecifiedWindowBaseItemInformation specifiedWindowBaseItemInformation,
            WindowProcessingInformation windowProcessingInformation,
            System.IntPtr hwnd,
            in WINDOWPLACEMENT windowPlacement,
            bool hotkey,
            bool doNotChangeOutOfScreen,
            MonitorInformation monitorInformation
            )
        {
            try
            {
                if (monitorInformation == null)
                {
                    monitorInformation = MonitorInformation.GetMonitorInformation();
                }

                ProcessingWindowPositionAndSize(specifiedWindowBaseItemInformation, windowProcessingInformation, hwnd, windowPlacement, hotkey, doNotChangeOutOfScreen, monitorInformation);
                if (windowProcessingInformation.PositionSize.ProcessingPositionAndSizeTwice)
                {
                    WINDOWPLACEMENT SecondWindowPlacement;       // WINDOWPLACEMENT
                    NativeMethods.GetWindowPlacement(hwnd, out SecondWindowPlacement);
                    ProcessingWindowPositionAndSize(specifiedWindowBaseItemInformation, windowProcessingInformation, hwnd, SecondWindowPlacement, hotkey, doNotChangeOutOfScreen, monitorInformation);
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
        /// <param name="monitorInformation">モニター情報</param>
        /// <param name="second">2度目の処理かの値 (いいえ「false」/はい「true」)</param>
        public static void ProcessingWindowPositionAndSize(
            SpecifiedWindowBaseItemInformation specifiedWindowBaseItemInformation,
            WindowProcessingInformation windowProcessingInformation,
            System.IntPtr hwnd,
            in WINDOWPLACEMENT windowPlacement,
            bool hotkey,
            bool doNotChangeOutOfScreen,
            MonitorInformation monitorInformation,
            bool second = false
            )
        {
            try
            {
                // 2回目は処理しない
                if (second)
                {
                    switch (windowProcessingInformation.WindowState)
                    {
                        case WindowState.Maximize:
                        case WindowState.Minimize:
                            return;
                    }
                }

                RectangleDecimal windowRectangle = new(windowPlacement.rcNormalPosition.Left, windowPlacement.rcNormalPosition.Top, windowPlacement.rcNormalPosition.Right - windowPlacement.rcNormalPosition.Left, windowPlacement.rcNormalPosition.Bottom - windowPlacement.rcNormalPosition.Top);     // ウィンドウの位置とサイズ
                bool doNormal = false;     // 「通常のウィンドウ」にするかの値
                MonitorInfoEx monitorInfo;     // モニター情報
                MonitorInformation.GetMonitorWithWindow(hwnd, out monitorInfo, monitorInformation);
                RectangleDecimal changedDisplayRectangle = WindowProcessing.GetDisplayPositionAndSizeAfterProcessing(Common.ApplicationData.Settings.CoordinateType, hwnd, monitorInformation, specifiedWindowBaseItemInformation.StandardDisplay, windowProcessingInformation.PositionSize.Display, hotkey);        // 変更後のディスプレイの位置とサイズ

                // 位置やサイズを計算
                switch (windowProcessingInformation.WindowState)
                {
                    case WindowState.DoNotChange:
                        if ((Common.ApplicationData.Settings.CoordinateType == CoordinateType.Display)
                            && (specifiedWindowBaseItemInformation.StandardDisplay == StandardDisplay.SpecifiedDisplay)
                            && (windowProcessingInformation.PositionSize.Display != monitorInfo.DeviceName))
                        {
                            windowRectangle.X = changedDisplayRectangle.X + windowPlacement.rcNormalPosition.Right - windowPlacement.rcNormalPosition.Left;
                            windowRectangle.Y = changedDisplayRectangle.Y + windowPlacement.rcNormalPosition.Bottom - windowPlacement.rcNormalPosition.Top;
                        }
                        break;
                    case WindowState.Normal:
                        windowRectangle = WindowProcessing.CalculatePositionAndSize(hwnd, windowPlacement, windowProcessingInformation.PositionSize, specifiedWindowBaseItemInformation.StandardDisplay, windowProcessingInformation.PositionSize.ClientArea, monitorInformation);
                        doNormal = true;
                        break;
                    case WindowState.Maximize:
                    case WindowState.Minimize:
                        if ((Common.ApplicationData.Settings.CoordinateType == CoordinateType.Display)
                            && (specifiedWindowBaseItemInformation.StandardDisplay == StandardDisplay.SpecifiedDisplay)
                            && (windowProcessingInformation.PositionSize.Display != monitorInfo.DeviceName))
                        {
                            windowRectangle.X = changedDisplayRectangle.X;
                            windowRectangle.Y = changedDisplayRectangle.Y;
                        }
                        break;
                }

                bool doSetWindowPos = false;        // SetWindowPosを処理するかの値

                // 位置やサイズを変更する場合は処理を有効にする
                if ((windowRectangle.X != windowPlacement.rcNormalPosition.Left)
                    || (windowRectangle.Y != windowPlacement.rcNormalPosition.Top)
                    || (windowRectangle.Width != (windowPlacement.rcNormalPosition.Right - windowPlacement.rcNormalPosition.Left))
                    || (windowRectangle.Height != (windowPlacement.rcNormalPosition.Bottom - windowPlacement.rcNormalPosition.Top)))
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
                // 位置やサイズ変更できるようにウィンドウの状態を「通常のウィンドウ」にする
                if (doNormal && (windowPlacement.showCmd != (int)SW.SW_SHOWNORMAL))
                {
                    NativeMethods.ShowWindow(hwnd, (int)SW.SW_SHOWNORMAL);
                }
                if (doSetWindowPos)
                {
                    if (doNotChangeOutOfScreen && WindowProcessing.CheckWindowIsInTheScreen(windowRectangle, monitorInformation) == false)
                    {
                        flag |= (int)SWP.SWP_NOMOVE | (int)SWP.SWP_NOSIZE;
                    }
                    NativeMethods.SetWindowPos(hwnd, hwndInsertAfter, (int)windowRectangle.X, (int)windowRectangle.Y, (int)windowRectangle.Width, (int)windowRectangle.Height, flag);
                }
                switch (windowProcessingInformation.WindowState)
                {
                    case WindowState.Maximize:
                        if (doNormal || (windowPlacement.showCmd != (int)SW.SW_SHOWMAXIMIZED))
                        {
                            NativeMethods.ShowWindow(hwnd, (int)SW.SW_SHOWMAXIMIZED);
                        }
                        break;
                    case WindowState.Minimize:
                        if (doNormal || (windowPlacement.showCmd != (int)SW.SW_MINIMIZE))
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
}
