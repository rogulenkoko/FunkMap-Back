﻿using Autofac.Extras.Moq;
using Funkmap.Auth.Notifications;
using Funkmap.Common.Notifications.Notification;
using Funkmap.Common.Settings;
using Funkmap.Middleware.Settings;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Funkmap.Auth.Tests
{
    [TestClass]
    public class NotificationTest
    {
        [TestMethod]
        public void SendConfirmationTest()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Provide<ISettingsService>(new MonolithSettingsService());
                var service = mock.Create<EmailExternalNotificationService>();
                var message = new ConfirmationNotification("rogulenkoko@gmail.com", "Кирилл Рогуленко", "123321");
                var success = service.TrySendNotificationAsync(message).Result;
                Assert.IsTrue(success);
            }
        }

        [TestMethod]
        public void SendRecoveryPasswordTest()
        {
            using (var mock = AutoMock.GetLoose())
            {
                mock.Provide<ISettingsService>(new MonolithSettingsService());
                var service = mock.Create<EmailExternalNotificationService>();
                var message = new PasswordRecoverNotification("rogulenkoko@gmail.com", "Кирилл Рогуленко", "123321");
                var success = service.TrySendNotificationAsync(message).Result;
                Assert.IsTrue(success);
            }
        }
    }
}
