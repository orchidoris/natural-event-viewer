using System.Net.Http;

namespace Eonet.Core
{
    public interface IEonetHttpClientFactory
    {
        HttpClient GetEonetHttpClient();
    }
}
