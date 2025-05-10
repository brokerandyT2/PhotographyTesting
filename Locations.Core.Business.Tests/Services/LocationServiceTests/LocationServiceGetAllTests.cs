// LocationServiceGetAllTests.cs
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Locations.Core.Business.DataAccess.Services;
using Locations.Core.Business.DataAccess.Interfaces;
using Locations.Core.Data.Models;
using Locations.Core.Data.Queries.Interfaces;
using Locations.Core.Shared.ViewModels;
using Locations.Core.Business.Tests.Base;
using Locations.Core.Business.Tests.TestHelpers;
using MockFactory = Locations.Core.Business.Tests.TestHelpers.MockFactory;

namespace Locations.Core.Business.Tests.Services.LocationServiceTests
{
    [TestClass]
    [TestCategory("LocationService")]
    public class LocationServiceGetAllTests : BaseServiceTests
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
        public async Task GetAllAsync_WhenLocationsExist_ShouldReturnSuccessResult()
        {
            // Arrange
            var testLocations = TestDataFactory.CreateTestLocations(3);

            _mockLocationRepository.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(DataOperationResult<IList<LocationViewModel>>.Success(testLocations));

            // Act
            var result = await _locationService.GetAllAsync();

            // Assert
            Assert.IsTrue(result.IsSuccess);
        }

        [TestMethod]
        public async Task GetAllAsync_WhenLocationsExist_ShouldReturnNonNullData()
        {
            // Arrange
            var testLocations = TestDataFactory.CreateTestLocations(3);

            _mockLocationRepository.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(DataOperationResult<IList<LocationViewModel>>.Success(testLocations));

            // Act
            var result = await _locationService.GetAllAsync();

            // Assert
            Assert.IsNotNull(result.Data);
        }

        [TestMethod]
        public async Task GetAllAsync_WhenLocationsExist_ShouldReturnCorrectCount()
        {
            // Arrange
            var testLocations = TestDataFactory.CreateTestLocations(3);

            _mockLocationRepository.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(DataOperationResult<IList<LocationViewModel>>.Success(testLocations));

            // Act
            var result = await _locationService.GetAllAsync();

            // Assert
            Assert.AreEqual(3, result.Data.Count);
        }

        [TestMethod]
        public async Task GetAllAsync_WhenLocationsExist_FirstLocationShouldHaveCorrectId()
        {
            // Arrange
            var testLocations = TestDataFactory.CreateTestLocations(3);

            _mockLocationRepository.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(DataOperationResult<IList<LocationViewModel>>.Success(testLocations));

            // Act
            var result = await _locationService.GetAllAsync();

            // Assert
            Assert.AreEqual(1, result.Data[0].Id);
        }

        [TestMethod]
        public async Task GetAllAsync_WhenLocationsExist_FirstLocationShouldHaveCorrectTitle()
        {
            // Arrange
            var testLocations = TestDataFactory.CreateTestLocations(3);

            _mockLocationRepository.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(DataOperationResult<IList<LocationViewModel>>.Success(testLocations));

            // Act
            var result = await _locationService.GetAllAsync();

            // Assert
            Assert.AreEqual(testLocations[0].Title, result.Data[0].Title);
        }

        [TestMethod]
        public async Task GetAllAsync_WhenNoLocationsExist_ShouldReturnSuccessResult()
        {
            // Arrange
            _mockLocationRepository.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(DataOperationResult<IList<LocationViewModel>>.Success(
                    new List<LocationViewModel>()));

            // Act
            var result = await _locationService.GetAllAsync();

            // Assert
            Assert.IsTrue(result.IsSuccess);
        }

        [TestMethod]
        public async Task GetAllAsync_WhenNoLocationsExist_ShouldReturnEmptyList()
        {
            // Arrange
            _mockLocationRepository.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(DataOperationResult<IList<LocationViewModel>>.Success(
                    new List<LocationViewModel>()));

            // Act
            var result = await _locationService.GetAllAsync();

            // Assert
            Assert.AreEqual(0, result.Data.Count);
        }

        [TestMethod]
        public async Task GetAllAsync_WhenRepositoryFails_ShouldReturnFailureResult()
        {
            // Arrange
            string errorMessage = "Database error";

            _mockLocationRepository.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(DataOperationResult<IList<LocationViewModel>>.Failure(
                    ErrorSource.Database, errorMessage));

            // Act
            var result = await _locationService.GetAllAsync();

            // Assert
            Assert.IsFalse(result.IsSuccess);
        }

        [TestMethod]
        public async Task GetAllAsync_WhenRepositoryFails_ShouldReturnNullData()
        {
            // Arrange
            string errorMessage = "Database error";

            _mockLocationRepository.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(DataOperationResult<IList<LocationViewModel>>.Failure(
                    ErrorSource.Database, errorMessage));

            // Act
            var result = await _locationService.GetAllAsync();

            // Assert
            Assert.IsNull(result.Data);
        }

        [TestMethod]
        public async Task GetAllAsync_WhenRepositoryFails_ShouldReturnCorrectErrorSource()
        {
            // Arrange
            string errorMessage = "Database error";

            _mockLocationRepository.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(DataOperationResult<IList<LocationViewModel>>.Failure(
                    ErrorSource.Database, errorMessage));

            // Act
            var result = await _locationService.GetAllAsync();

            // Assert
            Assert.AreEqual(ErrorSource.Database, result.ErrorSource);
        }

        [TestMethod]
        public async Task GetAllAsync_WhenExceptionOccurs_ShouldReturnFailureResult()
        {
            // Arrange
            var expectedException = new Exception("Test exception");

            _mockLocationRepository.Setup(repo => repo.GetAllAsync())
                .ThrowsAsync(expectedException);

            // Act
            var result = await _locationService.GetAllAsync();

            // Assert
            Assert.IsFalse(result.IsSuccess);
        }

        [TestMethod]
        public async Task GetAllAsync_WhenExceptionOccurs_ShouldLogError()
        {
            // Arrange
            var expectedException = new Exception("Test exception");

            _mockLocationRepository.Setup(repo => repo.GetAllAsync())
                .ThrowsAsync(expectedException);

            // Act
            await _locationService.GetAllAsync();

            // Assert
            MockBusinessLoggerService.Verify(
                logger => logger.LogError(It.IsAny<string>(), expectedException),
                Times.Once);
        }
    }
}