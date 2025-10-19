using System.Threading.Tasks;

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
    /// ディスプレイの設定を変更
    /// </summary>
    /// <param name="changeDisplaySettingsData">設定しているディスプレイが存在しなくなった場合に設定するデータ</param>
    /// <returns>設定を変更したかの値 (「false」変更していない/「true」変更した)</returns>
    public static bool ChangeDisplaySettings(
        ChangeDisplaySettingsData changeDisplaySettingsData
        )
    {
        bool result = false;        // 設定を変更したかの値

        try
        {
            bool checkMatch = false;        // 一致確認

            foreach (MonitorInfoEx nowMIE in ApplicationData.MonitorInformation.MonitorInfo)
            {
                if (nowMIE.DeviceName == ApplicationData.Settings.AllWindowInformation.PositionSize.Display)
                {
                    checkMatch = true;
                    break;
                }
            }

            if (checkMatch == false)
            {
                if (changeDisplaySettingsData.ApplySameSettingToRemaining)
                {
                    if (changeDisplaySettingsData.IsModified)
                    {
                        ApplicationData.Settings.AllWindowInformation.PositionSize.Display = changeDisplaySettingsData.AutoSettingDisplayName;
                    }
                }
                else
                {
                    SelectDisplayWindow window = new(ApplicationData.Strings.AllWindow, "");

                    window.ShowDialog();
                    changeDisplaySettingsData.ApplySameSettingToRemaining = window.ApplySameSettingToRemaining;
                    if (window.ApplySameSettingToRemaining)
                    {
                        changeDisplaySettingsData.AutoSettingDisplayName = window.SelectedDisplay;
                    }
                    if (window.IsModified)
                    {
                        changeDisplaySettingsData.IsModified = true;
                        ApplicationData.Settings.AllWindowInformation.PositionSize.Display = window.SelectedDisplay;
                        result = true;
                    }
                }
            }
        }
        catch
        {
        }

        return result;
    }

    /// <summary>
    /// 設定されているディスプレイが存在するかを確認
    /// </summary>
    /// <param name="newMonitorInformation">新しいモニター情報</param>
    /// <returns>存在するかの値 (「false」存在しない/「true」存在する)</returns>
    public static bool CheckSettingDisplaysExist(
        MonitorInformation newMonitorInformation
        )
    {
        try
        {
            foreach (MonitorInfoEx nowMIE in newMonitorInformation.MonitorInfo)
            {
                if (nowMIE.DeviceName == ApplicationData.Settings.AllWindowInformation.PositionSize.Display)
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
    /// 処理が有効か確認
    /// </summary>
    /// <returns>処理が有効かの値 (無効「false」/有効「true」)</returns>
    public static bool CheckIfTheProcessingIsValid() => ApplicationData.Settings.AllWindowInformation.IsEnabled
            && (ApplicationData.Settings.AllWindowInformation.StopProcessingFullScreen == false || ApplicationData.FullScreenExists == false);

    /// <summary>
    /// 処理設定
    /// </summary>
    public void ProcessingSettings()
    {
        FreeEcho.FEWindowEvent.HookWindowEventType type = 0;

        if (ApplicationData.Settings.AllWindowInformation.WindowEvent.MoveSizeEnd)
        {
            type |= FreeEcho.FEWindowEvent.HookWindowEventType.MoveSizeEnd;
        }
        if (ApplicationData.Settings.AllWindowInformation.WindowEvent.Show)
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
    private async void WindowEvent_WindowEventOccurrence(
        object? sender,
        FreeEcho.FEWindowEvent.WindowEventArgs e
        )
    {
        try
        {
            IntPtr hwnd = e.Hwnd;       // ウィンドウハンドル
            if (FreeEcho.FEWindowEvent.WindowEvent.ConfirmWindowVisible(hwnd, e.EventType) == false)
            {
                return;
            }
            hwnd = FreeEcho.FEWindowEvent.WindowEvent.GetAncestorHwnd(e.Hwnd, e.EventType);

            if (ApplicationData.Settings.AllWindowInformation.StopProcessingFullScreen && CheckFullScreenProcessing.CheckFullScreenWindow(hwnd))
            {
                return;
            }

            // 処理を遅らせる
            if (e.EventType == FreeEcho.FEWindowEvent.EVENT_CONSTANTS.EVENT_OBJECT_SHOW
                && 0 < ApplicationData.Settings.AllWindowInformation.WindowEvent.DelayTime)
            {
                await Task.Delay(ApplicationData.Settings.AllWindowInformation.WindowEvent.DelayTime);
            }

            WindowInformation windowInformation = WindowProcessing.GetWindowInformationFromHandle(hwnd);      // ウィンドウの情報
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
        // 除外するウィンドウの判定
        if (WindowProcessing.CheckExclusionProcessing(windowInformation))
        {
            return false;
        }

        // 処理しないウィンドウの判定 (指定ウィンドウ)
        foreach (SpecifyWindowItemInformation nowEII in ApplicationData.Settings.SpecifyWindowInformation.Items)
        {
            if (SpecifyWindowProcessing.CheckWindowToProcessing(nowEII, windowInformation))
            {
                return false;
            }
        }

        // 処理しないウィンドウの判定 (全てのウィンドウ)
        string titleName;      // タイトル名
        string className;      // クラス名
        string fileName;       // ファイル名
        if (ApplicationData.Settings.AllWindowInformation.CaseSensitive)
        {
            titleName = windowInformation.TitleName;
            className = windowInformation.ClassName;
            fileName = windowInformation.FileName;
        }
        else
        {
            titleName = windowInformation.TitleName.ToLower();
            className = windowInformation.ClassName.ToLower();
            fileName = windowInformation.FileName.ToLower();
        }
        foreach (WindowJudgementInformation nowData in ApplicationData.Settings.AllWindowInformation.Items)
        {
            bool check = true;      // 処理の確認値
            string checkString;      // 確認する文字列

            // タイトル名判定
            if (string.IsNullOrEmpty(nowData.TitleName) == false)
            {
                // タイトル名がない場合は処理しない
                if (string.IsNullOrEmpty(titleName))
                {
                    return false;
                }

                if (ApplicationData.Settings.AllWindowInformation.CaseSensitive)
                {
                    checkString = nowData.TitleName;
                }
                else
                {
                    checkString = nowData.TitleName.ToLower();
                }

                if (string.IsNullOrEmpty(checkString) == false)
                {
                    switch (nowData.TitleNameMatchCondition)
                    {
                        case NameMatchCondition.PartialMatch:
                            if (titleName.Contains(checkString) == false)
                            {
                                check = false;
                            }
                            break;
                        case NameMatchCondition.ForwardMatch:
                            if (titleName.Length < checkString.Length || titleName.StartsWith(checkString) == false)
                            {
                                check = false;
                            }
                            break;
                        case NameMatchCondition.BackwardMatch:
                            if (titleName.Length < checkString.Length || titleName.EndsWith(checkString) == false)
                            {
                                check = false;
                            }
                            break;
                        default:
                            if (titleName != checkString)
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
                // 取得に失敗している場合は処理しない
                if (string.IsNullOrEmpty(className))
                {
                    return false;
                }

                if (ApplicationData.Settings.AllWindowInformation.CaseSensitive)
                {
                    checkString = nowData.ClassName;
                }
                else
                {
                    checkString = nowData.ClassName.ToLower();
                }

                switch (nowData.ClassNameMatchCondition)
                {
                    case NameMatchCondition.PartialMatch:
                        if (className.Contains(checkString) == false)
                        {
                            check = false;
                        }
                        break;
                    case NameMatchCondition.ForwardMatch:
                        if (className.Length < checkString.Length || className.StartsWith(checkString) == false)
                        {
                            check = false;
                        }
                        break;
                    case NameMatchCondition.BackwardMatch:
                        if (className.Length < checkString.Length || className.EndsWith(checkString) == false)
                        {
                            check = false;
                        }
                        break;
                    default:
                        if (checkString != className)
                        {
                            check = false;
                        }
                        break;
                }
            }

            // ファイル名判定
            if (string.IsNullOrEmpty(nowData.FileName) == false)
            {
                // 取得に失敗している場合は処理しない
                if (string.IsNullOrEmpty(fileName))
                {
                    return false;
                }

                if (ApplicationData.Settings.AllWindowInformation.CaseSensitive)
                {
                    checkString = nowData.FileName;
                }
                else
                {
                    checkString = nowData.FileName.ToLower();
                }

                switch (nowData.FileNameMatchCondition)
                {
                    case FileNameMatchCondition.NotInclude:
                        if (checkString != Path.GetFileNameWithoutExtension(fileName)
                            && checkString != Path.GetFileName(fileName))
                        {
                            check = false;
                        }
                        break;
                    default:
                        if (checkString != fileName)
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
        Rect windowRectangle = WindowProcessing.GetPositionSizeOfWindowAfterProcessing(hwnd, windowInformation, ApplicationData.Settings.AllWindowInformation.PositionSize, ApplicationData.Settings.AllWindowInformation.StandardDisplay, false);     // 処理後のウィンドウの位置とサイズ

        if (windowRectangle.X != windowInformation.Rectangle.Left
            || windowRectangle.Y != windowInformation.Rectangle.Top)
        {
            NativeMethods.SetWindowPos(hwnd, (int)HwndInsertAfter.HWND_NOTOPMOST, (int)windowRectangle.X, (int)windowRectangle.Y, 0, 0, (int)SWP.SWP_NOZORDER | (int)SWP.SWP_NOSIZE);
            // 違うディスプレイに移動してウィンドウのサイズが変更になった場合、正しい位置に移動しない場合があるので2回処理している。
            WindowInformation secondWindowInformation = WindowProcessing.GetWindowInformationFromHandle(hwnd);      // ウィンドウの情報
            windowRectangle = WindowProcessing.GetPositionSizeOfWindowAfterProcessing(hwnd, secondWindowInformation, ApplicationData.Settings.AllWindowInformation.PositionSize, ApplicationData.Settings.AllWindowInformation.StandardDisplay, false);
            if (windowRectangle.X != secondWindowInformation.Rectangle.Left
            || windowRectangle.Y != secondWindowInformation.Rectangle.Top)
            {
                NativeMethods.SetWindowPos(hwnd, (int)HwndInsertAfter.HWND_NOTOPMOST, (int)windowRectangle.X, (int)windowRectangle.Y, 0, 0, (int)SWP.SWP_NOZORDER | (int)SWP.SWP_NOSIZE);
            }
        }
    }

    /// <summary>
    /// 「WindowJudgementInformation」のコピーを追加
    /// </summary>
    /// <param name="copyData">コピーするデータ</param>
    public static void AddCopyWindowJudgementInformation(
        WindowJudgementInformation copyData
        )
    {
        WindowJudgementInformation newItem = new(copyData);     // 新しい項目
        int number = 1;     // 番号
        string stringData = newItem.RegisteredName + WindowControlValue.CopySeparateString + ApplicationData.Strings.Copy + WindowControlValue.SpaceSeparateString;
        // 登録名に含める番号を決める
        for (int count = 0; count < ApplicationData.Settings.AllWindowInformation.Items.Count; count++)
        {
            if (ApplicationData.Settings.AllWindowInformation.Items[count].RegisteredName == (stringData + number))
            {
                // 番号を変えて最初から確認
                count = 0;
                number++;
            }
        }
        newItem.RegisteredName = stringData + number;
        ApplicationData.Settings.AllWindowInformation.Items.Add(new(newItem));
    }
}
