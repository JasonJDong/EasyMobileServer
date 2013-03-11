using DMobile.Server.Common.Entity.Exception;
using DMobile.Server.Session.Entity;

namespace DMobile.Server.Session.Interface
{
    public interface ISessionResolver
    {
        string SessionText { get; set; }
        ErrorDetectiveBase Error { get; set; }
        SessionBase Resolve();
    }
}