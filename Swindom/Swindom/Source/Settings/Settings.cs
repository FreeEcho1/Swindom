namespace Swindom.Source.Settings
{
    /// <summary>
    /// 設定
    /// </summary>
    public class Settings
    {
        /// <summary>
        /// メインウィンドウの位置とサイズ
        /// </summary>
        public System.Drawing.Rectangle MainWindowRectangle = new(0, 0, 600, 400);
        /// <summary>
        /// メインウィンドウのウィンドウ状態 (通常のウィンドウ又は最大化のみ)
        /// </summary>
        public System.Windows.WindowState WindowStateOfTheMainWindow = System.Windows.WindowState.Normal;
        /// <summary>
        /// 言語
        /// </summary>
        public string Language = "";
        /// <summary>
        /// 座標指定の種類
        /// </summary>
        public CoordinateType CoordinateType = CoordinateType.Display;
        /// <summary>
        /// 実行時に自動で更新確認 (無効「false」/有効「true」)
        /// </summary>
        public bool AutomaticallyUpdateCheck = false;
        /// <summary>
        /// ベータバージョンを含めて更新確認 (無効「false」/有効「true」)
        /// </summary>
        public bool CheckBetaVersion = false;
        /// <summary>
        /// 「貼り付ける位置をずらす」情報
        /// </summary>
        public ShiftPastePosition ShiftPastePosition = new();

        /// <summary>
        /// 「イベント」機能の設定情報
        /// </summary>
        public EventInformation EventInformation = new();
        /// <summary>
        /// 「タイマー」機能の設定情報
        /// </summary>
        public TimerInformation TimerInformation = new();
        /// <summary>
        /// 「マグネット」機能の設定情報
        /// </summary>
        public MagnetInformation MagnetInformation = new();
        /// <summary>
        /// 「ホットキー」機能の設定情報
        /// </summary>
        public HotkeyInformation HotkeyInformation = new();
        /// <summary>
        /// 「ウィンドウを画面外から画面内に移動」機能の設定情報
        /// </summary>
        public OutsideToInsideInformation OutsideToInsideInformation = new();
        /// <summary>
        /// 「ウィンドウを画面の中央に移動」機能の設定情報
        /// </summary>
        public MoveCenterInformation MoveCenterInformation = new();

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
                System.Xml.Linq.XDocument document = new();
                System.Xml.Linq.XElement element1, element2, element3, element4, element5, element6;

                element1 = new("Settings");
                element1.Add(new System.Xml.Linq.XElement("SettingVersion", "1"));
                element1.Add(new System.Xml.Linq.XElement("Language", Language));
                element1.Add(new System.Xml.Linq.XElement("CoordinateType", System.Enum.GetName(typeof(CoordinateType), CoordinateType)));
                element1.Add(new System.Xml.Linq.XElement("AutomaticallyUpdateCheck", AutomaticallyUpdateCheck.ToString()));
                element1.Add(new System.Xml.Linq.XElement("CheckBetaVersion", CheckBetaVersion.ToString()));
                // Start ShiftPastePosition
                element2 = new("ShiftPastePosition");
                element2.Add(new System.Xml.Linq.XElement("Enabled", ShiftPastePosition.Enabled.ToString()));
                element2.Add(new System.Xml.Linq.XElement("Left", ShiftPastePosition.Left.ToString()));
                element2.Add(new System.Xml.Linq.XElement("Top", ShiftPastePosition.Top.ToString()));
                element2.Add(new System.Xml.Linq.XElement("Right", ShiftPastePosition.Right.ToString()));
                element2.Add(new System.Xml.Linq.XElement("Bottom", ShiftPastePosition.Bottom.ToString()));
                element1.Add(element2);
                // End ShiftPastePosition
                // Start MainWindow
                element2 = new("MainWindow");
                element2.Add(new System.Xml.Linq.XElement("X", MainWindowRectangle.X.ToString()));
                element2.Add(new System.Xml.Linq.XElement("Y", MainWindowRectangle.Y.ToString()));
                element2.Add(new System.Xml.Linq.XElement("Width", MainWindowRectangle.Width.ToString()));
                element2.Add(new System.Xml.Linq.XElement("Height", MainWindowRectangle.Height.ToString()));
                element2.Add(new System.Xml.Linq.XElement("WindowState", System.Enum.GetName(typeof(System.Windows.WindowState), WindowStateOfTheMainWindow)));
                element1.Add(element2);
                // End MainWindow
                // Start EventInformation
                element2 = new("EventInformation");
                element2.Add(new System.Xml.Linq.XElement("Enabled", EventInformation.Enabled.ToString()));
                element2.Add(new System.Xml.Linq.XElement("HotkeysDoNotStopFullScreen", EventInformation.HotkeysDoNotStopFullScreen.ToString()));
                element2.Add(new System.Xml.Linq.XElement("StopProcessingFullScreen", EventInformation.StopProcessingFullScreen.ToString()));
                element2.Add(new System.Xml.Linq.XElement("RegisterMultiple", EventInformation.RegisterMultiple.ToString()));
                element2.Add(new System.Xml.Linq.XElement("CaseSensitiveWindowQueries", EventInformation.CaseSensitiveWindowQueries.ToString()));
                element2.Add(new System.Xml.Linq.XElement("DoNotChangeOutOfScreen", EventInformation.DoNotChangeOutOfScreen.ToString()));
                element2.Add(new System.Xml.Linq.XElement("StopProcessingShowAddModifyWindow", EventInformation.StopProcessingShowAddModifyWindow.ToString()));
                element2.Add(new System.Xml.Linq.XElement("EventTypeForeground", EventInformation.EventTypeInformation.Foreground.ToString()));
                element2.Add(new System.Xml.Linq.XElement("EventTypeMoveSizeEnd", EventInformation.EventTypeInformation.MoveSizeEnd.ToString()));
                element2.Add(new System.Xml.Linq.XElement("EventTypeMinimizeStart", EventInformation.EventTypeInformation.MinimizeStart.ToString()));
                element2.Add(new System.Xml.Linq.XElement("EventTypeMinimizeEnd", EventInformation.EventTypeInformation.MinimizeEnd.ToString()));
                element2.Add(new System.Xml.Linq.XElement("EventTypeCreate", EventInformation.EventTypeInformation.Create.ToString()));
                element2.Add(new System.Xml.Linq.XElement("EventTypeShow", EventInformation.EventTypeInformation.Show.ToString()));
                element2.Add(new System.Xml.Linq.XElement("EventTypeNameChange", EventInformation.EventTypeInformation.NameChange.ToString()));
                element2.Add(new System.Xml.Linq.XElement("AddModifyWindowWidth", EventInformation.AddModifyWindowSize.Width.ToString()));
                element2.Add(new System.Xml.Linq.XElement("AddModifyWindowHeight", EventInformation.AddModifyWindowSize.Height.ToString()));
                element2.Add(new System.Xml.Linq.XElement("AcquiredItemsTitleName", EventInformation.AcquiredItems.TitleName.ToString()));
                element2.Add(new System.Xml.Linq.XElement("AcquiredItemsClassName", EventInformation.AcquiredItems.ClassName.ToString()));
                element2.Add(new System.Xml.Linq.XElement("AcquiredItemsFileName", EventInformation.AcquiredItems.FileName.ToString()));
                element2.Add(new System.Xml.Linq.XElement("AcquiredItemsWindowState", EventInformation.AcquiredItems.WindowState.ToString()));
                element2.Add(new System.Xml.Linq.XElement("AcquiredItemsX", EventInformation.AcquiredItems.X.ToString()));
                element2.Add(new System.Xml.Linq.XElement("AcquiredItemsY", EventInformation.AcquiredItems.Y.ToString()));
                element2.Add(new System.Xml.Linq.XElement("AcquiredItemsWidth", EventInformation.AcquiredItems.Width.ToString()));
                element2.Add(new System.Xml.Linq.XElement("AcquiredItemsHeight", EventInformation.AcquiredItems.Height.ToString()));
                element2.Add(new System.Xml.Linq.XElement("AcquiredItemsDisplay", EventInformation.AcquiredItems.Display.ToString()));
                element3 = new("ItemsList");
                foreach (EventItemInformation nowItem in EventInformation.Items)
                {
                    element4 = new("Item");
                    element4.Add(new System.Xml.Linq.XElement("Enabled", nowItem.Enabled.ToString()));
                    element4.Add(new System.Xml.Linq.XElement("RegisteredName", nowItem.RegisteredName));
                    element4.Add(new System.Xml.Linq.XElement("TitleName", nowItem.TitleName));
                    element4.Add(new System.Xml.Linq.XElement("TitleNameMatchCondition", System.Enum.GetName(typeof(NameMatchCondition), nowItem.TitleNameMatchCondition)));
                    element4.Add(new System.Xml.Linq.XElement("ClassName", nowItem.ClassName));
                    element4.Add(new System.Xml.Linq.XElement("ClassNameMatchCondition", System.Enum.GetName(typeof(NameMatchCondition), nowItem.ClassNameMatchCondition)));
                    element4.Add(new System.Xml.Linq.XElement("FileName", nowItem.FileName));
                    element4.Add(new System.Xml.Linq.XElement("FileNameMatchCondition", System.Enum.GetName(typeof(FileNameMatchCondition), nowItem.FileNameMatchCondition)));
                    element4.Add(new System.Xml.Linq.XElement("TitleProcessingConditions", System.Enum.GetName(typeof(TitleProcessingConditions), nowItem.TitleProcessingConditions)));
                    element4.Add(new System.Xml.Linq.XElement("Foreground", nowItem.WindowEventData.Foreground.ToString()));
                    element4.Add(new System.Xml.Linq.XElement("MoveSizeEnd", nowItem.WindowEventData.MoveSizeEnd.ToString()));
                    element4.Add(new System.Xml.Linq.XElement("MinimizeStart", nowItem.WindowEventData.MinimizeStart.ToString()));
                    element4.Add(new System.Xml.Linq.XElement("MinimizeEnd", nowItem.WindowEventData.MinimizeEnd.ToString()));
                    element4.Add(new System.Xml.Linq.XElement("Create", nowItem.WindowEventData.Create.ToString()));
                    element4.Add(new System.Xml.Linq.XElement("Show", nowItem.WindowEventData.Show.ToString()));
                    element4.Add(new System.Xml.Linq.XElement("NameChange", nowItem.WindowEventData.NameChange.ToString()));
                    element4.Add(new System.Xml.Linq.XElement("ProcessingOnlyOnce", System.Enum.GetName(typeof(ProcessingOnlyOnce), nowItem.ProcessingOnlyOnce)));
                    element4.Add(new System.Xml.Linq.XElement("CloseWindow", nowItem.CloseWindow.ToString()));
                    element4.Add(new System.Xml.Linq.XElement("StandardDisplay", System.Enum.GetName(typeof(StandardDisplay), nowItem.StandardDisplay)));
                    element5 = new("ItemsList");
                    foreach (WindowProcessingInformation nowWPI in nowItem.WindowProcessingInformation)
                    {
                        element6 = new("Item");
                        element6.Add(new System.Xml.Linq.XElement("ProcessingName", nowWPI.ProcessingName));
                        element6.Add(new System.Xml.Linq.XElement("Forefront", System.Enum.GetName(typeof(Forefront), nowWPI.Forefront)));
                        element6.Add(new System.Xml.Linq.XElement("EnabledTransparency", nowWPI.EnabledTransparency.ToString()));
                        element6.Add(new System.Xml.Linq.XElement("Transparency", nowWPI.Transparency.ToString()));
                        element6.Add(new System.Xml.Linq.XElement("HotkeyAlt", nowWPI.Hotkey.Alt.ToString()));
                        element6.Add(new System.Xml.Linq.XElement("HotkeyCtrl", nowWPI.Hotkey.Ctrl.ToString()));
                        element6.Add(new System.Xml.Linq.XElement("HotkeyShift", nowWPI.Hotkey.Shift.ToString()));
                        element6.Add(new System.Xml.Linq.XElement("HotkeyWindows", nowWPI.Hotkey.Windows.ToString()));
                        element6.Add(new System.Xml.Linq.XElement("HotkeyKeyCharacter", nowWPI.Hotkey.KeyCharacter.ToString()));
                        element6.Add(new System.Xml.Linq.XElement("WindowState", System.Enum.GetName(typeof(WindowState), nowWPI.WindowState)));
                        element6.Add(new System.Xml.Linq.XElement("OnlyNormalWindow", nowWPI.OnlyNormalWindow.ToString()));
                        element6.Add(new System.Xml.Linq.XElement("Display", nowWPI.PositionSize.Display));
                        element6.Add(new System.Xml.Linq.XElement("X", nowWPI.PositionSize.Position.X.ToString()));
                        element6.Add(new System.Xml.Linq.XElement("Y", nowWPI.PositionSize.Position.Y.ToString()));
                        element6.Add(new System.Xml.Linq.XElement("XType", System.Enum.GetName(typeof(WindowXType), nowWPI.PositionSize.XType)));
                        element6.Add(new System.Xml.Linq.XElement("YType", System.Enum.GetName(typeof(WindowYType), nowWPI.PositionSize.YType)));
                        element6.Add(new System.Xml.Linq.XElement("Width", nowWPI.PositionSize.Size.Width.ToString()));
                        element6.Add(new System.Xml.Linq.XElement("Height", nowWPI.PositionSize.Size.Height.ToString()));
                        element6.Add(new System.Xml.Linq.XElement("WidthType", System.Enum.GetName(typeof(WindowSizeType), nowWPI.PositionSize.WidthType)));
                        element6.Add(new System.Xml.Linq.XElement("HeightType", System.Enum.GetName(typeof(WindowSizeType), nowWPI.PositionSize.HeightType)));
                        element6.Add(new System.Xml.Linq.XElement("XValueType", System.Enum.GetName(typeof(ValueType), nowWPI.PositionSize.XValueType)));
                        element6.Add(new System.Xml.Linq.XElement("YValueType", System.Enum.GetName(typeof(ValueType), nowWPI.PositionSize.YValueType)));
                        element6.Add(new System.Xml.Linq.XElement("WidthValueType", System.Enum.GetName(typeof(ValueType), nowWPI.PositionSize.WidthValueType)));
                        element6.Add(new System.Xml.Linq.XElement("HeightValueType", System.Enum.GetName(typeof(ValueType), nowWPI.PositionSize.HeightValueType)));
                        element6.Add(new System.Xml.Linq.XElement("ClientArea", nowWPI.PositionSize.ClientArea.ToString()));
                        element6.Add(new System.Xml.Linq.XElement("ProcessingPositionAndSizeTwice", nowWPI.PositionSize.ProcessingPositionAndSizeTwice.ToString()));
                        element6.Add(new System.Xml.Linq.XElement("Active", nowWPI.Active.ToString()));
                        element5.Add(element6);
                    }
                    element4.Add(element5);
                    element5 = new("DoNotProcessingSizeList");
                    foreach (System.Drawing.Size nowSize in nowItem.DoNotProcessingSize)
                    {
                        element6 = new("Item");
                        element6.Add(new System.Xml.Linq.XElement("Width", nowSize.Width.ToString()));
                        element6.Add(new System.Xml.Linq.XElement("Height", nowSize.Height.ToString()));
                        element5.Add(element6);
                    }
                    element4.Add(element5);
                    element5 = new("DoNotProcessingTitleNameList");
                    foreach (string nowTitleName in nowItem.DoNotProcessingTitleName)
                    {
                        element6 = new("Item");
                        element6.Add(new System.Xml.Linq.XElement("String", nowTitleName));
                        element5.Add(element6);
                    }
                    element4.Add(element5);
                    element3.Add(element4);
                }
                element2.Add(element3);
                element1.Add(element2);
                // End EventInformation
                // Start TimerInformation
                element2 = new("TimerInformation");
                element2.Add(new System.Xml.Linq.XElement("Enabled", TimerInformation.Enabled.ToString()));
                element2.Add(new System.Xml.Linq.XElement("HotkeysDoNotStopFullScreen", TimerInformation.HotkeysDoNotStopFullScreen.ToString()));
                element2.Add(new System.Xml.Linq.XElement("StopProcessingFullScreen", TimerInformation.StopProcessingFullScreen.ToString()));
                element2.Add(new System.Xml.Linq.XElement("ProcessingWindowRange", System.Enum.GetName(typeof(ProcessingWindowRange), TimerInformation.ProcessingWindowRange)));
                element2.Add(new System.Xml.Linq.XElement("RegisterMultiple", TimerInformation.RegisterMultiple.ToString()));
                element2.Add(new System.Xml.Linq.XElement("ProcessingInterval", TimerInformation.ProcessingInterval.ToString()));
                element2.Add(new System.Xml.Linq.XElement("WaitTimeToProcessingNextWindow", TimerInformation.WaitTimeToProcessingNextWindow.ToString()));
                element2.Add(new System.Xml.Linq.XElement("CaseSensitiveWindowQueries", TimerInformation.CaseSensitiveWindowQueries.ToString()));
                element2.Add(new System.Xml.Linq.XElement("DoNotChangeOutOfScreen", TimerInformation.DoNotChangeOutOfScreen.ToString()));
                element2.Add(new System.Xml.Linq.XElement("StopProcessingShowAddModifyWindow", TimerInformation.StopProcessingShowAddModifyWindow.ToString()));
                element2.Add(new System.Xml.Linq.XElement("AddModifyWindowWidth", TimerInformation.AddModifyWindowSize.Width.ToString()));
                element2.Add(new System.Xml.Linq.XElement("AddModifyWindowHeight", TimerInformation.AddModifyWindowSize.Height.ToString()));
                element2.Add(new System.Xml.Linq.XElement("AcquiredItemsTitleName", TimerInformation.AcquiredItems.TitleName.ToString()));
                element2.Add(new System.Xml.Linq.XElement("AcquiredItemsClassName", TimerInformation.AcquiredItems.ClassName.ToString()));
                element2.Add(new System.Xml.Linq.XElement("AcquiredItemsFileName", TimerInformation.AcquiredItems.FileName.ToString()));
                element2.Add(new System.Xml.Linq.XElement("AcquiredItemsWindowState", TimerInformation.AcquiredItems.WindowState.ToString()));
                element2.Add(new System.Xml.Linq.XElement("AcquiredItemsX", TimerInformation.AcquiredItems.X.ToString()));
                element2.Add(new System.Xml.Linq.XElement("AcquiredItemsY", TimerInformation.AcquiredItems.Y.ToString()));
                element2.Add(new System.Xml.Linq.XElement("AcquiredItemsWidth", TimerInformation.AcquiredItems.Width.ToString()));
                element2.Add(new System.Xml.Linq.XElement("AcquiredItemsHeight", TimerInformation.AcquiredItems.Height.ToString()));
                element2.Add(new System.Xml.Linq.XElement("AcquiredItemsDisplay", TimerInformation.AcquiredItems.Display.ToString()));
                element3 = new("ItemsList");
                foreach (TimerItemInformation nowItem in TimerInformation.Items)
                {
                    element4 = new("Item");
                    element4.Add(new System.Xml.Linq.XElement("Enabled", nowItem.Enabled.ToString()));
                    element4.Add(new System.Xml.Linq.XElement("RegisteredName", nowItem.RegisteredName));
                    element4.Add(new System.Xml.Linq.XElement("TitleName", nowItem.TitleName));
                    element4.Add(new System.Xml.Linq.XElement("TitleNameMatchCondition", System.Enum.GetName(typeof(NameMatchCondition), nowItem.TitleNameMatchCondition)));
                    element4.Add(new System.Xml.Linq.XElement("ClassName", nowItem.ClassName));
                    element4.Add(new System.Xml.Linq.XElement("ClassNameMatchCondition", System.Enum.GetName(typeof(NameMatchCondition), nowItem.ClassNameMatchCondition)));
                    element4.Add(new System.Xml.Linq.XElement("FileName", nowItem.FileName));
                    element4.Add(new System.Xml.Linq.XElement("FileNameMatchCondition", System.Enum.GetName(typeof(FileNameMatchCondition), nowItem.FileNameMatchCondition)));
                    element4.Add(new System.Xml.Linq.XElement("TitleProcessingConditions", System.Enum.GetName(typeof(TitleProcessingConditions), nowItem.TitleProcessingConditions)));
                    element4.Add(new System.Xml.Linq.XElement("ProcessingOnlyOnce", System.Enum.GetName(typeof(ProcessingOnlyOnce), nowItem.ProcessingOnlyOnce)));
                    element4.Add(new System.Xml.Linq.XElement("CloseWindow", nowItem.CloseWindow.ToString()));
                    element4.Add(new System.Xml.Linq.XElement("NumberOfTimesNotToProcessingFirst", nowItem.NumberOfTimesNotToProcessingFirst.ToString()));
                    element4.Add(new System.Xml.Linq.XElement("StandardDisplay", System.Enum.GetName(typeof(StandardDisplay), nowItem.StandardDisplay)));
                    element5 = new("ItemsList");
                    foreach (WindowProcessingInformation nowWPI in nowItem.WindowProcessingInformation)
                    {
                        element6 = new("Item");
                        element6.Add(new System.Xml.Linq.XElement("ProcessingName", nowWPI.ProcessingName));
                        element6.Add(new System.Xml.Linq.XElement("Forefront", System.Enum.GetName(typeof(Forefront), nowWPI.Forefront)));
                        element6.Add(new System.Xml.Linq.XElement("EnabledTransparency", nowWPI.EnabledTransparency.ToString()));
                        element6.Add(new System.Xml.Linq.XElement("Transparency", nowWPI.Transparency.ToString()));
                        element6.Add(new System.Xml.Linq.XElement("HotkeyAlt", nowWPI.Hotkey.Alt.ToString()));
                        element6.Add(new System.Xml.Linq.XElement("HotkeyCtrl", nowWPI.Hotkey.Ctrl.ToString()));
                        element6.Add(new System.Xml.Linq.XElement("HotkeyShift", nowWPI.Hotkey.Shift.ToString()));
                        element6.Add(new System.Xml.Linq.XElement("HotkeyWindows", nowWPI.Hotkey.Windows.ToString()));
                        element6.Add(new System.Xml.Linq.XElement("HotkeyKeyCharacter", nowWPI.Hotkey.KeyCharacter.ToString()));
                        element6.Add(new System.Xml.Linq.XElement("WindowState", System.Enum.GetName(typeof(WindowState), nowWPI.WindowState)));
                        element6.Add(new System.Xml.Linq.XElement("OnlyNormalWindow", nowWPI.OnlyNormalWindow.ToString()));
                        element6.Add(new System.Xml.Linq.XElement("Display", nowWPI.PositionSize.Display));
                        element6.Add(new System.Xml.Linq.XElement("X", nowWPI.PositionSize.Position.X.ToString()));
                        element6.Add(new System.Xml.Linq.XElement("Y", nowWPI.PositionSize.Position.Y.ToString()));
                        element6.Add(new System.Xml.Linq.XElement("XType", System.Enum.GetName(typeof(WindowXType), nowWPI.PositionSize.XType)));
                        element6.Add(new System.Xml.Linq.XElement("YType", System.Enum.GetName(typeof(WindowYType), nowWPI.PositionSize.YType)));
                        element6.Add(new System.Xml.Linq.XElement("Width", nowWPI.PositionSize.Size.Width.ToString()));
                        element6.Add(new System.Xml.Linq.XElement("Height", nowWPI.PositionSize.Size.Height.ToString()));
                        element6.Add(new System.Xml.Linq.XElement("WidthType", System.Enum.GetName(typeof(WindowSizeType), nowWPI.PositionSize.WidthType)));
                        element6.Add(new System.Xml.Linq.XElement("HeightType", System.Enum.GetName(typeof(WindowSizeType), nowWPI.PositionSize.HeightType)));
                        element6.Add(new System.Xml.Linq.XElement("XValueType", System.Enum.GetName(typeof(ValueType), nowWPI.PositionSize.XValueType)));
                        element6.Add(new System.Xml.Linq.XElement("YValueType", System.Enum.GetName(typeof(ValueType), nowWPI.PositionSize.YValueType)));
                        element6.Add(new System.Xml.Linq.XElement("WidthValueType", System.Enum.GetName(typeof(ValueType), nowWPI.PositionSize.WidthValueType)));
                        element6.Add(new System.Xml.Linq.XElement("HeightValueType", System.Enum.GetName(typeof(ValueType), nowWPI.PositionSize.HeightValueType)));
                        element6.Add(new System.Xml.Linq.XElement("ClientArea", nowWPI.PositionSize.ClientArea.ToString()));
                        element6.Add(new System.Xml.Linq.XElement("ProcessingPositionAndSizeTwice", nowWPI.PositionSize.ProcessingPositionAndSizeTwice.ToString()));
                        element6.Add(new System.Xml.Linq.XElement("Active", nowWPI.Active.ToString()));
                        element5.Add(element6);
                    }
                    element4.Add(element5);
                    element5 = new("DoNotProcessingSizeList");
                    foreach (System.Drawing.Size nowSize in nowItem.DoNotProcessingSize)
                    {
                        element6 = new("Item");
                        element6.Add(new System.Xml.Linq.XElement("Width", nowSize.Width.ToString()));
                        element6.Add(new System.Xml.Linq.XElement("Height", nowSize.Height.ToString()));
                        element5.Add(element6);
                    }
                    element4.Add(element5);
                    element5 = new("DoNotProcessingTitleNameList");
                    foreach (string nowTitleName in nowItem.DoNotProcessingTitleName)
                    {
                        element6= new("Item");
                        element6.Add(new System.Xml.Linq.XElement("String", nowTitleName));
                        element5.Add(element6);
                    }
                    element4.Add(element5);
                    element3.Add(element4);
                }
                element2.Add(element3);
                element1.Add(element2);
                // End TimerInformation
                // Start MagnetInformation
                element2 = new("MagnetInformation");
                element2.Add(new System.Xml.Linq.XElement("Enabled", MagnetInformation.Enabled.ToString()));
                element2.Add(new System.Xml.Linq.XElement("PasteToScreenEdge", MagnetInformation.PasteToScreenEdge.ToString()));
                element2.Add(new System.Xml.Linq.XElement("PasteToAnotherWindow", MagnetInformation.PasteToAnotherWindow.ToString()));
                element2.Add(new System.Xml.Linq.XElement("PressTheKeyToPaste", MagnetInformation.PressTheKeyToPaste.ToString()));
                element2.Add(new System.Xml.Linq.XElement("StopTimeWhenPasted", MagnetInformation.StopTimeWhenPasted.ToString()));
                element2.Add(new System.Xml.Linq.XElement("DecisionDistanceToPaste", MagnetInformation.DecisionDistanceToPaste.ToString()));
                element2.Add(new System.Xml.Linq.XElement("StopProcessingFullScreen", MagnetInformation.StopProcessingFullScreen.ToString()));
                element1.Add(element2);
                // End MagnetInformation
                // Start HotkeyInformation
                element2 = new("HotkeyInformation");
                element2.Add(new System.Xml.Linq.XElement("Enabled", HotkeyInformation.Enabled.ToString()));
                element2.Add(new System.Xml.Linq.XElement("DoNotChangeOutOfScreen", HotkeyInformation.DoNotChangeOutOfScreen.ToString()));
                element2.Add(new System.Xml.Linq.XElement("StopProcessingFullScreen", HotkeyInformation.StopProcessingFullScreen.ToString()));
                element2.Add(new System.Xml.Linq.XElement("AddModifyWindowWidth", HotkeyInformation.AddModifyWindowSize.Width.ToString()));
                element2.Add(new System.Xml.Linq.XElement("AddModifyWindowHeight", HotkeyInformation.AddModifyWindowSize.Height.ToString()));
                element3 = new("ItemsList");
                foreach (HotkeyItemInformation nowItem in HotkeyInformation.Items)
                {
                    element4 = new("Item");
                    element4.Add(new System.Xml.Linq.XElement("RegisteredName", nowItem.RegisteredName));
                    element4.Add(new System.Xml.Linq.XElement("ProcessingType", System.Enum.GetName(typeof(HotkeyProcessingType), nowItem.ProcessingType)));
                    element4.Add(new System.Xml.Linq.XElement("X", nowItem.PositionSize.Position.X.ToString()));
                    element4.Add(new System.Xml.Linq.XElement("Y", nowItem.PositionSize.Position.Y.ToString()));
                    element4.Add(new System.Xml.Linq.XElement("Width", nowItem.PositionSize.Size.Width.ToString()));
                    element4.Add(new System.Xml.Linq.XElement("Height", nowItem.PositionSize.Size.Height.ToString()));
                    element4.Add(new System.Xml.Linq.XElement("XType", System.Enum.GetName(typeof(WindowXType), nowItem.PositionSize.XType)));
                    element4.Add(new System.Xml.Linq.XElement("YType", System.Enum.GetName(typeof(WindowYType), nowItem.PositionSize.YType)));
                    element4.Add(new System.Xml.Linq.XElement("WidthType", System.Enum.GetName(typeof(WindowSizeType), nowItem.PositionSize.WidthType)));
                    element4.Add(new System.Xml.Linq.XElement("HeightType", System.Enum.GetName(typeof(WindowSizeType), nowItem.PositionSize.HeightType)));
                    element4.Add(new System.Xml.Linq.XElement("XValueType", System.Enum.GetName(typeof(ValueType), nowItem.PositionSize.XValueType)));
                    element4.Add(new System.Xml.Linq.XElement("YValueType", System.Enum.GetName(typeof(ValueType), nowItem.PositionSize.YValueType)));
                    element4.Add(new System.Xml.Linq.XElement("WidthValueType", System.Enum.GetName(typeof(ValueType), nowItem.PositionSize.WidthValueType)));
                    element4.Add(new System.Xml.Linq.XElement("HeightValueType", System.Enum.GetName(typeof(ValueType), nowItem.PositionSize.HeightValueType)));
                    element4.Add(new System.Xml.Linq.XElement("StandardDisplay", System.Enum.GetName(typeof(StandardDisplay), nowItem.StandardDisplay)));
                    element4.Add(new System.Xml.Linq.XElement("Display", nowItem.PositionSize.Display));
                    element4.Add(new System.Xml.Linq.XElement("ClientArea", nowItem.PositionSize.ClientArea.ToString()));
                    element4.Add(new System.Xml.Linq.XElement("ProcessingPositionAndSizeTwice", nowItem.PositionSize.ProcessingPositionAndSizeTwice.ToString()));
                    element4.Add(new System.Xml.Linq.XElement("ProcessingValue", nowItem.ProcessingValue.ToString()));
                    element4.Add(new System.Xml.Linq.XElement("HotkeyAlt", nowItem.Hotkey.Alt.ToString()));
                    element4.Add(new System.Xml.Linq.XElement("HotkeyCtrl", nowItem.Hotkey.Ctrl.ToString()));
                    element4.Add(new System.Xml.Linq.XElement("HotkeyShift", nowItem.Hotkey.Shift.ToString()));
                    element4.Add(new System.Xml.Linq.XElement("HotkeyWindows", nowItem.Hotkey.Windows.ToString()));
                    element4.Add(new System.Xml.Linq.XElement("HotkeyKeyCharacter", nowItem.Hotkey.KeyCharacter.ToString()));
                    element3.Add(element4);
                }
                element2.Add(element3);
                element1.Add(element2);
                // End HotkeyInformation
                // Start OutsideToInsideInformation
                element2 = new("OutsideToInsideInformation");
                element2.Add(new System.Xml.Linq.XElement("Enabled", OutsideToInsideInformation.Enabled.ToString()));
                element2.Add(new System.Xml.Linq.XElement("ProcessingInterval", OutsideToInsideInformation.ProcessingInterval.ToString()));
                element2.Add(new System.Xml.Linq.XElement("Display", OutsideToInsideInformation.Display));
                element1.Add(element2);
                // End OutsideToInsideInformation
                // Start MoveCenterInformation
                element2 = new("MoveCenterInformation");
                element2.Add(new System.Xml.Linq.XElement("Enabled", MoveCenterInformation.Enabled.ToString()));
                element1.Add(element2);
                // End MoveCenterInformation

                document.Add(element1);

                document.Save(path);
                result = true;
            }
            catch
            {
            }

            return (result);
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
                System.Xml.Linq.XDocument document = System.Xml.Linq.XDocument.Load(path);
                System.Xml.Linq.XElement element = document.Element("Settings");

                if (element != null)
                {
                    int settingVersion = Processing.GetIntXml(element, "SettingVersion", 0);        // 設定のバージョン

                    switch (settingVersion)
                    {
                        case 1:
                            {
                                System.Xml.Linq.XElement elementNode1;

                                Language = Processing.GetStringXml(element, "Language", Language);
                                System.Enum.TryParse(Processing.GetStringXml(element, "CoordinateType", System.Enum.GetName(typeof(CoordinateType), CoordinateType)), out CoordinateType);
                                AutomaticallyUpdateCheck = Processing.GetBoolXml(element, "AutomaticallyUpdateCheck", AutomaticallyUpdateCheck);
                                CheckBetaVersion = Processing.GetBoolXml(element, "CheckBetaVersion", CheckBetaVersion);
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
                                // Start MainWindow
                                elementNode1 = element.Element("MainWindow");
                                if (elementNode1 != null)
                                {
                                    MainWindowRectangle.X = Processing.GetIntXml(elementNode1, "X", MainWindowRectangle.X);
                                    MainWindowRectangle.Y = Processing.GetIntXml(elementNode1, "Y", MainWindowRectangle.Y);
                                    MainWindowRectangle.Width = Processing.GetIntXml(elementNode1, "Width", MainWindowRectangle.Width);
                                    MainWindowRectangle.Height = Processing.GetIntXml(elementNode1, "Height", MainWindowRectangle.Height);
                                    System.Enum.TryParse(Processing.GetStringXml(elementNode1, "WindowState", System.Enum.GetName(typeof(System.Windows.WindowState), WindowStateOfTheMainWindow)), out WindowStateOfTheMainWindow);
                                }
                                // End MainWindow
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
                                    EventInformation.EventTypeInformation.Foreground = Processing.GetBoolXml(elementNode1, "EventTypeForeground", EventInformation.EventTypeInformation.Foreground);
                                    EventInformation.EventTypeInformation.MoveSizeEnd = Processing.GetBoolXml(elementNode1, "EventTypeMoveSizeEnd", EventInformation.EventTypeInformation.MoveSizeEnd);
                                    EventInformation.EventTypeInformation.MinimizeStart = Processing.GetBoolXml(elementNode1, "EventTypeMinimizeStart", EventInformation.EventTypeInformation.MinimizeStart);
                                    EventInformation.EventTypeInformation.MinimizeEnd = Processing.GetBoolXml(elementNode1, "EventTypeMinimizeEnd", EventInformation.EventTypeInformation.MinimizeEnd);
                                    EventInformation.EventTypeInformation.Create = Processing.GetBoolXml(elementNode1, "EventTypeCreate", EventInformation.EventTypeInformation.Create);
                                    EventInformation.EventTypeInformation.Show = Processing.GetBoolXml(elementNode1, "EventTypeShow", EventInformation.EventTypeInformation.Show);
                                    EventInformation.EventTypeInformation.NameChange = Processing.GetBoolXml(elementNode1, "EventTypeNameChange", EventInformation.EventTypeInformation.NameChange);
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
                                    System.Xml.Linq.XElement elementNode2 = elementNode1.Element("ItemsList");
                                    if (elementNode2 != null)
                                    {
                                        // EventItemInformation
                                        System.Collections.Generic.IEnumerable<System.Xml.Linq.XElement> enumElement = elementNode2.Elements("Item");
                                        foreach (System.Xml.Linq.XElement nowElementEII in enumElement)
                                        {
                                            EventItemInformation newEII = new();

                                            newEII.Enabled = Processing.GetBoolXml(nowElementEII, "Enabled", newEII.Enabled);
                                            newEII.RegisteredName = Processing.GetStringXml(nowElementEII, "RegisteredName", newEII.RegisteredName);
                                            newEII.TitleName = Processing.GetStringXml(nowElementEII, "TitleName", newEII.TitleName);
                                            System.Enum.TryParse(Processing.GetStringXml(nowElementEII, "TitleNameMatchCondition", System.Enum.GetName(typeof(NameMatchCondition), newEII.TitleNameMatchCondition)), out newEII.TitleNameMatchCondition);
                                            newEII.ClassName = Processing.GetStringXml(nowElementEII, "ClassName", newEII.ClassName);
                                            System.Enum.TryParse(Processing.GetStringXml(nowElementEII, "ClassNameMatchCondition", System.Enum.GetName(typeof(NameMatchCondition), newEII.ClassNameMatchCondition)), out newEII.ClassNameMatchCondition);
                                            newEII.FileName = Processing.GetStringXml(nowElementEII, "FileName", newEII.FileName);
                                            System.Enum.TryParse(Processing.GetStringXml(nowElementEII, "FileNameMatchCondition", System.Enum.GetName(typeof(FileNameMatchCondition), newEII.FileNameMatchCondition)), out newEII.FileNameMatchCondition);
                                            System.Enum.TryParse(Processing.GetStringXml(nowElementEII, "TitleProcessingConditions", System.Enum.GetName(typeof(TitleProcessingConditions), newEII.TitleProcessingConditions)), out newEII.TitleProcessingConditions);
                                            newEII.WindowEventData.Foreground = Processing.GetBoolXml(nowElementEII, "CloseWindow", newEII.WindowEventData.Foreground);
                                            newEII.WindowEventData.MoveSizeEnd = Processing.GetBoolXml(nowElementEII, "MoveSizeEnd", newEII.WindowEventData.MoveSizeEnd);
                                            newEII.WindowEventData.MinimizeStart = Processing.GetBoolXml(nowElementEII, "MinimizeStart", newEII.WindowEventData.MinimizeStart);
                                            newEII.WindowEventData.MinimizeEnd = Processing.GetBoolXml(nowElementEII, "MinimizeEnd", newEII.WindowEventData.MinimizeEnd);
                                            newEII.WindowEventData.Create = Processing.GetBoolXml(nowElementEII, "Create", newEII.WindowEventData.Create);
                                            newEII.WindowEventData.Show = Processing.GetBoolXml(nowElementEII, "Show", newEII.WindowEventData.Show);
                                            newEII.WindowEventData.NameChange = Processing.GetBoolXml(nowElementEII, "NameChange", newEII.WindowEventData.NameChange);
                                            System.Enum.TryParse(Processing.GetStringXml(nowElementEII, "ProcessingOnlyOnce", System.Enum.GetName(typeof(ProcessingOnlyOnce), newEII.ProcessingOnlyOnce)), out newEII.ProcessingOnlyOnce);
                                            newEII.CloseWindow = Processing.GetBoolXml(nowElementEII, "CloseWindow", newEII.CloseWindow);
                                            System.Enum.TryParse(Processing.GetStringXml(nowElementEII, "StandardDisplay", System.Enum.GetName(typeof(StandardDisplay), newEII.StandardDisplay)), out newEII.StandardDisplay);
                                            // WindowProcessingInformation
                                            System.Xml.Linq.XElement elementNode3 = nowElementEII.Element("ItemsList");
                                            if (elementNode3 != null)
                                            {
                                                foreach (System.Xml.Linq.XElement nowElement in elementNode3.Elements("Item"))
                                                {
                                                    WindowProcessingInformation newWPI = new();

                                                    newWPI.ProcessingName = Processing.GetStringXml(nowElement, "ProcessingName", newWPI.ProcessingName);
                                                    System.Enum.TryParse(Processing.GetStringXml(nowElement, "Forefront", System.Enum.GetName(typeof(Forefront), newWPI.Forefront)), out newWPI.Forefront);
                                                    newWPI.EnabledTransparency = Processing.GetBoolXml(nowElement, "EnabledTransparency", newWPI.EnabledTransparency);
                                                    newWPI.Transparency = Processing.GetIntXml(nowElement, "Transparency", newWPI.Transparency);
                                                    newWPI.Hotkey.Alt = Processing.GetBoolXml(nowElement, "HotkeyAlt", newWPI.Hotkey.Alt);
                                                    newWPI.Hotkey.Ctrl = Processing.GetBoolXml(nowElement, "HotkeyCtrl", newWPI.Hotkey.Ctrl);
                                                    newWPI.Hotkey.Shift = Processing.GetBoolXml(nowElement, "HotkeyShift", newWPI.Hotkey.Shift);
                                                    newWPI.Hotkey.Windows = Processing.GetBoolXml(nowElement, "HotkeyWindows", newWPI.Hotkey.Windows);
                                                    newWPI.Hotkey.KeyCharacter = Processing.GetIntXml(nowElement, "HotkeyKeyCharacter", newWPI.Hotkey.KeyCharacter);
                                                    System.Enum.TryParse(Processing.GetStringXml(nowElement, "WindowState", System.Enum.GetName(typeof(WindowState), newWPI.WindowState)), out newWPI.WindowState);
                                                    newWPI.OnlyNormalWindow = Processing.GetBoolXml(nowElement, "OnlyNormalWindow", newWPI.OnlyNormalWindow);
                                                    newWPI.PositionSize.Display = Processing.GetStringXml(nowElement, "Display", newWPI.PositionSize.Display);
                                                    newWPI.PositionSize.Position.X = Processing.GetDecimalXml(nowElement, "X", newWPI.PositionSize.Position.X);
                                                    newWPI.PositionSize.Position.Y = Processing.GetDecimalXml(nowElement, "Y", newWPI.PositionSize.Position.Y);
                                                    System.Enum.TryParse(Processing.GetStringXml(nowElement, "XType", System.Enum.GetName(typeof(WindowXType), newWPI.PositionSize.XType)), out newWPI.PositionSize.XType);
                                                    System.Enum.TryParse(Processing.GetStringXml(nowElement, "YType", System.Enum.GetName(typeof(WindowYType), newWPI.PositionSize.YType)), out newWPI.PositionSize.YType);
                                                    newWPI.PositionSize.Size.Width = Processing.GetDecimalXml(nowElement, "Width", newWPI.PositionSize.Size.Width);
                                                    newWPI.PositionSize.Size.Height = Processing.GetDecimalXml(nowElement, "Height", newWPI.PositionSize.Size.Height);
                                                    System.Enum.TryParse(Processing.GetStringXml(nowElement, "WidthType", System.Enum.GetName(typeof(WindowSizeType), newWPI.PositionSize.WidthType)), out newWPI.PositionSize.WidthType);
                                                    System.Enum.TryParse(Processing.GetStringXml(nowElement, "HeightType", System.Enum.GetName(typeof(WindowSizeType), newWPI.PositionSize.HeightType)), out newWPI.PositionSize.HeightType);
                                                    System.Enum.TryParse(Processing.GetStringXml(nowElement, "XValueType", System.Enum.GetName(typeof(ValueType), newWPI.PositionSize.XValueType)), out newWPI.PositionSize.XValueType);
                                                    System.Enum.TryParse(Processing.GetStringXml(nowElement, "YValueType", System.Enum.GetName(typeof(ValueType), newWPI.PositionSize.YValueType)), out newWPI.PositionSize.YValueType);
                                                    System.Enum.TryParse(Processing.GetStringXml(nowElement, "WidthValueType", System.Enum.GetName(typeof(ValueType), newWPI.PositionSize.WidthValueType)), out newWPI.PositionSize.WidthValueType);
                                                    System.Enum.TryParse(Processing.GetStringXml(nowElement, "HeightValueType", System.Enum.GetName(typeof(ValueType), newWPI.PositionSize.HeightValueType)), out newWPI.PositionSize.HeightValueType);
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
                                                foreach (System.Xml.Linq.XElement nowElement in elementNode3.Elements("Item"))
                                                {
                                                    System.Drawing.Size newSize = new();
                                                    newSize.Width = Processing.GetIntXml(nowElement, "Width", newSize.Width);
                                                    newSize.Height = Processing.GetIntXml(nowElement, "Height", newSize.Height);
                                                    newEII.DoNotProcessingSize.Add(newSize);
                                                }
                                            }
                                            // DoNotProcessingTitleName
                                            elementNode3 = nowElementEII.Element("DoNotProcessingTitleName");
                                            if (elementNode3 != null)
                                            {
                                                foreach (System.Xml.Linq.XElement nowElement in elementNode3.Elements("Item"))
                                                {
                                                    string newString = "";
                                                    newString = Processing.GetStringXml(nowElement, "String", newString);
                                                    newEII.DoNotProcessingTitleName.Add(newString);
                                                }
                                            }

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
                                    System.Enum.TryParse(Processing.GetStringXml(elementNode1, "ProcessingWindowRange", System.Enum.GetName(typeof(ProcessingWindowRange), TimerInformation.ProcessingWindowRange)), out TimerInformation.ProcessingWindowRange);
                                    TimerInformation.RegisterMultiple = Processing.GetBoolXml(elementNode1, "RegisterMultiple", TimerInformation.RegisterMultiple);
                                    TimerInformation.ProcessingInterval = Processing.GetIntXml(elementNode1, "ProcessingInterval", TimerInformation.ProcessingInterval);
                                    TimerInformation.WaitTimeToProcessingNextWindow = Processing.GetIntXml(elementNode1, "WaitTimeToProcessingNextWindow", TimerInformation.WaitTimeToProcessingNextWindow);
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
                                    System.Xml.Linq.XElement elementNode2 = elementNode1.Element("ItemsList");
                                    if (elementNode2 != null)
                                    {
                                        // TimerItemInformation
                                        System.Collections.Generic.IEnumerable<System.Xml.Linq.XElement> enumElement = elementNode2.Elements("Item");
                                        foreach (System.Xml.Linq.XElement nowElementTII in enumElement)
                                        {
                                            TimerItemInformation newTII = new();

                                            newTII.Enabled = Processing.GetBoolXml(nowElementTII, "Enabled", newTII.Enabled);
                                            newTII.RegisteredName = Processing.GetStringXml(nowElementTII, "RegisteredName", newTII.RegisteredName);
                                            newTII.TitleName = Processing.GetStringXml(nowElementTII, "TitleName", newTII.TitleName);
                                            System.Enum.TryParse(Processing.GetStringXml(nowElementTII, "TitleNameMatchCondition", System.Enum.GetName(typeof(NameMatchCondition), newTII.TitleNameMatchCondition)), out newTII.TitleNameMatchCondition);
                                            newTII.ClassName = Processing.GetStringXml(nowElementTII, "ClassName", newTII.ClassName);
                                            System.Enum.TryParse(Processing.GetStringXml(nowElementTII, "ClassNameMatchCondition", System.Enum.GetName(typeof(NameMatchCondition), newTII.ClassNameMatchCondition)), out newTII.ClassNameMatchCondition);
                                            newTII.FileName = Processing.GetStringXml(nowElementTII, "FileName", newTII.FileName);
                                            System.Enum.TryParse(Processing.GetStringXml(nowElementTII, "FileNameMatchCondition", System.Enum.GetName(typeof(FileNameMatchCondition), newTII.FileNameMatchCondition)), out newTII.FileNameMatchCondition);
                                            System.Enum.TryParse(Processing.GetStringXml(nowElementTII, "TitleProcessingConditions", System.Enum.GetName(typeof(TitleProcessingConditions), newTII.TitleProcessingConditions)), out newTII.TitleProcessingConditions);
                                            System.Enum.TryParse(Processing.GetStringXml(nowElementTII, "ProcessingOnlyOnce", System.Enum.GetName(typeof(ProcessingOnlyOnce), newTII.ProcessingOnlyOnce)), out newTII.ProcessingOnlyOnce);
                                            newTII.CloseWindow = Processing.GetBoolXml(nowElementTII, "CloseWindow", newTII.CloseWindow);
                                            newTII.NumberOfTimesNotToProcessingFirst = Processing.GetIntXml(nowElementTII, "NumberOfTimesNotToProcessingFirst", newTII.NumberOfTimesNotToProcessingFirst);
                                            System.Enum.TryParse(Processing.GetStringXml(nowElementTII, "StandardDisplay", System.Enum.GetName(typeof(StandardDisplay), newTII.StandardDisplay)), out newTII.StandardDisplay);
                                            // WindowProcessingInformation
                                            System.Xml.Linq.XElement elementNode3 = nowElementTII.Element("ItemsList");
                                            if (elementNode3 != null)
                                            {
                                                foreach (System.Xml.Linq.XElement nowElement in elementNode3.Elements("Item"))
                                                {
                                                    WindowProcessingInformation newWPI = new();

                                                    newWPI.ProcessingName = Processing.GetStringXml(nowElement, "ProcessingName", newWPI.ProcessingName);
                                                    System.Enum.TryParse(Processing.GetStringXml(nowElement, "Forefront", System.Enum.GetName(typeof(Forefront), newWPI.Forefront)), out newWPI.Forefront);
                                                    newWPI.EnabledTransparency = Processing.GetBoolXml(nowElement, "EnabledTransparency", newWPI.EnabledTransparency);
                                                    newWPI.Transparency = Processing.GetIntXml(nowElement, "Transparency", newWPI.Transparency);
                                                    newWPI.Hotkey.Alt = Processing.GetBoolXml(nowElement, "HotkeyAlt", newWPI.Hotkey.Alt);
                                                    newWPI.Hotkey.Ctrl = Processing.GetBoolXml(nowElement, "HotkeyCtrl", newWPI.Hotkey.Ctrl);
                                                    newWPI.Hotkey.Shift = Processing.GetBoolXml(nowElement, "HotkeyShift", newWPI.Hotkey.Shift);
                                                    newWPI.Hotkey.Windows = Processing.GetBoolXml(nowElement, "HotkeyWindows", newWPI.Hotkey.Windows);
                                                    newWPI.Hotkey.KeyCharacter = Processing.GetIntXml(nowElement, "HotkeyKeyCharacter", newWPI.Hotkey.KeyCharacter);
                                                    System.Enum.TryParse(Processing.GetStringXml(nowElement, "WindowState", System.Enum.GetName(typeof(WindowState), newWPI.WindowState)), out newWPI.WindowState);
                                                    newWPI.OnlyNormalWindow = Processing.GetBoolXml(nowElement, "OnlyNormalWindow", newWPI.OnlyNormalWindow);
                                                    newWPI.PositionSize.Display = Processing.GetStringXml(nowElement, "Display", newWPI.PositionSize.Display);
                                                    newWPI.PositionSize.Position.X = Processing.GetDecimalXml(nowElement, "X", newWPI.PositionSize.Position.X);
                                                    newWPI.PositionSize.Position.Y = Processing.GetDecimalXml(nowElement, "Y", newWPI.PositionSize.Position.Y);
                                                    System.Enum.TryParse(Processing.GetStringXml(nowElement, "XType", System.Enum.GetName(typeof(WindowXType), newWPI.PositionSize.XType)), out newWPI.PositionSize.XType);
                                                    System.Enum.TryParse(Processing.GetStringXml(nowElement, "YType", System.Enum.GetName(typeof(WindowYType), newWPI.PositionSize.YType)), out newWPI.PositionSize.YType);
                                                    newWPI.PositionSize.Size.Width = Processing.GetDecimalXml(nowElement, "Width", newWPI.PositionSize.Size.Width);
                                                    newWPI.PositionSize.Size.Height = Processing.GetDecimalXml(nowElement, "Height", newWPI.PositionSize.Size.Height);
                                                    System.Enum.TryParse(Processing.GetStringXml(nowElement, "WidthType", System.Enum.GetName(typeof(WindowSizeType), newWPI.PositionSize.WidthType)), out newWPI.PositionSize.WidthType);
                                                    System.Enum.TryParse(Processing.GetStringXml(nowElement, "HeightType", System.Enum.GetName(typeof(WindowSizeType), newWPI.PositionSize.HeightType)), out newWPI.PositionSize.HeightType);
                                                    System.Enum.TryParse(Processing.GetStringXml(nowElement, "XValueType", System.Enum.GetName(typeof(ValueType), newWPI.PositionSize.XValueType)), out newWPI.PositionSize.XValueType);
                                                    System.Enum.TryParse(Processing.GetStringXml(nowElement, "YValueType", System.Enum.GetName(typeof(ValueType), newWPI.PositionSize.YValueType)), out newWPI.PositionSize.YValueType);
                                                    System.Enum.TryParse(Processing.GetStringXml(nowElement, "WidthValueType", System.Enum.GetName(typeof(ValueType), newWPI.PositionSize.WidthValueType)), out newWPI.PositionSize.WidthValueType);
                                                    System.Enum.TryParse(Processing.GetStringXml(nowElement, "HeightValueType", System.Enum.GetName(typeof(ValueType), newWPI.PositionSize.HeightValueType)), out newWPI.PositionSize.HeightValueType);
                                                    newWPI.PositionSize.ClientArea = Processing.GetBoolXml(nowElement, "ClientArea", newWPI.PositionSize.ClientArea);
                                                    newWPI.PositionSize.ProcessingPositionAndSizeTwice = Processing.GetBoolXml(nowElement, "ProcessingPositionAndSizeTwice", newWPI.PositionSize.ProcessingPositionAndSizeTwice);
                                                    newWPI.Active = Processing.GetBoolXml(nowElement, "Active", newWPI.Active);

                                                    newTII.WindowProcessingInformation.Add(newWPI);
                                                }
                                            }
                                            // DoNotProcessingSize
                                            elementNode3 = nowElementTII.Element("DoNotProcessingSizeList");
                                            if (elementNode3 != null)
                                            {
                                                foreach (System.Xml.Linq.XElement nowElement in elementNode3.Elements("Item"))
                                                {
                                                    System.Drawing.Size newSize = new();
                                                    newSize.Width = Processing.GetIntXml(nowElement, "Width", newSize.Width);
                                                    newSize.Height = Processing.GetIntXml(nowElement, "Height", newSize.Height);
                                                    newTII.DoNotProcessingSize.Add(newSize);
                                                }
                                            }
                                            // DoNotProcessingTitleName
                                            elementNode3 = nowElementTII.Element("DoNotProcessingTitleName");
                                            if (elementNode3 != null)
                                            {
                                                foreach (System.Xml.Linq.XElement nowElement in elementNode3.Elements("Item"))
                                                {
                                                    string newString = "";
                                                    newString = Processing.GetStringXml(nowElement, "String", newString);
                                                    newTII.DoNotProcessingTitleName.Add(newString);
                                                }
                                            }

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
                                    System.Xml.Linq.XElement elementNode2 = elementNode1.Element("ItemsList");
                                    if (elementNode2 != null)
                                    {
                                        foreach (System.Xml.Linq.XElement nowElementHII in elementNode2.Elements("Item"))
                                        {
                                            HotkeyItemInformation newHII = new();

                                            newHII.RegisteredName = Processing.GetStringXml(nowElementHII, "RegisteredName", newHII.RegisteredName);
                                            System.Enum.TryParse(Processing.GetStringXml(nowElementHII, "ProcessingType", System.Enum.GetName(typeof(HotkeyProcessingType), newHII.ProcessingType)), out newHII.ProcessingType);
                                            newHII.PositionSize.Position.X = Processing.GetDecimalXml(nowElementHII, "X", newHII.PositionSize.Position.X);
                                            newHII.PositionSize.Position.Y = Processing.GetDecimalXml(nowElementHII, "Y", newHII.PositionSize.Position.Y);
                                            newHII.PositionSize.Size.Width = Processing.GetDecimalXml(nowElementHII, "Width", newHII.PositionSize.Size.Width);
                                            newHII.PositionSize.Size.Height = Processing.GetDecimalXml(nowElementHII, "Height", newHII.PositionSize.Size.Height);
                                            System.Enum.TryParse(Processing.GetStringXml(nowElementHII, "XType", System.Enum.GetName(typeof(WindowXType), newHII.PositionSize.XType)), out newHII.PositionSize.XType);
                                            System.Enum.TryParse(Processing.GetStringXml(nowElementHII, "YType", System.Enum.GetName(typeof(WindowYType), newHII.PositionSize.YType)), out newHII.PositionSize.YType);
                                            System.Enum.TryParse(Processing.GetStringXml(nowElementHII, "WidthType", System.Enum.GetName(typeof(WindowSizeType), newHII.PositionSize.WidthType)), out newHII.PositionSize.WidthType);
                                            System.Enum.TryParse(Processing.GetStringXml(nowElementHII, "HeightType", System.Enum.GetName(typeof(WindowSizeType), newHII.PositionSize.HeightType)), out newHII.PositionSize.HeightType);
                                            System.Enum.TryParse(Processing.GetStringXml(nowElementHII, "XValueType", System.Enum.GetName(typeof(ValueType), newHII.PositionSize.XValueType)), out newHII.PositionSize.XValueType);
                                            System.Enum.TryParse(Processing.GetStringXml(nowElementHII, "YValueType", System.Enum.GetName(typeof(ValueType), newHII.PositionSize.YValueType)), out newHII.PositionSize.YValueType);
                                            System.Enum.TryParse(Processing.GetStringXml(nowElementHII, "WidthValueType", System.Enum.GetName(typeof(ValueType), newHII.PositionSize.WidthValueType)), out newHII.PositionSize.WidthValueType);
                                            System.Enum.TryParse(Processing.GetStringXml(nowElementHII, "HeightValueType", System.Enum.GetName(typeof(ValueType), newHII.PositionSize.HeightValueType)), out newHII.PositionSize.HeightValueType);
                                            System.Enum.TryParse(Processing.GetStringXml(nowElementHII, "StandardDisplay", System.Enum.GetName(typeof(StandardDisplay), newHII.StandardDisplay)), out newHII.StandardDisplay);
                                            newHII.PositionSize.Display = Processing.GetStringXml(nowElementHII, "Display", newHII.PositionSize.Display);
                                            newHII.PositionSize.ClientArea = Processing.GetBoolXml(nowElementHII, "ClientArea", newHII.PositionSize.ClientArea);
                                            newHII.PositionSize.ProcessingPositionAndSizeTwice = Processing.GetBoolXml(nowElementHII, "ProcessingPositionAndSizeTwice", newHII.PositionSize.ProcessingPositionAndSizeTwice);
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
                                // Start OutsideToInsideInformation
                                elementNode1 = element.Element("OutsideToInsideInformation");
                                if (elementNode1 != null)
                                {
                                    OutsideToInsideInformation.Enabled = Processing.GetBoolXml(elementNode1, "Enabled", OutsideToInsideInformation.Enabled);
                                    OutsideToInsideInformation.ProcessingInterval = Processing.GetIntXml(elementNode1, "ProcessingInterval", OutsideToInsideInformation.ProcessingInterval);
                                    OutsideToInsideInformation.Display = Processing.GetStringXml(elementNode1, "Display", OutsideToInsideInformation.Display);
                                }
                                // End OutsideToInsideInformation
                                // Start MoveCenterInformation
                                elementNode1 = element.Element("MoveCenterInformation");
                                if (elementNode1 != null)
                                {
                                    MoveCenterInformation.Enabled = Processing.GetBoolXml(elementNode1, "Enabled", MoveCenterInformation.Enabled);
                                }
                                // End MoveCenterInformation
                            }
                            break;
                    }

                    result = true;
                }
            }
            catch
            {
            }

            return (result);
        }
    }
}
