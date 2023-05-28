namespace Swindom;

/// <summary>
/// 「全てのウィンドウを指定した位置に移動」の「移動しないウィンドウ」の追加/修正ウィンドウ
/// </summary>
public partial class CancelAllWindowPositionSizeWindow : Window
{
    /// <summary>
    /// 追加/修正したかの値 (いいえ「false」/はい「true」)
    /// </summary>
    public bool AddedOrModified { get; private set; }
    /// <summary>
    /// 修正する項目のインデックス (追加「-1」)
    /// </summary>
    private readonly int IndexOfItemToBeModified = -1;
    /// <summary>
    /// ウィンドウ情報取得の待ち時間
    /// </summary>
    private const int WaitTimeForWindowInformationAcquisition = 5000;
    /// <summary>
    /// ウィンドウ判定情報
    /// </summary>
    private readonly WindowJudgementInformation WindowJudgementInformation;
    /// <summary>
    /// ウィンドウ情報取得タイマー
    /// </summary>
    private readonly System.Windows.Threading.DispatcherTimer WindowInformationAcquisitionTimer = new()
    {
        Interval = new(0, 0, 0, 0, WaitTimeForWindowInformationAcquisition)
    };
    /// <summary>
    /// ウィンドウ選択枠
    /// </summary>
    private readonly FreeEcho.FEWindowSelectionMouse.WindowSelectionFrame WindowSelectionMouse = new()
    {
        MouseLeftUpStop = true
    };
    /// <summary>
    /// ウィンドウ情報のバッファ
    /// </summary>
    private readonly WindowInformationBuffer WindowInformationBuffer = new();

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <exception cref="Exception"></exception>
    public CancelAllWindowPositionSizeWindow()
    {
        throw new Exception("Do not use. - CancelAllWindowPositionSizeWindow()");
    }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="ownerWindow">オーナーウィンドウ</param>
    /// <param name="indexOfItemToBeModified">修正する項目のインデックス (追加「-1」)</param>
    /// <exception cref="Exception"></exception>
    public CancelAllWindowPositionSizeWindow(
        Window? ownerWindow,
        int indexOfItemToBeModified = -1
        )
    {
        if (ApplicationData.Languages.LanguagesWindow == null)
        {
            throw new Exception("Languages value is null. - CancelAllWindowPositionSizeWindow()");
        }

        InitializeComponent();

        IndexOfItemToBeModified = indexOfItemToBeModified;
        WindowJudgementInformation = (IndexOfItemToBeModified == -1) ? new() : new(ApplicationData.Settings.AllWindowInformation.CancelAllWindowPositionSize[IndexOfItemToBeModified]);

        if (ownerWindow != null)
        {
            Owner = ownerWindow;
        }
        SizeToContent = SizeToContent.Manual;
        Width = (ApplicationData.Settings.AllWindowInformation.AddModifyWindowSize.Width < MinWidth) ? MinWidth : ApplicationData.Settings.AllWindowInformation.AddModifyWindowSize.Width;
        Height = (ApplicationData.Settings.AllWindowInformation.AddModifyWindowSize.Height < MinHeight) ? MinHeight : ApplicationData.Settings.AllWindowInformation.AddModifyWindowSize.Height;
        if (ApplicationData.Settings.DarkMode)
        {
            TargetImage.Source = new BitmapImage(new(ApplicationData.Settings.DarkMode ? "/Resources/TargetWhite.png" : "/Resources/TargetDark.png", UriKind.Relative));
        }

        if (IndexOfItemToBeModified == -1)
        {
            Title = ApplicationData.Languages.LanguagesWindow.Add;
            AddOrModifyButton.Content = ApplicationData.Languages.LanguagesWindow.Add;
        }
        else
        {
            Title = ApplicationData.Languages.LanguagesWindow.Modify;
            AddOrModifyButton.Content = ApplicationData.Languages.LanguagesWindow.Modify;
        }
        RegisteredNameLabel.Content = ApplicationData.Languages.LanguagesWindow.RegisteredName;
        GetWindowInformationButton.Content = ApplicationData.Languages.LanguagesWindow.GetWindowInformation;
        TargetButton.ToolTip = ApplicationData.Languages.LanguagesWindow.HoldDownMouseCursorMoveToSelectWindow;
        WindowDesignateMethodButton.Content = ApplicationData.Languages.LanguagesWindow.WindowDesignateMethod;
        TitleNameLabel.Content = ApplicationData.Languages.LanguagesWindow.TitleName;
        ((ComboBoxItem)TitleNameMatchingConditionComboBox.Items[0]).Content = ApplicationData.Languages.LanguagesWindow.ExactMatch;
        ((ComboBoxItem)TitleNameMatchingConditionComboBox.Items[1]).Content = ApplicationData.Languages.LanguagesWindow.PartialMatch;
        ((ComboBoxItem)TitleNameMatchingConditionComboBox.Items[2]).Content = ApplicationData.Languages.LanguagesWindow.ForwardMatch;
        ((ComboBoxItem)TitleNameMatchingConditionComboBox.Items[3]).Content = ApplicationData.Languages.LanguagesWindow.BackwardMatch;
        ClassNameLabel.Content = ApplicationData.Languages.LanguagesWindow.ClassName;
        ((ComboBoxItem)ClassNameMatchingConditionComboBox.Items[0]).Content = ApplicationData.Languages.LanguagesWindow.ExactMatch;
        ((ComboBoxItem)ClassNameMatchingConditionComboBox.Items[1]).Content = ApplicationData.Languages.LanguagesWindow.PartialMatch;
        ((ComboBoxItem)ClassNameMatchingConditionComboBox.Items[2]).Content = ApplicationData.Languages.LanguagesWindow.ForwardMatch;
        ((ComboBoxItem)ClassNameMatchingConditionComboBox.Items[3]).Content = ApplicationData.Languages.LanguagesWindow.BackwardMatch;
        FileNameLabel.Content = ApplicationData.Languages.LanguagesWindow.FileName;
        FileNameFileSelectionButton.ToolTip = ApplicationData.Languages.LanguagesWindow.FileSelection;
        ((ComboBoxItem)FileNameMatchingConditionComboBox.Items[0]).Content = ApplicationData.Languages.LanguagesWindow.IncludePath;
        ((ComboBoxItem)FileNameMatchingConditionComboBox.Items[1]).Content = ApplicationData.Languages.LanguagesWindow.NotIncludePath;
        CancelButton.Content = ApplicationData.Languages.LanguagesWindow.Cancel;

        string stringData;     // 文字列データ
        RegisteredNameTextBox.Text = WindowJudgementInformation.RegisteredName;
        TitleNameTextBox.Text = WindowJudgementInformation.TitleName;
        stringData = WindowJudgementInformation.TitleNameMatchCondition switch
        {
            NameMatchCondition.PartialMatch => ApplicationData.Languages.LanguagesWindow.PartialMatch,
            NameMatchCondition.ForwardMatch => ApplicationData.Languages.LanguagesWindow.ForwardMatch,
            NameMatchCondition.BackwardMatch => ApplicationData.Languages.LanguagesWindow.BackwardMatch,
            _ => ApplicationData.Languages.LanguagesWindow.ExactMatch
        };
        VariousProcessing.SelectComboBoxItem(TitleNameMatchingConditionComboBox, stringData);
        ClassNameTextBox.Text = WindowJudgementInformation.ClassName;
        stringData = WindowJudgementInformation.ClassNameMatchCondition switch
        {
            NameMatchCondition.PartialMatch => ApplicationData.Languages.LanguagesWindow.PartialMatch,
            NameMatchCondition.ForwardMatch => ApplicationData.Languages.LanguagesWindow.ForwardMatch,
            NameMatchCondition.BackwardMatch => ApplicationData.Languages.LanguagesWindow.BackwardMatch,
            _ => ApplicationData.Languages.LanguagesWindow.ExactMatch
        };
        VariousProcessing.SelectComboBoxItem(ClassNameMatchingConditionComboBox, stringData);
        FileNameTextBox.Text = WindowJudgementInformation.FileName;
        stringData = WindowJudgementInformation.FileNameMatchCondition switch
        {
            FileNameMatchCondition.NotInclude => ApplicationData.Languages.LanguagesWindow.NotIncludePath,
            _ => ApplicationData.Languages.LanguagesWindow.IncludePath
        };
        VariousProcessing.SelectComboBoxItem(FileNameMatchingConditionComboBox, stringData);

        SettingsAddAndModifyButtonEnableState();

        Closed += CancelAllWindowPositionSizeWindow_Closed;
        GetWindowInformationButton.Click += GetWindowInformationButton_Click;
        TargetButton.PreviewMouseDown += TargetButton_PreviewMouseDown;
        WindowDesignateMethodButton.Click += WindowDesignateMethodButton_Click;
        TitleNameTextBox.TextChanged += TitleNameTextBox_TextChanged;
        ClassNameTextBox.TextChanged += ClassNameTextBox_TextChanged;
        FileNameTextBox.TextChanged += FileNameTextBox_TextChanged;
        FileNameFileSelectionButton.Click += FileNameFileSelectionButton_Click;
        AddOrModifyButton.Click += AddOrModifyButton_Click;
        CancelButton.Click += CancelButton_Click;

        WindowInformationAcquisitionTimer.Tick += WindowInformationAcquisitionTimer_Tick;
        WindowSelectionMouse.MouseLeftButtonUp += WindowSelectionMouse_MouseLeftButtonUp;
    }

    /// <summary>
    /// ウィンドウの「Closed」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void CancelAllWindowPositionSizeWindow_Closed(
        object? sender,
        EventArgs e
        )
    {
        try
        {
            ApplicationData.Settings.AllWindowInformation.AddModifyWindowSize.Width = (int)Width;
            ApplicationData.Settings.AllWindowInformation.AddModifyWindowSize.Height = (int)Height;
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「ウィンドウ情報取得」Buttonの「Click」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void GetWindowInformationButton_Click(
        object sender,
        RoutedEventArgs e
        )
    {
        try
        {
            if (FEMessageBox.Show(ApplicationData.Languages.LanguagesWindow?.RetrievedAfterFiveSeconds ?? "", ApplicationData.Languages.Check, MessageBoxButton.OK) == MessageBoxResult.OK)
            {
                GetWindowInformationStackPanel.IsEnabled = false;
                WindowInformationAcquisitionTimer.Start();
            }
        }
        catch
        {
            GetWindowInformationStackPanel.IsEnabled = true;
            FEMessageBox.Show(ApplicationData.Languages.ErrorOccurred, ApplicationData.Languages.Check, MessageBoxButton.OK);
        }
    }

    /// <summary>
    /// 「ウィンドウ情報取得タイマー」の「Tick」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void WindowInformationAcquisitionTimer_Tick(
        object? sender,
        EventArgs e
        )
    {
        try
        {
            WindowInformationAcquisitionTimer.Stop();
            GetInformationFromWindowHandle(NativeMethods.GetForegroundWindow());
            Activate();
            FEMessageBox.Show(ApplicationData.Languages.LanguagesWindow?.SelectionComplete ?? "", ApplicationData.Languages.Check, MessageBoxButton.OK);
        }
        catch
        {
            Activate();
            FEMessageBox.Show(ApplicationData.Languages.ErrorOccurred, ApplicationData.Languages.Check, MessageBoxButton.OK);
        }
        finally
        {
            GetWindowInformationStackPanel.IsEnabled = true;
        }
    }

    /// <summary>
    /// 「マウスでウィンドウ情報取得」Buttonの「PreviewMouseDown」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void TargetButton_PreviewMouseDown(
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
    /// 「マウスでウィンドウ情報取得」Buttonの「MouseLeftButtonUp」イベント
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
            GetInformationFromWindowHandle(WindowSelectionMouse.SelectedHwnd);
            FEMessageBox.Show(ApplicationData.Languages.LanguagesWindow?.SelectionComplete ?? "", ApplicationData.Languages.Check, MessageBoxButton.OK);
        }
        catch
        {
            FEMessageBox.Show(ApplicationData.Languages.ErrorOccurred, ApplicationData.Languages.Check, MessageBoxButton.OK);
        }
    }

    /// <summary>
    /// 「ウィンドウ判定の指定方法」Buttonの「Click」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void WindowDesignateMethodButton_Click(
        object sender,
        RoutedEventArgs e
        )
    {
        try
        {
            ApplicationData.WindowManagement.ShowDesignateMethodWindow(this);
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「タイトル名」TextBoxの「TextChanged」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void TitleNameTextBox_TextChanged(
        object sender,
        TextChangedEventArgs e
        )
    {
        try
        {
            SettingsAddAndModifyButtonEnableState();
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「クラス名」TextBoxの「TextChanged」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ClassNameTextBox_TextChanged(
        object sender,
        TextChangedEventArgs e
        )
    {
        try
        {
            SettingsAddAndModifyButtonEnableState();
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「ファイル名」TextBoxの「TextChanged」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void FileNameTextBox_TextChanged(
        object sender,
        TextChangedEventArgs e
        )
    {
        try
        {
            SettingsAddAndModifyButtonEnableState();
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「ファイル名選択」Buttonの「Click」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void FileNameFileSelectionButton_Click(
        object sender,
        RoutedEventArgs e
        )
    {
        try
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new()
            {
                Title = ApplicationData.Languages.LanguagesWindow?.FileSelection,
                Filter = ".exe|*.exe*",
                Multiselect = false
            };

            if (openFileDialog.ShowDialog() == true)
            {
                FileNameTextBox.Text = (string)((ComboBoxItem)FileNameMatchingConditionComboBox.SelectedItem).Content == ApplicationData.Languages.LanguagesWindow?.NotIncludePath
                    ? Path.GetFileNameWithoutExtension(openFileDialog.FileName)
                    : openFileDialog.FileName;
            }
        }
        catch
        {
            FEMessageBox.Show(ApplicationData.Languages.ErrorOccurred, ApplicationData.Languages.Check, MessageBoxButton.OK);
        }
    }

    /// <summary>
    /// 「追加/修正」Buttonの「Click」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void AddOrModifyButton_Click(
        object sender,
        RoutedEventArgs e
        )
    {
        try
        {
            // 登録名が入力されていない場合は名前を決める
            if (string.IsNullOrEmpty(RegisteredNameTextBox.Text))
            {
                if (string.IsNullOrEmpty(TitleNameTextBox.Text) == false)
                {
                    RegisteredNameTextBox.Text = TitleNameTextBox.Text;
                }
                else if (string.IsNullOrEmpty(ClassNameTextBox.Text) == false)
                {
                    RegisteredNameTextBox.Text = ClassNameTextBox.Text;
                }
                else if (string.IsNullOrEmpty(FileNameTextBox.Text) == false)
                {
                    RegisteredNameTextBox.Text = FileNameTextBox.Text;
                }
            }

            GetWindowJudgementInformationFromControls();
            string? check = CheckWindowJudgementInformation();
            if (string.IsNullOrEmpty(check))
            {
                // 追加
                if (IndexOfItemToBeModified == -1)
                {
                    ApplicationData.Settings.AllWindowInformation.CancelAllWindowPositionSize.Add(WindowJudgementInformation);
                    ApplicationData.WindowProcessingManagement.SpecifyWindowProcessing?.ProcessingSettings();
                    FEMessageBox.Show(ApplicationData.Languages.LanguagesWindow?.Added ?? "", ApplicationData.Languages.Check, MessageBoxButton.OK);
                    AddedOrModified = true;
                    Close();
                }
                // 修正
                else
                {
                    ApplicationData.WindowProcessingManagement.SpecifyWindowProcessing?.UnregisterHotkeys();
                    ApplicationData.Settings.AllWindowInformation.CancelAllWindowPositionSize[IndexOfItemToBeModified] = WindowJudgementInformation;
                    ApplicationData.WindowProcessingManagement.SpecifyWindowProcessing?.ProcessingSettings();
                    FEMessageBox.Show(ApplicationData.Languages.LanguagesWindow?.Modified ?? "", ApplicationData.Languages.Check, MessageBoxButton.OK);
                    AddedOrModified = true;
                    Close();
                }
            }
            else
            {
                FEMessageBox.Show(check, ApplicationData.Languages.Check, MessageBoxButton.OK);
            }
        }
        catch
        {
            FEMessageBox.Show(ApplicationData.Languages.ErrorOccurred, ApplicationData.Languages.Check, MessageBoxButton.OK);
        }
    }

    /// <summary>
    /// 「キャンセル」Buttonの「Click」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void CancelButton_Click(
        object sender,
        RoutedEventArgs e
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
    /// 「追加/修正」ボタンの有効状態を設定
    /// </summary>
    private void SettingsAddAndModifyButtonEnableState()
    {
        AddOrModifyButton.IsEnabled = !string.IsNullOrEmpty(TitleNameTextBox.Text) || !string.IsNullOrEmpty(ClassNameTextBox.Text) || !string.IsNullOrEmpty(FileNameTextBox.Text);
    }

    /// <summary>
    /// ウィンドウハンドルから情報を取得してコントロールに値を設定
    /// </summary>
    /// <param name="hwnd">ウィンドウハンドル</param>
    private void GetInformationFromWindowHandle(
        IntPtr hwnd
        )
    {
        WindowInformation windowInformation = VariousWindowProcessing.GetWindowInformationFromHandle(hwnd, WindowInformationBuffer);
        TitleNameTextBox.Text = windowInformation.TitleName;
        ClassNameTextBox.Text = windowInformation.ClassName;
        FileNameTextBox.Text = (string)((ComboBoxItem)FileNameMatchingConditionComboBox.SelectedItem).Content == ApplicationData.Languages.LanguagesWindow?.IncludePath ? windowInformation.FileName : Path.GetFileNameWithoutExtension(windowInformation.FileName);
    }

    /// <summary>
    /// コントロールから「WindowJudgementInformation」の値を取得
    /// </summary>
    private void GetWindowJudgementInformationFromControls()
    {
        string stringData;     // 文字列

        WindowJudgementInformation.RegisteredName = RegisteredNameTextBox.Text;
        WindowJudgementInformation.TitleName = TitleNameTextBox.Text;
        stringData = (string)((ComboBoxItem)TitleNameMatchingConditionComboBox.SelectedItem).Content;
        if (stringData == ApplicationData.Languages.LanguagesWindow?.ExactMatch)
        {
            WindowJudgementInformation.TitleNameMatchCondition = NameMatchCondition.ExactMatch;
        }
        else if (stringData == ApplicationData.Languages.LanguagesWindow?.PartialMatch)
        {
            WindowJudgementInformation.TitleNameMatchCondition = NameMatchCondition.PartialMatch;
        }
        else if (stringData == ApplicationData.Languages.LanguagesWindow?.ForwardMatch)
        {
            WindowJudgementInformation.TitleNameMatchCondition = NameMatchCondition.ForwardMatch;
        }
        else if (stringData == ApplicationData.Languages.LanguagesWindow?.BackwardMatch)
        {
            WindowJudgementInformation.TitleNameMatchCondition = NameMatchCondition.BackwardMatch;
        }
        WindowJudgementInformation.ClassName = ClassNameTextBox.Text;
        stringData = (string)((ComboBoxItem)ClassNameMatchingConditionComboBox.SelectedItem).Content;
        if (stringData == ApplicationData.Languages.LanguagesWindow?.ExactMatch)
        {
            WindowJudgementInformation.ClassNameMatchCondition = NameMatchCondition.ExactMatch;
        }
        else if (stringData == ApplicationData.Languages.LanguagesWindow?.PartialMatch)
        {
            WindowJudgementInformation.ClassNameMatchCondition = NameMatchCondition.PartialMatch;
        }
        else if (stringData == ApplicationData.Languages.LanguagesWindow?.ForwardMatch)
        {
            WindowJudgementInformation.ClassNameMatchCondition = NameMatchCondition.ForwardMatch;
        }
        else if (stringData == ApplicationData.Languages.LanguagesWindow?.BackwardMatch)
        {
            WindowJudgementInformation.ClassNameMatchCondition = NameMatchCondition.BackwardMatch;
        }
        WindowJudgementInformation.FileName = FileNameTextBox.Text;
        stringData = (string)((ComboBoxItem)FileNameMatchingConditionComboBox.SelectedItem).Content;
        if (stringData == ApplicationData.Languages.LanguagesWindow?.IncludePath)
        {
            WindowJudgementInformation.FileNameMatchCondition = FileNameMatchCondition.Include;
        }
        else if (stringData == ApplicationData.Languages.LanguagesWindow?.NotIncludePath)
        {
            WindowJudgementInformation.FileNameMatchCondition = FileNameMatchCondition.NotInclude;
        }
    }

    /// <summary>
    /// 「WindowJudgementInformation」の値を確認
    /// </summary>
    /// <returns>結果の文字列 (問題ない「null」/問題がある「理由の文字列」)</returns>
    private string? CheckWindowJudgementInformation()
    {
        string? result = null;     // 結果

        // 登録名の重複確認
        if (IndexOfItemToBeModified == -1)
        {
            foreach (WindowJudgementInformation nowItem in ApplicationData.Settings.AllWindowInformation.CancelAllWindowPositionSize)
            {
                if (WindowJudgementInformation.RegisteredName == nowItem.RegisteredName)
                {
                    result = ApplicationData.Languages.LanguagesWindow?.ThereIsADuplicateRegistrationName;
                    break;
                }
            }
        }
        else
        {
            int count = 0;
            foreach (WindowJudgementInformation nowItem in ApplicationData.Settings.AllWindowInformation.CancelAllWindowPositionSize)
            {
                if (count != IndexOfItemToBeModified
                    && WindowJudgementInformation.RegisteredName == nowItem.RegisteredName)
                {
                    result = ApplicationData.Languages.LanguagesWindow?.ThereIsADuplicateRegistrationName;
                    break;
                }
                count++;
            }
        }

        return result;
    }
}
