using DMobile.Biz.Interface;
using DMobile.Server.Common.Request;

namespace DMobile.Server.Common.Context
{
    public class ServerDataContext : DataContext
    {
        public RequestSchema Request { get; set; }

        public IBusiness Business { get; set; }

        public override void Dispose()
        {
        }
    }
}