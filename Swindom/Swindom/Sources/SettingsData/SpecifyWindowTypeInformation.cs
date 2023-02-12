namespace Swindom.Sources.SettingsData;

/// <summary>
/// 「指定ウィンドウ」機能の種類の情報
/// </summary>
public class SpecifyWindowTypeInformation
{
    /// <summary>
    /// Foreground
    /// </summary>
    public bool Foreground;
    /// <summary>
    /// MoveSizeEnd
    /// </summary>
    public bool MoveSizeEnd;
    /// <summary>
    /// MinimizeStart
    /// </summary>
    public bool MinimizeStart;
    /// <summary>
    /// MinimizeEnd
    /// </summary>
    public bool MinimizeEnd;
    /// <summary>
    /// Create
    /// </summary>
    public bool Create;
    /// <summary>
    /// Show
    /// </summary>
    public bool Show;
    /// <summary>
    /// NameChange
    /// </summary>
    public bool NameChange;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public SpecifyWindowTypeInformation()
    {
        Foreground = true;
        MoveSizeEnd = true;
        MinimizeStart = true;
        MinimizeEnd = true;
        Create = true;
        Show = true;
        NameChange = true;
    }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="specifyWindowTypeInformation">情報</param>
    public SpecifyWindowTypeInformation(
        SpecifyWindowTypeInformation specifyWindowTypeInformation
        )
    { 
        Foreground = specifyWindowTypeInformation.Foreground;
        MoveSizeEnd = specifyWindowTypeInformation.MoveSizeEnd;
        MinimizeStart = specifyWindowTypeInformation.MinimizeStart;
        MinimizeEnd = specifyWindowTypeInformation.MinimizeEnd;
        Create = specifyWindowTypeInformation.Create;
        Show = specifyWindowTypeInformation.Show;
        NameChange = specifyWindowTypeInformation.NameChange;
    }

    /// <summary>
    /// コピー
    /// </summary>
    /// <param name="specifyWindowTypeInformation">情報</param>
    public void Copy(
        SpecifyWindowTypeInformation specifyWindowTypeInformation
        )
    {
        Foreground = specifyWindowTypeInformation.Foreground;
        MoveSizeEnd = specifyWindowTypeInformation.MoveSizeEnd;
        MinimizeStart = specifyWindowTypeInformation.MinimizeStart;
        MinimizeEnd = specifyWindowTypeInformation.MinimizeEnd;
        Create = specifyWindowTypeInformation.Create;
        Show = specifyWindowTypeInformation.Show;
        NameChange = specifyWindowTypeInformation.NameChange;
    }
}
