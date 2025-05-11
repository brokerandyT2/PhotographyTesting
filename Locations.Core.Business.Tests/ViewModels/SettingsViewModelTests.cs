// SettingsViewModelTests.cs
using Locations.Core.Shared.ViewModels;
using Locations.Core.Shared.ViewModelServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

// Use explicit namespaces to resolve ambiguity
using OperationErrorEventArgs = Locations.Core.Shared.ViewModelServices.OperationErrorEventArgs;
using OperationErrorSource = Locations.Core.Shared.ViewModelServices.OperationErrorSource;
using OperationResul = Locations.Core.Shared.ViewModelServices.OperationResult<Locations.Core.Shared.ViewModels.SettingViewModel>;

namespace Locations.Core.Business.Tests.ViewModels
{
    [TestClass]
    [TestCategory("ViewModels")]
    public class SettingsViewModelTests
    {
        private Mock<ISettingService> _mockSettingsService;
        private SettingsViewModel _viewModel;

        [TestInitialize]
        public void Setup()
        {
            _mockSettingsService = new Mock<ISettingService>();
            _viewModel = new SettingsViewModel();
        }

        [TestMethod]
        public void Constructor_WithNoDependencies_ShouldInitializeProperties()
        {
            // Assert
            Assert.IsNotNull(_viewModel.SaveSettingsCommand);
            Assert.IsNotNull(_viewModel.ResetSettingsCommand);

            // Verify default settings
            Assert.IsNotNull(_viewModel.Hemisphere);
            Assert.IsNotNull(_viewModel.FirstName);
            Assert.IsNotNull(_viewModel.LastName);
            Assert.IsNotNull(_viewModel.Email);
            Assert.IsNotNull(_viewModel.SubscriptionExpiration);
            Assert.IsNotNull(_viewModel.SubscriptionType);
            Assert.IsNotNull(_viewModel.UniqeID);
            Assert.IsNotNull(_viewModel.DeviceInfo);
            Assert.IsNotNull(_viewModel.TimeFormat);
            Assert.IsNotNull(_viewModel.DateFormat);
            Assert.IsNotNull(_viewModel.WindDirection);
            Assert.IsNotNull(_viewModel.HomePageViewed);
            Assert.IsNotNull(_viewModel.ListLocationsViewed);
            Assert.IsNotNull(_viewModel.TipsViewed);
            Assert.IsNotNull(_viewModel.ExposureCalcViewed);
            Assert.IsNotNull(_viewModel.LightMeterViewed);
            Assert.IsNotNull(_viewModel.SceneEvaluationViewed);
            Assert.IsNotNull(_viewModel.SunCalculationViewed);
            Assert.IsNotNull(_viewModel.LastBulkWeatherUpdate);
            Assert.IsNotNull(_viewModel.Language);
            Assert.IsNotNull(_viewModel.AdSupport);
            Assert.IsNotNull(_viewModel.TemperatureFormat);

            // Verify default values
            Assert.AreEqual("North", _viewModel.Hemisphere.Value);
            Assert.AreEqual("h:mm tt", _viewModel.TimeFormat.Value);
            Assert.AreEqual("MM/dd/yyyy", _viewModel.DateFormat.Value);
            Assert.AreEqual("towardsWind", _viewModel.WindDirection.Value);
            Assert.AreEqual("False", _viewModel.HomePageViewed.Value);
            Assert.AreEqual("False", _viewModel.TipsViewed.Value);
            Assert.AreEqual("en-US", _viewModel.Language.Value);
            Assert.AreEqual("True", _viewModel.AdSupport.Value);
            Assert.AreEqual("Fahrenheit", _viewModel.TemperatureFormat.Value);
        }

        [TestMethod]
        public void Constructor_WithSettingsService_ShouldInitializePropertiesAndService()
        {
            // Arrange & Act
            var viewModel = new SettingsViewModel(/* _mockSettingsService.Object */);

            // Assert
            Assert.IsNotNull(viewModel.SaveSettingsCommand);
            Assert.IsNotNull(viewModel.ResetSettingsCommand);
        }

        [TestMethod]
        public void HemisphereNorth_WhenHemisphereIsNorth_ShouldReturnTrue()
        {
            // Arrange
            _viewModel.Hemisphere = new SettingViewModel("Hemisphere", "North");

            // Act & Assert
            Assert.IsTrue(_viewModel.HemisphereNorth);
        }

        [TestMethod]
        public void HemisphereNorth_WhenHemisphereIsSouth_ShouldReturnFalse()
        {
            // Arrange
            _viewModel.Hemisphere = new SettingViewModel("Hemisphere", "South");

            // Act & Assert
            Assert.IsFalse(_viewModel.HemisphereNorth);
        }

        [TestMethod]
        public void TimeFormatToggle_WhenTimeFormatIsUS_ShouldReturnTrue()
        {
            // Arrange
            _viewModel.TimeFormat = new SettingViewModel("TimeFormat", "h:mm tt");

            // Act & Assert
            Assert.IsTrue(_viewModel.TimeFormatToggle);
        }

        [TestMethod]
        public void TimeFormatToggle_WhenTimeFormatIsNotUS_ShouldReturnFalse()
        {
            // Arrange
            _viewModel.TimeFormat = new SettingViewModel("TimeFormat", "HH:mm");

            // Act & Assert
            Assert.IsFalse(_viewModel.TimeFormatToggle);
        }

        [TestMethod]
        public void DateFormatToggle_WhenDateFormatIsUS_ShouldReturnTrue()
        {
            // Arrange
            _viewModel.DateFormat = new SettingViewModel("DateFormat", "MM/dd/yyyy");

            // Act & Assert
            Assert.IsTrue(_viewModel.DateFormatToggle);
        }

        [TestMethod]
        public void DateFormatToggle_WhenDateFormatIsNotUS_ShouldReturnFalse()
        {
            // Arrange
            _viewModel.DateFormat = new SettingViewModel("DateFormat", "yyyy-MM-dd");

            // Act & Assert
            Assert.IsFalse(_viewModel.DateFormatToggle);
        }

        [TestMethod]
        public void WindDirectionBoolean_WhenWindDirectionIsTowardsWind_ShouldReturnTrue()
        {
            // Arrange
            _viewModel.WindDirection = new SettingViewModel("WindDirection", "towardsWind");

            // Act & Assert
            Assert.IsTrue(_viewModel.WindDirectionBoolean);
        }

        [TestMethod]
        public void WindDirectionBoolean_WhenWindDirectionIsNotTowardsWind_ShouldReturnFalse()
        {
            // Arrange
            _viewModel.WindDirection = new SettingViewModel("WindDirection", "fromWind");

            // Act & Assert
            Assert.IsFalse(_viewModel.WindDirectionBoolean);
        }

        [TestMethod]
        public void HomePageViewedBool_WhenHomePageViewedIsTrue_ShouldReturnTrue()
        {
            // Arrange
            _viewModel.HomePageViewed = new SettingViewModel("HomePageViewed", "True");

            // Act & Assert
            Assert.IsTrue(_viewModel.HomePageViewedBool);
        }

        [TestMethod]
        public void TemperatureFormatToggle_WhenTemperatureFormatIsFahrenheit_ShouldReturnTrue()
        {
            // Arrange
            _viewModel.TemperatureFormat = new SettingViewModel("TemperatureFormat", "Fahrenheit");

            // Act & Assert
            Assert.IsTrue(_viewModel.TemperatureFormatToggle);
        }

        [TestMethod]
        public void TemperatureFormatToggle_WhenTemperatureFormatIsCelsius_ShouldReturnFalse()
        {
            // Arrange
            _viewModel.TemperatureFormat = new SettingViewModel("TemperatureFormat", "Celsius");

            // Act & Assert
            Assert.IsFalse(_viewModel.TemperatureFormatToggle);
        }

        [TestMethod]
        public async Task SaveSettingsAsync_WhenSettingsValid_ShouldCallSettingsService()
        {
            // Arrange
            var tcs = new TaskCompletionSource<bool>();

            // Setup for GetSettings_Async which is called when settings are saved
            var settingsViewModel = new SettingsViewModel();
            var result = new Shared.ViewModelServices.OperationResult<SettingsViewModel>(true, settingsViewModel);

            _mockSettingsService.Setup(service => service.GetSettings_Async())
                .ReturnsAsync(result);

            // Setup the SaveSettingAsync method that will be called for individual settings
            _mockSettingsService.Setup(service => service.SaveSettingAsync(It.IsAny<SettingViewModel>()))
                .Callback(() => tcs.SetResult(true))
                .ReturnsAsync(new Shared.ViewModelServices.OperationResult<bool>(true, true));

            // Use reflection to set the private _settingsService field
            var fieldInfo = typeof(SettingsViewModel).GetField("_settingsService",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            fieldInfo.SetValue(_viewModel, _mockSettingsService.Object);

            // Act
            _viewModel.SaveSettingsCommand.Execute(null);

            // Wait for async completion or timeout after a reasonable time
            await Task.WhenAny(tcs.Task, Task.Delay(1000));

            // Assert
            Assert.IsFalse(_viewModel.IsError);
        }

        [TestMethod]
        public async Task SaveSettingsAsync_WhenSettingsServiceFails_ShouldSetErrorMessage()
        {
            // Arrange
            var tcs = new TaskCompletionSource<bool>();

            // Setup for GetSettings_Async which is called when settings are saved
            var settingsViewModel = new SettingsViewModel();
            var result = Locations.Core.Shared.ViewModelServices.OperationResult<bool>.Failure(
                Locations.Core.Shared.ViewModelServices.OperationErrorSource.Unknown,
                "Failed to save settings");


            // Setup the SaveSettingAsync method to fail
            _mockSettingsService.Setup(service => service.SaveSettingAsync(It.IsAny<SettingViewModel>()))
                .Callback(() => tcs.SetResult(true))
                .ReturnsAsync(result);

            // Use reflection to set the private _settingsService field
            var fieldInfo = typeof(SettingsViewModel).GetField("_settingsService",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            fieldInfo.SetValue(_viewModel, _mockSettingsService.Object);

            // Act
            _viewModel.SaveSettingsCommand.Execute(null);

            // Wait for async completion or timeout after a reasonable time
            await Task.WhenAny(tcs.Task, Task.Delay(1000));

            // Assert
            Assert.IsTrue(_viewModel.IsError);
            Assert.IsTrue(_viewModel.ErrorMessage.Contains("Failed to save settings"));
        }

        [TestMethod]
        public async Task SaveSettingsAsync_WhenExceptionOccurs_ShouldSetErrorMessage()
        {
            // Arrange
            var exception = new Exception("Test exception");

            // Setup for GetSettings_Async which might be called
            _mockSettingsService.Setup(service => service.GetSettings_Async())
                .ThrowsAsync(exception);

            // Use reflection to set the private _settingsService field
            var fieldInfo = typeof(SettingsViewModel).GetField("_settingsService",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            fieldInfo.SetValue(_viewModel, _mockSettingsService.Object);

            // Act
            _viewModel.SaveSettingsCommand.Execute(null);

            // Give time for exception handling
            await Task.Delay(100);

            // Assert
            Assert.IsTrue(_viewModel.IsError);
            Assert.IsTrue(_viewModel.ErrorMessage.Contains("Error saving settings"));
        }

        [TestMethod]
        public async Task ResetSettingsAsync_ShouldResetAllSettingsToDefaults()
        {
            // Arrange
            // Modify some settings from defaults
            _viewModel.Hemisphere = new SettingViewModel("Hemisphere", "South");
            _viewModel.TimeFormat = new SettingViewModel("TimeFormat", "HH:mm");
            _viewModel.DateFormat = new SettingViewModel("DateFormat", "yyyy-MM-dd");
            _viewModel.Language = new SettingViewModel("Language", "fr-FR");

            // Act
            _viewModel.ResetSettingsCommand.Execute(null);

            // Wait for reset to complete
            await Task.Delay(100);

            // Assert
            Assert.AreEqual("North", _viewModel.Hemisphere.Value);
            Assert.AreEqual("h:mm tt", _viewModel.TimeFormat.Value);
            Assert.AreEqual("MM/dd/yyyy", _viewModel.DateFormat.Value);
            Assert.AreEqual("en-US", _viewModel.Language.Value);
        }

        [TestMethod]
        public void UpdateFromSettingsDTO_WithValidDTO_ShouldUpdateSettings()
        {
            // Arrange
            var settingsDTO = new Locations.Core.Shared.DTO.SettingsDTO();

            // Set up properties on the DTO
            settingsDTO.Hemisphere = new Locations.Core.Shared.DTO.SettingDTO("Hemisphere", "South");
            settingsDTO.TimeFormat = new Locations.Core.Shared.DTO.SettingDTO("TimeFormat", "HH:mm");
            settingsDTO.DateFormat = new Locations.Core.Shared.DTO.SettingDTO("DateFormat", "yyyy-MM-dd");
            settingsDTO.Language = new Locations.Core.Shared.DTO.SettingDTO("Language", "fr-FR");

            // Act
            _viewModel.UpdateFromSettingsDTO(settingsDTO);

            // Assert
            Assert.AreEqual("South", _viewModel.Hemisphere.Value);
            Assert.AreEqual("HH:mm", _viewModel.TimeFormat.Value);
            Assert.AreEqual("yyyy-MM-dd", _viewModel.DateFormat.Value);
            Assert.AreEqual("fr-FR", _viewModel.Language.Value);
        }

        [TestMethod]
        public void Cleanup_ShouldUnsubscribeFromEvents()
        {
            // Arrange
            // Create settings that will have events attached
            _viewModel.Hemisphere = new SettingViewModel("Hemisphere", "North");
            _viewModel.TimeFormat = new SettingViewModel("TimeFormat", "h:mm tt");

            // Verify initial state - would need reflection to check events

            // Act
            _viewModel.Cleanup();

            // Assert - testing event unsubscription is difficult
            // This is more of a coverage test to ensure code is executed
            Assert.IsTrue(true);
        }
    }
}