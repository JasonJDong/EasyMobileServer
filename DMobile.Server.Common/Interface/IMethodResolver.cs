using DMobile.Server.Common.Entity.Exception;
using DMobile.Server.Common.Request;

namespace DMobile.Server.Common.Interface
{
    public interface IMethodResolver
    {
        string MethodText { get; set; }
        ErrorDetectiveBase Error { get; }
        MethodSchema Resolve();
    }
}