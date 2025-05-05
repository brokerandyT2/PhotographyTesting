using Microsoft.Maui.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Location.Core.Views.Premium
{
    public class LunaProDrawable : IDrawable
    {
        private float centerX;
        private float centerY;
        private float radius;

        private int isoIndex = 2;
        private int shutterIndex = 5;
        private int apertureIndex = 3;
        public float Width { get; set; }
        public float Height { get; set; }
        public List<string> _isoValues = new List<string> { "50", "100", "200", "400", "800", "1600" };
        public List<string> _fStopValues = new List<string> { "1.4", "2.0", "2.8", "4.0", "5.6", "8.0", "11.0", "16.0", "22.0" };
        public List<string> _shutterSpeedValues = new List<string> { "1/4000", "1/2000", "1/1000", "1/500", "1/250", "1/125", "1/60", "1/30", "1/15", "1/8", "1/4", "1/2", "1" };

        public float CenterX => Width / 2;
        public float CenterY => Height / 2;
        private readonly string[] isoValues = { "25", "50", "100", "200", "400", "800", "1600", "3200" };
        private readonly string[] shutterValues = { "1/1000", "1/500", "1/250", "1/125", "1/60", "1/30", "1/15", "1/8", "1/4", "1/2", "1", "2" };
        private readonly string[] apertureValues = { "f/1.4", "f/2", "f/2.8", "f/4", "f/5.6", "f/8", "f/11", "f/16", "f/22" };

        private double needleEV = 10; // Mock initial EV

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            canvas.SaveState();

            centerX = dirtyRect.Center.X;
            centerY = dirtyRect.Center.Y;
            radius = Math.Min(dirtyRect.Width, dirtyRect.Height) / 2 - 10;

            DrawDial(canvas, isoValues, isoIndex, 0.9f, Colors.DarkGreen);
            DrawDial(canvas, shutterValues, shutterIndex, 0.7f, Colors.DarkRed);
            DrawDial(canvas, apertureValues, apertureIndex, 0.5f, Colors.Navy);

            DrawNeedle(canvas);
            DrawCenterCap(canvas);

            canvas.RestoreState();
        }

        private void DrawDial(ICanvas canvas, string[] values, int selectedIndex, float radiusFactor, Color highlight)
        {
            float r = radius * radiusFactor;
            float angleStep = 360f / values.Length;

            for (int i = 0; i < values.Length; i++)
            {
                float angleDeg = angleStep * i - 90;
                float angleRad = angleDeg * (float)Math.PI / 180f;

                float x = centerX + r * (float)Math.Cos(angleRad);
                float y = centerY + r * (float)Math.Sin(angleRad);

                canvas.FontColor = i == selectedIndex ? highlight : Colors.Black;
                canvas.FontSize = 12;
                canvas.DrawString(values[i], x, y, HorizontalAlignment.Center);
            }
        }

        private void DrawNeedle(ICanvas canvas)
        {
            float angle = (float)((needleEV - 5) / 10.0 * Math.PI); // normalize EV between -5 and +5
            float needleLength = radius * 0.95f;
            float x = centerX + needleLength * (float)Math.Cos(angle);
            float y = centerY - needleLength * (float)Math.Sin(angle);

            canvas.StrokeColor = Colors.OrangeRed;
            canvas.StrokeSize = 3;
            canvas.DrawLine(centerX, centerY, x, y);
        }

        private void DrawCenterCap(ICanvas canvas)
        {
            canvas.FillColor = Colors.Black;
            canvas.FillCircle(centerX, centerY, radius * 0.1f);
        }

        // Rotate a dial by a given angle in radians
        public void RotateDial(int dialIndex, double angleDelta)
        {
            int steps = (int)Math.Round(angleDelta / (2 * Math.PI) * GetValuesForDial(dialIndex).Length);
            switch (dialIndex)
            {
                case 0:
                    isoIndex = (isoIndex + steps + isoValues.Length) % isoValues.Length;
                    break;
                case 1:
                    shutterIndex = (shutterIndex + steps + shutterValues.Length) % shutterValues.Length;
                    break;
                case 2:
                    apertureIndex = (apertureIndex + steps + apertureValues.Length) % apertureValues.Length;
                    break;
            }
        }

        // Determine which dial is being touched
        public int HitTestDial(Point p)
        {
            double dx = p.X - centerX;
            double dy = p.Y - centerY;
            double distance = Math.Sqrt(dx * dx + dy * dy);

            if (distance < radius * 0.55f)
                return 2; // aperture
            else if (distance < radius * 0.75f)
                return 1; // shutter
            else if (distance < radius * 0.95f)
                return 0; // ISO

            return -1; // outside
        }

        private string[] GetValuesForDial(int dialIndex)
        {
            return dialIndex switch
            {
                0 => isoValues,
                1 => shutterValues,
                2 => apertureValues,
                _ => Array.Empty<string>()
            };
        }

        public int ISO => int.Parse(isoValues[isoIndex]);
        public float Aperture => float.Parse(apertureValues[apertureIndex].Substring(2));
        public float Shutter =>
            shutterValues[shutterIndex].Contains("/")
            ? 1f / float.Parse(shutterValues[shutterIndex].Split('/')[1])
            : float.Parse(shutterValues[shutterIndex]);
    }
}
