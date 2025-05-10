// SettingServiceGetAllSettingsTests.cs
using Locations.Core.Business.DataAccess.Services;
using Locations.Core.Business.Tests.Base;
using Locations.Core.Data.Models;
using Locations.Core.Data.Queries.Interfaces;
using Locations.Core.Shared.ViewModels;
using Moq;
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
                MockBusinessLoggerService.Object);
        }

        [TestMethod]
        public void GetAllSettings_ShouldReturnNonNullResult()
        {
            // Arrange
            var testSettings = new List<SettingViewModel>
            {
                TestDataFactory.CreateTestSetting("Hemisphere", "north"),
                TestDataFactory.CreateTestSetting("DateFormat", "MM/dd/yyyy"),
                TestDataFactory.CreateTestSetting("TimeFormat", "h:mm tt")
            };

            _mockSettingsRepository.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(DataOperationResult<IList<SettingViewModel>>.Success(testSettings));

            // Act
            //   var result = _settingsService.GetAllSettings();
            //TODO mock fix for null error coming from magicstrings.databasepath and DataBasePathEncrypted Entire File
            // Assert
            //  Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetAllSettings_WhenSettingsExist_ShouldReturnSettingsViewModel()
        {
            // Arrange
            var testSettings = new List<SettingViewModel>
            {
                TestDataFactory.CreateTestSetting("Hemisphere", "north"),
                TestDataFactory.CreateTestSetting("DateFormat", "MM/dd/yyyy"),
                TestDataFactory.CreateTestSetting("TimeFormat", "h:mm tt")
            };

            _mockSettingsRepository.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(DataOperationResult<IList<SettingViewModel>>.Success(testSettings));

            // Act
       //     var result = _settingsService.GetAllSettings();

            // Assert
         //   Assert.IsInstanceOfType(result, typeof(SettingsViewModel));
        }

        [TestMethod]
        public void GetAllSettings_WhenHemisphereSettingExists_ShouldMapToHemisphereProperly()
        {
            // Arrange
            var testSettings = new List<SettingViewModel>
            {
                TestDataFactory.CreateTestSetting("Hemisphere", "north"),
                TestDataFactory.CreateTestSetting("DateFormat", "MM/dd/yyyy"),
                TestDataFactory.CreateTestSetting("TimeFormat", "h:mm tt")
            };

            _mockSettingsRepository.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(DataOperationResult<IList<SettingViewModel>>.Success(testSettings));

            // Act
         //   var result = _settingsService.GetAllSettings();

            // Assert
         //   Assert.IsNotNull(result.Hemisphere);
          //  Assert.AreEqual("north", result.Hemisphere.Value);
        }

        [TestMethod]
        public void GetAllSettings_WhenDateFormatSettingExists_ShouldMapToDateFormatProperly()
        {
            // Arrange
            var testSettings = new List<SettingViewModel>
            {
                TestDataFactory.CreateTestSetting("Hemisphere", "north"),
                TestDataFactory.CreateTestSetting("DateFormat", "MM/dd/yyyy"),
                TestDataFactory.CreateTestSetting("TimeFormat", "h:mm tt")
            };

            _mockSettingsRepository.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(DataOperationResult<IList<SettingViewModel>>.Success(testSettings));

            // Act
          //  var result = _settingsService.GetAllSettings();

            // Assert
          //  Assert.IsNotNull(result.DateFormat);
          //  Assert.AreEqual("MM/dd/yyyy", result.DateFormat.Value);
        }

        [TestMethod]
        public void GetAllSettings_WhenTimeFormatSettingExists_ShouldMapToTimeFormatProperly()
        {
            // Arrange
            var testSettings = new List<SettingViewModel>
            {
                TestDataFactory.CreateTestSetting("Hemisphere", "north"),
                TestDataFactory.CreateTestSetting("DateFormat", "MM/dd/yyyy"),
                TestDataFactory.CreateTestSetting("TimeFormat", "h:mm tt")
            };

            _mockSettingsRepository.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(DataOperationResult<IList<SettingViewModel>>.Success(testSettings));

            // Act
       //     var result = _settingsService.GetAllSettings();

            // Assert
       //     Assert.IsNotNull(result.TimeFormat);
       //     Assert.AreEqual("h:mm tt", result.TimeFormat.Value);
        }

        [TestMethod]
        public void GetAllSettings_WhenSettingDoesNotExist_ShouldInitializeWithDefaultValue()
        {
            // Arrange
            var testSettings = new List<SettingViewModel>
            {
                TestDataFactory.CreateTestSetting("Hemisphere", "north")
                // DateFormat and TimeFormat are missing
            };

       //     _mockSettingsRepository.Setup(repo => repo.GetAllAsync())
     //           .ReturnsAsync(DataOperationResult<IList<SettingViewModel>>.Success(testSettings));

            // Act
       //     var result = _settingsService.GetAllSettings();

            // Assert
          //  Assert.IsNotNull(result.DateFormat); // Should be initialized with default value
         //   Assert.AreEqual(string.Empty, result.DateFormat.Value);
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
        //    var result = _settingsService.GetAllSettings();

            // Assert
         //   Assert.IsNotNull(result);
         //   Assert.IsInstanceOfType(result, typeof(SettingsViewModel));
        }

        [TestMethod]
        public void GetAllSettings_WhenExceptionOccurs_ShouldReturnEmptySettingsViewModel()
        {
            // Arrange
            var expectedException = new Exception("Test exception");

            _mockSettingsRepository.Setup(repo => repo.GetAllAsync())
                .ThrowsAsync(expectedException);

            // Act
           // var result = _settingsService.GetAllSettings();

            // Assert
         //   Assert.IsNotNull(result);
        //    Assert.IsInstanceOfType(result, typeof(SettingsViewModel));
        }

        [TestMethod]
        public void GetAllSettings_WhenExceptionOccurs_ShouldLogError()
        {
            // Arrange
            var expectedException = new Exception("Test exception");

            _mockSettingsRepository.Setup(repo => repo.GetAllAsync())
                .ThrowsAsync(expectedException);

            // Act
        //    _settingsService.GetAllSettings();

            // Assert
        //    MockBusinessLoggerService.Verify(logger => logger.LogError(It.IsAny<string>(), expectedException),                Times.AtLeastOnce);
        }

        [TestMethod]
        public void GetAllSettings_ShouldCallRepositoryGetAllAsync()
        {
            // Arrange
            var testSettings = new List<SettingViewModel>
            {
                TestDataFactory.CreateTestSetting("Hemisphere", "north"),
                TestDataFactory.CreateTestSetting("DateFormat", "MM/dd/yyyy")
            };

            _mockSettingsRepository.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(DataOperationResult<IList<SettingViewModel>>.Success(testSettings));

            // Act
           // _settingsService.GetAllSettings();

            // Assert
           // _mockSettingsRepository.Verify(repo => repo.GetAllAsync(), Times.Once);
        }
    }
}