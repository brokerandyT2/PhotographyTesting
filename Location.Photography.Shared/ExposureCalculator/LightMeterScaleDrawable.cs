using System;
using Microsoft.Maui.Graphics;
using Microsoft.Maui.Controls;
using Microsoft.Maui.Gestures;

namespace Location.Photography.Shared.ExposureCalculator
{
    public class LightMeterScaleDrawable : IDrawable
    {
        private float _needleAngle;
        private float _secondNeedleAngle;
        private bool _showSecondNeedle;
        private string _iso = "800";
        private string _aperture = "f/5.6";
        private string _shutterSpeed = "1/125";
        private string exposureStep = "Full"; // Default exposure step
        private string _lastISO, _lastAperture, _lastShutterSpeed;

        public void Draw(ICanvas canvas, RectF dirtyRect)
        {
            DrawMeterScale(canvas);
            DrawNeedle(canvas);
            if (_showSecondNeedle)
            {
                DrawSecondNeedle(canvas);
            }
        }

        private void DrawMeterScale(ICanvas canvas)
        {
            var center = new PointF(150, 150);
            var radius = 100;

            // Draw scale for ISO, Aperture, and Shutter Speed
            canvas.DrawCircle(center, radius);

            DrawScaleTicks(canvas, center, radius, ISOs.GetScale(exposureStep), 10f, 0);   // ISO scale
            DrawScaleTicks(canvas, center, radius - 20, Apetures.GetScale(exposureStep), 20f, 120); // Aperture scale
            DrawScaleTicks(canvas, center, radius - 40, ShutterSpeeds.GetScale(exposureStep), 30f, 240); // Shutter speed scale
        }

        private void DrawScaleTicks(ICanvas canvas, PointF center, float radius, string[] scale, float offset, float startAngle)
        {
            var angleStep = 360f / scale.Length;
            for (int i = 0; i < scale.Length; i++)
            {
                float angle = startAngle + (i * angleStep);
                var x = center.X + radius * (float)Math.Cos(angle * Math.PI / 180);
                var y = center.Y + radius * (float)Math.Sin(angle * Math.PI / 180);
                canvas.DrawText(scale[i], x, y);
            }
        }

        private void DrawNeedle(ICanvas canvas)
        {
            var center = new PointF(150, 150);
            var radius = 80f;

            // Calculate the end point of the needle based on the current angle
            var needleEndX = center.X + radius * (float)Math.Cos(_needleAngle * Math.PI / 180);
            var needleEndY = center.Y + radius * (float)Math.Sin(_needleAngle * Math.PI / 180);

            // Draw the needle
            canvas.DrawLine(center.X, center.Y, needleEndX, needleEndY, new StrokeStyle { LineWidth = 2, LineColor = Colors.Red });
        }

        private void DrawSecondNeedle(ICanvas canvas)
        {
            var center = new PointF(150, 150);
            var radius = 80f;

            // Calculate the end point of the second needle based on the last reading angle
            var needleEndX = center.X + radius * (float)Math.Cos(_secondNeedleAngle * Math.PI / 180);
            var needleEndY = center.Y + radius * (float)Math.Sin(_secondNeedleAngle * Math.PI / 180);

            // Draw the second needle
            canvas.DrawLine(center.X, center.Y, needleEndX, needleEndY, new StrokeStyle { LineWidth = 2, LineColor = Colors.Blue });
        }

        // Handle dragging of the scales
        public void OnPanUpdated(object sender, PanUpdatedEventArgs e)
        {
            var deltaX = e.TotalX;
            var deltaY = e.TotalY;

            var angleChange = (float)Math.Atan2(deltaY, deltaX) * 180 / (float)Math.PI;
            _needleAngle += angleChange;
            _needleAngle %= 360;
        }

        // Set exposure steps (Full, Halves, Thirds)
        public void SetExposureSteps(string step)
        {
            exposureStep = step;
        }

        public void SaveLastReading()
        {
            // Save the last settings (ISO, Aperture, Shutter Speed)
            _lastISO = _iso;
            _lastAperture = _aperture;
            _lastShutterSpeed = _shutterSpeed;

            // Calculate the second needle's angle
            _secondNeedleAngle = CalculateNeedleAngle();

            // Show the second needle temporarily
            _showSecondNeedle = true;

            // Start a timer to hide the second needle after 1 second
            Device.StartTimer(TimeSpan.FromSeconds(1), () =>
            {
                _showSecondNeedle = false;
                return false; // Timer will stop
            });
        }

        private float CalculateNeedleAngle()
        {
            // Define the scale arrays based on the selected exposure step
            string[] isoScale = exposureStep switch
            {
                "Full" => ISOs.Full,
                "Halves" => ISOs.Halves,
                "Thirds" => ISOs.Thirds,
                _ => ISOs.Full
            };

            string[] apertureScale = exposureStep switch
            {
                "Full" => Apetures.Full,
                "Halves" => Apetures.Halves,
                "Thirds" => Apetures.Thirds,
                _ => Apetures.Full
            };

            string[] shutterScale = exposureStep switch
            {
                "Full" => ShutterSpeeds.Full,
                "Halves" => ShutterSpeeds.Halves,
                "Thirds" => ShutterSpeeds.Thirds,
                _ => ShutterSpeeds.Full
            };

            // Find the indices of the current settings in each scale
            int isoIndex = Array.IndexOf(isoScale, _iso);
            int apertureIndex = Array.IndexOf(apertureScale, _aperture);
            int shutterIndex = Array.IndexOf(shutterScale, _shutterSpeed);

            // If any value is not found, return the current angle (0 degrees)
            if (isoIndex == -1 || apertureIndex == -1 || shutterIndex == -1)
                return 0f;

            // Calculate the total number of possible steps based on the selected exposure step
            int totalSteps = isoScale.Length + apertureScale.Length + shutterScale.Length;

            // Calculate the individual angle for each scale (ISO, Aperture, Shutter Speed)
            float isoAngle = (isoIndex / (float)isoScale.Length) * 360f;
            float apertureAngle = (apertureIndex / (float)apertureScale.Length) * 360f;
            float shutterAngle = (shutterIndex / (float)shutterScale.Length) * 360f;

            // Combine the three angles by averaging them (with some weights if necessary)
            float finalAngle = (isoAngle + apertureAngle + shutterAngle) / 3f;

            return finalAngle;
        }
    }
}
