// LocationServiceGetByIdTests.cs
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading.Tasks;
using Locations.Core.Business.DataAccess.Services;
using Locations.Core.Business.DataAccess.Interfaces;
using Locations.Core.Data.Models;
using Locations.Core.Shared.ViewModels;
using Locations.Core.Business.Tests.Base;
using Locations.Core.Business.Tests.TestHelpers;
using Locations.Core.Data.Queries.Interfaces;
using MockFactory = Locations.Core.Business.Tests.TestHelpers.MockFactory;

namespace Locations.Core.Business.Tests.Services.LocationServiceTests
{
    [TestClass]
    [TestCategory("LocationService")]
    public class LocationServiceGetByIdTests : BaseServiceTests
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
        public async Task GetByIdAsync_WhenLocationExists_ShouldReturnSuccessResult()
        {
            // Arrange
            int locationId = 1;
            var testLocation = TestDataFactory.CreateTestLocation(locationId);

            _mockLocationRepository.Setup(repo => repo.GetByIdAsync(locationId))
                .ReturnsAsync(DataOperationResult<LocationViewModel>.Success(testLocation));

            // Act
            var result = await _locationService.GetByIdAsync(locationId);

            // Assert
            Assert.IsTrue(result.IsSuccess);
        }

        [TestMethod]
        public async Task GetByIdAsync_WhenLocationExists_ShouldReturnNonNullData()
        {
            // Arrange
            int locationId = 1;
            var testLocation = TestDataFactory.CreateTestLocation(locationId);

            _mockLocationRepository.Setup(repo => repo.GetByIdAsync(locationId))
                .ReturnsAsync(DataOperationResult<LocationViewModel>.Success(testLocation));

            // Act
            var result = await _locationService.GetByIdAsync(locationId);

            // Assert
            Assert.IsNotNull(result.Data);
        }

        [TestMethod]
        public async Task GetByIdAsync_WhenLocationExists_ShouldReturnLocationWithCorrectId()
        {
            // Arrange
            int locationId = 1;
            var testLocation = TestDataFactory.CreateTestLocation(locationId);

            _mockLocationRepository.Setup(repo => repo.GetByIdAsync(locationId))
                .ReturnsAsync(DataOperationResult<LocationViewModel>.Success(testLocation));

            // Act
            var result = await _locationService.GetByIdAsync(locationId);

            // Assert
            Assert.AreEqual(locationId, result.Data.Id);
        }

        [TestMethod]
        public async Task GetByIdAsync_WhenLocationExists_ShouldReturnLocationWithCorrectTitle()
        {
            // Arrange
            int locationId = 1;
            var testLocation = TestDataFactory.CreateTestLocation(locationId);

            _mockLocationRepository.Setup(repo => repo.GetByIdAsync(locationId))
                .ReturnsAsync(DataOperationResult<LocationViewModel>.Success(testLocation));

            // Act
            var result = await _locationService.GetByIdAsync(locationId);

            // Assert
            Assert.AreEqual(testLocation.Title, result.Data.Title);
        }

        [TestMethod]
        public async Task GetByIdAsync_WhenLocationDoesNotExist_ShouldReturnFailureResult()
        {
            // Arrange
            int locationId = 999;
            string errorMessage = "Location not found";

            _mockLocationRepository.Setup(repo => repo.GetByIdAsync(locationId))
                .ReturnsAsync(DataOperationResult<LocationViewModel>.Failure(
                    ErrorSource.Database, errorMessage));

            // Act
            var result = await _locationService.GetByIdAsync(locationId);

            // Assert
            Assert.IsFalse(result.IsSuccess);
        }

        [TestMethod]
        public async Task GetByIdAsync_WhenLocationDoesNotExist_ShouldReturnNullData()
        {
            // Arrange
            int locationId = 999;
            string errorMessage = "Location not found";

            _mockLocationRepository.Setup(repo => repo.GetByIdAsync(locationId))
                .ReturnsAsync(DataOperationResult<LocationViewModel>.Failure(
                    ErrorSource.Database, errorMessage));

            // Act
            var result = await _locationService.GetByIdAsync(locationId);

            // Assert
            Assert.IsNull(result.Data);
        }

        [TestMethod]
        public async Task GetByIdAsync_WhenLocationDoesNotExist_ShouldReturnCorrectErrorSource()
        {
            // Arrange
            int locationId = 999;
            string errorMessage = "Location not found";

            _mockLocationRepository.Setup(repo => repo.GetByIdAsync(locationId))
                .ReturnsAsync(DataOperationResult<LocationViewModel>.Failure(
                    ErrorSource.Database, errorMessage));

            // Act
            var result = await _locationService.GetByIdAsync(locationId);

            // Assert
            Assert.AreEqual(ErrorSource.Database, result.ErrorSource);
        }

        [TestMethod]
        public async Task GetByIdAsync_WhenExceptionOccurs_ShouldReturnFailureResult()
        {
            // Arrange
            int locationId = 1;
            var expectedException = new Exception("Test exception");

            _mockLocationRepository.Setup(repo => repo.GetByIdAsync(locationId))
                .ThrowsAsync(expectedException);

            // Act
            var result = await _locationService.GetByIdAsync(locationId);

            // Assert
            Assert.IsFalse(result.IsSuccess);
        }

        [TestMethod]
        public async Task GetByIdAsync_WhenExceptionOccurs_ShouldLogError()
        {
            // Arrange
            int locationId = 1;
            var expectedException = new Exception("Test exception");

            _mockLocationRepository.Setup(repo => repo.GetByIdAsync(locationId))
                .ThrowsAsync(expectedException);

            // Act
            await _locationService.GetByIdAsync(locationId);

            // Assert
            MockBusinessLoggerService.Verify(logger => logger.LogError(It.IsAny<string>(), expectedException), Times.Exactly(2));
        }
    }
}