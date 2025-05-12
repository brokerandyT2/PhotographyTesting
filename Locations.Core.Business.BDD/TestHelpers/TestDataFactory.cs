using System;
using System.Collections.Generic;
using Locations.Core.Shared.ViewModels;

namespace Locations.Core.Business.BDD.TestHelpers
{
    public static class TestDataFactory
    {
        public static LocationViewModel CreateTestLocation(int id = 1)
        {
            return new LocationViewModel
            {
                Id = id,
                Title = $"Test Location {id}",
                Description = $"Description for test location {id}",
                Lattitude = 40.7128 + (id * 0.01),
                Longitude = -74.0060 + (id * 0.01),
                City = "Test City",
                State = "Test State",
                Timestamp = DateTime.Now,
                IsDeleted = false
            };
        }

        public static List<LocationViewModel> CreateTestLocations(int count)
        {
            var locations = new List<LocationViewModel>();
            for (int i = 1; i <= count; i++)
            {
                locations.Add(CreateTestLocation(i));
            }
            return locations;
        }

        public static WeatherViewModel CreateTestWeather(int id = 1, int locationId = 1)
        {
            return new WeatherViewModel
            {
                Id = id,
                LocationId = locationId,
                Temperature = 72.5,
                Description = "Partly Cloudy",
                Humidity = 65,
                WindSpeed = 12.5,
                WindDirection = 180,
                Pressure = 1013.25,
                Timestamp = DateTime.Now,
                Forecast = "Clear skies ahead",
                LastUpdate = DateTime.Now,
                IsDeleted = false
            };
        }

        public static TipViewModel CreateTestTip(int id = 1, int tipTypeId = 1)
        {
            return new TipViewModel
            {
                Id = id,
                TipTypeId = tipTypeId,
                Title = $"Test Tip {id}",
                Description = $"Test tip description {id}",
                Apeture = "f/2.8",
                Shutterspeed = "1/125",
                Iso = "400",
                IsDeleted = false
            };
        }

        public static List<TipViewModel> CreateTestTips(int count, int tipTypeId = 1)
        {
            var tips = new List<TipViewModel>();
            for (int i = 1; i <= count; i++)
            {
                tips.Add(CreateTestTip(i, tipTypeId));
            }
            return tips;
        }

        public static TipTypeViewModel CreateTestTipType(int id = 1)
        {
            return new TipTypeViewModel
            {
                Id = id,
                Name = $"TipType{id}",
                Description = $"Test tip type {id}",
                DisplayOrder = id,
                Icon = $"icon{id}.png"
            };
        }

        public static List<TipTypeViewModel> CreateTestTipTypes()
        {
            return new List<TipTypeViewModel>
            {
                new TipTypeViewModel { Id = 1, Name = "Landscape", DisplayOrder = 1 },
                new TipTypeViewModel { Id = 2, Name = "Portrait", DisplayOrder = 2 },
                new TipTypeViewModel { Id = 3, Name = "Night", DisplayOrder = 3 },
                new TipTypeViewModel { Id = 4, Name = "Macro", DisplayOrder = 4 },
                new TipTypeViewModel { Id = 5, Name = "Wildlife", DisplayOrder = 5 }
            };
        }

        public static SettingViewModel CreateTestSetting(string key, string value)
        {
            return new SettingViewModel
            {
                Id = key.GetHashCode(),
                Key = key,
                Value = value,
                Description = $"Test setting for {key}"
            };
        }

        public static SettingsViewModel CreateTestSettings()
        {
            return new SettingsViewModel
            {
                Email = CreateTestSetting("Email", "test@example.com"),
                Hemisphere = CreateTestSetting("Hemisphere", "North"),
                TimeFormat = CreateTestSetting("TimeFormat", "12-hour"),
                DateFormat = CreateTestSetting("DateFormat", "MM/DD/YYYY"),
                WindDirection = CreateTestSetting("WindDirection", "Towards Wind"),
                TemperatureFormat = CreateTestSetting("TemperatureType", "Fahrenheit"),
                AdSupport = CreateTestSetting("AdSupport", "false"),
                SubscriptionType = CreateTestSetting("SubscriptionType", "Free"),
                SubscriptionExpiration = CreateTestSetting("SubscriptionExpiration", DateTime.Now.AddDays(-1).ToString())
            };
        }

        public static Location.Photography.Shared.ViewModels.ExposureCalculator CreateTestExposureCalculator()
        {
            return new Location.Photography.Shared.ViewModels.ExposureCalculator
            {
                FStopSelected = "f/2.8",
                ShutterSpeedSelected = "1/125",
                ISOSelected = "400",
                OldFstop = "f/2.8",
                OldShutterSpeed = "1/125",
                OldISO = "400",
                FullHalfThirds = Location.Photography.Shared.ViewModels.ExposureCalculator.Divisions.Full
            };
        }
    }
}