using System;
using System.Collections.Generic;
using System.Linq;
using DMobile.Server.Common.Entity.Exception;
using DMobile.Server.Common.Interface;
using DMobile.Server.Configuration.Error;
using DMobile.Server.Utilities;

namespace DMobile.Server.Configuration
{
    public class RequestSpliter : IRequestSpliter
    {
        public const string METHOD_KEY = "METHOD";
        public const string ATTACH_KEY = "ATTACHMENT";

        private string m_CacheRequest = string.Empty;
        private List<TextKeyValuePair> m_KeyValue;

        public RequestSpliter()
        {
            Error = new ServerConfigurationErrorHandle();
        }

        #region IRequestSpliter Members

        public ErrorDetectiveBase Error { get; private set; }

        public string GetMethodText(string request)
        {
            return GetValueByKey(request, METHOD_KEY);
        }

        public string GetAttachmentText(string request)
        {
            return GetValueByKey(request, ATTACH_KEY);
        }

        #endregion

        private string GetValueByKey(string request, string key)
        {
            if (!string.Equals(m_CacheRequest, request))
            {
                m_CacheRequest = request;
                try
                {
                    m_KeyValue = JSONConvert.ConvertToObject<List<TextKeyValuePair>>(m_CacheRequest);
                }
                catch (Exception)
                {
                    return string.Empty;
                }
            }
            if (m_KeyValue == null)
            {
                return string.Empty;
            }
            TextKeyValuePair method = m_KeyValue.FirstOrDefault(k => string.Equals(key, k.Key.ToUpper()));
            return method == null ? string.Empty : method.Value;
        }
    }

    public class TextKeyValuePair
    {
        public string Key { get; set; }

        public string Value { get; set; }
    }
}