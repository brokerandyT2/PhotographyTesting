// TestDataFactory.cs
using System;
using System.Collections.Generic;
using Locations.Core.Data.Models;
using Locations.Core.Shared.ViewModels;
using Locations.Core.Shared.DTO;

namespace Locations.Core.Business.Tests.TestHelpers
{
    /// <summary>
    /// Factory class to create test data objects for unit tests
    /// </summary>
    public static class TestDataFactory
    {
        /// <summary>
        /// Creates a test location with specified ID
        /// </summary>
        public static LocationViewModel CreateTestLocation(int id = 1)
        {
            return new LocationViewModel
            {
                Id = id,
                Title = $"Test Location {id}",
                Description = "This is a test location created for unit tests",
                Lattitude = 40.7128,
                Longitude = -74.0060,
                City = "Test City",
                State = "Test State",
                Timestamp = DateTime.Now.AddDays(-id),
                DateFormat = "MM/dd/yyyy",
                IsDeleted = false,
                Photo = $"photo_{id}.jpg"
            };
        }

        /// <summary>
        /// Creates a list of test locations
        /// </summary>
        public static List<LocationViewModel> CreateTestLocations(int count = 3)
        {
            var locations = new List<LocationViewModel>();
            for (int i = 1; i <= count; i++)
            {
                locations.Add(CreateTestLocation(i));
            }
            return locations;
        }

        /// <summary>
        /// Creates a test setting with specified key and value
        /// </summary>
        public static SettingViewModel CreateTestSetting(string key = "TestKey", string value = "TestValue")
        {
            return new SettingViewModel
            {
                Id = 1,
                Key = key,
                Value = value,
                Description = $"Test setting for {key}"
            };
        }

        /// <summary>
        /// Creates a test settings collection
        /// </summary>
        public static SettingsViewModel CreateTestSettings()
        {
            var settings = new SettingsViewModel
            {
                Hemisphere = CreateTestSetting("Hemisphere", "north"),
                FirstName = CreateTestSetting("FirstName", "Test"),
                LastName = CreateTestSetting("LastName", "User"),
                Email = CreateTestSetting("Email", "test@example.com"),
                DateFormat = CreateTestSetting("DateFormat", "MM/dd/yyyy"),
                TimeFormat = CreateTestSetting("TimeFormat", "h:mm tt"),
                WindDirection = CreateTestSetting("WindDirection", "towardsWind"),
                Language = CreateTestSetting("Language", "en-US"),
                LastBulkWeatherUpdate = CreateTestSetting("LastBulkWeatherUpdate", DateTime.Now.AddDays(-1).ToString()),
                HomePageViewed = CreateTestSetting("HomePageViewed", "True"),
                TemperatureFormat = CreateTestSetting("TemperatureFormat", "F")
            };
            return settings;
        }

        /// <summary>
        /// Creates a test weather view model
        /// </summary>
        public static WeatherViewModel CreateTestWeather(int locationId = 1)
        {
            return new WeatherViewModel
            {
                Id = locationId,
                LocationId = locationId,
                Temperature = 72.5,
                Description = "Partly Cloudy",
                WindSpeed = 5.2,
                WindDirection = 180,
                Humidity = 65,
                Pressure = 1013,
                Visibility = 10,
                Precipitation = 0,
                CloudCover = 30,
                FeelsLike = 73.2,
                UVIndex = 6,
                AirQuality = 42,
                Forecast = "Mostly sunny with a chance of rain",
                LastUpdate = DateTime.Now,
                SunriseTime = DateTime.Today.AddHours(6).AddMinutes(30),
                SunsetTime = DateTime.Today.AddHours(20).AddMinutes(15),
                IsDeleted = false
            };
        }

        /// <summary>
        /// Creates a test tip type
        /// </summary>
        public static TipTypeViewModel CreateTestTipType(int id = 1)
        {
            return new TipTypeViewModel
            {
                Id = id,
                Name = $"Test Tip Type {id}",
                Description = $"Description for test tip type {id}",
                DisplayOrder = id,
                I8n = "en-US"
            };
        }

        /// <summary>
        /// Creates a test tip
        /// </summary>
        public static TipViewModel CreateTestTip(int id = 1, int tipTypeId = 1)
        {
            return new TipViewModel
            {
                Id = id,
                TipTypeId = tipTypeId,
                TipTypeName = $"Test Tip Type {tipTypeId}",
                Title = $"Test Tip {id}",
                Description = $"This is test tip {id}",
                Apeture = "f/2.8",
                Shutterspeed = "1/250",
                Iso = "400",
                IsDeleted = false
            };
        }

        /// <summary>
        /// Creates a list of test tips
        /// </summary>
        public static List<TipViewModel> CreateTestTips(int count = 3, int tipTypeId = 1)
        {
            var tips = new List<TipViewModel>();
            for (int i = 1; i <= count; i++)
            {
                tips.Add(CreateTestTip(i, tipTypeId));
            }
            return tips;
        }

        /// <summary>
        /// Creates a success operation result
        /// </summary>
        public static DataOperationResult<T> CreateSuccessResult<T>(T data)
        {
            return DataOperationResult<T>.Success(data);
        }

        /// <summary>
        /// Creates a failure operation result
        /// </summary>
        public static DataOperationResult<T> CreateFailureResult<T>(
            ErrorSource source = ErrorSource.Unknown,
            string message = "Test error message",
            Exception exception = null)
        {
            return DataOperationResult<T>.Failure(source, message, exception);
        }
    }
}