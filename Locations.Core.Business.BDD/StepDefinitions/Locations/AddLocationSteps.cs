using TechTalk.SpecFlow;
using NUnit.Framework;
using System.Threading;
using Locations.Core.Business.BDD.Support;
using Locations.Core.Business.Tests.UITests.PageObjects.Authentication;
using Locations.Core.Business.Tests.UITests.PageObjects.Locations;
using TechTalk.SpecFlow.Infrastructure;
using Assert = NUnit.Framework.Assert;

namespace Locations.Core.Business.BDD.StepDefinitions.Locations
{
    [Binding]
    public class AddLocationSteps
    {
        private readonly AppiumDriverWrapper _driverWrapper;
        private readonly ScenarioContext _scenarioContext;
        private AddLocationPage _addLocationPage;
        private ListLocationsPage _listLocationsPage;
        private string _enteredTitle;

        public AddLocationSteps(AppiumDriverWrapper driverWrapper, ScenarioContext scenarioContext)
        {
            _driverWrapper = driverWrapper;
            _scenarioContext = scenarioContext;
            _addLocationPage = new AddLocationPage(_driverWrapper.Driver, _driverWrapper.Platform);
            _listLocationsPage = new ListLocationsPage(_driverWrapper.Driver, _driverWrapper.Platform);
        }

        // Helper method for pending steps
        private void MarkAsPending(string message = "This step is not yet implemented")
        {
            throw new PendingStepException(message);
        }

        [Given(@"I am on the add location page")]
        public void GivenIAmOnTheAddLocationPage()
        {
            // Navigate to add location page if not already there
            if (!_addLocationPage.IsCurrentPage())
            {
                // This might require navigation from the locations list
                if (_listLocationsPage.IsCurrentPage())
                {
                    // Implementation will depend on how to access the add button
                    // _listLocationsPage.ClickAddLocationButton();
                    MarkAsPending("Navigation to add location page not implemented");
                }
                else
                {
                    Assert.Fail("Cannot navigate to add location page from current state");
                }

                Thread.Sleep(2000); // Wait for navigation
            }

            Assert.That(_addLocationPage.IsCurrentPage(), Is.True, "Not on the add location page");
        }

        [When(@"I enter ""(.*)"" as the location title")]
        public void WhenIEnterAsTheLocationTitle(string title)
        {
            _addLocationPage.EnterTitle(title);
            _enteredTitle = title; // Store for later verification
        }

        [When(@"I enter ""(.*)"" as the description")]
        public void WhenIEnterAsTheDescription(string description)
        {
            _addLocationPage.EnterDescription(description);
        }

        [When(@"I leave the description empty")]
        public void WhenILeaveTheDescriptionEmpty()
        {
            _addLocationPage.EnterDescription(string.Empty);
        }

        [When(@"I leave the location title empty")]
        public void WhenILeaveTheLocationTitleEmpty()
        {
            _addLocationPage.EnterTitle(string.Empty);
        }

        [When(@"I tap the save button")]
        public void WhenITapTheSaveButton()
        {
            _addLocationPage.ClickSave();
            Thread.Sleep(1000); // Give time for validation to occur
        }

        [When(@"I add a new location")]
        public void WhenIAddANewLocation()
        {
            _addLocationPage.EnterTitle("Test Location");
            _addLocationPage.EnterDescription("Test Description");
            _enteredTitle = "Test Location";
        }

        [When(@"I tap the add photo button")]
        public void WhenITapTheAddPhotoButton()
        {
            _addLocationPage.TakePhoto();
        }

        [Then(@"the location should be saved")]
        public void ThenTheLocationShouldBeSaved()
        {
            Assert.That(_addLocationPage.WaitForSaveToComplete(), Is.True, "Save operation did not complete");
        }

        [Then(@"I should be returned to the locations list")]
        public void ThenIShouldBeReturnedToTheLocationsList()
        {
            Assert.That(_listLocationsPage.IsCurrentPage(), Is.True, "Not returned to locations list");
        }

        [Then(@"I should see ""(.*)"" in my locations list")]
        public void ThenIShouldSeeInMyLocationsList(string locationTitle)
        {
            Assert.That(_listLocationsPage.WaitForLocationsToLoad(), Is.True, "Locations failed to load");

            var locationTitles = _listLocationsPage.GetLocationTitles();
            Assert.That(locationTitles.Contains(locationTitle), Is.True, $"Location '{locationTitle}' not found in list");
        }

        [Then(@"I should see a validation error message")]
        public void ThenIShouldSeeAValidationErrorMessage()
        {
            Assert.That(_addLocationPage.HasError(), Is.True, "No validation error displayed");
        }

        [Then(@"the location should not be saved")]
        public void ThenTheLocationShouldNotBeSaved()
        {
            // Still on the add location page
            Assert.That(_addLocationPage.IsCurrentPage(), Is.True, "Not on add location page");
        }

        [Then(@"the latitude and longitude should be automatically populated with my current position")]
        public void ThenTheLatitudeAndLongitudeShouldBeAutomaticallyPopulatedWithMyCurrentPosition()
        {
            string latitude = _addLocationPage.GetLatitude();
            string longitude = _addLocationPage.GetLongitude();

            Assert.That(string.IsNullOrEmpty(latitude), Is.False, "Latitude not populated");
            Assert.That(string.IsNullOrEmpty(longitude), Is.False, "Longitude not populated");

            // Verify these are valid coordinate values
            double lat, lon;
            Assert.That(double.TryParse(latitude, out lat), Is.True, "Latitude is not a valid number");
            Assert.That(double.TryParse(longitude, out lon), Is.True, "Longitude is not a valid number");
            Assert.That(lat >= -90 && lat <= 90, Is.True, "Latitude out of valid range");
            Assert.That(lon >= -180 && lon <= 180, Is.True, "Longitude out of valid range");
        }

        [Then(@"the camera should open")]
        public void ThenTheCameraShouldOpen()
        {
            // This is challenging to test in an automated environment
            // We might need to mock camera behavior or check if a dialog appears
            MarkAsPending("Camera interaction testing not implemented");
        }

        [Then(@"I should be able to take a photo")]
        public void ThenIShouldBeAbleToTakeAPhoto()
        {
            // This is challenging to test in an automated environment
            MarkAsPending("Camera interaction testing not implemented");
        }

        [Then(@"the photo should be attached to the location")]
        public void ThenThePhotoShouldBeAttachedToTheLocation()
        {
            // Verify photo attachment - implementation depends on UI design
            MarkAsPending("Photo attachment verification not implemented");
        }

        [Then(@"I should see ""(.*)""")]
        public void ThenIShouldSee(string result)
        {
            if (result == "Location saved successfully")
            {
                Assert.That(_addLocationPage.WaitForSaveToComplete(), Is.True, "Save operation did not complete");
                Assert.That(_listLocationsPage.IsCurrentPage(), Is.True, "Not returned to locations list");
            }
            else
            {
                Assert.That(_addLocationPage.HasError(), Is.True, "No validation error displayed");
                string errorMessage = _addLocationPage.GetErrorMessage();
                Assert.That(errorMessage.Contains(result), Is.True, $"Error message '{errorMessage}' does not match expected '{result}'");
            }
        }
    }
}