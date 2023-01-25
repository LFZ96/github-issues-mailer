using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http;
using System.Collections.Generic;
using System.Net.Http.Formatting;

namespace GitHubIssuesMailer
{
    public static class MailTrigger
    {
        [FunctionName("MailTrigger")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation($"MailTrigger started at: {DateTime.Now}");
            var apimServiceName = System.Environment.GetEnvironmentVariable("APIM_SERVICE_NAME", EnvironmentVariableTarget.Process);
            var subscriptionKey = System.Environment.GetEnvironmentVariable("SUBSCRIPTION_KEY", EnvironmentVariableTarget.Process);
            var githubRepoName = System.Environment.GetEnvironmentVariable("GITHUB_REPO_NAME", EnvironmentVariableTarget.Process);
            var githubRepoOwner = System.Environment.GetEnvironmentVariable("GITHUB_REPO_OWNER", EnvironmentVariableTarget.Process);
            log.LogInformation($"MailTrigger read the following settings: APIM_SERVICE_NAME={apimServiceName}, GITHUB_REPO_NAME={githubRepoName}, GITHUB_REPO_OWNER={githubRepoOwner}");

            var job = new SendOutlookMail(log, apimServiceName, subscriptionKey, githubRepoName, githubRepoOwner);
            await job.RunAsync();

            // Get GitHub webhook request
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonConvert.DeserializeObject(requestBody);

            // Create MicrosoftGraphEmail object
            var microsoftGraphEmail = new MicrosoftGraphEmail
            {
                Message = new Message
                {
                    Subject = "New GitHub Issue",
                    Body = new Body
                    {
                        ContentType = "Text",
                        Content = $"A new GitHub Issue has been created in {data.repository.name}.\n\n Navigate to {data.repository.issue_url} to view the new issue."
                    },
                    ToRecipients = new Recipients(new List<string> { "loganzipkes@microsoft.com" })
                }
            };

            // Send microsoftGraphEmail object to API Management operation
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "https://apim-github-issues-mailer.azure-api.net/issue-mailer/send-email");
            request.Content = new ObjectContent<MicrosoftGraphEmail>(microsoftGraphEmail, new JsonMediaTypeFormatter());
            var response = await client.SendAsync(request);

            return new OkObjectResult(response.StatusCode);
        }
    }
}
