namespace Swindom;

/// <summary>
/// 「マグネット」コントロール
/// </summary>
public partial class MagnetControl : UserControl, IDisposable
{
    /// <summary>
    /// Disposeが呼ばれたかの値
    /// </summary>
    private bool Disposed;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public MagnetControl()
    {
        InitializeComponent();

        SetTextOnControls();

        ProcessingStateToggleSwitch.IsOnDoNotEvent = Common.ApplicationData.Settings.MagnetInformation.Enabled;
        PasteToScreenEdgeToggleSwitch.IsOnDoNotEvent = Common.ApplicationData.Settings.MagnetInformation.PasteToScreenEdge;
        PasteToAnotherWindowToggleSwitch.IsOnDoNotEvent = Common.ApplicationData.Settings.MagnetInformation.PasteToAnotherWindow;
        PressTheKeyToPasteToggleSwitch.IsOnDoNotEvent = Common.ApplicationData.Settings.MagnetInformation.PressTheKeyToPaste;
        DecisionDistanceToPasteNumericUpDown.ValueInt = Common.ApplicationData.Settings.MagnetInformation.DecisionDistanceToPaste;
        StopTimeWhenPastedNumericUpDown.ValueInt = Common.ApplicationData.Settings.MagnetInformation.StopTimeWhenPasted;
        StopProcessingFullScreenToggleSwitch.IsOnDoNotEvent = Common.ApplicationData.Settings.MagnetInformation.StopProcessingFullScreen;

        ProcessingStateToggleSwitch.IsOnChange += ProcessingStateToggleSwitch_IsOnChange;
        PasteToScreenEdgeToggleSwitch.IsOnChange += PasteToScreenEdgeToggleSwitch_IsOnChange;
        PasteToAnotherWindowToggleSwitch.IsOnChange += PasteToAnotherWindowToggleSwitch_IsOnChange;
        PressTheKeyToPasteToggleSwitch.IsOnChange += PressTheKeyToPasteToggleSwitch_IsOnChange;
        DecisionDistanceToPasteNumericUpDown.ChangeValue += DecisionDistanceToPasteNumericUpDown_ChangeValue;
        StopTimeWhenPastedNumericUpDown.ChangeValue += StopTimeWhenPastedNumericUpDown_ChangeValue;
        StopProcessingFullScreenToggleSwitch.IsOnChange += StopProcessingFullScreenToggleSwitch_IsOnChange;

        Common.ApplicationData.ProcessingEvent += ApplicationData_ProcessingEvent;
    }

    /// <summary>
    /// デストラクタ
    /// </summary>
    ~MagnetControl()
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
            Common.ApplicationData.ProcessingEvent -= ApplicationData_ProcessingEvent;
        }
    }

    /// <summary>
    /// 「マグネット」ToggleSwitchの「IsOnChange」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ProcessingStateToggleSwitch_IsOnChange(
        object sender,
        FreeEcho.FEControls.ToggleSwitchEventArgs e
        )
    {
        try
        {
            Common.ApplicationData.Settings.MagnetInformation.Enabled = ProcessingStateToggleSwitch.IsOn;
            Common.ApplicationData.DoProcessingEvent(ProcessingEventType.MagnetProcessingStateChanged);
        }
        catch
        {
            FreeEcho.FEControls.MessageBox.Show(Common.ApplicationData.Languages.Check, Common.ApplicationData.Languages.ErrorOccurred, FreeEcho.FEControls.MessageBoxButton.Ok, Window.GetWindow(this));
        }
    }

    /// <summary>
    /// 「画面端に貼り付ける」ToggleSwitchの「IsOnChange」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void PasteToScreenEdgeToggleSwitch_IsOnChange(
        object sender,
        FreeEcho.FEControls.ToggleSwitchEventArgs e
        )
    {
        try
        {
            Common.ApplicationData.Settings.MagnetInformation.PasteToScreenEdge = PasteToScreenEdgeToggleSwitch.IsOn;
        }
        catch
        {
            FreeEcho.FEControls.MessageBox.Show(Common.ApplicationData.Languages.Check, Common.ApplicationData.Languages.ErrorOccurred, FreeEcho.FEControls.MessageBoxButton.Ok, Window.GetWindow(this));
        }
    }

    /// <summary>
    /// 「別のウィンドウに貼り付ける」ToggleSwitchの「IsOnChange」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void PasteToAnotherWindowToggleSwitch_IsOnChange(
        object sender,
        FreeEcho.FEControls.ToggleSwitchEventArgs e
        )
    {
        try
        {
            Common.ApplicationData.Settings.MagnetInformation.PasteToAnotherWindow = PasteToAnotherWindowToggleSwitch.IsOn;
        }
        catch
        {
            FreeEcho.FEControls.MessageBox.Show(Common.ApplicationData.Languages.Check, Common.ApplicationData.Languages.ErrorOccurred, FreeEcho.FEControls.MessageBoxButton.Ok, Window.GetWindow(this));
        }
    }

    /// <summary>
    /// 「Ctrlキーを押した状態で貼り付ける」ToggleSwitchの「IsOnChange」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void PressTheKeyToPasteToggleSwitch_IsOnChange(
        object sender,
        FreeEcho.FEControls.ToggleSwitchEventArgs e
        )
    {
        try
        {
            Common.ApplicationData.Settings.MagnetInformation.PressTheKeyToPaste = PressTheKeyToPasteToggleSwitch.IsOn;
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「貼り付ける判定距離」NumericUpDownの「ChangeValue」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void DecisionDistanceToPasteNumericUpDown_ChangeValue(
        object sender,
        FreeEcho.FEControls.NumericUpDownChangeValueArgs e
        )
    {
        try
        {
            Common.ApplicationData.Settings.MagnetInformation.DecisionDistanceToPaste = DecisionDistanceToPasteNumericUpDown.ValueInt;
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「貼り付いた時の停止時間 (ミリ秒)」NumericUpDownの「ChangeValue」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void StopTimeWhenPastedNumericUpDown_ChangeValue(
        object sender,
        FreeEcho.FEControls.NumericUpDownChangeValueArgs e
        )
    {
        try
        {
            Common.ApplicationData.Settings.MagnetInformation.StopTimeWhenPasted = StopTimeWhenPastedNumericUpDown.ValueInt;
            Common.ApplicationData.DoProcessingEvent(ProcessingEventType.MagnetStopTimeWhenPastingChanged);
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
                    ProcessingStateToggleSwitch.IsOnDoNotEvent = Common.ApplicationData.Settings.MagnetInformation.Enabled;
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
    /// 「ウィンドウが全画面表示の場合は処理停止」ToggleSwitchの「IsOnChange」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void StopProcessingFullScreenToggleSwitch_IsOnChange(
        object sender,
        FreeEcho.FEControls.ToggleSwitchEventArgs e
        )
    {
        try
        {
            Common.ApplicationData.Settings.MagnetInformation.StopProcessingFullScreen = StopProcessingFullScreenToggleSwitch.IsOn;
            Common.ApplicationData.DoProcessingEvent(ProcessingEventType.StopWhenWindowIsFullScreenChanged);
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
            if (Common.ApplicationData.Languages.LanguagesWindow == null)
            {
                throw new Exception("LanguagesWindow value is null. - MagnetControl.SetTextOnControls()");
            }

            ProcessingStateToggleSwitch.Text = Common.ApplicationData.Languages.LanguagesWindow.ProcessingState;
            PasteToScreenEdgeToggleSwitch.Text = Common.ApplicationData.Languages.LanguagesWindow.PasteToTheEdgeOfScreen;
            PasteToAnotherWindowToggleSwitch.Text = Common.ApplicationData.Languages.LanguagesWindow.PasteIntoAnotherWindow;
            PressTheKeyToPasteToggleSwitch.Text = Common.ApplicationData.Languages.LanguagesWindow.PasteWithTheCtrlKeyPressed;
            DecisionDistanceToPasteLabel.Content = Common.ApplicationData.Languages.LanguagesWindow.DecisionDistanceToPaste;
            DecisionDistanceToPastePixelLabel.Content = Common.ApplicationData.Languages.LanguagesWindow.Pixel;
            StopTimeWhenPastedLabel.Content = Common.ApplicationData.Languages.LanguagesWindow.StopTimeWhenPasting;
            StopProcessingFullScreenToggleSwitch.Text = Common.ApplicationData.Languages.LanguagesWindow.StopProcessingWhenWindowIsFullScreen;
            ExplanationTextBlock.Text = Common.ApplicationData.Languages.LanguagesWindow.MagnetExplanation;
        }
        catch
        {
        }
    }
}
