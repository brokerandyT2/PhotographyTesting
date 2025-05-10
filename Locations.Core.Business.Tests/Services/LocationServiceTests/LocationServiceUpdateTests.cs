// LocationServiceUpdateTests.cs
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
    public class LocationServiceUpdateTests : BaseServiceTests
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
        public async Task UpdateAsync_WithValidLocation_ShouldReturnSuccessResult()
        {
            // Arrange
            var testLocation = TestDataFactory.CreateTestLocation();
            testLocation.Title = "Updated Title";

            _mockLocationRepository.Setup(repo => repo.UpdateAsync(It.IsAny<LocationViewModel>()))
                .ReturnsAsync(DataOperationResult<bool>.Success(true));

            // Act
            var result = await _locationService.UpdateAsync(testLocation);

            // Assert
            Assert.IsTrue(result.IsSuccess);
        }

        [TestMethod]
        public async Task UpdateAsync_WithValidLocation_ShouldReturnTrueData()
        {
            // Arrange
            var testLocation = TestDataFactory.CreateTestLocation();
            testLocation.Title = "Updated Title";

            _mockLocationRepository.Setup(repo => repo.UpdateAsync(It.IsAny<LocationViewModel>()))
                .ReturnsAsync(DataOperationResult<bool>.Success(true));

            // Act
            var result = await _locationService.UpdateAsync(testLocation);

            // Assert
            Assert.IsTrue(result.Data);
        }

        [TestMethod]
        public async Task UpdateAsync_ShouldPassCorrectLocationToRepository()
        {
            // Arrange
            var testLocation = TestDataFactory.CreateTestLocation();
            testLocation.Title = "Updated Title";

            LocationViewModel capturedLocation = null;

            _mockLocationRepository.Setup(repo => repo.UpdateAsync(It.IsAny<LocationViewModel>()))
                .Callback<LocationViewModel>(location => capturedLocation = location)
                .ReturnsAsync(DataOperationResult<bool>.Success(true));

            // Act
            await _locationService.UpdateAsync(testLocation);

            // Assert
            Assert.IsNotNull(capturedLocation);
            Assert.AreEqual(testLocation.Title, capturedLocation.Title);
        }

        [TestMethod]
        public async Task UpdateAsync_WithNullLocation_ShouldReturnFailureResult()
        {
            // Act
            var result = await _locationService.UpdateAsync(null);

            // Assert
            Assert.IsFalse(result.IsSuccess);
        }

        [TestMethod]
        public async Task UpdateAsync_WithNullLocation_ShouldReturnErrorSourceUnknown()
        {
            // Act
            var result = await _locationService.UpdateAsync(null);

            // Assert
            Assert.AreEqual(ErrorSource.Unknown, result.ErrorSource);
        }

        [TestMethod]
        public async Task UpdateAsync_WithNullLocation_ShouldReturnFalseData()
        {
            // Act
            var result = await _locationService.UpdateAsync(null);

            // Assert
            Assert.IsFalse(result.Data);
        }

        [TestMethod]
        public async Task UpdateAsync_WhenRepositoryFails_ShouldReturnFailureResult()
        {
            // Arrange
            var testLocation = TestDataFactory.CreateTestLocation();
            testLocation.Title = "Updated Title";

            string errorMessage = "Database error";

            _mockLocationRepository.Setup(repo => repo.UpdateAsync(It.IsAny<LocationViewModel>()))
                .ReturnsAsync(DataOperationResult<bool>.Failure(
                    ErrorSource.Database, errorMessage));

            // Act
            var result = await _locationService.UpdateAsync(testLocation);

            // Assert
            Assert.IsFalse(result.IsSuccess);
        }

        [TestMethod]
        public async Task UpdateAsync_WhenRepositoryFails_ShouldReturnCorrectErrorSource()
        {
            // Arrange
            var testLocation = TestDataFactory.CreateTestLocation();
            testLocation.Title = "Updated Title";

            string errorMessage = "Database error";

            _mockLocationRepository.Setup(repo => repo.UpdateAsync(It.IsAny<LocationViewModel>()))
                .ReturnsAsync(DataOperationResult<bool>.Failure(
                    ErrorSource.Database, errorMessage));

            // Act
            var result = await _locationService.UpdateAsync(testLocation);

            // Assert
            Assert.AreEqual(ErrorSource.Database, result.ErrorSource);
        }

        [TestMethod]
        public async Task UpdateAsync_WhenRepositoryFails_ShouldReturnFalseData()
        {
            // Arrange
            var testLocation = TestDataFactory.CreateTestLocation();
            testLocation.Title = "Updated Title";

            string errorMessage = "Database error";

            _mockLocationRepository.Setup(repo => repo.UpdateAsync(It.IsAny<LocationViewModel>()))
                .ReturnsAsync(DataOperationResult<bool>.Failure(
                    ErrorSource.Database, errorMessage));

            // Act
            var result = await _locationService.UpdateAsync(testLocation);

            // Assert
            Assert.IsFalse(result.Data);
        }

        [TestMethod]
        public async Task UpdateAsync_WhenExceptionOccurs_ShouldReturnFailureResult()
        {
            // Arrange
            var testLocation = TestDataFactory.CreateTestLocation();
            testLocation.Title = "Updated Title";

            var expectedException = new Exception("Test exception");

            _mockLocationRepository.Setup(repo => repo.UpdateAsync(It.IsAny<LocationViewModel>()))
                .ThrowsAsync(expectedException);

            // Act
            var result = await _locationService.UpdateAsync(testLocation);

            // Assert
            Assert.IsFalse(result.IsSuccess);
        }

        [TestMethod]
        public async Task UpdateAsync_WhenExceptionOccurs_ShouldLogError()
        {
            // Arrange
            var testLocation = TestDataFactory.CreateTestLocation();
            testLocation.Title = "Updated Title";

            var expectedException = new Exception("Test exception");

            _mockLocationRepository.Setup(repo => repo.UpdateAsync(It.IsAny<LocationViewModel>()))
                .ThrowsAsync(expectedException);

            // Act
            await _locationService.UpdateAsync(testLocation);

            // Assert
            MockBusinessLoggerService.Verify(logger => logger.LogError(It.IsAny<string>(), expectedException),                Times.Exactly(2));
        }

        [TestMethod]
        public async Task UpdateAsync_WhenExceptionOccurs_ShouldReturnFalseData()
        {
            // Arrange
            var testLocation = TestDataFactory.CreateTestLocation();
            testLocation.Title = "Updated Title";

            var expectedException = new Exception("Test exception");

            _mockLocationRepository.Setup(repo => repo.UpdateAsync(It.IsAny<LocationViewModel>()))
                .ThrowsAsync(expectedException);

            // Act
            var result = await _locationService.UpdateAsync(testLocation);

            // Assert
            Assert.IsFalse(result.Data);
        }

        [TestMethod]
        public async Task UpdateAsync_WhenExceptionOccurs_ShouldReturnErrorSourceUnknown()
        {
            // Arrange
            var testLocation = TestDataFactory.CreateTestLocation();
            testLocation.Title = "Updated Title";

            var expectedException = new Exception("Test exception");

            _mockLocationRepository.Setup(repo => repo.UpdateAsync(It.IsAny<LocationViewModel>()))
                .ThrowsAsync(expectedException);

            // Act
            var result = await _locationService.UpdateAsync(testLocation);

            // Assert
            Assert.AreEqual(ErrorSource.Unknown, result.ErrorSource);
        }
    }
}