using Microsoft.Maui.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Location.Photography.Business.LightMeter
{

    public class LunaProDrawable : IDrawable
    {
        private float evNeedleValue = 2.3f; // Simulated EV value
        private float centerX;
        private float centerY;
        private float deviceWidth;
        private float devceHeight;
        private float radius;
        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            canvas.SaveState();

            centerX = dirtyRect.Center.X;
            centerY = dirtyRect.Center.Y;
            deviceWidth = dirtyRect.Width;
            devceHeight = dirtyRect.Height;
            radius = Math.Min(dirtyRect.Width, dirtyRect.Height) / 2f - 20f;

            canvas.Translate(centerX, centerY);
            DrawBody(canvas);
            DrawScaleBackground(canvas, radius-30);
            DrawEVTicksAndLabels(canvas, radius-50);
            DrawNeedle(canvas, radius-40, evNeedleValue);

            canvas.RestoreState();
        }

        private void DrawBody(ICanvas canvas)
        {
            canvas.FillColor = Color.FromArgb("#866445");
            canvas.FillRoundedRectangle(-radius - 10, -radius - 10, (radius+10)*2, radius*3, 20);

        }

        private void DrawScaleBackground(ICanvas canvas, float radius)
        {
            float sweepAngle = 140f;
            float startAngle = 270f - sweepAngle / 2f;

            // Draw semi-circle background behind scale
            canvas.FillColor = Color.FromArgb("#f8f8f2"); // Off-white
            canvas.FillRoundedRectangle(-radius - 5, -radius - 20, radius * 2 + 5, radius, 10);

        }

        private void DrawEVTicksAndLabels(ICanvas canvas, float radius)
        {
            int minEV = -7;
            int maxEV = 7;
            float sweepAngle = 140f;

            canvas.StrokeColor = Colors.Black;
            canvas.StrokeSize = 2;
            canvas.FontColor = Colors.Black;
            canvas.FontSize = 12;

            for (int i = 0; i <= (maxEV - minEV); i++)
            {
                float ev = minEV + i;
                float angle = 270f + (ev / 14f) * sweepAngle; // 14 = range from -7 to +7
                float rad = DegreesToRadians(angle);

                float x1 = radius * (float)Math.Cos(rad) ;
                float y1 = radius * (float)Math.Sin(rad);
                float x2 = (radius - 10) * (float)Math.Cos(rad);
                float y2 = (radius - 10) * (float)Math.Sin(rad);
                canvas.DrawLine(x1, y1, x2, y2);

                float labelRadius = radius + 10;
                float lx = labelRadius * (float)Math.Cos(rad);
                float ly = labelRadius * (float)Math.Sin(rad);
                canvas.DrawString(ev.ToString(), lx - 10, ly - 6, 20, 20, HorizontalAlignment.Center, VerticalAlignment.Center);
            }
        }

        private void DrawNeedle(ICanvas canvas, float radius, float ev)
        {
            float sweepAngle = 140f;
            float angle = 270f + (ev / 14f) * sweepAngle;

            float rad = DegreesToRadians(angle);
            float x = (radius - 15) * (float)Math.Cos(rad);
            float y = (radius - 15) * (float)Math.Sin(rad);

            canvas.StrokeColor = Colors.Red;
            canvas.StrokeSize = 2;
            canvas.DrawLine(0, 0, x, y);
        }

        private float DegreesToRadians(float degrees) => (float)(Math.PI / 180) * degrees;
    }



}




