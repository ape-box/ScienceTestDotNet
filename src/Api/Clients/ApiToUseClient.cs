using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace ScienceTest.Api.Clients
{
    public class ApiToUseClient : IUpstreamApiClient
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<ApiToUseClient> _logger;
        private const string ResourceUri = "api/values";
        
        public  const string ClientName = nameof(ApiToUseClient);

        public ApiToUseClient(IHttpClientFactory httpClientFactory, ILogger<ApiToUseClient> logger)
        {
            _httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<string[]> GetValues(Guid CorrelationId)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            
            using (var client = _httpClientFactory.CreateClient(ClientName))
            {
                var method = "GET";
                var requestPath = Path.Join(client.BaseAddress.ToString(), ResourceUri);
                try
                {
                    var response = await client.GetAsync(ResourceUri);
                    response.EnsureSuccessStatusCode();
                    var content = await response.Content.ReadAsStringAsync();
                    var model = JsonConvert.DeserializeObject<string[]>(content);

                    var elapsedTime = stopwatch.ElapsedMilliseconds;
                    _logger.LogInformation("Success for {@method} {@requestPath} with {@CorrelationId} in {@elapsedTime} millis", method, requestPath, CorrelationId, elapsedTime);

                    return model;
                }
                catch (Exception exception)
                {
                    var elapsedTime = stopwatch.ElapsedMilliseconds;
                    _logger.LogInformation("Exception {@exception.Message} for {@method} {@requestPath} with {@CorrelationId} in {@elapsedTime} millis", exception.Message, method, requestPath, CorrelationId, elapsedTime);
                    Console.WriteLine(exception);
                    throw;
                }
            }
        }
    }
}
