namespace Swindom;

/// <summary>
/// サイズ
/// </summary>
public class SizeInt
{
    /// <summary>
    /// Width
    /// </summary>
    public int Width { get; set; }
    /// <summary>
    /// Height
    /// </summary>
    public int Height { get; set; }

    public SizeInt()
    {
        Width = 0;
        Height = 0;
    }

    public SizeInt(
        int width,
        int height
        )
    {
        Width = width;
        Height = height;
    }
}
