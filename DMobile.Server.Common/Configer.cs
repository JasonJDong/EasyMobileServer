using System;
using System.Configuration;
using System.IO;

namespace DMobile.Server.Common
{
    public class Configer
    {
        private static string m_ServerListFileLocation;

        public static string m_DataCommandFileListConfigFile;

        private static int m_FileChangeInteravl = -1;

        private static string m_ServerConfigFile;

        private static string m_BizFile;

        public static string ServerListFileLocation
        {
            get
            {
                if (string.IsNullOrWhiteSpace(m_ServerListFileLocation))
                {
                    m_ServerListFileLocation = ConfigurationManager.AppSettings["ServerList"];

                    m_ServerListFileLocation = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                                                            m_ServerListFileLocation.Replace('/', '\\').TrimStart('\\'));
                }

                return m_ServerListFileLocation;
            }
        }

        public static string DataCommandFileListConfigFile
        {
            get
            {
                if (string.IsNullOrWhiteSpace(m_DataCommandFileListConfigFile))
                {
                    m_DataCommandFileListConfigFile = ConfigurationManager.AppSettings["DataCommandFiles"];

                    m_DataCommandFileListConfigFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                                                                   m_DataCommandFileListConfigFile.Replace('/', '\\').
                                                                       TrimStart('\\'));
                }

                return m_DataCommandFileListConfigFile;
            }
        }

        public static int FileChangeInteravl
        {
            get
            {
                if (m_FileChangeInteravl == -1)
                {
                    object objInterval = ConfigurationManager.AppSettings["FileChangeInteravl"];

                    if (objInterval == null)
                    {
                        m_FileChangeInteravl = 500;
                    }
                    else
                    {
                        try
                        {
                            m_FileChangeInteravl = Convert.ToInt32(objInterval);
                        }
                        catch
                        {
                            m_FileChangeInteravl = 500;
                        }
                    }
                }

                return m_FileChangeInteravl;
            }
        }

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