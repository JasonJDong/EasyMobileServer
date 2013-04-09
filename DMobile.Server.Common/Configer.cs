using System;
using System.Configuration;
using System.IO;

namespace DMobile.Server.Common
{
    public class Configer
    {
        private static string m_ServerConfigFile;

        private static string m_BizFile;

        public static string ServerConfigFile
        {
            get
            {
                if (string.IsNullOrWhiteSpace(m_ServerConfigFile))
                {
                    m_ServerConfigFile = ConfigurationManager.AppSettings["ServerConfigFile"];

                    m_ServerConfigFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                                                                   m_ServerConfigFile.Replace('/', '\\').
                                                                       TrimStart('\\'));
                }

                return m_ServerConfigFile;
            }
        }

        public static string BizFile
        {
            get
            {
                if (string.IsNullOrWhiteSpace(m_BizFile))
                {
                    m_BizFile = ConfigurationManager.AppSettings["BizFile"];

                    m_BizFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                                                                   m_BizFile.Replace('/', '\\').
                                                                       TrimStart('\\'));
                }

                return m_BizFile;
            }
        }
    }
}