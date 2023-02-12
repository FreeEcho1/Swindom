namespace Swindom.Sources.LanguagesData;

/// <summary>
/// 言語
/// </summary>
[DataContract]
public class Languages : IExtensibleDataObject
{
    /// <summary>
    /// 言語名
    /// </summary>
    [DataMember]
    public string LanguageName = "";

    /// <summary>
    /// エラーが発生しました。
    /// </summary>
    [DataMember]
    public string ErrorOccurred = "エラーが発生しました。";
    /// <summary>
    /// 確認
    /// </summary>
    [DataMember]
    public string Check = "確認";
    /// <summary>
    /// バージョンが変更されました
    /// </summary>
    [DataMember]
    public string ChangeVersion = "バージョンが変更されました";
    /// <summary>
    /// 設定を新しいバージョンに変更「はい」 / 手動で変更「いいえ」/処理を無効にする「キャンセル」
    /// </summary>
    [DataMember]
    public string ChangeSettingToNewVersion = "設定を新しいバージョンに変更「はい」 / 手動で変更「いいえ」/処理を無効にする「キャンセル」";
    /// <summary>
    /// ウィンドウで使用する言語
    /// </summary>
    [DataMember]
    public LanguagesWindow? LanguagesWindow = new();

    [OnDeserializing]
    public void DefaultDeserializing(
        StreamingContext context
        )
    {
        LanguageName = "日本語";
        ErrorOccurred = "エラーが発生しました。";
        Check = "確認";
        ChangeVersion = "バージョンが変更されました";
        ChangeSettingToNewVersion = "設定を新しいバージョンに変更「はい」 / 手動で変更「いいえ」/処理を無効にする「キャンセル」";
        LanguagesWindow = new();
    }

    public ExtensionDataObject? ExtensionData { get; set; }
}
