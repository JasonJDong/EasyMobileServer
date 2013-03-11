using DMobile.Server.Session.Entity;

namespace DMobile.Biz.Interface
{
    public interface IBusiness : ILoginEntry
    {
        bool SessionAccess(SessionBase session);
    }
}