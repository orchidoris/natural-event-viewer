namespace Eonet.Core.Models
{
    public class EonetEventsResponse
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public EonetEvent[] Events { get; set; }
    }
}
