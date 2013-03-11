using System;
using System.ServiceModel.Activation;
using System.Web;
using System.Web.Routing;
using DMobile.Server.Login;
using DMobile.Server.Main;

namespace DMobile.Server.Publish
{
    public class Global : HttpApplication
    {
        private void Application_Start(object sender, EventArgs e)
        {
            RegisterRoutes();
        }

        private void RegisterRoutes()
        {
            RouteTable.Routes.Add(new ServiceRoute("CommunicationCenter", new WebServiceHostFactory(),
                                                   typeof (CommunicationCenter)));
            RouteTable.Routes.Add(new ServiceRoute("LoginService", new WebServiceHostFactory(), typeof (LoginService)));
        }
    }
}