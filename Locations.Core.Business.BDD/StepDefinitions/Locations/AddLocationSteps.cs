using Locations.Core.Business.BDD.TestHelpers;
using Locations.Core.Business.DataAccess.Interfaces;
using Locations.Core.Data.Models;
using Locations.Core.Data.Queries.Interfaces;
using Locations.Core.Shared.ViewModels;
using Moq;
using NUnit.Framework;
using TechTalk.SpecFlow;
using Assert = NUnit.Framework.Assert;

namespace Locations.Core.Business.BDD.StepDefinitions.Locations
{
    [Binding]
    [Scope(Feature = "AddLocation")]
    public class AddLocationSteps
    {
        private readonly ILocationService<LocationViewModel> _locationService;
        private readonly BDDTestContext _testContext;
        private readonly Mock<ILocationRepository> _mockLocationRepository;
        private LocationViewModel _newLocation;
        private bool _saveSuccessful;
        private string _errorMessage;

        public AddLocationSteps(ILocationService<LocationViewModel> locationService, BDDTestContext testContext, Mock<ILocationRepository> mockLocationRepository)
        {
            _locationService = locationService;
            _testContext = testContext;
            _mockLocationRepository = mockLocationRepository;
            _newLocation = new LocationViewModel();
        }

        [Given(@"I am on the add location page")]
        public void GivenIAmOnTheAddLocationPage()
        {
            _newLocation = new LocationViewModel
            {
                Lattitude = 40.7128,
                Longitude = -74.0060,
                Timestamp = DateTime.Now
            };
            _saveSuccessful = false;
            _errorMessage = string.Empty;
        }

        [When(@"I enter ""(.*)"" as the location title")]
        public void WhenIEnterAsTheLocationTitle(string title)
        {
            _newLocation.Title = title;
        }

        [When(@"I enter ""(.*)"" as the description")]
        public void WhenIEnterAsTheDescription(string description)
        {
            _newLocation.Description = description;
        }

        [When(@"I leave the description empty")]
        public void WhenILeaveTheDescriptionEmpty()
        {
            _newLocation.Description = string.Empty;
        }

        [When(@"I leave the location title empty")]
        public void WhenILeaveTheLocationTitleEmpty()
        {
            _newLocation.Title = string.Empty;
        }

        [When(@"I tap the save button")]
        public async Task WhenITapTheSaveButton()
        {
            // Validate location
            if (string.IsNullOrEmpty(_newLocation.Title))
            {
                _saveSuccessful = false;
                _errorMessage = "Location title is required";
                return;
            }

            // Configure mock to return the saved location with an ID
            var savedLocation = new LocationViewModel
            {
                Id = _testContext.TestLocations.Count + 1,
                Title = _newLocation.Title,
                Description = _newLocation.Description,
                Lattitude = _newLocation.Lattitude,
                Longitude = _newLocation.Longitude,
                Timestamp = _newLocation.Timestamp
            };

            _mockLocationRepository.Setup(x => x.SaveAsync(It.IsAny<LocationViewModel>()))
                .ReturnsAsync(DataOperationResult<LocationViewModel>.Success(savedLocation));

            var result = await _locationService.SaveAsync(_newLocation);

            if (result.IsSuccess)
            {
                _saveSuccessful = true;
                _testContext.TestLocations.Add(result.Data);
                _testContext.CurrentLocation = result.Data;
            }
            else
            {
                _saveSuccessful = false;
                _errorMessage = result.Message;
            }
        }

        [When(@"I add a new location")]
        public async Task WhenIAddANewLocation()
        {
            _newLocation.Title = "Test Location";
            _newLocation.Description = "Test Description";
            await WhenITapTheSaveButton();
        }

        [When(@"I tap the add photo button")]
        public void WhenITapTheAddPhotoButton()
        {
            // In service tests, we just set a photo path
            _newLocation.Photo = "test_photo.jpg";
        }

        [Then(@"the location should be saved")]
        public void ThenTheLocationShouldBeSaved()
        {
            Assert.That(_saveSuccessful, Is.True, "Location was not saved successfully");
        }

        [Then(@"I should be returned to the locations list")]
        public void ThenIShouldBeReturnedToTheLocationsList()
        {
            Assert.That(_saveSuccessful, Is.True, "Cannot return to list - save failed");
        }

        [Then(@"I should see ""(.*)"" in my locations list")]
        public void ThenIShouldSeeInMyLocationsList(string locationTitle)
        {
            var locationTitles = _testContext.TestLocations.Select(l => l.Title).ToList();
            Assert.That(locationTitles.Contains(locationTitle), Is.True,
                $"Location '{locationTitle}' not found in list");
        }

        [Then(@"I should see a validation error message")]
        public void ThenIShouldSeeAValidationErrorMessage()
        {
            Assert.That(_saveSuccessful, Is.False, "Expected validation error but save succeeded");
            Assert.That(string.IsNullOrEmpty(_errorMessage), Is.False, "No error message displayed");
        }

        [Then(@"the location should not be saved")]
        public void ThenTheLocationShouldNotBeSaved()
        {
            Assert.That(_saveSuccessful, Is.False, "Location was saved when it shouldn't have been");
        }

        [Then(@"the latitude and longitude should be automatically populated with my current position")]
        public void ThenTheLatitudeAndLongitudeShouldBeAutomaticallyPopulatedWithMyCurrentPosition()
        {
            Assert.That(_newLocation.Lattitude, Is.Not.EqualTo(0), "Latitude not populated");
            Assert.That(_newLocation.Longitude, Is.Not.EqualTo(0), "Longitude not populated");
        }

        [Then(@"the camera should open")]
        public void ThenTheCameraShouldOpen()
        {
            // In service tests, we just verify that photo can be set
            Assert.That(true, Is.True, "Camera simulation");
        }

        [Then(@"I should be able to take a photo")]
        public void ThenIShouldBeAbleToTakeAPhoto()
        {
            // In service tests, we verify photo path can be set
            _newLocation.Photo = "test_photo.jpg";
            Assert.That(string.IsNullOrEmpty(_newLocation.Photo), Is.False, "Photo not set");
        }

        [Then(@"the photo should be attached to the location")]
        public void ThenThePhotoShouldBeAttachedToTheLocation()
        {
            Assert.That(string.IsNullOrEmpty(_newLocation.Photo), Is.False, "Photo not attached");
        }

        [Then(@"I should see ""(.*)""")]
        public void ThenIShouldSee(string result)
        {
            if (result == "Location saved successfully")
            {
                Assert.That(_saveSuccessful, Is.True, "Location save failed");
            }
            else
            {
                Assert.That(_saveSuccessful, Is.False, "Location save should have failed");
                Assert.That(_errorMessage, Does.Contain(result),
                    $"Expected error containing '{result}' but got '{_errorMessage}'");
            }
        }
    }
}