// Locations.Core.Business.Tests.UITests/PageObjects/Configuration/SettingsPage.cs
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.iOS;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;

namespace Locations.Core.Business.Tests.UITests.PageObjects.Configuration
{
    /// <summary>
    /// Page object for the Settings view
    /// </summary>
    public class SettingsPage : BasePage
    {
        // Element identifiers
        private const string HemisphereSwitch = "HemisphereSwitch";
        private const string TimeSwitch = "TimeSwitch";
        private const string DateFormatSwitch = "DateFormat";
        private const string WindDirectionSwitch = "WindDirectionSwitch";
        private const string TempFormatSwitch = "TempFormatSwitch";
        private const string AdSupportSwitch = "adsupport";
        private const string HemisphereLabel = "//Label[contains(@Text, 'Hemisphere')]";
        private const string WindDirectionLabel = "WindDirection";
        private const string SubscriptionTypeLabel = "//Label[contains(@Text, 'Subscription Type')]";
        private const string SubscriptionExpirationLabel = "//Label[contains(@Text, 'Subscription Expiration')]";

        public SettingsPage(AppiumDriver driver = null,
                    AppiumSetup.Platform platform = AppiumSetup.Platform.Android)
    : base(driver, platform)
        {
        }

        public override bool IsCurrentPage()
        {
            try
            {
                // Check for elements unique to the Settings page
                return IsElementDisplayed(HemisphereSwitch) && IsElementDisplayed(AdSupportSwitch);
            }
            catch
            {
                return false;
            }
        }

        public void ToggleHemisphere(bool north)
        {
            ToggleSwitch(HemisphereSwitch, north);
        }

        public void ToggleTimeFormat(bool useUSFormat)
        {
            ToggleSwitch(TimeSwitch, useUSFormat);
        }

        public void ToggleDateFormat(bool useUSFormat)
        {
            ToggleSwitch(DateFormatSwitch, useUSFormat);
        }

        public void ToggleWindDirection(bool towardsWind)
        {
            ToggleSwitch(WindDirectionSwitch, towardsWind);
        }

        public void ToggleTemperatureFormat(bool useFahrenheit)
        {
            ToggleSwitch(TempFormatSwitch, useFahrenheit);
        }

        public void ToggleAdSupport(bool enabled)
        {
            ToggleSwitch(AdSupportSwitch, enabled);
        }

        public string GetWindDirectionText()
        {
            return GetElementText(WindDirectionLabel);
        }

        public string GetHemisphereText()
        {
            try
            {
                return FindElementByXPath(HemisphereLabel).Text;
            }
            catch
            {
                return string.Empty;
            }
        }

        public string GetSubscriptionTypeText()
        {
            try
            {
                var element = FindElementByXPath(SubscriptionTypeLabel).FindElement(By.XPath("following-sibling::Label"));
                return element.Text;
            }
            catch
            {
                return string.Empty;
            }
        }

        public string GetSubscriptionExpirationText()
        {
            try
            {
                var element = FindElementByXPath(SubscriptionExpirationLabel).FindElement(By.XPath("following-sibling::Label"));
                return element.Text;
            }
            catch
            {
                return string.Empty;
            }
        }

        // Verify if settings were saved correctly
        public bool VerifySettingsApplied(bool north, bool useUSTimeFormat, bool useUSDateFormat,
                                       bool towardsWind, bool useFahrenheit, bool adSupport)
        {
            try
            {
                bool settingsMatch = true;

                // Check if the switch states match expected values
                settingsMatch &= GetSwitchState(HemisphereSwitch) == north;
                settingsMatch &= GetSwitchState(TimeSwitch) == useUSTimeFormat;
                settingsMatch &= GetSwitchState(DateFormatSwitch) == useUSDateFormat;
                settingsMatch &= GetSwitchState(WindDirectionSwitch) == towardsWind;
                settingsMatch &= GetSwitchState(TempFormatSwitch) == useFahrenheit;
                settingsMatch &= GetSwitchState(AdSupportSwitch) == adSupport;

                return settingsMatch;
            }
            catch
            {
                return false;
            }
        }

        private bool GetSwitchState(string accessibilityId)
        {
            var element = FindElementByAccessibilityId(accessibilityId);
            switch (CurrentPlatform)
            {
                case AppiumSetup.Platform.Android:
                    return element.GetAttribute("checked") == "true";
                case AppiumSetup.Platform.iOS:
                    return element.GetAttribute("value") == "1";
                case AppiumSetup.Platform.Windows:
                    return element.GetAttribute("Toggle.ToggleState") == "1";
                default:
                    return false;
            }
        }
    }
}