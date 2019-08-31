using System;
using System.Linq;
using System.Threading.Tasks;
using App.Metrics;
using GitHub;
using Microsoft.Extensions.Logging;

namespace ScienceTest.Api.Clients.Experiments {
    public partial class UpstreamApiClientExperiment : IUpstreamApiClient
    {
        private readonly IMetrics _metrics;
        private readonly ILogger<UpstreamApiClientExperiment> _logger;
        private readonly ApiToUseClient _apiToUseClient;
        private readonly ApiToTestClient _apiToTestClient;

        public UpstreamApiClientExperiment(
            ApiToUseClient apiToUseClient, 
            ApiToTestClient apiToTestClient, 
            IMetrics metrics, 
            ILogger<UpstreamApiClientExperiment> logger)
        {
            _apiToUseClient = apiToUseClient ?? throw new ArgumentNullException(nameof(apiToUseClient));
            _apiToTestClient = apiToTestClient ?? throw new ArgumentNullException(nameof(apiToTestClient));
            _metrics = metrics ?? throw new ArgumentNullException(nameof(metrics));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<string[]> GetValues(Guid correlationId)
        {
            var result = await Scientist.ScienceAsync<string[]>("upstream-api", experiment =>
            {
                experiment.Use(async () =>
                {
                    using (_metrics.Measure.Timer.Time(_apiToUseTimerOptions))
                    {
                        return await _apiToUseClient.GetValues(correlationId);
                    }
                });

                experiment.Try(async () =>
                {
                    using (_metrics.Measure.Timer.Time(_apiToTryTimerOptions))
                    {
                        return await _apiToTestClient.GetValues(correlationId);
                    }
                });

                experiment.Compare((a, b) =>
                {
                    _logger.LogInformation("{@CorrelationId} Sets: {@a} - {@b}", correlationId, a, b);
                    
                    return a.SequenceEqual(b);
                });
            });

            return result;
        }
    }
}