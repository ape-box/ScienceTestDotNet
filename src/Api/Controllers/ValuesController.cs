using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using App.Metrics;
using Honeycomb.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using ScienceTest.Api.Clients;
using ScienceTest.Api.Logging;

namespace ScienceTest.Api.Controllers
{
    [Route("api/[controller]"), ApiController]
    public partial class ValuesController : ControllerBase
    {
        private readonly IMetrics _metrics;
        private readonly IHoneycombEventManager _eventManager; 
        private readonly IUpstreamApiClient _upstreamApiClient;

        public ValuesController(
            IMetrics metrics,
            IHoneycombEventManager eventManager,
            IUpstreamApiClient upstreamApiClient)
        {
            _metrics = metrics ?? throw new ArgumentNullException(nameof(metrics));
            _eventManager = eventManager ?? throw new ArgumentNullException(nameof(eventManager));
            _upstreamApiClient = upstreamApiClient ?? throw new ArgumentNullException(nameof(upstreamApiClient));
        }
        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<string>>> Get()
        {
            var upstreamValues = await _upstreamApiClient.GetValues(HttpContext.GetCorrelationId());
            CountUpstreamApiCall();
            _eventManager.AddData("values_count", upstreamValues?.Length ?? 0);

            return upstreamValues;
        }
    }
}
