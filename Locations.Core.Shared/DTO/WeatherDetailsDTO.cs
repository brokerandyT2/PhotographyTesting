using Locations.Core.Shared.DTO.Interfaces;
using System;
using System.ComponentModel;
using SQLite;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Locations.Core.Shared.DTO
{
    [Table("Weather")]
    public partial class WeatherDTO : DTOBase, INotifyPropertyChanged, IWeatherDTO
    {
        // Basic properties
        [ObservableProperty]
        [property: PrimaryKey, AutoIncrement]
        private int _id;

        [ObservableProperty]
        private double _latitude;

        [ObservableProperty]
        private double _longitude;

        [ObservableProperty]
        private string _timezone;

        [ObservableProperty]
        private int _timezoneOffset;

        [ObservableProperty]
        private DateTime _lastUpdate;

        // Format settings
        [ObservableProperty]
        [property: Ignore]
        private string _timeFormat;

        [ObservableProperty]
        [property: Ignore]
        private string _dateFormat;

        [ObservableProperty]
        private string _windDirectionArrow;

        // Day One properties
        [ObservableProperty]
        private DateTime _sunrise_day_one;

        [ObservableProperty]
        private DateTime _sunset_day_one;

        [ObservableProperty]
        private double _temperature_day_one;

        [ObservableProperty]
        private double _temperature_day_one_low;

        [ObservableProperty]
        private string _forecast_day_one;

        [ObservableProperty]
        private double _windSpeedDay_One;

        [ObservableProperty]
        private double _windDirectionDay_One;

        [ObservableProperty]
        private double _windGustDay_one;

        [ObservableProperty]
        private int _clouds_day_one;

        [ObservableProperty]
        private int _rain_Day_One;

        [ObservableProperty]
        private DateTime _moonRise_Day_One;

        [ObservableProperty]
        private DateTime _moonSet_Day_One;

        [ObservableProperty]
        private double _moonPhaseAs_Number_DayOne;

        [ObservableProperty]
        private string _summary_Day_One;

        [ObservableProperty]
        private double _temperature_Day_One_Min;

        [ObservableProperty]
        private double _temperature_Day_One_Max;

        [ObservableProperty]
        private double _temperature_Day_One_Night;

        [ObservableProperty]
        private double _temperature_Day_One_Morning;

        [ObservableProperty]
        private double _uV_Index_DayOne;

        [ObservableProperty]
        private double _day_One_Dew_Point;

        [ObservableProperty]
        private int _day_One_Humidity;

        [ObservableProperty]
        private int _day_One_Pressure;

        [ObservableProperty]
        private string _weather_Day_One_Icon;

        [ObservableProperty]
        private string _weather_Day_One_Description;

        [ObservableProperty]
        private double _temperature_Day_One_Low_Feels_Like;

        [ObservableProperty]
        private double _temperature_Day_One_Night_Feels_Like;

        [ObservableProperty]
        private double _temperature_Day_One_Eve_Feels_Like;

        [ObservableProperty]
        private int _weather_Day_One_ID;

        [ObservableProperty]
        private string _weather_Day_One_Main;

        [ObservableProperty]
        private double _temperature_Day_One_Morn_Feels_Like;

        // Day Two properties
        [ObservableProperty]
        private DateTime _sunrise_day_two;

        [ObservableProperty]
        private DateTime _sunset_day_two;

        [ObservableProperty]
        private double _temperature_day_two;

        [ObservableProperty]
        private double _temperature_day_two_low;

        [ObservableProperty]
        private string _forecast_day_two;

        [ObservableProperty]
        private double _windSpeedDay_Two;

        [ObservableProperty]
        private double _windDirectionDay_Two;

        [ObservableProperty]
        private double _windGustDay_Two;

        [ObservableProperty]
        private int _clouds_day_two;

        [ObservableProperty]
        private int _rain_Day_Two;

        [ObservableProperty]
        private DateTime _moonRise_Day_Two;

        [ObservableProperty]
        private DateTime _moonSet_Day_Two;

        [ObservableProperty]
        private double _moonPhaseAs_Number_DayTwo;

        [ObservableProperty]
        private string _summary_Day_Two;

        [ObservableProperty]
        private double _temperature_Day_Two_Min;

        [ObservableProperty]
        private double _temperature_Day_Two_Max;

        [ObservableProperty]
        private double _temperature_Day_Two_Night;

        [ObservableProperty]
        private double _temperature_Day_Two_Morning;

        [ObservableProperty]
        private double _uV_Index_DayTwo;

        [ObservableProperty]
        private double _day_Two_Dew_Point;

        [ObservableProperty]
        private int _day_Two_Humidity;

        [ObservableProperty]
        private int _day_Two_Pressure;

        [ObservableProperty]
        private string _weather_Day_Two_Icon;

        [ObservableProperty]
        private string _weather_Day_Two_Description;

        [ObservableProperty]
        private double _temperature_Day_Two_Low_Feels_Like;

        [ObservableProperty]
        private double _temperature_Day_Two_Night_Feels_Like;

        [ObservableProperty]
        private double _temperature_Day_Two_Eve_Feels_Like;

        [ObservableProperty]
        private int _weather_Day_Two_ID;

        [ObservableProperty]
        private string _weather_Day_Two_Main;

        [ObservableProperty]
        private double _temperature_Day_Two_Morn_Feels_Like;

        // Day Three properties
        [ObservableProperty]
        private DateTime _sunrise_day_three;

        [ObservableProperty]
        private DateTime _sunset_day_three;

        [ObservableProperty]
        private double _temperature_day_three;

        [ObservableProperty]
        private double _temperature_day_three_low;

        [ObservableProperty]
        private string _forecast_day_three;

        [ObservableProperty]
        private double _windSpeedDay_Three;

        [ObservableProperty]
        private double _windDirectionDay_Three;

        [ObservableProperty]
        private double _windGustDay_Three;

        [ObservableProperty]
        private int _clouds_day_Three;

        [ObservableProperty]
        private int _rain_Day_Three;

        [ObservableProperty]
        private DateTime _moonRise_Day_Three;

        [ObservableProperty]
        private DateTime _moonSet_Day_Three;

        [ObservableProperty]
        private double _moonPhaseAs_Number_DayThree;

        [ObservableProperty]
        private string _summary_Day_Three;

        [ObservableProperty]
        private double _temperature_Day_Three_Min;

        [ObservableProperty]
        private double _temperature_Day_Three_Max;

        [ObservableProperty]
        private double _temperature_Day_Three_Night;

        [ObservableProperty]
        private double _temperature_Day_Three_Morning;

        [ObservableProperty]
        private double _uV_Index_DayThree;

        [ObservableProperty]
        private double _day_Three_Dew_Point;

        [ObservableProperty]
        private int _day_Three_Humidity;

        [ObservableProperty]
        private int _day_Three_Pressure;

        [ObservableProperty]
        private string _weather_Day_Three_Icon;

        [ObservableProperty]
        private string _weather_Day_Three_Description;

        [ObservableProperty]
        private double _temperature_Day_Three_Low_Feels_Like;

        [ObservableProperty]
        private double _temperature_Day_Three_Night_Feels_Like;

        [ObservableProperty]
        private double _temperature_Day_Three_Eve_Feels_Like;

        [ObservableProperty]
        private int _weather_Day_Three_ID;

        [ObservableProperty]
        private string _weather_Day_Three_Main;

        [ObservableProperty]
        private double _temperature_Day_Three_Morn_Feels_Like;

        // Day Four properties
        [ObservableProperty]
        private DateTime _sunrise_day_four;

        [ObservableProperty]
        private DateTime _sunset_day_four;

        [ObservableProperty]
        private double _temperature_day_four;

        [ObservableProperty]
        private double _temperature_day_four_low;

        [ObservableProperty]
        private string _forecast_day_four;

        [ObservableProperty]
        private double _windSpeedDay_Four;

        [ObservableProperty]
        private double _windDirectionDay_Four;

        [ObservableProperty]
        private double _windGustDay_Four;

        [ObservableProperty]
        private int _clouds_day_Four;

        [ObservableProperty]
        private int _rain_Day_Four;

        [ObservableProperty]
        private DateTime _moonRise_Day_Four;

        [ObservableProperty]
        private DateTime _moonSet_Day_Four;

        [ObservableProperty]
        private double _moonPhaseAs_Number_DayFour;

        [ObservableProperty]
        private string _summary_Day_Four;

        [ObservableProperty]
        private double _temperature_Day_Four_Min;

        [ObservableProperty]
        private double _temperature_Day_Four_Max;

        [ObservableProperty]
        private double _temperature_Day_Four_Night;

        [ObservableProperty]
        private double _temperature_Day_Four_Morning;

        [ObservableProperty]
        private double _uV_Index_DayFour;

        [ObservableProperty]
        private double _day_Four_Dew_Point;

        [ObservableProperty]
        private int _day_Four_Humidity;

        [ObservableProperty]
        private int _day_Four_Pressure;

        [ObservableProperty]
        private string _weather_Day_Four_Icon;

        [ObservableProperty]
        private string _weather_Day_Four_Description;

        [ObservableProperty]
        private double _temperature_Day_Four_Low_Feels_Like;

        [ObservableProperty]
        private double _temperature_Day_Four_Night_Feels_Like;

        [ObservableProperty]
        private double _temperature_Day_Four_Eve_Feels_Like;

        [ObservableProperty]
        private int _weather_Day_Four_ID;

        [ObservableProperty]
        private string _weather_Day_Four_Main;

        [ObservableProperty]
        private double _temperature_Day_Four_Morn_Feels_Like;

        // Day Five properties
        [ObservableProperty]
        private DateTime _sunrise_day_five;

        [ObservableProperty]
        private DateTime _sunset_day_five;

        [ObservableProperty]
        private double _temperature_day_five;

        [ObservableProperty]
        private double _temperature_day_five_low;

        [ObservableProperty]
        private string _forecast_day_five;

        [ObservableProperty]
        private double _windSpeedDay_Five;

        [ObservableProperty]
        private double _windDirectionDay_Five;

        [ObservableProperty]
        private double _windGustDay_Five;

        [ObservableProperty]
        private int _clouds_day_Five;

        [ObservableProperty]
        private int _rain_Day_Five;

        [ObservableProperty]
        private DateTime _moonRise_Day_Five;

        [ObservableProperty]
        private DateTime _moonSet_Day_Five;

        [ObservableProperty]
        private double _moonPhaseAs_Number_DayFive;

        [ObservableProperty]
        private string _summary_Day_Five;

        [ObservableProperty]
        private double _temperature_Day_Five_Min;

        [ObservableProperty]
        private double _temperature_Day_Five_Max;

        [ObservableProperty]
        private double _temperature_Day_Five_Night;

        [ObservableProperty]
        private double _temperature_Day_Five_Morning;

        [ObservableProperty]
        private double _uV_Index_DayFive;

        [ObservableProperty]
        private double _day_Five_Dew_Point;

        [ObservableProperty]
        private int _day_Five_Humidity;

        [ObservableProperty]
        private int _day_Five_Pressure;

        [ObservableProperty]
        private string _weather_Day_Five_Icon;

        [ObservableProperty]
        private string _weather_Day_Five_Description;

        [ObservableProperty]
        private double _temperature_Day_Five_Low_Feels_Like;

        [ObservableProperty]
        private double _temperature_Day_Five_Night_Feels_Like;

        [ObservableProperty]
        private double _temperature_Day_Five_Eve_Feels_Like;

        [ObservableProperty]
        private int _weather_Day_Five_ID;

        [ObservableProperty]
        private string _weather_Day_Five_Main;

        [ObservableProperty]
        private double _temperature_Day_Five_Morn_Feels_Like;

        // Day Six properties
        [ObservableProperty]
        private DateTime _sunrise_day_six;

        [ObservableProperty]
        private DateTime _sunset_day_six;

        [ObservableProperty]
        private double _temperature_Day_Six;

        [ObservableProperty]
        private double _temperature_Day_Six_Low;

        [ObservableProperty]
        private string _forecasts_Day_Six;

        [ObservableProperty]
        private double _windSpeedDay_Six;

        [ObservableProperty]
        private int _windDirectionDay_Six;

        [ObservableProperty]
        private double _windGustDay_Six;

        [ObservableProperty]
        private int _clouds_day_Six;

        [ObservableProperty]
        private int _rain_Day_Six;

        [ObservableProperty]
        private DateTime _moonRise_Day_Six;

        [ObservableProperty]
        private DateTime _moonSet_Day_Six;

        [ObservableProperty]
        private double _moonPhaseAs_Number_DaySix;

        [ObservableProperty]
        private string _summary_Day_Six;

        [ObservableProperty]
        private double _temperature_Day_Six_Min;

        [ObservableProperty]
        private double _temperature_Day_Six_Max;

        [ObservableProperty]
        private double _temperature_Day_Six_Night;

        [ObservableProperty]
        private double _temperature_Day_Six_Morning;

        [ObservableProperty]
        private double _uV_Index_DaySix;

        [ObservableProperty]
        private double _day_Six_Dew_Point;

        [ObservableProperty]
        private int _day_Six_Humidity;

        [ObservableProperty]
        private int _day_Six_Pressure;

        [ObservableProperty]
        private string _weather_Day_Six_Icon;

        [ObservableProperty]
        private string _weather_Day_Six_Description;

        [ObservableProperty]
        private double _temperature_Day_Six_Low_Feels_Like;

        [ObservableProperty]
        private double _temperature_Day_Six_Night_Feels_Like;

        [ObservableProperty]
        private double _temperature_Day_Six_Eve_Feels_Like;

        [ObservableProperty]
        private int _weather_Day_Six_ID;

        [ObservableProperty]
        private string _weather_Day_Six_Main;

        [ObservableProperty]
        private double _temperature_Day_Six_Morn_Feels_Like;

        // Day Seven properties
        [ObservableProperty]
        private DateTime _sunrise_day_seven;

        [ObservableProperty]
        private DateTime _sunset_day_seven;

        [ObservableProperty]
        private double _temperature_Day_Seven;

        [ObservableProperty]
        private double _temperature_Day_Seven_Low;

        [ObservableProperty]
        private string _forecasts_Day_Seven;

        [ObservableProperty]
        private double _windSpeedDay_Seven;

        [ObservableProperty]
        private int _windDirectionDay_Seven;

        [ObservableProperty]
        private double _windGustDay_Seven;

        [ObservableProperty]
        private int _clouds_day_Seven;

        [ObservableProperty]
        private int _rain_Day_Seven;

        [ObservableProperty]
        private DateTime _moonRise_Day_Seven;

        [ObservableProperty]
        private DateTime _moonSet_Day_Seven;

        [ObservableProperty]
        private double _moonPhaseAs_Number_DaySeven;

        [ObservableProperty]
        private string _summary_Day_Seven;

        [ObservableProperty]
        private double _temperature_Day_Seven_Min;

        [ObservableProperty]
        private double _temperature_Day_Seven_Max;

        [ObservableProperty]
        private double _temperature_Day_Seven_Night;

        [ObservableProperty]
        private double _temperature_Day_Seven_Morning;

        [ObservableProperty]
        private double _uV_Index_DaySeven;

        [ObservableProperty]
        private double _day_Seven_Dew_Point;

        [ObservableProperty]
        private int _day_Seven_Humidity;

        [ObservableProperty]
        private int _day_Seven_Pressure;

        [ObservableProperty]
        private string _weather_Day_Seven_Icon;

        [ObservableProperty]
        private string _weather_Day_Seven_Description;

        [ObservableProperty]
        private double _temperature_Day_Seven_Low_Feels_Like;

        [ObservableProperty]
        private double _temperature_Day_Seven_Night_Feels_Like;

        [ObservableProperty]
        private double _temperature_Day_Seven_Eve_Feels_Like;

        [ObservableProperty]
        private int _weather_Day_Seven_ID;

        [ObservableProperty]
        private string _weather_Day_Seven_Main;

        [ObservableProperty]
        private double _temperature_Day_Seven_Morn_Feels_Like;
        
             
        // Computed properties with getters only
        [Ignore]
        public string Sunrise_Day_One_String => Sunrise_day_one.ToShortTimeString().ToString(TimeFormat);

        [Ignore]
        public string Sunrise_Day_Two_String => Sunrise_day_two.ToShortTimeString(TimeFormat);

        [Ignore]
        public string Sunrise_Day_Three_String => Sunrise_day_three.ToShortTimeString(TimeFormat);

        [Ignore]
        public string Sunrise_Day_Four_String => Sunrise_day_four.ToShortTimeString(TimeFormat);

        [Ignore]
        public string Sunrise_Day_Five_String => Sunrise_day_five.ToShortTimeString(TimeFormat);

        [Ignore]
        public string Sunrise_Day_Six_String => Sunrise_day_six.ToShortTimeString(TimeFormat);

        [Ignore]
        public string Sunrise_Day_Seven_String => Sunrise_day_seven.ToShortTimeString(TimeFormat);

        [Ignore]
        public string Sunset_Day_One_String => Sunset_day_one.ToShortTimeString(TimeFormat);

        [Ignore]
        public string Sunset_Day_Two_String => Sunset_day_two.ToShortTimeString(TimeFormat);

        [Ignore]
        public string Sunset_Day_Three_String => Sunset_day_three.ToShortTimeString(TimeFormat);

        [Ignore]
        public string Sunset_Day_Four_String => Sunset_day_four.ToShortTimeString(TimeFormat);

        [Ignore]
        public string Sunset_Day_Five_String => Sunset_day_five.ToShortTimeString(TimeFormat);

        [Ignore]
        public string Sunset_Day_Six_String => Sunset_day_six.ToShortTimeString(TimeFormat);

        [Ignore]
        public string Sunset_Day_Seven_String => Sunset_day_seven.ToShortTimeString(TimeFormat);

        [Ignore]
        public string DayOne => Sunrise_day_one.ToString(TimeFormat);

        [Ignore]
        public string DayTwo => Sunrise_day_two.ToString(TimeFormat);

        [Ignore]
        public string DayThree => Sunrise_day_three.ToString(TimeFormat);

        [Ignore]
        public string DayFour => Sunrise_day_four.ToString(TimeFormat);

        [Ignore]
        public string DayFive => Sunrise_day_five.ToString(TimeFormat);

        [Ignore]
        public string Today => DateTime.Today.ToShortDateString();

        [Ignore]
        public string TodayPlusOne => DateTime.Today.AddDays(1).ToString(DateFormat);

        [Ignore]
        public string TodayPlusTwo => DateTime.Today.AddDays(2).ToString(DateFormat);

        [Ignore]
        public string TodayPlusThree => DateTime.Today.AddDays(3).ToString(DateFormat);

        [Ignore]
        public string TodayPlusFour => DateTime.Today.AddDays(4).ToString(DateFormat);

        [Ignore]
        public string TodayPlusFive => DateTime.Today.AddDays(5).ToString(DateFormat);

        [Ignore]
        public string Weather_Day_One_IconUrl =>  Weather_Day_One_Icon + ".png";

        [Ignore]
        public string Weather_Day_Two_IconUrl =>  Weather_Day_Two_Icon + ".png";

        [Ignore]
        public string Weather_Day_Three_IconUrl => Weather_Day_Three_Icon + ".png";

        [Ignore]
        public string Weather_Day_Four_IconUrl => Weather_Day_Four_Icon + ".png";

        [Ignore]
        public string Weather_Day_Five_IconUrl => Weather_Day_Five_Icon + ".png";

        [Ignore]
        public string Weather_Day_Six_IconUrl => Weather_Day_Six_Icon + ".png";

        [Ignore]
        public string Weather_Day_Seven_IconUrl => Weather_Day_Seven_Icon + ".png";

        public event PropertyChangedEventHandler? PropertyChanged;

        // Partial methods for specific property changes
        partial void OnWindDirectionDay_OneChanged(double oldValue, double newValue);
        partial void OnWindDirectionDay_TwoChanged(double oldValue, double newValue);
        partial void OnWindDirectionDay_ThreeChanged(double oldValue, double newValue);
        partial void OnWindDirectionDay_FourChanged(double oldValue, double newValue);
        partial void OnWindDirectionDay_FiveChanged(double oldValue, double newValue);
    }
}