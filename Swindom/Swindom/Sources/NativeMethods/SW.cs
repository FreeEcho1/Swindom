namespace Swindom;

public enum SW : int
{
    /// <summary>
    /// ウィンドウを非表示にして、他のウィンドウをアクティブにする
    /// </summary>
    SW_HIDE = 0,
    /// <summary>
    /// SW_RESTOREと同じ
    /// </summary>
    SW_SHOWNORMAL = 1,
    /// <summary>
    /// ウィンドウをアクティブにして、最小化されたウィンドウとして表示する
    /// </summary>
    SW_SHOWMINIMIZED = 2,
    /// <summary>
    /// ウィンドウをアクティブにして、最大化されたウィンドウとして表示する
    /// </summary>
    SW_SHOWMAXIMIZED = 3,
    /// <summary>
    /// ウィンドウをアクティブにしない
    /// </summary>
    SWP_NOACTIVATE = 4,
    /// <summary>
    /// ウィンドウをアクティブにする
    /// </summary>
    SW_SHOW = 5,
    /// <summary>
    /// ウィンドウを最小化して、次の Z オーダーにあるトップレベルウィンドウをアクティブにする
    /// </summary>
    SW_MINIMIZE = 6,
    /// <summary>
    /// ウィンドウを最小化されたウィンドウとして表示する。アクティブにしない
    /// </summary>
    SW_SHOWMINNOACTIVE = 7,
    /// <summary>
    /// ウィンドウを現在の位置とサイズで表示する。アクティブにしない
    /// </summary>
    SW_SHOWNA = 8,
    /// <summary>
    /// ウィンドウをアクティブにして表示する。ウィンドウが最小化または最大化されている場合は、ウィンドウの位置とサイズを元に戻す
    /// </summary>
    SW_RESTORE = 9
}
