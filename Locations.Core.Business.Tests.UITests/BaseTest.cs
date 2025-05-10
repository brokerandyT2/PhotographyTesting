// Locations.Core.Business.Tests.UITests/BaseTest.cs
using NUnit.Framework;
using System;
using System.Threading;

namespace Locations.Core.Business.Tests.UITests
{
    [TestFixture]
    public abstract class BaseTest : AppiumSetup
    {
        protected const string LogTag = "UITest";

        [SetUp]
        public virtual void SetUp()
        {
            // By default, use Android for testing
            // This can be overridden in specific test classes if needed
            InitializeDriver(Platform.Android);

            // Wait for app to fully load
            Thread.Sleep(5000);

            Console.WriteLine($"{LogTag}: Test setup complete");
        }

        [TearDown]
        public virtual void TearDown()
        {
            try
            {
                // Take screenshot on test failure
                if (TestContext.CurrentContext.Result.Outcome.Status == NUnit.Framework.Interfaces.TestStatus.Failed)
                {
                    string screenshotName = $"{TestContext.CurrentContext.Test.Name}_{DateTime.Now:yyyyMMdd_HHmmss}.png";
                    string screenshotPath = System.IO.Path.Combine(TestContext.CurrentContext.WorkDirectory, "Screenshots", screenshotName);

                    // Ensure directory exists
                    System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(screenshotPath));

                    // Take screenshot based on current platform
                    switch (CurrentPlatform)
                    {
                        case Platform.Windows:
                            WindowsDriver.GetScreenshot().SaveAsFile(screenshotPath);
                            break;
                        case Platform.Android:
                            AndroidDriver.GetScreenshot().SaveAsFile(screenshotPath);
                            break;
                        case Platform.iOS:
                            iOSDriver.GetScreenshot().SaveAsFile(screenshotPath);
                            break;
                    }

                    TestContext.AddTestAttachment(screenshotPath, "Screenshot on failure");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"{LogTag}: Failed to capture screenshot: {ex.Message}");
            }
            finally
            {
                CleanupDriver();
                Console.WriteLine($"{LogTag}: Test teardown complete");
            }
        }

        // Helper method to log events
        protected void Log(string message)
        {
            Console.WriteLine($"{LogTag}: {message}");
            TestContext.WriteLine($"{message}");
        }

        // Common UI interaction methods
        protected void ClickElement(string accessibilityId)
        {
            Log($"Clicking element with accessibility ID: {accessibilityId}");
            FindElementByAccessibilityId(accessibilityId).Click();
        }

        protected void EnterText(string accessibilityId, string text)
        {
            Log($"Entering text '{text}' into element with accessibility ID: {accessibilityId}");
            var element = FindElementByAccessibilityId(accessibilityId);
            element.Clear();
            element.SendKeys(text);
        }

        protected bool IsElementDisplayed(string accessibilityId, int timeoutSeconds = 5)
        {
            Log($"Checking if element with accessibility ID '{accessibilityId}' is displayed");
            try
            {
                WaitForElementPresent(() => FindElementByAccessibilityId(accessibilityId), timeoutSeconds);
                return true;
            }
            catch
            {
                return false;
            }
        }

        protected string GetElementText(string accessibilityId)
        {
            Log($"Getting text from element with accessibility ID: {accessibilityId}");
            return FindElementByAccessibilityId(accessibilityId).Text;
        }

        protected void ToggleSwitch(string accessibilityId, bool toggleOn)
        {
            Log($"Setting switch with accessibility ID '{accessibilityId}' to {(toggleOn ? "ON" : "OFF")}");
            var switchElement = FindElementByAccessibilityId(accessibilityId);
            bool isOn = false;

            // Handle platform differences in getting switch state
            switch (CurrentPlatform)
            {
                case Platform.Android:
                    isOn = switchElement.GetAttribute("checked") == "true";
                    break;
                case Platform.iOS:
                    isOn = switchElement.GetAttribute("value") == "1";
                    break;
                case Platform.Windows:
                    isOn = switchElement.GetAttribute("Toggle.ToggleState") == "1";
                    break;
            }

            // Only click if the current state doesn't match the desired state
            if (isOn != toggleOn)
            {
                switchElement.Click();
            }
        }

        // Navigation helper methods can be added as needed
        protected void GoBack()
        {
            Log("Navigating back");
            switch (CurrentPlatform)
            {
                case Platform.Android:
                    AndroidDriver.Navigate().Back();
                    break;
                case Platform.iOS:
                    // iOS typically uses a back button in the UI
                    try
                    {
                        FindElementByXPath("//XCUIElementTypeButton[@name='Back']").Click();
                    }
                    catch
                    {
                        Log("Back button not found, trying to navigate back through other means");
                    }
                    break;
                case Platform.Windows:
                    try
                    {
                        FindElementByXPath("//Button[@Name='Back']").Click();
                    }
                    catch
                    {
                        Log("Back button not found, trying to navigate back through other means");
                    }
                    break;
            }
        }
    }
}