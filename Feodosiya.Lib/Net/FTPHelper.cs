using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using Feodosiya.Lib.Logs;
using System.Reflection;

namespace Feodosiya.Lib.Net {
    /// <summary>
    /// 
    /// </summary>
    public class FTPHelper {

        #region Приватные члены

        #region Поля
        private string _host;
        private string _rootDirectory;
        private NetworkCredential _credentials;
        private int _timeout;
        #endregion

        #endregion

        #region Свойства

        /// <summary>
        /// Возвращает или задает IP адрес или доменное имя сервера FTP, к которому будет осуществляться подключение
        /// </summary>
        public string Host {
            get {
                return _host;
            }
            set {
                _host = "ftp://" + value;
            }
        }

        /// <summary>
        /// Возвращает или задает удаленный корневой каталог
        /// </summary>
        public string RootDirectory {
            get {
                return _rootDirectory;
            }
            set {
                _rootDirectory = value;
            }
        }

        /// <summary>
        /// Возвращает или задает учетные данные для подключения к серверу FTP
        /// </summary>
        public NetworkCredential Credentials {
            get {
                return _credentials;
            }
            set {
                _credentials = value;
            }
        }

        /// <summary>
        /// Возвращает или задает промежуток времени в миллисекундах до истечения срока действия запроса
        /// </summary>
        public int Timeout {
            get {
                return _timeout;
            }
            set {
                _timeout = value;
            }
        }

        #endregion


        /// <summary>
        /// Инициализирует новый экземпляр класса FTPHelper
        /// </summary>
        public FTPHelper() {
            Timeout = System.Threading.Timeout.Infinite;
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса FTPHelper с указанными параметрами
        /// </summary>
        /// <param name="host">IP адрес или доменное имя сервера FTP, к которому будет осуществляться подключение</param>
        /// <param name="rootDirectory">Удаленный корневой  каталог</param>
        /// <param name="credentials">Учетные данные для подключения к серверу FTP</param>
        public FTPHelper(string host, string rootDirectory, NetworkCredential credentials) : this() {
            Host = host;
            RootDirectory = rootDirectory;
            Credentials = credentials;
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса FTPHelper с указанными параметрами
        /// </summary>
        /// <param name="host">IP адрес или доменное имя сервера FTP, к которому будет осуществляться подключение</param>
        /// <param name="rootDirectory">Удаленный корневой  каталог</param>
        /// <param name="credentials">Учетные данные для подключения к серверу FTP</param>
        /// <param name="timeout">Промежуток времени в миллисекундах до истечения срока действия запроса</param>
        public FTPHelper(string host, string rootDirectory, NetworkCredential credentials, int timeout) :
            this(host, rootDirectory, credentials) {
            Timeout = timeout;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="directory"></param>
        /// <returns></returns>
        public IEnumerable<string> GetDirectoryList(string directory) {
            //Assembly net = Assembly.Load("System, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089");
            //Type designHost = net.GetType("System.Net.FtpDataStream");
            

            Uri host = new Uri(Host);
            Uri uri = new Uri(host, RootDirectory);
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create(new Uri(uri, directory));
            request.Method = WebRequestMethods.Ftp.ListDirectory;
            request.Credentials = Credentials;

            using (FtpWebResponse resp = (FtpWebResponse)request.GetResponse()) {
                if (resp.StatusCode != FtpStatusCode.OpeningData) {
                    string message = string.Format("Ошибка при получении информации о директории на FTP\nХост: {0}\nДиректория: {1}\nКод состояния: {2}\nТекст состояния: {3}",
                            host.Host, directory, resp.StatusCode, resp.StatusDescription);
                    throw new Exception(message);
                }

                using (StreamReader reader = new StreamReader(resp.GetResponseStream())) {
                    string line = string.Empty;
                    do {
                        line = reader.ReadLine();
                        yield return line;
                    }
                    while (!string.IsNullOrEmpty(line));
                }
            }
        }

        /// <summary>
        /// Выгружает заданный файл на FTP сервер
        /// </summary>
        /// <param name="file">Файл подлежащий выгрузке на сервер</param>
        /// <param name="destDirectory">Удаленный каталог, в который будет выгружен файл. Путь указывается относительно корневого каталога</param>
        public void UploadFile(string file, string destDirectory) {
            Uri host = new Uri(Host);
            Uri uri = new Uri(host, RootDirectory);
            WebRequest request = WebRequest.Create(new Uri(uri, destDirectory));
            request.Method = WebRequestMethods.Ftp.UploadFile;
            request.Credentials = Credentials;
            request.Timeout = System.Threading.Timeout.Infinite;

            byte[] data = File.ReadAllBytes(file);
            using (Stream stream = request.GetRequestStream()) {
                stream.Write(data, 0, data.Length);
            }
            using (FtpWebResponse resp = (FtpWebResponse)request.GetResponse()) {
                if (resp.StatusCode != FtpStatusCode.ClosingData) {
                    string message = string.Format("Ошибка при выгрузке файла на FTP\nХост: {0}\nФайл: {1}\nКод состояния: {2}\nТекст состояния: {3}",
                            host.Host, file, resp.StatusCode, resp.StatusDescription);
                    throw new Exception(message);
                }
            }

            DebugHelper.ConsoleWriteLine("Файл " + file + " выгружен на сервер " + host.Host, DebugHelper.DebugLevels.Information);
        }

        /// <summary>
        /// Загружает заданный файл с FTP сервера
        /// </summary>
        /// <param name="file">Удаленный файл. Путь указывается относительно корневого каталога</param>
        /// <param name="destDirectory">Локальный каталог, в который будет выгружен файл.</param>
        public void DownloadFile(string file, string destDirectory) {
            Uri host = new Uri(Host);
            Uri uri = new Uri(host, RootDirectory);
            WebRequest request = WebRequest.Create(new Uri(uri, file));
            request.Method = WebRequestMethods.Ftp.DownloadFile;
            request.Credentials = Credentials;

            using (FtpWebResponse resp = (FtpWebResponse)request.GetResponse()) {
                if (resp.StatusCode != FtpStatusCode.OpeningData) {
                    string message = string.Format("Ошибка при загрузке файла с FTP\nХост: {0}\nФайл: {1}\nКод состояния: {2}\nТекст состояния: {3}",
                            host.Host, file, resp.StatusCode, resp.StatusDescription);
                    throw new Exception(message);
                }
                using (Stream fileStream = File.Create(file)) {
                    using (Stream respStream = resp.GetResponseStream()) {
                        respStream.CopyTo(fileStream);
                    }
                }
            }
        }

        /// <summary>
        /// Удаляет заданный файл на FTP сервере
        /// </summary>
        /// <param name="file">Удаленный файл. Путь указывается относительно корневого каталога</param>
        public void DeleteFile(string file) {
            Uri host = new Uri(Host);
            Uri uri = new Uri(host, RootDirectory);
            WebRequest request = WebRequest.Create(new Uri(uri, file));
            request.Method = WebRequestMethods.Ftp.DeleteFile;
            request.Credentials = Credentials;

            using (FtpWebResponse resp = (FtpWebResponse)request.GetResponse()) {
                if (resp.StatusCode != FtpStatusCode.FileActionOK) {
                    string message = string.Format("Ошибка при удалении файла на FTP\nХост: {0}\nФайл: {1}\nКод состояния: {2}\nТекст состояния: {3}",
                            host.Host, file, resp.StatusCode, resp.StatusDescription);
                        DebugHelper.ConsoleWriteLine(message, DebugHelper.DebugLevels.Error);
                        throw new Exception(message);
                }

                DebugHelper.ConsoleWriteLine("Файл " + file + " успешно удален", DebugHelper.DebugLevels.Information);
            }
        }
    }
}
