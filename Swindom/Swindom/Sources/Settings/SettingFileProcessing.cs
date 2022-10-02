namespace Swindom;

/// <summary>
/// 設定ファイルの処理
/// </summary>
public static class SettingFileProcessing
{
    /// <summary>
    /// 設定ファイルが存在するか確認して無い場合は作成
    /// </summary>
    /// <returns>初めての実行かの値 (いいえ「false」/はい「true」)</returns>
    public static bool CheckAndCreateSettingFile()
    {
        bool firstRun = true;        // 初めての実行かの値

        if (string.IsNullOrEmpty(Common.ApplicationData.SpecifySettingsFilePath))
        {
            // 設定ファイルがない場合はディレクトリも含めて作成
            if (string.IsNullOrEmpty(GetSettingFilePath()))
            {
                bool allUser = false;      // 設定を全てのユーザーで共有するかの値

                switch (FreeEcho.FEControls.MessageBox.Show(Common.ApplicationData.Languages.Check + Common.CopySeparateString + Common.ApplicationName, Common.ApplicationData.Languages.LanguagesWindow?.ShareSettings, FreeEcho.FEControls.MessageBoxButton.YesNo))
                {
                    case FreeEcho.FEControls.MessageBoxResult.Yes:
                        allUser = true;
                        break;
                }
                CreateSettingDirectoryAndFile(allUser);
            }
            else
            {
                firstRun = false;
            }
        }
        else
        {
            if (File.Exists(Common.ApplicationData.SpecifySettingsFilePath))
            {
                firstRun = false;
            }
            else
            {
                WriteSettings();
            }
        }

        return firstRun;
    }

    /// <summary>
    /// 設定ファイルを読み込む
    /// </summary>
    public static void ReadSettings()
    {
        string path = Common.ApplicationData.SpecifySettingsFilePath ?? SettingFileProcessing.GetSettingFilePath();        // パス

        Common.ApplicationData.Settings.ReadFile(path);

        //if (File.Exists(path))
        //{
        //    DataContractSerializer serializer = new(typeof(Settings));
        //    using XmlReader reader = XmlReader.Create(path);
        //    object? readObject = (Settings?)serializer.ReadObject(reader);
        //    Common.ApplicationData.Settings = readObject == null ? new Settings() : (Settings)readObject;
        //}
    }

    /// <summary>
    /// 設定ファイルに書き込む
    /// 「path」が「null」の場合、ユーザー指定のパス又は既に存在するファイルのパス。
    /// </summary>
    /// <param name="path">設定ファイルのパス (指定しない「null」)</param>
    public static void WriteSettings(
        string? path = null
        )
    {
        if (string.IsNullOrEmpty(path))
        {
            path = Common.ApplicationData.SpecifySettingsFilePath ?? SettingFileProcessing.GetSettingFilePath();      // パス
        }

        Common.ApplicationData.Settings.WriteFile(path);

        //if (string.IsNullOrEmpty(path) == false)
        //{
        //    DataContractSerializer serializer = new(typeof(Settings));
        //    XmlWriterSettings xmlWriterSettings = new()
        //    {
        //        Encoding = new UTF8Encoding(),
        //        Indent = true,
        //        IndentChars = " ",
        //        NewLineOnAttributes = true
        //    };
        //    using XmlWriter writer = XmlWriter.Create(path, xmlWriterSettings);
        //    serializer.WriteObject(writer, Common.ApplicationData.Settings);
        //}
    }

    /// <summary>
    /// 設定ディレクトリと設定ファイルを作成
    /// </summary>
    /// <param name="allUser">設定データを全ユーザーで共有するかの値 (共有しない「false」/共有する「true」)</param>
    public static void CreateSettingDirectoryAndFile(
        bool allUser
        )
    {
        string settingDirectoryPath;      // 設定ディレクトリのパス
        bool checkDirectorySecurity = false;       // ディレクトリのセキュリティ属性を設定したかの値

        if (Processing.CheckInstalled())
        {
            settingDirectoryPath = Environment.GetFolderPath(allUser ? Environment.SpecialFolder.CommonApplicationData : Environment.SpecialFolder.LocalApplicationData) + Path.DirectorySeparatorChar + Common.ApplicationDirectoryName;
            // アプリケーションディレクトリがない場合はディレクトリを作成
            if (Directory.Exists(settingDirectoryPath) == false)
            {
                Directory.CreateDirectory(settingDirectoryPath);

                // 全ユーザーの場合はアクセス制御を全ユーザーに設定
                if (allUser)
                {
                    SetDirectorySecurity(settingDirectoryPath);
                    checkDirectorySecurity = true;
                }
            }
            settingDirectoryPath += Path.DirectorySeparatorChar + Common.SettingsDirectoryName;
        }
        else
        {
            settingDirectoryPath = Processing.GetApplicationDirectoryPath() + Path.DirectorySeparatorChar + Common.SettingsDirectoryName;
        }

        // 設定ディレクトリがなければ作成
        if (Directory.Exists(settingDirectoryPath) == false)
        {
            Directory.CreateDirectory(settingDirectoryPath);

            if (checkDirectorySecurity == false)
            {
                SetDirectorySecurity(settingDirectoryPath);
            }
        }

        // 設定ファイル作成
        WriteSettings(settingDirectoryPath + Path.DirectorySeparatorChar + (allUser ? Common.SettingFileNameForAllUsers : Environment.UserName) + Common.SettingFileExtension);
    }

    /// <summary>
    /// ディレクトリのセキュリティ属性を設定
    /// </summary>
    /// <param name="directoryPath">ディレクトリのパス</param>
    private static void SetDirectorySecurity(
        string directoryPath
        )
    {
        try
        {
            DirectoryInfo directoryInfo = new(directoryPath);
            DirectorySecurity fileSecurity = FileSystemAclExtensions.GetAccessControl(directoryInfo);
            FileSystemAccessRule accessRule = new(new System.Security.Principal.NTAccount("everyone"), FileSystemRights.FullControl, InheritanceFlags.ContainerInherit | InheritanceFlags.ObjectInherit, PropagationFlags.None, AccessControlType.Allow);
            fileSecurity.AddAccessRule(accessRule);
            FileSystemAclExtensions.SetAccessControl(directoryInfo, fileSecurity);
        }
        catch
        {
        }
    }

    /// <summary>
    /// 設定ディレクトリのパスを取得
    /// </summary>
    /// <returns>設定ディレクトリのパス (ディレクトリがない「""」)</returns>
    public static string GetSettingDirectoryPath()
    {
        string path = "";     // パス
        string tempPath;      // パスの一時保管用

        if (Processing.CheckInstalled())
        {
            string rearPath = Path.DirectorySeparatorChar + Common.ApplicationDirectoryName + Path.DirectorySeparatorChar + Common.SettingsDirectoryName;     // パスの後方

            if (Directory.Exists(tempPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + rearPath))
            {
                path = tempPath;
            }
            else if (Directory.Exists(tempPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + rearPath))
            {
                path = tempPath;
            }
        }
        else
        {
            if (Directory.Exists(tempPath = Processing.GetApplicationDirectoryPath() + Path.DirectorySeparatorChar + Common.SettingsDirectoryName))
            {
                path = tempPath;
            }
        }

        return path;
    }

    /// <summary>
    /// 設定ファイルのパスを取得
    /// </summary>
    /// <returns>設定ファイルのパス (ファイルがない「""」)</returns>
    public static string GetSettingFilePath()
    {
        string path = "";        // パス
        string directoryPath = GetSettingDirectoryPath();      // ディレクトリのパス

        if (string.IsNullOrEmpty(directoryPath) == false)
        {
            string stringData;     // 文字列

            if (File.Exists(stringData = directoryPath + Path.DirectorySeparatorChar + Environment.UserName + Common.SettingFileExtension))
            {
                path = stringData;
            }
            else if (File.Exists(stringData = directoryPath + Path.DirectorySeparatorChar + Common.SettingFileNameForAllUsers + Common.SettingFileExtension))
            {
                path = stringData;
            }
        }

        return path;
    }
}
