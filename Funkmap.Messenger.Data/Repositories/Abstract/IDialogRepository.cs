﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Funkmap.Common.Data.Mongo.Abstract;
using Funkmap.Messenger.Data.Entities;

namespace Funkmap.Messenger.Data.Repositories.Abstract
{
    public interface IDialogRepository : IMongoRepository<DialogEntity>
    {
        Task<ICollection<DialogEntity>> GetUserDialogs(string login);
    }
}
