using System.Collections.Generic;

namespace DMobile.Server.Common.Entity.Exception
{
    /// <summary>
    /// 定义用户错误
    /// </summary>
    public abstract class ErrorDetectiveBase
    {
        public string ErrorCode { get; set; }

        public ErrorLevels Level { get; set; }

        public string SimpleErrorDescription { get; set; }

        public string Advice { get; set; }

        public IList<ErrorDetectiveBase> Errors { get; set; }
    }

    public enum ErrorLevels
    {
        CanBeIgnored,
        Warning,
        Error,
        FatalError,
        NeedNotice,
    }
}