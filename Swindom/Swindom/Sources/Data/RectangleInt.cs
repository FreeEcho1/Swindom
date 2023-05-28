namespace Swindom;

/// <summary>
/// int型のRectangle
/// </summary>
public class RectangleInt
{
    /// <summary>
    /// Left
    /// </summary>
    public int Left;
    /// <summary>
    /// Top
    /// </summary>
    public int Top;
    /// <summary>
    /// Right
    /// </summary>
    public int Right;
    /// <summary>
    /// Bottom
    /// </summary>
    public int Bottom;
    /// <summary>
    /// Width
    /// </summary>
    public int Width
    {
        get { return Right - Left; }
    }
    /// <summary>
    /// Height
    /// </summary>
    public int Height
    {
        get { return Bottom - Top; }
    }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public RectangleInt()
    {
    }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="left">Left</param>
    /// <param name="top">Top</param>
    /// <param name="right">Right</param>
    /// <param name="bottom">Bottom</param>
    public RectangleInt(
        int left,
        int top,
        int right,
        int bottom
        )
    {
        Left = left;
        Top = top;
        Right = right;
        Bottom = bottom;
    }
}
