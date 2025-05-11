// WeatherViewModelTests.cs
using Locations.Core.Shared.ViewModels;
using Locations.Core.Shared.ViewModelServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading.Tasks;
using OperationResult = Locations.Core.Shared.ViewModelServices.OperationResult<Locations.Core.Shared.ViewModels.WeatherViewModel>;

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
            // Arrange & Act
            var viewModel = new WeatherViewModel();

            // Assert
            Assert.IsNotNull(viewModel.RefreshWeatherCommand);
        }

        [TestMethod]
        public void Constructor_WithWeatherService_ShouldInitializePropertiesAndService()
        {
            // Assert
            Assert.IsNotNull(_viewModel.RefreshWeatherCommand);
        }

        [TestMethod]
        public void InitializeFromDTO_WithValidDTO_ShouldInitializeProperties()
        {
            // Arrange
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

            // Act
            _viewModel.InitializeFromDTO(dto);

            // Assert
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
            // Act & Assert - Should not throw
            _viewModel.InitializeFromDTO(null);
        }

        [TestMethod]
        public async Task RefreshWeatherAsync_WithValidLocationId_ShouldCallWeatherService()
        {
            // Arrange
            _viewModel.LocationId = 1;

            var weatherData = new WeatherViewModel
            {
                Id = 1,
                LocationId = 1,
                Temperature = 72.5,
                Description = "Partly Cloudy",
                LastUpdate = DateTime.Now
            };

            var result = new OperationResult<WeatherViewModel>(true, weatherData);

            _mockWeatherService.Setup(service => service.GetWeatherForLocationAsync(1))
                .ReturnsAsync(result);

            // Act
            await _viewModel.RefreshWeatherCommand.ExecuteAsync(null);

            // Assert
            _mockWeatherService.Verify(service => service.GetWeatherForLocationAsync(1), Times.Once);
            Assert.AreEqual(72.5, _viewModel.Temperature);
            Assert.AreEqual("Partly Cloudy", _viewModel.Description);
        }

        [TestMethod]
        public async Task RefreshWeatherAsync_WithInvalidLocationId_ShouldSetErrorMessage()
        {
            // Arrange
            _viewModel.LocationId = 0; // Invalid ID

            // Act
            await _viewModel.RefreshWeatherCommand.ExecuteAsync(null);

            // Assert
            Assert.IsTrue(_viewModel.IsError);
            Assert.IsTrue(_viewModel.ErrorMessage.Contains("Invalid location ID"));
        }

        [TestMethod]
        public async Task RefreshWeatherAsync_WhenWeatherServiceFails_ShouldSetErrorMessage()
        {
            // Arrange
            _viewModel.LocationId = 1;

            var result = new OperationResult<WeatherViewModel>(false, null, "Failed to fetch weather data");

            _mockWeatherService.Setup(service => service.GetWeatherForLocationAsync(1))
                .ReturnsAsync(result);

            // Act
            await _viewModel.RefreshWeatherCommand.ExecuteAsync(null);

            // Assert
            Assert.IsTrue(_viewModel.IsError);
            Assert.AreEqual("Failed to fetch weather data", _viewModel.ErrorMessage);
        }

        // WeatherViewModelTests.cs (continued)
        [TestMethod]
        public async Task RefreshWeatherAsync_WhenExceptionOccurs_ShouldSetErrorMessage()
        {
            // Arrange
            _viewModel.LocationId = 1;

            var exception = new Exception("Test exception");

            _mockWeatherService.Setup(service => service.GetWeatherForLocationAsync(1))
                .ThrowsAsync(exception);

            // Act
            await _viewModel.RefreshWeatherCommand.ExecuteAsync(null);

            // Assert
            Assert.IsTrue(_viewModel.IsError);
            Assert.IsTrue(_viewModel.ErrorMessage.Contains("Error refreshing weather"));
        }

        [TestMethod]
        public async Task FetchForecastAsync_WithValidLocationId_ShouldUpdateForecast()
        {
            // Arrange
            _viewModel.LocationId = 1;
            string forecastData = "5-day forecast data";

            var result = new OperationResult<string>(true, forecastData);

            _mockWeatherService.Setup(service => service.GetForecastForLocationAsync(1, 5))
                .ReturnsAsync(result);

            // Act
            await _viewModel.FetchForecastAsync(5);

            // Assert
            _mockWeatherService.Verify(service => service.GetForecastForLocationAsync(1, 5), Times.Once);
            Assert.AreEqual(forecastData, _viewModel.Forecast);
        }

        [TestMethod]
        public async Task FetchForecastAsync_WithInvalidLocationId_ShouldSetErrorMessage()
        {
            // Arrange
            _viewModel.LocationId = 0; // Invalid ID

            // Act
            await _viewModel.FetchForecastAsync(5);

            // Assert
            Assert.IsTrue(_viewModel.IsError);
            Assert.IsTrue(_viewModel.ErrorMessage.Contains("Invalid location ID"));
        }

        [TestMethod]
        public async Task FetchForecastAsync_WhenWeatherServiceFails_ShouldSetErrorMessage()
        {
            // Arrange
            _viewModel.LocationId = 1;

            var result = new OperationResult<string>(false, null, "Failed to fetch forecast data");

            _mockWeatherService.Setup(service => service.GetForecastForLocationAsync(1, 5))
                .ReturnsAsync(result);

            // Act
            await _viewModel.FetchForecastAsync(5);

            // Assert
            Assert.IsTrue(_viewModel.IsError);
            Assert.AreEqual("Failed to fetch forecast data", _viewModel.ErrorMessage);
        }

        [TestMethod]
        public async Task FetchForecastAsync_WhenExceptionOccurs_ShouldSetErrorMessage()
        {
            // Arrange
            _viewModel.LocationId = 1;

            var exception = new Exception("Test exception");

            _mockWeatherService.Setup(service => service.GetForecastForLocationAsync(1, 5))
                .ThrowsAsync(exception);

            // Act
            await _viewModel.FetchForecastAsync(5);

            // Assert
            Assert.IsTrue(_viewModel.IsError);
            Assert.IsTrue(_viewModel.ErrorMessage.Contains("Error fetching forecast"));
        }

        [TestMethod]
        public void GetFormattedTemperature_WithFahrenheit_ShouldReturnCorrectFormat()
        {
            // Arrange
            _viewModel.Temperature = 72.5;

            // Act
            string result = _viewModel.GetFormattedTemperature(true);

            // Assert
            Assert.AreEqual("72.5 °F", result);
        }

        [TestMethod]
        public void GetFormattedTemperature_WithCelsius_ShouldReturnCorrectFormat()
        {
            // Arrange
            _viewModel.Temperature = 72.5;

            // Act
            string result = _viewModel.GetFormattedTemperature(false);

            // Assert
            Assert.AreEqual("72.5 °C", result);
        }

        [TestMethod]
        public void GetFormattedWindSpeed_WithImperial_ShouldReturnCorrectFormat()
        {
            // Arrange
            _viewModel.WindSpeed = 5.2;

            // Act
            string result = _viewModel.GetFormattedWindSpeed(true);

            // Assert
            Assert.AreEqual("5.2 mph", result);
        }

        [TestMethod]
        public void GetFormattedWindSpeed_WithMetric_ShouldReturnCorrectFormat()
        {
            // Arrange
            _viewModel.WindSpeed = 5.2;

            // Act
            string result = _viewModel.GetFormattedWindSpeed(false);

            // Assert
            Assert.AreEqual("5.2 kph", result);
        }

        [TestMethod]
        public void GetFormattedSunriseTime_ShouldReturnCorrectFormat()
        {
            // Arrange
            _viewModel.SunriseTime = new DateTime(2023, 1, 1, 6, 30, 0);

            // Act
            string result = _viewModel.GetFormattedSunriseTime("h:mm tt");

            // Assert
            Assert.AreEqual("6:30 AM", result);
        }

        [TestMethod]
        public void GetFormattedSunsetTime_ShouldReturnCorrectFormat()
        {
            // Arrange
            _viewModel.SunsetTime = new DateTime(2023, 1, 1, 20, 15, 0);

            // Act
            string result = _viewModel.GetFormattedSunsetTime("h:mm tt");

            // Assert
            Assert.AreEqual("8:15 PM", result);
        }
    }
}