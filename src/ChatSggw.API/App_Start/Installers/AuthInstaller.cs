using System.Configuration;
using System.Data.Entity;
using System.Web;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using ChatSggw.API.Models;
using ChatSggw.DataLayer;
using ChatSggw.Domain;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;

namespace ChatSggw.API.Installers
{
    public class AuthInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
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