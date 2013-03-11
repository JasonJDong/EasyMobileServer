using System;
using DMobile.Server.Common.Context;
using DMobile.Server.Extension.Plugin.Common.Error;

namespace DMobile.Server.Extension.Plugin.Common
{
    public abstract class PluginBase : IDisposable
    {
        public PluginErrorHandle Error { get; set; }

        public DataContext DataContext { get; set; }

        public PluginSchema PluginInfo { get; set; }

        public abstract bool ReceiveDataContext(DataContext dataContext);

        public abstract bool Close();

        public abstract bool Open();

        public abstract bool Stop();

        public abstract bool Restart();

        #region IDisposable 成员

        public virtual void Dispose()
        {
            if (null != DataContext)
            {
                DataContext.Dispose();
            }
        }

        #endregion
    }
}