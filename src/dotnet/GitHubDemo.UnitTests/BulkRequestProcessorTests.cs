using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GitHubDemo.Services;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace GitHubDemo.UnitTests
{
    public class BulkRequestProcessorTests
    {
        ILogger<BulkRequestProcessor> _logger;
        TelemetryConfiguration _telemetryConfiguration;

        // System.Console.WriteLine does not work in XUnit, thats why we use ITestOutputHelper to display outputs in the test window
        public BulkRequestProcessorTests(ITestOutputHelper output)
        {
            // explicitly setting up the _logger, NOT through dependency injection
            _logger = new XunitLogger<BulkRequestProcessor>(output);

            // app insights is not gonna get any logs that are happening
            // when running a tests we do not really need that observability
            _telemetryConfiguration = new TelemetryConfiguration();
        }

        [Trait("Category", "Unit")]
        [Fact]
        public async Task Test1()
        {
            var services = new BulkRequestProcessor(_logger, _telemetryConfiguration);

            var myNumber = await services.DoSomethingAsync();

            Assert.Equal(5, myNumber);
        }
    }
}
