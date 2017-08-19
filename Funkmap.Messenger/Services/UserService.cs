﻿using System;
using System.Threading.Tasks;
using Funkmap.Auth.Contracts.Models;
using Funkmap.Auth.Contracts.Services;
using ServiceStack.Messaging;

namespace Funkmap.Messenger.Services
{
    public class UserService : IUserMqService
    {

        private readonly IMessageFactory _redisMqFactory;

        private readonly string _messengerMQName = "mq:messenger:123";

        private readonly TimeSpan _mqTimeOut = TimeSpan.FromSeconds(15);

        public UserService(IMessageFactory redisMqFactory)
        {
            _redisMqFactory = redisMqFactory;
        }

        public UserLastVisitDateResponse GetLastVisitDate(UserLastVisitDateRequest request)
        {
            using (var mqClient = _redisMqFactory.CreateMessageQueueClient())
            {
                var uniqueCallbackQ = "mq:c1" + ":" + Guid.NewGuid().ToString("N");

                mqClient.Publish(new Message<UserLastVisitDateRequest>(request)
                {
                    ReplyTo = uniqueCallbackQ
                });

                var response = mqClient.Get<UserLastVisitDateResponse>(uniqueCallbackQ, _mqTimeOut)?.GetBody();
                return response;
            }
        }
    }
}