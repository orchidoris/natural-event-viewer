using Eonet.Core.Models;
using NaturalEventsViewer.Domain.Models;
using System.Threading.Tasks;

namespace NaturalEventsViewer.Domain
{
    public interface IEonetRepository
    {
        Task<EventsResponse> GetEvents(EventsRequest request);

        Task<Event> GetSingeEvent(string id);

        Task<EonetCategory[]> GetCurrentlyAvailableCategories();

        Task<string[]> GetCurrentlyAvailableSourceIds();
    }
}
