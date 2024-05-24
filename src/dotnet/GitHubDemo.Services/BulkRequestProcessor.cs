using GitHubDemo.Services.Interfaces;
using Microsoft.Extensions.Logging;


namespace GitHubDemo.Services
{
    public class BulkRequestProcessor : IBulkRequestProcessor
    {
        private readonly ILogger<BulkRequestProcessor> _logger;
        public BulkRequestProcessor(ILogger<BulkRequestProcessor> logger)
        {
            _logger = logger;
        }

        public async Task<int> DoSomethingAsync()
        {
            _logger.LogInformation("BulkRequesrProcessor.DoSomethingAsync()");
            return 5;
        }
    }
}
