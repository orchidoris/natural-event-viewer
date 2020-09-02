using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Eonet.Core
{
    public class EonetHttpClientFactory : IEonetHttpClientFactory
    {
        private const string EonetApiBase = "https://eonet.sci.gsfc.nasa.gov/api/v3/"; // TODO: Consider move this value into app.config

        public HttpClient GetEonetHttpClient()
        {
            // TODO: Consider implementing Wrapper for HttpClient, so we can use dependency injection and write better unit tests for EonetApiClient class
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(EonetApiBase);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            return client;
        }
    }
}
