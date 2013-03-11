using DMobile.Server.Common.Base;
using DMobile.Server.Common.Entity.Exception;
using DMobile.Server.Common.Interface;
using DMobile.Server.Session.Entity;
using DMobile.Server.Session.Interface;

namespace DMobile.Server.Common.Request
{
    public sealed class RequestSchema : CommonBase
    {
        public RequestSchema(string sessionText, string methodText, string attachmentText = null)
        {
            MethodText = methodText;
            SessionText = sessionText;
            AttachmentText = attachmentText;

            Error = new ErrorHandle();
        }

        public RequestSchema(string methodText, string attachmentText = null)
            : this(string.Empty, methodText, attachmentText)
        {
        }

        public string MethodText { get; private set; }

        public string SessionText { get; private set; }

        public string AttachmentText { get; private set; }

        public MethodSchema RequestMethod { get; set; }

        public SessionBase Session { get; set; }

        public AttachmentSchema RequestAttachment { get; set; }

        public void ResolveRequest(IMethodResolver methodResolver, ISessionResolver sessionResolver,
                                   IAttachmentResolver attachmentResolver)
        {
            methodResolver.MethodText = MethodText;
            sessionResolver.SessionText = SessionText;
            attachmentResolver.AttachmentText = AttachmentText;

            RequestMethod = methodResolver.Resolve();
            Session = sessionResolver.Resolve();
            RequestAttachment = attachmentResolver.Resolve();

            if (methodResolver.Error.Level == ErrorLevels.NeedNotice)
            {
                Error.Errors.Add(methodResolver.Error);
            }
            if (attachmentResolver.Error.Level == ErrorLevels.NeedNotice)
            {
                Error.Errors.Add(attachmentResolver.Error);
            }
            if (sessionResolver.Error.Level == ErrorLevels.NeedNotice)
            {
                Error.Errors.Add(sessionResolver.Error);
            }
        }

        public void ResolveRequestWithoutSession(IMethodResolver methodResolver, IAttachmentResolver attachmentResolver)
        {
            methodResolver.MethodText = MethodText;
            attachmentResolver.AttachmentText = AttachmentText;

            RequestMethod = methodResolver.Resolve();
            RequestAttachment = attachmentResolver.Resolve();

            if (methodResolver.Error.Level == ErrorLevels.NeedNotice)
            {
                Error.Errors.Add(methodResolver.Error);
            }
            if (attachmentResolver.Error.Level == ErrorLevels.NeedNotice)
            {
                Error.Errors.Add(attachmentResolver.Error);
            }
        }
    }
}