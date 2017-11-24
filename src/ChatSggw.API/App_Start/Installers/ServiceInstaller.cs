using System;
using System.Collections.Generic;
using System.Linq;
using Castle.MicroKernel.Registration;
using Castle.MicroKernel.SubSystems.Configuration;
using Castle.Windsor;
using Neat.CQRSLite.Contract.Commands;
using Neat.CQRSLite.Contract.Events;
using Neat.CQRSLite.Contract.Helpers;
using Neat.CQRSLite.Contract.Queries;
using Neat.CQRSLite.CQRS;

namespace ChatSggw.API.Installers
{
    public class ServiceInstaller : IWindsorInstaller
    {
        public void Install(IWindsorContainer container, IConfigurationStore store)
        {
            container.Register(
                Classes
                    .FromAssemblyInThisApplication()
                    .BasedOn(typeof(ICommandHandler<>))
                    .WithService.Base()
                    .LifestyleTransient(),
                Classes
                    .FromAssemblyInThisApplication()
                    .BasedOn(typeof(ICommandValidator<>))
                    .WithService.Base()
                    .LifestyleTransient(),
                Classes
                    .FromAssemblyInThisApplication()
                    .BasedOn(typeof(IEventHandler<>))
                    .WithService.Base()
                    .LifestyleTransient(),
                Classes
                    .FromAssemblyInThisApplication()
                    .BasedOn(typeof(IQueryPerformer<,>))
                    .WithServiceBase()
                    .LifestyleTransient(),
                //buses register 
                Component.For<ICommandBus>().ImplementedBy<CommandBus>().LifestyleSingleton(),
                Component.For<IQueryBus>().ImplementedBy<QueryBus>().LifestyleSingleton(),
                Component.For<IEventBus>().ImplementedBy<EventBus>().LifestyleSingleton(),
                Component.For<Assistant>().LifestyleSingleton(),
                //resolving functions register
                Component.For<Func<Type, ICommandValidator>>()
                    .UsingFactoryMethod<Func<Type, ICommandValidator>>(kernel =>
                    {
                        var ctx = kernel;
                        return type => (ctx.HasComponent(type) ? ctx.Resolve(type) : null) as ICommandValidator;
                    }),
                Component.For<Func<Type, ICommandHandler>>()
                    .UsingFactoryMethod<Func<Type, ICommandHandler>>(kernel =>
                    {
                        var ctx = kernel;
                        return (type) => (ICommandHandler) ctx.Resolve(type);
                    }),
                Component.For<Func<Type, IQueryPerformer>>()
                    .UsingFactoryMethod<Func<Type, IQueryPerformer>>(kernel =>
                    {
                        var ctx = kernel;
                        return (type) => (IQueryPerformer) ctx.Resolve(type);
                    }),
                Component.For<Func<Type, IEnumerable<IEventHandler>>>()
                    .UsingFactoryMethod<Func<Type, IEnumerable<IEventHandler>>>(kernel =>
                    {
                        var ctx = kernel;
                        return (type) => ctx.ResolveAll(type).Cast<IEventHandler>();
                    })
            );
        }
    }
}