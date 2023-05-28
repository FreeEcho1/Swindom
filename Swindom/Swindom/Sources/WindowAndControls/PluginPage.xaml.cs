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
        
        SettingsRowDefinition.Height = new(Common.SettingsRowDefinitionMinimize);
        SettingsScrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Disabled;
        ProcessingStateToggleSwitch.IsOn = ApplicationData.Settings.PluginInformation.Enabled;
        UpdatePluginListBox();
        PluginSettingsButton.IsEnabled = false;
        RestartStackPanel.Visibility = Visibility.Collapsed;
        SettingsControlsImage();
        SetTextOnControls();

        PluginListBox.SelectionChanged += PluginListBox_SelectionChanged;
        PluginSettingsButton.Click += PluginSettingsButton_Click;
        SettingsButton.Click += SettingsButton_Click;
        ProcessingStateToggleSwitch.Toggled += ProcessingStateToggleSwitch_Toggled;
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

                foreach (RunningPluginInformation nowItem in ApplicationData.WindowProcessingManagement.PluginProcessing.RunningPluginInformation)
                {
                    if (selectedPlugin == nowItem.IPlugin.PluginName && nowItem.IPlugin.IsWindowExist)
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
        object sender,
        RoutedEventArgs e
        )
    {
        try
        {
            string selectedPlugin = (string)((CheckListBoxItem)PluginListBox.SelectedItem).Text;

            foreach (RunningPluginInformation nowItem in ApplicationData.WindowProcessingManagement.PluginProcessing.RunningPluginInformation)
            {
                if (selectedPlugin == nowItem.IPlugin.PluginName && nowItem.IPlugin.IsWindowExist)
                {
                    nowItem.IPlugin.ShowWindow();
                    break;
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
        object sender,
        RoutedEventArgs e
        )
    {
        try
        {
            if ((int)SettingsRowDefinition.Height.Value == Common.SettingsRowDefinitionMinimize)
            {
                ItemsRowDefinition.Height = new(0);
                SettingsRowDefinition.Height = new(1, GridUnitType.Star);
                SettingsImage.Source = new BitmapImage(new(ApplicationData.Settings.DarkMode ? "/Resources/CloseSettingsWhite.png" : "/Resources/CloseSettingsDark.png", UriKind.Relative));
                SettingsScrollViewer.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            }
            else
            {
                ItemsRowDefinition.Height = new(1, GridUnitType.Star);
                SettingsRowDefinition.Height = new(Common.SettingsRowDefinitionMinimize);
                SettingsImage.Source = new BitmapImage(new(ApplicationData.Settings.DarkMode ? "/Resources/SettingsWhite.png" : "/Resources/SettingsDark.png", UriKind.Relative));
                SettingsScrollViewer.ScrollToVerticalOffset(0);
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
            if (ApplicationData.Settings.PluginInformation.Enabled
                && ProcessingStateToggleSwitch.IsOn == false)
            {
                RestartStackPanel.Visibility = Visibility.Visible;
            }
            ApplicationData.Settings.PluginInformation.Enabled = ProcessingStateToggleSwitch.IsOn;
            ApplicationData.EventData.DoProcessingEvent(ProcessingEventType.PluginProcessingStateChanged);
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
            if (ApplicationData.Languages.LanguagesWindow == null)
            {
                throw new Exception("Languages value is null. - PluginPage.SetTextOnControls()");
            }

            PluginSettingsLabel.Content = ApplicationData.Languages.LanguagesWindow.PluginSettings;
            SettingsLabel.Content = ApplicationData.Languages.LanguagesWindow.Setting;
            ProcessingStateToggleSwitch.OffContent = ProcessingStateToggleSwitch.OnContent = ApplicationData.Languages.LanguagesWindow.ProcessingState;
            RestartButton.Content = ApplicationData.Languages.LanguagesWindow.Restart;
            RestartLabel.Content = ApplicationData.Languages.LanguagesWindow.NeedRestart;
            TestMessageTextBlock.Text = ApplicationData.Languages.LanguagesWindow.PluginTestMessage;
            ExplanationTextBlock.Text = ApplicationData.Languages.LanguagesWindow.PluginExplanation;
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
        if (ApplicationData.Settings.DarkMode)
        {
            PluginSettingsImage.Source = new BitmapImage(new("/Resources/SettingsWhite.png", UriKind.Relative));
            SettingsImage.Source = new BitmapImage(new((int)SettingsRowDefinition.Height.Value == Common.SettingsRowDefinitionMinimize ? "/Resources/SettingsWhite.png" : "/Resources/CloseSettingsWhite.png", UriKind.Relative));
        }
        else
        {
            PluginSettingsImage.Source = new BitmapImage(new("/Resources/SettingsDark.png", UriKind.Relative));
            SettingsImage.Source = new BitmapImage(new((int)SettingsRowDefinition.Height.Value == Common.SettingsRowDefinitionMinimize ? "/Resources/SettingsDark.png" : "/Resources/CloseSettingsDark.png", UriKind.Relative));
        }
    }
}
