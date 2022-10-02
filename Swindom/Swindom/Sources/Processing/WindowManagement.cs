namespace Swindom;

/// <summary>
/// ウィンドウの管理
/// </summary>
public class WindowManagement : IDisposable
{
    /// <summary>
    /// Disposeが呼ばれたかの値
    /// </summary>
    private bool Disposed;
    /// <summary>
    /// 「メイン」ウィンドウ
    /// </summary>
    private MainWindow? MainWindow;
    /// <summary>
    /// 「ウィンドウ情報取得」ウィンドウ
    /// </summary>
    private GetInformationWindow? GetInformationWindow;
    /// <summary>
    /// 「数値計算」ウィンドウ
    /// </summary>
    private NumericCalculationWindow? NumericCalculationWindow;
    /// <summary>
    /// 「更新確認」ウィンドウ
    /// </summary>
    private UpdateCheckWindow? UpdateCheckWindow;

    /// <summary>
    /// デストラクタ
    /// </summary>
    ~WindowManagement()
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
            MainWindow?.Close();
            GetInformationWindow?.Close();
            NumericCalculationWindow?.Close();
            UpdateCheckWindow?.Close();
        }
    }

    /// <summary>
    /// 「メイン」ウィンドウを表示
    /// </summary>
    /// <param name="firstRun">初めての実行かの値 (いいえ「false」/はい「true」)</param>
    public void ShowMainWindow(
        bool firstRun = false
        )
    {
        if (MainWindow == null)
        {
            Common.MonitorInformation = MonitorInformation.GetMonitorInformation();
            ReadLanguage();
            MainWindow = new(firstRun);
            MainWindow.Closed += MainWindow_Closed;
            MainWindow.Show();
        }
        else
        {
            MainWindow.Activate();
        }
    }

    /// <summary>
    /// 「メイン」ウィンドウの「Closed」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void MainWindow_Closed(
        object? sender,
        EventArgs e
        )
    {
        try
        {
            MainWindow = null;
            UnnecessaryLanguageDataDelete();
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「ウィンドウ情報取得」ウィンドウを表示
    /// </summary>
    public void ShowGetInformationWindow()
    {
        if (GetInformationWindow == null)
        {
            Common.MonitorInformation = MonitorInformation.GetMonitorInformation();
            ReadLanguage();
            GetInformationWindow = new();
            GetInformationWindow.Closed += GetInformationWindow_Closed;
            GetInformationWindow.Show();
        }
        else
        {
            GetInformationWindow.Activate();
        }
    }

    /// <summary>
    /// 「ウィンドウ情報取得」ウィンドウの「Closed」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void GetInformationWindow_Closed(
        object? sender,
        EventArgs e
        )
    {
        try
        {
            GetInformationWindow = null;
            UnnecessaryLanguageDataDelete();
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「数値計算」ウィンドウを表示
    /// </summary>
    public void ShowNumericCalculationWindow()
    {
        if (NumericCalculationWindow == null)
        {
            ReadLanguage();
            NumericCalculationWindow = new();
            NumericCalculationWindow.Closed += NumericCalculationWindow_Closed;
            NumericCalculationWindow.Show();
        }
        else
        {
            NumericCalculationWindow.Activate();
        }
    }

    /// <summary>
    /// 「数値計算」ウィンドウの「Closed」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void NumericCalculationWindow_Closed(
        object? sender,
        EventArgs e
        )
    {
        try
        {
            NumericCalculationWindow = null;
            UnnecessaryLanguageDataDelete();
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「更新確認」ウィンドウを表示
    /// </summary>
    /// <param name="parentWindow">親ウィンドウ (なし「null」)</param>
    public void ShowUpdateCheckWindow(
        Window? parentWindow = null
        )
    {
        if (UpdateCheckWindow == null)
        {
            ReadLanguage();
            UpdateCheckWindow = new(parentWindow);
            UpdateCheckWindow.Closed += UpdateCheckWindow_Closed;
            UpdateCheckWindow.Show();
        }
        else
        {
            UpdateCheckWindow.Activate();
        }
    }

    /// <summary>
    /// 「更新確認」ウィンドウの「Closed」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void UpdateCheckWindow_Closed(
        object? sender,
        EventArgs e
        )
    {
        try
        {
            UpdateCheckWindow = null;
            UnnecessaryLanguageDataDelete();
        }
        catch
        {
        }
    }

    /// <summary>
    /// ウィンドウで使用する言語情報がない場合は読み込む
    /// </summary>
    private static void ReadLanguage()
    {
        if (Common.ApplicationData.Languages.LanguagesWindow == null)
        {
            LanguageFileProcessing.ReadLanguage();
        }
    }

    /// <summary>
    /// 不要な言語データ削除
    /// </summary>
    public void UnnecessaryLanguageDataDelete()
    {
        if (MainWindow == null && GetInformationWindow == null && UpdateCheckWindow == null & NumericCalculationWindow == null)
        {
            Common.ApplicationData.Languages.LanguagesWindow = null;
        }
    }
}
