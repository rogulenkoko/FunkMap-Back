﻿using System;
using Microsoft.Owin.Security.Infrastructure;

namespace Funkmap.Auth
{
    public class FunkmapRefreshTokenProvider : AuthenticationTokenProvider
    {
        public override void Create(AuthenticationTokenCreateContext context)
        {
            context.Ticket.Properties.ExpiresUtc = new DateTimeOffset(DateTime.UtcNow.AddDays(7));
            context.Ticket.Properties.AllowRefresh = true;
            context.SetToken(context.SerializeTicket());
        }

        public override void Receive(AuthenticationTokenReceiveContext context)
        {
            context.DeserializeTicket(context.Token);
        }
    }
}
