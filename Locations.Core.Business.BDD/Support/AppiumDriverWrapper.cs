// Locations.Core.Business.BDD.Support/AppiumDriverWrapper.cs
using Locations.Core.Business.Tests.UITests;

namespace Locations.Core.Business.BDD.Support
{
    public class AppiumDriverWrapper
    {
        public OpenQA.Selenium.Appium.AppiumDriver Driver { get; }
        public AppiumSetup.Platform Platform { get; }

        public AppiumDriverWrapper(OpenQA.Selenium.Appium.AppiumDriver driver, AppiumSetup.Platform platform)
        {
            Driver = driver;
            Platform = platform;
        }
    }
}