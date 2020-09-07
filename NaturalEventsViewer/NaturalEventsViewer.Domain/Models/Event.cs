using Eonet.Core.Models;
using System;

namespace NaturalEventsViewer.Domain.Models
{
    public class Event
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public DateTime? ClosedDate { get; set; }
        public EonetCategory Category { get; set; }
        public EonetSource[] Sources { get; set; }
        public EonetGeometry[] Geometry { get; set; }
        public EonetEventStatus Status { get; set; } = EonetEventStatus.Open;
        public DateTime LastDate { get; set; }
    }
}
