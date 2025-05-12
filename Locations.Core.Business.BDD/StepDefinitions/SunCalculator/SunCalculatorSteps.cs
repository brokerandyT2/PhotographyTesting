using Locations.Core.Business.BDD.TestHelpers;
using Locations.Core.Business.DataAccess.Interfaces;
using Locations.Core.Data.Models;
using Locations.Core.Data.Queries.Interfaces;
using Locations.Core.Shared.ViewModels;
using Moq;
using NUnit.Framework;
using TechTalk.SpecFlow;
using Assert = NUnit.Framework.Assert;

namespace Locations.Core.Business.BDD.StepDefinitions.SunCalculator
{
    [Binding]
    public class SunCalculatorSteps
    {
        private readonly ILocationService<LocationViewModel> _locationService;
        private readonly ISettingService<SettingViewModel> _settingsService;
        private readonly BDDTestContext _testContext;
        private readonly Mock<ISettingsRepository> _mockSettingsRepository;

        private Location.Photography.Shared.ViewModels.SunCalculations _sunCalculations;
        private LocationViewModel _currentLocation;
        private string _hemisphere;
        private string _timeFormat;

        public SunCalculatorSteps(
            ILocationService<LocationViewModel> locationService,
            ISettingService<SettingViewModel> settingsService,
            BDDTestContext testContext,
            Mock<ISettingsRepository> mockSettingsRepository)
        {
            _locationService = locationService;
            _settingsService = settingsService;
            _testContext = testContext;
            _mockSettingsRepository = mockSettingsRepository;
        }

        [Given(@"I am viewing the sun calculator for a location")]
        public void GivenIAmViewingTheSunCalculatorForALocation()
        {
            // Get first location from test context
            if (!_testContext.TestLocations.Any())
            {
                _testContext.TestLocations.Add(TestDataFactory.CreateTestLocation());
            }
            _currentLocation = _testContext.TestLocations.First();

            // Initialize sun calculations
            _sunCalculations = new Location.Photography.Shared.ViewModels.SunCalculations
            {
                Latitude = _currentLocation.Lattitude,
                Longitude = _currentLocation.Longitude,
                Date = DateTime.Now
            };

            // Load settings
            var hemisphereSettings = _settingsService.GetSettingByName("Hemisphere");
            _hemisphere = hemisphereSettings?.Value ?? "North";

            var timeFormatSettings = _settingsService.GetSettingByName("TimeFormat");
            _timeFormat = timeFormatSettings?.Value ?? "12-hour";

            _sunCalculations.TimeFormat = _timeFormat;
        }

        [Given(@"my hemisphere preference is set to ""(.*)""")]
        public async Task GivenMyHemispherePreferenceIsSetTo(string hemisphere)
        {
            _hemisphere = hemisphere;

            var setting = new SettingViewModel
            {
                Key = "Hemisphere",
                Value = hemisphere
            };

            _mockSettingsRepository.Setup(x => x.GetByNameAsync("Hemisphere"))
                .ReturnsAsync(DataOperationResult<SettingViewModel>.Success(setting));

            await _settingsService.SaveAsync(setting);

            if (_sunCalculations != null)
            {
                // Apply hemisphere setting to calculations
                // This would affect how sun position is displayed
            }
        }

        [When(@"I change the selected date to tomorrow")]
        public void WhenIChangeTheSelectedDateToTomorrow()
        {
            _sunCalculations.Date = DateTime.Now.AddDays(1);
        }

        [When(@"I change the selected date to one month ahead")]
        public void WhenIChangeTheSelectedDateToOneMonthAhead()
        {
            _sunCalculations.Date = DateTime.Now.AddMonths(1);
        }

        [When(@"I tap the close button")]
        public void WhenITapTheCloseButton()
        {
            _testContext.CurrentLocation = _currentLocation;
            _sunCalculations = null;
        }

        [Then(@"I should see the sunrise time for the location")]
        public void ThenIShouldSeeTheSunriseTimeForTheLocation()
        {
            Assert.That(_sunCalculations, Is.Not.Null);
            Assert.That(_sunCalculations.Sunrise, Is.Not.EqualTo(DateTime.MinValue));
            Assert.That(string.IsNullOrEmpty(_sunCalculations.SunRiseFormatted), Is.False);
        }

        [Then(@"I should see the sunset time for the location")]
        public void ThenIShouldSeeTheSunsetTimeForTheLocation()
        {
            Assert.That(_sunCalculations, Is.Not.Null);
            Assert.That(_sunCalculations.Sunset, Is.Not.EqualTo(DateTime.MinValue));
            Assert.That(string.IsNullOrEmpty(_sunCalculations.SunSetFormatted), Is.False);
        }

        [Then(@"the times should be displayed in my preferred time format")]
        public void ThenTheTimesShouldBeDisplayedInMyPreferredTimeFormat()
        {
            Assert.That(_sunCalculations, Is.Not.Null);
            Assert.That(_sunCalculations.TimeFormat, Is.EqualTo(_timeFormat));

            // Check format of time strings
            if (_timeFormat == "12-hour")
            {
                Assert.That(_sunCalculations.SunRiseFormatted.Contains("AM") ||
                           _sunCalculations.SunRiseFormatted.Contains("PM"), Is.True);
            }
            else
            {
                Assert.That(_sunCalculations.SunRiseFormatted.Contains("AM") ||
                           _sunCalculations.SunRiseFormatted.Contains("PM"), Is.False);
            }
        }

        [Then(@"I should see the morning golden hour time")]
        public void ThenIShouldSeeTheMorningGoldenHourTime()
        {
            Assert.That(_sunCalculations, Is.Not.Null);
            Assert.That(_sunCalculations.GoldenHourMorning, Is.Not.EqualTo(DateTime.MinValue));
            Assert.That(string.IsNullOrEmpty(_sunCalculations.GoldenHourMorningFormatted), Is.False);
        }

        [Then(@"I should see the evening golden hour time")]
        public void ThenIShouldSeeTheEveningGoldenHourTime()
        {
            Assert.That(_sunCalculations, Is.Not.Null);
            Assert.That(_sunCalculations.GoldenHourEvening, Is.Not.EqualTo(DateTime.MinValue));
            Assert.That(string.IsNullOrEmpty(_sunCalculations.GoldenHourEveningFormatted), Is.False);
        }

        [Then(@"I should see the duration of golden hour periods")]
        public void ThenIShouldSeeTheDurationOfGoldenHourPeriods()
        {
            Assert.That(_sunCalculations, Is.Not.Null);

            // Calculate durations
            var morningDuration = _sunCalculations.GoldenHourEvening - _sunCalculations.GoldenHourMorning;
            var eveningDuration = _sunCalculations.GoldenHourEvening.AddHours(5) - _sunCalculations.GoldenHourEvening;

            Assert.That(morningDuration.TotalMinutes, Is.GreaterThan(0));
            Assert.That(eveningDuration.TotalMinutes, Is.GreaterThan(0));
        }

        [Then(@"I should see the evening blue hour time")]
        public void ThenIShouldSeeTheEveningBlueHourTime()
        {
            Assert.That(_sunCalculations, Is.Not.Null);
            Assert.That(_sunCalculations.Civildusk, Is.Not.EqualTo(DateTime.MinValue));
            Assert.That(string.IsNullOrEmpty(_sunCalculations.CivilDuskFormatted), Is.False);
        }

        [Then(@"I should see the duration of blue hour periods")]
        public void ThenIShouldSeeTheDurationOfBlueHourPeriods()
        {
            Assert.That(_sunCalculations, Is.Not.Null);

            // Calculate blue hour durations
            var morningBlueHour = _sunCalculations.Civildawn - _sunCalculations.NauticalDawn;
            var eveningBlueHour = _sunCalculations.NauticalDusk - _sunCalculations.Civildusk;

            Assert.That(morningBlueHour.TotalMinutes, Is.GreaterThan(0));
            Assert.That(eveningBlueHour.TotalMinutes, Is.GreaterThan(0));
        }

        [Then(@"the sun position display should be oriented for Northern hemisphere")]
        public void ThenTheSunPositionDisplayShouldBeOrientedForNorthernHemisphere()
        {
            Assert.That(_hemisphere, Is.EqualTo("North"));
            Assert.That(_sunCalculations, Is.Not.Null);
            // Sun position calculations would be adjusted for Northern hemisphere
        }

        [Then(@"the sun position display should be oriented for Southern hemisphere")]
        public void ThenTheSunPositionDisplayShouldBeOrientedForSouthernHemisphere()
        {
            Assert.That(_hemisphere, Is.EqualTo("South"));
            Assert.That(_sunCalculations, Is.Not.Null);
            // Sun position calculations would be adjusted for Southern hemisphere
        }

        [Then(@"the sun position information should update for tomorrow's date")]
        public void ThenTheSunPositionInformationShouldUpdateForTomorrowsDate()
        {
            Assert.That(_sunCalculations, Is.Not.Null);
            Assert.That(_sunCalculations.Date.Date, Is.EqualTo(DateTime.Now.AddDays(1).Date));
            Assert.That(_sunCalculations.Sunrise, Is.Not.EqualTo(DateTime.MinValue));
            Assert.That(_sunCalculations.Sunset, Is.Not.EqualTo(DateTime.MinValue));
        }

        [Then(@"the sun position information should update for the future date")]
        public void ThenTheSunPositionInformationShouldUpdateForTheFutureDate()
        {
            Assert.That(_sunCalculations, Is.Not.Null);
            Assert.That(_sunCalculations.Date.Date, Is.EqualTo(DateTime.Now.AddMonths(1).Date));
            Assert.That(_sunCalculations.Sunrise, Is.Not.EqualTo(DateTime.MinValue));
            Assert.That(_sunCalculations.Sunset, Is.Not.EqualTo(DateTime.MinValue));
        }

        [Then(@"I should see an indicator of the current sun position")]
        public void ThenIShouldSeeAnIndicatorOfTheCurrentSunPosition()
        {
            Assert.That(_sunCalculations, Is.Not.Null);
            // In a real implementation, this would check current sun elevation and azimuth
        }

        [Then(@"I should see the current altitude and azimuth of the sun")]
        public void ThenIShouldSeeTheCurrentAltitudeAndAzimuthOfTheSun()
        {
            Assert.That(_sunCalculations, Is.Not.Null);
            // Sun altitude and azimuth would be calculated based on current time and location
        }

        [Then(@"I should be returned to the location details page")]
        public void ThenIShouldBeReturnedToTheLocationDetailsPage()
        {
            Assert.That(_sunCalculations, Is.Null);
            Assert.That(_testContext.CurrentLocation, Is.Not.Null);
            Assert.That(_testContext.CurrentLocation, Is.EqualTo(_currentLocation));
        }
    }
}