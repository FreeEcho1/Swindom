namespace Swindom.Source.Settings
{
    /// <summary>
    /// 「指定ウィンドウ」機能の基礎の項目情報
    /// </summary>
    public class SpecifiedWindowBaseItemInformation
    {
        /// <summary>
        /// 有効状態 (無効「false」/有効「true」)
        /// </summary>
        public bool Enabled = true;
        /// <summary>
        /// 登録名
        /// </summary>
        public string RegisteredName = "";
        /// <summary>
        /// タイトル名
        /// </summary>
        public string TitleName = "";
        /// <summary>
        /// タイトル名の一致条件
        /// </summary>
        public NameMatchCondition TitleNameMatchCondition = NameMatchCondition.ExactMatch;
        /// <summary>
        /// クラス名
        /// </summary>
        public string ClassName = "";
        /// <summary>
        /// クラス名の一致条件
        /// </summary>
        public NameMatchCondition ClassNameMatchCondition = NameMatchCondition.ExactMatch;
        /// <summary>
        /// ファイル名
        /// </summary>
        public string FileName = "";
        /// <summary>
        /// ファイル名の一致条件
        /// </summary>
        public FileNameMatchCondition FileNameMatchCondition = FileNameMatchCondition.Include;
        /// <summary>
        /// 1度だけ処理
        /// </summary>
        public ProcessingOnlyOnce ProcessingOnlyOnce = ProcessingOnlyOnce.NotSpecified;
        /// <summary>
        /// ウィンドウを閉じる (無効「false」/有効「true」)
        /// </summary>
        public bool CloseWindow = false;
        /// <summary>
        /// 基準にするディスプレイ
        /// </summary>
        public StandardDisplay StandardDisplay = StandardDisplay.SpecifiedDisplay;
        /// <summary>
        /// ウィンドウの処理情報
        /// </summary>
        public System.Collections.Generic.List<WindowProcessingInformation> WindowProcessingInformation = new();
        /// <summary>
        /// タイトルの処理条件
        /// </summary>
        public TitleProcessingConditions TitleProcessingConditions = TitleProcessingConditions.NotSpecified;
        /// <summary>
        /// 処理しないウィンドウのサイズ
        /// </summary>
        public System.Collections.Generic.List<System.Drawing.Size> DoNotProcessingSize = new();
        /// <summary>
        /// 処理しないタイトル名の一部の文字列
        /// </summary>
        public System.Collections.Generic.List<string> DoNotProcessingTitleName = new();

        // ------------------ 設定ファイルに保存しないデータ ------------------ //
        /// <summary>
        /// 1度だけ処理が終わっているかの値 (終わってない「false」/終わってる「true」)
        /// </summary>
        public bool EndedProcessingOnlyOnce = false;
        /// <summary>
        /// ウィンドウハンドル
        /// </summary>
        public System.IntPtr Hwnd = System.IntPtr.Zero;
        // ------------------ 設定ファイルに保存しないデータ ------------------ //

        /// <summary>
        /// コンストラクタ
        /// </summary>
        public SpecifiedWindowBaseItemInformation()
        {
        }

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="item">「指定ウィンドウ」機能の基礎の項目情報</param>
        /// <param name="copyHotkey">ホットキーをコピーするかの値 (いいえ「false」/はい「true」)</param>
        public SpecifiedWindowBaseItemInformation(
            SpecifiedWindowBaseItemInformation item,
            bool copyHotkey = true
            )
        {
            Enabled = item.Enabled;
            RegisteredName = item.RegisteredName;
            TitleName = item.TitleName;
            TitleNameMatchCondition = item.TitleNameMatchCondition;
            ClassName = item.ClassName;
            ClassNameMatchCondition = item.ClassNameMatchCondition;
            FileName = item.FileName;
            FileNameMatchCondition = item.FileNameMatchCondition;
            ProcessingOnlyOnce = item.ProcessingOnlyOnce;
            CloseWindow = item.CloseWindow;
            StandardDisplay = item.StandardDisplay;
            foreach (WindowProcessingInformation nowWPI in item.WindowProcessingInformation)
            {
                WindowProcessingInformation.Add(new WindowProcessingInformation(nowWPI, copyHotkey));
            }
            TitleProcessingConditions = item.TitleProcessingConditions;
            foreach (System.Drawing.Size nowSize in item.DoNotProcessingSize)
            {
                DoNotProcessingSize.Add(nowSize);
            }
            DoNotProcessingTitleName = new(item.DoNotProcessingTitleName);
        }
    }
}
