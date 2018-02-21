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
    public class RehearsalPointRepository : MongoLoginRepository<RehearsalPointEntity>, IRehearsalPointRepository
    {
        public RehearsalPointRepository(IMongoCollection<RehearsalPointEntity> collection) : base(collection)
        {
        }

        public override async Task<List<RehearsalPointEntity>> GetAllAsync()
        {
            var filter = Builders<RehearsalPointEntity>.Filter.Eq(x => x.EntityType, EntityType.RehearsalPoint);
            var result = await _collection.Find(filter).ToListAsync();
            return result;
        }

        public override Task UpdateAsync(RehearsalPointEntity entity)
        {
            throw new NotImplementedException("Использовать для обновления BaseRepository");
        }
    }
}
