using System;
using System.IO;
using System.IO.Compression;
using System.Text;


namespace Feodosiya.Lib.Logs {

    /// <summary>
    /// Представляет перечисление возможных типов лог-файла
    /// </summary>
    public enum LogTypes {
        /// <summary>
        /// Текстовый лог-файл
        /// </summary>
        Text,
        /// <summary>
        /// Лог-файл в памяти
        /// </summary>
        Memory
    }

    /// <summary>
    /// Представляет перечисление возможных типов выводимых сообщений
    /// </summary>
    public enum MessageType {
        /// <summary>
        /// Обычный
        /// </summary>
        None,
        /// <summary>
        /// Информация
        /// </summary>
        Information,
        /// <summary>
        /// Предупреждение
        /// </summary>
        Warning,
        /// <summary>
        /// Ошибка
        /// </summary>
        Error,
        /// <summary>
        /// Отладка
        /// </summary>
        Debug
    }

    /// <summary>
    /// Представляет методы для создания лог-файла и его последующей записи и сохранения
    /// </summary>
    public class Log {

        /// <summary>
        /// Представляет метод, обрабатывающий событие ExceptionThrownEvent
        /// <param name="exception">Ошибка инициатор события</param>
        /// </summary>
        public delegate void ExceptionEventHandler(Exception exception);
        /// <summary>
        /// Происходит при ошибке во время работы с лог-файлом
        /// </summary>
        public event ExceptionEventHandler ExceptionThrownEvent;


        internal string _logPath;
        internal StringBuilder _memory;
        internal bool _insertDateTime = false;
        internal string _dateFormat;
        internal LogTypes _logType;
        internal bool _autoCompress = false;
        internal const long _minLogSize = 1024; // 1 KB
        internal long _maxLogSize = 1024 * 500; // 500 KB
        internal const string EMPTY_STRING = "";
        internal bool _enableThrows = false;
        internal object _fileLock = new object();


        internal void _CheckType() {
            if (this._logType == LogTypes.Memory) {
                if (this._memory == null) {
                    this._memory = new StringBuilder();
                }
            }
        }

        internal string _StringFromMessageType(MessageType msgType) {
            string result = (msgType == MessageType.None) ? string.Empty : string.Format("[{0}]", Enum.GetName(typeof(MessageType), msgType));

            return result.ToUpperInvariant();
        }

        /// <summary>
        /// Инициализирует пустой экземпляр класса Log
        /// </summary>
        public Log() {
            this._logPath = String.Empty;
            this._dateFormat = String.Empty;
            this._logType = LogTypes.Text;
            this._memory = new StringBuilder();

            _CheckAutoCompress();
        }

        /// <summary>
        /// Инициализирует пустой экземпляр класса Log с заданным путем лог-файла
        /// </summary>
        /// <param name="logPath">Имя создаваемого лог-файла</param>
        public Log(string logPath) : this() {
            this._logPath = logPath;
        }

        /// <summary>
        /// Инициализирует пустой экземпляр класса Log с заданным путем и типом лог-файла
        /// </summary>
        /// <param name="logPath">Имя создаваемого лог-файла</param>
        /// <param name="logType">Тип создаваемого лог-файла</param>
        public Log(string logPath, LogTypes logType) : this(logPath) {
            this._logType = logType;
            _CheckType();
        }

        /// <summary>
        /// Возвращает или задает имя текущего лог-файла
        /// </summary>
        public string LogPath {
            get { return this._logPath; }
            set {
                this._logPath = value;
                _CheckType();
            }
        }

        /// <summary>
        /// Возвращает или задает значение, определяющее, будет ли при записи в лог-файл, в начале строки, выводиться текущая дата операции
        /// </summary>
        public bool InsertDate {
            get { return this._insertDateTime; }
            set {
                this._insertDateTime = value;
            }
        }

        /// <summary>
        /// Возвращает или задает значение, определяющее формат выводимой даты в начале строки. Для обращения к данному свойсту, значение свойства InsertDate  должно быть true
        /// </summary>
        /// <exception cref="System.InvalidOperationException"/>
        public string DateFormat {
            get {
                if (!this._insertDateTime) {
                    InvalidOperationException ex = new InvalidOperationException("Невозможно обратиться к свойству. Для данного действия необходимо установить значение свойства InsertDate в true.");
                    ExceptionThrownEvent?.Invoke(ex);
                    if (_enableThrows) {
                        throw ex;
                    }
                }
                return this._dateFormat;
            }
            set {
                if (!this._insertDateTime) {
                    InvalidOperationException ex = new InvalidOperationException("Невозможно обратиться к свойству. Для данного действия необходимо установить значение свойства InsertDate в true.");
                    ExceptionThrownEvent?.Invoke(ex);
                    if (_enableThrows) {
                        throw ex;
                    }
                }
                this._dateFormat = value;
            }
        }

        /// <summary>
        /// Возвращает тип текущего лог-файла
        /// </summary>
        public LogTypes LogType {
            get { return this._logType; }
        }

        /// <summary>
        /// Получает размер текущего лог-файла в байтах
        /// </summary>
        public long LogLength {
            get {
                try {
                    if (!File.Exists(LogPath)) {
                        return 0;
                    }

                    return new FileInfo(LogPath).Length;
                }
                catch (Exception ex) {
                    ExceptionThrownEvent?.Invoke(ex);
                    if (_enableThrows) {
                        throw;
                    }

                    return 0;
                }
            }
        }

        /// <summary>
        /// Возвращает или задает значение, определяющее будет-ли автоматически сжат лог-файл, после достижения его размера до значения заданного в параметре MaxLogSize
        /// </summary>
        public bool AutoCompress {
            get {
                return _autoCompress;
            }
            set {
                this._autoCompress = value;
            }
        }

        /// <summary>
        /// При установленном значении true параметра AutoCompress, определяет максимальный размер лог-файла в байтах, после превышения которого произойдет его архивация. Значение по умолчанию равняется 500 KB
        /// </summary>
        public long MaxLogLength {
            get {
                return _maxLogSize;
            }
            set {
                this._maxLogSize = System.Math.Max(_minLogSize, value);
            }
        }

        /// <summary>
        /// Определяет будет ли перехвачена ошибка возникшая во время работы с лог-файлом
        /// </summary>
        public bool EnableThrows {
            get {
                return _enableThrows;
            }
            set {
                _enableThrows = value;
            }
        }


        internal void _CheckAutoCompress() {
            try {
                if (AutoCompress && LogLength >= MaxLogLength) {
                    Compress(this.LogPath, string.Format("{0}_{1}.gz", this.LogPath, DateTime.Now.ToString("dd_MM_yyyy_HH_mm_ss")));
                    Delete();
                }
            }
            catch (Exception ex) {
                ExceptionThrownEvent?.Invoke(ex);
                if (_enableThrows) {
                    throw;
                }
            }
        }

        /// <summary>
        /// Записывает строку с указанным текстом в лог-файл
        /// </summary>
        /// <param name="text">Текст для записи</param>
        /// <param name="msgType">Тип выводимого сообщения</param>
        /// <exception cref="System.FormatException"/>
        /// <exception cref="System.ArgumentException"/>
        /// <exception cref="System.ArgumentNullException"/>
        /// <exception cref="System.IO.PathTooLongException"/>
        /// <exception cref="System.IO.DirectoryNotFoundException"/>
        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.UnauthorizedAccessException"/>
        /// <exception cref="System.IO.FileNotFoundException"/>
        /// <exception cref="System.NotSupportedException"/>
        /// <exception cref="System.Security.SecurityException"/>
        public void Write(string text, MessageType msgType = MessageType.None) {
            try {
                string date = (this._insertDateTime) ? "[" + DateTime.Now.ToString(this._dateFormat) + "]" : "";
                string result = String.Format("{0} {1} {2}{3}", date, _StringFromMessageType(msgType), text, "\r\n");

                if (this._logType == LogTypes.Text) {
                    lock (_fileLock) {
                        File.AppendAllText(this._logPath, result);
                    }
                    _CheckAutoCompress();
                }
                else if (this._logType == LogTypes.Memory) {
                    this._memory.Append(result);
                }
            }
            catch (Exception ex) {
                ExceptionThrownEvent?.Invoke(ex);
                if (_enableThrows) {
                    throw;
                }
            }
        }

        /// <summary>
        /// Записывает строку с указанным текстом в лог-файл
        /// </summary>
        /// <param name="text">Текст для записи</param>
        /// <param name="encoding">Кодировка записываемого текста</param>
        /// <param name="msgType">Тип выводимого сообщения</param>
        /// <exception cref="System.FormatException"/>
        /// <exception cref="System.ArgumentException"/>
        /// <exception cref="System.ArgumentNullException"/>
        /// <exception cref="System.IO.PathTooLongException"/>
        /// <exception cref="System.IO.DirectoryNotFoundException"/>
        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.UnauthorizedAccessException"/>
        /// <exception cref="System.IO.FileNotFoundException"/>
        /// <exception cref="System.NotSupportedException"/>
        /// <exception cref="System.Security.SecurityException"/>
        public void Write(string text, Encoding encoding, MessageType msgType = MessageType.None) {
            try {
                string date = (this._insertDateTime) ? "[" + DateTime.Now.ToString(this._dateFormat) + "]" : "";
                string result = String.Format("{0} {1} {2}{3}", date, _StringFromMessageType(msgType), text, "\r\n");

                if (this._logType == LogTypes.Text) {
                    lock (_fileLock) {
                        File.AppendAllText(this._logPath, result, encoding);
                    }
                    _CheckAutoCompress();
                }
                else if (this._logType == LogTypes.Memory) {
                    this._memory.Append(result);
                }
            }
            catch (Exception ex) {
                ExceptionThrownEvent?.Invoke(ex);
                if (_enableThrows) {
                    throw;
                }
            }
        }

        /// <summary>
        /// Сохраняет лог-файл из памяти на накопитель информации
        /// </summary>
        /// <exception cref="System.InvalidOperationException"/>
        /// <exception cref="System.ArgumentNullException"/>
        public void SaveFromMemory() {
            try {
                if (this._logType != LogTypes.Memory) {
                    throw new InvalidOperationException("Для выполнения данной операции лог-файл должен иметь тип LogTypes.Memory");
                }
                if (String.IsNullOrEmpty(this._logPath)) {
                    throw new ArgumentNullException("Не задан путь для сохранения лог-файла");
                }

                lock (_fileLock) {
                    File.AppendAllText(this._logPath, this._memory.ToString());
                }
                _CheckAutoCompress();
            }
            catch (Exception ex) {
                ExceptionThrownEvent?.Invoke(ex);
                if (_enableThrows) {
                    throw;
                }
            }
        }

        /// <summary>
        /// Сохраняет лог-файл из памяти на накопитель информации
        /// </summary>
        /// <param name="encoding">Кодировка записываемого текста</param>
        /// <exception cref="System.InvalidOperationException"/>
        /// <exception cref="System.ArgumentNullException"/>
        public void SaveFromMemory(Encoding encoding) {
            try {
                if (this._logType != LogTypes.Memory) {
                    throw new InvalidOperationException("Для выполнения данной операции лог-файл должен иметь тип LogTypes.Memory");
                }
                if (String.IsNullOrEmpty(this._logPath)) {
                    throw new ArgumentNullException("Не задан путь для сохранения лог-файла");
                }

                lock (_fileLock) {
                    File.AppendAllText(this._logPath, this._memory.ToString(), encoding);
                }
                _CheckAutoCompress();
            }
            catch (Exception ex) {
                ExceptionThrownEvent?.Invoke(ex);
                if (_enableThrows) {
                    throw;
                }
            }
        }


        /// <summary>
        /// Помещает указанный текстовый файл-лог в архив и сохраняет его по заданному пути. Если файл архива с таким именем уже существует, то он будет перезаписан
        /// </summary>
        /// <param name="logPath">Путь к исходному лог-файлу</param>
        /// <param name="gzName">Имя создаваемого сжатого лог-файла</param>
        /// <exception cref="System.ArgumentException"/>
        /// <exception cref="System.ArgumentNullException"/>
        /// <exception cref="System.IO.PathTooLongException"/>
        /// <exception cref="System.IO.DirectoryNotFoundException"/>
        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.IO.FileNotFoundException"/>
        /// <exception cref="System.UnauthorizedAccessException"/>
        /// <exception cref="System.NotSupportedException"/>
        /// <exception cref="System.Security.SecurityException"/>
        /// <exception cref="System.Text.EncoderFallbackException"/>
        /// <exception cref="System.InvalidOperationException"/>
        /// <exception cref="System.ObjectDisposedException"/>
        public static void Compress(string logPath, string gzName = EMPTY_STRING) {
            byte[] data = Encoding.Default.GetBytes(File.ReadAllText(logPath));

            using (MemoryStream memory = new MemoryStream()) {
                using (GZipStream gzStream = new GZipStream(memory, CompressionMode.Compress, true)) {
                    gzStream.Write(data, 0, data.Length);
                }

                if (String.IsNullOrEmpty(gzName)) {
                    gzName = logPath + ".gz";
                }
                File.WriteAllBytes(gzName, memory.ToArray());
            }
        }

        /// <summary>
        /// Удаляет лог-файл с накопителя информации либо из памяти
        /// </summary>
        /// <exception cref="System.ArgumentException"/>
        /// <exception cref="System.ArgumentNullException"/>
        /// <exception cref="System.IO.DirectoryNotFoundException"/>
        /// <exception cref="System.IO.IOException"/>
        /// <exception cref="System.IO.PathTooLongException"/>
        /// <exception cref="System.UnauthorizedAccessException"/>
        /// <exception cref="System.NotSupportedException"/>
        public void Delete() {
            try {
                if (this._logType == LogTypes.Text) {
                    if (String.IsNullOrEmpty(this._logPath)) {
                        throw new ArgumentNullException("Не задан путь расположения лог-файла");
                    }
                    if (File.Exists(this._logPath)) {
                        lock (_fileLock) {
                            File.Delete(this._logPath);
                        }
                    }
                }
                else {
                    this._memory.Clear();
                }
            }
            catch (Exception ex) {
                ExceptionThrownEvent?.Invoke(ex);
                if (_enableThrows) {
                    throw;
                }
            }
        }
    }
}
