namespace Swindom;

/// <summary>
/// MessageBoxのウィンドウ
/// </summary>
public partial class FEMessageBoxWindow : Window
{
    /// <summary>
    /// 「OK」ボタンの文字列
    /// </summary>
    public static string Ok
    {
        get;
        set;
    } = "OK";
    /// <summary>
    /// 「Yes」ボタンの文字列
    /// </summary>
    public static string Yes
    {
        get;
        set;
    } = "Yes";
    /// <summary>
    /// 「No」ボタンの文字列
    /// </summary>
    public static string No
    {
        get;
        set;
    } = "No";
    /// <summary>
    /// 「Cancel」ボタンの文字列
    /// </summary>
    public static string Cancel
    {
        get;
        set;
    } = "Cancel";
    /// <summary>
    /// 押されたボタン
    /// </summary>
    public MessageBoxResult Result
    {
        get;
        set;
    } = MessageBoxResult.Cancel;
    /// <summary>
    /// ディスプレイの幅からウィンドウを小さくする値
    /// </summary>
    private static int MakeWindowSmall { get; } = 80;

    /// <summary>
    /// コンストラクタ (使用しない)
    /// </summary>
    public FEMessageBoxWindow()
    {
        throw new Exception("Do not use. - " + GetType().Name + "()");
    }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="window">Window</param>
    /// <param name="message">メッセージ</param>
    /// <param name="title">タイトル</param>
    /// <param name="button">表示するボタン</param>
    public FEMessageBoxWindow(
        Window? window,
        string message,
        string title,
        MessageBoxButton button
        )
    {
        InitializeComponent();

        if (window == null)
        {
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }
        else
        {
            Owner = window;
        }
        Title = title;
        MessageTextBlock.Text = message;
        switch (button)
        {
            case MessageBoxButton.OK:
                OkButton.Focus();
                YesButton.Visibility = Visibility.Collapsed;
                NoButton.Visibility = Visibility.Collapsed;
                CancelButton.Visibility = Visibility.Collapsed;
                break;
            case MessageBoxButton.OKCancel:
                OkButton.Focus();
                YesButton.Visibility = Visibility.Collapsed;
                NoButton.Visibility = Visibility.Collapsed;
                break;
            case MessageBoxButton.YesNo:
                OkButton.Visibility = Visibility.Collapsed;
                YesButton.Focus();
                CancelButton.Visibility = Visibility.Collapsed;
                break;
            case MessageBoxButton.YesNoCancel:
                OkButton.Visibility = Visibility.Collapsed;
                YesButton.Focus();
                break;
        }
        OkButton.Content = Ok;
        YesButton.Content = Yes;
        NoButton.Content = No;
        CancelButton.Content = Cancel;

        Loaded += MessageBoxWindow_Loaded;
        OkButton.Click += OkButton_Click;
        YesButton.Click += YesButton_Click;
        NoButton.Click += NoButton_Click;
        CancelButton.Click += CancelButton_Click;
    }

    /// <summary>
    /// 「ContentRendered」イベント
    /// </summary>
    /// <param name="e"></param>
    protected override void OnContentRendered(
        EventArgs e
        )
    {
        base.OnContentRendered(e);
        // WPFの「SizeToContent」「WidthAndHeight」のバグ対策
        InvalidateMeasure();
    }

    /// <summary>
    /// ウィンドウの「Loaded」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void MessageBoxWindow_Loaded(
        object sender,
        RoutedEventArgs e
        )
    {
        try
        {
            // タイトルが途中までしか表示されていない場合はウィンドウの幅を大きくする
            const int spaceWidth = 80;      // 余白の幅
            System.Windows.Media.FormattedText formattedText = new(Title, System.Globalization.CultureInfo.CurrentUICulture, FlowDirection.LeftToRight, new System.Windows.Media.Typeface(FontFamily, FontStyle, FontWeight, FontStretch), FontSize, System.Windows.Media.Brushes.Black, System.Windows.Media.VisualTreeHelper.GetDpi(this).PixelsPerDip);
            if (Width < formattedText.Width + spaceWidth)
            {
                MinWidth = formattedText.Width + spaceWidth;
            }
            // ウィンドウの幅が大きい場合は小さくする
            if (SystemParameters.WorkArea.Width - MakeWindowSmall < Width)
            {
                MinWidth = MaxWidth = SystemParameters.WorkArea.Width - MakeWindowSmall;
            }

            Activate();
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「OK」Buttonの「Click」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OkButton_Click(
        object sender,
        RoutedEventArgs e
        )
    {
        try
        {
            Result = MessageBoxResult.OK;
            Close();
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「Yes」Buttonの「Click」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void YesButton_Click(
        object sender,
        RoutedEventArgs e
        )
    {
        try
        {
            Result = MessageBoxResult.Yes;
            Close();
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「No」Buttonの「Click」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void NoButton_Click(
        object sender,
        RoutedEventArgs e
        )
    {
        try
        {
            Result = MessageBoxResult.No;
            Close();
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「Cancel」Buttonの「Click」イベント
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
            Result = MessageBoxResult.Cancel;
            Close();
        }
        catch
        {
        }
    }
}
