using Eonet.Core.Models;
using NaturalEventsViewer.Domain.Models;
using System.Threading.Tasks;

namespace NaturalEventsViewer.Domain
{
    public interface IEonetRepository
    {
        Task<EonetEventsResponse> GetEvents(EonetEventsCustomRequest request);

        Task<EonetEvent> GetSingeEvent(string id);

        Task<EonetCategory[]> GetCurrentlyAvailableCategories();

        Task<string[]> GetCurrentlyAvailableSourceIds();
    }
}
