namespace Swindom;

/// <summary>
/// ホットキー管理処理
/// </summary>
public static class HotkeyControlProcessing
{
    /// <summary>
    /// ホットキーを登録
    /// </summary>
    /// <param name="hotkeyInformation">ホットキー情報</param>
    /// <returns>ホットキーの識別子 (失敗「-1」)</returns>
    public static int RegisterHotkey(
        FreeEcho.FEHotKeyWPF.HotKeyInformation hotkeyInformation
        )
    {
        int id = -1;        // ホットキーの識別子
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
            id = GetAvailableId();
            NativeMethods.RegisterHotKey(ApplicationData.MainHwnd, id, modKey, (uint)hotkeyInformation.KeyCharacter);
            ApplicationData.UsedHotkeyId.Add(id);
        }

        return id;
    }

    /// <summary>
    /// ホットキーを解除
    /// </summary>
    /// <param name="id">ID</param>
    public static void UnregisterHotkey(
        int id
        )
    {
        if (id != -1)
        {
            NativeMethods.UnregisterHotKey(ApplicationData.MainHwnd, id);
            ApplicationData.UsedHotkeyId.Remove(id);
        }
    }

    /// <summary>
    /// ホットキーの使用可能な識別子を取得
    /// </summary>
    /// <returns>使用可能な識別子</returns>
    public static int GetAvailableId()
    {
        int id = 0;

        // 使用可能範囲は0x0000から0xBFFFまで
        for (int count = 0x0000; count < 0xBFFF; count++)
        {
            if (ApplicationData.UsedHotkeyId.Contains(count) == false)
            {
                id = count;
                break;
            }
        }

        return id;
    }
}
