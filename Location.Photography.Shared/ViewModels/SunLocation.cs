using Innovative.SolarCalculator;
using Location.Photography.Shared.ViewModels.Interfaces;
using Locations.Core.Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Location.Photography.Shared.ViewModels
{
    public class SunLocation : ViewModelBase, ISunLocation
    {
       
        public override event PropertyChangedEventHandler? PropertyChanged;
        public List<LocationViewModel> List_Locations { get; set; }
        private string _imageURL  ;
        public string ImageURL
        { get { return _imageURL; } set { _imageURL = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ImageURL))); } }

        private double _latitude;
        private double _longitude;
        private bool _isNorthernHemisphere;

        private DateOnly _selectedDate = DateOnly.FromDateTime(DateTime.Now);
        private TimeOnly _selectedTime = TimeOnly.FromDateTime(DateTime.Now);

        public double Latitude
        { get => _latitude; set { _latitude = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Latitude))); } }
        public double Longitude
        {
            get => _longitude;
            set { _longitude = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Longitude))); }
        }
        public DateOnly SelectedDate
        { get => _selectedDate; set { _selectedDate = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedDate))); } }

        public TimeOnly SelectedTime
        { get => _selectedTime; set { _selectedTime = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedTime))); } }

        public DateTime SelectedDateTime
        { get { return new DateTime(SelectedDate, SelectedTime); } }

        private string _hemisphere;
        public string Hemisphere
        {
            get
            {
                if (_hemisphere == Locations.Core.Shared.MagicStrings.North)
                { return "arrow_clipart_north.png"; }
                else
                { return "arrow_clipart_south.png"; }
            }
            set
            {
                _hemisphere = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Hemisphere)));
            }
        }
        private double _rotationAngle;
        public double RotationAngle
        {
            get
            {
                return (_rotationAngle + 360) % 360;

            }
        }
        private double _inclination;
        public double Inclination
        { get => _inclination; }
        public SunLocation()
        {
            this.ToggleSensors();
        }

        private void ToggleSensors()
        {
            if (!Compass.IsMonitoring)
            {
                Compass.ReadingChanged += Compass_ReadingChanged;
                Compass.Start(SensorSpeed.Default);
            }
            if (!Accelerometer.IsMonitoring)
            {
                Accelerometer.ReadingChanged += Accelerometer_ReadingChanged;
                Accelerometer.Start(SensorSpeed.Default);
            }
        }

        private void Accelerometer_ReadingChanged(object? sender, AccelerometerChangedEventArgs e)
        {
            Vector3 acceleration = e.Reading.Acceleration;

            // Calculate phone inclination (pitch)
            var holder = Math.Atan2(acceleration.Y, acceleration.Z) * (180.0 / Math.PI);

            if (Math.Abs(holder - _inclination) < 5)
            {
                Vibration.Default.Vibrate(TimeSpan.FromMilliseconds(200));
                Vibration.Default.Vibrate(TimeSpan.FromMilliseconds(200));

            }
        }

        private void Compass_ReadingChanged(object? sender, CompassChangedEventArgs e)
        {
            var heading = e.Reading.HeadingMagneticNorth;
            double angleDiff = _rotationAngle - heading;

            // Normalize to -180 to 180 degrees
            angleDiff = (angleDiff + 360) % 360;
            if (angleDiff > 180) angleDiff -= 360;

            // Apply rotation (horizontal azimuth)
           _rotationAngle = Convert.ToDouble(angleDiff);

        }

        public void Calculate()
        {
            TimeZoneInfo cst = TimeZoneInfo.FindSystemTimeZoneById(TimeZoneInfo.Local.Id);

            DateTime dt = this.SelectedDateTime;
            SolarTimes solarTimes = new SolarTimes(dt, this._latitude, this._longitude);
            _rotationAngle = Math.Round((double)solarTimes.SolarAzimuth, 0);
            _inclination = Math.Round ((double)solarTimes.SolarElevation, 0);

        }
    }
}
