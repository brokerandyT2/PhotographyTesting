using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Locations.Core.Shared.DTO;
using Locations.Core.Shared.ViewModels;
namespace Locations.Core.Business.Weather
{
    internal class MapperConfigs
    {
        /*   #region DayOne
           internal MapperConfiguration configDayOne = new MapperConfiguration(cfg =>
           {
               cfg.CreateMap<WeatherViewModel, DailyWeatherWithDetailsDTO>()
           .ForMember(x => x.Forecast, opt => opt.MapFrom(z => z.Forecast_Day_One))
           .ForMember(x => x.High_temp, opt => opt.MapFrom(z => z.Temperature_Day_One_Max))
           .ForMember(x => x.Low_temp, opt => opt.MapFrom(z => z.Temperature_Day_One_Min))
           .ForMember(x => x.Icon, opt => opt.MapFrom(z => z.Weather_Day_One_Icon))
           .ForMember(x => x.SunRise, opt => opt.MapFrom(z => z.Sunrise_Day_One_String))
           .ForMember(x => x.SunSet, opt => opt.MapFrom(z => z.Sunset_Day_One_String));
           });
           internal MapperConfiguration detailsDayOne = new MapperConfiguration(cfg =>
           {
               cfg.CreateMap<WeatherViewModel, WeatherDetailsDTO>()
               .ForMember(x => x.WindGust, opt => opt.MapFrom(z => z.WindGustDay_One))
               .ForMember(x => x.WindDirection, opt => opt.MapFrom(z => z.WindDirectionDay_One))
               .ForMember(x => x.WindSpeed, opt => opt.MapFrom(z => z.WindSpeedDay_One))
               .ForMember(x => x.Pressure, opt => opt.MapFrom(z => z.Day_One_Pressure))
               .ForMember(x => x.Humidity, opt => opt.MapFrom(z => z.Day_One_Humidity))
               .ForMember(x => x.CloudCover, opt => opt.MapFrom(z => z.Clouds_day_one))
               .ForMember(x => x.UVIndex, opt => opt.MapFrom(z => z.UV_Index_DayOne))
               .ForMember(x => x.DewPoint, opt => opt.MapFrom(z => z.Day_One_Dew_Point))
               .ForMember(x => x.MoonPhase, opt => opt.MapFrom(z => z.MoonPhaseAs_Number_DayOne))
               .ForMember(x => x.MoonRise, opt => opt.MapFrom(z => z.MoonRise_Day_One))
               .ForMember(x => x.MoonSet, opt => opt.MapFrom(z => z.MoonSet_Day_One))
               .ForMember(x => x.GoldenHourMorning, opt => opt.MapFrom(z => z.Sunrise_Day_One.AddHours(1)))
               .ForMember(z => z.GoldenHourEvening, opt => opt.MapFrom(z => z.Sunset_Day_One.AddHours(-1)));
           });



           #endregion
           internal MapperConfiguration configDayTwo = new MapperConfiguration(cfg =>
           {
               cfg.CreateMap<WeatherViewModel, DailyWeatherWithDetailsDTO>()
              .ForMember(x => x.Forecast, opt => opt.MapFrom(z => z.Forecast_Day_Two))
              .ForMember(x => x.High_temp, opt => opt.MapFrom(z => z.Temperature_Day_Two_Max))
              .ForMember(x => x.Low_temp, opt => opt.MapFrom(z => z.Temperature_Day_Two_Min))
              .ForMember(x => x.Icon, opt => opt.MapFrom(z => z.Weather_Day_Two_Icon))
              .ForMember(x => x.SunRise, opt => opt.MapFrom(z => z.Sunrise_Day_Two_String))
              .ForMember(x => x.SunSet, opt => opt.MapFrom(z => z.Sunset_Day_Two));
           });
           internal MapperConfiguration detailsDayTwo = new MapperConfiguration(cfg =>
           {
               cfg.CreateMap<WeatherViewModel, WeatherDetailsDTO>()
               .ForMember(x => x.WindGust, opt => opt.MapFrom(z => z.WindGustDay_Two))
               .ForMember(x => x.WindDirection, opt => opt.MapFrom(z => z.WindDirectionDay_Two))
               .ForMember(x => x.WindSpeed, opt => opt.MapFrom(z => z.WindSpeedDay_Two))
               .ForMember(x => x.Pressure, opt => opt.MapFrom(z => z.Day_Two_Pressure))
               .ForMember(x => x.Humidity, opt => opt.MapFrom(z => z.Day_Two_Humidity))
               .ForMember(x => x.CloudCover, opt => opt.MapFrom(z => z.Clouds_day_two))
               .ForMember(x => x.UVIndex, opt => opt.MapFrom(z => z.UV_Index_DayTwo))
               .ForMember(x => x.DewPoint, opt => opt.MapFrom(z => z.Day_Two_Dew_Point))
               .ForMember(x => x.MoonPhase, opt => opt.MapFrom(z => z.MoonPhaseAs_Number_DayTwo))
               .ForMember(x => x.MoonRise, opt => opt.MapFrom(z => z.MoonRise_Day_Two))
               .ForMember(x => x.MoonSet, opt => opt.MapFrom(z => z.MoonSet_Day_Two))
               .ForMember(x => x.GoldenHourMorning, opt => opt.MapFrom(z => z.Sunrise_Day_Two.AddHours(1)))
               .ForMember(z => z.GoldenHourEvening, opt => opt.MapFrom(z => z.Sunset_Day_Two.AddHours(-1)));
           });
           internal MapperConfiguration configDayThree = new MapperConfiguration(cfg =>
           {
               cfg.CreateMap<WeatherViewModel, DailyWeatherWithDetailsDTO>()
                         .ForMember(x => x.Forecast, opt => opt.MapFrom(z => z.Forecasts_Day_Three))
                         .ForMember(x => x.High_temp, opt => opt.MapFrom(z => z.Temperature_Day_Three_Max))
                         .ForMember(x => x.Low_temp, opt => opt.MapFrom(z => z.Temperature_Day_Three_Min))
                         .ForMember(x => x.Icon, opt => opt.MapFrom(z => z.Weather_Day_Three_Icon))
                         .ForMember(x => x.SunRise, opt => opt.MapFrom(z => z.Sunrise_Day_Three_String))
                         .ForMember(x => x.SunSet, opt => opt.MapFrom(z => z.Sunset_Day_Three));
           });
           internal MapperConfiguration detailsDayThree = new MapperConfiguration(cfg =>
           {
               cfg.CreateMap<WeatherViewModel, WeatherDetailsDTO>()
               .ForMember(x => x.WindGust, opt => opt.MapFrom(z => z.WindGustDay_Three))
               .ForMember(x => x.WindDirection, opt => opt.MapFrom(z => z.WindDirectionDay_Three))
               .ForMember(x => x.WindSpeed, opt => opt.MapFrom(z => z.WindSpeedDay_Three))
               .ForMember(x => x.Pressure, opt => opt.MapFrom(z => z.Day_Three_Pressure))
               .ForMember(x => x.Humidity, opt => opt.MapFrom(z => z.Day_Three_Humidity))
               .ForMember(x => x.CloudCover, opt => opt.MapFrom(z => z.Clouds_day_Three))
               .ForMember(x => x.UVIndex, opt => opt.MapFrom(z => z.UV_Index_DayThree))
               .ForMember(x => x.DewPoint, opt => opt.MapFrom(z => z.Day_Three_Dew_Point))
               .ForMember(x => x.MoonPhase, opt => opt.MapFrom(z => z.MoonPhaseAs_Number_DayThree))
               .ForMember(x => x.MoonRise, opt => opt.MapFrom(z => z.MoonRise_Day_Three))
               .ForMember(x => x.MoonSet, opt => opt.MapFrom(z => z.MoonSet_Day_Three))
               .ForMember(x => x.GoldenHourMorning, opt => opt.MapFrom(z => z.Sunrise_Day_Three.AddHours(1)))
               .ForMember(z => z.GoldenHourEvening, opt => opt.MapFrom(z => z.Sunset_Day_Three.AddHours(-1)));
           });
           internal MapperConfiguration configDayFour = new MapperConfiguration(cfg =>
           {
               cfg.CreateMap<WeatherViewModel, DailyWeatherWithDetailsDTO>()
                         .ForMember(x => x.Forecast, opt => opt.MapFrom(z => z.Forecasts_Day_Four))
                         .ForMember(x => x.High_temp, opt => opt.MapFrom(z => z.Temperature_Day_Four_Max))
                         .ForMember(x => x.Low_temp, opt => opt.MapFrom(z => z.Temperature_Day_Four_Min))
                         .ForMember(x => x.Icon, opt => opt.MapFrom(z => z.Weather_Day_Four_Icon))
                         .ForMember(x => x.SunRise, opt => opt.MapFrom(z => z.Sunrise_Day_Four_String))
                         .ForMember(x => x.SunSet, opt => opt.MapFrom(z => z.Sunset_Day_Four));
           });
           internal MapperConfiguration detailsDayFour = new MapperConfiguration(cfg =>
           {
               cfg.CreateMap<WeatherViewModel, WeatherDetailsDTO>()
               .ForMember(x => x.WindGust, opt => opt.MapFrom(z => z.WindGustDay_Four))
               .ForMember(x => x.WindDirection, opt => opt.MapFrom(z => z.WindDirectionDay_Four))
               .ForMember(x => x.WindSpeed, opt => opt.MapFrom(z => z.WindSpeedDay_Four))
               .ForMember(x => x.Pressure, opt => opt.MapFrom(z => z.Day_Four_Pressure))
               .ForMember(x => x.Humidity, opt => opt.MapFrom(z => z.Day_Four_Humidity))
               .ForMember(x => x.CloudCover, opt => opt.MapFrom(z => z.Clouds_day_Four))
               .ForMember(x => x.UVIndex, opt => opt.MapFrom(z => z.UV_Index_DayFour))
               .ForMember(x => x.DewPoint, opt => opt.MapFrom(z => z.Day_Four_Dew_Point))
               .ForMember(x => x.MoonPhase, opt => opt.MapFrom(z => z.MoonPhaseAs_Number_DayFour))
               .ForMember(x => x.MoonRise, opt => opt.MapFrom(z => z.MoonRise_Day_Four))
               .ForMember(x => x.MoonSet, opt => opt.MapFrom(z => z.MoonSet_Day_Four))
               .ForMember(x => x.GoldenHourMorning, opt => opt.MapFrom(z => z.Sunrise_Day_Four.AddHours(1)))
               .ForMember(z => z.GoldenHourEvening, opt => opt.MapFrom(z => z.Sunset_Day_Four.AddHours(-1)));
           });
           internal MapperConfiguration configDayFive = new MapperConfiguration(cfg =>
           {
               cfg.CreateMap<WeatherViewModel, DailyWeatherWithDetailsDTO>()
                         .ForMember(x => x.Forecast, opt => opt.MapFrom(z => z.Forecasts_Day_Five))
                         .ForMember(x => x.High_temp, opt => opt.MapFrom(z => z.Temperature_Day_Five_Max))
                         .ForMember(x => x.Low_temp, opt => opt.MapFrom(z => z.Temperature_Day_Five_Min))
                         .ForMember(x => x.Icon, opt => opt.MapFrom(z => z.Weather_Day_Five_Icon))
                         .ForMember(x => x.SunRise, opt => opt.MapFrom(z => z.Sunrise_Day_Five_String))
                         .ForMember(x => x.SunSet, opt => opt.MapFrom(z => z.Sunset_Day_Five));
           });
           internal MapperConfiguration detailsDayFive = new MapperConfiguration(cfg =>
           {
               cfg.CreateMap<WeatherViewModel, WeatherDetailsDTO>()
               .ForMember(x => x.WindGust, opt => opt.MapFrom(z => z.WindGustDay_Five))
               .ForMember(x => x.WindDirection, opt => opt.MapFrom(z => z.WindDirectionDay_Five))
               .ForMember(x => x.WindSpeed, opt => opt.MapFrom(z => z.WindSpeedDay_Five))
               .ForMember(x => x.Pressure, opt => opt.MapFrom(z => z.Day_Five_Pressure))
               .ForMember(x => x.Humidity, opt => opt.MapFrom(z => z.Day_Five_Humidity))
               .ForMember(x => x.CloudCover, opt => opt.MapFrom(z => z.Clouds_day_Five))
               .ForMember(x => x.UVIndex, opt => opt.MapFrom(z => z.UV_Index_DayFive))
               .ForMember(x => x.DewPoint, opt => opt.MapFrom(z => z.Day_Five_Dew_Point))
               .ForMember(x => x.MoonPhase, opt => opt.MapFrom(z => z.MoonPhaseAs_Number_DayFive))
               .ForMember(x => x.MoonRise, opt => opt.MapFrom(z => z.MoonRise_Day_Five))
               .ForMember(x => x.MoonSet, opt => opt.MapFrom(z => z.MoonSet_Day_Five))
               .ForMember(x => x.GoldenHourMorning, opt => opt.MapFrom(z => z.Sunrise_Day_Five.AddHours(1)))
               .ForMember(z => z.GoldenHourEvening, opt => opt.MapFrom(z => z.Sunset_Day_Five.AddHours(-1)));
           });
           internal MapperConfiguration configDaySix = new MapperConfiguration(cfg =>
           {
               cfg.CreateMap<WeatherViewModel, DailyWeatherWithDetailsDTO>()
                         .ForMember(x => x.Forecast, opt => opt.MapFrom(z => z.Forecasts_Day_Six))
                         .ForMember(x => x.High_temp, opt => opt.MapFrom(z => z.Temperature_Day_Six_Max))
                         .ForMember(x => x.Low_temp, opt => opt.MapFrom(z => z.Temperature_Day_Six_Min))
                         .ForMember(x => x.Icon, opt => opt.MapFrom(z => z.Weather_Day_Six_Icon))
                         .ForMember(x => x.SunRise, opt => opt.MapFrom(z => z.Sunrise_Day_Six_String))
                         .ForMember(x => x.SunSet, opt => opt.MapFrom(z => z.Sunset_Day_Six));
           });
           internal MapperConfiguration detailsDaySix = new MapperConfiguration(cfg =>
           {
               cfg.CreateMap<WeatherViewModel, WeatherDetailsDTO>()
               .ForMember(x => x.WindGust, opt => opt.MapFrom(z => z.WindGustDay_Six))
               .ForMember(x => x.WindDirection, opt => opt.MapFrom(z => z.WindDirectionDay_Six))
               .ForMember(x => x.WindSpeed, opt => opt.MapFrom(z => z.WindSpeedDay_Six))
               .ForMember(x => x.Pressure, opt => opt.MapFrom(z => z.Day_Six_Pressure))
               .ForMember(x => x.Humidity, opt => opt.MapFrom(z => z.Day_Six_Humidity))
               .ForMember(x => x.CloudCover, opt => opt.MapFrom(z => z.Clouds_day_Six))
               .ForMember(x => x.UVIndex, opt => opt.MapFrom(z => z.UV_Index_DaySix))
               .ForMember(x => x.DewPoint, opt => opt.MapFrom(z => z.Day_Six_Dew_Point))
               .ForMember(x => x.MoonPhase, opt => opt.MapFrom(z => z.MoonPhaseAs_Number_DaySix))
               .ForMember(x => x.MoonRise, opt => opt.MapFrom(z => z.MoonRise_Day_Six))
               .ForMember(x => x.MoonSet, opt => opt.MapFrom(z => z.MoonSet_Day_Six))
               .ForMember(x => x.GoldenHourMorning, opt => opt.MapFrom(z => z.Sunrise_Day_Six.AddHours(1)))
               .ForMember(z => z.GoldenHourEvening, opt => opt.MapFrom(z => z.Sunset_Day_Six.AddHours(-1)));
           });
           internal MapperConfiguration configDayseven = new MapperConfiguration(cfg =>
           {
               cfg.CreateMap<WeatherViewModel, DailyWeatherWithDetailsDTO>()
                         .ForMember(x => x.Forecast, opt => opt.MapFrom(z => z.Forecasts_Day_Seven))
                         .ForMember(x => x.High_temp, opt => opt.MapFrom(z => z.Temperature_Day_Seven_Max))
                         .ForMember(x => x.Low_temp, opt => opt.MapFrom(z => z.Temperature_Day_Seven_Min))
                         .ForMember(x => x.Icon, opt => opt.MapFrom(z => z.Weather_Day_Seven_Icon))
                         .ForMember(x => x.SunRise, opt => opt.MapFrom(z => z.Sunrise_Day_Seven_String))
                         .ForMember(x => x.SunSet, opt => opt.MapFrom(z => z.Sunset_Day_Seven));
           }); 

           internal MapperConfiguration detailsDaySeven = new MapperConfiguration(cfg =>
           {
               cfg.CreateMap<WeatherViewModel, WeatherDetailsDTO>()
               .ForMember(x => x.WindGust, opt => opt.MapFrom(z => z.WindGustDay_Seven))
               .ForMember(x => x.WindDirection, opt => opt.MapFrom(z => z.WindDirectionDay_Seven))
               .ForMember(x => x.WindSpeed, opt => opt.MapFrom(z => z.WindSpeedDay_Seven))
               .ForMember(x => x.Pressure, opt => opt.MapFrom(z => z.Day_Seven_Pressure))
               .ForMember(x => x.Humidity, opt => opt.MapFrom(z => z.Day_Seven_Humidity))
               .ForMember(x => x.CloudCover, opt => opt.MapFrom(z => z.Clouds_day_Seven))
               .ForMember(x => x.UVIndex, opt => opt.MapFrom(z => z.UV_Index_DaySeven))
               .ForMember(x => x.DewPoint, opt => opt.MapFrom(z => z.Day_Seven_Dew_Point))
               .ForMember(x => x.MoonPhase, opt => opt.MapFrom(z => z.MoonPhaseAs_Number_DaySeven))
               .ForMember(x => x.MoonRise, opt => opt.MapFrom(z => z.MoonRise_Day_Seven))
               .ForMember(x => x.MoonSet, opt => opt.MapFrom(z => z.MoonSet_Day_Seven))
               .ForMember(x => x.GoldenHourMorning, opt => opt.MapFrom(z => z.Sunrise_Day_Seven.AddHours(1)))
               .ForMember(z => z.GoldenHourEvening, opt => opt.MapFrom(z => z.Sunset_Day_Seven.AddHours(-1)));
           });
       }*/
    }
}
