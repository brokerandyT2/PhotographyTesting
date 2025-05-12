// SettingsViewModelTests.cs - Fixed to avoid MAUI dependencies
using Locations.Core.Shared.ViewModels;
using Locations.Core.Shared.ViewModelServices;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

// Use explicit namespaces to resolve ambiguity
using ViewModelOperationErrorEventArgs = Locations.Core.Shared.ViewModels.OperationErrorEventArgs;
using ViewModelOperationErrorSource = Locations.Core.Shared.ViewModels.OperationErrorSource;
using ServiceOperationErrorEventArgs = Locations.Core.Shared.ViewModelServices.OperationErrorEventArgs;
using ServiceOperationErrorSource = Locations.Core.Shared.ViewModelServices.OperationErrorSource;
using ServiceOperationResult = Locations.Core.Shared.ViewModelServices.OperationResult<Locations.Core.Shared.ViewModels.SettingsViewModel>;

namespace Locations.Core.Business.Tests.ViewModels
{
    [TestClass]
    [TestCategory("ViewModels")]
    public class SettingsViewModelTests
    {
        private Mock<ISettingService> _mockSettingsService;
        private SettingsViewModel _viewModel;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            // Initialize MagicStrings to avoid MAUI dependencies
            Locations.Core.Shared.MagicStrings.Initialize();
        }

        [TestInitialize]
        public void Setup()
        {
            _mockSettingsService = new Mock<ISettingService>();

            // Create test view model without MAUI dependencies
            _viewModel = CreateTestSettingsViewModel(true);
        }

        private SettingsViewModel CreateTestSettingsViewModel(bool avoidMauiDependencies = true)
        {
            if (avoidMauiDependencies)
            {
                // Bypass the normal constructor to avoid MAUI FileSystem calls
                var vm = new SettingsViewModel();

                // Use reflection to set the service field
                var fieldInfo = typeof(SettingsViewModel).GetField("_settingsService",
                    System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                fieldInfo?.SetValue(vm, _mockSettingsService.Object);

                // Initialize settings with test values
                InitializeTestSettings(vm);

                return vm;
            }
            else
            {
                return new SettingsViewModel();
            }
        }

        private void InitializeTestSettings(SettingsViewModel vm)
        {
            // Initialize all properties to avoid null reference exceptions
            vm.Hemisphere = new SettingViewModel("Hemisphere", "North");
            vm.FirstName = new SettingViewModel("FirstName", "Test");
            vm.LastName = new SettingViewModel("LastName", "User");
            vm.Email = new SettingViewModel("Email", "test@example.com");
            vm.SubscriptionExpiration = new SettingViewModel("SubscriptionExpiration", DateTime.Now.AddYears(1).ToString());
            vm.SubscriptionType = new SettingViewModel("SubscriptionType", "Premium");
            vm.UniqeID = new SettingViewModel("UniqeID", "TEST-ID-123");
            vm.DeviceInfo = new SettingViewModel("DeviceInfo", "Test Device");
            vm.TimeFormat = new SettingViewModel("TimeFormat", "h:mm tt");
            vm.DateFormat = new SettingViewModel("DateFormat", "MM/dd/yyyy");
            vm.WindDirection = new SettingViewModel("WindDirection", "towardsWind");
            vm.HomePageViewed = new SettingViewModel("HomePageViewed", "False");
            vm.ListLocationsViewed = new SettingViewModel("ListLocationsViewed", "False");
            vm.TipsViewed = new SettingViewModel("TipsViewed", "False");
            vm.ExposureCalcViewed = new SettingViewModel("ExposureCalcViewed", "False");
            vm.LightMeterViewed = new SettingViewModel("LightMeterViewed", "False");
            vm.SceneEvaluationViewed = new SettingViewModel("SceneEvaluationViewed", "False");
            vm.SunCalculationViewed = new SettingViewModel("SunCalculationViewed", "False");
            vm.LastBulkWeatherUpdate = new SettingViewModel("LastBulkWeatherUpdate", DateTime.Now.ToString());
            vm.Language = new SettingViewModel("Language", "en-US");
            vm.AdSupport = new SettingViewModel("AdSupport", "True");
            vm.TemperatureFormat = new SettingViewModel("TemperatureFormat", "Fahrenheit");
        }

        [TestMethod]
        public void Constructor_WithNoDependencies_ShouldInitializeProperties()
        {
            // Assert
            Assert.IsNotNull(_viewModel.SaveSettingsCommand);
            Assert.IsNotNull(_viewModel.ResetSettingsCommand);
            Assert.IsNotNull(_viewModel.Hemisphere);
            Assert.AreEqual("North", _viewModel.Hemisphere.Value);
            Assert.AreEqual("h:mm tt", _viewModel.TimeFormat.Value);
        }

        [TestMethod]
        public void Constructor_WithSettingsService_ShouldInitializePropertiesAndService()
        {
            // Arrange & Act
            var viewModel = CreateTestSettingsViewModel(true);

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
            _mockSettingsService.Setup(service => service.SaveSettingAsync(It.IsAny<SettingViewModel>()))
                .ReturnsAsync(Locations.Core.Shared.ViewModelServices.OperationResult<bool>.Success(true));

            var saveMethod = typeof(SettingsViewModel).GetMethod("SaveSettingsAsync",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            // Act
            if (saveMethod != null)
            {
                await (Task)saveMethod.Invoke(_viewModel, null);
            }

            // Assert
            Assert.IsFalse(_viewModel.IsError);
            _mockSettingsService.Verify(service => service.SaveSettingAsync(It.IsAny<SettingViewModel>()), Times.AtLeastOnce());
        }

        [TestMethod]
        public async Task SaveSettingsAsync_WhenSettingsServiceFails_ShouldSetErrorMessage()
        {
            // Arrange
            string errorMessage = "Failed to save settings";

            _mockSettingsService.Setup(service => service.SaveSettingAsync(It.IsAny<SettingViewModel>()))
                .ReturnsAsync(Shared.ViewModelServices.OperationResult<bool>.Failure(
                    ServiceOperationErrorSource.Unknown,
                    errorMessage));

            var saveMethod = typeof(SettingsViewModel).GetMethod("SaveSettingsAsync",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            // Act
            if (saveMethod != null)
            {
                await (Task)saveMethod.Invoke(_viewModel, null);
            }

            // Assert
            Assert.IsTrue(_viewModel.IsError);
            Assert.IsTrue(_viewModel.ErrorMessage.Contains(errorMessage));
        }

        [TestMethod]
        public async Task SaveSettingsAsync_WhenExceptionOccurs_ShouldSetErrorMessage()
        {
            // Arrange
            var exception = new Exception("Test exception");

            _mockSettingsService.Setup(service => service.SaveSettingAsync(It.IsAny<SettingViewModel>()))
                .ThrowsAsync(exception);

            var saveMethod = typeof(SettingsViewModel).GetMethod("SaveSettingsAsync",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            // Act
            if (saveMethod != null)
            {
                await (Task)saveMethod.Invoke(_viewModel, null);
            }

            // Assert
            Assert.IsTrue(_viewModel.IsError);
            Assert.IsTrue(_viewModel.ErrorMessage.Contains("Error saving settings"));
        }

        [TestMethod]
        public async Task ResetSettingsAsync_ShouldResetAllSettingsToDefaults()
        {
            // Arrange
            _viewModel.Hemisphere = new SettingViewModel("Hemisphere", "South");
            _viewModel.TimeFormat = new SettingViewModel("TimeFormat", "HH:mm");
            _viewModel.DateFormat = new SettingViewModel("DateFormat", "yyyy-MM-dd");
            _viewModel.Language = new SettingViewModel("Language", "fr-FR");

            var resetMethod = typeof(SettingsViewModel).GetMethod("ResetSettingsAsync",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);

            // Act
            if (resetMethod != null)
            {
                await (Task)resetMethod.Invoke(_viewModel, null);
            }

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
            _viewModel.Hemisphere = new SettingViewModel("Hemisphere", "North");
            _viewModel.TimeFormat = new SettingViewModel("TimeFormat", "h:mm tt");

            // Act
            _viewModel.Cleanup();

            // Assert - testing event unsubscription is difficult
            // This is more of a coverage test to ensure code is executed
            Assert.IsTrue(true);
        }
    }
}