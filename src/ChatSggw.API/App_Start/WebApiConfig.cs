using System.ComponentModel;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using Castle.Windsor;
using Castle.Windsor.Installer;
using ChatSggw.API.Infrastructure;
using Elmah.Contrib.WebApi;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;

namespace ChatSggw.API
{
    public static class WebApiConfig
    {
        public static WindsorContainer Container { get; set; }

        public static void Register(HttpConfiguration config)
        {
            Container = new WindsorContainer();
            Container.Install(FromAssembly.This());
            GlobalConfiguration.Configuration.DependencyResolver = new DependencyResolver(Container.Kernel);

            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            // Use camel case for JSON data.
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver =
                new CamelCasePropertyNamesContractResolver();

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                "DefaultApi",
                "api/{controller}/{id}",
                new {id = RouteParameter.Optional}
            );

            config.Services.Add(typeof(IExceptionLogger), new ElmahExceptionLogger());

            config.Routes.MapHttpRoute(
                name: "NotFound",
                routeTemplate: "{*path}",
                defaults: new { controller = "Error", action = "NotFound" }
            );
        }
    }
}