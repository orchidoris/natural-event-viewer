using System;

namespace NaturalEventsViewer.Domain
{
    public class CurrentTimeProvider : ICurrentTimeProvider
    {
        public DateTime GetCurrentTime()
        {
            return DateTime.UtcNow;
        }
    }
}
