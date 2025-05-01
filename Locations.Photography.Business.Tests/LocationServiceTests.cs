using Microsoft.VisualStudio.TestTools.UnitTesting;
using Locations.Core.Business.DataAccess;
using Locations.Core.Shared.ViewModels;

namespace Locations.Core.Business.Tests
{
    [TestClass]
    public class LocationsServiceTests
    {
        private LocationsService _service;

        [TestInitialize]
        public void Setup()
        {
            _service = new LocationsService();
        }

        [TestMethod]
        public void Save_ShouldReturnNewLocation_WhenReturnNewIsTrue()
        {
            // Arrange
            var location = new LocationViewModel { Lattitude = 10.0, Longitude = 20.0 };

            // Act
            var result = _service.Save(location, returnNew: true);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreNotEqual(location, result);
        }

        [TestMethod]
        public void Get_ShouldReturnLocation_WhenIdIsValid()
        {
            // Arrange
            int validId = 1;

            // Act
            var result = _service.Get(validId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(validId, result.Id);
        }

        [TestMethod]
        public void Delete_ShouldReturnFalse_WhenLocationDoesNotExist()
        {
            // Arrange
            int invalidId = -1;

            // Act
            var result = _service.Delete(invalidId);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void GetAll_ShouldReturnAllLocations()
        {
            // Act
            var result = _service.GetLocations();

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(result.Count > 0, "There should be at least one location.");
        }

        [TestMethod]
        public void Update_ShouldModifyExistingLocation()
        {
            // Arrange
            var location = new LocationViewModel { Id = 1, Lattitude = 15.0, Longitude = 25.0 };

            // Act
            var result = _service.Update(location);

            // Assert
            Assert.IsTrue(result, "Update should return true for a valid location.");
        }
    }
}
