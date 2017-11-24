using System.Web.Http;
using Castle.Windsor;
using Castle.Windsor.Installer;
using ChatSggw.API.Infrastructure;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;
using Castle.MicroKernel.Registration;
using Microsoft.Owin.Security;
using System.Web;
using Microsoft.AspNet.Identity;
using System.Data.Entity;
using ChatSggw.API.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace ChatSggw.API
{
    public static class WebApiConfig
    {
        private static WindsorContainer Container { get; set; }

        public static void Register(HttpConfiguration config)
        {
            Container = new WindsorContainer();
            Container.Install(FromAssembly.This());
            GlobalConfiguration.Configuration.DependencyResolver = new DependencyResolver(Container.Kernel);
            
            Container.Register(
                Component
                    .For<ApplicationDbContext>()
                    .DependsOn(Dependency.OnValue<string>("DefaultConnection"))
                    .LifestyleTransient(),
                Component
                    .For<IUserStore<ApplicationUser>>()
                    .ImplementedBy<UserStore<ApplicationUser>>()
                    .DependsOn(Dependency.OnComponent<DbContext, ApplicationDbContext>())
                    .LifestyleTransient(),
                Component
                    .For<IAuthenticationManager>()
                    .UsingFactoryMethod(kernel => HttpContext.Current.GetOwinContext().Authentication)
                    .LifestyleTransient(),
                Component
                    .For<ApplicationSignInManager>()
                    .LifestyleTransient(),
                Component
                    .For<ApplicationUserManager>()
                    .LifestyleTransient()
            );

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
        }
    }
}