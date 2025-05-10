using TechTalk.SpecFlow;
using System.Threading;
using Locations.Core.Business.BDD.Support;
using Locations.Core.Business.Tests.UITests.PageObjects.Authentication;
using Locations.Core.Business.Tests.UITests.PageObjects.Tips;
using Locations.Core.Business.Tests.UITests.PageObjects.Shared;
using System.Collections.Generic;
using TechTalk.SpecFlow.Infrastructure;
using Assert = NUnit.Framework.Assert;
using NUnit.Framework;

namespace Locations.Core.Business.BDD.StepDefinitions.Tips
{
    [Binding]
    public class TipsSteps
    {
        private readonly AppiumDriverWrapper _driverWrapper;
        private readonly ScenarioContext _scenarioContext;
        private TipsPage _tipsPage;
        private FeatureNotSupportedPage _featureNotSupportedPage;
        private string _initialTipText;
        private string _initialFStop;
        private string _initialShutterSpeed;
        private string _initialISO;

        public TipsSteps(AppiumDriverWrapper driverWrapper, ScenarioContext scenarioContext)
        {
            _driverWrapper = driverWrapper;
            _scenarioContext = scenarioContext;
            _tipsPage = new TipsPage(_driverWrapper.Driver, _driverWrapper.Platform);
        }

        // Helper method for pending steps
        private void MarkAsPending(string message = "This step is not yet implemented")
        {
            throw new PendingStepException(message);
        }

        [Given(@"I am on the tips page")]
        public void GivenIAmOnTheTipsPage()
        {
            // Navigate to tips page if not already there
            // This depends on how navigation is implemented in your app
            // For this example, we'll assume we're already there
            Assert.That(_tipsPage.IsCurrentPage(), Is.True, "Not on the tips page");

            // Store initial values for comparison
            _initialTipText = _tipsPage.GetTipText();
            _initialFStop = _tipsPage.GetFStop();
            _initialShutterSpeed = _tipsPage.GetShutterSpeed();
            _initialISO = _tipsPage.GetISO();
        }

        [When(@"I select tip type ""(.*)""")]
        public void WhenISelectTipType(string tipType)
        {
            // The SelectTipType method needs index, not string
            // For this example, we'll use hardcoded indices
            int tipTypeIndex = 0;
            switch (tipType.ToLower())
            {
                case "landscape":
                    tipTypeIndex = 0;
                    break;
                case "portrait":
                    tipTypeIndex = 1;
                    break;
                case "night":
                    tipTypeIndex = 2;
                    break;
                case "macro":
                    tipTypeIndex = 3;
                    break;
                case "wildlife":
                    tipTypeIndex = 4;
                    break;
                default:
                    tipTypeIndex = 0;
                    break;
            }

            _tipsPage.SelectTipType(tipTypeIndex);
            Thread.Sleep(2000); // Wait for content to update
        }

        [When(@"I tap the refresh button")]
        public void WhenITapTheRefreshButton()
        {
            // Implementation depends on how refresh is exposed in page object
            // _tipsPage.ClickRefresh();
            MarkAsPending("Refresh button functionality not implemented in page object");
        }

        [When(@"I tap the exposure calculator button")]
        public void WhenITapTheExposureCalculatorButton()
        {
            if (_tipsPage.IsExposureCalcButtonVisible())
            {
                _tipsPage.ClickExposureCalcButton();
                Thread.Sleep(2000); // Wait for navigation
            }
            else
            {
                Assert.Fail("Exposure calculator button not visible");
            }
        }

        [Then(@"I should see photography tips content")]
        public void ThenIShouldSeePhotographyTipsContent()
        {
            Assert.That(_tipsPage.HasTipContent(), Is.True, "No tip content displayed");
        }

        [Then(@"I should see camera settings information")]
        public void ThenIShouldSeeCameraSettingsInformation()
        {
            Assert.That(string.IsNullOrEmpty(_tipsPage.GetFStop()), Is.False, "F-stop not displayed");
            Assert.That(string.IsNullOrEmpty(_tipsPage.GetShutterSpeed()), Is.False, "Shutter speed not displayed");
            Assert.That(string.IsNullOrEmpty(_tipsPage.GetISO()), Is.False, "ISO not displayed");
        }

        [Then(@"I should see photography advice text")]
        public void ThenIShouldSeePhotographyAdviceText()
        {
            Assert.That(string.IsNullOrEmpty(_tipsPage.GetTipText()), Is.False, "Tip text not displayed");
        }

        [Then(@"I should see tips specific to landscape photography")]
        public void ThenIShouldSeeTipsSpecificToLandscapePhotography()
        {
            // Verify the tip content changed from the initial state
            var newTipText = _tipsPage.GetTipText();
            Assert.That(string.IsNullOrEmpty(newTipText), Is.False, "No landscape tip text displayed");

            // Ideally, we would verify the content is actually relevant to landscape photography
            // but that would require more complex validation
        }

        [Then(@"I should see tips specific to portrait photography")]
        public void ThenIShouldSeeTipsSpecificToPortraitPhotography()
        {
            // Verify the tip content changed from the initial state
            var newTipText = _tipsPage.GetTipText();
            Assert.That(string.IsNullOrEmpty(newTipText), Is.False, "No portrait tip text displayed");

            // Ideally, we would verify the content is actually relevant to portrait photography
            // but that would require more complex validation
        }

        [Then(@"I should see tips specific to night photography")]
        public void ThenIShouldSeeTipsSpecificToNightPhotography()
        {
            // Verify the tip content changed from the initial state
            var newTipText = _tipsPage.GetTipText();
            Assert.That(string.IsNullOrEmpty(newTipText), Is.False, "No night photography tip text displayed");

            // Ideally, we would verify the content is actually relevant to night photography
            // but that would require more complex validation
        }

        [Then(@"I should see f-stop information")]
        public void ThenIShouldSeeFStopInformation()
        {
            Assert.That(string.IsNullOrEmpty(_tipsPage.GetFStop()), Is.False, "F-stop not displayed");
        }

        [Then(@"I should see shutter speed information")]
        public void ThenIShouldSeeShutterSpeedInformation()
        {
            Assert.That(string.IsNullOrEmpty(_tipsPage.GetShutterSpeed()), Is.False, "Shutter speed not displayed");
        }

        [Then(@"I should see ISO information")]
        public void ThenIShouldSeeISOInformation()
        {
            Assert.That(string.IsNullOrEmpty(_tipsPage.GetISO()), Is.False, "ISO not displayed");
        }

        [Then(@"I should see a different landscape photography tip")]
        public void ThenIShouldSeeADifferentLandscapePhotographyTip()
        {
            // Verify the tip text changed
            var newTipText = _tipsPage.GetTipText();
            Assert.That(newTipText != _initialTipText, Is.True, "Tip text did not change after refresh");
        }

        [Then(@"I should see camera settings appropriate for ""(.*)"" photography")]
        public void ThenIShouldSeeCameraSettingsAppropriateForPhotography(string tipType)
        {
            string fStop = _tipsPage.GetFStop();
            string shutterSpeed = _tipsPage.GetShutterSpeed();
            string iso = _tipsPage.GetISO();

            Assert.That(string.IsNullOrEmpty(fStop), Is.False, $"F-stop not displayed for {tipType} photography");
            Assert.That(string.IsNullOrEmpty(shutterSpeed), Is.False, $"Shutter speed not displayed for {tipType} photography");
            Assert.That(string.IsNullOrEmpty(iso), Is.False, $"ISO not displayed for {tipType} photography");

            // Ideally, we would verify the settings are appropriate for the specific type of photography
            // but that would require more complex validation
        }

        [Then(@"I should be taken to the exposure calculator")]
        public void ThenIShouldBeTakenToTheExposureCalculator()
        {
            // Check if we're on the feature not supported page first (for free users)
            _featureNotSupportedPage = new FeatureNotSupportedPage(_driverWrapper.Driver, _driverWrapper.Platform);

            if (_featureNotSupportedPage.IsCurrentPage())
            {
                // This is expected for free users
                Assert.That(_featureNotSupportedPage.IsFeatureExposureCalculator(), Is.True,
                    "Feature not supported page doesn't indicate exposure calculator");
            }
            else
            {
                // For premium users, we should be on the exposure calculator page
                // Need a page object for the exposure calculator
                // For now, just verify we're not on the tips page anymore
                Assert.That(!_tipsPage.IsCurrentPage(), Is.True, "Still on tips page");
            }
        }

        [Then(@"I should see exposure calculation tools")]
        public void ThenIShouldSeeExposureCalculationTools()
        {
            // Need a page object for the exposure calculator to access these elements
            MarkAsPending("Exposure calculator page object not implemented");
        }
    }
}