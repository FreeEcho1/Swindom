namespace Swindom;

/// <summary>
/// 「ホットキー」機能の項目情報
/// </summary>
public class HotkeyItemInformation
{
    /// <summary>
    /// 登録名
    /// </summary>
    public string RegisteredName;
    /// <summary>
    /// 処理の種類
    /// </summary>
    private HotkeyProcessingType PrivateProcessingType;
    /// <summary>
    /// 処理の種類
    /// </summary>
    public HotkeyProcessingType ProcessingType
    {
        get
        {
            return PrivateProcessingType;
        }
        set
        {
            PrivateProcessingType = value;
            ProcessingValue = PrivateProcessingValue;
        }
    }
    /// <summary>
    /// 基準にするディスプレイ
    /// </summary>
    public StandardDisplay StandardDisplay;
    /// <summary>
    /// 位置とサイズ
    /// </summary>
    public PositionSize PositionSize;
    /// <summary>
    /// 移動量の最小値
    /// </summary>
    public const int MinimumAmountOfMovement = int.MinValue;
    /// <summary>
    /// 移動量の最大値
    /// </summary>
    public const int MaximumAmountOfMovement = int.MaxValue;
    /// <summary>
    /// サイズ変更量の最小値
    /// </summary>
    public const int MinimumSizeChangeAmount = int.MinValue;
    /// <summary>
    /// サイズ変更量の最大値
    /// </summary>
    public const int MaximumSizeChangeAmount = int.MaxValue;
    /// <summary>
    /// 透明度の最小値
    /// </summary>
    public const int MinimumTransparency = 0;
    /// <summary>
    /// 透明度の最大値
    /// </summary>
    public const int MaximumTransparency = 255;
    /// <summary>
    /// 処理値 (移動量、サイズ変更量、透明度)
    /// </summary>
    private int PrivateProcessingValue;
    /// <summary>
    /// 処理値 (移動量、サイズ変更量、透明度)
    /// </summary>
    public int ProcessingValue
    {
        get
        {
            return PrivateProcessingValue;
        }
        set
        {
            switch (ProcessingType)
            {
                case HotkeyProcessingType.MoveX:
                case HotkeyProcessingType.MoveY:
                    if (value < MinimumAmountOfMovement)
                    {
                        PrivateProcessingValue = MinimumAmountOfMovement;
                    }
                    else if (MaximumAmountOfMovement < value)
                    {
                        PrivateProcessingValue = MaximumAmountOfMovement;
                    }
                    else
                    {
                        PrivateProcessingValue = value;
                    }
                    break;
                case HotkeyProcessingType.IncreaseDecreaseWidth:
                case HotkeyProcessingType.IncreaseDecreaseHeight:
                case HotkeyProcessingType.IncreaseDecreaseWidthHeight:
                    if (value < MinimumSizeChangeAmount)
                    {
                        PrivateProcessingValue = MinimumSizeChangeAmount;
                    }
                    else if (MaximumSizeChangeAmount < value)
                    {
                        PrivateProcessingValue = MaximumSizeChangeAmount;
                    }
                    else
                    {
                        PrivateProcessingValue = value;
                    }
                    break;
                case HotkeyProcessingType.SpecifyTransparencyOrCancel:
                    if (value < MinimumTransparency)
                    {
                        PrivateProcessingValue = MinimumTransparency;
                    }
                    else if (MaximumTransparency < value)
                    {
                        PrivateProcessingValue = MaximumTransparency;
                    }
                    else
                    {
                        PrivateProcessingValue = value;
                    }
                    break;
            }
        }
    }
    /// <summary>
    /// ホットキー情報
    /// </summary>
    public FreeEcho.FEHotKeyWPF.HotKeyInformation Hotkey;

    // ------------------ 設定ファイルに保存しないデータ ------------------ //
    /// <summary>
    /// ホットキーの識別子 (ホットキー無し「-1」)
    /// </summary>
    public int HotkeyId;
    // ------------------ 設定ファイルに保存しないデータ ------------------ //

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public HotkeyItemInformation()
    {
        RegisteredName = "";
        ProcessingType = HotkeyProcessingType.PositionSize;
        StandardDisplay = StandardDisplay.SpecifiedDisplay;
        PositionSize = new();
        ProcessingValue = 0;
        Hotkey = new();
        HotkeyId = -1;
    }

    /// <summary>
    /// コンストラクタ
    /// </summary>
    /// <param name="item">情報</param>
    public HotkeyItemInformation(
        HotkeyItemInformation item
        )
    {
        RegisteredName = item.RegisteredName;
        ProcessingType = item.ProcessingType;
        StandardDisplay = item.StandardDisplay;
        PositionSize = new(item.PositionSize);
        ProcessingValue = item.ProcessingValue;
        Hotkey = new(item.Hotkey);
        HotkeyId = -1;
    }

    /// <summary>
    /// コピー
    /// </summary>
    /// <param name="item">情報</param>
    public void Copy(
        HotkeyItemInformation item
        )
    {
        RegisteredName = item.RegisteredName;
        ProcessingType = item.ProcessingType;
        StandardDisplay = item.StandardDisplay;
        PositionSize = new(item.PositionSize);
        ProcessingValue = item.ProcessingValue;
        Hotkey = new(item.Hotkey);
        HotkeyId = -1;
    }
}
