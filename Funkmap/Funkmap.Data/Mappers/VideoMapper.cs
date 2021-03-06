﻿using System.Collections.Generic;
using System.Linq;
using Funkmap.Data.Entities.Entities;
using Funkmap.Domain.Models;

namespace Funkmap.Data.Mappers
{
    public static class VideoMapper
    {

        public static List<VideoInfoEntity> ToEntities(this IReadOnlyCollection<VideoInfo> source)
        {
            return source?.Select(x => x.ToEntity()).ToList();
        }

        public static VideoInfoEntity ToEntity(this VideoInfo source)
        {
            if (source == null) return null;

            return new VideoInfoEntity
            {
                Id = source.Id,
                Name = source.Name,
                Type = source.Type,
                Description = source.Description,
                SaveDateUtc = source.SaveDateUtc
            };
        }

        public static VideoInfo ToModel(this VideoInfoEntity source)
        {
            if (source == null) return null;
            return new VideoInfo()
            {
                Id = source.Id,
                Name = source.Name,
                Type = source.Type,
                Description = source.Description,
                SaveDateUtc = source.SaveDateUtc
            };
        }

        public static List<AudioInfoEntity> ToEntities(this IReadOnlyCollection<AudioInfo> source)
        {
            return source?.Select(x => x.ToEntity()).ToList();
        }

        public static AudioInfoEntity ToEntity(this AudioInfo source)
        {
            if (source == null) return null;
            return new AudioInfoEntity()
            {
                Id = source.Id,
                SaveDateUtc = source.Date.ToUniversalTime()
            };
        }

        public static AudioInfo ToModel(this AudioInfoEntity source)
        {
            if (source == null) return null;
            return new AudioInfo
            {
                Id = source.Id,
                Date = source.SaveDateUtc
            };
        }
    }
}
