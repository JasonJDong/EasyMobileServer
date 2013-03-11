using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DMobile.Server.Common.Context;
using DMobile.Server.Common.Entity.Exception;
using DMobile.Server.Extension.Plugin.Common;
using DMobile.Server.Extension.Plugin.Common.Error;

namespace DMobile.Server.Extension.Plugin.System
{
    public class PluginManager : List<PluginBase>, IDisposable
    {
        private static PluginManager m_Instance;

        private PluginManager()
        {
        }

        public static PluginManager Instance
        {
            get { return m_Instance ?? (m_Instance = new PluginManager()); }
        }

        public PluginErrorHandle Error { get; set; }

        #region IDisposable Members

        public void Dispose()
        {
            foreach (PluginBase pluginBase in this)
            {
                pluginBase.Dispose();
                if (pluginBase.Error.Level == ErrorLevels.NeedNotice)
                {
                    Error.Errors.Add(pluginBase.Error);
                }
            }
        }

        #endregion

        public PluginBase GetPluginByIndex(int idx)
        {
            if (idx > -1 && Count > idx)
            {
                return this[idx];
            }
            return null;
        }

        public PluginBase GetPluginByName(string name)
        {
            return Find(p => string.Equals(p.PluginInfo.Name, name));
        }

        public bool OpenAll()
        {
            bool allSuccess = true;
            foreach (PluginBase pluginBase in this)
            {
                allSuccess &= pluginBase.Open();
                if (pluginBase.Error.Level == ErrorLevels.NeedNotice)
                {
                    Error.Errors.Add(pluginBase.Error);
                }
            }
            return allSuccess;
        }

        public bool CloseAll()
        {
            bool allSuccess = true;
            foreach (PluginBase pluginBase in this)
            {
                allSuccess &= pluginBase.Close();
                if (pluginBase.Error.Level == ErrorLevels.NeedNotice)
                {
                    Error.Errors.Add(pluginBase.Error);
                }
            }
            return allSuccess;
        }

        public void LoadPlugins(IList<PluginBase> plugins)
        {
            AddRange(plugins);
        }

        public void UpdateDataContext(DataContext dataContext)
        {
            var task = new Task(() =>
                {
                    if (dataContext != null)
                    {
                        lock (this)
                        {
                            foreach (PluginBase plugin in this)
                            {
                                plugin.ReceiveDataContext(dataContext);
                            }
                        }
                    }
                });
            task.Start();
            //ThreadPool.QueueUserWorkItem(ThreadForUpdateDataContext, dataContext);
        }

        private void ThreadForUpdateDataContext(object obj)
        {
            var dataContext = obj as DataContext;
            if (dataContext != null)
            {
                lock (this)
                {
                    foreach (PluginBase plugin in this)
                    {
                        plugin.ReceiveDataContext(dataContext);
                    }
                }
            }
        }
    }
}