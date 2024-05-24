using GitHubDemo.Services.Interfaces;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Logging;


namespace GitHubDemo.Services
{
    public class BulkRequestProcessor : IBulkRequestProcessor
    {
        private readonly ILogger<BulkRequestProcessor> _logger;
        private readonly TelemetryClient _telemetryClient;
        public BulkRequestProcessor(ILogger<BulkRequestProcessor> logger, TelemetryConfiguration telemetryConfiguration)
        {
            _logger = logger;
            _telemetryClient = new TelemetryClient(telemetryConfiguration);
        }

        public async Task<int> DoSomethingAsync()
        {
            _logger.LogInformation("BulkRequesrProcessor.DoSomethingAsync()");
            _telemetryClient.TrackEvent("Inside the BulkRequestProcessor");
            return 5;
        }
    }
}
