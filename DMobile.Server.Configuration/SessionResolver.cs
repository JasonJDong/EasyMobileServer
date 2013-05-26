using System;
using DMobile.Server.Common.Entity.Exception;
using DMobile.Server.Session.Entity;
using DMobile.Server.Session.Error;
using DMobile.Server.Session.Interface;

namespace DMobile.Server.Configuration
{
    public class SessionResolver : ISessionResolver
    {
        public SessionResolver()
        {
            Error = new SessionErrorHandle();
        }

        #region ISessionResolver Members

        public string SessionText { get; set; }

        public SessionBase Resolve()
        {
            if (string.IsNullOrWhiteSpace(SessionText))
            {
                throw new ArgumentNullException("SessionText");
            }
            //TODO:此处session的生命周期和创建时间应从session解析
            return new DefaultSession
                {SessionText = SessionText, CreateLife = DateTime.Now, LifePeriod = TimeSpan.FromDays(10)};
        }

        public ErrorDetectiveBase Error { get; set; }

        #endregion
    }
}