using System;

namespace DMobile.Server.Session.Entity
{
    public abstract class SessionBase
    {
        public string SessionText { get; set; }

        public TimeSpan LifePeriod { get; set; }

        public DateTime CreateLife { get; set; }
    }
}