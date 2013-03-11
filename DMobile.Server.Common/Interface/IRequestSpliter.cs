using DMobile.Server.Common.Entity.Exception;

namespace DMobile.Server.Common.Interface
{
    public interface IRequestSpliter
    {
        ErrorDetectiveBase Error { get; }
        string GetMethodText(string request);
        string GetAttachmentText(string request);
    }
}