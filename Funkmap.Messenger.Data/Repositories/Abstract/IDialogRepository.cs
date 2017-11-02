﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Funkmap.Common.Data.Mongo.Abstract;
using Funkmap.Messenger.Data.Entities;
using Funkmap.Messenger.Data.Objects;
using Funkmap.Messenger.Data.Parameters;
using MongoDB.Bson;

namespace Funkmap.Messenger.Data.Repositories.Abstract
{
    public interface IDialogRepository : IMongoRepository<DialogEntity>
    {
        Task<ObjectId> CreateAndGetIdAsync(DialogEntity item);
        Task<ICollection<DialogEntity>> GetUserDialogsAsync(string parameter);
        Task<ICollection<string>> GetDialogMembers(string id);
        Task UpdateLastMessageDate(UpdateLastMessageDateParameter parameter);

        Task<bool> CheckDialogExist(List<string> particpants);
        Task<bool> CheckDialogExist(string dialogId);

        Task<ICollection<DialogEntity>> GetDialogsAvatarsAsync(string[] ids);

        Task<DialogEntity> GetDialogAvatarAsync(string id);

    }
}
