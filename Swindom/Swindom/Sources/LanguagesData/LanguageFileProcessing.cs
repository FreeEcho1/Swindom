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
    /// <returns>言語名 (失敗「""」)</returns>
    public static string GetLanguageNameFromFile(
        string path
        )
    {
        string name = "";       // 言語名

        try
        {
            string readString = "";
            using (StreamReader reader = new(path))
            {
                readString = reader.ReadToEnd();
            }
            using JsonDocument document = JsonDocument.Parse(readString);
            string? languageName = document.RootElement.GetProperty(JsonNamingPolicy.CamelCase.ConvertName(nameof(ApplicationData.Languages.LanguageName))).GetString();
            if (languageName != null)
            {
                name = languageName;
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
    /// <param name="fileName">ファイル名 (拡張子なし) (設定で指定している言語を使用する場合は「null」or「""」)</param>
    /// <param name="failDefaultValue">読み込めなかった場合はデフォルト値で初期化するかの値 (いいえ「false」/はい「true」)</param>
    /// <returns>読み込みの結果 (失敗「false」/成功「true」)</returns>
    public static bool ReadLanguage(
        string? fileName = null,
        bool failDefaultValue = true
        )
    {
        bool result = false;        // 結果

        try
        {
            if (string.IsNullOrEmpty(fileName))
            {
                fileName = ApplicationData.Settings.Language;
            }
            string path = ApplicationPath.GetLanguageDirectory() + fileName + LanguageValue.LanguageFileExtension;
            if (File.Exists(path))
            {
                string readString = "";

                using (StreamReader reader = new(path))
                {
                    readString = reader.ReadToEnd();
                }
                JsonSerializerOptions? options = new()
                {
                    Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                    WriteIndented = true,
                    IgnoreReadOnlyProperties = true,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };
                Languages? languages = JsonSerializer.Deserialize<Languages>(readString, options);
                if (languages != null)
                {
                    ApplicationData.Languages = languages;
                    result = true;
                }
                else if (failDefaultValue)
                {
                    ApplicationData.Languages = new();
                }
            }
            else
            {
                throw new("ReadLanguage() - Failed.");
            }
        }
        catch
        {
            if (failDefaultValue)
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
        JsonSerializerOptions? options = new()
        {
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
            WriteIndented = true,
            IgnoreReadOnlyProperties = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        string jsonString = JsonSerializer.Serialize(new Languages(), options);
        using StreamWriter writer = new(path);
        writer.Write(jsonString);
    }
#endif
}
