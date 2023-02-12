namespace Swindom.Sources.WindowAndControls;

/// <summary>
/// 「マグネット」ページ
/// </summary>
public partial class MagnetPage : Page
{
    /// <summary>
    /// 次のイベントをキャンセル
    /// </summary>
    private bool CancelNextEvent;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public MagnetPage()
    {
        InitializeComponent();

        DecisionDistanceToPasteNumberBox.Minimum = MagnetInformation.MinimumPastingDecisionDistance;
        DecisionDistanceToPasteNumberBox.Maximum = MagnetInformation.MaximumPastingDecisionDistance;
        StopTimeWhenPastedNumberBox.Minimum = MagnetInformation.MinimumStopTimeWhenPasted;
        StopTimeWhenPastedNumberBox.Maximum = MagnetInformation.MaximumStopTimeWhenPasted;

        SetTextOnControls();

        ProcessingStateToggleSwitch.IsOn = ApplicationData.Settings.MagnetInformation.Enabled;
        PasteToScreenEdgeToggleSwitch.IsOn = ApplicationData.Settings.MagnetInformation.PasteToEdgeOfScreen;
        PasteToAnotherWindowToggleSwitch.IsOn = ApplicationData.Settings.MagnetInformation.PasteToAnotherWindow;
        PressTheKeyToPasteToggleSwitch.IsOn = ApplicationData.Settings.MagnetInformation.PasteToPressKey;
        DecisionDistanceToPasteNumberBox.Value = ApplicationData.Settings.MagnetInformation.PastingDecisionDistance;
        StopTimeWhenPastedNumberBox.Value = ApplicationData.Settings.MagnetInformation.StopTimeWhenPasted;
        StopProcessingFullScreenToggleSwitch.IsOn = ApplicationData.Settings.MagnetInformation.StopProcessingFullScreen;

        ProcessingStateToggleSwitch.Toggled += ProcessingStateToggleSwitch_Toggled;
        PasteToScreenEdgeToggleSwitch.Toggled += PasteToScreenEdgeToggleSwitch_Toggled;
        PasteToAnotherWindowToggleSwitch.Toggled += PasteToAnotherWindowToggleSwitch_Toggled;
        PressTheKeyToPasteToggleSwitch.Toggled += PressTheKeyToPasteToggleSwitch_Toggled;
        DecisionDistanceToPasteNumberBox.ValueChanged += DecisionDistanceToPasteNumberBox_ValueChanged;
        StopTimeWhenPastedNumberBox.ValueChanged += StopTimeWhenPastedNumberBox_ValueChanged;
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
    /// 「マグネット」ToggleSwitchの「Toggled」イベント
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
            if (CancelNextEvent)
            {
                CancelNextEvent = false;
            }
            else
            {
                CancelNextEvent = true;
                ApplicationData.Settings.MagnetInformation.Enabled = ProcessingStateToggleSwitch.IsOn;
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
            ApplicationData.Settings.MagnetInformation.PastingDecisionDistance = (int)DecisionDistanceToPasteNumberBox.Value;
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「貼り付いた時の停止時間 (ミリ秒)」NumberBoxの「ValueChanged」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="args"></param>
    private void StopTimeWhenPastedNumberBox_ValueChanged(
        ModernWpf.Controls.NumberBox sender,
        ModernWpf.Controls.NumberBoxValueChangedEventArgs args
        )
    {
        try
        {
            ApplicationData.Settings.MagnetInformation.StopTimeWhenPasted = (int)StopTimeWhenPastedNumberBox.Value;
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
                    if (CancelNextEvent)
                    {
                        CancelNextEvent = false;
                    }
                    else
                    {
                        CancelNextEvent = true;
                        ProcessingStateToggleSwitch.IsOn = ApplicationData.Settings.MagnetInformation.Enabled;
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
    /// 「ウィンドウが全画面表示の場合は処理停止」ToggleSwitchの「Toggled」イベント
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
            if (ApplicationData.Languages.LanguagesWindow == null)
            {
                throw new Exception("Languages value is null. - MagnetPage.SetTextOnControls()");
            }

            ProcessingStateToggleSwitch.OffContent = ProcessingStateToggleSwitch.OnContent = ApplicationData.Languages.LanguagesWindow.ProcessingState;
            PasteToScreenEdgeToggleSwitch.OffContent = PasteToScreenEdgeToggleSwitch.OnContent = ApplicationData.Languages.LanguagesWindow.PasteToEdgeOfScreen;
            PasteToAnotherWindowToggleSwitch.OffContent = PasteToAnotherWindowToggleSwitch.OnContent = ApplicationData.Languages.LanguagesWindow.PasteIntoAnotherWindow;
            PressTheKeyToPasteToggleSwitch.OffContent = PressTheKeyToPasteToggleSwitch.OnContent = ApplicationData.Languages.LanguagesWindow.PasteWithCtrlKeyPressed;
            DecisionDistanceToPasteLabel.Content = ApplicationData.Languages.LanguagesWindow.DecisionDistanceToPaste;
            DecisionDistanceToPastePixelLabel.Content = ApplicationData.Languages.LanguagesWindow.Pixel;
            StopTimeWhenPastedLabel.Content = ApplicationData.Languages.LanguagesWindow.StopTimeWhenPasting;
            StopProcessingFullScreenToggleSwitch.OffContent = StopProcessingFullScreenToggleSwitch.OnContent = ApplicationData.Languages.LanguagesWindow.StopProcessingWhenWindowIsFullScreen;
            ExplanationTextBlock.Text = ApplicationData.Languages.LanguagesWindow.MagnetExplanation;
        }
        catch
        {
        }
    }
}
