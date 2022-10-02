namespace Swindom;

public class LanguageFileProcessing
{
    /// <summary>
    /// 言語ファイルから言語名を取得
    /// </summary>
    /// <param name="path">パス</param>
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
    /// 言語ファイルを読み込む
    /// </summary>
    /// <param name="languageFileName">言語のファイル名 (拡張子なし) (設定で指定している言語を使用する場合は「null」or「""」)</param>
    /// <param name="useDefaultValue">ファイルが無い、読み込めなかった、場合はデフォルト値で初期化するかの値 (いいえ「false」/はい「true」)</param>
    /// <returns>読み込みの結果 (失敗「false」/成功「true」)</returns>
    public static bool ReadLanguage(
        string? languageFileName = null,
        bool noFileDefaultValue = true
        )
    {
        bool result = false;        // 結果

        try
        {
            if (string.IsNullOrEmpty(languageFileName))
            {
                languageFileName = Common.ApplicationData.Settings.Language;
            }
            string path = Processing.GetApplicationDirectoryPath() + Path.DirectorySeparatorChar + Common.LanguagesDirectoryName + Path.DirectorySeparatorChar + languageFileName + Common.LanguageFileExtension;
            if (File.Exists(path))
            {
                DataContractSerializer serializer = new(typeof(Languages));
                using XmlReader reader = XmlReader.Create(path);
                object? readObject = (Languages?)serializer.ReadObject(reader);
                if (readObject != null)
                {
                    Common.ApplicationData.Languages = (Languages)readObject;
                    Common.ApplicationData.Settings.Language = languageFileName;
                    result = true;
                }
                else if (noFileDefaultValue)
                {
                    Common.ApplicationData.Languages = new Languages();
                }
            }
            else
            {
                if (noFileDefaultValue)
                {
                    Common.ApplicationData.Languages = new();
                }
            }
        }
        catch
        {
        }

        return result;
    }

#if DEBUG
    /// <summary>
    /// 言語ファイルに書き込む (言語ファイル作成用)
    /// </summary>
    /// <param name="path">パス</param>
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
        serializer.WriteObject(writer, Common.ApplicationData.Languages);
    }
#endif
}
