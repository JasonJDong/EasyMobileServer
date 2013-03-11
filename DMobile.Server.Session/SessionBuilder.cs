using System;
using System.Text;
using DMobile.Server.Session.Entity;

namespace DMobile.Server.Session
{
    public static class SessionBuilder
    {
        public static SessionBase BuildSession(string head, string suffix, TimeSpan lifePeriod)
        {
            DateTime bornTime = DateTime.Now;
            var sessionText = new StringBuilder();
            sessionText.Append(head);
            sessionText.Append(bornTime.Year);
            sessionText.Append(bornTime.Month);
            sessionText.Append(bornTime.Day);
            sessionText.Append(bornTime.Hour);
            sessionText.Append(bornTime.Minute);
            sessionText.Append(bornTime.Second);
            sessionText.Append(lifePeriod.TotalSeconds);
            sessionText.Append(Guid.NewGuid().ToString().Replace("-", string.Empty));
            sessionText.Append(suffix);

            byte[] buffer = Encoding.UTF8.GetBytes(sessionText.ToString());
            string base64 = Convert.ToBase64String(buffer);
            return new DefaultSession {CreateLife = bornTime, LifePeriod = lifePeriod, SessionText = base64};
        }
    }
}