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

        public EonetApiClient(IEonetHttpClientFactory clientFactory) {
            if (clientFactory == null) throw new ArgumentNullException("Client factory has not bee passed");

            _clientFactory = clientFactory;
        }

        public async Task<EonetEventsResponse> GetEventsAsync(EonetEventsApiRequest request = null)
        {
            int retryMaxConut = 5;

            for (int retryCount = 0; retryCount <= retryMaxConut; retryCount++)
            {

                try
                {
                    using (var client = _clientFactory.GetEonetHttpClient())
                    {
                        EonetEventsResponse eventsResponse = null;

                        string eventsGetRequestPath = GetEventsRequestPath(request);
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
                    if (retryCount == retryMaxConut)
                    {
                        // TODO: Log exception
                        throw;
                    }
                    else
                    {
                        Thread.Sleep(1000); // Dummy retry logic to cope with random failure issue
                        continue;
                    }                    
                }
            }

            throw new Exception("Reached max retry count");
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

        private string GetEventsRequestPath(EonetEventsApiRequest request)
        {
            if (request == null)
            {
                request = new EonetEventsApiRequest();
            }

            var days = request.Days ?? 180; // TODO: Consider moving value 180 into app.config
            string requestString = $"events?days={days}&";

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
