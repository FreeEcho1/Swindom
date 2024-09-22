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
    /// ウィンドウ判定情報
    /// </summary>
    private readonly WindowJudgementInformation WindowJudgementInformation;
    /// <summary>
    /// ウィンドウ情報取得タイマー
    /// </summary>
    private readonly System.Windows.Threading.DispatcherTimer WindowInformationAcquisitionTimer = new()
    {
        Interval = new(0, 0, 0, 0, WindowProcessingValue.WaitTimeForWindowInformationAcquisition)
    };
    /// <summary>
    /// ウィンドウ選択枠
    /// </summary>
    private readonly FreeEcho.FEWindowSelectionMouse.WindowSelectionFrame WindowSelectionMouse = new()
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
            Title = ApplicationData.Languages.Add;
            AddOrModifyButton.Content = ApplicationData.Languages.Add;
        }
        else
        {
            Title = ApplicationData.Languages.Modify;
            AddOrModifyButton.Content = ApplicationData.Languages.Modify;
        }
        CancelProcessConditionsExplanationLabel.Content = ApplicationData.Languages.ConditionsNotToBeProcessed;
        RegisteredNameLabel.Content = ApplicationData.Languages.RegisteredName;
        GetWindowInformationButton.Content = ApplicationData.Languages.GetWindowInformation;
        TargetButton.ToolTip = ApplicationData.Languages.HoldDownMousePointerMoveToSelectWindow;
        WindowDecisionExplanationButton.Content = ApplicationData.Languages.Question;
        WindowDecisionExplanationButton.ToolTip = ApplicationData.Languages.Help;
        TitleNameLabel.Content = ApplicationData.Languages.TitleName;
        TitleNameMatchingConditionExactMatchComboBoxItem.Content = ApplicationData.Languages.ExactMatch;
        TitleNameMatchingConditionPartialMatchComboBoxItem.Content = ApplicationData.Languages.PartialMatch;
        TitleNameMatchingConditionForwardMatchComboBoxItem.Content = ApplicationData.Languages.ForwardMatch;
        TitleNameMatchingConditionBackwardMatchComboBoxItem.Content = ApplicationData.Languages.BackwardMatch;
        ClassNameLabel.Content = ApplicationData.Languages.ClassName;
        ClassNameMatchingConditionExactMatchComboBoxItem.Content = ApplicationData.Languages.ExactMatch;
        ClassNameMatchingConditionPartialMatchComboBoxItem.Content = ApplicationData.Languages.PartialMatch;
        ClassNameMatchingConditionForwardMatchComboBoxItem.Content = ApplicationData.Languages.ForwardMatch;
        ClassNameMatchingConditionBackwardMatchComboBoxItem.Content = ApplicationData.Languages.BackwardMatch;
        FileNameLabel.Content = ApplicationData.Languages.FileName;
        FileNameFileSelectionButton.ToolTip = ApplicationData.Languages.FileSelection;
        FileNameMatchingConditionIncludePathComboBoxItem.Content = ApplicationData.Languages.IncludePath;
        FileNameMatchingConditionNotIncludePathComboBoxItem.Content = ApplicationData.Languages.NotIncludePath;
        CancelButton.Content = ApplicationData.Languages.Cancel;

        string stringData;     // 文字列データ
        RegisteredNameTextBox.Text = WindowJudgementInformation.RegisteredName;
        TitleNameTextBox.Text = WindowJudgementInformation.TitleName;
        stringData = WindowJudgementInformation.TitleNameMatchCondition switch
        {
            NameMatchCondition.PartialMatch => ApplicationData.Languages.PartialMatch,
            NameMatchCondition.ForwardMatch => ApplicationData.Languages.ForwardMatch,
            NameMatchCondition.BackwardMatch => ApplicationData.Languages.BackwardMatch,
            _ => ApplicationData.Languages.ExactMatch
        };
        ControlsProcessing.SelectComboBoxItem(TitleNameMatchingConditionComboBox, stringData);
        ClassNameTextBox.Text = WindowJudgementInformation.ClassName;
        stringData = WindowJudgementInformation.ClassNameMatchCondition switch
        {
            NameMatchCondition.PartialMatch => ApplicationData.Languages.PartialMatch,
            NameMatchCondition.ForwardMatch => ApplicationData.Languages.ForwardMatch,
            NameMatchCondition.BackwardMatch => ApplicationData.Languages.BackwardMatch,
            _ => ApplicationData.Languages.ExactMatch
        };
        ControlsProcessing.SelectComboBoxItem(ClassNameMatchingConditionComboBox, stringData);
        FileNameTextBox.Text = WindowJudgementInformation.FileName;
        stringData = WindowJudgementInformation.FileNameMatchCondition switch
        {
            FileNameMatchCondition.NotInclude => ApplicationData.Languages.NotIncludePath,
            _ => ApplicationData.Languages.IncludePath
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
            if (FEMessageBox.Show(ApplicationData.Languages.RetrievedAfterFiveSeconds, ApplicationData.Languages.Check, MessageBoxButton.OK) == MessageBoxResult.OK)
            {
                WindowInformationAcquisitionTimer.Start();
                GetWindowInformationStackPanel.IsEnabled = false;
            }
        }
        catch
        {
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
            FEMessageBox.Show(ApplicationData.Languages.Obtained, ApplicationData.Languages.Check, MessageBoxButton.OK);
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
            PreviousOwnerWindowState = Owner.WindowState;
            WindowState = WindowState.Minimized;
            Owner.WindowState = WindowState.Minimized;
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
            Owner.WindowState = PreviousOwnerWindowState;
            WindowState = WindowState.Normal;
            Activate();
            FEMessageBox.Show(ApplicationData.Languages.Obtained, ApplicationData.Languages.Check, MessageBoxButton.OK);
        }
        catch
        {
            FEMessageBox.Show(ApplicationData.Languages.ErrorOccurred, ApplicationData.Languages.Check, MessageBoxButton.OK);
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
                Title = ApplicationData.Languages.FileSelection,
                Filter = ".exe|*.exe*",
                Multiselect = false
            };

            if (openFileDialog.ShowDialog() == true)
            {
                FileNameTextBox.Text = (string)((ComboBoxItem)FileNameMatchingConditionComboBox.SelectedItem).Content == ApplicationData.Languages.NotIncludePath
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
                    ApplicationData.Settings.AllWindowInformation.Items.Add(WindowJudgementInformation);
                    ApplicationData.WindowProcessingManagement.SpecifyWindowProcessing?.ProcessingSettings();
                    FEMessageBox.Show(ApplicationData.Languages.Added, ApplicationData.Languages.Check, MessageBoxButton.OK);
                    AddedOrModified = true;
                    Close();
                }
                // 修正
                else
                {
                    ApplicationData.WindowProcessingManagement.SpecifyWindowProcessing?.UnregisterHotkeys();
                    ApplicationData.Settings.AllWindowInformation.Items[IndexOfItemToBeModified] = WindowJudgementInformation;
                    ApplicationData.WindowProcessingManagement.SpecifyWindowProcessing?.ProcessingSettings();
                    FEMessageBox.Show(ApplicationData.Languages.Modified, ApplicationData.Languages.Check, MessageBoxButton.OK);
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
        WindowInformation windowInformation = WindowProcessing.GetWindowInformationFromHandle(hwnd);
        TitleNameTextBox.Text = windowInformation.TitleName;
        ClassNameTextBox.Text = windowInformation.ClassName;
        FileNameTextBox.Text = (string)((ComboBoxItem)FileNameMatchingConditionComboBox.SelectedItem).Content == ApplicationData.Languages.IncludePath ? windowInformation.FileName : Path.GetFileNameWithoutExtension(windowInformation.FileName);
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
        if (stringData == ApplicationData.Languages.ExactMatch)
        {
            WindowJudgementInformation.TitleNameMatchCondition = NameMatchCondition.ExactMatch;
        }
        else if (stringData == ApplicationData.Languages.PartialMatch)
        {
            WindowJudgementInformation.TitleNameMatchCondition = NameMatchCondition.PartialMatch;
        }
        else if (stringData == ApplicationData.Languages.ForwardMatch)
        {
            WindowJudgementInformation.TitleNameMatchCondition = NameMatchCondition.ForwardMatch;
        }
        else if (stringData == ApplicationData.Languages.BackwardMatch)
        {
            WindowJudgementInformation.TitleNameMatchCondition = NameMatchCondition.BackwardMatch;
        }
        WindowJudgementInformation.ClassName = ClassNameTextBox.Text;
        stringData = (string)((ComboBoxItem)ClassNameMatchingConditionComboBox.SelectedItem).Content;
        if (stringData == ApplicationData.Languages.ExactMatch)
        {
            WindowJudgementInformation.ClassNameMatchCondition = NameMatchCondition.ExactMatch;
        }
        else if (stringData == ApplicationData.Languages.PartialMatch)
        {
            WindowJudgementInformation.ClassNameMatchCondition = NameMatchCondition.PartialMatch;
        }
        else if (stringData == ApplicationData.Languages.ForwardMatch)
        {
            WindowJudgementInformation.ClassNameMatchCondition = NameMatchCondition.ForwardMatch;
        }
        else if (stringData == ApplicationData.Languages.BackwardMatch)
        {
            WindowJudgementInformation.ClassNameMatchCondition = NameMatchCondition.BackwardMatch;
        }
        WindowJudgementInformation.FileName = FileNameTextBox.Text;
        stringData = (string)((ComboBoxItem)FileNameMatchingConditionComboBox.SelectedItem).Content;
        if (stringData == ApplicationData.Languages.IncludePath)
        {
            WindowJudgementInformation.FileNameMatchCondition = FileNameMatchCondition.Include;
        }
        else if (stringData == ApplicationData.Languages.NotIncludePath)
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
                    result = ApplicationData.Languages.ThereIsADuplicateRegistrationName;
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
                    result = ApplicationData.Languages.ThereIsADuplicateRegistrationName;
                    break;
                }
                count++;
            }
        }

        return result;
    }
}
