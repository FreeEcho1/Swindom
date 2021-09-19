using Swindom.Source.Settings;

namespace Swindom.Source
{
    /// <summary>
    /// アプリケーションデータ
    /// </summary>
    public class ApplicationData : System.IDisposable
    {
        /// <summary>
        /// Disposeが呼ばれたかの値
        /// </summary>
        private bool Disposed;
        /// <summary>
        /// ウィンドウの管理
        /// </summary>
        public WindowManagement WindowManagement = new();
        /// <summary>
        /// ウィンドウ処理の管理
        /// </summary>
        public WindowProcessingManagement WindowProcessingManagement;
        /// <summary>
        /// 設定データ
        /// </summary>
        public Settings.Settings Settings = new();
        /// <summary>
        /// ユーザー指定の設定ファイルのパス (指定しない「null」)
        /// </summary>
        public string SpecifySettingsFilePath;
        /// <summary>
        /// 言語データ
        /// </summary>
        public Languages.Languages Languages = new();
        /// <summary>
        /// 処理イベント
        /// </summary>
        public event System.EventHandler<ProcessingEventArgs> ProcessingEvent;
        /// <summary>
        /// 処理イベントを実行
        /// </summary>
        /// <param name="processingEventType">処理イベントの種類</param>
        public virtual void DoProcessingEvent(
            ProcessingEventType processingEventType
            )
        {
            try
            {
                ProcessingEvent?.Invoke(this, new(processingEventType));
            }
            catch
            {
            }
        }
        /// <summary>
        /// 全画面のウィンドウがあるかの値 (いいえ「false」/はい「true」)
        /// </summary>
        public bool FullScreenExists;
        /// <summary>
        /// システムトレイアイコン用のウィンドウのウィンドウハンドル
        /// </summary>
        public System.IntPtr SystemTrayIconHwnd;

        /// <summary>
        /// デストラクタ
        /// </summary>
        ~ApplicationData()
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
                if (WindowManagement != null)
                {
                    WindowManagement.Dispose();
                    WindowManagement = null;
                }
                if (WindowProcessingManagement != null)
                {
                    WindowProcessingManagement.Dispose();
                    WindowProcessingManagement = null;
                }
            }
        }

        /// <summary>
        /// 言語を読み込む
        /// </summary>
        /// <param name="language">言語のファイル名 (拡張子なし) (設定で指定している言語「null」)</param>
        /// <returns>読み込みの結果 (失敗「false」/成功「true」)</returns>
        public bool ReadLanguages(
            string language = null
            )
        {
            bool result = false;        // 結果

            try
            {
                if (string.IsNullOrEmpty(language))
                {
                    language = Settings.Language;
                }
                language = Processing.GetApplicationDirectoryPath() + System.IO.Path.DirectorySeparatorChar + Common.LanguagesFolderName + System.IO.Path.DirectorySeparatorChar + language + Common.LanguageFileExtension;
                if (System.IO.File.Exists(language))
                {
                    result = Languages.ReadFile(language);
                }
                else
                {
                    Languages = new();
                    result = true;
                }
            }
            catch
            {
            }

            return (result);
        }

        /// <summary>
        /// 設定を読み込む
        /// </summary>
        public void ReadSettings()
        {
            string path = string.IsNullOrEmpty(SpecifySettingsFilePath) ? SettingFile.GetSettingFilePath() : SpecifySettingsFilePath;        // パス
            if (System.IO.File.Exists(path))
            {
                Settings.ReadFile(path);
            }
        }

        /// <summary>
        /// 設定を書き込む
        /// </summary>
        public void WriteSettings()
        {
            string path = string.IsNullOrEmpty(SpecifySettingsFilePath) ? SettingFile.GetSettingFilePath() : SpecifySettingsFilePath;      // パス
            if (System.IO.File.Exists(path))
            {
                Settings.WriteFile(path);
            }
        }
    }
}
