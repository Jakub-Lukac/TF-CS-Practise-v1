using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using GitHubDemo.Services.Interfaces;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using System.Collections.Generic;

namespace GitHubDemo
{
    public class GitHubDemoFunction
    {
        private readonly IBulkRequestProcessor _bulkRequestProcessor;
        private readonly ILogger<GitHubDemoFunction> _logger;
        private readonly TelemetryClient _telementryClient;
        public GitHubDemoFunction(IBulkRequestProcessor bulkRequestProcessor, ILogger<GitHubDemoFunction> logger, TelemetryConfiguration telemetryConfiguration)
        {
            _bulkRequestProcessor = bulkRequestProcessor;
            _logger = logger;
            // we set in the terraform code the application insights for the azure function,
            // thanks to which connection is automatically made (application_insights_key)
            _telementryClient = new TelemetryClient(telemetryConfiguration);
        }

        [FunctionName("GitHubDemoFunction")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "foo")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");

            string name = req.Query["name"];

            var myNumber = await _bulkRequestProcessor.DoSomethingAsync();

            _logger.LogInformation($"My number is : {myNumber}");

            var eventAttributes = new Dictionary<string, string>();
            eventAttributes.Add("Foo", "5");
            eventAttributes.Add("Bar", "41");

            _telementryClient.TrackEvent("Azure Function Event", eventAttributes);

            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);
            name = name ?? data?.name;

            string responseMessage = string.IsNullOrEmpty(name)
                ? "This HTTP triggered function executed successfully. Pass a name in the query string or in the request body for a personalized response."
                : $"Hello, {name}. This HTTP triggered function executed successfully. My number is {myNumber}";

            return new OkObjectResult(responseMessage);
        }
    }
}
