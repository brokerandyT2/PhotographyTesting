// LocationServiceCoordinatesTests.cs
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading.Tasks;
using Locations.Core.Business.DataAccess.Services;
using Locations.Core.Business.DataAccess.Interfaces;
using Locations.Core.Data.Models;
using Locations.Core.Data.Queries.Interfaces;
using Locations.Core.Shared.ViewModels;
using Locations.Core.Business.Tests.Base;
using MockFactory = Locations.Core.Business.Tests.TestHelpers.MockFactory;
using TestDataFactory = Locations.Core.Business.Tests.TestHelpers.TestDataFactory;
using Microsoft.Maui;

namespace Locations.Core.Business.Tests.Services.LocationServiceTests
{
    [TestClass]
    [TestCategory("LocationService")]
    public class LocationServiceCoordinatesTests : BaseServiceTests
    {
        private Mock<ILocationRepository> _mockLocationRepository;
        private Mock<IWeatherService<WeatherViewModel>> _mockWeatherService;
        private LocationService<LocationViewModel> _locationService;

        [TestInitialize]
        public override void Setup()
        {
            base.Setup();

            _mockLocationRepository = MockFactory.CreateLocationRepositoryMock();
            _mockWeatherService = new Mock<IWeatherService<WeatherViewModel>>();

            // Setup location service with mocks
            _locationService = new LocationService<LocationViewModel>(
                _mockLocationRepository.Object,
                MockAlertService.Object,
                MockLoggerService.Object,
                _mockWeatherService.Object);
        }

        [TestMethod]
        public void GetLocationByCoordinates_WhenLocationExists_ShouldReturnNonNullResult()
        {
            // Arrange
            double latitude = 40.7128;
            double longitude = -74.0060;
            var testLocation = TestDataFactory.CreateTestLocation();
            testLocation.Lattitude = latitude;
            testLocation.Longitude = longitude;

            _mockLocationRepository.Setup(repo => repo.GetByCoordinatesAsync(latitude, longitude))
                .ReturnsAsync(DataOperationResult<LocationViewModel>.Success(testLocation));

            // Act
            var result = _locationService.GetLocationByCoordinates(latitude, longitude);

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetLocationByCoordinates_WhenLocationExists_ShouldReturnLocationWithCorrectCoordinates()
        {
            // Arrange
            double latitude = 40.7128;
            double longitude = -74.0060;
            var testLocation = TestDataFactory.CreateTestLocation();
            testLocation.Lattitude = latitude;
            testLocation.Longitude = longitude;

            _mockLocationRepository.Setup(repo => repo.GetByCoordinatesAsync(latitude, longitude))
                .ReturnsAsync(DataOperationResult<LocationViewModel>.Success(testLocation));

            // Act
            var result = _locationService.GetLocationByCoordinates(latitude, longitude);

            // Assert
            Assert.AreEqual(latitude, result.Lattitude);
        }

        [TestMethod]
        public void GetLocationByCoordinates_WhenLocationExists_ShouldReturnLocationWithCorrectLongitude()
        {
            // Arrange
            double latitude = 40.7128;
            double longitude = -74.0060;
            var testLocation = TestDataFactory.CreateTestLocation();
            testLocation.Lattitude = latitude;
            testLocation.Longitude = longitude;

            _mockLocationRepository.Setup(repo => repo.GetByCoordinatesAsync(latitude, longitude))
                .ReturnsAsync(DataOperationResult<LocationViewModel>.Success(testLocation));

            // Act
            var result = _locationService.GetLocationByCoordinates(latitude, longitude);

            // Assert
            Assert.AreEqual(longitude, result.Longitude);
        }

        [TestMethod]
        public void GetLocationByCoordinates_WhenLocationExists_ShouldReturnLocationWithCorrectTitle()
        {
            // Arrange
            double latitude = 40.7128;
            double longitude = -74.0060;
            var testLocation = TestDataFactory.CreateTestLocation();
            testLocation.Lattitude = latitude;
            testLocation.Longitude = longitude;
            testLocation.Title = "New York City";

            _mockLocationRepository.Setup(repo => repo.GetByCoordinatesAsync(latitude, longitude))
                .ReturnsAsync(DataOperationResult<LocationViewModel>.Success(testLocation));

            // Act
            var result = _locationService.GetLocationByCoordinates(latitude, longitude);

            // Assert
            Assert.AreEqual("New York City", result.Title);
        }

        [TestMethod]
        public void GetLocationByCoordinates_WhenRepositoryFails_ShouldReturnEmptyLocation()
        {
            // Arrange
            double latitude = 39.7685;
            double longitude = -86.1580;
            string errorMessage = "Database error";

            _mockLocationRepository.Setup(repo => repo.GetByCoordinatesAsync(latitude, longitude))
                .ReturnsAsync(DataOperationResult<LocationViewModel>.Failure(
                    ErrorSource.Database, errorMessage));

            // Act
            var result = _locationService.GetLocationByCoordinates(latitude, longitude);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Id);
        }

        // In your test: Change your verification to not expect an Exception parameter
        [TestMethod]
        public void GetLocationByCoordinates_WhenLocationDoesNotExist_ShouldLogWarning()
        {
            // Arrange
            double latitude = 40.7128;
            double longitude = -74.0060;
            string errorMessage = "Location not found";

            _mockLocationRepository.Setup(repo => repo.GetByCoordinatesAsync(latitude, longitude))
                .ReturnsAsync(DataOperationResult<LocationViewModel>.Failure(
                    ErrorSource.Database, errorMessage));

            // Act
            _locationService.GetLocationByCoordinates(latitude, longitude);

            // Assert
            MockLoggerService.Verify(
                logger => logger.LogWarning(It.IsAny<string>(), null),
                Times.Once);
        }

        [TestMethod]
        public void GetLocationByCoordinates_WhenExceptionOccurs_ShouldReturnEmptyLocation()
        {
            // Arrange
            double latitude = 39.7685;
            double longitude = -86.1580;
            var expectedException = new Exception("Test exception");

            _mockLocationRepository.Setup(repo => repo.GetByCoordinatesAsync(latitude, longitude))
                .ThrowsAsync(expectedException);

            // Act
            var result = _locationService.GetLocationByCoordinates(latitude, longitude);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Id);
        }

        [TestMethod]
        public void GetLocationByCoordinates_WhenExceptionOccurs_ShouldLogError()
        {
            // Arrange
            double latitude = 40.7128;
            double longitude = -74.0060;
            var expectedException = new Exception("Test exception");

            _mockLocationRepository.Setup(repo => repo.GetByCoordinatesAsync(latitude, longitude))
                .ThrowsAsync(expectedException);

            // Act
            _locationService.GetLocationByCoordinates(latitude, longitude);

            // Assert
            MockLoggerService.Verify(
                logger => logger.LogError(It.IsAny<string>(), It.IsAny<Exception>()),
                Times.AtLeastOnce);
        }

        [TestMethod]
        public void GetLocationByCoordinates_ShouldPassCorrectCoordinatesToRepository()
        {
            // Arrange
            double latitude = 40.7128;
            double longitude = -74.0060;
            double capturedLatitude = 0;
            double capturedLongitude = 0;

            _mockLocationRepository.Setup(repo => repo.GetByCoordinatesAsync(It.IsAny<double>(), It.IsAny<double>()))
                .Callback<double, double>((lat, lon) =>
                {
                    capturedLatitude = lat;
                    capturedLongitude = lon;
                })
                .ReturnsAsync(DataOperationResult<LocationViewModel>.Success(new LocationViewModel()));

            // Act
            _locationService.GetLocationByCoordinates(latitude, longitude);

            // Assert
            Assert.AreEqual(latitude, capturedLatitude);
            Assert.AreEqual(longitude, capturedLongitude);
        }
    }
}