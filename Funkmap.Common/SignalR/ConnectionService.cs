﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Funkmap.Common.SignalR
{
    public class ConnectionService
    {
        protected readonly ConcurrentDictionary<string, UserConnection> _onlineUsers;


        public ConnectionService()
        {
            _onlineUsers = new ConcurrentDictionary<string, UserConnection>();
        }

        public void AddOnlineUser(string id, string login)
        {
            _onlineUsers[id] = new UserConnection()
            {
                Login = login
            };
        }

        public void RemoveOnlineUser(string id, out string login)
        {
            if (_onlineUsers.ContainsKey(id))
            {
                UserConnection user;
                _onlineUsers.TryRemove(id, out user);
                var removedLogin = user.Login;
                login = user.Login;
                var removedLoginKey = _onlineUsers.Where(x => x.Value.Login == removedLogin).Select(x => x.Key).ToList();
                if (removedLoginKey.Count > 0)
                {
                    login = String.Empty;
                }

            }
            else
            {
                login = String.Empty;
            }

        }

        public ICollection<string> GetConnectionIdsByLogins(ICollection<string> logins)
        {
            return _onlineUsers.Where(x => logins.Contains(x.Value.Login)).Select(x => x.Key).ToList();
        }

        public ICollection<string> GetOnlineUsersLogins()
        {
            return _onlineUsers.Values.Select(x => x.Login).Distinct().ToList();
        }
    }
}
