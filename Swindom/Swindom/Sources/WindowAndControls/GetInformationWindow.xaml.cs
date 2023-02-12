namespace Swindom.Sources.WindowAndControls;

/// <summary>
/// ウィンドウ情報取得ウィンドウ
/// </summary>
public partial class GetInformationWindow : Window
{
    /// <summary>
    /// ウィンドウ情報のバッファ
    /// </summary>
    private readonly WindowInformationBuffer WindowInformationBuffer = new();
    /// <summary>
    /// ウィンドウ選択枠
    /// </summary>
    private readonly FreeEcho.FEWindowSelectionMouse.WindowSelectionFrame WindowSelectionMouse = new()
    {
        MouseLeftUpStop = true
    };

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public GetInformationWindow()
    {
        InitializeComponent();

        if (ApplicationData.MonitorInformation.MonitorInfo.Count == 1 || ApplicationData.Settings.CoordinateType == CoordinateType.PrimaryDisplay)
        {
            DisplayLabel.Visibility = Visibility.Collapsed;
            DisplayTextBox.Visibility = Visibility.Collapsed;
        }
        if (ApplicationData.Settings.DarkMode)
        {
            TargetImage.Source = new BitmapImage(new("/Resources/TargetDark.png", UriKind.Relative));
        }

        Title = ApplicationData.Languages.LanguagesWindow?.GetWindowInformation;
        TargetButton.ToolTip = ApplicationData.Languages.LanguagesWindow?.HoldDownMouseCursorMoveToSelectWindow;
        TitleNameLabel.Content = ApplicationData.Languages.LanguagesWindow?.TitleName;
        ClassNameLabel.Content = ApplicationData.Languages.LanguagesWindow?.ClassName;
        FileNameLabel.Content = ApplicationData.Languages.LanguagesWindow?.FileName;
        DisplayLabel.Content = ApplicationData.Languages.LanguagesWindow?.Display;
        XLabel.Content = ApplicationData.Languages.LanguagesWindow?.X;
        YLabel.Content = ApplicationData.Languages.LanguagesWindow?.Y;
        WidthLabel.Content = ApplicationData.Languages.LanguagesWindow?.Width;
        HeightLabel.Content = ApplicationData.Languages.LanguagesWindow?.Height;

        Loaded += GetInformationWindow_Loaded;
        TargetButton.PreviewMouseDown += SelectWindowTargetButton_PreviewMouseDown;
        WindowSelectionMouse.MouseLeftButtonUp += WindowSelectionMouse_MouseLeftButtonUp;
    }

    /// <summary>
    /// 「ContentRendered」イベント
    /// </summary>
    /// <param name="e"></param>
    protected override void OnContentRendered(
        EventArgs e
        )
    {
        base.OnContentRendered(e);
        // WPFの「SizeToContent」「WidthAndHeight」のバグ対策
        InvalidateMeasure();
    }

    /// <summary>
    /// ウィンドウの「Loaded」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void GetInformationWindow_Loaded(
        object sender,
        RoutedEventArgs e
        )
    {
        try
        {
            Activate();
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「ウィンドウ選択」Buttonの「PreviewMouseDown」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void SelectWindowTargetButton_PreviewMouseDown(
        object sender,
        MouseButtonEventArgs e
        )
    {
        try
        {
            WindowSelectionMouse.StartWindowSelection();
        }
        catch
        {
            FEMessageBox.Show(ApplicationData.Languages.ErrorOccurred, ApplicationData.Languages.Check, MessageBoxButton.OK);
        }
    }

    /// <summary>
    /// 「ウィンドウ選択」Buttonの「MouseLeftButtonUp」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void WindowSelectionMouse_MouseLeftButtonUp(
        object sender,
        FreeEcho.FEWindowSelectionMouse.MouseLeftButtonUpEventArgs e
        )
    {
        try
        {
            WindowSelectionMouse.StopWindowSelection();

            WindowInformation windowInformation = VariousWindowProcessing.GetWindowInformationFromHandle(WindowSelectionMouse.SelectedHwnd, WindowInformationBuffer);
            TitleNameTextBox.Text = windowInformation.TitleName;
            ClassNameTextBox.Text = windowInformation.ClassName;
            FileNameTextBox.Text = windowInformation.FileName;

            MonitorInformation.GetMonitorInformationOnWindowShown(windowInformation.Rectangle, out MonitorInfoEx monitorInfo);
            DisplayTextBox.Text = monitorInfo.DeviceName;

            if (ApplicationData.Settings.CoordinateType == CoordinateType.EachDisplay)
            {
                XTextBox.Text = (windowInformation.Rectangle.Left - monitorInfo.Monitor.Left).ToString();
                YTextBox.Text = (windowInformation.Rectangle.Top - monitorInfo.Monitor.Top).ToString();
                WidthTextBox.Text = (windowInformation.Rectangle.Right - windowInformation.Rectangle.Left).ToString();
                HeightTextBox.Text = (windowInformation.Rectangle.Bottom - windowInformation.Rectangle.Top).ToString();
            }
            else
            {
                XTextBox.Text = windowInformation.Rectangle.Left.ToString();
                YTextBox.Text = windowInformation.Rectangle.Top.ToString();
                WidthTextBox.Text = (windowInformation.Rectangle.Right - windowInformation.Rectangle.Left).ToString();
                HeightTextBox.Text = (windowInformation.Rectangle.Bottom - windowInformation.Rectangle.Top).ToString();
            }
        }
        catch
        {
            FEMessageBox.Show(ApplicationData.Languages.ErrorOccurred, ApplicationData.Languages.Check, MessageBoxButton.OK);
        }
    }
}
