
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using SuperAniki.Laggen.Models;
using SuperAniki.Laggen.Services;
using SuperAniki.Laggen.Utilities;

namespace SuperAniki.Laggen
{
    public class LaggToImage
    {
        private readonly ILogger<LaggToImage> _logger;

        public LaggToImage(ILogger<LaggToImage> logger)
        {
            _logger = logger;
        }

        [Function("BarrelToImage")]
        public static async Task<HttpResponseData> Run(
               [HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req,
               FunctionContext executionContext)
        {
            var logger = executionContext.GetLogger("SampleFunction");
            Barrel barrelData;
            string jsonData;



            /* Load JSON from request body */
            try
            {
                jsonData = await new StreamReader(req.Body).ReadToEndAsync();
            }
            catch (Exception)
            {
                return await ReturnErrorMessage(req, logger, "Could not serialise JSON data");
            }

            /* Create Barrel class object from JSON */
            JsonResult<Barrel> result = JsonHandler.ExtractJsonData<Barrel>(jsonData, logger);

            if (!result.Success)
            {
                return await ReturnErrorMessage(req, logger, result.ErrorMessage!);
            }
            barrelData = result.Data!;
            int scale = 6;

            /* Create image from class object */
            try
            {
                Stream? imageStream = ImageGenerator.Draw(barrelData, scale, logger);
                if (imageStream == null)
                {
                    var errorResponse = req.CreateResponse(System.Net.HttpStatusCode.InternalServerError);
                    await errorResponse.WriteStringAsync("Stream error");
                    return errorResponse;
                }
                var okResponse = req.CreateResponse(System.Net.HttpStatusCode.OK);
                okResponse.Headers.Add("Content-Type", "image/png");

                await imageStream.CopyToAsync(okResponse.Body);
                logger.LogError("image ok. response coming:");

                return okResponse;
            }
            catch (Exception)
            {
                return await ReturnErrorMessage(req, logger, "Problem creating image from json");
            }
        }

        private async static Task<HttpResponseData> ReturnErrorMessage(HttpRequestData req, ILogger logger, string logMessage)
        {
            logger.LogError(logMessage);
            var errorResponse = req.CreateResponse(System.Net.HttpStatusCode.InternalServerError);
            await errorResponse.WriteStringAsync("Image generation error");
            return errorResponse;
        }

    }
}





/*
            //"isolated worker"
            [Function("SimpleAsyncFunction")]
            public async Task<HttpResponseData> Run(
                [HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequestData req,
                FunctionContext executionContext)
            {
                var logger = executionContext.GetLogger("HttpExample");
                logger.LogInformation("C# HTTP trigger function processed a request.");

                // You can perform any asynchronous operation here
                await Task.Delay(1000);  // Simulating an async operation

                var response = req.CreateResponse(System.Net.HttpStatusCode.OK);
                _logger.LogInformation("C# HTTP trigger function processed a request.");

                await response.WriteStringAsync("Welcome to Azure Functions!");

                return response;
            }
    */




/*
        // synchronus pattern
        public IActionResult Run(
          [HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequest req)
        {
            string requestBody = new StreamReader(req.Body).ReadToEnd();
            dynamic data = JsonSerializer.Deserialize<int>(requestBody);

            string json = "{ \"Text\": \"Hello\", \"Enum\": \"Two\" }";
            var _ = JsonSerializer.Deserialize<MyObj>(json); // Throws exception.

            _logger.LogInformation("C# HTTP trigger function processed a request.");
            return new OkObjectResult($"Welcome to Azure Functions, {req.Query["name"]}!");
        }
        */




/*
        public IActionResult Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            dynamic data = JsonSerializer.Deserialize<int>(requestBody);

            string json = "{ \"Text\": \"Hello\", \"Enum\": \"Two\" }";
            var _ = JsonSerializer.Deserialize<MyObj>(json); // Throws exception.

            _logger.LogInformation("C# HTTP trigger function processed a request.");
            // return Task<IActionResult>.FromResult(new FileStreamResult(Draw(), "image/png"));
            try
            {
                Stream imageStream = Draw();
                if (imageStream == null)
                {
                    return new BadRequestObjectResult("Failed to generate image");
                }
                return new FileStreamResult(imageStream, "image/png");
            }
            catch (Exception ex)
            {
                //log.LogError($"Error generating image: {ex.Message}");
                return new StatusCodeResult(500); // Internal Server Error
            }
            //return new FileStreamResult(Draw(), "image/png");
        }
        */