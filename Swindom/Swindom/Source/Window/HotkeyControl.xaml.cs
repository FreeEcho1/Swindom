using Swindom.Source.Settings;

namespace Swindom.Source.Window
{
    /// <summary>
    /// ホットキーのコントロール
    /// </summary>
    public partial class HotkeyControl : System.Windows.Controls.UserControl, System.IDisposable
    {
        /// <summary>
        /// Disposeが呼ばれたかの値
        /// </summary>
        private bool Disposed;
        /// <summary>
        /// 「ホットキー」ウィンドウ処理の追加/修正ウィンドウ
        /// </summary>
        private AddModifyWindowHotkey AddModifyWindowHotkey;
        /// <summary>
        /// ウィンドウ選択枠
        /// </summary>
        private FreeEcho.FEWindowSelectionMouse.WindowSelectionFrame WindowSelectionMouse;

        /// <summary>
        /// 区切り文字列1
        /// </summary>
        private const string SeparateString1 = ":";
        /// <summary>
        /// 区切り文字列2
        /// </summary>
        private const string SeparateString2 = " | ";
        /// <summary>
        /// 区切り文字列3
        /// </summary>
        private const string SeparateString3 = " / ";
        /// <summary>
        /// ListBoxItemの高さ
        /// </summary>
        private const int ListBoxItemHeight = 50;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public HotkeyControl()
        {
            InitializeComponent();

            SettingsRowDefinition.Height = new(46);
            ModifyButton.IsEnabled = false;
            DeleteButton.IsEnabled = false;
            MoveUpButton.IsEnabled = false;
            MoveDownButton.IsEnabled = false;
            SelectWindowTargetButton.IsEnabled = false;
            SettingsLanguage();

            HotkeyListBox.SelectionChanged += HotkeyListBox_SelectionChanged;
            AdditionButton.Click += AdditionButton_Click;
            ModifyButton.Click += ModifyButton_Click;
            DeleteButton.Click += DeleteButton_Click;
            MoveUpButton.Click += MoveUpButton_Click;
            MoveDownButton.Click += MoveDownButton_Click;
            SelectWindowTargetButton.PreviewMouseDown += SelectWindowTargetButton_PreviewMouseDown;
            SettingsButton.Click += SettingsButton_Click;

            ProcessingStateToggleSwitch.IsOn = Common.ApplicationData.Settings.HotkeyInformation.Enabled;
            DoNotChangeOutOfScreenToggleSwitch.IsOn = Common.ApplicationData.Settings.HotkeyInformation.DoNotChangeOutOfScreen;
            StopProcessingFullScreenToggleSwitch.IsOn = Common.ApplicationData.Settings.HotkeyInformation.StopProcessingFullScreen;
            ProcessingStateToggleSwitch.IsOnChange += ProcessingStateToggleSwitch_IsOnChange;
            DoNotChangeOutOfScreenToggleSwitch.IsOnChange += DoNotChangeOutOfScreenToggleSwitch_IsOnChange;
            StopProcessingFullScreenToggleSwitch.IsOnChange += StopProcessingFullScreenToggleSwitch_IsOnChange;

            Common.ApplicationData.ProcessingEvent += ApplicationData_ProcessingEvent;
        }

        /// <summary>
        /// デストラクタ
        /// </summary>
        ~HotkeyControl()
        {
            Dispose(false);
        }

        /// <summary>
        /// Dispose
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            System.GC.SuppressFinalize(this);
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
                if (AddModifyWindowHotkey != null)
                {
                    AddModifyWindowHotkey.Close();
                }
            }
        }

        /// <summary>
        /// 「ホットキー項目」ListBoxの「SelectionChanged」イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HotkeyListBox_SelectionChanged(
            object sender,
            System.Windows.Controls.SelectionChangedEventArgs e
            )
        {
            try
            {
                if (HotkeyListBox.SelectedItems.Count == 0)
                {
                    ModifyButton.IsEnabled = false;
                    DeleteButton.IsEnabled = false;
                    MoveUpButton.IsEnabled = false;
                    MoveDownButton.IsEnabled = false;
                    SelectWindowTargetButton.IsEnabled = false;
                }
                else
                {
                    ModifyButton.IsEnabled = true;
                    DeleteButton.IsEnabled = true;
                    MoveUpButton.IsEnabled = HotkeyListBox.SelectedIndex != 0;
                    MoveDownButton.IsEnabled = (HotkeyListBox.SelectedIndex + 1) != HotkeyListBox.Items.Count;
                    SelectWindowTargetButton.IsEnabled = Common.ApplicationData.Settings.HotkeyInformation.Enabled;
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// 「追加」Buttonの「Click」イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AdditionButton_Click(
            object sender,
            System.Windows.RoutedEventArgs e
            )
        {
            try
            {
                if (AddModifyWindowHotkey == null)
                {
                    AddModifyWindowHotkey = new(System.Windows.Window.GetWindow(this));
                    AddModifyWindowHotkey.Closed += AddModifyWindowHotkey_Closed;
                    AddModifyWindowHotkey.Show();
                }
                else
                {
                    AddModifyWindowHotkey.Activate();
                }
            }
            catch
            {
                FreeEcho.FEControls.MessageBox.Show(Common.ApplicationData.Languages.Check, Common.ApplicationData.Languages.ErrorOccurred, FreeEcho.FEControls.MessageBoxButton.Ok, System.Windows.Window.GetWindow(this));
            }
        }

        /// <summary>
        /// 「修正」Buttonの「Click」イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ModifyButton_Click(
            object sender,
            System.Windows.RoutedEventArgs e
            )
        {
            try
            {
                if (AddModifyWindowHotkey == null)
                {
                    if (HotkeyListBox.SelectedItems.Count == 1)
                    {
                        AddModifyWindowHotkey = new(System.Windows.Window.GetWindow(this), HotkeyListBox.SelectedIndex);
                        AddModifyWindowHotkey.Closed += AddModifyWindowHotkey_Closed;
                        AddModifyWindowHotkey.Show();
                    }
                }
                else
                {
                    AddModifyWindowHotkey.Activate();
                }
            }
            catch
            {
                FreeEcho.FEControls.MessageBox.Show(Common.ApplicationData.Languages.Check, Common.ApplicationData.Languages.ErrorOccurred, FreeEcho.FEControls.MessageBoxButton.Ok, System.Windows.Window.GetWindow(this));
            }
        }

        /// <summary>
        /// 「追加/修正」ウィンドウの「Closed」イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddModifyWindowHotkey_Closed(
            object sender,
            System.EventArgs e
            )
        {
            try
            {
                if (AddModifyWindowHotkey != null)
                {
                    if (AddModifyWindowHotkey.AddedModified)
                    {
                        int selectedIndex = HotkeyListBox.SelectedIndex;
                        RenewalHotkeyListBox();
                        HotkeyListBox.SelectedIndex = selectedIndex;
                    }
                    AddModifyWindowHotkey.Owner.Activate();
                    AddModifyWindowHotkey = null;
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// 「削除」Buttonの「Click」イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DeleteButton_Click(
            object sender,
            System.Windows.RoutedEventArgs e
            )
        {
            try
            {
                if (HotkeyListBox.SelectedItems.Count == 1)
                {
                    if (FreeEcho.FEControls.MessageBox.Show(Common.ApplicationData.Languages.Check, Common.ApplicationData.Languages.LanguagesWindow.AllowDelete, FreeEcho.FEControls.MessageBoxButton.YesNo, System.Windows.Window.GetWindow(this)) == FreeEcho.FEControls.MessageBoxResult.Yes)
                    {
                        Common.ApplicationData.WindowProcessingManagement.Hotkey?.UnregisterHotkeys();
                        try
                        {
                            Common.ApplicationData.Settings.HotkeyInformation.Items.RemoveAt(HotkeyListBox.SelectedIndex);
                            HotkeyListBox.Items.RemoveAt(HotkeyListBox.SelectedIndex);
                            FreeEcho.FEControls.MessageBox.Show(Common.ApplicationData.Languages.Check, Common.ApplicationData.Languages.LanguagesWindow.Deleted, FreeEcho.FEControls.MessageBoxButton.Ok, System.Windows.Window.GetWindow(this));
                        }
                        catch
                        {
                            FreeEcho.FEControls.MessageBox.Show(Common.ApplicationData.Languages.Check, Common.ApplicationData.Languages.ErrorOccurred, FreeEcho.FEControls.MessageBoxButton.Ok, System.Windows.Window.GetWindow(this));
                        }
                        Common.ApplicationData.WindowProcessingManagement.Hotkey?.RegisterHotkeys();
                    }
                }
            }
            catch
            {
                FreeEcho.FEControls.MessageBox.Show(Common.ApplicationData.Languages.Check, Common.ApplicationData.Languages.ErrorOccurred, FreeEcho.FEControls.MessageBoxButton.Ok, System.Windows.Window.GetWindow(this));
            }
        }

        /// <summary>
        /// 「上に移動」Buttonの「Click」イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MoveUpButton_Click(
            object sender,
            System.Windows.RoutedEventArgs e
            )
        {
            try
            {
                if (HotkeyListBox.SelectedIndex != 0)
                {
                    int selectedIndex = HotkeyListBox.SelectedIndex;      // 選択している項目のインデックス
                    Common.ApplicationData.Settings.HotkeyInformation.Items.Reverse(selectedIndex - 1, 2);
                    RenewalHotkeyListBox();
                    HotkeyListBox.SelectedIndex = selectedIndex - 1;
                }
            }
            catch
            {
                FreeEcho.FEControls.MessageBox.Show(Common.ApplicationData.Languages.Check, Common.ApplicationData.Languages.ErrorOccurred, FreeEcho.FEControls.MessageBoxButton.Ok, System.Windows.Window.GetWindow(this));
            }
        }

        /// <summary>
        /// 「下に移動」Buttonの「Click」イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MoveDownButton_Click(
            object sender,
            System.Windows.RoutedEventArgs e
            )
        {
            try
            {
                if ((HotkeyListBox.SelectedIndex + 1) != HotkeyListBox.Items.Count)
                {
                    int selectedIndex = HotkeyListBox.SelectedIndex;      // 選択している項目のインデックス
                    Common.ApplicationData.Settings.HotkeyInformation.Items.Reverse(selectedIndex, 2);
                    RenewalHotkeyListBox();
                    HotkeyListBox.SelectedIndex = selectedIndex + 1;
                }
            }
            catch
            {
                FreeEcho.FEControls.MessageBox.Show(Common.ApplicationData.Languages.Check, Common.ApplicationData.Languages.ErrorOccurred, FreeEcho.FEControls.MessageBoxButton.Ok, System.Windows.Window.GetWindow(this));
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
                if (WindowSelectionMouse == null)
                {
                    System.Windows.Window.GetWindow(this).WindowState = System.Windows.WindowState.Minimized;
                    WindowSelectionMouse = new()
                    {
                        MouseLeftUpStop = true
                    };
                    WindowSelectionMouse.MouseLeftButtonUp += WindowSelectionMouse_MouseLeftButtonUp;
                    WindowSelectionMouse.StartWindowSelection();
                }
            }
            catch
            {
                System.Windows.Window.GetWindow(this).WindowState = System.Windows.WindowState.Normal;
                FreeEcho.FEControls.MessageBox.Show(Common.ApplicationData.Languages.Check, Common.ApplicationData.Languages.ErrorOccurred, FreeEcho.FEControls.MessageBoxButton.Ok, System.Windows.Window.GetWindow(this));
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

                if (HotkeyListBox.SelectedItems.Count == 1)
                {
                    Common.ApplicationData.WindowProcessingManagement.Hotkey.PerformHotkeyProcessing(WindowSelectionMouse.SelectedHwnd, Common.ApplicationData.Settings.HotkeyInformation.Items[HotkeyListBox.SelectedIndex], null);
                    System.Windows.Window.GetWindow(this).WindowState = System.Windows.WindowState.Normal;
                }
            }
            catch
            {
                System.Windows.Window.GetWindow(this).WindowState = System.Windows.WindowState.Normal;
                FreeEcho.FEControls.MessageBox.Show(Common.ApplicationData.Languages.Check, Common.ApplicationData.Languages.ErrorOccurred, FreeEcho.FEControls.MessageBoxButton.Ok, System.Windows.Window.GetWindow(this));
            }
            finally
            {
                WindowSelectionMouse = null;
            }
        }

        /// <summary>
        /// 「設定」Buttonの「Click」イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SettingsButton_Click(
            object sender,
            System.Windows.RoutedEventArgs e
            )
        {
            try
            {
                if ((int)SettingsRowDefinition.Height.Value == Common.SettingsRowDefinitionMinimize)
                {
                    ItemsRowDefinition.Height = new(0);
                    SettingsRowDefinition.Height = new(1, System.Windows.GridUnitType.Star);
                    SettingsButton.ImageSource = new System.Windows.Media.Imaging.BitmapImage(new("pack://application:,,,/SwindomImage;component/Resources/CloseSettings.png", System.UriKind.Absolute));
                }
                else
                {
                    ItemsRowDefinition.Height = new(1, System.Windows.GridUnitType.Star);
                    SettingsRowDefinition.Height = new(Common.SettingsRowDefinitionMinimize);
                    SettingsButton.ImageSource = new System.Windows.Media.Imaging.BitmapImage(new("pack://application:,,,/SwindomImage;component/Resources/Settings.png", System.UriKind.Absolute));
                    SettingsScrollViewer.ScrollToVerticalOffset(0);
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// 「処理状態」ToggleSwitchの「IsOnChange」イベント
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
                Common.ApplicationData.Settings.HotkeyInformation.Enabled = ProcessingStateToggleSwitch.IsOn;
                SelectWindowTargetButton.IsEnabled = (HotkeyListBox.SelectedItems.Count == 1) && Common.ApplicationData.Settings.HotkeyInformation.Enabled;
                Common.ApplicationData.DoProcessingEvent(ProcessingEventType.HotkeyValidState);
            }
            catch
            {
            }
        }

        /// <summary>
        /// 「画面外に出る場合は位置やサイズを変更しない」ToggleSwitchの「IsOnChange」イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DoNotChangeOutOfScreenToggleSwitch_IsOnChange(
            object sender,
            FreeEcho.FEControls.ToggleSwitchEventArgs e
            )
        {
            try
            {
                Common.ApplicationData.Settings.HotkeyInformation.DoNotChangeOutOfScreen = DoNotChangeOutOfScreenToggleSwitch.IsOn;
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
                Common.ApplicationData.Settings.HotkeyInformation.StopProcessingFullScreen = StopProcessingFullScreenToggleSwitch.IsOn;
                Common.ApplicationData.DoProcessingEvent(ProcessingEventType.StopWhenWindowIsFullScreenChanged);
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
            object sender,
            ProcessingEventArgs e
            )
        {
            try
            {
                switch (e.ProcessingEventType)
                {
                    case ProcessingEventType.HotkeyValidState:
                        ProcessingStateToggleSwitch.IsOnDoNotEvent = Common.ApplicationData.Settings.HotkeyInformation.Enabled;
                        break;
                    case ProcessingEventType.LanguageChanged:
                        SettingsLanguage();
                        break;
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// 言語設定
        /// </summary>
        private void SettingsLanguage()
        {
            try
            {
                RenewalHotkeyListBox();
                AdditionButton.Text = Common.ApplicationData.Languages.LanguagesWindow.Add;
                ModifyButton.Text = Common.ApplicationData.Languages.LanguagesWindow.Modify;
                DeleteButton.Text = Common.ApplicationData.Languages.LanguagesWindow.Delete;
                MoveUpButton.Text = Common.ApplicationData.Languages.LanguagesWindow.MoveUp;
                MoveDownButton.Text = Common.ApplicationData.Languages.LanguagesWindow.MoveDown;
                SelectWindowTargetButton.Text = Common.ApplicationData.Languages.LanguagesWindow.Select;
                SelectWindowTargetButton.ToolTip = Common.ApplicationData.Languages.LanguagesWindow.HoldDownMouseCursorMoveToSelectWindow;
                SettingsButton.Text = Common.ApplicationData.Languages.LanguagesWindow.Setting;
                ExplanationTextBlock.Text = Common.ApplicationData.Languages.LanguagesWindow.HotkeyExplanation;

                ProcessingStateToggleSwitch.Text = Common.ApplicationData.Languages.LanguagesWindow.ProcessingState;
                DoNotChangeOutOfScreenToggleSwitch.Text = Common.ApplicationData.Languages.LanguagesWindow.DoNotChangePositionSizeOutOfScreen;
                StopProcessingFullScreenToggleSwitch.Text = Common.ApplicationData.Languages.LanguagesWindow.StopProcessingWhenWindowIsFullScreen;
            }
            catch
            {
            }
        }

        /// <summary>
        /// 「ホットキー」ListBoxの項目を更新
        /// </summary>
        private void RenewalHotkeyListBox()
        {
            int selectedIndex = HotkeyListBox.SelectedIndex;       // 選択している項目のインデックス

            HotkeyListBox.Items.Clear();
            if (Common.ApplicationData.Settings.HotkeyInformation.Items.Count != 0)
            {
                foreach (HotkeyItemInformation nowHII in Common.ApplicationData.Settings.HotkeyInformation.Items)
                {
                    FreeEcho.FEControls.ListBoxItemValidState newItem = new()
                    {
                        Content = GetStringHotkeyInformation(nowHII),
                        Height = ListBoxItemHeight
                    };
                    HotkeyListBox.Items.Add(newItem);
                }
                HotkeyListBox.SelectedIndex = selectedIndex;
            }
        }

        /// <summary>
        /// 「ホットキー」ListBoxの項目に表示する文字列取得
        /// </summary>
        /// <param name="hotkeyItemInformation">「ホットキー」機能の項目情報</param>
        /// <returns>ホットキーのListBoxの項目に表示する文字列</returns>
        private string GetStringHotkeyInformation(
            HotkeyItemInformation hotkeyItemInformation
            )
        {
            string stringData = "";        // 文字列

            try
            {
                stringData += hotkeyItemInformation.RegisteredName + System.Environment.NewLine + "  ";
                switch (hotkeyItemInformation.ProcessingType)
                {
                    case HotkeyProcessingType.PositionSize:
                        string separateString = "";        // 区切り文字列

                        stringData += Common.ApplicationData.Languages.LanguagesWindow.SpecifyPositionAndSize + SeparateString2;
                        switch (hotkeyItemInformation.PositionSize.XType)
                        {
                            case WindowXType.Left:
                                stringData += Common.ApplicationData.Languages.LanguagesWindow.X + SeparateString1 + Common.ApplicationData.Languages.LanguagesWindow.LeftEdge;
                                separateString = SeparateString3;
                                break;
                            case WindowXType.Middle:
                                stringData += Common.ApplicationData.Languages.LanguagesWindow.X + SeparateString1 + Common.ApplicationData.Languages.LanguagesWindow.Middle;
                                separateString = SeparateString3;
                                break;
                            case WindowXType.Right:
                                stringData += Common.ApplicationData.Languages.LanguagesWindow.X + SeparateString1 + Common.ApplicationData.Languages.LanguagesWindow.RightEdge;
                                separateString = SeparateString3;
                                break;
                            case WindowXType.Value:
                                stringData += Common.ApplicationData.Languages.LanguagesWindow.X + SeparateString1 + hotkeyItemInformation.PositionSize.Position.X + " " + ((hotkeyItemInformation.PositionSize.XValueType == ValueType.Pixel) ? Common.ApplicationData.Languages.LanguagesWindow.Pixel : Common.ApplicationData.Languages.LanguagesWindow.Percent);
                                separateString = SeparateString3;
                                break;
                        }
                        switch (hotkeyItemInformation.PositionSize.YType)
                        {
                            case WindowYType.Top:
                                stringData += separateString + Common.ApplicationData.Languages.LanguagesWindow.Y + SeparateString1 + Common.ApplicationData.Languages.LanguagesWindow.TopEdge;
                                separateString = SeparateString3;
                                break;
                            case WindowYType.Middle:
                                stringData += separateString + Common.ApplicationData.Languages.LanguagesWindow.Y + SeparateString1 + Common.ApplicationData.Languages.LanguagesWindow.Middle;
                                separateString = SeparateString3;
                                break;
                            case WindowYType.Bottom:
                                stringData += separateString + Common.ApplicationData.Languages.LanguagesWindow.Y + SeparateString1 + Common.ApplicationData.Languages.LanguagesWindow.BottomEdge;
                                separateString = SeparateString3;
                                break;
                            case WindowYType.Value:
                                stringData += separateString + Common.ApplicationData.Languages.LanguagesWindow.Y + SeparateString1 + hotkeyItemInformation.PositionSize.Position.Y + " " + ((hotkeyItemInformation.PositionSize.YValueType == ValueType.Pixel) ? Common.ApplicationData.Languages.LanguagesWindow.Pixel : Common.ApplicationData.Languages.LanguagesWindow.Percent);
                                separateString = SeparateString3;
                                break;
                        }
                        switch (hotkeyItemInformation.PositionSize.WidthType)
                        {
                            case WindowSizeType.Value:
                                stringData += separateString + Common.ApplicationData.Languages.LanguagesWindow.Width + SeparateString1 + hotkeyItemInformation.PositionSize.Size.Width + " " + ((hotkeyItemInformation.PositionSize.WidthValueType == ValueType.Pixel) ? Common.ApplicationData.Languages.LanguagesWindow.Pixel : Common.ApplicationData.Languages.LanguagesWindow.Percent);
                                separateString = SeparateString3;
                                break;
                        }
                        switch (hotkeyItemInformation.PositionSize.HeightType)
                        {
                            case WindowSizeType.Value:
                                stringData += separateString + Common.ApplicationData.Languages.LanguagesWindow.Height + SeparateString1 + hotkeyItemInformation.PositionSize.Size.Height + " " + ((hotkeyItemInformation.PositionSize.HeightValueType == ValueType.Pixel) ? Common.ApplicationData.Languages.LanguagesWindow.Pixel : Common.ApplicationData.Languages.LanguagesWindow.Percent);
                                break;
                        }
                        break;
                    case HotkeyProcessingType.MoveX:
                        stringData += Common.ApplicationData.Languages.LanguagesWindow.MoveXCoordinate + SeparateString2 + hotkeyItemInformation.ProcessingValue + " " + Common.ApplicationData.Languages.LanguagesWindow.Pixel;
                        break;
                    case HotkeyProcessingType.MoveY:
                        stringData += Common.ApplicationData.Languages.LanguagesWindow.MoveYCoordinate + SeparateString2 + hotkeyItemInformation.ProcessingValue + " " + Common.ApplicationData.Languages.LanguagesWindow.Pixel;
                        break;
                    case HotkeyProcessingType.MoveXY:
                        stringData += Common.ApplicationData.Languages.LanguagesWindow.MoveXAndYCoordinate + SeparateString2 + hotkeyItemInformation.ProcessingValue + " " + Common.ApplicationData.Languages.LanguagesWindow.Pixel;
                        break;
                    case HotkeyProcessingType.IncreaseDecreaseWidth:
                        stringData += Common.ApplicationData.Languages.LanguagesWindow.IncreaseDecreaseWidth + SeparateString2 + hotkeyItemInformation.ProcessingValue + " " + Common.ApplicationData.Languages.LanguagesWindow.Pixel;
                        break;
                    case HotkeyProcessingType.IncreaseDecreaseHeight:
                        stringData += Common.ApplicationData.Languages.LanguagesWindow.IncreaseDecreaseHeight + SeparateString2 + hotkeyItemInformation.ProcessingValue + " " + Common.ApplicationData.Languages.LanguagesWindow.Pixel;
                        break;
                    case HotkeyProcessingType.IncreaseDecreaseWidthHeight:
                        stringData += Common.ApplicationData.Languages.LanguagesWindow.IncreaseDecreaseWidthAndHeight + SeparateString2 + hotkeyItemInformation.ProcessingValue + " " + Common.ApplicationData.Languages.LanguagesWindow.Pixel;
                        break;
                    case HotkeyProcessingType.StartStopEvent:
                        stringData += Common.ApplicationData.Languages.LanguagesWindow.StartStopProcessingOfEvent;
                        break;
                    case HotkeyProcessingType.StartStopTimer:
                        stringData += Common.ApplicationData.Languages.LanguagesWindow.StartStopProcessingOfTimer;
                        break;
                    case HotkeyProcessingType.StartStopMagnet:
                        stringData += Common.ApplicationData.Languages.LanguagesWindow.StartStopProcessingOfMagnet;
                        break;
                    case HotkeyProcessingType.AlwaysForefrontOrCancel:
                        stringData += Common.ApplicationData.Languages.LanguagesWindow.AlwaysShowOrCancelOnTop;
                        break;
                    case HotkeyProcessingType.SpecifyTransparencyOrCancel:
                        stringData += Common.ApplicationData.Languages.LanguagesWindow.SpecifyCancelTransparency + SeparateString2 + hotkeyItemInformation.ProcessingValue;
                        break;
                    case HotkeyProcessingType.BatchEvent:
                        stringData += Common.ApplicationData.Languages.LanguagesWindow.BatchProcessingOfEvent;
                        break;
                    case HotkeyProcessingType.OnlyActiveWindowEvent:
                        stringData += Common.ApplicationData.Languages.LanguagesWindow.OnlyActiveWindowEvent;
                        break;
                    case HotkeyProcessingType.BatchTimer:
                        stringData += Common.ApplicationData.Languages.LanguagesWindow.BatchProcessingOfTimer;
                        break;
                    case HotkeyProcessingType.OnlyActiveWindowTimer:
                        stringData += Common.ApplicationData.Languages.LanguagesWindow.OnlyActiveWindowTimer;
                        break;
                }
                stringData += System.Environment.NewLine + "  " + FreeEcho.FEHotKeyWPF.HotKeyWPF.GetHotKeyString(hotkeyItemInformation.Hotkey);
            }
            catch
            {
            }

            return (stringData);
        }
    }
}
