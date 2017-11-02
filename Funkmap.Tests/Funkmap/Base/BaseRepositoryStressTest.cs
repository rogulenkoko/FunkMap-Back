﻿using System.Collections.Generic;
using Funkmap.Data.Entities.Abstract;
using Funkmap.Data.Repositories;
using Funkmap.Data.Repositories.Abstract;
using Funkmap.Data.Services;
using Funkmap.Data.Services.Abstract;
using Funkmap.Tests.Funkmap.Data;
using Funkmap.Tests.Funkmap.Stress;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Funkmap.Tests.Funkmap.Base
{
    [TestClass]
    public class EntityRepositoryStressTest
    {
        private IBaseRepository _baseRepository;

        [TestInitialize]
        public void Initialize()
        {
            var filterServices = new List<IFilterService>() { new MusicianFilterService() };
            IFilterFactory factory = new FilterFactory(filterServices);
            var db = FunkmapTestDbProvider.DropAndCreateDatabase;
            _baseRepository = new BaseRepository(db.GetCollection<BaseEntity>(CollectionNameProvider.BaseCollectionName), FunkmapTestDbProvider.GetGridFsBucket(db), factory);

        }

        [TestMethod]
        public void GetAll()
        {
            var all = _baseRepository.GetAllAsyns().Result;
            Assert.AreEqual(all.Count, 14);
        }
    }
}