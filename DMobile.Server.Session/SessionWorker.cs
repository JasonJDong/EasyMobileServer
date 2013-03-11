using System.Collections.Generic;
using DMobile.Server.Session.Entity;
using DMobile.Server.Session.Interface;

namespace DMobile.Server.Session
{
    public class SessionWorker
    {
        private readonly List<ISessionEntry> sessionEntries;

        private SessionWorker()
        {
            sessionEntries = new List<ISessionEntry>();
        }

        public static SessionWorker Create()
        {
            return new SessionWorker();
        }

        public void AddSessionEntry(ISessionEntry sessionEntry)
        {
            if (!sessionEntries.Contains(sessionEntry))
            {
                sessionEntries.Add(sessionEntry);
                SortSessionEntriesOrder();
            }
        }

        public void AddSessionEntry(IList<ISessionEntry> entries)
        {
            foreach (ISessionEntry entry in entries)
            {
                AddSessionEntry(entry);
            }
        }

        public void RemoveSessionEntry(ISessionEntry sessionEntry)
        {
            if (sessionEntries.Contains(sessionEntry))
            {
                sessionEntries.Remove(sessionEntry);
            }
        }

        public void RemoveSessionEntry()
        {
            sessionEntries.Clear();
        }

        private void SortSessionEntriesOrder()
        {
            sessionEntries.Sort((x, y) =>
                {
                    if (x.UsePriority > y.UsePriority)
                    {
                        return 1;
                    }
                    if (x.UsePriority < y.UsePriority)
                    {
                        return -1;
                    }
                    return 0;
                });
        }

        public bool FindExists(SessionBase session)
        {
            bool find = false;
            var notFind = new List<int>();
            for (int i = 0; i < sessionEntries.Count; i++)
            {
                if (sessionEntries[i].FindExists(session))
                {
                    find = true;
                }
                else
                {
                    notFind.Add(i);
                }
            }

            //同步需要保存的会话
            if (find && notFind.Count > 0)
            {
                foreach (int t in notFind)
                {
                    if (sessionEntries[t].NeedSynchronize)
                    {
                        sessionEntries[t].InsertOne(session);
                    }
                }
            }
            return find;
        }

        public bool InsertOne(SessionBase session)
        {
            bool allSuccess = true;
            foreach (ISessionEntry communication in sessionEntries)
            {
                if (!communication.InsertOne(session))
                {
                    allSuccess = false;
                }
            }
            return allSuccess;
        }
    }
}