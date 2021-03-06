﻿using System.Collections.Generic;
using Funkmap.Data.Entities.Entities.Abstract;
using Funkmap.Domain.Enums;
using MongoDB.Bson.Serialization.Attributes;
using Funkmap.Domain;

namespace Funkmap.Data.Entities.Entities
{
    public class BandEntity : BaseEntity
    {
        public BandEntity()
        {
            EntityType = EntityType.Band;
        }

        [BsonElement("dinstr")]
        [BsonIgnoreIfDefault]
        public List<Instruments> DesiredInstruments { get; set; }

        [BsonElement("mus")]
        [BsonIgnoreIfDefault]
        public List<string> MusicianLogins{ get; set; }

        [BsonElement("stls")]
        [BsonIgnoreIfDefault]
        public List<Styles> Styles { get; set; }

        [BsonElement("inv")]
        [BsonIgnoreIfDefault]
        public List<string> InvitedMusicians { get; set; }
    }
}
