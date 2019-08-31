using App.Metrics;
using App.Metrics.Timer;

namespace ScienceTest.Api.Clients.Experiments {
    public partial class UpstreamApiClientExperiment
    {
        private readonly TimerOptions _apiToUseTimerOptions = new TimerOptions
        {
            MeasurementUnit = Unit.Requests,
            Name = "GetValues ToUse Timer",
            DurationUnit = TimeUnit.Milliseconds,
            RateUnit = TimeUnit.Milliseconds,
        };

        private readonly TimerOptions _apiToTryTimerOptions = new TimerOptions
        {
            MeasurementUnit = Unit.Requests,
            Name = "GetValues ToTry Timer",
            DurationUnit = TimeUnit.Milliseconds,
            RateUnit = TimeUnit.Milliseconds,
        };
    }
}