using ModernWpf.Controls;

namespace Swindom;

/// <summary>
/// コントロール処理
/// </summary>
public static class ControlsProcessing
{
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
    /// NumberBoxの値の連動処理
    /// </summary>
    /// <param name="numberBox1">NumberBox</param>
    /// <param name="numberBox2">NumberBox</param>
    /// <param name="args">NumberBoxValueChangedEventArgs</param>
    /// <param name="interlockRatioValues">連動での変更かの値 (いいえ「false」/はい「true」)</param>
    public static void InterlockProcessing(
        NumberBox numberBox1,
        NumberBox numberBox2,
        NumberBoxValueChangedEventArgs args,
        ref bool interlockRatioValues
        )
    {
        if (interlockRatioValues)
        {
            interlockRatioValues = false;
        }
        else if (numberBox1.Value > 0 && numberBox2.Value > 0)
        {
            if (args.OldValue > 0)
            {
                interlockRatioValues = true;
                double setValue = numberBox1.Value / args.OldValue * numberBox2.Value;
                if (setValue < 0)
                {
                    setValue = 0;
                }
                numberBox2.Value = setValue;
            }
        }
    }
}
