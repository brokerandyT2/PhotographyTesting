using Microsoft.VisualStudio.TestTools.UnitTesting;
using Location.Photography.Business.DataAccess;
using Locations.Core.Shared.ViewModels;
using System;

namespace Location.Photography.Business.Tests
{
    [TestClass]
    public class LocationServiceTests
    {
        private LocationService _service;

        [TestInitialize]
        public void Setup()
        {
            _service = new LocationService();
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void Delete_ShouldThrowNotImplementedException_WhenCalledWithModel()
        {
            // Arrange
            var model = new LocationViewModel();

            // Act
            _service.Delete(model);
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void Delete_ShouldThrowNotImplementedException_WhenCalledWithId()
        {
            // Arrange
            int id = 1;

            // Act
            _service.Delete(id);
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void Delete_ShouldThrowNotImplementedException_WhenCalledWithCoordinates()
        {
            // Arrange
            double latitude = 10.0;
            double longitude = 20.0;

            // Act
            _service.Delete(latitude, longitude);
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void Save_ShouldThrowNotImplementedException_WhenCalledWithModel()
        {
            // Arrange
            var model = new LocationViewModel();

            // Act
            _service.Save(model);
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void Save_ShouldThrowNotImplementedException_WhenCalledWithModelAndReturnNew()
        {
            // Arrange
            var model = new LocationViewModel();

            // Act
            _service.Save(model, returnNew: true);
        }

        [TestMethod]
        [ExpectedException(typeof(NotImplementedException))]
        public void Get_ShouldThrowNotImplementedException_WhenCalledWithId()
        {
            // Arrange
            int id = 1;

            // Act
            _service.Get(id);
        }
    }
}
