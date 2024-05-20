#if !DISABLE_FUNCTION
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace SuperAniki.Laggen
{
    public class HttpTriggerAdmin

    {
        private readonly ILogger<HttpTriggerAdmin> _logger;

        public HttpTriggerAdmin(ILogger<HttpTriggerAdmin> logger)
        {
            _logger = logger;
        }

        [Function("HttpTriggerAdmin")]
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Admin, "get", "post")] HttpRequest req)
        {
            _logger.LogInformation("C# HTTP trigger function processed a request.");
            return new OkObjectResult("Welcome to Azure Functions!");
        }
    }
}
#endif

