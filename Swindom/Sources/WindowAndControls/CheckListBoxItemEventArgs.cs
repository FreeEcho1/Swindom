namespace Swindom;

/// <summary>
/// CheckListBoxItemのイベントデータ
/// </summary>
public class CheckListBoxItemEventArgs
{
    /// <summary>
    /// 項目
    /// </summary>
    public CheckListBoxItem? Item { get; set; }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public CheckListBoxItemEventArgs()
    {
    }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="itemData">CheckListBoxItem</param>
    public CheckListBoxItemEventArgs(
        CheckListBoxItem itemData
        )
    {
        Item = itemData;
    }
}
