using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locations.Core.Shared.DTO
{
    public class WeatherDetailsDTO
    {
        public DateTime GoldenHourMorning { get; set; }
        public DateTime GoldenHourEvening { get; set; }
        public DateTime MoonRise { get; set; }
        public DateTime MoonSet { get; set; }
        public double MoonPhase { get; set; }
        public double Pressure  { get; set; }
        public double Humidity { get; set; }
        public double WindSpeed { get; set; }
        public double WindDirection { get; set; }
        public double WindGust { get; set; }
        public int CloudCover { get; set; }
        public double Visibility { get; set; }
        public double DewPoint { get; set; }
        public double Precipitation { get; set; }
        public double PrecipitationProbability { get; set; }
        public double UVIndex { get; set; }

    }
}
