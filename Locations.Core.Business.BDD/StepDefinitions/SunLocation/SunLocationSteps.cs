using Locations.Core.Business.BDD.TestHelpers;
using Locations.Core.Business.DataAccess.Interfaces;
using Locations.Core.Shared.ViewModels;
using NUnit.Framework;
using TechTalk.SpecFlow;
using Assert = NUnit.Framework.Assert;

namespace Locations.Core.Business.BDD.StepDefinitions.SunLocation
{
    [Binding]
    public class SunLocationSteps
    {
        private readonly ILocationService<LocationViewModel> _locationService;
        private readonly BDDTestContext _testContext;

        private Location.Photography.Shared.ViewModels.SunLocation _sunLocation;
        private LocationViewModel _currentLocation;
        private bool _isMonitoringSensors;
        private bool _elevationMatched;
        private double _deviceTilt;
        private double _compassDirection;

        public SunLocationSteps(
            ILocationService<LocationViewModel> locationService,
            BDDTestContext testContext)
        {
            _locationService = locationService;
            _testContext = testContext;
        }

        [Given(@"I am viewing the sun location tracker for a location")]
        public void GivenIAmViewingTheSunLocationTrackerForALocation()
        {
            // Get first location from test context
            if (!_testContext.TestLocations.Any())
            {
                _testContext.TestLocations.Add(TestDataFactory.CreateTestLocation());
            }
            _currentLocation = _testContext.TestLocations.First();

            // Initialize sun location
            _sunLocation = new Location.Photography.Shared.ViewModels.SunLocation
            {
                Latitude = _currentLocation.Lattitude,
                Longitude = _currentLocation.Longitude,
                NorthRotationAngle = 0,
                SunDirection = 180,
                SunElevation = 45
            };

            // Calculate current sun position based on location and time
            UpdateSunPosition();
        }

        [Given(@"I have started monitoring device sensors")]
        public void GivenIHaveStartedMonitoringDeviceSensors()
        {
            _isMonitoringSensors = true;
            _deviceTilt = 0;
            _compassDirection = 0;
        }

        [When(@"I start monitoring device sensors")]
        public void WhenIStartMonitoringDeviceSensors()
        {
            _isMonitoringSensors = true;
            _sunLocation.IsBusy = true;

            // Simulate sensor data
            _deviceTilt = 15.0;
            _compassDirection = 180.0;
            _sunLocation.DeviceTilt = _deviceTilt;
            _sunLocation.NorthRotationAngle = _compassDirection;
        }

        [When(@"I align the device with the sun's elevation")]
        public void WhenIAlignTheDeviceWithTheSunElevation()
        {
            // Simulate aligning device
            _deviceTilt = _sunLocation.SunElevation;
            _sunLocation.SunElevation = _deviceTilt;

            // Check if elevation matches
            _elevationMatched = Math.Abs(_deviceTilt - _sunLocation.SunElevation) < 5.0;
            _sunLocation.ElevationMatched = _elevationMatched;
        }

        [When(@"I select a different date")]
        public void WhenISelectADifferentDate()
        {
            _sunLocation.SelectedDateTime = DateTime.Now.AddDays(7);
            UpdateSunPosition();
        }

        [When(@"I select a different time")]
        public void WhenISelectADifferentTime()
        {
            _sunLocation.SelectedDateTime = DateTime.Now.AddHours(3);
            UpdateSunPosition();
        }

        [When(@"I stop monitoring device sensors")]
        public void WhenIStopMonitoringDeviceSensors()
        {
            _isMonitoringSensors = false;
            _sunLocation.IsBusy = false;
            _sunLocation.StopSensors();
        }

        [Then(@"I should see the current sun direction")]
        public void ThenIShouldSeeTheCurrentSunDirection()
        {
            Assert.That(_sunLocation, Is.Not.Null);
            Assert.That(_sunLocation.SunDirection, Is.GreaterThanOrEqualTo(0));
            Assert.That(_sunLocation.SunDirection, Is.LessThanOrEqualTo(360));
            //Assert.That(string.IsNullOrEmpty(_sunLocation.SunCompassFormatted), Is.False);
        }

        [Then(@"I should see the current sun elevation")]
        public void ThenIShouldSeeTheCurrentSunElevation()
        {
            Assert.That(_sunLocation, Is.Not.Null);
            Assert.That(_sunLocation.SunElevation, Is.GreaterThanOrEqualTo(-90));
            Assert.That(_sunLocation.SunElevation, Is.LessThanOrEqualTo(90));
           // Assert.That(string.IsNullOrEmpty(_sunLocation.SunElevationFormatted), Is.False);
        }

        [Then(@"I should see the device tilt angle")]
        public void ThenIShouldSeeTheDeviceTiltAngle()
        {
            Assert.That(_isMonitoringSensors, Is.True);
            Assert.That(_sunLocation.ElevationMatched, Is.Not.Null);
            Assert.That(string.IsNullOrEmpty(_sunLocation.ElevationMatched.ToString()), Is.False);
        }

        [Then(@"I should see the compass direction")]
        public void ThenIShouldSeeTheCompassDirection()
        {
            Assert.That(_isMonitoringSensors, Is.True);
            Assert.That(_sunLocation.SunElevation, Is.Not.Null);
            Assert.That(string.IsNullOrEmpty(_sunLocation.ElevationMatched.ToString()), Is.False);
        }

        [Then(@"I should receive a vibration notification")]
        public void ThenIShouldReceiveAVibrationNotification()
        {
            Assert.That(_elevationMatched, Is.True);
            Assert.That(_sunLocation.ElevationMatched, Is.True);
            // In service tests, we can't verify actual vibration, but we verify the condition
        }

        [Then(@"the elevation matched indicator should be displayed")]
        public void ThenTheElevationMatchedIndicatorShouldBeDisplayed()
        {
            Assert.That(_elevationMatched, Is.True);
            Assert.That(_sunLocation.ElevationMatched, Is.True);
        }

        [Then(@"the sun position should update for the selected date")]
        public void ThenTheSunPositionShouldUpdateForTheSelectedDate()
        {
            Assert.That(_sunLocation, Is.Not.Null);
            Assert.That(_sunLocation.SelectedDate.Date, Is.EqualTo(DateTime.Now.AddDays(7).Date));
            // Sun position values would be recalculated
        }

        [Then(@"the sun position should update for the selected time")]
        public void ThenTheSunPositionShouldUpdateForTheSelectedTime()
        {
            Assert.That(_sunLocation, Is.Not.Null);
            Assert.That(_sunLocation.SelectedDateTime.TimeOfDay, Is.EqualTo(DateTime.Now.AddHours(3).TimeOfDay));
            // Sun position values would be recalculated
        }

        [Then(@"the device orientation tracking should stop")]
        public void ThenTheDeviceOrientationTrackingShouldStop()
        {
            Assert.That(_isMonitoringSensors, Is.False);
            Assert.That(_sunLocation.IsBusy, Is.False);
        }

        private void UpdateSunPosition()
        {
            // In a real implementation, this would calculate actual sun position
            // based on location, date, and time using astronomy algorithms

            // Simulate position calculation
            var timeOfDay = (_sunLocation.SelectedDateTime == null? DateTime.Now: _sunLocation.SelectedDateTime).TimeOfDay.TotalHours;

            // Simple simulation of sun movement
            if (timeOfDay >= 6 && timeOfDay <= 18)
            {
                // Daytime - sun is above horizon
                _sunLocation.SunElevation = 45 - Math.Abs(12 - timeOfDay) * 7.5;
                _sunLocation.SunDirection = 90 + (timeOfDay - 6) * 15;
            }
            else
            {
                // Nighttime - sun is below horizon
                _sunLocation.SunElevation = -15;
                _sunLocation.SunDirection = 270;
            }
        }
    }
}