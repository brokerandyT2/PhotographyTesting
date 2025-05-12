using Locations.Core.Business.BDD.TestHelpers;
using Locations.Core.Business.DataAccess.Interfaces;
using Locations.Core.Data.Queries.Interfaces;
using Locations.Core.Shared;
using Locations.Core.Shared.ViewModels;
using Moq;
using NUnit.Framework;
using TechTalk.SpecFlow;
using Assert = NUnit.Framework.Assert;

namespace Locations.Core.Business.BDD.StepDefinitions.Authentication
{
    [Binding]
    [Scope(Feature = "Login")]
    public class LoginSteps
    {
        private readonly ISettingService<SettingViewModel> _settingsService;
        private readonly BDDTestContext _testContext;
        private readonly Mock<ISettingsRepository> _mockSettingsRepository;
        private string _currentEmail;
        private bool _loginSuccessful;
        private string _errorMessage;

        public LoginSteps(ISettingService<SettingViewModel> settingsService, BDDTestContext testContext, Mock<ISettingsRepository> mockSettingsRepository)
        {
            _settingsService = settingsService;
            _testContext = testContext;
            _mockSettingsRepository = mockSettingsRepository;
        }

        [Given(@"I am on the login page")]
        public void GivenIAmOnTheLoginPage()
        {
            // In service tests, we just verify we're not logged in
            _testContext.IsUserLoggedIn = false;
            _loginSuccessful = false;
            _errorMessage = string.Empty;
        }

        [When(@"I enter email ""(.*)""")]
        public void WhenIEnterEmail(string email)
        {
            _currentEmail = email;
            _testContext.CurrentUserEmail = email;
        }

        [When(@"I select ""(.*)"" hemisphere")]
        public void WhenISelectHemisphere(string hemisphere)
        {
            _testContext.TestSettings[MagicStrings.Hemisphere] = new SettingViewModel
            {
                Key = MagicStrings.Hemisphere,
                Value = hemisphere
            };
        }

        [When(@"I select ""(.*)"" time format")]
        public void WhenISelectTimeFormat(string timeFormat)
        {
            _testContext.TestSettings[MagicStrings.TimeFormat] = new SettingViewModel
            {
                Key = MagicStrings.TimeFormat,
                Value = timeFormat
            };
        }

        [When(@"I select ""(.*)"" date format")]
        public void WhenISelectDateFormat(string dateFormat)
        {
            _testContext.TestSettings[MagicStrings.DateFormat] = new SettingViewModel
            {
                Key = MagicStrings.DateFormat,
                Value = dateFormat
            };
        }

        [When(@"I select ""(.*)"" wind direction")]
        public void WhenISelectWindDirection(string windDirection)
        {
            _testContext.TestSettings[MagicStrings.WindDirection] = new SettingViewModel
            {
                Key = MagicStrings.WindDirection,
                Value = windDirection
            };
        }

        [When(@"I select ""(.*)"" temperature format")]
        public void WhenISelectTemperatureFormat(string temperatureFormat)
        {
            _testContext.TestSettings[MagicStrings.TemperatureType] = new SettingViewModel
            {
                Key = MagicStrings.TemperatureType,
                Value = temperatureFormat
            };
        }

        [When(@"I tap the login save button")]
        public async Task WhenITapTheLoginSaveButton()
        {
            // Validate email
            if (string.IsNullOrEmpty(_currentEmail))
            {
                _loginSuccessful = false;
                _errorMessage = "Email is required";
                return;
            }

            if (!IsValidEmail(_currentEmail))
            {
                _loginSuccessful = false;
                _errorMessage = "Please enter a valid email";
                return;
            }

            if (_currentEmail.Length > 100)
            {
                _loginSuccessful = false;
                _errorMessage = "Email exceeds maximum length";
                return;
            }

            // Save settings via service
            _testContext.TestSettings[MagicStrings.Email] = new SettingViewModel
            {
                Key = MagicStrings.Email,
                Value = _currentEmail
            };

            // Apply default settings if not specified
            if (!_testContext.TestSettings.ContainsKey(MagicStrings.Hemisphere))
                _testContext.TestSettings[MagicStrings.Hemisphere] = new SettingViewModel { Key = MagicStrings.Hemisphere, Value = "North" };

            if (!_testContext.TestSettings.ContainsKey(MagicStrings.TimeFormat))
                _testContext.TestSettings[MagicStrings.TimeFormat] = new SettingViewModel { Key = MagicStrings.TimeFormat, Value = "12-hour" };

            // Save all settings
            foreach (var setting in _testContext.TestSettings.Values)
            {
                var result = await _settingsService.SaveAsync(setting);
                if (!result.IsSuccess)
                {
                    _loginSuccessful = false;
                    _errorMessage = result.Message;
                    return;
                }
            }

            _loginSuccessful = true;
            _testContext.IsUserLoggedIn = true;
        }

        [Then(@"I should be logged in successfully")]
        public void ThenIShouldBeLoggedInSuccessfully()
        {
            Assert.That(_loginSuccessful, Is.True, "Login was not successful");
            Assert.That(_testContext.IsUserLoggedIn, Is.True, "User is not logged in");
        }

        [Then(@"I should be taken to the main page")]
        public void ThenIShouldBeTakenToTheMainPage()
        {
            // In service tests, we just verify login state
            Assert.That(_testContext.IsUserLoggedIn, Is.True, "User is not logged in");
        }

        [Then(@"I should see an email validation message")]
        public void ThenIShouldSeeAnEmailValidationMessage()
        {
            Assert.That(_loginSuccessful, Is.False, "Login should have failed");
            Assert.That(string.IsNullOrEmpty(_errorMessage), Is.False, "No error message provided");
        }

        [Then(@"I should remain on the login page")]
        public void ThenIShouldRemainOnTheLoginPage()
        {
            Assert.That(_testContext.IsUserLoggedIn, Is.False, "User should not be logged in");
        }

        [Then(@"default settings should be applied")]
        public void ThenDefaultSettingsShouldBeApplied()
        {
            Assert.That(_testContext.TestSettings.ContainsKey(MagicStrings.Hemisphere), Is.True);
            Assert.That(_testContext.TestSettings[MagicStrings.Hemisphere].Value, Is.EqualTo("North"));
        }

        [Then(@"I should see ""(.*)""")]
        public void ThenIShouldSee(string result)
        {
            if (result == "Login successful")
            {
                Assert.That(_loginSuccessful, Is.True, "Login was not successful");
            }
            else
            {
                Assert.That(_loginSuccessful, Is.False, "Login should have failed");
                Assert.That(_errorMessage, Is.EqualTo(result), $"Expected error: {result}, but got: {_errorMessage}");
            }
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}