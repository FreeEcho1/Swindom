namespace Swindom;

/// <summary>
/// Size (decimal)
/// </summary>
public struct SizeDecimal
{
    /// <summary>
    /// Width
    /// </summary>
    public decimal Width;
    /// <summary>
    /// Height
    /// </summary>
    public decimal Height;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="sizeDecimal">SizeDecimal</param>
    public SizeDecimal(
        SizeDecimal sizeDecimal
        )
    {
        Width = sizeDecimal.Width;
        Height = sizeDecimal.Height;
    }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="width">Width</param>
    /// <param name="height">Height</param>
    public SizeDecimal(
        decimal width,
        decimal height
        )
    {
        Width = width;
        Height = height;
    }
}
