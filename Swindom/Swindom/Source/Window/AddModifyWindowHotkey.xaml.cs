using Swindom.Source.Settings;

namespace Swindom.Source.Window
{
    /// <summary>
    /// 「ホットキー」の「追加/修正」ウィンドウ
    /// </summary>
    public partial class AddModifyWindowHotkey : System.Windows.Window
    {
        /// <summary>
        /// 修正する項目のインデックス (追加「-1」)
        /// </summary>
        private readonly int IndexOfItemToBeModified = -1;
        /// <summary>
        /// 追加/修正したかの値 (いいえ「false」/はい「true」)
        /// </summary>
        public bool AddedModified;
        /// <summary>
        /// 「ホットキー」機能の項目情報
        /// </summary>
        private readonly HotkeyItemInformation HotkeyItemInformation = new();

        /// <summary>
        /// コンストラクタ (使用しない)
        /// </summary>
        public AddModifyWindowHotkey()
        {
            throw new System.Exception("Do not use. - AddModifyWindowHotkey()");
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="ownerWindow">オーナーウィンドウ</param>
        /// <param name="indexOfItemToBeModified">修正する項目のインデックス (追加「-1」)</param>
        public AddModifyWindowHotkey(
            System.Windows.Window ownerWindow,
            int indexOfItemToBeModified = -1
            )
        {
            InitializeComponent();

            IndexOfItemToBeModified = indexOfItemToBeModified;
            if (IndexOfItemToBeModified != -1)
            {
                HotkeyItemInformation.Copy(Common.ApplicationData.Settings.HotkeyInformation.Items[IndexOfItemToBeModified]);
            }
            Owner = ownerWindow;
            SizeToContent = System.Windows.SizeToContent.Manual;
            MonitorInformation monitorInformation = MonitorInformation.GetMonitorInformation();
            if (monitorInformation.MonitorInfo.Count == 1)
            {
                MaxHeight = 678;
            }
            Width = (Common.ApplicationData.Settings.HotkeyInformation.AddModifyWindowSize.Width < MinWidth) ? MinWidth : Common.ApplicationData.Settings.HotkeyInformation.AddModifyWindowSize.Width;
            Height = (Common.ApplicationData.Settings.HotkeyInformation.AddModifyWindowSize.Height < MinHeight) ? MinHeight : Common.ApplicationData.Settings.HotkeyInformation.AddModifyWindowSize.Height;
            if ((Common.ApplicationData.Settings.CoordinateType == CoordinateType.Global) || (monitorInformation.MonitorInfo.Count == 1))
            {
                DisplayGroupBox.Visibility = System.Windows.Visibility.Collapsed;
            }
            foreach (MonitorInfoEx nowMonitorInfo in monitorInformation.MonitorInfo)
            {
                System.Windows.Controls.ComboBoxItem newItem = new()
                {
                    Content = nowMonitorInfo.DeviceName
                };
                DisplayComboBox.Items.Add(newItem);
            }

            if (IndexOfItemToBeModified == -1)
            {
                Title = Common.ApplicationData.Languages.LanguagesWindow.Add;
                AddModifyButton.Content = Common.ApplicationData.Languages.LanguagesWindow.Add;
            }
            else
            {
                Title = Common.ApplicationData.Languages.LanguagesWindow.Modify;
                AddModifyButton.Content = Common.ApplicationData.Languages.LanguagesWindow.Modify;
            }
            RegisteredNameLabel.Content = Common.ApplicationData.Languages.LanguagesWindow.RegisteredName;
            ProcessingTypeLabel.Content = Common.ApplicationData.Languages.LanguagesWindow.ProcessingType;
            ((System.Windows.Controls.ComboBoxItem)ProcessingTypeComboBox.Items[0]).Content = Common.ApplicationData.Languages.LanguagesWindow.SpecifyPositionAndSize;
            ((System.Windows.Controls.ComboBoxItem)ProcessingTypeComboBox.Items[1]).Content = Common.ApplicationData.Languages.LanguagesWindow.MoveXCoordinate;
            ((System.Windows.Controls.ComboBoxItem)ProcessingTypeComboBox.Items[2]).Content = Common.ApplicationData.Languages.LanguagesWindow.MoveYCoordinate;
            ((System.Windows.Controls.ComboBoxItem)ProcessingTypeComboBox.Items[3]).Content = Common.ApplicationData.Languages.LanguagesWindow.MoveXAndYCoordinate;
            ((System.Windows.Controls.ComboBoxItem)ProcessingTypeComboBox.Items[4]).Content = Common.ApplicationData.Languages.LanguagesWindow.IncreaseDecreaseWidth;
            ((System.Windows.Controls.ComboBoxItem)ProcessingTypeComboBox.Items[5]).Content = Common.ApplicationData.Languages.LanguagesWindow.IncreaseDecreaseHeight;
            ((System.Windows.Controls.ComboBoxItem)ProcessingTypeComboBox.Items[6]).Content = Common.ApplicationData.Languages.LanguagesWindow.IncreaseDecreaseWidthAndHeight;
            ((System.Windows.Controls.ComboBoxItem)ProcessingTypeComboBox.Items[7]).Content = Common.ApplicationData.Languages.LanguagesWindow.AlwaysShowOrCancelOnTop;
            ((System.Windows.Controls.ComboBoxItem)ProcessingTypeComboBox.Items[8]).Content = Common.ApplicationData.Languages.LanguagesWindow.SpecifyCancelTransparency;
            ((System.Windows.Controls.ComboBoxItem)ProcessingTypeComboBox.Items[9]).Content = Common.ApplicationData.Languages.LanguagesWindow.StartStopProcessingOfEvent;
            ((System.Windows.Controls.ComboBoxItem)ProcessingTypeComboBox.Items[10]).Content = Common.ApplicationData.Languages.LanguagesWindow.BatchProcessingOfEvent;
            ((System.Windows.Controls.ComboBoxItem)ProcessingTypeComboBox.Items[11]).Content = Common.ApplicationData.Languages.LanguagesWindow.OnlyActiveWindowEvent;
            ((System.Windows.Controls.ComboBoxItem)ProcessingTypeComboBox.Items[12]).Content = Common.ApplicationData.Languages.LanguagesWindow.StartStopProcessingOfTimer;
            ((System.Windows.Controls.ComboBoxItem)ProcessingTypeComboBox.Items[13]).Content = Common.ApplicationData.Languages.LanguagesWindow.BatchProcessingOfTimer;
            ((System.Windows.Controls.ComboBoxItem)ProcessingTypeComboBox.Items[14]).Content = Common.ApplicationData.Languages.LanguagesWindow.OnlyActiveWindowTimer;
            ((System.Windows.Controls.ComboBoxItem)ProcessingTypeComboBox.Items[15]).Content = Common.ApplicationData.Languages.LanguagesWindow.StartStopProcessingOfMagnet;
            DisplayGroupBox.Header = Common.ApplicationData.Languages.LanguagesWindow.Display;
            StandardDisplayLabel.Content = Common.ApplicationData.Languages.LanguagesWindow.DisplayToUseAsStandard;
            ((System.Windows.Controls.ComboBoxItem)StandardDisplayComboBox.Items[0]).Content = Common.ApplicationData.Languages.LanguagesWindow.DisplayWithWindow;
            ((System.Windows.Controls.ComboBoxItem)StandardDisplayComboBox.Items[1]).Content = Common.ApplicationData.Languages.LanguagesWindow.TheSpecifiedDisplay;
            DisplayLabel.Content = Common.ApplicationData.Languages.LanguagesWindow.Display;
            PositionSizeGroupBox.Header = Common.ApplicationData.Languages.LanguagesWindow.PositionAndSize;
            XLabel.Content = Common.ApplicationData.Languages.LanguagesWindow.X;
            ((System.Windows.Controls.ComboBoxItem)XComboBox.Items[0]).Content = Common.ApplicationData.Languages.LanguagesWindow.DoNotChange;
            ((System.Windows.Controls.ComboBoxItem)XComboBox.Items[1]).Content = Common.ApplicationData.Languages.LanguagesWindow.LeftEdge;
            ((System.Windows.Controls.ComboBoxItem)XComboBox.Items[2]).Content = Common.ApplicationData.Languages.LanguagesWindow.Middle;
            ((System.Windows.Controls.ComboBoxItem)XComboBox.Items[3]).Content = Common.ApplicationData.Languages.LanguagesWindow.RightEdge;
            ((System.Windows.Controls.ComboBoxItem)XComboBox.Items[4]).Content = Common.ApplicationData.Languages.LanguagesWindow.CoordinateSpecification;
            ((System.Windows.Controls.ComboBoxItem)XTypeComboBox.Items[0]).Content = Common.ApplicationData.Languages.LanguagesWindow.Pixel;
            ((System.Windows.Controls.ComboBoxItem)XTypeComboBox.Items[1]).Content = Common.ApplicationData.Languages.LanguagesWindow.Percent;
            YLabel.Content = Common.ApplicationData.Languages.LanguagesWindow.Y;
            ((System.Windows.Controls.ComboBoxItem)YComboBox.Items[0]).Content = Common.ApplicationData.Languages.LanguagesWindow.DoNotChange;
            ((System.Windows.Controls.ComboBoxItem)YComboBox.Items[1]).Content = Common.ApplicationData.Languages.LanguagesWindow.TopEdge;
            ((System.Windows.Controls.ComboBoxItem)YComboBox.Items[2]).Content = Common.ApplicationData.Languages.LanguagesWindow.Middle;
            ((System.Windows.Controls.ComboBoxItem)YComboBox.Items[3]).Content = Common.ApplicationData.Languages.LanguagesWindow.BottomEdge;
            ((System.Windows.Controls.ComboBoxItem)YComboBox.Items[4]).Content = Common.ApplicationData.Languages.LanguagesWindow.CoordinateSpecification;
            ((System.Windows.Controls.ComboBoxItem)YTypeComboBox.Items[0]).Content = Common.ApplicationData.Languages.LanguagesWindow.Pixel;
            ((System.Windows.Controls.ComboBoxItem)YTypeComboBox.Items[1]).Content = Common.ApplicationData.Languages.LanguagesWindow.Percent;
            WidthLabel.Content = Common.ApplicationData.Languages.LanguagesWindow.Width;
            ((System.Windows.Controls.ComboBoxItem)WidthComboBox.Items[0]).Content = Common.ApplicationData.Languages.LanguagesWindow.DoNotChange;
            ((System.Windows.Controls.ComboBoxItem)WidthComboBox.Items[1]).Content = Common.ApplicationData.Languages.LanguagesWindow.WidthSpecification;
            ((System.Windows.Controls.ComboBoxItem)WidthTypeComboBox.Items[0]).Content = Common.ApplicationData.Languages.LanguagesWindow.Pixel;
            ((System.Windows.Controls.ComboBoxItem)WidthTypeComboBox.Items[1]).Content = Common.ApplicationData.Languages.LanguagesWindow.Percent;
            HeightLabel.Content = Common.ApplicationData.Languages.LanguagesWindow.Height;
            ((System.Windows.Controls.ComboBoxItem)HeightComboBox.Items[0]).Content = Common.ApplicationData.Languages.LanguagesWindow.DoNotChange;
            ((System.Windows.Controls.ComboBoxItem)HeightComboBox.Items[1]).Content = Common.ApplicationData.Languages.LanguagesWindow.HeightSpecification;
            ((System.Windows.Controls.ComboBoxItem)HeightTypeComboBox.Items[0]).Content = Common.ApplicationData.Languages.LanguagesWindow.Pixel;
            ((System.Windows.Controls.ComboBoxItem)HeightTypeComboBox.Items[1]).Content = Common.ApplicationData.Languages.LanguagesWindow.Percent;
            ClientAreaToggleSwitch.Text = Common.ApplicationData.Languages.LanguagesWindow.ClientArea;
            ProcessingPositionAndSizeTwiceToggleSwitch.Text = Common.ApplicationData.Languages.LanguagesWindow.ProcessingPositionAndSizeTwice;
            AmountOfMovementLabel.Content = Common.ApplicationData.Languages.LanguagesWindow.AmountOfMovement;
            AmountOfMovementPixelLabel.Content = Common.ApplicationData.Languages.LanguagesWindow.Pixel;
            SizeChangeAmountLabel.Content = Common.ApplicationData.Languages.LanguagesWindow.SizeChangeAmount;
            SizeChangeAmountPixelLabel.Content = Common.ApplicationData.Languages.LanguagesWindow.Pixel;
            TransparencyLabel.Content = Common.ApplicationData.Languages.LanguagesWindow.Transparency;
            HotkeyLabel.Content = Common.ApplicationData.Languages.LanguagesWindow.Hotkey;
            CancelButton.Content = Common.ApplicationData.Languages.LanguagesWindow.Cancel;

            string stringData;
            RegisteredNameTextBox.Text = HotkeyItemInformation.RegisteredName;
            switch (HotkeyItemInformation.ProcessingType)
            {
                case HotkeyProcessingType.MoveX:
                    stringData = Common.ApplicationData.Languages.LanguagesWindow.MoveXCoordinate;
                    break;
                case HotkeyProcessingType.MoveY:
                    stringData = Common.ApplicationData.Languages.LanguagesWindow.MoveYCoordinate;
                    break;
                case HotkeyProcessingType.MoveXY:
                    stringData = Common.ApplicationData.Languages.LanguagesWindow.MoveXAndYCoordinate;
                    break;
                case HotkeyProcessingType.IncreaseDecreaseWidth:
                    stringData = Common.ApplicationData.Languages.LanguagesWindow.IncreaseDecreaseWidth;
                    break;
                case HotkeyProcessingType.IncreaseDecreaseHeight:
                    stringData = Common.ApplicationData.Languages.LanguagesWindow.IncreaseDecreaseHeight;
                    break;
                case HotkeyProcessingType.IncreaseDecreaseWidthHeight:
                    stringData = Common.ApplicationData.Languages.LanguagesWindow.IncreaseDecreaseWidthAndHeight;
                    break;
                case HotkeyProcessingType.AlwaysForefrontOrCancel:
                    stringData = Common.ApplicationData.Languages.LanguagesWindow.AlwaysShowOrCancelOnTop;
                    break;
                case HotkeyProcessingType.SpecifyTransparencyOrCancel:
                    stringData = Common.ApplicationData.Languages.LanguagesWindow.SpecifyCancelTransparency;
                    break;
                case HotkeyProcessingType.StartStopEvent:
                    stringData = Common.ApplicationData.Languages.LanguagesWindow.StartStopProcessingOfEvent;
                    break;
                case HotkeyProcessingType.BatchEvent:
                    stringData = Common.ApplicationData.Languages.LanguagesWindow.BatchProcessingOfEvent;
                    break;
                case HotkeyProcessingType.OnlyActiveWindowEvent:
                    stringData = Common.ApplicationData.Languages.LanguagesWindow.OnlyActiveWindowEvent;
                    break;
                case HotkeyProcessingType.StartStopTimer:
                    stringData = Common.ApplicationData.Languages.LanguagesWindow.StartStopProcessingOfTimer;
                    break;
                case HotkeyProcessingType.BatchTimer:
                    stringData = Common.ApplicationData.Languages.LanguagesWindow.BatchProcessingOfTimer;
                    break;
                case HotkeyProcessingType.OnlyActiveWindowTimer:
                    stringData = Common.ApplicationData.Languages.LanguagesWindow.OnlyActiveWindowTimer;
                    break;
                case HotkeyProcessingType.StartStopMagnet:
                    stringData = Common.ApplicationData.Languages.LanguagesWindow.StartStopProcessingOfMagnet;
                    break;
                default:
                    stringData = Common.ApplicationData.Languages.LanguagesWindow.SpecifyPositionAndSize;
                    break;
            }
            Processing.SelectComboBoxItem(ProcessingTypeComboBox, stringData);
            switch (HotkeyItemInformation.StandardDisplay)
            {
                case StandardDisplay.DisplayWithWindow:
                    stringData = Common.ApplicationData.Languages.LanguagesWindow.DisplayWithWindow;
                    DisplayLabel.IsEnabled = false;
                    DisplayComboBox.IsEnabled = false;
                    break;
                default:
                    stringData = Common.ApplicationData.Languages.LanguagesWindow.TheSpecifiedDisplay;
                    break;
            }
            Processing.SelectComboBoxItem(StandardDisplayComboBox, stringData);
            Processing.SelectComboBoxItem(DisplayComboBox, HotkeyItemInformation.PositionSize.Display);
            if (DisplayComboBox.SelectedIndex == -1)
            {
                DisplayComboBox.SelectedIndex = 0;
            }
            switch (HotkeyItemInformation.PositionSize.XType)
            {
                case WindowXType.Left:
                    stringData = Common.ApplicationData.Languages.LanguagesWindow.LeftEdge;
                    break;
                case WindowXType.Middle:
                    stringData = Common.ApplicationData.Languages.LanguagesWindow.Middle;
                    break;
                case WindowXType.Right:
                    stringData = Common.ApplicationData.Languages.LanguagesWindow.RightEdge;
                    break;
                case WindowXType.Value:
                    stringData = Common.ApplicationData.Languages.LanguagesWindow.CoordinateSpecification;
                    break;
                default:
                    stringData = Common.ApplicationData.Languages.LanguagesWindow.DoNotChange;
                    break;
            }
            Processing.SelectComboBoxItem(XComboBox, stringData);
            switch (HotkeyItemInformation.PositionSize.XValueType)
            {
                case ValueType.Percent:
                    stringData = Common.ApplicationData.Languages.LanguagesWindow.Percent;
                    XNumericUpDown.IsUseDecimal = true;
                    break;
                default:
                    stringData = Common.ApplicationData.Languages.LanguagesWindow.Pixel;
                    XNumericUpDown.IsUseDecimal = false;
                    break;
            }
            Processing.SelectComboBoxItem(XTypeComboBox, stringData);
            XNumericUpDown.Value = HotkeyItemInformation.PositionSize.Position.X;
            switch (HotkeyItemInformation.PositionSize.YType)
            {
                case WindowYType.Top:
                    stringData = Common.ApplicationData.Languages.LanguagesWindow.TopEdge;
                    break;
                case WindowYType.Middle:
                    stringData = Common.ApplicationData.Languages.LanguagesWindow.Middle;
                    break;
                case WindowYType.Bottom:
                    stringData = Common.ApplicationData.Languages.LanguagesWindow.BottomEdge;
                    break;
                case WindowYType.Value:
                    stringData = Common.ApplicationData.Languages.LanguagesWindow.CoordinateSpecification;
                    break;
                default:
                    stringData = Common.ApplicationData.Languages.LanguagesWindow.DoNotChange;
                    break;
            }
            Processing.SelectComboBoxItem(YComboBox, stringData);
            switch (HotkeyItemInformation.PositionSize.YValueType)
            {
                case ValueType.Percent:
                    stringData = Common.ApplicationData.Languages.LanguagesWindow.Percent;
                    YNumericUpDown.IsUseDecimal = true;
                    break;
                default:
                    stringData = Common.ApplicationData.Languages.LanguagesWindow.Pixel;
                    YNumericUpDown.IsUseDecimal = false;
                    break;
            }
            Processing.SelectComboBoxItem(YTypeComboBox, stringData);
            YNumericUpDown.Value = HotkeyItemInformation.PositionSize.Position.Y;
            switch (HotkeyItemInformation.PositionSize.WidthType)
            {
                case WindowSizeType.Value:
                    stringData = Common.ApplicationData.Languages.LanguagesWindow.WidthSpecification;
                    break;
                default:
                    stringData = Common.ApplicationData.Languages.LanguagesWindow.DoNotChange;
                    break;
            }
            Processing.SelectComboBoxItem(WidthComboBox, stringData);
            switch (HotkeyItemInformation.PositionSize.WidthValueType)
            {
                case ValueType.Percent:
                    stringData = Common.ApplicationData.Languages.LanguagesWindow.Percent;
                    WidthNumericUpDown.IsUseDecimal = true;
                    break;
                default:
                    stringData = Common.ApplicationData.Languages.LanguagesWindow.Pixel;
                    WidthNumericUpDown.IsUseDecimal = false;
                    break;
            }
            Processing.SelectComboBoxItem(WidthTypeComboBox, stringData);
            WidthNumericUpDown.Value = HotkeyItemInformation.PositionSize.Size.Width;
            switch (HotkeyItemInformation.PositionSize.HeightType)
            {
                case WindowSizeType.Value:
                    stringData = Common.ApplicationData.Languages.LanguagesWindow.HeightSpecification;
                    break;
                default:
                    stringData = Common.ApplicationData.Languages.LanguagesWindow.DoNotChange;
                    break;
            }
            Processing.SelectComboBoxItem(HeightComboBox, stringData);
            switch (HotkeyItemInformation.PositionSize.HeightValueType)
            {
                case ValueType.Percent:
                    stringData = Common.ApplicationData.Languages.LanguagesWindow.Percent;
                    HeightNumericUpDown.IsUseDecimal = true;
                    break;
                default:
                    stringData = Common.ApplicationData.Languages.LanguagesWindow.Pixel;
                    HeightNumericUpDown.IsUseDecimal = false;
                    break;
            }
            Processing.SelectComboBoxItem(HeightTypeComboBox, stringData);
            HeightNumericUpDown.Value = HotkeyItemInformation.PositionSize.Size.Height;
            ClientAreaToggleSwitch.IsOn = HotkeyItemInformation.PositionSize.ClientArea;
            ProcessingPositionAndSizeTwiceToggleSwitch.IsOn = HotkeyItemInformation.PositionSize.ProcessingPositionAndSizeTwice;
            switch (HotkeyItemInformation.ProcessingType)
            {
                case HotkeyProcessingType.MoveX:
                case HotkeyProcessingType.MoveY:
                case HotkeyProcessingType.MoveXY:
                    AmountOfMovementNumericUpDown.Value = HotkeyItemInformation.ProcessingValue;
                    break;
                case HotkeyProcessingType.IncreaseDecreaseWidth:
                case HotkeyProcessingType.IncreaseDecreaseHeight:
                case HotkeyProcessingType.IncreaseDecreaseWidthHeight:
                    SizeChangeAmountNumericUpDown.Value = HotkeyItemInformation.ProcessingValue;
                    break;
                case HotkeyProcessingType.SpecifyTransparencyOrCancel:
                    TransparencyNumericUpDown.Value = HotkeyItemInformation.ProcessingValue;
                    break;
            }
            HotkeyTextBox.Text = FreeEcho.FEHotKeyWPF.HotKeyWPF.GetHotKeyString(HotkeyItemInformation.Hotkey);
            SettingsControlEnabled();
            SettingsAddModifyButtonEnabled();

            Closed += HotkeyWindowProcessingAddModifyWindow_Closed;
            RegisteredNameTextBox.TextChanged += RegisteredNameTextBox_TextChanged;
            ProcessingTypeComboBox.SelectionChanged += ProcessingTypeComboBox_SelectionChanged;
            StandardDisplayComboBox.SelectionChanged += StandardDisplayComboBox_SelectionChanged;
            XComboBox.SelectionChanged += XComboBox_SelectionChanged;
            XTypeComboBox.SelectionChanged += ComboBoxXType_SelectionChanged;
            YComboBox.SelectionChanged += ComboBoxY_SelectionChanged;
            YTypeComboBox.SelectionChanged += YTypeComboBox_SelectionChanged;
            WidthComboBox.SelectionChanged += WidthComboBox_SelectionChanged;
            WidthTypeComboBox.SelectionChanged += WidthTypeComboBox_SelectionChanged;
            HeightComboBox.SelectionChanged += HeightComboBox_SelectionChanged;
            HeightTypeComboBox.SelectionChanged += HeightTypeComboBox_SelectionChanged;
            HotkeyTextBox.GotFocus += HotkeyTextBox_GotFocus;
            HotkeyTextBox.LostFocus += HotkeyTextBox_LostFocus;
            HotkeyTextBox.PreviewKeyDown += HotkeyTextBox_PreviewKeyDown;
            AddModifyButton.Click += AddModifyButton_Click;
            CancelButton.Click += CancelButton_Click;
        }

        /// <summary>
        /// ウィンドウの「Closed」イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HotkeyWindowProcessingAddModifyWindow_Closed(
            object sender,
            System.EventArgs e
            )
        {
            try
            {
                Common.ApplicationData.Settings.HotkeyInformation.AddModifyWindowSize.Width = (int)Width;
                Common.ApplicationData.Settings.HotkeyInformation.AddModifyWindowSize.Height = (int)Height;
            }
            catch
            {
            }
        }

        /// <summary>
        /// 「登録名」TextBoxの「TextChanged」イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RegisteredNameTextBox_TextChanged(
            object sender,
            System.Windows.Controls.TextChangedEventArgs e
            )
        {
            try
            {
                SettingsAddModifyButtonEnabled();
            }
            catch
            {
            }
        }

        /// <summary>
        /// 「処理の種類」ComboBoxの「SelectionChanged」イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ProcessingTypeComboBox_SelectionChanged(
            object sender,
            System.Windows.Controls.SelectionChangedEventArgs e
            )
        {
            try
            {
                SettingsControlEnabled();
            }
            catch
            {
            }
        }

        /// <summary>
        /// 「基準にするディスプレイ」ComboBoxの「SelectionChanged」イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StandardDisplayComboBox_SelectionChanged(
            object sender,
            System.Windows.Controls.SelectionChangedEventArgs e
            )
        {
            try
            {
                if ((string)((System.Windows.Controls.ComboBoxItem)StandardDisplayComboBox.SelectedItem).Content == Common.ApplicationData.Languages.LanguagesWindow.DisplayWithWindow)
                {
                    DisplayLabel.IsEnabled = false;
                    DisplayComboBox.IsEnabled = false;
                }
                else
                {
                    DisplayLabel.IsEnabled = true;
                    DisplayComboBox.IsEnabled = true;
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// 「X」ComboBoxの「SelectionChanged」イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void XComboBox_SelectionChanged(
            object sender,
            System.Windows.Controls.SelectionChangedEventArgs e
            )
        {
            try
            {
                SettingsXControlEnabled();
            }
            catch
            {
            }
        }

        /// <summary>
        /// 「X」の「値の種類」ComboBoxの「SelectionChanged」イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBoxXType_SelectionChanged(
            object sender,
            System.Windows.Controls.SelectionChangedEventArgs e
            )
        {
            try
            {
                if ((string)((System.Windows.Controls.ComboBoxItem)XTypeComboBox.SelectedItem).Content == Common.ApplicationData.Languages.LanguagesWindow.Percent)
                {
                    XNumericUpDown.IsUseDecimal = true;
                    XNumericUpDown.MinimumValue = Common.PercentMinimize;
                    XNumericUpDown.MaximumValue = Common.PercentMaximize;
                }
                else
                {
                    XNumericUpDown.IsUseDecimal = false;
                    XNumericUpDown.MinimumValue = int.MinValue;
                    XNumericUpDown.MaximumValue = int.MaxValue;
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// 「Y」ComboBoxの「SelectionChange」イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ComboBoxY_SelectionChanged(
            object sender,
            System.Windows.Controls.SelectionChangedEventArgs e
            )
        {
            try
            {
                SettingsYControlEnabled();
            }
            catch
            {
            }
        }

        /// <summary>
        /// 「Y」の「値の種類」ComboBoxの「SelectionChanged」イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void YTypeComboBox_SelectionChanged(
            object sender,
            System.Windows.Controls.SelectionChangedEventArgs e
            )
        {
            try
            {
                if ((string)((System.Windows.Controls.ComboBoxItem)YTypeComboBox.SelectedItem).Content == Common.ApplicationData.Languages.LanguagesWindow.Percent)
                {
                    YNumericUpDown.IsUseDecimal = true;
                    YNumericUpDown.MinimumValue = Common.PercentMinimize;
                    YNumericUpDown.MaximumValue = Common.PercentMaximize;
                }
                else
                {
                    YNumericUpDown.IsUseDecimal = false;
                    YNumericUpDown.MinimumValue = int.MinValue;
                    YNumericUpDown.MaximumValue = int.MaxValue;
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// 「幅」ComboBoxの「SelectionChanged」イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WidthComboBox_SelectionChanged(
            object sender,
            System.Windows.Controls.SelectionChangedEventArgs e
            )
        {
            try
            {
                SettingsWidthControlEnabled();
            }
            catch
            {
            }
        }

        /// <summary>
        /// 「幅」の「値の種類」ComboBoxの「SelectionChanged」イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WidthTypeComboBox_SelectionChanged(
            object sender,
            System.Windows.Controls.SelectionChangedEventArgs e
            )
        {
            try
            {
                if ((string)((System.Windows.Controls.ComboBoxItem)WidthTypeComboBox.SelectedItem).Content == Common.ApplicationData.Languages.LanguagesWindow.Percent)
                {
                    WidthNumericUpDown.IsUseDecimal = true;
                    WidthNumericUpDown.MaximumValue = Common.PercentMaximize;
                }
                else
                {
                    WidthNumericUpDown.IsUseDecimal = false;
                    WidthNumericUpDown.MaximumValue = int.MaxValue;
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// 「高さ」ComboBoxの「SelectionChanged」イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HeightComboBox_SelectionChanged(
            object sender,
            System.Windows.Controls.SelectionChangedEventArgs e
            )
        {
            try
            {
                SettingsHeightControlEnabled();
            }
            catch
            {
            }
        }

        /// <summary>
        /// 「高さ」の「値の種類」ComboBoxの「SelectionChanged」イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HeightTypeComboBox_SelectionChanged(
            object sender,
            System.Windows.Controls.SelectionChangedEventArgs e
            )
        {
            try
            {
                if ((string)((System.Windows.Controls.ComboBoxItem)HeightTypeComboBox.SelectedItem).Content == Common.ApplicationData.Languages.LanguagesWindow.Percent)
                {
                    HeightNumericUpDown.IsUseDecimal = true;
                    HeightNumericUpDown.MaximumValue = Common.PercentMaximize;
                }
                else
                {
                    HeightNumericUpDown.IsUseDecimal = false;
                    HeightNumericUpDown.MaximumValue = int.MaxValue;
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// 「ホットキー」TextBoxの「GotFocus」イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HotkeyTextBox_GotFocus(
            object sender,
            System.Windows.RoutedEventArgs e
            )
        {
            try
            {
                Common.ApplicationData.DoProcessingEvent(ProcessingEventType.PauseHotkeyProcessing);
            }
            catch
            {
            }
        }

        /// <summary>
        /// 「ホットキー」TextBoxの「LostFocus」イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HotkeyTextBox_LostFocus(
            object sender,
            System.Windows.RoutedEventArgs e
            )
        {
            try
            {
                Common.ApplicationData.DoProcessingEvent(ProcessingEventType.UnpauseHotkeyProcessing);
            }
            catch
            {
            }
        }

        /// <summary>
        /// 「ホットキー」TextBoxの「PreviewKeyDown」イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void HotkeyTextBox_PreviewKeyDown(
            object sender,
            System.Windows.Input.KeyEventArgs e
            )
        {
            try
            {
                FreeEcho.FEHotKeyWPF.HotKeyInformation hotkeyInformation = FreeEcho.FEHotKeyWPF.HotKeyWPF.GetHotKey(e, true);
                string hotkeyString = FreeEcho.FEHotKeyWPF.HotKeyWPF.GetHotKeyString(hotkeyInformation);

                if (hotkeyString != "Tab")
                {
                    HotkeyItemInformation.Hotkey.Copy(hotkeyInformation);
                    HotkeyTextBox.Text = hotkeyString;
                    e.Handled = true;
                }
            }
            catch
            {
                FreeEcho.FEControls.MessageBox.Show(Common.ApplicationData.Languages.Check, Common.ApplicationData.Languages.ErrorOccurred, FreeEcho.FEControls.MessageBoxButton.Ok, this);
            }
        }

        /// <summary>
        /// 「追加/修正」Buttonの「Click」イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddModifyButton_Click(
            object sender,
            System.Windows.RoutedEventArgs e
            )
        {
            try
            {
                GetHotkeyInformation();
                if (CheckValue())
                {
                    Common.ApplicationData.WindowProcessingManagement.Hotkey?.UnregisterHotkeys();
                    try
                    {
                        if (IndexOfItemToBeModified == -1)
                        {
                            Common.ApplicationData.Settings.HotkeyInformation.Items.Add(HotkeyItemInformation);
                            FreeEcho.FEControls.MessageBox.Show(Common.ApplicationData.Languages.Check, Common.ApplicationData.Languages.LanguagesWindow.Added, FreeEcho.FEControls.MessageBoxButton.Ok, this);
                        }
                        else
                        {
                            Common.ApplicationData.Settings.HotkeyInformation.Items[IndexOfItemToBeModified] = HotkeyItemInformation;
                            FreeEcho.FEControls.MessageBox.Show(Common.ApplicationData.Languages.Check, Common.ApplicationData.Languages.LanguagesWindow.Modified, FreeEcho.FEControls.MessageBoxButton.Ok, this);
                        }
                        AddedModified = true;
                    }
                    catch
                    {
                        FreeEcho.FEControls.MessageBox.Show(Common.ApplicationData.Languages.Check, Common.ApplicationData.Languages.ErrorOccurred, FreeEcho.FEControls.MessageBoxButton.Ok, this);
                    }
                    Common.ApplicationData.WindowProcessingManagement.Hotkey?.RegisterHotkeys();
                    Close();
                }
            }
            catch
            {
                FreeEcho.FEControls.MessageBox.Show(Common.ApplicationData.Languages.Check, Common.ApplicationData.Languages.ErrorOccurred, FreeEcho.FEControls.MessageBoxButton.Ok, this);
            }
        }

        /// <summary>
        /// 「キャンセル」Buttonの「Click」イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CancelButton_Click(
            object sender,
            System.Windows.RoutedEventArgs e
            )
        {
            try
            {
                Close();
            }
            catch
            {
            }
        }

        /// <summary>
        /// 「X」関係のコントロールの有効状態を設定
        /// </summary>
        private void SettingsXControlEnabled()
        {
            if ((string)((System.Windows.Controls.ComboBoxItem)XComboBox.SelectedItem).Content == Common.ApplicationData.Languages.LanguagesWindow.CoordinateSpecification)
            {
                XNumericUpDown.IsEnabled = true;
                XTypeComboBox.IsEnabled = true;
            }
            else
            {
                XNumericUpDown.IsEnabled = false;
                XTypeComboBox.IsEnabled = false;
            }
        }

        /// <summary>
        /// 「Y」関係のコントロールの有効状態を設定
        /// </summary>
        private void SettingsYControlEnabled()
        {
            if ((string)((System.Windows.Controls.ComboBoxItem)YComboBox.SelectedItem).Content == Common.ApplicationData.Languages.LanguagesWindow.CoordinateSpecification)
            {
                YNumericUpDown.IsEnabled = true;
                YTypeComboBox.IsEnabled = true;
            }
            else
            {
                YNumericUpDown.IsEnabled = false;
                YTypeComboBox.IsEnabled = false;
            }
        }

        /// <summary>
        /// 「幅」関係のコントロールの有効状態を設定
        /// </summary>
        private void SettingsWidthControlEnabled()
        {
            if ((string)((System.Windows.Controls.ComboBoxItem)WidthComboBox.SelectedItem).Content == Common.ApplicationData.Languages.LanguagesWindow.WidthSpecification)
            {
                WidthNumericUpDown.IsEnabled = true;
                WidthTypeComboBox.IsEnabled = true;
            }
            else
            {
                WidthNumericUpDown.IsEnabled = false;
                WidthTypeComboBox.IsEnabled = false;
            }
        }

        /// <summary>
        /// 「高さ」関係のコントロールの有効状態を設定
        /// </summary>
        private void SettingsHeightControlEnabled()
        {
            if ((string)((System.Windows.Controls.ComboBoxItem)HeightComboBox.SelectedItem).Content == Common.ApplicationData.Languages.LanguagesWindow.HeightSpecification)
            {
                HeightNumericUpDown.IsEnabled = true;
                HeightTypeComboBox.IsEnabled = true;
            }
            else
            {
                HeightNumericUpDown.IsEnabled = false;
                HeightTypeComboBox.IsEnabled = false;
            }
        }

        /// <summary>
        /// コントロールの有効状態を設定
        /// </summary>
        private void SettingsControlEnabled()
        {
            if ((string)((System.Windows.Controls.ComboBoxItem)ProcessingTypeComboBox.SelectedItem).Content == Common.ApplicationData.Languages.LanguagesWindow.SpecifyPositionAndSize)
            {
                PositionSizeGroupBox.IsEnabled = true;
                SettingsXControlEnabled();
                SettingsYControlEnabled();
                SettingsWidthControlEnabled();
                SettingsHeightControlEnabled();
            }
            else
            {
                PositionSizeGroupBox.IsEnabled = false;
            }

            string selectedItemString = (string)((System.Windows.Controls.ComboBoxItem)ProcessingTypeComboBox.SelectedItem).Content;
            if ((selectedItemString == Common.ApplicationData.Languages.LanguagesWindow.MoveXCoordinate)
                || (selectedItemString == Common.ApplicationData.Languages.LanguagesWindow.MoveYCoordinate)
                || (selectedItemString == Common.ApplicationData.Languages.LanguagesWindow.MoveXAndYCoordinate))
            {
                AmountOfMovementGrid.IsEnabled = true;
                SizeChangeAmountGrid.IsEnabled = false;
                TransparencyGrid.IsEnabled = false;
            }
            else if ((selectedItemString == Common.ApplicationData.Languages.LanguagesWindow.IncreaseDecreaseWidth)
                || (selectedItemString == Common.ApplicationData.Languages.LanguagesWindow.IncreaseDecreaseHeight)
                || (selectedItemString == Common.ApplicationData.Languages.LanguagesWindow.IncreaseDecreaseWidthAndHeight))
            {
                AmountOfMovementGrid.IsEnabled = false;
                SizeChangeAmountGrid.IsEnabled = true;
                TransparencyGrid.IsEnabled = false;
            }
            else if (selectedItemString == Common.ApplicationData.Languages.LanguagesWindow.SpecifyCancelTransparency)
            {
                AmountOfMovementGrid.IsEnabled = false;
                SizeChangeAmountGrid.IsEnabled = false;
                TransparencyGrid.IsEnabled = true;
            }
            else
            {
                AmountOfMovementGrid.IsEnabled = false;
                SizeChangeAmountGrid.IsEnabled = false;
                TransparencyGrid.IsEnabled = false;
            }
        }

        /// <summary>
        /// 「追加/修正」ボタンの有効状態を設定
        /// </summary>
        private void SettingsAddModifyButtonEnabled()
        {
            AddModifyButton.IsEnabled = !string.IsNullOrEmpty(RegisteredNameTextBox.Text);
        }

        /// <summary>
        /// ホットキー情報取得
        /// </summary>
        private void GetHotkeyInformation()
        {
            string stringData;     // 文字列データ

            HotkeyItemInformation.RegisteredName = RegisteredNameTextBox.Text;
            stringData = (string)((System.Windows.Controls.ComboBoxItem)ProcessingTypeComboBox.SelectedItem).Content;
            if (stringData == Common.ApplicationData.Languages.LanguagesWindow.SpecifyPositionAndSize)
            {
                HotkeyItemInformation.ProcessingType = HotkeyProcessingType.PositionSize;
            }
            else if (stringData == Common.ApplicationData.Languages.LanguagesWindow.MoveXCoordinate)
            {
                HotkeyItemInformation.ProcessingType = HotkeyProcessingType.MoveX;
            }
            else if (stringData == Common.ApplicationData.Languages.LanguagesWindow.MoveYCoordinate)
            {
                HotkeyItemInformation.ProcessingType = HotkeyProcessingType.MoveY;
            }
            else if (stringData == Common.ApplicationData.Languages.LanguagesWindow.MoveXAndYCoordinate)
            {
                HotkeyItemInformation.ProcessingType = HotkeyProcessingType.MoveXY;
            }
            else if (stringData == Common.ApplicationData.Languages.LanguagesWindow.IncreaseDecreaseWidth)
            {
                HotkeyItemInformation.ProcessingType = HotkeyProcessingType.IncreaseDecreaseWidth;
            }
            else if (stringData == Common.ApplicationData.Languages.LanguagesWindow.IncreaseDecreaseHeight)
            {
                HotkeyItemInformation.ProcessingType = HotkeyProcessingType.IncreaseDecreaseHeight;
            }
            else if (stringData == Common.ApplicationData.Languages.LanguagesWindow.IncreaseDecreaseWidthAndHeight)
            {
                HotkeyItemInformation.ProcessingType = HotkeyProcessingType.IncreaseDecreaseWidthHeight;
            }
            else if (stringData == Common.ApplicationData.Languages.LanguagesWindow.AlwaysShowOrCancelOnTop)
            {
                HotkeyItemInformation.ProcessingType = HotkeyProcessingType.AlwaysForefrontOrCancel;
            }
            else if (stringData == Common.ApplicationData.Languages.LanguagesWindow.SpecifyCancelTransparency)
            {
                HotkeyItemInformation.ProcessingType = HotkeyProcessingType.SpecifyTransparencyOrCancel;
            }
            else if (stringData == Common.ApplicationData.Languages.LanguagesWindow.StartStopProcessingOfEvent)
            {
                HotkeyItemInformation.ProcessingType = HotkeyProcessingType.StartStopEvent;
            }
            else if (stringData == Common.ApplicationData.Languages.LanguagesWindow.BatchProcessingOfEvent)
            {
                HotkeyItemInformation.ProcessingType = HotkeyProcessingType.BatchEvent;
            }
            else if (stringData == Common.ApplicationData.Languages.LanguagesWindow.OnlyActiveWindowEvent)
            {
                HotkeyItemInformation.ProcessingType = HotkeyProcessingType.OnlyActiveWindowEvent;
            }
            else if (stringData == Common.ApplicationData.Languages.LanguagesWindow.StartStopProcessingOfTimer)
            {
                HotkeyItemInformation.ProcessingType = HotkeyProcessingType.StartStopTimer;
            }
            else if (stringData == Common.ApplicationData.Languages.LanguagesWindow.BatchProcessingOfTimer)
            {
                HotkeyItemInformation.ProcessingType = HotkeyProcessingType.BatchTimer;
            }
            else if (stringData == Common.ApplicationData.Languages.LanguagesWindow.OnlyActiveWindowTimer)
            {
                HotkeyItemInformation.ProcessingType = HotkeyProcessingType.OnlyActiveWindowTimer;
            }
            else if (stringData == Common.ApplicationData.Languages.LanguagesWindow.StartStopProcessingOfMagnet)
            {
                HotkeyItemInformation.ProcessingType = HotkeyProcessingType.StartStopMagnet;
            }
            switch (HotkeyItemInformation.ProcessingType)
            {
                case HotkeyProcessingType.PositionSize:
                    if (StandardDisplayComboBox.Text == Common.ApplicationData.Languages.LanguagesWindow.TheSpecifiedDisplay)
                    {
                        HotkeyItemInformation.StandardDisplay = StandardDisplay.SpecifiedDisplay;
                    }
                    else if (StandardDisplayComboBox.Text == Common.ApplicationData.Languages.LanguagesWindow.DisplayWithWindow)
                    {
                        HotkeyItemInformation.StandardDisplay = StandardDisplay.DisplayWithWindow;
                    }
                    HotkeyItemInformation.PositionSize.Display = (string)((System.Windows.Controls.ComboBoxItem)DisplayComboBox.SelectedItem).Content;
                    stringData = (string)((System.Windows.Controls.ComboBoxItem)XComboBox.SelectedItem).Content;
                    if (stringData == Common.ApplicationData.Languages.LanguagesWindow.DoNotChange)
                    {
                        HotkeyItemInformation.PositionSize.XType = WindowXType.DoNotChange;
                    }
                    else if (stringData == Common.ApplicationData.Languages.LanguagesWindow.LeftEdge)
                    {
                        HotkeyItemInformation.PositionSize.XType = WindowXType.Left;
                    }
                    else if (stringData == Common.ApplicationData.Languages.LanguagesWindow.Middle)
                    {
                        HotkeyItemInformation.PositionSize.XType = WindowXType.Middle;
                    }
                    else if (stringData == Common.ApplicationData.Languages.LanguagesWindow.RightEdge)
                    {
                        HotkeyItemInformation.PositionSize.XType = WindowXType.Right;
                    }
                    else if (stringData == Common.ApplicationData.Languages.LanguagesWindow.CoordinateSpecification)
                    {
                        HotkeyItemInformation.PositionSize.XType = WindowXType.Value;
                        HotkeyItemInformation.PositionSize.Position.X = XNumericUpDown.Value;
                        if ((string)((System.Windows.Controls.ComboBoxItem)XTypeComboBox.SelectedItem).Content == Common.ApplicationData.Languages.LanguagesWindow.Percent)
                        {
                            HotkeyItemInformation.PositionSize.XValueType = ValueType.Percent;
                        }
                        else
                        {
                            HotkeyItemInformation.PositionSize.XValueType = ValueType.Pixel;
                        }
                    }
                    stringData = (string)((System.Windows.Controls.ComboBoxItem)YComboBox.SelectedItem).Content;
                    if (stringData == Common.ApplicationData.Languages.LanguagesWindow.DoNotChange)
                    {
                        HotkeyItemInformation.PositionSize.YType = WindowYType.DoNotChange;
                    }
                    else if (stringData == Common.ApplicationData.Languages.LanguagesWindow.TopEdge)
                    {
                        HotkeyItemInformation.PositionSize.YType = WindowYType.Top;
                    }
                    else if (stringData == Common.ApplicationData.Languages.LanguagesWindow.Middle)
                    {
                        HotkeyItemInformation.PositionSize.YType = WindowYType.Middle;
                    }
                    else if (stringData == Common.ApplicationData.Languages.LanguagesWindow.BottomEdge)
                    {
                        HotkeyItemInformation.PositionSize.YType = WindowYType.Bottom;
                    }
                    else if (stringData == Common.ApplicationData.Languages.LanguagesWindow.CoordinateSpecification)
                    {
                        HotkeyItemInformation.PositionSize.YType = WindowYType.Value;
                        HotkeyItemInformation.PositionSize.Position.Y = YNumericUpDown.Value;
                        if ((string)((System.Windows.Controls.ComboBoxItem)YTypeComboBox.SelectedItem).Content == Common.ApplicationData.Languages.LanguagesWindow.Percent)
                        {
                            HotkeyItemInformation.PositionSize.YValueType = ValueType.Percent;
                        }
                        else
                        {
                            HotkeyItemInformation.PositionSize.YValueType = ValueType.Pixel;
                        }
                    }
                    stringData = (string)((System.Windows.Controls.ComboBoxItem)WidthComboBox.SelectedItem).Content;
                    if (stringData == Common.ApplicationData.Languages.LanguagesWindow.DoNotChange)
                    {
                        HotkeyItemInformation.PositionSize.WidthType = WindowSizeType.DoNotChange;
                    }
                    else if (stringData == Common.ApplicationData.Languages.LanguagesWindow.WidthSpecification)
                    {
                        HotkeyItemInformation.PositionSize.WidthType = WindowSizeType.Value;
                        HotkeyItemInformation.PositionSize.Size.Width = WidthNumericUpDown.Value;
                        if ((string)((System.Windows.Controls.ComboBoxItem)WidthTypeComboBox.SelectedItem).Content == Common.ApplicationData.Languages.LanguagesWindow.Percent)
                        {
                            HotkeyItemInformation.PositionSize.WidthValueType = ValueType.Percent;
                        }
                        else
                        {
                            HotkeyItemInformation.PositionSize.WidthValueType = ValueType.Pixel;
                        }
                    }
                    stringData = (string)((System.Windows.Controls.ComboBoxItem)HeightComboBox.SelectedItem).Content;
                    if (stringData == Common.ApplicationData.Languages.LanguagesWindow.DoNotChange)
                    {
                        HotkeyItemInformation.PositionSize.HeightType = WindowSizeType.DoNotChange;
                    }
                    else if (stringData == Common.ApplicationData.Languages.LanguagesWindow.HeightSpecification)
                    {
                        HotkeyItemInformation.PositionSize.HeightType = WindowSizeType.Value;
                        HotkeyItemInformation.PositionSize.Size.Height = HeightNumericUpDown.Value;
                        if ((string)((System.Windows.Controls.ComboBoxItem)HeightTypeComboBox.SelectedItem).Content == Common.ApplicationData.Languages.LanguagesWindow.Percent)
                        {
                            HotkeyItemInformation.PositionSize.HeightValueType = ValueType.Percent;
                        }
                        else
                        {
                            HotkeyItemInformation.PositionSize.HeightValueType = ValueType.Pixel;
                        }
                    }
                    HotkeyItemInformation.PositionSize.ClientArea = ClientAreaToggleSwitch.IsOn;
                    HotkeyItemInformation.PositionSize.ProcessingPositionAndSizeTwice = ProcessingPositionAndSizeTwiceToggleSwitch.IsOn;
                    break;
                case HotkeyProcessingType.MoveX:
                case HotkeyProcessingType.MoveY:
                case HotkeyProcessingType.MoveXY:
                    HotkeyItemInformation.ProcessingValue = AmountOfMovementNumericUpDown.ValueInt;
                    break;
                case HotkeyProcessingType.IncreaseDecreaseWidth:
                case HotkeyProcessingType.IncreaseDecreaseHeight:
                case HotkeyProcessingType.IncreaseDecreaseWidthHeight:
                    HotkeyItemInformation.ProcessingValue = SizeChangeAmountNumericUpDown.ValueInt;
                    break;
                case HotkeyProcessingType.SpecifyTransparencyOrCancel:
                    HotkeyItemInformation.ProcessingValue = TransparencyNumericUpDown.ValueInt;
                    break;
            }
        }

        /// <summary>
        /// 値の確認
        /// </summary>
        /// <returns>値に問題があるかの値 (問題がある「false」/問題ない「true」)</returns>
        private bool CheckValue()
        {
            bool result = true;     // 結果
            int count = 0;      // カウント

            // 重複確認
            foreach (HotkeyItemInformation nowItem in Common.ApplicationData.Settings.HotkeyInformation.Items)
            {
                if ((nowItem.RegisteredName == HotkeyItemInformation.RegisteredName)
                    && ((IndexOfItemToBeModified == -1) || (IndexOfItemToBeModified != count)))
                {
                    FreeEcho.FEControls.MessageBox.Show(Common.ApplicationData.Languages.Check, Common.ApplicationData.Languages.LanguagesWindow.ThereIsADuplicateRegistrationName, FreeEcho.FEControls.MessageBoxButton.Ok, this);
                    result = false;
                    break;
                }
                count++;
            }

            return (result);
        }
    }
}
