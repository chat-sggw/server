using System.Configuration;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using ChatSggw.DataLayer;
using ChatSggw.Domain;

namespace ChatSggw.API.Installers
{
    public class DataLayerInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            var dbSettings = new CoreDbContext.Settings()
            {
                ConnectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString,
            };

            container.Register(
                Component.For<CoreDbContext>()
                    .DependsOn(Dependency.OnValue<CoreDbContext.Settings>(dbSettings))
                    .LifestyleScoped(),
                Classes.FromAssemblyInThisApplication()
                    .BasedOn(typeof(IRepository<,>))
                    .WithService.Base()
                    .LifestyleScoped()
            );
        }
    }
}