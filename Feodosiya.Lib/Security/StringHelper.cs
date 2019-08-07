using System.Text;


namespace Feodosiya.Lib.Security {
    /// <summary>
    /// 
    /// </summary>
    public static class StringHelper {

        /// <summary>
        /// 
        /// </summary>
        public static Encoding Encoding { get; set; } = Encoding.UTF8;

        /// <summary>
        /// 
        /// </summary>
        public static string PassPhrase { get; set; }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetDecryptedString(this string str) {
            int count;
            byte[] data = Pbkdf2Cryptography.Decrypt(str, PassPhrase, out count);

            return Encoding.GetString(data, 0, count);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="passPhrase"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        public static string GetDecryptedString(this string str, string passPhrase, Encoding encoding) {
            int count;
            byte[] data = Pbkdf2Cryptography.Decrypt(str, passPhrase, out count);

            return encoding.GetString(data, 0, count);
        }
    }
}
