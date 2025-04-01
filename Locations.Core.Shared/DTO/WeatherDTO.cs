using Locations.Core.Shared.DTO.Interfaces;
using Locations.Core.Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locations.Core.Shared.DTO
{
    [Table("Weather")]
    public class WeatherDTO : INotifyPropertyChanged, IWeatherDTO
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private int _id;
        private double _latitude;
        private double _longitude;
        private DateTime _sunrise_day_one;
        private DateTime _sunrise_day_two;
        private DateTime _sunrise_day_three;
        private DateTime _sunrise_day_four;
        private DateTime _sunrise_day_five;
        private DateTime _sunset_day_one;
        private DateTime _sunset_day_two;
        private DateTime _sunset_day_three;
        private DateTime _sunset_day_four;
        private DateTime _sunset_day_five;

        private string _timezone;
        private int _timezoneOffset;
        private int _temperature_day_one;
        private int _temperature_day_two;
        private int _temperature_day_three;
        private int _temperature_day_four;
        private int _temperature_day_five;
        private int _temperature_day_one_low;
        private int _temperature_day_two_low;
        private int _temperature_day_three_low;
        private int _temperature_day_four_low;
        private int _temperature_day_five_low;
        private string _forecast_day_one;
        private string _forecast_day_two;
        private string _forecast_day_three;
        private string _forecast_day_four;
        private string _forecast_day_five;

        private decimal _windSpeedDay_One;
        private int _windDirectionDay_One;
        private decimal _windGustDay_one;

        private decimal _windSpeedDay_Two;
        private int _windDirectionDay_Two;
        private decimal _windGustDay_Two;

        private decimal _windSpeedDay_Three;
        private int _windDirectionDay_Three;
        private decimal _windGustDay_Three;

        private decimal _windSpeedDay_Four;
        private int _windDirectionDay_Four;
        private decimal _windGustDay_Four;

        private decimal _windSpeedDay_Five;
        private int _windDirectionDay_Five;
        private decimal _windGustDay_Five;

        private DateTime today;
        private DateTime todayPlusOne;
        private DateTime todayPlusTwo;
        private DateTime todayPlusThree;
        private DateTime todayPlusFour;
        private DateTime todayPlusFive;
        private DateTime _lastUpdate;
        public string DateFormat { get; set; }
        [Ignore]
        public string DayOne => Sunrise_Day_One.ToShortDateString();
        [Ignore]
        public string DayTwo => Sunrise_Day_Two.ToShortDateString();
        [Ignore]
        public string DayThree => Sunrise_Day_Three.ToShortDateString();
        [Ignore]
        public string DayFour => Sunrise_Day_Four.ToShortDateString();
        [Ignore]
        public string DayFive => Sunrise_Day_Five.ToShortDateString();
        public decimal WindSpeedDay_One
        {
            get
            {
                return _windSpeedDay_One;
            }
            set
            {
                if (_windSpeedDay_One != value)
                {
                    _windSpeedDay_One = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(WindSpeedDay_One)));
                }
            }
        }
        public int WindDirectionDay_One
        {
            get
            {

                return (_windDirectionDay_One + 360) % 360;
            }
            set
            {
                if (_windDirectionDay_One != value)
                {
                    _windDirectionDay_One = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(WindDirectionDay_One)));
                }
            }
        }
        public decimal WindGustDay_One
        {
            get
            {

                return _windGustDay_one;
            }
            set { _windGustDay_one = value; }
        }
        public decimal WindSpeedDay_Two
        {
            get
            {

                return _windSpeedDay_Two;
            }
            set
            {
                if (_windSpeedDay_Two != value)
                {
                    _windSpeedDay_Two = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(WindSpeedDay_Two)));
                }
            }
        }

        public int WindDirectionDay_Two
        {
            get
            {

                return _windDirectionDay_Two;
            }
            set
            {
                if (_windDirectionDay_Two != value)
                {
                    _windDirectionDay_Two = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(WindDirectionDay_Two)));
                }
            }
        }
        public decimal WindGustDay_Two
        {
            get
            {

                return _windGustDay_Two;
            }
            set { _windGustDay_Two = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(WindGustDay_Two)));
            }
        }
        public decimal WindSpeedDay_Three
        {
            get
            {
                return _windSpeedDay_Three;
            }
            set
            {
                if (_windSpeedDay_Three != value)
                {
                    _windSpeedDay_Three = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(WindSpeedDay_Three)));
                }
            }
        }
        public int WindDirectionDay_Three
        {
            get
            {

                return (_windDirectionDay_Three + 360) % 360;
            }


            set
            {
                if (_windDirectionDay_Three != value)
                {
                    _windDirectionDay_Three = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(WindDirectionDay_Three)));
                }
            }
        }
        public decimal WindGustDay_Three
        {
            get
            {

                return _windGustDay_Three;
            }
            set { _windGustDay_Three = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(WindGustDay_Three))); }
        }
        public decimal WindSpeedDay_Four
        {
            get
            {

                return _windSpeedDay_Four;
            }
            set
            {
                if (_windSpeedDay_Four != value)
                {
                    _windSpeedDay_Four = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(WindSpeedDay_Four)));
                }
            }
        }
        public int WindDirectionDay_Four
        {
            get
            {

                return (_windDirectionDay_Four + 360) % 360;
            }
            set
            {
                if (_windDirectionDay_Four != value)
                {
                    _windDirectionDay_Four = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(WindDirectionDay_Four)));
                }
            }
        }

        public decimal WindGustDay_Four
        {
            get
            {

                return _windGustDay_Four;
            }
            set
            {
                if (_windGustDay_Four != value)
                {
                    _windGustDay_Four = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(WindGustDay_Four)));
                }
            }
        }
        public decimal WindSpeedDay_Five
        {
            get
            {

                return _windSpeedDay_Five;
            }
            set
            {
                if (_windSpeedDay_Five != value)
                {
                    _windSpeedDay_Five = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(WindSpeedDay_Five)));
                }
            }
        }

        public int WindDirectionDay_Five
        {
            get
            {

                return (_windDirectionDay_Five + 360) % 360;
            }
            set
            {
                if (_windDirectionDay_Five != value)
                {
                    _windDirectionDay_Five = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(WindDirectionDay_Five)));
                }
            }
        }
        public decimal WindGustDay_Five
        {
            get
            {

                return (_windGustDay_Five + 360) % 360;
            }
            set
            {
                if (_windGustDay_Five != value)
                {
                    _windGustDay_Five = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(WindGustDay_Five)));
                }
            }
        }





        public DateTime LastUpdate { get { return _lastUpdate; } set { _lastUpdate = value; } }
        public string Today { get { return DateTime.Today.ToShortDateString(); } }
        public string TodayPlusOne { get { return DateTime.Today.AddDays(1).ToShortDateString(); } }
        public string TodayPlusTwo { get { return DateTime.Today.AddDays(2).ToShortDateString(); } }
        public string TodayPlusThree { get { return DateTime.Today.AddDays(3).ToShortDateString(); } }
        public string TodayPlusFour { get { return DateTime.Today.AddDays(4).ToShortDateString(); } }
        public string TodayPlusFive { get { return DateTime.Today.AddDays(5).ToShortDateString(); } }

        public int Temperature_Day_One_Low
        {
            get => _temperature_day_one_low;
            set
            {
                if (_temperature_day_one_low != value)
                {
                    _temperature_day_one_low = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Temperature_Day_One_Low)));
                }
            }
        }
        public int Temperature_Day_Two_Low
        {
            get => _temperature_day_two_low;
            set
            {
                if (_temperature_day_two_low != value)
                {
                    _temperature_day_two_low = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Temperature_Day_Two_Low)));
                }
            }
        }

        public int Temperature_Day_Three_Low
        {
            get => _temperature_day_three_low;
            set
            {
                if (_temperature_day_three_low != value)
                {
                    _temperature_day_three_low = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Temperature_Day_Three_Low)));
                }
            }
        }

        public int Temperature_Day_Four_Low
        {
            get => _temperature_day_four_low;
            set
            {
                if (_temperature_day_four_low != value)
                {
                    _temperature_day_four_low = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Temperature_Day_Four_Low)));
                }
            }
        }

        public int Temperature_Day_Five_Low
        {
            get => _temperature_day_five_low;
            set
            {
                if (_temperature_day_five_low != value)
                {
                    _temperature_day_five_low = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Temperature_Day_Five_Low)));
                }
            }
        }



        [PrimaryKey, AutoIncrement]
        public int ID
        {
            get => _id;
            set
            {
                if (_id != value)
                {
                    _id = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ID)));
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
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Latitude)));
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
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Longitude)));
                }
            }
        }
        public DateTime Sunrise_Day_One
        {
            get => _sunrise_day_one;
            set
            {
                if (_sunrise_day_one != value)
                {
                    _sunrise_day_one = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Sunrise_Day_One)));
                }
            }
        }
        public DateTime Sunrise_Day_Two
        {
            get => _sunrise_day_two;
            set
            {
                if (_sunrise_day_two != value)
                {
                    _sunrise_day_two = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Sunrise_Day_Two)));
                }
            }
        }
        public DateTime Sunrise_Day_Three
        {
            get => _sunrise_day_three;
            set
            {
                if (_sunrise_day_three != value)
                {
                    _sunrise_day_three = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Sunrise_Day_Three)));
                }
            }
        }
        public DateTime Sunrise_Day_Four
        {
            get => _sunrise_day_four;
            set
            {
                if (_sunrise_day_four != value)
                {
                    _sunrise_day_four = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Sunrise_Day_Four)));
                }
            }
        }
        public DateTime Sunrise_Day_Five
        {
            get => _sunrise_day_five;
            set
            {
                if (_sunrise_day_five != value)
                {
                    _sunrise_day_five = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Sunrise_Day_Five)));
                }
            }
        }
        public DateTime Sunset_Day_One
        {
            get => _sunset_day_one;
            set
            {
                if (_sunset_day_one != value)
                {
                    _sunset_day_one = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Sunset_Day_One)));
                }
            }
        }
        public DateTime Sunset_Day_Two
        {
            get => _sunset_day_two;
            set
            {
                if (_sunset_day_two != value)
                {
                    _sunset_day_two = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Sunset_Day_Two)));
                }
            }
        }
        public DateTime Sunset_Day_Three
        {
            get => _sunset_day_three;
            set
            {
                if (_sunset_day_three != value)
                {
                    _sunset_day_three = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Sunset_Day_Three)));
                }
            }
        }
        public DateTime Sunset_Day_Four
        {
            get => _sunset_day_four;
            set
            {
                if (_sunset_day_four != value)
                {
                    _sunset_day_four = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Sunset_Day_Four)));
                }
            }
        }
        public DateTime Sunset_Day_Five
        {
            get => _sunset_day_five;
            set
            {
                if (_sunset_day_five != value)
                {
                    _sunset_day_five = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Sunset_Day_Five)));
                }
            }
        }
        public string Timezone
        {
            get => _timezone;
            set
            {
                if (_timezone != value)
                {
                    _timezone = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Timezone)));
                }
            }
        }
        public int TimezoneOffset
        {
            get => _timezoneOffset;
            set
            {
                if (_timezoneOffset != value)
                {
                    _timezoneOffset = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TimezoneOffset)));
                }
            }
        }
        public int Temperature_Day_One
        {
            get => _temperature_day_one;
            set
            {
                if (_temperature_day_one != value)
                {
                    _temperature_day_one = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Temperature_Day_One)));
                }
            }
        }
        public int Temperature_Day_Two
        {
            get => _temperature_day_two;
            set
            {
                if (_temperature_day_two != value)
                {
                    _temperature_day_two = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Temperature_Day_Two)));
                }
            }
        }
        public int Temperature_Day_Three
        {
            get => _temperature_day_three;
            set
            {
                if (_temperature_day_three != value)
                {
                    _temperature_day_three = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Temperature_Day_Three)));
                }
            }
        }
        public int Temperature_Day_Four
        {
            get => _temperature_day_four;
            set
            {
                if (_temperature_day_four != value)
                {
                    _temperature_day_four = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Temperature_Day_Four)));
                }
            }
        }
        public int Temperature_Day_Five
        {
            get => _temperature_day_five;
            set
            {
                if (_temperature_day_five != value)
                {
                    _temperature_day_five = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Temperature_Day_Five)));
                }
            }
        }
        public string Forecast_Day_One
        {
            get => _forecast_day_one;
            set
            {
                if (_forecast_day_one != value)
                {
                    _forecast_day_one = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Forecast_Day_One)));
                }
            }
        }
        public string Forecast_Day_Two
        {
            get => _forecast_day_two;
            set
            {
                if (_forecast_day_two != value)
                {
                    _forecast_day_two = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Forecast_Day_Two)));
                }
            }
        }
        public string Forecasts_Day_Three
        {
            get => _forecast_day_three;
            set
            {
                if (_forecast_day_three != value)
                {
                    _forecast_day_three = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Forecasts_Day_Three)));
                }
            }
        }
        public string Forecasts_Day_Four
        {
            get => _forecast_day_four;
            set
            {
                if (_forecast_day_four != value)
                {
                    _forecast_day_four = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Forecasts_Day_Four)));
                }
            }
        }
        public string Forecasts_Day_Five
        {
            get => _forecast_day_five;
            set
            {
                if (_forecast_day_five != value)
                {
                    _forecast_day_five = value;
                    PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Forecasts_Day_Five)));
                }
            }
        }


    }
}
