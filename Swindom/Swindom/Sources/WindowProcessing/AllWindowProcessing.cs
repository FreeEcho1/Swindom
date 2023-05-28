namespace Swindom;

/// <summary>
/// 「全てのウィンドウ」処理
/// </summary>
public class AllWindowProcessing : IDisposable
{
    /// <summary>
    /// Disposed
    /// </summary>
    private bool Disposed;

    /// <summary>
    /// ウィンドウイベント
    /// </summary>
    private FreeEcho.FEWindowEvent.WindowEvent? WindowEvent;
    /// <summary>
    /// ウィンドウ情報のバッファ
    /// </summary>
    private readonly WindowInformationBuffer WindowInformationBuffer = new();

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public AllWindowProcessing()
    {
        ProcessingSettings();
    }

    /// <summary>
    /// デストラクタ
    /// </summary>
    ~AllWindowProcessing()
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
            DisposeWindowEvent();
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
    /// 処理が有効か確認
    /// </summary>
    /// <returns>処理が有効かの値 (無効「false」/有効「true」)</returns>
    public static bool CheckIfTheProcessingIsValid() => ApplicationData.Settings.AllWindowInformation.Enabled
            && (ApplicationData.Settings.AllWindowInformation.StopProcessingFullScreen == false || ApplicationData.FullScreenExists == false);

    /// <summary>
    /// 処理設定
    /// </summary>
    public void ProcessingSettings()
    {
        FreeEcho.FEWindowEvent.HookWindowEventType type = 0;

        if (ApplicationData.Settings.AllWindowInformation.AllWindowPositionSizeWindowEventData.MoveSizeEnd)
        {
            type |= FreeEcho.FEWindowEvent.HookWindowEventType.MoveSizeEnd;
        }
        if (ApplicationData.Settings.AllWindowInformation.AllWindowPositionSizeWindowEventData.Show)
        {
            type |= FreeEcho.FEWindowEvent.HookWindowEventType.Show;
        }

        if (CheckIfTheProcessingIsValid()
            && type != 0)
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
        else
        {
            DisposeWindowEvent();
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
            DecisionAndWindowProcessing(e);
        }
        catch
        {
        }
    }

    /// <summary>
    /// 判定してウィンドウ処理
    /// </summary>
    /// <param name="e">WindowEventArgs</param>
    private void DecisionAndWindowProcessing(
        FreeEcho.FEWindowEvent.WindowEventArgs e
        )
    {
        try
        {
            IntPtr hwnd = FreeEcho.FEWindowEvent.WindowEvent.GetWindowHwnd(e.Hwnd, e.EventType);
            if (FreeEcho.FEWindowEvent.WindowEvent.ConfirmWindowVisible(hwnd, e.EventType) == false)
            {
                return;
            }

            HwndList hwndList = new();
            hwndList.Hwnd.Add(hwnd);
            if (ApplicationData.Settings.AllWindowInformation.StopProcessingFullScreen && VariousWindowProcessing.CheckFullScreenWindow(hwndList))
            {
                return;
            }

            WindowInformation windowInformation = VariousWindowProcessing.GetWindowInformationFromHandle(hwnd, WindowInformationBuffer);      // ウィンドウの情報
            if (CheckWindowToProcessing(windowInformation))
            {
                ProcessingWindow(hwnd, windowInformation);
            }
        }
        catch
        {
        }
    }

    /// <summary>
    /// 処理するウィンドウか確認
    /// </summary>
    /// <param name="windowInformation">ウィンドウの情報</param>
    /// <returns>処理するウィンドウかの値 (いいえ「false」/はい「true」)</returns>
    private bool CheckWindowToProcessing(
        WindowInformation windowInformation
        )
    {
        foreach (SpecifyWindowItemInformation nowEII in ApplicationData.Settings.SpecifyWindowInformation.Items)
        {
            if (SpecifyWindowProcessing.CheckWindowToProcessing(nowEII, windowInformation))
            {
                return false;
            }
        }

        foreach (WindowJudgementInformation nowData in ApplicationData.Settings.AllWindowInformation.CancelAllWindowPositionSize)
        {
            bool check = true;      // 確認
            string checkString;      // 確認する文字列
            string windowString;       // ウィンドウ文字列

            // タイトル名判定
            if (string.IsNullOrEmpty(windowInformation.TitleName) == false)
            {
                if (ApplicationData.Settings.AllWindowInformation.CaseSensitive)
                {
                    checkString = nowData.TitleName;
                    windowString = windowInformation.TitleName;
                }
                else
                {
                    checkString = nowData.TitleName.ToLower();
                    windowString = windowInformation.TitleName.ToLower();
                }

                if (string.IsNullOrEmpty(checkString) == false)
                {
                    switch (nowData.TitleNameMatchCondition)
                    {
                        case NameMatchCondition.PartialMatch:
                            if (windowString.Contains(checkString) == false)
                            {
                                check = false;
                            }
                            break;
                        case NameMatchCondition.ForwardMatch:
                            if (windowString.Length < checkString.Length || windowString.StartsWith(checkString) == false)
                            {
                                check = false;
                            }
                            break;
                        case NameMatchCondition.BackwardMatch:
                            if (windowString.Length < checkString.Length || windowString.EndsWith(checkString) == false)
                            {
                                check = false;
                            }
                            break;
                        default:
                            if (windowString != checkString)
                            {
                                check = false;
                            }
                            break;
                    }
                }
            }

            // クラス名判定
            if (string.IsNullOrEmpty(nowData.ClassName) == false)
            {
                // 取得に失敗した場合は処理しない
                if (string.IsNullOrEmpty(windowInformation.ClassName))
                {
                    return false;
                }

                if (ApplicationData.Settings.AllWindowInformation.CaseSensitive)
                {
                    checkString = nowData.ClassName;
                    windowString = windowInformation.ClassName;
                }
                else
                {
                    checkString = nowData.ClassName.ToLower();
                    windowString = windowInformation.ClassName.ToLower();
                }

                switch (nowData.ClassNameMatchCondition)
                {
                    case NameMatchCondition.PartialMatch:
                        if (windowString.Contains(checkString) == false)
                        {
                            check = false;
                        }
                        break;
                    case NameMatchCondition.ForwardMatch:
                        if (windowString.Length < checkString.Length || windowString.StartsWith(checkString) == false)
                        {
                            check = false;
                        }
                        break;
                    case NameMatchCondition.BackwardMatch:
                        if (windowString.Length < checkString.Length || windowString.EndsWith(checkString) == false)
                        {
                            check = false;
                        }
                        break;
                    default:
                        if (checkString != windowString)
                        {
                            check = false;
                        }
                        break;
                }
            }

            // ファイル名判定
            if (string.IsNullOrEmpty(nowData.FileName) == false)
            {
                // 取得に失敗した場合は処理しない
                if (string.IsNullOrEmpty(windowInformation.FileName))
                {
                    return false;
                }

                if (ApplicationData.Settings.AllWindowInformation.CaseSensitive)
                {
                    checkString = nowData.FileName;
                    windowString = windowInformation.FileName;
                }
                else
                {
                    checkString = nowData.FileName.ToLower();
                    windowString = windowInformation.FileName.ToLower();
                }

                switch (nowData.FileNameMatchCondition)
                {
                    case FileNameMatchCondition.NotInclude:
                        if (checkString != Path.GetFileNameWithoutExtension(windowString)
                            && checkString != Path.GetFileName(windowString))
                        {
                            check = false;
                        }
                        break;
                    default:
                        if (checkString != windowString)
                        {
                            check = false;
                        }
                        break;
                }
            }

            if (check)
            {
                return false;
            }
        }

        return true;
    }

    /// <summary>
    /// ウィンドウを処理
    /// </summary>
    /// <param name="hwnd">ウィンドウハンドル</param>
    /// <param name="windowInformation">ウィンドウの情報</param>
    private void ProcessingWindow(
        IntPtr hwnd,
        WindowInformation windowInformation
        )
    {
        Rect windowRectangle = VariousWindowProcessing.GetPositionSizeOfWindowAfterProcessing(hwnd, windowInformation, ApplicationData.Settings.AllWindowInformation.PositionSize, ApplicationData.Settings.AllWindowInformation.StandardDisplay, false);     // 処理後のウィンドウの位置とサイズ

        if (windowRectangle.X != windowInformation.Rectangle.Left
            || windowRectangle.Y != windowInformation.Rectangle.Top)
        {
            NativeMethods.SetWindowPos(hwnd, (int)HwndInsertAfter.HWND_NOTOPMOST, (int)windowRectangle.X, (int)windowRectangle.Y, 0, 0, (int)SWP.SWP_NOZORDER | (int)SWP.SWP_NOSIZE);
        }
    }
}
