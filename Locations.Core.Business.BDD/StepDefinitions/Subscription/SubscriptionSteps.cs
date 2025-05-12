using Locations.Core.Business.BDD.TestHelpers;
using Locations.Core.Business.DataAccess.Interfaces;
using Locations.Core.Data.Models;
using Locations.Core.Data.Queries.Interfaces;
using Locations.Core.Shared;
using Locations.Core.Shared.ViewModels;
using Moq;
using NUnit.Framework;
using TechTalk.SpecFlow;
using Assert = NUnit.Framework.Assert;

namespace Locations.Core.Business.BDD.StepDefinitions.Subscription
{
    [Binding]
    public class SubscriptionSteps
    {
        private readonly ISettingService<SettingViewModel> _settingsService;
        private readonly BDDTestContext _testContext;
        private readonly Mock<ISettingsRepository> _mockSettingsRepository;

        private bool _subscriptionUpgraded;
        private bool _adWatched;
        private string _subscriptionType;
        private DateTime _subscriptionExpiration;
        private bool _showingSubscriptionOptions;
        private string _premiumFeatureAttempted;

        public SubscriptionSteps(
            ISettingService<SettingViewModel> settingsService,
            BDDTestContext testContext,
            Mock<ISettingsRepository> mockSettingsRepository)
        {
            _settingsService = settingsService;
            _testContext = testContext;
            _mockSettingsRepository = mockSettingsRepository;
        }

        [Given(@"I have a free account")]
        public async Task GivenIHaveAFreeAccount()
        {
            _subscriptionType = "Free";
            _testContext.SubscriptionType = "Free";

            var subscriptionSetting = new SettingViewModel
            {
                Key = MagicStrings.SubscriptionType,
                Value = "Free"
            };

            var expirationSetting = new SettingViewModel
            {
                Key = MagicStrings.SubscriptionExpiration,
                Value = DateTime.Now.AddDays(-1).ToString()
            };

            _mockSettingsRepository.Setup(x => x.GetByNameAsync(MagicStrings.SubscriptionType))
                .ReturnsAsync(DataOperationResult<SettingViewModel>.Success(subscriptionSetting));

            _mockSettingsRepository.Setup(x => x.GetByNameAsync(MagicStrings.SubscriptionExpiration))
                .ReturnsAsync(DataOperationResult<SettingViewModel>.Success(expirationSetting));

            await _settingsService.SaveAsync(subscriptionSetting);
            await _settingsService.SaveAsync(expirationSetting);
        }

        [Given(@"I have an active premium subscription")]
        public async Task GivenIHaveAnActivePremiumSubscription()
        {
            _subscriptionType = "Premium";
            _testContext.SubscriptionType = "Premium";
            _subscriptionExpiration = DateTime.Now.AddDays(30);

            var subscriptionSetting = new SettingViewModel
            {
                Key = MagicStrings.SubscriptionType,
                Value = "Premium"
            };

            var expirationSetting = new SettingViewModel
            {
                Key = MagicStrings.SubscriptionExpiration,
                Value = _subscriptionExpiration.ToString()
            };

            _mockSettingsRepository.Setup(x => x.GetByNameAsync(MagicStrings.SubscriptionType))
                .ReturnsAsync(DataOperationResult<SettingViewModel>.Success(subscriptionSetting));

            _mockSettingsRepository.Setup(x => x.GetByNameAsync(MagicStrings.SubscriptionExpiration))
                .ReturnsAsync(DataOperationResult<SettingViewModel>.Success(expirationSetting));

            await _settingsService.SaveAsync(subscriptionSetting);
            await _settingsService.SaveAsync(expirationSetting);
        }

        [Given(@"I have an expired premium subscription")]
        public async Task GivenIHaveAnExpiredPremiumSubscription()
        {
            _subscriptionType = "Premium";
            _testContext.SubscriptionType = "Premium";
            _subscriptionExpiration = DateTime.Now.AddDays(-10);

            var subscriptionSetting = new SettingViewModel
            {
                Key = MagicStrings.SubscriptionType,
                Value = "Premium"
            };

            var expirationSetting = new SettingViewModel
            {
                Key = MagicStrings.SubscriptionExpiration,
                Value = _subscriptionExpiration.ToString()
            };

            _mockSettingsRepository.Setup(x => x.GetByNameAsync(MagicStrings.SubscriptionType))
                .ReturnsAsync(DataOperationResult<SettingViewModel>.Success(subscriptionSetting));

            _mockSettingsRepository.Setup(x => x.GetByNameAsync(MagicStrings.SubscriptionExpiration))
                .ReturnsAsync(DataOperationResult<SettingViewModel>.Success(expirationSetting));

            await _settingsService.SaveAsync(subscriptionSetting);
            await _settingsService.SaveAsync(expirationSetting);
        }

        [Given(@"I am on the subscription options page")]
        public void GivenIAmOnTheSubscriptionOptionsPage()
        {
            _showingSubscriptionOptions = true;
        }

        [When(@"I attempt to access a premium feature")]
        public void WhenIAttemptToAccessAPremiumFeature()
        {
            _premiumFeatureAttempted = "ExposureCalculator";

            // Check subscription status
            if (_subscriptionType == "Free" ||
                (_subscriptionType == "Premium" && _subscriptionExpiration < DateTime.Now))
            {
                _showingSubscriptionOptions = true;
            }
        }

        [When(@"I select the subscription option")]
        public void WhenISelectTheSubscriptionOption()
        {
            _subscriptionUpgraded = true;
        }

        [When(@"I complete the payment process")]
        public async Task WhenICompleteThePaymentProcess()
        {
            if (_subscriptionUpgraded)
            {
                _subscriptionType = "Premium";
                _testContext.SubscriptionType = "Premium";
                _subscriptionExpiration = DateTime.Now.AddDays(365);

                var subscriptionSetting = new SettingViewModel
                {
                    Key = MagicStrings.SubscriptionType,
                    Value = "Premium"
                };

                var expirationSetting = new SettingViewModel
                {
                    Key = MagicStrings.SubscriptionExpiration,
                    Value = _subscriptionExpiration.ToString()
                };

                await _settingsService.SaveAsync(subscriptionSetting);
                await _settingsService.SaveAsync(expirationSetting);
            }
        }

        [When(@"I select the watch ad option")]
        public void WhenISelectTheWatchAdOption()
        {
            _adWatched = true;
        }

        [When(@"I complete viewing the ad")]
        public async Task WhenICompleteViewingTheAd()
        {
            if (_adWatched)
            {
                // Grant temporary access
                var adViewedSetting = new SettingViewModel
                {
                    Key = $"{_premiumFeatureAttempted}AdViewed_TimeStamp",
                    Value = DateTime.Now.ToString()
                };

                await _settingsService.SaveAsync(adViewedSetting);
            }
        }

        [When(@"I access a premium feature")]
        public void WhenIAccessAPremiumFeature()
        {
            _premiumFeatureAttempted = "ExposureCalculator";

            // Check if access is allowed
            bool hasAccess = false;

            if (_subscriptionType == "Premium" && _subscriptionExpiration > DateTime.Now)
            {
                hasAccess = true;
            }
            else
            {
                // Check for temporary ad-based access
                var adViewedSetting = _settingsService.GetSettingByName($"{_premiumFeatureAttempted}AdViewed_TimeStamp");
                if (adViewedSetting != null && !string.IsNullOrEmpty(adViewedSetting.Value))
                {
                    if (DateTime.TryParse(adViewedSetting.Value, out DateTime adViewedTime))
                    {
                        // Check if within the access window (e.g., 24 hours)
                        var adGivesHoursSetting = _settingsService.GetSettingByName(MagicStrings.AdGivesHours);
                        int hours = int.Parse(adGivesHoursSetting?.Value ?? "24");

                        if (DateTime.Now < adViewedTime.AddHours(hours))
                        {
                            hasAccess = true;
                        }
                    }
                }
            }

            if (!hasAccess)
            {
                _showingSubscriptionOptions = true;
            }
        }

        [Then(@"I should be shown the subscription options page")]
        public void ThenIShouldBeShownTheSubscriptionOptionsPage()
        {
            Assert.That(_showingSubscriptionOptions, Is.True);
        }

        [Then(@"I should see options to subscribe or watch an ad")]
        public void ThenIShouldSeeOptionsToSubscribeOrWatchAnAd()
        {
            Assert.That(_showingSubscriptionOptions, Is.True);
        }

        [Then(@"my account should be upgraded to premium")]
        public void ThenMyAccountShouldBeUpgradedToPremium()
        {
            Assert.That(_subscriptionType, Is.EqualTo("Premium"));
            Assert.That(_testContext.SubscriptionType, Is.EqualTo("Premium"));
        }

        [Then(@"I should have access to all premium features")]
        public void ThenIShouldHaveAccessToAllPremiumFeatures()
        {
            Assert.That(_subscriptionType, Is.EqualTo("Premium"));
            Assert.That(_subscriptionExpiration, Is.GreaterThan(DateTime.Now));
        }

        [Then(@"my subscription expiration date should be set correctly")]
        public void ThenMySubscriptionExpirationDateShouldBeSetCorrectly()
        {
            Assert.That(_subscriptionExpiration, Is.GreaterThan(DateTime.Now));
            Assert.That(_subscriptionExpiration, Is.LessThanOrEqualTo(DateTime.Now.AddDays(366)));
        }

        [Then(@"I should have temporary access to the premium feature")]
        public void ThenIShouldHaveTemporaryAccessToThePremiumFeature()
        {
            var adViewedSetting = _settingsService.GetSettingByName($"{_premiumFeatureAttempted}AdViewed_TimeStamp");
            Assert.That(adViewedSetting, Is.Not.Null);
            Assert.That(string.IsNullOrEmpty(adViewedSetting.Value), Is.False);
        }

        [Then(@"the access duration should be set correctly")]
        public void ThenTheAccessDurationShouldBeSetCorrectly()
        {
            var adGivesHoursSetting = _settingsService.GetSettingByName(MagicStrings.AdGivesHours);
            Assert.That(adGivesHoursSetting, Is.Not.Null);

            int hours = int.Parse(adGivesHoursSetting.Value ?? "24");
            Assert.That(hours, Is.GreaterThan(0));
        }

        [Then(@"I should be able to use the feature without interruption")]
        public void ThenIShouldBeAbleToUseTheFeatureWithoutInterruption()
        {
            Assert.That(_subscriptionType, Is.EqualTo("Premium"));
            Assert.That(_subscriptionExpiration, Is.GreaterThan(DateTime.Now));
            Assert.That(_showingSubscriptionOptions, Is.False);
        }

        [Then(@"I should see options to renew my subscription or watch an ad")]
        public void ThenIShouldSeeOptionsToRenewMySubscriptionOrWatchAnAd()
        {
            Assert.That(_showingSubscriptionOptions, Is.True);
            Assert.That(_subscriptionType, Is.EqualTo("Premium"));
            Assert.That(_subscriptionExpiration, Is.LessThan(DateTime.Now));
        }

        [Then(@"premium feature buttons should show a locked indicator")]
        public void ThenPremiumFeatureButtonsShouldShowALockedIndicator()
        {
            Assert.That(_subscriptionType, Is.EqualTo("Free"));
        }

        [Then(@"attempting to access premium features should show subscription options")]
        public void ThenAttemptingToAccessPremiumFeaturesShouldShowSubscriptionOptions()
        {
            WhenIAttemptToAccessAPremiumFeature();
            Assert.That(_showingSubscriptionOptions, Is.True);
        }
    }
}