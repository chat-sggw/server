using Castle.Windsor.Installer;
using ChatSggw.API.App_Start.Installers;
using ChatSggw.API.Infrastructure;
using System.Web.Http;

[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(AuthInstaller), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethod(typeof(AuthInstaller), "Shutdown")]

namespace ChatSggw.API.App_Start.Installers
{
    public class AuthInstaller
    {
        public static void Start()
        {
            var container = DIContainer.GetConfiguredContainer();
            container.Install(FromAssembly.This());
            GlobalConfiguration.Configuration.DependencyResolver = new DependencyResolver(container.Kernel);
        }

        public static void Shutdown()
        {
            var container = DIContainer.GetConfiguredContainer();
            container.Dispose();
        }
    }
}