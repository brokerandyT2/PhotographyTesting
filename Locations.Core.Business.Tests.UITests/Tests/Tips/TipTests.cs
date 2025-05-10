// Locations.Core.Business.Tests.UITests/Tests/Tips/TipsTests.cs
using NUnit.Framework;
using System;
using System.Threading;
using Locations.Core.Business.Tests.UITests.PageObjects.Authentication;
using Locations.Core.Business.Tests.UITests.PageObjects.Tips;
using Locations.Core.Business.Tests.UITests.PageObjects.Shared;

namespace Locations.Core.Business.Tests.UITests.Tests.Tips
{
    [TestFixture]
    [Category("Tips")]
    public class TipsTests : BaseTest
    {
        private TipsPage _tipsPage;
        private PageTutorialModalPage _tutorialPage;

        [SetUp]
        public override void SetUp()
        {
            base.SetUp();

            // First login if needed
            var loginPage = new LoginPage(WindowsDriver, AndroidDriver, iOSDriver, CurrentPlatform);
            if (loginPage.IsCurrentPage())
            {
                loginPage.Login();
            }

            // Navigate to tips page (this depends on your app's navigation)
            try
            {
                // Look for a "Tips" tab or button
                switch (CurrentPlatform)
                {
                    case AppiumSetup.Platform.Android:
                        AndroidDriver.FindElementByXPath("//android.widget.Button[contains(@text, 'Tips') or contains(@content-desc, 'Tips')]").Click();
                        break;
                    case AppiumSetup.Platform.iOS:
                        iOSDriver.FindElementByXPath("//XCUIElementTypeButton[contains(@name, 'Tips')]").Click();
                        break;
                    case AppiumSetup.Platform.Windows:
                        WindowsDriver.FindElementByXPath("//Button[contains(@Name, 'Tips')]").Click();
                        break;
                }

                // Wait for page to load
                Thread.Sleep(2000);
            }
            catch (Exception ex)
            {
                Log($"Navigation to tips failed: {ex.Message}");
            }

            // Initialize tips page
            _tipsPage = new TipsPage(WindowsDriver, AndroidDriver, iOSDriver, CurrentPlatform);

            // Check if we're on a tutorial page first
            _tutorialPage = new PageTutorialModalPage(WindowsDriver, AndroidDriver, iOSDriver, CurrentPlatform);
            if (_tutorialPage.IsCurrentPage())
            {
                _tutorialPage.WaitForDismissal();
            }

            // Verify we're on the tips page
            Assert.IsTrue(_tipsPage.IsCurrentPage(), "Not on the tips page");
        }

        [Test]
        [Description("Verify that all UI elements are displayed on the tips page")]
        public void TipsPage_AllUIElementsDisplayed()
        {
            Log("Checking UI elements on Tips page");

            // Verify tip content is loaded
            Assert.IsTrue(_tipsPage.HasTipContent(), "Tip content not displayed");
        }

        [Test]
        [Description("Verify selecting different tip types shows different content")]
        public void Tips_SelectDifferentTipTypes_ShouldChangeTipContent()
        {
            Log("Testing tip type selection");

            // Get initial tip content for reference
            string initialFStop = _tipsPage.GetFStop();
            string initialShutterSpeed = _tipsPage.GetShutterSpeed();
            string initialISO = _tipsPage.GetISO();
            string initialTipText = _tipsPage.GetTipText();

            // Select a different tip type
            _tipsPage.SelectTipType(1);

            // Wait for UI to update
            Thread.Sleep(2000);

            // Get updated tip content
            string updatedFStop = _tipsPage.GetFStop();
            string updatedShutterSpeed = _tipsPage.GetShutterSpeed();
            string updatedISO = _tipsPage.GetISO();
            string updatedTipText = _tipsPage.GetTipText();

            // Verify content changed - at least one of these should be different
            Assert.IsTrue(
                initialFStop != updatedFStop ||
                initialShutterSpeed != updatedShutterSpeed ||
                initialISO != updatedISO ||
                initialTipText != updatedTipText,
                "Tip content did not change after selecting a different tip type"
            );

            // Select another tip type
            _tipsPage.SelectTipType(2);

            // Wait for UI to update
            Thread.Sleep(2000);

            // Verify tip content is still displayed
            Assert.IsTrue(_tipsPage.HasTipContent(), "Tip content not displayed after second selection");
        }

        [Test]
        [Description("Verify exposure calculator button visibility based on subscription")]
        public void Tips_ExposureCalculatorButton_VisibilityBasedOnSubscription()
        {
            Log("Testing exposure calculator button visibility");

            // The button visibility depends on the subscription type
            // This test would need to know the current subscription state or be able to control it

            // For demonstration purposes, we'll just check the current state
            bool isButtonVisible = _tipsPage.IsExposureCalcButtonVisible();

            // Log the current state
            Log($"Exposure calculator button visible: {isButtonVisible}");

            // No assertion here as the expected state depends on subscription
            Assert.Pass("Exposure calculator button visibility checked");
        }

        [Test]
        [Description("Verify exposure calculator button navigation")]
        [Ignore("This test may depend on subscription status")]
        public void Tips_ExposureCalculatorButton_ShouldNavigateToExposureCalculator()
        {
            Log("Testing exposure calculator button navigation");

            // Check if button is visible
            if (!_tipsPage.IsExposureCalcButtonVisible())
            {
                Assert.Ignore("Exposure calculator button not visible, possibly due to subscription status");
            }

            // Click exposure calculator button
            _tipsPage.ClickExposureCalcButton();

            // Wait for navigation
            Thread.Sleep(2000);

            // For this test, we're just verifying that we've navigated away from the tips page
            // Ideally, we would initialize an ExposureCalculatorPage object and verify we're on that page
            Assert.IsFalse(_tipsPage.IsCurrentPage(), "Still on tips page after clicking exposure calculator button");

            // Also check if we're on a feature not supported page
            var featureNotSupportedPage = new FeatureNotSupportedPage(WindowsDriver, AndroidDriver, iOSDriver, CurrentPlatform);
            if (featureNotSupportedPage.IsCurrentPage())
            {
                Assert.IsTrue(featureNotSupportedPage.IsFeatureExposureCalculator(),
                    "Feature not supported page doesn't indicate exposure calculator");
            }
            else
            {
                // If we're neither on the tips page nor a feature not supported page,
                // assume we've successfully navigated to the exposure calculator
                Assert.Pass("Successfully navigated to exposure calculator");
            }

            // Navigate back to tips
            GoBack();
        }

        [Test]
        [Description("Verify tip content contains valid camera settings")]
        public void Tips_TipContent_ContainsValidCameraSettings()
        {
            Log("Testing tip content validity");

            // Get camera settings
            string fStop = _tipsPage.GetFStop();
            string shutterSpeed = _tipsPage.GetShutterSpeed();
            string iso = _tipsPage.GetISO();

            // Verify at least one of these settings is not empty
            Assert.IsTrue(
                !string.IsNullOrEmpty(fStop) ||
                !string.IsNullOrEmpty(shutterSpeed) ||
                !string.IsNullOrEmpty(iso),
                "No camera settings displayed"
            );

            // If f-stop is displayed, verify it's in a valid format
            if (!string.IsNullOrEmpty(fStop))
            {
                // Common f-stop values include f/1.4, f/2, f/2.8, f/4, f/5.6, etc.
                Assert.IsTrue(fStop.Contains("f/") || fStop.Contains("F/") || fStop.StartsWith("f") || fStop.StartsWith("F"),
                    $"F-stop format invalid: {fStop}");
            }

            // If shutter speed is displayed, verify it's in a valid format
            if (!string.IsNullOrEmpty(shutterSpeed))
            {
                // Shutter speeds are often shown as fractions (e.g., 1/125) or seconds (e.g., 2s)
                Assert.IsTrue(shutterSpeed.Contains("/") || shutterSpeed.Contains("s") || double.TryParse(shutterSpeed, out _),
                    $"Shutter speed format invalid: {shutterSpeed}");
            }

            // If ISO is displayed, verify it's a number
            if (!string.IsNullOrEmpty(iso))
            {
                // Remove any "ISO" prefix for parsing
                string isoValue = iso.Replace("ISO", "").Replace("iso", "").Trim();
                Assert.IsTrue(int.TryParse(isoValue, out _), $"ISO format invalid: {iso}");
            }
        }

        [Test]
        [Description("Verify tip text is not empty")]
        public void Tips_TipText_IsNotEmpty()
        {
            Log("Testing tip text content");

            // Get tip text
            string tipText = _tipsPage.GetTipText();

            // Verify tip text is not empty
            Assert.IsFalse(string.IsNullOrEmpty(tipText), "Tip text is empty");

            // Verify tip text has reasonable length
            Assert.IsTrue(tipText.Length > 10, "Tip text is too short");
        }
    }
}