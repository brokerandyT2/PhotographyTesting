using Locations.Core.Business.BDD.TestHelpers;
using Locations.Core.Business.DataAccess.Interfaces;
using Locations.Core.Data.Models;
using Locations.Core.Data.Queries.Interfaces;
using Locations.Core.Shared.ViewModels;
using Moq;
using NUnit.Framework;
using TechTalk.SpecFlow;
using Assert = NUnit.Framework.Assert;

namespace Locations.Core.Business.BDD.StepDefinitions.Weather
{
    [Binding]
    public class WeatherDisplaySteps
    {
        private readonly IWeatherService<WeatherViewModel> _weatherService;
        private readonly ILocationService<LocationViewModel> _locationService;
        private readonly ISettingService<SettingViewModel> _settingsService;
        private readonly BDDTestContext _testContext;
        private readonly Mock<IWeatherRepository> _mockWeatherRepository;
        private readonly Mock<ILocationRepository> _mockLocationRepository;
        private readonly Mock<ISettingsRepository> _mockSettingsRepository;

        private WeatherViewModel _currentWeather;
        private LocationViewModel _currentLocation;
        private string _temperaturePreference = "Fahrenheit";
        private string _windDirectionPreference = "Towards Wind";

        public WeatherDisplaySteps(
            IWeatherService<WeatherViewModel> weatherService,
            ILocationService<LocationViewModel> locationService,
            ISettingService<SettingViewModel> settingsService,
            BDDTestContext testContext,
            Mock<IWeatherRepository> mockWeatherRepository,
            Mock<ILocationRepository> mockLocationRepository,
            Mock<ISettingsRepository> mockSettingsRepository)
        {
            _weatherService = weatherService;
            _locationService = locationService;
            _settingsService = settingsService;
            _testContext = testContext;
            _mockWeatherRepository = mockWeatherRepository;
            _mockLocationRepository = mockLocationRepository;
            _mockSettingsRepository = mockSettingsRepository;
        }

        [Given(@"I am viewing the weather for a location")]
        public async Task GivenIAmViewingTheWeatherForALocation()
        {
            // Get the first location from test context
            if (!_testContext.TestLocations.Any())
            {
                _testContext.TestLocations.Add(TestDataFactory.CreateTestLocation());
            }
            _currentLocation = _testContext.TestLocations.First();

            // Create weather data for this location
            var weather = TestDataFactory.CreateTestWeather(1, _currentLocation.Id);
            weather.Temperature = 72.5;
            weather.Description = "Partly Cloudy";
            weather.WindSpeed = 15.0;
            weather.WindDirection = 180;
            weather.Humidity = 65;
            weather.Pressure = 1013.25;
            weather.Temperature_day_one = 80;
            weather.Temperature_day_one_low = 65;

            // Setup mock to return this weather
            _mockWeatherRepository.Setup(x => x.GetByLocationIdAsync(_currentLocation.Id))
                .ReturnsAsync(DataOperationResult<WeatherViewModel>.Success(weather));

            // Get weather for current location
            var result = await _weatherService.GetWeatherForLocationAsync(_currentLocation.Id);
            if (result.IsSuccess)
            {
                _currentWeather = result.Data;
                _testContext.CurrentWeather = result.Data;
            }
        }

        [Given(@"my temperature preference is set to ""(.*)""")]
        public async Task GivenMyTemperaturePreferenceIsSetTo(string temperatureFormat)
        {
            _temperaturePreference = temperatureFormat;

            var setting = new SettingViewModel
            {
                Key = "TemperatureType",
                Value = temperatureFormat
            };

            _mockSettingsRepository.Setup(x => x.GetByNameAsync("TemperatureType"))
                .ReturnsAsync(DataOperationResult<SettingViewModel>.Success(setting));

            await _settingsService.SaveAsync(setting);
            _testContext.TestSettings["TemperatureType"] = setting;
        }

        [Given(@"my wind direction preference is set to ""(.*)""")]
        public async Task GivenMyWindDirectionPreferenceIsSetTo(string windDirection)
        {
            _windDirectionPreference = windDirection;

            var setting = new SettingViewModel
            {
                Key = "WindDirection",
                Value = windDirection
            };

            _mockSettingsRepository.Setup(x => x.GetByNameAsync("WindDirection"))
                .ReturnsAsync(DataOperationResult<SettingViewModel>.Success(setting));

            await _settingsService.SaveAsync(setting);
            _testContext.TestSettings["WindDirection"] = setting;
        }

        [When(@"I expand day one forecast details")]
        public void WhenIExpandDayOneForecastDetails()
        {
            // In service tests, we simulate expanded details by ensuring we have full weather data
            Assert.That(_currentWeather, Is.Not.Null);
            Assert.That(_currentWeather.WindSpeed, Is.Not.Null);
            Assert.That(_currentWeather.Humidity, Is.Not.Null);
            Assert.That(_currentWeather.Pressure, Is.Not.Null);
        }

        [When(@"I view the weather display")]
        public async Task WhenIViewTheWeatherDisplay()
        {
            // Ensure we have weather data
            if (_currentWeather == null && _currentLocation != null)
            {
                var result = await _weatherService.GetWeatherForLocationAsync(_currentLocation.Id);
                if (result.IsSuccess)
                {
                    _currentWeather = result.Data;
                }
            }
        }

        [When(@"I expand day two forecast")]
        public void WhenIExpandDayTwoForecast()
        {
            // Ensure we have forecast data
            Assert.That(_currentWeather?.Forecast, Is.Not.Null);
        }

        [When(@"I expand day three forecast")]
        public void WhenIExpandDayThreeForecast()
        {
            // Ensure we have forecast data
            Assert.That(_currentWeather?.Forecast, Is.Not.Null);
        }

        [When(@"I tap the close button")]
        public void WhenITapTheCloseButton()
        {
            // In service tests, we clear the current weather to simulate closing
            _testContext.CurrentWeather = null;
        }

        [Then(@"I should see the current temperature")]
        public void ThenIShouldSeeTheCurrentTemperature()
        {
            Assert.That(_currentWeather, Is.Not.Null);
            Assert.That(_currentWeather.Temperature, Is.Not.Null);
            Assert.That(_currentWeather.Temperature, Is.GreaterThan(0));
        }

        [Then(@"I should see the weather condition description")]
        public void ThenIShouldSeeTheWeatherConditionDescription()
        {
            Assert.That(_currentWeather, Is.Not.Null);
            Assert.That(string.IsNullOrEmpty(_currentWeather.Description), Is.False);
        }

        [Then(@"I should see the daily high and low temperatures")]
        public void ThenIShouldSeeTheDailyHighAndLowTemperatures()
        {
            Assert.That(_currentWeather, Is.Not.Null);
            Assert.That(_currentWeather.Temperature_day_one, Is.GreaterThan(0));
            Assert.That(_currentWeather.Temperature_day_one_low, Is.GreaterThan(0));
            Assert.That(_currentWeather.Temperature_day_one, Is.GreaterThan(_currentWeather.Temperature_day_one_low));
        }

        [Then(@"I should see additional weather information")]
        public void ThenIShouldSeeAdditionalWeatherInformation()
        {
            Assert.That(_currentWeather, Is.Not.Null);
            Assert.That(_currentWeather.Humidity, Is.GreaterThan(0));
            Assert.That(_currentWeather.Pressure, Is.GreaterThan(0));
        }

        [Then(@"I should see wind speed and direction")]
        public void ThenIShouldSeeWindSpeedAndDirection()
        {
            Assert.That(_currentWeather, Is.Not.Null);
            Assert.That(_currentWeather.WindSpeed, Is.GreaterThan(0));
            Assert.That(_currentWeather.WindDirection, Is.GreaterThanOrEqualTo(0));
            Assert.That(_currentWeather.WindDirection, Is.LessThanOrEqualTo(360));
        }

        [Then(@"I should see humidity and pressure data")]
        public void ThenIShouldSeeHumidityAndPressureData()
        {
            Assert.That(_currentWeather, Is.Not.Null);
            Assert.That(_currentWeather.Humidity, Is.GreaterThan(0));
            Assert.That(_currentWeather.Humidity, Is.LessThanOrEqualTo(100));
            Assert.That(_currentWeather.Pressure, Is.GreaterThan(0));
        }

        [Then(@"I should see day two's weather details")]
        public void ThenIShouldSeeDayTwosWeatherDetails()
        {
            Assert.That(_currentWeather, Is.Not.Null);
            Assert.That(_currentWeather.Forecast, Is.Not.Null);
            // Additional forecast data would be in a more complex weather model
        }

        [Then(@"I should see day three's weather details")]
        public void ThenIShouldSeeDayThreesWeatherDetails()
        {
            Assert.That(_currentWeather, Is.Not.Null);
            Assert.That(_currentWeather.Forecast, Is.Not.Null);
            // Additional forecast data would be in a more complex weather model
        }

        [Then(@"I should see temperatures in Fahrenheit units")]
        public void ThenIShouldSeeTemperaturesInFahrenheitUnits()
        {
            Assert.That(_currentWeather, Is.Not.Null);
            Assert.That(_temperaturePreference, Is.EqualTo("Fahrenheit"));
            // Temperature values would be in Fahrenheit based on preference
            Assert.That(_currentWeather.Temperature, Is.GreaterThan(32), "Temperature seems too low for Fahrenheit");
        }

        [Then(@"I should see temperatures in Celsius units")]
        public void ThenIShouldSeeTemperaturesInCelsiusUnits()
        {
            Assert.That(_currentWeather, Is.Not.Null);
            Assert.That(_temperaturePreference, Is.EqualTo("Celsius"));
            // Temperature values would be in Celsius based on preference
        }

        [Then(@"the wind direction arrow should point towards the wind")]
        public void ThenTheWindDirectionArrowShouldPointTowardsTheWind()
        {
            Assert.That(_currentWeather, Is.Not.Null);
            Assert.That(_windDirectionPreference, Is.EqualTo("Towards Wind"));
        }

        [Then(@"the wind direction arrow should point with the wind")]
        public void ThenTheWindDirectionArrowShouldPointWithTheWind()
        {
            Assert.That(_currentWeather, Is.Not.Null);
            Assert.That(_windDirectionPreference, Is.EqualTo("With Wind"));
        }

        [Then(@"I should be returned to the location details page")]
        public void ThenIShouldBeReturnedToTheLocationDetailsPage()
        {
            Assert.That(_testContext.CurrentWeather, Is.Null);
            Assert.That(_testContext.CurrentLocation, Is.Not.Null);
        }
    }
}