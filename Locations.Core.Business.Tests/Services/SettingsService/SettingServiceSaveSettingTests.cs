// SettingServiceSaveSettingTests.cs - Fixed
using Locations.Core.Business.DataAccess.Services;
using Locations.Core.Business.Tests.Base;
using Locations.Core.Data.Models;
using Locations.Core.Data.Queries.Interfaces;
using Locations.Core.Shared.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Threading.Tasks;
using MockFactory = Locations.Core.Business.Tests.TestHelpers.MockFactory;
using TestDataFactory = Locations.Core.Business.Tests.TestHelpers.TestDataFactory;

namespace Locations.Core.Business.Tests.Services.SettingsServiceTests
{
    [TestClass]
    [TestCategory("SettingsService")]
    public class SettingServiceSaveSettingTests : BaseServiceTests
    {
        private Mock<ISettingsRepository> _mockSettingsRepository;
        private SettingsService<SettingViewModel> _settingsService;

        [TestInitialize]
        public override void Setup()
        {
            base.Setup();

            _mockSettingsRepository = MockFactory.CreateSettingsRepositoryMock();

            // Setup settings service with mocks
            _settingsService = new SettingsService<SettingViewModel>(
                _mockSettingsRepository.Object,
                MockAlertService.Object,
                MockLoggerService.Object);
        }

        [TestMethod]
        public void SaveSetting_WhenSettingExists_ShouldCallUpdateAsync()
        {
            // Arrange
            string settingName = "TestSetting";
            string settingValue = "TestValue";
            var existingSetting = TestDataFactory.CreateTestSetting(settingName, "OldValue");

            _mockSettingsRepository.Setup(repo => repo.GetByNameAsync(settingName))
                .ReturnsAsync(DataOperationResult<SettingViewModel>.Success(existingSetting));

            _mockSettingsRepository.Setup(repo => repo.UpdateAsync(It.IsAny<SettingViewModel>()))
                .ReturnsAsync(DataOperationResult<bool>.Success(true));

            // Act
            var result = _settingsService.SaveSetting(settingName, settingValue);

            // Assert
            _mockSettingsRepository.Verify(repo => repo.UpdateAsync(It.IsAny<SettingViewModel>()), Times.Once);
        }

        [TestMethod]
        public void SaveSetting_WhenSettingDoesNotExist_ShouldCallSaveAsync()
        {
            // Arrange
            string settingName = "NewSetting";
            string settingValue = "NewValue";

            _mockSettingsRepository.Setup(repo => repo.GetByNameAsync(settingName))
                .ReturnsAsync(DataOperationResult<SettingViewModel>.Failure(
                    ErrorSource.Database, "Setting not found"));

            _mockSettingsRepository.Setup(repo => repo.SaveAsync(It.IsAny<SettingViewModel>()))
                .ReturnsAsync(DataOperationResult<SettingViewModel>.Success(
                    TestDataFactory.CreateTestSetting(settingName, settingValue)));

            // Act
            var result = _settingsService.SaveSetting(settingName, settingValue);

            // Assert
            _mockSettingsRepository.Verify(repo => repo.SaveAsync(It.IsAny<SettingViewModel>()), Times.Once);
        }

        [TestMethod]
        public void SaveSetting_WhenUpdateSucceeds_ShouldReturnTrue()
        {
            // Arrange
            string settingName = "TestSetting";
            string settingValue = "TestValue";
            var existingSetting = TestDataFactory.CreateTestSetting(settingName, "OldValue");

            _mockSettingsRepository.Setup(repo => repo.GetByNameAsync(settingName))
                .ReturnsAsync(DataOperationResult<SettingViewModel>.Success(existingSetting));

            _mockSettingsRepository.Setup(repo => repo.UpdateAsync(It.IsAny<SettingViewModel>()))
                .ReturnsAsync(DataOperationResult<bool>.Success(true));

            // Act
            var result = _settingsService.SaveSetting(settingName, settingValue);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void SaveSetting_WhenSaveSucceeds_ShouldReturnTrue()
        {
            // Arrange
            string settingName = "NewSetting";
            string settingValue = "NewValue";

            _mockSettingsRepository.Setup(repo => repo.GetByNameAsync(settingName))
                .ReturnsAsync(DataOperationResult<SettingViewModel>.Failure(
                    ErrorSource.Database, "Setting not found"));

            _mockSettingsRepository.Setup(repo => repo.SaveAsync(It.IsAny<SettingViewModel>()))
                .ReturnsAsync(DataOperationResult<SettingViewModel>.Success(
                    TestDataFactory.CreateTestSetting(settingName, settingValue)));

            // Act
            var result = _settingsService.SaveSetting(settingName, settingValue);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void SaveSetting_WhenUpdateFails_ShouldReturnFalse()
        {
            // Arrange
            string settingName = "TestSetting";
            string settingValue = "TestValue";
            var existingSetting = TestDataFactory.CreateTestSetting(settingName, "OldValue");

            _mockSettingsRepository.Setup(repo => repo.GetByNameAsync(settingName))
                .ReturnsAsync(DataOperationResult<SettingViewModel>.Success(existingSetting));

            _mockSettingsRepository.Setup(repo => repo.UpdateAsync(It.IsAny<SettingViewModel>()))
                .ReturnsAsync(DataOperationResult<bool>.Failure(
                    ErrorSource.Database, "Update failed"));

            // Act
            var result = _settingsService.SaveSetting(settingName, settingValue);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void SaveSetting_WhenSaveFails_ShouldReturnFalse()
        {
            // Arrange
            string settingName = "NewSetting";
            string settingValue = "NewValue";

            _mockSettingsRepository.Setup(repo => repo.GetByNameAsync(settingName))
                .ReturnsAsync(DataOperationResult<SettingViewModel>.Failure(
                    ErrorSource.Database, "Setting not found"));

            _mockSettingsRepository.Setup(repo => repo.SaveAsync(It.IsAny<SettingViewModel>()))
                .ReturnsAsync(DataOperationResult<SettingViewModel>.Failure(
                    ErrorSource.Database, "Save failed"));

            // Act
            var result = _settingsService.SaveSetting(settingName, settingValue);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void SaveSetting_WhenExceptionOccurs_ShouldReturnFalse()
        {
            // Arrange
            string settingName = "TestSetting";
            string settingValue = "TestValue";
            var expectedException = new Exception("Test exception");

            _mockSettingsRepository.Setup(repo => repo.GetByNameAsync(settingName))
                .ReturnsAsync(DataOperationResult<SettingViewModel>.Failure(
                    ErrorSource.Database, "Setting not found"));

            _mockSettingsRepository.Setup(repo => repo.SaveAsync(It.IsAny<SettingViewModel>()))
                .ThrowsAsync(expectedException);

            // Act
            var result = _settingsService.SaveSetting(settingName, settingValue);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void SaveSetting_WhenExceptionOccurs_ShouldLogError()
        {
            // Arrange
            string settingName = "TestSetting";
            string settingValue = "TestValue";
            var expectedException = new Exception("Test exception");

            _mockSettingsRepository.Setup(repo => repo.GetByNameAsync(settingName))
                .ThrowsAsync(expectedException);

            // Act
            _settingsService.SaveSetting(settingName, settingValue);

            // Assert
            MockLoggerService.Verify(
                logger => logger.LogError(It.IsAny<string>(), It.IsAny<Exception>()),
                Times.AtLeastOnce);
        }

        [TestMethod]
        public void SaveSetting_ShouldPassCorrectNameAndValueToRepository()
        {
            // Arrange
            string settingName = "NewSetting";
            string settingValue = "NewValue";
            SettingViewModel capturedSetting = null;

            _mockSettingsRepository.Setup(repo => repo.GetByNameAsync(settingName))
                .ReturnsAsync(DataOperationResult<SettingViewModel>.Failure(
                    ErrorSource.Database, "Setting not found"));

            _mockSettingsRepository.Setup(repo => repo.SaveAsync(It.IsAny<SettingViewModel>()))
                .Callback<SettingViewModel>(setting => capturedSetting = setting)
                .ReturnsAsync(DataOperationResult<SettingViewModel>.Success(
                    TestDataFactory.CreateTestSetting(settingName, settingValue)));

            // Act
            _settingsService.SaveSetting(settingName, settingValue);

            // Assert
            Assert.IsNotNull(capturedSetting);
            Assert.AreEqual(settingName, capturedSetting.Key);
            Assert.AreEqual(settingValue, capturedSetting.Value);
        }
    }
}