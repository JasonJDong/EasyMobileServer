using System.IO;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace DMobile.Server.Login
{
    [ServiceContract(Namespace = "http://www.easyspecify.com")]
    public interface ILoginService
    {
        /// <summary>
        /// Get data.
        /// </summary>
        /// <param name="encryptData"></param>
        /// <returns></returns>
        [WebGet(UriTemplate = "/request?data={encryptData}",
            ResponseFormat = WebMessageFormat.Json)]
        string GetData(string encryptData);

        [WebInvoke(UriTemplate = "/post/{dataLength}", Method = "POST",
            RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        string PostData(string dataLength, Stream encryptData);
    }
}