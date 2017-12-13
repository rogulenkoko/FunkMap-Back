﻿using System.Collections.Generic;
using System.Configuration;
using Autofac;
using Funkmap.Common.Abstract;
using Funkmap.Common.Azure;
using Funkmap.Common.Data.Mongo;
using Funkmap.Data.Entities;
using Funkmap.Data.Entities.Abstract;
using Microsoft.Azure;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;

namespace Funkmap.Module
{
    public partial class FunkmapModule 
    {
        private void RegisterMongoDependiences(ContainerBuilder builder)
        {
            var connectionString = ConfigurationManager.ConnectionStrings["FunkmapMongoConnection"].ConnectionString;
            var databaseName = ConfigurationManager.AppSettings["FunkmapDbName"];
            var mongoClient = new MongoClient(connectionString);

            var databaseIocName = "funkmap";

            builder.Register(x => mongoClient.GetDatabase(databaseName)).As<IMongoDatabase>().Named<IMongoDatabase>(databaseIocName).SingleInstance();


            builder.Register(container => container.ResolveNamed<IMongoDatabase>(databaseIocName).GetCollection<BaseEntity>(CollectionNameProvider.BaseCollectionName))
                .As<IMongoCollection<BaseEntity>>();
            
            builder.Register(container => container.ResolveNamed<IMongoDatabase>(databaseIocName).GetCollection<MusicianEntity>(CollectionNameProvider.BaseCollectionName))
                .As<IMongoCollection<MusicianEntity>>();

            builder.Register(container => container.ResolveNamed<IMongoDatabase>(databaseIocName).GetCollection<BandEntity>(CollectionNameProvider.BaseCollectionName))
                .As<IMongoCollection<BandEntity>>();

            builder.Register(container => container.ResolveNamed<IMongoDatabase>(databaseIocName).GetCollection<ShopEntity>(CollectionNameProvider.BaseCollectionName))
                .As<IMongoCollection<ShopEntity>>();

            builder.Register(container => container.ResolveNamed<IMongoDatabase>(databaseIocName).GetCollection<RehearsalPointEntity>(CollectionNameProvider.BaseCollectionName))
                .As<IMongoCollection<RehearsalPointEntity>>();

            builder.Register(container => container.ResolveNamed<IMongoDatabase>(databaseIocName).GetCollection<StudioEntity>(CollectionNameProvider.BaseCollectionName))
                .As<IMongoCollection<StudioEntity>>();


            var storageName = "funkmapstorage";

            //builder.Register(container =>
            //{
            //    var database = container.ResolveNamed<IMongoDatabase>(databaseIocName);
            //    //database.CreateCollection("fs.files");
            //    //database.CreateCollection("fs.chunks");
            //    return new GridFSBucket(database);

            //}).As<IGridFSBucket>().Named<IGridFSBucket>(storageName);

            //builder.Register(container =>
            //{
            //    var gridFs = container.ResolveNamed<IGridFSBucket>(storageName);
            //    return new GridFsFileStorage(gridFs);
            //}).Named<GridFsFileStorage>(storageName);
            //builder.Register(context => context.ResolveNamed<GridFsFileStorage>(storageName)).As<IFileStorage>().InstancePerDependency();


            builder.Register(container =>
            {
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(CloudConfigurationManager.GetSetting("azureStorage"));
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();
                return new AzureFileStorage(blobClient, storageName);
            }).Named<AzureFileStorage>(storageName).InstancePerDependency();



            builder.Register(context => context.ResolveNamed<AzureFileStorage>(storageName)).As<IFileStorage>().InstancePerDependency();
            


            var loginBaseIndexModel = new CreateIndexModel<BaseEntity>(Builders<BaseEntity>.IndexKeys.Ascending(x => x.Login), new CreateIndexOptions() { Unique = true });
            var entityTypeBaseIndexModel = new CreateIndexModel<BaseEntity>(Builders<BaseEntity>.IndexKeys.Ascending(x => x.EntityType));
            var geoBaseIndexModel = new CreateIndexModel<BaseEntity>(Builders<BaseEntity>.IndexKeys.Geo2DSphere(x => x.Location));


            builder.RegisterBuildCallback(async c =>
            {
                //создание индексов при запуске приложения
                var collection = c.Resolve<IMongoCollection<BaseEntity>>();
                await collection.Indexes.CreateManyAsync(new List<CreateIndexModel<BaseEntity>>
                {
                    loginBaseIndexModel,
                    entityTypeBaseIndexModel,
                    geoBaseIndexModel,
                });
            });
        }
    }
}
