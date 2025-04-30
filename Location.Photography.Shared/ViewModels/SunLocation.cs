using Innovative.SolarCalculator;
using Location.Photography.Shared.ViewModels.Interfaces;
using Locations.Core.Shared.ViewModels;
using Microsoft.Maui;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Location.Photography.Shared.ViewModels
{
    public class SunLocation : ViewModelBase, ISunLocation
    {
        public override event PropertyChangedEventHandler? PropertyChanged;
        private DateTime _selectedDateTime = DateTime.Now;
        private double _latitude;
        private double _longitude;
        private double _northRotationAngle;
        private double _sunDirection;

        private const double SunSmoothingFactor = 0.1;
        private const double NorthSmoothingFactor = 0.1;

        public SunLocation()
        {
            if (Compass.Default.IsSupported)
            {
                Compass.Default.ReadingChanged += Compass_ReadingChanged;
                if (!Compass.Default.IsMonitoring)
                    Compass.Default.Start(SensorSpeed.UI);
            }

            Latitude = 0;
            Longitude = 0;
        }

        public DateTime SelectedDateTime
        {
            get => _selectedDateTime;
            set
            {
                if (_selectedDateTime != value)
                {
                    _selectedDateTime = value;
                    OnPropertyChanged();
                    CalculateSunDirection(NorthRotationAngle);
                }
            }
        }

        public double Latitude
        {
            get => _latitude;
            set
            {
                if (_latitude != value)
                {
                    _latitude = value;
                    OnPropertyChanged();
                    CalculateSunDirection(NorthRotationAngle);
                }
            }
        }

        public double Longitude
        {
            get => _longitude;
            set
            {
                if (_longitude != value)
                {
                    _longitude = value;
                    OnPropertyChanged();
                    CalculateSunDirection(NorthRotationAngle);
                }
            }
        }

        public double NorthRotationAngle
        {
            get => _northRotationAngle;
            set
            {
                if (_northRotationAngle != value)
                {
                    _northRotationAngle = value;
                    OnPropertyChanged();
                }
            }
        }

        public double SunDirection
        {
            get => _sunDirection;
            set
            {
                if (_sunDirection != value)
                {
                    _sunDirection = value;
                    OnPropertyChanged();
                }
            }
        }

        private void Compass_ReadingChanged(object sender, CompassChangedEventArgs e)
        {
            var heading = e.Reading.HeadingMagneticNorth;

            // Update both directions using smoothing
            UpdateNorthRotationAngle(heading);
            CalculateSunDirection(heading);
        }

        private void UpdateNorthRotationAngle(double rawHeading)
        {
            NorthRotationAngle = SmoothAngle(NorthRotationAngle, rawHeading, NorthSmoothingFactor);
        }

        private void CalculateSunDirection(double heading)
        {
            DateTime dt = SelectedDateTime;
            SolarTimes solarTimes = new SolarTimes(dt, Latitude, Longitude);
            double solarAzimuth = Math.Round((double)solarTimes.SolarAzimuth, 0);

            // Difference between sun and compass heading
            double angleDiff = NormalizeAngle(solarAzimuth - heading);

            SunDirection = SmoothAngle(SunDirection, angleDiff, SunSmoothingFactor);
        }

        public void Calculate()
        {
            CalculateSunDirection(NorthRotationAngle);
        }

        private double SmoothAngle(double current, double target, double smoothingFactor)
        {
            double difference = ((target - current + 540) % 360) - 180;
            return (current + difference * smoothingFactor + 360) % 360;
        }

        private double NormalizeAngle(double angle)
        {
            angle = angle % 360;
            if (angle < 0) angle += 360;
            return angle;
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
