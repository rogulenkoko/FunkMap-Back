﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace Funkmap.Common.Data.Mongo.Abstract
{
    public interface IMongoRepository<T> where T : class 
    {
        Task<ICollection<T>> GetAllAsync();
        Task<T> GetAsync(string id);
        Task CreateAsync(T item);
        Task<T> DeleteAsync(string id);
        Task UpdateAsync(T entity);
    }
}
