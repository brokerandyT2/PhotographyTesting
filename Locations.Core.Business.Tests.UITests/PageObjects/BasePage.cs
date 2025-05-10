// Locations.Core.Business.Tests.UITests/PageObjects/BasePage.cs
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using System;

namespace Locations.Core.Business.Tests.UITests.PageObjects
{
    /// <summary>
    /// Base class for all page objects
    /// </summary>
    public abstract class BasePage
    {
        // Single driver reference for all platforms
        protected AppiumDriver Driver;
        protected AppiumSetup.Platform CurrentPlatform;

        // Constructor to initialize the appropriate driver
        protected BasePage(AppiumDriver driver = null,
                          AppiumSetup.Platform platform = AppiumSetup.Platform.Android)
        {
            Driver = driver;
            CurrentPlatform = platform;
        }

        // Updated helper methods using standard FindElement with AppiumBy
        protected IWebElement FindElementByAccessibilityId(string id)
        {
            return Driver.FindElement(By.Id(id));
        }

        protected IWebElement FindElementById(string id)
        {
            return Driver.FindElement(By.Id(id));
        }

        protected IWebElement FindElementByXPath(string xpath)
        {
            return Driver.FindElement(By.XPath(xpath));
        }

        // Common UI interaction methods
        protected void ClickElement(string accessibilityId)
        {
            FindElementByAccessibilityId(accessibilityId).Click();
        }

        protected void EnterText(string accessibilityId, string text)
        {
            var element = FindElementByAccessibilityId(accessibilityId);
            element.Clear();
            element.SendKeys(text);
        }

        protected bool IsElementDisplayed(string accessibilityId)
        {
            try
            {
                return FindElementByAccessibilityId(accessibilityId).Displayed;
            }
            catch
            {
                return false;
            }
        }

        protected string GetElementText(string accessibilityId)
        {
            return FindElementByAccessibilityId(accessibilityId).Text;
        }

        protected void ToggleSwitch(string accessibilityId, bool toggleOn)
        {
            var switchElement = FindElementByAccessibilityId(accessibilityId);
            bool isOn = false;

            // Handle platform differences in getting switch state
            switch (CurrentPlatform)
            {
                case AppiumSetup.Platform.Android:
                    isOn = switchElement.GetAttribute("checked") == "true";
                    break;
                case AppiumSetup.Platform.iOS:
                    isOn = switchElement.GetAttribute("value") == "1";
                    break;
                case AppiumSetup.Platform.Windows:
                    isOn = switchElement.GetAttribute("Toggle.ToggleState") == "1";
                    break;
            }

            // Only click if the current state doesn't match the desired state
            if (isOn != toggleOn)
            {
                switchElement.Click();
            }
        }

        // Wait for an element to be present
        protected void WaitForElementPresent(Func<IWebElement> elementFinder, int timeoutSeconds = 10)
        {
            DateTime endTime = DateTime.Now.AddSeconds(timeoutSeconds);
            while (DateTime.Now < endTime)
            {
                try
                {
                    var element = elementFinder();
                    if (element != null && element.Displayed)
                        return;
                }
                catch (WebDriverException)
                {
                    // Element not found yet, continue waiting
                }
                System.Threading.Thread.Sleep(500);
            }
            throw new TimeoutException($"Element not found after waiting {timeoutSeconds} seconds");
        }

        // Each page should implement a method to verify that it's the current page
        public abstract bool IsCurrentPage();
    }
}