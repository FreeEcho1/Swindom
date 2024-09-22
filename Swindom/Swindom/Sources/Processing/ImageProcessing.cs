using System.Windows.Media;

namespace Swindom;

/// <summary>
/// 画像処理
/// </summary>
public class ImageProcessing
{
    /// <summary>
    /// 「Addition」の画像
    /// </summary>
    /// <returns>ImageSource</returns>
    public static ImageSource GetImageAddition() => new BitmapImage(new(ApplicationData.Settings.DarkMode ? "/Resources/AdditionWhite.png" : "/Resources/AdditionDark.png", UriKind.Relative));

    /// <summary>
    /// 「Modify」の画像
    /// </summary>
    /// <returns>ImageSource</returns>
    public static ImageSource GetImageModify() => new BitmapImage(new(ApplicationData.Settings.DarkMode ? "/Resources/ModifyWhite.png" : "/Resources/ModifyDark.png", UriKind.Relative));

    /// <summary>
    /// 「Delete」の画像
    /// </summary>
    /// <returns>ImageSource</returns>
    public static ImageSource GetImageDelete() => new BitmapImage(new(ApplicationData.Settings.DarkMode ? "/Resources/DeleteWhite.png" : "/Resources/DeleteDark.png", UriKind.Relative));

    /// <summary>
    /// 「Copy」の画像
    /// </summary>
    /// <returns>ImageSource</returns>
    public static ImageSource GetImageCopy() => new BitmapImage(new(ApplicationData.Settings.DarkMode ? "/Resources/CopyWhite.png" : "/Resources/CopyDark.png", UriKind.Relative));

    /// <summary>
    /// 「CloseSettings」の画像
    /// </summary>
    /// <returns>ImageSource</returns>
    public static ImageSource GetImageCloseSettings() => new BitmapImage(new(ApplicationData.Settings.DarkMode ? "/Resources/CloseSettingsWhite.png" : "/Resources/CloseSettingsDark.png", UriKind.Relative));

    /// <summary>
    /// 「Settings」の画像
    /// </summary>
    /// <returns>ImageSource</returns>
    public static ImageSource GetImageSettings() => new BitmapImage(new(ApplicationData.Settings.DarkMode ? "/Resources/SettingsWhite.png" : "/Resources/SettingsDark.png", UriKind.Relative));

    /// <summary>
    /// 「Target」の画像
    /// </summary>
    /// <returns>ImageSource</returns>
    public static ImageSource GetImageTarget() => new BitmapImage(new(ApplicationData.Settings.DarkMode ? "/Resources/TargetWhite.png" : "/Resources/TargetDark.png", UriKind.Relative));

    /// <summary>
    /// 「Up」の画像
    /// </summary>
    /// <returns>ImageSource</returns>
    public static ImageSource GetImageUp() => new BitmapImage(new(ApplicationData.Settings.DarkMode ? "/Resources/UpWhite.png" : "/Resources/UpDark.png", UriKind.Relative));

    /// <summary>
    /// 「Down」の画像
    /// </summary>
    /// <returns>ImageSource</returns>
    public static ImageSource GetImageDown() => new BitmapImage(new(ApplicationData.Settings.DarkMode ? "/Resources/DownWhite.png" : "/Resources/DownDark.png", UriKind.Relative));

    /// <summary>
    /// 「SpecifyWindowProcessing」の画像
    /// </summary>
    /// <returns>ImageSource</returns>
    public static ImageSource GetImageSpecifyWindowProcessing() => new BitmapImage(new(ApplicationData.Settings.DarkMode ? "/Resources/SpecifyWindowProcessingWhite.png" : "/Resources/SpecifyWindowProcessingDark.png", UriKind.Relative));

    /// <summary>
    /// 「AllWindowProcessing」の画像
    /// </summary>
    /// <returns>ImageSource</returns>
    public static ImageSource GetImageAllWindowProcessing() => new BitmapImage(new(ApplicationData.Settings.DarkMode ? "/Resources/AllWindowProcessingWhite.png" : "/Resources/AllWindowProcessingDark.png", UriKind.Relative));

    /// <summary>
    /// 「MagnetProcessing」の画像
    /// </summary>
    /// <returns>ImageSource</returns>
    public static ImageSource GetImageMagnetProcessing() => new BitmapImage(new(ApplicationData.Settings.DarkMode ? "/Resources/MagnetProcessingWhite.png" : "/Resources/MagnetProcessingDark.png", UriKind.Relative));

    /// <summary>
    /// 「HotkeyProcessing」の画像
    /// </summary>
    /// <returns>ImageSource</returns>
    public static ImageSource GetImageHotkeyProcessing() => new BitmapImage(new(ApplicationData.Settings.DarkMode ? "/Resources/HotkeyProcessingWhite.png" : "/Resources/HotkeyProcessingDark.png", UriKind.Relative));

    /// <summary>
    /// 「PluginProcessing」の画像
    /// </summary>
    /// <returns>ImageSource</returns>
    public static ImageSource GetImagePluginProcessing() => new BitmapImage(new(ApplicationData.Settings.DarkMode ? "/Resources/PluginProcessingWhite.png" : "/Resources/PluginProcessingDark.png", UriKind.Relative));

    /// <summary>
    /// 「Information」の画像
    /// </summary>
    /// <returns>ImageSource</returns>
    public static ImageSource GetImageInformation() => new BitmapImage(new(ApplicationData.Settings.DarkMode ? "/Resources/InformationWhite.png" : "/Resources/InformationDark.png", UriKind.Relative));

    /// <summary>
    /// 「UnattachedChain」の画像
    /// </summary>
    /// <returns>ImageSource</returns>
    public static ImageSource GetImageUnattachedChain() => new BitmapImage(new(ApplicationData.Settings.DarkMode ? "/Resources/UnattachedChainWhite.png" : "/Resources/UnattachedChainDark.png", UriKind.Relative));

    /// <summary>
    /// 「LinkedChain」の画像
    /// </summary>
    /// <returns>ImageSource</returns>
    public static ImageSource GetImageLinkedChain() => new BitmapImage(new(ApplicationData.Settings.DarkMode ? "/Resources/LinkedChainWhite.png" : "/Resources/LinkedChainDark.png", UriKind.Relative));
}
