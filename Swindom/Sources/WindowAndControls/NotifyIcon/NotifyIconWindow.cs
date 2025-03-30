namespace Swindom;

/// <summary>
/// システムトレイアイコンのウィンドウ
/// </summary>
public class NotifyIconWindow : System.Windows.Forms.Form
{
    /// <summary>
    /// Disposeが呼ばれたかの値
    /// </summary>
    private new bool Disposed;
    /// <summary>
    /// NOTIFYICONDATA
    /// </summary>
    private NOTIFYICONDATA NotifyIconData;
    /// <summary>
    /// システムトレイのアイコン
    /// </summary>
    public Icon? SystemTrayIcon
    {
        get
        {
            return Icon;
        }
        set
        {
            if (value != null)
            {
                NOTIFYICONDATA data = new()
                {
                    cbSize = NotifyIconData.cbSize,
                    uFlags = NotifyIconData.uFlags,
                    hwnd = NotifyIconData.hwnd,
                    hIcon = value.ToBitmap().GetHicon(),
                    uID = NotifyIconData.uID,
                    uCallbackMessage = NotifyIconData.uCallbackMessage,
                    szTip = NotifyIconData.szTip
                };
                if (ModifyNotifyIcon(data))
                {
                    Icon = value;
                }
            }
        }
    }
    /// <summary>
    /// 「システムトレイアイコン」のContextMenu
    /// </summary>
    public ContextMenu? NotifyIconContextMenu { get; set; }

    /// <summary>
    /// システムトレイアイコンの作成でリトライする回数
    /// </summary>
    public uint RetryToCreateNotifyIcon { get; set; } = 10;

    /// <summary>
    /// システムトレイアイコンで「左ボタンがクリックされた」イベント
    /// </summary>
    public event EventHandler<EventArgs> NotifyIconLeftButtonClick = delegate { };
    /// <summary>
    /// システムトレイアイコンで「左ボタンがクリックされた」イベントを実行
    /// </summary>
    public virtual void DoNotifyIconLeftButtonClick()
    {
        NotifyIconLeftButtonClick?.Invoke(this, new EventArgs());
    }
    /// <summary>
    /// システムトレイアイコンで「左ボタンがダブルクリックされた」イベント
    /// </summary>
    public event EventHandler<EventArgs> NotifyIconLeftButtonDoubleClick = delegate { };
    /// <summary>
    /// システムトレイアイコンで「左ボタンがダブルクリックされた」イベントを実行
    /// </summary>
    public virtual void DoNotifyIconLeftButtonDoubleClick()
    {
        NotifyIconLeftButtonDoubleClick?.Invoke(this, new EventArgs());
    }
    /// <summary>
    /// システムトレイアイコンで「右ボタンがクリックされた」イベント
    /// </summary>
    public event EventHandler<EventArgs> NotifyIconRightButtonClick = delegate { };
    /// <summary>
    /// システムトレイアイコンで「右ボタンがクリックされた」イベントを実行
    /// </summary>
    public virtual void DoNotifyIconRightButtonClick()
    {
        NotifyIconRightButtonClick?.Invoke(this, new EventArgs());
    }
    /// <summary>
    /// クリックのタイマー
    /// </summary>
    private System.Windows.Threading.DispatcherTimer? ClickTimer;

    /// <summary>
    /// 「WndProcのメッセージ」イベント
    /// </summary>
    public event EventHandler<WndProcEventArgs> WndProcMessage = delegate { };
    /// <summary>
    /// 「WndProcのメッセージ」イベントを実行
    /// </summary>
    /// <param name="message">メッセージ</param>
    public virtual void DoWndProcMessage(
        System.Windows.Forms.Message message
        )
    {
        WndProcMessage?.Invoke(this, new WndProcEventArgs(message));
    }

    /// <summary>
    /// 見えないフォーム (メニューを閉じる判定用)
    /// </summary>
    private System.Windows.Forms.Form? InvisibleForm;

    /// <summary>
    /// システムトレイアイコンのアプリケーション定義識別子
    /// </summary>
    private const int NotifyIconId = 1;
    /// <summary>
    /// システムトレイアイコンのメッセージ識別子
    /// </summary>
    private const int MY_WM_NOTIFYICON = 0x8000;

    /// <summary>
    /// Dispose
    /// </summary>
    public new void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// 非公開Dispose
    /// </summary>
    /// <param name="disposing"></param>
    protected override void Dispose(
        bool disposing
        )
    {
        if (Disposed)
        {
            return;
        }
        DeleteNotifyIcon();
        base.Dispose(disposing);
        Disposed = true;
    }

    /// <summary>
    /// ウィンドウプロシージャ
    /// </summary>
    /// <param name="m"></param>
    protected override void WndProc(
        ref System.Windows.Forms.Message m
        )
    {
        try
        {
            if (m.Msg == MY_WM_NOTIFYICON && m.WParam == NotifyIconId)
            {
                switch (m.LParam)
                {
                    case (IntPtr)WM.WM_LBUTTONDOWN:
                        LeftButtonClick();
                        break;
                    case (IntPtr)WM.WM_LBUTTONDBLCLK:
                        LeftButtonDoubleClick();
                        break;
                    case (IntPtr)WM.WM_RBUTTONDOWN:
                        RightButtonClick();
                        break;
                    default:
                        break;
                }
            }

            DoWndProcMessage(m);
        }
        catch
        {
        }

        base.WndProc(ref m);
    }

    /// <summary>
    /// 「クリックのタイマー」の「Tick」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ClickTimer_Tick(
        object? sender,
        EventArgs e
        )
    {
        try
        {
            if (ClickTimer != null)
            {
                ClickTimer.Stop();
                ClickTimer = null;
                DoNotifyIconLeftButtonClick();
            }
        }
        catch
        {
        }
    }

    /// <summary>
    /// 左ボタンのクリック処理
    /// </summary>
    private void LeftButtonClick()
    {
        if (ClickTimer == null)
        {
            ClickTimer = new()
            {
                Interval = new(0, 0, 0, 0, (int)NativeMethods.GetDoubleClickTime())
            };
            ClickTimer.Tick += ClickTimer_Tick;
            ClickTimer.Start();
        }
    }

    /// <summary>
    /// 左ボタンのダブルクリック処理
    /// </summary>
    private void LeftButtonDoubleClick()
    {
        if (ClickTimer != null)
        {
            ClickTimer.Stop();
            ClickTimer = null;
        }

        DoNotifyIconLeftButtonDoubleClick();
    }

    /// <summary>
    /// 右ボタンのクリック処理
    /// </summary>
    /// <param name="doClickEvent">「NotifyIconRightButtonClick」イベントを実行するかの値 (いいえ「false」/はい「true」)</param>
    private void RightButtonClick(
        bool doClickEvent = true
        )
    {
        if (NotifyIconContextMenu != null)
        {
            if (InvisibleForm == null)
            {
                InvisibleForm = new()
                {
                    Opacity = 0,
                    ShowInTaskbar = false,
                    Left = -100,
                    Top = -100,
                    Width = 0,
                    Height = 0
                };
                InvisibleForm.LostFocus += InvisibleForm_LostFocus;
                InvisibleForm.Show();
                InvisibleForm.Activate();
            }

            NotifyIconContextMenu.IsOpen = true;
        }

        if (doClickEvent)
        {
            DoNotifyIconRightButtonClick();
        }
    }

    /// <summary>
    /// 「見えないフォーム」の「LostFocus」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void InvisibleForm_LostFocus(
        object? sender,
        EventArgs e
        )
    {
        try
        {
            if (NotifyIconContextMenu != null)
            {
                NotifyIconContextMenu.IsOpen = false;
            }
            if (InvisibleForm != null)
            {
                InvisibleForm.Close();
                InvisibleForm = null;
            }
        }
        catch
        {
        }
    }

    /// <summary>
    /// システムトレイアイコンを追加
    /// </summary>
    /// <param name="icon">アイコン</param>
    /// <param name="tip">ツールチップの文字列</param>
    /// <returns>追加に成功したかの値 (失敗「false」/成功「true」)</returns>
    public bool Add(
        Icon icon,
        string tip = "",
        ContextMenu? notifyIconContextMenu = null
        )
    {
        bool result = false;        // 結果

        if (NotifyIconData.hwnd == IntPtr.Zero)
        {
            NOTIFYICONDATA data = new()
            {
                cbSize = Marshal.SizeOf(typeof(NOTIFYICONDATA)),
                uFlags = (int)NIF.NIF_ICON | (int)NIF.NIF_MESSAGE | (int)NIF.NIF_TIP,
                hwnd = Handle,
                hIcon = icon.ToBitmap().GetHicon(),
                uID = NotifyIconId,
                uCallbackMessage = MY_WM_NOTIFYICON,
                szTip = tip
            };

            // システムトレイアイコンを登録 (失敗した場合はリトライする)
            result = NativeMethods.Shell_NotifyIcon((uint)NIM.NIM_ADD, ref data);
            if (result == false)
            {
                int countRetry = 0;       // リトライした回数
                do
                {
                    if (RetryToCreateNotifyIcon <= countRetry)
                    {
                        break;
                    }
                    countRetry++;
                } while ((result = NativeMethods.Shell_NotifyIcon((uint)NIM.NIM_MODIFY, ref data)) == false);
            }
            if (result)
            {
                NotifyIconData = data;
                if (notifyIconContextMenu != null)
                {
                    NotifyIconContextMenu = notifyIconContextMenu;
                }
            }
        }

        return result;
    }

    /// <summary>
    /// システムトレイアイコンを変更
    /// </summary>
    /// <param name="data">NOTIFYICONDATA</param>
    /// <returns>変更に成功したかの値 (失敗「false」/成功「true」)</returns>
    private bool ModifyNotifyIcon(
        NOTIFYICONDATA data
        )
    {
        bool result = false;        // 結果

        if (NotifyIconData.hwnd != IntPtr.Zero)
        {
            int countRetry = 0;       // リトライした回数
            while (result = NativeMethods.Shell_NotifyIcon((uint)NIM.NIM_MODIFY, ref data))
            {
                if (RetryToCreateNotifyIcon <= countRetry)
                {
                    break;
                }
                countRetry++;
            }
            if (result)
            {
                NotifyIconData = data;
            }
        }

        return result;
    }

    /// <summary>
    /// システムトレイアイコンを削除
    /// </summary>
    public void DeleteNotifyIcon()
    {
        if (NotifyIconData.hwnd != IntPtr.Zero)
        {
            NativeMethods.Shell_NotifyIcon((uint)NIM.NIM_DELETE, ref NotifyIconData);
            NotifyIconData = new();
        }
    }

    /// <summary>
    /// 「ContextMenu」を表示
    /// </summary>
    public void ShowContextMenu()
    {
        RightButtonClick(false);
    }
}
