using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GitHubIssuesMailer
{
    public class SendOutlookMail
    {
        private readonly ILogger _log;
        private ApimService _apimService;
        private string _gitHubRepoName;

        public SendOutlookMail(ILogger log, string apimServiceName, string subscriptionKey, string gitHubRepoName)
        {
            _log = log;
            _apimService = new ApimService($"https://{apimServiceName}.azure-api.net", $"{subscriptionKey}");
            _gitHubRepoName = gitHubRepoName;
        }

        public async Task RunAsync()
        {
            await _apimService.GetTokenBackAsync("GitHub", "GitHubIssuesMailer");
        }
    }
}
