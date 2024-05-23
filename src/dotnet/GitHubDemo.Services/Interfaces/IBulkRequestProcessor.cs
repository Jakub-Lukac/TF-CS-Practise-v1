using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitHubDemo.Services.Interfaces
{
    public interface IBulkRequestProcessor
    {
        Task<int> DoSomethingAsync();
    }
}
