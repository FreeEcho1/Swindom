namespace Swindom;

/// <summary>
/// ウィンドウ情報取得ウィンドウ
/// </summary>
public partial class GetInformationWindow : Window
{
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
        TargetImage.Source = ImageProcessing.GetImageTarget();

        Title = ApplicationData.Languages.GetWindowInformation + WindowControlValue.CopySeparateString + ApplicationValue.ApplicationName;
        TargetButton.ToolTip = ApplicationData.Languages.HoldDownMousePointerMoveToSelectWindow;
        TitleNameLabel.Content = ApplicationData.Languages.TitleName;
        ClassNameLabel.Content = ApplicationData.Languages.ClassName;
        FileNameLabel.Content = ApplicationData.Languages.FileName;
        DisplayLabel.Content = ApplicationData.Languages.Display;
        VersionLabel.Content = ApplicationData.Languages.Version;
        XLabel.Content = ApplicationData.Languages.X;
        YLabel.Content = ApplicationData.Languages.Y;
        WidthLabel.Content = ApplicationData.Languages.Width;
        HeightLabel.Content = ApplicationData.Languages.Height;

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
            WindowInformation windowInformation = WindowProcessing.GetWindowInformationFromHandle(WindowSelectionMouse.SelectedHwnd);
            TitleNameTextBox.Text = windowInformation.TitleName;
            ClassNameTextBox.Text = windowInformation.ClassName;
            FileNameTextBox.Text = windowInformation.FileName;

            MonitorInformation.GetMonitorInformationOnWindowShown(windowInformation.Rectangle, out MonitorInfoEx monitorInfo);
            DisplayTextBox.Text = monitorInfo.DeviceName;

            VersionTextBox.Text = windowInformation.Version;

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
