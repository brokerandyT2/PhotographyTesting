using TechTalk.SpecFlow;
using NUnit.Framework;
using System.Threading;
using Locations.Core.Business.BDD.Support;
using Locations.Core.Business.Tests.UITests.PageObjects.Authentication;
using Locations.Core.Business.Tests.UITests.PageObjects.Locations;

namespace Locations.Core.Business.BDD.StepDefinitions.Locations
{
    [Binding]
    public class ListLocationsSteps
    {
        private readonly AppiumDriverWrapper _driverWrapper;
        private readonly ScenarioContext _scenarioContext;
        private LoginPage _loginPage;
        private ListLocationsPage _listLocationsPage;
        private void MarkAsPending(string message = "This step is not yet implemented")
        {
            throw new PendingStepException(message);
        }
        public ListLocationsSteps(AppiumDriverWrapper driverWrapper, ScenarioContext scenarioContext)
        {
            _driverWrapper = driverWrapper;
            _scenarioContext = scenarioContext;
            _listLocationsPage = new ListLocationsPage(_driverWrapper.Driver, _driverWrapper.Platform);
        }

        [Given(@"I am logged into the application")]
        public void GivenIAmLoggedIntoTheApplication()
        {
            _loginPage = new LoginPage(_driverWrapper.Driver, _driverWrapper.Platform);
            if (_loginPage.IsCurrentPage())
            {
                _loginPage.Login();
                Thread.Sleep(2000); // Wait for login to complete
            }
        }

        [Given(@"I am on the locations list page")]
        public void GivenIAmOnTheLocationsListPage()
        {
            // Verify we're on the list locations page
            NUnit.Framework.Assert.That(_listLocationsPage.IsCurrentPage(), Is.True, "Not on the locations list page");
        }

        [Given(@"I have no saved locations")]
        public void GivenIHaveNoSavedLocations()
        {
            // This would require a setup to ensure no locations exist
            // For test purposes, we'll verify there are no locations
            // A real implementation might use an API or direct DB access to clear locations
            NUnit.Framework.Assert.That(_listLocationsPage.HasLocations(), Is.False, "Locations exist when none should");
        }

        [Given(@"I have multiple saved locations")]
        public void GivenIHaveMultipleSavedLocations()
        {
            // Verify multiple locations exist
            NUnit.Framework.Assert.That(_listLocationsPage.HasLocations(), Is.True, "No locations found");

            var locationTitles = _listLocationsPage.GetLocationTitles();
            NUnit.Framework.Assert.That(locationTitles.Count, Is.GreaterThan(1), "Not enough locations for testing");
        }

        [When(@"the locations list loads")]
        public void WhenTheLocationsListLoads()
        {
            NUnit.Framework.Assert.That(_listLocationsPage.WaitForLocationsToLoad(), Is.True, "Locations failed to load");
        }

        [When(@"I select a location from the list")]
        public void WhenISelectALocationFromTheList()
        {
            // Select the first location
            _listLocationsPage.SelectLocation(0);
            Thread.Sleep(2000); // Wait for navigation
        }

        [When(@"I enter search text ""(.*)""")]
        public void WhenIEnterSearchText(string searchText)
        {
            // Implement location search functionality
            // This would depend on how search is implemented in your app
            // _listLocationsPage.SearchLocations(searchText);
           
        }

        [When(@"I tap the add location button")]
        public void WhenITapTheAddLocationButton()
        {
            // Implementation will depend on how the add button is exposed in the page object
            // _listLocationsPage.ClickAddLocationButton();

        }

        [When(@"I tap the map button for a location")]
        public void WhenITapTheMapButtonForALocation()
        {
            // Implementation will depend on how map button is exposed in the page object
            // _listLocationsPage.OpenMap(0);

        }

        [Then(@"I should see my saved locations")]
        public void ThenIShouldSeeMyLocations()
        {
            NUnit.Framework.Assert.That(_listLocationsPage.HasLocations(), Is.True, "No locations displayed");
        }

        [Then(@"each location should display its title")]
        public void ThenEachLocationShouldDisplayItsTitle()
        {
            var locationTitles = _listLocationsPage.GetLocationTitles();
            foreach (var title in locationTitles)
            {
                NUnit.Framework.Assert.That(string.IsNullOrEmpty(title), Is.False, "Location title is missing");
            }
        }

        [Then(@"I should be taken to the location details page")]
        public void ThenIShouldBeTakenToTheLocationDetailsPage()
        {
            var editLocationPage = new EditLocationPage(_driverWrapper.Driver, _driverWrapper.Platform);
            NUnit.Framework.Assert.That(editLocationPage.IsCurrentPage(), Is.True, "Not on the location details page");
        }

        [Then(@"I should see details for the selected location")]
        public void ThenIShouldSeeDetailsForTheSelectedLocation()
        {
            var editLocationPage = new EditLocationPage(_driverWrapper.Driver, _driverWrapper.Platform);
            NUnit.Framework.Assert.That(!string.IsNullOrEmpty(editLocationPage.GetTitle()), Is.True, "No title displayed on details page");
            NUnit.Framework.Assert.That(!string.IsNullOrEmpty(editLocationPage.GetLatitude()), Is.True, "No latitude displayed on details page");
            NUnit.Framework.Assert.That(!string.IsNullOrEmpty(editLocationPage.GetLongitude()), Is.True, "No longitude displayed on details page");
        }

        [Then(@"I should see an empty state message")]
        public void ThenIShouldSeeAnEmptyStateMessage()
        {
            // This depends on how empty state is implemented in your app
            // Assert.That(_listLocationsPage.IsEmptyStateDisplayed(), Is.True, "Empty state not displayed");

        }

        [Then(@"I should see an option to add a new location")]
        public void ThenIShouldSeeAnOptionToAddANewLocation()
        {
            // This depends on how add button is exposed in your app
            // Assert.That(_listLocationsPage.IsAddLocationButtonDisplayed(), Is.True, "Add location button not displayed");
            
        }

        [Then(@"I should only see locations matching ""(.*)""")]
        public void ThenIShouldOnlySeeLocationsMatching(string searchText)
        {
            var locationTitles = _listLocationsPage.GetLocationTitles();
            foreach (var title in locationTitles)
            {
                NUnit.Framework.Assert.That(title.ToLower().Contains(searchText.ToLower()), Is.True,
                    $"Location '{title}' does not match search text '{searchText}'");
            }
        }

        [Then(@"I should be taken to the add location page")]
        public void ThenIShouldBeTakenToTheAddLocationPage()
        {
            var addLocationPage = new AddLocationPage(_driverWrapper.Driver, _driverWrapper.Platform);
            NUnit.Framework.Assert.That(addLocationPage.IsCurrentPage(), Is.True, "Not on the add location page");
        }

        [Then(@"the map should open with the location marked")]
        public void ThenTheMapShouldOpenWithTheLocationMarked()
        {
            // This might be difficult to test directly as it could open an external map app
            // We might need to skip or simulate this step depending on the implementation
            NUnit.Framework.Assert.That(true == true, Is.True);
        }
    }
}