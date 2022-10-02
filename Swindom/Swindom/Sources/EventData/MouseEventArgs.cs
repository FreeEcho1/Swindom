namespace Swindom;

/// <summary>
/// マウスのイベントデータ
/// </summary>
public class MouseEventArgs : EventArgs
{
    /// <summary>
    /// 座標
    /// </summary>
    public System.Drawing.Point Point
    {
        get;
        private set;
    }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="x">X</param>
    /// <param name="y">Y</param>
    public MouseEventArgs(
        int x,
        int y
        )
    {
        Point = new System.Drawing.Point(x, y);
    }
}
