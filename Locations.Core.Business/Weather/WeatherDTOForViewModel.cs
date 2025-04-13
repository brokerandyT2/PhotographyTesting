using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locations.Core.Business.Weather
{
    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
    public class Current
    {
        public int dt { get; set; }
        public int sunrise { get; set; }
        public int sunset { get; set; }
        public double temp { get; set; }
        public double feels_like { get; set; }
        public int pressure { get; set; }
        public int humidity { get; set; }
        public double dew_point { get; set; }
        public double uvi { get; set; }
        public int clouds { get; set; }
        public int visibility { get; set; }
        public double wind_speed { get; set; }
        public int wind_deg { get; set; }
        public double wind_gust { get; set; }
        public List<Weather> weathers { get; set; }
    }

    public class Dailys
    {
        public int dt { get; set; }
        public int sunrise { get; set; }
        public int sunset { get; set; }
        public int moonrise { get; set; }
        public int moonset { get; set; }
        public double moon_phase { get; set; }
        public string summary { get; set; }
        public Temps temps { get; set; }
        public FeelsLikeC feels_like { get; set; }
        public int pressure { get; set; }
        public int humidity { get; set; }
        public double dew_point { get; set; }
        public double wind_speed { get; set; }
        public int wind_deg { get; set; }
        public double wind_gust { get; set; }
        public List<Weather> weathers { get; set; }
        public int clouds { get; set; }
        public int pop { get; set; }
        public double uvi { get; set; }
        public double? rain { get; set; }
    }

    public class FeelsLikeC
    {
        public double day { get; set; }
        public double night { get; set; }
        public double eve { get; set; }
        public double morn { get; set; }
    }

    public class Root
    {
        public double lat { get; set; }
        public int lon { get; set; }
        public string timezone { get; set; }
        public int timezone_offset { get; set; }
        public Current current { get; set; }
        public List<Daily> days { get; set; }
    }

    public class Temps
    {
        public double day { get; set; }
        public double min { get; set; }
        public double max { get; set; }
        public double night { get; set; }
        public double eve { get; set; }
        public double morn { get; set; }
    }

    public class Weathers
    {
        public int id { get; set; }
        public string main { get; set; }
        public string description { get; set; }
        public string icon { get; set; }
    }

}