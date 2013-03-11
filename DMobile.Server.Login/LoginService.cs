using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;
using DMobile.Biz.Interface;
using DMobile.Server.Common.Context;
using DMobile.Server.Common.Interface;
using DMobile.Server.Common.MethodReflection;
using DMobile.Server.Common.Request;
using DMobile.Server.Extension.Plugin.System;
using DMobile.Server.Initializer.Server;
using DMobile.Server.Utilities;

namespace DMobile.Server.Login
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple,
        UseSynchronizationContext = false)]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class LoginService : ILoginService
    {
        private const string BAD_REQUEST = "Bad Request";

        public LoginService()
        {
            PluginManager.Instance.LoadPlugins(PluginDetector.GetPlugins());
            MethodRoute.Register(ConfigurationLoader.Instance.Business.GetType());
        }

        private static IList<string> MethodsName { get; set; }

        #region ILoginService Members

        public string GetData(string encryptData)
        {
            return DoWork(encryptData);
        }

        public string PostData(string dataLength, Stream encryptData)
        {
            return DoWork(GetRequestData(dataLength, encryptData));
        }

        #endregion

        private string DoWork(string encryptData)
        {
            if (string.IsNullOrWhiteSpace(encryptData))
            {
                return BAD_REQUEST;
            }
            //初始化服务器(插件、配置)
            string decrypString = ConfigurationLoader.Instance.DataParser.DecryptData(encryptData);
            string methodText = ConfigurationLoader.Instance.RequestSpliter.GetMethodText(decrypString);
            string attatchmentText = ConfigurationLoader.Instance.RequestSpliter.GetAttachmentText(decrypString);

            var request = new RequestSchema(methodText, attatchmentText);
            request.ResolveRequestWithoutSession(ConfigurationLoader.Instance.MethodResolver,
                                                 ConfigurationLoader.Instance.AttachmentResolver);
            var dataContext = new ServerDataContext { Request = request };

            PluginManager.Instance.UpdateDataContext(dataContext);

            //定义授权方法
            ISecurityAuthorize security = ConfigurationLoader.Instance.SecurityAuthorize;

            try
            {
                //获取授权信息
                if (!security.Authorized(dataContext))
                {
                    return security.Error.SimpleErrorDescription;
                }

                //获取允许的方法
                MethodsName = MethodsName ?? (MethodsName = AvailableInvokeMethod(typeof(ILoginEntry)));

                //获取请求响应
                //TODO:此处的异常应统一处理
                if (request.RequestMethod == null)
                {
                    return BAD_REQUEST;
                }
                object obj = MethodRoute.Invoke(request.RequestMethod, MethodsName);
                string json = JSONConvert.ConvertToString(obj);

                //压缩发送
                return CompressProvider.Compress(json);
            }
            catch (Exception ex)
            {
                return ex.InnerException != null ? ex.InnerException.ToString() : ex.ToString();
            }
        }

        /// <summary>
        /// 获取类可用方法
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private IList<string> AvailableInvokeMethod(Type type)
        {
            MethodInfo[] methods = type.GetMethods(BindingFlags.Public | BindingFlags.Instance);
            IList<string> methodsName = new List<string>(methods.Length);
            foreach (MethodInfo method in methods)
            {
                methodsName.Add(method.Name);
            }
            return methodsName;
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