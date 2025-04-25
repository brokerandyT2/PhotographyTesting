using Microsoft.VisualStudio.TestTools.UnitTesting;
using Location.Photography.Business.DataAccess;

namespace Location.Photography.Business.Tests
{
    [TestClass]
    public class LocationTests
    {
        private Location _location;

        [TestInitialize]
        public void Setup()
        {
            _location = new Location();
        }

        [TestMethod]
        public void Location_ShouldInheritFromLocationsService()
        {
            // Assert
            Assert.IsInstanceOfType(_location, typeof(Locations.Core.Business.DataAccess.LocationsService));
        }
    }
}
