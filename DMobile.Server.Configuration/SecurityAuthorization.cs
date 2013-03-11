using DMobile.Server.Common.Context;
using DMobile.Server.Common.Entity.Exception;
using DMobile.Server.Common.Interface;
using DMobile.Server.Configuration.Error;

namespace DMobile.Server.Configuration
{
    public class SecurityAuthorization : ISecurityAuthorize
    {
        public SecurityAuthorization(IAttachmentResolver attach)
            : this()
        {
            AttachResolver = attach;
        }

        public SecurityAuthorization()
        {
            Error = new ServerConfigurationErrorHandle();
        }

        public IAttachmentResolver AttachResolver { get; set; }

        #region Implementation of ISecurityAuthorize

        public ErrorDetectiveBase Error { get; private set; }

        public bool Authorized(DataContext context)
        {
            return true;
        }

        #endregion
    }
}