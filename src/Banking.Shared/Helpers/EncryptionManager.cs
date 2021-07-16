using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;
using System.Text.RegularExpressions;

namespace Banking.Shared.Helpers
{
    public static class EncryptionManager
    {
        internal const string Inputkey = "560A18CD-6346-4CF0-A2E8-671F9B6B9EA9";

        public static string Encrypt(string valueToEncrypt, string passPhrase)
        {
            if (string.IsNullOrEmpty(valueToEncrypt))
                throw new ArgumentNullException("text");

            var aesAlg = NewRijndaelManaged(passPhrase);

            var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
            var msEncrypt = new MemoryStream();
            using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
            using (var swEncrypt = new StreamWriter(csEncrypt))
            {
                swEncrypt.Write(valueToEncrypt);
            }

            return Convert.ToBase64String(msEncrypt.ToArray());
        }

        public static string Decrypt(string encryptedValue, string passPhrase)
        {
            if (string.IsNullOrEmpty(encryptedValue))
                throw new ArgumentNullException("cipherText");

            if (!IsBase64String(encryptedValue))
                throw new Exception("The cipherText input parameter is not base64 encoded");

            string text;

            var aesAlg = NewRijndaelManaged(passPhrase);
            var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
            var cipher = Convert.FromBase64String(encryptedValue);

            using (var msDecrypt = new MemoryStream(cipher))
            {
                using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (var srDecrypt = new StreamReader(csDecrypt))
                    {
                        text = srDecrypt.ReadToEnd();
                    }
                }
            }
            return text;
        }

        public static bool IsBase64String(string base64String)
        {
            base64String = base64String.Trim();
            return (base64String.Length % 4 == 0) &&
                   Regex.IsMatch(base64String, @"^[a-zA-Z0-9\+/]*={0,3}$", RegexOptions.None);

        }

        private static RijndaelManaged NewRijndaelManaged(string salt)
        {
            if (salt == null) throw new ArgumentNullException("salt");
            var saltBytes = Encoding.ASCII.GetBytes(salt);
            var key = new Rfc2898DeriveBytes(Inputkey, saltBytes);

            var aesAlg = new RijndaelManaged();
            aesAlg.Key = key.GetBytes(aesAlg.KeySize / 8);
            aesAlg.IV = key.GetBytes(aesAlg.BlockSize / 8);

            return aesAlg;
        }
    }
}
