namespace Swindom;

/// <summary>
/// ディスプレイの設定を変更する方法
/// </summary>
public enum DisplayChangeMode
{
    /// <summary>
    /// 自動
    /// </summary>
    Auto,
    /// <summary>
    /// 1枚は自動/2枚以上は手動
    /// </summary>
    AutoOrManual,
    /// <summary>
    /// 手動
    /// </summary>
    Manual,
}
