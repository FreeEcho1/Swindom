namespace Swindom;

[Flags]
public enum MOD : int
{
    /// <summary>
    /// Alt
    /// </summary>
    MOD_ALT = 0x0001,
    /// <summary>
    /// Ctrl
    /// </summary>
    MOD_CONTROL = 0x0002,
    /// <summary>
    /// Shift
    /// </summary>
    MOD_SHIFT = 0x0004,
    /// <summary>
    /// Windows
    /// </summary>
    MOD_WIN = 0x0008
}
