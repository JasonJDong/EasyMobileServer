using System;
using System.Collections.Generic;

namespace DMobile.Server.Common.Context
{
    public abstract class DataContext : IDisposable
    {
        public EnvironmentContext Environment { get; protected set; }

        public object Data { get; set; }

        public Dictionary<string, object> Params { get; set; }

        #region IDisposable 成员

        public abstract void Dispose();

        #endregion
    }
}