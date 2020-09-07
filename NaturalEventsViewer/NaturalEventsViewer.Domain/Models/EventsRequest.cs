using Eonet.Core.Models;

namespace NaturalEventsViewer.Domain.Models
{
    public class EventsRequest
    {
        public int Days { get; set; }
        public EonetEventStatus? Status { get; set; }    
        public string[] Sources { get; set; } = new string[0];
        public string[] Categories { get; set; } = new string[0];
        public string TitleSearch { get; set; }
        public EventOrderAttribute[] Ordering { get; set; } = new EventOrderAttribute[0];
    }
}
