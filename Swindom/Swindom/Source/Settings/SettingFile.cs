namespace Swindom.Source.Settings
{
    /// <summary>
    /// 設定ファイルの処理
    /// </summary>
    public static class SettingFile
    {
        /// <summary>
        /// 設定ファイルが存在するか確認して無い場合は作成
        /// </summary>
        /// <returns>初めての実行かの値 (いいえ「false」/はい「true」)</returns>
        public static bool CheckAndCreateTheSettingFile()
        {
            bool firstRun = true;        // 初めての実行かの値

            // 設定ファイルがない場合はフォルダを作成
            if (GetSettingFilePath() == null)
            {
                bool allUser = false;      // 設定を全てのユーザーで共有するかの値

                switch (FreeEcho.FEControls.MessageBox.Show(Common.ApplicationData.Languages.Check + Common.SeparateString + Common.ApplicationName, Common.ApplicationData.Languages.LanguagesWindow.ShareSettings, FreeEcho.FEControls.MessageBoxButton.YesNo))
                {
                    case FreeEcho.FEControls.MessageBoxResult.Yes:
                        allUser = true;
                        break;
                }
                CreateSettingFolderAndFile(allUser);
            }
            else
            {
                firstRun = false;
            }

            return (firstRun);
        }

        /// <summary>
        /// 設定フォルダと設定ファイルを作成
        /// </summary>
        /// <param name="allUser">設定データを全ユーザーで共有するかの値 (共有しない「false」/共有する「true」)</param>
        public static void CreateSettingFolderAndFile(
            bool allUser
            )
        {
            string settingFolderPath;      // 設定フォルダのパス
            bool checkFolderSecurity = false;       // ディレクトリのセキュリティ属性を設定したかの値

            if (Processing.CheckInstalled())
            {
                settingFolderPath = System.Environment.GetFolderPath(allUser ? System.Environment.SpecialFolder.CommonApplicationData : System.Environment.SpecialFolder.LocalApplicationData) + System.IO.Path.DirectorySeparatorChar + Common.ApplicationFolderName;
                // アプリケーションフォルダがない場合はフォルダを作成
                if (System.IO.Directory.Exists(settingFolderPath) == false)
                {
                    System.IO.Directory.CreateDirectory(settingFolderPath);

                    // 全ユーザーの場合はアクセス制御を全ユーザーに設定
                    if (allUser)
                    {
                        SetDirectorySecurity(settingFolderPath);
                        checkFolderSecurity = true;
                    }
                }
                settingFolderPath += System.IO.Path.DirectorySeparatorChar + Common.SettingsFolderName;
            }
            else
            {
                settingFolderPath = Processing.GetApplicationDirectoryPath() + System.IO.Path.DirectorySeparatorChar + Common.SettingsFolderName;
            }

            // 設定フォルダがなければ作成
            if (System.IO.Directory.Exists(settingFolderPath) == false)
            {
                System.IO.Directory.CreateDirectory(settingFolderPath);

                if (checkFolderSecurity == false)
                {
                    SetDirectorySecurity(settingFolderPath);
                }
            }

            // 設定ファイル作成
            using System.IO.FileStream fs = System.IO.File.Create(settingFolderPath + System.IO.Path.DirectorySeparatorChar + (allUser ? Common.SettingFileNameForAllUsers : System.Environment.UserName) + Common.SettingFileExtension);
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
                System.IO.DirectoryInfo directoryInfo = new(directoryPath);
                System.Security.AccessControl.DirectorySecurity fileSecurity = System.IO.FileSystemAclExtensions.GetAccessControl(directoryInfo);
                System.Security.AccessControl.FileSystemAccessRule accessRule = new(new System.Security.Principal.NTAccount("everyone"), System.Security.AccessControl.FileSystemRights.FullControl, System.Security.AccessControl.InheritanceFlags.ContainerInherit | System.Security.AccessControl.InheritanceFlags.ObjectInherit, System.Security.AccessControl.PropagationFlags.None, System.Security.AccessControl.AccessControlType.Allow);
                fileSecurity.AddAccessRule(accessRule);
                System.IO.FileSystemAclExtensions.SetAccessControl(directoryInfo, fileSecurity);
            }
            catch
            {
            }
        }

        /// <summary>
        /// 設定フォルダのパスを取得
        /// </summary>
        /// <returns>設定フォルダのパス (フォルダがない「null」)</returns>
        public static string GetSettingFolderPath()
        {
            string path = null;     // パス
            string tempPath;      // パスの一時保管用

            if (Processing.CheckInstalled())
            {
                string rearPath = System.IO.Path.DirectorySeparatorChar + Common.ApplicationFolderName + System.IO.Path.DirectorySeparatorChar + Common.SettingsFolderName;     // パスの後方

                if (System.IO.Directory.Exists(tempPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.LocalApplicationData) + rearPath))
                {
                    path = tempPath;
                }
                else if (System.IO.Directory.Exists(tempPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.CommonApplicationData) + rearPath))
                {
                    path = tempPath;
                }
            }
            else
            {
                if (System.IO.Directory.Exists(tempPath = Processing.GetApplicationDirectoryPath() + System.IO.Path.DirectorySeparatorChar + Common.SettingsFolderName))
                {
                    path = tempPath;
                }
            }

            return (path);
        }

        /// <summary>
        /// 設定ファイルのパスを取得
        /// </summary>
        /// <returns>設定ファイルのパス (ファイルがない「null」)</returns>
        public static string GetSettingFilePath()
        {
            string path = null;        // パス
            string folderPath = GetSettingFolderPath();      // フォルダのパス

            if (folderPath != null)
            {
                string stringData;     // 文字列

                if (System.IO.File.Exists(stringData = folderPath + System.IO.Path.DirectorySeparatorChar + System.Environment.UserName + Common.SettingFileExtension))
                {
                    path = stringData;
                }
                else if (System.IO.File.Exists(stringData = folderPath + System.IO.Path.DirectorySeparatorChar + Common.SettingFileNameForAllUsers + Common.SettingFileExtension))
                {
                    path = stringData;
                }
            }

            return (path);
        }
    }
}
