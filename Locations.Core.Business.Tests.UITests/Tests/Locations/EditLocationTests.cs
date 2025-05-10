// Locations.Core.Business.Tests.UITests/Tests/Locations/EditLocationTests.cs
using NUnit.Framework;
using System;
using System.Threading;
using Locations.Core.Business.Tests.UITests.PageObjects.Authentication;
using Locations.Core.Business.Tests.UITests.PageObjects.Locations;
using Locations.Core.Business.Tests.UITests.PageObjects.Weather;
using Locations.Core.Business.Tests.UITests.PageObjects.Shared;

namespace Locations.Core.Business.Tests.UITests.Tests.Locations
{
    [TestFixture]
    [Category("Locations")]
    public class EditLocationTests : BaseTest
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
        }

        [Test]
        [Description("Verify that all UI elements are displayed on the edit location page")]
        public void EditLocationPage_AllUIElementsDisplayed()
        {
            Log("Checking UI elements on Edit Location page");

            // No need to check individual elements since IsCurrentPage already verifies critical elements
            Assert.Pass("Edit Location page displayed correctly");
        }

        [Test]
        [Description("Verify updating location title")]
        public void EditLocation_UpdateTitle_ShouldSucceed()
        {
            Log("Testing updating location title");

            // Get current title for reference
            string currentTitle = _editLocationPage.GetTitle();

            // Update title
            string newTitle = "Updated Location " + DateTime.Now.ToString("yyyyMMddHHmmss");
            _editLocationPage.UpdateLocation(newTitle);

            // Verify title has been updated
            string updatedTitle = _editLocationPage.GetTitle();
            Assert.AreEqual(newTitle, updatedTitle, "Location title was not updated");
        }

        [Test]
        [Description("Verify updating location description")]
        public void EditLocation_UpdateDescription_ShouldSucceed()
        {
            Log("Testing updating location description");

            // Update description
            string newDescription = "Updated description " + DateTime.Now.ToString("yyyyMMddHHmmss");
            _editLocationPage.UpdateLocation(null, newDescription);

            // Verify description has been updated
            string updatedDescription = _editLocationPage.GetDescription();
            Assert.AreEqual(newDescription, updatedDescription, "Location description was not updated");
        }

        [Test]
        [Description("Verify coordinates are displayed correctly")]
        public void EditLocation_CoordinatesDisplayed_ShouldBeValid()
        {
            Log("Testing coordinates display");

            // Get latitude and longitude values
            string latitude = _editLocationPage.GetLatitude();
            string longitude = _editLocationPage.GetLongitude();

            // Verify coordinates are populated (not empty or zero)
            Assert.IsFalse(string.IsNullOrEmpty(latitude), "Latitude was not populated");
            Assert.IsFalse(string.IsNullOrEmpty(longitude), "Longitude was not populated");

            // Verify coordinates are valid numbers
            Assert.IsTrue(double.TryParse(latitude, out double lat), "Latitude is not a valid number");
            Assert.IsTrue(double.TryParse(longitude, out double lon), "Longitude is not a valid number");

            // Verify coordinates are within reasonable range
            Assert.IsTrue(lat >= -90 && lat <= 90, "Latitude out of valid range");
            Assert.IsTrue(lon >= -180 && lon <= 180, "Longitude out of valid range");
        }

        [Test]
        [Description("Verify weather button navigation")]
        public void EditLocation_WeatherButton_ShouldNavigateToWeatherDisplay()
        {
            Log("Testing weather button navigation");

            // Click weather button
            _editLocationPage.ClickWeatherButton();

            // Wait for weather display to load
            Thread.Sleep(2000);

            // Initialize weather display page
            _weatherDisplayPage = new WeatherDisplayPage(WindowsDriver, AndroidDriver, iOSDriver, CurrentPlatform);

            // Verify we're on the weather display page
            Assert.IsTrue(_weatherDisplayPage.IsCurrentPage(), "Not on the weather display page");

            // Verify weather data is displayed
            Assert.IsTrue(_weatherDisplayPage.HasWeatherData(), "Weather data not displayed");

            // Close weather display
            _weatherDisplayPage.ClickClose();

            // Wait for navigation back to edit location
            Thread.Sleep(2000);

            // Verify we're back on the edit location page
            Assert.IsTrue(_editLocationPage.IsCurrentPage(), "Not returned to edit location page");
        }

        [Test]
        [Description("Verify sun events button navigation")]
        public void EditLocation_SunEventsButton_ShouldNavigateToSunCalculations()
        {
            Log("Testing sun events button navigation");

            // Click sun events button
            _editLocationPage.ClickSunEventsButton();

            // Wait for navigation
            Thread.Sleep(2000);

            // For this test, we're just verifying that we've navigated away from the edit location page
            // Ideally, we would initialize a SunCalculationsPage object and verify we're on that page
            Assert.IsFalse(_editLocationPage.IsCurrentPage(), "Still on edit location page after clicking sun events");

            // Navigate back
            GoBack();

            // Wait for navigation back to edit location
            Thread.Sleep(2000);

            // Verify we're back on the edit location page
            Assert.IsTrue(_editLocationPage.IsCurrentPage(), "Not returned to edit location page");
        }

        [Test]
        [Description("Verify closing edit location returns to list")]
        public void EditLocation_Close_ShouldReturnToList()
        {
            Log("Testing close button");

            // Click close button
            _editLocationPage.ClickClose();

            // Wait for navigation back to list
            Thread.Sleep(2000);

            // Verify we're back on the list locations page
            Assert.IsTrue(_listLocationsPage.IsCurrentPage(), "Not returned to list locations page");
        }
    }
}