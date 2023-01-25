using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GitHubIssuesMailer
{
    internal class ApimService
    {
        private readonly string _baseUrl;
        private readonly string _subscriptionKey;
        private readonly HttpClient _httpClient;

        public ApimService(string baseUrl, string subscriptionKey, string identityToken = null)
        {
            _baseUrl = baseUrl;
            _subscriptionKey = subscriptionKey;
            _httpClient = new HttpClient();
            _httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", _subscriptionKey);
        }

        public async Task<string> GetTokenBackAsync(string authorizationProviderId, string authorizationId)
        {
            var endpoint = $"{_baseUrl}/tokenstore/fetch?authorizationProviderId";
        }
    }
}
