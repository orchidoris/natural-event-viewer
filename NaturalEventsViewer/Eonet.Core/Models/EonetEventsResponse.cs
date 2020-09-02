using System;

namespace Eonet.Core.Models
{
    public class EonetEventsResponse
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public EonetEvent[] Events { get; set; }

        /// <summary>
        /// Search text used to filter response
        /// </summary>
        /// This is populated in Repository
        public string TitleSearch { get; set; }
        public int TotalCount { get; set; }
    }
}
