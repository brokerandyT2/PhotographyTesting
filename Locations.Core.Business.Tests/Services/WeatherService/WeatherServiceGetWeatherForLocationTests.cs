// WeatherServiceGetWeatherForLocationTests.cs
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Locations.Core.Business.DataAccess.Services;
using Locations.Core.Business.DataAccess.Interfaces;
using Locations.Core.Data.Models;
using Locations.Core.Data.Queries.Interfaces;
using Locations.Core.Shared.ViewModels;
using Location.Core.Helpers.AlertService;
using Location.Core.Helpers.LoggingService;
using Locations.Core.Business.Tests.Base;
using MockFactory = Locations.Core.Business.Tests.TestHelpers.MockFactory;
using TestDataFactory = Locations.Core.Business.Tests.TestHelpers.TestDataFactory;

namespace Locations.Core.Business.Tests.Services.WeatherServiceTests
{
    [TestClass]
    [TestCategory("WeatherService")]
    public class WeatherServiceGetWeatherForLocationTests : BaseServiceTests
    {
        private Mock<IWeatherRepository> _mockWeatherRepository;
        private Mock<ILocationService<LocationViewModel>> _mockLocationService;
        private WeatherService<WeatherViewModel> _weatherService;

        [TestInitialize]
        public override void Setup()
        {
            base.Setup();

            _mockWeatherRepository = MockFactory.CreateWeatherRepositoryMock();
            _mockLocationService = new Mock<ILocationService<LocationViewModel>>();

            // Setup weather service with mocks
            _weatherService = new WeatherService<WeatherViewModel>(
                _mockWeatherRepository.Object,
                MockAlertService.Object,
                MockBusinessLoggerService.Object,
                _mockLocationService.Object);
        }

        [TestMethod]
        public async Task GetWeatherForLocationAsync_ShouldReturnNonNullResult()
        {
            // Arrange
            int locationId = 1;
            var testWeather = TestDataFactory.CreateTestWeather(locationId);

            _mockWeatherRepository.Setup(repo => repo.GetByLocationIdAsync(locationId))
                .ReturnsAsync(DataOperationResult<WeatherViewModel>.Success(testWeather));

            // Act
            var result = await _weatherService.GetWeatherForLocationAsync(locationId);

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task GetWeatherForLocationAsync_WhenWeatherExists_ShouldReturnSuccessResult()
        {
            // Arrange
            int locationId = 1;
            var testWeather = TestDataFactory.CreateTestWeather(locationId);

            _mockWeatherRepository.Setup(repo => repo.GetByLocationIdAsync(locationId))
                .ReturnsAsync(DataOperationResult<WeatherViewModel>.Success(testWeather));

            // Act
            var result = await _weatherService.GetWeatherForLocationAsync(locationId);

            // Assert
            Assert.IsTrue(result.IsSuccess);
        }

        [TestMethod]
        public async Task GetWeatherForLocationAsync_WhenWeatherExists_ShouldReturnCorrectWeatherData()
        {
            // Arrange
            int locationId = 1;
            var testWeather = TestDataFactory.CreateTestWeather(locationId);

            _mockWeatherRepository.Setup(repo => repo.GetByLocationIdAsync(locationId))
                .ReturnsAsync(DataOperationResult<WeatherViewModel>.Success(testWeather));

            // Act
            var result = await _weatherService.GetWeatherForLocationAsync(locationId);

            // Assert
            Assert.AreEqual(locationId, result.Data.LocationId);
        }

        [TestMethod]
        public async Task GetWeatherForLocationAsync_WhenWeatherDoesNotExist_ShouldReturnFailureResult()
        {
            // Arrange
            int locationId = 999;
            string errorMessage = "Weather not found";

            _mockWeatherRepository.Setup(repo => repo.GetByLocationIdAsync(locationId))
                .ReturnsAsync(DataOperationResult<WeatherViewModel>.Failure(
                    ErrorSource.Database, errorMessage));

            // Act
            var result = await _weatherService.GetWeatherForLocationAsync(locationId);

            // Assert
            Assert.IsFalse(result.IsSuccess);
        }

        [TestMethod]
        public async Task GetWeatherForLocationAsync_WhenExceptionOccurs_ShouldReturnFailureResult()
        {
            // Arrange
            int locationId = 1;
            var expectedException = new Exception("Test exception");

            _mockWeatherRepository.Setup(repo => repo.GetByLocationIdAsync(locationId))
                .ThrowsAsync(expectedException);

            // Act
            var result = await _weatherService.GetWeatherForLocationAsync(locationId);

            // Assert
            Assert.IsFalse(result.IsSuccess);
        }

        [TestMethod]
        public async Task GetWeatherForLocationAsync_WhenExceptionOccurs_ShouldLogError()
        {
            // Arrange
            int locationId = 1;
            var expectedException = new Exception("Test exception");

            _mockWeatherRepository.Setup(repo => repo.GetByLocationIdAsync(locationId))
                .ThrowsAsync(expectedException);

            // Act
            await _weatherService.GetWeatherForLocationAsync(locationId);

            // Assert
            MockBusinessLoggerService.Verify(
                logger => logger.LogError(It.IsAny<string>(), expectedException),
                Times.AtLeastOnce);
        }

        [TestMethod]
        public async Task GetWeatherForLocationAsync_ShouldPassCorrectLocationIdToRepository()
        {
            // Arrange
            int locationId = 1;
            int capturedLocationId = 0;

            _mockWeatherRepository.Setup(repo => repo.GetByLocationIdAsync(It.IsAny<int>()))
                .Callback<int>(id => capturedLocationId = id)
                .ReturnsAsync(DataOperationResult<WeatherViewModel>.Success(new WeatherViewModel()));

            // Act
            await _weatherService.GetWeatherForLocationAsync(locationId);

            // Assert
            Assert.AreEqual(locationId, capturedLocationId);
        }
    }
}