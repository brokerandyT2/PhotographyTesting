using Locations.Core.Business.BDD.TestHelpers;
using Locations.Core.Business.DataAccess.Interfaces;
using Locations.Core.Data.Models;
using Locations.Core.Data.Queries.Interfaces;
using Locations.Core.Shared;
using Locations.Core.Shared.ViewModels;
using Moq;
using NUnit.Framework;
using TechTalk.SpecFlow;
using Assert = NUnit.Framework.Assert;

namespace Locations.Core.Business.BDD.StepDefinitions.Configuration
{
    [Binding]
    public class SettingsSteps
    {
        private readonly ISettingService<SettingViewModel> _settingsService;
        private readonly BDDTestContext _testContext;
        private readonly Mock<ISettingsRepository> _mockSettingsRepository;
        private Dictionary<string, string> _currentSettings;

        public SettingsSteps(ISettingService<SettingViewModel> settingsService, BDDTestContext testContext, Mock<ISettingsRepository> mockSettingsRepository)
        {
            _settingsService = settingsService;
            _testContext = testContext;
            _mockSettingsRepository = mockSettingsRepository;
            _currentSettings = new Dictionary<string, string>();
        }

        [Given(@"I am on the settings page")]
        public void GivenIAmOnTheSettingsPage()
        {
            // In service tests, we just ensure we have access to settings
            _currentSettings.Clear();

            // Load current settings
            var allSettings = _settingsService.GetAllSettings();
            if (allSettings != null)
            {
                _currentSettings[MagicStrings.Hemisphere] = allSettings.Hemisphere?.Value ?? "North";
                _currentSettings[MagicStrings.TimeFormat] = allSettings.TimeFormat?.Value ?? "12-hour";
                _currentSettings[MagicStrings.DateFormat] = allSettings.DateFormat?.Value ?? "MM/DD/YYYY";
                _currentSettings[MagicStrings.WindDirection] = allSettings.WindDirection?.Value ?? "Towards Wind";
                _currentSettings[MagicStrings.TemperatureType] = allSettings.TemperatureFormat?.Value ?? "Fahrenheit";
                _currentSettings[MagicStrings.FreePremiumAdSupported] = allSettings.AdSupport?.Value ?? "false";
            }
        }

        [When(@"I toggle the hemisphere setting to ""(.*)""")]
        public async Task WhenIToggleTheHemisphereSettingTo(string hemisphere)
        {
            await SaveSettingAsync(MagicStrings.Hemisphere, hemisphere);
        }

        [When(@"I toggle the time format setting to ""(.*)""")]
        public async Task WhenIToggleTheTimeFormatSettingTo(string timeFormat)
        {
            await SaveSettingAsync(MagicStrings.TimeFormat, timeFormat);
        }

        [When(@"I toggle the date format setting to ""(.*)""")]
        public async Task WhenIToggleTheDateFormatSettingTo(string dateFormat)
        {
            await SaveSettingAsync(MagicStrings.DateFormat, dateFormat);
        }

        [When(@"I toggle the wind direction setting to ""(.*)""")]
        public async Task WhenIToggleTheWindDirectionSettingTo(string windDirection)
        {
            await SaveSettingAsync(MagicStrings.WindDirection, windDirection);
        }

        [When(@"I toggle the temperature format setting to ""(.*)""")]
        public async Task WhenIToggleTheTemperatureFormatSettingTo(string temperatureFormat)
        {
            await SaveSettingAsync(MagicStrings.TemperatureType, temperatureFormat);
        }

        [When(@"I toggle the ad support setting to ""(.*)""")]
        public async Task WhenIToggleTheAdSupportSettingTo(string adSupport)
        {
            var value = adSupport == "Enabled" ? "true" : "false";
            await SaveSettingAsync(MagicStrings.FreePremiumAdSupported, value);
        }

        [When(@"I set all settings to custom values")]
        public async Task WhenISetAllSettingsToCustomValues()
        {
            await SaveSettingAsync(MagicStrings.Hemisphere, "North");
            await SaveSettingAsync(MagicStrings.TimeFormat, "12-hour");
            await SaveSettingAsync(MagicStrings.DateFormat, "MM/DD/YYYY");
            await SaveSettingAsync(MagicStrings.WindDirection, "Towards Wind");
            await SaveSettingAsync(MagicStrings.TemperatureType, "Fahrenheit");
            await SaveSettingAsync(MagicStrings.FreePremiumAdSupported, "false");
        }

        [When(@"I close and reopen the application")]
        public async Task WhenICloseAndReopenTheApplication()
        {
            // In service tests, we simulate this by clearing in-memory data
            // and reloading from the repository
            _currentSettings.Clear();

            // Simulate app restart by retrieving settings again
            var allSettings = _settingsService.GetAllSettings();
            if (allSettings != null)
            {
                _currentSettings[MagicStrings.Hemisphere] = allSettings.Hemisphere?.Value ?? "";
                _currentSettings[MagicStrings.TimeFormat] = allSettings.TimeFormat?.Value ?? "";
                _currentSettings[MagicStrings.DateFormat] = allSettings.DateFormat?.Value ?? "";
                _currentSettings[MagicStrings.WindDirection] = allSettings.WindDirection?.Value ?? "";
                _currentSettings[MagicStrings.TemperatureType] = allSettings.TemperatureFormat?.Value ?? "";
                _currentSettings[MagicStrings.FreePremiumAdSupported] = allSettings.AdSupport?.Value ?? "";
            }
        }

        [Then(@"the hemisphere value should be saved as ""(.*)""")]
        public void ThenTheHemisphereValueShouldBeSavedAs(string hemisphere)
        {
            AssertSettingValue(MagicStrings.Hemisphere, hemisphere);
        }

        [Then(@"the time format value should be saved as ""(.*)""")]
        public void ThenTheTimeFormatValueShouldBeSavedAs(string timeFormat)
        {
            AssertSettingValue(MagicStrings.TimeFormat, timeFormat);
        }

        [Then(@"the date format value should be saved as ""(.*)""")]
        public void ThenTheDateFormatValueShouldBeSavedAs(string dateFormat)
        {
            AssertSettingValue(MagicStrings.DateFormat, dateFormat);
        }

        [Then(@"the wind direction value should be saved as ""(.*)""")]
        public void ThenTheWindDirectionValueShouldBeSavedAs(string windDirection)
        {
            AssertSettingValue(MagicStrings.WindDirection, windDirection);
        }

        [Then(@"the temperature format value should be saved as ""(.*)""")]
        public void ThenTheTemperatureFormatValueShouldBeSavedAs(string temperatureFormat)
        {
            AssertSettingValue(MagicStrings.TemperatureType, temperatureFormat);
        }

        [Then(@"the ad support value should be saved as ""(.*)""")]
        public void ThenTheAdSupportValueShouldBeSavedAs(string adSupport)
        {
            var expectedValue = adSupport == "Enabled" ? "true" : "false";
            AssertSettingValue(MagicStrings.FreePremiumAdSupported, expectedValue);
        }

        [Then(@"all my custom settings should be preserved")]
        public void ThenAllMyCustomSettingsShouldBePreserved()
        {
            AssertSettingValue(MagicStrings.Hemisphere, "North");
            AssertSettingValue(MagicStrings.TimeFormat, "12-hour");
            AssertSettingValue(MagicStrings.DateFormat, "MM/DD/YYYY");
            AssertSettingValue(MagicStrings.WindDirection, "Towards Wind");
            AssertSettingValue(MagicStrings.TemperatureType, "Fahrenheit");
            AssertSettingValue(MagicStrings.FreePremiumAdSupported, "false");
        }

        private async Task SaveSettingAsync(string key, string value)
        {
            var setting = new SettingViewModel { Key = key, Value = value };

            // Update mock to return the saved value
            _mockSettingsRepository.Setup(x => x.GetByNameAsync(key))
                .ReturnsAsync(DataOperationResult<SettingViewModel>.Success(setting));

            var result = await _settingsService.SaveAsync(setting);
            Assert.That(result.IsSuccess, Is.True, $"Failed to save setting {key}");

            _currentSettings[key] = value;
        }

        private void AssertSettingValue(string key, string expectedValue)
        {
            var setting = _settingsService.GetSettingByName(key);
            Assert.That(setting, Is.Not.Null, $"Setting {key} not found");
            Assert.That(setting.Value, Is.EqualTo(expectedValue),
                $"Setting {key} has value '{setting.Value}' but expected '{expectedValue}'");
        }
    }
}