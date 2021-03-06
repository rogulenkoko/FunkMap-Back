﻿using System;
using System.Collections.Generic;
using Funkmap.Domain;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver.GeoJsonObjectModel;

namespace Funkmap.Data.Entities.Entities.Abstract
{
    [BsonDiscriminator(RootClass = true)]

    [BsonKnownTypes(
        typeof(MusicianEntity),
        typeof(ShopEntity),
        typeof(BandEntity),
        typeof(StudioEntity),
        typeof(RehearsalPointEntity))]
    public class BaseEntity
    {

        public BaseEntity()
        {
            FavoriteFor = new List<string>();
            VideoInfos = new List<VideoInfoEntity>();
            SoundCloudTracks = new List<AudioInfoEntity>();
        }

        [BsonId]
        public string Login { get; set; }

        [BsonElement("user")]
        public string UserLogin { get; set; }

        [BsonElement("n")]
        public string Name { get; set; }

        [BsonElement("t")]
        public EntityType EntityType { get; set; }

        [BsonElement("cd")]
        public DateTime CreationDate { get; set; }

        [BsonElement("loc")]
        public GeoJsonPoint<GeoJson2DGeographicCoordinates> Location { get; set; }

        [BsonElement("a")]
        public string Address { get; set; }

        [BsonElement("p")]
        [BsonIgnoreIfDefault]
        public string AvatarUrl { get; set; }

        [BsonElement("pm")]
        [BsonIgnoreIfDefault]
        public string AvatarMiniUrl { get; set; }

        [BsonElement("ytv")]
        public List<VideoInfoEntity> VideoInfos { get; set; }

        [BsonElement("sct")]
        public List<AudioInfoEntity> SoundCloudTracks { get; set; }

        [BsonElement("d")]
        [BsonIgnoreIfDefault]
        public string Description { get; set; }

        [BsonElement("vk")]
        [BsonIgnoreIfDefault]
        public string VkLink { get; set; }

        [BsonElement("yt")]
        [BsonIgnoreIfDefault]
        public string YouTubeLink { get; set; }

        [BsonElement("fb")]
        [BsonIgnoreIfDefault]
        public string FacebookLink { get; set; }

        [BsonElement("sc")]
        [BsonIgnoreIfDefault]
        public string SoundCloudLink { get; set; }

        [BsonElement("gallery")]
        [BsonIgnoreIfDefault]
        public List<BsonBinaryData> Gallery { get; set; }

        [BsonElement("ia")]
        public bool? IsActive { get; set; }

        [BsonElement("fav")]
        [BsonIgnoreIfDefault]
        public List<string> FavoriteFor { get; set; }

        [BsonElement("prior")]
        public bool IsPriority { get; set; }
    }
}
