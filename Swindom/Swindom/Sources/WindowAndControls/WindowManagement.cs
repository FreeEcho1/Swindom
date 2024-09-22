using System.Linq;

namespace Swindom;

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
    /// 「説明」ウィンドウ
    /// </summary>
    private ExplanationWindow? ExplanationWindow;

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
        ExplanationWindow?.Close();
        ExplanationWindow = null;
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
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「更新確認」ウィンドウを表示
    /// </summary>
    /// <param name="settingParentWindow">親ウィンドウを設定するかの値 (設定しない「false」/設定する「true」)</param>
    public void ShowUpdateCheckWindow(
        bool settingParentWindow = false
        )
    {
        if (UpdateInformationWindow == null)
        {
            UpdateInformationWindow = new(settingParentWindow);
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
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「説明」ウィンドウを表示
    /// </summary>
    /// <param name="explanationType">説明の種類</param>
    public void ShowExplanationWindow(
        SelectExplanationType explanationType = SelectExplanationType.Coordinate
        )
    {
        if (ExplanationWindow == null)
        {
            ExplanationWindow = new(explanationType);
            ExplanationWindow.Closed += ExplanationWindow_Closed;
            ExplanationWindow.Show();
        }
        else
        {
            ExplanationWindow.Activate();
            ExplanationWindow.SelectTabItem(explanationType);
        }
    }

    /// <summary>
    /// 「説明」ウィンドウの「Closed」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ExplanationWindow_Closed(
        object? sender,
        EventArgs e
        )
    {
        try
        {
            ExplanationWindow = null;
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「Window」の「Owner」に設定するための「Window」を取得
    /// </summary>
    /// <returns>Window</returns>
    public static Window? GetWindowToSetOwner()
    {
        return Application.Current.Windows.Cast<Window>().FirstOrDefault(window => window.IsActive && window.ShowActivated);
    }
}
