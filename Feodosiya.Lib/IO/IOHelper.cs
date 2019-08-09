using System.Diagnostics;
using System.IO;
using System.Reflection;


namespace Feodosiya.Lib.IO {
    /// <summary>
    /// Предоставляет методы для работы с файлами
    /// </summary>
    public class IOHelper {
        /// <summary>
        /// Возвращает значение определяющее является заданный путь папкой или файлом
        /// </summary>
        /// <param name="path">Проверяемый путь</param>
        /// <exception cref="System.ArgumentException"></exception>
        /// <exception cref="System.IO.PathTooLongException"></exception>
        /// <exception cref="System.IO.FileNotFoundException"></exception>
        /// <exception cref="System.IO.DirectoryNotFoundException"></exception>
        /// <exception cref="System.NotSupportedException"></exception>
        /// <returns></returns>
        public static bool IsPathDirectory(string path) {
            FileAttributes attr = File.GetAttributes(path);

            return ((attr & FileAttributes.Directory) == FileAttributes.Directory);
        }

        /// <summary>
        /// Возвращает значение определяющее является ли заданный путь полным
        /// </summary>
        /// <param name="path">Проверяемый путь</param>
        /// <exception cref="System.ArgumentException"></exception>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <returns></returns>
        public static bool IsFullPath(string path) {
            if (string.IsNullOrWhiteSpace(path) || path.IndexOfAny(Path.GetInvalidPathChars()) != -1 || !Path.IsPathRooted(path))
                return false;

            var pathRoot = Path.GetPathRoot(path);
            if (pathRoot.Length <= 2 && pathRoot != "/")
                return false;

            return !(pathRoot == path && pathRoot.StartsWith("\\\\") && pathRoot.IndexOf('\\', 2) == -1);
        }

        /// <summary>
        /// Возвращает версию данной программы
        /// </summary>
        /// <param name="assembly">Сборка, версию которой необходимо получить</param>
        /// <exception cref="System.IO.FileNotFoundException"></exception>
        /// <returns></returns>
        public static string GetExeVersion(Assembly assembly) {
            FileVersionInfo fileInfo = FileVersionInfo.GetVersionInfo(assembly.Location);

            return fileInfo.FileVersion;
        }

        /// <summary>
        /// Возвращает путь к текущей рабочей папке
        /// </summary>
        /// <param name="assembly">Сборка, папку которой необходимо получить</param>
        /// <returns></returns>
        public static string GetCurrentDir(Assembly assembly) {
            return Path.GetDirectoryName(assembly.Location);
        }

        /// <summary>
        /// Проверяет, является ли заданный файл сборкой .NET
        /// </summary>
        /// <param name="fileName">Путь к файлу</param>
        /// <returns></returns>
        public static bool IsFileAssembly(string fileName) {
            if (Path.GetExtension(fileName) != ".dll")
                return false;
            try {
                AssemblyName.GetAssemblyName(fileName);
                return true;
            }
            catch {
                return false;
            }
        }
    }
}
