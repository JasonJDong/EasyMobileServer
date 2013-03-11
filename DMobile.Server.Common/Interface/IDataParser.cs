using DMobile.Server.Common.Entity.Exception;

namespace DMobile.Server.Common.Interface
{
    public interface IDataParser
    {
        ErrorDetectiveBase Error { get; }
        string EncryptData(string data);
        string DecryptData(string encryptData);
    }
}