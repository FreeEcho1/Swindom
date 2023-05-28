namespace Swindom;

/// <summary>
/// 処理
/// </summary>
public static class VariousProcessing
{
    /// <summary>
    /// このアプリケーションのパスを取得
    /// </summary>
    /// <returns>パス</returns>
    public static string OwnApplicationPath() => Path.GetFullPath(Process.GetCurrentProcess().MainModule?.FileName ?? Common.ApplicationFileName);

    /// <summary>
    /// このアプリケーションのディレクトリを取得
    /// </summary>
    /// <param name="changePath">デバッグの場合にパスを変更するかの値 (変更しない「false」/変更する「true」)</param>
    /// <returns>パス</returns>
    public static string GetApplicationDirectory(
        bool changePath = true
        )
    {
        string path = Path.GetDirectoryName(OwnApplicationPath()) ?? "";
#if DEBUG
        if (changePath)
        {
            path += Path.DirectorySeparatorChar + ".." + Path.DirectorySeparatorChar + ".." + Path.DirectorySeparatorChar + ".." + Path.DirectorySeparatorChar;
        }
#endif
        return path;
    }

    /// <summary>
    /// このアプリケーションのファイル名を取得
    /// </summary>
    /// <returns>このアプリケーションのファイル名</returns>
    public static string GetApplicationFileName()
    {
        return Path.GetFileName(OwnApplicationPath());
    }

    /// <summary>
    /// 言語ディレクトリのパスを取得
    /// </summary>
    /// <returns>パス</returns>
    public static string GetLanguageDirectoryPath() => (GetApplicationDirectory() + Path.DirectorySeparatorChar + Common.LanguagesDirectoryName + Path.DirectorySeparatorChar);

    /// <summary>
    /// インストールされているか確認
    /// </summary>
    /// <returns>インストール状態 (インストールされていない「false」/インストールされている「true」)</returns>
    public static bool CheckInstalled()
    {
        bool result = false;        // インストール状態

        try
        {
            using Microsoft.Win32.RegistryKey? registryKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE/Microsoft/Windows/CurrentVersion/Uninstall/" + Common.ApplicationName);
            if (registryKey != null)
            {
                result = true;
            }
        }
        catch
        {
        }

        return result;
    }

    /// <summary>
    /// ComboBoxの項目を選択
    /// </summary>
    /// <param name="comboBox">ComboBox</param>
    /// <param name="stringData">文字列</param>
    public static void SelectComboBoxItem(
        ComboBox comboBox,
        string stringData
        )
    {
        foreach (ComboBoxItem nowItem in comboBox.Items)
        {
            if (stringData == nowItem.Content as string)
            {
                comboBox.SelectedItem = nowItem;
                break;
            }
        }
    }

    /// <summary>
    /// XElementからstring型の値を取り出す
    /// </summary>
    /// <param name="element">XElement</param>
    /// <param name="name">名前</param>
    /// <param name="defaultValue">既定値</param>
    /// <returns>取り出した値</returns>
    public static string GetStringXElement(
        XElement? element,
        string name,
        string defaultValue
        )
    {
        string returnValue = defaultValue;        // 取り出した値

        try
        {
            XElement? targetElement = element?.Element(name);
            if (targetElement != null)
            {
                returnValue = targetElement.Value;
            }
        }
        catch
        {
        }

        return returnValue;
    }

    /// <summary>
    /// XElementからint型の値を取り出す
    /// </summary>
    /// <param name="element">XElement</param>
    /// <param name="name">名前</param>
    /// <param name="defaultValue">既定値</param>
    /// <returns>取り出した値</returns>
    public static int GetIntXElement(
        XElement? element,
        string name,
        int defaultValue
        )
    {
        int returnValue = defaultValue;        // 取り出した値

        try
        {
            XElement? targetElement = element?.Element(name);
            if (targetElement != null)
            {
                returnValue = int.Parse(targetElement.Value);
            }
        }
        catch
        {
        }

        return returnValue;
    }

    /// <summary>
    /// XElementからbool型の値を取り出す
    /// </summary>
    /// <param name="element">XElement</param>
    /// <param name="name">名前</param>
    /// <param name="defaultValue">既定値</param>
    /// <returns>取り出した値</returns>
    public static bool GetBoolXElement(
        XElement? element,
        string name,
        bool defaultValue
        )
    {
        bool returnValue = defaultValue;        // 取り出した値

        try
        {
            XElement? targetElement = element?.Element(name);
            if (targetElement != null)
            {
                returnValue = bool.Parse(targetElement.Value);
            }
        }
        catch
        {
        }

        return returnValue;
    }

    /// <summary>
    /// XElementからdouble型の値を取り出す
    /// </summary>
    /// <param name="element">XElement</param>
    /// <param name="name">名前</param>
    /// <param name="defaultValue">既定値</param>
    /// <returns>取り出した値</returns>
    public static double GetDoubleXElement(
        XElement? element,
        string name,
        double defaultValue
        )
    {
        double returnValue = defaultValue;        // 取り出した値

        try
        {
            XElement? targetElement = element?.Element(name);
            if (targetElement != null)
            {
                returnValue = double.Parse(targetElement.Value);
            }
        }
        catch
        {
        }

        return returnValue;
    }

    /// <summary>
    /// XElementからfloat型の値を取り出す
    /// </summary>
    /// <param name="element">XElement</param>
    /// <param name="name">名前</param>
    /// <param name="defaultValue">既定値</param>
    /// <returns>取り出した値</returns>
    public static float GetFloatXElement(
        XElement? element,
        string name,
        float defaultValue
        )
    {
        float returnValue = defaultValue;        // 取り出した値

        try
        {
            XElement? targetElement = element?.Element(name);
            if (targetElement != null)
            {
                returnValue = float.Parse(targetElement.Value);
            }
        }
        catch
        {
        }

        return returnValue;
    }

    /// <summary>
    /// XElementからdecimal型の値を取り出す
    /// </summary>
    /// <param name="element">XElement</param>
    /// <param name="name">名前</param>
    /// <param name="defaultValue">既定値</param>
    /// <returns>取り出した値</returns>
    public static decimal GetDecimalXElement(
        XElement? element,
        string name,
        decimal defaultValue
        )
    {
        decimal returnValue = defaultValue;        // 取り出した値

        try
        {
            XElement? targetElement = element?.Element(name);
            if (targetElement != null)
            {
                returnValue = decimal.Parse(targetElement.Value);
            }
        }
        catch
        {
        }

        return returnValue;
    }

    /// <summary>
    /// ホットキーを登録
    /// </summary>
    /// <param name="hotkeyInformation">ホットキー情報</param>
    /// <param name="id">ホットキーの識別子</param>
    /// <returns>結果 (失敗「false」/成功「true」)</returns>
    public static bool RegisterHotkey(
        FreeEcho.FEHotKeyWPF.HotKeyInformation hotkeyInformation,
        int id
        )
    {
        bool result = false;        // 結果
        uint modKey = 0;       // Modifiers

        if (hotkeyInformation.Alt)
        {
            modKey |= (int)MOD.MOD_ALT;
        }
        if (hotkeyInformation.Ctrl)
        {
            modKey |= (int)MOD.MOD_CONTROL;
        }
        if (hotkeyInformation.Shift)
        {
            modKey |= (int)MOD.MOD_SHIFT;
        }
        if (hotkeyInformation.Windows)
        {
            modKey |= (int)MOD.MOD_WIN;
        }
        if (modKey != 0 || hotkeyInformation.KeyCharacter != 0)
        {
            result = true;
            NativeMethods.RegisterHotKey(ApplicationData.MainHwnd, id, modKey, (uint)hotkeyInformation.KeyCharacter);
        }

        return result;
    }

    /// <summary>
    /// ファイルやウェブページを開く
    /// </summary>
    /// <param name="fileAndUrl">ファイルパス/URL</param>
    public static void OpenFileAndWebPage(
        string path
        )
    {
        ProcessStartInfo info = new()
        {
            FileName = path,
            UseShellExecute = true
        };
        Process.Start(info);
    }

    /// <summary>
    /// 文字列からVersionNumberに変換
    /// </summary>
    /// <param name="versionString">バージョン文字列</param>
    /// <returns></returns>
    public static VersionNumber SplitVersionNumber(
        string versionString
        )
    {
        VersionNumber versionNumber = new();
        string[] splitString = versionString.Split(".");

        if (0 < splitString.Length)
        {
            versionNumber.MajorNumber = int.Parse(splitString[0]);
        }
        if (1 < splitString.Length)
        {
            versionNumber.MinorNumber = int.Parse(splitString[1]);
        }
        if (2 < splitString.Length)
        {
            versionNumber.BuildNumber = int.Parse(splitString[2]);
        }
        if (3 < splitString.Length)
        {
            versionNumber.PrivateNumber = int.Parse(splitString[3]);
        }

        return versionNumber;
    }

    /// <summary>
    /// バージョン比較 (値が「-1」の場合は比較しない)
    /// </summary>
    /// <param name="versionNumber1">バージョン番号1</param>
    /// <param name="versionNumber2">バージョン番号2</param>
    /// <returns>結果 (不一致「false」/一致「true」)</returns>
    public static bool VersionComparison(
        VersionNumber versionNumber1,
        VersionNumber versionNumber2
        )
    {
        if (versionNumber1.MajorNumber != -1 && versionNumber2.MajorNumber != -1)
        {
            if (versionNumber1.MajorNumber != versionNumber2.MajorNumber)
            {
                return false;
            }
        }
        if (versionNumber1.MinorNumber != -1 && versionNumber2.MinorNumber != -1)
        {
            if (versionNumber1.MinorNumber != versionNumber2.MinorNumber)
            {
                return false;
            }
        }
        if (versionNumber1.BuildNumber != -1 && versionNumber2.BuildNumber != -1)
        {
            if (versionNumber1.BuildNumber != versionNumber2.BuildNumber)
            {
                return false;
            }
        }
        if (versionNumber1.PrivateNumber != -1 && versionNumber2.PrivateNumber != -1)
        {
            if (versionNumber1.PrivateNumber != versionNumber2.PrivateNumber)
            {
                return false;
            }
        }

        return true;
    }
}
