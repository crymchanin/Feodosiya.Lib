using System;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace Feodosiya.Lib.Conf {
    /// <summary>
    /// Предоставляет методы для работы с файлом конфигурации в формате JSON
    /// </summary>
    public class ConfHelper {

        internal bool _isSuccess;
        internal string _confPath;
        internal Exception _lastError;

        /// <summary>
        /// Инициализирует пустой экземпляр класса Feodosiya.Lib.Conf.ConfHelper с заданным путем конфигурационного файла
        /// </summary>
        /// <param name="confPath">Путь к конфигурационному файлу</param>
        public ConfHelper(string confPath) {
            _isSuccess = false;
            _confPath = confPath;
            _lastError = null;
        }

        /// <summary>
        /// Загружает файл конфигурации и десериализует его содержимое
        /// </summary>
        /// <returns></returns>
        public T LoadConfig<T>() {
            try {
                using (MemoryStream ms = new MemoryStream(File.ReadAllBytes(_confPath))) {
                    DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));

                    T result = (T)ser.ReadObject(ms);
                    _isSuccess = true;

                    return result;
                }
            }
            catch (Exception error) {
                _isSuccess = false;
                System.Diagnostics.Debug.WriteLine(error.ToString());
                _lastError = error;

                return default(T);
            }
        }

        /// <summary>
        /// Выполняет сериализацию объекта конфигурации и сохраняет его содержимое в файл конфигурации
        /// </summary>
        /// <param name="configuration">Объект конфигурации</param>
        /// <returns></returns>
        public void SaveConfig(object configuration) {
            try {
                using (MemoryStream ms = new MemoryStream()) {
                    DataContractJsonSerializer ser = new DataContractJsonSerializer(configuration.GetType());

                    ser.WriteObject(ms, configuration);
                    File.WriteAllText(_confPath, Encoding.Default.GetString(ms.ToArray()));
                }
                _isSuccess = true;
            }
            catch (Exception error) {
                System.Diagnostics.Debug.WriteLine(error.ToString());
                _isSuccess = false;
                _lastError = error;
            }
        }

        /// <summary>
        /// Выполняет сериализацию объекта конфигурации и сохраняет его содержимое в файл конфигурации
        /// </summary>
        /// <param name="configuration">Объект конфигурации</param>
        /// <param name="encoding">Кодировка в которой будет сохранен файл конфигурации</param>
        /// <returns></returns>
        public void SaveConfig(object configuration, Encoding encoding) {
            try {
                using (MemoryStream ms = new MemoryStream()) {
                    DataContractJsonSerializer ser = new DataContractJsonSerializer(configuration.GetType());

                    ser.WriteObject(ms, configuration);
                    File.WriteAllText(_confPath, encoding.GetString(ms.ToArray()));
                }
                _isSuccess = true;
            }
            catch (Exception error) {
                System.Diagnostics.Debug.WriteLine(error.ToString());
                _isSuccess = false;
                _lastError = error;
            }
        }

        /// <summary>
        /// Получает значение, указывающее на то, успешно ли загружен файл конфигурации 
        /// </summary>
        public bool Success {
            get {
                return _isSuccess;
            }
        }

        /// <summary>
        /// Возвращает ошибку возникшую в ходе загрузки файла конфигурации
        /// </summary>
        public Exception LastError {
            get {
                return _lastError;
            }
        }
    }
}
