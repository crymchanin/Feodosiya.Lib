using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Feodosiya.Lib.Logs {
    /// <summary>
    /// Класс предоставляющий методы и свойства для работы с логами и отладкой программы
    /// </summary>
    public static class DebugHelper {
        #region Приватные члены

        #region Поля
        private static DebugLevels _debugLevel = DebugLevels.None;
        #endregion

        #endregion

        #region Перечисления
        /// <summary>
        /// Представляет типы уровней логирования 
        /// </summary>
        [Flags]
        public enum DebugLevels : byte {
            /// <summary>
            /// Без логирования
            /// </summary>
            None,
            /// <summary>
            /// Уровень логирования - отладка
            /// </summary>
            Debug = 1,
            /// <summary>
            /// Уровень логирования - информация
            /// </summary>
            Information = 2,
            /// <summary>
            /// Уровень логирования - предупреждение
            /// </summary>
            Warning = 4,
            /// <summary>
            /// Уровень логирования - ошибка
            /// </summary>
            Error = 8,
            /// <summary>
            /// Уровень логирования - критическая ошибка
            /// </summary>
            Fatal = 16
        }
        #endregion

        #region Свойства
        /// <summary>
        /// Возвращает или задает текущий уровень логирования
        /// </summary>
        public static DebugLevels DebugLevel {
            get {
                return _debugLevel;
            }
            set {
                _debugLevel = value;
            }
        }
        #endregion


        /// <summary>
        /// Выводит в консоль строку отформатированную в соответствии с уровнем логирования
        /// </summary>
        /// <param name="text">Выводимая строка</param>
        /// <param name="level">Уровень логирования</param>
        public static void ConsoleWriteLine(string text, DebugLevels level) {
            if (level == DebugLevels.None) {
                return;
            }
            int lvl = (int)level;
            if (lvl != 1 && (lvl % 2) != 0) {
                throw new FormatException("Неверно задан параметр определяющий уровень логирования");
            }

            if (DebugLevel.HasFlag(level)) {
                string fmt = string.Format("[{0}] {1}", Enum.GetName(typeof(DebugLevels), level), text);
                Console.WriteLine(fmt);
            }
        }
    }
}
