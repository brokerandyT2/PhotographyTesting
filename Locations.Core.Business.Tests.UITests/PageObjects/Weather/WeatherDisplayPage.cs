// Locations.Core.Business.Tests.UITests/PageObjects/Weather/WeatherDisplayPage.cs
using OpenQA.Selenium;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Appium.Android;
using OpenQA.Selenium.Appium.iOS;
using System;
using OpenQA.Selenium.Appium;

namespace Locations.Core.Business.Tests.UITests.PageObjects.Weather
{
    /// <summary>
    /// Page object for the WeatherDisplay view
    /// </summary>
    public class WeatherDisplayPage : BasePage
    {
        // Element identifiers
        private const string CloseButton = "//ImageButton";
        private const string DayOneLabel = "//Label[@Text='DayOne']";
        private const string DayOneForecast = "Forecast_Day_One";
        private const string DayOneLowTemp = "Temperature_Day_One_Min";
        private const string DayOneHighTemp = "Temperature_Day_One_Max";
        private const string DayOneIcon = "Weather_Day_One_Icon";
        private const string DetailsExpander = "//maui:Expander";
        private const string WindDirectionImage = "//Image[@Source='arrow_up_custom.png']";
        private const string WindSpeedLabel = "WindSpeedDay_One";
        private const string WindGustLabel = "WindGustDay_One";
        private const string SunriseLabel = "Sunrise_Day_One_String";
        private const string SunsetLabel = "Sunset_Day_One_String";
        private const string DayTwoExpander = "//maui:Expander[@HeaderContent=\"Grid\"][contains(., 'DayTwo')]";
        private const string DayThreeExpander = "//maui:Expander[@HeaderContent=\"Grid\"][contains(., 'DayThree')]";

        public WeatherDisplayPage(AppiumDriver driver = null,
                    AppiumSetup.Platform platform = AppiumSetup.Platform.Android)
    : base(driver, platform)
        {
        }

        public override bool IsCurrentPage()
        {
            try
            {
                // Check for elements unique to the WeatherDisplay page
                return IsElementByXPathDisplayed(CloseButton) && IsElementByXPathDisplayed(DayOneLabel);
            }
            catch
            {
                return false;
            }
        }

        private bool IsElementByXPathDisplayed(string xpath)
        {
            try
            {
                return FindElementByXPath(xpath).Displayed;
            }
            catch
            {
                return false;
            }
        }

        public void ClickClose()
        {
            FindElementByXPath(CloseButton).Click();
        }

        public string GetDayOneForecast()
        {
            return GetElementText(DayOneForecast);
        }

        public string GetDayOneLowTemperature()
        {
            return GetElementText(DayOneLowTemp);
        }

        public string GetDayOneHighTemperature()
        {
            return GetElementText(DayOneHighTemp);
        }

        public void ExpandDayOneDetails()
        {
            try
            {
                // The first expander should be for day one details
                var expanderElement = FindElementByXPath(DetailsExpander);
                if (!IsDetailsExpanded(expanderElement))
                {
                    expanderElement.Click();
                    System.Threading.Thread.Sleep(500); // Wait for animation
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to expand day one details: {ex.Message}");
            }
        }

        private bool IsDetailsExpanded(IWebElement expander)
        {
            // This will depend on how the expander's expanded state is represented
            // You might need to adjust this based on your application's implementation
            try
            {
                // Check if any nested content (that would only be visible when expanded) is displayed
                var nestedContent = expander.FindElement(By.XPath(".//Grid"));
                return nestedContent.Displayed;
            }
            catch
            {
                return false;
            }
        }

        public double GetWindDirection()
        {
            try
            {
                ExpandDayOneDetails();
                var windDirectionImage = FindElementByXPath(WindDirectionImage);
                string rotationAttribute = windDirectionImage.GetAttribute("Rotation");

                if (double.TryParse(rotationAttribute, out double rotation))
                {
                    return rotation;
                }

                return 0.0;
            }
            catch
            {
                return 0.0;
            }
        }

        public string GetWindSpeed()
        {
            try
            {
                ExpandDayOneDetails();
                return GetElementText(WindSpeedLabel);
            }
            catch
            {
                return string.Empty;
            }
        }

        public string GetWindGust()
        {
            try
            {
                ExpandDayOneDetails();
                return GetElementText(WindGustLabel);
            }
            catch
            {
                return string.Empty;
            }
        }

        public string GetSunrise()
        {
            try
            {
                ExpandDayOneDetails();
                return GetElementText(SunriseLabel);
            }
            catch
            {
                return string.Empty;
            }
        }

        public string GetSunset()
        {
            try
            {
                ExpandDayOneDetails();
                return GetElementText(SunsetLabel);
            }
            catch
            {
                return string.Empty;
            }
        }

        public void ExpandDayTwo()
        {
            try
            {
                var expanderElement = FindElementByXPath(DayTwoExpander);
                expanderElement.Click();
                System.Threading.Thread.Sleep(500); // Wait for animation
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to expand day two: {ex.Message}");
            }
        }

        public void ExpandDayThree()
        {
            try
            {
                var expanderElement = FindElementByXPath(DayThreeExpander);
                expanderElement.Click();
                System.Threading.Thread.Sleep(500); // Wait for animation
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to expand day three: {ex.Message}");
            }
        }

        // Helper method to check if the weather data is displayed correctly
        public bool HasWeatherData()
        {
            try
            {
                bool hasForecast = !string.IsNullOrEmpty(GetDayOneForecast());
                bool hasTemperatures = !string.IsNullOrEmpty(GetDayOneLowTemperature()) &&
                                      !string.IsNullOrEmpty(GetDayOneHighTemperature());

                return hasForecast && hasTemperatures;
            }
            catch
            {
                return false;
            }
        }
    }
}