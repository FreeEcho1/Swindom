namespace Swindom;

/// <summary>
/// CheckListBoxItem
/// </summary>
public partial class CheckListBoxItem : UserControl
{
    /// <summary>
    /// チェック状態変更のイベントハンドラ
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void EventHandlerCheckStateChanged(
        object sender,
        CheckListBoxItemEventArgs e
        );
    /// <summary>
    /// チェック状態変更イベント
    /// </summary>
    public event EventHandlerCheckStateChanged CheckStateChanged = delegate { };
    /// <summary>
    /// チェック状態が変更されたイベントを実行
    /// </summary>
    public virtual void DoCheckStateChanged()
    {
        CheckStateChanged?.Invoke(this, new(this));
    }
    /// <summary>
    /// テキスト
    /// </summary>
    [System.ComponentModel.DefaultValue("")]
    [System.ComponentModel.Category("CheckListBoxItem Value")]
    [System.ComponentModel.Description("テキスト")]
    public object Text
    {
        set
        {
            TextLabel.Content = value;
        }
        get
        {
            return (TextLabel.Content);
        }
    }
    /// <summary>
    /// チェック状態
    /// </summary>
    [System.ComponentModel.DefaultValue(false)]
    [System.ComponentModel.Category("CheckListBoxItem Value")]
    [System.ComponentModel.Description("チェック状態")]
    public bool IsChecked
    {
        set
        {
            CheckBox.IsChecked = value;
        }
        get
        {
            return (CheckBox.IsChecked == true);
        }
    }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public CheckListBoxItem()
    {
        InitializeComponent();

        CheckBox.Click += CheckBox_Click;
    }

    /// <summary>
    /// 「チェックボックス」の「Click」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void CheckBox_Click(
        object sender,
        RoutedEventArgs e
        )
    {
        try
        {
            DoCheckStateChanged();
        }
        catch
        {
        }
    }
}
