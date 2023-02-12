namespace Swindom.Sources.WindowAndControls;

/// <summary>
/// ウィンドウ管理
/// </summary>
public class WindowManagement
{
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
    private UpdateInformationWindow? UpdateInformationWindow;

    /// <summary>
    /// 全てのウィンドウを閉じる
    /// </summary>
    public void CloseAll()
    {
        MainWindow?.Close();
        MainWindow = null;
        GetInformationWindow?.Close();
        GetInformationWindow = null;
        NumericCalculationWindow?.Close();
        NumericCalculationWindow = null;
        UpdateInformationWindow?.Close();
        UpdateInformationWindow = null;
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
        if (UpdateInformationWindow == null)
        {
            ReadLanguage();
            UpdateInformationWindow = new(parentWindow);
            UpdateInformationWindow.Closed += UpdateInformationWindow_Closed;
            UpdateInformationWindow.Show();
        }
        else
        {
            UpdateInformationWindow.Activate();
        }
    }

    /// <summary>
    /// 「更新確認」ウィンドウの「Closed」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void UpdateInformationWindow_Closed(
        object? sender,
        EventArgs e
        )
    {
        try
        {
            UpdateInformationWindow = null;
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
        if (ApplicationData.Languages.LanguagesWindow == null)
        {
            LanguageFileProcessing.ReadLanguage();
        }
    }

    /// <summary>
    /// 不要な言語データ削除
    /// </summary>
    public void UnnecessaryLanguageDataDelete()
    {
        if (MainWindow == null
            && GetInformationWindow == null
            && UpdateInformationWindow == null
            && NumericCalculationWindow == null)
        {
            ApplicationData.Languages.LanguagesWindow = null;
        }
    }
}
