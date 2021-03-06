﻿using System;
using System.Reflection;
using Autofac;
using Autofac.Integration.WebApi;
using Funkmap.Common.Abstract;

namespace Funkmap.Module
{
    public partial class FunkmapModule : IFunkmapModule
    {
        public void Register(ContainerBuilder builder)
        {
            RegisterDomainDependencies(builder);
            
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            Console.WriteLine("Funkmap module has been loaded.");
        }
    }
}
