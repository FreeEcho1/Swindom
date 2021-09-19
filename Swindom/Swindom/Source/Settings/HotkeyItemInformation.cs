namespace Swindom.Source.Settings
{
    /// <summary>
    /// 「ホットキー」機能の項目情報
    /// </summary>
    public class HotkeyItemInformation
    {
        /// <summary>
        /// 登録名
        /// </summary>
        public string RegisteredName = "";
        /// <summary>
        /// 処理の種類
        /// </summary>
        public HotkeyProcessingType ProcessingType = HotkeyProcessingType.PositionSize;
        /// <summary>
        /// 位置とサイズ
        /// </summary>
        public PositionSize PositionSize = new();
        /// <summary>
        /// 基準にするディスプレイ
        /// </summary>
        public StandardDisplay StandardDisplay = StandardDisplay.SpecifiedDisplay;
        /// <summary>
        /// 処理値 (移動量、サイズ変更量、透明度)
        /// </summary>
        public int ProcessingValue = 0;
        /// <summary>
        /// ホットキー情報
        /// </summary>
        public FreeEcho.FEHotKeyWPF.HotKeyInformation Hotkey = new();

        // ------------------ 設定ファイルに保存しないデータ ------------------ //
        /// <summary>
        /// ホットキーの識別子 (ホットキー無し「-1」)
        /// </summary>
        public int HotkeyId = -1;
        // ------------------ 設定ファイルに保存しないデータ ------------------ //

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public HotkeyItemInformation()
        {
        }

        /// <summary>
        /// コピー
        /// </summary>
        /// <param name="item">「ホットキー」機能の項目情報</param>
        public void Copy(
            HotkeyItemInformation item
            )
        {
            RegisteredName = item.RegisteredName;
            ProcessingType = item.ProcessingType;
            PositionSize.Position = new(item.PositionSize.Position.X, item.PositionSize.Position.Y);
            PositionSize.Size = new(item.PositionSize.Size.Width, item.PositionSize.Size.Height);
            PositionSize.XType = item.PositionSize.XType;
            PositionSize.YType = item.PositionSize.YType;
            PositionSize.WidthType = item.PositionSize.WidthType;
            PositionSize.HeightType = item.PositionSize.HeightType;
            PositionSize.XValueType = item.PositionSize.XValueType;
            PositionSize.YValueType = item.PositionSize.YValueType;
            PositionSize.WidthValueType = item.PositionSize.WidthValueType;
            PositionSize.HeightValueType = item.PositionSize.HeightValueType;
            PositionSize.Display = item.PositionSize.Display;
            PositionSize.ClientArea = item.PositionSize.ClientArea;
            PositionSize.ProcessingPositionAndSizeTwice = item.PositionSize.ProcessingPositionAndSizeTwice;
            StandardDisplay = item.StandardDisplay;
            ProcessingValue = item.ProcessingValue;
            Hotkey.Copy(item.Hotkey);
        }
    }
}
