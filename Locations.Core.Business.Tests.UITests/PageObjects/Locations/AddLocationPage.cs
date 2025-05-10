// Locations.Core.Business.Tests.UITests/PageObjects/Locations/AddLocationPage.cs
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using System;

namespace Locations.Core.Business.Tests.UITests.PageObjects.Locations
{
    /// <summary>
    /// Page object for the AddLocation view
    /// </summary>
    public class AddLocationPage : BasePage
    {
        // Element identifiers
        private const string AddPhotoButton = "AddPhoto";
        private const string TitleField = "//Entry[@Text='Title']";
        private const string LatitudeLabel = "Latitude";
        private const string LongitudeLabel = "Longitude";
        private const string DescriptionEditor = "//Editor";
        private const string SaveButton = "Save";
        private const string CloseModalButton = "CloseModal";
        private const string BusyIndicator = "//ActivityIndicator";
        private const string ErrorMessageLabel = "//Label[contains(@Text, 'Error')]";

        public AddLocationPage(AppiumDriver driver = null,
                              AppiumSetup.Platform platform = AppiumSetup.Platform.Android)
            : base(driver, platform)
        {
        }

        public override bool IsCurrentPage()
        {
            try
            {
                return IsElementDisplayed(AddPhotoButton) && IsElementDisplayed(SaveButton);
            }
            catch
            {
                return false;
            }
        }

        public void TakePhoto()
        {
            ClickElement(AddPhotoButton);
        }

        public void EnterTitle(string title)
        {
            // Using XPath since this is a generic Entry with a placeholder/label
            var titleElement = FindElementByXPath(TitleField);
            titleElement.Clear();
            titleElement.SendKeys(title);
        }

        public void EnterDescription(string description)
        {
            // Using XPath since this is a generic Editor
            var descriptionElement = FindElementByXPath(DescriptionEditor);
            descriptionElement.Clear();
            descriptionElement.SendKeys(description);
        }

        public string GetLatitude()
        {
            return GetElementText(LatitudeLabel);
        }

        public string GetLongitude()
        {
            return GetElementText(LongitudeLabel);
        }

        public void ClickSave()
        {
            ClickElement(SaveButton);
        }

        public void ClickCloseModal()
        {
            if (IsElementDisplayed(CloseModalButton))
            {
                ClickElement(CloseModalButton);
            }
        }

        public bool IsBusy()
        {
            try
            {
                return FindElementByXPath(BusyIndicator).Displayed;
            }
            catch
            {
                return false;
            }
        }

        public bool HasError()
        {
            try
            {
                return FindElementByXPath(ErrorMessageLabel).Displayed;
            }
            catch
            {
                return false;
            }
        }

        public string GetErrorMessage()
        {
            try
            {
                return FindElementByXPath(ErrorMessageLabel).Text;
            }
            catch
            {
                return string.Empty;
            }
        }

        public bool WaitForSaveToComplete(int timeoutSeconds = 15)
        {
            DateTime endTime = DateTime.Now.AddSeconds(timeoutSeconds);
            while (DateTime.Now < endTime)
            {
                if (!IsBusy())
                    return true;
                System.Threading.Thread.Sleep(500);
            }
            return false; // Still busy after timeout
        }

        // Create a new location with the given details
        public void CreateLocation(string title, string description = "")
        {
            EnterTitle(title);
            if (!string.IsNullOrEmpty(description))
            {
                EnterDescription(description);
            }
            ClickSave();
            WaitForSaveToComplete();
        }
    }
}