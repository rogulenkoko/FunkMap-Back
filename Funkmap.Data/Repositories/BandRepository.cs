﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Funkmap.Common;
using Funkmap.Common.Data.Mongo;
using Funkmap.Data.Entities;
using Funkmap.Data.Repositories.Abstract;
using MongoDB.Driver;

namespace Funkmap.Data.Repositories
{
    public class BandRepository : MongoLoginRepository<BandEntity>, IBandRepository
    {
        public BandRepository(IMongoCollection<BandEntity> collection) : base(collection)
        {
        }

        public override async Task<ICollection<BandEntity>> GetAllAsync()
        {
            var filter = Builders<BandEntity>.Filter.Eq(x => x.EntityType, EntityType.Band);
            var result = await _collection.Find(filter).ToListAsync();
            return result;
        }

        public override Task UpdateAsync(BandEntity entity)
        {
            throw new NotImplementedException("Использовать для обновления BaseRepository");
        }

        public async Task CleanMusiciansDependencies(MusicianEntity musician, string bandLogin = null)
        {
            if (musician?.BandLogins == null || musician.BandLogins.Count == 0) return;

            //db.bases.updateMany({t:3, log:{$in:['rhcp', 'coldplay']}}, {$pull:{inv: 'rogulenkoko', mus: 'rogulenkoko'}})

            var bandUpdate = Builders<BandEntity>.Update.Pull(x => x.InvitedMusicians, musician.Login).Pull(x => x.MusicianLogins, musician.Login);

            if (!String.IsNullOrEmpty(bandLogin))
            {
                var bandFilter = Builders<BandEntity>.Filter.Eq(x => x.EntityType, EntityType.Band) & Builders<BandEntity>.Filter.Eq(x => x.Login, bandLogin);
                await _collection.UpdateOneAsync(bandFilter, bandUpdate);
            }
            else
            {
                var bandFilter = Builders<BandEntity>.Filter.Eq(x => x.EntityType, EntityType.Band) & Builders<BandEntity>.Filter.In(x => x.Login, musician.BandLogins);
                await _collection.UpdateManyAsync(bandFilter, bandUpdate);
            }
        }
    }
}
