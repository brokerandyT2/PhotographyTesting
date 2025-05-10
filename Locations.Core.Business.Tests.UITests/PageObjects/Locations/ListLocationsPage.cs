// Locations.Core.Business.Tests.UITests/PageObjects/Locations/ListLocationsPage.cs
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Locations.Core.Business.Tests.UITests.PageObjects.Locations
{
    /// <summary>
    /// Page object for the ListLocations view
    /// </summary>
    public class ListLocationsPage : BasePage
    {
        // Element identifiers
        private const string CollectionView = "cv";
        private const string LoadingIndicator = "//ActivityIndicator";
        private const string ErrorMessageLabel = "//Label[contains(@Text, 'Error')]";
        private const string LocationItemTemplate = "//DataTemplate";
        private const string MapButton = "//ImageButton[@Source='map.svg']";

        public ListLocationsPage(AppiumDriver driver = null,
                    AppiumSetup.Platform platform = AppiumSetup.Platform.Android)
            : base(driver, platform)
        {
        }

        public override bool IsCurrentPage()
        {
            try
            {
                return IsElementDisplayed(CollectionView) || IsElementDisplayed(LoadingIndicator);
            }
            catch
            {
                return false;
            }
        }
        public bool WaitForLocationsToLoad(int timeoutSeconds = 15)
        {
            try
            {
                // Wait for either locations to appear or loading to finish
                DateTime endTime = DateTime.Now.AddSeconds(timeoutSeconds);
                while (DateTime.Now < endTime)
                {
                    // Check if still loading
                    if (IsLoading())
                    {
                        Thread.Sleep(500);
                        continue;
                    }

                    // Check if error is displayed
                    if (HasError())
                    {
                        return false;
                    }

                    // If not loading and no error, check if we have locations
                    // or at least the collection view is visible
                    if (IsElementDisplayed(CollectionView))
                    {
                        return true;
                    }

                    Thread.Sleep(500);
                }

                // Timeout reached
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception in WaitForLocationsToLoad: {ex.Message}");
                return false;
            }
        }
        public bool IsLoading()
        {
            try
            {
                return FindElementByXPath(LoadingIndicator).Displayed;
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

        public IReadOnlyList<string> GetLocationTitles()
        {
            try
            {
                List<string> titles = new List<string>();
                IReadOnlyCollection<IWebElement> elements;

                // Approach will vary by platform
                switch (CurrentPlatform)
                {
                    case AppiumSetup.Platform.Android:
                        elements = Driver.FindElements(By.XPath("//android.widget.TextView"));
                        break;

                    case AppiumSetup.Platform.iOS:
                        elements = Driver.FindElements(By.XPath("//XCUIElementTypeStaticText"));
                        break;

                    case AppiumSetup.Platform.Windows:
                        elements = Driver.FindElements(By.XPath("//Text"));
                        break;

                    default:
                        return new List<string>();
                }

                foreach (var element in elements)
                {
                    titles.Add(element.Text);
                }

                return titles;
            }
            catch
            {
                return new List<string>();
            }
        }

        public bool HasLocations()
        {
            return GetLocationTitles().Count > 0;
        }

        public void SelectLocation(int index)
        {
            try
            {
                IReadOnlyCollection<IWebElement> elements;

                // The approach will vary by platform
                switch (CurrentPlatform)
                {
                    case AppiumSetup.Platform.Android:
                        elements = Driver.FindElements(By.XPath("//android.widget.TextView"));
                        break;

                    case AppiumSetup.Platform.iOS:
                        elements = Driver.FindElements(By.XPath("//XCUIElementTypeStaticText"));
                        break;

                    case AppiumSetup.Platform.Windows:
                        elements = Driver.FindElements(By.XPath("//Text"));
                        break;

                    default:
                        return;
                }

                var elementsList = elements.ToList();
                if (index < elementsList.Count)
                {
                    elementsList[index].Click();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to select location at index {index}: {ex.Message}");
            }
        }

        public void SelectLocation(string title)
        {
            try
            {
                // The approach will vary by platform
                switch (CurrentPlatform)
                {
                    case AppiumSetup.Platform.Android:
                        Driver.FindElement(By.XPath($"//android.widget.TextView[contains(@text, '{title}')]")).Click();
                        break;

                    case AppiumSetup.Platform.iOS:
                        Driver.FindElement(By.XPath($"//XCUIElementTypeStaticText[contains(@name, '{title}')]")).Click();
                        break;

                    case AppiumSetup.Platform.Windows:
                        Driver.FindElement(By.XPath($"//Text[contains(@Name, '{title}')]")).Click();
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to select location with title '{title}': {ex.Message}");
            }
        }
    }
}