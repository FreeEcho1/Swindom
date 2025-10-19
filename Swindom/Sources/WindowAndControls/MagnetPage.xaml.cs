namespace Swindom;

/// <summary>
/// 「マグネット」ページ
/// </summary>
public partial class MagnetPage : Page
{
    /// <summary>
    /// コンストラクタ
    /// </summary>
    public MagnetPage()
    {
        InitializeComponent();

        DecisionDistanceToPasteNumberBox.Minimum = MagnetInformation.MinimumPastingDecisionDistance;
        DecisionDistanceToPasteNumberBox.Maximum = MagnetInformation.MaximumPastingDecisionDistance;
        PastingTimeNumberBox.Minimum = MagnetInformation.MinimumPastingTime;
        PastingTimeNumberBox.Maximum = MagnetInformation.MaximumPastingTime;

        SetTextOnControls();

        ProcessingStateToggleSwitch.IsOn = ApplicationData.Settings.MagnetInformation.IsEnabled;
        PasteToScreenEdgeToggleSwitch.IsOn = ApplicationData.Settings.MagnetInformation.PasteToEdgeOfScreen;
        PasteToAnotherWindowToggleSwitch.IsOn = ApplicationData.Settings.MagnetInformation.PasteToAnotherWindow;
        PressTheKeyToPasteToggleSwitch.IsOn = ApplicationData.Settings.MagnetInformation.PasteToPressKey;
        KeepPasteUntilKeyUpToggleSwitch.IsOn = ApplicationData.Settings.MagnetInformation.KeepPasteUntilKeyUp;
        KeepPasteUntilKeyUpToggleSwitch.IsEnabled = ApplicationData.Settings.MagnetInformation.PasteToPressKey;
        DecisionDistanceToPasteNumberBox.Value = ApplicationData.Settings.MagnetInformation.PastingDecisionDistance;
        PastingTimeNumberBox.Value = ApplicationData.Settings.MagnetInformation.PastingTime;
        StopProcessingFullScreenToggleSwitch.IsOn = ApplicationData.Settings.MagnetInformation.StopProcessingFullScreen;

        ProcessingStateToggleSwitch.Toggled += ProcessingStateToggleSwitch_Toggled;
        PasteToScreenEdgeToggleSwitch.Toggled += PasteToScreenEdgeToggleSwitch_Toggled;
        PasteToAnotherWindowToggleSwitch.Toggled += PasteToAnotherWindowToggleSwitch_Toggled;
        PressTheKeyToPasteToggleSwitch.Toggled += PressTheKeyToPasteToggleSwitch_Toggled;
        KeepPasteUntilKeyUpToggleSwitch.Toggled += KeepPasteUntilKeyUpToggleSwitch_Toggled;
        DecisionDistanceToPasteNumberBox.ValueChanged += DecisionDistanceToPasteNumberBox_ValueChanged;
        PastingTimeNumberBox.ValueChanged += PastingTimeNumberBox_ValueChanged;
        StopProcessingFullScreenToggleSwitch.Toggled += StopProcessingFullScreenToggleSwitch_Toggled;

        ApplicationData.EventData.ProcessingEvent += ApplicationData_ProcessingEvent;
    }

    /// <summary>
    /// 破棄
    /// </summary>
    public void Release()
    {
        ApplicationData.EventData.ProcessingEvent -= ApplicationData_ProcessingEvent;
    }

    /// <summary>
    /// 「処理状態」ToggleSwitchの「Toggled」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ProcessingStateToggleSwitch_Toggled(
        object sender,
        RoutedEventArgs e
        )
    {
        try
        {
            if (ApplicationData.Settings.MagnetInformation.IsEnabled !=  ProcessingStateToggleSwitch.IsOn)
            {
                ApplicationData.Settings.MagnetInformation.IsEnabled = ProcessingStateToggleSwitch.IsOn;
                ApplicationData.EventData.DoProcessingEvent(ProcessingEventType.MagnetProcessingStateChanged);
            }
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「画面端に貼り付ける」ToggleSwitchの「Toggled」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void PasteToScreenEdgeToggleSwitch_Toggled(
        object sender,
        RoutedEventArgs e
        )
    {
        try
        {
            ApplicationData.Settings.MagnetInformation.PasteToEdgeOfScreen = PasteToScreenEdgeToggleSwitch.IsOn;
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「別のウィンドウに貼り付ける」ToggleSwitchの「Toggled」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void PasteToAnotherWindowToggleSwitch_Toggled(
        object sender,
        RoutedEventArgs e
        )
    {
        try
        {
            ApplicationData.Settings.MagnetInformation.PasteToAnotherWindow = PasteToAnotherWindowToggleSwitch.IsOn;
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「Ctrlキーを押した状態で貼り付ける」ToggleSwitchの「Toggled」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void PressTheKeyToPasteToggleSwitch_Toggled(
        object sender,
        RoutedEventArgs e
        )
    {
        try
        {
            ApplicationData.Settings.MagnetInformation.PasteToPressKey = PressTheKeyToPasteToggleSwitch.IsOn;
            KeepPasteUntilKeyUpToggleSwitch.IsEnabled = PressTheKeyToPasteToggleSwitch.IsOn;
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「キーを離すまで貼り付いた状態にする」ToggleSwitchの「Toggled」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void KeepPasteUntilKeyUpToggleSwitch_Toggled(
        object sender,
        RoutedEventArgs e
        )
    {
        try
        {
            ApplicationData.Settings.MagnetInformation.KeepPasteUntilKeyUp = KeepPasteUntilKeyUpToggleSwitch.IsOn;
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「貼り付ける判定距離」NumberBoxの「ValueChanged」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void DecisionDistanceToPasteNumberBox_ValueChanged(
        ModernWpf.Controls.NumberBox sender,
        ModernWpf.Controls.NumberBoxValueChangedEventArgs args
        )
    {
        try
        {
            ApplicationData.Settings.MagnetInformation.PastingDecisionDistance = double.IsNaN(DecisionDistanceToPasteNumberBox.Value) ? 0 : (int)DecisionDistanceToPasteNumberBox.Value;
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「貼り付く時間 (ミリ秒)」NumberBoxの「ValueChanged」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void PastingTimeNumberBox_ValueChanged(
        ModernWpf.Controls.NumberBox sender,
        ModernWpf.Controls.NumberBoxValueChangedEventArgs args
        )
    {
        try
        {
            ApplicationData.Settings.MagnetInformation.PastingTime = double.IsNaN(PastingTimeNumberBox.Value) ? 0 : (int)PastingTimeNumberBox.Value;
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「処理」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ApplicationData_ProcessingEvent(
        object? sender,
        ProcessingEventArgs e
        )
    {
        try
        {
            switch (e.ProcessingEventType)
            {
                case ProcessingEventType.MagnetProcessingStateChanged:
                    if (ApplicationData.Settings.MagnetInformation.IsEnabled != ProcessingStateToggleSwitch.IsOn)
                    {
                        ProcessingStateToggleSwitch.IsOn = ApplicationData.Settings.MagnetInformation.IsEnabled;
                    }
                    break;
                case ProcessingEventType.LanguageChanged:
                    SetTextOnControls();
                    break;
            }
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「全画面のウィンドウが存在する場合は処理停止」ToggleSwitchの「Toggled」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void StopProcessingFullScreenToggleSwitch_Toggled(
        object sender,
        RoutedEventArgs e
        )
    {
        try
        {
            ApplicationData.Settings.MagnetInformation.StopProcessingFullScreen = StopProcessingFullScreenToggleSwitch.IsOn;
            ApplicationData.EventData.DoProcessingEvent(ProcessingEventType.StopWhenWindowIsFullScreenChanged);
        }
        catch
        {
        }
    }

    /// <summary>
    /// コントロールにテキストを設定
    /// </summary>
    private void SetTextOnControls()
    {
        try
        {
            ProcessingStateToggleSwitch.OffContent = ProcessingStateToggleSwitch.OnContent = ApplicationData.Strings.ProcessingState;
            PasteToScreenEdgeToggleSwitch.OffContent = PasteToScreenEdgeToggleSwitch.OnContent = ApplicationData.Strings.PasteToEdgeOfScreen;
            PasteToAnotherWindowToggleSwitch.OffContent = PasteToAnotherWindowToggleSwitch.OnContent = ApplicationData.Strings.PasteIntoAnotherWindow;
            PressTheKeyToPasteToggleSwitch.OffContent = PressTheKeyToPasteToggleSwitch.OnContent = ApplicationData.Strings.PasteWithCtrlKeyPressed;
            KeepPasteUntilKeyUpToggleSwitch.OffContent = KeepPasteUntilKeyUpToggleSwitch.OnContent = ApplicationData.Strings.KeepPasteUntilKeyUp;
            DecisionDistanceToPasteLabel.Content = ApplicationData.Strings.DecisionDistanceToPaste;
            DecisionDistanceToPastePixelLabel.Content = ApplicationData.Strings.Pixel;
            PastingTimeLabel.Content = ApplicationData.Strings.PastingTime;
            StopProcessingFullScreenToggleSwitch.OffContent = StopProcessingFullScreenToggleSwitch.OnContent = ApplicationData.Strings.StopProcessingWhenWindowIsFullScreen;
            ExplanationTextBlock.Text = ApplicationData.Strings.MagnetExplanation;
        }
        catch
        {
        }
    }
}
