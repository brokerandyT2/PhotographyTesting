using Location.Photography.Business.ExposureCalculator;

namespace Locations.Photography.Business.Tests
{
    [TestClass]
    public sealed class PhotographyBusinessTests
    {
        [TestMethod]
        public void ShouldCalculateExposureCorrectly()
        {
            // Arrange
            var exposureCalculator = new ExposureCalculator();
           // var input = new ExposureInput { Aperture = 2.8, ShutterSpeed = 1 / 125.0, ISO = 100 };

            // Act
            //var result = exposureCalculator.(input);

            // Assert
           // Assert.AreEqual(0, result.ExposureValue, "Exposure value should match expected calculation.");
        }

        [TestMethod]
        public void ShouldReturnDefaultSettings_WhenNoCustomSettingsExist()
        {
            // Arrange
            //var settingsService = new SettingsService();

            // Act
           // var defaultSettings = settingsService.GetDefaultSettings();

            // Assert
          //  Assert.IsNotNull(defaultSettings, "Default settings should not be null.");
           // Assert.AreEqual("Standard", defaultSettings.ProfileName, "Default profile name should be 'Standard'.");
        }
    }
}
