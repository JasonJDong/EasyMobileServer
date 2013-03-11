using System;

namespace DMobile.Server.Utilities
{
    public static class UniqueIDProvider
    {
        public static string GetUniqueID()
        {
            return Guid.NewGuid().ToString().Replace("-", "");
        }
    }
}