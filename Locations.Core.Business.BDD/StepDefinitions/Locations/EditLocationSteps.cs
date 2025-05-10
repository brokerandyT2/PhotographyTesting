using TechTalk.SpecFlow;
using System.Threading;
using Locations.Core.Business.BDD.Support;
using Locations.Core.Business.Tests.UITests.PageObjects.Authentication;
using Locations.Core.Business.Tests.UITests.PageObjects.Locations;
using Locations.Core.Business.Tests.UITests.PageObjects.Weather;
using TechTalk.SpecFlow.Infrastructure;
using Assert = NUnit.Framework.Assert;
using NUnit.Framework;

namespace Locations.Core.Business.BDD.StepDefinitions.Locations
{
    [Binding]
    public class EditLocationSteps
    {
        private readonly AppiumDriverWrapper _driverWrapper;
        private readonly ScenarioContext _scenarioContext;
        private EditLocationPage _editLocationPage;
        private ListLocationsPage _listLocationsPage;
        private WeatherDisplayPage _weatherDisplayPage;
        private string _originalTitle;
        private string _originalDescription;

        public EditLocationSteps(AppiumDriverWrapper driverWrapper, ScenarioContext scenarioContext)
        {
            _driverWrapper = driverWrapper;
            _scenarioContext = scenarioContext;
            _editLocationPage = new EditLocationPage(_driverWrapper.Driver, _driverWrapper.Platform);
            _listLocationsPage = new ListLocationsPage(_driverWrapper.Driver, _driverWrapper.Platform);
        }

        // Helper method for pending steps
        private void MarkAsPending(string message = "This step is not yet implemented")
        {
            throw new PendingStepException(message);
        }

        [Given(@"I have at least one saved location")]
        public void GivenIHaveAtLeastOneSavedLocation()
        {
            Assert.That(_listLocationsPage.IsCurrentPage(), Is.True, "Not on the locations list page");
            Assert.That(_listLocationsPage.WaitForLocationsToLoad(), Is.True, "Locations failed to load");
            Assert.That(_listLocationsPage.HasLocations(), Is.True, "No locations found");
        }

        [Given(@"I have selected a location to edit")]
        public void GivenIHaveSelectedALocationToEdit()
        {
            if (_listLocationsPage.IsCurrentPage())
            {
                _listLocationsPage.SelectLocation(0);
                Thread.Sleep(2000); // Wait for navigation
            }

            Assert.That(_editLocationPage.IsCurrentPage(), Is.True, "Not on edit location page");

            // Store original values for later comparison
            _originalTitle = _editLocationPage.GetTitle();
            _originalDescription = _editLocationPage.GetDescription();
        }

        [When(@"I change the location title to ""(.*)""")]
        public void WhenIChangeTheLocationTitleTo(string newTitle)
        {
            _editLocationPage.EnterTitle(newTitle);
        }

        [When(@"I change the location description to ""(.*)""")]
        public void WhenIChangeTheLocationDescriptionTo(string newDescription)
        {
            _editLocationPage.EnterDescription(newDescription);
        }

        [When(@"I tap the delete button")]
        public void WhenITapTheDeleteButton()
        {
            // Implementation depends on how delete functionality is exposed
            // _editLocationPage.ClickDelete();
            MarkAsPending("Delete functionality not implemented in page object");
        }

        [When(@"I confirm the deletion")]
        public void WhenIConfirmTheDeletion()
        {
            // Implementation depends on how confirmation is implemented
            // _editLocationPage.ConfirmDelete();
            MarkAsPending("Delete confirmation not implemented in page object");
        }

        [When(@"I tap the weather button")]
        public void WhenITapTheWeatherButton()
        {
            _editLocationPage.ClickWeatherButton();
            Thread.Sleep(2000); // Wait for navigation

            _weatherDisplayPage = new WeatherDisplayPage(_driverWrapper.Driver, _driverWrapper.Platform);
        }

        [When(@"I tap the sun events button")]
        public void WhenITapTheSunEventsButton()
        {
            _editLocationPage.ClickSunEventsButton();
            Thread.Sleep(2000); // Wait for navigation
        }

        [When(@"I make changes to the location")]
        public void WhenIMakeChangesToTheLocation()
        {
            _editLocationPage.EnterTitle(_originalTitle + " (modified)");
            _editLocationPage.EnterDescription(_originalDescription + " (modified)");
        }

        [When(@"I tap the close button without saving")]
        public void WhenITapTheCloseButtonWithoutSaving()
        {
            _editLocationPage.ClickClose();
        }

        [When(@"I confirm discarding changes")]
        public void WhenIConfirmDiscardingChanges()
        {
            // Implementation depends on how confirmation is exposed
            // _editLocationPage.ConfirmDiscard();
            MarkAsPending("Confirm discard changes functionality not implemented in page object");
        }

        [When(@"I tap the change photo button")]
        public void WhenITapTheChangePhotoButton()
        {
            // Implementation depends on how photo functionality is exposed
            // _editLocationPage.ChangePhoto();
            MarkAsPending("Change photo functionality not implemented in page object");
        }

        [When(@"I save the location")]
        public void WhenISaveTheLocation()
        {
            _editLocationPage.ClickSave();
            Thread.Sleep(2000); // Wait for save to complete
        }

        [Then(@"the location should be updated with the new title")]
        public void ThenTheLocationShouldBeUpdatedWithTheNewTitle()
        {
            // Verify title has been updated - this might require refreshing the page or navigating back
            var updatedTitle = _editLocationPage.GetTitle();
            Assert.That(!string.IsNullOrEmpty(updatedTitle), Is.True, "Title is empty after update");
            Assert.That(updatedTitle != _originalTitle, Is.True, "Title was not updated");
        }

        [Then(@"I should see ""(.*)"" in my locations list")]
        public void ThenIShouldSeeInMyLocationsList(string locationTitle)
        {
            // Navigate back to list if needed
            if (!_listLocationsPage.IsCurrentPage())
            {
                _editLocationPage.ClickClose();
                Thread.Sleep(2000); // Wait for navigation
            }

            Assert.That(_listLocationsPage.WaitForLocationsToLoad(), Is.True, "Locations failed to load");

            var locationTitles = _listLocationsPage.GetLocationTitles();
            Assert.That(locationTitles.Contains(locationTitle), Is.True, $"Location '{locationTitle}' not found in list");
        }

        [Then(@"the location should be updated with the new description")]
        public void ThenTheLocationShouldBeUpdatedWithTheNewDescription()
        {
            // Verify description has been updated
            var updatedDescription = _editLocationPage.GetDescription();
            Assert.That(!string.IsNullOrEmpty(updatedDescription), Is.True, "Description is empty after update");
            Assert.That(updatedDescription != _originalDescription, Is.True, "Description was not updated");
        }

        [Then(@"the location should be removed from my saved locations")]
        public void ThenTheLocationShouldBeRemovedFromMySavedLocations()
        {
            // We should be back on the list page
            Assert.That(_listLocationsPage.IsCurrentPage(), Is.True, "Not on locations list page after deletion");
            Assert.That(_listLocationsPage.WaitForLocationsToLoad(), Is.True, "Locations failed to load");

            // Verify the deleted location is not in the list
            var locationTitles = _listLocationsPage.GetLocationTitles();
            Assert.That(!locationTitles.Contains(_originalTitle), Is.True, $"Deleted location '{_originalTitle}' still found in list");
        }

        [Then(@"I should be returned to the locations list")]
        public void ThenIShouldBeReturnedToTheLocationsList()
        {
            Assert.That(_listLocationsPage.IsCurrentPage(), Is.True, "Not returned to locations list");
        }

        [Then(@"I should not see the deleted location in my list")]
        public void ThenIShouldNotSeeTheDeletedLocationInMyList()
        {
            var locationTitles = _listLocationsPage.GetLocationTitles();
            Assert.That(!locationTitles.Contains(_originalTitle), Is.True, $"Deleted location '{_originalTitle}' still found in list");
        }

        [Then(@"I should see the weather details for this location")]
        public void ThenIShouldSeeTheWeatherDetailsForThisLocation()
        {
            Assert.That(_weatherDisplayPage.IsCurrentPage(), Is.True, "Not on weather display page");
            Assert.That(_weatherDisplayPage.HasWeatherData(), Is.True, "Weather data not displayed");
        }

        [Then(@"I should see forecast information for multiple days")]
        public void ThenIShouldSeeForecastInformationForMultipleDays()
        {
            // This depends on the weather display implementation
            // Could check that day one forecast exists and expand others
            Assert.That(!string.IsNullOrEmpty(_weatherDisplayPage.GetDayOneForecast()), Is.True, "Day one forecast not displayed");
        }

        [Then(@"I should see the sun calculator for this location")]
        public void ThenIShouldSeeTheSunCalculatorForThisLocation()
        {
            // Need a page object for the sun calculator
            // For now, just verify we're not on the edit location page anymore
            Assert.That(!_editLocationPage.IsCurrentPage(), Is.True, "Still on edit location page");
        }

        [Then(@"I should see sunrise and sunset times")]
        public void ThenIShouldSeeSunriseAndSunsetTimes()
        {
            // Need a page object for the sun calculator to access these elements
            MarkAsPending("Sun calculator page object not implemented");
        }

        [Then(@"I should see golden hour information")]
        public void ThenIShouldSeeGoldenHourInformation()
        {
            // Need a page object for the sun calculator to access these elements
            MarkAsPending("Sun calculator page object not implemented");
        }

        [Then(@"I should be prompted to confirm discarding changes")]
        public void ThenIShouldBePromptedToConfirmDiscardingChanges()
        {
            // This depends on how the confirmation dialog is implemented
            // _editLocationPage.IsDiscardConfirmationDisplayed()
            MarkAsPending("Discard confirmation dialog detection not implemented");
        }

        [Then(@"the location should remain unchanged")]
        public void ThenTheLocationShouldRemainUnchanged()
        {
            // Navigate back to the edit page to check
            if (!_editLocationPage.IsCurrentPage())
            {
                _listLocationsPage.SelectLocation(0);
                Thread.Sleep(2000); // Wait for navigation
            }

            var currentTitle = _editLocationPage.GetTitle();
            var currentDescription = _editLocationPage.GetDescription();

            Assert.That(currentTitle, Is.EqualTo(_originalTitle), "Title changed despite cancellation");
            Assert.That(currentDescription, Is.EqualTo(_originalDescription), "Description changed despite cancellation");
        }

        [Then(@"the location should be updated with the new photo")]
        public void ThenTheLocationShouldBeUpdatedWithTheNewPhoto()
        {
            // This would depend on how photos are displayed and verified
            MarkAsPending("Photo update verification not implemented");
        }
    }
}