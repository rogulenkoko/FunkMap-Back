﻿using System;
using System.Collections.Generic;
using Funkmap.Auth.Data;
using Funkmap.Auth.Data.Entities;
using Funkmap.Data.Entities;
using Funkmap.Data.Repositories;
using Funkmap.Module.Auth;
using Funkmap.Tests.Images;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Driver;
using MongoDB.Driver.GeoJsonObjectModel;

namespace Funkmap.Tests.Funkmap.Data
{
    [TestClass]
    public class FunkmapDataSeeder
    {
        private readonly IMongoDatabase _database;

        public FunkmapDataSeeder(IMongoDatabase database)
        {
            _database = database;
        }

        public void SeedData()
        {
            SeedMusicians();
            SeedBands();
            SeedShops();
            
            SeedStudios();
            SeedRehearsalPoints();
        }

        

        private void SeedMusicians()
        {
            var repository = new MusicianRepository(_database.GetCollection<MusicianEntity>(CollectionNameProvider.BaseCollectionName));

            var m1 = new MusicianEntity()
            {
                Sex = Sex.Male,
                Login = "rogulenkoko",
                UserLogin = "test",
                BirthDate = DateTime.Now,
                Description = "Описание",
                Name = "Кирилл Рогуленко",
                Location = new GeoJsonPoint<GeoJson2DGeographicCoordinates>(new GeoJson2DGeographicCoordinates(30,50)),
                Styles = new List<Styles>() {Styles.Funk, Styles.HipHop},
                Instrument = InstrumentType.Brass,
                VkLink = "https://vk.com/id30724049",
                YouTubeLink = "https://www.youtube.com/user/Urgantshow",
                BandLogin = "funkmap",
                ExpirienceType = ExpirienceType.Advanced
            };


            m1.Photo = ImageProvider.GetAvatar("avatar.jpg");

            var m2 = new MusicianEntity()
            {
                Sex = Sex.Female,
                Login = "madlib",
                BirthDate = DateTime.Now,
                Description = "Большое описание музыканта, тудым сюдым. Как дела братва?",
                Name = "Madlib",
                Location = new GeoJsonPoint<GeoJson2DGeographicCoordinates>(new GeoJson2DGeographicCoordinates(30, 51)),
                Styles = new List<Styles>() { Styles.Funk, Styles.Rock},
                Instrument = InstrumentType.Drums,
                FacebookLink = "https://ru-ru.facebook.com/",
                BandLogin = "beatles",
                ExpirienceType = ExpirienceType.Begginer
            };

            var m3 = new MusicianEntity()
            {
                Sex = Sex.Male,
                Login = "razrab",
                BirthDate = DateTime.Now,
                Description = "Razrab описание!!!",
                Name = "Razrab Razrab",
                Location = new GeoJsonPoint<GeoJson2DGeographicCoordinates>(new GeoJson2DGeographicCoordinates(31, 51)),
                Styles = new List<Styles>() { Styles.HipHop},
                Instrument = InstrumentType.Keyboard,
                BandLogin = "metallica",
                ExpirienceType = ExpirienceType.SuperStar
            };

            repository.CreateAsync(m1).Wait();
            repository.CreateAsync(m2).Wait();
            repository.CreateAsync(m3).Wait();
        }

        private void SeedBands()
        {
            var repository = new BandRepository(_database.GetCollection<BandEntity>(CollectionNameProvider.BaseCollectionName));

            var b1 = new BandEntity()
            {
                UserLogin = "test",
                DesiredInstruments = new List<InstrumentType>() { InstrumentType.Bass, InstrumentType.Guitar},
                Name = "The Beatles",
                Login = "beatles",
                VideoLinks = new List<string>() { "firstVideo", "secondVideo" },
                Location = new GeoJsonPoint<GeoJson2DGeographicCoordinates>(new GeoJson2DGeographicCoordinates(29, 52)),
                MusicianLogins = new List<string>() { "rogulenkoko", "razrab"},
                Styles = new List<Styles>() { Styles.Funk, Styles.HipHop},
                Description = "Даже не буду ничего говорить",
                VkLink = "vk"
            };

            b1.Photo = ImageProvider.GetAvatar("beatles-avatar.jpg");

            var b2 = new BandEntity()
            {
                UserLogin = "rogulenkoko",
                Name = "Red Hot Chili Peppers",
                Login = "rhcp",
                VideoLinks = new List<string>() { "firstVideo", "secondVideo" },
                Location = new GeoJsonPoint<GeoJson2DGeographicCoordinates>(new GeoJson2DGeographicCoordinates(28, 52)),
                MusicianLogins = new List<string>() { "rogulenkoko" },
                Styles = new List<Styles>() { Styles.Funk, Styles.Rock },
                Description = "Мы жгучие перцы из солнечной калифорнии и этим все сказано",
                YouTubeLink = "yt"
            };

            var b3 = new BandEntity()
            {
                UserLogin = "rogulenkoko",
                Name = "Coldplay",
                Login = "coldplay",
                Location = new GeoJsonPoint<GeoJson2DGeographicCoordinates>(new GeoJson2DGeographicCoordinates(31, 50)),
                Styles = new List<Styles>() { Styles.Rock },
                Description = "Мы грустная группа которая играет холодно",
                FacebookLink = "asda",
                VkLink = "aaa"
            };

            repository.CreateAsync(b1).Wait();
            repository.CreateAsync(b2).Wait();
            repository.CreateAsync(b3).Wait();
        }

        private void SeedShops()
        {
            var repository = new ShopRepository(_database.GetCollection<ShopEntity>(CollectionNameProvider.BaseCollectionName));

            var s1 = new ShopEntity()
            {
                Login = "guitars",
                Name = "Гитарушки",
                Location = new GeoJsonPoint<GeoJson2DGeographicCoordinates>(new GeoJson2DGeographicCoordinates(32, 52)),
                Website = "https://ru.wikipedia.org/wiki/C_Sharp",
                Description = "Небольшой магазин с гитарами и прочим барахлом",
                VkLink = "qwe",
                Address = "пр-т Независимости 12"
            };

            var s2 = new ShopEntity()
            {
                Login = "pinkponk",
                Name = "Пинк и Понк",
                Location = new GeoJsonPoint<GeoJson2DGeographicCoordinates>(new GeoJson2DGeographicCoordinates(33, 51)),
                Website = "http://online-simpsons.ru",
                VkLink = "qwe"
            };

            var s3 = new ShopEntity()
            {
                Login = "monkeyb",
                Name = "Monkey Business",
                Location = new GeoJsonPoint<GeoJson2DGeographicCoordinates>(new GeoJson2DGeographicCoordinates(31, 54)),
                Website = "https://сайт.com",
                Description = "Большой магазин с гитарами и прочим барахлом",
                YouTubeLink = "asdasda",
                Address = "пр-т Каменоостровский 11"
            };
            var s4 = new ShopEntity()
            {
                Login = "oneshop",
                Name = "One-shop",
                Location = new GeoJsonPoint<GeoJson2DGeographicCoordinates>(new GeoJson2DGeographicCoordinates(31, 51)),
                Website = "http://tttt.ru",
                YouTubeLink = "asdasda",
                FacebookLink = "asdasda",
                Address = "пр-т Невский 5"
            };

            repository.CreateAsync(s1).Wait();
            repository.CreateAsync(s2).Wait();
            repository.CreateAsync(s3).Wait();
            repository.CreateAsync(s4).Wait();
        }

        private void SeedStudios()
        {
            var repository = new StudioRepository(_database.GetCollection<StudioEntity>(CollectionNameProvider.BaseCollectionName));

            var s1 = new StudioEntity()
            {
                Login = "blackstar",
                Name = "Black Star",
                VkLink = "vk",
                Location = new GeoJsonPoint<GeoJson2DGeographicCoordinates>(new GeoJson2DGeographicCoordinates(25, 24)),
                UserLogin = "rogulenkoko",
                Address = "ул. Мовчанского 12"
            };

            var s2 = new StudioEntity()
            {
                Login = "gaz",
                Name = "Gazgolder",
                FacebookLink = "face",
                Location = new GeoJsonPoint<GeoJson2DGeographicCoordinates>(new GeoJson2DGeographicCoordinates(25, 27)),
                UserLogin = "test",
                Address = "ул. Торжковская 15",
                Description = "Супер клевая студия на которой записываются суперклевые пацаны"
            };

            repository.CreateAsync(s1).Wait();
            repository.CreateAsync(s2).Wait();
        }

        private void SeedRehearsalPoints()
        {
            var repository = new RehearsalPointRepository(_database.GetCollection<RehearsalPointEntity>(CollectionNameProvider.BaseCollectionName));
            var r1 = new RehearsalPointEntity()
            {
                Login = "monkey",
                Name = "Monkey Business",
                Address = "пр-т Мира 12",
                Location = new GeoJsonPoint<GeoJson2DGeographicCoordinates>(new GeoJson2DGeographicCoordinates(27, 27)),
                UserLogin = "test",
                YouTubeLink = "yout",
                Description = "Точка при магазине, супер оборудование и все такое еу еу"
            };
            

            var r2 = new RehearsalPointEntity()
            {
                Login = "grandsound",
                Name = "Grand Sound",
                Location = new GeoJsonPoint<GeoJson2DGeographicCoordinates>(new GeoJson2DGeographicCoordinates(29, 27)),
                UserLogin = "test",
                YouTubeLink = "yout",
            };

            repository.CreateAsync(r1).Wait();
            repository.CreateAsync(r2).Wait();
        }
    }
}
