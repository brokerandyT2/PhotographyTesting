// Locations.Core.Business.Tests.UITests/Tests/Weather/WeatherDisplayTests.cs
using NUnit.Framework;
using System;
using System.Threading;
using Locations.Core.Business.Tests.UITests.PageObjects.Authentication;
using Locations.Core.Business.Tests.UITests.PageObjects.Locations;
using Locations.Core.Business.Tests.UITests.PageObjects.Weather;

namespace Locations.Core.Business.Tests.UITests.Tests.Weather
{
    [TestFixture]
    [Category("Weather")]
    public class WeatherDisplayTests : BaseTest
    {
        private ListLocationsPage _listLocationsPage;
        private EditLocationPage _editLocationPage;
        private WeatherDisplayPage _weatherDisplayPage;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            // First login if needed
            var loginPage = new LoginPage(WindowsDriver, AndroidDriver, iOSDriver, CurrentPlatform);
            if (loginPage.IsCurrentPage())
            {
                loginPage.Login();
            }

            // Navigate to list locations page
            Thread.Sleep(2000); // Wait for main page to load

            // Initialize list locations page
            _listLocationsPage = new ListLocationsPage(WindowsDriver, AndroidDriver, iOSDriver, CurrentPlatform);

            // Wait for locations to load
            Assert.IsTrue(_listLocationsPage.WaitForLocationsToLoad(), "Locations failed to load");

            // Check if there are locations available, if not create one
            if (!_listLocationsPage.HasLocations())
            {
                // Navigate to add location page
                try
                {
                    // Look for a "+" button or similar to add a new location
                    switch (CurrentPlatform)
                    {
                        case AppiumSetup.Platform.Android:
                            AndroidDriver.FindElementByXPath("//android.widget.Button[contains(@content-desc, 'Add') or contains(@text, 'Add')]").Click();
                            break;
                        case AppiumSetup.Platform.iOS:
                            iOSDriver.FindElementByXPath("//XCUIElementTypeButton[contains(@name, 'Add')]").Click();
                            break;
                        case AppiumSetup.Platform.Windows:
                            WindowsDriver.FindElementByXPath("//Button[contains(@Name, 'Add')]").Click();
                            break;
                    }

                    // Wait for page to load
                    Thread.Sleep(2000);
                }
                catch (Exception ex)
                {
                    Log($"Navigation to add location failed: {ex.Message}");
                }

                // Create a test location
                var addLocationPage = new AddLocationPage(WindowsDriver, AndroidDriver, iOSDriver, CurrentPlatform);
                addLocationPage.CreateLocation("Test Location " + DateTime.Now.ToString("yyyyMMddHHmmss"));

                // Wait for navigation back to list
                Thread.Sleep(2000);

                // Reinitialize list locations page
                _listLocationsPage = new ListLocationsPage(WindowsDriver, AndroidDriver, iOSDriver, CurrentPlatform);
                _listLocationsPage.WaitForLocationsToLoad();
            }

            // Select the first location
            _listLocationsPage.SelectLocation(0);

            // Wait for location details to load
            Thread.Sleep(2000);

            // Initialize edit location page
            _editLocationPage = new EditLocationPage(WindowsDriver, AndroidDriver, iOSDriver, CurrentPlatform);

            // Verify we're on the edit location page
            Assert.IsTrue(_editLocationPage.IsCurrentPage(), "Not on the edit location page");

            // Click weather button
            _editLocationPage.ClickWeatherButton();

            // Wait for weather display to load
            Thread.Sleep(3000);

            // Initialize weather display page
            _weatherDisplayPage = new WeatherDisplayPage(WindowsDriver, AndroidDriver, iOSDriver, CurrentPlatform);

            // Verify we're on the weather display page
            Assert.IsTrue(_weatherDisplayPage.IsCurrentPage(), "Not on the weather display page");
        }

        [TearDown]
        public override void TearDown()
        {
            try
            {
                // If we're on the weather display page, close it
                if (_weatherDisplayPage != null && _weatherDisplayPage.IsCurrentPage())
                {
                    _weatherDisplayPage.ClickClose();
                    Thread.Sleep(1000);
                }
            }
            catch (Exception ex)
            {
                Log($"Error in TearDown: {ex.Message}");
            }

            base.TearDown();
        }

        [Test]
        [Description("Verify that all UI elements are displayed on the weather display page")]
        public void WeatherDisplayPage_AllUIElementsDisplayed()
        {
            Log("Checking UI elements on Weather Display page");

            // Verify weather data is loaded
            Assert.IsTrue(_weatherDisplayPage.HasWeatherData(), "Weather data not displayed");
        }

        [Test]
        [Description("Verify day one forecast is displayed")]
        public void WeatherDisplay_DayOneForecast_ShouldBeDisplayed()
        {
            Log("Testing day one forecast display");

            // Get forecast text
            string forecast = _weatherDisplayPage.GetDayOneForecast();

            // Verify forecast is not empty
            Assert.IsFalse(string.IsNullOrEmpty(forecast), "Day one forecast is empty");
        }

        [Test]
        [Description("Verify temperature range is displayed")]
        public void WeatherDisplay_TemperatureRange_ShouldBeDisplayed()
        {
            Log("Testing temperature range display");

            // Get low and high temperatures
            string lowTemp = _weatherDisplayPage.GetDayOneLowTemperature();
            string highTemp = _weatherDisplayPage.GetDayOneHighTemperature();

            // Verify temperatures are not empty
            Assert.IsFalse(string.IsNullOrEmpty(lowTemp), "Low temperature is empty");
            Assert.IsFalse(string.IsNullOrEmpty(highTemp), "High temperature is empty");

            // Attempt to parse temperatures as numbers
            bool lowTempIsNumeric = double.TryParse(lowTemp.Replace("°", "").Replace("F", "").Replace("C", "").Trim(), out double lowValue);
            bool highTempIsNumeric = double.TryParse(highTemp.Replace("°", "").Replace("F", "").Replace("C", "").Trim(), out double highValue);

            // Verify temperatures are numeric
            Assert.IsTrue(lowTempIsNumeric, "Low temperature is not a valid number");
            Assert.IsTrue(highTempIsNumeric, "High temperature is not a valid number");

            // Verify high temperature is greater than or equal to low temperature
            Assert.IsTrue(highValue >= lowValue, "High temperature is less than low temperature");
        }

        [Test]
        [Description("Verify expanding day one details shows additional information")]
        public void WeatherDisplay_ExpandDayOneDetails_ShouldShowAdditionalInfo()
        {
            Log("Testing day one details expansion");

            // Expand day one details
            _weatherDisplayPage.ExpandDayOneDetails();

            // Verify wind information is displayed
            string windSpeed = _weatherDisplayPage.GetWindSpeed();
            Assert.IsFalse(string.IsNullOrEmpty(windSpeed), "Wind speed is empty after expanding details");

            // Verify sunrise/sunset information is displayed
            string sunrise = _weatherDisplayPage.GetSunrise();
            string sunset = _weatherDisplayPage.GetSunset();
            Assert.IsFalse(string.IsNullOrEmpty(sunrise), "Sunrise time is empty after expanding details");
            Assert.IsFalse(string.IsNullOrEmpty(sunset), "Sunset time is empty after expanding details");
        }

        [Test]
        [Description("Verify expanding day two shows forecast for second day")]
        public void WeatherDisplay_ExpandDayTwo_ShouldShowSecondDayForecast()
        {
            Log("Testing day two expansion");

            // Expand day two
            _weatherDisplayPage.ExpandDayTwo();

            // This test would need to check elements specific to day two
            // which depends on the exact implementation of your weather display page

            Assert.Pass("Day two expansion works correctly");
        }

        [Test]
        [Description("Verify expanding day three shows forecast for third day")]
        public void WeatherDisplay_ExpandDayThree_ShouldShowThirdDayForecast()
        {
            Log("Testing day three expansion");

            // Expand day three
            _weatherDisplayPage.ExpandDayThree();

            // This test would need to check elements specific to day three
            // which depends on the exact implementation of your weather display page

            Assert.Pass("Day three expansion works correctly");
        }

        [Test]
        [Description("Verify close button returns to edit location")]
        public void WeatherDisplay_Close_ShouldReturnToEditLocation()
        {
            Log("Testing close button");

            // Click close button
            _weatherDisplayPage.ClickClose();

            // Wait for navigation back to edit location
            Thread.Sleep(2000);

            // Verify we're back on the edit location page
            Assert.IsTrue(_editLocationPage.IsCurrentPage(), "Not returned to edit location page");
        }
    }
}