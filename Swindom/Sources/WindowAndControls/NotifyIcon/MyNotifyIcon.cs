namespace Swindom;

/// <summary>
/// システムトレイアイコン
/// </summary>
public class MyNotifyIcon : IDisposable
{
    /// <summary>
    /// Disposeが呼ばれたかの値
    /// </summary>
    private bool Disposed;
    /// <summary>
    /// システムトレイアイコンのウィンドウ
    /// </summary>
    private NotifyIconWindow NotifyIconWindow { get; } = new();
    /// <summary>
    /// アイコン
    /// </summary>
    public Icon? Icon
    {
        get
        {
            return NotifyIconWindow.SystemTrayIcon;
        }
        set
        {
            NotifyIconWindow.SystemTrayIcon = value;
        }
    }
    /// <summary>
    /// 「システムトレイアイコン」のContextMenu
    /// </summary>
    public ContextMenu? NotifyIconContextMenu
    {
        get
        {
            return NotifyIconWindow.NotifyIconContextMenu;
        }
        set
        {
            NotifyIconWindow.NotifyIconContextMenu = value;
        }
    }
    /// <summary>
    /// ウィンドウハンドル
    /// </summary>
    public IntPtr Handle
    {
        get
        {
            return NotifyIconWindow.Handle;
        }
    }

    /// <summary>
    /// システムトレイアイコンの作成でリトライする回数
    /// </summary>
    public uint RetryToCreateNotifyIcon
    {
        get
        {
            return NotifyIconWindow.RetryToCreateNotifyIcon;
        }
        set
        {
            NotifyIconWindow.RetryToCreateNotifyIcon = value;
        }
    }

    /// <summary>
    /// 「左ボタンがクリックされた」イベント
    /// </summary>
    public event EventHandler<EventArgs> NotifyIconLeftButtonClick = delegate { };
    /// <summary>
    /// 「左ボタンがクリックされた」イベントを実行
    /// </summary>
    public virtual void DoNotifyIconLeftButtonClick()
    {
        NotifyIconLeftButtonClick?.Invoke(this, new EventArgs());
    }
    /// <summary>
    /// 「左ボタンがダブルクリックされた」イベント
    /// </summary>
    public event EventHandler<EventArgs> NotifyIconLeftButtonDoubleClick = delegate { };
    /// <summary>
    /// 「左ボタンがダブルクリックされた」イベントを実行
    /// </summary>
    public virtual void DoNotifyIconLeftButtonDoubleClick()
    {
        NotifyIconLeftButtonDoubleClick?.Invoke(this, new EventArgs());
    }
    /// <summary>
    /// 「右ボタンがクリックされた」イベント
    /// </summary>
    public event EventHandler<EventArgs> NotifyIconRightButtonClick = delegate { };
    /// <summary>
    /// 「右ボタンがクリックされた」イベントを実行
    /// </summary>
    public virtual void DoNotifyIconRightButtonClick()
    {
        NotifyIconRightButtonClick?.Invoke(this, new EventArgs());
    }

    /// <summary>
    /// 「WndProcのメッセージ」イベント
    /// </summary>
    public event EventHandler<WndProcEventArgs> WndProcMessage = delegate { };
    /// <summary>
    /// 「WndProcのメッセージ」イベントを実行
    /// </summary>
    /// <param name="message">メッセージ</param>
    public virtual void DoWndProcMessage(
        WndProcEventArgs e
        )
    {
        WndProcMessage?.Invoke(this, e);
    }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public MyNotifyIcon()
    {
        NotifyIconWindow.NotifyIconLeftButtonClick += NotifyIconWindow_NotifyIconLeftButtonClick;
        NotifyIconWindow.NotifyIconRightButtonClick += NotifyIconWindow_NotifyIconRightButtonClick;
        NotifyIconWindow.NotifyIconLeftButtonDoubleClick += NotifyIconWindow_NotifyIconLeftButtonDoubleClick;
        NotifyIconWindow.WndProcMessage += NotifyIconWindow_WndProcMessage;
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
    protected void Dispose(
        bool disposing
        )
    {
        if (Disposed)
        {
            return;
        }
        NotifyIconWindow.Dispose();
        Disposed = true;
    }

    /// <summary>
    /// 「左ボタンがクリックされた」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void NotifyIconWindow_NotifyIconLeftButtonClick(
        object? sender,
        EventArgs e
        )
    {
        try
        {
            DoNotifyIconLeftButtonClick();
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「右ボタンがクリックされた」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void NotifyIconWindow_NotifyIconRightButtonClick(
        object? sender,
        EventArgs e
        )
    {
        try
        {
            DoNotifyIconRightButtonClick();
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「左ボタンがダブルクリックされた」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void NotifyIconWindow_NotifyIconLeftButtonDoubleClick(
        object? sender,
        EventArgs e
        )
    {
        try
        {
            DoNotifyIconLeftButtonDoubleClick();
        }
        catch
        {
        }
    }

    /// <summary>
    /// 「WndProc」イベント
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void NotifyIconWindow_WndProcMessage(
        object? sender,
        WndProcEventArgs e
        )
    {
        try
        {
            DoWndProcMessage(e);
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
        return NotifyIconWindow.Add(icon, tip, notifyIconContextMenu);
    }

    /// <summary>
    /// システムトレイアイコンを削除
    /// </summary>
    public void DeleteNotifyIcon()
    {
        NotifyIconWindow.DeleteNotifyIcon();
    }

    /// <summary>
    /// 「ContextMenu」を表示
    /// </summary>
    public void ShowContextMenu()
    {
        NotifyIconWindow.ShowContextMenu();
    }
}
