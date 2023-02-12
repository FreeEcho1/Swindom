namespace Swindom.Sources.SettingsData;

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
        string path = GetSettingFilePath();
        return string.IsNullOrEmpty(path) == false && ReadSettingsFile(path);
    }

    /// <summary>
    /// 設定ファイルに書き込む
    /// </summary>
    /// <param name="path">設定ファイルのパス (指定しない (自動選択)「null」)</param>
    public static bool WriteSettings(
        string? path = null
        )
    {
        if (string.IsNullOrEmpty(path))
        {
            path = GetSettingFilePath();
        }
        if (string.IsNullOrEmpty(path))
        {
            return CreateSettingFile();
        }
        return WriteSettingsFile(path);
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

            if (VariousProcessing.CheckInstalled())
            {
                string rearPath = Path.DirectorySeparatorChar + Common.ApplicationName;     // パスの後方
                string tempPath;      // パスの一時保管用

                if (Directory.Exists(tempPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + rearPath))
                {
                    settingDirectoryPath = tempPath;
                }
                else if (Directory.Exists(tempPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + rearPath))
                {
                    settingDirectoryPath = tempPath;
                }

                if (string.IsNullOrEmpty(settingDirectoryPath))
                {
                    switch (FEMessageBox.Show(ApplicationData.Languages.LanguagesWindow?.ShareSettings ?? "", ApplicationData.Languages.Check + Common.CopySeparateString + Common.ApplicationName, MessageBoxButton.YesNo))
                    {
                        case MessageBoxResult.Yes:
                            allUser = true;
                            break;
                    }

                    settingDirectoryPath = Environment.GetFolderPath(allUser ? Environment.SpecialFolder.CommonApplicationData : Environment.SpecialFolder.LocalApplicationData) + Path.DirectorySeparatorChar + Common.ApplicationName;
                    // アプリケーションディレクトリを作成
                    Directory.CreateDirectory(settingDirectoryPath);
                    // 全ユーザーの場合はアクセス制御を全ユーザーに設定
                    if (allUser)
                    {
                        SettingDirectorySecurity(settingDirectoryPath);
                        checkDirectorySecurity = true;
                    }
                }
                settingDirectoryPath += Path.DirectorySeparatorChar + Common.SettingsDirectoryName;
            }
            else
            {
                switch (FEMessageBox.Show(ApplicationData.Languages.LanguagesWindow?.ShareSettings ?? "", ApplicationData.Languages.Check + Common.CopySeparateString + Common.ApplicationName, MessageBoxButton.YesNo))
                {
                    case MessageBoxResult.Yes:
                        allUser = true;
                        break;
                }

                settingDirectoryPath = VariousProcessing.GetApplicationDirectoryPath(false) + Path.DirectorySeparatorChar + Common.SettingsDirectoryName;
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
            return WriteSettings(settingDirectoryPath + Path.DirectorySeparatorChar + (allUser ? Common.SettingFileNameForAllUsers : Environment.UserName) + Common.SettingFileExtension);
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
    /// 設定ディレクトリのパスを取得
    /// </summary>
    /// <returns>設定ディレクトリのパス (ディレクトリがない「""」)</returns>
    private static string GetSettingsDirectoryPath()
    {
        string path = "";     // パス

        if (string.IsNullOrEmpty(ApplicationData.SpecifySettingsFilePath))
        {
            string tempPath;      // パスの一時保管用

            if (VariousProcessing.CheckInstalled())
            {
                string rearPath = Path.DirectorySeparatorChar + Common.ApplicationName + Path.DirectorySeparatorChar + Common.SettingsDirectoryName;     // パスの後方

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
                if (Directory.Exists(tempPath = VariousProcessing.GetApplicationDirectoryPath(false) + Path.DirectorySeparatorChar + Common.SettingsDirectoryName))
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
            string directoryPath = GetSettingsDirectoryPath();      // ディレクトリのパス

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
            XDocument document = new();
            XElement element1, element2, element3, element4, element5, element6, element7;

            element1 = new("Settings");
            // Start MainWindowRectangle
            element2 = new("MainWindowRectangle");
            element2.Add(new XElement("X", ApplicationData.Settings.MainWindowRectangle.X.ToString()));
            element2.Add(new XElement("Y", ApplicationData.Settings.MainWindowRectangle.Y.ToString()));
            element2.Add(new XElement("Width", ApplicationData.Settings.MainWindowRectangle.Width.ToString()));
            element2.Add(new XElement("Height", ApplicationData.Settings.MainWindowRectangle.Height.ToString()));
            element1.Add(element2);
            // End MainWindowRectangle
            element1.Add(new XElement("WindowStateMainWindow", Enum.GetName(typeof(WindowState), ApplicationData.Settings.WindowStateMainWindow)));
            element1.Add(new XElement("Language", ApplicationData.Settings.Language));
            element1.Add(new XElement("CoordinateType", ApplicationData.Settings.CoordinateType.ToString()));
            element1.Add(new XElement("DarkMode", ApplicationData.Settings.DarkMode.ToString()));
            element1.Add(new XElement("AutomaticallyUpdateCheck", ApplicationData.Settings.AutomaticallyUpdateCheck.ToString()));
            element1.Add(new XElement("CheckBetaVersion", ApplicationData.Settings.CheckBetaVersion.ToString()));
            element1.Add(new XElement("UseLongPath", ApplicationData.Settings.UseLongPath.ToString()));
            // Start ShiftPastePosition
            element2 = new("ShiftPastePosition");
            element2.Add(new XElement("Enabled", ApplicationData.Settings.ShiftPastePosition.Enabled.ToString()));
            element2.Add(new XElement("Left", ApplicationData.Settings.ShiftPastePosition.Left.ToString()));
            element2.Add(new XElement("Top", ApplicationData.Settings.ShiftPastePosition.Top.ToString()));
            element2.Add(new XElement("Right", ApplicationData.Settings.ShiftPastePosition.Right.ToString()));
            element2.Add(new XElement("Bottom", ApplicationData.Settings.ShiftPastePosition.Bottom.ToString()));
            element1.Add(element2);
            // End ShiftPastePosition
            // Start SpecifyWindowInformation
            element2 = new("SpecifyWindowInformation");
            element2.Add(new XElement("Enabled", ApplicationData.Settings.SpecifyWindowInformation.Enabled.ToString()));
            element2.Add(new XElement("MultipleRegistrations", ApplicationData.Settings.SpecifyWindowInformation.MultipleRegistrations.ToString()));
            element2.Add(new XElement("CaseSensitive", ApplicationData.Settings.SpecifyWindowInformation.CaseSensitive.ToString()));
            element2.Add(new XElement("DoNotChangeOutOfScreen", ApplicationData.Settings.SpecifyWindowInformation.DoNotChangeOutOfScreen.ToString()));
            element2.Add(new XElement("StopProcessingShowAddModifyWindow", ApplicationData.Settings.SpecifyWindowInformation.StopProcessingShowAddModifyWindow.ToString()));
            element2.Add(new XElement("StopProcessingFullScreen", ApplicationData.Settings.SpecifyWindowInformation.StopProcessingFullScreen.ToString()));
            element2.Add(new XElement("HotkeysDoNotStopFullScreen", ApplicationData.Settings.SpecifyWindowInformation.HotkeysDoNotStopFullScreen.ToString()));
            element2.Add(new XElement("ProcessingInterval", ApplicationData.Settings.SpecifyWindowInformation.ProcessingInterval.ToString()));
            element2.Add(new XElement("ProcessingWindowRange", Enum.GetName(typeof(ProcessingWindowRange), ApplicationData.Settings.SpecifyWindowInformation.ProcessingWindowRange)));
            element2.Add(new XElement("WaitTimeToProcessingNextWindow", ApplicationData.Settings.SpecifyWindowInformation.WaitTimeToProcessingNextWindow.ToString()));
            element3 = new("AddModifyWindowSize");
            element3.Add(new XElement("Width", ApplicationData.Settings.SpecifyWindowInformation.AddModifyWindowSize.Width.ToString()));
            element3.Add(new XElement("Height", ApplicationData.Settings.SpecifyWindowInformation.AddModifyWindowSize.Height.ToString()));
            element2.Add(element3);
            element3 = new("AcquiredItems");
            element3.Add(new XElement("TitleName", ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.TitleName.ToString()));
            element3.Add(new XElement("ClassName", ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.ClassName.ToString()));
            element3.Add(new XElement("FileName", ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.FileName.ToString()));
            element3.Add(new XElement("Display", ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.Display.ToString()));
            element3.Add(new XElement("WindowState", ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.WindowState.ToString()));
            element3.Add(new XElement("X", ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.X.ToString()));
            element3.Add(new XElement("Y", ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.Y.ToString()));
            element3.Add(new XElement("Width", ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.Width.ToString()));
            element3.Add(new XElement("Height", ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.Height.ToString()));
            element3.Add(new XElement("Version", ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.Version.ToString()));
            element2.Add(element3);
            element3 = new("Items");
            foreach (SpecifyWindowItemInformation nowItem in ApplicationData.Settings.SpecifyWindowInformation.Items)
            {
                element4 = new("Item");
                element4.Add(new XElement("Enabled", nowItem.Enabled.ToString()));
                element4.Add(new XElement("RegisteredName", nowItem.RegisteredName));
                element4.Add(new XElement("TitleName", nowItem.TitleName));
                element4.Add(new XElement("TitleNameMatchCondition", Enum.GetName(typeof(NameMatchCondition), nowItem.TitleNameMatchCondition)));
                element4.Add(new XElement("ClassName", nowItem.ClassName));
                element4.Add(new XElement("ClassNameMatchCondition", Enum.GetName(typeof(NameMatchCondition), nowItem.ClassNameMatchCondition)));
                element4.Add(new XElement("FileName", nowItem.FileName));
                element4.Add(new XElement("FileNameMatchCondition", Enum.GetName(typeof(FileNameMatchCondition), nowItem.FileNameMatchCondition)));
                element4.Add(new XElement("StandardDisplay", Enum.GetName(typeof(StandardDisplay), nowItem.StandardDisplay)));
                element4.Add(new XElement("ProcessingOnlyOnce", Enum.GetName(typeof(ProcessingOnlyOnce), nowItem.ProcessingOnlyOnce)));
                // Start WindowEventData
                element5 = new("WindowEventData");
                element5.Add(new XElement("Foreground", nowItem.WindowEventData.Foreground.ToString()));
                element5.Add(new XElement("MoveSizeEnd", nowItem.WindowEventData.MoveSizeEnd.ToString()));
                element5.Add(new XElement("MinimizeStart", nowItem.WindowEventData.MinimizeStart.ToString()));
                element5.Add(new XElement("MinimizeEnd", nowItem.WindowEventData.MinimizeEnd.ToString()));
                element5.Add(new XElement("Create", nowItem.WindowEventData.Create.ToString()));
                element5.Add(new XElement("Show", nowItem.WindowEventData.Show.ToString()));
                element5.Add(new XElement("NameChange", nowItem.WindowEventData.NameChange.ToString()));
                element4.Add(element5);
                // End WindowEventData
                element4.Add(new XElement("TimerProcessing", nowItem.TimerProcessing.ToString()));
                element4.Add(new XElement("NumberOfTimesNotToProcessingFirst", nowItem.NumberOfTimesNotToProcessingFirst.ToString()));
                // Start WindowProcessingInformation
                element5 = new("WindowProcessingInformation");
                foreach (WindowProcessingInformation nowWPI in nowItem.WindowProcessingInformation)
                {
                    element6 = new("WindowProcessingInformation");
                    element6.Add(new XElement("Active", nowWPI.Active.ToString()));
                    element6.Add(new XElement("ProcessingName", nowWPI.ProcessingName));
                    // Start PositionSize
                    element7 = new("PositionSize");
                    element7.Add(new XElement("Display", nowWPI.PositionSize.Display));
                    element7.Add(new XElement("SettingsWindowState", Enum.GetName(typeof(SettingsWindowState), nowWPI.PositionSize.SettingsWindowState)));
                    element7.Add(new XElement("X", nowWPI.PositionSize.X.ToString()));
                    element7.Add(new XElement("Y", nowWPI.PositionSize.Y.ToString()));
                    element7.Add(new XElement("Width", nowWPI.PositionSize.Width.ToString()));
                    element7.Add(new XElement("Height", nowWPI.PositionSize.Height.ToString()));
                    element7.Add(new XElement("XType", Enum.GetName(typeof(WindowXType), nowWPI.PositionSize.XType)));
                    element7.Add(new XElement("YType", Enum.GetName(typeof(WindowYType), nowWPI.PositionSize.YType)));
                    element7.Add(new XElement("WidthType", Enum.GetName(typeof(WindowSizeType), nowWPI.PositionSize.WidthType)));
                    element7.Add(new XElement("HeightType", Enum.GetName(typeof(WindowSizeType), nowWPI.PositionSize.HeightType)));
                    element7.Add(new XElement("XValueType", Enum.GetName(typeof(PositionSizeValueType), nowWPI.PositionSize.XValueType)));
                    element7.Add(new XElement("YValueType", Enum.GetName(typeof(PositionSizeValueType), nowWPI.PositionSize.YValueType)));
                    element7.Add(new XElement("WidthValueType", Enum.GetName(typeof(PositionSizeValueType), nowWPI.PositionSize.WidthValueType)));
                    element7.Add(new XElement("HeightValueType", Enum.GetName(typeof(PositionSizeValueType), nowWPI.PositionSize.HeightValueType)));
                    element7.Add(new XElement("ClientArea", nowWPI.PositionSize.ClientArea.ToString()));
                    element6.Add(element7);
                    // End PositionSize
                    element6.Add(new XElement("NormalWindowOnly", nowWPI.NormalWindowOnly.ToString()));
                    element6.Add(new XElement("Forefront", Enum.GetName(typeof(Forefront), nowWPI.Forefront)));
                    element6.Add(new XElement("EnabledTransparency", nowWPI.EnabledTransparency.ToString()));
                    element6.Add(new XElement("Transparency", nowWPI.Transparency.ToString()));
                    element6.Add(new XElement("CloseWindow", nowWPI.CloseWindow.ToString()));
                    element7 = new("Hotkey");
                    element7.Add(new XElement("Alt", nowWPI.Hotkey.Alt.ToString()));
                    element7.Add(new XElement("Ctrl", nowWPI.Hotkey.Ctrl.ToString()));
                    element7.Add(new XElement("Shift", nowWPI.Hotkey.Shift.ToString()));
                    element7.Add(new XElement("Windows", nowWPI.Hotkey.Windows.ToString()));
                    element7.Add(new XElement("KeyCharacter", nowWPI.Hotkey.KeyCharacter.ToString()));
                    element6.Add(element7);
                    element5.Add(element6);
                }
                element4.Add(element5);
                // End WindowProcessingInformation
                element4.Add(new XElement("DoNotProcessingTitleNameConditions", Enum.GetName(typeof(TitleNameProcessingConditions), nowItem.DoNotProcessingTitleNameConditions)));
                // Start DoNotProcessingStringContainedInTitleName
                element5 = new("DoNotProcessingStringContainedInTitleName");
                foreach (string nowTitleName in nowItem.DoNotProcessingStringContainedInTitleName)
                {
                    element6 = new("Item");
                    element6.Add(new XElement("String", nowTitleName));
                    element5.Add(element6);
                }
                element4.Add(element5);
                // End DoNotProcessingStringContainedInTitleName
                // Start DoNotProcessingSize
                element5 = new("DoNotProcessingSize");
                foreach (System.Drawing.Size nowSize in nowItem.DoNotProcessingSize)
                {
                    element6 = new("Item");
                    element6.Add(new XElement("Width", nowSize.Width.ToString()));
                    element6.Add(new XElement("Height", nowSize.Height.ToString()));
                    element5.Add(element6);
                }
                element4.Add(element5);
                // End DoNotProcessingSize
                element4.Add(new XElement("DoNotProcessingOtherThanSpecifiedVersion", nowItem.DoNotProcessingOtherThanSpecifiedVersion));
                element4.Add(new XElement("DoNotProcessingOtherThanSpecifiedVersionAnnounce", nowItem.DoNotProcessingOtherThanSpecifiedVersionAnnounce.ToString()));
                element3.Add(element4);
            }
            element2.Add(element3);
            element1.Add(element2);
            // End SpecifyWindowInformation
            // Start MagnetInformation
            element2 = new("MagnetInformation");
            element2.Add(new XElement("Enabled", ApplicationData.Settings.MagnetInformation.Enabled.ToString()));
            element2.Add(new XElement("PasteToEdgeOfScreen", ApplicationData.Settings.MagnetInformation.PasteToEdgeOfScreen.ToString()));
            element2.Add(new XElement("PasteToAnotherWindow", ApplicationData.Settings.MagnetInformation.PasteToAnotherWindow.ToString()));
            element2.Add(new XElement("PasteToPressKey", ApplicationData.Settings.MagnetInformation.PasteToPressKey.ToString()));
            element2.Add(new XElement("PastingDecisionDistance", ApplicationData.Settings.MagnetInformation.PastingDecisionDistance.ToString()));
            element2.Add(new XElement("StopTimeWhenPasted", ApplicationData.Settings.MagnetInformation.StopTimeWhenPasted.ToString()));
            element2.Add(new XElement("StopProcessingFullScreen", ApplicationData.Settings.MagnetInformation.StopProcessingFullScreen.ToString()));
            element1.Add(element2);
            // End MagnetInformation
            // Start HotkeyInformation
            element2 = new("HotkeyInformation");
            element2.Add(new XElement("Enabled", ApplicationData.Settings.HotkeyInformation.Enabled.ToString()));
            element2.Add(new XElement("DoNotChangeOutOfScreen", ApplicationData.Settings.HotkeyInformation.DoNotChangeOutOfScreen.ToString()));
            element2.Add(new XElement("StopProcessingFullScreen", ApplicationData.Settings.HotkeyInformation.StopProcessingFullScreen.ToString()));
            element3 = new("AddModifyWindowSize");
            element3.Add(new XElement("Width", ApplicationData.Settings.HotkeyInformation.AddModifyWindowSize.Width.ToString()));
            element3.Add(new XElement("Height", ApplicationData.Settings.HotkeyInformation.AddModifyWindowSize.Height.ToString()));
            element2.Add(element3);
            element3 = new("Items");
            foreach (HotkeyItemInformation nowItem in ApplicationData.Settings.HotkeyInformation.Items)
            {
                element4 = new("Item");
                element4.Add(new XElement("RegisteredName", nowItem.RegisteredName));
                element4.Add(new XElement("ProcessingType", Enum.GetName(typeof(HotkeyProcessingType), nowItem.ProcessingType)));
                element4.Add(new XElement("StandardDisplay", Enum.GetName(typeof(StandardDisplay), nowItem.StandardDisplay)));
                element5 = new("PositionSize");
                element5.Add(new XElement("Display", nowItem.PositionSize.Display));
                element5.Add(new XElement("SettingsWindowState", Enum.GetName(typeof(SettingsWindowState), nowItem.PositionSize.SettingsWindowState)));
                element5.Add(new XElement("X", nowItem.PositionSize.X.ToString()));
                element5.Add(new XElement("Y", nowItem.PositionSize.Y.ToString()));
                element5.Add(new XElement("Width", nowItem.PositionSize.Width.ToString()));
                element5.Add(new XElement("Height", nowItem.PositionSize.Height.ToString()));
                element5.Add(new XElement("XType", Enum.GetName(typeof(WindowXType), nowItem.PositionSize.XType)));
                element5.Add(new XElement("YType", Enum.GetName(typeof(WindowYType), nowItem.PositionSize.YType)));
                element5.Add(new XElement("WidthType", Enum.GetName(typeof(WindowSizeType), nowItem.PositionSize.WidthType)));
                element5.Add(new XElement("HeightType", Enum.GetName(typeof(WindowSizeType), nowItem.PositionSize.HeightType)));
                element5.Add(new XElement("XValueType", Enum.GetName(typeof(PositionSizeValueType), nowItem.PositionSize.XValueType)));
                element5.Add(new XElement("YValueType", Enum.GetName(typeof(PositionSizeValueType), nowItem.PositionSize.YValueType)));
                element5.Add(new XElement("WidthValueType", Enum.GetName(typeof(PositionSizeValueType), nowItem.PositionSize.WidthValueType)));
                element5.Add(new XElement("HeightValueType", Enum.GetName(typeof(PositionSizeValueType), nowItem.PositionSize.HeightValueType)));
                element5.Add(new XElement("ClientArea", nowItem.PositionSize.ClientArea.ToString()));
                element4.Add(element5);
                element4.Add(new XElement("ProcessingValue", nowItem.ProcessingValue.ToString()));
                element5 = new("Hotkey");
                element5.Add(new XElement("Alt", nowItem.Hotkey.Alt.ToString()));
                element5.Add(new XElement("Ctrl", nowItem.Hotkey.Ctrl.ToString()));
                element5.Add(new XElement("Shift", nowItem.Hotkey.Shift.ToString()));
                element5.Add(new XElement("Windows", nowItem.Hotkey.Windows.ToString()));
                element5.Add(new XElement("KeyCharacter", nowItem.Hotkey.KeyCharacter.ToString()));
                element4.Add(element5);
                element3.Add(element4);
            }
            element2.Add(element3);
            element1.Add(element2);
            // End HotkeyInformation

            document.Add(element1);

            document.Save(path);
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
            XDocument document = XDocument.Load(path);
            XElement? element = document.Element("Settings");

            if (element != null)
            {
                XElement? elementNode1;

                // Start MainWindowRectangle
                elementNode1 = element.Element("MainWindowRectangle");
                if (elementNode1 != null)
                {
                    ApplicationData.Settings.MainWindowRectangle.X = VariousProcessing.GetIntXElement(elementNode1, "X", ApplicationData.Settings.MainWindowRectangle.X);
                    ApplicationData.Settings.MainWindowRectangle.Y = VariousProcessing.GetIntXElement(elementNode1, "Y", ApplicationData.Settings.MainWindowRectangle.Y);
                    ApplicationData.Settings.MainWindowRectangle.Width = VariousProcessing.GetIntXElement(elementNode1, "Width", ApplicationData.Settings.MainWindowRectangle.Width);
                    ApplicationData.Settings.MainWindowRectangle.Height = VariousProcessing.GetIntXElement(elementNode1, "Height", ApplicationData.Settings.MainWindowRectangle.Height);
                }
                // End MainWindowRectangle
                _ = Enum.TryParse(VariousProcessing.GetStringXElement(element, "WindowStateMainWindow", Enum.GetName(typeof(WindowState), ApplicationData.Settings.WindowStateMainWindow) ?? ""), out ApplicationData.Settings.WindowStateMainWindow);
                ApplicationData.Settings.Language = VariousProcessing.GetStringXElement(element, "Language", ApplicationData.Settings.Language);
                _ = Enum.TryParse(VariousProcessing.GetStringXElement(element, "CoordinateType", Enum.GetName(typeof(CoordinateType), ApplicationData.Settings.CoordinateType) ?? ""), out ApplicationData.Settings.CoordinateType);
                ApplicationData.Settings.DarkMode = VariousProcessing.GetBoolXElement(element, "DarkMode", ApplicationData.Settings.DarkMode);
                ApplicationData.Settings.AutomaticallyUpdateCheck = VariousProcessing.GetBoolXElement(element, "AutomaticallyUpdateCheck", ApplicationData.Settings.AutomaticallyUpdateCheck);
                ApplicationData.Settings.CheckBetaVersion = VariousProcessing.GetBoolXElement(element, "CheckBetaVersion", ApplicationData.Settings.CheckBetaVersion);
                ApplicationData.Settings.UseLongPath = VariousProcessing.GetBoolXElement(element, "UseLongPath", ApplicationData.Settings.UseLongPath);
                // Start ShiftPastePosition
                elementNode1 = element.Element("ShiftPastePosition");
                if (elementNode1 != null)
                {
                    ApplicationData.Settings.ShiftPastePosition.Enabled = VariousProcessing.GetBoolXElement(elementNode1, "Enabled", ApplicationData.Settings.ShiftPastePosition.Enabled);
                    ApplicationData.Settings.ShiftPastePosition.Left = VariousProcessing.GetIntXElement(elementNode1, "Left", ApplicationData.Settings.ShiftPastePosition.Left);
                    ApplicationData.Settings.ShiftPastePosition.Top = VariousProcessing.GetIntXElement(elementNode1, "Top", ApplicationData.Settings.ShiftPastePosition.Top);
                    ApplicationData.Settings.ShiftPastePosition.Right = VariousProcessing.GetIntXElement(elementNode1, "Right", ApplicationData.Settings.ShiftPastePosition.Right);
                    ApplicationData.Settings.ShiftPastePosition.Bottom = VariousProcessing.GetIntXElement(elementNode1, "Bottom", ApplicationData.Settings.ShiftPastePosition.Bottom);
                }
                // End ShiftPastePosition
                // Start SpecifyWindowInformation
                elementNode1 = element.Element("SpecifyWindowInformation");
                if (elementNode1 != null)
                {
                    ApplicationData.Settings.SpecifyWindowInformation.Enabled = VariousProcessing.GetBoolXElement(elementNode1, "Enabled", ApplicationData.Settings.SpecifyWindowInformation.Enabled);
                    ApplicationData.Settings.SpecifyWindowInformation.MultipleRegistrations = VariousProcessing.GetBoolXElement(elementNode1, "MultipleRegistrations", ApplicationData.Settings.SpecifyWindowInformation.MultipleRegistrations);
                    ApplicationData.Settings.SpecifyWindowInformation.CaseSensitive = VariousProcessing.GetBoolXElement(elementNode1, "CaseSensitive", ApplicationData.Settings.SpecifyWindowInformation.CaseSensitive);
                    ApplicationData.Settings.SpecifyWindowInformation.DoNotChangeOutOfScreen = VariousProcessing.GetBoolXElement(elementNode1, "DoNotChangeOutOfScreen", ApplicationData.Settings.SpecifyWindowInformation.DoNotChangeOutOfScreen);
                    ApplicationData.Settings.SpecifyWindowInformation.StopProcessingShowAddModifyWindow = VariousProcessing.GetBoolXElement(elementNode1, "StopProcessingShowAddModifyWindow", ApplicationData.Settings.SpecifyWindowInformation.StopProcessingShowAddModifyWindow);
                    ApplicationData.Settings.SpecifyWindowInformation.StopProcessingFullScreen = VariousProcessing.GetBoolXElement(elementNode1, "StopProcessingFullScreen", ApplicationData.Settings.SpecifyWindowInformation.StopProcessingFullScreen);
                    ApplicationData.Settings.SpecifyWindowInformation.HotkeysDoNotStopFullScreen = VariousProcessing.GetBoolXElement(elementNode1, "HotkeysDoNotStopFullScreen", ApplicationData.Settings.SpecifyWindowInformation.HotkeysDoNotStopFullScreen);
                    ApplicationData.Settings.SpecifyWindowInformation.ProcessingInterval = VariousProcessing.GetIntXElement(elementNode1, "ProcessingInterval", ApplicationData.Settings.SpecifyWindowInformation.ProcessingInterval);
                    _ = Enum.TryParse(VariousProcessing.GetStringXElement(elementNode1, "ProcessingWindowRange", Enum.GetName(typeof(ProcessingWindowRange), ApplicationData.Settings.SpecifyWindowInformation.ProcessingWindowRange) ?? ""), out ApplicationData.Settings.SpecifyWindowInformation.ProcessingWindowRange);
                    ApplicationData.Settings.SpecifyWindowInformation.WaitTimeToProcessingNextWindow = VariousProcessing.GetIntXElement(elementNode1, "WaitTimeToProcessingNextWindow", ApplicationData.Settings.SpecifyWindowInformation.WaitTimeToProcessingNextWindow);
                    XElement? elementNode2;
                    elementNode2 = elementNode1.Element("AddModifyWindowSize");
                    if (elementNode2 != null)
                    {
                        ApplicationData.Settings.SpecifyWindowInformation.AddModifyWindowSize.Width = VariousProcessing.GetIntXElement(elementNode2, "Width", ApplicationData.Settings.SpecifyWindowInformation.AddModifyWindowSize.Width);
                        ApplicationData.Settings.SpecifyWindowInformation.AddModifyWindowSize.Height = VariousProcessing.GetIntXElement(elementNode2, "Height", ApplicationData.Settings.SpecifyWindowInformation.AddModifyWindowSize.Height);
                    }
                    elementNode2 = elementNode1.Element("AcquiredItems");
                    if (elementNode2 != null)
                    {
                        ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.TitleName = VariousProcessing.GetBoolXElement(elementNode1, "TitleName", ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.TitleName);
                        ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.ClassName = VariousProcessing.GetBoolXElement(elementNode1, "ClassName", ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.ClassName);
                        ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.FileName = VariousProcessing.GetBoolXElement(elementNode1, "FileName", ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.FileName);
                        ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.Display = VariousProcessing.GetBoolXElement(elementNode1, "Display", ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.Display);
                        ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.WindowState = VariousProcessing.GetBoolXElement(elementNode1, "WindowState", ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.WindowState);
                        ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.X = VariousProcessing.GetBoolXElement(elementNode1, "X", ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.X);
                        ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.Y = VariousProcessing.GetBoolXElement(elementNode1, "Y", ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.Y);
                        ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.Width = VariousProcessing.GetBoolXElement(elementNode1, "Width", ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.Width);
                        ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.Height = VariousProcessing.GetBoolXElement(elementNode1, "Height", ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.Height);
                        ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.Version = VariousProcessing.GetBoolXElement(elementNode1, "Version", ApplicationData.Settings.SpecifyWindowInformation.AcquiredItems.Version);
                    }
                    elementNode2 = elementNode1.Element("Items");
                    if (elementNode2 != null)
                    {
                        // Start SpecifyWindowItemInformation
                        foreach (XElement nowElementEII in elementNode2.Elements("Item"))
                        {
                            SpecifyWindowItemInformation newEII = new();

                            newEII.Enabled = VariousProcessing.GetBoolXElement(nowElementEII, "Enabled", newEII.Enabled);
                            newEII.RegisteredName = VariousProcessing.GetStringXElement(nowElementEII, "RegisteredName", newEII.RegisteredName);
                            newEII.TitleName = VariousProcessing.GetStringXElement(nowElementEII, "TitleName", newEII.TitleName);
                            _ = Enum.TryParse(VariousProcessing.GetStringXElement(nowElementEII, "TitleNameMatchCondition", Enum.GetName(typeof(NameMatchCondition), newEII.TitleNameMatchCondition) ?? ""), out newEII.TitleNameMatchCondition);
                            newEII.ClassName = VariousProcessing.GetStringXElement(nowElementEII, "ClassName", newEII.ClassName);
                            _ = Enum.TryParse(VariousProcessing.GetStringXElement(nowElementEII, "ClassNameMatchCondition", Enum.GetName(typeof(NameMatchCondition), newEII.ClassNameMatchCondition) ?? ""), out newEII.ClassNameMatchCondition);
                            newEII.FileName = VariousProcessing.GetStringXElement(nowElementEII, "FileName", newEII.FileName);
                            _ = Enum.TryParse(VariousProcessing.GetStringXElement(nowElementEII, "FileNameMatchCondition", Enum.GetName(typeof(FileNameMatchCondition), newEII.FileNameMatchCondition) ?? ""), out newEII.FileNameMatchCondition);
                            _ = Enum.TryParse(VariousProcessing.GetStringXElement(nowElementEII, "StandardDisplay", Enum.GetName(typeof(StandardDisplay), newEII.StandardDisplay) ?? ""), out newEII.StandardDisplay);
                            _ = Enum.TryParse(VariousProcessing.GetStringXElement(nowElementEII, "ProcessingOnlyOnce", Enum.GetName(typeof(ProcessingOnlyOnce), newEII.ProcessingOnlyOnce) ?? ""), out newEII.ProcessingOnlyOnce);
                            XElement? elementNode4 = nowElementEII.Element("WindowEventData");
                            if (elementNode4 != null)
                            {
                                newEII.WindowEventData.Foreground = VariousProcessing.GetBoolXElement(elementNode4, "Foreground", newEII.WindowEventData.Foreground);
                                newEII.WindowEventData.MoveSizeEnd = VariousProcessing.GetBoolXElement(elementNode4, "MoveSizeEnd", newEII.WindowEventData.MoveSizeEnd);
                                newEII.WindowEventData.MinimizeStart = VariousProcessing.GetBoolXElement(elementNode4, "MinimizeStart", newEII.WindowEventData.MinimizeStart);
                                newEII.WindowEventData.MinimizeEnd = VariousProcessing.GetBoolXElement(elementNode4, "MinimizeEnd", newEII.WindowEventData.MinimizeEnd);
                                newEII.WindowEventData.Create = VariousProcessing.GetBoolXElement(elementNode4, "Create", newEII.WindowEventData.Create);
                                newEII.WindowEventData.Show = VariousProcessing.GetBoolXElement(elementNode4, "Show", newEII.WindowEventData.Show);
                                newEII.WindowEventData.NameChange = VariousProcessing.GetBoolXElement(elementNode4, "NameChange", newEII.WindowEventData.NameChange);
                            }
                            newEII.TimerProcessing = VariousProcessing.GetBoolXElement(nowElementEII, "TimerProcessing", newEII.TimerProcessing);
                            newEII.NumberOfTimesNotToProcessingFirst = VariousProcessing.GetIntXElement(nowElementEII, "NumberOfTimesNotToProcessingFirst", newEII.NumberOfTimesNotToProcessingFirst);
                            // Start WindowProcessingInformation
                            XElement? elementNode5 = nowElementEII.Element("WindowProcessingInformation");
                            if (elementNode5 != null)
                            {
                                foreach (XElement nowElement in elementNode5.Elements("WindowProcessingInformation"))
                                {
                                    WindowProcessingInformation newWPI = new();

                                    newWPI.Active = VariousProcessing.GetBoolXElement(nowElement, "Active", newWPI.Active);
                                    newWPI.ProcessingName = VariousProcessing.GetStringXElement(nowElement, "ProcessingName", newWPI.ProcessingName);
                                    // Start PositionSize
                                    XElement? elementNode6 = nowElement.Element("PositionSize");
                                    if (elementNode6 != null)
                                    {
                                        newWPI.PositionSize.Display = VariousProcessing.GetStringXElement(elementNode6, "Display", newWPI.PositionSize.Display);
                                        _ = Enum.TryParse(VariousProcessing.GetStringXElement(elementNode6, "SettingsWindowState", Enum.GetName(typeof(SettingsWindowState), newWPI.PositionSize.SettingsWindowState) ?? ""), out newWPI.PositionSize.SettingsWindowState);
                                        newWPI.PositionSize.X = VariousProcessing.GetDoubleXElement(elementNode6, "X", newWPI.PositionSize.X);
                                        newWPI.PositionSize.Y = VariousProcessing.GetDoubleXElement(elementNode6, "Y", newWPI.PositionSize.Y);
                                        newWPI.PositionSize.Width = VariousProcessing.GetDoubleXElement(elementNode6, "Width", newWPI.PositionSize.Width);
                                        newWPI.PositionSize.Height = VariousProcessing.GetDoubleXElement(elementNode6, "Height", newWPI.PositionSize.Height);
                                        _ = Enum.TryParse(VariousProcessing.GetStringXElement(elementNode6, "XType", Enum.GetName(typeof(WindowXType), newWPI.PositionSize.XType) ?? ""), out newWPI.PositionSize.XType);
                                        _ = Enum.TryParse(VariousProcessing.GetStringXElement(elementNode6, "YType", Enum.GetName(typeof(WindowYType), newWPI.PositionSize.YType) ?? ""), out newWPI.PositionSize.YType);
                                        _ = Enum.TryParse(VariousProcessing.GetStringXElement(elementNode6, "WidthType", Enum.GetName(typeof(WindowSizeType), newWPI.PositionSize.WidthType) ?? ""), out newWPI.PositionSize.WidthType);
                                        _ = Enum.TryParse(VariousProcessing.GetStringXElement(elementNode6, "HeightType", Enum.GetName(typeof(WindowSizeType), newWPI.PositionSize.HeightType) ?? ""), out newWPI.PositionSize.HeightType);
                                        PositionSizeValueType value;
                                        _ = Enum.TryParse(VariousProcessing.GetStringXElement(elementNode6, "XValueType", Enum.GetName(typeof(PositionSizeValueType), newWPI.PositionSize.XValueType) ?? ""), out value);
                                        newWPI.PositionSize.XValueType = value;
                                        _ = Enum.TryParse(VariousProcessing.GetStringXElement(elementNode6, "YValueType", Enum.GetName(typeof(PositionSizeValueType), newWPI.PositionSize.YValueType) ?? ""), out value);
                                        newWPI.PositionSize.YValueType = value;
                                        _ = Enum.TryParse(VariousProcessing.GetStringXElement(elementNode6, "WidthValueType", Enum.GetName(typeof(PositionSizeValueType), newWPI.PositionSize.WidthValueType) ?? ""), out value);
                                        newWPI.PositionSize.WidthValueType = value;
                                        _ = Enum.TryParse(VariousProcessing.GetStringXElement(elementNode6, "HeightValueType", Enum.GetName(typeof(PositionSizeValueType), newWPI.PositionSize.HeightValueType) ?? ""), out value);
                                        newWPI.PositionSize.HeightValueType = value;
                                        newWPI.PositionSize.ClientArea = VariousProcessing.GetBoolXElement(elementNode6, "ClientArea", newWPI.PositionSize.ClientArea);
                                    }
                                    // End PositionSize
                                    newWPI.NormalWindowOnly = VariousProcessing.GetBoolXElement(nowElement, "NormalWindowOnly", newWPI.NormalWindowOnly);
                                    _ = Enum.TryParse(VariousProcessing.GetStringXElement(nowElement, "Forefront", Enum.GetName(typeof(Forefront), newWPI.Forefront) ?? ""), out newWPI.Forefront);
                                    newWPI.EnabledTransparency = VariousProcessing.GetBoolXElement(nowElement, "EnabledTransparency", newWPI.EnabledTransparency);
                                    newWPI.Transparency = VariousProcessing.GetIntXElement(nowElement, "Transparency", newWPI.Transparency);
                                    newWPI.CloseWindow = VariousProcessing.GetBoolXElement(nowElement, "CloseWindow", newWPI.CloseWindow);
                                    XElement? elementNode7 = nowElement.Element("Hotkey");
                                    if (elementNode7 != null)
                                    {
                                        newWPI.Hotkey.Alt = VariousProcessing.GetBoolXElement(elementNode7, "Alt", newWPI.Hotkey.Alt);
                                        newWPI.Hotkey.Ctrl = VariousProcessing.GetBoolXElement(elementNode7, "Ctrl", newWPI.Hotkey.Ctrl);
                                        newWPI.Hotkey.Shift = VariousProcessing.GetBoolXElement(elementNode7, "Shift", newWPI.Hotkey.Shift);
                                        newWPI.Hotkey.Windows = VariousProcessing.GetBoolXElement(elementNode7, "Windows", newWPI.Hotkey.Windows);
                                        newWPI.Hotkey.KeyCharacter = VariousProcessing.GetIntXElement(elementNode7, "KeyCharacter", newWPI.Hotkey.KeyCharacter);
                                    }

                                    newEII.WindowProcessingInformation.Add(newWPI);
                                }
                            }
                            // End WindowProcessingInformation
                            _ = Enum.TryParse(VariousProcessing.GetStringXElement(nowElementEII, "DoNotProcessingTitleNameConditions", Enum.GetName(typeof(TitleNameProcessingConditions), newEII.DoNotProcessingTitleNameConditions) ?? ""), out newEII.DoNotProcessingTitleNameConditions);
                            // Start DoNotProcessingStringContainedInTitleName
                            elementNode5 = nowElementEII.Element("DoNotProcessingStringContainedInTitleName");
                            if (elementNode5 != null)
                            {
                                foreach (XElement nowElement in elementNode5.Elements("Item"))
                                {
                                    string newString = "";
                                    newString = VariousProcessing.GetStringXElement(nowElement, "String", newString);
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
                                    System.Drawing.Size newSize = new();
                                    newSize.Width = VariousProcessing.GetIntXElement(nowElement, "Width", newSize.Width);
                                    newSize.Height = VariousProcessing.GetIntXElement(nowElement, "Height", newSize.Height);
                                    newEII.DoNotProcessingSize.Add(newSize);
                                }
                            }
                            // End DoNotProcessingSize
                            newEII.DoNotProcessingOtherThanSpecifiedVersion = VariousProcessing.GetStringXElement(nowElementEII, "DoNotProcessingOtherThanSpecifiedVersion", newEII.DoNotProcessingOtherThanSpecifiedVersion);
                            newEII.DoNotProcessingOtherThanSpecifiedVersionAnnounce = VariousProcessing.GetBoolXElement(nowElementEII, "DoNotProcessingOtherThanSpecifiedVersionAnnounce", newEII.DoNotProcessingOtherThanSpecifiedVersionAnnounce);

                            ApplicationData.Settings.SpecifyWindowInformation.Items.Add(newEII);
                        }
                        // End SpecifyWindowItemInformation
                    }
                }
                // End SpecifyWindowInformation
                // Start MagnetInformation
                elementNode1 = element.Element("MagnetInformation");
                if (elementNode1 != null)
                {
                    ApplicationData.Settings.MagnetInformation.Enabled = VariousProcessing.GetBoolXElement(elementNode1, "Enabled", ApplicationData.Settings.MagnetInformation.Enabled);
                    ApplicationData.Settings.MagnetInformation.PasteToEdgeOfScreen = VariousProcessing.GetBoolXElement(elementNode1, "PasteToEdgeOfScreen", ApplicationData.Settings.MagnetInformation.PasteToEdgeOfScreen);
                    ApplicationData.Settings.MagnetInformation.PasteToAnotherWindow = VariousProcessing.GetBoolXElement(elementNode1, "PasteToAnotherWindow", ApplicationData.Settings.MagnetInformation.PasteToAnotherWindow);
                    ApplicationData.Settings.MagnetInformation.PasteToPressKey = VariousProcessing.GetBoolXElement(elementNode1, "PasteToPressKey", ApplicationData.Settings.MagnetInformation.PasteToPressKey);
                    ApplicationData.Settings.MagnetInformation.PastingDecisionDistance = VariousProcessing.GetIntXElement(elementNode1, "PastingDecisionDistance", ApplicationData.Settings.MagnetInformation.PastingDecisionDistance);
                    ApplicationData.Settings.MagnetInformation.StopTimeWhenPasted = VariousProcessing.GetIntXElement(elementNode1, "StopTimeWhenPasted", ApplicationData.Settings.MagnetInformation.StopTimeWhenPasted);
                    ApplicationData.Settings.MagnetInformation.StopProcessingFullScreen = VariousProcessing.GetBoolXElement(elementNode1, "StopProcessingFullScreen", ApplicationData.Settings.MagnetInformation.StopProcessingFullScreen);
                }
                // End MagnetInformation
                // Start HotkeyInformation
                elementNode1 = element.Element("HotkeyInformation");
                if (elementNode1 != null)
                {
                    ApplicationData.Settings.HotkeyInformation.Enabled = VariousProcessing.GetBoolXElement(elementNode1, "Enabled", ApplicationData.Settings.HotkeyInformation.Enabled);
                    ApplicationData.Settings.HotkeyInformation.DoNotChangeOutOfScreen = VariousProcessing.GetBoolXElement(elementNode1, "DoNotChangeOutOfScreen", ApplicationData.Settings.HotkeyInformation.DoNotChangeOutOfScreen);
                    ApplicationData.Settings.HotkeyInformation.StopProcessingFullScreen = VariousProcessing.GetBoolXElement(elementNode1, "StopProcessingFullScreen", ApplicationData.Settings.HotkeyInformation.StopProcessingFullScreen);
                    XElement? elementNode2 = elementNode1.Element("AddModifyWindowSize");
                    if (elementNode2 != null)
                    {
                        ApplicationData.Settings.HotkeyInformation.AddModifyWindowSize.Width = VariousProcessing.GetIntXElement(elementNode2, "Width", ApplicationData.Settings.HotkeyInformation.AddModifyWindowSize.Width);
                        ApplicationData.Settings.HotkeyInformation.AddModifyWindowSize.Height = VariousProcessing.GetIntXElement(elementNode2, "Height", ApplicationData.Settings.HotkeyInformation.AddModifyWindowSize.Height);
                    }
                    // Start Items
                    elementNode2 = elementNode1.Element("Items");
                    if (elementNode2 != null)
                    {
                        foreach (XElement nowElementHII in elementNode2.Elements("Item"))
                        {
                            HotkeyItemInformation newHII = new();

                            newHII.RegisteredName = VariousProcessing.GetStringXElement(nowElementHII, "RegisteredName", newHII.RegisteredName);
                            HotkeyProcessingType hptValue;
                            _ = Enum.TryParse(VariousProcessing.GetStringXElement(nowElementHII, "ProcessingType", System.Enum.GetName(typeof(HotkeyProcessingType), newHII.ProcessingType) ?? ""), out hptValue);
                            newHII.ProcessingType = hptValue;
                            _ = Enum.TryParse(VariousProcessing.GetStringXElement(nowElementHII, "StandardDisplay", Enum.GetName(typeof(StandardDisplay), newHII.StandardDisplay) ?? ""), out newHII.StandardDisplay);
                            XElement? elementNode3 = nowElementHII.Element("PositionSize");
                            if (elementNode3 != null )
                            {
                                newHII.PositionSize.Display = VariousProcessing.GetStringXElement(elementNode3, "Display", newHII.PositionSize.Display);
                                _ = Enum.TryParse(VariousProcessing.GetStringXElement(elementNode3, "SettingsWindowState", Enum.GetName(typeof(SettingsWindowState), newHII.PositionSize.SettingsWindowState) ?? ""), out newHII.PositionSize.SettingsWindowState);
                                newHII.PositionSize.X = VariousProcessing.GetDoubleXElement(elementNode3, "X", newHII.PositionSize.X);
                                newHII.PositionSize.Y = VariousProcessing.GetDoubleXElement(elementNode3, "Y", newHII.PositionSize.Y);
                                newHII.PositionSize.Width = VariousProcessing.GetDoubleXElement(elementNode3, "Width", newHII.PositionSize.Width);
                                newHII.PositionSize.Height = VariousProcessing.GetDoubleXElement(elementNode3, "Height", newHII.PositionSize.Height);
                                _ = Enum.TryParse(VariousProcessing.GetStringXElement(elementNode3, "XType", Enum.GetName(typeof(WindowXType), newHII.PositionSize.XType) ?? ""), out newHII.PositionSize.XType);
                                _ = Enum.TryParse(VariousProcessing.GetStringXElement(elementNode3, "YType", Enum.GetName(typeof(WindowYType), newHII.PositionSize.YType) ?? ""), out newHII.PositionSize.YType);
                                _ = Enum.TryParse(VariousProcessing.GetStringXElement(elementNode3, "WidthType", Enum.GetName(typeof(WindowSizeType), newHII.PositionSize.WidthType) ?? ""), out newHII.PositionSize.WidthType);
                                _ = Enum.TryParse(VariousProcessing.GetStringXElement(elementNode3, "HeightType", Enum.GetName(typeof(WindowSizeType), newHII.PositionSize.HeightType) ?? ""), out newHII.PositionSize.HeightType);
                                PositionSizeValueType value;
                                _ = Enum.TryParse(VariousProcessing.GetStringXElement(elementNode3, "XValueType", Enum.GetName(typeof(PositionSizeValueType), newHII.PositionSize.XValueType) ?? ""), out value);
                                newHII.PositionSize.XValueType = value;
                                _ = Enum.TryParse(VariousProcessing.GetStringXElement(elementNode3, "YValueType", Enum.GetName(typeof(PositionSizeValueType), newHII.PositionSize.YValueType) ?? ""), out value);
                                newHII.PositionSize.YValueType = value;
                                _ = Enum.TryParse(VariousProcessing.GetStringXElement(elementNode3, "WidthValueType", Enum.GetName(typeof(PositionSizeValueType), newHII.PositionSize.WidthValueType) ?? ""), out value);
                                newHII.PositionSize.WidthValueType = value;
                                _ = Enum.TryParse(VariousProcessing.GetStringXElement(elementNode3, "HeightValueType", Enum.GetName(typeof(PositionSizeValueType), newHII.PositionSize.HeightValueType) ?? ""), out value);
                                newHII.PositionSize.HeightValueType = value;
                                newHII.PositionSize.ClientArea = VariousProcessing.GetBoolXElement(elementNode3, "ClientArea", newHII.PositionSize.ClientArea);
                            }
                            newHII.ProcessingValue = VariousProcessing.GetIntXElement(nowElementHII, "ProcessingValue", newHII.ProcessingValue);
                            elementNode3 = nowElementHII.Element("Hotkey");
                            if (elementNode3 != null)
                            {
                                newHII.Hotkey.Alt = VariousProcessing.GetBoolXElement(elementNode3, "Alt", newHII.Hotkey.Alt);
                                newHII.Hotkey.Ctrl = VariousProcessing.GetBoolXElement(elementNode3, "Ctrl", newHII.Hotkey.Ctrl);
                                newHII.Hotkey.Shift = VariousProcessing.GetBoolXElement(elementNode3, "Shift", newHII.Hotkey.Shift);
                                newHII.Hotkey.Windows = VariousProcessing.GetBoolXElement(elementNode3, "Windows", newHII.Hotkey.Windows);
                                newHII.Hotkey.KeyCharacter = VariousProcessing.GetIntXElement(elementNode3, "KeyCharacter", newHII.Hotkey.KeyCharacter);
                            }

                            ApplicationData.Settings.HotkeyInformation.Items.Add(newHII);
                        }
                    }
                    // End Items
                }
                // End HotkeyInformation

                result = true;
            }
        }
        catch
        {
        }

        return result;
    }
}
