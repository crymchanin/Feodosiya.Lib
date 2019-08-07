using System;
using Microsoft.Win32;
using System.IO;
using System.Threading;

namespace Feodosiya.Lib.App {
    /// <summary>
    /// 
    /// </summary>
    public static class AppHelper {
        /// <summary>
        /// Добавляет указанную программу в автозагрузку
        /// </summary>
        /// <param name="pathToApp">Путь к программе добавляемой в автозагрузку</param>
        public static void AddToAutorun(string pathToApp) {
            RegistryView regView = (Environment.Is64BitOperatingSystem) ? RegistryView.Registry64 : RegistryView.Registry32;
            using (RegistryKey key = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, regView).OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true)) {
                string appName = Path.GetFileName(pathToApp);
                key.SetValue(appName, pathToApp, RegistryValueKind.String);
            }
        }

        /// <summary>
        /// Удаляет указанную программу из автозагрузки
        /// </summary>
        /// <param name="pathToApp">Путь к программе удаляемой из автозагрузки</param>
        public static void RemoveFromAutorun(string pathToApp) {
            RegistryView regView = (Environment.Is64BitOperatingSystem) ? RegistryView.Registry64 : RegistryView.Registry32;
            using (RegistryKey key = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, regView).OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true)) {
                string appName = Path.GetFileName(pathToApp);
                key.DeleteValue(appName);
            }
        }

        /// <summary>
        /// Сопоставляет указанное расширение файла с указанной программой
        /// </summary>
        /// <param name="pathToApp">Путь к программе</param>
        /// <param name="extension">Расширение файла (например .txt)</param>
        public static void AssociateExtensionWithApp(string pathToApp, string extension) {
            using (RegistryKey key = Registry.CurrentUser.OpenSubKey("Software\\Classes", true)) {
                if (key != null) {
                    key.CreateSubKey("." + extension).SetValue(string.Empty, extension + "_auto_file");

                    using (RegistryKey auto_file = key.CreateSubKey(extension + "_auto_file")) {
                        auto_file.CreateSubKey("DefaultIcon").SetValue(string.Empty, pathToApp + ",0");

                        using (RegistryKey shell = auto_file.CreateSubKey("Shell")) {
                            shell.SetValue(string.Empty, "Open");

                            using (RegistryKey open = shell.CreateSubKey("Open")) {
                                open.CreateSubKey("Command").SetValue(string.Empty, "\"" + pathToApp + "\" \"%1\"");
                                open.CreateSubKey("ddeexec\\Topic").SetValue(string.Empty, "System");
                            }
                        }   
                    }
                }
            }
        }
    }
}
