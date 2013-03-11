using System;
using System.Text;
using DMobile.Server.Common.Entity;
using DMobile.Server.Common.Entity.Exception;
using DMobile.Server.Common.Interface;
using DMobile.Server.Configuration.Error;

namespace DMobile.Server.Configuration
{
    public class DataParser : IDataParser
    {
        private const int HeaderCount = 20;
        private readonly Encoding Encoding = Encoding.UTF8;

        #region 构造函数

        public DataParser()
        {
            Error = new ServerConfigurationErrorHandle();
        }

        #endregion

        #region Implementation of IDataParser

        public ErrorDetectiveBase Error { get; private set; }

        public virtual string EncryptData(string data)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(data);

            string base64 = Convert.ToBase64String(buffer);

            if (base64.Length < HeaderCount)
            {
                return base64;
            }

            var encrypt = new StringBuilder(base64, 0, HeaderCount, HeaderCount);
            for (int i = 0; i < encrypt.Length; i++)
            {
                if (char.IsLower(encrypt[i]))
                {
                    encrypt[i] = (char) (encrypt[i] - 32);
                    continue;
                }
                if (char.IsUpper(encrypt[i]))
                {
                    encrypt[i] = (char) (encrypt[i] + 32);
                    continue;
                }
                if (char.IsNumber(encrypt[i]))
                {
                    encrypt[i] = (char) (105 - encrypt[i]); //57-(encrypt[i] - 48)
                }
            }
            if (base64.Length - HeaderCount < 0)
            {
                Error.ErrorCode = ErrorMapping.BUSINESS_FFFF;
                throw new ArgumentException();
            }
            var newSb = new StringBuilder(base64, HeaderCount, base64.Length - HeaderCount, base64.Length);
            newSb.Insert(0, encrypt);

            return newSb.ToString();
        }

        public virtual string DecryptData(string encryptData)
        {
            if (encryptData.Length < HeaderCount)
            {
                return encryptData;
            }
            var encrypt = new StringBuilder(encryptData, 0, HeaderCount, HeaderCount);
            for (int i = 0; i < encrypt.Length; i++)
            {
                if (char.IsUpper(encrypt[i]))
                {
                    encrypt[i] = (char) (encrypt[i] + 32);
                    continue;
                }
                if (char.IsLower(encrypt[i]))
                {
                    encrypt[i] = (char) (encrypt[i] - 32);
                    continue;
                }
                if (char.IsNumber(encrypt[i]))
                {
                    encrypt[i] = (char) (105 - encrypt[i]);
                }
            }
            if (encryptData.Length - HeaderCount < 0)
            {
                Error.ErrorCode = ErrorMapping.BUSINESS_FFFF;
                throw new ArgumentException();
            }
            var newSb = new StringBuilder(encryptData, HeaderCount, encryptData.Length - HeaderCount, encryptData.Length);
            newSb.Insert(0, encrypt);

            byte[] dataBuffer = Convert.FromBase64String(newSb.ToString());
            string finalData = Encoding.GetString(dataBuffer);
            return finalData;
        }

        #endregion
    }
}