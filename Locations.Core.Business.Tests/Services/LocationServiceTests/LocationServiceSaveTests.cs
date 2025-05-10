// LocationServiceSaveTests.cs
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

namespace Locations.Core.Business.Tests.Services.LocationServiceTests
{
    [TestClass]
    [TestCategory("LocationService")]
    public class LocationServiceSaveTests : BaseServiceTests
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
        public async Task SaveAsync_WithValidLocation_ShouldReturnSuccessResult()
        {
            // Arrange
            var testLocation = TestDataFactory.CreateTestLocation();
            testLocation.Id = 0; // New location

            var savedLocation = TestDataFactory.CreateTestLocation();
            savedLocation.Id = 1; // Location with ID assigned

            _mockLocationRepository.Setup(repo => repo.SaveAsync(It.IsAny<LocationViewModel>()))
                .ReturnsAsync(DataOperationResult<LocationViewModel>.Success(savedLocation));

            // Act
            var result = await _locationService.SaveAsync(testLocation);

            // Assert
            Assert.IsTrue(result.IsSuccess);
        }

        [TestMethod]
        public async Task SaveAsync_WithValidLocation_ShouldReturnNonNullData()
        {
            // Arrange
            var testLocation = TestDataFactory.CreateTestLocation();
            testLocation.Id = 0; // New location

            var savedLocation = TestDataFactory.CreateTestLocation();
            savedLocation.Id = 1; // Location with ID assigned

            _mockLocationRepository.Setup(repo => repo.SaveAsync(It.IsAny<LocationViewModel>()))
                .ReturnsAsync(DataOperationResult<LocationViewModel>.Success(savedLocation));

            // Act
            var result = await _locationService.SaveAsync(testLocation);

            // Assert
            Assert.IsNotNull(result.Data);
        }

        [TestMethod]
        public async Task SaveAsync_WithValidLocation_ShouldReturnLocationWithAssignedId()
        {
            // Arrange
            var testLocation = TestDataFactory.CreateTestLocation();
            testLocation.Id = 0; // New location

            var savedLocation = TestDataFactory.CreateTestLocation();
            savedLocation.Id = 1; // Location with ID assigned

            _mockLocationRepository.Setup(repo => repo.SaveAsync(It.IsAny<LocationViewModel>()))
                .ReturnsAsync(DataOperationResult<LocationViewModel>.Success(savedLocation));

            // Act
            var result = await _locationService.SaveAsync(testLocation);

            // Assert
            Assert.AreEqual(1, result.Data.Id);
        }

        [TestMethod]
        public async Task SaveAsync_ShouldPassCorrectLocationToRepository()
        {
            // Arrange
            var testLocation = TestDataFactory.CreateTestLocation();
            testLocation.Id = 0; // New location
            testLocation.Title = "Test Save Location";

            var savedLocation = TestDataFactory.CreateTestLocation();
            savedLocation.Id = 1; // Location with ID assigned

            LocationViewModel capturedLocation = null;

            _mockLocationRepository.Setup(repo => repo.SaveAsync(It.IsAny<LocationViewModel>()))
                .Callback<LocationViewModel>(location => capturedLocation = location)
                .ReturnsAsync(DataOperationResult<LocationViewModel>.Success(savedLocation));

            // Act
            await _locationService.SaveAsync(testLocation);

            // Assert
            Assert.IsNotNull(capturedLocation);
            Assert.AreEqual(testLocation.Title, capturedLocation.Title);
        }

        [TestMethod]
        public async Task SaveAsync_WithNullLocation_ShouldReturnFailureResult()
        {
            // Act
            var result = await _locationService.SaveAsync(null);

            // Assert
            Assert.IsFalse(result.IsSuccess);
        }

        [TestMethod]
        public async Task SaveAsync_WithNullLocation_ShouldReturnErrorSourceUnknown()
        {
            // Act
            var result = await _locationService.SaveAsync(null);

            // Assert
            Assert.AreEqual(ErrorSource.Unknown, result.ErrorSource);
        }

        [TestMethod]
        public async Task SaveAsync_WithNullLocation_ShouldReturnNullData()
        {
            // Act
            var result = await _locationService.SaveAsync(null);

            // Assert
            Assert.IsNull(result.Data);
        }

        [TestMethod]
        public async Task SaveAsync_WhenRepositoryFails_ShouldReturnFailureResult()
        {
            // Arrange
            var testLocation = TestDataFactory.CreateTestLocation();
            testLocation.Id = 0; // New location

            string errorMessage = "Database error";

            _mockLocationRepository.Setup(repo => repo.SaveAsync(It.IsAny<LocationViewModel>()))
                .ReturnsAsync(DataOperationResult<LocationViewModel>.Failure(
                    ErrorSource.Database, errorMessage));

            // Act
            var result = await _locationService.SaveAsync(testLocation);

            // Assert
            Assert.IsFalse(result.IsSuccess);
        }

        [TestMethod]
        public async Task SaveAsync_WhenRepositoryFails_ShouldReturnCorrectErrorSource()
        {
            // Arrange
            var testLocation = TestDataFactory.CreateTestLocation();
            testLocation.Id = 0; // New location

            string errorMessage = "Database error";

            _mockLocationRepository.Setup(repo => repo.SaveAsync(It.IsAny<LocationViewModel>()))
                .ReturnsAsync(DataOperationResult<LocationViewModel>.Failure(
                    ErrorSource.Database, errorMessage));

            // Act
            var result = await _locationService.SaveAsync(testLocation);

            // Assert
            Assert.AreEqual(ErrorSource.Database, result.ErrorSource);
        }

        [TestMethod]
        public async Task SaveAsync_WhenExceptionOccurs_ShouldReturnFailureResult()
        {
            // Arrange
            var testLocation = TestDataFactory.CreateTestLocation();
            testLocation.Id = 0; // New location

            var expectedException = new Exception("Test exception");

            _mockLocationRepository.Setup(repo => repo.SaveAsync(It.IsAny<LocationViewModel>()))
                .ThrowsAsync(expectedException);

            // Act
            var result = await _locationService.SaveAsync(testLocation);

            // Assert
            Assert.IsFalse(result.IsSuccess);
        }

        [TestMethod]
        public async Task SaveAsync_WhenExceptionOccurs_ShouldLogError()
        {
            // Arrange
            var testLocation = TestDataFactory.CreateTestLocation();
            testLocation.Id = 0; // New location

            var expectedException = new Exception("Test exception");

            _mockLocationRepository.Setup(repo => repo.SaveAsync(It.IsAny<LocationViewModel>()))
                .ThrowsAsync(expectedException);

            // Act
            await _locationService.SaveAsync(testLocation);

            // Assert
            MockBusinessLoggerService.Verify(logger => logger.LogError(It.IsAny<string>(), expectedException),                Times.Once);
        }
    }
}