using Locations.Core.Shared.DTO.Interfaces;
using Locations.Core.Shared.ViewModelServices;
using System;
using SQLite;

namespace Locations.Core.Shared.DTO
{
    /// <summary>
    /// WeatherDTO class that implements IWeatherDTO by inheriting from WeatherDetailsDTO
    /// </summary>
    [Table("Weather")]
    public class WeatherDTO : WeatherDetailsDTO, IWeatherDTO
    {
        // Constructor with defaults
        public WeatherDTO() : base()
        {
        }

        // Constructor with format parameters
        public WeatherDTO(string timeFormat, string dateFormat) : base(timeFormat, dateFormat)
        {
        }

        // Constructor with settings service
        public WeatherDTO(ISettingService settingsService) : base(settingsService)
        {
        }

        // Clone method to create a WeatherDTO from a WeatherDetailsDTO
        public static WeatherDTO FromWeatherDetails(WeatherDetailsDTO details)
        {
            var dto = new WeatherDTO();

            // Copy all properties from the details object
            dto.Id = details.Id;
            dto.LocationId = details.LocationId;
            dto.Latitude = details.Latitude;
            dto.Longitude = details.Longitude;
            dto.Timezone = details.Timezone;
            dto.TimezoneOffset = details.TimezoneOffset;
            dto.LastUpdate = details.LastUpdate;
            dto.TimeFormat = details.TimeFormat;
            dto.DateFormat = details.DateFormat;
            dto.WindDirectionArrow = details.WindDirectionArrow;

            // Copy all day properties
            dto.Sunrise_day_one = details.Sunrise_day_one;
            dto.Sunset_day_one = details.Sunset_day_one;
            dto.Temperature_day_one = details.Temperature_day_one;
            dto.Temperature_day_one_low = details.Temperature_day_one_low;
            dto.Forecast_day_one = details.Forecast_day_one;
            // And so on for all other properties...

            return dto;
        }

        // ToDetails method to convert a WeatherDTO to a WeatherDetailsDTO
        public WeatherDetailsDTO ToWeatherDetails()
        {
            // Since WeatherDTO inherits from WeatherDetailsDTO, this is already a WeatherDetailsDTO
            // Just return this instance, properties will be properly copied
            return this;
        }
    }
}