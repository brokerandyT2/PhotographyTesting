using TechTalk.SpecFlow;
using System.Threading;
using Locations.Core.Business.BDD.Support;
using Locations.Core.Business.Tests.UITests.PageObjects.Authentication;
using Locations.Core.Business.Tests.UITests.PageObjects.Configuration;
using Locations.Core.Business.Tests.UITests.PageObjects.Shared;
using System;
using OpenQA.Selenium;
using TechTalk.SpecFlow.Infrastructure;
using Assert = NUnit.Framework.Assert;
using Locations.Core.Business.Tests.UITests;
using NUnit.Framework;

namespace Locations.Core.Business.BDD.StepDefinitions.Configuration
{
    [Binding]
    public class SettingsSteps
    {
        private readonly AppiumDriverWrapper _driverWrapper;
        private readonly ScenarioContext _scenarioContext;
        private SettingsPage _settingsPage;
        private LoginPage _loginPage;

        // Store original settings to restore after tests
        private bool _originalHemisphere;
        private bool _originalTimeFormat;
        private bool _originalDateFormat;
        private bool _originalWindDirection;
        private bool _originalTemperatureFormat;
        private bool _originalAdSupport;

        public SettingsSteps(AppiumDriverWrapper driverWrapper, ScenarioContext scenarioContext)
        {
            _driverWrapper = driverWrapper;
            _scenarioContext = scenarioContext;
            _settingsPage = new SettingsPage(_driverWrapper.Driver, _driverWrapper.Platform);
            _loginPage = new LoginPage(_driverWrapper.Driver, _driverWrapper.Platform);
        }

        // Helper method for pending steps
        private void MarkAsPending(string message = "This step is not yet implemented")
        {
            throw new PendingStepException(message);
        }

        [Given(@"I am on the settings page")]
        public void GivenIAmOnTheSettingsPage()
        {
            // Navigate to settings page if not already there
            if (!_settingsPage.IsCurrentPage())
            {
                // Implementation depends on how navigation is structured
                try
                {
                    // Look for settings/cog button - implementation will vary by platform
                    switch (_driverWrapper.Platform)
                    {
                        case AppiumSetup.Platform.Android:
                            _driverWrapper.Driver.FindElement(By.XPath("//android.widget.Button[@content-desc='cogbox.png']")).Click();
                            break;
                        case AppiumSetup.Platform.iOS:
                            _driverWrapper.Driver.FindElement(By.XPath("//XCUIElementTypeButton[@name='cogbox.png']")).Click();
                            break;
                        case AppiumSetup.Platform.Windows:
                            _driverWrapper.Driver.FindElement(By.XPath("//Button[@AutomationId='cogbox.png']")).Click();
                            break;
                    }

                    // Wait for settings page to load
                    Thread.Sleep(2000);
                }
                catch (Exception ex)
                {
                    Assert.Fail($"Failed to navigate to settings: {ex.Message}");
                }
            }

            Assert.That(_settingsPage.IsCurrentPage(), Is.True, "Not on the settings page");

            // Store original settings for restoration after tests
            StoreOriginalSettings();
        }

        private void StoreOriginalSettings()
        {
            // This is a placeholder - actual implementation would depend on how
            // to access the current state of toggles in the SettingsPage

            // Example:
            // _originalHemisphere = _settingsPage.GetHemisphereState();
            // _originalTimeFormat = _settingsPage.GetTimeFormatState();
            // etc.
        }

        [When(@"I toggle the hemisphere setting to ""(.*)""")]
        public void WhenIToggleTheHemisphereSettingTo(string hemisphere)
        {
            bool isNorth = hemisphere.Equals("North", StringComparison.OrdinalIgnoreCase);
            _settingsPage.ToggleHemisphere(isNorth);
        }

        [When(@"I toggle the time format setting to ""(.*)""")]
        public void WhenIToggleTheTimeFormatSettingTo(string timeFormat)
        {
            bool is12Hour = timeFormat.Equals("12-hour", StringComparison.OrdinalIgnoreCase);
            _settingsPage.ToggleTimeFormat(is12Hour);
        }

        [When(@"I toggle the date format setting to ""(.*)""")]
        public void WhenIToggleTheDateFormatSettingTo(string dateFormat)
        {
            bool isMMDDYYYY = dateFormat.Equals("MM/DD/YYYY", StringComparison.OrdinalIgnoreCase);
            _settingsPage.ToggleDateFormat(isMMDDYYYY);
        }

        [When(@"I toggle the wind direction setting to ""(.*)""")]
        public void WhenIToggleTheWindDirectionSettingTo(string windDirection)
        {
            bool isTowardsWind = windDirection.Equals("Towards Wind", StringComparison.OrdinalIgnoreCase);
            _settingsPage.ToggleWindDirection(isTowardsWind);
        }

        [When(@"I toggle the temperature format setting to ""(.*)""")]
        public void WhenIToggleTheTemperatureFormatSettingTo(string temperatureFormat)
        {
            bool isFahrenheit = temperatureFormat.Equals("Fahrenheit", StringComparison.OrdinalIgnoreCase);
            _settingsPage.ToggleTemperatureFormat(isFahrenheit);
        }

        [When(@"I toggle the ad support setting to ""(.*)""")]
        public void WhenIToggleTheAdSupportSettingTo(string adSupport)
        {
            bool isEnabled = adSupport.Equals("Enabled", StringComparison.OrdinalIgnoreCase);
            _settingsPage.ToggleAdSupport(isEnabled);
        }

        [When(@"I set all settings to custom values")]
        public void WhenISetAllSettingsToCustomValues()
        {
            // Set all settings to specific values for testing
            _settingsPage.ToggleHemisphere(true); // North
            _settingsPage.ToggleTimeFormat(true); // 12-hour
            _settingsPage.ToggleDateFormat(true); // MM/DD/YYYY
            _settingsPage.ToggleWindDirection(true); // Towards Wind
            _settingsPage.ToggleTemperatureFormat(true); // Fahrenheit
            _settingsPage.ToggleAdSupport(false); // Disabled
        }

        [When(@"I close and reopen the application")]
        public void WhenICloseAndReopenTheApplication()
        {
            // This is challenging to implement in automated tests
            // For now, we'll just simulate by navigating away and back

            // Navigate away
            try
            {
                // Look for a back button or other navigation element
                _driverWrapper.Driver.Navigate().Back();
                Thread.Sleep(2000);

                // Navigate back to settings
                GivenIAmOnTheSettingsPage();
            }
            catch (Exception ex)
            {
                Assert.Fail($"Failed to simulate app restart: {ex.Message}");
            }
        }

        [Then(@"the hemisphere value should be saved as ""(.*)""")]
        public void ThenTheHemisphereValueShouldBeSavedAs(string hemisphere)
        {
            bool expectedValue = hemisphere.Equals("North", StringComparison.OrdinalIgnoreCase);

            // Refresh settings page to ensure values are loaded from storage
            // This might require navigating away and back
            WhenICloseAndReopenTheApplication();

            // Verify the setting using VerifySettingsApplied
            Assert.That(_settingsPage.VerifySettingsApplied(
                north: expectedValue,
                useUSTimeFormat: null,
                useUSDateFormat: null,
                towardsWind: null,
                useFahrenheit: null,
                adSupport: null
            ), Is.True, $"Hemisphere setting was not saved as {hemisphere}");
        }

        [Then(@"the time format value should be saved as ""(.*)""")]
        public void ThenTheTimeFormatValueShouldBeSavedAs(string timeFormat)
        {
            bool expectedValue = timeFormat.Equals("12-hour", StringComparison.OrdinalIgnoreCase);

            // Refresh settings page to ensure values are loaded from storage
            WhenICloseAndReopenTheApplication();

            // Verify the setting
            Assert.That(_settingsPage.VerifySettingsApplied(
                north: null,
                useUSTimeFormat: expectedValue,
                useUSDateFormat: null,
                towardsWind: null,
                useFahrenheit: null,
                adSupport: null
            ), Is.True, $"Time format setting was not saved as {timeFormat}");
        }

        [Then(@"the date format value should be saved as ""(.*)""")]
        public void ThenTheDateFormatValueShouldBeSavedAs(string dateFormat)
        {
            bool expectedValue = dateFormat.Equals("MM/DD/YYYY", StringComparison.OrdinalIgnoreCase);

            // Refresh settings page to ensure values are loaded from storage
            WhenICloseAndReopenTheApplication();

            // Verify the setting
            Assert.That(_settingsPage.VerifySettingsApplied(
                north: null,
                useUSTimeFormat: null,
                useUSDateFormat: expectedValue,
                towardsWind: null,
                useFahrenheit: null,
                adSupport: null
            ), Is.True, $"Date format setting was not saved as {dateFormat}");
        }

        [Then(@"the wind direction value should be saved as ""(.*)""")]
        public void ThenTheWindDirectionValueShouldBeSavedAs(string windDirection)
        {
            bool expectedValue = windDirection.Equals("Towards Wind", StringComparison.OrdinalIgnoreCase);

            // Refresh settings page to ensure values are loaded from storage
            WhenICloseAndReopenTheApplication();

            // Verify the setting
            Assert.That(_settingsPage.VerifySettingsApplied(
                north: null,
                useUSTimeFormat: null,
                useUSDateFormat: null,
                towardsWind: expectedValue,
                useFahrenheit: null,
                adSupport: null
            ), Is.True, $"Wind direction setting was not saved as {windDirection}");
        }

        [Then(@"the temperature format value should be saved as ""(.*)""")]
        public void ThenTheTemperatureFormatValueShouldBeSavedAs(string temperatureFormat)
        {
            bool expectedValue = temperatureFormat.Equals("Fahrenheit", StringComparison.OrdinalIgnoreCase);

            // Refresh settings page to ensure values are loaded from storage
            WhenICloseAndReopenTheApplication();

            // Verify the setting
            Assert.That(_settingsPage.VerifySettingsApplied(
                north: null,
                useUSTimeFormat: null,
                useUSDateFormat: null,
                towardsWind: null,
                useFahrenheit: expectedValue,
                adSupport: null
            ), Is.True, $"Temperature format setting was not saved as {temperatureFormat}");
        }

        [Then(@"the ad support value should be saved as ""(.*)""")]
        public void ThenTheAdSupportValueShouldBeSavedAs(string adSupport)
        {
            bool expectedValue = adSupport.Equals("Enabled", StringComparison.OrdinalIgnoreCase);

            // Refresh settings page to ensure values are loaded from storage
            WhenICloseAndReopenTheApplication();

            // Verify the setting
            Assert.That(_settingsPage.VerifySettingsApplied(
                north: null,
                useUSTimeFormat: null,
                useUSDateFormat: null,
                towardsWind: null,
                useFahrenheit: null,
                adSupport: expectedValue
            ), Is.True, $"Ad support setting was not saved as {adSupport}");
        }

        [Then(@"all my custom settings should be preserved")]
        public void ThenAllMyCustomSettingsShouldBePreserved()
        {
            // Verify all settings match the custom values set earlier
            Assert.That(_settingsPage.VerifySettingsApplied(
                north: true, // North
                useUSTimeFormat: true, // 12-hour
                useUSDateFormat: true, // MM/DD/YYYY
                towardsWind: true, // Towards Wind
                useFahrenheit: true, // Fahrenheit
                adSupport: false // Disabled
            ), Is.True, "Custom settings were not preserved");
        }
    }
}