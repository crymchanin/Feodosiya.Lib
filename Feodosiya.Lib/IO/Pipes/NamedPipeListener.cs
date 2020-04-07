using System;
using System.IO.Pipes;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.AccessControl;
using System.Security.Principal;


namespace Feodosiya.Lib.IO.Pipes {

    /// <summary>Содержит данные о событии для <see cref="NamedPipeMessageReceivedHandler{T}" /> событий</summary>
    /// <typeparam name="T"></typeparam>
    public class NamedPipeListenerMessageReceivedEventArgs<T> : EventArgs {

        /// <summary>Инициализирует экземпляр <see cref="NamedPipeListenerMessageReceivedEventArgs{T}" /> с указанным <paramref name="message" />.</summary>
        /// <param name="message">Сообщение, переданное событием</param>
        public NamedPipeListenerMessageReceivedEventArgs(T message) {
            Message = message;
        }

        /// <summary>Получает сообщение, переданное событием</summary>
        public T Message { get; private set; }
    }

    /// <summary>Содержит данные о событии для <see cref="NamedPipeMessageErrorHandler" /> событий</summary>
    public class NamedPipeListenerErrorEventArgs : EventArgs {
        /// <summary>Инициализирует экземпляр <see cref="NamedPipeListenerErrorEventArgs" /> с указанным <paramref name="errorType" /> и <paramref name="ex" />.</summary>
        /// <param name="errorType">Тип ошибки <see cref="NamedPipeListenerErrorType" />, который описывает часть процесса прослушивания, где была обнаружена ошибка</param>
        /// <param name="ex">Исключение <see cref="Exception" /> которое было выброшено</param>
        public NamedPipeListenerErrorEventArgs(NamedPipeListenerErrorType errorType, Exception ex) {
            ErrorType = errorType;
            Exception = ex;
        }

        /// <summary>Получает тип ошибки <see cref="NamedPipeListenerErrorType" />, который описывает часть процесса прослушивания, где была обнаружена ошибка</summary>
        public NamedPipeListenerErrorType ErrorType { get; private set; }

        /// <summary>Получает <see cref="Exception" /> которое было перехвачено</summary>
        public Exception Exception { get; private set; }
    }

    /// <summary>Представляет метод, который будет обрабатывать событие, когда сообщение получено через именованные каналы</summary>
    /// <typeparam name="T">Тип сообщения, которое было получено</typeparam>
    /// <param name="sender">Источник события</param>
    /// <param name="e">Данные о событии, переданные событием, в том числе полученное сообщение</param>
    public delegate void NamedPipeMessageReceivedHandler<T>(object sender, NamedPipeListenerMessageReceivedEventArgs<T> e);

    /// <summary>Представляет метод, который будет обрабатывать событие, которое запускается при обнаружении исключения</summary>
    /// <param name="sender">Источник события</param>
    /// <param name="e">Данные события, переданные событием, включали тип ошибки и исключение, которое было перехвачено</param>
    public delegate void NamedPipeMessageErrorHandler(Object sender, NamedPipeListenerErrorEventArgs e);


    /// <summary>Включает в себя различные типы ошибок, которые описывают, где в процессе прослушивания было обнаружено исключение</summary>
    public enum NamedPipeListenerErrorType : byte {
        /// <summary>Указывает на то, что исключение было обнаружено во время вызова <see cref="NamedPipeServerStream.BeginWaitForConnection" />.</summary>
        BeginWaitForConnection = 1,

        /// <summary>Указывает на то, что исключение было обнаружено во время вызова <see cref="NamedPipeServerStream.EndWaitForConnection" />.</summary>
        EndWaitForConnection = 2,

        /// <summary>Указывает на то, что исключение было обнаружено во время десериализации сообщения, полученного из именованного канала</summary>
        DeserializeMessage = 3,

        /// <summary>Указывает на то, что исключение было обнаружено во время закрытия или освобождения ресурсов использованного именованного канала</summary>
        CloseAndDisposePipe = 4,

        /// <summary>Указывает на то, что исключение было обнаружено во время вызова <see cref="NamedPipeListener{TMessage}.MessageReceived"/> события</summary>
        NotifyMessageReceived = 5
    }


    /// <summary>Вспомогательный класс для отправки и получения сообщений с использованием именованных каналов</summary>
    /// <typeparam name="T">Тип сообщения, которое будет отправлено или получено</typeparam>
    public class NamedPipeListener<T> : IDisposable {

        /// <summary>Происходит при получении сообщения</summary>
        public event NamedPipeMessageReceivedHandler<T> MessageReceived;

        /// <summary>Происходит при обнаружении исключения</summary>
        public event NamedPipeMessageErrorHandler Error;

        private static readonly string DEFAULT_PIPENAME = typeof(NamedPipeListener<T>).FullName;
        private static readonly BinaryFormatter formatter = new BinaryFormatter();

        NamedPipeServerStream pipeServer;


        /// <summary>Инициализирует новый экземпляр <see cref="NamedPipeListener{T}" /> используя указанное <paramref name="pipeName" /></summary>
        /// <param name="pipeName">Имя именованного канала, которое будет использоваться для прослушивания</param>
        /// <param name="sidType">Тип идентификатора безопасности (SID)</param>
        public NamedPipeListener(string pipeName, WellKnownSidType sidType) : this(pipeName) {
            SidType = sidType;
        }

        /// <summary>Инициализирует новый экземпляр <see cref="NamedPipeListener{T}" /> используя указанное <paramref name="pipeName" /></summary>
        /// <param name="pipeName">Имя именованного канала, которое будет использоваться для прослушивания</param>
        public NamedPipeListener(string pipeName) {
            PipeName = pipeName;
        }

        /// <summary>Инициализирует новый экземпляр <see cref="NamedPipeListener{T}" /> используя имя канала по умолчанию</summary>
        /// <remarks>Имя канала по умолчанию - это полное имя типа экземпляра</remarks>
        public NamedPipeListener()
            : this(DEFAULT_PIPENAME) { }

        /// <summary>Имя именованного канала, который будет использоваться для прослушивания</summary>
        public string PipeName { get; private set; }

        /// <summary>
        /// Тип идентификатора безопасности (SID)
        /// </summary>
        public WellKnownSidType SidType { get; private set; } = WellKnownSidType.NullSid;

        private PipeSecurity GetPipeSecurityObject() {
            if (SidType == WellKnownSidType.NullSid) {
                return null;
            }

            SecurityIdentifier sid = new SecurityIdentifier(SidType, null);
            PipeAccessRule pipeAccessRule = new PipeAccessRule(sid, PipeAccessRights.ReadWrite, AccessControlType.Allow);
            PipeSecurity ps = new PipeSecurity();
            ps.AddAccessRule(pipeAccessRule);

            return ps;
        }

        /// <summary>Начинает прослушивать именованный канал для указанного экземпляра</summary>
        public void Start() {
            if (pipeServer == null) {
                pipeServer = new NamedPipeServerStream(PipeName, PipeDirection.In, 1, PipeTransmissionMode.Message, PipeOptions.Asynchronous, 0, 0, GetPipeSecurityObject());
            }

            try {
                pipeServer.BeginWaitForConnection(new AsyncCallback(PipeConnectionCallback), null);
            }
            catch (Exception ex) {
                OnError(NamedPipeListenerErrorType.BeginWaitForConnection, ex);
            }
        }

        private void PipeConnectionCallback(IAsyncResult result) {
            try {
                pipeServer.EndWaitForConnection(result);
            }
            catch (Exception ex) {
                OnError(NamedPipeListenerErrorType.EndWaitForConnection, ex);
                return;
            }

            T message;
            try {
                message = (T)formatter.Deserialize(pipeServer);
            }
            catch (Exception ex) {
                OnError(NamedPipeListenerErrorType.DeserializeMessage, ex);
                return;
            }

            try {
                OnMessageReceived(new NamedPipeListenerMessageReceivedEventArgs<T>(message));
            }
            catch (Exception ex) {
                OnError(NamedPipeListenerErrorType.NotifyMessageReceived, ex);
                return;
            }

            if (End()) {
                Start();
            }
        }

        public bool End() {
            try {
                pipeServer.Close();
                pipeServer.Dispose();
                pipeServer = null;

                return true;
            }
            catch (Exception ex) {
                OnError(NamedPipeListenerErrorType.CloseAndDisposePipe, ex);
                return false;
            }
        }

        private void OnMessageReceived(T message) {
            OnMessageReceived(new NamedPipeListenerMessageReceivedEventArgs<T>(message));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnMessageReceived(NamedPipeListenerMessageReceivedEventArgs<T> e) {
            MessageReceived?.Invoke(this, e);
        }

        private void OnError(NamedPipeListenerErrorType errorType, Exception ex) {
            OnError(new NamedPipeListenerErrorEventArgs(errorType, ex));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        protected virtual void OnError(NamedPipeListenerErrorEventArgs e) {
            Error?.Invoke(this, e);
        }

        void IDisposable.Dispose() {
            if (pipeServer != null) {
                try { pipeServer.Disconnect(); }
                catch { }

                try { pipeServer.Close(); }
                catch { }

                try { pipeServer.Dispose(); }
                catch { }
            }
        }

        /// <summary>Отправляет указанное <paramref name="message" /> на именованный по умолчанию канал</summary>        
        /// <param name="message">Сообщение для отправки</param>
        public static void SendMessage(T message) {
            SendMessage(DEFAULT_PIPENAME, message);
        }

        /// <summary>Отправляет указанное <paramref name="message" /> на указанный именованный канал</summary>
        /// <param name="pipeName">Имя именованного канала, на который будет отправлено сообщение</param>
        /// <param name="message">Сообщение для отправки</param>
        public static void SendMessage(string pipeName, T message) {
            using (var pipeClient = new NamedPipeClientStream(".", pipeName, PipeDirection.Out, PipeOptions.None)) {
                pipeClient.Connect();

                formatter.Serialize(pipeClient, message);
                pipeClient.Flush();

                pipeClient.WaitForPipeDrain();
                pipeClient.Close();
            }
        }
    }
}