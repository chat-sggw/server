using System.ComponentModel;
using System.Web.Http;
using System.Web.Http.Cors;
using Castle.Windsor;
using Castle.Windsor.Installer;
using ChatSggw.API.Infrastructure;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;
using Swashbuckle.Application;

namespace ChatSggw.API
{
    public static class WebApiConfig
    {
        public static WindsorContainer Container { get; set; }

        public static void Register(HttpConfiguration config)
        {
            Container = new WindsorContainer();
            Container.Install(FromAssembly.This());
            config.DependencyResolver = new DependencyResolver(Container.Kernel);

            // Web API configuration and services
            // Configure Web API to use only bearer token authentication.
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));

            // Use camel case for JSON data.
            config.Formatters.JsonFormatter.SerializerSettings.ContractResolver =
                new CamelCasePropertyNamesContractResolver();

            //CORS support
            config.EnableCors(new EnableCorsAttribute("*", "*", "*"));

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "Swagger UI",
                routeTemplate: "", // "/" root path
                defaults: null,
                constraints: null,
                handler: new RedirectHandler(SwaggerDocsConfig.DefaultRootUrlResolver, "swagger/ui/index"));

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new {id = RouteParameter.Optional}
            );
        }
    }
}