using Location.Photography.Shared.ViewModels.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locations.Core.Shared;
using Locations.Core.Shared.ViewModels;
using System.ComponentModel;
using Innovative.SolarCalculator;
namespace Location.Photography.Shared.ViewModels
{
    public class SunCalculations : ViewModelBase, ISunCalculation
    {
        private List<LocationViewModel> locations = new List<LocationViewModel>();
        private double _latitude;
        private double _longitude;
        private DateTime _date;

        private DateTime _sunrise;
        private DateTime _sunset;
        private DateTime _goldenhourMorning;
        private DateTime _goldenmhourEvening;
        private DateTime _solarnoon;
        private DateTime _astronomicalDawn;
        private DateTime _nauticaldawn;
        private DateTime _nauticaldusk;
        private DateTime _astronomicalDusk;
        private DateTime _civildawn;
        private DateTime _civildusk;
        private List<LocationViewModel> locations1 = new List<LocationViewModel>();

        public List<LocationViewModel> Locations { get => locations1; 
            set { locations1 = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Locations))); } }
        public void CalculateSun()
        {
            TimeZoneInfo cst = TimeZoneInfo.FindSystemTimeZoneById(TimeZoneInfo.Local.Id);
            SolarTimes solarTimes = new SolarTimes(_date, _latitude, _longitude);
            this._sunrise = TimeZoneInfo.ConvertTimeFromUtc(solarTimes.Sunrise.ToUniversalTime(), cst);
            PropertyUpdater(nameof(SunRiseFormatted));
            this._sunset = TimeZoneInfo.ConvertTimeFromUtc(solarTimes.Sunset.ToUniversalTime(), cst);
            PropertyUpdater(nameof(SunSetFormatted));
            this._solarnoon = TimeZoneInfo.ConvertTimeFromUtc(solarTimes.SolarNoon.ToUniversalTime(), cst);
            PropertyUpdater(nameof(SolarNoonFormatted));
            this._astronomicalDawn = TimeZoneInfo.ConvertTimeFromUtc(solarTimes.DawnAstronomical.ToUniversalTime(), cst);
            PropertyUpdater(nameof(AstronomicalDawnFormatted));
            this._nauticaldawn = TimeZoneInfo.ConvertTimeFromUtc(solarTimes.DawnNautical.ToUniversalTime(), cst);
            PropertyUpdater(nameof(NauticalDawnFormatted));
            this._nauticaldusk = TimeZoneInfo.ConvertTimeFromUtc(solarTimes.DuskNautical.ToUniversalTime(), cst);
            PropertyUpdater(nameof(NauticalDuskFormatted));
            this._astronomicalDusk = TimeZoneInfo.ConvertTimeFromUtc(solarTimes.DuskAstronomical.ToUniversalTime(), cst);
            PropertyUpdater(nameof(AstronomicalDuskFormatted));
            this._civildawn = TimeZoneInfo.ConvertTimeFromUtc(solarTimes.DawnCivil.ToUniversalTime(), cst);
            PropertyUpdater(nameof(CivilDawnFormatted));
            this._civildusk = TimeZoneInfo.ConvertTimeFromUtc(solarTimes.DuskCivil.ToUniversalTime(), cst);
            PropertyUpdater(nameof(CivilDuskFormatted));
        }
        public string AstronomicalDawnFormatted { get => _astronomicalDawn.ToString(TimeFormat); }
        public string AstronomicalDuskFormatted { get => _astronomicalDusk.ToString(TimeFormat); }
        public string GoldenHourForm { get => _sunset.AddHours(-1).ToString(TimeFormat); }
        public DateTime AstronomicalDawn
        {
            get => _astronomicalDawn; set
            {
                _astronomicalDawn = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AstronomicalDawn)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AstronomicalDawnFormatted)));
            }
        }
        public DateTime AstronomicalDusk

        {
            get => _astronomicalDusk; set
            {
                _astronomicalDusk = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AstronomicalDusk)));
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(AstronomicalDuskFormatted)));
            }
        }

        public string DateFormat { get; set; }
        public double Latitude
        { get { return _latitude; } set { _latitude = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Latitude))); } }
        public double Longitude
        { get { return _longitude; } set { _longitude = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Longitude))); } }
        public DateTime Date
        { get => _date; set { _date = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Date))); } }


        public DateTime Sunrise
        {
            get => _sunrise; set
            {
                _sunrise = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Sunrise)));
                PropertyUpdater(nameof(SunRiseFormatted));
                PropertyUpdater(nameof(GoldenHourMorning));
                PropertyUpdater(nameof(GoldenHourMorningFormatted));
            }
        }
        public DateTime Sunset
        {
            get => _sunset; set
            {
                _sunset = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Sunset)));
                PropertyUpdater(nameof(SunSetFormatted));
                PropertyUpdater(nameof(GoldenHourEvening));
                PropertyUpdater(nameof(GoldenHourEveningFormatted));

            }
        }

        public DateTime GoldenHourMorning
        { get => _sunrise.AddHours(1); }
        public DateTime GoldenHourEvening
        { get => _sunset.AddHours(-1); }

        public DateTime SolarNoon { get => _solarnoon; set { _solarnoon = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SolarNoon))); } }

        public DateTime NauticalDawn
        { get => _nauticaldawn; set { _nauticaldawn = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NauticalDawn))); } }
        public DateTime NauticalDusk
        { get => _nauticaldusk; set { _nauticaldusk = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(NauticalDusk))); } }

        public string SunRiseFormatted { get => _sunrise.ToString(TimeFormat); }
        public string SunSetFormatted { get => _sunset.ToString(TimeFormat); }
        public string GoldenHourMorningFormatted { get => _sunrise.AddHours(1).ToString(TimeFormat); }
        public string GoldenHourEveningFormatted { get => _sunset.AddHours(-1).ToString(TimeFormat); }
        public string SolarNoonFormatted { get => _solarnoon.ToString(TimeFormat); }
        public string NauticalDawnFormatted { get => _nauticaldawn.ToString(TimeFormat); }
        public string NauticalDuskFormatted { get => _nauticaldusk.ToString(TimeFormat); }
        public string TimeFormat { get; set; }
        public string CivilDawnFormatted { get => _civildawn.ToString(TimeFormat); }
        public string CivilDuskFormatted { get => _civildawn.ToString(TimeFormat); }
        public DateTime Civildawn { get => _civildawn; set => _civildawn = value; }
        public DateTime Civildusk
        {
            get => _civildusk; set
            {
                _civildusk = value;
            }
        }

        private void PropertyUpdater(string v)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(v));
        }

        public override event PropertyChangedEventHandler? PropertyChanged;
    }
}
