﻿using System;
using System.Collections.Generic;
using Funkmap.Common.Core.Extensions;
using Funkmap.Domain.Models;
using Microsoft.AspNetCore.Http;

namespace Funkmap.Tools
{
    /// <summary>
    /// Converter of relative MongoDB avatar ids to relative urls
    /// </summary>
    public static class AvatarHelper
    {
        /// <summary>
        /// Convert avatars for one model
        /// </summary>
        /// <param name="request">Current http-request</param>
        /// <param name="model">Model</param>
        public static void SetProfileCorrectAvatarUrls(this HttpRequest request, IHasAvatar model)
        {
            if (model == null) return;
            model.AvatarUrl = request.ToAbsoluteMediaUrl(model.AvatarUrl, $"api/base/avatar/{model.Login}?date={DateTime.UtcNow}");
            model.AvatarMiniUrl = request.ToAbsoluteMediaUrl(model.AvatarMiniUrl, $"api/base/avatar/{model.Login}?date={DateTime.UtcNow}");
        }

        /// <summary>
        /// Convert avatars for collection of models
        /// </summary>
        /// <param name="request">Current http-request</param>
        /// <param name="models">Collection of models</param>
        public static void SetProfilesCorrectAvatarUrls(this HttpRequest request, IEnumerable<IHasAvatar> models)
        {
            foreach (var model in models)
            {
                request.SetProfileCorrectAvatarUrls(model);
            }
        }
    }
}
