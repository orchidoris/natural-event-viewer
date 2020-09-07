namespace NaturalEventsViewer.Domain.Models
{
    public class EventFilter
    {
        /// <summary>
        /// Type defines meaning of the value and teh way of filtering
        /// </summary>
        public EventCustomFilterType Type { get; set; }

        /// <summary>
        /// Contains substring to look for in Title or Description or comma separated list of categories depending on type above
        /// </summary>
        public string Value { get; set; }
    }
}
