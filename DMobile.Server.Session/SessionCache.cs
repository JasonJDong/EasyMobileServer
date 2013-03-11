using System;
using System.Collections.Generic;
using System.Globalization;
using DMobile.Server.Session.Entity;
using DMobile.Server.Session.Interface;

namespace DMobile.Server.Session
{
    /// <summary>
    /// 存储会话缓存，比较消耗内存
    /// </summary>
    public class SessionCache : ISessionEntry
    {
        private static readonly Dictionary<string, int> SessionIndex = new Dictionary<string, int>();

        private static readonly CacheRank SessionMemoryCache = new CacheRank();

        #region Implementation of ISessionEntry

        public int UsePriority { get; set; }

        public bool NeedSynchronize { get; set; }

        public bool FindExists(SessionBase session)
        {
            if (string.IsNullOrWhiteSpace(session.SessionText))
            {
                return false;
            }
            int rank1 = GetRank(session.SessionText[0]);
            int rank2 = GetRank(session.SessionText[1]);
            int rank3 = GetRank(session.SessionText[2]);

            return SessionMemoryCache[rank1][rank2][rank3].Find(session.SessionText);
        }

        public bool InsertOne(SessionBase session)
        {
            int rank1 = GetRank(session.SessionText[0]);
            int rank2 = GetRank(session.SessionText[1]);
            int rank3 = GetRank(session.SessionText[2]);

            try
            {
                SessionMemoryCache[rank1][rank2][rank3].InsertSession(session.SessionText);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        #endregion

        public SessionCache()
        {
            if (SessionIndex.Count == 0)
            {
                InitializeSessionIndex();
            }
            NeedSynchronize = true;
            UsePriority = 1;
        }

        private void InitializeSessionIndex()
        {
            for (int i = 0; i < 10; i++)
            {
                AddSession(i.ToString(CultureInfo.InvariantCulture), i);
            }
            for (int i = 0; i < 26; i++)
            {
                AddSession(Convert.ToChar(97 + i).ToString(CultureInfo.InvariantCulture), i + 10);
            }
            for (int i = 0; i < 26; i++)
            {
                AddSession(Convert.ToChar(65 + i).ToString(CultureInfo.InvariantCulture), i + 36);
            }
        }

        private void AddSession(string key, int value)
        {
            if (!SessionIndex.ContainsKey(key))
            {
                SessionIndex.Add(key, value);
            }
            else
            {
                SessionIndex[key] = value;
            }
        }

        private int GetRank(char ch)
        {
            return SessionIndex[ch.ToString(CultureInfo.InvariantCulture)];
        }
    }

    internal class CacheRank
    {
        private readonly List<SessionStatus> Content = new List<SessionStatus>(100);
        public List<CacheRank> NextRank = new List<CacheRank>(62);

        public CacheRank this[int index]
        {
            get
            {
                if (NextRank.Count != 62)
                {
                    for (int i = 0; i < 62; i++)
                    {
                        NextRank.Add(new CacheRank());
                    }
                }
                return NextRank[index] ?? (NextRank[index] = new CacheRank());
            }
        }

        public bool Find(string session)
        {
            lock (Content)
            {
                return Content.Find(p => p.Session.Equals(session)) != null;
            }
        }

        public void InsertSession(string session)
        {
            lock (Content)
            {
                var sessionStatus = new SessionStatus {Session = session, BornTime = DateTime.Now};
                if (Content.Contains(sessionStatus))
                {
                    SessionStatus find = Content.Find(p => string.Equals(p.Session, session));
                    Content.Remove(find);
                }
                if (Content.Count == 100)
                {
                    DeleteSession(1800);
                }
                if (Content.Count == 100)
                {
                    Comparison<SessionStatus> p =
                        (span, timeSpan) =>
                        (int) span.BornTime.Subtract(timeSpan.BornTime).TotalSeconds;
                    Content.Sort(p);
                    Content.RemoveAt(0);
                }
                Content.Add(sessionStatus);
            }
        }

        public void DeleteSession(int deleteTime)
        {
            lock (Content)
            {
                List<SessionStatus> all =
                    Content.FindAll(p => p.BornTime.Subtract(DateTime.Now).TotalSeconds > deleteTime);
                foreach (SessionStatus status in all)
                {
                    Content.Remove(status);
                }
            }
        }

        public void DeleteSession(string session)
        {
            lock (Content)
            {
                SessionStatus find = Content.Find(p => p.Session.Equals(session));
                if (find != null)
                {
                    Content.Remove(find);
                }
            }
        }
    }

    internal class SessionStatus
    {
        public string Session { get; set; }

        public DateTime BornTime { get; set; }

        public override bool Equals(object obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (!(obj is SessionStatus))
            {
                return false;
            }

            return Session.Equals((obj as SessionStatus).Session);
        }

        public override int GetHashCode()
        {
            return Session.GetHashCode();
        }

        public override string ToString()
        {
            return Session.ToString(CultureInfo.InvariantCulture);
        }
    }
}