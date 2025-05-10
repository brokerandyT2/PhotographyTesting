// Locations.Core.Business.BDD.Support/AppiumDriverManager.cs
using System;
using System.Threading;
using Locations.Core.Business.Tests.UITests;

namespace Locations.Core.Business.BDD.Support
{
    public static class AppiumDriverManager
    {
        private static readonly ThreadLocal<OpenQA.Selenium.Appium.AppiumDriver> _driver =
            new ThreadLocal<OpenQA.Selenium.Appium.AppiumDriver>();

        private static readonly ThreadLocal<AppiumSetup.Platform> _platform =
            new ThreadLocal<AppiumSetup.Platform>();

        public static OpenQA.Selenium.Appium.AppiumDriver Driver
        {
            get
            {
                if (_driver.Value == null)
                {
                    throw new InvalidOperationException("AppiumDriver has not been initialized. Call Initialize() first.");
                }
                return _driver.Value;
            }
        }

        public static AppiumSetup.Platform CurrentPlatform
        {
            get
            {
                if (!_platform.IsValueCreated)
                {
                    _platform.Value = AppiumSetup.Platform.Android; // Default to Android
                }
                return _platform.Value;
            }
        }

        public static void Initialize(OpenQA.Selenium.Appium.AppiumDriver driver, AppiumSetup.Platform platform)
        {
            _driver.Value = driver;
            _platform.Value = platform;
        }

        public static void Cleanup()
        {
            // No need to cleanup the driver here since BaseTest will handle it
            _driver.Value = null;
        }
    }
}