namespace Swindom;

/// <summary>
/// Rectangle (int)
/// </summary>
public class RectangleInt
{
    /// <summary>
    /// X
    /// </summary>
    public int X { get; set; }
    /// <summary>
    /// Y
    /// </summary>
    public int Y { get; set; }
    /// <summary>
    /// Width
    /// </summary>
    public int Width { get; set; }
    /// <summary>
    /// Height
    /// </summary>
    public int Height { get; set; }

    /// <summary>
    /// Left
    /// </summary>
    [JsonIgnore]
    public int Left { get => X; set => X = value; }
    /// <summary>
    /// Top
    /// </summary>
    [JsonIgnore]
    public int Top { get => Y; set => Y = value; }
    /// <summary>
    /// Right
    /// </summary>
    [JsonIgnore]
    public int Right { get => X + Width; set => Width = value - Left; }
    /// <summary>
    /// Bottom
    /// </summary>
    [JsonIgnore]
    public int Bottom { get => Y + Height; set => Height = value - Top; }

    public RectangleInt()
    {
        X = 0;
        Y = 0;
        Width = 0;
        Height = 0;
    }

    public RectangleInt(
        int x,
        int y,
        int width,
        int height
        )
    {
        X = x;
        Y = y;
        Width = width;
        Height = height;
    }

    public bool IsEmpty => Height == 0 && Width == 0 && X == 0 && Y == 0;

    public void Intersect(RectangleInt rect)
    {
        RectangleInt result = Intersect(rect, this);

        X = result.X;
        Y = result.Y;
        Width = result.Width;
        Height = result.Height;
    }

    public static RectangleInt Intersect(RectangleInt a, RectangleInt b)
    {
        int x1 = Math.Max(a.X, b.X);
        int x2 = Math.Min(a.X + a.Width, b.X + b.Width);
        int y1 = Math.Max(a.Y, b.Y);
        int y2 = Math.Min(a.Y + a.Height, b.Y + b.Height);

        if (x2 >= x1 && y2 >= y1)
        {
            return new(x1, y1, x2 - x1, y2 - y1);
        }

        return new();
    }
}
