﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Funkmap.Common.Data.Mongo;
using Funkmap.Common.Tools;
using Funkmap.Data.Entities.Abstract;
using Funkmap.Data.Objects;
using Funkmap.Data.Parameters;
using Funkmap.Data.Repositories.Abstract;
using Funkmap.Data.Services.Abstract;
using Funkmap.Data.Tools;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace Funkmap.Data.Repositories
{
    public class BaseRepository : MongoLoginRepository<BaseEntity>, IBaseRepository
    {
        private readonly IFilterFactory _filterFactory;
        private readonly IGridFSBucket _bucket;

        public BaseRepository(IMongoCollection<BaseEntity> collection,
                              IGridFSBucket bucket,
                              IFilterFactory filterFactory) : base(collection)
        {
            _filterFactory = filterFactory;
            _bucket = bucket;
        }

        public async Task<ICollection<BaseEntity>> GetAllAsyns()
        {
            var filter = Builders<BaseEntity>.Filter.Eq(x => x.IsActive, true);

            var projection = Builders<BaseEntity>.Projection
                .Exclude(x => x.Description)
                .Exclude(x => x.Name);

            return await _collection.Find(filter).Project<BaseEntity>(projection).ToListAsync();
        }

        public async Task<ICollection<BaseEntity>> GetNearestAsync(LocationParameter parameter)
        {
            //db.bases.find({ loc: { $nearSphere: [50, 30], $minDistance: 0, $maxDistance: 1 } }).limit(20)

            //todo придумать адекватную проекцию для геопозиции
            var projection = Builders<BaseEntity>.Projection
                .Exclude(x => x.Description)
                .Exclude(x => x.Name)
                .Exclude(x => x.Address)
                .Exclude(x => x.VideoInfos)
                .Exclude(x => x.YouTubeLink)
                .Exclude(x => x.FacebookLink)
                .Exclude(x => x.SoundCloudLink)
                .Exclude(x => x.VkLink);


            ICollection<BaseEntity> result;
            if (parameter.Longitude == null || parameter.Latitude == null)
            {
                var filter = Builders<BaseEntity>.Filter.Eq(x => x.IsActive, true);
                result = await _collection.Find(filter).Project<BaseEntity>(projection).Limit(parameter.Take).ToListAsync();
            }
            else
            {
                var filter = Builders<BaseEntity>.Filter.NearSphere(x => x.Location, parameter.Longitude.Value, parameter.Latitude.Value, parameter.RadiusDeg)
                    & Builders<BaseEntity>.Filter.Eq(x => x.IsActive, true);
                result = await _collection.Find(filter).Limit(parameter.Take).ToListAsync();
            }
            return result;

        }

        public async Task<ICollection<BaseEntity>> GetFullNearestAsync(LocationParameter parameter)
        {
            ICollection<BaseEntity> result;
            if (parameter.Longitude == null || parameter.Latitude == null)
            {
                result = await _collection.Find(x => true).Skip(parameter.Skip).Limit(parameter.Take).ToListAsync();
            }
            else
            {
                var filter = Builders<BaseEntity>.Filter.NearSphere(x => x.Location, parameter.Longitude.Value, parameter.Latitude.Value, parameter.RadiusDeg)
                             & Builders<BaseEntity>.Filter.Eq(x => x.IsActive, true);
                result = await _collection.Find(filter).Skip(parameter.Skip).Limit(parameter.Take).ToListAsync();
            }

            return result;
        }

        public async Task<ICollection<BaseEntity>> GetSpecificNavigationAsync(string[] logins)
        {
            //todo придумать адекватную проекцию для геопозиции
            var projection = Builders<BaseEntity>.Projection
                .Exclude(x => x.Description)
                .Exclude(x => x.Name)
                .Exclude(x => x.Address)
                .Exclude(x => x.VideoInfos)
                .Exclude(x => x.YouTubeLink)
                .Exclude(x => x.FacebookLink)
                .Exclude(x => x.SoundCloudLink)
                .Exclude(x => x.VkLink);

            var filter = Builders<BaseEntity>.Filter.In(x => x.Login, logins);
            var result = await _collection.Find(filter).Project<BaseEntity>(projection).ToListAsync();
            return result;
        }

        public async Task<ICollection<BaseEntity>> GetSpecificFullAsync(string[] logins)
        {
            var filter = Builders<BaseEntity>.Filter.In(x => x.Login, logins);
            var result = await _collection.Find(filter).ToListAsync();
            return result;
        }

        public async Task<ICollection<string>> GetUserEntitiesLoginsAsync(string userLogin)
        {
            //db.bases.find({"user":"rogulenkoko"},{"log":1})
            var filter = Builders<BaseEntity>.Filter.Eq(x => x.UserLogin, userLogin);
            var projection = Builders<BaseEntity>.Projection.Include(x => x.Login);
            var entities = await _collection.Find(filter).Project<BaseEntity>(projection).ToListAsync();
            var result = entities.Select(x => x.Login).ToList();
            return result;
        }

        public async Task<ICollection<UserEntitiesCountInfo>> GetUserEntitiesCountInfoAsync(string userLogin)
        {
            //db.bases.aggregate([
            //{ $match: { user: "rogulenkoko" } },
            //{ $group: { _id: "$t", count: {$sum: 1} } } 
            //])

            var countResult = await _collection.Aggregate()
                .Match(x => x.UserLogin == userLogin)
                .Group(x => x.EntityType, y => new UserEntitiesCountInfo()
                {
                    EntityType = y.Key,
                    Count = y.Count(),
                    Logins = y.Select(x => x.Login).ToList()
                }).ToListAsync();

            return countResult;
        }

        public async Task<ICollection<FileInfo>> GetFilesAsync(string[] fileIds)
        {
            //todo оптимизация
            var fileInfos = new List<FileInfo>(fileIds.Length);
            foreach (var id in fileIds)
            {
                try
                {
                    var bytes = await _bucket.DownloadAsBytesAsync(new ObjectId(id));
                    fileInfos.Add(new FileInfo() { Id = id, Bytes = bytes });
                }
                catch (GridFSFileNotFoundException e)
                {
                   continue;
                }
            }
            return fileInfos;
        }

        public async Task<ICollection<BaseEntity>> GetFilteredAsync(CommonFilterParameter commonFilter, IFilterParameter parameter = null)
        {
            var filter = CreateFilter(commonFilter, parameter);

            if (commonFilter.Longitude.HasValue && commonFilter.Latitude.HasValue)
            {
                filter = filter & Builders<BaseEntity>.Filter.NearSphere(x => x.Location, commonFilter.Longitude.Value, commonFilter.Latitude.Value, commonFilter.RadiusDeg);
            }

            var result = await _collection.Find(filter).Skip(commonFilter.Skip).Limit(commonFilter.Take).ToListAsync();
            return result;
        }

        public async Task<ICollection<string>> GetAllFilteredLoginsAsync(CommonFilterParameter commonFilter, IFilterParameter parameter)
        {
            var filter = CreateFilter(commonFilter, parameter);
            var profection = Builders<BaseEntity>.Projection.Include(x => x.Login);

            if (commonFilter.Longitude.HasValue && commonFilter.Latitude.HasValue)
            {
                filter = filter & Builders<BaseEntity>.Filter.NearSphere(x => x.Location, commonFilter.Longitude.Value, commonFilter.Latitude.Value, commonFilter.RadiusDeg);
            }

            var result = await _collection.Find(filter).Project<BaseEntity>(profection).Limit(commonFilter.Limit).ToListAsync();
            return result.Select(x => x.Login).ToList();
        }

        public async Task<bool> CheckIfLoginExistAsync(string login)
        {
            var projection = Builders<BaseEntity>.Projection.Include(x => x.Login);
            var entity = await _collection.Find(x => x.Login == login).Project(projection).FirstOrDefaultAsync();
            return entity != null;
        }

        private FilterDefinition<BaseEntity> CreateFilter(CommonFilterParameter commonFilter, IFilterParameter parameter = null)
        {
            var filter = Builders<BaseEntity>.Filter.Empty;

            filter = filter & Builders<BaseEntity>.Filter.Eq(x => x.IsActive, true);

            if (parameter != null)
            {
                filter = filter & _filterFactory.CreateFilter(parameter);
            }

            if (commonFilter.EntityType != 0)
            {
                filter = filter & Builders<BaseEntity>.Filter.Eq(x => x.EntityType, commonFilter.EntityType);
            }

            if (!String.IsNullOrEmpty(commonFilter.SearchText))
            {
                filter = filter & Builders<BaseEntity>.Filter.Regex(x => x.Name, $"/{commonFilter.SearchText}/i");
            }

            if (!String.IsNullOrEmpty(commonFilter.UserLogin))
            {
                filter = filter & Builders<BaseEntity>.Filter.Eq(x => x.UserLogin, commonFilter.UserLogin);
            }

            return filter;
        }

        public override async Task UpdateAsync(BaseEntity entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            if (entity.Photo != null && entity.Photo.Image != null && entity.Photo.Image.AsByteArray.Length != 0)
            {
                var imageBytes = entity.Photo.Image.AsByteArray;

                var fileName = ImageNameBuilder.BuildAvatarName(entity);
                var imageBytesNormal = FunkmapImageProcessor.MinifyImage(imageBytes, 200);
                var photoId = await _bucket.UploadFromBytesAsync(fileName, imageBytesNormal);
                entity.PhotoId = photoId;

                var fileMiniName = ImageNameBuilder.BuildAvatarMiniName(entity);
                var imageMiniBytes = FunkmapImageProcessor.MinifyImage(imageBytes);
                var photoMiniId = await _bucket.UploadFromBytesAsync(fileMiniName, imageMiniBytes);
                entity.PhotoMiniId = photoMiniId;
            }
            else if (entity.Photo != null && (entity.Photo.Image == null || entity.Photo.Image.AsByteArray.Length == 0))
            {
                entity.PhotoId = null;
                entity.PhotoMiniId = null;
            }

            var filter = Builders<BaseEntity>.Filter.Eq(x => x.Login, entity.Login) & Builders<BaseEntity>.Filter.Eq(x => x.EntityType, entity.EntityType);

            await _collection.ReplaceOneAsync(filter, entity);
        }

        public override async Task<BaseEntity> DeleteAsync(string id)
        {
            var filter = Builders<BaseEntity>.Filter.Eq(x => x.Login, id);
            var entity = await _collection.FindOneAndDeleteAsync(filter);
            if (entity == null) return null; //todo подумать как лучше, может просить эксепшен

            if (entity.PhotoId.HasValue) _bucket.DeleteAsync(entity.PhotoId.Value);
            if (entity.PhotoMiniId.HasValue) _bucket.DeleteAsync(entity.PhotoMiniId.Value);

            return entity;
        }

        public async Task UpdateFavoriteAsync(UpdateFavoriteParameter parameter)
        {
            if(String.IsNullOrEmpty(parameter?.EntityLogin) || String.IsNullOrEmpty(parameter?.UserLogin)) throw new ArgumentNullException(nameof(parameter));

            FilterDefinition<BaseEntity> filter = Builders<BaseEntity>.Filter.Eq(x => x.Login, parameter.EntityLogin);
            UpdateDefinition<BaseEntity> update;

            if (parameter.IsFavorite)
            {
                filter = filter & Builders<BaseEntity>.Filter.AnyNe(x => x.FavoriteFor, parameter.UserLogin);
                var favoriteProjection = Builders<BaseEntity>.Projection.Include(x => x.FavoriteFor);

                //todo удалить как будет понятно как задать дефолтное значение для коллекции
                var entity = await _collection.Find(filter).Project<BaseEntity>(favoriteProjection).SingleOrDefaultAsync();
                if (entity?.FavoriteFor == null)
                {
                    var emptyArrayFilter = Builders<BaseEntity>.Filter.Eq(x => x.Login, parameter.EntityLogin);
                    var emptyArrayUpdate = Builders<BaseEntity>.Update.Set(x=>x.FavoriteFor, new List<string>() {parameter.UserLogin});
                    await _collection.UpdateOneAsync(emptyArrayFilter, emptyArrayUpdate);
                }

                update = Builders<BaseEntity>.Update.Push(x => x.FavoriteFor, parameter.UserLogin);
            }
            else
            {
                filter = filter & Builders<BaseEntity>.Filter.AnyEq(x => x.FavoriteFor, parameter.UserLogin);
                update = Builders<BaseEntity>.Update.Pull(x => x.FavoriteFor, parameter.UserLogin);
            }

            await _collection.FindOneAndUpdateAsync(filter, update);
        }

        public async Task<ICollection<string>> GetFavoritesLoginsAsync(string userLogin)
        {
            var filter = Builders<BaseEntity>.Filter.AnyEq(x => x.FavoriteFor, userLogin);
            var projection = Builders<BaseEntity>.Projection.Include(x => x.Login);
            var favorites = await _collection.Find(filter).Project<BaseEntity>(projection).ToListAsync();
            return favorites?.Select(x=>x.Login).ToList();
        }
    }
}
