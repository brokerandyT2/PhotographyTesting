// Locations.Core.Business.Tests.UITests/PageObjects/Locations/EditLocationPage.cs
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.iOS;
using System;
using OpenQA.Selenium.Appium;

namespace Locations.Core.Business.Tests.UITests.PageObjects.Locations
{
    /// <summary>
    /// Page object for the EditLocation view
    /// </summary>
    public class EditLocationPage : BasePage
    {
        // Element identifiers
        private const string WeatherButton = "WeatherButton";
        private const string SunEventsButton = "SunEvents";
        private const string TitleField = "//Entry";
        private const string LatitudeLabel = "Latitude";
        private const string LongitudeLabel = "Longitude";
        private const string DescriptionEditor = "//Editor";
        private const string SaveButton = "Save";
        private const string CloseModalButton = "CloseModal";
        private const string CloseButton = "//ImageButton";

        public EditLocationPage(AppiumDriver driver = null,
                    AppiumSetup.Platform platform = AppiumSetup.Platform.Android)
    : base(driver, platform)
        {
        }

        public override bool IsCurrentPage()
        {
            try
            {
                return IsElementDisplayed(WeatherButton) && IsElementDisplayed(SunEventsButton);
            }
            catch
            {
                return false;
            }
        }

        public void ClickWeatherButton()
        {
            ClickElement(WeatherButton);
        }

        public void ClickSunEventsButton()
        {
            ClickElement(SunEventsButton);
        }

        public void EnterTitle(string title)
        {
            var titleElement = FindElementByXPath(TitleField);
            titleElement.Clear();
            titleElement.SendKeys(title);
        }

        public void EnterDescription(string description)
        {
            var descriptionElement = FindElementByXPath(DescriptionEditor);
            descriptionElement.Clear();
            descriptionElement.SendKeys(description);
        }

        public string GetTitle()
        {
            return FindElementByXPath(TitleField).Text;
        }

        public string GetLatitude()
        {
            return GetElementText(LatitudeLabel);
        }

        public string GetLongitude()
        {
            return GetElementText(LongitudeLabel);
        }

        public string GetDescription()
        {
            return FindElementByXPath(DescriptionEditor).Text;
        }

        public void ClickSave()
        {
            ClickElement(SaveButton);
        }

        public void ClickClose()
        {
            try
            {
                FindElementByXPath(CloseButton).Click();
            }
            catch
            {
                // Try alternative close method
                if (IsElementDisplayed(CloseModalButton))
                {
                    ClickElement(CloseModalButton);
                }
            }
        }

        // Update the location with new details
        public void UpdateLocation(string newTitle = null, string newDescription = null)
        {
            if (!string.IsNullOrEmpty(newTitle))
            {
                EnterTitle(newTitle);
            }

            if (!string.IsNullOrEmpty(newDescription))
            {
                EnterDescription(newDescription);
            }

            ClickSave();
            // Allow time for the save operation to complete
            System.Threading.Thread.Sleep(2000);
        }
    }
}