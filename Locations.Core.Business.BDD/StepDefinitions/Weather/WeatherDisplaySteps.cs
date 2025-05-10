using TechTalk.SpecFlow;
using System.Threading;
using Locations.Core.Business.BDD.Support;
using Locations.Core.Business.Tests.UITests.PageObjects.Authentication;
using Locations.Core.Business.Tests.UITests.PageObjects.Locations;
using Locations.Core.Business.Tests.UITests.PageObjects.Weather;
using Locations.Core.Business.Tests.UITests.PageObjects.Shared;
using TechTalk.SpecFlow.Infrastructure;
using Assert = NUnit.Framework.Assert;
using Locations.Core.Business.Tests.UITests.PageObjects.Configuration;
using NUnit.Framework;

namespace Locations.Core.Business.BDD.StepDefinitions.Weather
{
    [Binding]
    public class WeatherDisplaySteps
    {
        private readonly AppiumDriverWrapper _driverWrapper;
        private readonly ScenarioContext _scenarioContext;
        private WeatherDisplayPage _weatherDisplayPage;
        private EditLocationPage _editLocationPage;
        private ListLocationsPage _listLocationsPage;
        private SettingsPage _settingsPage;

        public WeatherDisplaySteps(AppiumDriverWrapper driverWrapper, ScenarioContext scenarioContext)
        {
            _driverWrapper = driverWrapper;
            _scenarioContext = scenarioContext;
            _weatherDisplayPage = new WeatherDisplayPage(_driverWrapper.Driver, _driverWrapper.Platform);
            _editLocationPage = new EditLocationPage(_driverWrapper.Driver, _driverWrapper.Platform);
            _listLocationsPage = new ListLocationsPage(_driverWrapper.Driver, _driverWrapper.Platform);
        }

        // Helper method for pending steps
        private void MarkAsPending(string message = "This step is not yet implemented")
        {
            throw new PendingStepException(message);
        }

        [Given(@"I am viewing the weather for a location")]
        public void GivenIAmViewingTheWeatherForALocation()
        {
            // Navigate to weather display if not already there
            if (!_weatherDisplayPage.IsCurrentPage())
            {
                // First ensure we're on the edit location page
                if (!_editLocationPage.IsCurrentPage())
                {
                    // If on locations list, select the first location
                    if (_listLocationsPage.IsCurrentPage())
                    {
                        _listLocationsPage.SelectLocation(0);
                        Thread.Sleep(2000); // Wait for navigation
                    }
                    else
                    {
                        // Can't navigate from here
                        Assert.Fail("Cannot navigate to weather display from current state");
                    }
                }

                // Now we should be on edit location page, click weather button
                Assert.That(_editLocationPage.IsCurrentPage(), Is.True, "Not on edit location page");
                _editLocationPage.ClickWeatherButton();
                Thread.Sleep(2000); // Wait for navigation
            }

            Assert.That(_weatherDisplayPage.IsCurrentPage(), Is.True, "Not on weather display page");
        }

        [Given(@"my temperature preference is set to ""(.*)""")]
        public void GivenMyTemperaturePreferenceIsSetTo(string temperatureFormat)
        {
            // Navigate to settings and set temperature format
            // Need to implement navigation to settings
            _settingsPage = new SettingsPage(_driverWrapper.Driver, _driverWrapper.Platform);

            bool setToFahrenheit = temperatureFormat == "Fahrenheit";
            _settingsPage.ToggleTemperatureFormat(setToFahrenheit);

            // Return to weather display
            // Need to implement navigation back
            GivenIAmViewingTheWeatherForALocation();
        }

        [Given(@"my wind direction preference is set to ""(.*)""")]
        public void GivenMyWindDirectionPreferenceIsSetTo(string windDirection)
        {
            // Navigate to settings and set wind direction
            // Need to implement navigation to settings
            _settingsPage = new SettingsPage(_driverWrapper.Driver, _driverWrapper.Platform);

            bool setToTowardsWind = windDirection == "Towards Wind";
            _settingsPage.ToggleWindDirection(setToTowardsWind);

            // Return to weather display
            // Need to implement navigation back
            GivenIAmViewingTheWeatherForALocation();
        }

        [When(@"I expand day one forecast details")]
        public void WhenIExpandDayOneForecastDetails()
        {
            _weatherDisplayPage.ExpandDayOneDetails();
        }

        [When(@"I view the weather display")]
        public void WhenIViewTheWeatherDisplay()
        {
            Assert.That(_weatherDisplayPage.IsCurrentPage(), Is.True, "Not on weather display page");
        }

        [When(@"I expand day two forecast")]
        public void WhenIExpandDayTwoForecast()
        {
            _weatherDisplayPage.ExpandDayTwo();
        }

        [When(@"I expand day three forecast")]
        public void WhenIExpandDayThreeForecast()
        {
            _weatherDisplayPage.ExpandDayThree();
        }

        [When(@"I tap the close button")]
        public void WhenITapTheCloseButton()
        {
            _weatherDisplayPage.ClickClose();
            Thread.Sleep(2000); // Wait for navigation
        }

        [Then(@"I should see the current temperature")]
        public void ThenIShouldSeeTheCurrentTemperature()
        {
            // Implementation depends on how temperature is exposed in page object
            string highTemp = _weatherDisplayPage.GetDayOneHighTemperature();
            string lowTemp = _weatherDisplayPage.GetDayOneLowTemperature();

            Assert.That(string.IsNullOrEmpty(highTemp), Is.False, "High temperature not displayed");
            Assert.That(string.IsNullOrEmpty(lowTemp), Is.False, "Low temperature not displayed");
        }

        [Then(@"I should see the weather condition description")]
        public void ThenIShouldSeeTheWeatherConditionDescription()
        {
            string forecast = _weatherDisplayPage.GetDayOneForecast();
            Assert.That(string.IsNullOrEmpty(forecast), Is.False, "Weather condition description not displayed");
        }

        [Then(@"I should see the daily high and low temperatures")]
        public void ThenIShouldSeeTheDailyHighAndLowTemperatures()
        {
            string highTemp = _weatherDisplayPage.GetDayOneHighTemperature();
            string lowTemp = _weatherDisplayPage.GetDayOneLowTemperature();

            Assert.That(string.IsNullOrEmpty(highTemp), Is.False, "High temperature not displayed");
            Assert.That(string.IsNullOrEmpty(lowTemp), Is.False, "Low temperature not displayed");
        }

        [Then(@"I should see additional weather information")]
        public void ThenIShouldSeeAdditionalWeatherInformation()
        {
            // This could check for any additional information elements
            string windSpeed = _weatherDisplayPage.GetWindSpeed();
            Assert.That(string.IsNullOrEmpty(windSpeed), Is.False, "Additional weather info (wind speed) not displayed");
        }

        [Then(@"I should see wind speed and direction")]
        public void ThenIShouldSeeWindSpeedAndDirection()
        {
            string windSpeed = _weatherDisplayPage.GetWindSpeed();
            double windDirection = _weatherDisplayPage.GetWindDirection();

            Assert.That(string.IsNullOrEmpty(windSpeed), Is.False, "Wind speed not displayed");
            Assert.That(windDirection >= 0, Is.True, "Wind direction not displayed");
        }

        [Then(@"I should see humidity and pressure data")]
        public void ThenIShouldSeeHumidityAndPressureData()
        {
            // Need to add these to the page object if not already there
            MarkAsPending("Humidity and pressure data verification not implemented");
        }

        [Then(@"I should see day two's weather details")]
        public void ThenIShouldSeeDayTwosWeatherDetails()
        {
            // Implementation depends on how day two details are exposed in page object
            MarkAsPending("Day two weather details verification not implemented");
        }

        [Then(@"I should see day three's weather details")]
        public void ThenIShouldSeeDayThreesWeatherDetails()
        {
            // Implementation depends on how day three details are exposed in page object
            MarkAsPending("Day three weather details verification not implemented");
        }

        [Then(@"I should see temperatures in Fahrenheit units")]
        public void ThenIShouldSeeTemperaturesInFahrenheitUnits()
        {
            string highTemp = _weatherDisplayPage.GetDayOneHighTemperature();
            // Check if the temperature string contains the Fahrenheit symbol (°F)
            Assert.That(highTemp.Contains("F") || highTemp.Contains("°F"), Is.True, "Temperature not displayed in Fahrenheit units");
        }

        [Then(@"I should see temperatures in Celsius units")]
        public void ThenIShouldSeeTemperaturesInCelsiusUnits()
        {
            string highTemp = _weatherDisplayPage.GetDayOneHighTemperature();
            // Check if the temperature string contains the Celsius symbol (°C)
            Assert.That(highTemp.Contains("C") || highTemp.Contains("°C"), Is.True, "Temperature not displayed in Celsius units");
        }

        [Then(@"the wind direction arrow should point towards the wind")]
        public void ThenTheWindDirectionArrowShouldPointTowardsTheWind()
        {
            // Implementation depends on how wind direction is determined in the UI
            MarkAsPending("Wind direction arrow verification not implemented");
        }

        [Then(@"the wind direction arrow should point with the wind")]
        public void ThenTheWindDirectionArrowShouldPointWithTheWind()
        {
            // Implementation depends on how wind direction is determined in the UI
            MarkAsPending("Wind direction arrow verification not implemented");
        }

        [Then(@"I should be returned to the location details page")]
        public void ThenIShouldBeReturnedToTheLocationDetailsPage()
        {
            Assert.That(_editLocationPage.IsCurrentPage(), Is.True, "Not returned to the location details page");
        }
    }
}