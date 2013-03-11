using DMobile.Server.Session.Entity;

namespace DMobile.Biz.Interface
{
    public interface ISessionMethod
    {
        bool FindExists(SessionBase session);
        bool InsertOne(SessionBase session);
    }
}