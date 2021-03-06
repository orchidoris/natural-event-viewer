﻿using System;

namespace Eonet.Core.Models
{
    public class EonetEvent
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public DateTime? Closed { get; set; }
        public EonetCategory[] Categories { get; set; }
        public EonetSource[] Sources { get; set; }
        public EonetGeometry[] Geometry { get; set; }
    }
}
