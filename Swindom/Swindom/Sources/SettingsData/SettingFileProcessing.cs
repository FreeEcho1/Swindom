namespace Swindom;

/// <summary>
/// 設定ファイルの処理
/// </summary>
public static class SettingFileProcessing
{
    /// <summary>
    /// 設定ファイルを読み込む
    /// </summary>
    /// <returns>結果 (失敗「false」/成功「true」)</returns>
    public static bool ReadSettings()
    {
        string path = GetSettingFilePath();     // パス

        if (string.IsNullOrEmpty(path))
        {
            return false;
        }

        // 以前の形式の設定ファイルを読み込む
        // 新しい形式の設定ファイルを作成
        // 以前の形式の設定ファイルを削除
        if (ApplicationData.SpecifySettingsFilePath == null
            && path.Contains(SettingsValue.OldSettingFileExtension))
        {
            // 読み込む
            bool result = ReadSettingsFileXML(path);        // 結果

            string? directoryName = Path.GetDirectoryName(path);        // ディレクトリ
            string? fileNameWithoutExtension = Path.GetFileNameWithoutExtension(path);      // ファイル名
            if (directoryName != null && fileNameWithoutExtension != null)
            {
                string newSettingFilePath = directoryName + Path.DirectorySeparatorChar + fileNameWithoutExtension + SettingsValue.SettingFileExtension;       // 新しい設定ファイルのパス

                // 書き込む
                if (WriteSettings(newSettingFilePath))
                {
                    // 旧設定ファイルの削除確認をして削除
                    if (MessageBox.Show(ApplicationData.Languages.NewSettingFile, ApplicationData.Languages.Check + WindowControlValue.CopySeparateString + ApplicationValue.ApplicationName, MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                    {
                        try
                        {
                            File.Delete(path);
                            MessageBox.Show(ApplicationData.Languages.Deleted, ApplicationData.Languages.Check + WindowControlValue.CopySeparateString + ApplicationValue.ApplicationName, MessageBoxButton.OK);
                        }
                        catch
                        {
                            MessageBox.Show(ApplicationData.Languages.ErrorOccurred, ApplicationData.Languages.Check + WindowControlValue.CopySeparateString + ApplicationValue.ApplicationName, MessageBoxButton.OK);
                        }
                    }
                }
            }

            return result;
        }

        return ReadSettingsFile(path);
    }

    /// <summary>
    /// 設定ファイルに書き込む
    /// </summary>
    /// <param name="path">設定ファイルのパス (指定しない (存在するファイル又は作成)「null」)</param>
    public static bool WriteSettings(
        string? path = null
        )
    {
        if (string.IsNullOrEmpty(path))
        {
            path = GetSettingFilePath();
        }
        return string.IsNullOrEmpty(path) ? CreateSettingFile() : WriteSettingsFile(path);
    }

    /// <summary>
    /// 設定ファイルを作成
    /// <para>設定を全てのユーザーで共有するかを確認するメッセージを表示 (ユーザー指定を除く)。</para>
    /// <para>ディレクトリがない場合は作成 (ユーザー指定を除く)。</para>
    /// </summary>
    private static bool CreateSettingFile()
    {
        if (string.IsNullOrEmpty(ApplicationData.SpecifySettingsFilePath))
        {
            bool allUser = false;      // 設定を全てのユーザーで共有するかの値
            string settingDirectoryPath = "";      // 設定ディレクトリのパス
            bool checkDirectorySecurity = false;       // ディレクトリのセキュリティ属性を設定したかの値

            if (ApplicationPath.CheckInstalled())
            {
                string rearPath = Path.DirectorySeparatorChar + ApplicationValue.ApplicationName;     // パスの後方
                string tempPath;      // パスの一時保管用

                // 存在するディレクトリを取得
                if (Directory.Exists(tempPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + rearPath))
                {
                    settingDirectoryPath = tempPath;
                }
                else if (Directory.Exists(tempPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + rearPath))
                {
                    settingDirectoryPath = tempPath;
                }

                // ディレクトリが存在しない場合は全てのユーザーで設定を共有するか確認して作成
                if (string.IsNullOrEmpty(settingDirectoryPath))
                {
                    switch (FEMessageBox.Show(ApplicationData.Languages.ShareSettings, ApplicationData.Languages.Check + WindowControlValue.CopySeparateString + ApplicationValue.ApplicationName, MessageBoxButton.YesNo))
                    {
                        case MessageBoxResult.Yes:
                            allUser = true;
                            break;
                    }

                    settingDirectoryPath = Environment.GetFolderPath(allUser ? Environment.SpecialFolder.CommonApplicationData : Environment.SpecialFolder.LocalApplicationData) + Path.DirectorySeparatorChar + ApplicationValue.ApplicationName;
                    // アプリケーションディレクトリを作成
                    Directory.CreateDirectory(settingDirectoryPath);
                    // 全ユーザーの場合はアクセス制御を全ユーザーに設定
                    if (allUser)
                    {
                        SettingDirectorySecurity(settingDirectoryPath);
                        checkDirectorySecurity = true;
                    }
                }

                settingDirectoryPath += Path.DirectorySeparatorChar + SettingsValue.SettingsDirectoryName;
            }
            else
            {
                switch (FEMessageBox.Show(ApplicationData.Languages.ShareSettings, ApplicationData.Languages.Check + WindowControlValue.CopySeparateString + ApplicationValue.ApplicationName, MessageBoxButton.YesNo))
                {
                    case MessageBoxResult.Yes:
                        allUser = true;
                        break;
                }

                settingDirectoryPath = ApplicationPath.GetApplicationDirectory() + Path.DirectorySeparatorChar + SettingsValue.SettingsDirectoryName;
            }

            // 設定ディレクトリがない場合は作成
            if (Directory.Exists(settingDirectoryPath) == false)
            {
                Directory.CreateDirectory(settingDirectoryPath);
                if (checkDirectorySecurity == false)
                {
                    SettingDirectorySecurity(settingDirectoryPath);
                }
            }

            // 設定ファイル作成
            return WriteSettings(settingDirectoryPath + Path.DirectorySeparatorChar + (allUser ? SettingsValue.SettingFileNameForAllUsers : Environment.UserName) + SettingsValue.SettingFileExtension);
        }
        else
        {
            return WriteSettings();
        }
    }

    /// <summary>
    /// ディレクトリのセキュリティ属性を設定
    /// </summary>
    /// <param name="directoryPath">ディレクトリのパス</param>
    private static void SettingDirectorySecurity(
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
    /// 設定ディレクトリを取得
    /// </summary>
    /// <returns>設定ディレクトリのパス (ディレクトリがない「""」)</returns>
    public static string GetSettingsDirectory()
    {
        string path = "";     // パス

        if (string.IsNullOrEmpty(ApplicationData.SpecifySettingsFilePath))
        {
            string tempPath;      // パスの一時保管用

            if (ApplicationPath.CheckInstalled())
            {
                string rearPath = Path.DirectorySeparatorChar + ApplicationValue.ApplicationName + Path.DirectorySeparatorChar + SettingsValue.SettingsDirectoryName;     // パスの後方

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
                if (Directory.Exists(tempPath = ApplicationPath.GetApplicationDirectory() + Path.DirectorySeparatorChar + SettingsValue.SettingsDirectoryName))
                {
                    path = tempPath;
                }
            }
        }
        else
        {
            path = Path.GetDirectoryName(ApplicationData.SpecifySettingsFilePath) ?? "";
        }

        return path;
    }

    /// <summary>
    /// 設定ファイルのパスを取得
    /// </summary>
    /// <returns>設定ファイルのパス (ファイルがない「""」)</returns>
    private static string GetSettingFilePath()
    {
        string path = "";        // パス

        if (string.IsNullOrEmpty(ApplicationData.SpecifySettingsFilePath))
        {
            string directoryPath = GetSettingsDirectory();      // ディレクトリのパス

            if (string.IsNullOrEmpty(directoryPath) == false)
            {
                string stringData;     // 文字列

                // 優先順位が高い順に判定
                if (File.Exists(stringData = directoryPath + Path.DirectorySeparatorChar + Environment.UserName + SettingsValue.SettingFileExtension))
                {
                    path = stringData;
                }
                else if (File.Exists(stringData = directoryPath + Path.DirectorySeparatorChar + Environment.UserName + SettingsValue.OldSettingFileExtension))
                {
                    path = stringData;
                }
                else if (File.Exists(stringData = directoryPath + Path.DirectorySeparatorChar + SettingsValue.SettingFileNameForAllUsers + SettingsValue.SettingFileExtension))
                {
                    path = stringData;
                }
                else if (File.Exists(stringData = directoryPath + Path.DirectorySeparatorChar + SettingsValue.SettingFileNameForAllUsers + SettingsValue.OldSettingFileExtension))
                {
                    path = stringData;
                }
            }
        }
        else
        {
            if (File.Exists(ApplicationData.SpecifySettingsFilePath))
            {
                path = ApplicationData.SpecifySettingsFilePath;
            }
        }

        return path;
    }

    /// <summary>
    /// 設定ファイルに書き込む
    /// </summary>
    /// <param name="path">設定ファイルのパス</param>
    /// <returns>結果 (失敗「false」/成功「true」)</returns>
    private static bool WriteSettingsFile(
        string path
        )
    {
        bool result = false;        // 結果

        try
        {
            JsonSerializerOptions options = new()
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                WriteIndented = true,
                IgnoreReadOnlyProperties = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            string writeString = JsonSerializer.Serialize(ApplicationData.Settings, options);

            using (StreamWriter writer = new(path))
            {
                writer.Write(writeString);
            }

            result = true;
        }
        catch
        {
        }

        return result;
    }

    /// <summary>
    /// 設定ファイルから読み込む
    /// </summary>
    /// <param name="path">設定ファイルのパス</param>
    /// <returns>結果 (失敗「false」/成功「true」)</returns>
    private static bool ReadSettingsFile(
        string path
        )
    {
        bool result = false;        // 結果

        try
        {
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
                Settings? settings = JsonSerializer.Deserialize<Settings>(readString, options);
                if (settings != null)
                {
                    ApplicationData.Settings = settings;
                    result = true;
                }
            }
        }
        catch
        {
        }

        return result;
    }

    /// <summary>
    /// 設定ファイルから読み込む (XML)
    /// </summary>
    /// <param name="path">設定ファイルのパス</param>
    /// <returns>結果 (失敗「false」/成功「true」)</returns>
    private static bool ReadSettingsFileXML(
        string path
        )
    {
        bool result = false;        // 結果

        try
        {
            XDocument document = XDocument.Load(path);
            XElement? element = document.Element("Settings");

            if (element != null)
            {
                XElement? elementNode1;

                // Start MainWindowRectangle
                elementNode1 = element.Element("MainWindowRectangle");
                if (elementNode1 != null)
                {
                    ApplicationData.Settings.MainWindowRectangle.X = FileProcessing.GetIntXElement(elementNode1, "X", ApplicationData.Settings.MainWindowRectangle.X);
                    ApplicationData.Settings.MainWindowRectangle.Y = FileProcessing.GetIntXElement(elementNode1, "Y", ApplicationData.Settings.MainWindowRectangle.Y);
                    ApplicationData.Settings.MainWindowRectangle.Width = FileProcessing.GetIntXElement(elementNode1, "Width", ApplicationData.Settings.MainWindowRectangle.Width);
                    ApplicationData.Settings.MainWindowRectangle.Height = FileProcessing.GetIntXElement(elementNode1, "Height", ApplicationData.Settings.MainWindowRectangle.Height);
                }
                // End MainWindowRectangle
                _ = Enum.TryParse(FileProcessing.GetStringXElement(element, "WindowStateMainWindow", Enum.GetName(typeof(WindowState), ApplicationData.Settings.WindowStateMainWindow) ?? ""), out WindowState getWindowState);
                ApplicationData.Settings.WindowStateMainWindow = getWindowState;
                ApplicationData.Settings.Language = FileProcessing.GetStringXElement(element, "Language", ApplicationData.Settings.Language);
                _ = Enum.TryParse(FileProcessing.GetStringXElement(element, "CoordinateType", Enum.GetName(typeof(CoordinateType), ApplicationData.Settings.CoordinateType) ?? ""), out CoordinateType getCoordinateType);
                ApplicationData.Settings.CoordinateType = getCoordinateType;
                ApplicationData.Settings.DarkMode = FileProcessing.GetBoolXElement(element, "DarkMode", ApplicationData.Settings.DarkMode);
                ApplicationData.Settings.AutomaticallyUpdateCheck = FileProcessing.GetBoolXElement(element, "AutomaticallyUpdateCheck", ApplicationData.Settings.AutomaticallyUpdateCheck);
                ApplicationData.Settings.CheckBetaVersion = FileProcessing.GetBoolXElement(element, "CheckBetaVersion", ApplicationData.Settings.CheckBetaVersion);
                ApplicationData.Settings.UseLongPath = FileProcessing.GetBoolXElement(element, "UseLongPath", ApplicationData.Settings.UseLongPath);
                // Start ShiftPastePosition
                elementNode1 = element.Element("ShiftPastePosition");
                if (elementNode1 != null)
                {
                    ApplicationData.Settings.ShiftPastePosition.IsEnabled = FileProcessing.GetBoolXElement(elementNode1, "Enabled", ApplicationData.Settings.ShiftPastePosition.IsEnabled);
                    ApplicationData.Settings.ShiftPastePosition.Left = FileProcessing.GetIntXElement(elementNode1, "Left", ApplicationData.Settings.ShiftPastePosition.Left);
                    ApplicationData.Settings.ShiftPastePosition.Top = FileProcessing.GetIntXElement(elementNode1, "Top", ApplicationData.Settings.ShiftPastePosition.Top);
                    ApplicationData.Settings.ShiftPastePosition.Right = FileProcessing.GetIntXElement(elementNode1, "Right", ApplicationData.Settings.ShiftPastePosition.Right);
                    ApplicationData.Settings.ShiftPastePosition.Bottom = FileProcessing.GetIntXElement(elementNode1, "Bottom", ApplicationData.Settings.ShiftPastePosition.Bottom);
                }
                // End ShiftPastePosition
                // Start SpecifyWindowInformation
                elementNode1 = element.Element("SpecifyWindowInformation");
                if (elementNode1 == null)
                {
                    elementNode1 = element.Element("EventInformation");
                }
                if (elementNode1 != null)
                {
                    ApplicationData.Settings.SpecifyWindowInformation.IsEnabled = FileProcessing.GetBoolXElement(elementNode1, "Enabled", ApplicationData.Settings.SpecifyWindowInformation.IsEnabled);
                    ApplicationData.Settings.SpecifyWindowInformation.MultipleRegistrations = FileProcessing.GetBoolXElement(elementNode1, "MultipleRegistrations", ApplicationData.Settings.SpecifyWindowInformation.MultipleRegistrations);
                    ApplicationData.Settings.SpecifyWindowInformation.CaseSensitive = FileProcessing.GetBoolXElement(elementNode1, "CaseSensitive", ApplicationData.Settings.SpecifyWindowInformation.CaseSensitive);
                    ApplicationData.Settings.SpecifyWindowInformation.DoNotChangeOutOfScreen = FileProcessing.GetBoolXElement(elementNode1, "DoNotChangeOutOfScreen", ApplicationData.Settings.SpecifyWindowInformation.DoNotChangeOutOfScreen);
                    ApplicationData.Settings.SpecifyWindowInformation.StopProcessingShowAddModifyWindow = FileProcessing.GetBoolXElement(elementNode1, "StopProcessingShowAddModifyWindow", ApplicationData.Settings.SpecifyWindowInformation.StopProcessingShowAddModifyWindow);
                    ApplicationData.Settings.SpecifyWindowInformation.StopProcessingFullScreen = FileProcessing.GetBoolXElement(elementNode1, "StopProcessingFullScreen", ApplicationData.Settings.SpecifyWindowInformation.StopProcessingFullScreen);
                    ApplicationData.Settings.SpecifyWindowInformation.HotkeysDoNotStopFullScreen = FileProcessing.GetBoolXElement(elementNode1, "HotkeysDoNotStopFullScreen", ApplicationData.Settings.SpecifyWindowInformation.HotkeysDoNotStopFullScreen);
                    ApplicationData.Settings.SpecifyWindowInformation.ProcessingInterval = FileProcessing.GetIntXElement(elementNode1, "ProcessingInterval", ApplicationData.Settings.SpecifyWindowInformation.ProcessingInterval);
                    _ = Enum.TryParse(FileProcessing.GetStringXElement(elementNode1, "ProcessingWindowRange", Enum.GetName(typeof(ProcessingWindowRange), ApplicationData.Settings.SpecifyWindowInformation.ProcessingWindowRange) ?? ""), out ProcessingWindowRange getProcessingWindowRange);
                    ApplicationData.Settings.SpecifyWindowInformation.ProcessingWindowRange = getProcessingWindowRange;
                    ApplicationData.Settings.SpecifyWindowInformation.WaitTimeToProcessingNextWindow = FileProcessing.GetIntXElement(elementNode1, "WaitTimeToProcessingNextWindow", ApplicationData.Settings.SpecifyWindowInformation.WaitTimeToProcessingNextWindow);
                    XElement? elementNode2;
                    elementNode2 = elementNode1.Element("AddModifyWindowSize");
                    if (elementNode2 != null)
                    {
                        ApplicationData.Settings.SpecifyWindowInformation.AddModifyWindowSize.Width = FileProcessing.GetIntXElement(elementNode2, "Width", ApplicationData.Settings.SpecifyWindowInformation.AddModifyWindowSize.Width);
                        ApplicationData.Settings.SpecifyWindowInformation.AddModifyWindowSize.Height = FileProcessing.GetIntXElement(elementNode2, "Height", ApplicationData.Settings.SpecifyWindowInformation.AddModifyWindowSize.Height);
                    }
                    elementNode2 = elementNode1.Element("AcquiredItems");
                    if (elementNode2 != null)
                    {
                        ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.TitleName = FileProcessing.GetBoolXElement(elementNode1, "TitleName", ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.TitleName);
                        ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.ClassName = FileProcessing.GetBoolXElement(elementNode1, "ClassName", ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.ClassName);
                        ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.FileName = FileProcessing.GetBoolXElement(elementNode1, "FileName", ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.FileName);
                        ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.Display = FileProcessing.GetBoolXElement(elementNode1, "Display", ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.Display);
                        ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.WindowState = FileProcessing.GetBoolXElement(elementNode1, "WindowState", ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.WindowState);
                        ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.X = FileProcessing.GetBoolXElement(elementNode1, "X", ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.X);
                        ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.Y = FileProcessing.GetBoolXElement(elementNode1, "Y", ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.Y);
                        ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.Width = FileProcessing.GetBoolXElement(elementNode1, "Width", ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.Width);
                        ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.Height = FileProcessing.GetBoolXElement(elementNode1, "Height", ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.Height);
                        ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.Version = FileProcessing.GetBoolXElement(elementNode1, "Version", ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.Version);
                    }
                    elementNode2 = elementNode1.Element("Items");
                    if (elementNode2 != null)
                    {
                        // Start SpecifyWindowItemInformation
                        foreach (XElement nowElementEII in elementNode2.Elements("Item"))
                        {
                            SpecifyWindowItemInformation newEII = new();

                            newEII.IsEnabled = FileProcessing.GetBoolXElement(nowElementEII, "Enabled", newEII.IsEnabled);
                            newEII.RegisteredName = FileProcessing.GetStringXElement(nowElementEII, "RegisteredName", newEII.RegisteredName);
                            newEII.TitleName = FileProcessing.GetStringXElement(nowElementEII, "TitleName", newEII.TitleName);
                            _ = Enum.TryParse(FileProcessing.GetStringXElement(nowElementEII, "TitleNameMatchCondition", Enum.GetName(typeof(NameMatchCondition), newEII.TitleNameMatchCondition) ?? ""), out NameMatchCondition getNameMatchCondition);
                            newEII.TitleNameMatchCondition = getNameMatchCondition;
                            newEII.ClassName = FileProcessing.GetStringXElement(nowElementEII, "ClassName", newEII.ClassName);
                            _ = Enum.TryParse(FileProcessing.GetStringXElement(nowElementEII, "ClassNameMatchCondition", Enum.GetName(typeof(NameMatchCondition), newEII.ClassNameMatchCondition) ?? ""), out getNameMatchCondition);
                            newEII.ClassNameMatchCondition = getNameMatchCondition;
                            newEII.FileName = FileProcessing.GetStringXElement(nowElementEII, "FileName", newEII.FileName);
                            _ = Enum.TryParse(FileProcessing.GetStringXElement(nowElementEII, "FileNameMatchCondition", Enum.GetName(typeof(FileNameMatchCondition), newEII.FileNameMatchCondition) ?? ""), out FileNameMatchCondition getFileNameMatchCondition);
                            newEII.FileNameMatchCondition = getFileNameMatchCondition;
                            _ = Enum.TryParse(FileProcessing.GetStringXElement(nowElementEII, "StandardDisplay", Enum.GetName(typeof(StandardDisplay), newEII.StandardDisplay) ?? ""), out StandardDisplay getStandardDisplay);
                            newEII.StandardDisplay = getStandardDisplay;
                            _ = Enum.TryParse(FileProcessing.GetStringXElement(nowElementEII, "ProcessingOnlyOnce", Enum.GetName(typeof(ProcessingOnlyOnce), newEII.ProcessingOnlyOnce) ?? ""), out ProcessingOnlyOnce getProcessingOnlyOnce);
                            newEII.ProcessingOnlyOnce = getProcessingOnlyOnce;
                            XElement? elementNode4 = nowElementEII.Element("WindowEventData");
                            if (elementNode4 != null)
                            {
                                newEII.WindowEventData.Foreground = FileProcessing.GetBoolXElement(elementNode4, "Foreground", newEII.WindowEventData.Foreground);
                                newEII.WindowEventData.MoveSizeEnd = FileProcessing.GetBoolXElement(elementNode4, "MoveSizeEnd", newEII.WindowEventData.MoveSizeEnd);
                                newEII.WindowEventData.MinimizeStart = FileProcessing.GetBoolXElement(elementNode4, "MinimizeStart", newEII.WindowEventData.MinimizeStart);
                                newEII.WindowEventData.MinimizeEnd = FileProcessing.GetBoolXElement(elementNode4, "MinimizeEnd", newEII.WindowEventData.MinimizeEnd);
                                newEII.WindowEventData.Show = FileProcessing.GetBoolXElement(elementNode4, "Show", newEII.WindowEventData.Show);
                                newEII.WindowEventData.NameChange = FileProcessing.GetBoolXElement(elementNode4, "NameChange", newEII.WindowEventData.NameChange);
                            }
                            newEII.TimerProcessing = FileProcessing.GetBoolXElement(nowElementEII, "TimerProcessing", newEII.TimerProcessing);
                            newEII.NumberOfTimesNotToProcessingFirst = FileProcessing.GetIntXElement(nowElementEII, "NumberOfTimesNotToProcessingFirst", newEII.NumberOfTimesNotToProcessingFirst);
                            // Start WindowProcessingInformation
                            XElement? elementNode5 = nowElementEII.Element("WindowProcessingInformation");
                            if (elementNode5 != null)
                            {
                                foreach (XElement nowElement in elementNode5.Elements("WindowProcessingInformation"))
                                {
                                    WindowProcessingInformation newWPI = new();

                                    newWPI.Active = FileProcessing.GetBoolXElement(nowElement, "Active", newWPI.Active);
                                    newWPI.ProcessingName = FileProcessing.GetStringXElement(nowElement, "ProcessingName", newWPI.ProcessingName);
                                    // Start PositionSize
                                    XElement? elementNode6 = nowElement.Element("PositionSize");
                                    if (elementNode6 != null)
                                    {
                                        newWPI.PositionSize.Display = FileProcessing.GetStringXElement(elementNode6, "Display", newWPI.PositionSize.Display);
                                        _ = Enum.TryParse(FileProcessing.GetStringXElement(elementNode6, "SettingsWindowState", Enum.GetName(typeof(SettingsWindowState), newWPI.PositionSize.SettingsWindowState) ?? ""), out SettingsWindowState getSettingsWindowState);
                                        newWPI.PositionSize.SettingsWindowState = getSettingsWindowState;
                                        newWPI.PositionSize.X = FileProcessing.GetDoubleXElement(elementNode6, "X", newWPI.PositionSize.X);
                                        newWPI.PositionSize.Y = FileProcessing.GetDoubleXElement(elementNode6, "Y", newWPI.PositionSize.Y);
                                        newWPI.PositionSize.Width = FileProcessing.GetDoubleXElement(elementNode6, "Width", newWPI.PositionSize.Width);
                                        newWPI.PositionSize.Height = FileProcessing.GetDoubleXElement(elementNode6, "Height", newWPI.PositionSize.Height);
                                        _ = Enum.TryParse(FileProcessing.GetStringXElement(elementNode6, "XType", Enum.GetName(typeof(WindowXType), newWPI.PositionSize.XType) ?? ""), out WindowXType getWindowXType);
                                        newWPI.PositionSize.XType = getWindowXType;
                                        _ = Enum.TryParse(FileProcessing.GetStringXElement(elementNode6, "YType", Enum.GetName(typeof(WindowYType), newWPI.PositionSize.YType) ?? ""), out WindowYType getWindowYType);
                                        newWPI.PositionSize.YType = getWindowYType;
                                        _ = Enum.TryParse(FileProcessing.GetStringXElement(elementNode6, "WidthType", Enum.GetName(typeof(WindowSizeType), newWPI.PositionSize.WidthType) ?? ""), out WindowSizeType getWindowSizeType);
                                        newWPI.PositionSize.WidthType = getWindowSizeType;
                                        _ = Enum.TryParse(FileProcessing.GetStringXElement(elementNode6, "HeightType", Enum.GetName(typeof(WindowSizeType), newWPI.PositionSize.HeightType) ?? ""), out getWindowSizeType);
                                        newWPI.PositionSize.HeightType = getWindowSizeType;
                                        _ = Enum.TryParse(FileProcessing.GetStringXElement(elementNode6, "XValueType", Enum.GetName(typeof(PositionSizeValueType), newWPI.PositionSize.XValueType) ?? ""), out PositionSizeValueType getPositionSizeValuleType);
                                        newWPI.PositionSize.XValueType = getPositionSizeValuleType;
                                        _ = Enum.TryParse(FileProcessing.GetStringXElement(elementNode6, "YValueType", Enum.GetName(typeof(PositionSizeValueType), newWPI.PositionSize.YValueType) ?? ""), out getPositionSizeValuleType);
                                        newWPI.PositionSize.YValueType = getPositionSizeValuleType;
                                        _ = Enum.TryParse(FileProcessing.GetStringXElement(elementNode6, "WidthValueType", Enum.GetName(typeof(PositionSizeValueType), newWPI.PositionSize.WidthValueType) ?? ""), out getPositionSizeValuleType);
                                        newWPI.PositionSize.WidthValueType = getPositionSizeValuleType;
                                        _ = Enum.TryParse(FileProcessing.GetStringXElement(elementNode6, "HeightValueType", Enum.GetName(typeof(PositionSizeValueType), newWPI.PositionSize.HeightValueType) ?? ""), out getPositionSizeValuleType);
                                        newWPI.PositionSize.HeightValueType = getPositionSizeValuleType;
                                        newWPI.PositionSize.ClientArea = FileProcessing.GetBoolXElement(elementNode6, "ClientArea", newWPI.PositionSize.ClientArea);
                                    }
                                    // End PositionSize
                                    newWPI.NormalWindowOnly = FileProcessing.GetBoolXElement(nowElement, "NormalWindowOnly", newWPI.NormalWindowOnly);
                                    _ = Enum.TryParse(FileProcessing.GetStringXElement(nowElement, "Forefront", Enum.GetName(typeof(Forefront), newWPI.Forefront) ?? ""), out Forefront getForefront);
                                    newWPI.Forefront = getForefront;
                                    newWPI.SpecifyTransparency = FileProcessing.GetBoolXElement(nowElement, "EnabledTransparency", newWPI.SpecifyTransparency);
                                    newWPI.Transparency = FileProcessing.GetIntXElement(nowElement, "Transparency", newWPI.Transparency);
                                    newWPI.CloseWindow = FileProcessing.GetBoolXElement(nowElement, "CloseWindow", newWPI.CloseWindow);
                                    XElement? elementNode7 = nowElement.Element("Hotkey");
                                    if (elementNode7 != null)
                                    {
                                        newWPI.Hotkey.Alt = FileProcessing.GetBoolXElement(elementNode7, "Alt", newWPI.Hotkey.Alt);
                                        newWPI.Hotkey.Ctrl = FileProcessing.GetBoolXElement(elementNode7, "Ctrl", newWPI.Hotkey.Ctrl);
                                        newWPI.Hotkey.Shift = FileProcessing.GetBoolXElement(elementNode7, "Shift", newWPI.Hotkey.Shift);
                                        newWPI.Hotkey.Windows = FileProcessing.GetBoolXElement(elementNode7, "Windows", newWPI.Hotkey.Windows);
                                        newWPI.Hotkey.KeyCharacter = FileProcessing.GetIntXElement(elementNode7, "KeyCharacter", newWPI.Hotkey.KeyCharacter);
                                    }

                                    newEII.WindowProcessingInformation.Add(newWPI);
                                }
                            }
                            // End WindowProcessingInformation
                            _ = Enum.TryParse(FileProcessing.GetStringXElement(nowElementEII, "DoNotProcessingTitleNameConditions", Enum.GetName(typeof(TitleNameProcessingConditions), newEII.DoNotProcessingTitleNameConditions) ?? ""), out TitleNameProcessingConditions getTitleNameProcessingConditions);
                            newEII.DoNotProcessingTitleNameConditions = getTitleNameProcessingConditions;
                            // Start DoNotProcessingStringContainedInTitleName
                            elementNode5 = nowElementEII.Element("DoNotProcessingStringContainedInTitleName");
                            if (elementNode5 != null)
                            {
                                foreach (XElement nowElement in elementNode5.Elements("Item"))
                                {
                                    string newString = "";
                                    newString = FileProcessing.GetStringXElement(nowElement, "String", newString);
                                    newEII.DoNotProcessingStringContainedInTitleName.Add(newString);
                                }
                            }
                            // End DoNotProcessingStringContainedInTitleName
                            // Start DoNotProcessingSize
                            elementNode5 = nowElementEII.Element("DoNotProcessingSize");
                            if (elementNode5 != null)
                            {
                                foreach (XElement nowElement in elementNode5.Elements("Item"))
                                {
                                    SizeInt newSize = new();
                                    newSize.Width = FileProcessing.GetIntXElement(nowElement, "Width", newSize.Width);
                                    newSize.Height = FileProcessing.GetIntXElement(nowElement, "Height", newSize.Height);
                                    newEII.DoNotProcessingSize.Add(newSize);
                                }
                            }
                            // End DoNotProcessingSize
                            newEII.DoNotProcessingOtherThanSpecifiedVersion = FileProcessing.GetStringXElement(nowElementEII, "DoNotProcessingOtherThanSpecifiedVersion", newEII.DoNotProcessingOtherThanSpecifiedVersion);
                            newEII.Notification = FileProcessing.GetBoolXElement(nowElementEII, "DoNotProcessingOtherThanSpecifiedVersionAnnounce", newEII.Notification);
                            newEII.NotificationOtherThanSpecifiedVersion = newEII.DoNotProcessingOtherThanSpecifiedVersion;

                            ApplicationData.Settings.SpecifyWindowInformation.Items.Add(newEII);
                        }
                        // End SpecifyWindowItemInformation
                    }
                }
                // End SpecifyWindowInformation
                // Start AllWindowInformation
                elementNode1 = element.Element("AllWindowInformation");
                if (elementNode1 != null)
                {
                    XElement? elementNode2;
                    ApplicationData.Settings.AllWindowInformation.IsEnabled = FileProcessing.GetBoolXElement(elementNode1, "Enabled", ApplicationData.Settings.AllWindowInformation.IsEnabled);
                    ApplicationData.Settings.AllWindowInformation.CaseSensitive = FileProcessing.GetBoolXElement(elementNode1, "CaseSensitive", ApplicationData.Settings.AllWindowInformation.CaseSensitive);
                    ApplicationData.Settings.AllWindowInformation.StopProcessingFullScreen = FileProcessing.GetBoolXElement(elementNode1, "StopProcessingFullScreen", ApplicationData.Settings.AllWindowInformation.StopProcessingFullScreen);
                    _ = Enum.TryParse(FileProcessing.GetStringXElement(elementNode1, "StandardDisplay", Enum.GetName(typeof(StandardDisplay), ApplicationData.Settings.AllWindowInformation.StandardDisplay) ?? ""), out StandardDisplay getStandardDisplay);
                    ApplicationData.Settings.AllWindowInformation.StandardDisplay = getStandardDisplay;
                    // Start PositionSize
                    elementNode2 = elementNode1.Element("PositionSize");
                    if (elementNode2 != null)
                    {
                        ApplicationData.Settings.AllWindowInformation.PositionSize.Display = FileProcessing.GetStringXElement(elementNode2, "Display", ApplicationData.Settings.AllWindowInformation.PositionSize.Display);
                        ApplicationData.Settings.AllWindowInformation.PositionSize.X = FileProcessing.GetDoubleXElement(elementNode2, "X", ApplicationData.Settings.AllWindowInformation.PositionSize.X);
                        ApplicationData.Settings.AllWindowInformation.PositionSize.Y = FileProcessing.GetDoubleXElement(elementNode2, "Y", ApplicationData.Settings.AllWindowInformation.PositionSize.Y);
                        _ = Enum.TryParse(FileProcessing.GetStringXElement(elementNode2, "XType", Enum.GetName(typeof(WindowXType), ApplicationData.Settings.AllWindowInformation.PositionSize.XType) ?? ""), out WindowXType getWindowXType);
                        ApplicationData.Settings.AllWindowInformation.PositionSize.XType = getWindowXType;
                        _ = Enum.TryParse(FileProcessing.GetStringXElement(elementNode2, "YType", Enum.GetName(typeof(WindowYType), ApplicationData.Settings.AllWindowInformation.PositionSize.YType) ?? ""), out WindowYType getWindowYType);
                        ApplicationData.Settings.AllWindowInformation.PositionSize.YType = getWindowYType;
                        _ = Enum.TryParse(FileProcessing.GetStringXElement(elementNode2, "XValueType", Enum.GetName(typeof(PositionSizeValueType), ApplicationData.Settings.AllWindowInformation.PositionSize.XValueType) ?? ""), out PositionSizeValueType getPositionSizeValueType);
                        ApplicationData.Settings.AllWindowInformation.PositionSize.XValueType = getPositionSizeValueType;
                        _ = Enum.TryParse(FileProcessing.GetStringXElement(elementNode2, "YValueType", Enum.GetName(typeof(PositionSizeValueType), ApplicationData.Settings.AllWindowInformation.PositionSize.YValueType) ?? ""), out getPositionSizeValueType);
                        ApplicationData.Settings.AllWindowInformation.PositionSize.YValueType = getPositionSizeValueType;
                    }
                    // End PositionSize
                    elementNode2 = elementNode1.Element("AddModifyWindowSize");
                    if (elementNode2 != null)
                    {
                        ApplicationData.Settings.AllWindowInformation.AddModifyWindowSize.Width = FileProcessing.GetIntXElement(elementNode2, "Width", ApplicationData.Settings.AllWindowInformation.AddModifyWindowSize.Width);
                        ApplicationData.Settings.AllWindowInformation.AddModifyWindowSize.Height = FileProcessing.GetIntXElement(elementNode2, "Height", ApplicationData.Settings.AllWindowInformation.AddModifyWindowSize.Height);
                    }
                    // Start AllWindowPositionSizeWindowEventData
                    elementNode2 = elementNode1.Element("AllWindowPositionSizeWindowEventData");
                    if (elementNode2 != null)
                    {
                        ApplicationData.Settings.AllWindowInformation.WindowEvent.MoveSizeEnd = FileProcessing.GetBoolXElement(elementNode2, "MoveSizeEnd", ApplicationData.Settings.AllWindowInformation.WindowEvent.MoveSizeEnd);
                        ApplicationData.Settings.AllWindowInformation.WindowEvent.Show = FileProcessing.GetBoolXElement(elementNode2, "Show", ApplicationData.Settings.AllWindowInformation.WindowEvent.Show);
                    }
                    // End AllWindowPositionSizeWindowEventData
                    // Start CancelAllWindowPositionSize
                    elementNode2 = elementNode1.Element("CancelAllWindowPositionSize");
                    if (elementNode2 != null)
                    {
                        foreach (XElement nowElementWJI in elementNode2.Elements("Item"))
                        {
                            WindowJudgementInformation newWJI = new();

                            newWJI.RegisteredName = FileProcessing.GetStringXElement(nowElementWJI, "RegisteredName", newWJI.RegisteredName);
                            newWJI.TitleName = FileProcessing.GetStringXElement(nowElementWJI, "TitleName", newWJI.TitleName);
                            _ = Enum.TryParse(FileProcessing.GetStringXElement(nowElementWJI, "TitleNameMatchCondition", Enum.GetName(typeof(NameMatchCondition), newWJI.TitleNameMatchCondition) ?? ""), out NameMatchCondition getNameMatchCondition);
                            newWJI.TitleNameMatchCondition = getNameMatchCondition;
                            newWJI.ClassName = FileProcessing.GetStringXElement(nowElementWJI, "ClassName", newWJI.ClassName);
                            _ = Enum.TryParse(FileProcessing.GetStringXElement(nowElementWJI, "ClassNameMatchCondition", Enum.GetName(typeof(NameMatchCondition), newWJI.ClassNameMatchCondition) ?? ""), out getNameMatchCondition);
                            newWJI.ClassNameMatchCondition = getNameMatchCondition;
                            newWJI.FileName = FileProcessing.GetStringXElement(nowElementWJI, "FileName", newWJI.FileName);
                            _ = Enum.TryParse(FileProcessing.GetStringXElement(nowElementWJI, "FileNameMatchCondition", Enum.GetName(typeof(FileNameMatchCondition), newWJI.FileNameMatchCondition) ?? ""), out FileNameMatchCondition getFileNameMatchCondition);
                            newWJI.FileNameMatchCondition = getFileNameMatchCondition;

                            ApplicationData.Settings.AllWindowInformation.Items.Add(newWJI);
                        }
                    }
                    // End CancelAllWindowPositionSize
                }
                // End AllWindowInformation
                // Start MagnetInformation
                elementNode1 = element.Element("MagnetInformation");
                if (elementNode1 != null)
                {
                    ApplicationData.Settings.MagnetInformation.IsEnabled = FileProcessing.GetBoolXElement(elementNode1, "Enabled", ApplicationData.Settings.MagnetInformation.IsEnabled);
                    ApplicationData.Settings.MagnetInformation.PasteToEdgeOfScreen = FileProcessing.GetBoolXElement(elementNode1, "PasteToEdgeOfScreen", ApplicationData.Settings.MagnetInformation.PasteToEdgeOfScreen);
                    ApplicationData.Settings.MagnetInformation.PasteToAnotherWindow = FileProcessing.GetBoolXElement(elementNode1, "PasteToAnotherWindow", ApplicationData.Settings.MagnetInformation.PasteToAnotherWindow);
                    ApplicationData.Settings.MagnetInformation.PasteToPressKey = FileProcessing.GetBoolXElement(elementNode1, "PasteToPressKey", ApplicationData.Settings.MagnetInformation.PasteToPressKey);
                    ApplicationData.Settings.MagnetInformation.PastingDecisionDistance = FileProcessing.GetIntXElement(elementNode1, "PastingDecisionDistance", ApplicationData.Settings.MagnetInformation.PastingDecisionDistance);
                    ApplicationData.Settings.MagnetInformation.PastingTime = FileProcessing.GetIntXElement(elementNode1, "StopTimeWhenPasted", ApplicationData.Settings.MagnetInformation.PastingTime);
                    ApplicationData.Settings.MagnetInformation.StopProcessingFullScreen = FileProcessing.GetBoolXElement(elementNode1, "StopProcessingFullScreen", ApplicationData.Settings.MagnetInformation.StopProcessingFullScreen);
                }
                // End MagnetInformation
                // Start HotkeyInformation
                elementNode1 = element.Element("HotkeyInformation");
                if (elementNode1 != null)
                {
                    ApplicationData.Settings.HotkeyInformation.IsEnabled = FileProcessing.GetBoolXElement(elementNode1, "Enabled", ApplicationData.Settings.HotkeyInformation.IsEnabled);
                    ApplicationData.Settings.HotkeyInformation.DoNotChangeOutOfScreen = FileProcessing.GetBoolXElement(elementNode1, "DoNotChangeOutOfScreen", ApplicationData.Settings.HotkeyInformation.DoNotChangeOutOfScreen);
                    ApplicationData.Settings.HotkeyInformation.StopProcessingFullScreen = FileProcessing.GetBoolXElement(elementNode1, "StopProcessingFullScreen", ApplicationData.Settings.HotkeyInformation.StopProcessingFullScreen);
                    XElement? elementNode2 = elementNode1.Element("AddModifyWindowSize");
                    if (elementNode2 != null)
                    {
                        ApplicationData.Settings.HotkeyInformation.AddModifyWindowSize.Width = FileProcessing.GetIntXElement(elementNode2, "Width", ApplicationData.Settings.HotkeyInformation.AddModifyWindowSize.Width);
                        ApplicationData.Settings.HotkeyInformation.AddModifyWindowSize.Height = FileProcessing.GetIntXElement(elementNode2, "Height", ApplicationData.Settings.HotkeyInformation.AddModifyWindowSize.Height);
                    }
                    // Start Items
                    elementNode2 = elementNode1.Element("Items");
                    if (elementNode2 != null)
                    {
                        foreach (XElement nowElementHII in elementNode2.Elements("Item"))
                        {
                            HotkeyItemInformation newHII = new();

                            newHII.RegisteredName = FileProcessing.GetStringXElement(nowElementHII, "RegisteredName", newHII.RegisteredName);
                            _ = Enum.TryParse(FileProcessing.GetStringXElement(nowElementHII, "ProcessingType", System.Enum.GetName(typeof(HotkeyProcessingType), newHII.ProcessingType) ?? ""), out HotkeyProcessingType hptValue);
                            newHII.ProcessingType = hptValue;
                            _ = Enum.TryParse(FileProcessing.GetStringXElement(nowElementHII, "StandardDisplay", Enum.GetName(typeof(StandardDisplay), newHII.StandardDisplay) ?? ""), out StandardDisplay getStandardDisplay);
                            newHII.StandardDisplay = getStandardDisplay;
                            XElement? elementNode3 = nowElementHII.Element("PositionSize");
                            if (elementNode3 != null)
                            {
                                newHII.PositionSize.Display = FileProcessing.GetStringXElement(elementNode3, "Display", newHII.PositionSize.Display);
                                _ = Enum.TryParse(FileProcessing.GetStringXElement(elementNode3, "SettingsWindowState", Enum.GetName(typeof(SettingsWindowState), newHII.PositionSize.SettingsWindowState) ?? ""), out SettingsWindowState getSettingsWindowState);
                                newHII.PositionSize.SettingsWindowState = getSettingsWindowState;
                                newHII.PositionSize.X = FileProcessing.GetDoubleXElement(elementNode3, "X", newHII.PositionSize.X);
                                newHII.PositionSize.Y = FileProcessing.GetDoubleXElement(elementNode3, "Y", newHII.PositionSize.Y);
                                newHII.PositionSize.Width = FileProcessing.GetDoubleXElement(elementNode3, "Width", newHII.PositionSize.Width);
                                newHII.PositionSize.Height = FileProcessing.GetDoubleXElement(elementNode3, "Height", newHII.PositionSize.Height);
                                _ = Enum.TryParse(FileProcessing.GetStringXElement(elementNode3, "XType", Enum.GetName(typeof(WindowXType), newHII.PositionSize.XType) ?? ""), out WindowXType getWindowXType);
                                newHII.PositionSize.XType = getWindowXType;
                                _ = Enum.TryParse(FileProcessing.GetStringXElement(elementNode3, "YType", Enum.GetName(typeof(WindowYType), newHII.PositionSize.YType) ?? ""), out WindowYType getWindowYType);
                                newHII.PositionSize.YType = getWindowYType;
                                _ = Enum.TryParse(FileProcessing.GetStringXElement(elementNode3, "WidthType", Enum.GetName(typeof(WindowSizeType), newHII.PositionSize.WidthType) ?? ""), out WindowSizeType getWindowSizeType);
                                newHII.PositionSize.WidthType = getWindowSizeType;
                                _ = Enum.TryParse(FileProcessing.GetStringXElement(elementNode3, "HeightType", Enum.GetName(typeof(WindowSizeType), newHII.PositionSize.HeightType) ?? ""), out getWindowSizeType);
                                newHII.PositionSize.HeightType = getWindowSizeType;
                                _ = Enum.TryParse(FileProcessing.GetStringXElement(elementNode3, "XValueType", Enum.GetName(typeof(PositionSizeValueType), newHII.PositionSize.XValueType) ?? ""), out PositionSizeValueType value);
                                newHII.PositionSize.XValueType = value;
                                _ = Enum.TryParse(FileProcessing.GetStringXElement(elementNode3, "YValueType", Enum.GetName(typeof(PositionSizeValueType), newHII.PositionSize.YValueType) ?? ""), out value);
                                newHII.PositionSize.YValueType = value;
                                _ = Enum.TryParse(FileProcessing.GetStringXElement(elementNode3, "WidthValueType", Enum.GetName(typeof(PositionSizeValueType), newHII.PositionSize.WidthValueType) ?? ""), out value);
                                newHII.PositionSize.WidthValueType = value;
                                _ = Enum.TryParse(FileProcessing.GetStringXElement(elementNode3, "HeightValueType", Enum.GetName(typeof(PositionSizeValueType), newHII.PositionSize.HeightValueType) ?? ""), out value);
                                newHII.PositionSize.HeightValueType = value;
                                newHII.PositionSize.ClientArea = FileProcessing.GetBoolXElement(elementNode3, "ClientArea", newHII.PositionSize.ClientArea);
                            }
                            newHII.ProcessingValue = FileProcessing.GetIntXElement(nowElementHII, "ProcessingValue", newHII.ProcessingValue);
                            elementNode3 = nowElementHII.Element("Hotkey");
                            if (elementNode3 != null)
                            {
                                newHII.Hotkey.Alt = FileProcessing.GetBoolXElement(elementNode3, "Alt", newHII.Hotkey.Alt);
                                newHII.Hotkey.Ctrl = FileProcessing.GetBoolXElement(elementNode3, "Ctrl", newHII.Hotkey.Ctrl);
                                newHII.Hotkey.Shift = FileProcessing.GetBoolXElement(elementNode3, "Shift", newHII.Hotkey.Shift);
                                newHII.Hotkey.Windows = FileProcessing.GetBoolXElement(elementNode3, "Windows", newHII.Hotkey.Windows);
                                newHII.Hotkey.KeyCharacter = FileProcessing.GetIntXElement(elementNode3, "KeyCharacter", newHII.Hotkey.KeyCharacter);
                            }

                            ApplicationData.Settings.HotkeyInformation.Items.Add(newHII);
                        }
                    }
                    // End Items
                }
                // End HotkeyInformation
                // Start PluginInformation
                elementNode1 = element.Element("PluginInformation");
                if (elementNode1 != null)
                {
                    ApplicationData.Settings.PluginInformation.IsEnabled = FileProcessing.GetBoolXElement(elementNode1, "Enabled", ApplicationData.Settings.PluginInformation.IsEnabled);
                    // Start Items
                    XElement? elementNode2 = elementNode1.Element("Items");
                    if (elementNode2 != null)
                    {
                        foreach (XElement nowElementPII in elementNode2.Elements("Item"))
                        {
                            PluginItemInformation newItem = new();

                            newItem.PluginFileName = FileProcessing.GetStringXElement(nowElementPII, "PluginFileName", newItem.PluginFileName);

                            ApplicationData.Settings.PluginInformation.Items.Add(newItem);
                        }
                    }
                    // End Items
                }
                // End PluginInformation

                result = true;
            }
        }
        catch
        {
        }

        return result;
    }

    /// <summary>
    /// 設定ファイルから読み込む
    /// </summary>
    /// <param name="path">設定ファイルのパス</param>
    /// <returns>結果 (失敗「false」/成功「true」)</returns>
    //private static bool ReadSettingsFile(
    //    string path
    //    )
    //{
    //    bool result = false;        // 結果

    //    try
    //    {
    //        string readString = "";
    //        using (StreamReader reader = new(path))
    //        {
    //            readString = reader.ReadToEnd();
    //        }
    //        JsonDocument? jsonDocument = JsonSerializer.Deserialize<JsonDocument>(readString);

    //        if (jsonDocument != null)
    //        {
    //            JsonElement rootElement = jsonDocument.RootElement;
    //            JsonElement element1, element2, element3, element4;

    //            if (rootElement.TryGetProperty(JsonNamingPolicy.CamelCase.ConvertName(nameof(ApplicationData.Settings.MainWindowRectangle)), out element1))
    //            {
    //                ApplicationData.Settings.MainWindowRectangle.X = FileProcessing.GetIntJsonElement(element1, JsonNamingPolicy.CamelCase.ConvertName(nameof(ApplicationData.Settings.MainWindowRectangle.X)), ApplicationData.Settings.MainWindowRectangle.X);
    //                ApplicationData.Settings.MainWindowRectangle.Y = FileProcessing.GetIntJsonElement(element1, JsonNamingPolicy.CamelCase.ConvertName(nameof(ApplicationData.Settings.MainWindowRectangle.Y)), ApplicationData.Settings.MainWindowRectangle.Y);
    //                ApplicationData.Settings.MainWindowRectangle.Width = FileProcessing.GetIntJsonElement(element1, JsonNamingPolicy.CamelCase.ConvertName(nameof(ApplicationData.Settings.MainWindowRectangle.Width)), ApplicationData.Settings.MainWindowRectangle.Width);
    //                ApplicationData.Settings.MainWindowRectangle.Height = FileProcessing.GetIntJsonElement(element1, JsonNamingPolicy.CamelCase.ConvertName(nameof(ApplicationData.Settings.MainWindowRectangle.Height)), ApplicationData.Settings.MainWindowRectangle.Height);
    //            }
    //            _ = Enum.TryParse(FileProcessing.GetStringJsonElement(rootElement, JsonNamingPolicy.CamelCase.ConvertName(nameof(ApplicationData.Settings.WindowStateMainWindow)), Enum.GetName(typeof(WindowState), ApplicationData.Settings.WindowStateMainWindow) ?? ""), out WindowState getWindowState);
    //            ApplicationData.Settings.WindowStateMainWindow = getWindowState;
    //            ApplicationData.Settings.Language = FileProcessing.GetStringJsonElement(rootElement, JsonNamingPolicy.CamelCase.ConvertName(nameof(ApplicationData.Settings.Language)), ApplicationData.Settings.Language);
    //            _ = Enum.TryParse(FileProcessing.GetStringJsonElement(rootElement, JsonNamingPolicy.CamelCase.ConvertName(nameof(ApplicationData.Settings.CoordinateType)), Enum.GetName(typeof(CoordinateType), ApplicationData.Settings.CoordinateType) ?? ""), out CoordinateType getCoordinateType);
    //            ApplicationData.Settings.CoordinateType = getCoordinateType;
    //            ApplicationData.Settings.DarkMode = FileProcessing.GetBoolJsonElement(rootElement, JsonNamingPolicy.CamelCase.ConvertName(nameof(ApplicationData.Settings.DarkMode)), ApplicationData.Settings.DarkMode);
    //            ApplicationData.Settings.AutomaticallyUpdateCheck = FileProcessing.GetBoolJsonElement(rootElement, JsonNamingPolicy.CamelCase.ConvertName(nameof(ApplicationData.Settings.AutomaticallyUpdateCheck)), ApplicationData.Settings.AutomaticallyUpdateCheck);
    //            ApplicationData.Settings.CheckBetaVersion = FileProcessing.GetBoolJsonElement(rootElement, JsonNamingPolicy.CamelCase.ConvertName(nameof(ApplicationData.Settings.CheckBetaVersion)), ApplicationData.Settings.CheckBetaVersion);
    //            ApplicationData.Settings.UseLongPath = FileProcessing.GetBoolJsonElement(rootElement, JsonNamingPolicy.CamelCase.ConvertName(nameof(ApplicationData.Settings.UseLongPath)), ApplicationData.Settings.UseLongPath);
    //            if (rootElement.TryGetProperty(JsonNamingPolicy.CamelCase.ConvertName(nameof(ApplicationData.Settings.ShiftPastePosition)), out element1))
    //            {
    //                ApplicationData.Settings.ShiftPastePosition.Enabled = FileProcessing.GetBoolJsonElement(element1, JsonNamingPolicy.CamelCase.ConvertName(nameof(ApplicationData.Settings.ShiftPastePosition.Enabled)), ApplicationData.Settings.ShiftPastePosition.Enabled);
    //                ApplicationData.Settings.ShiftPastePosition.Left = FileProcessing.GetIntJsonElement(element1, JsonNamingPolicy.CamelCase.ConvertName(nameof(ApplicationData.Settings.ShiftPastePosition.Left)), ApplicationData.Settings.ShiftPastePosition.Left);
    //                ApplicationData.Settings.ShiftPastePosition.Top = FileProcessing.GetIntJsonElement(element1, JsonNamingPolicy.CamelCase.ConvertName(nameof(ApplicationData.Settings.ShiftPastePosition.Top)), ApplicationData.Settings.ShiftPastePosition.Top);
    //                ApplicationData.Settings.ShiftPastePosition.Right = FileProcessing.GetIntJsonElement(element1, JsonNamingPolicy.CamelCase.ConvertName(nameof(ApplicationData.Settings.ShiftPastePosition.Right)), ApplicationData.Settings.ShiftPastePosition.Right);
    //                ApplicationData.Settings.ShiftPastePosition.Bottom = FileProcessing.GetIntJsonElement(element1, JsonNamingPolicy.CamelCase.ConvertName(nameof(ApplicationData.Settings.ShiftPastePosition.Bottom)), ApplicationData.Settings.ShiftPastePosition.Bottom);
    //            }
    //            if (rootElement.TryGetProperty(JsonNamingPolicy.CamelCase.ConvertName(nameof(ApplicationData.Settings.SpecifyWindowInformation)), out element1))
    //            {
    //                ApplicationData.Settings.SpecifyWindowInformation.Enabled = FileProcessing.GetBoolJsonElement(element1, JsonNamingPolicy.CamelCase.ConvertName(nameof(ApplicationData.Settings.SpecifyWindowInformation.Enabled)), ApplicationData.Settings.SpecifyWindowInformation.Enabled);
    //                ApplicationData.Settings.SpecifyWindowInformation.MultipleRegistrations = FileProcessing.GetBoolJsonElement(element1, JsonNamingPolicy.CamelCase.ConvertName(nameof(ApplicationData.Settings.SpecifyWindowInformation.MultipleRegistrations)), ApplicationData.Settings.SpecifyWindowInformation.MultipleRegistrations);
    //                ApplicationData.Settings.SpecifyWindowInformation.CaseSensitive = FileProcessing.GetBoolJsonElement(element1, JsonNamingPolicy.CamelCase.ConvertName(nameof(ApplicationData.Settings.SpecifyWindowInformation.CaseSensitive)), ApplicationData.Settings.SpecifyWindowInformation.CaseSensitive);
    //                ApplicationData.Settings.SpecifyWindowInformation.DoNotChangeOutOfScreen = FileProcessing.GetBoolJsonElement(element1, JsonNamingPolicy.CamelCase.ConvertName(nameof(ApplicationData.Settings.SpecifyWindowInformation.DoNotChangeOutOfScreen)), ApplicationData.Settings.SpecifyWindowInformation.DoNotChangeOutOfScreen);
    //                ApplicationData.Settings.SpecifyWindowInformation.StopProcessingShowAddModifyWindow = FileProcessing.GetBoolJsonElement(element1, JsonNamingPolicy.CamelCase.ConvertName(nameof(ApplicationData.Settings.SpecifyWindowInformation.StopProcessingShowAddModifyWindow)), ApplicationData.Settings.SpecifyWindowInformation.StopProcessingShowAddModifyWindow);
    //                ApplicationData.Settings.SpecifyWindowInformation.StopProcessingFullScreen = FileProcessing.GetBoolJsonElement(element1, JsonNamingPolicy.CamelCase.ConvertName(nameof(ApplicationData.Settings.SpecifyWindowInformation.StopProcessingFullScreen)), ApplicationData.Settings.SpecifyWindowInformation.StopProcessingFullScreen);
    //                ApplicationData.Settings.SpecifyWindowInformation.HotkeysDoNotStopFullScreen = FileProcessing.GetBoolJsonElement(element1, JsonNamingPolicy.CamelCase.ConvertName(nameof(ApplicationData.Settings.SpecifyWindowInformation.HotkeysDoNotStopFullScreen)), ApplicationData.Settings.SpecifyWindowInformation.HotkeysDoNotStopFullScreen);
    //                ApplicationData.Settings.SpecifyWindowInformation.ProcessingInterval = FileProcessing.GetIntJsonElement(element1, JsonNamingPolicy.CamelCase.ConvertName(nameof(ApplicationData.Settings.SpecifyWindowInformation.ProcessingInterval)), ApplicationData.Settings.SpecifyWindowInformation.ProcessingInterval);
    //                _ = Enum.TryParse(FileProcessing.GetStringJsonElement(element1, JsonNamingPolicy.CamelCase.ConvertName(nameof(ApplicationData.Settings.SpecifyWindowInformation.ProcessingWindowRange)), Enum.GetName(typeof(ProcessingWindowRange), ApplicationData.Settings.SpecifyWindowInformation.ProcessingWindowRange) ?? ""), out ProcessingWindowRange getProcessingWindowRange);
    //                ApplicationData.Settings.SpecifyWindowInformation.ProcessingWindowRange = getProcessingWindowRange;
    //                ApplicationData.Settings.SpecifyWindowInformation.WaitTimeToProcessingNextWindow = FileProcessing.GetIntJsonElement(element1, JsonNamingPolicy.CamelCase.ConvertName(nameof(ApplicationData.Settings.SpecifyWindowInformation.WaitTimeToProcessingNextWindow)), ApplicationData.Settings.SpecifyWindowInformation.WaitTimeToProcessingNextWindow);
    //                if (element1.TryGetProperty(JsonNamingPolicy.CamelCase.ConvertName(nameof(ApplicationData.Settings.SpecifyWindowInformation.AddModifyWindowSize)), out element2))
    //                {
    //                    ApplicationData.Settings.SpecifyWindowInformation.AddModifyWindowSize.Width = FileProcessing.GetIntJsonElement(element2, JsonNamingPolicy.CamelCase.ConvertName(nameof(ApplicationData.Settings.SpecifyWindowInformation.AddModifyWindowSize.Width)), ApplicationData.Settings.SpecifyWindowInformation.AddModifyWindowSize.Width);
    //                    ApplicationData.Settings.SpecifyWindowInformation.AddModifyWindowSize.Height = FileProcessing.GetIntJsonElement(element2, JsonNamingPolicy.CamelCase.ConvertName(nameof(ApplicationData.Settings.SpecifyWindowInformation.AddModifyWindowSize.Height)), ApplicationData.Settings.SpecifyWindowInformation.AddModifyWindowSize.Height);
    //                }
    //                if (element1.TryGetProperty(JsonNamingPolicy.CamelCase.ConvertName(nameof(ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems)), out element2))
    //                {
    //                    ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.TitleName = FileProcessing.GetBoolJsonElement(element2, JsonNamingPolicy.CamelCase.ConvertName(nameof(ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.TitleName)), ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.TitleName);
    //                    ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.ClassName = FileProcessing.GetBoolJsonElement(element2, JsonNamingPolicy.CamelCase.ConvertName(nameof(ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.ClassName)), ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.ClassName);
    //                    ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.FileName = FileProcessing.GetBoolJsonElement(element2, JsonNamingPolicy.CamelCase.ConvertName(nameof(ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.FileName)), ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.FileName);
    //                    ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.Display = FileProcessing.GetBoolJsonElement(element2, JsonNamingPolicy.CamelCase.ConvertName(nameof(ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.Display)), ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.Display);
    //                    ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.WindowState = FileProcessing.GetBoolJsonElement(element2, JsonNamingPolicy.CamelCase.ConvertName(nameof(ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.WindowState)), ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.WindowState);
    //                    ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.X = FileProcessing.GetBoolJsonElement(element2, JsonNamingPolicy.CamelCase.ConvertName(nameof(ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.X)), ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.X);
    //                    ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.Y = FileProcessing.GetBoolJsonElement(element2, JsonNamingPolicy.CamelCase.ConvertName(nameof(ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.Y)), ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.Y);
    //                    ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.Width = FileProcessing.GetBoolJsonElement(element2, JsonNamingPolicy.CamelCase.ConvertName(nameof(ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.Width)), ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.Width);
    //                    ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.Height = FileProcessing.GetBoolJsonElement(element2, JsonNamingPolicy.CamelCase.ConvertName(nameof(ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.Height)), ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.Height);
    //                    ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.Version = FileProcessing.GetBoolJsonElement(element2, JsonNamingPolicy.CamelCase.ConvertName(nameof(ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.Version)), ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.Version);
    //                }
    //                if (element1.TryGetProperty(JsonNamingPolicy.CamelCase.ConvertName(nameof(ApplicationData.Settings.SpecifyWindowInformation.Items)), out element2))
    //                {
    //                    foreach (var item1 in element2.EnumerateArray())
    //                    {
    //                        SpecifyWindowItemInformation newEII = new();

    //                        newEII.Enabled = FileProcessing.GetBoolJsonElement(item1, JsonNamingPolicy.CamelCase.ConvertName(nameof(newEII.Enabled)), newEII.Enabled);
    //                        newEII.RegisteredName = FileProcessing.GetStringJsonElement(item1, JsonNamingPolicy.CamelCase.ConvertName(nameof(newEII.RegisteredName)), newEII.RegisteredName);
    //                        newEII.TitleName = FileProcessing.GetStringJsonElement(item1, JsonNamingPolicy.CamelCase.ConvertName(nameof(newEII.TitleName)), newEII.TitleName);
    //                        _ = Enum.TryParse(FileProcessing.GetStringJsonElement(item1, JsonNamingPolicy.CamelCase.ConvertName(nameof(newEII.TitleNameMatchCondition)), Enum.GetName(typeof(NameMatchCondition), newEII.TitleNameMatchCondition) ?? ""), out NameMatchCondition getNameMatchCondition);
    //                        newEII.TitleNameMatchCondition = getNameMatchCondition;
    //                        newEII.ClassName = FileProcessing.GetStringJsonElement(item1, JsonNamingPolicy.CamelCase.ConvertName(nameof(newEII.ClassName)), newEII.ClassName);
    //                        _ = Enum.TryParse(FileProcessing.GetStringJsonElement(item1, JsonNamingPolicy.CamelCase.ConvertName(nameof(newEII.ClassNameMatchCondition)), Enum.GetName(typeof(NameMatchCondition), newEII.ClassNameMatchCondition) ?? ""), out getNameMatchCondition);
    //                        newEII.ClassNameMatchCondition = getNameMatchCondition;
    //                        newEII.FileName = FileProcessing.GetStringJsonElement(item1, JsonNamingPolicy.CamelCase.ConvertName(nameof(newEII.FileName)), newEII.FileName);
    //                        _ = Enum.TryParse(FileProcessing.GetStringJsonElement(item1, JsonNamingPolicy.CamelCase.ConvertName(nameof(newEII.FileNameMatchCondition)), Enum.GetName(typeof(FileNameMatchCondition), newEII.FileNameMatchCondition) ?? ""), out FileNameMatchCondition getFileNameMatchCondition);
    //                        newEII.FileNameMatchCondition = getFileNameMatchCondition;
    //                        _ = Enum.TryParse(FileProcessing.GetStringJsonElement(item1, JsonNamingPolicy.CamelCase.ConvertName(nameof(newEII.StandardDisplay)), Enum.GetName(typeof(StandardDisplay), newEII.StandardDisplay) ?? ""), out StandardDisplay getStandardDisplay);
    //                        newEII.StandardDisplay = getStandardDisplay;
    //                        _ = Enum.TryParse(FileProcessing.GetStringJsonElement(item1, JsonNamingPolicy.CamelCase.ConvertName(nameof(newEII.ProcessingOnlyOnce)), Enum.GetName(typeof(ProcessingOnlyOnce), newEII.ProcessingOnlyOnce) ?? ""), out ProcessingOnlyOnce getProcessingOnlyOnce);
    //                        newEII.ProcessingOnlyOnce = getProcessingOnlyOnce;
    //                        if (item1.TryGetProperty(JsonNamingPolicy.CamelCase.ConvertName(nameof(newEII.WindowEventData)), out element3))
    //                        {
    //                            newEII.WindowEventData.Foreground = FileProcessing.GetBoolJsonElement(element3, JsonNamingPolicy.CamelCase.ConvertName(nameof(newEII.WindowEventData.Foreground)), newEII.WindowEventData.Foreground);
    //                            newEII.WindowEventData.MoveSizeEnd = FileProcessing.GetBoolJsonElement(element3, JsonNamingPolicy.CamelCase.ConvertName(nameof(newEII.WindowEventData.MoveSizeEnd)), newEII.WindowEventData.MoveSizeEnd);
    //                            newEII.WindowEventData.MinimizeStart = FileProcessing.GetBoolJsonElement(element3, JsonNamingPolicy.CamelCase.ConvertName(nameof(newEII.WindowEventData.MinimizeStart)), newEII.WindowEventData.MinimizeStart);
    //                            newEII.WindowEventData.MinimizeEnd = FileProcessing.GetBoolJsonElement(element3, JsonNamingPolicy.CamelCase.ConvertName(nameof(newEII.WindowEventData.MinimizeEnd)), newEII.WindowEventData.MinimizeEnd);
    //                            newEII.WindowEventData.Show = FileProcessing.GetBoolJsonElement(element3, JsonNamingPolicy.CamelCase.ConvertName(nameof(newEII.WindowEventData.Show)), newEII.WindowEventData.Show);
    //                            newEII.WindowEventData.NameChange = FileProcessing.GetBoolJsonElement(element3, JsonNamingPolicy.CamelCase.ConvertName(nameof(newEII.WindowEventData.NameChange)), newEII.WindowEventData.NameChange);
    //                            newEII.WindowEventData.DelayTime = FileProcessing.GetIntJsonElement(element3, JsonNamingPolicy.CamelCase.ConvertName(nameof(newEII.WindowEventData.DelayTime)), newEII.WindowEventData.DelayTime);
    //                        }
    //                        newEII.TimerProcessing = FileProcessing.GetBoolJsonElement(item1, JsonNamingPolicy.CamelCase.ConvertName(nameof(newEII.TimerProcessing)), newEII.TimerProcessing);
    //                        newEII.NumberOfTimesNotToProcessingFirst = FileProcessing.GetIntJsonElement(item1, JsonNamingPolicy.CamelCase.ConvertName(nameof(newEII.NumberOfTimesNotToProcessingFirst)), newEII.NumberOfTimesNotToProcessingFirst);
    //                        if (item1.TryGetProperty(JsonNamingPolicy.CamelCase.ConvertName(nameof(newEII.WindowProcessingInformation)), out element3))
    //                        {
    //                            foreach (var item2 in element3.EnumerateArray())
    //                            {
    //                                WindowProcessingInformation newWPI = new();

    //                                newWPI.Active = FileProcessing.GetBoolJsonElement(item2, JsonNamingPolicy.CamelCase.ConvertName(nameof(newWPI.Active)), newWPI.Active);
    //                                newWPI.ProcessingName = FileProcessing.GetStringJsonElement(item2, JsonNamingPolicy.CamelCase.ConvertName(nameof(newWPI.ProcessingName)), newWPI.ProcessingName);
    //                                if (item2.TryGetProperty(JsonNamingPolicy.CamelCase.ConvertName(nameof(newWPI.PositionSize)), out element4))
    //                                {
    //                                    newWPI.PositionSize.Display = FileProcessing.GetStringJsonElement(element4, JsonNamingPolicy.CamelCase.ConvertName(nameof(newWPI.PositionSize.Display)), newWPI.PositionSize.Display);
    //                                    _ = Enum.TryParse(FileProcessing.GetStringJsonElement(element4, JsonNamingPolicy.CamelCase.ConvertName(nameof(newWPI.PositionSize.SettingsWindowState)), Enum.GetName(typeof(SettingsWindowState), newWPI.PositionSize.SettingsWindowState) ?? ""), out SettingsWindowState getSettingsWindowState);
    //                                    newWPI.PositionSize.SettingsWindowState = getSettingsWindowState;
    //                                    newWPI.PositionSize.X = FileProcessing.GetDoubleJsonElement(element4, JsonNamingPolicy.CamelCase.ConvertName(nameof(newWPI.PositionSize.X)), newWPI.PositionSize.X);
    //                                    newWPI.PositionSize.Y = FileProcessing.GetDoubleJsonElement(element4, JsonNamingPolicy.CamelCase.ConvertName(nameof(newWPI.PositionSize.Y)), newWPI.PositionSize.Y);
    //                                    newWPI.PositionSize.Width = FileProcessing.GetDoubleJsonElement(element4, JsonNamingPolicy.CamelCase.ConvertName(nameof(newWPI.PositionSize.Width)), newWPI.PositionSize.Width);
    //                                    newWPI.PositionSize.Height = FileProcessing.GetDoubleJsonElement(element4, JsonNamingPolicy.CamelCase.ConvertName(nameof(newWPI.PositionSize.Height)), newWPI.PositionSize.Height);
    //                                    _ = Enum.TryParse(FileProcessing.GetStringJsonElement(element4, JsonNamingPolicy.CamelCase.ConvertName(nameof(newWPI.PositionSize.XType)), Enum.GetName(typeof(WindowXType), newWPI.PositionSize.XType) ?? ""), out WindowXType getWindowXType);
    //                                    newWPI.PositionSize.XType = getWindowXType;
    //                                    _ = Enum.TryParse(FileProcessing.GetStringJsonElement(element4, JsonNamingPolicy.CamelCase.ConvertName(nameof(newWPI.PositionSize.YType)), Enum.GetName(typeof(WindowYType), newWPI.PositionSize.YType) ?? ""), out WindowYType getWindowYType);
    //                                    newWPI.PositionSize.YType = getWindowYType;
    //                                    _ = Enum.TryParse(FileProcessing.GetStringJsonElement(element4, JsonNamingPolicy.CamelCase.ConvertName(nameof(newWPI.PositionSize.WidthType)), Enum.GetName(typeof(WindowSizeType), newWPI.PositionSize.WidthType) ?? ""), out WindowSizeType getWindowSizeType);
    //                                    newWPI.PositionSize.WidthType = getWindowSizeType;
    //                                    _ = Enum.TryParse(FileProcessing.GetStringJsonElement(element4, JsonNamingPolicy.CamelCase.ConvertName(nameof(newWPI.PositionSize.HeightType)), Enum.GetName(typeof(WindowSizeType), newWPI.PositionSize.HeightType) ?? ""), out getWindowSizeType);
    //                                    newWPI.PositionSize.HeightType = getWindowSizeType;
    //                                    _ = Enum.TryParse(FileProcessing.GetStringJsonElement(element4, JsonNamingPolicy.CamelCase.ConvertName(nameof(newWPI.PositionSize.XValueType)), Enum.GetName(typeof(PositionSizeValueType), newWPI.PositionSize.XValueType) ?? ""), out PositionSizeValueType getPositionSizeValueType);
    //                                    newWPI.PositionSize.XValueType = getPositionSizeValueType;
    //                                    _ = Enum.TryParse(FileProcessing.GetStringJsonElement(element4, JsonNamingPolicy.CamelCase.ConvertName(nameof(newWPI.PositionSize.YValueType)), Enum.GetName(typeof(PositionSizeValueType), newWPI.PositionSize.YValueType) ?? ""), out getPositionSizeValueType);
    //                                    newWPI.PositionSize.YValueType = getPositionSizeValueType;
    //                                    _ = Enum.TryParse(FileProcessing.GetStringJsonElement(element4, JsonNamingPolicy.CamelCase.ConvertName(nameof(newWPI.PositionSize.WidthValueType)), Enum.GetName(typeof(PositionSizeValueType), newWPI.PositionSize.WidthValueType) ?? ""), out getPositionSizeValueType);
    //                                    newWPI.PositionSize.WidthValueType = getPositionSizeValueType;
    //                                    _ = Enum.TryParse(FileProcessing.GetStringJsonElement(element4, JsonNamingPolicy.CamelCase.ConvertName(nameof(newWPI.PositionSize.HeightValueType)), Enum.GetName(typeof(PositionSizeValueType), newWPI.PositionSize.HeightValueType) ?? ""), out getPositionSizeValueType);
    //                                    newWPI.PositionSize.HeightValueType = getPositionSizeValueType;
    //                                    newWPI.PositionSize.ClientArea = FileProcessing.GetBoolJsonElement(element4, JsonNamingPolicy.CamelCase.ConvertName(nameof(newWPI.PositionSize.ClientArea)), newWPI.PositionSize.ClientArea);
    //                                }
    //                                newWPI.NormalWindowOnly = FileProcessing.GetBoolJsonElement(item2, JsonNamingPolicy.CamelCase.ConvertName(nameof(newWPI.NormalWindowOnly)), newWPI.NormalWindowOnly);
    //                                _ = Enum.TryParse(FileProcessing.GetStringJsonElement(item2, JsonNamingPolicy.CamelCase.ConvertName(nameof(newWPI.Forefront)), Enum.GetName(typeof(Forefront), newWPI.Forefront) ?? ""), out Forefront getForefront);
    //                                newWPI.Forefront = getForefront;
    //                                newWPI.EnabledTransparency = FileProcessing.GetBoolJsonElement(item2, JsonNamingPolicy.CamelCase.ConvertName(nameof(newWPI.EnabledTransparency)), newWPI.EnabledTransparency);
    //                                newWPI.Transparency = FileProcessing.GetIntJsonElement(item2, JsonNamingPolicy.CamelCase.ConvertName(nameof(newWPI.Transparency)), newWPI.Transparency);
    //                                newWPI.CloseWindow = FileProcessing.GetBoolJsonElement(item2, JsonNamingPolicy.CamelCase.ConvertName(nameof(newWPI.CloseWindow)), newWPI.CloseWindow);
    //                                if (item2.TryGetProperty(JsonNamingPolicy.CamelCase.ConvertName(nameof(newWPI.Hotkey)), out element4))
    //                                {
    //                                    newWPI.Hotkey.Alt = FileProcessing.GetBoolJsonElement(element4, JsonNamingPolicy.CamelCase.ConvertName(nameof(newWPI.Hotkey.Alt)), newWPI.Hotkey.Alt);
    //                                    newWPI.Hotkey.Ctrl = FileProcessing.GetBoolJsonElement(element4, JsonNamingPolicy.CamelCase.ConvertName(nameof(newWPI.Hotkey.Ctrl)), newWPI.Hotkey.Ctrl);
    //                                    newWPI.Hotkey.Shift = FileProcessing.GetBoolJsonElement(element4, JsonNamingPolicy.CamelCase.ConvertName(nameof(newWPI.Hotkey.Shift)), newWPI.Hotkey.Shift);
    //                                    newWPI.Hotkey.Windows = FileProcessing.GetBoolJsonElement(element4, JsonNamingPolicy.CamelCase.ConvertName(nameof(newWPI.Hotkey.Windows)), newWPI.Hotkey.Windows);
    //                                    newWPI.Hotkey.KeyCharacter = FileProcessing.GetIntJsonElement(element4, JsonNamingPolicy.CamelCase.ConvertName(nameof(newWPI.Hotkey.KeyCharacter)), newWPI.Hotkey.KeyCharacter);
    //                                }

    //                                newEII.WindowProcessingInformation.Add(newWPI);
    //                            }
    //                        }
    //                        newEII.DoNotProcessingChildWindow = FileProcessing.GetBoolJsonElement(item1, JsonNamingPolicy.CamelCase.ConvertName(nameof(newEII.DoNotProcessingChildWindow)), newEII.DoNotProcessingChildWindow);
    //                        _ = Enum.TryParse(FileProcessing.GetStringJsonElement(item1, JsonNamingPolicy.CamelCase.ConvertName(nameof(newEII.DoNotProcessingTitleNameConditions)), Enum.GetName(typeof(TitleNameProcessingConditions), newEII.DoNotProcessingTitleNameConditions) ?? ""), out TitleNameProcessingConditions getTitleNameProcessingConditions);
    //                        newEII.DoNotProcessingTitleNameConditions = getTitleNameProcessingConditions;
    //                        if (item1.TryGetProperty(JsonNamingPolicy.CamelCase.ConvertName(nameof(newEII.DoNotProcessingStringContainedInTitleName)), out element3))
    //                        {
    //                            foreach (var item2 in element3.EnumerateArray())
    //                            {
    //                                newEII.DoNotProcessingStringContainedInTitleName.Add(item2.GetString() ?? "");
    //                            }
    //                        }
    //                        if (item1.TryGetProperty(JsonNamingPolicy.CamelCase.ConvertName(nameof(newEII.DoNotProcessingSize)), out element3))
    //                        {
    //                            foreach (var item2 in element3.EnumerateArray())
    //                            {
    //                                SizeInt newSize = new();
    //                                newSize.Width = FileProcessing.GetIntJsonElement(item2, JsonNamingPolicy.CamelCase.ConvertName(nameof(newSize.Width)), newSize.Width);
    //                                newSize.Height = FileProcessing.GetIntJsonElement(item2, JsonNamingPolicy.CamelCase.ConvertName(nameof(newSize.Height)), newSize.Height);
    //                                newEII.DoNotProcessingSize.Add(newSize);
    //                            }
    //                        }
    //                        newEII.DoNotProcessingOtherThanSpecifiedVersion = FileProcessing.GetStringJsonElement(item1, JsonNamingPolicy.CamelCase.ConvertName(nameof(newEII.DoNotProcessingOtherThanSpecifiedVersion)), newEII.DoNotProcessingOtherThanSpecifiedVersion);
    //                        newEII.DoNotProcessingOtherThanSpecifiedVersionAnnounce = FileProcessing.GetBoolJsonElement(item1, JsonNamingPolicy.CamelCase.ConvertName(nameof(newEII.DoNotProcessingOtherThanSpecifiedVersionAnnounce)), newEII.DoNotProcessingOtherThanSpecifiedVersionAnnounce);

    //                        ApplicationData.Settings.SpecifyWindowInformation.Items.Add(newEII);
    //                    }
    //                }
    //            }
    //            if (rootElement.TryGetProperty(JsonNamingPolicy.CamelCase.ConvertName(nameof(ApplicationData.Settings.AllWindowInformation)), out element1))
    //            {
    //                ApplicationData.Settings.AllWindowInformation.Enabled = FileProcessing.GetBoolJsonElement(element1, JsonNamingPolicy.CamelCase.ConvertName(nameof(ApplicationData.Settings.AllWindowInformation.Enabled)), ApplicationData.Settings.AllWindowInformation.Enabled);
    //                ApplicationData.Settings.AllWindowInformation.CaseSensitive = FileProcessing.GetBoolJsonElement(element1, JsonNamingPolicy.CamelCase.ConvertName(nameof(ApplicationData.Settings.AllWindowInformation.CaseSensitive)), ApplicationData.Settings.AllWindowInformation.CaseSensitive);
    //                ApplicationData.Settings.AllWindowInformation.StopProcessingFullScreen = FileProcessing.GetBoolJsonElement(element1, JsonNamingPolicy.CamelCase.ConvertName(nameof(ApplicationData.Settings.AllWindowInformation.StopProcessingFullScreen)), ApplicationData.Settings.AllWindowInformation.StopProcessingFullScreen);
    //                _ = Enum.TryParse(FileProcessing.GetStringJsonElement(element1, JsonNamingPolicy.CamelCase.ConvertName(nameof(ApplicationData.Settings.AllWindowInformation.StandardDisplay)), Enum.GetName(typeof(StandardDisplay), ApplicationData.Settings.AllWindowInformation.StandardDisplay) ?? ""), out StandardDisplay getStandardDisplay);
    //                ApplicationData.Settings.AllWindowInformation.StandardDisplay = getStandardDisplay;
    //                if (element1.TryGetProperty(JsonNamingPolicy.CamelCase.ConvertName(nameof(ApplicationData.Settings.AllWindowInformation.PositionSize)), out element2))
    //                {
    //                    ApplicationData.Settings.AllWindowInformation.PositionSize.Display = FileProcessing.GetStringJsonElement(element2, JsonNamingPolicy.CamelCase.ConvertName(nameof(ApplicationData.Settings.AllWindowInformation.PositionSize.Display)), ApplicationData.Settings.AllWindowInformation.PositionSize.Display);
    //                    ApplicationData.Settings.AllWindowInformation.PositionSize.X = FileProcessing.GetDoubleJsonElement(element2, JsonNamingPolicy.CamelCase.ConvertName(nameof(ApplicationData.Settings.AllWindowInformation.PositionSize.X)), ApplicationData.Settings.AllWindowInformation.PositionSize.X);
    //                    ApplicationData.Settings.AllWindowInformation.PositionSize.Y = FileProcessing.GetDoubleJsonElement(element2, JsonNamingPolicy.CamelCase.ConvertName(nameof(ApplicationData.Settings.AllWindowInformation.PositionSize.Y)), ApplicationData.Settings.AllWindowInformation.PositionSize.Y);
    //                    _ = Enum.TryParse(FileProcessing.GetStringJsonElement(element2, JsonNamingPolicy.CamelCase.ConvertName(nameof(ApplicationData.Settings.AllWindowInformation.PositionSize.XType)), Enum.GetName(typeof(WindowXType), ApplicationData.Settings.AllWindowInformation.PositionSize.XType) ?? ""), out WindowXType getWindowXType);
    //                    ApplicationData.Settings.AllWindowInformation.PositionSize.XType = getWindowXType;
    //                    _ = Enum.TryParse(FileProcessing.GetStringJsonElement(element2, JsonNamingPolicy.CamelCase.ConvertName(nameof(ApplicationData.Settings.AllWindowInformation.PositionSize.YType)), Enum.GetName(typeof(WindowYType), ApplicationData.Settings.AllWindowInformation.PositionSize.YType) ?? ""), out WindowYType getWindowYType);
    //                    ApplicationData.Settings.AllWindowInformation.PositionSize.YType = getWindowYType;
    //                    _ = Enum.TryParse(FileProcessing.GetStringJsonElement(element2, JsonNamingPolicy.CamelCase.ConvertName(nameof(ApplicationData.Settings.AllWindowInformation.PositionSize.XValueType)), Enum.GetName(typeof(PositionSizeValueType), ApplicationData.Settings.AllWindowInformation.PositionSize.XValueType) ?? ""), out PositionSizeValueType getPositionSizeValueType);
    //                    ApplicationData.Settings.AllWindowInformation.PositionSize.XValueType = getPositionSizeValueType;
    //                    _ = Enum.TryParse(FileProcessing.GetStringJsonElement(element2, JsonNamingPolicy.CamelCase.ConvertName(nameof(ApplicationData.Settings.AllWindowInformation.PositionSize.YValueType)), Enum.GetName(typeof(PositionSizeValueType), ApplicationData.Settings.AllWindowInformation.PositionSize.YValueType) ?? ""), out getPositionSizeValueType);
    //                    ApplicationData.Settings.AllWindowInformation.PositionSize.YValueType = getPositionSizeValueType;
    //                }
    //                if (element1.TryGetProperty(JsonNamingPolicy.CamelCase.ConvertName(nameof(ApplicationData.Settings.AllWindowInformation.AddModifyWindowSize)), out element2))
    //                {
    //                    ApplicationData.Settings.AllWindowInformation.AddModifyWindowSize.Width = FileProcessing.GetIntJsonElement(element2, JsonNamingPolicy.CamelCase.ConvertName(nameof(ApplicationData.Settings.AllWindowInformation.AddModifyWindowSize.Width)), ApplicationData.Settings.AllWindowInformation.AddModifyWindowSize.Width);
    //                    ApplicationData.Settings.AllWindowInformation.AddModifyWindowSize.Height = FileProcessing.GetIntJsonElement(element2, JsonNamingPolicy.CamelCase.ConvertName(nameof(ApplicationData.Settings.AllWindowInformation.AddModifyWindowSize.Height)), ApplicationData.Settings.AllWindowInformation.AddModifyWindowSize.Height);
    //                }
    //                if (element1.TryGetProperty(JsonNamingPolicy.CamelCase.ConvertName(nameof(ApplicationData.Settings.AllWindowInformation.WindowEvent)), out element2))
    //                {
    //                    ApplicationData.Settings.AllWindowInformation.WindowEvent.MoveSizeEnd = FileProcessing.GetBoolJsonElement(element2, JsonNamingPolicy.CamelCase.ConvertName(nameof(ApplicationData.Settings.AllWindowInformation.WindowEvent.MoveSizeEnd)), ApplicationData.Settings.AllWindowInformation.WindowEvent.MoveSizeEnd);
    //                    ApplicationData.Settings.AllWindowInformation.WindowEvent.Show = FileProcessing.GetBoolJsonElement(element2, JsonNamingPolicy.CamelCase.ConvertName(nameof(ApplicationData.Settings.AllWindowInformation.WindowEvent.Show)), ApplicationData.Settings.AllWindowInformation.WindowEvent.Show);
    //                    ApplicationData.Settings.AllWindowInformation.WindowEvent.DelayTime = FileProcessing.GetIntJsonElement(element2, JsonNamingPolicy.CamelCase.ConvertName(nameof(ApplicationData.Settings.AllWindowInformation.WindowEvent.DelayTime)), ApplicationData.Settings.AllWindowInformation.WindowEvent.DelayTime);
    //                }
    //                if (element1.TryGetProperty(JsonNamingPolicy.CamelCase.ConvertName(nameof(ApplicationData.Settings.AllWindowInformation.Items)), out element2))
    //                {
    //                    foreach (var item1 in element2.EnumerateArray())
    //                    {
    //                        WindowJudgementInformation newWJI = new();

    //                        newWJI.RegisteredName = FileProcessing.GetStringJsonElement(item1, JsonNamingPolicy.CamelCase.ConvertName(nameof(newWJI.RegisteredName)), newWJI.RegisteredName);
    //                        newWJI.TitleName = FileProcessing.GetStringJsonElement(item1, JsonNamingPolicy.CamelCase.ConvertName(nameof(newWJI.TitleName)), newWJI.TitleName);
    //                        _ = Enum.TryParse(FileProcessing.GetStringJsonElement(item1, JsonNamingPolicy.CamelCase.ConvertName(nameof(newWJI.TitleNameMatchCondition)), Enum.GetName(typeof(NameMatchCondition), newWJI.TitleNameMatchCondition) ?? ""), out NameMatchCondition getNameMatchCondition);
    //                        newWJI.TitleNameMatchCondition = getNameMatchCondition;
    //                        newWJI.ClassName = FileProcessing.GetStringJsonElement(item1, JsonNamingPolicy.CamelCase.ConvertName(nameof(newWJI.ClassName)), newWJI.ClassName);
    //                        _ = Enum.TryParse(FileProcessing.GetStringJsonElement(item1, JsonNamingPolicy.CamelCase.ConvertName(nameof(newWJI.ClassNameMatchCondition)), Enum.GetName(typeof(NameMatchCondition), newWJI.ClassNameMatchCondition) ?? ""), out getNameMatchCondition);
    //                        newWJI.ClassNameMatchCondition = getNameMatchCondition;
    //                        newWJI.FileName = FileProcessing.GetStringJsonElement(item1, JsonNamingPolicy.CamelCase.ConvertName(nameof(newWJI.FileName)), newWJI.FileName);
    //                        _ = Enum.TryParse(FileProcessing.GetStringJsonElement(item1, JsonNamingPolicy.CamelCase.ConvertName(nameof(newWJI.FileNameMatchCondition)), Enum.GetName(typeof(FileNameMatchCondition), newWJI.FileNameMatchCondition) ?? ""), out FileNameMatchCondition getFileNameMatchCondition);
    //                        newWJI.FileNameMatchCondition = getFileNameMatchCondition;

    //                        ApplicationData.Settings.AllWindowInformation.Items.Add(newWJI);
    //                    }
    //                }
    //            }
    //            if (rootElement.TryGetProperty(JsonNamingPolicy.CamelCase.ConvertName(nameof(ApplicationData.Settings.MagnetInformation)), out element1))
    //            {
    //                ApplicationData.Settings.MagnetInformation.Enabled = FileProcessing.GetBoolJsonElement(element1, JsonNamingPolicy.CamelCase.ConvertName(nameof(ApplicationData.Settings.MagnetInformation.Enabled)), ApplicationData.Settings.MagnetInformation.Enabled);
    //                ApplicationData.Settings.MagnetInformation.PasteToEdgeOfScreen = FileProcessing.GetBoolJsonElement(element1, JsonNamingPolicy.CamelCase.ConvertName(nameof(ApplicationData.Settings.MagnetInformation.PasteToEdgeOfScreen)), ApplicationData.Settings.MagnetInformation.PasteToEdgeOfScreen);
    //                ApplicationData.Settings.MagnetInformation.PasteToAnotherWindow = FileProcessing.GetBoolJsonElement(element1, JsonNamingPolicy.CamelCase.ConvertName(nameof(ApplicationData.Settings.MagnetInformation.PasteToAnotherWindow)), ApplicationData.Settings.MagnetInformation.PasteToAnotherWindow);
    //                ApplicationData.Settings.MagnetInformation.PasteToPressKey = FileProcessing.GetBoolJsonElement(element1, JsonNamingPolicy.CamelCase.ConvertName(nameof(ApplicationData.Settings.MagnetInformation.PasteToPressKey)), ApplicationData.Settings.MagnetInformation.PasteToPressKey);
    //                ApplicationData.Settings.MagnetInformation.PastingDecisionDistance = FileProcessing.GetIntJsonElement(element1, JsonNamingPolicy.CamelCase.ConvertName(nameof(ApplicationData.Settings.MagnetInformation.PastingDecisionDistance)), ApplicationData.Settings.MagnetInformation.PastingDecisionDistance);
    //                ApplicationData.Settings.MagnetInformation.StopTimeWhenPasted = FileProcessing.GetIntJsonElement(element1, JsonNamingPolicy.CamelCase.ConvertName(nameof(ApplicationData.Settings.MagnetInformation.StopTimeWhenPasted)), ApplicationData.Settings.MagnetInformation.StopTimeWhenPasted);
    //                ApplicationData.Settings.MagnetInformation.StopProcessingFullScreen = FileProcessing.GetBoolJsonElement(element1, JsonNamingPolicy.CamelCase.ConvertName(nameof(ApplicationData.Settings.MagnetInformation.StopProcessingFullScreen)), ApplicationData.Settings.MagnetInformation.StopProcessingFullScreen);
    //            }
    //            if (rootElement.TryGetProperty(JsonNamingPolicy.CamelCase.ConvertName(nameof(ApplicationData.Settings.HotkeyInformation)), out element1))
    //            {
    //                ApplicationData.Settings.HotkeyInformation.Enabled = FileProcessing.GetBoolJsonElement(element1, JsonNamingPolicy.CamelCase.ConvertName(nameof(ApplicationData.Settings.HotkeyInformation.Enabled)), ApplicationData.Settings.HotkeyInformation.Enabled);
    //                ApplicationData.Settings.HotkeyInformation.DoNotChangeOutOfScreen = FileProcessing.GetBoolJsonElement(element1, JsonNamingPolicy.CamelCase.ConvertName(nameof(ApplicationData.Settings.HotkeyInformation.DoNotChangeOutOfScreen)), ApplicationData.Settings.HotkeyInformation.DoNotChangeOutOfScreen);
    //                ApplicationData.Settings.HotkeyInformation.StopProcessingFullScreen = FileProcessing.GetBoolJsonElement(element1, JsonNamingPolicy.CamelCase.ConvertName(nameof(ApplicationData.Settings.HotkeyInformation.StopProcessingFullScreen)), ApplicationData.Settings.HotkeyInformation.StopProcessingFullScreen);
    //                if (element1.TryGetProperty(JsonNamingPolicy.CamelCase.ConvertName(nameof(ApplicationData.Settings.HotkeyInformation.AddModifyWindowSize)), out element2))
    //                {
    //                    ApplicationData.Settings.HotkeyInformation.AddModifyWindowSize.Width = FileProcessing.GetIntJsonElement(element2, JsonNamingPolicy.CamelCase.ConvertName(nameof(ApplicationData.Settings.HotkeyInformation.AddModifyWindowSize.Width)), ApplicationData.Settings.HotkeyInformation.AddModifyWindowSize.Width);
    //                    ApplicationData.Settings.HotkeyInformation.AddModifyWindowSize.Height = FileProcessing.GetIntJsonElement(element2, JsonNamingPolicy.CamelCase.ConvertName(nameof(ApplicationData.Settings.HotkeyInformation.AddModifyWindowSize.Height)), ApplicationData.Settings.HotkeyInformation.AddModifyWindowSize.Height);
    //                }
    //                if (element1.TryGetProperty(JsonNamingPolicy.CamelCase.ConvertName(nameof(ApplicationData.Settings.HotkeyInformation.Items)), out element2))
    //                {
    //                    foreach (var item1 in element2.EnumerateArray())
    //                    {
    //                        HotkeyItemInformation newHII = new();

    //                        newHII.RegisteredName = FileProcessing.GetStringJsonElement(item1, JsonNamingPolicy.CamelCase.ConvertName(nameof(newHII.RegisteredName)), newHII.RegisteredName);
    //                        _ = Enum.TryParse(FileProcessing.GetStringJsonElement(item1, JsonNamingPolicy.CamelCase.ConvertName(nameof(newHII.ProcessingType)), System.Enum.GetName(typeof(HotkeyProcessingType), newHII.ProcessingType) ?? ""), out HotkeyProcessingType getHotkeyProcessingType);
    //                        newHII.ProcessingType = getHotkeyProcessingType;
    //                        _ = Enum.TryParse(FileProcessing.GetStringJsonElement(item1, JsonNamingPolicy.CamelCase.ConvertName(nameof(newHII.StandardDisplay)), Enum.GetName(typeof(StandardDisplay), newHII.StandardDisplay) ?? ""), out StandardDisplay getStandardDisplay);
    //                        newHII.StandardDisplay = getStandardDisplay;
    //                        if (item1.TryGetProperty(JsonNamingPolicy.CamelCase.ConvertName(nameof(newHII.PositionSize)), out element4))
    //                        {
    //                            newHII.PositionSize.Display = FileProcessing.GetStringJsonElement(element4, JsonNamingPolicy.CamelCase.ConvertName(nameof(newHII.PositionSize.Display)), newHII.PositionSize.Display);
    //                            _ = Enum.TryParse(FileProcessing.GetStringJsonElement(element4, JsonNamingPolicy.CamelCase.ConvertName(nameof(newHII.PositionSize.SettingsWindowState)), Enum.GetName(typeof(SettingsWindowState), newHII.PositionSize.SettingsWindowState) ?? ""), out SettingsWindowState getSettingsWindowState);
    //                            newHII.PositionSize.SettingsWindowState = getSettingsWindowState;
    //                            newHII.PositionSize.X = FileProcessing.GetDoubleJsonElement(element4, JsonNamingPolicy.CamelCase.ConvertName(nameof(newHII.PositionSize.X)), newHII.PositionSize.X);
    //                            newHII.PositionSize.Y = FileProcessing.GetDoubleJsonElement(element4, JsonNamingPolicy.CamelCase.ConvertName(nameof(newHII.PositionSize.Y)), newHII.PositionSize.Y);
    //                            newHII.PositionSize.Width = FileProcessing.GetDoubleJsonElement(element4, JsonNamingPolicy.CamelCase.ConvertName(nameof(newHII.PositionSize.Width)), newHII.PositionSize.Width);
    //                            newHII.PositionSize.Height = FileProcessing.GetDoubleJsonElement(element4, JsonNamingPolicy.CamelCase.ConvertName(nameof(newHII.PositionSize.Height)), newHII.PositionSize.Height);
    //                            _ = Enum.TryParse(FileProcessing.GetStringJsonElement(element4, JsonNamingPolicy.CamelCase.ConvertName(nameof(newHII.PositionSize.XType)), Enum.GetName(typeof(WindowXType), newHII.PositionSize.XType) ?? ""), out WindowXType getWindowXType);
    //                            newHII.PositionSize.XType = getWindowXType;
    //                            _ = Enum.TryParse(FileProcessing.GetStringJsonElement(element4, JsonNamingPolicy.CamelCase.ConvertName(nameof(newHII.PositionSize.YType)), Enum.GetName(typeof(WindowYType), newHII.PositionSize.YType) ?? ""), out WindowYType getWindowYType);
    //                            newHII.PositionSize.YType = getWindowYType;
    //                            _ = Enum.TryParse(FileProcessing.GetStringJsonElement(element4, JsonNamingPolicy.CamelCase.ConvertName(nameof(newHII.PositionSize.WidthType)), Enum.GetName(typeof(WindowSizeType), newHII.PositionSize.WidthType) ?? ""), out WindowSizeType getWindowSizeType);
    //                            newHII.PositionSize.WidthType = getWindowSizeType;
    //                            _ = Enum.TryParse(FileProcessing.GetStringJsonElement(element4, JsonNamingPolicy.CamelCase.ConvertName(nameof(newHII.PositionSize.HeightType)), Enum.GetName(typeof(WindowSizeType), newHII.PositionSize.HeightType) ?? ""), out getWindowSizeType);
    //                            newHII.PositionSize.HeightType = getWindowSizeType;
    //                            _ = Enum.TryParse(FileProcessing.GetStringJsonElement(element4, JsonNamingPolicy.CamelCase.ConvertName(nameof(newHII.PositionSize.XValueType)), Enum.GetName(typeof(PositionSizeValueType), newHII.PositionSize.XValueType) ?? ""), out PositionSizeValueType getPositionSizeValueType);
    //                            newHII.PositionSize.XValueType = getPositionSizeValueType;
    //                            _ = Enum.TryParse(FileProcessing.GetStringJsonElement(element4, JsonNamingPolicy.CamelCase.ConvertName(nameof(newHII.PositionSize.YValueType)), Enum.GetName(typeof(PositionSizeValueType), newHII.PositionSize.YValueType) ?? ""), out getPositionSizeValueType);
    //                            newHII.PositionSize.YValueType = getPositionSizeValueType;
    //                            _ = Enum.TryParse(FileProcessing.GetStringJsonElement(element4, JsonNamingPolicy.CamelCase.ConvertName(nameof(newHII.PositionSize.WidthValueType)), Enum.GetName(typeof(PositionSizeValueType), newHII.PositionSize.WidthValueType) ?? ""), out getPositionSizeValueType);
    //                            newHII.PositionSize.WidthValueType = getPositionSizeValueType;
    //                            _ = Enum.TryParse(FileProcessing.GetStringJsonElement(element4, JsonNamingPolicy.CamelCase.ConvertName(nameof(newHII.PositionSize.HeightValueType)), Enum.GetName(typeof(PositionSizeValueType), newHII.PositionSize.HeightValueType) ?? ""), out getPositionSizeValueType);
    //                            newHII.PositionSize.HeightValueType = getPositionSizeValueType;
    //                            newHII.PositionSize.ClientArea = FileProcessing.GetBoolJsonElement(element4, JsonNamingPolicy.CamelCase.ConvertName(nameof(newHII.PositionSize.ClientArea)), newHII.PositionSize.ClientArea);
    //                        }
    //                        newHII.ProcessingValue = FileProcessing.GetIntJsonElement(item1, JsonNamingPolicy.CamelCase.ConvertName(nameof(newHII.ProcessingValue)), newHII.ProcessingValue);
    //                        if (item1.TryGetProperty(JsonNamingPolicy.CamelCase.ConvertName(nameof(newHII.Hotkey)), out element3))
    //                        {
    //                            newHII.Hotkey.Alt = FileProcessing.GetBoolJsonElement(element3, JsonNamingPolicy.CamelCase.ConvertName(nameof(newHII.Hotkey.Alt)), newHII.Hotkey.Alt);
    //                            newHII.Hotkey.Ctrl = FileProcessing.GetBoolJsonElement(element3, JsonNamingPolicy.CamelCase.ConvertName(nameof(newHII.Hotkey.Ctrl)), newHII.Hotkey.Ctrl);
    //                            newHII.Hotkey.Shift = FileProcessing.GetBoolJsonElement(element3, JsonNamingPolicy.CamelCase.ConvertName(nameof(newHII.Hotkey.Shift)), newHII.Hotkey.Shift);
    //                            newHII.Hotkey.Windows = FileProcessing.GetBoolJsonElement(element3, JsonNamingPolicy.CamelCase.ConvertName(nameof(newHII.Hotkey.Windows)), newHII.Hotkey.Windows);
    //                            newHII.Hotkey.KeyCharacter = FileProcessing.GetIntJsonElement(element3, JsonNamingPolicy.CamelCase.ConvertName(nameof(newHII.Hotkey.KeyCharacter)), newHII.Hotkey.KeyCharacter);
    //                        }

    //                        ApplicationData.Settings.HotkeyInformation.Items.Add(newHII);
    //                    }
    //                }
    //            }
    //            if (rootElement.TryGetProperty(JsonNamingPolicy.CamelCase.ConvertName(nameof(ApplicationData.Settings.PluginInformation)), out element1))
    //            {
    //                ApplicationData.Settings.PluginInformation.Enabled = FileProcessing.GetBoolJsonElement(element1, JsonNamingPolicy.CamelCase.ConvertName(nameof(ApplicationData.Settings.PluginInformation.Enabled)), ApplicationData.Settings.PluginInformation.Enabled);
    //                ApplicationData.Settings.PluginInformation.PluginDirectory = FileProcessing.GetStringJsonElement(element1, JsonNamingPolicy.CamelCase.ConvertName(nameof(ApplicationData.Settings.PluginInformation.PluginDirectory)), ApplicationData.Settings.PluginInformation.PluginDirectory);
    //                if (element1.TryGetProperty(JsonNamingPolicy.CamelCase.ConvertName(nameof(ApplicationData.Settings.PluginInformation.Items)), out element2))
    //                {
    //                    foreach (var item1 in element2.EnumerateArray())
    //                    {
    //                        PluginItemInformation newItem = new();

    //                        newItem.PluginFileName = FileProcessing.GetStringJsonElement(item1, JsonNamingPolicy.CamelCase.ConvertName(nameof(newItem.PluginFileName)), newItem.PluginFileName);

    //                        ApplicationData.Settings.PluginInformation.Items.Add(newItem);
    //                    }
    //                }
    //            }

    //            result = true;
    //        }
    //    }
    //    catch
    //    {
    //    }

    //    return result;
    //}

    /// <summary>
    /// 設定ファイルに書き込む
    /// </summary>
    /// <param name="path">設定ファイルのパス</param>
    /// <returns>結果 (失敗「false」/成功「true」)</returns>
    //private static bool WriteSettingsFile(
    //    string path
    //    )
    //{
    //    bool result = false;        // 結果

    //    try
    //    {
    //        XDocument document = new();
    //        XElement element1, element2, element3, element4, element5, element6, element7;

    //        element1 = new("Settings");
    //        // Start MainWindowRectangle
    //        element2 = new("MainWindowRectangle");
    //        element2.Add(new XElement("X", ApplicationData.Settings.MainWindowRectangle.X.ToString()));
    //        element2.Add(new XElement("Y", ApplicationData.Settings.MainWindowRectangle.Y.ToString()));
    //        element2.Add(new XElement("Width", ApplicationData.Settings.MainWindowRectangle.Width.ToString()));
    //        element2.Add(new XElement("Height", ApplicationData.Settings.MainWindowRectangle.Height.ToString()));
    //        element1.Add(element2);
    //        // End MainWindowRectangle
    //        element1.Add(new XElement("WindowStateMainWindow", Enum.GetName(typeof(WindowState), ApplicationData.Settings.WindowStateMainWindow)));
    //        element1.Add(new XElement("Language", ApplicationData.Settings.Language));
    //        element1.Add(new XElement("CoordinateType", ApplicationData.Settings.CoordinateType.ToString()));
    //        element1.Add(new XElement("DarkMode", ApplicationData.Settings.DarkMode.ToString()));
    //        element1.Add(new XElement("AutomaticallyUpdateCheck", ApplicationData.Settings.AutomaticallyUpdateCheck.ToString()));
    //        element1.Add(new XElement("CheckBetaVersion", ApplicationData.Settings.CheckBetaVersion.ToString()));
    //        element1.Add(new XElement("UseLongPath", ApplicationData.Settings.UseLongPath.ToString()));
    //        // Start ShiftPastePosition
    //        element2 = new("ShiftPastePosition");
    //        element2.Add(new XElement("Enabled", ApplicationData.Settings.ShiftPastePosition.Enabled.ToString()));
    //        element2.Add(new XElement("Left", ApplicationData.Settings.ShiftPastePosition.Left.ToString()));
    //        element2.Add(new XElement("Top", ApplicationData.Settings.ShiftPastePosition.Top.ToString()));
    //        element2.Add(new XElement("Right", ApplicationData.Settings.ShiftPastePosition.Right.ToString()));
    //        element2.Add(new XElement("Bottom", ApplicationData.Settings.ShiftPastePosition.Bottom.ToString()));
    //        element1.Add(element2);
    //        // End ShiftPastePosition
    //        // Start SpecifyWindowInformation
    //        element2 = new("SpecifyWindowInformation");
    //        element2.Add(new XElement("Enabled", ApplicationData.Settings.SpecifyWindowInformation.Enabled.ToString()));
    //        element2.Add(new XElement("MultipleRegistrations", ApplicationData.Settings.SpecifyWindowInformation.MultipleRegistrations.ToString()));
    //        element2.Add(new XElement("CaseSensitive", ApplicationData.Settings.SpecifyWindowInformation.CaseSensitive.ToString()));
    //        element2.Add(new XElement("DoNotChangeOutOfScreen", ApplicationData.Settings.SpecifyWindowInformation.DoNotChangeOutOfScreen.ToString()));
    //        element2.Add(new XElement("StopProcessingShowAddModifyWindow", ApplicationData.Settings.SpecifyWindowInformation.StopProcessingShowAddModifyWindow.ToString()));
    //        element2.Add(new XElement("StopProcessingFullScreen", ApplicationData.Settings.SpecifyWindowInformation.StopProcessingFullScreen.ToString()));
    //        element2.Add(new XElement("HotkeysDoNotStopFullScreen", ApplicationData.Settings.SpecifyWindowInformation.HotkeysDoNotStopFullScreen.ToString()));
    //        element2.Add(new XElement("ProcessingInterval", ApplicationData.Settings.SpecifyWindowInformation.ProcessingInterval.ToString()));
    //        element2.Add(new XElement("ProcessingWindowRange", Enum.GetName(typeof(ProcessingWindowRange), ApplicationData.Settings.SpecifyWindowInformation.ProcessingWindowRange)));
    //        element2.Add(new XElement("WaitTimeToProcessingNextWindow", ApplicationData.Settings.SpecifyWindowInformation.WaitTimeToProcessingNextWindow.ToString()));
    //        element3 = new("AddModifyWindowSize");
    //        element3.Add(new XElement("Width", ApplicationData.Settings.SpecifyWindowInformation.AddModifyWindowSize.Width.ToString()));
    //        element3.Add(new XElement("Height", ApplicationData.Settings.SpecifyWindowInformation.AddModifyWindowSize.Height.ToString()));
    //        element2.Add(element3);
    //        element3 = new("AcquiredItems");
    //        element3.Add(new XElement("TitleName", ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.TitleName.ToString()));
    //        element3.Add(new XElement("ClassName", ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.ClassName.ToString()));
    //        element3.Add(new XElement("FileName", ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.FileName.ToString()));
    //        element3.Add(new XElement("Display", ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.Display.ToString()));
    //        element3.Add(new XElement("WindowState", ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.WindowState.ToString()));
    //        element3.Add(new XElement("X", ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.X.ToString()));
    //        element3.Add(new XElement("Y", ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.Y.ToString()));
    //        element3.Add(new XElement("Width", ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.Width.ToString()));
    //        element3.Add(new XElement("Height", ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.Height.ToString()));
    //        element3.Add(new XElement("Version", ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.Version.ToString()));
    //        element2.Add(element3);
    //        element3 = new("Items");
    //        foreach (SpecifyWindowItemInformation nowItem in ApplicationData.Settings.SpecifyWindowInformation.Items)
    //        {
    //            element4 = new("Item");
    //            element4.Add(new XElement("Enabled", nowItem.Enabled.ToString()));
    //            element4.Add(new XElement("RegisteredName", nowItem.RegisteredName));
    //            element4.Add(new XElement("TitleName", nowItem.TitleName));
    //            element4.Add(new XElement("TitleNameMatchCondition", Enum.GetName(typeof(NameMatchCondition), nowItem.TitleNameMatchCondition)));
    //            element4.Add(new XElement("ClassName", nowItem.ClassName));
    //            element4.Add(new XElement("ClassNameMatchCondition", Enum.GetName(typeof(NameMatchCondition), nowItem.ClassNameMatchCondition)));
    //            element4.Add(new XElement("FileName", nowItem.FileName));
    //            element4.Add(new XElement("FileNameMatchCondition", Enum.GetName(typeof(FileNameMatchCondition), nowItem.FileNameMatchCondition)));
    //            element4.Add(new XElement("StandardDisplay", Enum.GetName(typeof(StandardDisplay), nowItem.StandardDisplay)));
    //            element4.Add(new XElement("ProcessingOnlyOnce", Enum.GetName(typeof(ProcessingOnlyOnce), nowItem.ProcessingOnlyOnce)));
    //            // Start WindowEventData
    //            element5 = new("WindowEventData");
    //            element5.Add(new XElement("Foreground", nowItem.WindowEventData.Foreground.ToString()));
    //            element5.Add(new XElement("MoveSizeEnd", nowItem.WindowEventData.MoveSizeEnd.ToString()));
    //            element5.Add(new XElement("MinimizeStart", nowItem.WindowEventData.MinimizeStart.ToString()));
    //            element5.Add(new XElement("MinimizeEnd", nowItem.WindowEventData.MinimizeEnd.ToString()));
    //            element5.Add(new XElement("Show", nowItem.WindowEventData.Show.ToString()));
    //            element5.Add(new XElement("NameChange", nowItem.WindowEventData.NameChange.ToString()));
    //            element4.Add(element5);
    //            // End WindowEventData
    //            element4.Add(new XElement("TimerProcessing", nowItem.TimerProcessing.ToString()));
    //            element4.Add(new XElement("NumberOfTimesNotToProcessingFirst", nowItem.NumberOfTimesNotToProcessingFirst.ToString()));
    //            // Start WindowProcessingInformation
    //            element5 = new("WindowProcessingInformation");
    //            foreach (WindowProcessingInformation nowWPI in nowItem.WindowProcessingInformation)
    //            {
    //                element6 = new("WindowProcessingInformation");
    //                element6.Add(new XElement("Active", nowWPI.Active.ToString()));
    //                element6.Add(new XElement("ProcessingName", nowWPI.ProcessingName));
    //                // Start PositionSize
    //                element7 = new("PositionSize");
    //                element7.Add(new XElement("Display", nowWPI.PositionSize.Display));
    //                element7.Add(new XElement("SettingsWindowState", Enum.GetName(typeof(SettingsWindowState), nowWPI.PositionSize.SettingsWindowState)));
    //                element7.Add(new XElement("X", nowWPI.PositionSize.X.ToString()));
    //                element7.Add(new XElement("Y", nowWPI.PositionSize.Y.ToString()));
    //                element7.Add(new XElement("Width", nowWPI.PositionSize.Width.ToString()));
    //                element7.Add(new XElement("Height", nowWPI.PositionSize.Height.ToString()));
    //                element7.Add(new XElement("XType", Enum.GetName(typeof(WindowXType), nowWPI.PositionSize.XType)));
    //                element7.Add(new XElement("YType", Enum.GetName(typeof(WindowYType), nowWPI.PositionSize.YType)));
    //                element7.Add(new XElement("WidthType", Enum.GetName(typeof(WindowSizeType), nowWPI.PositionSize.WidthType)));
    //                element7.Add(new XElement("HeightType", Enum.GetName(typeof(WindowSizeType), nowWPI.PositionSize.HeightType)));
    //                element7.Add(new XElement("XValueType", Enum.GetName(typeof(PositionSizeValueType), nowWPI.PositionSize.XValueType)));
    //                element7.Add(new XElement("YValueType", Enum.GetName(typeof(PositionSizeValueType), nowWPI.PositionSize.YValueType)));
    //                element7.Add(new XElement("WidthValueType", Enum.GetName(typeof(PositionSizeValueType), nowWPI.PositionSize.WidthValueType)));
    //                element7.Add(new XElement("HeightValueType", Enum.GetName(typeof(PositionSizeValueType), nowWPI.PositionSize.HeightValueType)));
    //                element7.Add(new XElement("ClientArea", nowWPI.PositionSize.ClientArea.ToString()));
    //                element6.Add(element7);
    //                // End PositionSize
    //                element6.Add(new XElement("NormalWindowOnly", nowWPI.NormalWindowOnly.ToString()));
    //                element6.Add(new XElement("Forefront", Enum.GetName(typeof(Forefront), nowWPI.Forefront)));
    //                element6.Add(new XElement("EnabledTransparency", nowWPI.EnabledTransparency.ToString()));
    //                element6.Add(new XElement("Transparency", nowWPI.Transparency.ToString()));
    //                element6.Add(new XElement("CloseWindow", nowWPI.CloseWindow.ToString()));
    //                element7 = new("Hotkey");
    //                element7.Add(new XElement("Alt", nowWPI.Hotkey.Alt.ToString()));
    //                element7.Add(new XElement("Ctrl", nowWPI.Hotkey.Ctrl.ToString()));
    //                element7.Add(new XElement("Shift", nowWPI.Hotkey.Shift.ToString()));
    //                element7.Add(new XElement("Windows", nowWPI.Hotkey.Windows.ToString()));
    //                element7.Add(new XElement("KeyCharacter", nowWPI.Hotkey.KeyCharacter.ToString()));
    //                element6.Add(element7);
    //                element5.Add(element6);
    //            }
    //            element4.Add(element5);
    //            // End WindowProcessingInformation
    //            element4.Add(new XElement("DoNotProcessingTitleNameConditions", Enum.GetName(typeof(TitleNameProcessingConditions), nowItem.DoNotProcessingTitleNameConditions)));
    //            // Start DoNotProcessingStringContainedInTitleName
    //            element5 = new("DoNotProcessingStringContainedInTitleName");
    //            foreach (string nowTitleName in nowItem.DoNotProcessingStringContainedInTitleName)
    //            {
    //                element6 = new("Item");
    //                element6.Add(new XElement("String", nowTitleName));
    //                element5.Add(element6);
    //            }
    //            element4.Add(element5);
    //            // End DoNotProcessingStringContainedInTitleName
    //            // Start DoNotProcessingSize
    //            element5 = new("DoNotProcessingSize");
    //            foreach (MySize nowSize in nowItem.DoNotProcessingSize)
    //            {
    //                element6 = new("Item");
    //                element6.Add(new XElement("Width", nowSize.Width.ToString()));
    //                element6.Add(new XElement("Height", nowSize.Height.ToString()));
    //                element5.Add(element6);
    //            }
    //            element4.Add(element5);
    //            // End DoNotProcessingSize
    //            element4.Add(new XElement("DoNotProcessingOtherThanSpecifiedVersion", nowItem.DoNotProcessingOtherThanSpecifiedVersion));
    //            element4.Add(new XElement("DoNotProcessingOtherThanSpecifiedVersionAnnounce", nowItem.DoNotProcessingOtherThanSpecifiedVersionAnnounce.ToString()));
    //            element3.Add(element4);
    //        }
    //        element2.Add(element3);
    //        element1.Add(element2);
    //        // End SpecifyWindowInformation
    //        // Start AllWindowInformation
    //        element2 = new("AllWindowInformation");
    //        element2.Add(new XElement("Enabled", ApplicationData.Settings.AllWindowInformation.Enabled.ToString()));
    //        element2.Add(new XElement("CaseSensitive", ApplicationData.Settings.AllWindowInformation.CaseSensitive.ToString()));
    //        element2.Add(new XElement("StopProcessingFullScreen", ApplicationData.Settings.AllWindowInformation.StopProcessingFullScreen.ToString()));
    //        element2.Add(new XElement("StandardDisplay", Enum.GetName(typeof(StandardDisplay), ApplicationData.Settings.AllWindowInformation.StandardDisplay)));
    //        // Start PositionSize
    //        element3 = new("PositionSize");
    //        element3.Add(new XElement("Display", ApplicationData.Settings.AllWindowInformation.PositionSize.Display));
    //        element3.Add(new XElement("X", ApplicationData.Settings.AllWindowInformation.PositionSize.X.ToString()));
    //        element3.Add(new XElement("Y", ApplicationData.Settings.AllWindowInformation.PositionSize.Y.ToString()));
    //        element3.Add(new XElement("XType", Enum.GetName(typeof(WindowXType), ApplicationData.Settings.AllWindowInformation.PositionSize.XType)));
    //        element3.Add(new XElement("YType", Enum.GetName(typeof(WindowYType), ApplicationData.Settings.AllWindowInformation.PositionSize.YType)));
    //        element3.Add(new XElement("XValueType", Enum.GetName(typeof(PositionSizeValueType), ApplicationData.Settings.AllWindowInformation.PositionSize.XValueType)));
    //        element3.Add(new XElement("YValueType", Enum.GetName(typeof(PositionSizeValueType), ApplicationData.Settings.AllWindowInformation.PositionSize.YValueType)));
    //        element2.Add(element3);
    //        // End PositionSize
    //        element3 = new("AddModifyWindowSize");
    //        element3.Add(new XElement("Width", ApplicationData.Settings.AllWindowInformation.AddModifyWindowSize.Width.ToString()));
    //        element3.Add(new XElement("Height", ApplicationData.Settings.AllWindowInformation.AddModifyWindowSize.Height.ToString()));
    //        element2.Add(element3);
    //        // Start AllWindowPositionSizeWindowEventData
    //        element3 = new("AllWindowPositionSizeWindowEventData");
    //        element3.Add(new XElement("MoveSizeEnd", ApplicationData.Settings.AllWindowInformation.AllWindowPositionSizeWindowEventData.MoveSizeEnd.ToString()));
    //        element3.Add(new XElement("Show", ApplicationData.Settings.AllWindowInformation.AllWindowPositionSizeWindowEventData.Show.ToString()));
    //        element2.Add(element3);
    //        // End AllWindowPositionSizeWindowEventData
    //        // Start CancelAllWindowPositionSize
    //        element3 = new("CancelAllWindowPositionSize");
    //        foreach (WindowJudgementInformation nowItem in ApplicationData.Settings.AllWindowInformation.CancelAllWindowPositionSize)
    //        {
    //            element4 = new("Item");
    //            element4.Add(new XElement("RegisteredName", nowItem.RegisteredName));
    //            element4.Add(new XElement("TitleName", nowItem.TitleName));
    //            element4.Add(new XElement("TitleNameMatchCondition", Enum.GetName(typeof(NameMatchCondition), nowItem.TitleNameMatchCondition)));
    //            element4.Add(new XElement("ClassName", nowItem.ClassName));
    //            element4.Add(new XElement("ClassNameMatchCondition", Enum.GetName(typeof(NameMatchCondition), nowItem.ClassNameMatchCondition)));
    //            element4.Add(new XElement("FileName", nowItem.FileName));
    //            element4.Add(new XElement("FileNameMatchCondition", Enum.GetName(typeof(FileNameMatchCondition), nowItem.FileNameMatchCondition)));
    //            element3.Add(element4);
    //        }
    //        element2.Add(element3);
    //        /// End CancelAllWindowPositionSize
    //        element1.Add(element2);
    //        // End AllWindowInformation
    //        // Start MagnetInformation
    //        element2 = new("MagnetInformation");
    //        element2.Add(new XElement("Enabled", ApplicationData.Settings.MagnetInformation.Enabled.ToString()));
    //        element2.Add(new XElement("PasteToEdgeOfScreen", ApplicationData.Settings.MagnetInformation.PasteToEdgeOfScreen.ToString()));
    //        element2.Add(new XElement("PasteToAnotherWindow", ApplicationData.Settings.MagnetInformation.PasteToAnotherWindow.ToString()));
    //        element2.Add(new XElement("PasteToPressKey", ApplicationData.Settings.MagnetInformation.PasteToPressKey.ToString()));
    //        element2.Add(new XElement("PastingDecisionDistance", ApplicationData.Settings.MagnetInformation.PastingDecisionDistance.ToString()));
    //        element2.Add(new XElement("StopTimeWhenPasted", ApplicationData.Settings.MagnetInformation.StopTimeWhenPasted.ToString()));
    //        element2.Add(new XElement("StopProcessingFullScreen", ApplicationData.Settings.MagnetInformation.StopProcessingFullScreen.ToString()));
    //        element1.Add(element2);
    //        // End MagnetInformation
    //        // Start HotkeyInformation
    //        element2 = new("HotkeyInformation");
    //        element2.Add(new XElement("Enabled", ApplicationData.Settings.HotkeyInformation.Enabled.ToString()));
    //        element2.Add(new XElement("DoNotChangeOutOfScreen", ApplicationData.Settings.HotkeyInformation.DoNotChangeOutOfScreen.ToString()));
    //        element2.Add(new XElement("StopProcessingFullScreen", ApplicationData.Settings.HotkeyInformation.StopProcessingFullScreen.ToString()));
    //        element3 = new("AddModifyWindowSize");
    //        element3.Add(new XElement("Width", ApplicationData.Settings.HotkeyInformation.AddModifyWindowSize.Width.ToString()));
    //        element3.Add(new XElement("Height", ApplicationData.Settings.HotkeyInformation.AddModifyWindowSize.Height.ToString()));
    //        element2.Add(element3);
    //        element3 = new("Items");
    //        foreach (HotkeyItemInformation nowItem in ApplicationData.Settings.HotkeyInformation.Items)
    //        {
    //            element4 = new("Item");
    //            element4.Add(new XElement("RegisteredName", nowItem.RegisteredName));
    //            element4.Add(new XElement("ProcessingType", Enum.GetName(typeof(HotkeyProcessingType), nowItem.ProcessingType)));
    //            element4.Add(new XElement("StandardDisplay", Enum.GetName(typeof(StandardDisplay), nowItem.StandardDisplay)));
    //            element5 = new("PositionSize");
    //            element5.Add(new XElement("Display", nowItem.PositionSize.Display));
    //            element5.Add(new XElement("SettingsWindowState", Enum.GetName(typeof(SettingsWindowState), nowItem.PositionSize.SettingsWindowState)));
    //            element5.Add(new XElement("X", nowItem.PositionSize.X.ToString()));
    //            element5.Add(new XElement("Y", nowItem.PositionSize.Y.ToString()));
    //            element5.Add(new XElement("Width", nowItem.PositionSize.Width.ToString()));
    //            element5.Add(new XElement("Height", nowItem.PositionSize.Height.ToString()));
    //            element5.Add(new XElement("XType", Enum.GetName(typeof(WindowXType), nowItem.PositionSize.XType)));
    //            element5.Add(new XElement("YType", Enum.GetName(typeof(WindowYType), nowItem.PositionSize.YType)));
    //            element5.Add(new XElement("WidthType", Enum.GetName(typeof(WindowSizeType), nowItem.PositionSize.WidthType)));
    //            element5.Add(new XElement("HeightType", Enum.GetName(typeof(WindowSizeType), nowItem.PositionSize.HeightType)));
    //            element5.Add(new XElement("XValueType", Enum.GetName(typeof(PositionSizeValueType), nowItem.PositionSize.XValueType)));
    //            element5.Add(new XElement("YValueType", Enum.GetName(typeof(PositionSizeValueType), nowItem.PositionSize.YValueType)));
    //            element5.Add(new XElement("WidthValueType", Enum.GetName(typeof(PositionSizeValueType), nowItem.PositionSize.WidthValueType)));
    //            element5.Add(new XElement("HeightValueType", Enum.GetName(typeof(PositionSizeValueType), nowItem.PositionSize.HeightValueType)));
    //            element5.Add(new XElement("ClientArea", nowItem.PositionSize.ClientArea.ToString()));
    //            element4.Add(element5);
    //            element4.Add(new XElement("ProcessingValue", nowItem.ProcessingValue.ToString()));
    //            element5 = new("Hotkey");
    //            element5.Add(new XElement("Alt", nowItem.Hotkey.Alt.ToString()));
    //            element5.Add(new XElement("Ctrl", nowItem.Hotkey.Ctrl.ToString()));
    //            element5.Add(new XElement("Shift", nowItem.Hotkey.Shift.ToString()));
    //            element5.Add(new XElement("Windows", nowItem.Hotkey.Windows.ToString()));
    //            element5.Add(new XElement("KeyCharacter", nowItem.Hotkey.KeyCharacter.ToString()));
    //            element4.Add(element5);
    //            element3.Add(element4);
    //        }
    //        element2.Add(element3);
    //        element1.Add(element2);
    //        // End HotkeyInformation
    //        // Start PluginInformation
    //        element2 = new("PluginInformation");
    //        element2.Add(new XElement("Enabled", ApplicationData.Settings.PluginInformation.Enabled.ToString()));
    //        element3 = new("Items");
    //        foreach (PluginItemInformation nowItem in ApplicationData.Settings.PluginInformation.Items)
    //        {
    //            element4 = new("Item");
    //            element4.Add(new XElement("PluginFileName", nowItem.PluginFileName));
    //            element3.Add(element4);
    //        }
    //        element2.Add(element3);
    //        element1.Add(element2);
    //        // End PluginInformation

    //        document.Add(element1);

    //        document.Save(path);
    //        result = true;
    //    }
    //    catch
    //    {
    //    }

    //    return result;
    //}
}
