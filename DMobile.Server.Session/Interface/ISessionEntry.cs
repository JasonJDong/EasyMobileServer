using DMobile.Server.Session.Entity;

namespace DMobile.Server.Session.Interface
{
    public interface ISessionEntry
    {
        int UsePriority { get; set; }
        bool NeedSynchronize { get; }
        bool FindExists(SessionBase session);
        bool InsertOne(SessionBase session);
    }
}