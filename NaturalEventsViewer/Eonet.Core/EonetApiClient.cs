using Eonet.Core.Models;
using System;
using System.Linq; // though unsed, please do not remove as it is useful for debugging purposes
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Eonet.Core.Converters;
using System.Net.Http.Formatting;
using System.Threading;

namespace Eonet.Core
{
    public class EonetApiClient : IEonetApiClient
    {
        private IEonetHttpClientFactory _clientFactory;
        private IEonetMemoryCache<EonetEventsResponse> _cache;

        public EonetApiClient(IEonetHttpClientFactory clientFactory, IEonetMemoryCache<EonetEventsResponse> cache) {
            if (clientFactory == null) throw new ArgumentNullException("Client factory has not bee passed");

            _clientFactory = clientFactory;
            _cache = cache;
        }

        public async Task<EonetEventsResponse> GetEventsAsync(EonetEventsRequest request = null)
        {
            string eventsGetRequestPath = GetEventsRequestPath(request);
            EonetEventsResponse eventsResponse = await _cache.GetOrCreate(
                eventsGetRequestPath,
                async () => await GetEventsFromApiAsync(eventsGetRequestPath));

            return eventsResponse;
        }

        private async Task<EonetEventsResponse> GetEventsFromApiAsync(string eventsGetRequestPath)
        {
            int retryMaxNum = 5;

            for (int retryNum = 0; retryNum <= retryMaxNum; retryNum++)
            {

                try
                {
                    using (var client = _clientFactory.GetEonetHttpClient())
                    {
                        EonetEventsResponse eventsResponse = null;

                        var eonetJsonFormatters = new[] { GetEonetJsonMediaTypeFormatter() };

                        HttpResponseMessage response = await client.GetAsync(eventsGetRequestPath);
                        if (response.IsSuccessStatusCode)
                        {
                            eventsResponse = await response.Content.ReadAsAsync<EonetEventsResponse>(eonetJsonFormatters);
                        }

                        return eventsResponse;
                    }
                }
                catch (Exception e)
                {
                    if (retryNum >= retryMaxNum)
                    {
                        // TODO: Log exception
                        throw;
                    }
                    else
                    {
                        Thread.Sleep(1000); // Dummy retry logic to reduce random failure issue impact
                        continue;
                    }
                }
            }

            throw new Exception("Reached max retries number");
        }

        private MediaTypeFormatter GetEonetJsonMediaTypeFormatter()
        {
            var jsonFormatter = new JsonMediaTypeFormatter();
            var settings = jsonFormatter.SerializerSettings;
            settings.Formatting = Formatting.Indented;
            settings.MissingMemberHandling = MissingMemberHandling.Error;
            settings.NullValueHandling = NullValueHandling.Include;
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            settings.Converters.Add(new StringEnumConverter());
            settings.Converters.Add(new EonetGeometryCreationConverter()); // used to parse Point and Polygon geometry types

            return jsonFormatter;
        }

        private string GetEventsRequestPath(EonetEventsRequest request)
        {
            if (request == null)
            {
                request = new EonetEventsRequest();
            }

            string requestString = $"events?days={request.Days}&";

            if (request.Limit.HasValue)
            {
                requestString += $"limit={request.Limit.Value}&";
            }

            if (request.Sources == null || request.Sources.Length != 0)
            {
                requestString += $"source={string.Join(",", request.Sources)}&";
            }

            if (request.Status.HasValue)
            {
                requestString += $"status={request.Status.Value.ToString().ToLower()}&";
            }

            return requestString.TrimEnd('&');
        }
    }
}
