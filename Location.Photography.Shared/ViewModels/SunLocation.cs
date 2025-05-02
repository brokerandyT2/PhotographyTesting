using Innovative.SolarCalculator;
using Location.Photography.Shared.ViewModels.Interfaces;
using Locations.Core.Shared.ViewModels;
using Microsoft.Maui.Devices.Sensors;
using Microsoft.Maui.ApplicationModel;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

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
        private double _sunElevation;
        private double _deviceTilt;
        private bool _elevationMatched;

        private const double SunSmoothingFactor = 0.1;
        private const double NorthSmoothingFactor = 0.1;
        private DateTime _selectedDate;
        private DateTime _seletedTime;

        public DateTime SelectedDate
        {
            get => _selectedDate;
            set
            {
                if (_selectedDate != value)
                {
                    _selectedDate = value;
                    OnPropertyChanged();
                    SelectedDateTime = new DateTime(_selectedDate.Year, _selectedDate.Month, _selectedDate.Day, SelectedDateTime.Hour, SelectedDateTime.Minute, SelectedDateTime.Second);
                }
            }
        }
        public DateTime SelectedTime
        {
            get => _seletedTime;
            set
            {
                if (_seletedTime != value)
                {
                    _seletedTime = value;
                    OnPropertyChanged();
                    SelectedDateTime = new DateTime(SelectedDateTime.Year, SelectedDateTime.Month, SelectedDateTime.Day, _seletedTime.Hour, _seletedTime.Minute, _seletedTime.Second);
                }
            }
        }

        public bool BeginMonitoring = false;
        public SunLocation()
        {
            if (Compass.Default.IsSupported)
            {
                Compass.Default.ReadingChanged += Compass_ReadingChanged;
                if (!Compass.Default.IsMonitoring)
                    Compass.Default.Start(SensorSpeed.UI);
            }

            StartSensors();
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

        public double SunElevation
        {
            get => _sunElevation;
            set
            {
                if (_sunElevation != value)
                {
                    _sunElevation = value;
                    OnPropertyChanged();
                }
            }
        }

        public double DeviceTilt
        {
            get => _deviceTilt;
            set
            {
                if (_deviceTilt != value)
                {
                    _deviceTilt = value;
                    OnPropertyChanged();
                    CheckElevationMatch();
                }
            }
        }

        public bool ElevationMatched
        {
            get => _elevationMatched;
            set
            {
                if (_elevationMatched != value)
                {
                    _elevationMatched = value;
                    OnPropertyChanged();
                }
            }
        }
       
        private void Compass_ReadingChanged(object sender, CompassChangedEventArgs e)
        {
            if (BeginMonitoring)
            {
                var heading = e.Reading.HeadingMagneticNorth;
                UpdateNorthRotationAngle(heading);
                CalculateSunDirection(heading);
            }
        }

        private void UpdateNorthRotationAngle(double rawHeading)
        {
            NorthRotationAngle = SmoothAngle(NorthRotationAngle, rawHeading, NorthSmoothingFactor);
        }

        private void CalculateSunDirection(double heading)
        {
            var dt = SelectedDateTime;
            var solarTimes = new SolarTimes(dt, Latitude, Longitude);
            double solarAzimuth = Math.Round((double)solarTimes.SolarAzimuth, 0);
            SunElevation = solarTimes.SolarElevation;

            double angleDiff = NormalizeAngle(solarAzimuth - heading);
            SunDirection = SmoothAngle(SunDirection, angleDiff, SunSmoothingFactor);
        }

        public void StartSensors()
        {
            if (Accelerometer.Default.IsSupported && !Accelerometer.Default.IsMonitoring)
            {
                Accelerometer.Default.ReadingChanged += Accelerometer_ReadingChanged;
                Accelerometer.Default.Start(SensorSpeed.UI);
            }
        }

        private void Accelerometer_ReadingChanged(object sender, AccelerometerChangedEventArgs e)
        {
            var z = e.Reading.Acceleration.Z;
            var tilt = Math.Acos(z) * 180 / Math.PI;
            DeviceTilt = tilt;
        }

        private async void CheckElevationMatch()
        {

            if (BeginMonitoring)
            {
                if (Math.Abs(DeviceTilt - SunElevation) <= 3)
                {
                    await MainThread.InvokeOnMainThreadAsync(async () =>
                    {
                        try
                        {
                            Vibration.Default.Vibrate(TimeSpan.FromMilliseconds(100));
                            await Task.Delay(100);
                            Vibration.Default.Vibrate(TimeSpan.FromMilliseconds(100));
                            ElevationMatched = true;
                        }
                        catch { }
                    });
                }
                else
                {
                    ElevationMatched = false;
                }
            }
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
