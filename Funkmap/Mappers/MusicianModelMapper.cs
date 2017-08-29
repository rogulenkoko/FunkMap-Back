﻿using System;
using System.Linq;
using Funkmap.Data.Entities;
using Funkmap.Models;
using MongoDB.Driver.GeoJsonObjectModel;

namespace Funkmap.Mappers
{
    public static class MusicianModelMapper
    {
        public static MusicianModel ToMusicianModel(this MusicianEntity source)
        {
            if (source == null) return null;
            int? age = source.BirthDate == null ? 0 : (int)Math.Round((DateTime.Now - source.BirthDate.Value).TotalDays / 365);
            return new MusicianModel()
            {
                Login = source.Login,
                Description = source.Description,
                Latitude = source.Location.Coordinates.Latitude,
                Longitude = source.Location.Coordinates.Longitude,
                Name = source.Name,
                Sex = source.Sex,
                BirthDate = source.BirthDate,
                Age = age.Value == 0 ? null : age,
                Styles = source.Styles.ToArray(),
                Avatar = source.Photo?.Bytes,
                VideosYoutube = source.YouTubeVideoLins,
                VkLink = source.VkLink,
                FacebookLink = source.FacebookLink,
                Instrument = source.Instrument,
                Expirience = source.ExpirienceType,
                SoundCloudLink = source.SoundCloudLink,
                YoutubeLink = source.YouTubeLink,
                Address = source.Address

            };
        }

        public static MusicianEntity ToMusicianEntity(this MusicianModel source)
        {
            if (source == null) return null;
            
            return new MusicianEntity()
            {
                Login = source.Login,
                Description = source.Description,
                Location = source.Longitude != 0 && source.Latitude != 0 ? new GeoJsonPoint<GeoJson2DGeographicCoordinates>(new GeoJson2DGeographicCoordinates(source.Longitude, source.Latitude)) : null,
                Name = source.Name,
                Sex = source.Sex,
                BirthDate = source.BirthDate,
                Styles = source.Styles?.ToList(),
                Photo = source.Avatar ?? new byte[] {},
                YouTubeVideoLins = source.VideosYoutube?.Where(x=> !String.IsNullOrEmpty(x)).Distinct().ToList(),
                YouTubeLink = source.YoutubeLink,
                VkLink = source.VkLink,
                FacebookLink = source.FacebookLink,
                Instrument = source.Instrument,
                ExpirienceType = source.Expirience,
                SoundCloudLink = source.SoundCloudLink,
                Address = source.Address
            };
        }

        public static MusicianPreviewModel ToPreviewModel(this MusicianEntity source)
        {
            if (source == null) return null;
            return new MusicianPreviewModel()
            {
                Login = source.Login,
                Styles = source.Styles?.ToArray(),
                Name = source.Name,
                Avatar = source.Photo?.AsByteArray,
                Expirience = source.ExpirienceType,
                VkLink = source.VkLink,
                YouTubeLink = source.YouTubeLink,
                FacebookLink = source.FacebookLink,
                SoundCloudLink = source.SoundCloudLink
            };
        }
    }
}
