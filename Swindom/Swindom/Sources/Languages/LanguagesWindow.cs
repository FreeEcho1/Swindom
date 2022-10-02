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
    public string Ok;
    /// <summary>
    /// はい
    /// </summary>
    [DataMember]
    public string Yes;
    /// <summary>
    /// いいえ
    /// </summary>
    [DataMember]
    public string No;
    /// <summary>
    /// キャンセル
    /// </summary>
    [DataMember]
    public string Cancel;
    /// <summary>
    /// 全てのユーザーで設定情報を共有しますか？
    /// </summary>
    [DataMember]
    public string ShareSettings;
    /// <summary>
    /// 開く
    /// </summary>
    [DataMember]
    public string Open;
    /// <summary>
    /// 「イベント」の一括処理
    /// </summary>
    [DataMember]
    public string BatchProcessingOfEvent;
    /// <summary>
    /// 「タイマー」の一括処理
    /// </summary>
    [DataMember]
    public string BatchProcessingOfTimer;
    /// <summary>
    /// ディスプレイ情報更新
    /// </summary>
    [DataMember]
    public string DisplayInformationUpdate;
    /// <summary>
    /// 終了
    /// </summary>
    [DataMember]
    public string End;
    /// <summary>
    /// 更新確認に失敗しました。
    /// </summary>
    [DataMember]
    public string UpdateCheckFailed;
    /// <summary>
    /// 最新バージョンです。
    /// </summary>
    [DataMember]
    public string LatestVersion;
    /// <summary>
    /// 最新バージョンがあります。
    /// </summary>
    [DataMember]
    public string ThereIsTheLatestVersion;
    /// <summary>
    /// イベント
    /// </summary>
    [DataMember]
    public string Event;
    /// <summary>
    /// タイマー
    /// </summary>
    [DataMember]
    public string Timer;
    /// <summary>
    /// ウィンドウ処理
    /// </summary>
    [DataMember]
    public string WindowProcessing;
    /// <summary>
    /// マグネット
    /// </summary>
    [DataMember]
    public string Magnet;
    /// <summary>
    /// 設定
    /// </summary>
    [DataMember]
    public string Setting;
    /// <summary>
    /// 情報
    /// </summary>
    [DataMember]
    public string Information;
    /// <summary>
    /// 追加
    /// </summary>
    [DataMember]
    public string Add;
    /// <summary>
    /// 修正
    /// </summary>
    [DataMember]
    public string Modify;
    /// <summary>
    /// 削除
    /// </summary>
    [DataMember]
    public string Delete;
    /// <summary>
    /// 選択
    /// </summary>
    [DataMember]
    public string Select;
    /// <summary>
    /// 有効化
    /// </summary>
    [DataMember]
    public string Enabling;
    /// <summary>
    /// 無効化
    /// </summary>
    [DataMember]
    public string Disabling;
    /// <summary>
    /// 上に移動
    /// </summary>
    [DataMember]
    public string MoveUp;
    /// <summary>
    /// 下に移動
    /// </summary>
    [DataMember]
    public string MoveDown;
    /// <summary>
    /// コピー
    /// </summary>
    [DataMember]
    public string Copy;
    /// <summary>
    /// 処理状態
    /// </summary>
    [DataMember]
    public string ProcessingState;
    /// <summary>
    /// 削除しました。
    /// </summary>
    [DataMember]
    public string Deleted;
    /// <summary>
    /// 削除していいですか？
    /// </summary>
    [DataMember]
    public string AllowDelete;
    /// <summary>
    /// ソフトウェア名
    /// </summary>
    [DataMember]
    public string SoftwareName;
    /// <summary>
    /// バージョン
    /// </summary>
    [DataMember]
    public string Version;
    /// <summary>
    /// 更新確認
    /// </summary>
    [DataMember]
    public string UpdateCheck;
    /// <summary>
    /// 説明書
    /// </summary>
    [DataMember]
    public string Manual;
    /// <summary>
    /// 更新履歴
    /// </summary>
    [DataMember]
    public string UpdateHistory;
    /// <summary>
    /// ウェブサイト
    /// </summary>
    [DataMember]
    public string Website;
    /// <summary>
    /// 報告、要望
    /// </summary>
    [DataMember]
    public string ReportsAndRequests;
    /// <summary>
    /// ライブラリ
    /// </summary>
    [DataMember]
    public string Library;
    /// <summary>
    /// ディスプレイ座標
    /// </summary>
    [DataMember]
    public string DisplayCoordinates;
    /// <summary>
    /// グローバル座標
    /// </summary>
    [DataMember]
    public string GlobalCoordinates;
    /// <summary>
    /// 全般
    /// </summary>
    [DataMember]
    public string General;
    /// <summary>
    /// ホットキー
    /// </summary>
    [DataMember]
    public string Hotkey;
    /// <summary>
    /// 表示項目
    /// </summary>
    [DataMember]
    public string DisplayItem;
    /// <summary>
    /// 言語
    /// </summary>
    [DataMember]
    public string Language;
    /// <summary>
    /// 翻訳者：
    /// </summary>
    [DataMember]
    public string Translators;
    /// <summary>
    /// 実行時に自動で更新確認
    /// </summary>
    [DataMember]
    public string AutomaticUpdateCheck;
    /// <summary>
    /// ベータバージョンを含めて更新確認
    /// </summary>
    [DataMember]
    public string CheckBetaVersion;
    /// <summary>
    /// ウィンドウ処理を複数登録
    /// </summary>
    [DataMember]
    public string RegisterMultipleWindowActions;
    /// <summary>
    /// 座標指定の方法
    /// </summary>
    [DataMember]
    public string CoordinateSpecificationMethod;
    /// <summary>
    /// 処理間隔 (ミリ秒)
    /// </summary>
    [DataMember]
    public string ProcessingInterval;
    /// <summary>
    /// Windows起動時に実行
    /// </summary>
    [DataMember]
    public string RunAtWindowsStartup;
    /// <summary>
    /// Windows起動時に実行 (管理者権限)
    /// </summary>
    [DataMember]
    public string RunAtWindowsStartupAdministrator;
    /// <summary>
    /// 通常実行ですか？ (管理者権限なし - 「はい」 / 管理者権限あり - 「いいえ」)
    /// </summary>
    [DataMember]
    public string NormalExecution;
    /// <summary>
    /// 次のウィンドウを処理する待ち時間 (ミリ秒)
    /// </summary>
    [DataMember]
    public string WaitTimeToProcessingNextWindow;
    /// <summary>
    /// 全画面ウィンドウがある場合は処理停止
    /// </summary>
    [DataMember]
    public string StopProcessingWhenWindowIsFullScreen;
    /// <summary>
    /// ウィンドウ判定で大文字と小文字を区別する
    /// </summary>
    [DataMember]
    public string CaseSensitiveWindowQueries;
    /// <summary>
    /// 画面外に出る場合は位置やサイズを変更しない
    /// </summary>
    [DataMember]
    public string DoNotChangePositionSizeOutOfScreen;
    /// <summary>
    /// 追加/修正のウィンドウが表示されている場合は処理停止
    /// </summary>
    [DataMember]
    public string StopProcessingShowAddModify;
    /// <summary>
    /// ホットキーは停止させない (全画面ウィンドウがある場合)
    /// </summary>
    [DataMember]
    public string HotkeysDoNotStop;
    /// <summary>
    /// 貼り付ける位置をずらす
    /// </summary>
    [DataMember]
    public string MoveThePastePosition;
    /// <summary>
    /// 画面端に貼り付ける
    /// </summary>
    [DataMember]
    public string PasteToTheEdgeOfScreen;
    /// <summary>
    /// 別のウィンドウに貼り付ける
    /// </summary>
    [DataMember]
    public string PasteIntoAnotherWindow;
    /// <summary>
    /// Ctrlキーを押した状態で貼り付ける
    /// </summary>
    [DataMember]
    public string PasteWithTheCtrlKeyPressed;
    /// <summary>
    /// 貼り付いた時の停止時間 (ミリ秒)
    /// </summary>
    [DataMember]
    public string StopTimeWhenPasting;
    /// <summary>
    /// 貼り付ける判定距離
    /// </summary>
    [DataMember]
    public string DecisionDistanceToPaste;
    /// <summary>
    /// 項目追加
    /// </summary>
    [DataMember]
    public string AddItem;
    /// <summary>
    /// 項目修正
    /// </summary>
    [DataMember]
    public string ItemModification;
    /// <summary>
    /// タイトル名
    /// </summary>
    [DataMember]
    public string TitleName;
    /// <summary>
    /// クラス名
    /// </summary>
    [DataMember]
    public string ClassName;
    /// <summary>
    /// ファイル名
    /// </summary>
    [DataMember]
    public string FileName;
    /// <summary>
    /// ディスプレイ
    /// </summary>
    [DataMember]
    public string Display;
    /// <summary>
    /// ウィンドウ状態
    /// </summary>
    [DataMember]
    public string WindowState;
    /// <summary>
    /// 処理の種類
    /// </summary>
    [DataMember]
    public string ProcessingType;
    /// <summary>
    /// 位置とサイズを指定
    /// </summary>
    [DataMember]
    public string SpecifyPositionAndSize;
    /// <summary>
    /// 位置とサイズ
    /// </summary>
    [DataMember]
    public string PositionAndSize;
    /// <summary>
    /// X座標を移動
    /// </summary>
    [DataMember]
    public string MoveXCoordinate;
    /// <summary>
    /// Y座標を移動
    /// </summary>
    [DataMember]
    public string MoveYCoordinate;
    /// <summary>
    /// 幅を増減
    /// </summary>
    [DataMember]
    public string IncreaseDecreaseWidth;
    /// <summary>
    /// 高さを増減
    /// </summary>
    [DataMember]
    public string IncreaseDecreaseHeight;
    /// <summary>
    /// 幅と高さを増減
    /// </summary>
    [DataMember]
    public string IncreaseDecreaseWidthAndHeight;
    /// <summary>
    /// 「イベント」の処理開始/停止
    /// </summary>
    [DataMember]
    public string StartStopProcessingOfEvent;
    /// <summary>
    /// 「タイマー」の処理開始/停止
    /// </summary>
    [DataMember]
    public string StartStopProcessingOfTimer;
    /// <summary>
    /// 「マグネット」の処理開始/停止
    /// </summary>
    [DataMember]
    public string StartStopProcessingOfMagnet;
    /// <summary>
    /// 常に最前面に表示/解除
    /// </summary>
    [DataMember]
    public string AlwaysShowOrCancelOnTop;
    /// <summary>
    /// 透明度を指定/解除
    /// </summary>
    [DataMember]
    public string SpecifyCancelTransparency;
    /// <summary>
    /// 左端
    /// </summary>
    [DataMember]
    public string LeftEdge;
    /// <summary>
    /// 中央
    /// </summary>
    [DataMember]
    public string Middle;
    /// <summary>
    /// 右端
    /// </summary>
    [DataMember]
    public string RightEdge;
    /// <summary>
    /// 上端
    /// </summary>
    [DataMember]
    public string TopEdge;
    /// <summary>
    /// 下端
    /// </summary>
    [DataMember]
    public string BottomEdge;
    /// <summary>
    /// X
    /// </summary>
    [DataMember]
    public string X;
    /// <summary>
    /// Y
    /// </summary>
    [DataMember]
    public string Y;
    /// <summary>
    /// 幅
    /// </summary>
    [DataMember]
    public string Width;
    /// <summary>
    /// 高さ
    /// </summary>
    [DataMember]
    public string Height;
    /// <summary>
    /// px
    /// </summary>
    [DataMember]
    public string Pixel;
    /// <summary>
    /// %
    /// </summary>
    [DataMember]
    public string Percent;
    /// <summary>
    /// 登録名
    /// </summary>
    [DataMember]
    public string RegisteredName;
    /// <summary>
    /// ウィンドウ判定
    /// </summary>
    [DataMember]
    public string WindowDecide;
    /// <summary>
    /// 処理設定
    /// </summary>
    [DataMember]
    public string ProcessingSettings;
    /// <summary>
    /// 押したまま選択するウィンドウにマウスカーソルを移動
    /// </summary>
    [DataMember]
    public string HoldDownMouseCursorMoveToSelectWindow;
    /// <summary>
    /// ウィンドウ情報取得
    /// </summary>
    [DataMember]
    public string GetWindowInformation;
    /// <summary>
    /// 取得するウィンドウ情報
    /// </summary>
    [DataMember]
    public string WindowInformationToGet;
    /// <summary>
    /// ファイル選択
    /// </summary>
    [DataMember]
    public string FileSelection;
    /// <summary>
    /// 完全一致
    /// </summary>
    [DataMember]
    public string ExactMatch;
    /// <summary>
    /// 部分一致
    /// </summary>
    [DataMember]
    public string PartialMatch;
    /// <summary>
    /// 前方一致
    /// </summary>
    [DataMember]
    public string ForwardMatch;
    /// <summary>
    /// 後方一致
    /// </summary>
    [DataMember]
    public string BackwardMatch;
    /// <summary>
    /// パスを含む
    /// </summary>
    [DataMember]
    public string IncludePath;
    /// <summary>
    /// パスを含まない
    /// </summary>
    [DataMember]
    public string DoNotIncludePath;
    /// <summary>
    /// 指定しない
    /// </summary>
    [DataMember]
    public string DoNotSpecify;
    /// <summary>
    /// タイトルの処理条件
    /// </summary>
    [DataMember]
    public string TitleProcessingConditions;
    /// <summary>
    /// タイトルがないウィンドウは処理しない
    /// </summary>
    [DataMember]
    public string DoNotProcessingUntitledWindow;
    /// <summary>
    /// タイトルがあるウィンドウは処理しない
    /// </summary>
    [DataMember]
    public string DoNotProcessingWindowWithTitle;
    /// <summary>
    /// 項目削除
    /// </summary>
    [DataMember]
    public string DeleteItem;
    /// <summary>
    /// アクティブ
    /// </summary>
    [DataMember]
    public string Active;
    /// <summary>
    /// 項目
    /// </summary>
    [DataMember]
    public string Item;
    /// <summary>
    /// 処理
    /// </summary>
    [DataMember]
    public string Processing;
    /// <summary>
    /// 処理名
    /// </summary>
    [DataMember]
    public string ProcessingName;
    /// <summary>
    /// 項目のウィンドウ情報のみ取得
    /// </summary>
    [DataMember]
    public string GetItemWindowInformationOnly;
    /// <summary>
    /// 基準にするディスプレイ
    /// </summary>
    [DataMember]
    public string DisplayToUseAsStandard;
    /// <summary>
    /// ウィンドウがあるディスプレイ
    /// </summary>
    [DataMember]
    public string DisplayWithWindow;
    /// <summary>
    /// 指定したディスプレイ
    /// </summary>
    [DataMember]
    public string TheSpecifiedDisplay;
    /// <summary>
    /// 指定したディスプレイにある場合のみ
    /// </summary>
    [DataMember]
    public string OnlyIfItIsOnTheSpecifiedDisplay;
    /// <summary>
    /// 1度だけ処理
    /// </summary>
    [DataMember]
    public string ProcessOnlyOnce;
    /// <summary>
    /// ウィンドウが開かれた時
    /// </summary>
    [DataMember]
    public string OnceWindowOpen;
    /// <summary>
    /// このソフトウェアが実行されている間
    /// </summary>
    [DataMember]
    public string OnceWhileItIsRunning;
    /// <summary>
    /// 処理するタイミング
    /// </summary>
    [DataMember]
    public string WhenToProcessing;
    /// <summary>
    /// 有効にするイベントの種類
    /// </summary>
    [DataMember]
    public string TypesOfEventsToEnable;
    /// <summary>
    /// フォアグラウンドが変更された
    /// </summary>
    [DataMember]
    public string TheForegroundHasBeenChanged;
    /// <summary>
    /// 移動及びサイズの変更が終了された
    /// </summary>
    [DataMember]
    public string MoveSizeChangeEnd;
    /// <summary>
    /// 最小化が開始された
    /// </summary>
    [DataMember]
    public string MinimizeStart;
    /// <summary>
    /// 最小化が終了された
    /// </summary>
    [DataMember]
    public string MinimizeEnd;
    /// <summary>
    /// 作成された
    /// </summary>
    [DataMember]
    public string Create;
    /// <summary>
    /// 表示された
    /// </summary>
    [DataMember]
    public string Show;
    /// <summary>
    /// 名前が変更された
    /// </summary>
    [DataMember]
    public string NameChanged;
    /// <summary>
    /// 最前面
    /// </summary>
    [DataMember]
    public string Forefront;
    /// <summary>
    /// 変更しない
    /// </summary>
    [DataMember]
    public string DoNotChange;
    /// <summary>
    /// 常に最前面
    /// </summary>
    [DataMember]
    public string AlwaysForefront;
    /// <summary>
    /// 常に最前面解除
    /// </summary>
    [DataMember]
    public string AlwaysCancelForefront;
    /// <summary>
    /// 透明度指定
    /// </summary>
    [DataMember]
    public string SpecifyTransparency;
    /// <summary>
    /// ウィンドウを閉じる
    /// </summary>
    [DataMember]
    public string CloseWindow;
    /// <summary>
    /// 最初に処理しない回数 (実行直後の処理を遅らせる)
    /// </summary>
    [DataMember]
    public string NumberOfTimesNotToProcessingFirst;
    /// <summary>
    /// 通常のウィンドウ
    /// </summary>
    [DataMember]
    public string NormalWindow;
    /// <summary>
    /// 最大化
    /// </summary>
    [DataMember]
    public string Maximize;
    /// <summary>
    /// 最小化
    /// </summary>
    [DataMember]
    public string Minimize;
    /// <summary>
    /// 「通常のウィンドウ」の時だけ処理する
    /// </summary>
    [DataMember]
    public string ProcessOnlyWhenNormalWindow;
    /// <summary>
    /// 座標指定
    /// </summary>
    [DataMember]
    public string CoordinateSpecification;
    /// <summary>
    /// 幅指定
    /// </summary>
    [DataMember]
    public string WidthSpecification;
    /// <summary>
    /// 高さ指定
    /// </summary>
    [DataMember]
    public string HeightSpecification;
    /// <summary>
    /// 処理しない条件
    /// </summary>
    [DataMember]
    public string ConditionsThatDoNotProcess;
    /// <summary>
    /// クライアントエリアを対象とする
    /// </summary>
    [DataMember]
    public string ClientArea;
    /// <summary>
    /// 位置とサイズを2回処理
    /// </summary>
    [DataMember]
    public string ProcessingPositionAndSizeTwice;
    /// <summary>
    /// タイトル名の除外文字列
    /// </summary>
    [DataMember]
    public string TitleNameExclusionString;
    /// <summary>
    /// サイズ
    /// </summary>
    [DataMember]
    public string Size;
    /// <summary>
    /// 5秒後にウィンドウ情報が取得されるので、5秒以内に情報を取得するウィンドウをアクティブにしてください。
    /// </summary>
    [DataMember]
    public string RetrievedAfterFiveSeconds;
    /// <summary>
    /// 選択しました。
    /// </summary>
    [DataMember]
    public string SelectionComplete;
    /// <summary>
    /// 重複した項目があります。
    /// </summary>
    [DataMember]
    public string ThereAreDuplicateItems;
    /// <summary>
    /// 重複した登録名があります。
    /// </summary>
    [DataMember]
    public string ThereIsADuplicateRegistrationName;
    /// <summary>
    /// 重複した処理名があります。
    /// </summary>
    [DataMember]
    public string ThereIsADuplicateProcessingName;
    /// <summary>
    /// 追加しました。
    /// </summary>
    [DataMember]
    public string Added;
    /// <summary>
    /// 修正しました。
    /// </summary>
    [DataMember]
    public string Modified;
    /// <summary>
    /// 処理するウィンドウの範囲
    /// </summary>
    [DataMember]
    public string ProcessingWindowRange;
    /// <summary>
    /// アクティブウィンドウのみ
    /// </summary>
    [DataMember]
    public string ActiveWindowOnly;
    /// <summary>
    /// 全てのウィンドウ
    /// </summary>
    [DataMember]
    public string AllWindow;
    /// <summary>
    /// 「イベント」のアクティブウィンドウのみ処理
    /// </summary>
    [DataMember]
    public string OnlyActiveWindowEvent;
    /// <summary>
    /// 「タイマー」のアクティブウィンドウのみ処理
    /// </summary>
    [DataMember]
    public string OnlyActiveWindowTimer;
    /// <summary>
    /// 移動量
    /// </summary>
    [DataMember]
    public string AmountOfMovement;
    /// <summary>
    /// サイズ変更量
    /// </summary>
    [DataMember]
    public string SizeChangeAmount;
    /// <summary>
    /// 透明度
    /// </summary>
    [DataMember]
    public string Transparency;
    /// <summary>
    /// 数値計算
    /// </summary>
    [DataMember]
    public string NumericCalculation;
    /// <summary>
    /// 比率
    /// </summary>
    [DataMember]
    public string Ratio;
    /// <summary>
    /// イベントの説明
    /// </summary>
    [DataMember]
    public string ExplanationOfTheEvent;
    /// <summary>
    /// タイマーの説明
    /// </summary>
    [DataMember]
    public string ExplanationOfTheTimer;
    /// <summary>
    /// ホットキーの説明
    /// </summary>
    [DataMember]
    public string HotkeyExplanation;
    /// <summary>
    /// マグネットの説明
    /// </summary>
    [DataMember]
    public string MagnetExplanation;
    /// <summary>
    /// ウィンドウが画面外の場合は画面内に移動
    /// </summary>
    [DataMember]
    public string WindowMoveOutsideToInside;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public LanguagesWindow()
    {
        SetDefaultValue();
    }

    [OnDeserializing]
    public void DefaultDeserializing(
        StreamingContext context
        )
    {
        SetDefaultValue();
    }

    public ExtensionDataObject? ExtensionData { get; set; }

    /// <summary>
    /// デフォルト値を設定
    /// </summary>
    private void SetDefaultValue()
    {
        Ok = "Ok";
        Yes = "はい";
        No = "いいえ";
        Cancel = "キャンセル";
        ShareSettings = "全てのユーザーで設定情報を共有しますか？";
        Open = "開く";
        BatchProcessingOfEvent = "「イベント」の一括処理";
        BatchProcessingOfTimer = "「タイマー」の一括処理";
        DisplayInformationUpdate = "ディスプレイ情報更新";
        End = "終了";
        UpdateCheckFailed = "更新確認に失敗しました。";
        LatestVersion = "最新バージョンです。";
        ThereIsTheLatestVersion = "最新バージョンがあります。";
        Event = "イベント";
        Timer = "タイマー";
        WindowProcessing = "ウィンドウ処理";
        Magnet = "マグネット";
        Setting = "設定";
        Information = "情報";
        Add = "追加";
        Modify = "修正";
        Delete = "削除";
        Select = "選択";
        Enabling = "有効化";
        Disabling = "無効化";
        MoveUp = "上に移動";
        MoveDown = "下に移動";
        Copy = "コピー";
        ProcessingState = "処理状態";
        Deleted = "削除しました。";
        AllowDelete = "削除していいですか？";
        SoftwareName = "ソフトウェア名";
        Version = "バージョン";
        UpdateCheck = "更新確認";
        Manual = "説明書";
        UpdateHistory = "更新履歴";
        Website = "ウェブサイト";
        ReportsAndRequests = "報告、要望";
        Library = "ライブラリ";
        DisplayCoordinates = "ディスプレイ座標";
        GlobalCoordinates = "グローバル座標";
        General = "全般";
        Hotkey = "ホットキー";
        DisplayItem = "表示項目";
        Language = "言語";
        Translators = "翻訳者：";
        AutomaticUpdateCheck = "実行時に自動で更新確認";
        CheckBetaVersion = "ベータバージョンを含めて更新確認";
        RegisterMultipleWindowActions = "ウィンドウ処理を複数登録";
        CoordinateSpecificationMethod = "座標指定の方法";
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
        MoveThePastePosition = "貼り付ける位置をずらす";
        PasteToTheEdgeOfScreen = "画面端に貼り付ける";
        PasteIntoAnotherWindow = "別のウィンドウに貼り付ける";
        PasteWithTheCtrlKeyPressed = "Ctrlキーを押した状態で貼り付ける";
        StopTimeWhenPasting = "貼り付いた時の停止時間 (ミリ秒)";
        DecisionDistanceToPaste = "貼り付ける判定距離";
        AddItem = "項目追加";
        ItemModification = "項目修正";
        TitleName = "タイトル名";
        ClassName = "クラス名";
        FileName = "ファイル名";
        Display = "ディスプレイ";
        WindowState = "ウィンドウの状態";
        ProcessingType = "処理の種類";
        SpecifyPositionAndSize = "位置とサイズを指定";
        PositionAndSize = "位置とサイズ";
        MoveXCoordinate = "X座標を移動";
        MoveYCoordinate = "Y座標を移動";
        IncreaseDecreaseWidth = "幅を増減";
        IncreaseDecreaseHeight = "高さを増減";
        IncreaseDecreaseWidthAndHeight = "幅と高さを増減";
        StartStopProcessingOfEvent = "「イベント」の処理開始/停止";
        StartStopProcessingOfTimer = "「タイマー」の処理開始/停止";
        StartStopProcessingOfMagnet = "「マグネット」の処理開始/停止";
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
        WindowInformationToGet = "取得するウィンドウ情報";
        FileSelection = "ファイル選択";
        ExactMatch = "完全一致";
        PartialMatch = "部分一致";
        ForwardMatch = "前方一致";
        BackwardMatch = "後方一致";
        IncludePath = "パスを含む";
        DoNotIncludePath = "パスを含まない";
        DoNotSpecify = "指定しない";
        TitleProcessingConditions = "タイトルの処理条件";
        DoNotProcessingUntitledWindow = "タイトルがないウィンドウは処理しない";
        DoNotProcessingWindowWithTitle = "タイトルがあるウィンドウは処理しない";
        DeleteItem = "項目削除";
        Active = "アクティブ";
        Item = "項目";
        Processing = "処理";
        ProcessingName = "処理名";
        GetItemWindowInformationOnly = "項目のウィンドウ情報のみ取得";
        DisplayToUseAsStandard = "基準にするディスプレイ";
        DisplayWithWindow = "ウィンドウがあるディスプレイ";
        TheSpecifiedDisplay = "指定したディスプレイ";
        OnlyIfItIsOnTheSpecifiedDisplay = "指定したディスプレイにある場合のみ";
        ProcessOnlyOnce = "1度だけ処理";
        OnceWindowOpen = "ウィンドウが開かれた時";
        OnceWhileItIsRunning = "このソフトウェアが実行されている間";
        WhenToProcessing = "処理するタイミング";
        TypesOfEventsToEnable = "有効にするイベントの種類";
        TheForegroundHasBeenChanged = "フォアグラウンドが変更された";
        MoveSizeChangeEnd = "移動及びサイズの変更が終了された";
        MinimizeStart = "最小化が開始された";
        MinimizeEnd = "最小化が終了された";
        Create = "作成された";
        Show = "表示された";
        NameChanged = "名前が変更された";
        Forefront = "最前面";
        DoNotChange = "変更しない";
        AlwaysForefront = "常に最前面";
        AlwaysCancelForefront = "常に最前面解除";
        SpecifyTransparency = "透明度指定";
        CloseWindow = "ウィンドウを閉じる";
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
        ProcessingPositionAndSizeTwice = "位置とサイズを2回処理";
        TitleNameExclusionString = "タイトル名の除外文字列";
        Size = "サイズ";
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
        OnlyActiveWindowEvent = "「イベント」のアクティブウィンドウのみ処理";
        OnlyActiveWindowTimer = "「タイマー」のアクティブウィンドウのみ処理";
        AmountOfMovement = "移動量";
        SizeChangeAmount = "サイズ変更量";
        Transparency = "透明度";
        NumericCalculation = "数値計算";
        Ratio = "比率";
        ExplanationOfTheEvent = "ウィンドウを指定して自動で処理させることができます。自動処理させずにホットキーだけで処理したい場合は、チェックを外して無効状態にしてください。";
        ExplanationOfTheTimer = "ウィンドウを指定して自動で処理させることができます。自動処理させずにホットキーだけで処理したい場合は、チェックを外して無効状態にしてください。「処理間隔」は全てのウィンドウ処理が終わってから、次に処理を開始するまでの間隔です。";
        HotkeyExplanation = "ホットキーでアクティブウィンドウ(入力を受け付けている状態)を処理させることができます。「選択」ボタンではホットキーを使用しなくても、処理させたいウィンドウを選択して処理させることができます。";
        MagnetExplanation = "マウスでウィンドウを移動させている場合に、画面端や別のウィンドウに貼り付けます。";
        WindowMoveOutsideToInside = "ウィンドウが画面外の場合は画面内に移動";
    }
}
