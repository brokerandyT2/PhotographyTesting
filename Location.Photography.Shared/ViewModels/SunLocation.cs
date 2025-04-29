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

        private const double SunSmoothingFactor = 0.1; // smaller = smoother (e.g., 0.1 = 10% change)

        public SunLocation()
        {
            if (Compass.Default.IsSupported)
            {
                Compass.Default.ReadingChanged += Compass_ReadingChanged;
                if(!Compass.Default.IsMonitoring)
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
            if (NorthRotationAngle > 0 && NorthRotationAngle < 180)
            {
                NorthRotationAngle = -NorthRotationAngle;
            }
            else if (NorthRotationAngle > 180 && NorthRotationAngle < 360)
            {
                NorthRotationAngle = 360 - NorthRotationAngle;
            }
            else
            {
                NorthRotationAngle = heading;
            }

            CalculateSunDirection(heading);
        }

        private void CalculateSunDirection(double heading)
        {
            TimeZoneInfo cst = TimeZoneInfo.FindSystemTimeZoneById(TimeZoneInfo.Local.Id);

            DateTime dt = SelectedDateTime;
            SolarTimes solarTimes = new SolarTimes(dt, this._latitude, this._longitude);
            var RotationAngle = Math.Round((double)solarTimes.SolarAzimuth, 0);
            //_inclination = Math.Round((double)solarTimes.SolarElevation, 0);
            double angleDiff = RotationAngle - heading;

            // Normalize to -180 to 180 degrees
            angleDiff = (angleDiff + 360) % 360;
            if (angleDiff > 180) angleDiff -= 360;

            // Fake simple sun azimuth calculation (replace later with real)
            var hourAngle = RotationAngle;


            // Smoothly interpolate from current to target
            SunDirection = SmoothAngle(SunDirection, angleDiff, SunSmoothingFactor);
        }
        public void Calculate()
        {
            this.CalculateSunDirection(this.NorthRotationAngle);
           

        }

        /// <summary>
        /// Smooth interpolation between angles, handling wrap-around at 360 degrees.
        /// </summary>
        private double SmoothAngle(double current, double target, double smoothingFactor)
        {
            double difference = ((target - current + 540) % 360) - 180;
            return (current + difference * smoothingFactor + 360) % 360;
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
