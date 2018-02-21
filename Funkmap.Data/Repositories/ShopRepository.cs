﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Funkmap.Common.Data.Mongo;
using Funkmap.Data.Entities;
using Funkmap.Data.Entities.Entities;
using Funkmap.Data.Repositories.Abstract;
using MongoDB.Driver;

namespace Funkmap.Data.Repositories
{
    public class ShopRepository : MongoLoginRepository<ShopEntity>, IShopRepository
    {
        public ShopRepository(IMongoCollection<ShopEntity> collection) : base(collection)
        {
        }

        public override async Task<List<ShopEntity>> GetAllAsync()
        {
            var filter = Builders<ShopEntity>.Filter.Eq(x => x.EntityType, EntityType.Shop);
            var result = await _collection.Find(filter).ToListAsync();
            return result;
        }

        public override Task UpdateAsync(ShopEntity entity)
        {
            throw new NotImplementedException("Использовать для обновления BaseRepository");
        }
    }
}
