using GitHubDemo.Services.Interfaces;

namespace GitHubDemo.Services
{
    public class BulkRequestProcessor : IBulkRequestProcessor
    {
        public async Task<int> DoSomethingAsync()
        {
            return 5;
        }
    }
}
