// Locations.Core.Business.Tests.UITests/PageObjects/Shared/PageTutorialModalPage.cs
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.iOS;
using System;
using OpenQA.Selenium.Appium;

namespace Locations.Core.Business.Tests.UITests.PageObjects.Shared
{
    /// <summary>
    /// Page object for the PageTutorialModal view
    /// </summary>
    public class PageTutorialModalPage : BasePage
    {
        // Element identifiers
        private const string ContentLabel = "content";
        private const string WebViewTutorial = "tutorial";
        private const string BackButton = "//Button[contains(@Text, 'Back')]";

        public PageTutorialModalPage(AppiumDriver driver = null,
                    AppiumSetup.Platform platform = AppiumSetup.Platform.Android)
    : base(driver, platform)
        {
        }

        public override bool IsCurrentPage()
        {
            try
            {
                // Check for elements unique to the PageTutorialModal
                return IsElementDisplayed(WebViewTutorial);
            }
            catch
            {
                return false;
            }
        }

        public string GetContentText()
        {
            try
            {
                return GetElementText(ContentLabel);
            }
            catch
            {
                return string.Empty;
            }
        }

        public bool HasWebViewContent()
        {
            return IsElementDisplayed(WebViewTutorial);
        }

        public void ClickBack()
        {
            try
            {
                FindElementByXPath(BackButton).Click();
            }
            catch
            {
                // No explicit back button, try system back
                switch (CurrentPlatform)
                {
                    case AppiumSetup.Platform.Android:
                        Driver.Navigate().Back();
                        break;
                    case AppiumSetup.Platform.iOS:
                        // On iOS, might need to look for a different back mechanism
                        break;
                    case AppiumSetup.Platform.Windows:
                        // On Windows, might need to look for a different back mechanism
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

            // If still on tutorial page after timeout, try to dismiss it
            if (IsCurrentPage())
            {
                ClickBack();
            }
        }
    }
}