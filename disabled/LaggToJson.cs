#if !DISABLE_FUNCTION

using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using System.Text.Json;


namespace SuperAniki.Laggen
{
    public class LaggToJson
    {
        private readonly ILogger<LaggToJson> _logger;

        public LaggToJson(ILogger<LaggToJson> logger)
        {
            _logger = logger;
        }

        [Function("BarrelToJson")]
        public static async Task<HttpResponseData> Run(
               [HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req,
               FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger("SampleFunction");
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            HttpResponseData response;
            dynamic? data;

            try
            {
                data = JsonSerializer.Deserialize<BarrelForPrintouts>(requestBody);
            }
            catch (JsonException)
            {
                logger.LogError("Could not serialise JSON data");
                response = req.CreateResponse(System.Net.HttpStatusCode.InternalServerError);
                await response.WriteStringAsync("JSON decoding error");
                return response;
            }


            response = req.CreateResponse(System.Net.HttpStatusCode.OK);
            await response.WriteStringAsync("JSON decoding success!");
            return response;
        }
    }
}
#endif