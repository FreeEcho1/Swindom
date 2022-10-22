namespace Swindom;

/// <summary>
/// 設定
/// </summary>
[DataContract]
public class Settings : IExtensibleDataObject
{
    /// <summary>
    /// メインウィンドウの位置とサイズ
    /// </summary>
    [DataMember]
    public Rectangle MainWindowRectangle;
    /// <summary>
    /// メインウィンドウのウィンドウ状態 (通常のウィンドウ、最大化)
    /// </summary>
    [DataMember]
    public WindowState WindowStateMainWindow;
    /// <summary>
    /// 言語
    /// </summary>
    [DataMember]
    public string Language;
    /// <summary>
    /// 座標指定の種類
    /// </summary>
    [DataMember]
    public CoordinateType CoordinateType;
    /// <summary>
    /// 実行時に自動で更新確認 (無効「false」/有効「true」)
    /// </summary>
    [DataMember]
    public bool AutomaticallyUpdateCheck;
    /// <summary>
    /// ベータバージョンを含めて更新確認 (無効「false」/有効「true」)
    /// </summary>
    [DataMember]
    public bool CheckBetaVersion;
    /// <summary>
    /// 長いパスを使用 (32767文字)
    /// </summary>
    [DataMember]
    public bool UseLongPath;
    /// <summary>
    /// 「貼り付ける位置をずらす」情報
    /// </summary>
    [DataMember]
    public ShiftPastePosition ShiftPastePosition;

    /// <summary>
    /// 「イベント」機能の設定情報
    /// </summary>
    [DataMember]
    public EventInformation EventInformation;
    /// <summary>
    /// 「タイマー」機能の設定情報
    /// </summary>
    [DataMember]
    public TimerInformation TimerInformation;
    /// <summary>
    /// 「マグネット」機能の設定情報
    /// </summary>
    [DataMember]
    public MagnetInformation MagnetInformation;
    /// <summary>
    /// 「ホットキー」機能の設定情報
    /// </summary>
    [DataMember]
    public HotkeyInformation HotkeyInformation;

    /// <summary>
    /// コンストラクタ
    /// </summary>
    public Settings()
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
        MainWindowRectangle = new(0, 0, 600, 400);
        WindowStateMainWindow = System.Windows.WindowState.Normal;
        Language = "";
        CoordinateType = CoordinateType.Display;
        AutomaticallyUpdateCheck = false;
        CheckBetaVersion = false;
        UseLongPath = false;
        ShiftPastePosition = new();
        EventInformation = new();
        TimerInformation = new();
        MagnetInformation = new();
        HotkeyInformation = new();
    }

    /// <summary>
    /// ファイルに書き込む
    /// </summary>
    /// <param name="path">ファイルのパス</param>
    /// <returns>結果 (失敗「false」/成功「true」)</returns>
    public bool WriteFile(
        string path
        )
    {
        bool result = false;        // 結果

        try
        {
            XDocument document = new();
            XElement element1, element2, element3, element4, element5, element6;

            element1 = new("Settings");
            // Start MainWindow
            element2 = new("MainWindow");
            element2.Add(new XElement("X", MainWindowRectangle.X.ToString()));
            element2.Add(new XElement("Y", MainWindowRectangle.Y.ToString()));
            element2.Add(new XElement("Width", MainWindowRectangle.Width.ToString()));
            element2.Add(new XElement("Height", MainWindowRectangle.Height.ToString()));
            element2.Add(new XElement("WindowState", Enum.GetName(typeof(WindowState), WindowStateMainWindow)));
            element1.Add(element2);
            // End MainWindow
            element1.Add(new XElement("Language", Language));
            element1.Add(new XElement("CoordinateType", Enum.GetName(typeof(CoordinateType), CoordinateType)));
            element1.Add(new XElement("AutomaticallyUpdateCheck", AutomaticallyUpdateCheck.ToString()));
            element1.Add(new XElement("CheckBetaVersion", CheckBetaVersion.ToString()));
            element1.Add(new XElement("UseLongPath", UseLongPath.ToString()));
            // Start ShiftPastePosition
            element2 = new("ShiftPastePosition");
            element2.Add(new XElement("Enabled", ShiftPastePosition.Enabled.ToString()));
            element2.Add(new XElement("Left", ShiftPastePosition.Left.ToString()));
            element2.Add(new XElement("Top", ShiftPastePosition.Top.ToString()));
            element2.Add(new XElement("Right", ShiftPastePosition.Right.ToString()));
            element2.Add(new XElement("Bottom", ShiftPastePosition.Bottom.ToString()));
            element1.Add(element2);
            // End ShiftPastePosition
            // Start EventInformation
            element2 = new("EventInformation");
            element2.Add(new XElement("Enabled", EventInformation.Enabled.ToString()));
            element2.Add(new XElement("HotkeysDoNotStopFullScreen", EventInformation.HotkeysDoNotStopFullScreen.ToString()));
            element2.Add(new XElement("StopProcessingFullScreen", EventInformation.StopProcessingFullScreen.ToString()));
            element2.Add(new XElement("RegisterMultiple", EventInformation.RegisterMultiple.ToString()));
            element2.Add(new XElement("CaseSensitiveWindowQueries", EventInformation.CaseSensitiveWindowQueries.ToString()));
            element2.Add(new XElement("DoNotChangeOutOfScreen", EventInformation.DoNotChangeOutOfScreen.ToString()));
            element2.Add(new XElement("StopProcessingShowAddModifyWindow", EventInformation.StopProcessingShowAddModifyWindow.ToString()));
            element2.Add(new XElement("AddModifyWindowWidth", EventInformation.AddModifyWindowSize.Width.ToString()));
            element2.Add(new XElement("AddModifyWindowHeight", EventInformation.AddModifyWindowSize.Height.ToString()));
            element2.Add(new XElement("AcquiredItemsTitleName", EventInformation.AcquiredItems.TitleName.ToString()));
            element2.Add(new XElement("AcquiredItemsClassName", EventInformation.AcquiredItems.ClassName.ToString()));
            element2.Add(new XElement("AcquiredItemsFileName", EventInformation.AcquiredItems.FileName.ToString()));
            element2.Add(new XElement("AcquiredItemsWindowState", EventInformation.AcquiredItems.WindowState.ToString()));
            element2.Add(new XElement("AcquiredItemsX", EventInformation.AcquiredItems.X.ToString()));
            element2.Add(new XElement("AcquiredItemsY", EventInformation.AcquiredItems.Y.ToString()));
            element2.Add(new XElement("AcquiredItemsWidth", EventInformation.AcquiredItems.Width.ToString()));
            element2.Add(new XElement("AcquiredItemsHeight", EventInformation.AcquiredItems.Height.ToString()));
            element2.Add(new XElement("AcquiredItemsDisplay", EventInformation.AcquiredItems.Display.ToString()));
            element2.Add(new XElement("EventTypeForeground", EventInformation.EventTypeInformation.Foreground.ToString()));
            element2.Add(new XElement("EventTypeMoveSizeEnd", EventInformation.EventTypeInformation.MoveSizeEnd.ToString()));
            element2.Add(new XElement("EventTypeMinimizeStart", EventInformation.EventTypeInformation.MinimizeStart.ToString()));
            element2.Add(new XElement("EventTypeMinimizeEnd", EventInformation.EventTypeInformation.MinimizeEnd.ToString()));
            element2.Add(new XElement("EventTypeCreate", EventInformation.EventTypeInformation.Create.ToString()));
            element2.Add(new XElement("EventTypeShow", EventInformation.EventTypeInformation.Show.ToString()));
            element2.Add(new XElement("EventTypeNameChange", EventInformation.EventTypeInformation.NameChange.ToString()));
            element3 = new("ItemsList");
            foreach (EventItemInformation nowItem in EventInformation.Items)
            {
                element4 = new("Item");
                element4.Add(new XElement("Enabled", nowItem.Enabled.ToString()));
                element4.Add(new XElement("RegisteredName", nowItem.RegisteredName));
                element4.Add(new XElement("TitleName", nowItem.TitleName));
                element4.Add(new XElement("TitleNameMatchCondition", Enum.GetName(typeof(NameMatchCondition), nowItem.TitleNameMatchCondition)));
                element4.Add(new XElement("ClassName", nowItem.ClassName));
                element4.Add(new XElement("ClassNameMatchCondition", Enum.GetName(typeof(NameMatchCondition), nowItem.ClassNameMatchCondition)));
                element4.Add(new XElement("FileName", nowItem.FileName));
                element4.Add(new XElement("FileNameMatchCondition", Enum.GetName(typeof(FileNameMatchCondition), nowItem.FileNameMatchCondition)));
                element4.Add(new XElement("ProcessingOnlyOnce", Enum.GetName(typeof(ProcessingOnlyOnce), nowItem.ProcessingOnlyOnce)));
                element4.Add(new XElement("CloseWindow", nowItem.CloseWindow.ToString()));
                element4.Add(new XElement("StandardDisplay", Enum.GetName(typeof(StandardDisplay), nowItem.StandardDisplay)));
                element4.Add(new XElement("TitleProcessingConditions", Enum.GetName(typeof(TitleProcessingConditions), nowItem.TitleProcessingConditions)));
                element5 = new("ItemsList");
                foreach (WindowProcessingInformation nowWPI in nowItem.WindowProcessingInformation)
                {
                    element6 = new("Item");
                    element6.Add(new XElement("ProcessingName", nowWPI.ProcessingName));
                    element6.Add(new XElement("Forefront", Enum.GetName(typeof(Forefront), nowWPI.Forefront)));
                    element6.Add(new XElement("EnabledTransparency", nowWPI.EnabledTransparency.ToString()));
                    element6.Add(new XElement("Transparency", nowWPI.Transparency.ToString()));
                    element6.Add(new XElement("HotkeyAlt", nowWPI.Hotkey.Alt.ToString()));
                    element6.Add(new XElement("HotkeyCtrl", nowWPI.Hotkey.Ctrl.ToString()));
                    element6.Add(new XElement("HotkeyShift", nowWPI.Hotkey.Shift.ToString()));
                    element6.Add(new XElement("HotkeyWindows", nowWPI.Hotkey.Windows.ToString()));
                    element6.Add(new XElement("HotkeyKeyCharacter", nowWPI.Hotkey.KeyCharacter.ToString()));
                    element6.Add(new XElement("WindowState", Enum.GetName(typeof(SettingsWindowState), nowWPI.SettingsWindowState)));
                    element6.Add(new XElement("OnlyNormalWindow", nowWPI.OnlyNormalWindow.ToString()));
                    element6.Add(new XElement("Display", nowWPI.PositionSize.Display));
                    element6.Add(new XElement("X", nowWPI.PositionSize.Position.X.ToString()));
                    element6.Add(new XElement("Y", nowWPI.PositionSize.Position.Y.ToString()));
                    element6.Add(new XElement("Width", nowWPI.PositionSize.Size.Width.ToString()));
                    element6.Add(new XElement("Height", nowWPI.PositionSize.Size.Height.ToString()));
                    element6.Add(new XElement("XType", Enum.GetName(typeof(WindowXType), nowWPI.PositionSize.XType)));
                    element6.Add(new XElement("YType", Enum.GetName(typeof(WindowYType), nowWPI.PositionSize.YType)));
                    element6.Add(new XElement("WidthType", Enum.GetName(typeof(WindowSizeType), nowWPI.PositionSize.WidthType)));
                    element6.Add(new XElement("HeightType", Enum.GetName(typeof(WindowSizeType), nowWPI.PositionSize.HeightType)));
                    element6.Add(new XElement("XValueType", Enum.GetName(typeof(PositionSizeValueType), nowWPI.PositionSize.XValueType)));
                    element6.Add(new XElement("YValueType", Enum.GetName(typeof(PositionSizeValueType), nowWPI.PositionSize.YValueType)));
                    element6.Add(new XElement("WidthValueType", Enum.GetName(typeof(PositionSizeValueType), nowWPI.PositionSize.WidthValueType)));
                    element6.Add(new XElement("HeightValueType", Enum.GetName(typeof(PositionSizeValueType), nowWPI.PositionSize.HeightValueType)));
                    element6.Add(new XElement("ClientArea", nowWPI.PositionSize.ClientArea.ToString()));
                    element6.Add(new XElement("ProcessingPositionAndSizeTwice", nowWPI.PositionSize.ProcessingPositionAndSizeTwice.ToString()));
                    element6.Add(new XElement("Active", nowWPI.Active.ToString()));
                    element5.Add(element6);
                }
                element4.Add(element5);
                element5 = new("DoNotProcessingSizeList");
                foreach (System.Drawing.Size nowSize in nowItem.DoNotProcessingSize)
                {
                    element6 = new("Item");
                    element6.Add(new XElement("Width", nowSize.Width.ToString()));
                    element6.Add(new XElement("Height", nowSize.Height.ToString()));
                    element5.Add(element6);
                }
                element4.Add(element5);
                element5 = new("DoNotProcessingTitleNameList");
                foreach (string nowTitleName in nowItem.DoNotProcessingTitleName)
                {
                    element6 = new("Item");
                    element6.Add(new XElement("String", nowTitleName));
                    element5.Add(element6);
                }
                element4.Add(element5);
                element4.Add(new XElement("DifferentVersionVersion", nowItem.DifferentVersionVersion));
                element4.Add(new XElement("DifferentVersionAnnounce", nowItem.DifferentVersionAnnounce.ToString()));
                element4.Add(new XElement("Foreground", nowItem.WindowEventData.Foreground.ToString()));
                element4.Add(new XElement("MoveSizeEnd", nowItem.WindowEventData.MoveSizeEnd.ToString()));
                element4.Add(new XElement("MinimizeStart", nowItem.WindowEventData.MinimizeStart.ToString()));
                element4.Add(new XElement("MinimizeEnd", nowItem.WindowEventData.MinimizeEnd.ToString()));
                element4.Add(new XElement("Create", nowItem.WindowEventData.Create.ToString()));
                element4.Add(new XElement("Show", nowItem.WindowEventData.Show.ToString()));
                element4.Add(new XElement("NameChange", nowItem.WindowEventData.NameChange.ToString()));
                element3.Add(element4);
            }
            element2.Add(element3);
            element1.Add(element2);
            // End EventInformation
            // Start TimerInformation
            element2 = new("TimerInformation");
            element2.Add(new XElement("Enabled", TimerInformation.Enabled.ToString()));
            element2.Add(new XElement("HotkeysDoNotStopFullScreen", TimerInformation.HotkeysDoNotStopFullScreen.ToString()));
            element2.Add(new XElement("StopProcessingFullScreen", TimerInformation.StopProcessingFullScreen.ToString()));
            element2.Add(new XElement("RegisterMultiple", TimerInformation.RegisterMultiple.ToString()));
            element2.Add(new XElement("CaseSensitiveWindowQueries", TimerInformation.CaseSensitiveWindowQueries.ToString()));
            element2.Add(new XElement("DoNotChangeOutOfScreen", TimerInformation.DoNotChangeOutOfScreen.ToString()));
            element2.Add(new XElement("StopProcessingShowAddModifyWindow", TimerInformation.StopProcessingShowAddModifyWindow.ToString()));
            element2.Add(new XElement("AddModifyWindowWidth", TimerInformation.AddModifyWindowSize.Width.ToString()));
            element2.Add(new XElement("AddModifyWindowHeight", TimerInformation.AddModifyWindowSize.Height.ToString()));
            element2.Add(new XElement("AcquiredItemsTitleName", TimerInformation.AcquiredItems.TitleName.ToString()));
            element2.Add(new XElement("AcquiredItemsClassName", TimerInformation.AcquiredItems.ClassName.ToString()));
            element2.Add(new XElement("AcquiredItemsFileName", TimerInformation.AcquiredItems.FileName.ToString()));
            element2.Add(new XElement("AcquiredItemsWindowState", TimerInformation.AcquiredItems.WindowState.ToString()));
            element2.Add(new XElement("AcquiredItemsX", TimerInformation.AcquiredItems.X.ToString()));
            element2.Add(new XElement("AcquiredItemsY", TimerInformation.AcquiredItems.Y.ToString()));
            element2.Add(new XElement("AcquiredItemsWidth", TimerInformation.AcquiredItems.Width.ToString()));
            element2.Add(new XElement("AcquiredItemsHeight", TimerInformation.AcquiredItems.Height.ToString()));
            element2.Add(new XElement("AcquiredItemsDisplay", TimerInformation.AcquiredItems.Display.ToString()));
            element2.Add(new XElement("ProcessingWindowRange", Enum.GetName(typeof(ProcessingWindowRange), TimerInformation.ProcessingWindowRange)));
            element2.Add(new XElement("ProcessingInterval", TimerInformation.ProcessingInterval.ToString()));
            element2.Add(new XElement("WaitTimeToProcessingNextWindow", TimerInformation.WaitTimeToProcessingNextWindow.ToString()));
            element3 = new("ItemsList");
            foreach (TimerItemInformation nowItem in TimerInformation.Items)
            {
                element4 = new("Item");
                element4.Add(new XElement("Enabled", nowItem.Enabled.ToString()));
                element4.Add(new XElement("RegisteredName", nowItem.RegisteredName));
                element4.Add(new XElement("TitleName", nowItem.TitleName));
                element4.Add(new XElement("TitleNameMatchCondition", Enum.GetName(typeof(NameMatchCondition), nowItem.TitleNameMatchCondition)));
                element4.Add(new XElement("ClassName", nowItem.ClassName));
                element4.Add(new XElement("ClassNameMatchCondition", Enum.GetName(typeof(NameMatchCondition), nowItem.ClassNameMatchCondition)));
                element4.Add(new XElement("FileName", nowItem.FileName));
                element4.Add(new XElement("FileNameMatchCondition", Enum.GetName(typeof(FileNameMatchCondition), nowItem.FileNameMatchCondition)));
                element4.Add(new XElement("ProcessingOnlyOnce", System.Enum.GetName(typeof(ProcessingOnlyOnce), nowItem.ProcessingOnlyOnce)));
                element4.Add(new XElement("CloseWindow", nowItem.CloseWindow.ToString()));
                element4.Add(new XElement("StandardDisplay", Enum.GetName(typeof(StandardDisplay), nowItem.StandardDisplay)));
                element4.Add(new XElement("TitleProcessingConditions", Enum.GetName(typeof(TitleProcessingConditions), nowItem.TitleProcessingConditions)));
                element5 = new("ItemsList");
                foreach (WindowProcessingInformation nowWPI in nowItem.WindowProcessingInformation)
                {
                    element6 = new("Item");
                    element6.Add(new XElement("ProcessingName", nowWPI.ProcessingName));
                    element6.Add(new XElement("Forefront", Enum.GetName(typeof(Forefront), nowWPI.Forefront)));
                    element6.Add(new XElement("EnabledTransparency", nowWPI.EnabledTransparency.ToString()));
                    element6.Add(new XElement("Transparency", nowWPI.Transparency.ToString()));
                    element6.Add(new XElement("HotkeyAlt", nowWPI.Hotkey.Alt.ToString()));
                    element6.Add(new XElement("HotkeyCtrl", nowWPI.Hotkey.Ctrl.ToString()));
                    element6.Add(new XElement("HotkeyShift", nowWPI.Hotkey.Shift.ToString()));
                    element6.Add(new XElement("HotkeyWindows", nowWPI.Hotkey.Windows.ToString()));
                    element6.Add(new XElement("HotkeyKeyCharacter", nowWPI.Hotkey.KeyCharacter.ToString()));
                    element6.Add(new XElement("WindowState", Enum.GetName(typeof(SettingsWindowState), nowWPI.SettingsWindowState)));
                    element6.Add(new XElement("OnlyNormalWindow", nowWPI.OnlyNormalWindow.ToString()));
                    element6.Add(new XElement("Display", nowWPI.PositionSize.Display));
                    element6.Add(new XElement("X", nowWPI.PositionSize.Position.X.ToString()));
                    element6.Add(new XElement("Y", nowWPI.PositionSize.Position.Y.ToString()));
                    element6.Add(new XElement("Width", nowWPI.PositionSize.Size.Width.ToString()));
                    element6.Add(new XElement("Height", nowWPI.PositionSize.Size.Height.ToString()));
                    element6.Add(new XElement("XType", Enum.GetName(typeof(WindowXType), nowWPI.PositionSize.XType)));
                    element6.Add(new XElement("YType", Enum.GetName(typeof(WindowYType), nowWPI.PositionSize.YType)));
                    element6.Add(new XElement("WidthType", Enum.GetName(typeof(WindowSizeType), nowWPI.PositionSize.WidthType)));
                    element6.Add(new XElement("HeightType", Enum.GetName(typeof(WindowSizeType), nowWPI.PositionSize.HeightType)));
                    element6.Add(new XElement("XValueType", Enum.GetName(typeof(PositionSizeValueType), nowWPI.PositionSize.XValueType)));
                    element6.Add(new XElement("YValueType", Enum.GetName(typeof(PositionSizeValueType), nowWPI.PositionSize.YValueType)));
                    element6.Add(new XElement("WidthValueType", Enum.GetName(typeof(PositionSizeValueType), nowWPI.PositionSize.WidthValueType)));
                    element6.Add(new XElement("HeightValueType", Enum.GetName(typeof(PositionSizeValueType), nowWPI.PositionSize.HeightValueType)));
                    element6.Add(new XElement("ClientArea", nowWPI.PositionSize.ClientArea.ToString()));
                    element6.Add(new XElement("ProcessingPositionAndSizeTwice", nowWPI.PositionSize.ProcessingPositionAndSizeTwice.ToString()));
                    element6.Add(new XElement("Active", nowWPI.Active.ToString()));
                    element5.Add(element6);
                }
                element4.Add(element5);
                element4.Add(new XElement("NumberOfTimesNotToProcessingFirst", nowItem.NumberOfTimesNotToProcessingFirst.ToString()));
                element5 = new("DoNotProcessingSizeList");
                foreach (System.Drawing.Size nowSize in nowItem.DoNotProcessingSize)
                {
                    element6 = new("Item");
                    element6.Add(new XElement("Width", nowSize.Width.ToString()));
                    element6.Add(new XElement("Height", nowSize.Height.ToString()));
                    element5.Add(element6);
                }
                element4.Add(element5);
                element5 = new("DoNotProcessingTitleNameList");
                foreach (string nowTitleName in nowItem.DoNotProcessingTitleName)
                {
                    element6 = new("Item");
                    element6.Add(new XElement("String", nowTitleName));
                    element5.Add(element6);
                }
                element4.Add(element5);
                element4.Add(new XElement("DifferentVersionVersion", nowItem.DifferentVersionVersion));
                element4.Add(new XElement("DifferentVersionAnnounce", nowItem.DifferentVersionAnnounce.ToString()));
                element3.Add(element4);
            }
            element2.Add(element3);
            element1.Add(element2);
            // End TimerInformation
            // Start MagnetInformation
            element2 = new("MagnetInformation");
            element2.Add(new XElement("Enabled", MagnetInformation.Enabled.ToString()));
            element2.Add(new XElement("PasteToScreenEdge", MagnetInformation.PasteToScreenEdge.ToString()));
            element2.Add(new XElement("PasteToAnotherWindow", MagnetInformation.PasteToAnotherWindow.ToString()));
            element2.Add(new XElement("PressTheKeyToPaste", MagnetInformation.PressTheKeyToPaste.ToString()));
            element2.Add(new XElement("StopTimeWhenPasted", MagnetInformation.StopTimeWhenPasted.ToString()));
            element2.Add(new XElement("DecisionDistanceToPaste", MagnetInformation.DecisionDistanceToPaste.ToString()));
            element2.Add(new XElement("StopProcessingFullScreen", MagnetInformation.StopProcessingFullScreen.ToString()));
            element1.Add(element2);
            // End MagnetInformation
            // Start HotkeyInformation
            element2 = new("HotkeyInformation");
            element2.Add(new XElement("Enabled", HotkeyInformation.Enabled.ToString()));
            element2.Add(new XElement("DoNotChangeOutOfScreen", HotkeyInformation.DoNotChangeOutOfScreen.ToString()));
            element2.Add(new XElement("StopProcessingFullScreen", HotkeyInformation.StopProcessingFullScreen.ToString()));
            element2.Add(new XElement("AddModifyWindowWidth", HotkeyInformation.AddModifyWindowSize.Width.ToString()));
            element2.Add(new XElement("AddModifyWindowHeight", HotkeyInformation.AddModifyWindowSize.Height.ToString()));
            element3 = new("ItemsList");
            foreach (HotkeyItemInformation nowItem in HotkeyInformation.Items)
            {
                element4 = new("Item");
                element4.Add(new XElement("RegisteredName", nowItem.RegisteredName));
                element4.Add(new XElement("ProcessingType", Enum.GetName(typeof(HotkeyProcessingType), nowItem.ProcessingType)));
                element4.Add(new XElement("Display", nowItem.PositionSize.Display));
                element4.Add(new XElement("X", nowItem.PositionSize.Position.X.ToString()));
                element4.Add(new XElement("Y", nowItem.PositionSize.Position.Y.ToString()));
                element4.Add(new XElement("Width", nowItem.PositionSize.Size.Width.ToString()));
                element4.Add(new XElement("Height", nowItem.PositionSize.Size.Height.ToString()));
                element4.Add(new XElement("XType", Enum.GetName(typeof(WindowXType), nowItem.PositionSize.XType)));
                element4.Add(new XElement("YType", Enum.GetName(typeof(WindowYType), nowItem.PositionSize.YType)));
                element4.Add(new XElement("WidthType", Enum.GetName(typeof(WindowSizeType), nowItem.PositionSize.WidthType)));
                element4.Add(new XElement("HeightType", Enum.GetName(typeof(WindowSizeType), nowItem.PositionSize.HeightType)));
                element4.Add(new XElement("XValueType", Enum.GetName(typeof(PositionSizeValueType), nowItem.PositionSize.XValueType)));
                element4.Add(new XElement("YValueType", Enum.GetName(typeof(PositionSizeValueType), nowItem.PositionSize.YValueType)));
                element4.Add(new XElement("WidthValueType", Enum.GetName(typeof(PositionSizeValueType), nowItem.PositionSize.WidthValueType)));
                element4.Add(new XElement("HeightValueType", Enum.GetName(typeof(PositionSizeValueType), nowItem.PositionSize.HeightValueType)));
                element4.Add(new XElement("ClientArea", nowItem.PositionSize.ClientArea.ToString()));
                element4.Add(new XElement("ProcessingPositionAndSizeTwice", nowItem.PositionSize.ProcessingPositionAndSizeTwice.ToString()));
                element4.Add(new XElement("StandardDisplay", Enum.GetName(typeof(StandardDisplay), nowItem.StandardDisplay)));
                element4.Add(new XElement("ProcessingValue", nowItem.ProcessingValue.ToString()));
                element4.Add(new XElement("HotkeyAlt", nowItem.Hotkey.Alt.ToString()));
                element4.Add(new XElement("HotkeyCtrl", nowItem.Hotkey.Ctrl.ToString()));
                element4.Add(new XElement("HotkeyShift", nowItem.Hotkey.Shift.ToString()));
                element4.Add(new XElement("HotkeyWindows", nowItem.Hotkey.Windows.ToString()));
                element4.Add(new XElement("HotkeyKeyCharacter", nowItem.Hotkey.KeyCharacter.ToString()));
                element3.Add(element4);
            }
            element2.Add(element3);
            element1.Add(element2);
            // End HotkeyInformation

            document.Add(element1);

            document.Save(path);
            result = true;
        }
        catch
        {
        }

        return result;
    }

    /// <summary>
    /// ファイルから読み込む
    /// </summary>
    /// <param name="path">ファイルのパス</param>
    /// <returns>結果 (失敗「false」/成功「true」)</returns>
    public bool ReadFile(
        string path
        )
    {
        bool result = false;        // 結果

        try
        {
            XDocument document = XDocument.Load(path);
            XElement? element = document.Element("Settings");

            if (element != null)
            {
                XElement? elementNode1;

                // Start MainWindow
                elementNode1 = element.Element("MainWindow");
                if (elementNode1 != null)
                {
                    MainWindowRectangle.X = Processing.GetIntXml(elementNode1, "X", MainWindowRectangle.X);
                    MainWindowRectangle.Y = Processing.GetIntXml(elementNode1, "Y", MainWindowRectangle.Y);
                    MainWindowRectangle.Width = Processing.GetIntXml(elementNode1, "Width", MainWindowRectangle.Width);
                    MainWindowRectangle.Height = Processing.GetIntXml(elementNode1, "Height", MainWindowRectangle.Height);
                    _ = Enum.TryParse(Processing.GetStringXml(elementNode1, "WindowState", Enum.GetName(typeof(WindowState), WindowStateMainWindow) ?? ""), out WindowStateMainWindow);
                }
                // End MainWindow
                Language = Processing.GetStringXml(element, "Language", Language);
                _ = Enum.TryParse(Processing.GetStringXml(element, "CoordinateType", Enum.GetName(typeof(CoordinateType), CoordinateType) ?? ""), out CoordinateType);
                AutomaticallyUpdateCheck = Processing.GetBoolXml(element, "AutomaticallyUpdateCheck", AutomaticallyUpdateCheck);
                CheckBetaVersion = Processing.GetBoolXml(element, "CheckBetaVersion", CheckBetaVersion);
                UseLongPath = Processing.GetBoolXml(element, "UseLongPath", UseLongPath);
                // Start ShiftPastePosition
                elementNode1 = element.Element("ShiftPastePosition");
                if (elementNode1 != null)
                {
                    ShiftPastePosition.Enabled = Processing.GetBoolXml(elementNode1, "Enabled", ShiftPastePosition.Enabled);
                    ShiftPastePosition.Left = Processing.GetIntXml(elementNode1, "Left", ShiftPastePosition.Left);
                    ShiftPastePosition.Top = Processing.GetIntXml(elementNode1, "Top", ShiftPastePosition.Top);
                    ShiftPastePosition.Right = Processing.GetIntXml(elementNode1, "Right", ShiftPastePosition.Right);
                    ShiftPastePosition.Bottom = Processing.GetIntXml(elementNode1, "Bottom", ShiftPastePosition.Bottom);
                }
                // End ShiftPastePosition
                // Start EventInformation
                elementNode1 = element.Element("EventInformation");
                if (elementNode1 != null)
                {
                    EventInformation.Enabled = Processing.GetBoolXml(elementNode1, "Enabled", EventInformation.Enabled);
                    EventInformation.HotkeysDoNotStopFullScreen = Processing.GetBoolXml(elementNode1, "HotkeysDoNotStopFullScreen", EventInformation.HotkeysDoNotStopFullScreen);
                    EventInformation.StopProcessingFullScreen = Processing.GetBoolXml(elementNode1, "StopProcessingFullScreen", EventInformation.StopProcessingFullScreen);
                    EventInformation.RegisterMultiple = Processing.GetBoolXml(elementNode1, "RegisterMultiple", EventInformation.RegisterMultiple);
                    EventInformation.CaseSensitiveWindowQueries = Processing.GetBoolXml(elementNode1, "CaseSensitiveWindowQueries", EventInformation.CaseSensitiveWindowQueries);
                    EventInformation.DoNotChangeOutOfScreen = Processing.GetBoolXml(elementNode1, "DoNotChangeOutOfScreen", EventInformation.DoNotChangeOutOfScreen);
                    EventInformation.StopProcessingShowAddModifyWindow = Processing.GetBoolXml(elementNode1, "StopProcessingShowAddModifyWindow", EventInformation.StopProcessingShowAddModifyWindow);
                    EventInformation.AddModifyWindowSize.Width = Processing.GetIntXml(elementNode1, "AddModifyWindowWidth", EventInformation.AddModifyWindowSize.Width);
                    EventInformation.AddModifyWindowSize.Height = Processing.GetIntXml(elementNode1, "AddModifyWindowHeight", EventInformation.AddModifyWindowSize.Height);
                    EventInformation.AcquiredItems.TitleName = Processing.GetBoolXml(elementNode1, "AcquiredItemsTitleName", EventInformation.AcquiredItems.TitleName);
                    EventInformation.AcquiredItems.ClassName = Processing.GetBoolXml(elementNode1, "AcquiredItemsClassName", EventInformation.AcquiredItems.ClassName);
                    EventInformation.AcquiredItems.FileName = Processing.GetBoolXml(elementNode1, "AcquiredItemsFileName", EventInformation.AcquiredItems.FileName);
                    EventInformation.AcquiredItems.WindowState = Processing.GetBoolXml(elementNode1, "AcquiredItemsWindowState", EventInformation.AcquiredItems.WindowState);
                    EventInformation.AcquiredItems.X = Processing.GetBoolXml(elementNode1, "AcquiredItemsX", EventInformation.AcquiredItems.X);
                    EventInformation.AcquiredItems.Y = Processing.GetBoolXml(elementNode1, "AcquiredItemsY", EventInformation.AcquiredItems.Y);
                    EventInformation.AcquiredItems.Width = Processing.GetBoolXml(elementNode1, "AcquiredItemsWidth", EventInformation.AcquiredItems.Width);
                    EventInformation.AcquiredItems.Height = Processing.GetBoolXml(elementNode1, "AcquiredItemsHeight", EventInformation.AcquiredItems.Height);
                    EventInformation.AcquiredItems.Display = Processing.GetBoolXml(elementNode1, "AcquiredItemsDisplay", EventInformation.AcquiredItems.Display);
                    EventInformation.EventTypeInformation.Foreground = Processing.GetBoolXml(elementNode1, "EventTypeForeground", EventInformation.EventTypeInformation.Foreground);
                    EventInformation.EventTypeInformation.MoveSizeEnd = Processing.GetBoolXml(elementNode1, "EventTypeMoveSizeEnd", EventInformation.EventTypeInformation.MoveSizeEnd);
                    EventInformation.EventTypeInformation.MinimizeStart = Processing.GetBoolXml(elementNode1, "EventTypeMinimizeStart", EventInformation.EventTypeInformation.MinimizeStart);
                    EventInformation.EventTypeInformation.MinimizeEnd = Processing.GetBoolXml(elementNode1, "EventTypeMinimizeEnd", EventInformation.EventTypeInformation.MinimizeEnd);
                    EventInformation.EventTypeInformation.Create = Processing.GetBoolXml(elementNode1, "EventTypeCreate", EventInformation.EventTypeInformation.Create);
                    EventInformation.EventTypeInformation.Show = Processing.GetBoolXml(elementNode1, "EventTypeShow", EventInformation.EventTypeInformation.Show);
                    EventInformation.EventTypeInformation.NameChange = Processing.GetBoolXml(elementNode1, "EventTypeNameChange", EventInformation.EventTypeInformation.NameChange);
                    XElement? elementNode2 = elementNode1.Element("ItemsList");
                    if (elementNode2 != null)
                    {
                        // EventItemInformation
                        foreach (XElement nowElementEII in elementNode2.Elements("Item"))
                        {
                            EventItemInformation newEII = new();

                            newEII.Enabled = Processing.GetBoolXml(nowElementEII, "Enabled", newEII.Enabled);
                            newEII.RegisteredName = Processing.GetStringXml(nowElementEII, "RegisteredName", newEII.RegisteredName);
                            newEII.TitleName = Processing.GetStringXml(nowElementEII, "TitleName", newEII.TitleName);
                            _ = Enum.TryParse(Processing.GetStringXml(nowElementEII, "TitleNameMatchCondition", Enum.GetName(typeof(NameMatchCondition), newEII.TitleNameMatchCondition) ?? ""), out newEII.TitleNameMatchCondition);
                            newEII.ClassName = Processing.GetStringXml(nowElementEII, "ClassName", newEII.ClassName);
                            _ = Enum.TryParse(Processing.GetStringXml(nowElementEII, "ClassNameMatchCondition", Enum.GetName(typeof(NameMatchCondition), newEII.ClassNameMatchCondition) ?? ""), out newEII.ClassNameMatchCondition);
                            newEII.FileName = Processing.GetStringXml(nowElementEII, "FileName", newEII.FileName);
                            _ = Enum.TryParse(Processing.GetStringXml(nowElementEII, "FileNameMatchCondition", Enum.GetName(typeof(FileNameMatchCondition), newEII.FileNameMatchCondition) ?? ""), out newEII.FileNameMatchCondition);
                            _ = Enum.TryParse(Processing.GetStringXml(nowElementEII, "ProcessingOnlyOnce", Enum.GetName(typeof(ProcessingOnlyOnce), newEII.ProcessingOnlyOnce) ?? ""), out newEII.ProcessingOnlyOnce);
                            newEII.CloseWindow = Processing.GetBoolXml(nowElementEII, "CloseWindow", newEII.CloseWindow);
                            _ = Enum.TryParse(Processing.GetStringXml(nowElementEII, "StandardDisplay", Enum.GetName(typeof(StandardDisplay), newEII.StandardDisplay) ?? ""), out newEII.StandardDisplay);
                            _ = Enum.TryParse(Processing.GetStringXml(nowElementEII, "TitleProcessingConditions", Enum.GetName(typeof(TitleProcessingConditions), newEII.TitleProcessingConditions) ?? ""), out newEII.TitleProcessingConditions);
                            // WindowProcessingInformation
                            XElement? elementNode3 = nowElementEII.Element("ItemsList");
                            if (elementNode3 != null)
                            {
                                foreach (XElement nowElement in elementNode3.Elements("Item"))
                                {
                                    WindowProcessingInformation newWPI = new();

                                    newWPI.ProcessingName = Processing.GetStringXml(nowElement, "ProcessingName", newWPI.ProcessingName);
                                    _ = Enum.TryParse(Processing.GetStringXml(nowElement, "Forefront", Enum.GetName(typeof(Forefront), newWPI.Forefront) ?? ""), out newWPI.Forefront);
                                    newWPI.EnabledTransparency = Processing.GetBoolXml(nowElement, "EnabledTransparency", newWPI.EnabledTransparency);
                                    newWPI.Transparency = Processing.GetIntXml(nowElement, "Transparency", newWPI.Transparency);
                                    newWPI.Hotkey.Alt = Processing.GetBoolXml(nowElement, "HotkeyAlt", newWPI.Hotkey.Alt);
                                    newWPI.Hotkey.Ctrl = Processing.GetBoolXml(nowElement, "HotkeyCtrl", newWPI.Hotkey.Ctrl);
                                    newWPI.Hotkey.Shift = Processing.GetBoolXml(nowElement, "HotkeyShift", newWPI.Hotkey.Shift);
                                    newWPI.Hotkey.Windows = Processing.GetBoolXml(nowElement, "HotkeyWindows", newWPI.Hotkey.Windows);
                                    newWPI.Hotkey.KeyCharacter = Processing.GetIntXml(nowElement, "HotkeyKeyCharacter", newWPI.Hotkey.KeyCharacter);
                                    _ = Enum.TryParse(Processing.GetStringXml(nowElement, "WindowState", Enum.GetName(typeof(SettingsWindowState), newWPI.SettingsWindowState) ?? ""), out newWPI.SettingsWindowState);
                                    newWPI.OnlyNormalWindow = Processing.GetBoolXml(nowElement, "OnlyNormalWindow", newWPI.OnlyNormalWindow);
                                    newWPI.PositionSize.Display = Processing.GetStringXml(nowElement, "Display", newWPI.PositionSize.Display);
                                    newWPI.PositionSize.Position.X = Processing.GetDecimalXml(nowElement, "X", newWPI.PositionSize.Position.X);
                                    newWPI.PositionSize.Position.Y = Processing.GetDecimalXml(nowElement, "Y", newWPI.PositionSize.Position.Y);
                                    newWPI.PositionSize.Size.Width = Processing.GetDecimalXml(nowElement, "Width", newWPI.PositionSize.Size.Width);
                                    newWPI.PositionSize.Size.Height = Processing.GetDecimalXml(nowElement, "Height", newWPI.PositionSize.Size.Height);
                                    _ = Enum.TryParse(Processing.GetStringXml(nowElement, "XType", Enum.GetName(typeof(WindowXType), newWPI.PositionSize.XType) ?? ""), out newWPI.PositionSize.XType);
                                    _ = Enum.TryParse(Processing.GetStringXml(nowElement, "YType", Enum.GetName(typeof(WindowYType), newWPI.PositionSize.YType) ?? ""), out newWPI.PositionSize.YType);
                                    _ = Enum.TryParse(Processing.GetStringXml(nowElement, "WidthType", Enum.GetName(typeof(WindowSizeType), newWPI.PositionSize.WidthType) ?? ""), out newWPI.PositionSize.WidthType);
                                    _ = Enum.TryParse(Processing.GetStringXml(nowElement, "HeightType", Enum.GetName(typeof(WindowSizeType), newWPI.PositionSize.HeightType) ?? ""), out newWPI.PositionSize.HeightType);
                                    _ = Enum.TryParse(Processing.GetStringXml(nowElement, "XValueType", Enum.GetName(typeof(PositionSizeValueType), newWPI.PositionSize.XValueType) ?? ""), out newWPI.PositionSize.XValueType);
                                    _ = Enum.TryParse(Processing.GetStringXml(nowElement, "YValueType", Enum.GetName(typeof(PositionSizeValueType), newWPI.PositionSize.YValueType) ?? ""), out newWPI.PositionSize.YValueType);
                                    _ = Enum.TryParse(Processing.GetStringXml(nowElement, "WidthValueType", Enum.GetName(typeof(PositionSizeValueType), newWPI.PositionSize.WidthValueType) ?? ""), out newWPI.PositionSize.WidthValueType);
                                    _ = Enum.TryParse(Processing.GetStringXml(nowElement, "HeightValueType", Enum.GetName(typeof(PositionSizeValueType), newWPI.PositionSize.HeightValueType) ?? ""), out newWPI.PositionSize.HeightValueType);
                                    newWPI.PositionSize.ClientArea = Processing.GetBoolXml(nowElement, "ClientArea", newWPI.PositionSize.ClientArea);
                                    newWPI.PositionSize.ProcessingPositionAndSizeTwice = Processing.GetBoolXml(nowElement, "ProcessingPositionAndSizeTwice", newWPI.PositionSize.ProcessingPositionAndSizeTwice);
                                    newWPI.Active = Processing.GetBoolXml(nowElement, "Active", newWPI.Active);

                                    newEII.WindowProcessingInformation.Add(newWPI);
                                }
                            }
                            // DoNotProcessingSize
                            elementNode3 = nowElementEII.Element("DoNotProcessingSizeList");
                            if (elementNode3 != null)
                            {
                                foreach (XElement nowElement in elementNode3.Elements("Item"))
                                {
                                    System.Drawing.Size newSize = new();
                                    newSize.Width = Processing.GetIntXml(nowElement, "Width", newSize.Width);
                                    newSize.Height = Processing.GetIntXml(nowElement, "Height", newSize.Height);
                                    newEII.DoNotProcessingSize.Add(newSize);
                                }
                            }
                            // DoNotProcessingTitleName
                            elementNode3 = nowElementEII.Element("DoNotProcessingTitleNameList");
                            if (elementNode3 != null)
                            {
                                foreach (XElement nowElement in elementNode3.Elements("Item"))
                                {
                                    string newString = "";
                                    newString = Processing.GetStringXml(nowElement, "String", newString);
                                    newEII.DoNotProcessingTitleName.Add(newString);
                                }
                            }
                            newEII.DifferentVersionVersion = Processing.GetStringXml(nowElementEII, "DifferentVersionVersion", newEII.DifferentVersionVersion);
                            newEII.DifferentVersionAnnounce = Processing.GetBoolXml(nowElementEII, "DifferentVersionAnnounce", newEII.DifferentVersionAnnounce);
                            newEII.WindowEventData.Foreground = Processing.GetBoolXml(nowElementEII, "Foreground", newEII.WindowEventData.Foreground);
                            newEII.WindowEventData.MoveSizeEnd = Processing.GetBoolXml(nowElementEII, "MoveSizeEnd", newEII.WindowEventData.MoveSizeEnd);
                            newEII.WindowEventData.MinimizeStart = Processing.GetBoolXml(nowElementEII, "MinimizeStart", newEII.WindowEventData.MinimizeStart);
                            newEII.WindowEventData.MinimizeEnd = Processing.GetBoolXml(nowElementEII, "MinimizeEnd", newEII.WindowEventData.MinimizeEnd);
                            newEII.WindowEventData.Create = Processing.GetBoolXml(nowElementEII, "Create", newEII.WindowEventData.Create);
                            newEII.WindowEventData.Show = Processing.GetBoolXml(nowElementEII, "Show", newEII.WindowEventData.Show);
                            newEII.WindowEventData.NameChange = Processing.GetBoolXml(nowElementEII, "NameChange", newEII.WindowEventData.NameChange);

                            EventInformation.Items.Add(newEII);
                        }
                    }
                }
                // End EventInformation
                // Start TimerInformation
                elementNode1 = element.Element("TimerInformation");
                if (elementNode1 != null)
                {
                    TimerInformation.Enabled = Processing.GetBoolXml(elementNode1, "Enabled", TimerInformation.Enabled);
                    TimerInformation.HotkeysDoNotStopFullScreen = Processing.GetBoolXml(elementNode1, "HotkeysDoNotStopFullScreen", TimerInformation.HotkeysDoNotStopFullScreen);
                    TimerInformation.StopProcessingFullScreen = Processing.GetBoolXml(elementNode1, "StopProcessingFullScreen", TimerInformation.StopProcessingFullScreen);
                    TimerInformation.RegisterMultiple = Processing.GetBoolXml(elementNode1, "RegisterMultiple", TimerInformation.RegisterMultiple);
                    TimerInformation.CaseSensitiveWindowQueries = Processing.GetBoolXml(elementNode1, "CaseSensitiveWindowQueries", TimerInformation.CaseSensitiveWindowQueries);
                    TimerInformation.DoNotChangeOutOfScreen = Processing.GetBoolXml(elementNode1, "DoNotChangeOutOfScreen", TimerInformation.DoNotChangeOutOfScreen);
                    TimerInformation.StopProcessingShowAddModifyWindow = Processing.GetBoolXml(elementNode1, "StopProcessingShowAddModifyWindow", TimerInformation.StopProcessingShowAddModifyWindow);
                    TimerInformation.AddModifyWindowSize.Width = Processing.GetIntXml(elementNode1, "AddModifyWindowWidth", TimerInformation.AddModifyWindowSize.Width);
                    TimerInformation.AddModifyWindowSize.Height = Processing.GetIntXml(elementNode1, "AddModifyWindowHeight", TimerInformation.AddModifyWindowSize.Height);
                    TimerInformation.AcquiredItems.TitleName = Processing.GetBoolXml(elementNode1, "AcquiredItemsTitleName", TimerInformation.AcquiredItems.TitleName);
                    TimerInformation.AcquiredItems.ClassName = Processing.GetBoolXml(elementNode1, "AcquiredItemsClassName", TimerInformation.AcquiredItems.ClassName);
                    TimerInformation.AcquiredItems.FileName = Processing.GetBoolXml(elementNode1, "AcquiredItemsFileName", TimerInformation.AcquiredItems.FileName);
                    TimerInformation.AcquiredItems.WindowState = Processing.GetBoolXml(elementNode1, "AcquiredItemsWindowState", TimerInformation.AcquiredItems.WindowState);
                    TimerInformation.AcquiredItems.X = Processing.GetBoolXml(elementNode1, "AcquiredItemsX", TimerInformation.AcquiredItems.X);
                    TimerInformation.AcquiredItems.Y = Processing.GetBoolXml(elementNode1, "AcquiredItemsY", TimerInformation.AcquiredItems.Y);
                    TimerInformation.AcquiredItems.Width = Processing.GetBoolXml(elementNode1, "AcquiredItemsWidth", TimerInformation.AcquiredItems.Width);
                    TimerInformation.AcquiredItems.Height = Processing.GetBoolXml(elementNode1, "AcquiredItemsHeight", TimerInformation.AcquiredItems.Height);
                    TimerInformation.AcquiredItems.Display = Processing.GetBoolXml(elementNode1, "AcquiredItemsDisplay", TimerInformation.AcquiredItems.Display);
                    _ = Enum.TryParse(Processing.GetStringXml(elementNode1, "ProcessingWindowRange", Enum.GetName(typeof(ProcessingWindowRange), TimerInformation.ProcessingWindowRange) ?? ""), out TimerInformation.ProcessingWindowRange);
                    TimerInformation.ProcessingInterval = Processing.GetIntXml(elementNode1, "ProcessingInterval", TimerInformation.ProcessingInterval);
                    TimerInformation.WaitTimeToProcessingNextWindow = Processing.GetIntXml(elementNode1, "WaitTimeToProcessingNextWindow", TimerInformation.WaitTimeToProcessingNextWindow);
                    XElement? elementNode2 = elementNode1.Element("ItemsList");
                    if (elementNode2 != null)
                    {
                        // TimerItemInformation
                        foreach (XElement nowElementTII in elementNode2.Elements("Item"))
                        {
                            TimerItemInformation newTII = new();

                            newTII.Enabled = Processing.GetBoolXml(nowElementTII, "Enabled", newTII.Enabled);
                            newTII.RegisteredName = Processing.GetStringXml(nowElementTII, "RegisteredName", newTII.RegisteredName);
                            newTII.TitleName = Processing.GetStringXml(nowElementTII, "TitleName", newTII.TitleName);
                            _ = Enum.TryParse(Processing.GetStringXml(nowElementTII, "TitleNameMatchCondition", Enum.GetName(typeof(NameMatchCondition), newTII.TitleNameMatchCondition) ?? ""), out newTII.TitleNameMatchCondition);
                            newTII.ClassName = Processing.GetStringXml(nowElementTII, "ClassName", newTII.ClassName);
                            _ = Enum.TryParse(Processing.GetStringXml(nowElementTII, "ClassNameMatchCondition", Enum.GetName(typeof(NameMatchCondition), newTII.ClassNameMatchCondition) ?? ""), out newTII.ClassNameMatchCondition);
                            newTII.FileName = Processing.GetStringXml(nowElementTII, "FileName", newTII.FileName);
                            _ = Enum.TryParse(Processing.GetStringXml(nowElementTII, "FileNameMatchCondition", Enum.GetName(typeof(FileNameMatchCondition), newTII.FileNameMatchCondition) ?? ""), out newTII.FileNameMatchCondition);
                            _ = Enum.TryParse(Processing.GetStringXml(nowElementTII, "ProcessingOnlyOnce", Enum.GetName(typeof(ProcessingOnlyOnce), newTII.ProcessingOnlyOnce) ?? ""), out newTII.ProcessingOnlyOnce);
                            newTII.CloseWindow = Processing.GetBoolXml(nowElementTII, "CloseWindow", newTII.CloseWindow);
                            _ = Enum.TryParse(Processing.GetStringXml(nowElementTII, "StandardDisplay", Enum.GetName(typeof(StandardDisplay), newTII.StandardDisplay) ?? ""), out newTII.StandardDisplay);
                            _ = Enum.TryParse(Processing.GetStringXml(nowElementTII, "TitleProcessingConditions", Enum.GetName(typeof(TitleProcessingConditions), newTII.TitleProcessingConditions) ?? ""), out newTII.TitleProcessingConditions);
                            // WindowProcessingInformation
                            XElement? elementNode3 = nowElementTII.Element("ItemsList");
                            if (elementNode3 != null)
                            {
                                foreach (XElement nowElement in elementNode3.Elements("Item"))
                                {
                                    WindowProcessingInformation newWPI = new();

                                    newWPI.ProcessingName = Processing.GetStringXml(nowElement, "ProcessingName", newWPI.ProcessingName);
                                    _ = Enum.TryParse(Processing.GetStringXml(nowElement, "Forefront", Enum.GetName(typeof(Forefront), newWPI.Forefront) ?? ""), out newWPI.Forefront);
                                    newWPI.EnabledTransparency = Processing.GetBoolXml(nowElement, "EnabledTransparency", newWPI.EnabledTransparency);
                                    newWPI.Transparency = Processing.GetIntXml(nowElement, "Transparency", newWPI.Transparency);
                                    newWPI.Hotkey.Alt = Processing.GetBoolXml(nowElement, "HotkeyAlt", newWPI.Hotkey.Alt);
                                    newWPI.Hotkey.Ctrl = Processing.GetBoolXml(nowElement, "HotkeyCtrl", newWPI.Hotkey.Ctrl);
                                    newWPI.Hotkey.Shift = Processing.GetBoolXml(nowElement, "HotkeyShift", newWPI.Hotkey.Shift);
                                    newWPI.Hotkey.Windows = Processing.GetBoolXml(nowElement, "HotkeyWindows", newWPI.Hotkey.Windows);
                                    newWPI.Hotkey.KeyCharacter = Processing.GetIntXml(nowElement, "HotkeyKeyCharacter", newWPI.Hotkey.KeyCharacter);
                                    _ = Enum.TryParse(Processing.GetStringXml(nowElement, "WindowState", Enum.GetName(typeof(SettingsWindowState), newWPI.SettingsWindowState) ?? ""), out newWPI.SettingsWindowState);
                                    newWPI.OnlyNormalWindow = Processing.GetBoolXml(nowElement, "OnlyNormalWindow", newWPI.OnlyNormalWindow);
                                    newWPI.PositionSize.Display = Processing.GetStringXml(nowElement, "Display", newWPI.PositionSize.Display);
                                    newWPI.PositionSize.Position.X = Processing.GetDecimalXml(nowElement, "X", newWPI.PositionSize.Position.X);
                                    newWPI.PositionSize.Position.Y = Processing.GetDecimalXml(nowElement, "Y", newWPI.PositionSize.Position.Y);
                                    newWPI.PositionSize.Size.Width = Processing.GetDecimalXml(nowElement, "Width", newWPI.PositionSize.Size.Width);
                                    newWPI.PositionSize.Size.Height = Processing.GetDecimalXml(nowElement, "Height", newWPI.PositionSize.Size.Height);
                                    _ = Enum.TryParse(Processing.GetStringXml(nowElement, "XType", Enum.GetName(typeof(WindowXType), newWPI.PositionSize.XType) ?? ""), out newWPI.PositionSize.XType);
                                    _ = Enum.TryParse(Processing.GetStringXml(nowElement, "YType", Enum.GetName(typeof(WindowYType), newWPI.PositionSize.YType) ?? ""), out newWPI.PositionSize.YType);
                                    _ = Enum.TryParse(Processing.GetStringXml(nowElement, "WidthType", Enum.GetName(typeof(WindowSizeType), newWPI.PositionSize.WidthType) ?? ""), out newWPI.PositionSize.WidthType);
                                    _ = Enum.TryParse(Processing.GetStringXml(nowElement, "HeightType", Enum.GetName(typeof(WindowSizeType), newWPI.PositionSize.HeightType) ?? ""), out newWPI.PositionSize.HeightType);
                                    _ = Enum.TryParse(Processing.GetStringXml(nowElement, "XValueType", Enum.GetName(typeof(PositionSizeValueType), newWPI.PositionSize.XValueType) ?? ""), out newWPI.PositionSize.XValueType);
                                    _ = Enum.TryParse(Processing.GetStringXml(nowElement, "YValueType", Enum.GetName(typeof(PositionSizeValueType), newWPI.PositionSize.YValueType) ?? ""), out newWPI.PositionSize.YValueType);
                                    _ = Enum.TryParse(Processing.GetStringXml(nowElement, "WidthValueType", Enum.GetName(typeof(PositionSizeValueType), newWPI.PositionSize.WidthValueType) ?? ""), out newWPI.PositionSize.WidthValueType);
                                    _ = Enum.TryParse(Processing.GetStringXml(nowElement, "HeightValueType", Enum.GetName(typeof(PositionSizeValueType), newWPI.PositionSize.HeightValueType) ?? ""), out newWPI.PositionSize.HeightValueType);
                                    newWPI.PositionSize.ClientArea = Processing.GetBoolXml(nowElement, "ClientArea", newWPI.PositionSize.ClientArea);
                                    newWPI.PositionSize.ProcessingPositionAndSizeTwice = Processing.GetBoolXml(nowElement, "ProcessingPositionAndSizeTwice", newWPI.PositionSize.ProcessingPositionAndSizeTwice);
                                    newWPI.Active = Processing.GetBoolXml(nowElement, "Active", newWPI.Active);

                                    newTII.WindowProcessingInformation.Add(newWPI);
                                }
                            }
                            newTII.NumberOfTimesNotToProcessingFirst = Processing.GetIntXml(nowElementTII, "NumberOfTimesNotToProcessingFirst", newTII.NumberOfTimesNotToProcessingFirst);
                            // DoNotProcessingSize
                            elementNode3 = nowElementTII.Element("DoNotProcessingSizeList");
                            if (elementNode3 != null)
                            {
                                foreach (XElement nowElement in elementNode3.Elements("Item"))
                                {
                                    System.Drawing.Size newSize = new();
                                    newSize.Width = Processing.GetIntXml(nowElement, "Width", newSize.Width);
                                    newSize.Height = Processing.GetIntXml(nowElement, "Height", newSize.Height);
                                    newTII.DoNotProcessingSize.Add(newSize);
                                }
                            }
                            // DoNotProcessingTitleName
                            elementNode3 = nowElementTII.Element("DoNotProcessingTitleNameList");
                            if (elementNode3 != null)
                            {
                                foreach (XElement nowElement in elementNode3.Elements("Item"))
                                {
                                    string newString = "";
                                    newString = Processing.GetStringXml(nowElement, "String", newString);
                                    newTII.DoNotProcessingTitleName.Add(newString);
                                }
                            }
                            newTII.DifferentVersionVersion = Processing.GetStringXml(nowElementTII, "DifferentVersionVersion", newTII.DifferentVersionVersion);
                            newTII.DifferentVersionAnnounce = Processing.GetBoolXml(nowElementTII, "DifferentVersionAnnounce", newTII.DifferentVersionAnnounce);

                            TimerInformation.Items.Add(newTII);
                        }
                    }
                }
                // End TimerInformation
                // Start MagnetInformation
                elementNode1 = element.Element("MagnetInformation");
                if (elementNode1 != null)
                {
                    MagnetInformation.Enabled = Processing.GetBoolXml(elementNode1, "Enabled", MagnetInformation.Enabled);
                    MagnetInformation.PasteToScreenEdge = Processing.GetBoolXml(elementNode1, "PasteToScreenEdge", MagnetInformation.PasteToScreenEdge);
                    MagnetInformation.PasteToAnotherWindow = Processing.GetBoolXml(elementNode1, "PasteToAnotherWindow", MagnetInformation.PasteToAnotherWindow);
                    MagnetInformation.PressTheKeyToPaste = Processing.GetBoolXml(elementNode1, "PressTheKeyToPaste", MagnetInformation.PressTheKeyToPaste);
                    MagnetInformation.StopTimeWhenPasted = Processing.GetIntXml(elementNode1, "StopTimeWhenPasted", MagnetInformation.StopTimeWhenPasted);
                    MagnetInformation.DecisionDistanceToPaste = Processing.GetIntXml(elementNode1, "DecisionDistanceToPaste", MagnetInformation.DecisionDistanceToPaste);
                    MagnetInformation.StopProcessingFullScreen = Processing.GetBoolXml(elementNode1, "StopProcessingFullScreen", MagnetInformation.StopProcessingFullScreen);
                }
                // End MagnetInformation
                // Start HotkeyInformation
                elementNode1 = element.Element("HotkeyInformation");
                if (elementNode1 != null)
                {
                    HotkeyInformation.Enabled = Processing.GetBoolXml(elementNode1, "Enabled", HotkeyInformation.Enabled);
                    HotkeyInformation.DoNotChangeOutOfScreen = Processing.GetBoolXml(elementNode1, "DoNotChangeOutOfScreen", HotkeyInformation.DoNotChangeOutOfScreen);
                    HotkeyInformation.StopProcessingFullScreen = Processing.GetBoolXml(elementNode1, "StopProcessingFullScreen", HotkeyInformation.StopProcessingFullScreen);
                    HotkeyInformation.AddModifyWindowSize.Width = Processing.GetIntXml(elementNode1, "AddModifyWindowWidth", HotkeyInformation.AddModifyWindowSize.Width);
                    HotkeyInformation.AddModifyWindowSize.Height = Processing.GetIntXml(elementNode1, "AddModifyWindowHeight", HotkeyInformation.AddModifyWindowSize.Height);
                    // HotkeyItemInformation
                    XElement? elementNode2 = elementNode1.Element("ItemsList");
                    if (elementNode2 != null)
                    {
                        foreach (XElement nowElementHII in elementNode2.Elements("Item"))
                        {
                            HotkeyItemInformation newHII = new();

                            newHII.RegisteredName = Processing.GetStringXml(nowElementHII, "RegisteredName", newHII.RegisteredName);
                            _ = Enum.TryParse(Processing.GetStringXml(nowElementHII, "ProcessingType", System.Enum.GetName(typeof(HotkeyProcessingType), newHII.ProcessingType) ?? ""), out newHII.ProcessingType);
                            newHII.PositionSize.Display = Processing.GetStringXml(nowElementHII, "Display", newHII.PositionSize.Display);
                            newHII.PositionSize.Position.X = Processing.GetDecimalXml(nowElementHII, "X", newHII.PositionSize.Position.X);
                            newHII.PositionSize.Position.Y = Processing.GetDecimalXml(nowElementHII, "Y", newHII.PositionSize.Position.Y);
                            newHII.PositionSize.Size.Width = Processing.GetDecimalXml(nowElementHII, "Width", newHII.PositionSize.Size.Width);
                            newHII.PositionSize.Size.Height = Processing.GetDecimalXml(nowElementHII, "Height", newHII.PositionSize.Size.Height);
                            _ = Enum.TryParse(Processing.GetStringXml(nowElementHII, "XType", Enum.GetName(typeof(WindowXType), newHII.PositionSize.XType) ?? ""), out newHII.PositionSize.XType);
                            _ = Enum.TryParse(Processing.GetStringXml(nowElementHII, "YType", Enum.GetName(typeof(WindowYType), newHII.PositionSize.YType) ?? ""), out newHII.PositionSize.YType);
                            _ = Enum.TryParse(Processing.GetStringXml(nowElementHII, "WidthType", Enum.GetName(typeof(WindowSizeType), newHII.PositionSize.WidthType) ?? ""), out newHII.PositionSize.WidthType);
                            _ = Enum.TryParse(Processing.GetStringXml(nowElementHII, "HeightType", Enum.GetName(typeof(WindowSizeType), newHII.PositionSize.HeightType) ?? ""), out newHII.PositionSize.HeightType);
                            _ = Enum.TryParse(Processing.GetStringXml(nowElementHII, "XValueType", Enum.GetName(typeof(PositionSizeValueType), newHII.PositionSize.XValueType) ?? ""), out newHII.PositionSize.XValueType);
                            _ = Enum.TryParse(Processing.GetStringXml(nowElementHII, "YValueType", Enum.GetName(typeof(PositionSizeValueType), newHII.PositionSize.YValueType) ?? ""), out newHII.PositionSize.YValueType);
                            _ = Enum.TryParse(Processing.GetStringXml(nowElementHII, "WidthValueType", Enum.GetName(typeof(PositionSizeValueType), newHII.PositionSize.WidthValueType) ?? ""), out newHII.PositionSize.WidthValueType);
                            _ = Enum.TryParse(Processing.GetStringXml(nowElementHII, "HeightValueType", Enum.GetName(typeof(PositionSizeValueType), newHII.PositionSize.HeightValueType) ?? ""), out newHII.PositionSize.HeightValueType);
                            newHII.PositionSize.ClientArea = Processing.GetBoolXml(nowElementHII, "ClientArea", newHII.PositionSize.ClientArea);
                            newHII.PositionSize.ProcessingPositionAndSizeTwice = Processing.GetBoolXml(nowElementHII, "ProcessingPositionAndSizeTwice", newHII.PositionSize.ProcessingPositionAndSizeTwice);
                            _ = Enum.TryParse(Processing.GetStringXml(nowElementHII, "StandardDisplay", Enum.GetName(typeof(StandardDisplay), newHII.StandardDisplay) ?? ""), out newHII.StandardDisplay);
                            newHII.ProcessingValue = Processing.GetIntXml(nowElementHII, "ProcessingValue", newHII.ProcessingValue);
                            newHII.Hotkey.Alt = Processing.GetBoolXml(nowElementHII, "HotkeyAlt", newHII.Hotkey.Alt);
                            newHII.Hotkey.Ctrl = Processing.GetBoolXml(nowElementHII, "HotkeyCtrl", newHII.Hotkey.Ctrl);
                            newHII.Hotkey.Shift = Processing.GetBoolXml(nowElementHII, "HotkeyShift", newHII.Hotkey.Shift);
                            newHII.Hotkey.Windows = Processing.GetBoolXml(nowElementHII, "HotkeyWindows", newHII.Hotkey.Windows);
                            newHII.Hotkey.KeyCharacter = Processing.GetIntXml(nowElementHII, "HotkeyKeyCharacter", newHII.Hotkey.KeyCharacter);

                            HotkeyInformation.Items.Add(newHII);
                        }
                    }
                }
                // End HotkeyInformation

                result = true;
            }
        }
        catch
        {
        }

        return result;
    }
}
