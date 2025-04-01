using Locations.Core.Shared.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locations.Core.Business.Weather
{
    public class WeatherAPI
    {
        private string API_KEY;
        private double lattitude;
        private double longituded;
        private string _url;
        public string URL
        {
            get { return _url; }
            set { _url = value; }
        }

        public string ApiKey
        {
            get { return API_KEY; }
            set { API_KEY = value; }
        }
        public WeatherAPI() { }
        public WeatherAPI(int id) { }

        public WeatherAPI(string API_KEY, double lattitude, double longituded)
        {
            this.API_KEY = API_KEY;
            this.lattitude = lattitude;
            this.longituded = longituded;

        }
        public WeatherAPI(string API_KEY, double lattitude, double longituded, string URL)
        {
            this.API_KEY = API_KEY;
            this.lattitude = lattitude;
            this.longituded = longituded;
            this._url = URL;

        }
        public async Task<WeatherViewModel> GetWeatherAsync()
        {
            WeatherViewModel weather = new WeatherViewModel();
            WeatherDataService weatherDataService = new WeatherDataService(this._url, this.lattitude, this.longituded, this.API_KEY);


            var returned = weatherDataService.GetDaysAsync().Result;

            weather.Latitude = this.lattitude;
            weather.Longitude = this.longituded;
            weather.Forecast_Day_One = returned[0].Weather[0].Description;
            weather.Forecast_Day_Two = returned[1].Weather[0].Description;
            weather.Forecasts_Day_Three = returned[2].Weather[0].Description;
            weather.Forecasts_Day_Four = returned[3].Weather[0].Description;
            weather.Forecasts_Day_Five = returned[4].Weather[0].Description;
            //weather.Temperature_Day_One = returned[0].Temperature.Day;
            weather.Temperature_Day_One = (int)Math.Round(returned[0].Temp.Max);
            weather.Temperature_Day_One_Low = (int)Math.Round(returned[0].Temp.Min);
            weather.Temperature_Day_Two = (int)Math.Round(returned[1].Temp.Max);
            weather.Temperature_Day_Two_Low = (int)Math.Round(returned[1].Temp.Min);
            weather.Temperature_Day_Three = (int)Math.Round(returned[2].Temp.Max);
            weather.Temperature_Day_Three_Low = (int)Math.Round(returned[2].Temp.Min);
            weather.Temperature_Day_Four = (int)Math.Round(returned[3].Temp.Max);
            weather.Temperature_Day_Four_Low = (int)Math.Round(returned[3].Temp.Min);
            weather.Temperature_Day_Five = (int)Math.Round(returned[4].Temp.Max);
            weather.Temperature_Day_One_Low = (int)Math.Round(returned[4].Temp.Min);
            weather.Sunrise_Day_One = new DateTime(returned[0].Sunrise);
            weather.Sunrise_Day_Two = new DateTime(returned[1].Sunrise);
            weather.Sunrise_Day_Three = new DateTime(returned[2].Sunrise);
            weather.Sunrise_Day_Four = new DateTime(returned[3].Sunrise);
            weather.Sunrise_Day_Five = new DateTime(returned[4].Sunrise);
            weather.Sunset_Day_One = new DateTime(returned[0].Sunset);
            weather.Sunrise_Day_Two = new DateTime(returned[1].Sunset);
            weather.Sunrise_Day_Three = new DateTime(returned[2].Sunset);
            weather.Sunset_Day_Four = new DateTime(returned[3].Sunset);
            weather.Sunset_Day_Five = new DateTime(returned[4].Sunset);
            weather.LastUpdate = DateTime.Now;


            weather.WindSpeedDay_One = (decimal)returned[0].Wind_speed;
            weather.WindSpeedDay_Two = (decimal)returned[1].Wind_speed;
            weather.WindSpeedDay_Three = (decimal)returned[2].Wind_speed;
            weather.WindSpeedDay_Four = (decimal)returned[3].Wind_speed;
            weather.WindSpeedDay_Five = (decimal)returned[4].Wind_speed;

            weather.WindDirectionDay_One = returned[0].Wind_deg;
            weather.WindDirectionDay_Two = returned[1].Wind_deg;
            weather.WindDirectionDay_Three = returned[2].Wind_deg;
            weather.WindDirectionDay_Four = returned[3].Wind_deg;
            weather.WindDirectionDay_Five = returned[4].Wind_deg;

            weather.WindGustDay_One = (decimal)returned[0].Wind_gust;
            weather.WindGustDay_Two = (decimal)returned[1].Wind_gust;
            weather.WindGustDay_Three = (decimal)returned[2].Wind_gust;
            weather.WindGustDay_Four = (decimal)returned[3].Wind_gust;
            weather.WindGustDay_Five = (decimal)returned[4].Wind_gust;


            return weather;
        }
    }
}
