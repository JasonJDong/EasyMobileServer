using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace DMobile.Server.Utilities.IO
{
    public class FileSystemChangeEventHandler
    {
        #region Field Define

        private readonly Object m_SyncObject;
        private readonly int m_Timeout;
        private readonly Dictionary<string, Timer> m_Timers;
        public event FileSystemEventHandler ActualHandler;

        #endregion

        #region constructor

        private FileSystemChangeEventHandler()
        {
            m_SyncObject = new object();
            m_Timers = new Dictionary<string, Timer>(new CaseInsensitiveStringEqualityComparer());
        }

        public FileSystemChangeEventHandler(int timeout)
            : this()
        {
            m_Timeout = timeout;
        }

        #endregion

        #region Public Function

        public void ChangeEventHandler(Object sender, FileSystemEventArgs e)
        {
            lock (m_SyncObject)
            {
                Timer t;

                // disable the existing timer
                if (m_Timers.ContainsKey(e.FullPath))
                {
                    t = m_Timers[e.FullPath];
                    t.Change(Timeout.Infinite, Timeout.Infinite);
                    t.Dispose();
                }

                // add a new timer
                if (ActualHandler != null)
                {
                    t = new Timer(TimerCallback, new FileChangeEventArg(sender, e), m_Timeout, Timeout.Infinite);
                    m_Timers[e.FullPath] = t;
                }
            }
        }

        #endregion

        #region Private Function

        private void TimerCallback(Object state)
        {
            var arg = state as FileChangeEventArg;
            ActualHandler(arg.Sender, arg.Argument);
        }

        #endregion
    }

    internal class FileChangeEventArg
    {
        public FileChangeEventArg(Object sender, FileSystemEventArgs arg)
        {
            Sender = sender;
            Argument = arg;
        }

        public Object Sender { get; private set; }
        public FileSystemEventArgs Argument { get; private set; }
    }
}