using System;
using System.ComponentModel;

namespace Locations.Core.Shared.DTO.Interfaces
{
    public interface IWeatherDTO : IDTOBase
    {
        int ID { get; set; }
        double Latitude { get; set; }
        double Longitude { get; set; }
        string Timezone { get; set; }
        int TimezoneOffset { get; set; }
        DateTime LastUpdate { get; set; }

        // Add all the relevant properties that should be exposed through the interface
        // These can include the core weather data properties

        // Day One
        DateTime Sunrise_day_one { get; set; }
        DateTime Sunset_day_one { get; set; }
        double Temperature_day_one { get; set; }
        double Temperature_day_one_low { get; set; }
        string Forecast_day_one { get; set; }

        // Day forecasts
        string DayOne { get; }
        string DayTwo { get; }
        string DayThree { get; }
        string DayFour { get; }
        string DayFive { get; }

        // Format settings
        string TimeFormat { get; set; }
        string DateFormat { get; set; }

        // Events
        event PropertyChangedEventHandler PropertyChanged;
    }
}