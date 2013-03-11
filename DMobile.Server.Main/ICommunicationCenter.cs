using System.IO;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace DMobile.Server.Main
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IServiceBase" in both code and config file together.
    [ServiceContract]
    public interface ICommunicationCenter
    {
        /// <summary>
        /// Get
        /// </summary>
        /// <param name="sessionId"></param>
        /// <param name="encryptData"></param>
        /// <returns></returns>
        [WebGet(UriTemplate = "/request?sessionid={sessionId}&encryptdata={encryptData}",
            BodyStyle = WebMessageBodyStyle.Bare,
            ResponseFormat = WebMessageFormat.Json)]
        string GetData(string sessionId, string encryptData);

        /// <summary>
        /// Create or update
        /// </summary>
        /// <param name="dataLength"></param>
        /// <param name="sessionId"></param>
        /// <param name="encryptData"></param>
        /// <returns></returns>
        [WebInvoke(UriTemplate = "/post/{dataLength}/{sessionId}", BodyStyle = WebMessageBodyStyle.Bare, Method = "POST",
            ResponseFormat = WebMessageFormat.Json)]
        string PostData(string dataLength, string sessionId, Stream encryptData);

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="dataLength"></param>
        /// <param name="sessionId"></param>
        /// <param name="encryptData"></param>
        /// <returns></returns>
        [WebInvoke(UriTemplate = "/delete/{dataLength}/{sessionId}", BodyStyle = WebMessageBodyStyle.Bare,
            Method = "DELETE",
            ResponseFormat = WebMessageFormat.Json)]
        string DeleteData(string dataLength, string sessionId, Stream encryptData);
    }
}