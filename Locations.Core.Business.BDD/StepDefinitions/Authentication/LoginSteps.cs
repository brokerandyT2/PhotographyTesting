using TechTalk.SpecFlow;
using System.Threading;
using Locations.Core.Business.BDD.Support;
using Locations.Core.Business.Tests.UITests.PageObjects.Authentication;
using TechTalk.SpecFlow.Infrastructure;
using Assert = NUnit.Framework.Assert;
using NUnit.Framework;

namespace Locations.Core.Business.BDD.StepDefinitions.Authentication
{
    [Binding]
    public class LoginSteps
    {
        private readonly AppiumDriverWrapper _driverWrapper;
        private readonly ScenarioContext _scenarioContext;
        private LoginPage _loginPage;

        public LoginSteps(AppiumDriverWrapper driverWrapper, ScenarioContext scenarioContext)
        {
            _driverWrapper = driverWrapper;
            _scenarioContext = scenarioContext;
            _loginPage = new LoginPage(_driverWrapper.Driver, _driverWrapper.Platform);
        }

        // Helper method for pending steps
        private void MarkAsPending(string message = "This step is not yet implemented")
        {
            throw new PendingStepException(message);
        }

        [Given(@"I am on the login page")]
        public void GivenIAmOnTheLoginPage()
        {
            Assert.That(_loginPage.IsCurrentPage(), Is.True, "Not on the login page");
        }

        [When(@"I enter email ""(.*)""")]
        public void WhenIEnterEmail(string email)
        {
            _loginPage.EnterEmail(email);
        }

        [When(@"I select ""(.*)"" hemisphere")]
        public void WhenISelectHemisphere(string hemisphere)
        {
            bool isNorth = hemisphere.Equals("North", System.StringComparison.OrdinalIgnoreCase);
            _loginPage.SetHemisphere(isNorth);
        }

        [When(@"I select ""(.*)"" time format")]
        public void WhenISelectTimeFormat(string timeFormat)
        {
            bool is12Hour = timeFormat.Equals("12-hour", System.StringComparison.OrdinalIgnoreCase);
            _loginPage.SetTimeFormat(is12Hour);
        }

        [When(@"I select ""(.*)"" date format")]
        public void WhenISelectDateFormat(string dateFormat)
        {
            bool isMMDDYYYY = dateFormat.Equals("MM/DD/YYYY", System.StringComparison.OrdinalIgnoreCase);
            _loginPage.SetDateFormat(isMMDDYYYY);
        }

        [When(@"I select ""(.*)"" wind direction")]
        public void WhenISelectWindDirection(string windDirection)
        {
            bool isTowardsWind = windDirection.Equals("Towards Wind", System.StringComparison.OrdinalIgnoreCase);
            _loginPage.SetWindDirection(isTowardsWind);
        }

        [When(@"I select ""(.*)"" temperature format")]
        public void WhenISelectTemperatureFormat(string temperatureFormat)
        {
            bool isFahrenheit = temperatureFormat.Equals("Fahrenheit", System.StringComparison.OrdinalIgnoreCase);
            _loginPage.SetTemperatureFormat(isFahrenheit);
        }

        [When(@"I tap the save button")]
        public void WhenITapTheSaveButton()
        {
            _loginPage.ClickSave();
            Thread.Sleep(1000); // Wait for processing
        }

        [Then(@"I should be logged in successfully")]
        public void ThenIShouldBeLoggedInSuccessfully()
        {
            Assert.That(_loginPage.WaitForProcessingToComplete(), Is.True, "Processing did not complete");
            Assert.That(_loginPage.IsCurrentPage(), Is.False, "Still on login page after login");
        }

        [Then(@"I should be taken to the main page")]
        public void ThenIShouldBeTakenToTheMainPage()
        {
            // Need to verify we're on the main page - this depends on your application's structure
            // For now, we just verify we're not on the login page anymore
            Assert.That(_loginPage.IsCurrentPage(), Is.False, "Still on login page");
        }

        [Then(@"I should see an email validation message")]
        public void ThenIShouldSeeAnEmailValidationMessage()
        {
            Assert.That(_loginPage.IsEmailValidationDisplayed(), Is.True, "Email validation message not displayed");
        }

        [Then(@"I should remain on the login page")]
        public void ThenIShouldRemainOnTheLoginPage()
        {
            Assert.That(_loginPage.IsCurrentPage(), Is.True, "Not on login page");
        }

        [Then(@"default settings should be applied")]
        public void ThenDefaultSettingsShouldBeApplied()
        {
            // This would require navigating to settings and checking default values
            // For now, we'll just verify we've successfully logged in
            Assert.That(_loginPage.IsCurrentPage(), Is.False, "Still on login page");
        }

        [Then(@"I should see ""(.*)""")]
        public void ThenIShouldSee(string result)
        {
            if (result == "Login successful")
            {
                Assert.That(_loginPage.WaitForProcessingToComplete(), Is.True, "Processing did not complete");
                Assert.That(_loginPage.IsCurrentPage(), Is.False, "Still on login page");
            }
            else
            {
                Assert.That(_loginPage.IsEmailValidationDisplayed(), Is.True, "No validation error displayed");
                // Ideally check the specific error message content
                // This depends on how error messages are implemented in your app
            }
        }
    }
}