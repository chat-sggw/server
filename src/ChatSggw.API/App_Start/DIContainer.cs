using Castle.MicroKernel.Registration;
using Castle.Windsor;
using ChatSggw.API.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using System;
using System.Data.Entity;
using System.Web;

namespace ChatSggw.API.App_Start
{
    public class DIContainer
    {
        private static Lazy<IWindsorContainer> container = new Lazy<IWindsorContainer>(() =>
        {
            var container = new WindsorContainer();
            RegisterTypes(container);
            return container;
        });

        public static IWindsorContainer GetConfiguredContainer()
        {
            return container.Value;
        }

        public static void RegisterTypes(IWindsorContainer container)
        {
            // Register your types here

            container.Register(
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
        }
    }
}