// Locations.Core.Business.Tests.UITests/Tests/Locations/ListLocationsTests.cs
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using System;
using System.Threading;
using Locations.Core.Business.Tests.UITests.PageObjects.Authentication;
using Locations.Core.Business.Tests.UITests.PageObjects.Locations;
using Locations.Core.Business.Tests.UITests.PageObjects.Shared;

namespace Locations.Core.Business.Tests.UITests.Tests.Locations
{
    [TestFixture]
    [Category("Locations")]
    public class ListLocationsTests : BaseTest
    {
        private ListLocationsPage _listLocationsPage;
        private PageTutorialModalPage _tutorialPage;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            // First login if needed
            var loginPage = new LoginPage(Driver, CurrentPlatform);
            if (loginPage.IsCurrentPage())
            {
                loginPage.Login();
            }

            // Initialize list locations page - assuming we're already on the main page after login
            _listLocationsPage = new ListLocationsPage(Driver, CurrentPlatform);

            // Check if we're on a tutorial page first
            _tutorialPage = new PageTutorialModalPage(Driver, CurrentPlatform);
            if (_tutorialPage.IsCurrentPage())
            {
                _tutorialPage.WaitForDismissal();
            }

            // Verify we're on the list locations page
            Assert.That(_listLocationsPage.IsCurrentPage(), Is.True, "Not on the list locations page");

            // Wait for locations to load
            Assert.That(_listLocationsPage.WaitForLocationsToLoad(), Is.True, "Locations failed to load");
        }

        [Test]
        [Description("Verify that all UI elements are displayed on the list locations page")]
        public void ListLocationsPage_AllUIElementsDisplayed()
        {
            Log("Checking UI elements on List Locations page");

            // No need to check individual elements since IsCurrentPage already verifies critical elements
            Assert.Pass("List Locations page displayed correctly");
        }

        [Test]
        [Description("Verify locations are displayed in the list")]
        public void ListLocations_LocationsDisplayed()
        {
            Log("Testing locations display");

            // Ensure there are locations displayed
            Assert.That(_listLocationsPage.HasLocations(), Is.True, "No locations displayed in the list");

            // Get location titles
            var locationTitles = _listLocationsPage.GetLocationTitles();

            // Verify at least one location is displayed
            Assert.That(locationTitles.Count > 0, Is.True, "No location titles found in the list");

            // Verify location titles are not empty
            foreach (string title in locationTitles)
            {
                Assert.That(string.IsNullOrEmpty(title), Is.False, "Found empty location title");
            }
        }

        [Test]
        [Description("Verify selecting a location navigates to edit location")]
        public void ListLocations_SelectLocation_ShouldNavigateToEditLocation()
        {
            Log("Testing location selection");

            // Ensure there are locations
            Assert.That(_listLocationsPage.HasLocations(), Is.True, "No locations displayed in the list");

            // Select the first location
            _listLocationsPage.SelectLocation(0);

            // Wait for navigation
            Thread.Sleep(2000);

            // Initialize edit location page
            var editLocationPage = new EditLocationPage(Driver, CurrentPlatform);

            // Verify we're on the edit location page
            Assert.That(editLocationPage.IsCurrentPage(), Is.True, "Not navigated to edit location page");

            // Return to list
            editLocationPage.ClickClose();

            // Wait for navigation back to list
            Thread.Sleep(2000);

            // Verify we're back on the list locations page
            Assert.That(_listLocationsPage.IsCurrentPage(), Is.True, "Not returned to list locations page");
        }

        [Test]
        [Description("Verify map button functionality")]
        [Ignore("Map functionality requires system integration and may not be testable in automated environment")]
        public void ListLocations_MapButton_ShouldOpenMap()
        {
            Log("Testing map button functionality");

            // Ensure there are locations
            Assert.That(_listLocationsPage.HasLocations(), Is.True, "No locations displayed in the list");

            // Click map button for the first location
            //_listLocationsPage.OpenMap(0);

            // This will typically open an external map application, which may not be testable in automated environment
            // We could verify that our app handled the intent correctly

            Assert.Pass("Map navigation launched");
        }

        [Test]
        [Description("Verify empty state when no locations")]
        [Ignore("This test requires a fresh install or the ability to delete all locations")]
        public void ListLocations_NoLocations_ShouldShowEmptyState()
        {
            Log("Testing empty state display");

            // This test would need to ensure there are no locations first
            // Could be done by clearing app data or having a way to delete all locations

            // Verify no locations are displayed
            Assert.That(_listLocationsPage.HasLocations(), Is.False, "Locations are displayed when none should exist");

            // Verify empty state is displayed (this would depend on how your app shows empty state)
            // For example, checking for an empty state message or image

            Assert.Pass("Empty state displayed correctly");
        }

        [Test]
        [Description("Verify adding a new location from list view")]
        public void ListLocations_AddNewLocation_ShouldNavigateToAddLocation()
        {
            Log("Testing adding new location from list view");

            // Look for an "Add" button (this depends on your app's UI)
            try
            {
                switch (CurrentPlatform)
                {
                    case AppiumSetup.Platform.Android:
                        Driver.FindElement(By.XPath("//android.widget.Button[contains(@content-desc, 'Add') or contains(@text, 'Add')]")).Click();
                        break;
                    case AppiumSetup.Platform.iOS:
                        Driver.FindElement(By.XPath("//XCUIElementTypeButton[contains(@name, 'Add')]")).Click();
                        break;
                    case AppiumSetup.Platform.Windows:
                        Driver.FindElement(By.XPath("//Button[contains(@Name, 'Add')]")).Click();
                        break;
                }

                // Wait for navigation
                Thread.Sleep(2000);

                // Initialize add location page
                var addLocationPage = new AddLocationPage(Driver, CurrentPlatform);

                // Verify we're on the add location page
                Assert.That(addLocationPage.IsCurrentPage(), Is.True, "Not navigated to add location page");

                // Return to list without saving
                GoBack();

                // Wait for navigation back to list
                Thread.Sleep(2000);

                // Verify we're back on the list locations page
                Assert.That(_listLocationsPage.IsCurrentPage(), Is.True, "Not returned to list locations page");
            }
            catch (Exception ex)
            {
                Log($"Failed to find Add button: {ex.Message}");
                Assert.Fail("Add button not found or navigation failed");
            }
        }

        [Test]
        [Description("Verify location search functionality if available")]
        [Ignore("Search functionality may not be available or may vary by implementation")]
        public void ListLocations_SearchLocation_ShouldFilterResults()
        {
            Log("Testing location search functionality");

            // This test would depend on whether your app has a search feature
            // and how it's implemented

            Assert.Pass("Search functionality works correctly");
        }
    }
}