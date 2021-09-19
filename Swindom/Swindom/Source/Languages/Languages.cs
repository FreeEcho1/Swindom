namespace Swindom.Source.Languages
{
    /// <summary>
    /// 言語
    /// </summary>
    public class Languages
    {
        /// <summary>
        /// エラーが発生しました。
        /// </summary>
        public string ErrorOccurred = "エラーが発生しました。";
        /// <summary>
        /// 確認
        /// </summary>
        public string Check = "確認";
        /// <summary>
        /// ウィンドウで使用する言語
        /// </summary>
        public LanguagesWindow LanguagesWindow = new();

        /// <summary>
        /// ファイルから言語名を取得
        /// </summary>
        /// <param name="path">パス</param>
        /// <returns></returns>
        public static string GetLanguageName(
            string path
            )
        {
            string name = "";       // 言語名

            try
            {
                System.Xml.Linq.XDocument document = System.Xml.Linq.XDocument.Load(path);
                System.Xml.Linq.XElement element = document.Element("Language");

                if (element != null)
                {
                    name = Processing.GetStringXml(element, "LanguageName", "");
                }
            }
            catch
            {
            }

            return (name);
        }

        /// <summary>
        /// 言語をファイルから読み込む
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
                if (LanguagesWindow == null)
                {
                    LanguagesWindow = new();
                }

                System.Xml.Linq.XDocument document = System.Xml.Linq.XDocument.Load(path);
                System.Xml.Linq.XElement element = document.Element("Language");

                if (element != null)
                {
                    ErrorOccurred = Processing.GetStringXml(element, "ErrorOccurred", ErrorOccurred);
                    Check = Processing.GetStringXml(element, "Check", Check);
                    LanguagesWindow.Ok = Processing.GetStringXml(element, "Ok", LanguagesWindow.Ok);
                    LanguagesWindow.Yes = Processing.GetStringXml(element, "Yes", LanguagesWindow.Yes);
                    LanguagesWindow.No = Processing.GetStringXml(element, "No", LanguagesWindow.No);
                    LanguagesWindow.Cancel = Processing.GetStringXml(element, "Cancel", LanguagesWindow.Cancel);
                    LanguagesWindow.SettingFileNotFound = Processing.GetStringXml(element, "SettingFileNotFound", LanguagesWindow.SettingFileNotFound);
                    LanguagesWindow.ShareSettings = Processing.GetStringXml(element, "ShareSettings", LanguagesWindow.ShareSettings);
                    LanguagesWindow.Open = Processing.GetStringXml(element, "Open", LanguagesWindow.Open);
                    LanguagesWindow.BatchProcessingOfEvent = Processing.GetStringXml(element, "BatchProcessingOfEvent", LanguagesWindow.BatchProcessingOfEvent);
                    LanguagesWindow.BatchProcessingOfTimer = Processing.GetStringXml(element, "BatchProcessingOfTimer", LanguagesWindow.BatchProcessingOfTimer);
                    LanguagesWindow.End = Processing.GetStringXml(element, "End", LanguagesWindow.End);
                    LanguagesWindow.UpdateCheckFailed = Processing.GetStringXml(element, "UpdateCheckFailed", LanguagesWindow.UpdateCheckFailed);
                    LanguagesWindow.LatestVersion = Processing.GetStringXml(element, "LatestVersion", LanguagesWindow.LatestVersion);
                    LanguagesWindow.ThereIsTheLatestVersion = Processing.GetStringXml(element, "ThereIsTheLatestVersion", LanguagesWindow.ThereIsTheLatestVersion);
                    LanguagesWindow.Event = Processing.GetStringXml(element, "Event", LanguagesWindow.Event);
                    LanguagesWindow.Timer = Processing.GetStringXml(element, "Timer", LanguagesWindow.Timer);
                    LanguagesWindow.WindowProcessing = Processing.GetStringXml(element, "WindowProcessing", LanguagesWindow.WindowProcessing);
                    LanguagesWindow.Magnet = Processing.GetStringXml(element, "Magnet", LanguagesWindow.Magnet);
                    LanguagesWindow.Setting = Processing.GetStringXml(element, "Setting", LanguagesWindow.Setting);
                    LanguagesWindow.Information = Processing.GetStringXml(element, "Information", LanguagesWindow.Information);
                    LanguagesWindow.Add = Processing.GetStringXml(element, "Add", LanguagesWindow.Add);
                    LanguagesWindow.Modify = Processing.GetStringXml(element, "Modify", LanguagesWindow.Modify);
                    LanguagesWindow.Delete = Processing.GetStringXml(element, "Delete", LanguagesWindow.Delete);
                    LanguagesWindow.Select = Processing.GetStringXml(element, "Select", LanguagesWindow.Select);
                    LanguagesWindow.Enabling = Processing.GetStringXml(element, "Enabling", LanguagesWindow.Enabling);
                    LanguagesWindow.Disabling = Processing.GetStringXml(element, "Disabling", LanguagesWindow.Disabling);
                    LanguagesWindow.MoveUp = Processing.GetStringXml(element, "MoveUp", LanguagesWindow.MoveUp);
                    LanguagesWindow.MoveDown = Processing.GetStringXml(element, "MoveDown", LanguagesWindow.MoveDown);
                    LanguagesWindow.Copy = Processing.GetStringXml(element, "Copy", LanguagesWindow.Copy);
                    LanguagesWindow.ProcessingState = Processing.GetStringXml(element, "ProcessingState", LanguagesWindow.ProcessingState);
                    LanguagesWindow.Deleted = Processing.GetStringXml(element, "Deleted", LanguagesWindow.Deleted);
                    LanguagesWindow.AllowDelete = Processing.GetStringXml(element, "AllowDelete", LanguagesWindow.AllowDelete);
                    LanguagesWindow.SoftwareName = Processing.GetStringXml(element, "SoftwareName", LanguagesWindow.SoftwareName);
                    LanguagesWindow.Version = Processing.GetStringXml(element, "Version", LanguagesWindow.Version);
                    LanguagesWindow.UpdateCheck = Processing.GetStringXml(element, "UpdateCheck", LanguagesWindow.UpdateCheck);
                    LanguagesWindow.Manual = Processing.GetStringXml(element, "Manual", LanguagesWindow.Manual);
                    LanguagesWindow.UpdateHistory = Processing.GetStringXml(element, "UpdateHistory", LanguagesWindow.UpdateHistory);
                    LanguagesWindow.Website = Processing.GetStringXml(element, "Website", LanguagesWindow.Website);
                    LanguagesWindow.ReportsAndRequests = Processing.GetStringXml(element, "ReportsAndRequests", LanguagesWindow.ReportsAndRequests);
                    LanguagesWindow.Library = Processing.GetStringXml(element, "Library", LanguagesWindow.Library);
                    LanguagesWindow.DisplayCoordinates = Processing.GetStringXml(element, "DisplayCoordinates", LanguagesWindow.DisplayCoordinates);
                    LanguagesWindow.GlobalCoordinates = Processing.GetStringXml(element, "GlobalCoordinates", LanguagesWindow.GlobalCoordinates);
                    LanguagesWindow.General = Processing.GetStringXml(element, "General", LanguagesWindow.General);
                    LanguagesWindow.Hotkey = Processing.GetStringXml(element, "Hotkey", LanguagesWindow.Hotkey);
                    LanguagesWindow.DisplayItem = Processing.GetStringXml(element, "DisplayItem", LanguagesWindow.DisplayItem);
                    LanguagesWindow.Language = Processing.GetStringXml(element, "Language", LanguagesWindow.Language);
                    LanguagesWindow.Translators = Processing.GetStringXml(element, "Translators", LanguagesWindow.Translators);
                    LanguagesWindow.AutomaticUpdateCheck = Processing.GetStringXml(element, "AutomaticUpdateCheck", LanguagesWindow.AutomaticUpdateCheck);
                    LanguagesWindow.CheckBetaVersion = Processing.GetStringXml(element, "CheckBetaVersion", LanguagesWindow.CheckBetaVersion);
                    LanguagesWindow.RegisterMultipleWindowActions = Processing.GetStringXml(element, "RegisterMultipleWindowActions", LanguagesWindow.RegisterMultipleWindowActions);
                    LanguagesWindow.CoordinateSpecificationMethod = Processing.GetStringXml(element, "CoordinateSpecificationMethod", LanguagesWindow.CoordinateSpecificationMethod);
                    LanguagesWindow.ProcessingInterval = Processing.GetStringXml(element, "ProcessingInterval", LanguagesWindow.ProcessingInterval);
                    LanguagesWindow.RunAtWindowsStartup = Processing.GetStringXml(element, "RunAtWindowsStartup", LanguagesWindow.RunAtWindowsStartup);
                    LanguagesWindow.RunAtWindowsStartupAdministrator = Processing.GetStringXml(element, "RunAtWindowsStartupAdministrator", LanguagesWindow.RunAtWindowsStartupAdministrator);
                    LanguagesWindow.NormalExecution = Processing.GetStringXml(element, "NormalExecution", LanguagesWindow.NormalExecution);
                    LanguagesWindow.WaitTimeToProcessingNextWindow = Processing.GetStringXml(element, "WaitTimeToProcessingNextWindow", LanguagesWindow.WaitTimeToProcessingNextWindow);
                    LanguagesWindow.StopProcessingWhenWindowIsFullScreen = Processing.GetStringXml(element, "StopProcessingWhenWindowIsFullScreen", LanguagesWindow.StopProcessingWhenWindowIsFullScreen);
                    LanguagesWindow.CaseSensitiveWindowQueries = Processing.GetStringXml(element, "CaseSensitiveWindowQueries", LanguagesWindow.CaseSensitiveWindowQueries);
                    LanguagesWindow.DoNotChangePositionSizeOutOfScreen = Processing.GetStringXml(element, "DoNotChangePositionSizeOutOfScreen", LanguagesWindow.DoNotChangePositionSizeOutOfScreen);
                    LanguagesWindow.StopProcessingShowAddModify = Processing.GetStringXml(element, "StopProcessingShowAddModify", LanguagesWindow.StopProcessingShowAddModify);
                    LanguagesWindow.HotkeysDoNotStop = Processing.GetStringXml(element, "HotkeysDoNotStop", LanguagesWindow.HotkeysDoNotStop);
                    LanguagesWindow.MoveThePastePosition = Processing.GetStringXml(element, "MoveThePastePosition", LanguagesWindow.MoveThePastePosition);
                    LanguagesWindow.PasteToTheEdgeOfScreen = Processing.GetStringXml(element, "PasteToTheEdgeOfScreen", LanguagesWindow.PasteToTheEdgeOfScreen);
                    LanguagesWindow.PasteIntoAnotherWindow = Processing.GetStringXml(element, "PasteIntoAnotherWindow", LanguagesWindow.PasteIntoAnotherWindow);
                    LanguagesWindow.PasteWithTheCtrlKeyPressed = Processing.GetStringXml(element, "PasteWithTheCtrlKeyPressed", LanguagesWindow.PasteWithTheCtrlKeyPressed);
                    LanguagesWindow.StopTimeWhenPasting = Processing.GetStringXml(element, "StopTimeWhenPasting", LanguagesWindow.StopTimeWhenPasting);
                    LanguagesWindow.DecisionDistanceToPaste = Processing.GetStringXml(element, "DecisionDistanceToPaste", LanguagesWindow.DecisionDistanceToPaste);
                    LanguagesWindow.AddItem = Processing.GetStringXml(element, "AddItem", LanguagesWindow.AddItem);
                    LanguagesWindow.ItemModification = Processing.GetStringXml(element, "ItemModification", LanguagesWindow.ItemModification);
                    LanguagesWindow.TitleName = Processing.GetStringXml(element, "TitleName", LanguagesWindow.TitleName);
                    LanguagesWindow.ClassName = Processing.GetStringXml(element, "ClassName", LanguagesWindow.ClassName);
                    LanguagesWindow.FileName = Processing.GetStringXml(element, "FileName", LanguagesWindow.FileName);
                    LanguagesWindow.Display = Processing.GetStringXml(element, "Display", LanguagesWindow.Display);
                    LanguagesWindow.WindowState = Processing.GetStringXml(element, "WindowState", LanguagesWindow.WindowState);
                    LanguagesWindow.ProcessingType = Processing.GetStringXml(element, "ProcessingType", LanguagesWindow.ProcessingType);
                    LanguagesWindow.SpecifyPositionAndSize = Processing.GetStringXml(element, "SpecifyPositionAndSize", LanguagesWindow.SpecifyPositionAndSize);
                    LanguagesWindow.PositionAndSize = Processing.GetStringXml(element, "PositionAndSize", LanguagesWindow.PositionAndSize);
                    LanguagesWindow.MoveXCoordinate = Processing.GetStringXml(element, "MoveXCoordinate", LanguagesWindow.MoveXCoordinate);
                    LanguagesWindow.MoveYCoordinate = Processing.GetStringXml(element, "MoveYCoordinate", LanguagesWindow.MoveYCoordinate);
                    LanguagesWindow.MoveXAndYCoordinate = Processing.GetStringXml(element, "MoveXAndYCoordinate", LanguagesWindow.MoveXAndYCoordinate);
                    LanguagesWindow.IncreaseDecreaseWidth = Processing.GetStringXml(element, "IncreaseDecreaseWidth", LanguagesWindow.IncreaseDecreaseWidth);
                    LanguagesWindow.IncreaseDecreaseHeight = Processing.GetStringXml(element, "IncreaseDecreaseHeight", LanguagesWindow.IncreaseDecreaseHeight);
                    LanguagesWindow.IncreaseDecreaseWidthAndHeight = Processing.GetStringXml(element, "IncreaseDecreaseWidthAndHeight", LanguagesWindow.IncreaseDecreaseWidthAndHeight);
                    LanguagesWindow.StartStopProcessingOfEvent = Processing.GetStringXml(element, "StartStopProcessingOfEvent", LanguagesWindow.StartStopProcessingOfEvent);
                    LanguagesWindow.StartStopProcessingOfTimer = Processing.GetStringXml(element, "StartStopProcessingOfTimer", LanguagesWindow.StartStopProcessingOfTimer);
                    LanguagesWindow.StartStopProcessingOfMagnet = Processing.GetStringXml(element, "StartStopProcessingOfMagnet", LanguagesWindow.StartStopProcessingOfMagnet);
                    LanguagesWindow.AlwaysShowOrCancelOnTop = Processing.GetStringXml(element, "AlwaysShowOrCancelOnTop", LanguagesWindow.AlwaysShowOrCancelOnTop);
                    LanguagesWindow.SpecifyCancelTransparency = Processing.GetStringXml(element, "SpecifyCancelTransparency", LanguagesWindow.SpecifyCancelTransparency);
                    LanguagesWindow.LeftEdge = Processing.GetStringXml(element, "LeftEdge", LanguagesWindow.LeftEdge);
                    LanguagesWindow.Middle = Processing.GetStringXml(element, "Middle", LanguagesWindow.Middle);
                    LanguagesWindow.RightEdge = Processing.GetStringXml(element, "RightEdge", LanguagesWindow.RightEdge);
                    LanguagesWindow.TopEdge = Processing.GetStringXml(element, "TopEdge", LanguagesWindow.TopEdge);
                    LanguagesWindow.BottomEdge = Processing.GetStringXml(element, "BottomEdge", LanguagesWindow.BottomEdge);
                    LanguagesWindow.X = Processing.GetStringXml(element, "X", LanguagesWindow.X);
                    LanguagesWindow.Y = Processing.GetStringXml(element, "Y", LanguagesWindow.Y);
                    LanguagesWindow.Width = Processing.GetStringXml(element, "Width", LanguagesWindow.Width);
                    LanguagesWindow.Height = Processing.GetStringXml(element, "Height", LanguagesWindow.Height);
                    LanguagesWindow.Pixel = Processing.GetStringXml(element, "Pixel", LanguagesWindow.Pixel);
                    LanguagesWindow.Percent = Processing.GetStringXml(element, "Percent", LanguagesWindow.Percent);
                    LanguagesWindow.RegisteredName = Processing.GetStringXml(element, "RegisteredName", LanguagesWindow.RegisteredName);
                    LanguagesWindow.WindowDecide = Processing.GetStringXml(element, "WindowDecide", LanguagesWindow.WindowDecide);
                    LanguagesWindow.ProcessingSettings = Processing.GetStringXml(element, "ProcessingSettings", LanguagesWindow.ProcessingSettings);
                    LanguagesWindow.HoldDownMouseCursorMoveToSelectWindow = Processing.GetStringXml(element, "HoldDownMouseCursorMoveToSelectWindow", LanguagesWindow.HoldDownMouseCursorMoveToSelectWindow);
                    LanguagesWindow.GetWindowInformation = Processing.GetStringXml(element, "GetWindowInformation", LanguagesWindow.GetWindowInformation);
                    LanguagesWindow.WindowInformationToGet = Processing.GetStringXml(element, "WindowInformationToGet", LanguagesWindow.WindowInformationToGet);
                    LanguagesWindow.FileSelection = Processing.GetStringXml(element, "FileSelection", LanguagesWindow.FileSelection);
                    LanguagesWindow.ExactMatch = Processing.GetStringXml(element, "ExactMatch", LanguagesWindow.ExactMatch);
                    LanguagesWindow.PartialMatch = Processing.GetStringXml(element, "PartialMatch", LanguagesWindow.PartialMatch);
                    LanguagesWindow.ForwardMatch = Processing.GetStringXml(element, "ForwardMatch", LanguagesWindow.ForwardMatch);
                    LanguagesWindow.BackwardMatch = Processing.GetStringXml(element, "BackwardMatch", LanguagesWindow.BackwardMatch);
                    LanguagesWindow.IncludePath = Processing.GetStringXml(element, "IncludePath", LanguagesWindow.IncludePath);
                    LanguagesWindow.DoNotIncludePath = Processing.GetStringXml(element, "DoNotIncludePath", LanguagesWindow.DoNotIncludePath);
                    LanguagesWindow.DoNotSpecify = Processing.GetStringXml(element, "DoNotSpecify", LanguagesWindow.DoNotSpecify);
                    LanguagesWindow.TitleProcessingConditions = Processing.GetStringXml(element, "TitleProcessingConditions", LanguagesWindow.TitleProcessingConditions);
                    LanguagesWindow.DoNotProcessingUntitledWindow = Processing.GetStringXml(element, "DoNotProcessingUntitledWindow", LanguagesWindow.DoNotProcessingUntitledWindow);
                    LanguagesWindow.DoNotProcessingWindowWithTitle = Processing.GetStringXml(element, "DoNotProcessingWindowWithTitle", LanguagesWindow.DoNotProcessingWindowWithTitle);
                    LanguagesWindow.DeleteItem = Processing.GetStringXml(element, "DeleteItem", LanguagesWindow.DeleteItem);
                    LanguagesWindow.Active = Processing.GetStringXml(element, "Active", LanguagesWindow.Active);
                    LanguagesWindow.Item = Processing.GetStringXml(element, "Item", LanguagesWindow.Item);
                    LanguagesWindow.Processing = Processing.GetStringXml(element, "Processing", LanguagesWindow.Processing);
                    LanguagesWindow.ProcessingName = Processing.GetStringXml(element, "ProcessingName", LanguagesWindow.ProcessingName);
                    LanguagesWindow.GetItemWindowInformationOnly = Processing.GetStringXml(element, "GetItemWindowInformationOnly", LanguagesWindow.GetItemWindowInformationOnly);
                    LanguagesWindow.Other = Processing.GetStringXml(element, "Other", LanguagesWindow.Other);
                    LanguagesWindow.DisplayToUseAsStandard = Processing.GetStringXml(element, "DisplayToUseAsStandard", LanguagesWindow.DisplayToUseAsStandard);
                    LanguagesWindow.TheSpecifiedDisplay = Processing.GetStringXml(element, "TheSpecifiedDisplay", LanguagesWindow.TheSpecifiedDisplay);
                    LanguagesWindow.OnlyIfItIsOnTheSpecifiedDisplay = Processing.GetStringXml(element, "OnlyIfItIsOnTheSpecifiedDisplay", LanguagesWindow.OnlyIfItIsOnTheSpecifiedDisplay);
                    LanguagesWindow.DisplayWithWindow = Processing.GetStringXml(element, "DisplayWithWindow", LanguagesWindow.DisplayWithWindow);
                    LanguagesWindow.ProcessOnlyOnce = Processing.GetStringXml(element, "ProcessOnlyOnce", LanguagesWindow.ProcessOnlyOnce);
                    LanguagesWindow.OnceWindowOpen = Processing.GetStringXml(element, "OnceWindowOpen", LanguagesWindow.OnceWindowOpen);
                    LanguagesWindow.OnceWhileItIsRunning = Processing.GetStringXml(element, "OnceWhileItIsRunning", LanguagesWindow.OnceWhileItIsRunning);
                    LanguagesWindow.WhenToProcessing = Processing.GetStringXml(element, "WhenToProcessing", LanguagesWindow.WhenToProcessing);
                    LanguagesWindow.TypesOfEventsToEnable = Processing.GetStringXml(element, "TypesOfEventsToEnable", LanguagesWindow.TypesOfEventsToEnable);
                    LanguagesWindow.TheForegroundHasBeenChanged = Processing.GetStringXml(element, "TheForegroundHasBeenChanged", LanguagesWindow.TheForegroundHasBeenChanged);
                    LanguagesWindow.MoveSizeChangeEnd = Processing.GetStringXml(element, "MoveSizeChangeEnd", LanguagesWindow.MoveSizeChangeEnd);
                    LanguagesWindow.MinimizeStart = Processing.GetStringXml(element, "MinimizeStart", LanguagesWindow.MinimizeStart);
                    LanguagesWindow.MinimizeEnd = Processing.GetStringXml(element, "MinimizeEnd", LanguagesWindow.MinimizeEnd);
                    LanguagesWindow.Create = Processing.GetStringXml(element, "Create", LanguagesWindow.Create);
                    LanguagesWindow.Show = Processing.GetStringXml(element, "Show", LanguagesWindow.Show);
                    LanguagesWindow.NameChanged = Processing.GetStringXml(element, "NameChanged", LanguagesWindow.NameChanged);
                    LanguagesWindow.Forefront = Processing.GetStringXml(element, "Forefront", LanguagesWindow.Forefront);
                    LanguagesWindow.DoNotChange = Processing.GetStringXml(element, "DoNotChange", LanguagesWindow.DoNotChange);
                    LanguagesWindow.AlwaysForefront = Processing.GetStringXml(element, "AlwaysForefront", LanguagesWindow.AlwaysForefront);
                    LanguagesWindow.AlwaysCancelForefront = Processing.GetStringXml(element, "AlwaysCancelForefront", LanguagesWindow.AlwaysCancelForefront);
                    LanguagesWindow.SpecifyTransparency = Processing.GetStringXml(element, "SpecifyTransparency", LanguagesWindow.SpecifyTransparency);
                    LanguagesWindow.CloseWindow = Processing.GetStringXml(element, "CloseWindow", LanguagesWindow.CloseWindow);
                    LanguagesWindow.NumberOfTimesNotToProcessingFirst = Processing.GetStringXml(element, "NumberOfTimesNotToProcessingFirst", LanguagesWindow.NumberOfTimesNotToProcessingFirst);
                    LanguagesWindow.NormalWindow = Processing.GetStringXml(element, "NormalWindow", LanguagesWindow.NormalWindow);
                    LanguagesWindow.Maximize = Processing.GetStringXml(element, "Maximize", LanguagesWindow.Maximize);
                    LanguagesWindow.Minimize = Processing.GetStringXml(element, "Minimize", LanguagesWindow.Minimize);
                    LanguagesWindow.ProcessOnlyWhenNormalWindow = Processing.GetStringXml(element, "ProcessOnlyWhenNormalWindow", LanguagesWindow.ProcessOnlyWhenNormalWindow);
                    LanguagesWindow.CoordinateSpecification = Processing.GetStringXml(element, "CoordinateSpecification", LanguagesWindow.CoordinateSpecification);
                    LanguagesWindow.WidthSpecification = Processing.GetStringXml(element, "WidthSpecification", LanguagesWindow.WidthSpecification);
                    LanguagesWindow.HeightSpecification = Processing.GetStringXml(element, "HeightSpecification", LanguagesWindow.HeightSpecification);
                    LanguagesWindow.ConditionsThatDoNotProcess = Processing.GetStringXml(element, "ConditionsThatDoNotProcess", LanguagesWindow.ConditionsThatDoNotProcess);
                    LanguagesWindow.ClientArea = Processing.GetStringXml(element, "ClientArea", LanguagesWindow.ClientArea);
                    LanguagesWindow.ProcessingPositionAndSizeTwice = Processing.GetStringXml(element, "ProcessingPositionAndSizeTwice", LanguagesWindow.ProcessingPositionAndSizeTwice);
                    LanguagesWindow.TitleNameExclusionString = Processing.GetStringXml(element, "TitleNameExclusionString", LanguagesWindow.TitleNameExclusionString);
                    LanguagesWindow.Size = Processing.GetStringXml(element, "Size", LanguagesWindow.Size);
                    LanguagesWindow.RetrievedAfterFiveSeconds = Processing.GetStringXml(element, "RetrievedAfterFiveSeconds", LanguagesWindow.RetrievedAfterFiveSeconds);
                    LanguagesWindow.SelectionComplete = Processing.GetStringXml(element, "SelectionComplete", LanguagesWindow.SelectionComplete);
                    LanguagesWindow.ThereAreDuplicateItems = Processing.GetStringXml(element, "ThereAreDuplicateItems", LanguagesWindow.ThereAreDuplicateItems);
                    LanguagesWindow.ThereIsADuplicateRegistrationName = Processing.GetStringXml(element, "ThereIsADuplicateRegistrationName", LanguagesWindow.ThereIsADuplicateRegistrationName);
                    LanguagesWindow.ThereIsADuplicateProcessingName = Processing.GetStringXml(element, "ThereIsADuplicateProcessingName", LanguagesWindow.ThereIsADuplicateProcessingName);
                    LanguagesWindow.Added = Processing.GetStringXml(element, "Added", LanguagesWindow.Added);
                    LanguagesWindow.Modified = Processing.GetStringXml(element, "Modified", LanguagesWindow.Modified);
                    LanguagesWindow.ProcessingWindowRange = Processing.GetStringXml(element, "ProcessingWindowRange", LanguagesWindow.ProcessingWindowRange);
                    LanguagesWindow.ActiveWindowOnly = Processing.GetStringXml(element, "ActiveWindowOnly", LanguagesWindow.ActiveWindowOnly);
                    LanguagesWindow.AllWindow = Processing.GetStringXml(element, "AllWindow", LanguagesWindow.AllWindow);
                    LanguagesWindow.OnlyActiveWindowEvent = Processing.GetStringXml(element, "OnlyActiveWindowEvent", LanguagesWindow.OnlyActiveWindowEvent);
                    LanguagesWindow.OnlyActiveWindowTimer = Processing.GetStringXml(element, "OnlyActiveWindowTimer", LanguagesWindow.OnlyActiveWindowTimer);
                    LanguagesWindow.AmountOfMovement = Processing.GetStringXml(element, "AmountOfMovement", LanguagesWindow.AmountOfMovement);
                    LanguagesWindow.SizeChangeAmount = Processing.GetStringXml(element, "SizeChangeAmount", LanguagesWindow.SizeChangeAmount);
                    LanguagesWindow.Transparency = Processing.GetStringXml(element, "Transparency", LanguagesWindow.Transparency);
                    LanguagesWindow.NumericCalculation = Processing.GetStringXml(element, "NumericCalculation", LanguagesWindow.NumericCalculation);
                    LanguagesWindow.Ratio = Processing.GetStringXml(element, "Ratio", LanguagesWindow.Ratio);
                    LanguagesWindow.ExplanationOfTheEvent = Processing.GetStringXml(element, "ExplanationOfTheEvent", LanguagesWindow.ExplanationOfTheEvent);
                    LanguagesWindow.ExplanationOfTheTimer = Processing.GetStringXml(element, "ExplanationOfTheTimer", LanguagesWindow.ExplanationOfTheTimer);
                    LanguagesWindow.HotkeyExplanation = Processing.GetStringXml(element, "HotkeyExplanation", LanguagesWindow.HotkeyExplanation);
                    LanguagesWindow.MagnetExplanation = Processing.GetStringXml(element, "MagnetExplanation", LanguagesWindow.MagnetExplanation);
                    LanguagesWindow.WindowMoveOutsideToInside = Processing.GetStringXml(element, "WindowMoveOutsideToInside", LanguagesWindow.WindowMoveOutsideToInside);
                    LanguagesWindow.MoveWindowToCenterOfScreen = Processing.GetStringXml(element, "MoveWindowToCenterOfScreen", LanguagesWindow.MoveWindowToCenterOfScreen);

                    result = true;
                }
            }
            catch
            {
            }

            return (result);
        }

        /// <summary>
        /// ファイルに書き込む (言語ファイル作成用)
        /// </summary>
        /// <param name="path">パス</param>
        public void Write(
            string path
            )
        {
            try
            {
                using System.IO.StreamWriter sw = new(path, false, System.Text.Encoding.UTF8);
                System.Xml.Serialization.XmlSerializer xml = new(typeof(Languages));
                xml.Serialize(sw, this);
            }
            catch
            {
            }
        }
    }
}
