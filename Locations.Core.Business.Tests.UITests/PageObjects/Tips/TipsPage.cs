// Locations.Core.Business.Tests.UITests/PageObjects/Tips/TipsPage.cs
using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.iOS;
using System;
using OpenQA.Selenium.Appium;

namespace Locations.Core.Business.Tests.UITests.PageObjects.Tips
{
    /// <summary>
    /// Page object for the Tips view
    /// </summary>
    public class TipsPage : BasePage
    {
        // Element identifiers
        private const string TipTypePicker = "pick";
        private const string FStopLabel = "fStop";
        private const string ShutterSpeedLabel = "shutterSpeed";
        private const string IsoLabel = "iso";
        private const string TipTextLabel = "tiptext";
        private const string ExposureCalcButton = "exposurecalc";

        public TipsPage(AppiumDriver driver = null,
                    AppiumSetup.Platform platform = AppiumSetup.Platform.Android)
    : base(driver, platform)
        {
        }

        public override bool IsCurrentPage()
        {
            try
            {
                // Check for elements unique to the Tips page
                return IsElementDisplayed(TipTypePicker) && IsElementDisplayed(FStopLabel);
            }
            catch
            {
                return false;
            }
        }

        public void SelectTipType(int index)
        {
            try
            {
                switch (CurrentPlatform)
                {
                    case AppiumSetup.Platform.Android:
                        // On Android, clicking the picker usually opens a dropdown
                        ClickElement(TipTypePicker);
                        System.Threading.Thread.Sleep(1000); // Wait for dropdown to open

                        // Find dropdown items and click the one at the specified index
                        var items = AndroidDriver.FindElementsByXPath("//android.widget.ListView/android.widget.CheckedTextView");
                        if (index < items.Count)
                        {
                            items[index].Click();
                        }
                        break;

                    case AppiumSetup.Platform.iOS:
                        // On iOS, pickers work differently
                        var picker = FindElementByAccessibilityId(TipTypePicker);
                        // Set the picker value - this may require a custom approach depending on the iOS implementation
                        break;

                    case AppiumSetup.Platform.Windows:
                        // On Windows, clicking the picker usually opens a dropdown
                        ClickElement(TipTypePicker);
                        System.Threading.Thread.Sleep(1000); // Wait for dropdown to open

                        // Find dropdown items and click the one at the specified index
                        var winItems = WindowsDriver.FindElementsByXPath("//ListItem");
                        if (index < winItems.Count)
                        {
                            winItems[index].Click();
                        }
                        break;
                }

                // Wait for the tip content to update
                System.Threading.Thread.Sleep(1000);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to select tip type at index {index}: {ex.Message}");
            }
        }

        public string GetFStop()
        {
            return GetElementText(FStopLabel);
        }

        public string GetShutterSpeed()
        {
            return GetElementText(ShutterSpeedLabel);
        }

        public string GetISO()
        {
            return GetElementText(IsoLabel);
        }

        public string GetTipText()
        {
            return GetElementText(TipTextLabel);
        }

        public bool IsExposureCalcButtonVisible()
        {
            return IsElementDisplayed(ExposureCalcButton);
        }

        public void ClickExposureCalcButton()
        {
            if (IsExposureCalcButtonVisible())
            {
                ClickElement(ExposureCalcButton);
            }
        }

        // Helper method to check if tip content is loaded
        public bool HasTipContent()
        {
            try
            {
                bool hasFStop = !string.IsNullOrEmpty(GetFStop());
                bool hasShutterSpeed = !string.IsNullOrEmpty(GetShutterSpeed());
                bool hasISO = !string.IsNullOrEmpty(GetISO());
                bool hasTipText = !string.IsNullOrEmpty(GetTipText());

                return hasFStop || hasShutterSpeed || hasISO || hasTipText;
            }
            catch
            {
                return false;
            }
        }
    }
}