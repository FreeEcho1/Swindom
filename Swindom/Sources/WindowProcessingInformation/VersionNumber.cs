namespace Swindom;

/// <summary>
/// バージョン番号
/// </summary>
public class VersionNumber
{
    /// <summary>
    /// メジャー番号 (ない「-1」)
    /// </summary>
    public int MajorNumber { get; set; } = -1;
    /// <summary>
    /// マイナー番号 (ない「-1」)
    /// </summary>
    public int MinorNumber { get; set; } = -1;
    /// <summary>
    /// ビルド番号 (ない「-1」)
    /// </summary>
    public int BuildNumber { get; set; } = -1;
    /// <summary>
    /// プライベート番号 (ない「-1」)
    /// </summary>
    public int PrivateNumber { get; set; } = -1;

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

    /// <summary>
    /// 文字列からVersionNumberに変換
    /// </summary>
    /// <param name="versionString">バージョン文字列</param>
    /// <returns>バージョン番号</returns>
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
}
