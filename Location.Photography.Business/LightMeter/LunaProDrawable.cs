using Microsoft.Maui.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Location.Photography.Business.LightMeter
{
    public class LunaProDrawable : IDrawable
    {
        public float EvValue { get; set; } = 10.0f;
        public int IsoIndex { get; set; } = 5;
        public int ApertureIndex { get; set; } = 5;
        public int ShutterIndex { get; set; } = 5;

        public List<int> IsoValues { get; set; } = new() { 25, 50, 100, 200, 400, 800, 1600 };
        public List<float> ApertureValues { get; set; } = new() { 1.0f, 1.4f, 2.0f, 2.8f, 4.0f, 5.6f, 8.0f, 11.0f, 16.0f, 22.0f, 32.0f };
        public List<string> ShutterValues { get; set; } = new() { "1s", "1/2", "1/4", "1/8", "1/15", "1/30", "1/60", "1/125", "1/250", "1/500", "1/1000" };

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            canvas.SaveState();

            // Outer casing
            float cornerRadius = 20f;
            canvas.FillColor = Colors.Black.WithAlpha(0.1f);
            canvas.FillRoundedRectangle(dirtyRect, cornerRadius);

            canvas.StrokeColor = Colors.DarkGray;
            canvas.StrokeSize = 2;
            canvas.DrawRoundedRectangle(dirtyRect, cornerRadius);

            // Split layout into top (meter) and bottom (dials)
            float meterHeight = dirtyRect.Height * 0.35f;
            var meterRect = new RectF(dirtyRect.Left + 10, dirtyRect.Top + 10, dirtyRect.Width - 20, meterHeight - 20);
            var dialRect = new RectF(dirtyRect.Left + 10, dirtyRect.Top + meterHeight + 10, dirtyRect.Width - 20, dirtyRect.Height - meterHeight - 20);

            DrawEvScaleAndNeedle(canvas, meterRect);
            DrawExposureDials(canvas, dialRect);

            canvas.RestoreState();
        }

        private void DrawEvScaleAndNeedle(ICanvas canvas, RectF rect)
        {
            canvas.SaveState();

            // Background for meter area
            canvas.FillColor = Colors.White;
            canvas.FillRoundedRectangle(rect, 10);

            canvas.StrokeColor = Colors.Gray;
            canvas.StrokeSize = 1;
            canvas.DrawRoundedRectangle(rect, 10);

            // Draw EV arc scale (EV 1 to 20)
            float centerX = rect.Center.X;
            float centerY = rect.Bottom;
            float radius = rect.Width * 0.45f;

            for (int i = 1; i <= 20; i++)
            {
                float angle = Map(i, 1, 20, 135, 45); // clockwise arc
                float rad = DegreesToRadians(angle);
                float x1 = centerX + radius * (float)Math.Cos(rad);
                float y1 = centerY - radius * (float)Math.Sin(rad);
                float x2 = centerX + (radius - 10) * (float)Math.Cos(rad);
                float y2 = centerY - (radius - 10) * (float)Math.Sin(rad);

                canvas.StrokeColor = Colors.Black;
                canvas.StrokeSize = 1;
                canvas.DrawLine(x1, y1, x2, y2);

                if (i % 2 == 0)
                {
                    float tx = centerX + (radius - 20) * (float)Math.Cos(rad);
                    float ty = centerY - (radius - 20) * (float)Math.Sin(rad);
                    canvas.FontSize = 10;
                    canvas.DrawString(i.ToString(), tx, ty, HorizontalAlignment.Center);
                }
            }

            // Draw needle
            float needleAngle = Map(EvValue, 1, 20, 135, 45);
            float needleRad = DegreesToRadians(needleAngle);
            float needleLength = radius - 15;

            float nx = centerX + needleLength * (float)Math.Cos(needleRad);
            float ny = centerY - needleLength * (float)Math.Sin(needleRad);

            canvas.StrokeColor = Colors.Red;
            canvas.StrokeSize = 2;
            canvas.DrawLine(centerX, centerY, nx, ny);

            canvas.RestoreState();
        }

        private void DrawExposureDials(ICanvas canvas, RectF rect)
        {
            canvas.SaveState();

            float centerX = rect.Center.X;
            float centerY = rect.Center.Y;
            float outerRadius = Math.Min(rect.Width, rect.Height) / 2 - 10;

            // Outer dial: Shutter Speed
            DrawDial(canvas, centerX, centerY, outerRadius, ShutterValues, ShutterIndex, Colors.DarkSlateBlue);

            // Middle dial: Aperture
            DrawDial(canvas, centerX, centerY, outerRadius * 0.75f, ApertureValues.ConvertAll(v => $"f/{v}"), ApertureIndex, Colors.Teal);

            // Inner dial: ISO
            DrawDial(canvas, centerX, centerY, outerRadius * 0.5f, IsoValues.ConvertAll(v => v.ToString()), IsoIndex, Colors.Orange);

            canvas.RestoreState();
        }

        private void DrawDial(ICanvas canvas, float cx, float cy, float radius, List<string> values, int selectedIndex, Color color)
        {
            canvas.SaveState();

            int count = values.Count;
            float angleStep = 360f / count;

            for (int i = 0; i < count; i++)
            {
                float angle = -90 + i * angleStep;
                float rad = DegreesToRadians(angle);
                float tx = cx + radius * (float)Math.Cos(rad);
                float ty = cy + radius * (float)Math.Sin(rad);

                canvas.FontSize = 10;
                canvas.FontColor = i == selectedIndex ? Colors.Yellow : Colors.White;
                canvas.DrawString(values[i], tx, ty, HorizontalAlignment.Center);
            }

            // Circle outline
            canvas.StrokeColor = color;
            canvas.StrokeSize = 2;
            canvas.DrawCircle(cx, cy, radius);

            canvas.RestoreState();
        }

        private static float Map(float value, float fromSource, float toSource, float fromTarget, float toTarget)
        {
            return fromTarget + (value - fromSource) * (toTarget - fromTarget) / (toSource - fromSource);
        }

        private static float DegreesToRadians(float degrees) => (float)(Math.PI / 180) * degrees;
    }
}

