﻿using System;
using System.Reflection;
using Autofac;
using Autofac.Integration.SignalR;
using Autofac.Integration.WebApi;
using Funkmap.Common.Abstract;
using Funkmap.Cqrs.Abstract;
using Funkmap.Notifications.Contracts;
using Funkmap.Notifications.Data;
using Funkmap.Notifications.Domain.Abstract;
using Funkmap.Notifications.Domain.Services;
using Funkmap.Notifications.Domain.Services.Abstract;
using Funkmap.Notifications.SignalR;

namespace Funkmap.Notifications
{
    public class NotificationsModule : IFunkmapModule
    {
        public void Register(ContainerBuilder builder)
        {
            builder.RegisterType<NotificationRepository>().As<INotificationRepository>();
            builder.RegisterType<NotificationsConnectionService>().As<INotificationsConnectionService>().SingleInstance();

            builder.RegisterType<NotificationService>()
                .As<IEventHandler>()
                .SingleInstance()
                .OnActivated(x => x.Instance.InitHandlers())
                .AutoActivate();

            builder.RegisterType<FunkmapNotificationService>().As<IFunkmapNotificationService>();
            
            builder.RegisterType<NotificationsSignalRHandler>()
                .As<IEventHandler>()
                .SingleInstance()
                .OnActivated(x => x.Instance.InitHandlers())
                .AutoActivate();

            builder.RegisterHubs(Assembly.GetExecutingAssembly());
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            Console.WriteLine("Notifications module has been loaded.");
        }
    }
}
