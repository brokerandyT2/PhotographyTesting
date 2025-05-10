// Locations.Core.Business.Tests.UITests/AppiumSetup.cs
using NUnit.Framework;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Enums;
using System.IO;
using System.Diagnostics;
using Newtonsoft.Json;
using System.Reflection;
using System.Collections.Generic;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.iOS;
using OpenQA.Selenium.Appium.Windows;

namespace Locations.Core.Business.Tests.UITests
{
    public class AppiumSetup
    {
        // Single driver for all platforms
        protected AppiumDriver Driver;

        // Current platform being tested
        protected Platform CurrentPlatform;

        // Appium server process
        private Process _appiumServerProcess;
        private bool _isLocalAppiumServer = false;

        // Enum to represent the platform being tested
        public enum Platform
        {
            Windows,
            Android,
            iOS
        }

        // Base capabilities for all platforms
        protected AppiumOptions GetBaseOptions(Platform platform)
        {
            var options = new AppiumOptions();
            var appConfig = LoadAppiumConfig();

            // Common capabilities for Appium
            Dictionary<string, object> appiumOptions = new Dictionary<string, object>();

            switch (platform)
            {
                case Platform.Windows:
                    options.PlatformName = "Windows";
                    appiumOptions.Add("app", GetWindowsAppPath());
                    appiumOptions.Add("deviceName", appConfig.Windows.DeviceName);
                    break;

                case Platform.Android:
                    options.PlatformName = "Android";
                    appiumOptions.Add("deviceName", appConfig.Android.DeviceName);
                    appiumOptions.Add("automationName", appConfig.Android.AutomationName);

                    // Check if we should use an APK file or an installed app
                    if (!string.IsNullOrEmpty(appConfig.Android.AppPath) && File.Exists(appConfig.Android.AppPath))
                    {
                        appiumOptions.Add("app", appConfig.Android.AppPath);
                    }
                    else
                    {
                        // Use installed app package and activity
                        appiumOptions.Add("appPackage", appConfig.Android.AppPackage);
                        appiumOptions.Add("appActivity", appConfig.Android.AppActivity);
                    }

                    // Add additional capabilities for Android
                    appiumOptions.Add("platformVersion", appConfig.Android.PlatformVersion);
                    appiumOptions.Add("noReset", appConfig.Android.NoReset);
                    appiumOptions.Add("fullReset", appConfig.Android.FullReset);

                    if (appConfig.Android.AdditionalCapabilities != null)
                    {
                        foreach (var cap in appConfig.Android.AdditionalCapabilities)
                        {
                            appiumOptions.Add(cap.Key, cap.Value);
                        }
                    }
                    break;

                case Platform.iOS:
                    options.PlatformName = "iOS";
                    appiumOptions.Add("deviceName", appConfig.iOS.DeviceName);
                    appiumOptions.Add("automationName", appConfig.iOS.AutomationName);

                    // Check if we should use an app file or an installed app
                    if (!string.IsNullOrEmpty(appConfig.iOS.AppPath) && File.Exists(appConfig.iOS.AppPath))
                    {
                        appiumOptions.Add("app", appConfig.iOS.AppPath);
                    }
                    else
                    {
                        // Use installed app bundle ID
                        appiumOptions.Add("bundleId", appConfig.iOS.BundleId);
                    }

                    // Add additional capabilities for iOS
                    appiumOptions.Add("platformVersion", appConfig.iOS.PlatformVersion);
                    appiumOptions.Add("noReset", appConfig.iOS.NoReset);
                    appiumOptions.Add("fullReset", appConfig.iOS.FullReset);

                    if (appConfig.iOS.AdditionalCapabilities != null)
                    {
                        foreach (var cap in appConfig.iOS.AdditionalCapabilities)
                        {
                            appiumOptions.Add(cap.Key, cap.Value);
                        }
                    }
                    break;
            }

            // Add the appium options to the main options object
            options.AddAdditionalOption("appium:options", appiumOptions);

            return options;
        }

        // Helper methods to get app paths
        private string GetWindowsAppPath()
        {
            var appConfig = LoadAppiumConfig();

            if (!string.IsNullOrEmpty(appConfig.Windows.AppPath) && File.Exists(appConfig.Windows.AppPath))
            {
                return appConfig.Windows.AppPath;
            }

            // Default fallback
            return Path.Combine(TestContext.CurrentContext.TestDirectory,
                "../../../../Location.Core/bin/Debug/net9.0-windows10.0.19041.0/win10-x64/AppPackages/Location.Core.msix");
        }

        private string GetAndroidAppPath()
        {
            var appConfig = LoadAppiumConfig();

            if (!string.IsNullOrEmpty(appConfig.Android.AppPath) && File.Exists(appConfig.Android.AppPath))
            {
                return appConfig.Android.AppPath;
            }

            // Default fallback
            return Path.Combine(TestContext.CurrentContext.TestDirectory,
                "../../../../Location.Core/bin/Debug/net9.0-android/com.x3squaredcircles.locations-Signed.apk");
        }

        private string GetiOSAppPath()
        {
            var appConfig = LoadAppiumConfig();

            if (!string.IsNullOrEmpty(appConfig.iOS.AppPath) && File.Exists(appConfig.iOS.AppPath))
            {
                return appConfig.iOS.AppPath;
            }

            // Default fallback
            return Path.Combine(TestContext.CurrentContext.TestDirectory,
                "../../../../Location.Core/bin/Debug/net9.0-ios/iossimulator-x64/Location.Core.app");
        }

        // Start Appium server if needed
        private void StartAppiumServer()
        {
            var appConfig = LoadAppiumConfig();

            if (appConfig.AppiumServer.AutoStart)
            {
                try
                {
                    TestContext.Progress.WriteLine("Starting local Appium server...");

                    var startInfo = new ProcessStartInfo
                    {
                        FileName = appConfig.AppiumServer.ExecutablePath,
                        Arguments = $"--address {appConfig.AppiumServer.Host} --port {appConfig.AppiumServer.Port}",
                        UseShellExecute = true,
                        CreateNoWindow = false
                    };

                    _appiumServerProcess = Process.Start(startInfo);
                    _isLocalAppiumServer = true;

                    // Give the server time to start
                    System.Threading.Thread.Sleep(5000);

                    TestContext.Progress.WriteLine("Appium server started");
                }
                catch (Exception ex)
                {
                    TestContext.Progress.WriteLine($"Failed to start Appium server: {ex.Message}");
                    throw;
                }
            }
        }

        // Stop Appium server if we started it
        private void StopAppiumServer()
        {
            if (_isLocalAppiumServer && _appiumServerProcess != null && !_appiumServerProcess.HasExited)
            {
                try
                {
                    TestContext.Progress.WriteLine("Stopping Appium server...");
                    _appiumServerProcess.Kill(true);
                    _appiumServerProcess = null;
                    TestContext.Progress.WriteLine("Appium server stopped");
                }
                catch (Exception ex)
                {
                    TestContext.Progress.WriteLine($"Failed to stop Appium server: {ex.Message}");
                }
            }
        }

        // Initialize the appropriate driver
        protected void InitializeDriver(Platform platform)
        {
            var appConfig = LoadAppiumConfig();
            CurrentPlatform = platform;
            var options = GetBaseOptions(platform);

            // Start Appium server if needed
            StartAppiumServer();

            // Build the Appium server URI
            string serverUri = $"http://{appConfig.AppiumServer.Host}:{appConfig.AppiumServer.Port}";
            if (!serverUri.EndsWith("/wd/hub") && platform != Platform.Windows)
            {
                serverUri += "/wd/hub";
            }

            TestContext.Progress.WriteLine($"Connecting to Appium server at {serverUri}");

            try
            {
                switch (platform)
                {
                    case Platform.Android:
                        // For Android
                        Driver = new AndroidDriver(new Uri(serverUri), options);
                        break;
                        case Platform.iOS:
                        // For iOS
                        Driver = new IOSDriver(new Uri(serverUri), options);
                        break;
                    default:
                        // For Windows
                        Driver = new WindowsDriver(new Uri(serverUri), options);
                        break;
                }
                // Create a single AppiumDriver for all platforms

                Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(appConfig.DefaultTimeoutSeconds);

                TestContext.Progress.WriteLine("Connected to Appium server successfully");
            }
            catch (Exception ex)
            {
                TestContext.Progress.WriteLine($"Failed to initialize driver: {ex.Message}");
                StopAppiumServer();
                throw;
            }
        }

        // Clean up drivers
        protected void CleanupDriver()
        {
            Driver?.Quit();
            Driver = null;

            // Stop Appium server if we started it
            StopAppiumServer();
        }

        // Helper method to locate element by accessibility ID
        protected IWebElement FindElementByAccessibilityId(string id)
        {
            return Driver.FindElement(MobileBy.AccessibilityId(id));
        }

        // Helper method to find element by ID
        protected IWebElement FindElementById(string id)
        {
            return Driver.FindElement(By.Id(id));
        }

        // Helper method to find element by XPath
        protected IWebElement FindElementByXPath(string xpath)
        {
            return Driver.FindElement(By.XPath(xpath));
        }

        // Wait for an element to be present
        protected void WaitForElementPresent(Func<IWebElement> elementFinder, int timeoutSeconds = 10)
        {
            var appConfig = LoadAppiumConfig();
            timeoutSeconds = timeoutSeconds > 0 ? timeoutSeconds : appConfig.DefaultTimeoutSeconds;

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

        // Load configuration from JSON file
        private AppiumConfig LoadAppiumConfig()
        {
            string configPath = Path.Combine(GetExecutingDirectory(), "appium.config.json");

            if (File.Exists(configPath))
            {
                string json = File.ReadAllText(configPath);
                return JsonConvert.DeserializeObject<AppiumConfig>(json);
            }

            // Return default config if file not found
            return new AppiumConfig
            {
                DefaultTimeoutSeconds = 10,
                LongTimeoutSeconds = 30,
                AppiumServer = new AppiumServerConfig
                {
                    Host = "127.0.0.1",
                    Port = 4723,
                    AutoStart = false,
                    ExecutablePath = "appium"
                },
                Android = new AndroidConfig
                {
                    DeviceName = "Android Emulator",
                    AutomationName = "UiAutomator2",
                    PlatformVersion = "13.0",
                    AppPackage = "com.x3squaredcircles.locations",
                    AppActivity = "com.x3squaredcircles.locations.MainActivity",
                    NoReset = true,
                    FullReset = false
                },
                iOS = new iOSConfig
                {
                    DeviceName = "iPhone Simulator",
                    AutomationName = "XCUITest",
                    PlatformVersion = "15.0",
                    BundleId = "com.x3squaredcircles.locations",
                    NoReset = true,
                    FullReset = false
                },
                Windows = new WindowsConfig
                {
                    DeviceName = "WindowsPC"
                }
            };
        }

        // Get the directory of the executing assembly
        private string GetExecutingDirectory()
        {
            var assembly = Assembly.GetExecutingAssembly();
            string codeBase = assembly.Location;
            return Path.GetDirectoryName(codeBase);
        }
    }

    // Configuration classes for JSON deserialization
    public class AppiumConfig
    {
        public int DefaultTimeoutSeconds { get; set; } = 10;
        public int LongTimeoutSeconds { get; set; } = 30;
        public AppiumServerConfig AppiumServer { get; set; } = new AppiumServerConfig();
        public AndroidConfig Android { get; set; } = new AndroidConfig();
        public iOSConfig iOS { get; set; } = new iOSConfig();
        public WindowsConfig Windows { get; set; } = new WindowsConfig();
    }

    public class AppiumServerConfig
    {
        public string Host { get; set; } = "127.0.0.1";
        public int Port { get; set; } = 4723;
        public bool AutoStart { get; set; } = false;
        public string ExecutablePath { get; set; } = "appium";
    }

    public class AndroidConfig
    {
        public string DeviceName { get; set; } = "Android Emulator";
        public string AutomationName { get; set; } = "UiAutomator2";
        public string PlatformVersion { get; set; } = "13.0";
        public string AppPath { get; set; }
        public string AppPackage { get; set; } = "com.x3squaredcircles.locations";
        public string AppActivity { get; set; } = "com.x3squaredcircles.locations.MainActivity";
        public bool NoReset { get; set; } = true;
        public bool FullReset { get; set; } = false;
        public Dictionary<string, object> AdditionalCapabilities { get; set; }
    }

    public class iOSConfig
    {
        public string DeviceName { get; set; } = "iPhone Simulator";
        public string AutomationName { get; set; } = "XCUITest";
        public string PlatformVersion { get; set; } = "15.0";
        public string AppPath { get; set; }
        public string BundleId { get; set; }
        public bool NoReset { get; set; } = true;
        public bool FullReset { get; set; } = false;
        public Dictionary<string, object> AdditionalCapabilities { get; set; }
    }

    public class WindowsConfig
    {
        public string DeviceName { get; set; } = "WindowsPC";
        public string AppPath { get; set; }
    }
}