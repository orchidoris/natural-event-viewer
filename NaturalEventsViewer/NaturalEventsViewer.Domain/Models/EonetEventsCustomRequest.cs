using Eonet.Core.Models;

namespace NaturalEventsViewer.Domain.Models
{
    public class EonetEventsCustomRequest : EonetEventsApiRequest
    {
        public string[] Categories { get; set; }
        public string TitleSearch { get; set; }
        public EonetEventOrderAttribute[] Ordering { get; set; }

    }
}
