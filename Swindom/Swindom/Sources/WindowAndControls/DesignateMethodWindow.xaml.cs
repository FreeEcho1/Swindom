namespace Swindom;

/// <summary>
/// 「ウィンドウ判定の指定方法」ウィンドウ
/// </summary>
public partial class DesignateMethodWindow : Window
{
    /// <summary>
    /// コンストラクタ
    /// </summary>
    public DesignateMethodWindow()
    {
        throw new Exception("Do not use. - DesignateMethodWindow()");
    }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="ownerWindow">オーナーウィンドウ (なし「null」)</param>
    public DesignateMethodWindow(
        Window? ownerWindow
        )
    {
        InitializeComponent();

        if (ApplicationData.Languages.LanguagesWindow == null)
        {
            throw new Exception("Languages value is null. - DesignateMethodWindow()");
        }

        if (ownerWindow != null)
        {
            Owner = ownerWindow;
            WindowStartupLocation = WindowStartupLocation.CenterOwner;
        }

        Title = ApplicationData.Languages.LanguagesWindow.WindowDesignateMethod;
        DesignateMethodExplainTextBox.Text = ApplicationData.Languages.LanguagesWindow.WindowDesignateMethodExplain;
    }
}
