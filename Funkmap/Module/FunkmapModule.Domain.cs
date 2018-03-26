﻿using Autofac;
using Funkmap.Common.Cqrs.Abstract;
using Funkmap.Domain.Services;
using Funkmap.Domain.Services.Abstract;
using Funkmap.Tools;
using Funkmap.Tools.Abstract;
namespace Funkmap.Module
{
    public partial class FunkmapModule 
    {
        private void RegisterDomainDependiences(ContainerBuilder builder)
        {
            builder.RegisterType<ParameterFactory>().As<IParameterFactory>();

            builder.RegisterType<EntityUpdateService>().As<IEntityUpdateService>();
            
            builder.RegisterType<FunkmapNotificationService>()
                .As<IFunkmapNotificationService>()
                .As<IEventHandler>()
                .SingleInstance()
                .OnActivated(x => x.Instance.InitHandlers())
                .AutoActivate();
            
            builder.RegisterType<BandUpdateService>().As<IBandUpdateService>();
            builder.RegisterType<BandUpdateService>().As<IDependenciesController>();
            
        }
    }
}
