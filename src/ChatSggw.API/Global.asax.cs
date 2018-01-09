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
            GlobalConfiguration.Configure(WebApiConfig.Register);
        }
    }
}
