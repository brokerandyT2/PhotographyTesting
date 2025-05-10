// LocationServiceNearbyTests.cs
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Locations.Core.Business.DataAccess.Services;
using Locations.Core.Business.DataAccess.Interfaces;
using Locations.Core.Data.Models;
using Locations.Core.Data.Queries.Interfaces;
using Locations.Core.Shared.ViewModels;
using Locations.Core.Business.Tests.Base;
using MockFactory = Locations.Core.Business.Tests.TestHelpers.MockFactory;
using TestDataFactory = Locations.Core.Business.Tests.TestHelpers.TestDataFactory;

namespace Locations.Core.Business.Tests.Services.LocationServiceTests
{
    [TestClass]
    [TestCategory("LocationService")]
    public class LocationServiceNearbyTests : BaseServiceTests
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
                MockBusinessLoggerService.Object,
                _mockWeatherService.Object);
        }

        [TestMethod]
        public async Task GetNearbyLocationsAsync_ShouldReturnNonNullResult()
        {
            // Arrange
            double latitude = 40.7128;
            double longitude = -74.0060;
            double radiusKm = 10;
            var testLocations = TestDataFactory.CreateTestLocations(3);

            // Set up locations with varying distances
            testLocations[0].Lattitude = latitude + 0.01; // Within radius
            testLocations[0].Longitude = longitude + 0.01;
            testLocations[1].Lattitude = latitude + 0.1; // Outside radius
            testLocations[1].Longitude = longitude + 0.1;
            testLocations[2].Lattitude = latitude - 0.01; // Within radius
            testLocations[2].Longitude = longitude - 0.01;

            _mockLocationRepository.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(DataOperationResult<IList<LocationViewModel>>.Success(testLocations));

            // Act
            var result = await _locationService.GetNearbyLocationsAsync(latitude, longitude, radiusKm);

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task GetNearbyLocationsAsync_ShouldFilterLocationsByDistance()
        {
            // Arrange
            double latitude = 40.7128;
            double longitude = -74.0060;
            double radiusKm = 2;
            var testLocations = TestDataFactory.CreateTestLocations(3);

            // Set up locations with varying distances
            testLocations[0].Lattitude = latitude + 0.01; // Within radius (roughly 1.1 km)
            testLocations[0].Longitude = longitude;
            testLocations[1].Lattitude = latitude + 0.1; // Outside radius (roughly 11 km)
            testLocations[1].Longitude = longitude;
            testLocations[2].Lattitude = latitude - 0.01; // Within radius (roughly 1.1 km)
            testLocations[2].Longitude = longitude;

            _mockLocationRepository.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(DataOperationResult<IList<LocationViewModel>>.Success(testLocations));

            // Act
            var result = await _locationService.GetNearbyLocationsAsync(latitude, longitude, radiusKm);

            // Assert
            Assert.AreEqual(2, result.Count);
        }

        [TestMethod]
        public async Task GetNearbyLocationsAsync_WithNoNearbyLocations_ShouldReturnEmptyList()
        {
            // Arrange
            double latitude = 40.7128;
            double longitude = -74.0060;
            double radiusKm = 1;
            var testLocations = TestDataFactory.CreateTestLocations(3);

            // Set up all locations outside the radius
            testLocations[0].Lattitude = latitude + 0.1; // Roughly 11 km away
            testLocations[0].Longitude = longitude;
            testLocations[1].Lattitude = latitude - 0.1; // Roughly 11 km away
            testLocations[1].Longitude = longitude;
            testLocations[2].Lattitude = latitude;
            testLocations[2].Longitude = longitude + 0.1; // Roughly 8 km away

            _mockLocationRepository.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(DataOperationResult<IList<LocationViewModel>>.Success(testLocations));

            // Act
            var result = await _locationService.GetNearbyLocationsAsync(latitude, longitude, radiusKm);

            // Assert
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public async Task GetNearbyLocationsAsync_WhenRepositoryFails_ShouldReturnEmptyList()
        {
            // Arrange
            double latitude = 40.7128;
            double longitude = -74.0060;
            double radiusKm = 10;
            string errorMessage = "Database error";

            _mockLocationRepository.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(DataOperationResult<IList<LocationViewModel>>.Failure(
                    ErrorSource.Database, errorMessage));

            // Act
            var result = await _locationService.GetNearbyLocationsAsync(latitude, longitude, radiusKm);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public async Task GetNearbyLocationsAsync_WhenExceptionOccurs_ShouldReturnEmptyList()
        {
            // Arrange
            double latitude = 40.7128;
            double longitude = -74.0060;
            double radiusKm = 10;
            var expectedException = new Exception("Test exception");

            _mockLocationRepository.Setup(repo => repo.GetAllAsync())
                .ThrowsAsync(expectedException);

            // Act
            var result = await _locationService.GetNearbyLocationsAsync(latitude, longitude, radiusKm);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public async Task GetNearbyLocationsAsync_WhenExceptionOccurs_ShouldLogError()
        {
            // Arrange
            double latitude = 40.7128;
            double longitude = -74.0060;
            double radiusKm = 10;
            var expectedException = new Exception("Test exception");

            _mockLocationRepository.Setup(repo => repo.GetAllAsync())
                .ThrowsAsync(expectedException);

            // Act
            await _locationService.GetNearbyLocationsAsync(latitude, longitude, radiusKm);

            // Assert
            MockBusinessLoggerService.Verify(
                logger => logger.LogError(It.IsAny<string>(), expectedException),
                Times.Once);
        }

        [TestMethod]
        public async Task GetNearbyLocationsAsync_WithDefaultRadius_ShouldUseDefaultRadius()
        {
            // Arrange
            double latitude = 40.7128;
            double longitude = -74.0060;
            var testLocations = TestDataFactory.CreateTestLocations(3);

            // Set up locations with varying distances
            testLocations[0].Lattitude = latitude + 0.01; // Within default radius
            testLocations[0].Longitude = longitude;
            testLocations[1].Lattitude = latitude + 0.1; // Outside default radius
            testLocations[1].Longitude = longitude;
            testLocations[2].Lattitude = latitude - 0.01; // Within default radius
            testLocations[2].Longitude = longitude;

            _mockLocationRepository.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(DataOperationResult<IList<LocationViewModel>>.Success(testLocations));

            // Act - using default radius
            var result = await _locationService.GetNearbyLocationsAsync(latitude, longitude);

            // Assert
            Assert.IsTrue(result.Count > 0); // At least some locations should be within default radius
        }

        [TestMethod]
        public async Task GetNearbyLocationsAsync_WithLargeRadius_ShouldIncludeAllLocations()
        {
            // Arrange
            double latitude = 40.7128;
            double longitude = -74.0060;
            double radiusKm = 1000; // Very large radius
            var testLocations = TestDataFactory.CreateTestLocations(3);

            _mockLocationRepository.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(DataOperationResult<IList<LocationViewModel>>.Success(testLocations));

            // Act
            var result = await _locationService.GetNearbyLocationsAsync(latitude, longitude, radiusKm);

            // Assert
            Assert.AreEqual(testLocations.Count, result.Count);
        }

        [TestMethod]
        public async Task GetNearbyLocationsAsync_WithZeroRadius_ShouldReturnNoLocations()
        {
            // Arrange
            double latitude = 40.7128;
            double longitude = -74.0060;
            double radiusKm = 0; // Zero radius
            var testLocations = TestDataFactory.CreateTestLocations(3);

            _mockLocationRepository.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(DataOperationResult<IList<LocationViewModel>>.Success(testLocations));

            // Act
            var result = await _locationService.GetNearbyLocationsAsync(latitude, longitude, radiusKm);

            // Assert
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public async Task GetNearbyLocationsAsync_WithNegativeRadius_ShouldReturnNoLocations()
        {
            // Arrange
            double latitude = 40.7128;
            double longitude = -74.0060;
            double radiusKm = -10; // Negative radius
            var testLocations = TestDataFactory.CreateTestLocations(3);

            _mockLocationRepository.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(DataOperationResult<IList<LocationViewModel>>.Success(testLocations));

            // Act
            var result = await _locationService.GetNearbyLocationsAsync(latitude, longitude, radiusKm);

            // Assert
            Assert.AreEqual(0, result.Count);
        }
    }
}