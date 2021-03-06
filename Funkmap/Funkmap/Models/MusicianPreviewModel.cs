﻿using System.Collections.Generic;
using Funkmap.Domain.Enums;

namespace Funkmap.Models
{
    public class MusicianPreviewModel : ProfilePreview
    {
        public Expiriences Expirience { get; set; }
        public List<Styles> Styles { get; set; }
        public string YouTubeLink { get; set; }
        public Instruments Instrument { get; set; }
    }
}
