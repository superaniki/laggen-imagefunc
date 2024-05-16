using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.Fonts;
using System.Net.Http.Json;
using System.Text.Json;
using Newtonsoft.Json;


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
            BarrelForPrintouts barrelData;
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
            JsonResult<BarrelForPrintouts> result = JsonHandler.ExtractJsonData<BarrelForPrintouts>(jsonData, logger);

            if (!result.Success)
            {
                return await ReturnErrorMessage(req, logger, result.ErrorMessage!);
            }
            barrelData = result.Data!;

            /* Create image from class object */
            try
            {
                Stream? imageStream = DrawPaper(barrelData, logger);
                if (imageStream == null)
                {
                    var errorResponse = req.CreateResponse(System.Net.HttpStatusCode.InternalServerError);
                    await errorResponse.WriteStringAsync("Stream error");
                    return errorResponse;
                }
                var okResponse = req.CreateResponse(System.Net.HttpStatusCode.OK);
                okResponse.Headers.Add("Content-Type", "image/png");

                await imageStream.CopyToAsync(okResponse.Body);
                logger.LogError("image ok. response comming:");

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


        private static MemoryStream? DrawPaper(BarrelForPrintouts barrel, ILogger logger)
        {
            FontCollection collection = new();
            StaveTool toolState = barrel.StaveToolState;
            try
            {
                //https://github.com/shantigilbert/liberation-fonts-ttf/blob/master/LiberationMono-Regular.ttf
                collection.Add("fonts/LiberationMono-Regular.ttf");
            }
            catch (Exception e)
            {
                logger.LogError("Font loading exception: " + e.Message);
            }


            //var fontPath = Path.Combine(context.FunctionAppDirectory, fFonts", "MyFont.ttf");
            //var path = System.IO.Path.Combine(context.FunctionDirectory, "twinkle.txt");

            int width = 640;
            int height = 480;
            using (Image<Rgba32> image = new(width, height))
            {

                if (collection.TryGet("Liberation Mono", out FontFamily family))
                {
                    // family will not be null here
                    Font font = family.CreateFont(12, FontStyle.Italic);
                    string detailsName = barrel.BarrelDetails.Name;
                    image.Mutate(x => x.DrawText(detailsName, font, Color.Black, new PointF(10, 10)));
                }

                MemoryStream memoryStream = new();
                image.SaveAsPng(memoryStream);
                memoryStream.Seek(0, SeekOrigin.Begin);
                return memoryStream;
            }
        }
        static MemoryStream DrawStar()
        {
            int width = 640;
            int height = 480;
            using (Image<Rgba32> image = new(width, height))
            {
                Star star = new(x: width / 2, y: height / 2, prongs: 5, innerRadii: 15.0f, outerRadii: 30.0f, 0.5f);
                image.Mutate(x => x.Fill(Color.Red, star)); // fill the star with red

                MemoryStream memoryStream = new();
                image.SaveAsPng(memoryStream);
                memoryStream.Seek(0, SeekOrigin.Begin);
                return memoryStream;
            }
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