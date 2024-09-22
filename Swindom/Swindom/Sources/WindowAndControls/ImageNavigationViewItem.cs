using ModernWpf.Controls;

namespace Swindom;

/// <summary>
/// 画像付きのNavigationViewItem
/// </summary>
public class ImageNavigationViewItem : NavigationViewItem
{
    /// <summary>
    /// コントロールを整列するためのコントロール
    /// </summary>
    private StackPanel ItemStackPanel { get; set; } = new()
    {
        Orientation = Orientation.Horizontal
    };
    /// <summary>
    /// 画像のコントロール
    /// </summary>
    private System.Windows.Controls.Image ItemImage { get; set; } = new()
    {
        Width = 30
    };
    /// <summary>
    /// テキストのコントロール
    /// </summary>
    private Label ItemLabel { get; set; } = new()
    {
        VerticalAlignment = VerticalAlignment.Center,
        Margin = new(5, 0, 0, 0)
    };

    /// <summary>
    /// 画像
    /// </summary>
    public System.Windows.Media.ImageSource Image
    {
        get => ItemImage.Source;
        set => ItemImage.Source = value;
    }
    /// <summary>
    /// テキスト
    /// </summary>
    public object Text
    {
        get => ItemLabel.Content;
        set => ItemLabel.Content = value;
    }

    /// <summary>
    /// ナビゲーションビュー項目の幅の余白
    /// </summary>
    private readonly int NatigationViewItemWidthMargin = 50;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public ImageNavigationViewItem()
    {
        ItemStackPanel.Children.Add(ItemImage);
        ItemStackPanel.Children.Add(ItemLabel);
        AddChild(ItemStackPanel);
    }

    /// <summary>
    /// 項目の幅を取得
    /// </summary>
    /// <returns></returns>
    public double ItemWidth()
    {
        return ItemImage.ActualWidth + ItemLabel.ActualWidth + NatigationViewItemWidthMargin;
    }
}
