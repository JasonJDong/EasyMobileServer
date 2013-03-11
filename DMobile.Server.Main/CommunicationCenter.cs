using System;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;
using DMobile.Server.Common.Context;
using DMobile.Server.Common.Interface;
using DMobile.Server.Common.MethodReflection;
using DMobile.Server.Common.Request;
using DMobile.Server.Extension.Plugin.System;
using DMobile.Server.Initializer.Server;
using DMobile.Server.Utilities;

namespace DMobile.Server.Main
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple,
        UseSynchronizationContext = false)]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class CommunicationCenter : ICommunicationCenter
    {
        private const string BAD_REQUEST = "Bad Request";

        #region Implementation of ICommunicationCenter

        public string GetData(string sessionId, string encryptData)
        {
            return DoWork(sessionId, encryptData);
        }

        public string PostData(string dataLength, string sessionId, Stream encryptData)
        {
            return DoWork(sessionId, GetRequestData(dataLength, encryptData));
        }

        public string DeleteData(string dataLength, string sessionId, Stream encryptData)
        {
            return DoWork(sessionId, GetRequestData(dataLength, encryptData));
        }

        #endregion

        public CommunicationCenter()
        {
            //插件初始化
            PluginManager.Instance.LoadPlugins(PluginDetector.GetPlugins());
            //业务
            MethodRoute.Register(ConfigurationLoader.Instance.Business.GetType());
        }

        private string DoWork(string sessionId, string encryptData)
        {
            if (string.IsNullOrWhiteSpace(encryptData))
            {
                return BAD_REQUEST;
            }
            //初始化服务器(插件、配置)
            string decrypString = ConfigurationLoader.Instance.DataParser.DecryptData(encryptData);
            string methodText = ConfigurationLoader.Instance.RequestSpliter.GetMethodText(decrypString);
            string attatchmentText = ConfigurationLoader.Instance.RequestSpliter.GetAttachmentText(decrypString);

            var request = new RequestSchema(sessionId, methodText, attatchmentText);
            request.ResolveRequest(ConfigurationLoader.Instance.MethodResolver,
                                   ConfigurationLoader.Instance.SessionResolver,
                                   ConfigurationLoader.Instance.AttachmentResolver);
            var dataContext = new ServerDataContext {Request = request};

            PluginManager.Instance.UpdateDataContext(dataContext);

            //定义授权方法
            ISecurityAuthorize security = ConfigurationLoader.Instance.SecurityAuthorize;

            //Session
            bool sessionPass = ConfigurationLoader.Instance.Business.SessionAccess(request.Session);
            try
            {
                //获取授权信息
                if (!security.Authorized(dataContext))
                {
                    return security.Error.SimpleErrorDescription;
                }

                if (!sessionPass)
                {
                    return "No Permission";
                }

                //获取请求响应
                //TODO:此处的异常应统一处理
                if (request.RequestMethod == null)
                {
                    return BAD_REQUEST;
                }
                object obj = MethodRoute.Invoke(request.RequestMethod);
                string json = JSONConvert.ConvertToString(obj);

                //压缩发送
                return CompressProvider.Compress(json);
            }
            catch (Exception)
            {
                return "NO RESULT";
            }
        }

        private string GetRequestData(string dataLength, Stream encryptData)
        {
            int length;
            if (int.TryParse(dataLength, out length))
            {
                if (length == 0)
                {
                    return string.Empty;
                }
                var buffer = new byte[length];
                encryptData.Read(buffer, 0, length);
                string data = Encoding.UTF8.GetString(buffer);
                return data;
            }
            return string.Empty;
        }
    }
}