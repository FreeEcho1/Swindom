namespace Swindom;

/// <summary>
/// ウィンドウで使用する言語情報
/// </summary>
[DataContract]
public class LanguagesWindow : IExtensibleDataObject
{
    /// <summary>
    /// OK
    /// </summary>
    [DataMember]
    public string Ok = "OK";
    /// <summary>
    /// はい
    /// </summary>
    [DataMember]
    public string Yes = "はい";
    /// <summary>
    /// いいえ
    /// </summary>
    [DataMember]
    public string No = "いいえ";
    /// <summary>
    /// キャンセル
    /// </summary>
    [DataMember]
    public string Cancel = "キャンセル";
    /// <summary>
    /// 全てのユーザーで設定情報を共有しますか？
    /// </summary>
    [DataMember]
    public string ShareSettings = "全てのユーザーで設定情報を共有しますか？";
    /// <summary>
    /// 開く
    /// </summary>
    [DataMember]
    public string Open = "開く";
    /// <summary>
    /// 「指定ウィンドウ」の一括処理
    /// </summary>
    [DataMember]
    public string BatchProcessingOfSpecifyWindow = "「指定ウィンドウ」の一括処理";
    /// <summary>
    /// ウィンドウが画面外の場合は画面内に移動
    /// </summary>
    [DataMember]
    public string WindowMoveOutsideToInside = "ウィンドウが画面外の場合は画面内に移動";
    /// <summary>
    /// ディスプレイ情報更新
    /// </summary>
    [DataMember]
    public string DisplayInformationUpdate = "ディスプレイ情報更新";
    /// <summary>
    /// 終了
    /// </summary>
    [DataMember]
    public string End = "終了";
    /// <summary>
    /// 更新確認に失敗しました。
    /// </summary>
    [DataMember]
    public string UpdateCheckFailed = "更新確認に失敗しました。";
    /// <summary>
    /// 最新バージョンです。
    /// </summary>
    [DataMember]
    public string LatestVersion = "最新バージョンです。";
    /// <summary>
    /// 最新バージョンがあります。
    /// </summary>
    [DataMember]
    public string ThereIsLatestVersion = "最新バージョンがあります。";
    /// <summary>
    /// イベント
    /// </summary>
    [DataMember]
    public string Event = "イベント";
    /// <summary>
    /// タイマー
    /// </summary>
    [DataMember]
    public string Timer = "タイマー";
    /// <summary>
    /// 指定ウィンドウ
    /// </summary>
    [DataMember]
    public string SpecifyWindow = "指定ウィンドウ";
    /// <summary>
    /// マグネット
    /// </summary>
    [DataMember]
    public string Magnet = "マグネット";
    /// <summary>
    /// プラグイン
    /// </summary>
    [DataMember]
    public string Plugin = "プラグイン";
    /// <summary>
    /// プラグイン設定
    /// </summary>
    [DataMember]
    public string PluginSettings = "プラグイン設定";
    /// <summary>
    /// 設定
    /// </summary>
    [DataMember]
    public string Setting = "設定";
    /// <summary>
    /// 情報
    /// </summary>
    [DataMember]
    public string Information = "情報";
    /// <summary>
    /// 追加
    /// </summary>
    [DataMember]
    public string Add = "追加";
    /// <summary>
    /// 修正
    /// </summary>
    [DataMember]
    public string Modify = "修正";
    /// <summary>
    /// 削除
    /// </summary>
    [DataMember]
    public string Delete = "削除";
    /// <summary>
    /// 選択
    /// </summary>
    [DataMember]
    public string Select = "選択";
    /// <summary>
    /// 上に移動
    /// </summary>
    [DataMember]
    public string MoveUp = "上に移動";
    /// <summary>
    /// 下に移動
    /// </summary>
    [DataMember]
    public string MoveDown = "下に移動";
    /// <summary>
    /// コピー
    /// </summary>
    [DataMember]
    public string Copy = "コピー";
    /// <summary>
    /// 処理状態
    /// </summary>
    [DataMember]
    public string ProcessingState = "処理状態";
    /// <summary>
    /// 削除しました。
    /// </summary>
    [DataMember]
    public string Deleted = "削除しました。";
    /// <summary>
    /// 削除しますか？
    /// </summary>
    [DataMember]
    public string AllowDelete = "削除しますか？";
    /// <summary>
    /// ソフトウェア名
    /// </summary>
    [DataMember]
    public string SoftwareName = "ソフトウェア名";
    /// <summary>
    /// バージョン
    /// </summary>
    [DataMember]
    public string Version = "バージョン";
    /// <summary>
    /// 更新確認
    /// </summary>
    [DataMember]
    public string UpdateCheck = "更新確認";
    /// <summary>
    /// 説明書
    /// </summary>
    [DataMember]
    public string Manual = "説明書";
    /// <summary>
    /// 更新履歴
    /// </summary>
    [DataMember]
    public string UpdateHistory = "更新履歴";
    /// <summary>
    /// ウェブサイト
    /// </summary>
    [DataMember]
    public string Website = "ウェブサイト";
    /// <summary>
    /// 報告、要望
    /// </summary>
    [DataMember]
    public string ReportsAndRequests = "報告、要望";
    /// <summary>
    /// ライブラリ
    /// </summary>
    [DataMember]
    public string Library = "ライブラリ";
    /// <summary>
    /// 全般
    /// </summary>
    [DataMember]
    public string General = "全般";
    /// <summary>
    /// ホットキー
    /// </summary>
    [DataMember]
    public string Hotkey = "ホットキー";
    /// <summary>
    /// 言語
    /// </summary>
    [DataMember]
    public string Language = "言語";
    /// <summary>
    /// 翻訳者：
    /// </summary>
    [DataMember]
    public string Translators = "翻訳者：";
    /// <summary>
    /// ダークモード
    /// </summary>
    [DataMember]
    public string DarkMode = "ダークモード";
    /// <summary>
    /// 実行時に自動で更新確認
    /// </summary>
    [DataMember]
    public string AutomaticUpdateCheck = "実行時に自動で更新確認";
    /// <summary>
    /// ベータバージョンを含めて更新確認
    /// </summary>
    [DataMember]
    public string CheckBetaVersion = "ベータバージョンを含めて更新確認";
    /// <summary>
    /// ウィンドウ処理を複数登録
    /// </summary>
    [DataMember]
    public string RegisterMultipleWindowActions = "ウィンドウ処理を複数登録";
    /// <summary>
    /// 座標
    /// </summary>
    [DataMember]
    public string Coordinate = "座標";
    /// <summary>
    /// 各ディスプレイの左上を「x=0, y=0」
    /// </summary>
    [DataMember]
    public string EachDisplay = "各ディスプレイの左上を「x=0, y=0」";
    /// <summary>
    /// プライマリディスプレイの左上を「x=0, y=0」
    /// </summary>
    [DataMember]
    public string PrimaryDisplay = "プライマリディスプレイの左上を「x=0, y=0」";
    /// <summary>
    /// 処理間隔 (ミリ秒)
    /// </summary>
    [DataMember]
    public string ProcessingInterval = "処理間隔 (ミリ秒)";
    /// <summary>
    /// Windows起動時に実行
    /// </summary>
    [DataMember]
    public string RunAtWindowsStartup = "Windows起動時に実行";
    /// <summary>
    /// Windows起動時に実行 (管理者権限)
    /// </summary>
    [DataMember]
    public string RunAtWindowsStartupAdministrator = "Windows起動時に実行 (管理者権限)";
    /// <summary>
    /// 通常実行ですか？ (管理者権限なし - 「はい」 / 管理者権限あり - 「いいえ」)
    /// </summary>
    [DataMember]
    public string NormalExecution = "通常実行ですか？ (管理者権限なし - 「はい」 / 管理者権限あり - 「いいえ」)";
    /// <summary>
    /// 次のウィンドウを処理する待ち時間 (ミリ秒)
    /// </summary>
    [DataMember]
    public string WaitTimeToProcessingNextWindow = "次のウィンドウを処理する待ち時間 (ミリ秒)";
    /// <summary>
    /// ウィンドウが全画面表示の場合は処理停止
    /// </summary>
    [DataMember]
    public string StopProcessingWhenWindowIsFullScreen = "ウィンドウが全画面表示の場合は処理停止";
    /// <summary>
    /// ウィンドウ判定で大文字と小文字を区別する
    /// </summary>
    [DataMember]
    public string CaseSensitiveWindowQueries = "ウィンドウ判定で大文字と小文字を区別する";
    /// <summary>
    /// 画面外に出る場合は位置やサイズを変更しない
    /// </summary>
    [DataMember]
    public string DoNotChangePositionSizeOutOfScreen = "画面外に出る場合は位置やサイズを変更しない";
    /// <summary>
    /// 追加/修正のウィンドウが表示されている場合は処理停止
    /// </summary>
    [DataMember]
    public string StopProcessingShowAddModify = "追加/修正のウィンドウが表示されている場合は処理停止";
    /// <summary>
    /// ホットキーは停止させない (全画面ウィンドウがある場合)
    /// </summary>
    [DataMember]
    public string HotkeysDoNotStop = "ホットキーは停止させない (全画面ウィンドウがある場合)";
    /// <summary>
    /// 貼り付ける位置をずらす
    /// </summary>
    [DataMember]
    public string MovePastePosition = "貼り付ける位置をずらす";
    /// <summary>
    /// 画面端に貼り付ける
    /// </summary>
    [DataMember]
    public string PasteToEdgeOfScreen = "画面端に貼り付ける";
    /// <summary>
    /// 別のウィンドウに貼り付ける
    /// </summary>
    [DataMember]
    public string PasteIntoAnotherWindow = "別のウィンドウに貼り付ける";
    /// <summary>
    /// Ctrlキーを押した状態で貼り付ける
    /// </summary>
    [DataMember]
    public string PasteWithCtrlKeyPressed = "Ctrlキーを押した状態で貼り付ける";
    /// <summary>
    /// 貼り付いた時の停止時間 (ミリ秒)
    /// </summary>
    [DataMember]
    public string StopTimeWhenPasting = "貼り付いた時の停止時間 (ミリ秒)";
    /// <summary>
    /// 貼り付ける判定距離
    /// </summary>
    [DataMember]
    public string DecisionDistanceToPaste = "貼り付ける判定距離";
    /// <summary>
    /// 指定方法は「Readme (説明書)」で確認してください。
    /// </summary>
    [DataMember]
    public string CheckReadme = "指定方法は「Readme (説明書)」で確認してください。";
    /// <summary>
    /// タイトル名
    /// </summary>
    [DataMember]
    public string TitleName = "タイトル名";
    /// <summary>
    /// クラス名
    /// </summary>
    [DataMember]
    public string ClassName = "クラス名";
    /// <summary>
    /// ファイル名
    /// </summary>
    [DataMember]
    public string FileName = "ファイル名";
    /// <summary>
    /// ディスプレイ
    /// </summary>
    [DataMember]
    public string Display = "ディスプレイ";
    /// <summary>
    /// ウィンドウの状態
    /// </summary>
    [DataMember]
    public string WindowState = "ウィンドウの状態";
    /// <summary>
    /// 処理の種類
    /// </summary>
    [DataMember]
    public string ProcessingType = "処理の種類";
    /// <summary>
    /// 位置とサイズを指定
    /// </summary>
    [DataMember]
    public string SpecifyPositionAndSize = "位置とサイズを指定";
    /// <summary>
    /// X座標を移動
    /// </summary>
    [DataMember]
    public string MoveXCoordinate = "X座標を移動";
    /// <summary>
    /// Y座標を移動
    /// </summary>
    [DataMember]
    public string MoveYCoordinate = "Y座標を移動";
    /// <summary>
    /// 幅を増減
    /// </summary>
    [DataMember]
    public string IncreaseDecreaseWidth = "幅を増減";
    /// <summary>
    /// 高さを増減
    /// </summary>
    [DataMember]
    public string IncreaseDecreaseHeight = "高さを増減";
    /// <summary>
    /// 幅と高さを増減
    /// </summary>
    [DataMember]
    public string IncreaseDecreaseWidthAndHeight = "幅と高さを増減";
    /// <summary>
    /// 「指定ウィンドウ」の処理開始/停止
    /// </summary>
    [DataMember]
    public string StartStopProcessingOfSpecifyWindow = "「指定ウィンドウ」の処理開始/停止";
    /// <summary>
    /// 「全てのウィンドウ」の処理開始/停止
    /// </summary>
    [DataMember]
    public string StartStopProcessingOfAllWindow = "「全てのウィンドウ」の処理開始/停止";
    /// <summary>
    /// 「マグネット」の処理開始/停止
    /// </summary>
    [DataMember]
    public string StartStopProcessingOfMagnet = "「マグネット」の処理開始/停止";
    /// <summary>
    /// このアプリケーションのウィンドウを表示
    /// </summary>
    [DataMember]
    public string ShowThisApplicationWindow = "このアプリケーションのウィンドウを表示";
    /// <summary>
    /// 常に最前面に表示/解除
    /// </summary>
    [DataMember]
    public string AlwaysShowOrCancelOnTop = "常に最前面に表示/解除";
    /// <summary>
    /// 透明度を指定/解除
    /// </summary>
    [DataMember]
    public string SpecifyCancelTransparency = "透明度を指定/解除";
    /// <summary>
    /// 左端
    /// </summary>
    [DataMember]
    public string LeftEdge = "左端";
    /// <summary>
    /// 中央
    /// </summary>
    [DataMember]
    public string Middle = "中央";
    /// <summary>
    /// 右端
    /// </summary>
    [DataMember]
    public string RightEdge = "右端";
    /// <summary>
    /// 上端
    /// </summary>
    [DataMember]
    public string TopEdge = "上端";
    /// <summary>
    /// 下端
    /// </summary>
    [DataMember]
    public string BottomEdge = "下端";
    /// <summary>
    /// X
    /// </summary>
    [DataMember]
    public string X = "X";
    /// <summary>
    /// Y
    /// </summary>
    [DataMember]
    public string Y = "Y";
    /// <summary>
    /// 幅
    /// </summary>
    [DataMember]
    public string Width = "幅";
    /// <summary>
    /// 高さ
    /// </summary>
    [DataMember]
    public string Height = "高さ";
    /// <summary>
    /// px
    /// </summary>
    [DataMember]
    public string Pixel = "px";
    /// <summary>
    /// %
    /// </summary>
    [DataMember]
    public string Percent = "%";
    /// <summary>
    /// 登録名
    /// </summary>
    [DataMember]
    public string RegisteredName = "登録名";
    /// <summary>
    /// ウィンドウ判定
    /// </summary>
    [DataMember]
    public string WindowDecide = "ウィンドウ判定";
    /// <summary>
    /// 処理設定
    /// </summary>
    [DataMember]
    public string ProcessingSettings = "処理設定";
    /// <summary>
    /// 押したまま選択するウィンドウにマウスカーソルを移動
    /// </summary>
    [DataMember]
    public string HoldDownMouseCursorMoveToSelectWindow = "押したまま選択するウィンドウにマウスカーソルを移動";
    /// <summary>
    /// ウィンドウ情報取得
    /// </summary>
    [DataMember]
    public string GetWindowInformation = "ウィンドウ情報取得";
    /// <summary>
    /// 取得する情報
    /// </summary>
    [DataMember]
    public string InformationToBeObtained = "取得する情報";
    /// <summary>
    /// ファイル選択
    /// </summary>
    [DataMember]
    public string FileSelection = "ファイル選択";
    /// <summary>
    /// 完全一致
    /// </summary>
    [DataMember]
    public string ExactMatch = "完全一致";
    /// <summary>
    /// 部分一致
    /// </summary>
    [DataMember]
    public string PartialMatch = "部分一致";
    /// <summary>
    /// 前方一致
    /// </summary>
    [DataMember]
    public string ForwardMatch = "前方一致";
    /// <summary>
    /// 後方一致
    /// </summary>
    [DataMember]
    public string BackwardMatch = "後方一致";
    /// <summary>
    /// パスを含む
    /// </summary>
    [DataMember]
    public string IncludePath = "パスを含む";
    /// <summary>
    /// パスを含まない
    /// </summary>
    [DataMember]
    public string NotIncludePath = "パスを含まない";
    /// <summary>
    /// 指定しない
    /// </summary>
    [DataMember]
    public string DoNotSpecify = "指定しない";
    /// <summary>
    /// タイトル名の処理条件
    /// </summary>
    [DataMember]
    public string TitleNameRequirements = "タイトル名の処理条件";
    /// <summary>
    /// タイトル名がないウィンドウ
    /// </summary>
    [DataMember]
    public string WindowWithoutTitleName = "タイトル名がないウィンドウ";
    /// <summary>
    /// タイトル名があるウィンドウ
    /// </summary>
    [DataMember]
    public string WindowWithTitleName = "タイトル名があるウィンドウ";
    /// <summary>
    /// 処理名
    /// </summary>
    [DataMember]
    public string ProcessingName = "処理名";
    /// <summary>
    /// 基準にするディスプレイ
    /// </summary>
    [DataMember]
    public string DisplayToUseAsStandard = "基準にするディスプレイ";
    /// <summary>
    /// 現在のディスプレイ
    /// </summary>
    [DataMember]
    public string CurrentDisplay = "現在のディスプレイ";
    /// <summary>
    /// 指定したディスプレイ
    /// </summary>
    [DataMember]
    public string SpecifiedDisplay = "指定したディスプレイ";
    /// <summary>
    /// 指定したディスプレイ限定
    /// </summary>
    [DataMember]
    public string LimitedToSpecifiedDisplay = "指定したディスプレイ限定";
    /// <summary>
    /// 一度だけ処理
    /// </summary>
    [DataMember]
    public string ProcessOnlyOnce = "一度だけ処理";
    /// <summary>
    /// ウィンドウが開かれた時
    /// </summary>
    [DataMember]
    public string OnceWindowOpen = "ウィンドウが開かれた時";
    /// <summary>
    /// このソフトウェアが実行されている間
    /// </summary>
    [DataMember]
    public string OnceWhileItIsRunning = "このソフトウェアが実行されている間";
    /// <summary>
    /// フォアグラウンドになった
    /// </summary>
    [DataMember]
    public string Foregrounded = "フォアグラウンドになった";
    /// <summary>
    /// 移動及びサイズの変更が終了した
    /// </summary>
    [DataMember]
    public string MoveSizeChangeEnd = "移動及びサイズの変更が終了した";
    /// <summary>
    /// 最小化が開始された
    /// </summary>
    [DataMember]
    public string MinimizeStart = "最小化が開始された";
    /// <summary>
    /// 最小化が終了した
    /// </summary>
    [DataMember]
    public string MinimizeEnd = "最小化が終了した";
    /// <summary>
    /// 表示された
    /// </summary>
    [DataMember]
    public string Show = "表示された";
    /// <summary>
    /// タイトル名が変更された
    /// </summary>
    [DataMember]
    public string TitleNameChanged = "タイトル名が変更された";
    /// <summary>
    /// 最前面
    /// </summary>
    [DataMember]
    public string Forefront = "最前面";
    /// <summary>
    /// 変更しない
    /// </summary>
    [DataMember]
    public string DoNotChange = "変更しない";
    /// <summary>
    /// 常に最前面
    /// </summary>
    [DataMember]
    public string AlwaysForefront = "常に最前面";
    /// <summary>
    /// 常に最前面解除
    /// </summary>
    [DataMember]
    public string AlwaysCancelForefront = "常に最前面解除";
    /// <summary>
    /// 透明度指定
    /// </summary>
    [DataMember]
    public string SpecifyTransparency = "透明度指定";
    /// <summary>
    /// ウィンドウを閉じる
    /// </summary>
    [DataMember]
    public string CloseWindow = "ウィンドウを閉じる";
    /// <summary>
    /// タイマー処理
    /// </summary>
    [DataMember]
    public string TimerProcessing = "タイマー処理";
    /// <summary>
    /// 最初に処理しない回数 (実行直後の処理を遅らせる)
    /// </summary>
    [DataMember]
    public string NumberOfTimesNotToProcessingFirst = "最初に処理しない回数 (実行直後の処理を遅らせる)";
    /// <summary>
    /// 通常のウィンドウ
    /// </summary>
    [DataMember]
    public string NormalWindow = "通常のウィンドウ";
    /// <summary>
    /// 最大化
    /// </summary>
    [DataMember]
    public string Maximize = "最大化";
    /// <summary>
    /// 最小化
    /// </summary>
    [DataMember]
    public string Minimize = "最小化";
    /// <summary>
    /// 「通常のウィンドウ」の時のみ処理する
    /// </summary>
    [DataMember]
    public string ProcessOnlyWhenNormalWindow = "「通常のウィンドウ」の時のみ処理する";
    /// <summary>
    /// 座標指定
    /// </summary>
    [DataMember]
    public string CoordinateSpecification = "座標指定";
    /// <summary>
    /// 幅指定
    /// </summary>
    [DataMember]
    public string WidthSpecification = "幅指定";
    /// <summary>
    /// 高さ指定
    /// </summary>
    [DataMember]
    public string HeightSpecification = "高さ指定";
    /// <summary>
    /// 処理しない条件
    /// </summary>
    [DataMember]
    public string ConditionsThatDoNotProcess = "処理しない条件";
    /// <summary>
    /// クライアントエリアを対象とする
    /// </summary>
    [DataMember]
    public string ClientArea = "クライアントエリアを対象とする";
    /// <summary>
    /// タイトル名に含まれる文字列
    /// </summary>
    [DataMember]
    public string TitleNameExclusionString = "タイトル名に含まれる文字列";
    /// <summary>
    /// サイズ
    /// </summary>
    [DataMember]
    public string Size = "サイズ";
    /// <summary>
    /// 指定したバージョン以外 (マイナーまでなど可能)
    /// </summary>
    [DataMember]
    public string OtherThanSpecifiedVersion = "指定したバージョン以外 (マイナーまでなど可能)";
    /// <summary>
    /// 知らせる
    /// </summary>
    [DataMember]
    public string Announce = "知らせる";
    /// <summary>
    /// 5秒後にウィンドウ情報が取得されるので、5秒以内に情報を取得するウィンドウをアクティブにしてください。
    /// </summary>
    [DataMember]
    public string RetrievedAfterFiveSeconds = "5秒後にウィンドウ情報が取得されるので、5秒以内に情報を取得するウィンドウをアクティブにしてください。";
    /// <summary>
    /// 選択しました。
    /// </summary>
    [DataMember]
    public string SelectionComplete = "選択しました。";
    /// <summary>
    /// 重複した項目があります。
    /// </summary>
    [DataMember]
    public string ThereAreDuplicateItems = "重複した項目があります。";
    /// <summary>
    /// 重複した登録名があります。
    /// </summary>
    [DataMember]
    public string ThereIsADuplicateRegistrationName = "重複した登録名があります。";
    /// <summary>
    /// 重複した処理名があります。
    /// </summary>
    [DataMember]
    public string ThereIsADuplicateProcessingName = "重複した処理名があります。";
    /// <summary>
    /// 追加しました。
    /// </summary>
    [DataMember]
    public string Added = "追加しました。";
    /// <summary>
    /// 修正しました。
    /// </summary>
    [DataMember]
    public string Modified = "修正しました。";
    /// <summary>
    /// 処理するウィンドウの範囲
    /// </summary>
    [DataMember]
    public string ProcessingWindowRange = "処理するウィンドウの範囲";
    /// <summary>
    /// アクティブウィンドウのみ
    /// </summary>
    [DataMember]
    public string ActiveWindowOnly = "アクティブウィンドウのみ";
    /// <summary>
    /// 全てのウィンドウ
    /// </summary>
    [DataMember]
    public string AllWindow = "全てのウィンドウ";
    /// <summary>
    /// 「指定ウィンドウ」のアクティブウィンドウのみ処理
    /// </summary>
    [DataMember]
    public string OnlyActiveWindowSpecifyWindow = "「指定ウィンドウ」のアクティブウィンドウのみ処理";
    /// <summary>
    /// 移動量
    /// </summary>
    [DataMember]
    public string AmountOfMovement = "移動量";
    /// <summary>
    /// サイズ変更量
    /// </summary>
    [DataMember]
    public string SizeChangeAmount = "サイズ変更量";
    /// <summary>
    /// 透明度
    /// </summary>
    [DataMember]
    public string Transparency = "透明度";
    /// <summary>
    /// 数値計算
    /// </summary>
    [DataMember]
    public string NumericCalculation = "数値計算";
    /// <summary>
    /// 比率
    /// </summary>
    [DataMember]
    public string Ratio = "比率";
    /// <summary>
    /// 「指定ウィンドウ」の説明
    /// </summary>
    [DataMember]
    public string ExplanationOfSpecifyWindow = "ウィンドウを指定して自動で処理させることができます。自動処理させずにホットキーだけで処理したい場合は、チェックを外して無効状態にしてください。";
    /// <summary>
    /// 「全てのウィンドウ」の説明
    /// </summary>
    [DataMember]
    public string ExplanationOfAllWindow = "指定したウィンドウを除く全てのウィンドウを処理させることができます。「指定ウィンドウ」で指定しているウィンドウも除外されます。";
    /// <summary>
    /// 「ホットキー」の説明
    /// </summary>
    [DataMember]
    public string HotkeyExplanation = "ホットキーでアクティブウィンドウ(入力を受け付けている状態)を処理させることができます。「選択」ボタンではホットキーを使用しなくても、処理させたいウィンドウを選択して処理させることができます。";
    /// <summary>
    /// 「マグネット」の説明
    /// </summary>
    [DataMember]
    public string MagnetExplanation = "マウスでウィンドウを移動させている場合に、画面端や別のウィンドウに貼り付けます。";
    /// <summary>
    /// テスト中の機能なので、動作が不安定などの可能性があります。
    /// </summary>
    [DataMember]
    public string PluginTestMessage = "テスト中の機能なので、動作が不安定などの可能性があります。";
    /// <summary>
    /// プラグインの説明
    /// </summary>
    [DataMember]
    public string PluginExplanation = "プラグインは機能を追加することができます。詳しい説明は「Readme (説明書)」で確認してください。";
    /// <summary>
    /// 設定している値
    /// </summary>
    [DataMember]
    public string SettingsValue = "設定している値";
    /// <summary>
    /// ウィンドウの情報
    /// </summary>
    [DataMember]
    public string WindowInformation = "ウィンドウの情報";
    /// <summary>
    /// 全てのウィンドウを指定した位置に移動
    /// </summary>
    [DataMember]
    public string MoveAllWindowToSpecifiedPosition = "全てのウィンドウを指定した位置に移動";
    /// <summary>
    /// 移動しないウィンドウ
    /// </summary>
    [DataMember]
    public string CancelMoveWindow = "移動しないウィンドウ";
    /// <summary>
    /// 位置
    /// </summary>
    [DataMember]
    public string Position = "位置";
    /// <summary>
    /// 再起動
    /// </summary>
    [DataMember]
    public string Restart = "再起動";
    /// <summary>
    /// Swindomを再起動する必要があります。
    /// </summary>
    [DataMember]
    public string NeedRestart = "Swindomを再起動する必要があります。";
    /// <summary>
    /// ウィンドウ判定の指定方法
    /// </summary>
    [DataMember]
    public string WindowDesignateMethod = "ウィンドウ判定の指定方法";
    /// <summary>
    /// ウィンドウ判定の指定方法の説明
    /// </summary>
    [DataMember]
    public string WindowDesignateMethodExplain = "ウィンドウの判定は「タイトル名」「クラス名」「ファイル名」を指定できます。\r\n何を指定して、何を指定してはいけないのか、それぞれのウィンドウで違います。\r\n全てを指定するとウィンドウが処理されない場合があります。\r\nウィンドウが表示される度に「クラス名」が変更されるソフトウェアの場合は、\r\n「クラス名」は指定しないでください。\r\nUWPアプリは「クラス名」と「ファイル名」は一部を除いてどのアプリも同じなので、\r\n「タイトル名」がない場合は正しく判別できず、他のアプリのウィンドウも処理されてしまいます。\r\n「タイトル名」「クラス名」「ファイル名」の全てを指定すると良いでしょう。\r\nクラス名は「ApplicationFrameWindow」、ファイル名は「C:\\WINDOWS\\system32\\ApplicationFrameHost.exe」、\r\nです (Windowsのアップデートで変更される可能性があります)。";

    [OnDeserializing]
    public void DefaultDeserializing(
        StreamingContext context
        )
    {
        Ok = "Ok";
        Yes = "はい";
        No = "いいえ";
        Cancel = "キャンセル";
        ShareSettings = "全てのユーザーで設定情報を共有しますか？";
        Open = "開く";
        BatchProcessingOfSpecifyWindow = "「指定ウィンドウ」の一括処理";
        WindowMoveOutsideToInside = "ウィンドウが画面外の場合は画面内に移動";
        DisplayInformationUpdate = "ディスプレイ情報更新";
        End = "終了";
        UpdateCheckFailed = "更新確認に失敗しました。";
        LatestVersion = "最新バージョンです。";
        ThereIsLatestVersion = "最新バージョンがあります。";
        Event = "イベント";
        Timer = "タイマー";
        SpecifyWindow = "指定ウィンドウ";
        Magnet = "マグネット";
        Plugin = "プラグイン";
        PluginSettings = "プラグイン設定";
        Setting = "設定";
        Information = "情報";
        Add = "追加";
        Modify = "修正";
        Delete = "削除";
        Select = "選択";
        MoveUp = "上に移動";
        MoveDown = "下に移動";
        Copy = "コピー";
        ProcessingState = "処理状態";
        Deleted = "削除しました。";
        AllowDelete = "削除しますか？";
        SoftwareName = "ソフトウェア名";
        Version = "バージョン";
        UpdateCheck = "更新確認";
        Manual = "説明書";
        UpdateHistory = "更新履歴";
        Website = "ウェブサイト";
        ReportsAndRequests = "報告、要望";
        Library = "ライブラリ";
        General = "全般";
        Hotkey = "ホットキー";
        Language = "言語";
        Translators = "翻訳者：";
        DarkMode = "ダークモード";
        AutomaticUpdateCheck = "実行時に自動で更新確認";
        CheckBetaVersion = "ベータバージョンを含めて更新確認";
        RegisterMultipleWindowActions = "ウィンドウ処理を複数登録";
        Coordinate = "座標";
        EachDisplay = "各ディスプレイの左上を「x=0, y=0」";
        PrimaryDisplay = "プライマリディスプレイの左上を「x=0, y=0」";
        ProcessingInterval = "処理間隔 (ミリ秒)";
        RunAtWindowsStartup = "Windows起動時に実行";
        RunAtWindowsStartupAdministrator = "Windows起動時に実行 (管理者権限)";
        NormalExecution = "通常実行ですか？ (管理者権限なし - 「はい」 / 管理者権限あり - 「いいえ」)";
        WaitTimeToProcessingNextWindow = "次のウィンドウを処理する待ち時間 (ミリ秒)";
        StopProcessingWhenWindowIsFullScreen = "全画面ウィンドウがある場合は処理停止";
        CaseSensitiveWindowQueries = "ウィンドウ判定で大文字と小文字を区別する";
        DoNotChangePositionSizeOutOfScreen = "画面外に出る場合は位置やサイズを変更しない";
        StopProcessingShowAddModify = "追加/修正のウィンドウが表示されている場合は処理停止";
        HotkeysDoNotStop = "ホットキーは停止させない (全画面ウィンドウがある場合)";
        MovePastePosition = "貼り付ける位置をずらす";
        PasteToEdgeOfScreen = "画面端に貼り付ける";
        PasteIntoAnotherWindow = "別のウィンドウに貼り付ける";
        PasteWithCtrlKeyPressed = "Ctrlキーを押した状態で貼り付ける";
        StopTimeWhenPasting = "貼り付いた時の停止時間 (ミリ秒)";
        DecisionDistanceToPaste = "貼り付ける判定距離";
        CheckReadme = "指定方法は「Readme (説明書)」で確認してください。";
        TitleName = "タイトル名";
        ClassName = "クラス名";
        FileName = "ファイル名";
        Display = "ディスプレイ";
        WindowState = "ウィンドウの状態";
        ProcessingType = "処理の種類";
        SpecifyPositionAndSize = "位置とサイズを指定";
        MoveXCoordinate = "X座標を移動";
        MoveYCoordinate = "Y座標を移動";
        IncreaseDecreaseWidth = "幅を増減";
        IncreaseDecreaseHeight = "高さを増減";
        IncreaseDecreaseWidthAndHeight = "幅と高さを増減";
        StartStopProcessingOfSpecifyWindow = "「指定ウィンドウ」の処理開始/停止";
        StartStopProcessingOfAllWindow = "「全てのウィンドウ」の処理開始/停止";
        StartStopProcessingOfMagnet = "「マグネット」の処理開始/停止";
        ShowThisApplicationWindow = "このアプリケーションのウィンドウを表示";
        AlwaysShowOrCancelOnTop = "常に最前面に表示/解除";
        SpecifyCancelTransparency = "透明度を指定/解除";
        LeftEdge = "左端";
        Middle = "中央";
        RightEdge = "右端";
        TopEdge = "上端";
        BottomEdge = "下端";
        X = "X";
        Y = "Y";
        Width = "幅";
        Height = "高さ";
        Pixel = "px";
        Percent = "%";
        RegisteredName = "登録名";
        WindowDecide = "ウィンドウ判定";
        ProcessingSettings = "処理設定";
        HoldDownMouseCursorMoveToSelectWindow = "押したまま選択するウィンドウにマウスカーソルを移動";
        GetWindowInformation = "ウィンドウ情報取得";
        InformationToBeObtained = "取得するウィンドウ情報";
        FileSelection = "ファイル選択";
        ExactMatch = "完全一致";
        PartialMatch = "部分一致";
        ForwardMatch = "前方一致";
        BackwardMatch = "後方一致";
        IncludePath = "パスを含む";
        NotIncludePath = "パスを含まない";
        DoNotSpecify = "指定しない";
        TitleNameRequirements = "タイトルの処理条件";
        WindowWithoutTitleName = "タイトルがないウィンドウは処理しない";
        WindowWithTitleName = "タイトルがあるウィンドウは処理しない";
        ProcessingName = "処理名";
        DisplayToUseAsStandard = "基準にするディスプレイ";
        CurrentDisplay = "ウィンドウがあるディスプレイ";
        SpecifiedDisplay = "指定したディスプレイ";
        LimitedToSpecifiedDisplay = "指定したディスプレイにある場合のみ";
        ProcessOnlyOnce = "1度だけ処理";
        OnceWindowOpen = "ウィンドウが開かれた時";
        OnceWhileItIsRunning = "このソフトウェアが実行されている間";
        Foregrounded = "フォアグラウンドになった";
        MoveSizeChangeEnd = "移動及びサイズの変更が終了した";
        MinimizeStart = "最小化が開始された";
        MinimizeEnd = "最小化が終了した";
        Show = "表示された";
        TitleNameChanged = "タイトル名が変更された";
        Forefront = "最前面";
        DoNotChange = "変更しない";
        AlwaysForefront = "常に最前面";
        AlwaysCancelForefront = "常に最前面解除";
        SpecifyTransparency = "透明度指定";
        CloseWindow = "ウィンドウを閉じる";
        TimerProcessing = "タイマー処理";
        NumberOfTimesNotToProcessingFirst = "最初に処理しない回数 (実行直後の処理を遅らせる)";
        NormalWindow = "通常のウィンドウ";
        Maximize = "最大化";
        Minimize = "最小化";
        ProcessOnlyWhenNormalWindow = "「通常のウィンドウ」の時だけ処理する";
        CoordinateSpecification = "座標指定";
        WidthSpecification = "幅指定";
        HeightSpecification = "高さ指定";
        ConditionsThatDoNotProcess = "処理しない条件";
        ClientArea = "クライアントエリアを対象とする";
        TitleNameExclusionString = "タイトル名の除外文字列";
        Size = "サイズ";
        OtherThanSpecifiedVersion = "指定したバージョン以外 (マイナーまでなど可能)";
        Announce = "知らせる";
        RetrievedAfterFiveSeconds = "5秒後にウィンドウ情報が取得されるので、5秒以内に情報を取得するウィンドウをアクティブにしてください。";
        SelectionComplete = "選択しました。";
        ThereAreDuplicateItems = "重複した項目があります。";
        ThereIsADuplicateRegistrationName = "重複した登録名があります。";
        ThereIsADuplicateProcessingName = "重複した処理名があります。";
        Added = "追加しました。";
        Modified = "修正しました。";
        ProcessingWindowRange = "処理するウィンドウの範囲";
        ActiveWindowOnly = "アクティブウィンドウのみ";
        AllWindow = "全てのウィンドウ";
        OnlyActiveWindowSpecifyWindow = "「指定ウィンドウ」のアクティブウィンドウのみ処理";
        AmountOfMovement = "移動量";
        SizeChangeAmount = "サイズ変更量";
        Transparency = "透明度";
        NumericCalculation = "数値計算";
        Ratio = "比率";
        ExplanationOfSpecifyWindow = "ウィンドウを指定して自動で処理させることができます。自動処理させずにホットキーだけで処理したい場合は、チェックを外して無効状態にしてください。";
        HotkeyExplanation = "ホットキーでアクティブウィンドウ(入力を受け付けている状態)を処理させることができます。「選択」ボタンではホットキーを使用しなくても、処理させたいウィンドウを選択して処理させることができます。";
        MagnetExplanation = "マウスでウィンドウを移動させている場合に、画面端や別のウィンドウに貼り付けます。";
        PluginTestMessage = "テスト中の機能なので、動作が不安定などの可能性があります。";
        PluginExplanation = "プラグインの機能です。";
        SettingsValue = "設定している値";
        WindowInformation = "ウィンドウの情報";
        MoveAllWindowToSpecifiedPosition = "全てのウィンドウを指定した位置に移動";
        CancelMoveWindow = "移動しないウィンドウ";
        Position = "位置";
        Restart = "再起動";
        NeedRestart = "Swindomを再起動する必要があります。";
        WindowDesignateMethod = "ウィンドウ判定の指定方法";
        WindowDesignateMethodExplain = "ウィンドウの判定は「タイトル名」「クラス名」「ファイル名」を指定できます。\r\n何を指定して、何を指定してはいけないのか、それぞれのウィンドウで違います。\r\n全てを指定するとウィンドウが処理されない場合があります。\r\nウィンドウが表示される度に「クラス名」が変更されるソフトウェアの場合は、\r\n「クラス名」は指定しないでください。\r\nUWPアプリは「クラス名」と「ファイル名」は一部を除いてどのアプリも同じなので、\r\n「タイトル名」がない場合は正しく判別できず、他のアプリのウィンドウも処理されてしまいます。\r\n「タイトル名」「クラス名」「ファイル名」の全てを指定すると良いでしょう。\r\nクラス名は「ApplicationFrameWindow」、ファイル名は「C:\\WINDOWS\\system32\\ApplicationFrameHost.exe」、\r\nです (Windowsのアップデートで変更される可能性があります)。";
    }

    public ExtensionDataObject? ExtensionData { get; set; }
}
