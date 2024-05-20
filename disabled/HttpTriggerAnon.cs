#if !DISABLE_FUNCTION
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
using System.Text.Json;

namespace SuperAniki.Laggen
{
    public class HttpTriggerAnon
    {
        private readonly ILogger<HttpTriggerAnon> _logger;

        public HttpTriggerAnon(ILogger<HttpTriggerAnon> logger)
        {
            _logger = logger;
        }

        [Function("HttpTriggerAnonJsonTest")]
        //public IActionResult Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post")] HttpRequest req, FunctionContext executionContext)
        public static async Task<HttpResponseData> Run(
               [HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req,
               FunctionContext executionContext)
        {
            {
                var logger = executionContext.GetLogger("HttpTriggerAnonJsonTest");
                // Use StreamReader asynchronously
                string requestBody;
                using (var reader = new StreamReader(req.Body))
                {
                    requestBody = await reader.ReadToEndAsync();
                }

                //string requestBody = new StreamReader(req.Body).ReadToEnd();
                logger.LogInformation("C# HTTP trigger function processed a request.");

                dynamic? data;

                try
                {
                    data = JsonSerializer.Deserialize<BarrelForPrintouts>(requestBody);
                }
                catch (JsonException)
                {
                    logger.LogError("Could not serialise JSON data");
                    //var statusCode = new StatusCodeResult(StatusCodes.Status500InternalServerError); // Internal Server Error
                    var resp = req.CreateResponse(System.Net.HttpStatusCode.InternalServerError);
                    return resp;
                }

                var response = req.CreateResponse(System.Net.HttpStatusCode.OK);
                await response.WriteStringAsync("JSON decoding success!");
                return response;
            }
        }
    }
}
#endif
