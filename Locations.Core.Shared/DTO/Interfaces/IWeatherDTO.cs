using System;

namespace Locations.Core.Shared.DTO.Interfaces
{
    public interface IWeatherDTO : IDTOBase
    {
        // Basic properties
        int Id { get; set; }
        double Latitude { get; set; }
        double Longitude { get; set; }
        string Timezone { get; set; }
        int TimezoneOffset { get; set; }
        DateTime LastUpdate { get; set; }

        // Format settings
        string TimeFormat { get; set; }
        string DateFormat { get; set; }
        string WindDirectionArrow { get; set; }

        // Day One properties
        DateTime Sunrise_day_one { get; set; }
        DateTime Sunset_day_one { get; set; }
        double Temperature_day_one { get; set; }
        double Temperature_day_one_low { get; set; }
        string Forecast_day_one { get; set; }
        double WindSpeedDay_One { get; set; }
        double WindDirectionDay_One { get; set; }
        double WindGustDay_one { get; set; }
        int Clouds_day_one { get; set; }
        int Rain_Day_One { get; set; }
        DateTime MoonRise_Day_One { get; set; }
        DateTime MoonSet_Day_One { get; set; }
        double MoonPhaseAs_Number_DayOne { get; set; }
        string Summary_Day_One { get; set; }
        double Temperature_Day_One_Min { get; set; }
        double Temperature_Day_One_Max { get; set; }
        double Temperature_Day_One_Night { get; set; }
        double Temperature_Day_One_Morning { get; set; }
        double UV_Index_DayOne { get; set; }
        double Day_One_Dew_Point { get; set; }
        int Day_One_Humidity { get; set; }
        int Day_One_Pressure { get; set; }
        string Weather_Day_One_Icon { get; set; }
        string Weather_Day_One_Description { get; set; }
        double Temperature_Day_One_Low_Feels_Like { get; set; }
        double Temperature_Day_One_Night_Feels_Like { get; set; }
        double Temperature_Day_One_Eve_Feels_Like { get; set; }
        int Weather_Day_One_ID { get; set; }
        string Weather_Day_One_Main { get; set; }
        double Temperature_Day_One_Morn_Feels_Like { get; set; }

        // Day Two properties
        DateTime Sunrise_day_two { get; set; }
        DateTime Sunset_day_two { get; set; }
        double Temperature_day_two { get; set; }
        double Temperature_day_two_low { get; set; }
        string Forecast_day_two { get; set; }
        double WindSpeedDay_Two { get; set; }
        double WindDirectionDay_Two { get; set; }
        double WindGustDay_Two { get; set; }
        int Clouds_day_two { get; set; }
        int Rain_Day_Two { get; set; }
        DateTime MoonRise_Day_Two { get; set; }
        DateTime MoonSet_Day_Two { get; set; }
        double MoonPhaseAs_Number_DayTwo { get; set; }
        string Summary_Day_Two { get; set; }
        double Temperature_Day_Two_Min { get; set; }
        double Temperature_Day_Two_Max { get; set; }
        double Temperature_Day_Two_Night { get; set; }
        double Temperature_Day_Two_Morning { get; set; }
        double UV_Index_DayTwo { get; set; }
        double Day_Two_Dew_Point { get; set; }
        int Day_Two_Humidity { get; set; }
        int Day_Two_Pressure { get; set; }
        string Weather_Day_Two_Icon { get; set; }
        string Weather_Day_Two_Description { get; set; }
        double Temperature_Day_Two_Low_Feels_Like { get; set; }
        double Temperature_Day_Two_Night_Feels_Like { get; set; }
        double Temperature_Day_Two_Eve_Feels_Like { get; set; }
        int Weather_Day_Two_ID { get; set; }
        string Weather_Day_Two_Main { get; set; }
        double Temperature_Day_Two_Morn_Feels_Like { get; set; }

        // Day Three properties
        DateTime Sunrise_day_three { get; set; }
        DateTime Sunset_day_three { get; set; }
        double Temperature_day_three { get; set; }
        double Temperature_day_three_low { get; set; }
        string Forecast_day_three { get; set; }
        double WindSpeedDay_Three { get; set; }
        double WindDirectionDay_Three { get; set; }
        double WindGustDay_Three { get; set; }
        int Clouds_day_Three { get; set; }
        int Rain_Day_Three { get; set; }
        DateTime MoonRise_Day_Three { get; set; }
        DateTime MoonSet_Day_Three { get; set; }
        double MoonPhaseAs_Number_DayThree { get; set; }
        string Summary_Day_Three { get; set; }
        double Temperature_Day_Three_Min { get; set; }
        double Temperature_Day_Three_Max { get; set; }
        double Temperature_Day_Three_Night { get; set; }
        double Temperature_Day_Three_Morning { get; set; }
        double UV_Index_DayThree { get; set; }
        double Day_Three_Dew_Point { get; set; }
        int Day_Three_Humidity { get; set; }
        int Day_Three_Pressure { get; set; }
        string Weather_Day_Three_Icon { get; set; }
        string Weather_Day_Three_Description { get; set; }
        double Temperature_Day_Three_Low_Feels_Like { get; set; }
        double Temperature_Day_Three_Night_Feels_Like { get; set; }
        double Temperature_Day_Three_Eve_Feels_Like { get; set; }
        int Weather_Day_Three_ID { get; set; }
        string Weather_Day_Three_Main { get; set; }
        double Temperature_Day_Three_Morn_Feels_Like { get; set; }

        // Day Four properties
        DateTime Sunrise_day_four { get; set; }
        DateTime Sunset_day_four { get; set; }
        double Temperature_day_four { get; set; }
        double Temperature_day_four_low { get; set; }
        string Forecast_day_four { get; set; }
        double WindSpeedDay_Four { get; set; }
        double WindDirectionDay_Four { get; set; }
        double WindGustDay_Four { get; set; }
        int Clouds_day_Four { get; set; }
        int Rain_Day_Four { get; set; }
        DateTime MoonRise_Day_Four { get; set; }
        DateTime MoonSet_Day_Four { get; set; }
        double MoonPhaseAs_Number_DayFour { get; set; }
        string Summary_Day_Four { get; set; }
        double Temperature_Day_Four_Min { get; set; }
        double Temperature_Day_Four_Max { get; set; }
        double Temperature_Day_Four_Night { get; set; }
        double Temperature_Day_Four_Morning { get; set; }
        double UV_Index_DayFour { get; set; }
        double Day_Four_Dew_Point { get; set; }
        int Day_Four_Humidity { get; set; }
        int Day_Four_Pressure { get; set; }
        string Weather_Day_Four_Icon { get; set; }
        string Weather_Day_Four_Description { get; set; }
        double Temperature_Day_Four_Low_Feels_Like { get; set; }
        double Temperature_Day_Four_Night_Feels_Like { get; set; }
        double Temperature_Day_Four_Eve_Feels_Like { get; set; }
        int Weather_Day_Four_ID { get; set; }
        string Weather_Day_Four_Main { get; set; }
        double Temperature_Day_Four_Morn_Feels_Like { get; set; }

        // Day Five properties
        DateTime Sunrise_day_five { get; set; }
        DateTime Sunset_day_five { get; set; }
        double Temperature_day_five { get; set; }
        double Temperature_day_five_low { get; set; }
        string Forecast_day_five { get; set; }
        double WindSpeedDay_Five { get; set; }
        double WindDirectionDay_Five { get; set; }
        double WindGustDay_Five { get; set; }
        int Clouds_day_Five { get; set; }
        int Rain_Day_Five { get; set; }
        DateTime MoonRise_Day_Five { get; set; }
        DateTime MoonSet_Day_Five { get; set; }
        double MoonPhaseAs_Number_DayFive { get; set; }
        string Summary_Day_Five { get; set; }
        double Temperature_Day_Five_Min { get; set; }
        double Temperature_Day_Five_Max { get; set; }
        double Temperature_Day_Five_Night { get; set; }
        double Temperature_Day_Five_Morning { get; set; }
        double UV_Index_DayFive { get; set; }
        double Day_Five_Dew_Point { get; set; }
        int Day_Five_Humidity { get; set; }
        int Day_Five_Pressure { get; set; }
        string Weather_Day_Five_Icon { get; set; }
        string Weather_Day_Five_Description { get; set; }
        double Temperature_Day_Five_Low_Feels_Like { get; set; }
        double Temperature_Day_Five_Night_Feels_Like { get; set; }
        double Temperature_Day_Five_Eve_Feels_Like { get; set; }
        int Weather_Day_Five_ID { get; set; }
        string Weather_Day_Five_Main { get; set; }
        double Temperature_Day_Five_Morn_Feels_Like { get; set; }

        // Day Six properties
        DateTime Sunrise_day_six { get; set; }
        DateTime Sunset_day_six { get; set; }
        double Temperature_Day_Six { get; set; }
        double Temperature_Day_Six_Low { get; set; }
        string Forecasts_Day_Six { get; set; }
        double WindSpeedDay_Six { get; set; }
        int WindDirectionDay_Six { get; set; }
        double WindGustDay_Six { get; set; }
        int Clouds_day_Six { get; set; }
        int Rain_Day_Six { get; set; }
        DateTime MoonRise_Day_Six { get; set; }
        DateTime MoonSet_Day_Six { get; set; }
        double MoonPhaseAs_Number_DaySix { get; set; }
        string Summary_Day_Six { get; set; }
        double Temperature_Day_Six_Min { get; set; }
        double Temperature_Day_Six_Max { get; set; }
        double Temperature_Day_Six_Night { get; set; }
        double Temperature_Day_Six_Morning { get; set; }
        double UV_Index_DaySix { get; set; }
        double Day_Six_Dew_Point { get; set; }
        int Day_Six_Humidity { get; set; }
        int Day_Six_Pressure { get; set; }
        string Weather_Day_Six_Icon { get; set; }
        string Weather_Day_Six_Description { get; set; }
        double Temperature_Day_Six_Low_Feels_Like { get; set; }
        double Temperature_Day_Six_Night_Feels_Like { get; set; }
        double Temperature_Day_Six_Eve_Feels_Like { get; set; }
        int Weather_Day_Six_ID { get; set; }
        string Weather_Day_Six_Main { get; set; }
        double Temperature_Day_Six_Morn_Feels_Like { get; set; }

        // Day Seven properties
        DateTime Sunrise_day_seven { get; set; }
        DateTime Sunset_day_seven { get; set; }
        double Temperature_Day_Seven { get; set; }
        double Temperature_Day_Seven_Low { get; set; }
        string Forecasts_Day_Seven { get; set; }
        double WindSpeedDay_Seven { get; set; }
        int WindDirectionDay_Seven { get; set; }
        double WindGustDay_Seven { get; set; }
        int Clouds_day_Seven { get; set; }
        int Rain_Day_Seven { get; set; }
        DateTime MoonRise_Day_Seven { get; set; }
        DateTime MoonSet_Day_Seven { get; set; }
        double MoonPhaseAs_Number_DaySeven { get; set; }
        string Summary_Day_Seven { get; set; }
        double Temperature_Day_Seven_Min { get; set; }
        double Temperature_Day_Seven_Max { get; set; }
        double Temperature_Day_Seven_Night { get; set; }
        double Temperature_Day_Seven_Morning { get; set; }
        double UV_Index_DaySeven { get; set; }
        double Day_Seven_Dew_Point { get; set; }
        int Day_Seven_Humidity { get; set; }
        int Day_Seven_Pressure { get; set; }
        string Weather_Day_Seven_Icon { get; set; }
        string Weather_Day_Seven_Description { get; set; }
        double Temperature_Day_Seven_Low_Feels_Like { get; set; }
        double Temperature_Day_Seven_Night_Feels_Like { get; set; }
        double Temperature_Day_Seven_Eve_Feels_Like { get; set; }
        int Weather_Day_Seven_ID { get; set; }
        string Weather_Day_Seven_Main { get; set; }
        double Temperature_Day_Seven_Morn_Feels_Like { get; set; }

        // Computed properties (read-only)
        string Sunrise_Day_One_String { get; }
        string Sunrise_Day_Two_String { get; }
        string Sunrise_Day_Three_String { get; }
        string Sunrise_Day_Four_String { get; }
        string Sunrise_Day_Five_String { get; }
        string Sunrise_Day_Six_String { get; }
        string Sunrise_Day_Seven_String { get; }
        string Sunset_Day_One_String { get; }
        string Sunset_Day_Two_String { get; }
        string Sunset_Day_Three_String { get; }
        string Sunset_Day_Four_String { get; }
        string Sunset_Day_Five_String { get; }
        string Sunset_Day_Six_String { get; }
        string Sunset_Day_Seven_String { get; }
        string DayOne { get; }
        string DayTwo { get; }
        string DayThree { get; }
        string DayFour { get; }
        string DayFive { get; }
        string Today { get; }
        string TodayPlusOne { get; }
        string TodayPlusTwo { get; }
        string TodayPlusThree { get; }
        string TodayPlusFour { get; }
        string TodayPlusFive { get; }
        string Weather_Day_One_IconUrl { get; }
        string Weather_Day_Two_IconUrl { get; }
        string Weather_Day_Three_IconUrl { get; }
        string Weather_Day_Four_IconUrl { get; }
        string Weather_Day_Five_IconUrl { get; }
        string Weather_Day_Six_IconUrl { get; }
        string Weather_Day_Seven_IconUrl { get; }
    }
}