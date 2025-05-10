// Locations.Core.Business.Tests.UITests/PageObjects/Authentication/LoginPage.cs
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.iOS;
using System;
using OpenQA.Selenium.Appium;

namespace Locations.Core.Business.Tests.UITests.PageObjects.Authentication
{
    /// <summary>
    /// Page object for the Login view
    /// </summary>
    public class LoginPage : BasePage
    {
        // Element identifiers (accessibility IDs, IDs, or XPaths)
        private const string EmailAddressField = "emailAddress";
        private const string HemisphereSwitch = "HemisphereSwitch";
        private const string TimeSwitch = "TimeSwitch";
        private const string DateFormatSwitch = "DateFormat";
        private const string WindDirectionSwitch = "WindDirectionSwitch";
        private const string TempFormatSwitch = "TempFormatSwitch";
        private const string SaveButton = "save";
        private const string EmailValidationMessage = "emailValidationMessage";
        private const string ProcessingOverlay = "processingOverlay";
        private const string HemisphereLabel = "//Label[contains(@Text, 'Hemisphere')]";
        private const string WindDirectionLabel = "WindDirection";

        public LoginPage(AppiumDriver driver = null,
                    AppiumSetup.Platform platform = AppiumSetup.Platform.Android)
    : base(driver, platform)
        {
        }

        public override bool IsCurrentPage()
        {
            try
            {
                return IsElementDisplayed(EmailAddressField) && IsElementDisplayed(SaveButton);
            }
            catch
            {
                return false;
            }
        }

        public void EnterEmail(string email)
        {
            EnterText(EmailAddressField, email);
        }

        public void SetHemisphere(bool north)
        {
            ToggleSwitch(HemisphereSwitch, north);
        }

        public void SetTimeFormat(bool useUSFormat)
        {
            ToggleSwitch(TimeSwitch, useUSFormat);
        }

        public void SetDateFormat(bool useUSFormat)
        {
            ToggleSwitch(DateFormatSwitch, useUSFormat);
        }

        public void SetWindDirection(bool towardsWind)
        {
            ToggleSwitch(WindDirectionSwitch, towardsWind);
        }

        public void SetTemperatureFormat(bool useFahrenheit)
        {
            ToggleSwitch(TempFormatSwitch, useFahrenheit);
        }

        public void ClickSave()
        {
            ClickElement(SaveButton);
        }

        public bool IsEmailValidationDisplayed()
        {
            return IsElementDisplayed(EmailValidationMessage);
        }

        public bool IsProcessing()
        {
            return IsElementDisplayed(ProcessingOverlay);
        }

        public bool WaitForProcessingToComplete(int timeoutSeconds = 15)
        {
            DateTime endTime = DateTime.Now.AddSeconds(timeoutSeconds);
            while (DateTime.Now < endTime)
            {
                if (!IsProcessing())
                    return true;
                System.Threading.Thread.Sleep(500);
            }
            return false; // Processing overlay still visible after timeout
        }

        // Login with specified settings or defaults
        public void Login(string email = "test@example.com",
                          bool north = true,
                          bool useUSTimeFormat = true,
                          bool useUSDateFormat = true,
                          bool towardsWind = true,
                          bool useFahrenheit = true)
        {
            EnterEmail(email);
            SetHemisphere(north);
            SetTimeFormat(useUSTimeFormat);
            SetDateFormat(useUSDateFormat);
            SetWindDirection(towardsWind);
            SetTemperatureFormat(useFahrenheit);
            ClickSave();
            WaitForProcessingToComplete();
        }
    }
}