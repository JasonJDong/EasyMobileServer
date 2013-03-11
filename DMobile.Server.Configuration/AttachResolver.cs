using DMobile.Server.Common.Entity.Exception;
using DMobile.Server.Common.Interface;
using DMobile.Server.Common.Request;
using DMobile.Server.Configuration.Error;
using DMobile.Server.Utilities;

namespace DMobile.Server.Configuration
{
    public class AttachResolver : IAttachmentResolver
    {
        public AttachResolver()
        {
            Error = new ServerConfigurationErrorHandle();
        }

        #region IAttachmentResolver Members

        public string AttachmentText { get; set; }

        public virtual AttachmentSchema Resolve()
        {
            return JSONConvert.ConvertToObject<AttachmentSchema>(AttachmentText);
        }

        public ErrorDetectiveBase Error { get; private set; }

        #endregion
    }
}