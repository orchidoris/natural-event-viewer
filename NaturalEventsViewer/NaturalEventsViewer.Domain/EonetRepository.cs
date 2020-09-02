using Eonet.Core;
using Eonet.Core.Models;
using NaturalEventsViewer.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NaturalEventsViewer.Domain
{
    public class EonetRepository : IEonetRepository
    {
        private IEonetApiClient _apiClient;

        public EonetRepository(IEonetApiClient apiClient) {
            _apiClient = apiClient;
        }

        public async Task<EonetEventsResponse> GetEvents(EonetEventsCustomRequest request) {
            EonetEventsResponse eventsResponse = await GetBothOpenAndClosedEvents(request); // there is a hack inside this method

            PopulateAuxiliaryProperties(eventsResponse.Events);
            request.TitleSearch = (request.TitleSearch ?? "").Trim();
            IEnumerable<EonetEvent> events = ApplyCustomFilters(eventsResponse.Events, request);
            IEnumerable<EonetEvent> orderedEvents = ApplyOrdering(events, request.Ordering);

            var finalEvents = (request.Limit.HasValue
                    ? orderedEvents.Take(request.Limit.Value) // Limit is re-applied due to hack in GetOpenClosedEventsTogether method
                    : orderedEvents)
                .ToArray();

            eventsResponse.Events = finalEvents;
            eventsResponse.TitleSearch = request.TitleSearch;
            eventsResponse.TotalCount = finalEvents.Count();
            return eventsResponse;
        }

        public async Task<EonetEvent> GetSingeEvent(string id)
        {
            var allEvents = await GetBothOpenAndClosedEvents();
            PopulateAuxiliaryProperties(allEvents.Events);
            var singleEvent = allEvents.Events.Where(e => e.Id == id).FirstOrDefault();
            return singleEvent;
        }

        public async Task<EonetCategory[]> GetCurrentlyAvailableCategories()
        {
            var allEvents = await GetBothOpenAndClosedEvents();
            var categories = allEvents.Events
                .SelectMany(e => e.Categories.Select(c => new { c.Id, c.Title }))
                .GroupBy(e => e.Id, e => e.Title)
                .Select(c => new EonetCategory
                {
                    Id = c.Key,
                    Title = c.First()
                })
                .ToArray();

            return categories;
        }

        public async Task<string[]> GetCurrentlyAvailableSourceIds()
        {
            var allEvents = await GetBothOpenAndClosedEvents();
            var sources = allEvents.Events
                .SelectMany(e => e.Sources.Select(c => c.Id))
                .Distinct()
                .ToArray();

            return sources;
        }

        private async Task<EonetEventsResponse> GetBothOpenAndClosedEvents(EonetEventsApiRequest request = null)
        {
            if (request == null) request = new EonetEventsApiRequest();

            var eventsResponse = await _apiClient.GetEventsAsync(request);

            // If status=null previous call returns only open events, so we override this behaviour by calling endpoint twice
            // Only if status is explicitly 'open' or 'closed' then only one request is performed            
            if (request.Status == null)
            {
                request.Status = EonetEventStatus.Closed;
                var closedEventsResponse = await _apiClient.GetEventsAsync(request);
                request.Status = null;

                // This hack breaks api-level Limit filter, so it need to be reapplied if present
                eventsResponse.Events = eventsResponse.Events.Concat(closedEventsResponse.Events).ToArray();
            }

            return eventsResponse;
        }

        private IEnumerable<EonetEvent> ApplyCustomFilters(IEnumerable<EonetEvent> events, EonetEventsCustomRequest request)
        {
            if (request == null)
            {
                return events;
            }

            if (request.Categories != null && request.Categories.Length > 0)
            {
                events = events.Where(e => e.Categories.Any(c => request.Categories.Contains(c.Id)));
            }

            if (!string.IsNullOrWhiteSpace(request.TitleSearch))
            {
                events = events.Where(e => e.Title.ToLower().Contains(request.TitleSearch.ToLower()));
            }

            return events;
        }

        private IEnumerable<EonetEvent> ApplyOrdering(IEnumerable<EonetEvent> events, EonetEventOrderAttribute[] orderAttributes)
        {
            if (orderAttributes == null || orderAttributes.Length == 0)
            {
                return events;
            }

            if (orderAttributes.GroupBy(so => so.AttributeType).Any(g => g.Count() > 1))
            {
                throw new ArgumentException("The same attribute included more than one time into ordering.");
            }

            IOrderedEnumerable<EonetEvent> orderedEvents = null;
            foreach (var attribute in orderAttributes)
            {
                switch (attribute.AttributeType)
                {
                    // TODO: Refactor it to decrease amount of duplicated code by using delegates
                    //       Make sure delegates usage doesn't decrease readability

                    case EonetEventOrderAttributeType.Id:
                        orderedEvents = orderedEvents == null
                            ? (attribute.IsDescending ? events.OrderByDescending(e => e.Id) : events.OrderBy(e => e.Id))
                            : (attribute.IsDescending ? orderedEvents.ThenByDescending(e => e.Id) : orderedEvents.ThenBy(e => e.Id));
                        break;
                    case EonetEventOrderAttributeType.Title:
                        orderedEvents = orderedEvents == null
                            ? (attribute.IsDescending ? events.OrderByDescending(e => e.Title) : events.OrderBy(e => e.Title))
                            : (attribute.IsDescending ? orderedEvents.ThenByDescending(e => e.Title) : orderedEvents.ThenBy(e => e.Title));
                        break;
                    case EonetEventOrderAttributeType.Category:
                        orderedEvents = orderedEvents == null
                            ? (attribute.IsDescending ? events.OrderByDescending(e => e.Categories.Min(c => c.Title)) : events.OrderBy(e => e.Categories.Max(c => c.Title)))
                            : (attribute.IsDescending ? orderedEvents.ThenByDescending(e => e.Categories.Min(c => c.Title)) : orderedEvents.ThenBy(e => e.Categories.Max(c => c.Title)));
                        break;
                    case EonetEventOrderAttributeType.Source:
                        orderedEvents = orderedEvents == null
                            ? (attribute.IsDescending ? events.OrderByDescending(e => e.Sources.Min(c => c.Id)) : events.OrderBy(e => e.Sources.Max(c => c.Id)))
                            : (attribute.IsDescending ? orderedEvents.ThenByDescending(e => e.Sources.Min(c => c.Id)) : orderedEvents.ThenBy(e => e.Sources.Max(c => c.Id)));
                        break;
                    case EonetEventOrderAttributeType.LastGeographyDate:
                        orderedEvents = orderedEvents == null
                            ? (attribute.IsDescending ? events.OrderByDescending(e => e.LastGeometryDate) : events.OrderBy(e => e.LastGeometryDate))
                            : (attribute.IsDescending ? orderedEvents.ThenByDescending(e => e.LastGeometryDate) : orderedEvents.ThenBy(e => e.LastGeometryDate));
                        break;
                    case EonetEventOrderAttributeType.Status:
                        orderedEvents = orderedEvents == null
                            ? (attribute.IsDescending ? events.OrderByDescending(e => e.Status) : events.OrderBy(e => e.Status))
                            : (attribute.IsDescending ? orderedEvents.ThenByDescending(e => e.Status) : orderedEvents.ThenBy(e => e.Status));
                        break;
                    case EonetEventOrderAttributeType.ClosedDate:
                        orderedEvents = orderedEvents == null
                            ? (attribute.IsDescending ? events.OrderByDescending(e => e.Closed) : events.OrderBy(e => e.Closed))
                            : (attribute.IsDescending ? orderedEvents.ThenByDescending(e => e.Closed) : orderedEvents.ThenBy(e => e.Closed));
                        break;
                }
            }

            return orderedEvents;
        }

        private void PopulateAuxiliaryProperties(IEnumerable<EonetEvent> events)
        {
            foreach (var naturalEvent in events)
            {
                naturalEvent.LastGeometryDate = naturalEvent.Geometry.Max(g => g.Date);
                naturalEvent.Status = naturalEvent.Closed == null ? EonetEventStatus.Open : EonetEventStatus.Closed;

                // TODO: Improve, as it is temp hack to simplify filtering as all events but one have 1 category
                if (naturalEvent.Categories.Count() > 1)
                {
                    naturalEvent.Categories = new[] { naturalEvent.Categories[1] };
                }
            }
        }
    }
}
