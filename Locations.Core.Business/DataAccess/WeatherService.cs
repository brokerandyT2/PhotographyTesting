using Locations.Core.Business.DataAccess.Interfaces;
using Locations.Core.Data.Queries;
using Locations.Core.Shared;
using Locations.Core.Shared.Customizations.Alerts.Implementation;
using Locations.Core.Shared.Customizations.Alerts.Interfraces;
using Locations.Core.Shared.Customizations.Logging.Implementation;
using Locations.Core.Shared.Customizations.Logging.Interfaces;
using Locations.Core.Shared.ViewModels;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using OpenWeatherAPI;


namespace Locations.Core.Business.DataAccess
{
    public class WeatherService : IWeatherService
    {
        WeatherQuery<WeatherViewModel> _weatherQuery = new WeatherQuery<WeatherViewModel>(new AlertService(), new LoggerService(new ServiceCollection().AddLogging().BuildServiceProvider().GetRequiredService<ILogger<LoggerService>>()));
        private IAlertService alertServ;
        private ILoggerService loggerService;
        SettingsService _settingsService = new SettingsService();
        private readonly IConnectivity _connectivity;

        public WeatherService() { }
        public WeatherService(IAlertService alertServ, ILoggerService loggerService, IConnectivity connectivity) : this(alertServ, loggerService)
        {
          
            _connectivity = connectivity;
        }
        public WeatherService(IAlertService alertServ, ILoggerService loggerService) : this()
        {
            this.alertServ = alertServ;
            this.loggerService = loggerService;
        }
        public WeatherViewModel GetWeather(double latitude, double longitude)
        {
            try
            {
                var update = _settingsService.GetSettingByName(MagicStrings.LastBulkWeatherUpdate);
                var lastUpdate = DateTime.Parse(update.Value);
                var x = _weatherQuery.GetWeather(latitude, longitude);
                WeatherViewModel wvm = new WeatherViewModel();
                if (x == null)
                {
                    NetworkAccess accessType = Connectivity.Current.NetworkAccess;
                    if (accessType == NetworkAccess.Internet && lastUpdate < DateTime.Now.AddDays(-1))
                    {
                        var key = _settingsService.GetSetting(MagicStrings.Weather_API_Key).Value;
                        var url = _settingsService.GetSetting(MagicStrings.WeatherURL).Value;
                        var tempSetting = _settingsService.GetSetting(MagicStrings.TemperatureType).Value;
                        //TODO: get rid of this library.
                        var client = new OpenWeatherAPI.OpenWeatherApiClient(key, url, tempSetting, true);
                        var output = client.GetData(latitude, longitude);
                        wvm = StringToWeatherViewModel(output);
                        update.Value = DateTime.Now.ToString();
                        _settingsService.SaveSettingWithObjectReturn(update);
                        Save(wvm);
                    }
                    wvm.DateFormat = _settingsService.GetSettingByName(MagicStrings.DateFormat).Value;
                    wvm.TimeFormat = _settingsService.GetSettingByName(MagicStrings.TimeFormat).Value;
                    return wvm;
                }
                else
                {
                    return x;
                }
            }
            catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
                return new WeatherViewModel();
            }


        }

        private WeatherViewModel StringToWeatherViewModel(string output)
        {
            WeatherViewModel wvm = new WeatherViewModel();
            var jsonData = JObject.Parse(output);
            if (jsonData != null && output != string.Empty)
            {


                wvm.Latitude = jsonData.SelectToken("lat").ToObject<double>();
                wvm.Longitude = jsonData.SelectToken("lon").ToObject<double>();
                wvm.LastUpdate = DateTime.Now;
                wvm.TimezoneOffset = jsonData.SelectToken("timezone_offset").ToObject<int>();
                var currentWeather = jsonData.SelectToken("current");

                var dailyweather = jsonData.SelectToken("daily");
                int i = 0;
                //epoch
                DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);

                foreach (JToken day in dailyweather)
                {
                    if (i == 0)
                    {
                        wvm.Forecast_Day_One = day.SelectToken("summary").ToObject<string>();
                        wvm.Sunrise_Day_One = dateTime.AddSeconds(day.SelectToken("sunrise").ToObject<int>()).ToLocalTime();
                        wvm.Sunset_Day_One = dateTime.AddSeconds(day.SelectToken("sunset").ToObject<int>()).ToLocalTime();
                        wvm.MoonRise_Day_One = dateTime.AddSeconds(day.SelectToken("moonrise").ToObject<int>()).ToLocalTime();
                        wvm.MoonSet_Day_One = dateTime.AddSeconds(day.SelectToken("moonset").ToObject<int>()).ToLocalTime();
                        wvm.MoonPhaseAs_Number_DayOne = day.SelectToken("moon_phase").ToObject<double>();
                        wvm.Summary_Day_One = day.SelectToken("summary").ToObject<string>();
                        var temps = day.SelectToken("temp");
                        var feels_likes = day.SelectToken("feels_like");
                        var weather = day.SelectToken("weather");
                        wvm.Temperature_Day_One = temps.SelectToken("day").ToObject<double>();
                        wvm.Temperature_Day_One_Min = temps.SelectToken("min").ToObject<double>();
                        wvm.Temperature_Day_One_Max = temps.SelectToken("max").ToObject<double>();
                        wvm.Temperature_Day_One_Night = temps.SelectToken("night").ToObject<double>();
                        wvm.Temperature_Day_One_Low = temps.SelectToken("eve").ToObject<double>();
                        wvm.Temperature_Day_One_Morning = temps.SelectToken("morn").ToObject<double>();
                        wvm.Temperature_Day_One_Low_Feels_Like = feels_likes.SelectToken("day").ToObject<double>();
                        wvm.Temperature_Day_One_Night_Feels_Like = feels_likes.SelectToken("night").ToObject<double>();
                        wvm.Temperature_Day_One_Eve_Feels_Like = feels_likes.SelectToken("eve").ToObject<double>();
                        wvm.Temperature_Day_One_Morn_Feels_Like = feels_likes.SelectToken("morn").ToObject<double>();
                        wvm.Weather_Day_One_ID = weather[0].SelectToken("id").ToObject<int>();
                        wvm.Weather_Day_One_Main = weather[0].SelectToken("main").ToObject<string>();
                        wvm.Weather_Day_One_Description = weather[0].SelectToken("description").ToObject<string>();
                        wvm.Weather_Day_One_Icon = weather[0].SelectToken("icon").ToObject<string>();
                        wvm.Day_One_Pressure = day.SelectToken("pressure").ToObject<int>();
                        wvm.Day_One_Humidity = day.SelectToken("humidity").ToObject<int>();
                        wvm.Day_One_Dew_Point = day.SelectToken("dew_point").ToObject<double>();
                        wvm.WindSpeedDay_One = day.SelectToken("wind_speed").ToObject<double>();
                        wvm.WindDirectionDay_One = day.SelectToken("wind_deg").ToObject<int>();
                        wvm.WindGustDay_One = day.SelectToken("wind_gust").ToObject<double>();
                        wvm.Temperature_Day_One_Low = day.SelectToken("clouds").ToObject<double>();
                        wvm.UV_Index_DayOne = day.SelectToken("uvi").ToObject<double>();
                        wvm.Clouds_day_one = day.SelectToken("clouds").ToObject<int>();
                        // wvm.Rain_Day_One = day.SelectToken("rain").ToObject<int>();
                    }
                    else if (i == 1)
                    {
                        wvm.Forecast_Day_Two = day.SelectToken("summary").ToObject<string>();
                        wvm.Sunrise_Day_Two = dateTime.AddSeconds(day.SelectToken("sunrise").ToObject<int>()).ToLocalTime();
                        wvm.Sunset_Day_Two = dateTime.AddSeconds(day.SelectToken("sunset").ToObject<int>()).ToLocalTime();
                        wvm.MoonRise_Day_Two = dateTime.AddSeconds(day.SelectToken("moonrise").ToObject<int>()).ToLocalTime();
                        wvm.MoonSet_Day_Two = dateTime.AddSeconds(day.SelectToken("moonset").ToObject<int>()).ToLocalTime();
                        wvm.MoonPhaseAs_Number_DayTwo = day.SelectToken("moon_phase").ToObject<double>();
                        wvm.Summary_Day_Two = day.SelectToken("summary").ToObject<string>();
                        var temps = day.SelectToken("temp");
                        var feels_likes = day.SelectToken("feels_like");
                        var weather = day.SelectToken("weather");
                        wvm.Temperature_Day_Two = temps.SelectToken("day").ToObject<double>();
                        wvm.Temperature_Day_Two_Min = temps.SelectToken("min").ToObject<double>();
                        wvm.Temperature_Day_Two_Max = temps.SelectToken("max").ToObject<double>();
                        wvm.Temperature_Day_Two_Night = temps.SelectToken("night").ToObject<double>();
                        wvm.Temperature_Day_Two_Low = temps.SelectToken("eve").ToObject<double>();
                        wvm.Temperature_Day_Two_Morning = temps.SelectToken("morn").ToObject<double>();
                        wvm.Temperature_Day_Two_Low_Feels_Like = feels_likes.SelectToken("day").ToObject<double>();
                        wvm.Temperature_Day_Two_Night_Feels_Like = feels_likes.SelectToken("night").ToObject<double>();
                        wvm.Temperature_Day_Two_Eve_Feels_Like = feels_likes.SelectToken("eve").ToObject<double>();
                        wvm.Temperature_Day_Two_Morn_Feels_Like = feels_likes.SelectToken("morn").ToObject<double>();
                        wvm.Weather_Day_Two_ID = weather[0].SelectToken("id").ToObject<int>();
                        wvm.Weather_Day_Two_Main = weather[0].SelectToken("main").ToObject<string>();
                        wvm.Weather_Day_Two_Description = weather[0].SelectToken("description").ToObject<string>();
                        wvm.Weather_Day_Two_Icon = weather[0].SelectToken("icon").ToObject<string>();
                        wvm.Day_Two_Pressure = day.SelectToken("pressure").ToObject<int>();
                        wvm.Day_Two_Humidity = day.SelectToken("humidity").ToObject<int>();
                        wvm.Day_Two_Dew_Point = day.SelectToken("dew_point").ToObject<double>();
                        wvm.WindSpeedDay_Two = day.SelectToken("wind_speed").ToObject<double>();
                        wvm.WindDirectionDay_Two = day.SelectToken("wind_deg").ToObject<int>();
                        wvm.WindGustDay_Two = day.SelectToken("wind_gust").ToObject<double>();
                        wvm.Temperature_Day_Two_Low = day.SelectToken("clouds").ToObject<double>();
                        wvm.UV_Index_DayTwo = day.SelectToken("uvi").ToObject<double>();
                        wvm.Clouds_day_two = day.SelectToken("clouds").ToObject<int>();
                        //wvm.Rain_Day_Two = day.SelectToken("rain").ToObject<int>();
                    }
                    else if (i == 2)
                    {
                        wvm.Forecasts_Day_Three = day.SelectToken("summary").ToObject<string>();
                        wvm.Sunrise_Day_Three = dateTime.AddSeconds(day.SelectToken("sunrise").ToObject<int>()).ToLocalTime();
                        wvm.Sunset_Day_Three = dateTime.AddSeconds(day.SelectToken("sunset").ToObject<int>()).ToLocalTime();
                        wvm.MoonRise_Day_Three = dateTime.AddSeconds(day.SelectToken("moonrise").ToObject<int>()).ToLocalTime();
                        wvm.MoonSet_Day_Three = dateTime.AddSeconds(day.SelectToken("moonset").ToObject<int>()).ToLocalTime();
                        wvm.MoonPhaseAs_Number_DayThree = day.SelectToken("moon_phase").ToObject<double>();
                        wvm.Summary_Day_Three = day.SelectToken("summary").ToObject<string>();
                        var temps = day.SelectToken("temp");
                        var feels_likes = day.SelectToken("feels_like");
                        var weather = day.SelectToken("weather");
                        wvm.Temperature_Day_Three = temps.SelectToken("day").ToObject<double>();
                        wvm.Temperature_Day_Three_Min = temps.SelectToken("min").ToObject<double>();
                        wvm.Temperature_Day_Three_Max = temps.SelectToken("max").ToObject<double>();
                        wvm.Temperature_Day_Three_Night = temps.SelectToken("night").ToObject<double>();
                        wvm.Temperature_Day_Three_Low = temps.SelectToken("eve").ToObject<double>();
                        wvm.Temperature_Day_Three_Morning = temps.SelectToken("morn").ToObject<double>();
                        wvm.Temperature_Day_Three_Low_Feels_Like = feels_likes.SelectToken("day").ToObject<double>();
                        wvm.Temperature_Day_Three_Night_Feels_Like = feels_likes.SelectToken("night").ToObject<double>();
                        wvm.Temperature_Day_Three_Eve_Feels_Like = feels_likes.SelectToken("eve").ToObject<double>();
                        wvm.Temperature_Day_Three_Morn_Feels_Like = feels_likes.SelectToken("morn").ToObject<double>();
                        wvm.Weather_Day_Three_ID = weather[0].SelectToken("id").ToObject<int>();
                        wvm.Weather_Day_Three_Main = weather[0].SelectToken("main").ToObject<string>();
                        wvm.Weather_Day_Three_Description = weather[0].SelectToken("description").ToObject<string>();
                        wvm.Weather_Day_Three_Icon = weather[0].SelectToken("icon").ToObject<string>();
                        wvm.Day_Three_Pressure = day.SelectToken("pressure").ToObject<int>();
                        wvm.Day_Three_Humidity = day.SelectToken("humidity").ToObject<int>();
                        wvm.Day_Three_Dew_Point = day.SelectToken("dew_point").ToObject<double>();
                        wvm.WindSpeedDay_Three = day.SelectToken("wind_speed").ToObject<double>();
                        wvm.WindDirectionDay_Three = day.SelectToken("wind_deg").ToObject<int>();
                        wvm.WindGustDay_Three = day.SelectToken("wind_gust").ToObject<double>();
                        wvm.Temperature_Day_Three_Low = day.SelectToken("clouds").ToObject<double>();
                        wvm.UV_Index_DayThree = day.SelectToken("uvi").ToObject<double>();
                        wvm.Clouds_day_Three = day.SelectToken("clouds").ToObject<int>();
                        // wvm.Rain_Day_Three = day.SelectToken("rain").ToObject<int>();
                    }
                    else if (i == 3)
                    {
                        wvm.Forecasts_Day_Four = day.SelectToken("summary").ToObject<string>();
                        wvm.Sunrise_Day_Four = dateTime.AddSeconds(day.SelectToken("sunrise").ToObject<int>()).ToLocalTime();
                        wvm.Sunset_Day_Four = dateTime.AddSeconds(day.SelectToken("sunset").ToObject<int>()).ToLocalTime();
                        wvm.MoonRise_Day_Four = dateTime.AddSeconds(day.SelectToken("moonrise").ToObject<int>()).ToLocalTime();
                        wvm.MoonSet_Day_Four = dateTime.AddSeconds(day.SelectToken("moonset").ToObject<int>()).ToLocalTime();
                        wvm.MoonPhaseAs_Number_DayFour = day.SelectToken("moon_phase").ToObject<double>();
                        wvm.Summary_Day_Four = day.SelectToken("summary").ToObject<string>();
                        var temps = day.SelectToken("temp");
                        var feels_likes = day.SelectToken("feels_like");
                        var weather = day.SelectToken("weather");
                        wvm.Temperature_Day_Four = temps.SelectToken("day").ToObject<double>();
                        wvm.Temperature_Day_Four_Min = temps.SelectToken("min").ToObject<double>();
                        wvm.Temperature_Day_Four_Max = temps.SelectToken("max").ToObject<double>();
                        wvm.Temperature_Day_Four_Night = temps.SelectToken("night").ToObject<double>();
                        wvm.Temperature_Day_Four_Low = temps.SelectToken("eve").ToObject<double>();
                        wvm.Temperature_Day_Four_Morning = temps.SelectToken("morn").ToObject<double>();
                        wvm.Temperature_Day_Four_Low_Feels_Like = feels_likes.SelectToken("day").ToObject<double>();
                        wvm.Temperature_Day_Four_Night_Feels_Like = feels_likes.SelectToken("night").ToObject<double>();
                        wvm.Temperature_Day_Four_Eve_Feels_Like = feels_likes.SelectToken("eve").ToObject<double>();
                        wvm.Temperature_Day_Four_Morn_Feels_Like = feels_likes.SelectToken("morn").ToObject<double>();
                        wvm.Weather_Day_Four_ID = weather[0].SelectToken("id").ToObject<int>();
                        wvm.Weather_Day_Four_Main = weather[0].SelectToken("main").ToObject<string>();
                        wvm.Weather_Day_Four_Description = weather[0].SelectToken("description").ToObject<string>();
                        wvm.Weather_Day_Four_Icon = weather[0].SelectToken("icon").ToObject<string>();
                        wvm.Day_Four_Pressure = day.SelectToken("pressure").ToObject<int>();
                        wvm.Day_Four_Humidity = day.SelectToken("humidity").ToObject<int>();
                        wvm.Day_Four_Dew_Point = day.SelectToken("dew_point").ToObject<double>();
                        wvm.WindSpeedDay_Four = day.SelectToken("wind_speed").ToObject<double>();
                        wvm.WindDirectionDay_Four = day.SelectToken("wind_deg").ToObject<int>();
                        wvm.WindGustDay_Four = day.SelectToken("wind_gust").ToObject<double>();
                        wvm.Temperature_Day_Four_Low = day.SelectToken("clouds").ToObject<double>();
                        wvm.UV_Index_DayFour = day.SelectToken("uvi").ToObject<double>();
                        wvm.Clouds_day_Four = day.SelectToken("clouds").ToObject<int>();
                        // wvm.Rain_Day_Four = day.SelectToken("rain").ToObject<int>();
                    }
                    else if (i == 4)
                    {
                        wvm.Forecasts_Day_Five = day.SelectToken("summary").ToObject<string>();
                        wvm.Sunrise_Day_Five = dateTime.AddSeconds(day.SelectToken("sunrise").ToObject<int>()).ToLocalTime();
                        wvm.Sunset_Day_Five = dateTime.AddSeconds(day.SelectToken("sunset").ToObject<int>()).ToLocalTime();
                        wvm.MoonRise_Day_Five = dateTime.AddSeconds(day.SelectToken("moonrise").ToObject<int>()).ToLocalTime();
                        wvm.MoonSet_Day_Five = dateTime.AddSeconds(day.SelectToken("moonset").ToObject<int>()).ToLocalTime();
                        wvm.MoonPhaseAs_Number_DayFive = day.SelectToken("moon_phase").ToObject<double>();
                        wvm.Summary_Day_Five = day.SelectToken("summary").ToObject<string>();
                        var temps = day.SelectToken("temp");
                        var feels_likes = day.SelectToken("feels_like");
                        var weather = day.SelectToken("weather");
                        wvm.Temperature_Day_Five = temps.SelectToken("day").ToObject<double>();
                        wvm.Temperature_Day_Five_Min = temps.SelectToken("min").ToObject<double>();
                        wvm.Temperature_Day_Five_Max = temps.SelectToken("max").ToObject<double>();
                        wvm.Temperature_Day_Five_Night = temps.SelectToken("night").ToObject<double>();
                        wvm.Temperature_Day_Five_Low = temps.SelectToken("eve").ToObject<double>();
                        wvm.Temperature_Day_Five_Morning = temps.SelectToken("morn").ToObject<double>();
                        wvm.Temperature_Day_Five_Low_Feels_Like = feels_likes.SelectToken("day").ToObject<double>();
                        wvm.Temperature_Day_Five_Night_Feels_Like = feels_likes.SelectToken("night").ToObject<double>();
                        wvm.Temperature_Day_Five_Eve_Feels_Like = feels_likes.SelectToken("eve").ToObject<double>();
                        wvm.Temperature_Day_Five_Morn_Feels_Like = feels_likes.SelectToken("morn").ToObject<double>();
                        wvm.Weather_Day_Five_ID = weather[0].SelectToken("id").ToObject<int>();
                        wvm.Weather_Day_Five_Main = weather[0].SelectToken("main").ToObject<string>();
                        wvm.Weather_Day_Five_Description = weather[0].SelectToken("description").ToObject<string>();
                        wvm.Weather_Day_Five_Icon = weather[0].SelectToken("icon").ToObject<string>();
                        wvm.Day_Five_Pressure = day.SelectToken("pressure").ToObject<int>();
                        wvm.Day_Five_Humidity = day.SelectToken("humidity").ToObject<int>();
                        wvm.Day_Five_Dew_Point = day.SelectToken("dew_point").ToObject<double>();
                        wvm.WindSpeedDay_Five = day.SelectToken("wind_speed").ToObject<double>();
                        wvm.WindDirectionDay_Five = day.SelectToken("wind_deg").ToObject<int>();
                        wvm.WindGustDay_Five = day.SelectToken("wind_gust").ToObject<double>();
                        wvm.Temperature_Day_Five_Low = day.SelectToken("clouds").ToObject<double>();
                        wvm.UV_Index_DayFive = day.SelectToken("uvi").ToObject<double>();
                        wvm.Clouds_day_Five = day.SelectToken("clouds").ToObject<int>();
                        // wvm.Rain_Day_Five = day.SelectToken("rain").ToObject<int>();
                    }
                    else if (i == 5)
                    {
                        wvm.Forecasts_Day_Six = day.SelectToken("summary").ToObject<string>();
                        wvm.Sunrise_Day_Six = dateTime.AddSeconds(day.SelectToken("sunrise").ToObject<int>()).ToLocalTime();
                        wvm.Sunset_Day_Six = dateTime.AddSeconds(day.SelectToken("sunset").ToObject<int>()).ToLocalTime();
                        wvm.MoonRise_Day_Six = dateTime.AddSeconds(day.SelectToken("moonrise").ToObject<int>()).ToLocalTime();
                        wvm.MoonSet_Day_Six = dateTime.AddSeconds(day.SelectToken("moonset").ToObject<int>()).ToLocalTime();
                        wvm.MoonPhaseAs_Number_DaySix = day.SelectToken("moon_phase").ToObject<double>();
                        wvm.Summary_Day_Six = day.SelectToken("summary").ToObject<string>();
                        var temps = day.SelectToken("temp");
                        var feels_likes = day.SelectToken("feels_like");
                        var weather = day.SelectToken("weather");
                        wvm.Temperature_Day_Six = temps.SelectToken("day").ToObject<double>();
                        wvm.Temperature_Day_Six_Min = temps.SelectToken("min").ToObject<double>();
                        wvm.Temperature_Day_Six_Max = temps.SelectToken("max").ToObject<double>();
                        wvm.Temperature_Day_Six_Night = temps.SelectToken("night").ToObject<double>();
                        wvm.Temperature_Day_Six_Low = temps.SelectToken("eve").ToObject<double>();
                        wvm.Temperature_Day_Six_Morning = temps.SelectToken("morn").ToObject<double>();
                        wvm.Temperature_Day_Six_Low_Feels_Like = feels_likes.SelectToken("day").ToObject<double>();
                        wvm.Temperature_Day_Six_Night_Feels_Like = feels_likes.SelectToken("night").ToObject<double>();
                        wvm.Temperature_Day_Six_Eve_Feels_Like = feels_likes.SelectToken("eve").ToObject<double>();
                        wvm.Temperature_Day_Six_Morn_Feels_Like = feels_likes.SelectToken("morn").ToObject<double>();
                        wvm.Weather_Day_Six_ID = weather[0].SelectToken("id").ToObject<int>();
                        wvm.Weather_Day_Six_Main = weather[0].SelectToken("main").ToObject<string>();
                        wvm.Weather_Day_Six_Description = weather[0].SelectToken("description").ToObject<string>();
                        wvm.Weather_Day_Six_Icon = weather[0].SelectToken("icon").ToObject<string>();
                        wvm.Day_Six_Pressure = day.SelectToken("pressure").ToObject<int>();
                        wvm.Day_Six_Humidity = day.SelectToken("humidity").ToObject<int>();
                        wvm.Day_Six_Dew_Point = day.SelectToken("dew_point").ToObject<double>();
                        wvm.WindSpeedDay_Six = day.SelectToken("wind_speed").ToObject<double>();
                        wvm.WindDirectionDay_Six = day.SelectToken("wind_deg").ToObject<int>();
                        wvm.WindGustDay_Six = day.SelectToken("wind_gust").ToObject<double>();
                        wvm.Temperature_Day_Six_Low = day.SelectToken("clouds").ToObject<double>();
                        wvm.UV_Index_DaySix = day.SelectToken("uvi").ToObject<double>();
                        wvm.Clouds_day_Six = day.SelectToken("clouds").ToObject<int>();
                        //  wvm.Rain_Day_Six = day.SelectToken("rain").ToObject<int>();
                    }
                    else if (i == 6)
                    {
                        wvm.Forecasts_Day_Seven = day.SelectToken("summary").ToObject<string>();
                        wvm.Sunrise_Day_Seven = dateTime.AddSeconds(day.SelectToken("sunrise").ToObject<int>()).ToLocalTime();
                        wvm.Sunset_Day_Seven = dateTime.AddSeconds(day.SelectToken("sunset").ToObject<int>()).ToLocalTime();
                        wvm.MoonRise_Day_Seven = dateTime.AddSeconds(day.SelectToken("moonrise").ToObject<int>()).ToLocalTime();
                        wvm.MoonSet_Day_Seven = dateTime.AddSeconds(day.SelectToken("moonset").ToObject<int>()).ToLocalTime();
                        wvm.MoonPhaseAs_Number_DaySeven = day.SelectToken("moon_phase").ToObject<double>();
                        wvm.Summary_Day_Seven = day.SelectToken("summary").ToObject<string>();
                        var temps = day.SelectToken("temp");
                        var feels_likes = day.SelectToken("feels_like");
                        var weather = day.SelectToken("weather");
                        wvm.Temperature_Day_Seven = temps.SelectToken("day").ToObject<double>();
                        wvm.Temperature_Day_Seven_Min = temps.SelectToken("min").ToObject<double>();
                        wvm.Temperature_Day_Seven_Max = temps.SelectToken("max").ToObject<double>();
                        wvm.Temperature_Day_Seven_Night = temps.SelectToken("night").ToObject<double>();
                        wvm.Temperature_Day_Seven_Low = temps.SelectToken("eve").ToObject<double>();
                        wvm.Temperature_Day_Seven_Morning = temps.SelectToken("morn").ToObject<double>();
                        wvm.Temperature_Day_Seven_Low_Feels_Like = feels_likes.SelectToken("day").ToObject<double>();
                        wvm.Temperature_Day_Seven_Night_Feels_Like = feels_likes.SelectToken("night").ToObject<double>();
                        wvm.Temperature_Day_Seven_Eve_Feels_Like = feels_likes.SelectToken("eve").ToObject<double>();
                        wvm.Temperature_Day_Seven_Morn_Feels_Like = feels_likes.SelectToken("morn").ToObject<double>();
                        wvm.Weather_Day_Seven_ID = weather[0].SelectToken("id").ToObject<int>();
                        wvm.Weather_Day_Seven_Main = weather[0].SelectToken("main").ToObject<string>();
                        wvm.Weather_Day_Seven_Description = weather[0].SelectToken("description").ToObject<string>();
                        wvm.Weather_Day_Seven_Icon = weather[0].SelectToken("icon").ToObject<string>();
                        wvm.Day_Seven_Pressure = day.SelectToken("pressure").ToObject<int>();
                        wvm.Day_Seven_Humidity = day.SelectToken("humidity").ToObject<int>();
                        wvm.Day_Seven_Dew_Point = day.SelectToken("dew_point").ToObject<double>();
                        wvm.WindSpeedDay_Seven = day.SelectToken("wind_speed").ToObject<double>();
                        wvm.WindDirectionDay_Seven = day.SelectToken("wind_deg").ToObject<int>();
                        wvm.WindGustDay_Seven = day.SelectToken("wind_gust").ToObject<double>();
                        wvm.Temperature_Day_Seven_Low = day.SelectToken("clouds").ToObject<double>();
                        wvm.UV_Index_DaySeven = day.SelectToken("uvi").ToObject<double>();
                        wvm.Clouds_day_Seven = day.SelectToken("clouds").ToObject<int>();
                        //  wvm.Rain_Day_Seven = day.SelectToken("rain").ToObject<int>();
                    }


                    i++;
                }

            }
            return wvm;
        }

        public WeatherViewModel Save(WeatherViewModel model)
        {
            try
            {
                _weatherQuery.SaveItem(model);
                return model;
            }
            catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
                return new WeatherViewModel();
            }
        }
        public WeatherViewModel Save(WeatherViewModel model, bool returnNew)
        {
            try
            {
                var x = Save(model);
                return returnNew ? new WeatherViewModel() : model;
            }
            catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
                return new WeatherViewModel();
            }
        }
        public WeatherViewModel Get(int id)
        {
            try
            {
                return _weatherQuery.GetItem<WeatherViewModel>(id);
            }
            catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
                return new WeatherViewModel();

            }
        }
        public bool Delete(WeatherViewModel model)
        {
            try
            {
                var y = _weatherQuery.DeleteItem<WeatherViewModel>(model);
                return y == 420 ? false : true;
            }
            catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
                return false;
            }
        }
        public bool Delete(int id)
        {
            try
            {
                return _weatherQuery.DeleteItem<WeatherViewModel>(_weatherQuery.GetItem<WeatherViewModel>(id)) == 420 ? false : true;
            }
            catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
                return false;
            }
        }
        public bool Delete(double latitude, double longitude)
        {
            try
            {
                return _weatherQuery.DeleteItem<WeatherViewModel>(_weatherQuery.GetWeather(latitude, longitude)) == 420 ? false : true;
            }catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
                return false;
            }
        }


    }
}
