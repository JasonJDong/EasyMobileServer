using DMobile.Server.Common.Context;
using DMobile.Server.Common.Entity.Exception;

namespace DMobile.Server.Common.Interface
{
    public interface ISecurityAuthorize
    {
        ErrorDetectiveBase Error { get; }
        bool Authorized(DataContext context);
    }
}