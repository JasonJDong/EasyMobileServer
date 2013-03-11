using DMobile.Server.Common.Entity.Exception;
using DMobile.Server.Common.Request;

namespace DMobile.Server.Common.Interface
{
    public interface IAttachmentResolver
    {
        string AttachmentText { get; set; }
        ErrorDetectiveBase Error { get; }
        AttachmentSchema Resolve();
    }
}