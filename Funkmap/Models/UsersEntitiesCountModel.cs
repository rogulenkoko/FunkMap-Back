﻿using System.Collections.Generic;
using Funkmap.Data.Entities;

namespace Funkmap.Models
{
    public class UsersEntitiesCountModel
    {
        public EntityType Type { get; set; }
        public int Count { get; set; }
        public ICollection<string> Logins { get; set; }
    }
}
