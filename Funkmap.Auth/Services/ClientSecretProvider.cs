﻿using System.Collections.Generic;
using Funkmap.Auth.Domain.Models;
using Funkmap.Module.Auth.Abstract;

namespace Funkmap.Module.Auth.Services
{
    public class ClientSecretProvider : IClientSecretProvider
    {
        private readonly HashSet<ClientSecrets> _clients;

        public ClientSecretProvider()
        {
            //todo в будущем, доставать из базы креды зарегестрированных клиентских приожений
            _clients = new HashSet<ClientSecrets>() { new ClientSecrets("funkmap", "funkmap") };
        }

        public bool ValidateClient(string clientId, string clientSecret)
        {
            return _clients.Contains(new ClientSecrets(clientId, clientSecret));
        }
    }
}
