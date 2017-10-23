using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace FinanceLibrary.Utils
{
    public class Encryption
    {
        /// <summary>
        /// Encrypts a text with a password and salt.
        /// </summary>
        /// <param name="input">The text to encrypt.</param>
        /// <param name="password">The password to encrypt the text with.</param>
        /// <param name="salt">The salt used for encryption.</param>
        /// <returns></returns>
        public string EncryptText(string input, string password, string salt)
        {
            // Get the bytes of the string
            byte[] bytesToBeEncrypted = Encoding.UTF8.GetBytes(input);
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            byte[] saltBytes = Encoding.UTF8.GetBytes(salt);

            // Hash the password with SHA256
            passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

            byte[] bytesEncrypted = AES_Encrypt(bytesToBeEncrypted, passwordBytes, saltBytes);

            string result = Convert.ToBase64String(bytesEncrypted);

            return result;
        }

        /// <summary>
        /// Decrypts a text with a password and salt.
        /// </summary>
        /// <param name="input">The text to decrypt.</param>
        /// <param name="password">The password to decrypt the text with.</param>
        /// <param name="salt">The salt used for the decryption.</param>
        /// <returns></returns>
        public string DecryptText(string input, string password, string salt)
        {
            // Get the bytes of the string
            byte[] bytesToBeDecrypted = Convert.FromBase64String(input);
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            byte[] saltBytes = Encoding.UTF8.GetBytes(salt);

            passwordBytes = SHA256.Create().ComputeHash(passwordBytes);

            byte[] bytesDecrypted = AES_Decrypt(bytesToBeDecrypted, passwordBytes, saltBytes);

            string result = Encoding.UTF8.GetString(bytesDecrypted);

            return result;
        }

        /// <summary>
        /// Decrypt bytes with AES-256 encryption using password and salt.
        /// </summary>
        /// <param name="bytesToBeDecrypted">Bytes to decrypt.</param>
        /// <param name="passwordBytes">Password in bytes.</param>
        /// <param name="saltBytes">Salt in bytes.</param>
        /// <returns></returns>
        private byte[] AES_Decrypt(byte[] bytesToBeDecrypted, byte[] passwordBytes, byte[] saltBytes)
        {
            byte[] decryptedBytes = null;

            // Set your salt here, change it to meet your flavor:
            // The salt bytes must be at least 8 bytes.
            //byte[] saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    AES.KeySize = 256;
                    AES.BlockSize = 128;

                    var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);

                    AES.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, AES.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
                        cs.Close();
                    }
                    decryptedBytes = ms.ToArray();
                }
            }

            return decryptedBytes;
        }

        /// <summary>
        /// Encrypt bytes with AES-256 encryption using password and salt.
        /// </summary>
        /// <param name="bytesToBeEncrypted">Bytes to encrypt.</param>
        /// <param name="passwordBytes">Password in bytes.</param>
        /// <param name="saltBytes">Salt in bytes.</param>
        /// <returns></returns>
        private byte[] AES_Encrypt(byte[] bytesToBeEncrypted, byte[] passwordBytes, byte[] saltBytes)
        {
            byte[] encryptedBytes = null;

            // Set your salt here, change it to meet your flavor:
            // The salt bytes must be at least 8 bytes.
            //byte[] saltBytes = new byte[] { 1, 2, 3, 4, 5, 6, 7, 8 };

            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    AES.KeySize = 256;
                    AES.BlockSize = 128;

                    var key = new Rfc2898DeriveBytes(passwordBytes, saltBytes, 1000);
                    AES.Key = key.GetBytes(AES.KeySize / 8);
                    AES.IV = key.GetBytes(AES.BlockSize / 8);

                    AES.Mode = CipherMode.CBC;

                    using (var cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                        cs.Close();
                    }
                    encryptedBytes = ms.ToArray();
                }
            }

            return encryptedBytes;
        }
    }
}
