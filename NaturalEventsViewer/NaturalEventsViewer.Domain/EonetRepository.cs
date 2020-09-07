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
        private static readonly Dictionary<OrderingType, Func<Event, object>> OrderingKeySelectors = new Dictionary<OrderingType, Func<Event, object>>
        {
            { new OrderingType(EventOrderAttributeType.Id, 0), e => e.Id },
            { new OrderingType(EventOrderAttributeType.Title, 0), e => e.Title },
            { new OrderingType(EventOrderAttributeType.Category, 0), e => e.Category.Title },
            { new OrderingType(EventOrderAttributeType.LastDate, 0), e => e.LastDate },
            { new OrderingType(EventOrderAttributeType.Status, 0), e => e.Status },
            { new OrderingType(EventOrderAttributeType.ClosedDate, 0), e => e.ClosedDate },

            // key selector depends on ordering direction for Source option
            { new OrderingType(EventOrderAttributeType.Source, 1), e => e.Sources.Min(s => s.Id) },
            { new OrderingType(EventOrderAttributeType.Source, -1), e => e.Sources.Max(s => s.Id) },
        };

        private readonly IEonetApiClient _apiClient;
        private readonly ICurrentTimeProvider _currentDateTimeProvider;
        private readonly int _maxEonetDaysPrior;

        public EonetRepository(
            IEonetApiClient apiClient,
            ICurrentTimeProvider currentDateTimeProvider,
            int maxEonetDaysPrior)
        {
            _apiClient = apiClient;
            _currentDateTimeProvider = currentDateTimeProvider;
            _maxEonetDaysPrior = maxEonetDaysPrior;
        }

        public async Task<EventsResponse> GetEvents(EventsRequest request) {
            try
            {
                EonetEventsResponse apiResponse = await GetBothOpenAndClosedEvents(new EonetEventsRequest()
                {
                    Days = _maxEonetDaysPrior,
                    Status = request.Status
                }); // we get all events to eliminate caching
                
                request.TitleSearch = (request.TitleSearch ?? "").Trim().ToLower();
                IEnumerable<Event> filteredEvents = FilterEvents(apiResponse.Events.Select(MapApiEventToDomainEvent), request);
                IEnumerable<Event> orderedEvents = OrderEvents(filteredEvents, request.Ordering);

                var response = new EventsResponse
                {
                    Title = apiResponse.Title,
                    Description = apiResponse.Description,
                    Link = apiResponse.Link,
                    Events = orderedEvents.ToArray(),
                    TitleSearch = request.TitleSearch,
                    TotalCount = orderedEvents.Count(),
                };

                return response;
            }
            catch (Exception ex)
            {
                // TODO: Log exception
                throw;
            }
        }

        public async Task<Event> GetSingeEvent(string id)
        {
            var allEvents = await GetBothOpenAndClosedEvents(new EonetEventsRequest() { Days = _maxEonetDaysPrior });
            var singleEvent = allEvents.Events.Where(e => e.Id == id).Select(MapApiEventToDomainEvent).FirstOrDefault();
            return singleEvent;
        }

        public async Task<EonetCategory[]> GetCurrentlyAvailableCategories()
        {
            var allEvents = await GetBothOpenAndClosedEvents(new EonetEventsRequest() { Days = _maxEonetDaysPrior });
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
            var allEvents = await GetBothOpenAndClosedEvents(new EonetEventsRequest() { Days = _maxEonetDaysPrior });
            var sources = allEvents.Events
                .SelectMany(e => e.Sources.Select(c => c.Id))
                .Distinct()
                .ToArray();

            return sources;
        }

        private Event MapApiEventToDomainEvent(EonetEvent e)
        {
            return new Event
            {
                Id = e.Id,
                Title = e.Title,
                Description = e.Description,
                Link = e.Link,
                ClosedDate = e.Closed,
                // TOD0: Categories.Count() = 0 in 99%, so it's decided to simplify solution by using only one category
                Category = e.Categories[e.Categories.Count() > 1 ? 1 : 0],
                Sources = e.Sources,
                Geometry = e.Geometry,
                LastDate = e.Geometry.Max(g => g.Date),
                Status = e.Closed.HasValue ? EonetEventStatus.Closed : EonetEventStatus.Open,
            };
        }

        private IEnumerable<Event> FilterEvents(IEnumerable<Event> events, EventsRequest request)
        {
            if (request == null)
            {
                return events;
            }

            if (request.Days != 0 && request.Days != _maxEonetDaysPrior) {
                var minDate = _currentDateTimeProvider.GetCurrentTime().AddDays(-1 * request.Days);
                events = events.Where(e => e.LastDate > minDate);
            }
                       
            if (request.Sources != null && request.Sources.Length > 0)
            {
                events = events.Where(e => e.Sources.Any(c => request.Sources.Contains(c.Id)));
            }

            if (request.Categories != null && request.Categories.Length > 0)
            {
                events = events.Where(e => request.Categories.Contains(e.Category.Id));
            }

            if (!string.IsNullOrWhiteSpace(request.TitleSearch))
            {
                events = events.Where(e => e.Title.ToLower().Contains(request.TitleSearch));
            }

            return events;
        }

        private IEnumerable<Event> OrderEvents(IEnumerable<Event> events, EventOrderAttribute[] orderAttributes)
        {
            if (orderAttributes == null || orderAttributes.Length == 0)
            {
                return events;
            }

            if (orderAttributes.GroupBy(so => so.AttributeType).Any(g => g.Count() > 1))
            {
                throw new ArgumentException("The same attribute included more than one time into ordering.");
            }

            IOrderedEnumerable<Event> orderedEvents = null;
            foreach (var attribute in orderAttributes)
            {
                Func<Event, object> keySelector;
                if (!OrderingKeySelectors.TryGetValue(new OrderingType(attribute.AttributeType, 0), out keySelector)
                    && !OrderingKeySelectors.TryGetValue(new OrderingType(attribute.AttributeType, (short)(attribute.IsDescending ? -1 : 1)), out keySelector))
                {
                    throw new Exception($"Key selector is not found for AttributeType={attribute.AttributeType} and IsDescending={attribute.IsDescending}");
                }

                if (orderedEvents == null)
                {
                    // this code is executed for the first attribute only
                    orderedEvents = attribute.IsDescending
                        ? events.OrderByDescending(keySelector)
                        : events.OrderBy(keySelector);
                }
                else
                {
                    orderedEvents = attribute.IsDescending
                        ? orderedEvents.ThenByDescending(keySelector)
                        : orderedEvents.ThenBy(keySelector);
                }

                /*
                switch (attribute.AttributeType)
                {
                    // TODO: Refactor it to decrease amount of duplicated code by using delegates
                    //       Make sure delegates usage doesn't decrease readability

                    case EventOrderAttributeType.Id:
                        orderedEvents = orderedEvents == null
                            ? (attribute.IsDescending ? events.OrderByDescending(e => e.Id) : events.OrderBy(e => e.Id))
                            : (attribute.IsDescending ? orderedEvents.ThenByDescending(e => e.Id) : orderedEvents.ThenBy(e => e.Id));
                        break;
                    case EventOrderAttributeType.Title:
                        orderedEvents = orderedEvents == null
                            ? (attribute.IsDescending ? events.OrderByDescending(e => e.Title) : events.OrderBy(e => e.Title))
                            : (attribute.IsDescending ? orderedEvents.ThenByDescending(e => e.Title) : orderedEvents.ThenBy(e => e.Title));
                        break;
                    case EventOrderAttributeType.Category:
                        orderedEvents = orderedEvents == null
                            ? (attribute.IsDescending ? events.OrderByDescending(e => e.Category) : events.OrderBy(e => e.Category))
                            : (attribute.IsDescending ? orderedEvents.ThenByDescending(e => e.Category) : orderedEvents.ThenBy(e => e.Category));
                        break;
                    case EventOrderAttributeType.Source:
                        orderedEvents = orderedEvents == null
                            ? (attribute.IsDescending ? events.OrderByDescending(e => e.Sources.Min(c => c.Id)) : events.OrderBy(e => e.Sources.Max(c => c.Id)))
                            : (attribute.IsDescending ? orderedEvents.ThenByDescending(e => e.Sources.Min(c => c.Id)) : orderedEvents.ThenBy(e => e.Sources.Max(c => c.Id)));
                        break;
                    case EventOrderAttributeType.LastDate:
                        orderedEvents = orderedEvents == null
                            ? (attribute.IsDescending ? events.OrderByDescending(e => e.LastDate) : events.OrderBy(e => e.LastDate))
                            : (attribute.IsDescending ? orderedEvents.ThenByDescending(e => e.LastDate) : orderedEvents.ThenBy(e => e.LastDate));
                        break;
                    case EventOrderAttributeType.Status:
                        orderedEvents = orderedEvents == null
                            ? (attribute.IsDescending ? events.OrderByDescending(e => e.Status) : events.OrderBy(e => e.Status))
                            : (attribute.IsDescending ? orderedEvents.ThenByDescending(e => e.Status) : orderedEvents.ThenBy(e => e.Status));
                        break;
                    case EventOrderAttributeType.ClosedDate:
                        orderedEvents = orderedEvents == null
                            ? (attribute.IsDescending ? events.OrderByDescending(e => e.ClosedDate) : events.OrderBy(e => e.ClosedDate))
                            : (attribute.IsDescending ? orderedEvents.ThenByDescending(e => e.ClosedDate) : orderedEvents.ThenBy(e => e.ClosedDate));
                        break;
                }
                */
            }

            return orderedEvents;
        }

        private async Task<EonetEventsResponse> GetBothOpenAndClosedEvents(EonetEventsRequest request)
        {
            if (request == null) request = new EonetEventsRequest();

            var eventsResponse = await _apiClient.GetEventsAsync(request);

            // If status=null previous call returns only open events, so we override this behaviour by calling endpoint twice
            // Only if status is explicitly 'open' or 'closed' then only one request is performed            
            if (request.Status == null)
            {
                request.Status = EonetEventStatus.Closed;
                var closedEventsResponse = await _apiClient.GetEventsAsync(request);
                request.Status = null;

                // This hack breaks api-level Limit filter, so it need to be reapplied if present
                eventsResponse = new EonetEventsResponse
                {
                    Title = eventsResponse.Title,
                    Description = eventsResponse.Description,
                    Link = eventsResponse.Link,
                    Events = eventsResponse.Events.Concat(closedEventsResponse.Events).ToArray()
                };
            }

            return eventsResponse;
        }

        private struct OrderingType
        {
            /// <summary>
            /// 1 - asc, -1 - desc, 0 - any
            /// </summary>
            public short OrderDirection { get; set; }
            public EventOrderAttributeType AttributeType { get; set; }

            public OrderingType(EventOrderAttributeType type, short direction)
            {
                OrderDirection = direction;
                AttributeType = type;
            }
        }
    }
}
