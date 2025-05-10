// WeatherAPITests.cs
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading.Tasks;
using Locations.Core.Business.Weather;
using Locations.Core.Shared.ViewModels;
using Locations.Core.Business.Tests.Base;
using MockFactory = Locations.Core.Business.Tests.TestHelpers.MockFactory;
using TestDataFactory = Locations.Core.Business.Tests.TestHelpers.TestDataFactory;

namespace Locations.Core.Business.Tests.Services.WeatherTests
{
    [TestClass]
    [TestCategory("WeatherAPI")]
    public class WeatherAPITests : BaseServiceTests
    {
        private WeatherAPI _weatherAPI;
        private string _apiKey = "test_api_key";
        private double _latitude = 40.7128;
        private double _longitude = -74.0060;
        private string _url = "https://test.api.url";

        [TestInitialize]
        public override void Setup()
        {
            base.Setup();

            _weatherAPI = new WeatherAPI(_apiKey, _latitude, _longitude, _url);
        }

        [TestMethod]
        public void Constructor_WithId_ShouldNotThrowException()
        {
            // Act
            var weatherAPI = new WeatherAPI(1);

            // Assert - no exception thrown
        }

        [TestMethod]
        public void Constructor_WithApiKeyAndCoordinates_ShouldSetProperties()
        {
            // Act
            var weatherAPI = new WeatherAPI(_apiKey, _latitude, _longitude);

            // Assert
            Assert.AreEqual(_apiKey, weatherAPI.ApiKey);
        }

        [TestMethod]
        public void Constructor_WithApiKeyCoordinatesAndUrl_ShouldSetAllProperties()
        {
            // Act
            var weatherAPI = new WeatherAPI(_apiKey, _latitude, _longitude, _url);

            // Assert
            Assert.AreEqual(_apiKey, weatherAPI.ApiKey);
            Assert.AreEqual(_url, weatherAPI.URL);
        }

        [TestMethod]
        public async Task GetWeatherAsync_ShouldReturnWeatherViewModel()
        {
            // Act
            var result = await _weatherAPI.GetWeatherAsync();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsInstanceOfType(result, typeof(WeatherViewModel));
        }
    }
}