using System;
using System.Diagnostics;
using Microsoft.Win32;

namespace DMobile.Server.Utilities
{
    /// <summary>
    /// RegistryHelper。
    /// </summary>
    public static class RegistryHelper
    {
        #region 读取值

        /// <summary>
        /// Gets the safe int64.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="name">The name.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public static long GetSafeInt64(RegistryKey key, string name, long defaultValue)
        {
            long ret = defaultValue;
            try
            {
                ret = Convert.ToInt64(key.GetValue(name, defaultValue));
            }
            catch (SystemException ex)
            {
                Debug.Fail(ex.Message);
            }

            return ret;
        }


        /// <summary>
        /// Gets the safe int32.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="name">The name.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public static int GetSafeInt32(RegistryKey key, string name, int defaultValue)
        {
            int ret = defaultValue;
            try
            {
                ret = Convert.ToInt32(key.GetValue(name, defaultValue));
            }
            catch (SystemException ex)
            {
                Debug.Fail(ex.Message);
            }

            return ret;
        }


        /// <summary>
        /// Gets the safe double.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="name">The name.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public static double GetSafeDouble(RegistryKey key, string name, double defaultValue)
        {
            double ret = defaultValue;
            try
            {
                ret = Convert.ToDouble(key.GetValue(name, defaultValue));
            }
            catch (SystemException ex)
            {
                Debug.Fail(ex.Message);
            }

            return ret;
        }


        /// <summary>
        /// Gets the safe string.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="name">The name.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public static string GetSafeString(RegistryKey key, string name, string defaultValue)
        {
            string ret = defaultValue;
            try
            {
                ret = (string) key.GetValue(name, defaultValue);
            }
            catch (SystemException ex)
            {
                Debug.Fail(ex.Message);
            }

            return ret;
        }


        /// <summary>
        /// Gets the safe date time.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="name">The name.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public static DateTime GetSafeDateTime(RegistryKey key, string name, DateTime defaultValue)
        {
            DateTime ret = defaultValue;
            try
            {
                ret = Convert.ToDateTime(key.GetValue(name, defaultValue));
            }
            catch (SystemException ex)
            {
                Debug.Fail(ex.Message);
            }

            return ret;
        }


        /// <summary>
        /// Gets the safe boolean.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="name">The name.</param>
        /// <param name="defaultValue">if set to <c>true</c> [default value].</param>
        /// <returns></returns>
        public static bool GetSafeBoolean(RegistryKey key, string name, bool defaultValue)
        {
            bool ret = defaultValue;
            try
            {
                ret = Convert.ToBoolean(key.GetValue(name, defaultValue));
            }
            catch (SystemException ex)
            {
                Debug.Fail(ex.Message);
            }

            return ret;
        }

        #endregion

        #region 打开注册表键

        /// <summary>
        /// Opens the key for read.
        /// </summary>
        /// <param name="subKey">The sub key.</param>
        /// <returns></returns>
        public static RegistryKey OpenKeyForRead(string subKey)
        {
            return OpenKeyForRead(Registry.LocalMachine, subKey);
        }


        /// <summary>
        /// Opens the key for read.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="subKey">The sub key.</param>
        /// <returns></returns>
        public static RegistryKey OpenKeyForRead(RegistryKey key, string subKey)
        {
            RegistryKey regKey = null;

            regKey = key.OpenSubKey(subKey, true);
            if (regKey == null)
            {
                regKey = key.CreateSubKey(subKey);
                regKey.Close();
                regKey = key.OpenSubKey(subKey);
            }

            return regKey;
        }


        /// <summary>
        /// Opens the key for write.
        /// </summary>
        /// <param name="subKey">The sub key.</param>
        /// <returns></returns>
        public static RegistryKey OpenKeyForWrite(string subKey)
        {
            return OpenKeyForWrite(Registry.LocalMachine, subKey);
        }


        /// <summary>
        /// Opens the key for write.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="subKey">The sub key.</param>
        /// <returns></returns>
        public static RegistryKey OpenKeyForWrite(RegistryKey key, string subKey)
        {
            RegistryKey regKey = null;

            regKey = key.OpenSubKey(subKey, true);
            if (regKey == null)
            {
                regKey = key.CreateSubKey(subKey);
            }

            return regKey;
        }

        #endregion
    }
}