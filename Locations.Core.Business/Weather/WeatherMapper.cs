using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Locations.Core.Shared.DTO;
using Locations.Core.Shared.ViewModels;
namespace Locations.Core.Business.Weather
{
    /// <summary>
    /// Maps the given WeatherViewModel to a WeatherDetailsDTO for the specified day.
    /// Configs are inerited in the MapperConfigs class.
    /// </summary>
    internal class WeatherMapper : MapperConfigs
    {
        public WeatherMapper()
        {

        }

        private DailyWeatherWithDetailsDTO MapDayOne(WeatherViewModel weather)
        {
            var mapper = configDayOne.CreateMapper();
            var y = mapper.Map<DailyWeatherWithDetailsDTO>(weather);
            y.Details = detailsDayOne.CreateMapper().Map<WeatherDetailsDTO>(weather);
            return y;
        }
        private DailyWeatherWithDetailsDTO MapDayTwo(WeatherViewModel weather)
        {
            var mapper = configDayTwo.CreateMapper();
            var y = mapper.Map<DailyWeatherWithDetailsDTO>(weather);
            y.Details = detailsDayOne.CreateMapper().Map<WeatherDetailsDTO>(weather);
            return y;
        }

        private DailyWeatherWithDetailsDTO MapDayThree(WeatherViewModel weather)
        {
            var mapper = configDayThree.CreateMapper();
            var y = mapper.Map<DailyWeatherWithDetailsDTO>(weather);
            y.Details = detailsDayOne.CreateMapper().Map<WeatherDetailsDTO>(weather);
            return y;
        }
        private DailyWeatherWithDetailsDTO MapDayFour(WeatherViewModel weather)
        {
            var mapper = configDayFour.CreateMapper();
            var y = mapper.Map<DailyWeatherWithDetailsDTO>(weather);
            y.Details = detailsDayOne.CreateMapper().Map<WeatherDetailsDTO>(weather);
            return y;
        }
        private DailyWeatherWithDetailsDTO MapDayFive(WeatherViewModel weather)
        {
            var mapper = configDayFive.CreateMapper();
            var y = mapper.Map<DailyWeatherWithDetailsDTO>(weather);
            y.Details = detailsDayOne.CreateMapper().Map<WeatherDetailsDTO>(weather);
            return y;
        }
        private DailyWeatherWithDetailsDTO MapDaySix(WeatherViewModel weather)
        {
            var mapper = configDaySix.CreateMapper();
            var y = mapper.Map<DailyWeatherWithDetailsDTO>(weather);
            y.Details = detailsDayOne.CreateMapper().Map<WeatherDetailsDTO>(weather);
            return y;
        }
        private DailyWeatherWithDetailsDTO MapDaySeven(WeatherViewModel weather)
        {
            var mapper = configDayseven.CreateMapper();
            var y = mapper.Map<DailyWeatherWithDetailsDTO>(weather);
            y.Details = detailsDayOne.CreateMapper().Map<WeatherDetailsDTO>(weather);
            return y;
        }
        public DailyWeatherWithDetailsDTO MapDay(WeatherViewModel weather, int day)
        {
            switch (day)
            {
                case 1:
                    return MapDayOne(weather);
                case 2:
                    return MapDayTwo(weather);
                case 3:
                    return MapDayThree(weather);
                case 4:
                    return MapDayFour(weather);
                case 5:
                    return MapDayFive(weather);
                case 6:
                    return MapDaySix(weather);
                case 7:
                    return MapDaySeven(weather);
                default:
                    throw new ArgumentOutOfRangeException(nameof(day), "Day must be between 1 and 7.");
            }
        }
    }
}
