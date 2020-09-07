using System;

namespace NaturalEventsViewer.Domain
{
    /// <summary>
    /// Scheduler time provider
    /// </summary>
    public interface ICurrentTimeProvider
    {
        /// <summary>
        /// Get current time
        /// </summary>
        /// <returns></returns>
        DateTime GetCurrentTime();
    }
}
