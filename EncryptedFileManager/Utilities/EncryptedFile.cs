using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Security.Cryptography;

namespace EncryptedFileManager.Utilities
{
    class EncryptedFile
    {
        public static void Encrypt(string unencryptedFile, User u, string k = null)
        {
            string encryptedFile;
            string key;

            if (k != null)
                key = k.Substring(0, 8);
            else
                key = u.PassWord.Substring(0, 8);

            encryptedFile = unencryptedFile + "E";
            byte[] originalFile = File.ReadAllBytes(unencryptedFile);

            using(DESCryptoServiceProvider DES = new DESCryptoServiceProvider())
            {
                DES.IV = Encoding.UTF8.GetBytes(key);
                DES.Key = Encoding.UTF8.GetBytes(key);
                DES.Mode = CipherMode.CBC;
                DES.Padding = PaddingMode.PKCS7;

                using(MemoryStream memStream = new MemoryStream())
                {
                    CryptoStream cryptoStream = new CryptoStream(memStream, DES.CreateEncryptor(), CryptoStreamMode.Write);

                    cryptoStream.Write(originalFile, 0, originalFile.Length);
                    cryptoStream.FlushFinalBlock();
                    File.WriteAllBytes(encryptedFile, memStream.ToArray());
                }
            }
        }

        public static void Decrypt(string unencryptedFile, User u, string k = null)
        {
            string encryptedFile;
            string key;

            if (k != null)
                key = k.Substring(0, 8);
            else
                key = u.PassWord.Substring(0, 8);

            encryptedFile = unencryptedFile + "E";
            byte[] eFile = File.ReadAllBytes(encryptedFile);

            using (DESCryptoServiceProvider DES = new DESCryptoServiceProvider())
            {
                DES.IV = Encoding.UTF8.GetBytes(key);
                DES.Key = Encoding.UTF8.GetBytes(key);
                DES.Mode = CipherMode.CBC;
                DES.Padding = PaddingMode.PKCS7;

                using (MemoryStream memStream = new MemoryStream())
                {
                    CryptoStream cryptoStream = new CryptoStream(memStream, DES.CreateDecryptor(), CryptoStreamMode.Write);

                    cryptoStream.Write(eFile, 0, eFile.Length);
                    cryptoStream.FlushFinalBlock();
                    File.WriteAllBytes(unencryptedFile, memStream.ToArray());
                }
            }

        }

        public static void DecryptLogin(string unencryptedFile)
        {
            string loginKey = "EncFman&";
            Decrypt(unencryptedFile, new User(), loginKey);
        }

        public static void EncryptLogin(string unencryptedFile)
        {
            string loginKey = "EncFman&";
            Encrypt(unencryptedFile, new User(), loginKey);
        }
    }
}
