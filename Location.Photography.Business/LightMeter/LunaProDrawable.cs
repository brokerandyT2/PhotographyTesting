using Microsoft.Maui.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Location.Photography.Business.LightMeter
{
    public class LunaProDrawable : IDrawable
    {

        private float outerDialAngle = 0f;
        private float middleDialAngle = 0f;
        private float innerDialAngle = 0f;
        private float evNeedleValue = 2.3f; // Simulated EV reading
        public LunaProDrawable()
        { }
        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            canvas.SaveState();

            float centerX = dirtyRect.Center.X;
            float centerY = dirtyRect.Center.Y;
            float radius = Math.Min(dirtyRect.Width, dirtyRect.Height) / 2f - 20f;

            canvas.Translate(centerX, centerY);

            DrawEVArc(canvas, radius * 0.65f);
            DrawReferenceNeedle(canvas, radius * 0.65f, evNeedleValue);
            DrawDials(canvas, radius);
            DrawCenterDisc(canvas, radius * 0.3f);

            canvas.RestoreState();
        }

        private void DrawEVArc(ICanvas canvas, float radius)
        {
            canvas.StrokeColor = Colors.Black;
            canvas.StrokeSize = 2;

            float startAngle = 150;
            float sweepAngle = 240;

            canvas.DrawArc(-radius, -radius, radius * 2, radius * 2, startAngle, sweepAngle, false, false);

            // Tick marks and numbers
            int min = -5;
            int max = 5;
            for (int i = min; i <= max; i++)
            {
                float angle = 180 + ((i - min) / (float)(max - min)) * sweepAngle - sweepAngle / 2;
                float rad = DegreesToRadians(angle);
                float x1 = radius * (float)Math.Cos(rad);
                float y1 = radius * (float)Math.Sin(rad);
                float x2 = (radius - 10) * (float)Math.Cos(rad);
                float y2 = (radius - 10) * (float)Math.Sin(rad);
                canvas.DrawLine(x1, y1, x2, y2);

                float labelRadius = radius + 10;
                float lx = labelRadius * (float)Math.Cos(rad);
                float ly = labelRadius * (float)Math.Sin(rad);
                canvas.FontSize = 12;
                canvas.DrawString(i.ToString(), lx - 6, ly - 6, 20, 20, HorizontalAlignment.Center, VerticalAlignment.Center);
            }
        }

        private void DrawReferenceNeedle(ICanvas canvas, float radius, float ev)
        {
            float sweepAngle = 240f;
            float angle = 180 + ((ev + 5) / 10f) * sweepAngle - sweepAngle / 2;
            float rad = DegreesToRadians(angle);
            float x = (radius - 15) * (float)Math.Cos(rad);
            float y = (radius - 15) * (float)Math.Sin(rad);

            canvas.StrokeColor = Colors.Red;
            canvas.StrokeSize = 3;
            canvas.DrawLine(0, 0, x, y);
        }

        private void DrawDials(ICanvas canvas, float radius)
        {
            DrawDialRing(canvas, radius, outerDialAngle, Colors.LightGreen, 0, 20, "EV");
            DrawDialRing(canvas, radius * 0.85f, middleDialAngle, Colors.Gray, 1, 15, "F");
            DrawDialRing(canvas, radius * 0.7f, innerDialAngle, Colors.DarkGray, 25, 6400, "ISO");
        }

        private void DrawDialRing(ICanvas canvas, float radius, float angle, Color color, int min, int max, string label)
        {
            canvas.SaveState();
            canvas.Rotate(angle);
            canvas.StrokeColor = color;
            canvas.FontSize = 10;

            int stepCount = 12;
            for (int i = 0; i < stepCount; i++)
            {
                float a = DegreesToRadians(i * (360f / stepCount));
                float x = radius * (float)Math.Cos(a);
                float y = radius * (float)Math.Sin(a);
                string value = label switch
                {
                    "EV" => (min + i).ToString(),
                    "F" => $"f/{Math.Pow(2, i / 2.0):0.#}",
                    "ISO" => ((int)(min * Math.Pow(2, i))).ToString(),
                    _ => "?"
                };
                canvas.DrawString(value, x - 10, y - 6, 20, 12, HorizontalAlignment.Center, VerticalAlignment.Center);
            }

            canvas.RestoreState();
        }

        private void DrawCenterDisc(ICanvas canvas, float radius)
        {
            canvas.FillColor = Colors.Black;
            canvas.FillCircle(0, 0, radius);
            canvas.FontColor = Colors.White;
            canvas.FontSize = 18;
            canvas.DrawString("PixMap", -radius, -12, radius * 2, 24, HorizontalAlignment.Center, VerticalAlignment.Center);
        }

        private float DegreesToRadians(float degrees) => (float)(Math.PI / 180) * degrees;
    }
}

