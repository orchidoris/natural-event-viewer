namespace NaturalEventsViewer.Domain.Models
{
    public class EventsResponse
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public Event[] Events { get; set; }
        public string TitleSearch { get; set; }
        public int TotalCount { get; set; }
    }
}


