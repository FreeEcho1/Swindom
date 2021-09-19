namespace Swindom.Source
{
    /// <summary>
    /// ウィンドウハンドルのリスト
    /// </summary>
    public class HwndList
    {
        /// <summary>
        /// ウィンドウハンドル
        /// </summary>
        public System.Collections.Generic.List<System.IntPtr> Hwnd = new();

        /// <summary>
        /// ウィンドウハンドルのリストを取得
        /// </summary>
        /// <returns>ウィンドウハンドルのリスト</returns>
        public static HwndList GetWindowHandleList()
        {
            HwndList hwndList = new();
            NativeMethods.EnumWindows(new(WindowProcessing.EnumerateHwnd), ref hwndList);
            return (hwndList);
        }
    }
}
