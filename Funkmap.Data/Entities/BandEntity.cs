﻿using System.Collections.Generic;
using Funkmap.Common;
using Funkmap.Data.Entities.Abstract;
using MongoDB.Bson.Serialization.Attributes;

namespace Funkmap.Data.Entities
{
    public class BandEntity : BaseEntity
    {
        public BandEntity()
        {
            EntityType = EntityType.Band;
        }

        [BsonElement("dinstr")]
        [BsonIgnoreIfDefault]
        public List<InstrumentType> DesiredInstruments { get; set; }

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
