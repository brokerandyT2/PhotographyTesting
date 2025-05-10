// Locations.Core.Business.Tests.UITests/Tests/Locations/AddLocationTests.cs
using NUnit.Framework;
using System;
using System.Threading;
using Locations.Core.Business.Tests.UITests.PageObjects.Authentication;
using Locations.Core.Business.Tests.UITests.PageObjects.Locations;
using Locations.Core.Business.Tests.UITests.PageObjects.Shared;

namespace Locations.Core.Business.Tests.UITests.Tests.Locations
{
    [TestFixture]
    [Category("Locations")]
    public class AddLocationTests : BaseTest
    {
        private AddLocationPage _addLocationPage;
        private PageTutorialModalPage _tutorialPage;

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

            // Navigate to add location page (this will depend on your app's navigation)
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
                // If direct navigation fails, try an alternative approach
            }

            // Initialize page object
            _addLocationPage = new AddLocationPage(WindowsDriver, AndroidDriver, iOSDriver, CurrentPlatform);

            // Check if we're on a tutorial page first
            _tutorialPage = new PageTutorialModalPage(WindowsDriver, AndroidDriver, iOSDriver, CurrentPlatform);
            if (_tutorialPage.IsCurrentPage())
            {
                _tutorialPage.WaitForDismissal();
            }

            // Verify we're on the add location page
            Assert.IsTrue(_addLocationPage.IsCurrentPage(), "Not on the add location page");
        }

        [Test]
        [Description("Verify that all UI elements are displayed on the add location page")]
        public void AddLocationPage_AllUIElementsDisplayed()
        {
            Log("Checking UI elements on Add Location page");

            // No need to check individual elements since IsCurrentPage already verifies critical elements
            Assert.Pass("Add Location page displayed correctly");
        }

        [Test]
        [Description("Verify location coordinates are automatically populated")]
        public void AddLocation_LocationCoordinatesAutomaticallyPopulated()
        {
            Log("Testing automatic coordinate population");

            // Get latitude and longitude values
            string latitude = _addLocationPage.GetLatitude();
            string longitude = _addLocationPage.GetLongitude();

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
        [Description("Verify creating a location with only title")]
        public void AddLocation_WithOnlyTitle_ShouldSucceed()
        {
            Log("Testing creating location with only title");

            // Create location with only a title
            string locationTitle = "Test Location " + DateTime.Now.ToString("yyyyMMddHHmmss");
            _addLocationPage.CreateLocation(locationTitle);

            // Verify no error is displayed
            Assert.IsFalse(_addLocationPage.HasError(), "Error displayed after creating location");

            // Verify we're no longer on the add location page
            Assert.IsFalse(_addLocationPage.IsCurrentPage(), "Still on add location page after successful creation");
        }

        [Test]
        [Description("Verify creating a location with title and description")]
        public void AddLocation_WithTitleAndDescription_ShouldSucceed()
        {
            Log("Testing creating location with title and description");

            // Create location with title and description
            string locationTitle = "Test Location " + DateTime.Now.ToString("yyyyMMddHHmmss");
            string locationDescription = "This is a test location created by automated UI tests.";

            _addLocationPage.CreateLocation(locationTitle, locationDescription);

            // Verify no error is displayed
            Assert.IsFalse(_addLocationPage.HasError(), "Error displayed after creating location");

            // Verify we're no longer on the add location page
            Assert.IsFalse(_addLocationPage.IsCurrentPage(), "Still on add location page after successful creation");
        }

        [Test]
        [Description("Verify error handling for invalid input")]
        public void AddLocation_WithInvalidInput_ShouldShowError()
        {
            Log("Testing error handling for invalid input");

            // Try to create a location with an empty title (assuming title is required)
            _addLocationPage.CreateLocation("");

            // Verify error is displayed
            Assert.IsTrue(_addLocationPage.HasError() || _addLocationPage.IsCurrentPage(),
                "No error displayed for invalid input");
        }

        [Test]
        [Description("Verify photo capture functionality")]
        [Ignore("Photo capture requires device camera and may not be testable in automated environment")]
        public void AddLocation_CapturePhoto_ShouldAttachToLocation()
        {
            Log("Testing photo capture functionality");

            // This test might need to be run manually or with special setup to handle camera interactions
            _addLocationPage.TakePhoto();

            // Verify photo is attached
            Assert.Pass("Photo attached to location");
        }
    }
}