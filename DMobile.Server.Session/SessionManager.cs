using System;
using DMobile.Server.Common.Entity;
using DMobile.Server.Common.Entity.Exception;
using DMobile.Server.Session.Entity;
using DMobile.Server.Session.Error;
using DMobile.Server.Session.Interface;

namespace DMobile.Server.Session
{
    public class SessionManager : ISessionValidator
    {
        #region 单例

        private static SessionManager m_Instance;

        public static SessionManager Instance
        {
            get { return m_Instance ?? (m_Instance = new SessionManager()); }
        }

        #endregion

        #region 属性

        public SessionWorker SessionWorker { get; private set; }

        #region Implementation of ISessionValidator

        public bool IsFaked { get; private set; }

        public ErrorDetectiveBase Error { get; private set; }

        public bool IsPassed { get; private set; }

        #endregion

        public SessionBase Session { get; set; }

        public void ValidateSession()
        {
            if (Session == null)
            {
                throw new ArgumentNullException("Session");
            }

            IsPassed = SessionWorker.FindExists(Session);
            if (!IsPassed)
            {
                Error.ErrorCode = ErrorMapping.SESSION_0001;
            }
            if (IsFaked)
            {
                Error.ErrorCode = ErrorMapping.SESSION_0002;
            }
        }

        public object AnalyzeSession()
        {
            return null;
        }

        #endregion

        private SessionManager()
        {
            IsFaked = false;
            IsPassed = true;
            SessionWorker = SessionWorker.Create();
            Error = new SessionErrorHandle();
        }
    }
}