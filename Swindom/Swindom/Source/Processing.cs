namespace Swindom.Source
{
    /// <summary>
    /// 処理
    /// </summary>
    public class Processing
    {
        /// <summary>
        /// 自分自身のアプリケーションのパスを取得
        /// </summary>
        /// <returns>自分自身のアプリケーションのパス</returns>
        public static string OwnApplicationPath => System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;

        /// <summary>
        /// インストールされているか確認
        /// </summary>
        /// <returns>インストール状態 (インストールされていない「false」/インストールされている「true」)</returns>
        public static bool CheckInstalled()
        {
            bool result = false;        // インストール状態

            try
            {
                using Microsoft.Win32.RegistryKey registryKey = Microsoft.Win32.Registry.LocalMachine.OpenSubKey("SOFTWARE" + System.IO.Path.DirectorySeparatorChar + "Microsoft" + System.IO.Path.DirectorySeparatorChar + "Windows" + System.IO.Path.DirectorySeparatorChar + "CurrentVersion" + System.IO.Path.DirectorySeparatorChar + "Uninstall" + System.IO.Path.DirectorySeparatorChar + Common.RegistryKeyNameForInstallInformation);
                if (registryKey != null)
                {
                    result = true;
                }
            }
            catch
            {
            }

            return (result);
        }

        /// <summary>
        /// アプリケーションが存在するディレクトリのパスを取得
        /// </summary>
        /// <returns>アプリケーションが存在するパス</returns>
        public static string GetApplicationDirectoryPath()
        {
            string path = System.IO.Path.GetDirectoryName(OwnApplicationPath);
            // デバッグの場合はパスを変更
#if DEBUG
            path += System.IO.Path.DirectorySeparatorChar + ".." + System.IO.Path.DirectorySeparatorChar + ".." + System.IO.Path.DirectorySeparatorChar + ".." + System.IO.Path.DirectorySeparatorChar;
#endif
            return (path);
        }

        /// <summary>
        /// ComboBoxの項目を選択
        /// </summary>
        /// <param name="comboBox">ComboBox</param>
        /// <param name="stringData">文字列</param>
        public static void SelectComboBoxItem(
            System.Windows.Controls.ComboBox comboBox,
            string stringData
            )
        {
            foreach (System.Windows.Controls.ComboBoxItem nowItem in comboBox.Items)
            {
                if (stringData == (nowItem.Content as string))
                {
                    comboBox.SelectedItem = nowItem;
                    break;
                }
            }
        }

        /// <summary>
        /// XMLからstring型の値を取り出す
        /// </summary>
        /// <param name="element">XElement</param>
        /// <param name="name">名前</param>
        /// <param name="defaultValue">既定値</param>
        /// <returns>取り出した値</returns>
        public static string GetStringXml(
            System.Xml.Linq.XElement element,
            string name,
            string defaultValue
            )
        {
            string returnValue = defaultValue;        // 取り出した値

            try
            {
                System.Xml.Linq.XElement targetElement = element.Element(name);
                if (targetElement != null)
                {
                    returnValue = targetElement.Value;
                }
            }
            catch
            {
            }

            return (returnValue);
        }

        /// <summary>
        /// XMLからint型の値を取り出す
        /// </summary>
        /// <param name="element">XElement</param>
        /// <param name="name">名前</param>
        /// <param name="defaultValue">既定値</param>
        /// <returns>取り出した値</returns>
        public static int GetIntXml(
            System.Xml.Linq.XElement element,
            string name,
            int defaultValue
            )
        {
            int returnValue = defaultValue;        // 取り出した値

            try
            {
                System.Xml.Linq.XElement targetElement = element.Element(name);
                if (targetElement != null)
                {
                    returnValue = int.Parse(targetElement.Value);
                }
            }
            catch
            {
            }

            return (returnValue);
        }

        /// <summary>
        /// XMLからbool型の値を取り出す
        /// </summary>
        /// <param name="element">XElement</param>
        /// <param name="name">名前</param>
        /// <param name="defaultValue">既定値</param>
        /// <returns>取り出した値</returns>
        public static bool GetBoolXml(
            System.Xml.Linq.XElement element,
            string name,
            bool defaultValue
            )
        {
            bool returnValue = defaultValue;        // 取り出した値

            try
            {
                System.Xml.Linq.XElement targetElement = element.Element(name);
                if (targetElement != null)
                {
                    returnValue = bool.Parse(targetElement.Value);
                }
            }
            catch
            {
            }

            return (returnValue);
        }

        /// <summary>
        /// XMLからdouble型の値を取り出す
        /// </summary>
        /// <param name="element">XElement</param>
        /// <param name="name">名前</param>
        /// <param name="defaultValue">既定値</param>
        /// <returns>取り出した値</returns>
        public static double GetDoubleXml(
            System.Xml.Linq.XElement element,
            string name,
            double defaultValue
            )
        {
            double returnValue = defaultValue;        // 取り出した値

            try
            {
                System.Xml.Linq.XElement targetElement = element.Element(name);
                if (targetElement != null)
                {
                    returnValue = double.Parse(targetElement.Value);
                }
            }
            catch
            {
            }

            return (returnValue);
        }

        /// <summary>
        /// XMLからfloat型の値を取り出す
        /// </summary>
        /// <param name="element">XElement</param>
        /// <param name="name">名前</param>
        /// <param name="defaultValue">既定値</param>
        /// <returns>取り出した値</returns>
        public static float GetFloatXml(
            System.Xml.Linq.XElement element,
            string name,
            float defaultValue
            )
        {
            float returnValue = defaultValue;        // 取り出した値

            try
            {
                System.Xml.Linq.XElement targetElement = element.Element(name);
                if (targetElement != null)
                {
                    returnValue = float.Parse(targetElement.Value);
                }
            }
            catch
            {
            }

            return (returnValue);
        }

        /// <summary>
        /// XMLからdecimal型の値を取り出す
        /// </summary>
        /// <param name="element">XElement</param>
        /// <param name="name">名前</param>
        /// <param name="defaultValue">既定値</param>
        /// <returns>取り出した値</returns>
        public static decimal GetDecimalXml(
            System.Xml.Linq.XElement element,
            string name,
            decimal defaultValue
            )
        {
            decimal returnValue = defaultValue;        // 取り出した値

            try
            {
                System.Xml.Linq.XElement targetElement = element.Element(name);
                if (targetElement != null)
                {
                    returnValue = decimal.Parse(targetElement.Value);
                }
            }
            catch
            {
            }

            return (returnValue);
        }

        /// <summary>
        /// ホットキーを登録
        /// </summary>
        /// <param name="hwnd">メッセージを受け取るウィンドウハンドル</param>
        /// <param name="hotkeyInformation">ホットキー情報</param>
        /// <param name="id">ホットキーの識別子</param>
        /// <returns>登録結果 (失敗「false」/成功「true」)</returns>
        public static bool RegisterHotkey(
            System.IntPtr hwnd,
            FreeEcho.FEHotKeyWPF.HotKeyInformation hotkeyInformation,
            int id
            )
        {
            bool result = false;        // 結果
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
            if ((modKey != 0) || (hotkeyInformation.KeyCharacter != 0))
            {
                result = true;
                NativeMethods.RegisterHotKey(hwnd, id, modKey, (uint)hotkeyInformation.KeyCharacter);
            }

            return (result);
        }

        /// <summary>
        /// ファイルやURLを開く
        /// </summary>
        /// <param name="fileAndUrl">ファイルパス/URL</param>
        public static void OpenFileAndUrl(
            string fileAndUrl
            )
        {
            System.Diagnostics.ProcessStartInfo info = new();
            info.FileName = fileAndUrl;
            info.UseShellExecute = true;
            System.Diagnostics.Process.Start(info);
        }
    }
}
