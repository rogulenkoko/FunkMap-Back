﻿using Funkmap.Data.Entities.Entities.Abstract;
using Funkmap.Domain;
using Funkmap.Domain.Models;
using MongoDB.Driver;

namespace Funkmap.Data.Services.Abstract
{
    public interface IUpdateDefinitionBuilder
    {
        EntityType EntityType { get; }

        UpdateDefinition<BaseEntity> Build(Profile profile);
    }
}
