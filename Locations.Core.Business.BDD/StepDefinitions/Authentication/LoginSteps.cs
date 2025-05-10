using TechTalk.SpecFlow;
using NUnit.Framework;
using Locations.Core.Business.Tests.UITests.PageObjects.Authentication;

namespace Locations.Core.Business.BDD.StepDefinitions.Authentication
{
    [Binding]
    public class LoginSteps
    {
        private readonly ScenarioContext _scenarioContext;
        private LoginPage _loginPage;

        public LoginSteps(ScenarioContext scenarioContext)
        {
            _scenarioContext = scenarioContext;
            _loginPage = new LoginPage(AppiumDriver.Current, AppiumDriver.CurrentPlatform);
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

        [When(@"I click the save button")]
        public void WhenIClickTheSaveButton()
        {
            _loginPage.ClickSave();
        }

        [Then(@"I should be logged in successfully")]
        public void ThenIShouldBeLoggedInSuccessfully()
        {
            Assert.That(_loginPage.WaitForProcessingToComplete(), Is.True, "Processing did not complete");
            Assert.That(_loginPage.IsCurrentPage(), Is.False, "Still on login page after valid login");
        }

        [Then(@"I should see an email validation message")]
        public void ThenIShouldSeeAnEmailValidationMessage()
        {
            Assert.That(_loginPage.IsEmailValidationDisplayed(), Is.True, "Email validation message not displayed");
        }
    }
}