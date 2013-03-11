using System;
using System.IO;
using System.Reflection;
using DMobile.Biz.Interface;
using DMobile.Server.Common;
using DMobile.Server.Common.Interface;
using DMobile.Server.Session.Interface;

namespace DMobile.Server.Initializer.Server
{
    public class ConfigurationLoader
    {
        private const string BUSINESS_SUFFIX = "Business";

        private static ConfigurationLoader m_Instance;
        private readonly Assembly BusinessAssembly;
        private readonly string BusinessPath;

        /// <summary>
        /// 定义为全局变量的原因是减少Assembly.Load的次数
        /// </summary>
        private readonly Assembly ConfigurationAssembly;

        private readonly string ConfigurationPath;
        private IAttachmentResolver m_AttachmentResolver;
        private IBusiness m_Business;

        private IDataParser m_DataParser;
        private IMethodResolver m_MethodResolver;
        private IRequestSpliter m_RequestSpliter;
        private ISecurityAuthorize m_SecurityAuthorize;
        private ISessionResolver m_SessionResolver;

        private ConfigurationLoader()
        {
            ConfigurationPath = Configer.ServerConfigFile;
            BusinessPath = FindBusinessAssemblyLocation();

            if (File.Exists(ConfigurationPath))
            {
                ConfigurationAssembly = Assembly.LoadFile(ConfigurationPath);
            }
            if (File.Exists(BusinessPath))
            {
                BusinessAssembly = Assembly.LoadFile(BusinessPath);
            }
        }

        public static ConfigurationLoader Instance
        {
            get { return m_Instance ?? (m_Instance = new ConfigurationLoader()); }
        }

        public IDataParser DataParser
        {
            get { return m_DataParser ?? (m_DataParser = LoadImplementFromAssembly<IDataParser>(ConfigurationAssembly)); }
        }

        public IRequestSpliter RequestSpliter
        {
            get
            {
                return m_RequestSpliter ??
                       (m_RequestSpliter = LoadImplementFromAssembly<IRequestSpliter>(ConfigurationAssembly));
            }
        }

        public ISecurityAuthorize SecurityAuthorize
        {
            get
            {
                return m_SecurityAuthorize ??
                       (m_SecurityAuthorize = LoadImplementFromAssembly<ISecurityAuthorize>(ConfigurationAssembly));
            }
        }

        public IAttachmentResolver AttachmentResolver
        {
            get
            {
                return m_AttachmentResolver ??
                       (m_AttachmentResolver = LoadImplementFromAssembly<IAttachmentResolver>(ConfigurationAssembly));
            }
        }

        public IMethodResolver MethodResolver
        {
            get
            {
                return m_MethodResolver ??
                       (m_MethodResolver = LoadImplementFromAssembly<IMethodResolver>(ConfigurationAssembly));
            }
        }

        public ISessionResolver SessionResolver
        {
            get
            {
                return m_SessionResolver ??
                       (m_SessionResolver = LoadImplementFromAssembly<ISessionResolver>(ConfigurationAssembly));
            }
        }

        public IBusiness Business
        {
            get { return m_Business ?? (m_Business = LoadImplementFromAssembly<IBusiness>(BusinessAssembly)); }
        }

        private T LoadImplementFromAssembly<T>(Assembly assembly)
        {
            if (assembly != null)
            {
                Type[] types = assembly.GetExportedTypes();
                Type interfaceType = typeof (T);
                foreach (Type type in types)
                {
                    if (interfaceType.FullName != null)
                    {
                        Type existInterface = type.GetInterface(interfaceType.FullName, true);
                        if (existInterface != null)
                        {
                            return (T) Activator.CreateInstance(type);
                        }
                    }
                }
            }
            return default(T);
        }

        private string FindBusinessAssemblyLocation()
        {
            if (!string.IsNullOrWhiteSpace(Configer.BizFile))
            {
                return Configer.BizFile;
            }
            Assembly assembly = Assembly.GetExecutingAssembly();
            string fullPath = assembly.CodeBase.Replace("file:///", "");
            var file = new FileInfo(fullPath);
            DirectoryInfo currentDir = file.Directory;

            if (currentDir != null)
            {
                FileInfo[] configs = currentDir.GetFiles(string.Format("*{0}.dll", BUSINESS_SUFFIX));

                if (configs.Length > 0)
                {
                    return configs[0].FullName;
                }
            }
            return string.Empty;
        }
    }
}