namespace Swindom;

[Flags]
public enum SWP : int
{
    /// <summary>
    /// サイズを変更しない
    /// </summary>
    SWP_NOSIZE = 0x0001,
    /// <summary>
    /// 位置を変更しない
    /// </summary>
    SWP_NOMOVE = 0x0002,
    /// <summary>
    /// Z順位を変更しない
    /// </summary>
    SWP_NOZORDER = 0x0004,
    /// <summary>
    /// 	ウインドウをアクティブ化しない
    /// </summary>
    SWP_NOACTIVATE = 0x0010,
    /// <summary>
    /// オーナーウィンドウのZオーダは変更しない
    /// </summary>
    SWP_NOOWNERZORDER = 0x0200,
}
