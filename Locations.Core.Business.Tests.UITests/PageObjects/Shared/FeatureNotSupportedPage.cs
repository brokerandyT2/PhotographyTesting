// Locations.Core.Business.Tests.UITests/PageObjects/Shared/FeatureNotSupportedPage.cs
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.iOS;
using System;
using OpenQA.Selenium.Appium;

namespace Locations.Core.Business.Tests.UITests.PageObjects.Shared
{
    /// <summary>
    /// Page object for the FeatureNotSupported view
    /// </summary>
    public class FeatureNotSupportedPage : BasePage
    {
        // Element identifiers
        private const string ContentLabel = "content";

        public FeatureNotSupportedPage(AppiumDriver driver = null,
                    AppiumSetup.Platform platform = AppiumSetup.Platform.Android)
    : base(driver, platform)
        {
        }

        public override bool IsCurrentPage()
        {
            try
            {
                return IsElementDisplayed(ContentLabel);
            }
            catch
            {
                return false;
            }
        }

        public string GetContentText()
        {
            return GetElementText(ContentLabel);
        }

        public bool IsFeatureExposureCalculator()
        {
            string content = GetContentText();
            return content.Contains("Exposure") && content.Contains("Premium");
        }
    }
}