﻿using System;
using System.Collections.Generic;
using System.Linq;
using Funkmap.Messenger.Data.Entities;
using Funkmap.Messenger.Data.Objects;
using Funkmap.Messenger.Models;
using MongoDB.Bson;

namespace Funkmap.Messenger.Mappers
{
    public static class DialogMapper
    {
        public static Dialog ToModel(this DialogEntity source, string userLogin, Message lastMessage = null)
        {
            if (source == null) return null;
            return new Dialog()
            {
                DialogId = source.Id.ToString(),
                Name = source.Name,
                LastMessage = lastMessage,
                Participants = source.Participants
            };
        }

        public static DialogsNewMessagesCountModel ToModel(this DialogsNewMessagesCountResult source)
        {
            if (source == null) return null;
            return new DialogsNewMessagesCountModel()
            {
                DialogId = source.DialogId.ToString(),
                NewMessagesCount = source.NewMessagesCount
            };
        }

        public static DialogEntity ToEntity(this Dialog source)
        {
            if (source == null) return null;
            return new DialogEntity()
            {
                Name = source.Name,
                Participants = source.Participants
            };
        }
    }
}
