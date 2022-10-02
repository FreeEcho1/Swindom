namespace Swindom;

/// <summary>
/// マウスをフック
/// </summary>
public class MouseHook
{
    /// <summary>
    /// フックプロシージャ
    /// </summary>
    private NativeMethods.HookProcDelegate? MouseHookProc;
    /// <summary>
    /// フックID
    /// </summary>
    private IntPtr HookId = IntPtr.Zero;

    /// <summary>
    /// マウスイベントのハンドラー
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void MouseEventHandler(
        object sender,
        MouseEventArgs e
        );
    /// <summary>
    /// マウスが移動したイベント
    /// </summary>
    public event MouseEventHandler? MouseMoveEvent;
    /// <summary>
    /// マウスの左ボタンが押されたイベント
    /// </summary>
    public event MouseEventHandler? LButtonDownEvent;
    /// <summary>
    /// マウスの左ボタンが離されたイベント
    /// </summary>
    public event MouseEventHandler? LButtonUpEvent;
    /// <summary>
    /// マウスの右ボタンが押されたイベント
    /// </summary>
    public event MouseEventHandler? RButtonDownEvent;
    /// <summary>
    /// マウスの右ボタンが離されたイベント
    /// </summary>
    public event MouseEventHandler? RButtonUpEvent;

    /// <summary>
    /// フック開始
    /// </summary>
    public void Hook()
    {
        if (HookId == IntPtr.Zero)
        {
            using Process curProcess = Process.GetCurrentProcess();
            using ProcessModule? curModule = curProcess.MainModule;
            if (curModule?.ModuleName == null)
            {
                throw new Exception("MouseHook.Hook() - Error 1.");
            }
            MouseHookProc = HookProcedure;
            HookId = NativeMethods.SetWindowsHookEx(WH.WH_MOUSE_LL, MouseHookProc, NativeMethods.GetModuleHandle(curModule.ModuleName), 0);
            if (HookId == IntPtr.Zero)
            {
                throw new Exception("MouseHook.Hook() - Error 2.");
            }
        }
    }

    /// <summary>
    /// フック終了
    /// </summary>
    public void UnHook()
    {
        if (HookId != IntPtr.Zero)
        {
            NativeMethods.UnhookWindowsHookEx(HookId);
            HookId = IntPtr.Zero;
        }
    }

    /// <summary>
    /// フックプロシージャ
    /// </summary>
    /// <param name="nCode"></param>
    /// <param name="wParam"></param>
    /// <param name="lParam"></param>
    /// <returns></returns>
    public IntPtr HookProcedure(
        int nCode,
        IntPtr wParam,
        IntPtr lParam
        )
    {
        MSLLHOOKSTRUCT? ms = (MSLLHOOKSTRUCT?)Marshal.PtrToStructure(lParam, typeof(MSLLHOOKSTRUCT));
        
        if (ms != null)
        {
            if (nCode >= 0 && (int)wParam == (int)WM.WM_MOUSEMOVE)
            {
                MouseMoveEvent?.Invoke(this, new MouseEventArgs(ms.pt.x, ms.pt.y));
            }
            if (nCode >= 0 && (int)wParam == (int)WM.WM_LBUTTONDOWN)
            {
                LButtonDownEvent?.Invoke(this, new MouseEventArgs(ms.pt.x, ms.pt.y));
            }
            if (nCode >= 0 && (int)wParam == (int)WM.WM_LBUTTONUP)
            {
                LButtonUpEvent?.Invoke(this, new MouseEventArgs(ms.pt.x, ms.pt.y));
            }
            if (nCode >= 0 && (int)wParam == (int)WM.WM_RBUTTONDOWN)
            {
                RButtonDownEvent?.Invoke(this, new MouseEventArgs(ms.pt.x, ms.pt.y));
            }
            if (nCode >= 0 && (int)wParam == (int)WM.WM_RBUTTONUP)
            {
                RButtonUpEvent?.Invoke(this, new MouseEventArgs(ms.pt.x, ms.pt.y));
            }
        }

        return NativeMethods.CallNextHookEx(HookId, nCode, wParam, lParam);
    }
}
