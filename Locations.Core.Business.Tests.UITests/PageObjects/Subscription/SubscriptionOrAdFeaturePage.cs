// Locations.Core.Business.Tests.UITests/PageObjects/Subscription/SubscriptionOrAdFeaturePage.cs
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.iOS;
using System;
using OpenQA.Selenium.Appium;

namespace Locations.Core.Business.Tests.UITests.PageObjects.Subscription
{
    /// <summary>
    /// Page object for the SubscriptionOrAdFeature view
    /// </summary>
    public class SubscriptionOrAdFeaturePage : BasePage
    {
        // Element identifiers
        private const string WelcomeLabel = "//Label[contains(@Text, 'Welcome')]";
        private const string SubscribeButton = "//Button[contains(@Text, 'Subscribe')]";
        private const string WatchAdButton = "//Button[contains(@Text, 'Watch Ad')]";
        private const string CancelButton = "//Button[contains(@Text, 'Cancel')]";

        public SubscriptionOrAdFeaturePage(AppiumDriver driver = null,
                    AppiumSetup.Platform platform = AppiumSetup.Platform.Android)
    : base(driver, platform)
        {
        }

        public override bool IsCurrentPage()
        {
            try
            {
                return IsElementByXPathDisplayed(WelcomeLabel);
            }
            catch
            {
                return false;
            }
        }

        private bool IsElementByXPathDisplayed(string xpath)
        {
            try
            {
                return FindElementByXPath(xpath).Displayed;
            }
            catch
            {
                return false;
            }
        }

        public void ClickSubscribe()
        {
            if (IsElementByXPathDisplayed(SubscribeButton))
            {
                FindElementByXPath(SubscribeButton).Click();
            }
        }

        public void ClickWatchAd()
        {
            if (IsElementByXPathDisplayed(WatchAdButton))
            {
                FindElementByXPath(WatchAdButton).Click();
            }
        }

        public void ClickCancel()
        {
            if (IsElementByXPathDisplayed(CancelButton))
            {
                FindElementByXPath(CancelButton).Click();
            }
            else
            {
                // Try system back if no cancel button
                switch (CurrentPlatform)
                {
                    case AppiumSetup.Platform.Android:
                        AndroidDriver.Navigate().Back();
                        break;
                    case AppiumSetup.Platform.iOS:
                        // On iOS, back navigation may vary
                        break;
                    case AppiumSetup.Platform.Windows:
                        // On Windows, back navigation may vary
                        break;
                }
            }
        }

        public void WaitForDismissal(int timeoutSeconds = 10)
        {
            DateTime endTime = DateTime.Now.AddSeconds(timeoutSeconds);
            while (DateTime.Now < endTime)
            {
                if (!IsCurrentPage())
                    return;
                System.Threading.Thread.Sleep(500);
            }

            // If still on subscription page after timeout, try to dismiss it
            if (IsCurrentPage())
            {
                ClickCancel();
            }
        }
    }
}