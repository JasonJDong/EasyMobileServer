using System;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace DMobile.Server.Utilities
{
    public static class CommonUtilities
    {
        #region Field Define

        private const string m_LongDefaultKey = "Easy.Special";
        private const string m_LongDefaultIV = "DJStudio";

        #endregion

        public static string ReadFileContent(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return string.Empty;
            }
            using (TextReader textReader = new StreamReader(filePath))
            {
                try
                {
                    return textReader.ReadToEnd();
                }
                catch (Exception)
                {
                    return string.Empty;
                }
            }
        }

        public static string GetClassMethodsNames(Type type)
        {
            MethodInfo[] ms = type.GetMethods(BindingFlags.Public | BindingFlags.Instance);
            var sb = new StringBuilder();
            foreach (MethodInfo item in ms)
            {
                sb.AppendFormat("{0};", item.Name);
            }
            return sb.ToString().Trim(';');
        }

        #region Encrypt & Decrypt

        public static string Encrypt(string encryptionText)
        {
            string result = string.Empty;

            if (encryptionText.Length > 0)
            {
                byte[] bytes = Encoding.Unicode.GetBytes(encryptionText);
                byte[] inArray = Encrypt(bytes);
                if (inArray.Length > 0)
                {
                    result = Convert.ToBase64String(inArray);
                }
            }
            return result;
        }

        public static string Decrypt(string encryptionText)
        {
            string result = string.Empty;

            if (encryptionText.Length > 0)
            {
                byte[] bytes = Convert.FromBase64String(encryptionText);
                byte[] inArray = Decrypt(bytes);
                if (inArray.Length > 0)
                {
                    result = Encoding.Unicode.GetString(inArray);
                }
            }
            return result;
        }

        private static byte[] Encrypt(byte[] bytesData)
        {
            var result = new byte[0];
            using (var stream = new MemoryStream())
            {
                ICryptoTransform cryptoServiceProvider = CreateAlgorithm().CreateEncryptor();
                using (var stream2 = new CryptoStream(stream, cryptoServiceProvider, CryptoStreamMode.Write))
                {
                    try
                    {
                        stream2.Write(bytesData, 0, bytesData.Length);
                        stream2.FlushFinalBlock();
                        stream2.Close();
                        result = stream.ToArray();
                    }
                    catch (Exception exception)
                    {
                        throw new Exception("Error while writing decrypted data to the stream: \n" + exception.Message);
                    }
                }
                stream.Close();
            }
            return result;
        }

        private static byte[] Decrypt(byte[] bytesData)
        {
            var result = new byte[0];
            using (var stream = new MemoryStream())
            {
                ICryptoTransform cryptoServiceProvider = CreateAlgorithm().CreateDecryptor();
                using (var stream2 = new CryptoStream(stream, cryptoServiceProvider, CryptoStreamMode.Write))
                {
                    try
                    {
                        stream2.Write(bytesData, 0, bytesData.Length);
                        stream2.FlushFinalBlock();
                        stream2.Close();
                        result = stream.ToArray();
                    }
                    catch (Exception exception)
                    {
                        throw new Exception("Error while writing encrypted data to the stream: \n" + exception.Message);
                    }
                }
                stream.Close();
            }
            return result;
        }

        private static Rijndael CreateAlgorithm()
        {
            Rijndael rijndael = new RijndaelManaged();
            rijndael.Mode = CipherMode.CBC;
            byte[] key = Encoding.Unicode.GetBytes(m_LongDefaultKey);
            byte[] iv = Encoding.Unicode.GetBytes(m_LongDefaultIV);
            rijndael.Key = key;
            rijndael.IV = iv;
            return rijndael;
        }

        #endregion
    }
}