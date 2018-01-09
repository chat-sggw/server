using System.Data.Entity.SqlServer;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace ChatSggw.API
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            SqlProviderServices.SqlServerTypesAssemblyName =
                "Microsoft.SqlServer.Types, Version=13.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91";
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
