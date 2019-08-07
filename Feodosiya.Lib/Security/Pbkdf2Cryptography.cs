using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Feodosiya.Lib.Security {
    /// <summary>
    /// Предоставляет методы для шифрования и защиты информации
    /// </summary>
    public static class Pbkdf2Cryptography {
        /// <summary>
        /// Количество байт соли
        /// </summary>
        public const int SaltByteSize = 24;
        /// <summary>
        /// Количество байт хеша
        /// </summary>
        public const int HashByteSize = 20;
        /// <summary>
        /// Количество байт ключа
        /// </summary>
        public const int KeyByteSize = 256;
        /// <summary>
        /// Количество итераций Pbkdf2
        /// </summary>
        public const int Pbkdf2Iterations = 1000;
        /// <summary>
        /// Индекс итераций
        /// </summary>
        public const int IterationIndex = 0;
        /// <summary>
        /// Индекс соли
        /// </summary>
        public const int SaltIndex = 1;
        /// <summary>
        /// Индекс Pbkdf2
        /// </summary>
        public const int Pbkdf2Index = 2;


        /// <summary>
        /// Возвращает хешированный пароль
        /// </summary>
        /// <param name="password">Пароль</param>
        /// <returns>Хеш пароля</returns>
        public static string HashPassword(string password) {
            RNGCryptoServiceProvider cryptoProvider = new RNGCryptoServiceProvider();
            byte[] salt = new byte[SaltByteSize];
            cryptoProvider.GetBytes(salt);

            byte[] hash = GetPbkdf2Bytes(password, salt, Pbkdf2Iterations, HashByteSize);
            return Pbkdf2Iterations + ":" +
                   Convert.ToBase64String(salt) + ":" +
                   Convert.ToBase64String(hash);
        }

        /// <summary>
        /// Проверяет валидность хеша пароля
        /// </summary>
        /// <param name="password">Пароль</param>
        /// <param name="correctHash">Хеш для сверки</param>
        /// <returns>Результат проверки</returns>
        public static bool ValidatePassword(string password, string correctHash) {
            char[] delimiter = { ':' };
            string[] split = correctHash.Split(delimiter);
            int iterations = int.Parse(split[IterationIndex]);
            byte[] salt = Convert.FromBase64String(split[SaltIndex]);
            byte[] hash = Convert.FromBase64String(split[Pbkdf2Index]);

            byte[] testHash = GetPbkdf2Bytes(password, salt, iterations, hash.Length);
            return SlowEquals(hash, testHash);
        }

        /// <summary>
        /// Сраавнивает два массива байт
        /// </summary>
        /// <param name="a">Первый массив байт</param>
        /// <param name="b">Второй массив байт</param>
        /// <returns></returns>
        private static bool SlowEquals(byte[] a, byte[] b) {
            uint diff = (uint)a.Length ^ (uint)b.Length;
            for (int i = 0; i < a.Length && i < b.Length; i++) {
                diff |= (uint)(a[i] ^ b[i]);
            }
            return diff == 0;
        }

        /// <summary>
        /// Возвращает псевдослучайный ключ пароля в соответствии со стандартом RFC 2898
        /// </summary>
        /// <param name="password">Пароль</param>
        /// <param name="salt">Соль</param>
        /// <param name="iterations">Количество итераций</param>
        /// <param name="outputBytes">Число байтов псевдослучайного ключа</param>
        /// <returns></returns>
        private static byte[] GetPbkdf2Bytes(string password, byte[] salt, int iterations, int outputBytes) {
            Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(password, salt);
            pbkdf2.IterationCount = iterations;

            return pbkdf2.GetBytes(outputBytes);
        }

        /// <summary>
        /// Возвращает произвольные байты
        /// </summary>
        /// <returns></returns>
        private static byte[] Generate256BitsOfRandomEntropy() {
            byte[] randomBytes = new byte[32];
            using (RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider()) {
                rngCsp.GetBytes(randomBytes);
            }
            return randomBytes;
        }

        /// <summary>
        /// Расчитывает MD5 хеш для заданной строки
        /// </summary>
        /// <param name="source">Строка для которой расчитывается MD5 хеш</param>
        /// <returns></returns>
        public static string GetMD5Hash(string source) {
            StringBuilder hash = new StringBuilder();
            MD5 md5Hasher = MD5.Create();
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(source));
            for (int index = 0; index < data.Length; index++) {
                hash.Append(data[index].ToString("x2"));
            }

            return hash.ToString();
        }

        /// <summary>
        /// Расчитывает MD5 хеш для заданной строки
        /// </summary>
        /// <param name="source">Массив байт для которого расчитывается MD5 хеш</param>
        /// <returns></returns>
        public static string GetMD5Hash(byte[] source) {
            StringBuilder hash = new StringBuilder();
            MD5 md5Hasher = MD5.Create();
            byte[] data = md5Hasher.ComputeHash(source);
            for (int index = 0; index < data.Length; index++) {
                hash.Append(data[index].ToString("x2"));
            }

            return hash.ToString();
        }

        /// <summary>
        /// Шифрует указанный массив байт
        /// </summary>
        /// <param name="plainTextBytes">Данные для шифрования</param>
        /// <param name="passPhrase">Пароль для шифрования</param>
        /// <returns></returns>
        public static string Encrypt(byte[] plainTextBytes, string passPhrase) {
            byte[] saltStringBytes = Generate256BitsOfRandomEntropy();
            byte[] ivStringBytes = Generate256BitsOfRandomEntropy();

            using (Rfc2898DeriveBytes password = new Rfc2898DeriveBytes(passPhrase, saltStringBytes, Pbkdf2Iterations)) {
                byte[] keyBytes = password.GetBytes(KeyByteSize / 8);
                using (RijndaelManaged symmetricKey = new RijndaelManaged()) {
                    symmetricKey.BlockSize = 256;
                    symmetricKey.Mode = CipherMode.CBC;
                    symmetricKey.Padding = PaddingMode.PKCS7;
                    using (ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, ivStringBytes)) {
                        using (MemoryStream memoryStream = new MemoryStream()) {
                            using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write)) {
                                cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                                cryptoStream.FlushFinalBlock();
                                byte[] cipherTextBytes = saltStringBytes;
                                cipherTextBytes = cipherTextBytes.Concat(ivStringBytes).ToArray();
                                cipherTextBytes = cipherTextBytes.Concat(memoryStream.ToArray()).ToArray();
                                memoryStream.Close();
                                cryptoStream.Close();

                                return Convert.ToBase64String(cipherTextBytes);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Дешифрует строку в массив байт
        /// </summary>
        /// <param name="cipherText">Строка для дешифрации</param>
        /// <param name="passPhrase">Пароль для дешифрации</param>
        /// <param name="decryptedCount">Возращает количество дешифрованных байт</param>
        /// <returns></returns>
        public static byte[] Decrypt(string cipherText, string passPhrase, out int decryptedCount) {
            byte[] cipherTextBytesWithSaltAndIv = Convert.FromBase64String(cipherText);
            byte[] saltStringBytes = cipherTextBytesWithSaltAndIv.Take(KeyByteSize / 8).ToArray();
            byte[] ivStringBytes = cipherTextBytesWithSaltAndIv.Skip(KeyByteSize / 8).Take(KeyByteSize / 8).ToArray();
            byte[] cipherTextBytes = cipherTextBytesWithSaltAndIv.Skip((KeyByteSize / 8) * 2).Take(cipherTextBytesWithSaltAndIv.Length - ((KeyByteSize / 8) * 2)).ToArray();

            using (Rfc2898DeriveBytes password = new Rfc2898DeriveBytes(passPhrase, saltStringBytes, Pbkdf2Iterations)) {
                byte[] keyBytes = password.GetBytes(KeyByteSize / 8);
                using (RijndaelManaged symmetricKey = new RijndaelManaged()) {
                    symmetricKey.BlockSize = 256;
                    symmetricKey.Mode = CipherMode.CBC;
                    symmetricKey.Padding = PaddingMode.PKCS7;
                    using (ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, ivStringBytes)) {
                        using (MemoryStream memoryStream = new MemoryStream(cipherTextBytes)) {
                            using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read)) {
                                byte[] plainTextBytes = new byte[cipherTextBytes.Length];
                                decryptedCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                                memoryStream.Close();
                                cryptoStream.Close();

                                return plainTextBytes;
                            }
                        }
                    }
                }
            }
        }
    }
}
