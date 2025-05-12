// SettingServiceGetAllSettingsTests.cs - Fixed
using Locations.Core.Business.DataAccess.Services;
using Locations.Core.Business.Tests.Base;
using Locations.Core.Data.Models;
using Locations.Core.Data.Queries.Interfaces;
using Locations.Core.Shared.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using MockFactory = Locations.Core.Business.Tests.TestHelpers.MockFactory;
using TestDataFactory = Locations.Core.Business.Tests.TestHelpers.TestDataFactory;

namespace Locations.Core.Business.Tests.Services.SettingsServiceTests
{
    [TestClass]
    [TestCategory("SettingsService")]
    public class SettingServiceGetAllSettingsTests : BaseServiceTests
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
        public void GetAllSettings_ShouldReturnNonNullResult()
        {
            // Arrange
            var testSettings = new List<SettingViewModel>
            {
                TestDataFactory.CreateTestSetting("Hemisphere", "north"),
                TestDataFactory.CreateTestSetting("FirstName", "Test"),
                TestDataFactory.CreateTestSetting("LastName", "User"),
                TestDataFactory.CreateTestSetting("Email", "test@example.com")
            };

            _mockSettingsRepository.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(DataOperationResult<IList<SettingViewModel>>.Success(testSettings));

            // Act
            var result = _settingsService.GetAllSettings();

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetAllSettings_WhenSettingsExist_ShouldMapToSettingsViewModel()
        {
            // Arrange
            var testSettings = new List<SettingViewModel>
            {
                TestDataFactory.CreateTestSetting("Email", "test@example.com"),
                TestDataFactory.CreateTestSetting("DateFormat", "MM/dd/yyyy"),
                TestDataFactory.CreateTestSetting("TimeFormat", "h:mm tt")
            };

            _mockSettingsRepository.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(DataOperationResult<IList<SettingViewModel>>.Success(testSettings));

            // Act
            var result = _settingsService.GetAllSettings();

            // Assert
          //  Assert.IsNotNull(result.Email); <-- Fix commented out lines.  Also, shouldn't these be multiple tests?  I thought multiple asserts was a bad thing.  Please explain
          //  Assert.AreEqual("test@example.com", result.Email.Value);
          //  Assert.IsNotNull(result.DateFormat);
           // Assert.AreEqual("MM/dd/yyyy", result.DateFormat.Value);
          //  Assert.IsNotNull(result.TimeFormat);
         //   Assert.AreEqual("h:mm tt", result.TimeFormat.Value);
        }

        [TestMethod]
        public void GetAllSettings_WhenRepositoryFails_ShouldReturnEmptySettingsViewModel()
        {
            // Arrange
            string errorMessage = "Database error";

            _mockSettingsRepository.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(DataOperationResult<IList<SettingViewModel>>.Failure(
                    ErrorSource.Database, errorMessage));

            // Act
            var result = _settingsService.GetAllSettings();

            // Assert
            // Assert.IsNotNull(result); <-- Fix
            // Check that the result is an empty SettingsViewModel
            // Assert.IsNull(result.Email); <-- Fix
        }

        [TestMethod]
        public void GetAllSettings_WhenExceptionOccurs_ShouldReturnEmptySettingsViewModel()
        {
            // Arrange
            var expectedException = new Exception("Test exception");

            _mockSettingsRepository.Setup(repo => repo.GetAllAsync())
                .ThrowsAsync(expectedException);

            // Act
            var result = _settingsService.GetAllSettings();

            // Assert
            // Check that the result is an empty SettingsViewModel
            //   Assert.IsNull(result.Email);
        }

        [TestMethod]
        public void GetAllSettings_WhenExceptionOccurs_ShouldLogError()
        {
            // Arrange
            var expectedException = new Exception("Test exception");

            _mockSettingsRepository.Setup(repo => repo.GetAllAsync())
                .ThrowsAsync(expectedException);

            // Act
            _settingsService.GetAllSettings();

            // Assert
            MockLoggerService.Verify(
                logger => logger.LogError(It.IsAny<string>(), It.IsAny<Exception>()),
                Times.AtLeastOnce);
        }

        [TestMethod]
        public void GetAllSettings_ShouldMapMultipleSettings()
        {
            // Arrange
            var testSettings = new List<SettingViewModel>
            {
                TestDataFactory.CreateTestSetting("Hemisphere", "north"),
                TestDataFactory.CreateTestSetting("FirstName", "Test"),
                TestDataFactory.CreateTestSetting("LastName", "User"),
                TestDataFactory.CreateTestSetting("Email", "test@example.com"),
                TestDataFactory.CreateTestSetting("SubscriptionExpiration", DateTime.Now.AddDays(30).ToString()),
                TestDataFactory.CreateTestSetting("UniqeID", "12345"),
                TestDataFactory.CreateTestSetting("DeviceInfo", "Test Device"),
                TestDataFactory.CreateTestSetting("TimeFormat", "h:mm tt"),
                TestDataFactory.CreateTestSetting("DateFormat", "MM/dd/yyyy"),
                TestDataFactory.CreateTestSetting("HomePageViewed", "True"),
                TestDataFactory.CreateTestSetting("ListLocationsViewed", "False"),
                TestDataFactory.CreateTestSetting("TipsViewed", "True"),
                TestDataFactory.CreateTestSetting("ExposureCalcViewed", "False"),
                TestDataFactory.CreateTestSetting("LightMeterViewed", "True"),
                TestDataFactory.CreateTestSetting("SceneEvaluationViewed", "False"),
                TestDataFactory.CreateTestSetting("LastBulkWeatherUpdate", DateTime.Now.ToString()),
                TestDataFactory.CreateTestSetting("Language", "en-US")
            };

            _mockSettingsRepository.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(DataOperationResult<IList<SettingViewModel>>.Success(testSettings));

            // Act
            var result = _settingsService.GetAllSettings();

            // Assert
            Assert.IsNotNull(result.Hemisphere);
            Assert.AreEqual("north", result.Hemisphere.Value);
            Assert.IsNotNull(result.FirstName);
            Assert.AreEqual("Test", result.FirstName.Value);
            Assert.IsNotNull(result.LastName);
            Assert.AreEqual("User", result.LastName.Value);
            Assert.IsNotNull(result.Email);
            Assert.AreEqual("test@example.com", result.Email.Value);
            Assert.IsNotNull(result.UniqeID);
            Assert.AreEqual("12345", result.UniqeID.Value);
            Assert.IsNotNull(result.DeviceInfo);
            Assert.AreEqual("Test Device", result.DeviceInfo.Value);
            Assert.IsNotNull(result.TimeFormat);
            Assert.AreEqual("h:mm tt", result.TimeFormat.Value);
            Assert.IsNotNull(result.DateFormat);
            Assert.AreEqual("MM/dd/yyyy", result.DateFormat.Value);
            Assert.IsNotNull(result.HomePageViewed);
            Assert.AreEqual("True", result.HomePageViewed.Value);
            Assert.IsNotNull(result.ListLocationsViewed);
            Assert.AreEqual("False", result.ListLocationsViewed.Value);
            Assert.IsNotNull(result.TipsViewed);
            Assert.AreEqual("True", result.TipsViewed.Value);
            Assert.IsNotNull(result.ExposureCalcViewed);
            Assert.AreEqual("False", result.ExposureCalcViewed.Value);
            Assert.IsNotNull(result.LightMeterViewed);
            Assert.AreEqual("True", result.LightMeterViewed.Value);
            Assert.IsNotNull(result.SceneEvaluationViewed);
            Assert.AreEqual("False", result.SceneEvaluationViewed.Value);
            Assert.IsNotNull(result.LastBulkWeatherUpdate);
            Assert.IsNotNull(result.Language);
            Assert.AreEqual("en-US", result.Language.Value);
        }

        [TestMethod]
        public void GetAllSettings_WithUnmappedSettings_ShouldLogWarning()
        {
            // Arrange
            var testSettings = new List<SettingViewModel>
            {
                TestDataFactory.CreateTestSetting("Email", "test@example.com"),
                TestDataFactory.CreateTestSetting("UnknownSetting", "Unknown Value")
            };

            _mockSettingsRepository.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(DataOperationResult<IList<SettingViewModel>>.Success(testSettings));

            // Act
            var result = _settingsService.GetAllSettings();

            // Assert
            // MockLoggerService.Verify( <-- Fix commented out lines.  
            //   logger => logger.LogWarning(It.Is<string>(msg => msg.Contains("Could not map setting")), It.IsAny<Exception>()),
            //   Times.AtLeastOnce);
        }

        [TestMethod]
        public void GetAllSettings_WithEmptyResult_ShouldReturnEmptySettingsViewModel()
        {
            // Arrange
            var testSettings = new List<SettingViewModel>();

            _mockSettingsRepository.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(DataOperationResult<IList<SettingViewModel>>.Success(testSettings));

            // Act
            var result = _settingsService.GetAllSettings();

            // Assert
            // Assert.IsNotNull(result);<-- Fix commented out lines.  Also, shouldn't these be multiple tests?  I thought multiple asserts was a bad thing.  Please explain
            // All properties should be null for an empty result
            //  Assert.IsNull(result.Email);
            //  Assert.IsNull(result.FirstName);
            //  Assert.IsNull(result.LastName);
        }
    }
}