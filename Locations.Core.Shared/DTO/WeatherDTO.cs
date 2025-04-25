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
        #region Private Backing Fields
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
        private DateTime moonRise_Day_One;
        private DateTime moonSet_Day_One;
        private double moonPhaseAs_Number_DayOne;
        private string? summary_Day_One;
        private double temperature_Day_One_Min;
        private double temperature_Day_One_Max;
        private double temperature_Day_One_Night;
        private double temperature_Day_One_Morning;
        private double uV_Index_DayOne;
        private double day_One_Dew_Point;
        private int day_One_Humidity;
        private int day_One_Pressure;
        private string? weather_Day_One_Icon;
        private string? weather_Day_One_Description;
        private double temperature_Day_One_Low_Feels_Like;
        private double temperature_Day_One_Night_Feels_Like;
        private double temperature_Day_One_Eve_Feels_Like;
        private int weather_Day_One_ID;
        private string? weather_Day_One_Main;
        private double temperature_Day_One_Morn_Feels_Like;
        private DateTime moonRise_Day_Two;
        private DateTime moonSet_Day_Two;
        private string? summary_Day_Two;
        private double temperature_Day_Two_Min;
        private double temperature_Day_Two_Max;
        private double temperature_Day_Two_Night;
        private double temperature_Day_Two_Morning;
        private double temperature_Day_Two_Low_Feels_Like;
        private double temperature_Day_Two_Night_Feels_Like;
        private double temperature_Day_Two_Eve_Feels_Like;
        private double temperature_Day_Two_Morn_Feels_Like;
        private int weather_Day_Two_ID;
        private string? weather_Day_Two_Main;
        private string? weather_Day_Two_Description;
        private string? weather_Day_Two_Icon;
        private DateTime moonRise_Day_Three;
        private DateTime moonSet_Day_Three;
        private string? summary_Day_Three;
        private double temperature_Day_Three_Min;
        private double temperature_Day_Three_Max;
        private double temperature_Day_Three_Night;
        private double temperature_Day_Three_Morning;
        private double temperature_Day_Three_Low_Feels_Like;
        private double temperature_Day_Three_Night_Feels_Like;
        private double temperature_Day_Three_Eve_Feels_Like;
        private double temperature_Day_Three_Morn_Feels_Like;
        private int weather_Day_Three_ID;
        private string? weather_Day_Three_Main;
        private string? weather_Day_Three_Description;
        private string? weather_Day_Three_Icon;
        private double uV_Index_DayTwo;
        private double uV_Index_DayThree;
        private int day_Three_Pressure;
        private int day_Three_Humidity;
        private double day_Three_Dew_Point;
        private int day_Two_Pressure;
        private int day_Two_Humidity;
        private double day_Two_Dew_Point;
        private DateTime moonRise_Day_Four;
        private DateTime moonSet_Day_Four;
        private string? summary_Day_Four;
        private double temperature_Day_Four_Min;
        private double temperature_Day_Four_Max;
        private double temperature_Day_Four_Night;
        private double temperature_Day_Four_Morning;
        private double temperature_Day_Four_Low_Feels_Like;
        private double temperature_Day_Four_Night_Feels_Like;
        private double temperature_Day_Four_Eve_Feels_Like;
        private double temperature_Day_Four_Morn_Feels_Like;
        private int weather_Day_Four_ID;
        private string? weather_Day_Four_Main;
        private string? weather_Day_Four_Description;
        private string? weather_Day_Four_Icon;
        private int day_Four_Pressure;
        private double day_Four_Dew_Point;
        private int day_Four_Humidity;
        private double uV_Index_DayFour;
        private DateTime moonRise_Day_Five;
        private DateTime moonSet_Day_Five;
        private string? summary_Day_Five;
        private double temperature_Day_Five_Min;
        private double temperature_Day_Five_Max;
        private double temperature_Day_Five_Night;
        private double temperature_Day_Five_Morning;
        private double temperature_Day_Five_Low_Feels_Like;
        private double temperature_Day_Five_Night_Feels_Like;
        private double temperature_Day_Five_Eve_Feels_Like;
        private double temperature_Day_Five_Morn_Feels_Like;
        private int weather_Day_Five_ID;
        private string? weather_Day_Five_Main;
        private string? weather_Day_Five_Description;
        private string? weather_Day_Five_Icon;
        private int day_Five_Pressure;
        private int day_Five_Humidity;
        private double day_Five_Dew_Point;
        private double uV_Index_DayFive;
        private DateTime sunrise_Day_Six;
        private double uV_Index_DaySeven;
        private double temperature_Day_Seven_Low;
        private double windGustDay_Seven;
        private int windDirectionDay_Seven;
        private double windSpeedDay_Seven;
        private double day_Seven_Dew_Point;
        private int day_Seven_Humidity;
        private int day_Seven_Pressure;
        private string? weather_Day_Seven_Icon;
        private string? weather_Day_Seven_Description;
        private string? weather_Day_Seven_Main;
        private int weather_Day_Seven_ID;
        private double temperature_Day_Seven_Morn_Feels_Like;
        private double temperature_Day_Seven_Eve_Feels_Like;
        private double temperature_Day_Seven_Night_Feels_Like;
        private double temperature_Day_Seven_Low_Feels_Like;
        private double temperature_Day_Seven_Morning;
        private double temperature_Day_Seven_Night;
        private double temperature_Day_Seven_Max;
        private double temperature_Day_Seven_Min;
        private double temperature_Day_Seven;
        private string? summary_Day_Seven;
        private DateTime moonSet_Day_Seven;
        private DateTime moonRise_Day_Seven;
        private DateTime _sunset_day_seven;
        private DateTime sunrise_Day_Seven;
        private double uV_Index_DaySix;
        private double temperature_Day_Six_Low;
        private double windGustDay_Six;
        private int windDirectionDay_Six;
        private double windSpeedDay_Six;
        private double day_Six_Dew_Point;
        private int day_Six_Humidity;
        private int day_Six_Pressure;
        private string? weather_Day_Six_Icon;
        private string? weather_Day_Six_Description;
        private string? weather_Day_Six_Main;
        private int weather_Day_Six_ID;
        private double temperature_Day_Six_Morn_Feels_Like;
        private double temperature_Day_Six_Eve_Feels_Like;
        private double temperature_Day_Six_Night_Feels_Like;
        private double temperature_Day_Six_Low_Feels_Like;
        private double temperature_Day_Six_Morning;
        private double temperature_Day_Six_Night;
        private double temperature_Day_Six_Max;
        private double temperature_Day_Six;
        private double temperature_Day_Six_Min;
        private string? summary_Day_Six;
        private DateTime moonRise_Day_Six;
        private DateTime _sunset_day_six;
        private DateTime moonSet_Day_Six;
        private string _timezone;
        private int _timezoneOffset;
        private double _temperature_day_one;
        private double _temperature_day_two;
        private double _temperature_day_three;
        private double _temperature_day_four;
        private double _temperature_day_five;
        private double _temperature_day_one_low;
        private double _temperature_day_two_low;
        private double _temperature_day_three_low;
        private double _temperature_day_four_low;
        private double _temperature_day_five_low;
        private string _forecast_day_one;
        private string _forecast_day_two;
        private string _forecast_day_three;
        private string _forecast_day_four;
        private string _forecast_day_five;
        private double _windSpeedDay_One;
        private double _windDirectionDay_One;
        private double _windGustDay_one;
        private double _windSpeedDay_Two;
        private double _windDirectionDay_Two;
        private double _windGustDay_Two;
        private double _windSpeedDay_Three;
        private double _windDirectionDay_Three;
        private double _windGustDay_Three;
        private double _windSpeedDay_Four;
        private double _windDirectionDay_Four;
        private double _windGustDay_Four;
        private double _windSpeedDay_Five;
        private double _windDirectionDay_Five;
        private double _windGustDay_Five;
        private DateTime _lastUpdate;

        private int clouds_day_one;
        private int clouds_day_two;
        private int clouds_day_Three;
        private int clouds_day_Four;
        private int clouds_day_Five;
        private int clouds_day_Six;
        private int clouds_day_Seven;
        private int rain_Day_One;
        private int rain_Day_Two;
        private int rain_Day_Three;
        private int rain_Day_Five;
        private int rain_Day_Six;
        private int rain_Day_Seven;

        private double moonPhaseAs_Number_DayFour;
        private int rain_Day_Four;
        private double moonPhaseAs_Number_DayTwo;
        private double moonPhaseAs_Number_DayThree;
        private double moonPhaseAs_Number_DayFive;
        private double moonPhaseAs_Number_DaySix;
        private double moonPhaseAs_Number_DaySeven;
        #endregion
        public double MoonPhaseAs_Number_DayFour { get => moonPhaseAs_Number_DayFour; set => moonPhaseAs_Number_DayFour = value; }

        public int Rain_Day_Four { get => rain_Day_Four; set => rain_Day_Four = value; }
        public double MoonPhaseAs_Number_DayTwo { get => moonPhaseAs_Number_DayTwo; set => moonPhaseAs_Number_DayTwo = value; }
        public double MoonPhaseAs_Number_DayThree { get => moonPhaseAs_Number_DayThree; set => moonPhaseAs_Number_DayThree = value; }
        public double MoonPhaseAs_Number_DayFive { get => moonPhaseAs_Number_DayFive; set => moonPhaseAs_Number_DayFive = value; }
        public double MoonPhaseAs_Number_DaySix { get => moonPhaseAs_Number_DaySix; set => moonPhaseAs_Number_DaySix = value; }
        public double MoonPhaseAs_Number_DaySeven { get => moonPhaseAs_Number_DaySeven; set => moonPhaseAs_Number_DaySeven = value; }
    
        public int Rain_Day_One { get => rain_Day_One; set => rain_Day_One = value; }
        public int Rain_Day_Two { get => rain_Day_Two; set => rain_Day_Two = value; }
        public int Rain_Day_Three { get => rain_Day_Three; set => rain_Day_Three = value; }
        public int Rain_Day_Five { get => rain_Day_Five; set => rain_Day_Five = value; }
        public int Rain_Day_Six { get => rain_Day_Six; set => rain_Day_Six = value; }
        public int Rain_Day_Seven { get => rain_Day_Seven; set => rain_Day_Seven = value; }
        public int Clouds_day_one { get => clouds_day_one; set => clouds_day_one = value; }
        public int Clouds_day_two { get => clouds_day_two; set => clouds_day_two = value; }
        public int Clouds_day_Three { get => clouds_day_Three; set => clouds_day_Three = value; }
        public int Clouds_day_Four { get => clouds_day_Four; set => clouds_day_Four = value; }
        public int Clouds_day_Five { get => clouds_day_Five; set => clouds_day_Five = value; }
        public int Clouds_day_Six { get => clouds_day_Six; set => clouds_day_Six = value; }
        public int Clouds_day_Seven { get => clouds_day_Seven; set => clouds_day_Seven = value; }
        private string timeFormat;
        [Ignore]
        public string TimeFormat { get => timeFormat; set { timeFormat = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TimeFormat))); } }
        [Ignore]
        public string DateFormat { get; set; }
        [Ignore]
        public string DayOne => Sunrise_Day_One.ToString("D");
        [Ignore]
        public string DayTwo => Sunrise_Day_Two.ToString("D");
        [Ignore]
        public string DayThree => Sunrise_Day_Three.ToString("D");
        [Ignore]
        public string DayFour => Sunrise_Day_Four.ToString("D");
        [Ignore]
        public string DayFive => Sunrise_Day_Five.ToString("D");
        public double WindSpeedDay_One
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
        
        public string WindDirectionArrow { private get; set; }
       
        
      
        public double WindDirectionDay_One
        {
            get
            {
                double angle;
                if (WindDirectionArrow == MagicStrings.TowardsWind)
                {
                    angle = _windDirectionDay_One;
                }
                else
                {
                    angle = (_windDirectionDay_One + 360) % 360;
                }
                return angle;
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
        public double WindGustDay_One
        {
            get
            {

                return _windGustDay_one;
            }
            set { _windGustDay_one = value; }
        }
        public double WindSpeedDay_Two
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

        public double WindDirectionDay_Two
        {
            get
            {

                double angle;
                if (WindDirectionArrow == MagicStrings.TowardsWind)
                {
                    angle = _windDirectionDay_Two;
                }
                else
                {
                    angle = (_windDirectionDay_Two + 360) % 360;
                }
                return angle;
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
        public double WindGustDay_Two
        {
            get
            {

                return _windGustDay_Two;
            }
            set
            {
                _windGustDay_Two = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(WindGustDay_Two)));
            }
        }
        public double WindSpeedDay_Three
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
        public double WindDirectionDay_Three
        {
            get
            {

                double angle;
                if (WindDirectionArrow == MagicStrings.TowardsWind)
                {
                    angle = _windDirectionDay_Three;
                }
                else
                {
                    angle = (_windDirectionDay_Three + 360) % 360;
                }
                return angle;
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
        public double WindGustDay_Three
        {
            get
            {

                return _windGustDay_Three;
            }
            set { _windGustDay_Three = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(WindGustDay_Three))); }
        }
        public double WindSpeedDay_Four
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
        public double WindDirectionDay_Four
        {
            get
            {
                double angle;
                if (WindDirectionArrow == MagicStrings.TowardsWind)
                {
                    angle = _windDirectionDay_Four;
                }
                else
                {
                    angle = (_windDirectionDay_Four + 360) % 360;
                }
                return angle;
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

        public double WindGustDay_Four
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
        public double WindSpeedDay_Five
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

        public double WindDirectionDay_Five
        {
            get
            {
                double angle;
                if (WindDirectionArrow == MagicStrings.TowardsWind)
                {
                    angle = _windDirectionDay_Five;
                }
                else
                {
                    angle = (_windDirectionDay_Five + 360) % 360;
                }
                return angle;
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
        public double WindGustDay_Five
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

        public double Temperature_Day_One_Low
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
        public double Temperature_Day_Two_Low
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

        public double Temperature_Day_Three_Low
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

        public double Temperature_Day_Four_Low
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

        public double Temperature_Day_Five_Low
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

        private DateTime _sunrise_day_seven;
        private DateTime _sunrise_day_six;

        [Ignore]
        public string Sunrise_Day_One_String => _sunrise_day_one.ToShortTimeString();
        [Ignore]
        public string Sunrise_Day_Two_String => _sunrise_day_two.ToShortTimeString();
        [Ignore]
        public string Sunrise_Day_Three_String => _sunrise_day_three.ToShortTimeString();
        [Ignore]
        public string Sunrise_Day_Four_String => _sunrise_day_four.ToShortTimeString();
        [Ignore]
        public string Sunrise_Day_Five_String => _sunrise_day_five.ToShortTimeString();
        [Ignore]
        public string Sunrise_Day_Six_String => _sunrise_day_six.ToShortTimeString();
        [Ignore]
        public string Sunrise_Day_Seven_String => _sunrise_day_seven.ToShortTimeString();
        [Ignore]
        public string Sunset_Day_One_String => _sunset_day_one.ToShortTimeString();
        [Ignore]
        public string Sunset_Day_Two_String => _sunset_day_two.ToShortTimeString();
        [Ignore]
        public string Sunset_Day_Three_String => _sunset_day_three.ToShortTimeString();
        [Ignore]
        public string Sunset_Day_Four_String => _sunset_day_four.ToShortTimeString();
        [Ignore]
        public string Sunset_Day_Five_String => _sunset_day_five.ToShortTimeString();
        [Ignore]
        public string Sunset_Day_Six_String => _sunset_day_six.ToShortTimeString();
        [Ignore]
        public string Sunset_Day_Seven_String => _sunset_day_seven.ToShortTimeString();
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
        public double Temperature_Day_One
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
        public double Temperature_Day_Two
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
        public double Temperature_Day_Three
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
        public double Temperature_Day_Four
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
        public double Temperature_Day_Five
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

        public DateTime MoonRise_Day_One { get => moonRise_Day_One; set { moonRise_Day_One = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MoonRise_Day_One))); } }
        public DateTime MoonSet_Day_One { get => moonSet_Day_One; set { moonSet_Day_One = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MoonSet_Day_One))); } }
        public double MoonPhaseAs_Number_DayOne { get => moonPhaseAs_Number_DayOne; set { moonPhaseAs_Number_DayOne = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MoonPhaseAs_Number_DayOne))); } }
        public string? Summary_Day_One { get => summary_Day_One; set { summary_Day_One = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Summary_Day_One))); } }
        public double Temperature_Day_One_Min { get => temperature_Day_One_Min; set { temperature_Day_One_Min = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Temperature_Day_One_Min))); } }
        public double Temperature_Day_One_Max { get => temperature_Day_One_Max; set { temperature_Day_One_Max = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Temperature_Day_One_Max))); } }
        public double Temperature_Day_One_Night { get => temperature_Day_One_Night; set { temperature_Day_One_Night = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Temperature_Day_One_Night))); } }
        public double Temperature_Day_One_Morning { get => temperature_Day_One_Morning; set { temperature_Day_One_Morning = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Temperature_Day_One_Morning))); } }
        public double UV_Index_DayOne { get => uV_Index_DayOne; set { uV_Index_DayOne = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UV_Index_DayOne))); } }
        public double Day_One_Dew_Point { get => day_One_Dew_Point; set { day_One_Dew_Point = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Day_One_Dew_Point))); } }
        public int Day_One_Humidity { get => day_One_Humidity; set { day_One_Humidity = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Day_One_Humidity))); } }
        public int Day_One_Pressure { get => day_One_Pressure; set { day_One_Pressure = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Day_One_Pressure))); } }
        public string? Weather_Day_One_Icon { get => "a" + weather_Day_One_Icon + ".png"; set { weather_Day_One_Icon = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Weather_Day_One_Icon))); } }
        public string? Weather_Day_One_Description { get => weather_Day_One_Description; set { weather_Day_One_Description = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Weather_Day_One_Description))); } }
        public double Temperature_Day_One_Low_Feels_Like { get => temperature_Day_One_Low_Feels_Like; set { temperature_Day_One_Low_Feels_Like = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Temperature_Day_One_Low_Feels_Like))); } }
        public double Temperature_Day_One_Night_Feels_Like { get => temperature_Day_One_Night_Feels_Like; set { temperature_Day_One_Night_Feels_Like = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Temperature_Day_One_Night_Feels_Like))); } }
        public double Temperature_Day_One_Eve_Feels_Like { get => temperature_Day_One_Eve_Feels_Like; set { temperature_Day_One_Eve_Feels_Like = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Temperature_Day_One_Eve_Feels_Like))); } }
        public int Weather_Day_One_ID { get => weather_Day_One_ID; set { weather_Day_One_ID = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Weather_Day_One_ID))); } }
        public string? Weather_Day_One_Main { get => weather_Day_One_Main; set { weather_Day_One_Main = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Weather_Day_One_Main))); } }
        public double Temperature_Day_One_Morn_Feels_Like { get => temperature_Day_One_Morn_Feels_Like; set { temperature_Day_One_Morn_Feels_Like = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Temperature_Day_One_Morn_Feels_Like))); } }
        public DateTime MoonRise_Day_Two { get => moonRise_Day_Two; set { moonRise_Day_Two = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MoonRise_Day_Two))); } }
        public DateTime MoonSet_Day_Two { get => moonSet_Day_Two; set { moonSet_Day_Two = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MoonSet_Day_Two))); } }
        public string? Summary_Day_Two { get => summary_Day_Two; set { summary_Day_Two = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Summary_Day_Two))); } }
        public double Temperature_Day_Two_Min { get => temperature_Day_Two_Min; set { temperature_Day_Two_Min = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Temperature_Day_Two_Min))); } }
        public double Temperature_Day_Two_Max { get => temperature_Day_Two_Max; set { temperature_Day_Two_Max = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Temperature_Day_Two_Max))); } }
        public double Temperature_Day_Two_Night { get => temperature_Day_Two_Night; set { temperature_Day_Two_Night = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Temperature_Day_Two_Night))); } }
        public double Temperature_Day_Two_Morning { get => temperature_Day_Two_Morning; set { temperature_Day_Two_Morning = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Temperature_Day_Two_Morning))); } }
        public double Temperature_Day_Two_Low_Feels_Like { get => temperature_Day_Two_Low_Feels_Like; set { temperature_Day_Two_Low_Feels_Like = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Temperature_Day_Two_Low_Feels_Like))); } }
        public double Temperature_Day_Two_Night_Feels_Like { get => temperature_Day_Two_Night_Feels_Like; set { temperature_Day_Two_Night_Feels_Like = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Temperature_Day_Two_Night_Feels_Like))); } }
        public double Temperature_Day_Two_Eve_Feels_Like { get => temperature_Day_Two_Eve_Feels_Like; set { temperature_Day_Two_Eve_Feels_Like = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Temperature_Day_Two_Eve_Feels_Like))); } }
        public double Temperature_Day_Two_Morn_Feels_Like { get => temperature_Day_Two_Morn_Feels_Like; set { temperature_Day_Two_Morn_Feels_Like = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Temperature_Day_Two_Morn_Feels_Like))); } }
        public int Weather_Day_Two_ID { get => weather_Day_Two_ID; set { weather_Day_Two_ID = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Weather_Day_Two_ID))); } }
        public string? Weather_Day_Two_Main { get => weather_Day_Two_Main; set { weather_Day_Two_Main = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Weather_Day_Two_Main))); } }
        public string? Weather_Day_Two_Description { get => weather_Day_Two_Description; set { weather_Day_Two_Description = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Weather_Day_Two_Description))); } }
        public string? Weather_Day_Two_Icon { get => "a" + weather_Day_Two_Icon + ".png"; set { weather_Day_Two_Icon = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Weather_Day_Two_Icon))); } }
        public DateTime MoonRise_Day_Three { get => moonRise_Day_Three; set { moonRise_Day_Three = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MoonRise_Day_Three))); } }
        public DateTime MoonSet_Day_Three { get => moonSet_Day_Three; set { moonSet_Day_Three = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MoonSet_Day_Three))); } }
        public string? Summary_Day_Three { get => summary_Day_Three; set { summary_Day_Three = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Summary_Day_Three))); } }
        public double Temperature_Day_Three_Min { get => temperature_Day_Three_Min; set { temperature_Day_Three_Min = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Temperature_Day_Three_Min))); } }
        public double Temperature_Day_Three_Max { get => temperature_Day_Three_Max; set { temperature_Day_Three_Max = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Temperature_Day_Three_Max))); } }
        public double Temperature_Day_Three_Night { get => temperature_Day_Three_Night; set { temperature_Day_Three_Night = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Temperature_Day_Three_Night))); } }
        public double Temperature_Day_Three_Morning { get => temperature_Day_Three_Morning; set { temperature_Day_Three_Morning = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Temperature_Day_Three_Morning))); } }
        public double Temperature_Day_Three_Low_Feels_Like { get => temperature_Day_Three_Low_Feels_Like; set { temperature_Day_Three_Low_Feels_Like = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Temperature_Day_Three_Low_Feels_Like))); } }
        public double Temperature_Day_Three_Night_Feels_Like { get => temperature_Day_Three_Night_Feels_Like; set { temperature_Day_Three_Night_Feels_Like = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Temperature_Day_Three_Night_Feels_Like))); } }
        public double Temperature_Day_Three_Eve_Feels_Like { get => temperature_Day_Three_Eve_Feels_Like; set { temperature_Day_Three_Eve_Feels_Like = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Temperature_Day_Three_Eve_Feels_Like))); } }
        public double Temperature_Day_Three_Morn_Feels_Like { get => temperature_Day_Three_Morn_Feels_Like; set { temperature_Day_Three_Morn_Feels_Like = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Temperature_Day_Three_Morn_Feels_Like))); } }
        public int Weather_Day_Three_ID { get => weather_Day_Three_ID; set { weather_Day_Three_ID = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Weather_Day_Three_ID))); } }
        public string? Weather_Day_Three_Main { get => weather_Day_Three_Main; set { weather_Day_Three_Main = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Weather_Day_Three_Main))); } }
        public string? Weather_Day_Three_Description { get => weather_Day_Three_Description; set { weather_Day_Three_Description = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Weather_Day_Three_Description))); } }
        public string? Weather_Day_Three_Icon { get => "a" + weather_Day_Three_Icon + ".png"; set { weather_Day_Three_Icon = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Weather_Day_Three_Icon))); } }
        public double UV_Index_DayTwo { get => uV_Index_DayTwo; set { uV_Index_DayTwo = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UV_Index_DayTwo))); } }
        public double UV_Index_DayThree { get => uV_Index_DayThree; set { uV_Index_DayThree = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UV_Index_DayThree))); } }
        public int Day_Three_Pressure { get => day_Three_Pressure; set { day_Three_Pressure = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Day_Three_Pressure))); } }
        public int Day_Three_Humidity { get => day_Three_Humidity; set { day_Three_Humidity = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Day_Three_Humidity))); } }
        public double Day_Three_Dew_Point { get => day_Three_Dew_Point; set { day_Three_Dew_Point = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Day_Three_Dew_Point))); } }
        public int Day_Two_Pressure { get => day_Two_Pressure; set { day_Two_Pressure = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Day_Two_Pressure))); } }
        public int Day_Two_Humidity { get => day_Two_Humidity; set { day_Two_Humidity = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Day_Two_Humidity))); } }
        public double Day_Two_Dew_Point { get => day_Two_Dew_Point; set { day_Two_Dew_Point = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Day_Two_Dew_Point))); } }
        public DateTime MoonRise_Day_Four { get => moonRise_Day_Four; set { moonRise_Day_Four = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MoonRise_Day_Four))); } }
        public DateTime MoonSet_Day_Four { get => moonSet_Day_Four; set { moonSet_Day_Four = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MoonSet_Day_Four))); } }
        public string? Summary_Day_Four { get => summary_Day_Four; set { summary_Day_Four = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Summary_Day_Four))); } }
        public double Temperature_Day_Four_Min { get => temperature_Day_Four_Min; set { temperature_Day_Four_Min = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Temperature_Day_Four_Min))); } }
        public double Temperature_Day_Four_Max { get => temperature_Day_Four_Max; set { temperature_Day_Four_Max = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Temperature_Day_Four_Max))); } }
        public double Temperature_Day_Four_Night { get => temperature_Day_Four_Night; set { temperature_Day_Four_Night = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Temperature_Day_Four_Night))); } }
        public double Temperature_Day_Four_Morning { get => temperature_Day_Four_Morning; set { temperature_Day_Four_Morning = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Temperature_Day_Four_Morning))); } }
        public double Temperature_Day_Four_Low_Feels_Like { get => temperature_Day_Four_Low_Feels_Like; set { temperature_Day_Four_Low_Feels_Like = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Temperature_Day_Four_Low_Feels_Like))); } }
        public double Temperature_Day_Four_Night_Feels_Like { get => temperature_Day_Four_Night_Feels_Like; set { temperature_Day_Four_Night_Feels_Like = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Temperature_Day_Four_Night_Feels_Like))); } }
        public double Temperature_Day_Four_Eve_Feels_Like { get => temperature_Day_Four_Eve_Feels_Like; set { temperature_Day_Four_Eve_Feels_Like = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Temperature_Day_Four_Eve_Feels_Like))); } }
        public double Temperature_Day_Four_Morn_Feels_Like { get => temperature_Day_Four_Morn_Feels_Like; set { temperature_Day_Four_Morn_Feels_Like = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Temperature_Day_Four_Morn_Feels_Like))); } }
        public int Weather_Day_Four_ID { get => weather_Day_Four_ID; set { weather_Day_Four_ID = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Weather_Day_Four_ID))); } }
        public string? Weather_Day_Four_Main { get => weather_Day_Four_Main; set { weather_Day_Four_Main = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Weather_Day_Four_Main))); } }
        public string? Weather_Day_Four_Description { get => weather_Day_Four_Description; set { weather_Day_Four_Description = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Weather_Day_Four_Description))); } }
        public string? Weather_Day_Four_Icon { get => "a" + weather_Day_Four_Icon + ".png"; set { weather_Day_Four_Icon = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Weather_Day_Four_Icon))); } }
        public int Day_Four_Pressure { get => day_Four_Pressure; set { day_Four_Pressure = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Day_Four_Pressure))); } }
        public double Day_Four_Dew_Point { get => day_Four_Dew_Point; set { day_Four_Dew_Point = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Day_Four_Dew_Point))); } }
        public int Day_Four_Humidity { get => day_Four_Humidity; set { day_Four_Humidity = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Day_Four_Humidity))); } }
        public double UV_Index_DayFour { get => uV_Index_DayFour; set { uV_Index_DayFour = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UV_Index_DayFour))); } }
        public DateTime MoonRise_Day_Five { get => moonRise_Day_Five; set { moonRise_Day_Five = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MoonRise_Day_Five))); } }
        public DateTime MoonSet_Day_Five { get => moonSet_Day_Five; set { moonSet_Day_Five = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MoonSet_Day_Five))); } }
        public string? Summary_Day_Five { get => summary_Day_Five; set { summary_Day_Five = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Summary_Day_Five))); } }

        public double Temperature_Day_Five_Min { get => temperature_Day_Five_Min; set { temperature_Day_Five_Min = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Temperature_Day_Five_Min))); } }
        public double Temperature_Day_Five_Max { get => temperature_Day_Five_Max; set { temperature_Day_Five_Max = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Temperature_Day_Five_Max))); } }
        public double Temperature_Day_Five_Night { get => temperature_Day_Five_Night; set { temperature_Day_Five_Night = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Temperature_Day_Five_Night))); } }
        public double Temperature_Day_Five_Morning { get => temperature_Day_Five_Morning; set { temperature_Day_Five_Morning = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Temperature_Day_Five_Morning))); } }
        public double Temperature_Day_Five_Low_Feels_Like { get => temperature_Day_Five_Low_Feels_Like; set { temperature_Day_Five_Low_Feels_Like = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Temperature_Day_Five_Low_Feels_Like))); } }
        public double Temperature_Day_Five_Night_Feels_Like { get => temperature_Day_Five_Night_Feels_Like; set { temperature_Day_Five_Night_Feels_Like = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Temperature_Day_Five_Night_Feels_Like))); } }
        public double Temperature_Day_Five_Eve_Feels_Like { get => temperature_Day_Five_Eve_Feels_Like; set { temperature_Day_Five_Eve_Feels_Like = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Temperature_Day_Five_Eve_Feels_Like))); } }
        public double Temperature_Day_Five_Morn_Feels_Like { get => temperature_Day_Five_Morn_Feels_Like; set { temperature_Day_Five_Morn_Feels_Like = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Temperature_Day_Five_Morn_Feels_Like))); } }
        public int Weather_Day_Five_ID { get => weather_Day_Five_ID; set { weather_Day_Five_ID = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Weather_Day_Five_ID))); } }
        public string? Weather_Day_Five_Main { get => weather_Day_Five_Main; set { weather_Day_Five_Main = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Weather_Day_Five_Main))); } }
        public string? Weather_Day_Five_Description { get => weather_Day_Five_Description; set { weather_Day_Five_Description = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Weather_Day_Five_Description))); } }
        public string? Weather_Day_Five_Icon { get => "a" + weather_Day_Five_Icon + ".png"; set { weather_Day_Five_Icon = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Weather_Day_Five_Icon))); } }
        public int Day_Five_Pressure { get => day_Five_Pressure; set { day_Five_Pressure = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Day_Five_Pressure))); } }
        public int Day_Five_Humidity { get => day_Five_Humidity; set { day_Five_Humidity = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Day_Five_Humidity))); } }
        public double Day_Five_Dew_Point { get => day_Five_Dew_Point; set { day_Five_Dew_Point = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Day_Five_Dew_Point))); } }
        public double UV_Index_DayFive { get => uV_Index_DayFive; set { uV_Index_DayFive = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UV_Index_DayFive))); } }
        public DateTime Sunrise_Day_Six { get => _sunrise_day_six; set { _sunrise_day_six = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Sunrise_Day_Six))); } }
        public double UV_Index_DaySeven { get => uV_Index_DaySeven; set { uV_Index_DaySeven = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UV_Index_DaySeven))); } }
        public double Temperature_Day_Seven_Low { get => temperature_Day_Seven_Low; set { temperature_Day_Seven_Low = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Temperature_Day_Seven_Low))); } }
        public double WindGustDay_Seven { get => windGustDay_Seven; set { windGustDay_Seven = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(WindGustDay_Seven))); } }
        public int WindDirectionDay_Seven { get => windDirectionDay_Seven; set { windDirectionDay_Seven = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(WindDirectionDay_Seven))); } }
        public double WindSpeedDay_Seven { get => windSpeedDay_Seven; set { windSpeedDay_Seven = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(WindSpeedDay_Seven))); } }
        public double Day_Seven_Dew_Point { get => day_Seven_Dew_Point; set { day_Seven_Dew_Point = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Day_Seven_Dew_Point))); } }
        public int Day_Seven_Humidity { get => day_Seven_Humidity; set { day_Seven_Humidity = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Day_Seven_Humidity))); } }
        public int Day_Seven_Pressure { get => day_Seven_Pressure; set { day_Seven_Pressure = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Day_Seven_Pressure))); } }
        public string? Weather_Day_Seven_Icon { get => "a" + weather_Day_Seven_Icon + ".png"; set { weather_Day_Seven_Icon = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Weather_Day_Seven_Icon))); } }
        public string? Weather_Day_Seven_Description { get => weather_Day_Seven_Description; set { weather_Day_Seven_Description = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Weather_Day_Seven_Description))); } }
        public string? Weather_Day_Seven_Main { get => weather_Day_Seven_Main; set { weather_Day_Seven_Main = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Weather_Day_Seven_Main))); } }
        public int Weather_Day_Seven_ID { get => weather_Day_Seven_ID; set { weather_Day_Seven_ID = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Weather_Day_Seven_ID))); } }
        public double Temperature_Day_Seven_Morn_Feels_Like { get => temperature_Day_Seven_Morn_Feels_Like; set { temperature_Day_Seven_Morn_Feels_Like = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Temperature_Day_Seven_Morn_Feels_Like))); } }
        public double Temperature_Day_Seven_Eve_Feels_Like { get => temperature_Day_Seven_Eve_Feels_Like; set { temperature_Day_Seven_Eve_Feels_Like = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Temperature_Day_Seven_Eve_Feels_Like))); } }
        public double Temperature_Day_Seven_Night_Feels_Like { get => temperature_Day_Seven_Night_Feels_Like; set { temperature_Day_Seven_Night_Feels_Like = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Temperature_Day_Seven_Night_Feels_Like))); } }
        public double Temperature_Day_Seven_Low_Feels_Like { get => temperature_Day_Seven_Low_Feels_Like; set { temperature_Day_Seven_Low_Feels_Like = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Temperature_Day_Seven_Low_Feels_Like))); } }
        public double Temperature_Day_Seven_Morning { get => temperature_Day_Seven_Morning; set { temperature_Day_Seven_Morning = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Temperature_Day_Seven_Morning))); } }
        public double Temperature_Day_Seven_Night { get => temperature_Day_Seven_Night; set { temperature_Day_Seven_Night = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Temperature_Day_Seven_Night))); } }
        public double Temperature_Day_Seven_Max { get => temperature_Day_Seven_Max; set { temperature_Day_Seven_Max = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Temperature_Day_Seven_Max))); } }
        public double Temperature_Day_Seven_Min { get => temperature_Day_Seven_Min; set { temperature_Day_Seven_Min = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Temperature_Day_Seven_Min))); } }
        public double Temperature_Day_Seven { get => temperature_Day_Seven; set { temperature_Day_Seven = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Temperature_Day_Seven))); } }
        public string? Summary_Day_Seven { get => summary_Day_Seven; set { summary_Day_Seven = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Summary_Day_Seven))); } }
        public DateTime MoonSet_Day_Seven { get => moonSet_Day_Seven; set { moonSet_Day_Seven = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MoonSet_Day_Seven))); } }
        public DateTime MoonRise_Day_Seven { get => moonRise_Day_Seven; set { moonRise_Day_Seven = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MoonRise_Day_Seven))); } }
        public DateTime Sunset_Day_Seven { get => _sunset_day_seven; set { _sunset_day_seven = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Sunset_Day_Seven))); } }
        public DateTime Sunrise_Day_Seven { get => _sunrise_day_seven; set { _sunrise_day_seven = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Sunrise_Day_Seven))); } }
        public double UV_Index_DaySix { get => uV_Index_DaySix; set { uV_Index_DaySix = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(UV_Index_DaySix))); } }
        public double Temperature_Day_Six_Low { get => temperature_Day_Six_Low; set { temperature_Day_Six_Low = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Temperature_Day_Six_Low))); } }
        public double WindGustDay_Six { get => windGustDay_Six; set { windGustDay_Six = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(WindGustDay_Six))); } }
        public int WindDirectionDay_Six { get => windDirectionDay_Six; set { windDirectionDay_Six = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(WindDirectionDay_Six))); } }
        public double WindSpeedDay_Six { get => windSpeedDay_Six; set { windSpeedDay_Six = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(WindSpeedDay_Six))); } }
        public double Day_Six_Dew_Point { get => day_Six_Dew_Point; set { day_Six_Dew_Point = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Day_Six_Dew_Point))); } }
        public int Day_Six_Humidity { get => day_Six_Humidity; set { day_Six_Humidity = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Day_Six_Humidity))); } }
        public int Day_Six_Pressure { get => day_Six_Pressure; set { day_Six_Pressure = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Day_Six_Pressure))); } }
        public string? Weather_Day_Six_Icon { get => "a" + weather_Day_Six_Icon + ".png"; set { weather_Day_Six_Icon = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Weather_Day_Six_Icon))); } }
        public string? Weather_Day_Six_Description { get => weather_Day_Six_Description; set { weather_Day_Six_Description = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Weather_Day_Six_Description))); } }
        public string? Weather_Day_Six_Main { get => weather_Day_Six_Main; set { weather_Day_Six_Main = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Weather_Day_Six_Main))); } }
        public int Weather_Day_Six_ID { get => weather_Day_Six_ID; set { weather_Day_Six_ID = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Weather_Day_Six_ID))); } }
        public double Temperature_Day_Six_Morn_Feels_Like { get => temperature_Day_Six_Morn_Feels_Like; set { temperature_Day_Six_Morn_Feels_Like = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Temperature_Day_Six_Morn_Feels_Like))); } }
        public double Temperature_Day_Six_Eve_Feels_Like { get => temperature_Day_Six_Eve_Feels_Like; set { temperature_Day_Six_Eve_Feels_Like = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Temperature_Day_Six_Eve_Feels_Like))); } }
        public double Temperature_Day_Six_Night_Feels_Like { get => temperature_Day_Six_Night_Feels_Like; set { temperature_Day_Six_Night_Feels_Like = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Temperature_Day_Six_Night_Feels_Like))); } }
        public double Temperature_Day_Six_Low_Feels_Like { get => temperature_Day_Six_Low_Feels_Like; set { temperature_Day_Six_Low_Feels_Like = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Temperature_Day_Six_Low_Feels_Like))); } }
        public double Temperature_Day_Six_Morning { get => temperature_Day_Six_Morning; set { temperature_Day_Six_Morning = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Temperature_Day_Six_Morning))); } }
        public double Temperature_Day_Six_Night { get => temperature_Day_Six_Night; set { temperature_Day_Six_Night = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Temperature_Day_Six_Night))); } }
        public double Temperature_Day_Six_Max { get => temperature_Day_Six_Max; set { temperature_Day_Six_Max = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Temperature_Day_Six_Max))); } }
        public double Temperature_Day_Six_Min { get => temperature_Day_Six_Min; set { temperature_Day_Six_Min = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Temperature_Day_Six_Min))); } }
        public double Temperature_Day_Six { get => temperature_Day_Six; set { temperature_Day_Six = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Temperature_Day_Six))); } }
        public string? Summary_Day_Six { get => summary_Day_Six; set { summary_Day_Six = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Summary_Day_Six))); } }
        public DateTime MoonRise_Day_Six { get => moonRise_Day_Six; set { moonRise_Day_Six = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MoonRise_Day_Six))); } }
        public DateTime Sunset_Day_Six { get => _sunset_day_six; set { _sunset_day_six = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Sunset_Day_Six))); } }
        public DateTime MoonSet_Day_Six { get => moonSet_Day_Six; set { moonSet_Day_Six = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(MoonSet_Day_Six))); } }

        private string? forecasts_Day_Seven;
        private string? forecasts_Day_Six;

        public string? Forecasts_Day_Six
        {
            get => forecasts_Day_Six; set { forecasts_Day_Six = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Forecasts_Day_Six))); }
        }

        public string? Forecasts_Day_Seven
        {
            get => forecasts_Day_Seven; set { forecasts_Day_Seven = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Forecasts_Day_Seven))); }
        }

    }
}
