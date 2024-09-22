using System.Windows.Media;

namespace Swindom;

/// <summary>
/// SideButton
/// </summary>
public partial class SideButton : UserControl
{
    /// <summary>
    /// 画像
    /// </summary>
    public ImageSource ButtonImage
    {
        get => BaseImage.Source;
        set => BaseImage.Source = value;
    }
    /// <summary>
    /// テキスト
    /// </summary>
    public object Text
    {
        get => BaseLabel.Content;
        set => BaseLabel.Content = value;
    }

    /// <summary>
    /// クリックされたときに発生
    /// </summary>
    public event EventHandler<RoutedEventArgs> Click = delegate { };
    /// <summary>
    /// 「Click」イベントを実行
    /// </summary>
    /// <param name="e"></param>
    public virtual void DoClick(RoutedEventArgs e) => Click?.Invoke(this, e);

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public SideButton()
    {
        InitializeComponent();
    }

    /// <summary>
    /// ボタンの「Click」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void BaseButton_Click(
        object sender,
        RoutedEventArgs e
        )
    {
        try
        {
            DoClick(e);
        }
        catch
        {
        }
    }
}
