namespace Swindom;

/// <summary>
/// 「ディスプレイ選択」ウィンドウ
/// </summary>
public partial class SelectDisplayWindow : Window
{
    /// <summary>
    /// 修正したかの値 (「false」キャンセル/「true」修正)
    /// </summary>
    public bool IsModified { get; private set; } = false;
    /// <summary>
    /// 選択したディスプレイ
    /// </summary>
    public string SelectedDisplay { get; private set; } = "";
    /// <summary>
    /// 以降も同じ設定や操作を適用 (「false」適用しない/「true」適用する)
    /// </summary>
    public bool ApplySameSettingToRemaining { get; private set; } = false;

    /// <summary>
    /// コンストラクタ (使用しない)
    /// </summary>
    public SelectDisplayWindow()
    {
        throw new Exception("Do not use. - " + GetType().Name + "()");
    }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="functionName">表示する機能名</param>
    /// <param name="registerName">表示する登録名</param>
    public SelectDisplayWindow(
        string functionName,
        string registerName
        )
    {
        InitializeComponent();

        foreach (MonitorInfoEx nowMonitorInfo in ApplicationData.MonitorInformation.MonitorInfo)
        {
            ComboBoxItem newItem = new()
            {
                Content = nowMonitorInfo.DeviceName
            };
            DisplayComboBox.Items.Add(newItem);
        }
        DisplayComboBox.SelectedIndex = 0;

        Title = ApplicationData.Strings.Check + WindowControlValue.CopySeparateString + ApplicationValue.ApplicationName;
        MessageTextBlock.Text = ApplicationData.Strings.DisplaySettingChangeRequiredMessage;
        FunctionNameLabel.Content = functionName;
        RegisterNameLabel.Content = registerName;
        DisplayLabel.Content = ApplicationData.Strings.Display;
        ApplySameSettingToRemainingCheckBox.Content = ApplicationData.Strings.ApplySameSettingToRemaining;
        ModifyButton.Content = ApplicationData.Strings.Modify;
        CancelButton.Content = ApplicationData.Strings.Cancel;

        ApplySameSettingToRemainingCheckBox.Checked += ApplySameSettingToRemainingCheckBox_Checked;
        ModifyButton.Click += ModifyButton_Click;
        CancelButton.Click += CancelButton_Click;
    }

    /// <summary>
    /// 「以降も同じ設定や操作を適用」CheckBoxの「Checked」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ApplySameSettingToRemainingCheckBox_Checked(
        object sender,
        RoutedEventArgs e
        )
    {
        try
        {
            ApplySameSettingToRemaining = ApplySameSettingToRemainingCheckBox.IsChecked == true;
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「変更」Buttonの「Click」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ModifyButton_Click(
        object sender,
        RoutedEventArgs e
        )
    {
        try
        {
            SelectedDisplay = (string)((ComboBoxItem)DisplayComboBox.SelectedItem).Content;
            ApplySameSettingToRemaining = ApplySameSettingToRemainingCheckBox.IsChecked ?? false;
            IsModified = true;

            Close();
        }
        catch
        {
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
}
