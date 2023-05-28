namespace Swindom;

/// <summary>
/// 言語ファイルの処理
/// </summary>
public static class LanguageFileProcessing
{
    /// <summary>
    /// 言語ファイルから言語名を取得
    /// </summary>
    /// <param name="path">言語ファイルのパス</param>
    /// <returns>言語名</returns>
    public static string GetLanguageNameFromLanguageFile(
        string path
        )
    {
        string name = "";       // 言語名

        try
        {
            DataContractSerializer serializer = new(typeof(Languages));
            using XmlReader reader = XmlReader.Create(path);
            object? readObject = (Languages?)serializer.ReadObject(reader);
            if (readObject != null)
            {
                Languages languages = (Languages)readObject;
                name = languages.LanguageName;
            }
        }
        catch
        {
        }

        return name;
    }

    /// <summary>
    /// 言語ファイルから読み込む
    /// </summary>
    /// <param name="fileName">ファイル名 (拡張子なし) (設定で指定している言語を使用する場合は「null」or「""」)</param>
    /// <param name="useDefaultValue">ファイルが無い、読み込めなかった、場合はデフォルト値で初期化するかの値 (いいえ「false」/はい「true」)</param>
    /// <returns>読み込みの結果 (失敗「false」/成功「true」)</returns>
    public static bool ReadLanguage(
        string? fileName = null,
        bool noFileDefaultValue = true
        )
    {
        bool result = false;        // 結果

        try
        {
            if (string.IsNullOrEmpty(fileName))
            {
                fileName = ApplicationData.Settings.Language;
            }
            string path = VariousProcessing.GetLanguageDirectoryPath() + fileName + Common.LanguageFileExtension;
            if (File.Exists(path))
            {
                DataContractSerializer serializer = new(typeof(Languages));
                using XmlReader reader = XmlReader.Create(path);
                object? readObject = (Languages?)serializer.ReadObject(reader);
                if (readObject != null)
                {
                    ApplicationData.Languages = (Languages)readObject;
                    ApplicationData.Settings.Language = fileName;
                    result = true;
                }
                else if (noFileDefaultValue)
                {
                    ApplicationData.Languages = new();
                }
            }
            else
            {
                throw new();
            }
        }
        catch
        {
            if (noFileDefaultValue)
            {
                ApplicationData.Languages = new();
            }
        }

        return result;
    }

#if DEBUG
    /// <summary>
    /// 言語ファイルに書き込む (言語ファイル作成用)
    /// </summary>
    /// <param name="path">言語ファイルのパス</param>
    public static void WriteLanguages(
        string path
        )
    {
        DataContractSerializer serializer = new(typeof(Languages));
        XmlWriterSettings xmlWriterSettings = new()
        {
            Encoding = new UTF8Encoding(),
            Indent = true,
            IndentChars = " ",
            NewLineOnAttributes = true
        };
        using XmlWriter writer = XmlWriter.Create(path, xmlWriterSettings);
        serializer.WriteObject(writer, new Languages());
    }
#endif
}
