// LocationServiceDeleteTests.cs
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
    public class LocationServiceDeleteTests : BaseServiceTests
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

        #region DeleteAsync(int id) Tests

        [TestMethod]
        public async Task DeleteAsyncById_WithValidId_ShouldReturnSuccessResult()
        {
            // Arrange
            int locationId = 1;

            _mockLocationRepository.Setup(repo => repo.DeleteAsync(locationId))
                .ReturnsAsync(DataOperationResult<bool>.Success(true));

            // Act
            var result = await _locationService.DeleteAsync(locationId);

            // Assert
            Assert.IsTrue(result.IsSuccess);
        }

        [TestMethod]
        public async Task DeleteAsyncById_WithValidId_ShouldReturnTrueData()
        {
            // Arrange
            int locationId = 1;

            _mockLocationRepository.Setup(repo => repo.DeleteAsync(locationId))
                .ReturnsAsync(DataOperationResult<bool>.Success(true));

            // Act
            var result = await _locationService.DeleteAsync(locationId);

            // Assert
            Assert.IsTrue(result.Data);
        }

        [TestMethod]
        public async Task DeleteAsyncById_ShouldPassCorrectIdToRepository()
        {
            // Arrange
            int locationId = 1;
            int capturedId = 0;

            _mockLocationRepository.Setup(repo => repo.DeleteAsync(It.IsAny<int>()))
                .Callback<int>(id => capturedId = id)
                .ReturnsAsync(DataOperationResult<bool>.Success(true));

            // Act
            await _locationService.DeleteAsync(locationId);

            // Assert
            Assert.AreEqual(locationId, capturedId);
        }

        [TestMethod]
        public async Task DeleteAsyncById_WithInvalidId_ShouldReturnFailureResult()
        {
            // Arrange
            int locationId = -1; // Invalid ID
            string errorMessage = "Invalid location ID";

            _mockLocationRepository.Setup(repo => repo.DeleteAsync(locationId))
                .ReturnsAsync(DataOperationResult<bool>.Failure(
                    ErrorSource.ModelValidation, errorMessage));

            // Act
            var result = await _locationService.DeleteAsync(locationId);

            // Assert
            Assert.IsFalse(result.IsSuccess);
        }

        [TestMethod]
        public async Task DeleteAsyncById_WithInvalidId_ShouldReturnCorrectErrorSource()
        {
            // Arrange
            int locationId = -1; // Invalid ID
            string errorMessage = "Invalid location ID";

            _mockLocationRepository.Setup(repo => repo.DeleteAsync(locationId))
                .ReturnsAsync(DataOperationResult<bool>.Failure(
                    ErrorSource.ModelValidation, errorMessage));

            // Act
            var result = await _locationService.DeleteAsync(locationId);

            // Assert
            Assert.AreEqual(ErrorSource.ModelValidation, result.ErrorSource);
        }

        [TestMethod]
        public async Task DeleteAsyncById_WhenExceptionOccurs_ShouldReturnFailureResult()
        {
            // Arrange
            int locationId = 1;
            var expectedException = new Exception("Test exception");

            _mockLocationRepository.Setup(repo => repo.DeleteAsync(locationId))
                .ThrowsAsync(expectedException);

            // Act
            var result = await _locationService.DeleteAsync(locationId);

            // Assert
            Assert.IsFalse(result.IsSuccess);
        }

        [TestMethod]
        public async Task DeleteAsyncById_WhenExceptionOccurs_ShouldLogError()
        {
            // Arrange
            int locationId = 1;
            var expectedException = new Exception("Test exception");

            _mockLocationRepository.Setup(repo => repo.DeleteAsync(locationId))
                .ThrowsAsync(expectedException);

            // Act
            await _locationService.DeleteAsync(locationId);

            // Assert
           MockBusinessLoggerService.Verify(logger => logger.LogError(It.IsAny<string>(), expectedException),                Times.Once);
        }

        #endregion

        #region DeleteAsync(LocationViewModel entity) Tests

        [TestMethod]
        public async Task DeleteAsyncEntity_WithValidEntity_ShouldReturnSuccessResult()
        {
            // Arrange
            var testLocation = TestDataFactory.CreateTestLocation();

            _mockLocationRepository.Setup(repo => repo.DeleteAsync(testLocation.Id))
                .ReturnsAsync(DataOperationResult<bool>.Success(true));

            // Act
            var result = await _locationService.DeleteAsync(testLocation);

            // Assert
            Assert.IsTrue(result.IsSuccess);
        }

        [TestMethod]
        public async Task DeleteAsyncEntity_WithValidEntity_ShouldReturnTrueData()
        {
            // Arrange
            var testLocation = TestDataFactory.CreateTestLocation();

            _mockLocationRepository.Setup(repo => repo.DeleteAsync(testLocation.Id))
                .ReturnsAsync(DataOperationResult<bool>.Success(true));

            // Act
            var result = await _locationService.DeleteAsync(testLocation);

            // Assert
            Assert.IsTrue(result.Data);
        }

        [TestMethod]
        public async Task DeleteAsyncEntity_ShouldCallDeleteWithCorrectId()
        {
            // Arrange
            var testLocation = TestDataFactory.CreateTestLocation();
            int capturedId = 0;

            _mockLocationRepository.Setup(repo => repo.DeleteAsync(It.IsAny<int>()))
                .Callback<int>(id => capturedId = id)
                .ReturnsAsync(DataOperationResult<bool>.Success(true));

            // Act
            await _locationService.DeleteAsync(testLocation);

            // Assert
            Assert.AreEqual(testLocation.Id, capturedId);
        }

        [TestMethod]
        public async Task DeleteAsyncEntity_WithNullEntity_ShouldReturnFailureResult()
        {
            // Act
            var result = await _locationService.DeleteAsync((LocationViewModel)null);

            // Assert
            Assert.IsFalse(result.IsSuccess);
        }

        [TestMethod]
        public async Task DeleteAsyncEntity_WithNullEntity_ShouldReturnErrorSourceUnknown()
        {
            // Act
            var result = await _locationService.DeleteAsync((LocationViewModel)null);

            // Assert
            Assert.AreEqual(ErrorSource.Unknown, result.ErrorSource);
        }

        [TestMethod]
        public async Task DeleteAsyncEntity_WithInvalidId_ShouldReturnFailureResult()
        {
            // Arrange
            var testLocation = TestDataFactory.CreateTestLocation();
            testLocation.Id = -1; // Invalid ID

            // Act
            var result = await _locationService.DeleteAsync(testLocation);

            // Assert
            Assert.IsFalse(result.IsSuccess);
        }

        [TestMethod]
        public async Task DeleteAsyncEntity_WithZeroId_ShouldReturnFailureResult()
        {
            // Arrange
            var testLocation = TestDataFactory.CreateTestLocation();
            testLocation.Id = 0; // Invalid ID for existing entity

            // Act
            var result = await _locationService.DeleteAsync(testLocation);

            // Assert
            Assert.IsFalse(result.IsSuccess);
        }

        [TestMethod]
        public async Task DeleteAsyncEntity_WhenExceptionOccurs_ShouldReturnFailureResult()
        {
            // Arrange
            var testLocation = TestDataFactory.CreateTestLocation();
            var expectedException = new Exception("Test exception");

            _mockLocationRepository.Setup(repo => repo.DeleteAsync(testLocation.Id))
                .ThrowsAsync(expectedException);

            // Act
            var result = await _locationService.DeleteAsync(testLocation);

            // Assert
            Assert.IsFalse(result.IsSuccess);
        }

        [TestMethod]
        public async Task DeleteAsyncEntity_WhenExceptionOccurs_ShouldLogError()
        {
            // Arrange
            var testLocation = TestDataFactory.CreateTestLocation();
            var expectedException = new Exception("Test exception");

            _mockLocationRepository.Setup(repo => repo.DeleteAsync(testLocation.Id))
                .ThrowsAsync(expectedException);

            // Act
            await _locationService.DeleteAsync(testLocation);

            // Assert

            MockBusinessLoggerService.Verify(logger => logger.LogError(It.IsAny<string>(), expectedException),                Times.Once);
        }

        #endregion
    }
}