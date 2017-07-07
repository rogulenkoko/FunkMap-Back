﻿using System.Collections.Generic;
using Funkmap.Data.Entities;
using Funkmap.Data.Parameters;
using Funkmap.Data.Repositories;
using Funkmap.Data.Repositories.Abstract;
using Funkmap.Tests.Funkmap.Data;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Funkmap.Tests.Funkmap.Musician
{
    [TestClass]
    public class MusicianRepositoryTest
    {
        private IMusicianRepository _musicianRepository;

        [TestInitialize]
        public void Initialize()
        {
            _musicianRepository = new MusicianRepository(FunkmapTestDbProvider.DropAndCreateDatabase.GetCollection<MusicianEntity>(CollectionNameProvider.BaseCollectionName));
        }

        [TestMethod]
        public void GetFilteredMusicianTest()
        {
            var parameter = new MusicianFilterParameter()
            {
                Styles = new List<Styles>() { Styles.Funk },
                Expirience = ExpirienceType.Advanced
            };

            var result = _musicianRepository.GetFilteredMusiciansAsync(parameter).Result;


        }

        [TestMethod]
        public void GetStyleFilteredMusicianTest()
        {
            var parameter = new MusicianFilterParameter()
            {
                Styles = new List<Styles>() { Styles.Funk }
            };

            var result = _musicianRepository.GetFilteredMusiciansAsync(parameter).Result;
            Assert.AreEqual(result.Count, 2);

            parameter.Styles.Add(Styles.Rock);
            result = _musicianRepository.GetFilteredMusiciansAsync(parameter).Result;
            Assert.AreEqual(result.Count, 1);

            parameter.Styles.Add(Styles.HipHop);
            result = _musicianRepository.GetFilteredMusiciansAsync(parameter).Result;
            Assert.AreEqual(result.Count, 0);


        }
    }
}
