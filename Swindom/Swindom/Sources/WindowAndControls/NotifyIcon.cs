namespace Swindom;

/// <summary>
/// Notify icon
/// </summary>
public class NotifyIcon : IDisposable
{
    /// <summary>
    /// Disposeが呼ばれたかの値
    /// </summary>
    private bool Disposed;
    /// <summary>
    /// HwndSource
    /// </summary>
    private HwndSource? HwndSource;
    /// <summary>
    /// ID
    /// </summary>
    private int Id = 1;
    /// <summary>
    /// Hwnd
    /// </summary>
    private IntPtr Hwnd = IntPtr.Zero;
    /// <summary>
    /// Tip
    /// </summary>
    public string Tip = "";

    /// <summary>
    /// 「左ボタンがダブルクリックされた」イベント
    /// </summary>
    public event EventHandler<NotifyIconMouseEventArgs>? LeftButtonDoubleClickEvent;
    /// <summary>
    /// 「左ボタンがダブルクリックされた」イベントを実行
    /// </summary>
    /// <param name="message">メッセージ</param>
    public virtual void DoLeftButtonDoubleClick()
    {
        LeftButtonDoubleClickEvent?.Invoke(this, new NotifyIconMouseEventArgs());
    }

    /// <summary>
    /// 「右ボタンがクリックされた」イベント
    /// </summary>
    public event EventHandler<NotifyIconMouseEventArgs>? RightButtonClickEvent;
    /// <summary>
    /// 「右ボタンがクリックされた」イベントを実行
    /// </summary>
    /// <param name="message">メッセージ</param>
    public virtual void DoRightButtonClick()
    {
        RightButtonClickEvent?.Invoke(this, new NotifyIconMouseEventArgs());
    }

    /// <summary>
    /// システムトレイアイコンのメッセージの値
    /// </summary>
    private const int WM_NOTIFYICON = 0x8000 + 1;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public NotifyIcon()
    {
    }

    /// <summary>
    /// デストラクタ
    /// </summary>
    ~NotifyIcon()
    {
        Dispose(false);
    }

    /// <summary>
    /// Dispose
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// 非公開Dispose
    /// </summary>
    /// <param name="disposing"></param>
    protected virtual void Dispose(
        bool disposing
        )
    {
        if (Disposed == false)
        {
            Disposed = true;
            Delete();
        }
    }

    /// <summary>
    /// ウィンドウプロシージャ
    /// </summary>
    /// <param name="hwnd"></param>
    /// <param name="msg"></param>
    /// <param name="wparam"></param>
    /// <param name="lparam"></param>
    /// <param name="handled"></param>
    /// <returns></returns>
    private IntPtr WndProc(
        IntPtr hwnd,
        int msg,
        IntPtr wparam,
        IntPtr lparam,
        ref bool handled
        )
    {
        if (msg == WM_NOTIFYICON && (int)wparam == Id)
        {
            switch ((int)lparam)
            {
                case (int)WM.WM_LBUTTONDBLCLK:
                    DoLeftButtonDoubleClick();
                    break;
                case (int)WM.WM_RBUTTONUP:
                    DoRightButtonClick();
                    break;
            }
        }

        return IntPtr.Zero;
    }

    /// <summary>
    /// システムトレイアイコンを追加
    /// </summary>
    /// <param name="window">Window</param>
    /// <param name="id">ID</param>
    /// <param name="hwnd">ウィンドウハンドル</param>
    /// <param name="icon">アイコン</param>
    /// <param name="tip">ツールチップの文字列</param>
    /// <param name="maxRetry">登録に失敗した場合の最大繰り返し回数</param>
    /// <returns>追加に成功したかの値 (失敗「false」/成功「true」)</returns>
    public bool Add(
        Window window,
        int id,
        IntPtr hwnd,
        Icon icon,
        string tip = "",
        int maxRetry = 3
        )
    {
        bool result = false;        // 結果

        if (Hwnd == IntPtr.Zero)
        {
            NOTIFYICONDATA data = new()
            {
                cbSize = Marshal.SizeOf(typeof(NOTIFYICONDATA)),
                uFlags = (int)NIF.NIF_ICON | (int)NIF.NIF_MESSAGE | (int)NIF.NIF_TIP,
                hwnd = hwnd,
                hIcon = icon.ToBitmap().GetHicon(),
                uID = id,
                uCallbackMessage = WM_NOTIFYICON,
                szTip = tip
            };

            // システムトレイアイコンを登録
            result = NativeMethods.Shell_NotifyIcon((uint)NIM.NIM_ADD, ref data);
            // 失敗した場合はリトライする
            if (result == false)
            {
                int countRetry = 0;       // リトライした回数
                do
                {
                    if (maxRetry <= countRetry)
                    {
                        break;
                    }
                    countRetry++;
                } while ((result = NativeMethods.Shell_NotifyIcon((uint)NIM.NIM_MODIFY, ref data)) == false);
            }
            if (result)
            {
                Id = id;
                Hwnd = hwnd;
                Tip = tip;

                HwndSource = HwndSource.FromHwnd(new WindowInteropHelper(window).Handle);
                HwndSource.AddHook(new(WndProc));
            }
        }

        return result;
    }

    /// <summary>
    /// システムトレイのアイコンを変更
    /// </summary>
    /// <param name="icon">アイコン</param>
    /// <param name="maxRetry">登録に失敗した場合の最大繰り返し回数</param>
    /// <returns>変更に成功したかの値 (失敗「false」/成功「true」)</returns>
    public bool ModifyIcon(
        Icon icon,
        int maxRetry = 3
        )
    {
        bool result = false;        // 結果

        if (Hwnd != IntPtr.Zero)
        {
            NOTIFYICONDATA data = new()
            {
                cbSize = Marshal.SizeOf(typeof(NOTIFYICONDATA)),
                uFlags = (int)NIF.NIF_ICON | (int)NIF.NIF_MESSAGE | (int)NIF.NIF_TIP,
                hwnd = Hwnd,
                hIcon = icon.ToBitmap().GetHicon(),
                uID = Id,
                uCallbackMessage = WM_NOTIFYICON,
                szTip = Tip
            };

            int countRetry = 0;       // リトライした回数
            do
            {
                result = NativeMethods.Shell_NotifyIcon((uint)NIM.NIM_MODIFY, ref data);
                countRetry++;
                if (maxRetry <= countRetry)
                {
                    break;
                }
            } while (result == false);
        }

        return result;
    }

    /// <summary>
    /// 削除
    /// </summary>
    public void Delete()
    {
        HwndSource?.RemoveHook(new(WndProc));
        HwndSource?.Dispose();
        HwndSource = null;

        if (Hwnd != IntPtr.Zero)
        {
            NOTIFYICONDATA data = new()
            {
                cbSize = Marshal.SizeOf(typeof(NOTIFYICONDATA)),
                hwnd = Hwnd,
                uID = Id
            };
            NativeMethods.Shell_NotifyIcon((uint)NIM.NIM_DELETE, ref data);
            Hwnd = IntPtr.Zero;
            Id = 0;
            Tip = "";
        }
    }
}
