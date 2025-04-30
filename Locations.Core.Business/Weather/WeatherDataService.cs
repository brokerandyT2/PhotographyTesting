
using Locations.Core.Shared.ViewModels;
using newtonsoft = Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using System.Threading.Tasks;
using Microsoft.Maui.Platform;
using Microsoft.Maui.Animations;


namespace Locations.Core.Business.Weather
{
    public class WeatherDataService
    {
        private readonly HttpClient _httpClient;
        private string endpoint;
        private double latitude;
        private double longitude;

        private string API_KEY;

        private string GenerateRequestUri(string endpoint, double latitude, double longitude, string API_KEY)
        {
            string requestUri = endpoint;
            requestUri += $"?lat={latitude}";
            requestUri += $"&lon={longitude}";
            requestUri += "&exclude=minutely,hourly,alerts&units=imperial";
            requestUri += $"&appid={API_KEY}";
            return requestUri;
        }

        public WeatherDataService()
        {
            _httpClient = new HttpClient();
        }
        public WeatherDataService(string endpoint, double latitude, double longitude, string API_KEY)
        {
            this.latitude = latitude;
            this.longitude = longitude;
            this.API_KEY = API_KEY;
            _httpClient = new HttpClient();
            this.endpoint = endpoint;
        }

        public async Task<WeatherViewModel> GetAllWeatherDataAsync()
        {
            try
            {
                return await GetAsyncMethodFactory<WeatherViewModel>(TypeMethod.GetAllWeatherDataAsync);
            }
            catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
                return new WeatherViewModel();
            }
        }



        public async Task<Placemark> GetPlacemarkAsync()
        {
            try
            {
                var location = await Geolocation.Default.GetLocationAsync();

                IEnumerable<Placemark> placemarks = await Geocoding.Default.GetPlacemarksAsync(this.latitude, this.longitude);
                Placemark placemark = placemarks?.FirstOrDefault();
                return placemark;
            }
            catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
                return new Placemark();
            }
            }

        public async Task<T> GetAsyncMethodFactory<T>(TypeMethod typeMethod) where T : class
        {
            try
            {
                Uri url = new(GenerateRequestUri(this.endpoint, this.latitude, this.longitude, this.API_KEY));

                HttpResponseMessage response = _httpClient.GetAsync(url).Result;

                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    return GetDeserializedContent<T>(typeMethod, content);
                }

                return default;
            }catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
                return default;
            }
        }

        public static T GetDeserializedContent<T>(TypeMethod typeMethod, string content) where T : class
        {
            try
            {
                var _serializerOptions = new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    WriteIndented = true
                };
                newtonsoft.JsonTextReader _reader = new newtonsoft.JsonTextReader(new StringReader(content));


                // var POCO = newtonsoft.JsonConvert.DeserializeObject<Current>(content);
                var PO = new Json.Net.JsonParser(); // (content);


                var x = content;


                return typeMethod switch
                {
                    TypeMethod.GetAllWeatherDataAsync =>
                    JsonSerializer.Deserialize<WeatherViewModel>(content) as T,
                    //TypeMethod.GetDaysAsync => JsonSerializer.Deserialize<Current>(content) as T,


                };
            }
            catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
                return default;
            }
            }

        public async Task<List<Daily>> GetDaysAsync()
        {
            try
            {
                return await GetAsyncMethodFactory<List<Daily>>(TypeMethod.GetDaysAsync);
            }
            catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
                return new List<Daily>();
            }
            
        }
        public async Task<WeatherViewModel> GetWeatherAsync()
        {
            try
            {
                WeatherViewModel weather = new WeatherViewModel();
                WeatherDataService weatherDataService = new WeatherDataService(this.endpoint, this.latitude, this.longitude, this.API_KEY);
                List<Daily> returned = null;
                try
                {
                    returned = weatherDataService.GetDaysAsync().Result;
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
                weather.Latitude = this.latitude;
                weather.Longitude = this.longitude;
                weather.Forecast_Day_One = returned[0].Weather[0].Description;
                weather.Forecast_Day_Two = returned[1].Weather[0].Description;
                weather.Forecasts_Day_Three = returned[2].Weather[0].Description;
                weather.Forecasts_Day_Four = returned[3].Weather[0].Description;
                weather.Forecasts_Day_Five = returned[4].Weather[0].Description;

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




                return weather;
            }
            catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
                return new WeatherViewModel();
            }
        }
    }
    public class Temp
    {
        [JsonPropertyName("day")]
        public double Day { get; set; }

        [JsonPropertyName("min")]
        public double Min { get; set; }

        [JsonPropertyName("max")]
        public double Max { get; set; }

        [JsonPropertyName("night")]
        public double Night { get; set; }

        [JsonPropertyName("eve")]
        public double Eve { get; set; }

        [JsonPropertyName("morn")]
        public double Morn { get; set; }
    }

    public class FeelsLike
    {
        [JsonPropertyName("day")]
        public double Day { get; set; }

        [JsonPropertyName("night")]
        public double Night { get; set; }

        [JsonPropertyName("eve")]
        public double Eve { get; set; }

        [JsonPropertyName("morn")]
        public double Morn { get; set; }
    }
    public enum TypeMethod
    {
        GetAllWeatherDataAsync,
        GetHourliesAsync,
        GetDaysAsync
    }
    public class WeatherData
    {
        [JsonPropertyName("lat")]
        public double Lat { get; set; }

        [JsonPropertyName("lon")]
        public double Lon { get; set; }

        [JsonPropertyName("timezone")]
        public string Timezone { get; set; }

        [JsonPropertyName("timezone_offset")]
        public int Timezone_offset { get; set; }

        [JsonPropertyName("hourly")]
        public List<Hourly> Hourly { get; set; }

        [JsonPropertyName("daily")]
        public List<Daily> Daily { get; set; }
    }

    public class Weather
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("main")]
        public string Main { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("icon")]
        public string Icon { get; set; }
    }

    public class Hourly
    {
        public bool IsAnimationSkeleton { get; set; }

        [JsonPropertyName("dt")]
        public long Dt { get; set; }

        [JsonPropertyName("temp")]
        public double Temp { get; set; }

        [JsonPropertyName("feels_like")]
        public double Feels_like { get; set; }

        [JsonPropertyName("pressure")]
        public int pressure { get; set; }

        [JsonPropertyName("humidity")]
        public int Humidity { get; set; }

        [JsonPropertyName("dew_point")]
        public double Dew_point { get; set; }

        [JsonPropertyName("uvi")]
        public double Uvi { get; set; }

        [JsonPropertyName("clouds")]
        public int Clouds { get; set; }

        [JsonPropertyName("visibility")]
        public int Visibility { get; set; }

        [JsonPropertyName("wind_speed")]
        public double Wind_speed { get; set; }

        [JsonPropertyName("wind_deg")]
        public int Wind_deg { get; set; }

        [JsonPropertyName("wind_gust")]
        public double Wind_gust { get; set; }

        [JsonPropertyName("weather")]
        public List<Weather> Weather { get; set; }

        [JsonPropertyName("pop")]
        public double Pop { get; set; }
    }





    public class Daily
    {
        [JsonPropertyName("dt")]
        public long Dt { get; set; }

        [JsonPropertyName("sunrise")]
        public int Sunrise { get; set; }

        [JsonPropertyName("sunset")]
        public int Sunset { get; set; }

        [JsonPropertyName("moonrise")]
        public int Moonrise { get; set; }

        [JsonPropertyName("moonset")]
        public int Moonset { get; set; }

        [JsonPropertyName("moon_phase")]
        public double Moon_phase { get; set; }

        [JsonPropertyName("temp")]
        public Temp Temp { get; set; }

        [JsonPropertyName("feels_like")]
        public FeelsLike Feels_like { get; set; }

        [JsonPropertyName("pressure")]
        public int Pressure { get; set; }

        [JsonPropertyName("humidity")]
        public int Humidity { get; set; }

        [JsonPropertyName("dew_point")]
        public double Dew_point { get; set; }

        [JsonPropertyName("wind_speed")]
        public double Wind_speed { get; set; }

        [JsonPropertyName("wind_deg")]
        public int Wind_deg { get; set; }

        [JsonPropertyName("wind_gust")]
        public double Wind_gust { get; set; }

        [JsonPropertyName("weather")]
        public List<Weather> Weather { get; set; }

        [JsonPropertyName("clouds")]
        public int Clouds { get; set; }

        [JsonPropertyName("pop")]
        public double Pop { get; set; }

        [JsonPropertyName("uvi")]
        public double Uvi { get; set; }
    }
}

