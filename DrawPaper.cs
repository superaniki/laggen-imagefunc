
using SixLabors.Fonts;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Drawing;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using Microsoft.Extensions.Logging;

namespace SuperAniki.Laggen
{
  public static class Paper
  {
    public static MemoryStream? DrawPaper(BarrelForPrintouts barrel, ILogger logger)
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
  }


}