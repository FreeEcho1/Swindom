namespace Swindom.Source.Window
{
    /// <summary>
    /// Notify icon
    /// </summary>
    public class NotifyIcon : System.IDisposable
    {
        /// <summary>
        /// Disposeが呼ばれたかの値
        /// </summary>
        private bool Disposed;
        /// <summary>
        /// HwndSource
        /// </summary>
        private System.Windows.Interop.HwndSource HwndSource;
        /// <summary>
        /// ID
        /// </summary>
        private int Id;
        /// <summary>
        /// Hwnd
        /// </summary>
        private System.IntPtr Hwnd;
        /// <summary>
        /// Tip
        /// </summary>
        public string Tip;

        /// <summary>
        /// 「左ボタンがダブルクリックされた」イベント
        /// </summary>
        public event System.EventHandler<NotifyIconMouseEventArgs> LeftButtonDoubleClickEvent;
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
        public event System.EventHandler<NotifyIconMouseEventArgs> RightButtonClickEvent;
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
        /// コンストラクタ (使用しない)
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
            System.GC.SuppressFinalize(this);
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
        private System.IntPtr WndProc(
            System.IntPtr hwnd,
            int msg,
            System.IntPtr wparam,
            System.IntPtr lparam,
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

            return (System.IntPtr.Zero);
        }

        /// <summary>
        /// システムトレイアイコンを追加
        /// </summary>
        /// <param name="window">Window</param>
        /// <param name="id">ID</param>
        /// <param name="hwnd">ウィンドウハンドル</param>
        /// <param name="icon">アイコン</param>
        /// <param name="tip">ツールチップの文字列</param>
        /// <returns>追加に成功したかの値 (失敗「false」/成功「true」)</returns>
        public bool Add(
            System.Windows.Window window,
            int id,
            System.IntPtr hwnd,
            System.Drawing.Icon icon,
            string tip = ""
            )
        {
            bool result = false;        // 結果

            if (Hwnd == System.IntPtr.Zero)
            {
                NOTIFYICONDATA data = new();
                data.cbSize = System.Runtime.InteropServices.Marshal.SizeOf(typeof(NOTIFYICONDATA));
                data.uFlags = (int)NIF.NIF_ICON | (int)NIF.NIF_MESSAGE | (int)NIF.NIF_TIP;
                data.hwnd = hwnd;
                data.hIcon = icon.ToBitmap().GetHicon();
                data.uID = id;
                data.uCallbackMessage = WM_NOTIFYICON;
                data.szTip = tip;

                result = NativeMethods.Shell_NotifyIcon((uint)NIM.NIM_ADD, ref data);
                if (result)
                {
                    Id = id;
                    Hwnd = hwnd;
                    Tip = tip;

                    HwndSource = System.Windows.Interop.HwndSource.FromHwnd(new System.Windows.Interop.WindowInteropHelper(window).Handle);
                    HwndSource.AddHook(new(WndProc));
                }
            }

            return (result);
        }

        /// <summary>
        /// アイコンを変更
        /// </summary>
        /// <param name="icon">アイコン</param>
        /// <returns>変更に成功したかの値 (失敗「false」/成功「true」)</returns>
        public bool ModifyIcon(
            System.Drawing.Icon icon
            )
        {
            bool result = false;        // 結果

            if (Hwnd != System.IntPtr.Zero)
            {
                NOTIFYICONDATA data = new();
                data.cbSize = System.Runtime.InteropServices.Marshal.SizeOf(typeof(NOTIFYICONDATA));
                data.uFlags = (int)NIF.NIF_ICON | (int)NIF.NIF_MESSAGE | (int)NIF.NIF_TIP;
                data.hwnd = Hwnd;
                data.hIcon = icon.ToBitmap().GetHicon();
                data.uID = Id;
                data.uCallbackMessage = WM_NOTIFYICON;
                data.szTip = Tip;

                result = NativeMethods.Shell_NotifyIcon((uint)NIM.NIM_MODIFY, ref data);
            }

            return (result);
        }

        /// <summary>
        /// 削除
        /// </summary>
        public void Delete()
        {
            if (HwndSource != null)
            {
                HwndSource.RemoveHook(new(WndProc));
                HwndSource.Dispose();
                HwndSource = null;
            }

            if (Hwnd != System.IntPtr.Zero)
            {
                NOTIFYICONDATA data = new();
                data.cbSize = System.Runtime.InteropServices.Marshal.SizeOf(typeof(NOTIFYICONDATA));
                data.hwnd = Hwnd;
                data.uID = Id;

                NativeMethods.Shell_NotifyIcon((uint)NIM.NIM_DELETE, ref data);
                Hwnd = System.IntPtr.Zero;
                Id = 0;
                Tip = null;
            }
        }
    }
}
