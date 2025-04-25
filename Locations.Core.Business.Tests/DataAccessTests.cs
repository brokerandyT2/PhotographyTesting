using Microsoft.VisualStudio.TestTools.UnitTesting;
using Locations.Core.Business.DataAccess;
using Locations.Core.Shared.ViewModels;

namespace Locations.Core.Business.Tests
{
    [TestClass]
    public class DataAccessTests
    {
        private Locations.Core.Business.DataAccess.DataAccess _dataAccess;

        [TestInitialize]
        public void Setup()
        {
            _dataAccess = new DataAccess.DataAccess();
        }

        [TestMethod]
        public void Connection_ShouldNotBeNull_WhenInitialized()
        {
            // Act
            var connection = _dataAccess.Connection;

            // Assert
            Assert.IsNotNull(connection);
        }

        [TestMethod]
        public void CheckForDB_ShouldCreateTables_WhenDatabaseIsEmpty()
        {
            // Act
            _dataAccess.Connection.CreateTableAsync<LocationViewModel>().Wait();

            // Assert
            // Verify that tables exist (this would require additional checks or mocks).
        }
    }
}
