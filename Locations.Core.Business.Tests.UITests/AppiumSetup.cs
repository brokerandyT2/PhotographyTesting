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
using AppiumDriver = OpenQA.Selenium.Appium.AppiumDriver;

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

            switch (platform)
            {
                case Platform.Windows:
                    options.PlatformName = "Windows";
                    options.AutomationName = "Windows";
                    options.DeviceName = appConfig.Windows.DeviceName;
                    options.App = GetWindowsAppPath();
                    break;

                case Platform.Android:
                    options.PlatformName = "Android";
                    options.AutomationName = appConfig.Android.AutomationName;
                    options.DeviceName = appConfig.Android.DeviceName;
                    options.PlatformVersion = appConfig.Android.PlatformVersion;

                    // Check if we should use an APK file or an installed app
                    if (!string.IsNullOrEmpty(appConfig.Android.AppPath) && File.Exists(appConfig.Android.AppPath))
                    {
                        options.App = appConfig.Android.AppPath;
                    }
                    else
                    {
                        // Use installed app package and activity
                        options.AddAdditionalAppiumOption("appPackage", appConfig.Android.AppPackage);
                        options.AddAdditionalAppiumOption("appActivity", appConfig.Android.AppActivity);
                    }

                    // Add additional capabilities for Android
                    options.AddAdditionalAppiumOption("noReset", appConfig.Android.NoReset);
                    options.AddAdditionalAppiumOption("fullReset", appConfig.Android.FullReset);

                    // Add any additional capabilities from config
                    if (appConfig.Android.AdditionalCapabilities != null)
                    {
                        foreach (var cap in appConfig.Android.AdditionalCapabilities)
                        {
                            // Don't add capabilities that already have dedicated properties
                            if (!IsReservedCapability(cap.Key))
                            {
                                options.AddAdditionalAppiumOption(cap.Key, cap.Value);
                            }
                        }
                    }
                    break;

                case Platform.iOS:
                    options.PlatformName = "iOS";
                    options.AutomationName = appConfig.iOS.AutomationName;
                    options.DeviceName = appConfig.iOS.DeviceName;
                    options.PlatformVersion = appConfig.iOS.PlatformVersion;

                    // Check if we should use an app file or an installed app
                    if (!string.IsNullOrEmpty(appConfig.iOS.AppPath) && File.Exists(appConfig.iOS.AppPath))
                    {
                        options.App = appConfig.iOS.AppPath;
                    }
                    else
                    {
                        // Use installed app bundle ID
                        options.AddAdditionalAppiumOption("bundleId", appConfig.iOS.BundleId);
                    }

                    // Add additional capabilities for iOS
                    options.AddAdditionalAppiumOption("noReset", appConfig.iOS.NoReset);
                    options.AddAdditionalAppiumOption("fullReset", appConfig.iOS.FullReset);

                    // Add any additional capabilities from config
                    if (appConfig.iOS.AdditionalCapabilities != null)
                    {
                        foreach (var cap in appConfig.iOS.AdditionalCapabilities)
                        {
                            // Don't add capabilities that already have dedicated properties
                            if (!IsReservedCapability(cap.Key))
                            {
                                options.AddAdditionalAppiumOption(cap.Key, cap.Value);
                            }
                        }
                    }
                    break;
            }

            return options;
        }

        // Check if a capability name is reserved (has a dedicated property)
        private bool IsReservedCapability(string capabilityName)
        {
            var reservedCapabilities = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "platformName",
                "automationName",
                "platformVersion",
                "deviceName",
                "app",
                "browserName",
                "newCommandTimeout",
                "language",
                "locale",
                "udid",
                "orientation",
                "autoWebview",
                "noReset",
                "fullReset",
                "eventTimings",
                "enablePerformanceLogging",
                "printPageSourceOnFindFailure"
            };

            return reservedCapabilities.Contains(capabilityName);
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
        // Updated StartAppiumServer method
        private void StartAppiumServer()
        {
            var appConfig = LoadAppiumConfig();

            if (appConfig.AppiumServer.AutoStart)
            {
                try
                {
                    TestContext.Progress.WriteLine("Starting local Appium server...");

                    // First, check if Appium is already running
                    if (IsAppiumServerRunning(appConfig.AppiumServer.Host, appConfig.AppiumServer.Port))
                    {
                        TestContext.Progress.WriteLine("Appium server is already running");
                        return;
                    }

                    var startInfo = new ProcessStartInfo
                    {
                        FileName = appConfig.AppiumServer.ExecutablePath,
                        Arguments = $"--port {appConfig.AppiumServer.Port}",
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        CreateNoWindow = true
                    };

                    _appiumServerProcess = Process.Start(startInfo);
                    _isLocalAppiumServer = true;

                    // Wait for the server to start properly
                    int retries = 0;
                    while (retries < 30) // Wait up to 30 seconds
                    {
                        if (IsAppiumServerRunning(appConfig.AppiumServer.Host, appConfig.AppiumServer.Port))
                        {
                            TestContext.Progress.WriteLine("Appium server started successfully");
                            return;
                        }
                        Thread.Sleep(1000);
                        retries++;
                    }

                    throw new Exception("Appium server failed to start within timeout period");
                }
                catch (Exception ex)
                {
                    TestContext.Progress.WriteLine($"Failed to start Appium server: {ex.Message}");
                    throw;
                }
            }
        }

        // Add this helper method to check if Appium is running
        private bool IsAppiumServerRunning(string host, int port)
        {
            try
            {
                using (var client = new System.Net.Http.HttpClient())
                {
                    client.Timeout = TimeSpan.FromSeconds(1);
                    var response = client.GetAsync($"http://{host}:{port}/status").Result;
                    return response.IsSuccessStatusCode;
                }
            }
            catch
            {
                return false;
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

            TestContext.Progress.WriteLine($"Connecting to Appium server at {serverUri}");

            try
            {
                switch (platform)
                {
                    case Platform.Android:
                        Driver = new AndroidDriver(new Uri(serverUri), options);
                        break;
                    case Platform.iOS:
                        Driver = new IOSDriver(new Uri(serverUri), options);
                        break;
                    default:
                        Driver = new WindowsDriver(new Uri(serverUri), options);
                        break;
                }

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