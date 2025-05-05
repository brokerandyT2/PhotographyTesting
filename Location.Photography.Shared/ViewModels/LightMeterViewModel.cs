using CommunityToolkit.Mvvm.ComponentModel;
using SkiaSharp;
using System;
using System.Collections.ObjectModel;
using Location.Core.Platforms.Android.Interface;
using Microsoft.Maui.Controls;

namespace Location.Photography.Shared.ViewModels
{
    public class LightMeterViewModel : ObservableObject
    {
        // Observable collections for selectable values
        public ObservableCollection<int> ISOs { get; set; }
        public ObservableCollection<double> Apertures { get; set; }
        public ObservableCollection<double> ShutterSpeeds { get; set; }

        // Currently selected values for ISO, Aperture (f-stop), and Shutter Speed
        public int SelectedISO { get; set; }
        public double SelectedAperture { get; set; }
        public double SelectedShutterSpeed { get; set; }

        private double _needleRotation;
        public double NeedleRotation
        {
            get => _needleRotation;
            set => SetProperty(ref _needleRotation, value);
        }

        private double _peakNeedleRotation;
        public double PeakNeedleRotation
        {
            get => _peakNeedleRotation;
            set => SetProperty(ref _peakNeedleRotation, value);
        }

        private double _evValue;
        public double EVValue
        {
            get => _evValue;
            set => SetProperty(ref _evValue, value);
        }

        private IAmbientLightSensorService _ambientLightSensorService;

        // Constructor to initialize values and sensor service
        public LightMeterViewModel(IAmbientLightSensorService ambientLightSensorService)
        {
            ISOs = new ObservableCollection<int> { 100, 200, 400, 800, 1600, 3200, 6400 };
            Apertures = new ObservableCollection<double> { 1.4, 2.0, 2.8, 4.0, 5.6, 8.0, 11.0, 16.0, 22.0 };
            ShutterSpeeds = new ObservableCollection<double> { 1 / 4000.0, 1 / 2000.0, 1 / 1000.0, 1 / 500.0, 1 / 250.0, 1 / 125.0, 1 / 60.0, 1 / 30.0, 1 / 15.0, 1 / 8.0, 1 / 4.0, 1 / 2.0, 1.0 };

            // Default selections
            SelectedISO = 100;
            SelectedAperture = 2.8;
            SelectedShutterSpeed = 1 / 125.0;

            // Start with EV = 0
            EVValue = 0;
            NeedleRotation = MapEVToNeedleRotation(EVValue);
            PeakNeedleRotation = NeedleRotation;

            // Initialize the light sensor service
            _ambientLightSensorService = ambientLightSensorService;
            _ambientLightSensorService.LightLevelChanged += OnLightLevelChanged;

            // Start listening to the ambient light sensor
            _ambientLightSensorService.StartListening();
        }

        // Event handler for light level changes from the sensor
        private void OnLightLevelChanged(object sender, float lux)
        {
            // Calculate EV from the light sensor data (lux)
            double ev = CalculateEVFromLux(lux);
            EVValue = ev;

            // Map the EV value to the needle rotation
            double angle = MapEVToNeedleRotation(ev);
            NeedleRotation = angle;

            // Update peak needle rotation if necessary
            if (angle > PeakNeedleRotation)
                PeakNeedleRotation = angle;
        }

        // Calculate EV from lux, considering ISO, Aperture, and Shutter Speed
        private double CalculateEVFromLux(float lux)
        {
            // The EV formula: EV = log2((Lux * Shutter Speed) / (ISO * Aperture^2))
            // For simplicity, we ignore the base EV offset and calculate based on sensor readings.
            double apertureSquared = Math.Pow(SelectedAperture, 2);
            double ev = Math.Log2((lux * SelectedShutterSpeed) / (SelectedISO * apertureSquared));

            // Clamp the EV value between -5 and 5
            ev = Math.Max(-5, Math.Min(5, ev));

            return ev;
        }

        // Map the EV value to a needle rotation (135-270 degrees)
        private double MapEVToNeedleRotation(double ev)
        {
            return 135 + (ev + 5) * (270 / 25);
        }

        // Stop listening to the light sensor
        public void StopLightSensor()
        {
            _ambientLightSensorService.LightLevelChanged -= OnLightLevelChanged;
            _ambientLightSensorService.StopListening();
        }
    }
}
