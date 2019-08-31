using App.Metrics;
using App.Metrics.Counter;

namespace ScienceTest.Api.Controllers
{
    public partial class ValuesController
    {
        private readonly CounterOptions _upstreamApiCounterOptions = new CounterOptions
        {
            MeasurementUnit = Unit.Calls,
            Name = "Upstream Api Call",
            ResetOnReporting = true
        };

        public void CountUpstreamApiCall() =>
            _metrics.Measure.Counter.Increment(_upstreamApiCounterOptions);
    }
}
