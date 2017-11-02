﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Funkmap.Common.Redis.Abstract;
using StackExchange.Redis;

namespace Funkmap.Common.Redis
{
    public class RedisStorage : IStorage
    {
        private readonly IDatabase _database;
        private readonly ISerializer _serializer;

        public RedisStorage(IDatabase database, ISerializer serializer)
        {
            _database = database;
            _serializer = serializer;
        }

        public async Task SetAsync<T>(string key, T value, TimeSpan? lifeTime = null) where T : class
        {
            var serialized = _serializer.Serialize(value);
            await _database.StringSetAsync(key, serialized, lifeTime);
        }

        public async Task<T> GetAsync<T>(string key) where T : class
        {
            var value = await _database.StringGetAsync(key);
            return _serializer.Deserialize<T>(value);

        }
    }
}
