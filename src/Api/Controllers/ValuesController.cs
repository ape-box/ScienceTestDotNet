using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using App.Metrics;
using Microsoft.AspNetCore.Mvc;
using ScienceTest.Api.Clients;
using ScienceTest.Api.Logging;

namespace ScienceTest.Api.Controllers
{
    [Route("api/[controller]"), ApiController]
    public partial class ValuesController : ControllerBase
    {
        private readonly IMetrics _metrics;
        private readonly IUpstreamApiClient _upstreamApiClient;

        public ValuesController(
            IMetrics metrics,
            IUpstreamApiClient upstreamApiClient)
        {
            _metrics = metrics ?? throw new ArgumentNullException(nameof(metrics));
            _upstreamApiClient = upstreamApiClient ?? throw new ArgumentNullException(nameof(upstreamApiClient));
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<string>>> Get()
        {
            var upstreamValues = await _upstreamApiClient.GetValues(HttpContext.GetCorrelationId());
            CountUpstreamApiCall();

            return upstreamValues;
        }
    }
}
