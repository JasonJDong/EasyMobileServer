using DMobile.Server.Common.Entity.Exception;
using DMobile.Server.Common.Interface;
using DMobile.Server.Common.Request;
using DMobile.Server.Configuration.Error;
using DMobile.Server.Utilities;

namespace DMobile.Server.Configuration
{
    public class MethodMatch : IMethodResolver
    {
        public MethodMatch()
        {
            Error = new ServerConfigurationErrorHandle();
        }

        #region IMethodResolver Members

        public string MethodText { get; set; }

        public virtual MethodSchema Resolve()
        {
            return JSONConvert.ConvertToObject<MethodSchema>(MethodText);
        }

        public ErrorDetectiveBase Error { get; private set; }

        #endregion
    }
}