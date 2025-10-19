namespace Swindom;

/// <summary>
/// 「説明」ウィンドウ
/// </summary>
public partial class ExplanationWindow : Window
{
    /// <summary>
    /// コンストラクタ (使用しない)
    /// </summary>
    public ExplanationWindow()
    {
        throw new Exception("Do not use. - " + GetType().Name + "()");
    }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="explanationType">説明の種類</param>
    public ExplanationWindow(
        SelectExplanationType explanationType = SelectExplanationType.Coordinate
        )
    {
        InitializeComponent();

        Width = ApplicationData.Settings.ExplanationWindowSize.Width;
        Height = ApplicationData.Settings.ExplanationWindowSize.Height;

        Title = ApplicationData.Strings.Help + WindowControlValue.CopySeparateString + ApplicationValue.ApplicationName;
        CoordinateTabItem.Header = ApplicationData.Strings.Coordinate;
        CoordinateExplanationTextBox.Text = ApplicationData.Strings.CoordinateDetailedExplanation;
        WindowDecisionTabItem.Header = ApplicationData.Strings.WindowDecide;
        WindowDecisionExplanationTextBox.Text = ApplicationData.Strings.WindowDecisionDetailedExplanation;
        EventTimerTabItem.Header = ApplicationData.Strings.EventTimer;
        EventTimerExplanationTextBox.Text = ApplicationData.Strings.EventTimerDetailedExplanation;
        ClientAreaTabItem.Header = ApplicationData.Strings.ClientArea;
        ClientAreaExplanationTextBox.Text = ApplicationData.Strings.ClientAreaDetailedExplanation;
        ActiveWindowTabItem.Header = ApplicationData.Strings.ActiveWindow;
        ActiveWindowExplanationTextBox.Text = ApplicationData.Strings.ActiveWindowDetailedExplanation;
        PluginTabItem.Header = ApplicationData.Strings.Plugin;
        PluginExplanationTextBox.Text = ApplicationData.Strings.PluginDetailedExplanation;
        ChildWindowTabItem.Header = ApplicationData.Strings.ChildWindow;
        ChildWindowExplanationTextBox.Text = ApplicationData.Strings.ChildWindowExplanation;

        SelectTabItem(explanationType);

        Closed += ExplanationWindow_Closed;
    }

    /// <summary>
    /// ウィンドウの「Closed」イベント
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
            ApplicationData.Settings.ExplanationWindowSize.Width = (int)Width;
            ApplicationData.Settings.ExplanationWindowSize.Height = (int)Height;
        }
        catch
        {
        }
    }

    /// <summary>
    /// TabItemを選択
    /// </summary>
    /// <param name="explanationType">選択する説明の種類</param>
    public void SelectTabItem(
        SelectExplanationType explanationType
        )
    {
        switch (explanationType)
        {
            case SelectExplanationType.Coordinate:
                CoordinateTabItem.IsSelected = true;
                break;
            case SelectExplanationType.WindowDecision:
                WindowDecisionTabItem.IsSelected = true;
                break;
            case SelectExplanationType.EventTimer:
                EventTimerTabItem.IsSelected = true;
                break;
            case SelectExplanationType.ClientArea:
                ClientAreaTabItem.IsSelected = true;
                break;
            case SelectExplanationType.ActiveWindow:
                ActiveWindowTabItem.IsSelected = true;
                break;
            case SelectExplanationType.Plugin:
                PluginTabItem.IsSelected = true;
                break;
            case SelectExplanationType.ChildWindow:
                ChildWindowTabItem.IsSelected = true;
                break;
        }
    }
}
