using System.Security.Cryptography.X509Certificates;
using System.Security.Principal;


namespace Feodosiya.Lib.Security {
    /// <summary>
    /// Предоставляет методы для работы с привилегиями и безопасностью в ОС
    /// </summary>
    public static class SecurityHelper {
        /// <summary>
        /// Выполняет проверку относится ли текущий пользователь к группе администраторов
        /// </summary>
        /// <returns></returns>
        public static bool IsAdministrator() {
            using (WindowsIdentity identity = WindowsIdentity.GetCurrent()) {
                WindowsPrincipal principal = new WindowsPrincipal(identity);

                return principal.IsInRole(WindowsBuiltInRole.Administrator);
            }
        }

        /// <summary>
        /// Проверяет наличие установленного сертификата в системе
        /// </summary>
        /// <param name="storeName">Одно из значений перечисления, указывающее имя хранилища сертификатов X.509</param>
        /// <param name="storeLocation">Одно из значений перечисления, определяющее расположение хранилища сертификатов X.509</param>
        /// <param name="certName">Имя сертфиката</param>
        /// <returns></returns>
        public static bool CheckCertificateExists(StoreName storeName, StoreLocation storeLocation, string certName) {
            X509Store store = new X509Store(storeName, storeLocation);
            store.Open(OpenFlags.ReadOnly);

            X509Certificate2Collection certificates = store.Certificates.Find(X509FindType.FindBySubjectName, certName, false);

            return (certificates != null && certificates.Count > 0);
        }
    }
}
