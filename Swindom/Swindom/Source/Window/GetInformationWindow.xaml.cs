namespace Swindom.Source.Window
{
    /// <summary>
    /// ウィンドウ情報取得ウィンドウ
    /// </summary>
    public partial class GetInformationWindow : System.Windows.Window
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

            if (Common.ApplicationData.Languages.LanguagesWindow == null)
            {
                Common.ApplicationData.ReadLanguages();
            }

            MonitorInformation monitorInformation = MonitorInformation.GetMonitorInformation();
            if (monitorInformation.MonitorInfo.Count == 1 || Common.ApplicationData.Settings.CoordinateType == Settings.CoordinateType.Global)
            {
                DisplayLabel.Visibility = System.Windows.Visibility.Collapsed;
                DisplayTextBox.Visibility = System.Windows.Visibility.Collapsed;
            }

            Title = Common.ApplicationData.Languages.LanguagesWindow.GetWindowInformation;
            SelectWindowTargetButton.ToolTip = Common.ApplicationData.Languages.LanguagesWindow.HoldDownMouseCursorMoveToSelectWindow;
            TitleNameLabel.Content = Common.ApplicationData.Languages.LanguagesWindow.TitleName;
            ClassNameLabel.Content = Common.ApplicationData.Languages.LanguagesWindow.ClassName;
            FileNameLabel.Content = Common.ApplicationData.Languages.LanguagesWindow.FileName;
            DisplayLabel.Content = Common.ApplicationData.Languages.LanguagesWindow.Display;
            XLabel.Content = Common.ApplicationData.Languages.LanguagesWindow.X;
            YLabel.Content = Common.ApplicationData.Languages.LanguagesWindow.Y;
            WidthLabel.Content = Common.ApplicationData.Languages.LanguagesWindow.Width;
            HeightLabel.Content = Common.ApplicationData.Languages.LanguagesWindow.Height;

            Loaded += GetInformationWindow_Loaded;
            SelectWindowTargetButton.PreviewMouseDown += SelectWindowTargetButton_PreviewMouseDown;
            WindowSelectionMouse.MouseLeftButtonUp += WindowSelectionMouse_MouseLeftButtonUp;
        }

        /// <summary>
        /// ウィンドウの「Loaded」イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GetInformationWindow_Loaded(
            object sender,
            System.Windows.RoutedEventArgs e
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
            System.Windows.Input.MouseButtonEventArgs e
            )
        {
            try
            {
                WindowSelectionMouse.StartWindowSelection();
            }
            catch
            {
                FreeEcho.FEControls.MessageBox.Show(Common.ApplicationData.Languages.Check, Common.ApplicationData.Languages.ErrorOccurred, FreeEcho.FEControls.MessageBoxButton.Ok, this);
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

                WindowInformation windowInformation = WindowProcessing.GetWindowInformation(WindowSelectionMouse.SelectedHwnd);
                TitleNameTextBox.Text = windowInformation.TitleName;
                ClassNameTextBox.Text = windowInformation.ClassName;
                FileNameTextBox.Text = windowInformation.FileName;

                MonitorInfoEx monitorInfo;
                MonitorInformation.GetMonitorWithWindow(WindowSelectionMouse.SelectedHwnd, out monitorInfo, null);
                DisplayTextBox.Text = monitorInfo.DeviceName;

                // ウィンドウの位置とサイズ
                WINDOWPLACEMENT windowPlacement;
                NativeMethods.GetWindowPlacement(WindowSelectionMouse.SelectedHwnd, out windowPlacement);
                switch (Common.ApplicationData.Settings.CoordinateType)
                {
                    case Settings.CoordinateType.Global:
                        XTextBox.Text = windowPlacement.rcNormalPosition.Left.ToString();
                        YTextBox.Text = windowPlacement.rcNormalPosition.Top.ToString();
                        WidthTextBox.Text = (windowPlacement.rcNormalPosition.Right - windowPlacement.rcNormalPosition.Left).ToString();
                        HeightTextBox.Text = (windowPlacement.rcNormalPosition.Bottom - windowPlacement.rcNormalPosition.Top).ToString();
                        break;
                    default:
                        XTextBox.Text = (windowPlacement.rcNormalPosition.Left - monitorInfo.Monitor.Left).ToString();
                        YTextBox.Text = (windowPlacement.rcNormalPosition.Top - monitorInfo.Monitor.Top).ToString();
                        WidthTextBox.Text = (windowPlacement.rcNormalPosition.Right - windowPlacement.rcNormalPosition.Left).ToString();
                        HeightTextBox.Text = (windowPlacement.rcNormalPosition.Bottom - windowPlacement.rcNormalPosition.Top).ToString();
                        break;
                }
            }
            catch
            {
                FreeEcho.FEControls.MessageBox.Show(Common.ApplicationData.Languages.Check, Common.ApplicationData.Languages.ErrorOccurred, FreeEcho.FEControls.MessageBoxButton.Ok, this);
            }
        }
    }
}
