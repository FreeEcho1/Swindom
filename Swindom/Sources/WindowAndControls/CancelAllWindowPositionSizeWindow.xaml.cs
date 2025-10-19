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
    private int IndexOfItemToBeModified { get; } = -1;
    /// <summary>
    /// ウィンドウ判定情報
    /// </summary>
    private WindowJudgementInformation WindowJudgementInformation { get; }
    /// <summary>
    /// ウィンドウ情報取得タイマー
    /// </summary>
    private System.Windows.Threading.DispatcherTimer WindowInformationAcquisitionTimer { get; } = new()
    {
        Interval = new(0, 0, 0, 0, WindowProcessingValue.WaitTimeForWindowInformationAcquisition)
    };
    /// <summary>
    /// ウィンドウ選択枠
    /// </summary>
    private FreeEcho.FEWindowSelectionMouse.WindowSelectionFrame WindowSelectionMouse { get; } = new()
    {
        MouseLeftUpStop = true
    };
    /// <summary>
    /// 最小化前のオーナーウィンドウの状態
    /// </summary>
    private WindowState PreviousOwnerWindowState = WindowState.Normal;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <exception cref="Exception"></exception>
    public CancelAllWindowPositionSizeWindow()
    {
        throw new Exception("Do not use. - " + GetType().Name + "()");
    }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="indexOfItemToBeModified">修正する項目のインデックス (追加「-1」)</param>
    /// <exception cref="Exception"></exception>
    public CancelAllWindowPositionSizeWindow(
        int indexOfItemToBeModified = -1
        )
    {
        InitializeComponent();

        IndexOfItemToBeModified = indexOfItemToBeModified;
        WindowJudgementInformation = (IndexOfItemToBeModified == -1) ? new() : new(ApplicationData.Settings.AllWindowInformation.Items[IndexOfItemToBeModified]);

        Owner = WindowManagement.GetWindowToSetOwner();
        SizeToContent = SizeToContent.Manual;
        Width = (ApplicationData.Settings.AllWindowInformation.AddModifyWindowSize.Width < MinWidth) ? MinWidth : ApplicationData.Settings.AllWindowInformation.AddModifyWindowSize.Width;
        Height = (ApplicationData.Settings.AllWindowInformation.AddModifyWindowSize.Height < MinHeight) ? MinHeight : ApplicationData.Settings.AllWindowInformation.AddModifyWindowSize.Height;
        TargetImage.Source = ImageProcessing.GetImageTarget();

        if (IndexOfItemToBeModified == -1)
        {
            Title = ApplicationData.Strings.Add;
            AddOrModifyButton.Content = ApplicationData.Strings.Add;
        }
        else
        {
            Title = ApplicationData.Strings.Modify;
            AddOrModifyButton.Content = ApplicationData.Strings.Modify;
        }
        CancelProcessConditionsExplanationLabel.Content = ApplicationData.Strings.ConditionsNotToBeProcessed;
        RegisteredNameLabel.Content = ApplicationData.Strings.RegisteredName;
        GetWindowInformationButton.Content = ApplicationData.Strings.GetWindowInformation;
        TargetButton.ToolTip = ApplicationData.Strings.HoldDownMousePointerMoveToSelectWindow;
        WindowDecisionExplanationButton.Content = ApplicationData.Strings.Question;
        WindowDecisionExplanationButton.ToolTip = ApplicationData.Strings.Help;
        TitleNameLabel.Content = ApplicationData.Strings.TitleName;
        TitleNameMatchingConditionExactMatchComboBoxItem.Content = ApplicationData.Strings.ExactMatch;
        TitleNameMatchingConditionPartialMatchComboBoxItem.Content = ApplicationData.Strings.PartialMatch;
        TitleNameMatchingConditionForwardMatchComboBoxItem.Content = ApplicationData.Strings.ForwardMatch;
        TitleNameMatchingConditionBackwardMatchComboBoxItem.Content = ApplicationData.Strings.BackwardMatch;
        ClassNameLabel.Content = ApplicationData.Strings.ClassName;
        ClassNameMatchingConditionExactMatchComboBoxItem.Content = ApplicationData.Strings.ExactMatch;
        ClassNameMatchingConditionPartialMatchComboBoxItem.Content = ApplicationData.Strings.PartialMatch;
        ClassNameMatchingConditionForwardMatchComboBoxItem.Content = ApplicationData.Strings.ForwardMatch;
        ClassNameMatchingConditionBackwardMatchComboBoxItem.Content = ApplicationData.Strings.BackwardMatch;
        FileNameLabel.Content = ApplicationData.Strings.FileName;
        FileNameFileSelectionButton.ToolTip = ApplicationData.Strings.FileSelection;
        FileNameMatchingConditionIncludePathComboBoxItem.Content = ApplicationData.Strings.IncludePath;
        FileNameMatchingConditionNotIncludePathComboBoxItem.Content = ApplicationData.Strings.NotIncludePath;
        CancelButton.Content = ApplicationData.Strings.Cancel;

        string stringData;     // 文字列データ
        RegisteredNameTextBox.Text = WindowJudgementInformation.RegisteredName;
        TitleNameTextBox.Text = WindowJudgementInformation.TitleName;
        stringData = WindowJudgementInformation.TitleNameMatchCondition switch
        {
            NameMatchCondition.PartialMatch => ApplicationData.Strings.PartialMatch,
            NameMatchCondition.ForwardMatch => ApplicationData.Strings.ForwardMatch,
            NameMatchCondition.BackwardMatch => ApplicationData.Strings.BackwardMatch,
            _ => ApplicationData.Strings.ExactMatch
        };
        ControlsProcessing.SelectComboBoxItem(TitleNameMatchingConditionComboBox, stringData);
        ClassNameTextBox.Text = WindowJudgementInformation.ClassName;
        stringData = WindowJudgementInformation.ClassNameMatchCondition switch
        {
            NameMatchCondition.PartialMatch => ApplicationData.Strings.PartialMatch,
            NameMatchCondition.ForwardMatch => ApplicationData.Strings.ForwardMatch,
            NameMatchCondition.BackwardMatch => ApplicationData.Strings.BackwardMatch,
            _ => ApplicationData.Strings.ExactMatch
        };
        ControlsProcessing.SelectComboBoxItem(ClassNameMatchingConditionComboBox, stringData);
        FileNameTextBox.Text = WindowJudgementInformation.FileName;
        stringData = WindowJudgementInformation.FileNameMatchCondition switch
        {
            FileNameMatchCondition.NotInclude => ApplicationData.Strings.NotIncludePath,
            _ => ApplicationData.Strings.IncludePath
        };
        ControlsProcessing.SelectComboBoxItem(FileNameMatchingConditionComboBox, stringData);

        SettingsAddAndModifyButtonEnableState();

        Closed += CancelAllWindowPositionSizeWindow_Closed;
        GetWindowInformationButton.Click += GetWindowInformationButton_Click;
        TargetButton.PreviewMouseDown += TargetButton_PreviewMouseDown;
        WindowDecisionExplanationButton.Click += WindowDecisionExplanationButton_Click;
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
            if (FEMessageBox.Show(ApplicationData.Strings.RetrievedAfterFiveSeconds, ApplicationData.Strings.Check, MessageBoxButton.OK) == MessageBoxResult.OK)
            {
                WindowInformationAcquisitionTimer.Start();
                GetWindowInformationStackPanel.IsEnabled = false;
            }
        }
        catch
        {
            FEMessageBox.Show(ApplicationData.Strings.ErrorOccurred, ApplicationData.Strings.Check, MessageBoxButton.OK);
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
            FEMessageBox.Show(ApplicationData.Strings.Obtained, ApplicationData.Strings.Check, MessageBoxButton.OK);
        }
        catch
        {
            Activate();
            FEMessageBox.Show(ApplicationData.Strings.ErrorOccurred, ApplicationData.Strings.Check, MessageBoxButton.OK);
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
            PreviousOwnerWindowState = Owner.WindowState;
            WindowState = WindowState.Minimized;
            Owner.WindowState = WindowState.Minimized;
            WindowSelectionMouse.StartWindowSelection();
        }
        catch
        {
            FEMessageBox.Show(ApplicationData.Strings.ErrorOccurred, ApplicationData.Strings.Check, MessageBoxButton.OK);
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
            Owner.WindowState = PreviousOwnerWindowState;
            WindowState = WindowState.Normal;
            Activate();
            FEMessageBox.Show(ApplicationData.Strings.Obtained, ApplicationData.Strings.Check, MessageBoxButton.OK);
        }
        catch
        {
            FEMessageBox.Show(ApplicationData.Strings.ErrorOccurred, ApplicationData.Strings.Check, MessageBoxButton.OK);
        }
    }

    /// <summary>
    /// 「ウィンドウ判定」の「?」Buttonの「Click」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void WindowDecisionExplanationButton_Click(
        object sender,
        RoutedEventArgs e
        )
    {
        try
        {
            ApplicationData.WindowManagement.ShowExplanationWindow(SelectExplanationType.WindowDecision);
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
                Title = ApplicationData.Strings.FileSelection,
                Filter = ".exe|*.exe*",
                Multiselect = false
            };

            if (openFileDialog.ShowDialog() == true)
            {
                FileNameTextBox.Text = (string)((ComboBoxItem)FileNameMatchingConditionComboBox.SelectedItem).Content == ApplicationData.Strings.NotIncludePath
                    ? Path.GetFileNameWithoutExtension(openFileDialog.FileName)
                    : openFileDialog.FileName;
            }
        }
        catch
        {
            FEMessageBox.Show(ApplicationData.Strings.ErrorOccurred, ApplicationData.Strings.Check, MessageBoxButton.OK);
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
                    ApplicationData.Settings.AllWindowInformation.Items.Add(WindowJudgementInformation);
                    ApplicationData.WindowProcessingManagement.SpecifyWindowProcessing?.ProcessingSettings();
                    FEMessageBox.Show(ApplicationData.Strings.Added, ApplicationData.Strings.Check, MessageBoxButton.OK);
                    AddedOrModified = true;
                    Close();
                }
                // 修正
                else
                {
                    ApplicationData.WindowProcessingManagement.SpecifyWindowProcessing?.UnregisterHotkeys();
                    ApplicationData.Settings.AllWindowInformation.Items[IndexOfItemToBeModified] = WindowJudgementInformation;
                    ApplicationData.WindowProcessingManagement.SpecifyWindowProcessing?.ProcessingSettings();
                    FEMessageBox.Show(ApplicationData.Strings.Modified, ApplicationData.Strings.Check, MessageBoxButton.OK);
                    AddedOrModified = true;
                    Close();
                }
            }
            else
            {
                FEMessageBox.Show(check, ApplicationData.Strings.Check, MessageBoxButton.OK);
            }
        }
        catch
        {
            FEMessageBox.Show(ApplicationData.Strings.ErrorOccurred, ApplicationData.Strings.Check, MessageBoxButton.OK);
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
        WindowInformation windowInformation = WindowProcessing.GetWindowInformationFromHandle(hwnd);
        TitleNameTextBox.Text = windowInformation.TitleName;
        ClassNameTextBox.Text = windowInformation.ClassName;
        FileNameTextBox.Text = (string)((ComboBoxItem)FileNameMatchingConditionComboBox.SelectedItem).Content == ApplicationData.Strings.IncludePath ? windowInformation.FileName : Path.GetFileNameWithoutExtension(windowInformation.FileName);
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
        if (stringData == ApplicationData.Strings.ExactMatch)
        {
            WindowJudgementInformation.TitleNameMatchCondition = NameMatchCondition.ExactMatch;
        }
        else if (stringData == ApplicationData.Strings.PartialMatch)
        {
            WindowJudgementInformation.TitleNameMatchCondition = NameMatchCondition.PartialMatch;
        }
        else if (stringData == ApplicationData.Strings.ForwardMatch)
        {
            WindowJudgementInformation.TitleNameMatchCondition = NameMatchCondition.ForwardMatch;
        }
        else if (stringData == ApplicationData.Strings.BackwardMatch)
        {
            WindowJudgementInformation.TitleNameMatchCondition = NameMatchCondition.BackwardMatch;
        }
        WindowJudgementInformation.ClassName = ClassNameTextBox.Text;
        stringData = (string)((ComboBoxItem)ClassNameMatchingConditionComboBox.SelectedItem).Content;
        if (stringData == ApplicationData.Strings.ExactMatch)
        {
            WindowJudgementInformation.ClassNameMatchCondition = NameMatchCondition.ExactMatch;
        }
        else if (stringData == ApplicationData.Strings.PartialMatch)
        {
            WindowJudgementInformation.ClassNameMatchCondition = NameMatchCondition.PartialMatch;
        }
        else if (stringData == ApplicationData.Strings.ForwardMatch)
        {
            WindowJudgementInformation.ClassNameMatchCondition = NameMatchCondition.ForwardMatch;
        }
        else if (stringData == ApplicationData.Strings.BackwardMatch)
        {
            WindowJudgementInformation.ClassNameMatchCondition = NameMatchCondition.BackwardMatch;
        }
        WindowJudgementInformation.FileName = FileNameTextBox.Text;
        stringData = (string)((ComboBoxItem)FileNameMatchingConditionComboBox.SelectedItem).Content;
        if (stringData == ApplicationData.Strings.IncludePath)
        {
            WindowJudgementInformation.FileNameMatchCondition = FileNameMatchCondition.Include;
        }
        else if (stringData == ApplicationData.Strings.NotIncludePath)
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
            foreach (WindowJudgementInformation nowItem in ApplicationData.Settings.AllWindowInformation.Items)
            {
                if (WindowJudgementInformation.RegisteredName == nowItem.RegisteredName)
                {
                    result = ApplicationData.Strings.ThereIsADuplicateRegistrationName;
                    break;
                }
            }
        }
        else
        {
            int count = 0;
            foreach (WindowJudgementInformation nowItem in ApplicationData.Settings.AllWindowInformation.Items)
            {
                if (count != IndexOfItemToBeModified
                    && WindowJudgementInformation.RegisteredName == nowItem.RegisteredName)
                {
                    result = ApplicationData.Strings.ThereIsADuplicateRegistrationName;
                    break;
                }
                count++;
            }
        }

        return result;
    }
}
