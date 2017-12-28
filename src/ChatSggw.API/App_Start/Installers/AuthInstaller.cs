using System;
using System.Configuration;
using System.Data.Entity;
using System.Web;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using ChatSggw.API.Models;
using ChatSggw.DataLayer;
using ChatSggw.DataLayer.IdentityModels;
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
                    .For<IUserStore<ApplicationUser, Guid>>()
                    .ImplementedBy<UserStore<ApplicationUser, CustomRole, Guid,
                        CustomUserLogin, CustomUserRole, CustomUserClaim>>()
                    .DependsOn(Dependency.OnComponent<DbContext, CoreDbContext>())
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