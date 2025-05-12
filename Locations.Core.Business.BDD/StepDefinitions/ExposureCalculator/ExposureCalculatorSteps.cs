using Locations.Core.Business.BDD.TestHelpers;
using Locations.Core.Business.DataAccess.Interfaces;
using Locations.Core.Shared.ViewModels;
using NUnit.Framework;
using TechTalk.SpecFlow;
using static Location.Photography.Shared.ViewModels.ExposureCalculator;
using Assert = NUnit.Framework.Assert;

namespace Locations.Core.Business.BDD.StepDefinitions.ExposureCalculator
{
    [Binding]
    public class ExposureCalculatorSteps
    {
        private readonly BDDTestContext _testContext;
        private readonly ISettingService<SettingViewModel> _settingsService;
        private Location.Photography.Shared.ViewModels.ExposureCalculator _calculator;
        private string _errorMessage;

        public ExposureCalculatorSteps(
            BDDTestContext testContext,
            ISettingService<SettingViewModel> settingsService)
        {
            _testContext = testContext;
            _settingsService = settingsService;
        }

        [Given(@"I am on the exposure calculator page")]
        public void GivenIAmOnTheExposureCalculatorPage()
        {
            // Check subscription status
            if (_testContext.SubscriptionType != "Premium")
            {
                var adViewedSetting = _settingsService.GetSettingByName("ExposureCalcAdViewed_TimeStamp");
                if (adViewedSetting == null || string.IsNullOrEmpty(adViewedSetting.Value))
                {
                    Assert.Inconclusive("Exposure calculator requires premium subscription or ad viewing");
                }
                else
                {
                    if (DateTime.TryParse(adViewedSetting.Value, out DateTime adViewedTime))
                    {
                        var adGivesHoursSetting = _settingsService.GetSettingByName("AdGivesHours");
                        int hours = int.Parse(adGivesHoursSetting?.Value ?? "24");

                        if (DateTime.Now > adViewedTime.AddHours(hours))
                        {
                            Assert.Inconclusive("Temporary ad-based access has expired");
                        }
                    }
                }
            }

            // Initialize calculator
            _calculator = TestDataFactory.CreateTestExposureCalculator();
            _errorMessage = string.Empty;
        }

        [Given(@"I have set initial exposure values")]
        public void GivenIHaveSetInitialExposureValues()
        {
            _calculator.FStopSelected = "f/2.8";
            _calculator.ShutterSpeedSelected = "1/125";
            _calculator.ISOSelected = "400";
            _calculator.OldFstop = "f/2.8";
            _calculator.OldShutterSpeed = "1/125";
            _calculator.OldISO = "400";
        }

        [Given(@"I select to calculate aperture")]
        public void GivenISelectToCalculateAperture()
        {
            _calculator.ToCalculate = FixedValue.Apeature;
        }

        [Given(@"I select to calculate shutter speed")]
        public void GivenISelectToCalculateShutterSpeed()
        {
            _calculator.ToCalculate = FixedValue.ShutterSpeeds;
        }

        [Given(@"I select to calculate ISO")]
        public void GivenISelectToCalculateISO()
        {
            _calculator.ToCalculate = FixedValue.ISOs;
        }

        [When(@"I set aperture to ""(.*)""")]
        public void WhenISetApertureTo(string aperture)
        {
            _calculator.FStopSelected = aperture;
            _calculator.OldFstop = aperture;
        }

        [When(@"I set shutter speed to ""(.*)""")]
        public void WhenISetShutterSpeedTo(string shutterSpeed)
        {
            _calculator.ShutterSpeedSelected = shutterSpeed;
            _calculator.OldShutterSpeed = shutterSpeed;
        }

        [When(@"I set ISO to ""(.*)""")]
        public void WhenISetISOTo(string iso)
        {
            _calculator.ISOSelected = iso;
            _calculator.OldISO = iso;
        }

        [When(@"I change aperture to ""(.*)""")]
        public void WhenIChangeApertureTo(string aperture)
        {
            _calculator.FStopSelected = aperture;
        }

        [When(@"I change shutter speed to ""(.*)""")]
        public void WhenIChangeShutterSpeedTo(string shutterSpeed)
        {
            _calculator.ShutterSpeedSelected = shutterSpeed;
        }

        [When(@"I change ISO to ""(.*)""")]
        public void WhenIChangeISOTo(string iso)
        {
            _calculator.ISOSelected = iso;
        }

        [When(@"I select ""(.*)"" stop increments")]
        public void WhenISelectStopIncrements(string incrementType)
        {
            switch (incrementType)
            {
                case "Full":
                    _calculator.FullHalfThirds = Location.Photography.Shared.ViewModels.ExposureCalculator.Divisions.Full;
                    break;
                case "Half":
                    _calculator.FullHalfThirds = Location.Photography.Shared.ViewModels.ExposureCalculator.Divisions.Half;
                    break;
                case "Thirds":
                    _calculator.FullHalfThirds = Location.Photography.Shared.ViewModels.ExposureCalculator.Divisions.Thirds;
                    break;
            }
        }

        [When(@"I tap the calculate button")]
        public void WhenITapTheCalculateButton()
        {
            try
            {
                _calculator.Calculate();
            }
            catch (Exception ex)
            {
                _errorMessage = ex.Message;
            }
        }

        [Then(@"the initial exposure values should be saved")]
        public void ThenTheInitialExposureValuesShouldBeSaved()
        {
            Assert.That(_calculator.OldFstop, Is.EqualTo(_calculator.FStopSelected));
            Assert.That(_calculator.OldShutterSpeed, Is.EqualTo(_calculator.ShutterSpeedSelected));
            Assert.That(_calculator.OldISO, Is.EqualTo(_calculator.ISOSelected));
        }

        [Then(@"the calculated aperture should be ""(.*)""")]
        public void ThenTheCalculatedApertureShouldBe(string aperture)
        {
            WhenITapTheCalculateButton();
            Assert.That(_calculator.FStopResult, Is.EqualTo(aperture));
        }

        [Then(@"the calculated shutter speed should be ""(.*)""")]
        public void ThenTheCalculatedShutterSpeedShouldBe(string shutterSpeed)
        {
            WhenITapTheCalculateButton();
            Assert.That(_calculator.ShutterSpeedResult, Is.EqualTo(shutterSpeed));
        }

        [Then(@"the calculated ISO should be ""(.*)""")]
        public void ThenTheCalculatedISOShouldBe(string iso)
        {
            WhenITapTheCalculateButton();
            Assert.That(_calculator.ISOResult, Is.EqualTo(iso));
        }

        [Then(@"the exposure values should update to full stop options")]
        public void ThenTheExposureValuesShouldUpdateToFullStopOptions()
        {
            var apertures = _calculator.ApeaturesForPicker;
            var shutterSpeeds = _calculator.ShutterSpeedsForPicker;
            var isos = _calculator.ISOsForPicker;

            Assert.That(apertures, Is.Not.Null);
            Assert.That(shutterSpeeds, Is.Not.Null);
            Assert.That(isos, Is.Not.Null);
            Assert.That(_calculator.FullHalfThirds, Is.EqualTo(Location.Photography.Shared.ViewModels.ExposureCalculator.Divisions.Full));
        }

        [Then(@"the exposure values should update to half stop options")]
        public void ThenTheExposureValuesShouldUpdateToHalfStopOptions()
        {
            var apertures = _calculator.ApeaturesForPicker;
            var shutterSpeeds = _calculator.ShutterSpeedsForPicker;
            var isos = _calculator.ISOsForPicker;

            Assert.That(apertures, Is.Not.Null);
            Assert.That(shutterSpeeds, Is.Not.Null);
            Assert.That(isos, Is.Not.Null);
            Assert.That(_calculator.FullHalfThirds, Is.EqualTo(Location.Photography.Shared.ViewModels.ExposureCalculator.Divisions.Half));
        }

        [Then(@"the exposure values should update to third stop options")]
        public void ThenTheExposureValuesShouldUpdateToThirdStopOptions()
        {
            var apertures = _calculator.ApeaturesForPicker;
            var shutterSpeeds = _calculator.ShutterSpeedsForPicker;
            var isos = _calculator.ISOsForPicker;

            Assert.That(apertures, Is.Not.Null);
            Assert.That(shutterSpeeds, Is.Not.Null);
            Assert.That(isos, Is.Not.Null);
            Assert.That(_calculator.FullHalfThirds, Is.EqualTo(Location.Photography.Shared.ViewModels.ExposureCalculator.Divisions.Thirds));
        }

        [Then(@"I should see an overexposure warning")]
        public void ThenIShouldSeeAnOverexposureWarning()
        {
            WhenITapTheCalculateButton();
            Assert.That(string.IsNullOrEmpty(_errorMessage), Is.False);
            Assert.That(_errorMessage.ToLower().Contains("overexpos") ||
                       _errorMessage.ToLower().Contains("too bright") ||
                       _calculator.FStopResult == "Too Bright", Is.True);
        }

        [Then(@"I should see an underexposure warning")]
        public void ThenIShouldSeeAnUnderexposureWarning()
        {
            WhenITapTheCalculateButton();
            Assert.That(string.IsNullOrEmpty(_errorMessage), Is.False);
            Assert.That(_errorMessage.ToLower().Contains("underexpos") ||
                       _errorMessage.ToLower().Contains("too dark") ||
                       _calculator.ShutterSpeedResult == "Too Dark", Is.True);
        }
    }
}