//#define CREATE_LANGUAGE_FILE        // 言語ファイルを作成する処理

namespace Swindom;

/// <summary>
/// アプリケーションの初期化処理
/// </summary>
public static class ApplicationInitialize
{
    /// <summary>
    /// 初期化
    /// </summary>
    /// <returns>初めての実行かの値</returns>
    public static bool Initialize()
    {
        // 言語ファイルを作成
#if CREATE_LANGUAGE_FILE
        string tempPath = ApplicationPath.GetLanguageDirectory() + "ja-JP.lang";
        LanguageFileProcessing.WriteLanguages(tempPath);
#endif

        // システムで設定されている言語を読み込む (システムで設定されている言語のファイルが無い場合は英語。言語ファイルが無い場合は読み込まない。)
        string language = "";        // 言語
        try
        {
            string[] listOfLanguages = Directory.GetFiles(ApplicationPath.GetLanguageDirectory(), "*" + LanguageValue.LanguageFileExtension);     // 存在する言語ファイルパス
            if (listOfLanguages.Length != 0)
            {
                string currentCultureName = System.Globalization.CultureInfo.CurrentCulture.Name;       // システムで設定されている言語
                // システムで設定されている言語のファイルがあるか確認
                foreach (string nowLanguage in listOfLanguages)
                {
                    if (currentCultureName == Path.GetFileNameWithoutExtension(nowLanguage))
                    {
                        language = currentCultureName;
                        break;
                    }
                }
                // システムで設定されている言語のファイルがない場合は英語のファイルがあるか確認
                if (string.IsNullOrEmpty(language))
                {
                    foreach (string nowLanguage in listOfLanguages)
                    {
                        if (Path.GetFileNameWithoutExtension(nowLanguage).Contains("en-US" + LanguageValue.LanguageFileExtension))
                        {
                            language = nowLanguage;
                            break;
                        }
                    }
                }
            }

            if (string.IsNullOrEmpty(language) == false)
            {
                LanguageFileProcessing.ReadLanguage(language);
            }
        }
        catch
        {
        }

        // 設定ファイルを読み込む
        bool firstRun = true;        // 初めての実行かの値
        // コマンドラインでファイルが指定されている場合はパスを取り出す
        foreach (string nowCommand in Environment.GetCommandLineArgs())
        {
            if (nowCommand.Contains(SettingsValue.SettingFileSpecificationString))
            {
                ApplicationData.SpecifySettingsFilePath = nowCommand.Remove(0, SettingsValue.SettingFileSpecificationString.Length);
                firstRun = false;
                break;
            }
        }
        if (SettingFileProcessing.ReadSettings())
        {
            firstRun = false;
            // 読み込んでいた言語と設定している言語が違う場合は設定している言語を読み込む
            if (language != ApplicationData.Settings.Language)
            {
                LanguageFileProcessing.ReadLanguage();
            }
        }
        else
        {
            SettingFileProcessing.WriteSettings();
        }
        if (string.IsNullOrEmpty(ApplicationData.Settings.Language))
        {
            ApplicationData.Settings.Language = language;
        }

        try
        {
            // プラグインのディレクトリがない場合は作成
            if (ApplicationPath.CheckInstalled() == false)
            {
                string pluginDirectory = ApplicationPath.GetPluginDirectory();     // プラグインのディレクトリ
                if (Directory.Exists(pluginDirectory) == false)
                {
                    Directory.CreateDirectory(pluginDirectory);
                }
            }
        }
        catch
        {
        }
        PluginProcessing.CheckExistsPlugin();

        if (ApplicationData.Settings.DarkMode)
        {
            ThemeManager.Current.ApplicationTheme = ApplicationTheme.Dark;
        }
        else
        {
            ThemeManager.Current.ApplicationTheme = ApplicationTheme.Light;
        }
        WindowProcessingValue.PathMaxLength = ApplicationData.Settings.UseLongPath ? WindowProcessingValue.LongPathMaxLength : WindowProcessingValue.PathMaxLength;

        // 更新確認
        if (ApplicationData.Settings.AutomaticallyUpdateCheck)
        {
            try
            {
                switch (FreeEcho.FECheckForUpdate.CheckForUpdate.CheckForUpdateFileURL(ApplicationPath.OwnApplicationPath(), ApplicationValue.UpdateCheckURL, out FreeEcho.FECheckForUpdate.VersionInformation? versionInformation, ApplicationData.Settings.CheckBetaVersion))
                {
                    case FreeEcho.FECheckForUpdate.CheckForUpdateResult.NotLatestVersion:
                        ApplicationData.WindowManagement.ShowUpdateCheckWindow();
                        break;
                }
            }
            catch
            {
            }
        }

        return firstRun;
    }
}
