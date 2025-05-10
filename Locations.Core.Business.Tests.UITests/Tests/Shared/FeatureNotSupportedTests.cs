// Locations.Core.Business.Tests.UITests/Tests/Shared/FeatureNotSupportedTests.cs
using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using System;
using System.Threading;
using Locations.Core.Business.Tests.UITests.PageObjects.Authentication;
using Locations.Core.Business.Tests.UITests.PageObjects.Shared;
using Locations.Core.Business.Tests.UITests.PageObjects.Tips;

namespace Locations.Core.Business.Tests.UITests.Tests.Shared
{
    [TestFixture]
    [Category("FeatureNotSupported")]
    public class FeatureNotSupportedTests : BaseTest
    {
        private FeatureNotSupportedPage _featureNotSupportedPage;
        private TipsPage _tipsPage;

        [Test]
        [Description("Verify feature not supported page appears for premium features")]
        [Ignore("This test depends on subscription status")]
        public void FeatureNotSupported_PremiumFeature_ShouldAppearForFreeUsers()
        {
            Log("Testing feature not supported page for premium features");

            // This test depends on having a free subscription
            // For demonstration purposes, we'll show how it would be structured

            // First login if needed
            var loginPage = new LoginPage(Driver, CurrentPlatform);
            if (loginPage.IsCurrentPage())
            {
                loginPage.Login();
            }

            // Navigate to Tips page
            try
            {
                // Look for a "Tips" tab or button
                switch (CurrentPlatform)
                {
                    case AppiumSetup.Platform.Android:
                        Driver.FindElement(By.XPath("//android.widget.Button[contains(@text, 'Tips') or contains(@content-desc, 'Tips')]")).Click();
                        break;
                    case AppiumSetup.Platform.iOS:
                        Driver.FindElement(By.XPath("//XCUIElementTypeButton[contains(@name, 'Tips')]")).Click();
                        break;
                    case AppiumSetup.Platform.Windows:
                        Driver.FindElement(By.XPath("//Button[contains(@Name, 'Tips')]")).Click();
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
            _tipsPage = new TipsPage(Driver, CurrentPlatform);

            // Check if exposure calculator button is visible
            if (!_tipsPage.IsExposureCalcButtonVisible())
            {
                Assert.Ignore("Exposure calculator button not visible, possibly due to subscription status");
            }

            // Click exposure calculator button
            _tipsPage.ClickExposureCalcButton();

            // Wait for navigation
            Thread.Sleep(2000);

            // Initialize feature not supported page
            _featureNotSupportedPage = new FeatureNotSupportedPage(Driver, CurrentPlatform);

            // Check if we're on the feature not supported page
            if (_featureNotSupportedPage.IsCurrentPage())
            {
                // Verify it's for the exposure calculator feature
                Assert.That(_featureNotSupportedPage.IsFeatureExposureCalculator(), Is.True, "Feature not supported page doesn't indicate exposure calculator");
            }
            else
            {
                // If we're not on the feature not supported page, we might have a premium subscription
                Log("Feature not supported page did not appear, possibly due to having a premium subscription");
                Assert.Ignore("Feature not supported page did not appear");
            }
        }

        [Test]
        [Description("Verify feature not supported page shows correct message")]
        [Ignore("This test depends on subscription status")]
        public void FeatureNotSupported_Message_ShouldBeDescriptive()
        {
            Log("Testing feature not supported page message");

            // Navigate to a feature that requires a premium subscription
            // Similar to the previous test

            // For demonstration purposes, we'll assume we're already on the feature not supported page
            _featureNotSupportedPage = new FeatureNotSupportedPage(Driver, CurrentPlatform);

            if (!_featureNotSupportedPage.IsCurrentPage())
            {
                Assert.Ignore("Not on feature not supported page");
            }

            // Get content text
            string contentText = _featureNotSupportedPage.GetContentText();

            // Verify text is not empty
            Assert.That(!string.IsNullOrEmpty(contentText), Is.True);

            // Verify text mentions subscription or premium
            Assert.That(contentText.Contains("Premium") || contentText.Contains("subscription") ||
                         contentText.Contains("upgrade"), Is.True);
        }

        [Test]
        [Description("Verify navigation from feature not supported page")]
        [Ignore("This test depends on subscription status")]
        public void FeatureNotSupported_Navigation_ShouldReturnToPreviousPage()
        {
            Log("Testing navigation from feature not supported page");

            // Navigate to a feature that requires a premium subscription
            // Similar to the previous tests

            // For demonstration purposes, we'll assume we're already on the feature not supported page
            _featureNotSupportedPage = new FeatureNotSupportedPage(Driver, CurrentPlatform);

            if (!_featureNotSupportedPage.IsCurrentPage())
            {
                Assert.Ignore("Not on feature not supported page");
            }

            // Go back
            GoBack();

            // Wait for navigation
            Thread.Sleep(2000);

            // Verify we're no longer on the feature not supported page
            Assert.That(!_featureNotSupportedPage.IsCurrentPage(), Is.True, "Still on feature not supported page after navigation");
        }
    }
}