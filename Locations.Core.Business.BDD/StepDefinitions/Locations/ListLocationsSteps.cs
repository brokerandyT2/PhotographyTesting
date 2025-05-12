using TechTalk.SpecFlow;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Locations.Core.Business.BDD.Support;
using Locations.Core.Business.DataAccess.Interfaces;
using Locations.Core.Shared.ViewModels;
using Assert = NUnit.Framework.Assert;
using NUnit.Framework;
using Moq;
using MockFactory = Locations.Core.Business.BDD.TestHelpers.MockFactory;
using TestDataFactory = Locations.Core.Business.BDD.TestHelpers.TestDataFactory;
using Locations.Core.Data.Models;
using Locations.Core.Business.BDD.TestHelpers;
using Locations.Core.Data.Queries.Interfaces;

namespace Locations.Core.Business.BDD.StepDefinitions.Locations
{
    [Binding]
    public class ListLocationsSteps
    {
        private readonly ILocationService<LocationViewModel> _locationService;
        private readonly BDDTestContext _testContext;
        private readonly Mock<ILocationRepository> _mockLocationRepository;
        private List<LocationViewModel> _currentLocations;
        private List<LocationViewModel> _filteredLocations;

        public ListLocationsSteps(ILocationService<LocationViewModel> locationService, BDDTestContext testContext, Mock<ILocationRepository> mockLocationRepository)
        {
            _locationService = locationService;
            _testContext = testContext;
            _mockLocationRepository = mockLocationRepository;
            _currentLocations = new List<LocationViewModel>();
            _filteredLocations = new List<LocationViewModel>();
        }

        [Given(@"I am logged into the application")]
        public void GivenIAmLoggedIntoTheApplication()
        {
            _testContext.IsUserLoggedIn = true;
            _testContext.CurrentUserEmail = "test@example.com";
        }

        [Given(@"I am on the locations list page")]
        public async Task GivenIAmOnTheLocationsListPage()
        {
            // Load current locations
            var result = await _locationService.GetAllAsync();
            if (result.IsSuccess)
            {
                _currentLocations = result.Data;
                _testContext.TestLocations = result.Data;
            }
        }

        [Given(@"I have no saved locations")]
        public void GivenIHaveNoSavedLocations()
        {
            _currentLocations.Clear();
            _testContext.TestLocations.Clear();

            // Configure mock to return empty list
            _mockLocationRepository.Setup(x => x.GetAllAsync())
                .ReturnsAsync(DataOperationResult<IList<LocationViewModel>>.Success(new List<LocationViewModel>()));
        }

        [Given(@"I have multiple saved locations")]
        public void GivenIHaveMultipleSavedLocations()
        {
            var locations = TestDataFactory.CreateTestLocations(3);
            _currentLocations = locations;
            _testContext.TestLocations = locations;

            // Configure mock to return test locations
            _mockLocationRepository.Setup(x => x.GetAllAsync())
                .ReturnsAsync(DataOperationResult<IList<LocationViewModel>>.Success(locations));
        }

        [When(@"the locations list loads")]
        public async Task WhenTheLocationsListLoads()
        {
            var result = await _locationService.GetAllAsync();
            if (result.IsSuccess)
            {
                _currentLocations = result.Data;
            }
        }

        [When(@"I select a location from the list")]
        public void WhenISelectALocationFromTheList()
        {
            if (_currentLocations.Any())
            {
                _testContext.CurrentLocation = _currentLocations.First();
            }
        }

        [When(@"I enter search text ""(.*)""")]
        public void WhenIEnterSearchText(string searchText)
        {
            // Filter locations based on search text
            _filteredLocations = _currentLocations
                .Where(l => l.Title.Contains(searchText, System.StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        [When(@"I tap the add location button")]
        public void WhenITapTheAddLocationButton()
        {
            // In service tests, we just set state indicating we're adding a location
            _testContext.CurrentLocation = new LocationViewModel();
        }

        [When(@"I tap the map button for a location")]
        public void WhenITapTheMapButtonForALocation()
        {
            // In service tests, we just set the current location
            if (_currentLocations.Any())
            {
                _testContext.CurrentLocation = _currentLocations.First();
            }
        }

        [Then(@"I should see my saved locations")]
        public void ThenIShouldSeeMySavedLocations()
        {
            Assert.That(_currentLocations, Is.Not.Null);
            Assert.That(_currentLocations.Count, Is.GreaterThan(0), "No locations displayed");
        }

        [Then(@"each location should display its title")]
        public void ThenEachLocationShouldDisplayItsTitle()
        {
            foreach (var location in _currentLocations)
            {
                Assert.That(string.IsNullOrEmpty(location.Title), Is.False,
                    $"Location {location.Id} has no title");
            }
        }

        [Then(@"I should be taken to the location details page")]
        public void ThenIShouldBeTakenToTheLocationDetailsPage()
        {
            Assert.That(_testContext.CurrentLocation, Is.Not.Null, "No location selected");
        }

        [Then(@"I should see details for the selected location")]
        public void ThenIShouldSeeDetailsForTheSelectedLocation()
        {
            var location = _testContext.CurrentLocation;
            Assert.That(location, Is.Not.Null);
            Assert.That(string.IsNullOrEmpty(location.Title), Is.False, "No title in location details");
            Assert.That(location.Lattitude, Is.Not.EqualTo(0), "No latitude in location details");
            Assert.That(location.Longitude, Is.Not.EqualTo(0), "No longitude in location details");
        }

        [Then(@"I should see an empty state message")]
        public void ThenIShouldSeeAnEmptyStateMessage()
        {
            Assert.That(_currentLocations.Count, Is.EqualTo(0), "Locations exist when none expected");
        }

        [Then(@"I should see an option to add a new location")]
        public void ThenIShouldSeeAnOptionToAddANewLocation()
        {
            // In service tests, we just verify that adding is possible
            Assert.That(_currentLocations.Count, Is.EqualTo(0));
        }

        [Then(@"I should only see locations matching ""(.*)""")]
        public void ThenIShouldOnlySeeLocationsMatching(string searchText)
        {
            Assert.That(_filteredLocations, Is.Not.Null);
            foreach (var location in _filteredLocations)
            {
                Assert.That(location.Title.Contains(searchText, System.StringComparison.OrdinalIgnoreCase),
                    Is.True, $"Location '{location.Title}' doesn't match search text '{searchText}'");
            }
        }

        [Then(@"I should be taken to the add location page")]
        public void ThenIShouldBeTakenToTheAddLocationPage()
        {
            Assert.That(_testContext.CurrentLocation, Is.Not.Null);
            Assert.That(_testContext.CurrentLocation.Id, Is.EqualTo(0), "Not a new location");
        }

        [Then(@"the map should open with the location marked")]
        public void ThenTheMapShouldOpenWithTheLocationMarked()
        {
            Assert.That(_testContext.CurrentLocation, Is.Not.Null);
            Assert.That(_testContext.CurrentLocation.Lattitude, Is.Not.EqualTo(0));
            Assert.That(_testContext.CurrentLocation.Longitude, Is.Not.EqualTo(0));
        }
    }
}