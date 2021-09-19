namespace Swindom.Source
{
    public enum HwndInsertAfter : int
    {
        /// <summary>
        /// ウインドウを最前面ウインドウの後ろに置く
        /// </summary>
        HWND_NOTOPMOST = -2,
        /// <summary>
        /// ウインドウを最前面ウインドウではないすべてのウインドウの前に置く
        /// </summary>
        HWND_TOPMOST = -1,
        /// <summary>
        /// ウインドウをZオーダーの先頭に置く
        /// </summary>
        HWND_TOP = 0,
        /// <summary>
        /// ウインドウをZオーダーの最後に置く
        /// </summary>
        HWND_BOTTOM = 1
    }
}
