namespace Swindom;

/// <summary>
/// 「プラグイン」ページ
/// </summary>
public partial class PluginPage : Page
{
    /// <summary>
    /// プラグインのファイル情報
    /// </summary>
    private readonly List<PluginFileInformation> PluginFileInformation;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public PluginPage()
    {
        InitializeComponent();

        PluginFileInformation = PluginProcessing.GetPluginsInformation();       // プラグインのファイルパス

        if (ApplicationPath.CheckInstalled() == false)
        {
            PluginFolderLabel.Visibility = Visibility.Collapsed;
            PluginFolderTextBox.Visibility = Visibility.Collapsed;
            PluginPathButton.Visibility = Visibility.Collapsed;
        }

        SettingsRowDefinition.Height = new(WindowControlValue.SettingsRowDefinitionMinimize);
        SettingsScrollViewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled;
        SettingsScrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Disabled;
        ProcessingStateToggleSwitch.IsOn = ApplicationData.Settings.PluginInformation.IsEnabled;
        PluginFolderTextBox.Text = ApplicationData.Settings.PluginInformation.PluginFolder;
        UpdatePluginListBox();
        PluginSettingsButton.IsEnabled = false;
        RestartStackPanel.Visibility = Visibility.Collapsed;
        SettingsControlsImage();
        SetTextOnControls();

        PluginListBox.SelectionChanged += PluginListBox_SelectionChanged;
        PluginSettingsButton.Click += PluginSettingsButton_Click;
        SettingsButton.Click += SettingsButton_Click;
        ProcessingStateToggleSwitch.Toggled += ProcessingStateToggleSwitch_Toggled;
        PluginFolderTextBox.TextChanged += PluginFolderTextBox_TextChanged;
        PluginPathButton.Click += PluginPathButton_Click;
        PluginExplanationButton.Click += PluginExplanationButton_Click;
        RestartButton.Click += RestartButton_Click;
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
    /// 「プラグイン」ListBoxの「SelectionChanged」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void PluginListBox_SelectionChanged(
        object sender,
        SelectionChangedEventArgs e
        )
    {
        try
        {
            if (PluginListBox.SelectedItems.Count == 0
                || ApplicationData.WindowProcessingManagement.PluginProcessing == null)
            {
                PluginSettingsButton.IsEnabled = false;
            }
            else
            {
                bool isEnabled = false;        // 有効状態

                string selectedPlugin = (string)((CheckListBoxItem)PluginListBox.SelectedItem).Text;

                foreach (RunningPluginInformation nowInformation in ApplicationData.WindowProcessingManagement.PluginProcessing.RunningPluginInformation)
                {
                    if (nowInformation.Path == PluginFileInformation[PluginListBox.SelectedIndex].Path && PluginFileInformation[PluginListBox.SelectedIndex].IsWindowExist)
                    {
                        isEnabled = true;
                        break;
                    }
                }

                PluginSettingsButton.IsEnabled = isEnabled;
            }
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「プラグイン設定」Buttonの「Click」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void PluginSettingsButton_Click(
        object? sender,
        RoutedEventArgs e
        )
    {
        try
        {
            if (ApplicationData.WindowProcessingManagement.PluginProcessing != null)
            {
                string selectedPlugin = (string)((CheckListBoxItem)PluginListBox.SelectedItem).Text;

                foreach (RunningPluginInformation nowInformation in ApplicationData.WindowProcessingManagement.PluginProcessing.RunningPluginInformation)
                {
                    if (nowInformation.IPlugin != null && selectedPlugin == nowInformation.IPlugin.PluginName && nowInformation.IPlugin.IsWindowExist)
                    {
                        nowInformation.IPlugin.ShowWindow();
                        break;
                    }
                }
            }
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「設定」Buttonの「Click」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void SettingsButton_Click(
        object? sender,
        RoutedEventArgs e
        )
    {
        try
        {
            if ((int)SettingsRowDefinition.Height.Value == WindowControlValue.SettingsRowDefinitionMinimize)
            {
                ItemsRowDefinition.Height = new(0);
                SettingsRowDefinition.Height = new(1, GridUnitType.Star);
                SettingsButton.ButtonImage = ImageProcessing.GetImageCloseSettings();
                SettingsScrollViewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
                SettingsScrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            }
            else
            {
                ItemsRowDefinition.Height = new(1, GridUnitType.Star);
                SettingsRowDefinition.Height = new(WindowControlValue.SettingsRowDefinitionMinimize);
                SettingsButton.ButtonImage = ImageProcessing.GetImageSettings();
                SettingsScrollViewer.ScrollToVerticalOffset(0);
                SettingsScrollViewer.HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled;
                SettingsScrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Disabled;
            }
        }
        catch
        {
        }
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
            if (ApplicationData.Settings.PluginInformation.IsEnabled
                && ProcessingStateToggleSwitch.IsOn == false)
            {
                RestartStackPanel.Visibility = Visibility.Visible;
            }
            ApplicationData.Settings.PluginInformation.IsEnabled = ProcessingStateToggleSwitch.IsOn;
            ApplicationData.EventData.DoProcessingEvent(ProcessingEventType.PluginProcessingStateChanged);
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「プラグインのフォルダー」TextBoxの「TextChanged」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void PluginFolderTextBox_TextChanged(
        object sender,
        TextChangedEventArgs e
        )
    {
        try
        {
            if (Directory.Exists(PluginFolderTextBox.Text))
            {
                ApplicationData.Settings.PluginInformation.PluginFolder = PluginFolderTextBox.Text;
            }
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「プラグインのフォルダー」Buttonの「Click」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void PluginPathButton_Click(
        object sender,
        RoutedEventArgs e
        )
    {
        try
        {
            Microsoft.Win32.OpenFolderDialog dialog = new()
            {
                Multiselect = false
            };
            if (dialog.ShowDialog() == true)
            {
                PluginFolderTextBox.Text = dialog.FolderName;
            }
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「?」Buttonの「Click」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void PluginExplanationButton_Click(
        object sender,
        RoutedEventArgs e
        )
    {
        try
        {
            ApplicationData.WindowManagement.ShowExplanationWindow(SelectExplanationType.Plugin);
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「再起動」Buttonの「Click」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void RestartButton_Click(
        object sender,
        RoutedEventArgs e
        )
    {
        try
        {
            ApplicationData.EventData.DoProcessingEvent(ProcessingEventType.RestartProcessing);
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
                case ProcessingEventType.LanguageChanged:
                    SetTextOnControls();
                    break;
                case ProcessingEventType.ThemeChanged:
                    SettingsControlsImage();
                    break;
            }
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「プラグイン」ListBoxの項目を更新
    /// </summary>
    private void UpdatePluginListBox()
    {
        PluginListBox.Items.Clear();

        try
        {
            int selectedIndex = PluginListBox.SelectedIndex;       // 選択している項目のインデックス

            foreach (PluginFileInformation nowPFI in PluginFileInformation)
            {
                bool isEnabled = false;     // 有効状態

                foreach (PluginItemInformation nowPII in ApplicationData.Settings.PluginInformation.Items)
                {
                    if (nowPII.PluginFileName == Path.GetFileNameWithoutExtension(nowPFI.Path))
                    {
                        isEnabled = true;
                        break;
                    }
                }

                CheckListBoxItem newItem = new()
                {
                    Text = string.IsNullOrEmpty(nowPFI.PluginName) ? Path.GetFileNameWithoutExtension(nowPFI.Path) : nowPFI.PluginName,
                    IsChecked = isEnabled
                };
                newItem.CheckStateChanged += PluginListBox_Item_CheckStateChanged;
                PluginListBox.Items.Add(newItem);
            }
            PluginListBox.SelectedIndex = selectedIndex;
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「プラグイン」ListBoxの項目の「CheckStateChanged」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void PluginListBox_Item_CheckStateChanged(
        object sender,
        CheckListBoxItemEventArgs e
        )
    {
        try
        {
            if (e.Item == null)
            {
                return;
            }

            PluginFileInformation? checkStateChangedItem = null;        // チェック状態が変更された項目のプラグインのファイル情報
            PluginItemInformation? runningItem = null;       // 実行中のプラグイン項目情報 (実行中ではない「null」)
            string itemText = (string)e.Item.Text;      // チェック状態が変更された項目のテキスト

            foreach (PluginFileInformation nowPFI in PluginFileInformation)
            {
                if (itemText == nowPFI.PluginName
                    || itemText == Path.GetFileNameWithoutExtension(nowPFI.Path))
                {
                    checkStateChangedItem = nowPFI;
                    break;
                }
            }
            if (checkStateChangedItem == null)
            {
                return;
            }

            foreach (PluginItemInformation nowPII in ApplicationData.Settings.PluginInformation.Items)
            {
                if (Path.GetFileNameWithoutExtension(checkStateChangedItem.Path) == nowPII.PluginFileName)
                {
                    runningItem = nowPII;
                    break;
                }
            }

            if (e.Item.IsChecked)
            {
                if (runningItem == null)
                {
                    PluginItemInformation newItem = new()
                    {
                        PluginFileName = Path.GetFileNameWithoutExtension(checkStateChangedItem.Path)
                    };
                    ApplicationData.Settings.PluginInformation.Items.Add(newItem);
                    ApplicationData.WindowProcessingManagement.PluginProcessing?.RunPlugins();
                }
            }
            else
            {
                if (runningItem != null)
                {
                    ApplicationData.Settings.PluginInformation.Items.Remove(runningItem);
                    RestartStackPanel.Visibility = Visibility.Visible;
                }
            }
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
            PluginSettingsButton.Text = ApplicationData.Languages.PluginSettings;
            SettingsButton.Text = ApplicationData.Languages.Setting;
            ProcessingStateToggleSwitch.OffContent = ProcessingStateToggleSwitch.OnContent = ApplicationData.Languages.ProcessingState;
            PluginFolderLabel.Content = ApplicationData.Languages.PluginFolder;
            PluginPathButton.ToolTip = ApplicationData.Languages.FolderSelection;
            PluginExplanationButton.Content = ApplicationData.Languages.Question;
            PluginExplanationButton.ToolTip = ApplicationData.Languages.Help;
            RestartButton.Content = ApplicationData.Languages.Restart;
            RestartLabel.Content = ApplicationData.Languages.NeedRestart;
            ExplanationTextBlock.Text = ApplicationData.Languages.PluginExplanation;
        }
        catch
        {
        }
    }

    /// <summary>
    /// コントロールの画像を設定
    /// </summary>
    private void SettingsControlsImage()
    {
        PluginSettingsButton.ButtonImage = ImageProcessing.GetImageSettings();
        SettingsButton.ButtonImage = (int)SettingsRowDefinition.Height.Value == WindowControlValue.SettingsRowDefinitionMinimize ? ImageProcessing.GetImageSettings() : ImageProcessing.GetImageCloseSettings();
    }
}
