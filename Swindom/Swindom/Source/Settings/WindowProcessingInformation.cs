namespace Swindom.Source.Settings
{
    /// <summary>
    /// ウィンドウの処理情報
    /// </summary>
    public class WindowProcessingInformation
    {
        /// <summary>
        /// 処理名
        /// </summary>
        public string ProcessingName = "";
        /// <summary>
        /// 最前面の種類
        /// </summary>
        public Forefront Forefront = Forefront.DoNotChange;
        /// <summary>
        /// 透明度を指定 (無効「false」/有効「true」)
        /// </summary>
        public bool EnabledTransparency = false;
        /// <summary>
        /// 透明度
        /// </summary>
        public int Transparency = 255;
        /// <summary>
        /// ホットキー
        /// </summary>
        public FreeEcho.FEHotKeyWPF.HotKeyInformation Hotkey = new();
        /// <summary>
        /// ウィンドウの状態
        /// </summary>
        public WindowState WindowState = WindowState.DoNotChange;
        /// <summary>
        /// 「通常のウィンドウ」の時だけ処理する (無効「false」/有効「true」)
        /// </summary>
        public bool OnlyNormalWindow = false;
        /// <summary>
        /// 位置とサイズ
        /// </summary>
        public PositionSize PositionSize = new();
        /// <summary>
        /// アクティブ状態 (いいえ「false」/はい「true」)
        /// </summary>
        public bool Active = false;

        // ------------------ 設定ファイルに保存しないデータ ------------------ //
        /// <summary>
        /// ホットキーの識別子 (ホットキー無し「-1」)
        /// </summary>
        public int HotkeyId = -1;
        // ------------------ 設定ファイルに保存しないデータ ------------------ //

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public WindowProcessingInformation()
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="item">項目情報</param>
        /// <param name="copyHotkey">ホットキーをコピーする (いいえ「false」/はい「true」)</param>
        public WindowProcessingInformation(
            WindowProcessingInformation item,
            bool copyHotkey = true
            )
        {
            Copy(item, copyHotkey);
        }

        /// <summary>
        /// コピー
        /// </summary>
        /// <param name="item">ウィンドウの処理情報</param>
        /// <param name="copyHotkey">ホットキーをコピーする (いいえ「false」/はい「true」)</param>
        public void Copy(
            WindowProcessingInformation item,
            bool copyHotkey = true
            )
        {
            ProcessingName = item.ProcessingName;
            Forefront = item.Forefront;
            EnabledTransparency = item.EnabledTransparency;
            Transparency = item.Transparency;
            if (copyHotkey)
            {
                Hotkey.Copy(item.Hotkey);
            }
            WindowState = item.WindowState;
            OnlyNormalWindow = item.OnlyNormalWindow;
            PositionSize.Display = item.PositionSize.Display;
            PositionSize.Position = item.PositionSize.Position;
            PositionSize.XType = item.PositionSize.XType;
            PositionSize.YType = item.PositionSize.YType;
            PositionSize.Size = item.PositionSize.Size;
            PositionSize.WidthType = item.PositionSize.WidthType;
            PositionSize.HeightType = item.PositionSize.HeightType;
            PositionSize.XValueType = item.PositionSize.XValueType;
            PositionSize.YValueType = item.PositionSize.YValueType;
            PositionSize.WidthValueType = item.PositionSize.WidthValueType;
            PositionSize.HeightValueType = item.PositionSize.HeightValueType;
            PositionSize.ClientArea = item.PositionSize.ClientArea;
            PositionSize.ProcessingPositionAndSizeTwice = item.PositionSize.ProcessingPositionAndSizeTwice;
            Active = item.Active;
        }
    }
}
