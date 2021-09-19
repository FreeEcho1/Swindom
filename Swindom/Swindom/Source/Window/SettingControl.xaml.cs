using Swindom.Source.Settings;

namespace Swindom.Source.Window
{
    /// <summary>
    /// 「設定」コントロール
    /// </summary>
    public partial class SettingControl : System.Windows.Controls.UserControl, System.IDisposable
    {
        /// <summary>
        /// Disposeが呼ばれたかの値
        /// </summary>
        private bool Disposed = false;

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SettingControl()
        {
            InitializeComponent();

            SettingsLanguage();

            // 「全般」タブのコントロール
            try
            {
                foreach (string path in System.IO.Directory.GetFiles(Processing.GetApplicationDirectoryPath() + System.IO.Path.DirectorySeparatorChar + Common.LanguagesFolderName + System.IO.Path.DirectorySeparatorChar))
                {
                    string languageName = Languages.Languages.GetLanguageName(path);        // 言語名
                    string fileName = System.IO.Path.GetFileNameWithoutExtension(path);     // ファイル名

                    System.Windows.Controls.ComboBoxItem newItem = new()
                    {
                        Content = string.IsNullOrEmpty(languageName) ? fileName : languageName
                    };
                    if (fileName == Common.ApplicationData.Settings.Language)
                    {
                        newItem.IsSelected = true;
                    }
                    LanguageComboBox.Items.Add(newItem);
                }
            }
            catch
            {
            }
            RunAtWindowsStartupToggleSwitch.IsOn = StartupProcessing.CheckTaskSchedulerRegistered();
            if (RunAtWindowsStartupToggleSwitch.IsOn)
            {
                RunAtWindowsStartupToggleSwitch.Text = Common.ApplicationData.Languages.LanguagesWindow.RunAtWindowsStartupAdministrator;
            }
            else
            {
                try
                {
                    if (StartupProcessing.CheckStartup())
                    {
                        RunAtWindowsStartupToggleSwitch.IsOn = true;
                    }
                }
                catch
                {
                }
            }
            CheckForUpdatesIncludingBetaVersionToggleSwitch.IsOn = Common.ApplicationData.Settings.CheckBetaVersion;
            AutomaticallyCheckForUpdatesWhenRunToggleSwitch.IsOn = Common.ApplicationData.Settings.AutomaticallyUpdateCheck;
            string stringData;      // 文字列データ
            stringData = Common.ApplicationData.Settings.CoordinateType switch
            {
                CoordinateType.Global => Common.ApplicationData.Languages.LanguagesWindow.GlobalCoordinates,
                _ => Common.ApplicationData.Languages.LanguagesWindow.DisplayCoordinates
            };
            Processing.SelectComboBoxItem(CoordinateSpecificationMethodComboBox, stringData);
            MonitorInformation monitorInformation = MonitorInformation.GetMonitorInformation();     // モニター情報
            if (monitorInformation.MonitorInfo.Count == 1)
            {
                CoordinateSpecificationMethodLabel.Visibility = System.Windows.Visibility.Collapsed;
                CoordinateSpecificationMethodComboBox.Visibility = System.Windows.Visibility.Collapsed;
            }

            // 「貼り付ける位置をずらす」タブのコントロール
            MoveThePastePositionToggleSwitch.IsOn = Common.ApplicationData.Settings.ShiftPastePosition.Enabled;
            LeftEdgeNumericUpDown.ValueInt = Common.ApplicationData.Settings.ShiftPastePosition.Left;
            TopEdgeNumericUpDown.ValueInt = Common.ApplicationData.Settings.ShiftPastePosition.Top;
            RightEdgeNumericUpDown.ValueInt = Common.ApplicationData.Settings.ShiftPastePosition.Right;
            BottomEdgeNumericUpDown.ValueInt = Common.ApplicationData.Settings.ShiftPastePosition.Bottom;

            LanguageComboBox.SelectionChanged += LanguageComboBox_SelectionChanged;
            RunAtWindowsStartupToggleSwitch.IsOnChange += RunAtWindowsStartupToggleSwitch_IsOnChange;
            CheckForUpdatesIncludingBetaVersionToggleSwitch.IsOnChange += CheckForUpdatesIncludingBetaVersionToggleSwitch_IsOnChange;
            AutomaticallyCheckForUpdatesWhenRunToggleSwitch.IsOnChange += AutomaticallyCheckForUpdatesWhenRunToggleSwitch_IsOnChange;
            CoordinateSpecificationMethodComboBox.SelectionChanged += CoordinateSpecificationMethodComboBox_SelectionChanged;
            MoveThePastePositionToggleSwitch.IsOnChange += MoveThePastePositionToggleSwitch_IsOnChange;
            LeftEdgeNumericUpDown.ChangeValue += LeftEdgeNumericUpDown_ChangeValue;
            TopEdgeNumericUpDown.ChangeValue += TopEdgeNumericUpDown_ChangeValue;
            RightEdgeNumericUpDown.ChangeValue += RightEdgeNumericUpDown_ChangeValue;
            BottomEdgeNumericUpDown.ChangeValue += BottomEdgeNumericUpDown_ChangeValue;

            Common.ApplicationData.ProcessingEvent += ApplicationData_ProcessingEvent;
        }

        /// <summary>
        /// デストラクタ
        /// </summary>
        ~SettingControl()
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
            }
        }

        /// <summary>
        /// 「言語」ComboBoxの「SelectionChanged」イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LanguageComboBox_SelectionChanged(
            object sender,
            System.Windows.Controls.SelectionChangedEventArgs e
            )
        {
            try
            {
                string language = (string)((System.Windows.Controls.ComboBoxItem)LanguageComboBox.SelectedItem).Content;        // 言語
                foreach (string path in System.IO.Directory.GetFiles(Processing.GetApplicationDirectoryPath() + System.IO.Path.DirectorySeparatorChar + Common.LanguagesFolderName + System.IO.Path.DirectorySeparatorChar))
                {
                    if (language == Languages.Languages.GetLanguageName(path))
                    {
                        language = System.IO.Path.GetFileNameWithoutExtension(path);
                        break;
                    }
                }

                string previousLanguage = Common.ApplicationData.Settings.Language;      // 変更前の言語
                Common.ApplicationData.Settings.Language = language;
                if (Common.ApplicationData.ReadLanguages())
                {
                    Common.ApplicationData.DoProcessingEvent(ProcessingEventType.LanguageChanged);
                }
                else
                {
                    Common.ApplicationData.Settings.Language = previousLanguage;
                    Processing.SelectComboBoxItem(LanguageComboBox, previousLanguage);
                    FreeEcho.FEControls.MessageBox.Show(Common.ApplicationData.Languages.Check, Common.ApplicationData.Languages.ErrorOccurred, FreeEcho.FEControls.MessageBoxButton.Ok, System.Windows.Window.GetWindow(this));
                }
            }
            catch
            {
                FreeEcho.FEControls.MessageBox.Show(Common.ApplicationData.Languages.Check, Common.ApplicationData.Languages.ErrorOccurred, FreeEcho.FEControls.MessageBoxButton.Ok, System.Windows.Window.GetWindow(this));
            }
        }

        /// <summary>
        /// 「Windows起動時に実行」ToggleSwitchの「IsOnChange」イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RunAtWindowsStartupToggleSwitch_IsOnChange(
            object sender,
            FreeEcho.FEControls.ToggleSwitchEventArgs e
            )
        {
            bool result = true;     // 結果

            try
            {
                string path = StartupProcessing.GetTaskSchedulerProcessExecutablePath();      // タスクスケジューラ処理の実行ファイルのパス

                // 有効化する
                if (RunAtWindowsStartupToggleSwitch.IsOn)
                {
                    if (string.IsNullOrEmpty(path))
                    {
                        try
                        {
                            StartupProcessing.RegisterStartup();
                        }
                        catch
                        {
                            result = false;
                        }
                    }
                    else
                    {
                        if (FreeEcho.FEControls.MessageBox.Show(Common.ApplicationData.Languages.Check, Common.ApplicationData.Languages.LanguagesWindow.NormalExecution, FreeEcho.FEControls.MessageBoxButton.YesNo) == FreeEcho.FEControls.MessageBoxResult.Yes)
                        {
                            try
                            {
                                StartupProcessing.RegisterStartup();
                            }
                            catch
                            {
                                result = false;
                            }
                        }
                        else
                        {
                            try
                            {
                                using System.Diagnostics.Process process = new();
                                process.StartInfo.FileName = path;
                                process.StartInfo.Arguments = Common.CreateTask;
                                //process.StartInfo.CreateNoWindow = true;
                                process.StartInfo.UseShellExecute = true;
                                process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                                process.StartInfo.Verb = "RunAs";
                                process.Start();
                                process.WaitForExit();
                            }
                            finally
                            {
                                if (StartupProcessing.CheckTaskSchedulerRegistered())
                                {
                                    RunAtWindowsStartupToggleSwitch.Text = Common.ApplicationData.Languages.LanguagesWindow.RunAtWindowsStartupAdministrator;
                                }
                                else
                                {
                                    result = false;
                                }
                            }
                        }
                    }
                }
                // 無効化する
                else
                {
                    if (StartupProcessing.CheckTaskSchedulerRegistered())
                    {
                        try
                        {
                            using System.Diagnostics.Process process = new();
                            process.StartInfo.FileName = path;
                            process.StartInfo.Arguments = Common.DeleteTask;
                            //process.StartInfo.CreateNoWindow = true;
                            process.StartInfo.UseShellExecute = true;
                            process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                            process.StartInfo.Verb = "RunAs";
                            process.Start();
                            process.WaitForExit();
                        }
                        finally
                        {
                            if (StartupProcessing.CheckTaskSchedulerRegistered())
                            {
                                result = false;
                            }
                            else
                            {
                                RunAtWindowsStartupToggleSwitch.Text = Common.ApplicationData.Languages.LanguagesWindow.RunAtWindowsStartup;
                            }
                        }
                    }
                    else
                    {
                        try
                        {
                            StartupProcessing.DeleteStartup();
                        }
                        catch
                        {
                            result = false;
                        }
                    }
                }
            }
            finally
            {
                if (result == false)
                {
                    RunAtWindowsStartupToggleSwitch.IsOnDoNotEvent = !RunAtWindowsStartupToggleSwitch.IsOn;
                    FreeEcho.FEControls.MessageBox.Show(Common.ApplicationData.Languages.Check, Common.ApplicationData.Languages.ErrorOccurred, FreeEcho.FEControls.MessageBoxButton.Ok, System.Windows.Window.GetWindow(this));
                }
            }
        }

        /// <summary>
        /// 「実行時に自動で更新確認」ToggleSwitchの「IsOnChange」イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AutomaticallyCheckForUpdatesWhenRunToggleSwitch_IsOnChange(
            object sender,
            FreeEcho.FEControls.ToggleSwitchEventArgs e
            )
        {
            try
            {
                Common.ApplicationData.Settings.AutomaticallyUpdateCheck = AutomaticallyCheckForUpdatesWhenRunToggleSwitch.IsOn;
            }
            catch
            {
            }
        }

        /// <summary>
        /// 「ベータバージョン確認」ToggleSwitchの「IsOnChange」イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckForUpdatesIncludingBetaVersionToggleSwitch_IsOnChange(
            object sender,
            FreeEcho.FEControls.ToggleSwitchEventArgs e
            )
        {
            try
            {
                Common.ApplicationData.Settings.CheckBetaVersion = CheckForUpdatesIncludingBetaVersionToggleSwitch.IsOn;
            }
            catch
            {
            }
        }

        /// <summary>
        /// 「座標指定の方法」ComboBoxの「SelectionChanged」イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CoordinateSpecificationMethodComboBox_SelectionChanged(
            object sender,
            System.Windows.Controls.SelectionChangedEventArgs e
            )
        {
            try
            {
                if ((string)((System.Windows.Controls.ComboBoxItem)CoordinateSpecificationMethodComboBox.SelectedItem).Content == Common.ApplicationData.Languages.LanguagesWindow.DisplayCoordinates)
                {
                    Common.ApplicationData.Settings.CoordinateType = CoordinateType.Display;
                }
                else if ((string)((System.Windows.Controls.ComboBoxItem)CoordinateSpecificationMethodComboBox.SelectedItem).Content == Common.ApplicationData.Languages.LanguagesWindow.GlobalCoordinates)
                {
                    Common.ApplicationData.Settings.CoordinateType = CoordinateType.Global;
                }
            }
            catch
            {
            }
        }

        /// <summary>
        /// 「画面端に貼り付ける場合にずらす」ToggleSwitchの「IsOnChange」イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MoveThePastePositionToggleSwitch_IsOnChange(
            object sender,
            FreeEcho.FEControls.ToggleSwitchEventArgs e
            )
        {
            try
            {
                Common.ApplicationData.Settings.ShiftPastePosition.Enabled = MoveThePastePositionToggleSwitch.IsOn;
            }
            catch
            {
            }
        }

        /// <summary>
        /// 「左端」NumericUpDownの「ChangeValue」イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LeftEdgeNumericUpDown_ChangeValue(
            object sender,
            FreeEcho.FEControls.NumericUpDownChangeValueArgs e
            )
        {
            try
            {
                Common.ApplicationData.Settings.ShiftPastePosition.Left = LeftEdgeNumericUpDown.ValueInt;
            }
            catch
            {
            }
        }

        /// <summary>
        /// 「上端」NumericUpDownの「ChangeValue」イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TopEdgeNumericUpDown_ChangeValue(
            object sender,
            FreeEcho.FEControls.NumericUpDownChangeValueArgs e
            )
        {
            try
            {
                Common.ApplicationData.Settings.ShiftPastePosition.Top = TopEdgeNumericUpDown.ValueInt;
            }
            catch
            {
            }
        }

        /// <summary>
        /// 「右端」NumericUpDownの「ChangeValue」イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RightEdgeNumericUpDown_ChangeValue(
            object sender,
            FreeEcho.FEControls.NumericUpDownChangeValueArgs e
            )
        {
            try
            {
                Common.ApplicationData.Settings.ShiftPastePosition.Right = RightEdgeNumericUpDown.ValueInt;
            }
            catch
            {
            }
        }

        /// <summary>
        /// 「下端」NumericUpDownの「ChangeValue」イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BottomEdgeNumericUpDown_ChangeValue(
            object sender,
            FreeEcho.FEControls.NumericUpDownChangeValueArgs e
            )
        {
            try
            {
                Common.ApplicationData.Settings.ShiftPastePosition.Bottom = BottomEdgeNumericUpDown.ValueInt;
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
                SettingsGeneralTabItem.Header = Common.ApplicationData.Languages.LanguagesWindow.General;
                LanguageLabel.Content = Common.ApplicationData.Languages.LanguagesWindow.Language;
                TranslatorsLabel.Content = Common.ApplicationData.Languages.LanguagesWindow.Translators;
                RunAtWindowsStartupToggleSwitch.Text = StartupProcessing.CheckTaskSchedulerRegistered() ? Common.ApplicationData.Languages.LanguagesWindow.RunAtWindowsStartupAdministrator : Common.ApplicationData.Languages.LanguagesWindow.RunAtWindowsStartup;
                CheckForUpdatesIncludingBetaVersionToggleSwitch.Text = Common.ApplicationData.Languages.LanguagesWindow.CheckBetaVersion;
                AutomaticallyCheckForUpdatesWhenRunToggleSwitch.Text = Common.ApplicationData.Languages.LanguagesWindow.AutomaticUpdateCheck;
                CoordinateSpecificationMethodLabel.Content = Common.ApplicationData.Languages.LanguagesWindow.CoordinateSpecificationMethod;
                ((System.Windows.Controls.ComboBoxItem)CoordinateSpecificationMethodComboBox.Items[0]).Content = Common.ApplicationData.Languages.LanguagesWindow.DisplayCoordinates;
                ((System.Windows.Controls.ComboBoxItem)CoordinateSpecificationMethodComboBox.Items[1]).Content = Common.ApplicationData.Languages.LanguagesWindow.GlobalCoordinates;

                SettingsMoveThePastePositionTabItem.Header = Common.ApplicationData.Languages.LanguagesWindow.MoveThePastePosition;
                MoveThePastePositionToggleSwitch.Text = Common.ApplicationData.Languages.LanguagesWindow.MoveThePastePosition;
                LeftEdgeLabel.Content = Common.ApplicationData.Languages.LanguagesWindow.LeftEdge;
                TopEdgeLabel.Content = Common.ApplicationData.Languages.LanguagesWindow.TopEdge;
                RightEdgeLabel.Content = Common.ApplicationData.Languages.LanguagesWindow.RightEdge;
                BottomEdgeLabel.Content = Common.ApplicationData.Languages.LanguagesWindow.BottomEdge;
            }
            catch
            {
            }
        }
    }
}
