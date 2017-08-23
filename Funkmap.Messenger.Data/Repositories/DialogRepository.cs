﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Funkmap.Messenger.Data.Entities;
using Funkmap.Messenger.Data.Parameters;
using Funkmap.Messenger.Data.Repositories.Abstract;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace Funkmap.Messenger.Data.Repositories
{
    public class DialogRepository : IDialogRepository
    {
        private readonly IMongoCollection<DialogEntity> _collection;

        public DialogRepository(IMongoCollection<DialogEntity> collection)
        {
            _collection = collection;
        }

        public async Task<ICollection<DialogEntity>> GetUserDialogsAsync(UserDialogsParameter parameter)
        {
            var participantsFilter = Builders<DialogEntity>.Filter.AnyEq(x => x.Participants, parameter.Login);

            var sortFilter = Builders<DialogEntity>.Sort.Descending(x => x.LastMessageDate);

            var dialogs = await _collection
                .Find(participantsFilter)
                .Sort(sortFilter)
                .Skip(parameter.Skip)
                .Limit(parameter.Take)
                .ToListAsync();

            return dialogs;
        }

        public async Task<ICollection<string>> GetDialogMembers(string id)
        {
            var filter = Builders<DialogEntity>.Filter.Eq(x => x.Id, new ObjectId(id));
            var projection = Builders<DialogEntity>.Projection.Include(x => x.Participants);
            var dialog = await _collection.Find(filter).Project<DialogEntity>(projection).FirstOrDefaultAsync();
            var members = dialog?.Participants;
            return members;
        }

        public async Task UpdateLastMessageDate(UpdateLastMessageDateParameter parameter)
        {
            if(String.IsNullOrEmpty(parameter.DialogId)) throw new ArgumentException(nameof(parameter.DialogId));
            if(parameter.Date == DateTime.MinValue) throw new ArgumentException(nameof(parameter.DialogId));

            var update = Builders<DialogEntity>.Update.Set(x => x.LastMessageDate, parameter.Date);
            await _collection.UpdateOneAsync(x => x.Id == new ObjectId(parameter.DialogId), update);
        }

        public async Task<ObjectId> CreateAsync(DialogEntity item)
        {
            if(item == null || item.Participants.Count < 2) throw new ArgumentException(nameof(item));
            await _collection.InsertOneAsync(item);

            return item.Id;

        }
    }

}
