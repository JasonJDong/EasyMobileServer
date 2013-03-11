using DMobile.Server.Common.Entity.Exception;
using DMobile.Server.Session.Entity;

namespace DMobile.Server.Session.Interface
{
    public interface ISessionValidator
    {
        SessionBase Session { get; set; }
        ErrorDetectiveBase Error { get; }
        bool IsPassed { get; }
        void ValidateSession();
        object AnalyzeSession();
    }
}