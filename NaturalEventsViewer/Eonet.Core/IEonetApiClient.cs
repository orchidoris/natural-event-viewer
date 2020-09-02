using Eonet.Core.Models;
using System.Threading.Tasks;

namespace Eonet.Core
{
    public interface IEonetApiClient
    {
        Task<EonetEventsResponse> GetEventsAsync(EonetEventsApiRequest request = null);
    }
}
