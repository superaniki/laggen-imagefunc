/*
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
*/
using Microsoft.Extensions.Logging;
using SkiaSharp;
using SuperAniki.Laggen.Models;

namespace SuperAniki.Laggen.Services
{
  public static class ImageGenerator
  {

    // Define the dictionary to map Paper enum values to their corresponding sizes
    public static readonly Dictionary<string, int[]> PaperSizes = new Dictionary<string, int[]>
        {
            { "A3", [297, 420] },
            { "A4", [210, 297] }
        };

    private static bool GetPaperType(StaveTool type, Barrel barrel, out string paperType)
    {
      switch (type)
      {
        case StaveTool.Curve:
          paperType = barrel.StaveCurveConfig!.DefaultPaperType!;
          return true;
        case StaveTool.End:
          paperType = barrel.StaveEndConfig!.DefaultPaperType!;
          return true;
        case StaveTool.Front:
          paperType = barrel.StaveFrontConfig!.DefaultPaperType!;
          return true;
        default:
          paperType = "GetPaperType error";
          return false;
      }
    }
    /*
        public static MemoryStream? Draw_ImageSharp(BarrelForPrintouts barrel, float scale, ILogger logger)
        {
          StaveTool toolState = barrel.StaveToolState;

          FontCollection collection = new();
          try
          {
            //https://github.com/shantigilbert/liberation-fonts-ttf/blob/master/LiberationMono-Regular.ttf
            collection.Add("fonts/LiberationMono-Regular.ttf");
          }
          catch (Exception e)
          {
            logger.LogError("Font loading exception: " + e.Message);
          }

          //IStaveConfig? config;
          string paperType;
          if (!GetPaperType(toolState, barrel, out paperType))
          {
            logger.LogError("error finding paper size");
          }
          //return DrawErrorMessage(collection, "error finding paper size");

          int[] paperSize = PaperSizes[paperType];


          int width = paperSize[0];
          int height = paperSize[1];

          using (Image<Rgba32> image = new(width, height))
          {
            if (collection.TryGet("Liberation Mono", out FontFamily family))
            {
              // family will not be null here
              Font font = family.CreateFont(12, FontStyle.Italic);
              string detailsName = barrel.BarrelDetails.Name;
              //Pen p = new Pen()
              var points = new PointF[4];
              points[0] = new PointF(x: 0, y: 0);
              points[1] = new PointF(x: width - 1, y: 0);
              points[2] = new PointF(x: width - 1, y: height - 1);
              points[3] = new PointF(x: 0, y: height - 1);
              //PatternPen pen = Pens.DashDot(Color.Green, 5);
              var pen = Pens.Solid(Color.BlueViolet, 2);

              image.Mutate(x => x.DrawLine(pen, points));
              image.Mutate(x => x.DrawText(detailsName, font, Color.Black, new PointF(10, 10)));
            }

            MemoryStream memoryStream = new();
            image.SaveAsPng(memoryStream);
            memoryStream.Seek(0, SeekOrigin.Begin);
            return memoryStream;
          }
        }

    */

    public static MemoryStream? Draw(Barrel barrel, int scale, ILogger logger)
    {
      StaveTool toolState = barrel.StaveToolState;
      string paperType;
      if (!GetPaperType(toolState, barrel, out paperType))
      {
        logger.LogError("error finding paper size");
      }

      int[] paperSize = PaperSizes[paperType];
      int width = paperSize[0] * scale;
      int height = paperSize[1] * scale;
      using (var bitmap = new SKBitmap(width, height))
      {
        // Create a canvas to draw on the bitmap
        using (var canvas = new SKCanvas(bitmap))
        {
          // Set the background color
          canvas.Scale(scale);
          canvas.Clear(SKColors.Beige);

          // Draw a rectangle
          var paint = new SKPaint
          {
            Color = SKColors.Blue,
            IsAntialias = true,
            Style = SKPaintStyle.Fill,
            StrokeWidth = 1
          };

          canvas.DrawRect(new SKRect(10, 10, 50, 50), paint);
          string detailsName = barrel.BarrelDetails!.Name!;
          canvas.DrawText(detailsName, 10, 10, paint);
        }
        using (var image = SKImage.FromBitmap(bitmap))
        using (var data = image.Encode(SKEncodedImageFormat.Png, 100))
        {
          MemoryStream memoryStream = new();
          data.SaveTo(memoryStream);
          memoryStream.Seek(0, SeekOrigin.Begin); // Reset the stream position to the beginning
          return memoryStream;
        }
      }
    }


    public static MemoryStream? Draw_SkiaSharp_Example(Barrel barrel, float scale, ILogger logger)
    {
      // Define the image dimensions
      int width = 800;
      int height = 600;

      // Create an empty bitmap with the specified dimensions
      using (var bitmap = new SKBitmap(width, height))
      {
        // Create a canvas to draw on the bitmap
        using (var canvas = new SKCanvas(bitmap))
        {
          // Set the background color
          canvas.Clear(SKColors.White);

          // Draw a rectangle
          var paint = new SKPaint
          {
            Color = SKColors.Blue,
            IsAntialias = true,
            Style = SKPaintStyle.Stroke,
            StrokeWidth = 3
          };

          canvas.DrawRect(new SKRect(100, 100, 700, 500), paint);

          // Draw a circle
          paint.Color = SKColors.Red;
          canvas.DrawCircle(400, 300, 100, paint);

          // Draw some text
          paint.Color = SKColors.Green;
          paint.TextSize = 50;
          canvas.DrawText("Hello SkiaSharp", 200, 100, paint);

          // Draw a line
          paint.Color = SKColors.Black;
          canvas.DrawLine(100, 100, 700, 500, paint);
        }

        // Save the bitmap as a PNG file
        using (var image = SKImage.FromBitmap(bitmap))
        using (var data = image.Encode(SKEncodedImageFormat.Png, 100))
        {
          MemoryStream memoryStream = new();
          data.SaveTo(memoryStream);
          memoryStream.Seek(0, SeekOrigin.Begin); // Reset the stream position to the beginning
          return memoryStream;


          // using (var fileStream = new FileStream("output.png", FileMode.Create, FileAccess.Write))
          // {
          //   memoryStream.CopyTo(fileStream);
          // }

        }

      }

    }
  }
}


/*
        if (config != null)
        {
          var configDetails = config.ConfigDetails.Select(item => item.PaperType == paperType);
        }
        */



/*



    public static MemoryStream DrawErrorMessage(FontCollection fonts, string message)
    {
      int width = 640;
      int height = 480;
      using (Image<Rgba32> image = new(width, height))
      {
        if (fonts.TryGet("Liberation Mono", out FontFamily family))
        {
          Font font = family.CreateFont(12, FontStyle.Italic);
          image.Mutate(x => x.DrawText(message, font, Color.Black, new PointF(10, 10)));
        }

        MemoryStream memoryStream = new();
        image.SaveAsPng(memoryStream);
        memoryStream.Seek(0, SeekOrigin.Begin);
        return memoryStream;
      }
    }


    public static MemoryStream DrawStar()
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

public static MemoryStream? DrawPaper_SkiaSharp(BarrelForPrintouts barrel, float scale, ILogger logger)
{
// Define the image dimensions
int width = 800;
int height = 600;

//  try
//  {
// Create an empty bitmap with the specified dimensions
using (var bitmap = new SKBitmap(width, height))
{
// Create a canvas to draw on the bitmap
using (var canvas = new SKCanvas(bitmap))
{
  // Set the background color
  canvas.Clear(SKColors.White);

  // Draw a rectangle
  var paint = new SKPaint
  {
    Color = SKColors.Blue,
    IsAntialias = true,
    Style = SKPaintStyle.Stroke,
    StrokeWidth = 3
  };

  canvas.DrawRect(new SKRect(100, 100, 700, 500), paint);

  // Draw a circle
  paint.Color = SKColors.Red;
  canvas.DrawCircle(400, 300, 100, paint);

  // Draw some text
  paint.Color = SKColors.Green;
  paint.TextSize = 50;
  canvas.DrawText("Hello SkiaSharp", 200, 100, paint);

  // Draw a line
  paint.Color = SKColors.Black;
  canvas.DrawLine(100, 100, 700, 500, paint);
}

// Save the bitmap as a PNG file
using (var image = SKImage.FromBitmap(bitmap))
using (var data = image.Encode(SKEncodedImageFormat.Png, 100))
{
  MemoryStream memoryStream = new();
  data.SaveTo(memoryStream);
  memoryStream.Seek(0, SeekOrigin.Begin); // Reset the stream position to the beginning
  return memoryStream;


    // using (var fileStream = new FileStream("output.png", FileMode.Create, FileAccess.Write))
    // {
    //   memoryStream.CopyTo(fileStream);
    // }

}
}


*/