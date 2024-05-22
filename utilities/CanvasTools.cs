using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SkiaSharp;
using SuperAniki.Laggen.Models;

namespace SuperAniki.Laggen.Utilities
{
    public static class CanvasTools
    {
        public static void DrawInfoText(SKCanvas canvas, string text, int textSize, int angle, int x, int y)
        {
            // Load the Liberation font
            //using var typeface = SKTypeface.FromFile("path/to/LiberationSans-Regular.ttf");

            // Create the paint object
            using var paint = new SKPaint
            {
                TextSize = textSize,
                Color = SKColors.Black,
                IsAntialias = true,
                Style = SKPaintStyle.Fill,
                StrokeWidth = 1,
            };

            // Save the current canvas state
            canvas.Save();

            // Rotate the canvas 90 degrees around the point (x, y)
            canvas.RotateDegrees(angle, x, y);

            // Draw the text at the specified position
            canvas.DrawText(text, x, y, paint);

            // Restore the canvas to its previous state
            canvas.Restore();
        }

        public static void DrawPath(SKCanvas canvas, double x, double y, double[] points, bool closed = false, bool useFill = false)
        {
            using (var path = new SKPath())
            {
                path.MoveTo((float)(points[0] + x), (float)(points[1] + y)); // Move to the first point

                // Iterate over the vector and draw line segments
                for (int i = 2; i < points.Length; i += 2)
                {
                    path.LineTo((float)(points[i] + x), (float)(points[i + 1] + y)); // Draw line to next point
                }

                if (closed)
                {
                    path.Close(); // Close the path to connect the last point to the first
                }

                var paint = new SKPaint
                {
                    Style = useFill ? SKPaintStyle.Fill : SKPaintStyle.Stroke,
                    StrokeWidth = 1,
                    IsAntialias = true
                };

                canvas.DrawPath(path, paint);
            }
        }
        /*
                public static void DrawCurve(SKCanvas canvas, double x, double y, double[] points, string title)
                {
                    DrawPath(canvas, x, y, points);
                    using (var paint = new SKPaint { Color = SKqColors.Black, TextSize = 6 * 96.0f / 72.0f })
                    {
                        canvas.DrawText(title, x + 4.5f, y - 6f, paint);
                    }
                }*/

        public static void DrawBarrelSide(SKCanvas canvas, float x, float y, BarrelDetails barrelDetails, float scale)
        {
            var angle = barrelDetails.Angle;
            var height = barrelDetails.Height;
            var bottomDiameter = barrelDetails.BottomDiameter;
            var staveTopThickness = barrelDetails.StaveTopThickness;
            var staveBottomThickness = barrelDetails.StaveBottomThickness;
            var bottomThickness = barrelDetails.BottomThickness;
            var bottomMargin = barrelDetails.BottomMargin;

            var tan = (float)Math.Tan(angle * Math.PI / 180);
            var length = tan * height; // position till motsatt sida av vinkeln
            var hypotenusaLength = (float)Math.Sqrt((height * height) + (length * length));
            var outlinePoints = new[] { 0f, 0f, -length, -height, bottomDiameter + length, -height, bottomDiameter, 0f };
            var leftStavePoints = new[] { 0f, 0f, 0f, -hypotenusaLength, staveTopThickness, -hypotenusaLength, staveBottomThickness, 0f };
            var rightStavePoints = new[] { 0f, 0f, 0f, -hypotenusaLength, -staveTopThickness, -hypotenusaLength, -staveBottomThickness, 0f };
            var angleLength = (tan * bottomMargin) - (staveBottomThickness - 5); // angle-dependent extra length to plate
            var bottomPlantePoints = new[] { 0 + angleLength, 0f, 0 + angleLength, -bottomThickness, -bottomDiameter - angleLength, -bottomThickness, -bottomDiameter - angleLength, 0f };

            canvas.Translate(x, y);
            canvas.Scale(scale, scale);
            //canvas.LineWidth = 1;

            DrawPath(canvas, -bottomDiameter - length, 0, outlinePoints);
            canvas.Save();
            canvas.Translate((float)(-bottomDiameter - length), 0);
            canvas.RotateDegrees((float)-angle);
            DrawPath(canvas, 0, 0, leftStavePoints);
            canvas.Restore();

            canvas.Save();
            canvas.Translate((float)-length, 0);
            canvas.RotateDegrees((float)angle);
            DrawPath(canvas, 0, 0, rightStavePoints);
            canvas.Restore();

            DrawPath(canvas, -length, -bottomMargin, bottomPlantePoints);
        }

        public static void DrawStaveCurve(SKCanvas canvas, BarrelDetails barrelDetails, StaveCurveConfigDetail config, string paperType)
        {
            DrawInfoText(canvas, "Test test DrawStaveCurve", 4, 45, 15, 15);
        }

        public static void DrawStaveEnds(SKCanvas canvas, float x, float y, BarrelDetails barrelDetails, StaveEndConfigDetail config, string paperType)
        {
            DrawInfoText(canvas, "Test test DrawStaveEnds", 4, 45, 15, 15);
        }

        public static void DrawStaveFront(SKCanvas canvas, float x, float y, BarrelDetails barrelDetails, StaveFrontConfigDetail config, string paperType)
        {
            DrawInfoText(canvas, "Test test DrawStaveFront", 4, 45, 15, 15);
        }
    }
}

/*
  drawStaveCurveCTX(ctx, config.defaultPaperType as Paper, barrelDetails, config)
      drawStaveEndsCTX(ctx, paperWidth * 0.5, paperHeight, barrelDetails, config, config.defaultPaperType as Paper)
      drawStaveFrontCTX(ctx, paperWidth * 0.5, margins, barrelDetails, config, config.defaultPaperType as Paper)


export function drawStaveEndsCTX(ctx: PImage.Context, x: number, y: number, barrelDetails: BarrelDetails, config: StaveEndConfigWithData, paperState: Paper) {
	const { angle, height, bottomDiameter, staveBottomThickness, staveTopThickness } = { ...barrelDetails };
	const configDetailsArray = config.configDetails;

    xport function drawStaveFrontCTX(ctx: PImage.Context, x: number, y: number, barrelDetails: BarrelDetails, config: StaveFrontConfigWithData, paperState: Paper) {
	const { bottomDiameter, topDiameter, staveLength } = { ...barrelDetails };
	const configDetailsArray = config.configDetails;
*/