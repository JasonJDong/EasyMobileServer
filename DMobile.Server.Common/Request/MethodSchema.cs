using System.Collections.Generic;

namespace DMobile.Server.Common.Request
{
    public class MethodSchema
    {
        public string MethodName { get; set; }

        public Dictionary<string, object> Parameters { get; set; }
    }
}