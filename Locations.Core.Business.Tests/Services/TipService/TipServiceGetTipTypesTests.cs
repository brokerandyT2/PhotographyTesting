// TipServiceGetTipTypesTests.cs - Fixed
using Locations.Core.Business.DataAccess.Interfaces;
using Locations.Core.Business.DataAccess.Services;
using Locations.Core.Business.Tests.Base;
using Locations.Core.Data.Queries.Interfaces;
using Locations.Core.Shared.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MockFactory = Locations.Core.Business.Tests.TestHelpers.MockFactory;
using TestDataFactory = Locations.Core.Business.Tests.TestHelpers.TestDataFactory;

namespace Locations.Core.Business.Tests.Services.TipServiceTests
{
    [TestClass]
    [TestCategory("TipService")]
    public class TipServiceGetTipTypesTests : BaseServiceTests
    {
        private Mock<ITipRepository> _mockTipRepository;
        private Mock<ITipTypeService<TipTypeViewModel>> _mockTipTypeService;
        private TipService<TipViewModel> _tipService;

        [TestInitialize]
        public override void Setup()
        {
            base.Setup();

            _mockTipRepository = MockFactory.CreateTipRepositoryMock();
            _mockTipTypeService = new Mock<ITipTypeService<TipTypeViewModel>>();

            // Setup tip service with mocks
            _tipService = new TipService<TipViewModel>(
                _mockTipRepository.Object,
                MockAlertService.Object,
                MockLoggerService.Object,
                _mockTipTypeService.Object);
        }

        [TestMethod]
        public async Task GetTipTypesAsync_ShouldReturnNonNullResult()
        {
            // Arrange
            var testTipTypes = new List<TipTypeViewModel>
            {
                TestDataFactory.CreateTestTipType(1),
                TestDataFactory.CreateTestTipType(2),
                TestDataFactory.CreateTestTipType(3)
            };

            var result = new Locations.Core.Shared.ViewModels.OperationResult<List<TipTypeViewModel>>(
                true, testTipTypes);

            _mockTipTypeService.Setup(service => service.GetAllSortedAsync())
                .ReturnsAsync(result);

            // Act
            var tipResult = await _tipService.GetTipTypesAsync();

            // Assert
            Assert.IsNotNull(tipResult);
        }

        [TestMethod]
        public async Task GetTipTypesAsync_WhenTipTypesExist_ShouldReturnSuccessResult()
        {
            // Arrange
            var testTipTypes = new List<TipTypeViewModel>
            {
                TestDataFactory.CreateTestTipType(1),
                TestDataFactory.CreateTestTipType(2),
                TestDataFactory.CreateTestTipType(3)
            };

            var result = new Locations.Core.Shared.ViewModels.OperationResult<List<TipTypeViewModel>>(
                true, testTipTypes);

            _mockTipTypeService.Setup(service => service.GetAllSortedAsync())
                .ReturnsAsync(result);

            // Act
            var tipResult = await _tipService.GetTipTypesAsync();

            // Assert
            Assert.IsTrue(tipResult.IsSuccess);
        }

        [TestMethod]
        public async Task GetTipTypesAsync_WhenTipTypesExist_ShouldReturnCorrectCount()
        {
            // Arrange
            var testTipTypes = new List<TipTypeViewModel>
            {
                TestDataFactory.CreateTestTipType(1),
                TestDataFactory.CreateTestTipType(2),
                TestDataFactory.CreateTestTipType(3)
            };

            var result = new Locations.Core.Shared.ViewModels.OperationResult<List<TipTypeViewModel>>(
                true, testTipTypes);

            _mockTipTypeService.Setup(service => service.GetAllSortedAsync())
                .ReturnsAsync(result);

            // Act
            var tipResult = await _tipService.GetTipTypesAsync();

            // Assert
            Assert.AreEqual(3, tipResult.Data.Count);
        }

        [TestMethod]
        public async Task GetTipTypesAsync_WithNoTipTypeService_ShouldReturnFailureResult()
        {
            // Arrange
            var tipServiceWithoutTipTypeService = new TipService<TipViewModel>(
                _mockTipRepository.Object,
                MockAlertService.Object,
                MockLoggerService.Object,
                null); // No tip type service

            // Act
            var result = await tipServiceWithoutTipTypeService.GetTipTypesAsync();

            // Assert
            Assert.IsFalse(result.IsSuccess);
        }

        [TestMethod]
        public async Task GetTipTypesAsync_WhenTipTypeServiceFails_ShouldReturnFailureResult()
        {
            // Arrange
            string errorMessage = "Failed to retrieve tip types";

            var result = new Locations.Core.Shared.ViewModels.OperationResult<List<TipTypeViewModel>>(
                false, null, errorMessage);

            _mockTipTypeService.Setup(service => service.GetAllSortedAsync())
                .ReturnsAsync(result);

            // Act
            var tipResult = await _tipService.GetTipTypesAsync();

            // Assert
            Assert.IsFalse(tipResult.IsSuccess);
            Assert.AreEqual(errorMessage, tipResult.ErrorMessage);
        }

        [TestMethod]
        public async Task GetTipTypesAsync_WhenExceptionOccurs_ShouldReturnFailureResult()
        {
            // Arrange
            var expectedException = new Exception("Test exception");

            _mockTipTypeService.Setup(service => service.GetAllSortedAsync())
                .ThrowsAsync(expectedException);

            // Act
            var result = await _tipService.GetTipTypesAsync();

            // Assert
            Assert.IsFalse(result.IsSuccess);
            Assert.IsTrue(result.ErrorMessage.Contains("Test exception"));
        }

        [TestMethod]
        public async Task GetTipTypesAsync_WhenExceptionOccurs_ShouldLogError()
        {
            // Arrange
            var expectedException = new Exception("Test exception");

            _mockTipTypeService.Setup(service => service.GetAllSortedAsync())
                .ThrowsAsync(expectedException);

            // Act
            await _tipService.GetTipTypesAsync();

            // Assert
            MockLoggerService.Verify(
                logger => logger.LogError(It.IsAny<string>(), expectedException),
                Times.AtLeastOnce);
        }
    }
}