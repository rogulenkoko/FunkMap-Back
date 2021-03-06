﻿using Funkmap.Data.Entities.Entities;
using Funkmap.Domain.Models;
using MongoDB.Driver.GeoJsonObjectModel;

namespace Funkmap.Data.Mappers
{
    public static class RehearsalPointMapper
    {

        public static RehearsalPoint ToModel(this RehearsalPointEntity source)
        {
            if (source == null) return null;
            return new RehearsalPoint()
            {
                Login = source.Login,
                Name = source.Name,
                AvatarUrl = source.AvatarUrl,
                AvatarMiniUrl = source.AvatarMiniUrl,
                VkLink = source.VkLink,
                YoutubeLink = source.YouTubeLink,
                FacebookLink = source.FacebookLink,
                WorkingHoursDescription = source.WorkingHoursDescription,
                Description = source.Description,
                Address = source.Address,
                SoundCloudLink = source.SoundCloudLink,
                Location = new Location(source.Location.Coordinates.Latitude, source.Location.Coordinates.Longitude),
                UserLogin = source.UserLogin,
                IsActive = source.IsActive,
                IsPriority = source.IsPriority
            };
        }
        
        public static RehearsalPointEntity ToRehearsalPointEntity(this RehearsalPoint source)
        {
            if (source == null) return null;
            return new RehearsalPointEntity()
            {
                Login = source.Login,
                Description = source.Description,
                FacebookLink = source.FacebookLink,
                SoundCloudLink = source.SoundCloudLink,
                VkLink = source.VkLink,
                Location = source.Location == null
                    ? null
                    : new GeoJsonPoint<GeoJson2DGeographicCoordinates>(new GeoJson2DGeographicCoordinates(source.Location.Longitude, source.Location.Latitude)),
                Name = source.Name,
                YouTubeLink = source.YoutubeLink,
                Address = source.Address,
                IsActive = source.IsActive,
                UserLogin = source.UserLogin
            };
        }
    }
}
