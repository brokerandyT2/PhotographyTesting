using Location.Photography.Shared.ViewModels.Interfaces;
using SkiaSharp;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using CommunityToolkit.Mvvm.ComponentModel;
namespace Location.Photography.Shared.ViewModels
{
    public class LightMeter : ViewModelBase, ILightMeter
    {
        // Properties for camera settings (e.g., ISO and Aperture)
        public ObservableCollection<int> ISOs { get; set; }
        public ObservableCollection<double> Apertures { get; set; }
        public int SelectedISO { get; set; }
        public double SelectedAperture { get; set; }

        // Properties for the Light Meter
        private double _needleRotation;
        public double NeedleRotation
        {
            get => _needleRotation;
            set => SetProperty(ref _needleRotation, value);
        }

        private double _evValue;

        public override event PropertyChangedEventHandler? PropertyChanged;

        public double EVValue
        {
            get => _evValue;
            set => SetProperty(ref _evValue, value);
        }

        // Constructor to initialize default values
        public LightMeter()
        {
            ISOs = new ObservableCollection<int> { 100, 200, 400, 800, 1600, 3200 };
            Apertures = new ObservableCollection<double> { 1.4, 2.0, 2.8, 4.0, 5.6, 8.0, 11.0, 16.0, 22.0 };

            SelectedISO = 100;
            SelectedAperture = 2.8;

            // Initialize EV to a default value (e.g., 0)
            EVValue = 0;
            NeedleRotation = 0;
        }

        // Method to process the camera frame
        public void ProcessFrame(SKBitmap frame)
        {
            if (frame == null)
                return;

            // Process the frame to calculate the EV value
            double ev = CalculateExposureValue(frame);

            // Update EV value and needle rotation
            EVValue = ev;
            NeedleRotation = MapEVToNeedleRotation(ev);

            // Optionally, trigger the UI update here or in a timer if needed
        }

        // Method to calculate Exposure Value from a frame (simplified)
        private double CalculateExposureValue(SKBitmap frame)
        {
            // Here, you could analyze the frame (e.g., compute brightness or average color)
            // For simplicity, let's assume a static EV calculation for now
            // In a real application, you'd likely use more complex image processing or AI to calculate EV

            double brightness = 0;
            int pixelCount = 0;

            // Example: Average brightness calculation (simplified)
            for (int y = 0; y < frame.Height; y++)
            {
                for (int x = 0; x < frame.Width; x++)
                {
                    var pixel = frame.GetPixel(x, y);
                    // Convert pixel color to grayscale brightness (simplified)
                    brightness += (0.2126 * pixel.Red + 0.7152 * pixel.Green + 0.0722 * pixel.Blue);
                    pixelCount++;
                }
            }

            brightness /= pixelCount; // average brightness

            // Convert brightness to EV (simplified formula)
            return brightness > 128 ? 0 : -5; // Placeholder logic, replace with actual EV calculation
        }

        // Method to map EV value to needle rotation
        private double MapEVToNeedleRotation(double ev)
        {
            // Map the EV range to a rotation angle for the needle (this is just an example)
            // For instance, -5 EV maps to 135 degrees, +20 EV maps to 270 degrees
            double angle = 135 + (ev + 5) * (270 / 25); // Assuming EV range is from -5 to +20
            return angle;
        }
    }
}
