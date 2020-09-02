namespace Eonet.Core.Models
{
    public class EonetEventsApiRequest
    {
        /// <summary>
        /// Limits the number of events returned
        /// Example: https://eonet.sci.gsfc.nasa.gov/api/v2.1/events ?limit=5
        /// </summary>
        public int? Limit { get; set; }

        /// <summary>
        /// Limit the number of prior days (including today) from which events will be returned.
        /// Example: https://eonet.sci.gsfc.nasa.gov/api/v2.1/events ?days=20
        /// </summary>
        public int? Days { get; set; }

        /// <summary>
        /// Filter the returned events by the Source.
        /// Multiple sources can be included in the parameter: comma separated, operates as a boolean OR.
        /// Example: https://eonet.sci.gsfc.nasa.gov/api/v2.1/events?source=InciWeb,EO
        /// </summary>
        public string[] Sources { get; set; } = new string[0];

        /// <summary>
        /// Events that have ended are assigned a closed date and the existence
        /// of that date will allow you to filter for only-open or only-closed events.
        /// Omitting the status parameter will return only the currently open events.
        /// Example: https://eonet.sci.gsfc.nasa.gov/api/v2.1/events ?status=open
        /// </summary>
        public EonetEventStatus? Status { get; set; }
    }
}
