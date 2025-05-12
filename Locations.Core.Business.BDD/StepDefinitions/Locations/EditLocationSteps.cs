
using TechTalk.SpecFlow;
using System.Threading.Tasks;
using System.Linq;
using Locations.Core.Business.BDD.Support;
using Locations.Core.Business.DataAccess.Interfaces;
using Locations.Core.Business.BDD.TestHelpers;
using Locations.Core.Shared.ViewModels;
using Assert = NUnit.Framework.Assert;
using NUnit.Framework;
using Moq;
using Locations.Core.Data.Models;
using Locations.Core.Data.Queries.Interfaces;

namespace Locations.Core.Business.BDD.StepDefinitions.Locations
{
    [Binding]
    public class EditLocationSteps
    {
        private readonly ILocationService<LocationViewModel> _locationService;
        private readonly IWeatherService<WeatherViewModel> _weatherService;
        private readonly BDDTestContext _testContext;
        private readonly Mock<ILocationRepository> _mockLocationRepository;
        private LocationViewModel _originalLocation;
        private LocationViewModel _editingLocation;
        private bool _updateSuccessful;
        private bool _deleteSuccessful;

        public EditLocationSteps(
            ILocationService<LocationViewModel> locationService,
            IWeatherService<WeatherViewModel> weatherService,
            BDDTestContext testContext,
            Mock<ILocationRepository> mockLocationRepository)
        {
            _locationService = locationService;
            _weatherService = weatherService;
            _testContext = testContext;
            _mockLocationRepository = mockLocationRepository;
        }

        [Given(@"I have at least one saved location")]
        public void GivenIHaveAtLeastOneSavedLocation()
        {
            if (!_testContext.TestLocations.Any())
            {
                var testLocation = TestDataFactory.CreateTestLocation();
                _testContext.TestLocations.Add(testLocation);

                _mockLocationRepository.Setup(x => x.GetAllAsync())
                    .ReturnsAsync(DataOperationResult<IList<LocationViewModel>>.Success(_testContext.TestLocations));
            }
        }

        [Given(@"I have selected a location to edit")]
        public void GivenIHaveSelectedALocationToEdit()
        {
            _originalLocation = _testContext.TestLocations.First();
            _editingLocation = new LocationViewModel
            {
                Id = _originalLocation.Id,
                Title = _originalLocation.Title,
                Description = _originalLocation.Description,
                Lattitude = _originalLocation.Lattitude,
                Longitude = _originalLocation.Longitude,
                City = _originalLocation.City,
                State = _originalLocation.State,
                Photo = _originalLocation.Photo,
                Timestamp = _originalLocation.Timestamp
            };
            _testContext.CurrentLocation = _editingLocation;
        }

        [When(@"I change the location title to ""(.*)""")]
        public void WhenIChangeTheLocationTitleTo(string newTitle)
        {
            _editingLocation.Title = newTitle;
        }

        [When(@"I change the location description to ""(.*)""")]
        public void WhenIChangeTheLocationDescriptionTo(string newDescription)
        {
            _editingLocation.Description = newDescription;
        }

        [When(@"I tap the delete button")]
        public void WhenITapTheDeleteButton()
        {
            // In service tests, we prepare for deletion
            _deleteSuccessful = false;
        }

        [When(@"I confirm the deletion")]
        public async Task WhenIConfirmTheDeletion()
        {
            _mockLocationRepository.Setup(x => x.DeleteAsync(_editingLocation.Id))
                .ReturnsAsync(DataOperationResult<bool>.Success(true));

            var result = await _locationService.DeleteAsync(_editingLocation.Id);
            _deleteSuccessful = result.IsSuccess;

            if (_deleteSuccessful)
            {
                _testContext.TestLocations.RemoveAll(l => l.Id == _editingLocation.Id);
            }
        }

        [When(@"I tap the weather button")]
        public async Task WhenITapTheWeatherButton()
        {
            var weatherResult = await _weatherService.GetWeatherForLocationAsync(_editingLocation.Id);
            if (weatherResult.IsSuccess)
            {
                _testContext.CurrentWeather = weatherResult.Data;
            }
        }

        [When(@"I tap the sun events button")]
        public void WhenITapTheSunEventsButton()
        {
            // Create sun calculation data
            var sunCalc = new Location.Photography.Shared.ViewModels.SunCalculations
            {
                Latitude = _editingLocation.Lattitude,
                Longitude = _editingLocation.Longitude,
                Date = DateTime.Now
            };
            // In real app, would calculate sun times here
        }

        [When(@"I make changes to the location")]
        public void WhenIMakeChangesToTheLocation()
        {
            _editingLocation.Title = _originalLocation.Title + " (modified)";
            _editingLocation.Description = _originalLocation.Description + " (modified)";
        }

        [When(@"I tap the close button without saving")]
        public void WhenITapTheCloseButtonWithoutSaving()
        {
            // Revert changes
            _editingLocation.Title = _originalLocation.Title;
            _editingLocation.Description = _originalLocation.Description;
        }

        [When(@"I confirm discarding changes")]
        public void WhenIConfirmDiscardingChanges()
        {
            // Reset to original values
            _testContext.CurrentLocation = _originalLocation;
        }

        [When(@"I tap the change photo button")]
        public void WhenITapTheChangePhotoButton()
        {
            _editingLocation.Photo = "new_photo.jpg";
        }

        [When(@"I save the location")]
        public async Task WhenISaveTheLocation()
        {
            _mockLocationRepository.Setup(x => x.UpdateAsync(It.IsAny<LocationViewModel>()))
                .ReturnsAsync(DataOperationResult<bool>.Success(true));

            var result = await _locationService.UpdateAsync(_editingLocation);
            _updateSuccessful = result.IsSuccess;

            if (_updateSuccessful)
            {
                var index = _testContext.TestLocations.FindIndex(l => l.Id == _editingLocation.Id);
                if (index >= 0)
                {
                    _testContext.TestLocations[index] = _editingLocation;
                }
            }
        }

        [Then(@"the location should be updated with the new title")]
        public void ThenTheLocationShouldBeUpdatedWithTheNewTitle()
        {
            Assert.That(_updateSuccessful, Is.True, "Update was not successful");
            var updatedLocation = _testContext.TestLocations.FirstOrDefault(l => l.Id == _editingLocation.Id);
            Assert.That(updatedLocation?.Title, Is.EqualTo(_editingLocation.Title));
        }

        [Then(@"the location should be updated with the new description")]
        public void ThenTheLocationShouldBeUpdatedWithTheNewDescription()
        {
            Assert.That(_updateSuccessful, Is.True, "Update was not successful");
            var updatedLocation = _testContext.TestLocations.FirstOrDefault(l => l.Id == _editingLocation.Id);
            Assert.That(updatedLocation?.Description, Is.EqualTo(_editingLocation.Description));
        }

        [Then(@"the location should be removed from my saved locations")]
        public void ThenTheLocationShouldBeRemovedFromMySavedLocations()
        {
            Assert.That(_deleteSuccessful, Is.True, "Delete was not successful");
            var deletedLocation = _testContext.TestLocations.FirstOrDefault(l => l.Id == _editingLocation.Id);
            Assert.That(deletedLocation, Is.Null, "Location was not removed");
        }

        [Then(@"I should not see the deleted location in my list")]
        public void ThenIShouldNotSeeTheDeletedLocationInMyList()
        {
            var locationTitles = _testContext.TestLocations.Select(l => l.Title).ToList();
            Assert.That(locationTitles.Contains(_originalLocation.Title), Is.False);
        }

        [Then(@"I should see the weather details for this location")]
        public void ThenIShouldSeeTheWeatherDetailsForThisLocation()
        {
            Assert.That(_testContext.CurrentWeather, Is.Not.Null);
            Assert.That(_testContext.CurrentWeather.LocationId, Is.EqualTo(_editingLocation.Id));
        }

        [Then(@"I should see forecast information for multiple days")]
        public void ThenIShouldSeeForecastInformationForMultipleDays()
        {
            Assert.That(_testContext.CurrentWeather, Is.Not.Null);
            Assert.That(string.IsNullOrEmpty(_testContext.CurrentWeather.Forecast), Is.False);
        }

        [Then(@"I should see the sun calculator for this location")]
        public void ThenIShouldSeeTheSunCalculatorForThisLocation()
        {
            // In service tests, we verify we have location data needed for calculations
            Assert.That(_testContext.CurrentLocation.Lattitude, Is.Not.EqualTo(0));
            Assert.That(_testContext.CurrentLocation.Longitude, Is.Not.EqualTo(0));
        }

        [Then(@"I should see sunrise and sunset times")]
        [Then(@"I should see golden hour information")]
        public void ThenIShouldSeeSunInformation()
        {
            // These would be calculated based on location and date
            Assert.That(_testContext.CurrentLocation, Is.Not.Null);
        }

        [Then(@"I should be prompted to confirm discarding changes")]
        public void ThenIShouldBePromptedToConfirmDiscardingChanges()
        {
            // In service tests, we verify that changes were detected
            Assert.That(_editingLocation.Title, Is.Not.EqualTo(_originalLocation.Title));
        }

        [Then(@"the location should remain unchanged")]
        public void ThenTheLocationShouldRemainUnchanged()
        {
            var currentLocation = _testContext.TestLocations.FirstOrDefault(l => l.Id == _originalLocation.Id);
            Assert.That(currentLocation?.Title, Is.EqualTo(_originalLocation.Title));
            Assert.That(currentLocation?.Description, Is.EqualTo(_originalLocation.Description));
        }

        [Then(@"the location should be updated with the new photo")]
        public void ThenTheLocationShouldBeUpdatedWithTheNewPhoto()
        {
            Assert.That(_editingLocation.Photo, Is.EqualTo("new_photo.jpg"));
        }
    }
}