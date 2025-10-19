namespace Swindom;

/// <summary>
/// ファイル処理
/// </summary>
public static class FileProcessing
{
    /// <summary>
    /// 読み書きで使用するJsonSerializerOptions
    /// </summary>
    public static JsonSerializerOptions JsonSerializerOptions { get; } = new()
    {
        Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
        WriteIndented = true,
        IgnoreReadOnlyProperties = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

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
    /// JsonElementからstring型の値を取り出す
    /// </summary>
    /// <param name="element">JsonElement</param>
    /// <param name="propertyName">プロパティ名</param>
    /// <param name="defaultValue">既定値</param>
    /// <returns>取り出した値</returns>
    public static string GetStringJsonElement(
        JsonElement element,
        string propertyName,
        string defaultValue
        )
    {
        string returnValue = defaultValue;        // 取り出した値

        try
        {
            if (element.TryGetProperty(propertyName, out JsonElement getElement))
            {
                string? getData = getElement.GetString();
                if (getData != null)
                {
                    returnValue = getData;
                }
            }
        }
        catch
        {
        }

        return returnValue;
    }

    /// <summary>
    /// JsonElementからint型の値を取り出す
    /// </summary>
    /// <param name="element">JsonElement</param>
    /// <param name="propertyName">プロパティ名</param>
    /// <param name="defaultValue">既定値</param>
    /// <returns>取り出した値</returns>
    public static int GetIntJsonElement(
        JsonElement element,
        string propertyName,
        int defaultValue
        )
    {
        int returnValue = defaultValue;        // 取り出した値

        try
        {
            if (element.TryGetProperty(propertyName, out JsonElement getElement))
            {
                returnValue = getElement.GetInt32();
            }
        }
        catch
        {
        }

        return returnValue;
    }

    /// <summary>
    /// JsonElementからbool型の値を取り出す
    /// </summary>
    /// <param name="element">JsonElement</param>
    /// <param name="propertyName">プロパティ名</param>
    /// <param name="defaultValue">既定値</param>
    /// <returns>取り出した値</returns>
    public static bool GetBoolJsonElement(
        JsonElement element,
        string propertyName,
        bool defaultValue
        )
    {
        bool returnValue = defaultValue;        // 取り出した値

        try
        {
            if (element.TryGetProperty(propertyName, out JsonElement getElement))
            {
                returnValue = getElement.GetBoolean();
            }
        }
        catch
        {
        }

        return returnValue;
    }

    /// <summary>
    /// JsonElementからdouble型の値を取り出す
    /// </summary>
    /// <param name="element">JsonElement</param>
    /// <param name="propertyName">プロパティ名</param>
    /// <param name="defaultValue">既定値</param>
    /// <returns>取り出した値</returns>
    public static double GetDoubleJsonElement(
        JsonElement element,
        string propertyName,
        double defaultValue
        )
    {
        double returnValue = defaultValue;        // 取り出した値

        try
        {
            if (element.TryGetProperty(propertyName, out JsonElement getElement))
            {
                returnValue = getElement.GetDouble();
            }
        }
        catch
        {
        }

        return returnValue;
    }

    /// <summary>
    /// JsonElementからfloat型の値を取り出す
    /// </summary>
    /// <param name="element">JsonElement</param>
    /// <param name="propertyName">プロパティ名</param>
    /// <param name="defaultValue">既定値</param>
    /// <returns>取り出した値</returns>
    public static float GetFloatJsonElement(
        JsonElement element,
        string propertyName,
        float defaultValue
        )
    {
        float returnValue = defaultValue;        // 取り出した値

        try
        {
            if (element.TryGetProperty(propertyName, out JsonElement getElement))
            {
                returnValue = (float)getElement.GetDouble();
            }
        }
        catch
        {
        }

        return returnValue;
    }

    /// <summary>
    /// JsonElementからdecimal型の値を取り出す
    /// </summary>
    /// <param name="element">JsonElement</param>
    /// <param name="propertyName">プロパティ名</param>
    /// <param name="defaultValue">既定値</param>
    /// <returns>取り出した値</returns>
    public static decimal GetDecimalJsonElement(
        JsonElement element,
        string propertyName,
        decimal defaultValue
        )
    {
        decimal returnValue = defaultValue;        // 取り出した値

        try
        {
            if (element.TryGetProperty(propertyName, out JsonElement getElement))
            {
                returnValue = getElement.GetDecimal();
            }
        }
        catch
        {
        }

        return returnValue;
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
}
