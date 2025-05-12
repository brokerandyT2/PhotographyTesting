// WeatherViewModelTests.cs - Fixed for test failures
using Locations.Core.Shared.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using Locations.Core.Shared.DTO;
using Locations.Core.Shared.ViewModelServices;
using OperationErrorSource = Locations.Core.Shared.ViewModels.OperationErrorSource;

namespace Locations.Core.Business.Tests.ViewModels
{
    [TestClass]
    [TestCategory("ViewModels")]
    public class WeatherViewModelTests
    {
        private Mock<IWeatherService> _mockWeatherService;
        private WeatherViewModel _viewModel;

        [TestInitialize]
        public void Setup()
        {
            _mockWeatherService = new Mock<IWeatherService>();
            _viewModel = new WeatherViewModel(_mockWeatherService.Object);
        }

        [TestMethod]
        public void Constructor_WithNoDependencies_ShouldInitializeProperties()
        {
            var viewModel = new WeatherViewModel();
            Assert.IsNotNull(viewModel.RefreshWeatherCommand);
        }

        [TestMethod]
        public void Constructor_WithWeatherService_ShouldInitializePropertiesAndService()
        {
            var viewModel = new WeatherViewModel(_mockWeatherService.Object);
            Assert.IsNotNull(viewModel.RefreshWeatherCommand);
        }

        [TestMethod]
        public void InitializeFromDTO_WithValidDTO_ShouldInitializeProperties()
        {
            var dto = new Locations.Core.Shared.DTO.WeatherDTO
            {
                Id = 1,
                LocationId = 2,
                Timestamp = new DateTime(2023, 1, 1),
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
                Forecast = "Sunny with afternoon clouds",
                LastUpdate = DateTime.Now,
                SunriseTime = new DateTime(2023, 1, 1, 6, 30, 0),
                SunsetTime = new DateTime(2023, 1, 1, 20, 15, 0),
                IsDeleted = false
            };

            _viewModel.InitializeFromDTO(dto);

            Assert.AreEqual(1, _viewModel.Id);
            Assert.AreEqual(2, _viewModel.LocationId);
            Assert.AreEqual(new DateTime(2023, 1, 1), _viewModel.Timestamp);
            Assert.AreEqual(72.5, _viewModel.Temperature);
            Assert.AreEqual("Partly Cloudy", _viewModel.Description);
            Assert.AreEqual(5.2, _viewModel.WindSpeed);
            Assert.AreEqual(180, _viewModel.WindDirection);
            Assert.AreEqual(65, _viewModel.Humidity);
            Assert.AreEqual(1013, _viewModel.Pressure);
            Assert.AreEqual(10, _viewModel.Visibility);
            Assert.AreEqual(0, _viewModel.Precipitation);
            Assert.AreEqual(30, _viewModel.CloudCover);
            Assert.AreEqual(73.2, _viewModel.FeelsLike);
            Assert.AreEqual(6, _viewModel.UVIndex);
            Assert.AreEqual(42, _viewModel.AirQuality);
            Assert.AreEqual("Sunny with afternoon clouds", _viewModel.Forecast);
            Assert.AreEqual(new DateTime(2023, 1, 1, 6, 30, 0), _viewModel.SunriseTime);
            Assert.AreEqual(new DateTime(2023, 1, 1, 20, 15, 0), _viewModel.SunsetTime);
            Assert.AreEqual(false, _viewModel.IsDeleted);
        }

        [TestMethod]
        public void InitializeFromDTO_WithNullDTO_ShouldNotThrowException()
        {
            _viewModel.InitializeFromDTO(null);
        }

        [TestMethod]
        public async Task RefreshWeatherAsync_WithValidLocationId_ShouldCallWeatherService()
        {
            _viewModel.LocationId = 1;
            _viewModel.IsError = false;

            var weatherDTO = new WeatherDTO
            {
                Id = 1,
                LocationId = 1,
                Temperature = 72.5,
                Description = "Partly Cloudy",
                LastUpdate = DateTime.Now
            };

            _mockWeatherService
                .Setup(service => service.GetWeatherForLocationAsync(1))
                .ReturnsAsync(Shared.ViewModelServices.OperationResult<WeatherDTO>.Success(weatherDTO));

            var refreshMethod = typeof(WeatherViewModel).GetMethod("RefreshWeatherAsync",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            await (Task)refreshMethod.Invoke(_viewModel, null);

            _mockWeatherService.Verify(service => service.GetWeatherForLocationAsync(1), Times.Once);
            Assert.AreEqual(72.5, _viewModel.Temperature);
            Assert.AreEqual("Partly Cloudy", _viewModel.Description);
        }

        [TestMethod]
        public async Task RefreshWeatherAsync_WithInvalidLocationId_ShouldSetErrorMessage()
        {
            _viewModel.LocationId = 0;
            _viewModel.IsError = false;
            _viewModel.ErrorMessage = string.Empty;

            var refreshMethod = typeof(WeatherViewModel).GetMethod("RefreshWeatherAsync",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            await (Task)refreshMethod.Invoke(_viewModel, null);

            Assert.IsTrue(_viewModel.IsError);
            Assert.IsTrue(_viewModel.ErrorMessage.Contains("Invalid location ID"));
        }

        [TestMethod]
        public async Task RefreshWeatherAsync_WhenWeatherServiceFails_ShouldSetErrorMessage()
        {
            _viewModel.LocationId = 1;
            _viewModel.IsError = false;
            _viewModel.ErrorMessage = string.Empty;

            _mockWeatherService
                .Setup(service => service.GetWeatherForLocationAsync(1))
                .ReturnsAsync(Shared.ViewModelServices.OperationResult<WeatherDTO>.Failure(
           Shared.ViewModelServices.OperationErrorSource.Unknown,
                    "Failed to fetch weather data"));

            var refreshMethod = typeof(WeatherViewModel).GetMethod("RefreshWeatherAsync",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            await (Task)refreshMethod.Invoke(_viewModel, null);

            Assert.IsTrue(_viewModel.IsError);
            Assert.AreEqual("Failed to fetch weather data", _viewModel.ErrorMessage);
        }

        [TestMethod]
        public async Task RefreshWeatherAsync_WhenExceptionOccurs_ShouldSetErrorMessage()
        {
            _viewModel.LocationId = 1;
            _viewModel.IsError = false;
            _viewModel.ErrorMessage = string.Empty;

            var exception = new Exception("Test exception");

            _mockWeatherService
                .Setup(service => service.GetWeatherForLocationAsync(1))
                .ThrowsAsync(exception);

            var refreshMethod = typeof(WeatherViewModel).GetMethod("RefreshWeatherAsync",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            await (Task)refreshMethod.Invoke(_viewModel, null);

            Assert.IsTrue(_viewModel.IsError);
            Assert.IsTrue(_viewModel.ErrorMessage.Contains("Error refreshing weather"));
        }

        [TestMethod]
        public async Task FetchForecastAsync_WithValidLocationId_ShouldUpdateForecast()
        {
            _viewModel.LocationId = 1;
            _viewModel.IsError = false;
            string forecastData = "5-day forecast data";

            _mockWeatherService
                .Setup(service => service.GetForecastForLocationAsync(1, 5))
                .ReturnsAsync(Shared.ViewModelServices.OperationResult<string>.Success(forecastData));

            await _viewModel.FetchForecastAsync(5);

            _mockWeatherService.Verify(service => service.GetForecastForLocationAsync(1, 5), Times.Once);
            Assert.AreEqual(forecastData, _viewModel.Forecast);
        }

        [TestMethod]
        public async Task FetchForecastAsync_WithInvalidLocationId_ShouldSetErrorMessage()
        {
            _viewModel.LocationId = 0;
            _viewModel.IsError = false;
            _viewModel.ErrorMessage = string.Empty;

            await _viewModel.FetchForecastAsync(5);

            Assert.IsTrue(_viewModel.IsError);
            Assert.IsTrue(_viewModel.ErrorMessage.Contains("Invalid location ID"));
        }

        [TestMethod]
        public async Task FetchForecastAsync_WhenWeatherServiceFails_ShouldSetErrorMessage()
        {
            _viewModel.LocationId = 1;
            _viewModel.IsError = false;
            _viewModel.ErrorMessage = string.Empty;

            _mockWeatherService
                .Setup(service => service.GetForecastForLocationAsync(1, 5))
                .ReturnsAsync(Shared.ViewModelServices.OperationResult<string>.Failure(
                  Shared.ViewModelServices.OperationErrorSource.Unknown,
                    "Failed to fetch forecast data"));

            await _viewModel.FetchForecastAsync(5);

            Assert.IsTrue(_viewModel.IsError);
            Assert.AreEqual("Failed to fetch forecast data", _viewModel.ErrorMessage);
        }

        [TestMethod]
        public async Task FetchForecastAsync_WhenExceptionOccurs_ShouldSetErrorMessage()
        {
            _viewModel.LocationId = 1;
            _viewModel.IsError = false;
            _viewModel.ErrorMessage = string.Empty;

            var exception = new Exception("Test exception");

            _mockWeatherService
                .Setup(service => service.GetForecastForLocationAsync(1, 5))
                .ThrowsAsync(exception);

            await _viewModel.FetchForecastAsync(5);

            Assert.IsTrue(_viewModel.IsError);
            Assert.IsTrue(_viewModel.ErrorMessage.Contains("Error fetching forecast"));
        }

        [TestMethod]
        public void GetFormattedTemperature_WithFahrenheit_ShouldReturnCorrectFormat()
        {
            _viewModel.Temperature = 72.5;
            string result = _viewModel.GetFormattedTemperature(true);
            Assert.AreEqual("72.5 °F", result);
        }

        [TestMethod]
        public void GetFormattedTemperature_WithCelsius_ShouldReturnCorrectFormat()
        {
            _viewModel.Temperature = 72.5;
            string result = _viewModel.GetFormattedTemperature(false);
            Assert.AreEqual("72.5 °C", result);
        }

        [TestMethod]
        public void GetFormattedWindSpeed_WithImperial_ShouldReturnCorrectFormat()
        {
            _viewModel.WindSpeed = 5.2;
            string result = _viewModel.GetFormattedWindSpeed(true);
            Assert.AreEqual("5.2 mph", result);
        }

        [TestMethod]
        public void GetFormattedWindSpeed_WithMetric_ShouldReturnCorrectFormat()
        {
            _viewModel.WindSpeed = 5.2;
            string result = _viewModel.GetFormattedWindSpeed(false);
            Assert.AreEqual("5.2 kph", result);
        }

        [TestMethod]
        public void GetFormattedSunriseTime_ShouldReturnCorrectFormat()
        {
            _viewModel.SunriseTime = new DateTime(2023, 1, 1, 6, 30, 0);
            string result = _viewModel.GetFormattedSunriseTime("h:mm tt");
            Assert.AreEqual("6:30 AM", result);
        }

        [TestMethod]
        public void GetFormattedSunsetTime_ShouldReturnCorrectFormat()
        {
            _viewModel.SunsetTime = new DateTime(2023, 1, 1, 20, 15, 0);
            string result = _viewModel.GetFormattedSunsetTime("h:mm tt");
            Assert.AreEqual("8:15 PM", result);
        }
    }
}