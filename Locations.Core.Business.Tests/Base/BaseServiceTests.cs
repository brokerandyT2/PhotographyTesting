// BaseServiceTests.cs - Replacement for the existing class
using Location.Core.Helpers.AlertService;
using Location.Core.Helpers.LoggingService;
using Locations.Core.Data.Models;
using Locations.Core.Shared.ViewModels;
using Moq;
using SQLite;
using System;
using System.Threading.Tasks;
using ErrorSource = Locations.Core.Data.Models.ErrorSource;

namespace Locations.Core.Business.Tests.Base
{
    /// <summary>
    /// Base test class to provide common setup and functionality for service tests
    /// </summary>
    [TestClass]
    public abstract class BaseServiceTests : IDisposable
    {
        // Common mocks that will be used across test classes
        protected Mock<IAlertService> MockAlertService;
        protected Mock<ILoggerService> MockLoggerService;

        // In-memory SQLite database for repository tests
        protected SQLiteAsyncConnection InMemoryDb;
        protected bool DbInitialized = false;

        // Constructor to ensure proper cleanup of resources
        protected BaseServiceTests()
        {
            // Initialize mocks
            MockAlertService = new Mock<IAlertService>();
            MockLoggerService = new Mock<ILoggerService>();

            // Setup common mock behaviors
            MockLoggerService.Setup(l => l.LogInformation(It.IsAny<string>())).Verifiable();
            MockLoggerService.Setup(l => l.LogWarning(It.IsAny<string>(), It.IsAny<Exception>())).Verifiable();
            MockLoggerService.Setup(l => l.LogError(It.IsAny<string>(), It.IsAny<Exception>())).Verifiable();
        }

        [TestInitialize]
        public virtual void Setup()
        {
            // Re-initialize mocks for each test
            MockAlertService = new Mock<IAlertService>();
            MockLoggerService = new Mock<ILoggerService>();

            // Setup common mock behaviors
            MockLoggerService.Setup(l => l.LogInformation(It.IsAny<string>())).Verifiable();
            MockLoggerService.Setup(l => l.LogWarning(It.IsAny<string>(), It.IsAny<Exception>())).Verifiable();
            MockLoggerService.Setup(l => l.LogError(It.IsAny<string>(), It.IsAny<Exception>())).Verifiable();
        }

        /// <summary>
        /// Initialize the in-memory SQLite database with the required tables
        /// </summary>
        protected async Task InitializeInMemoryDatabase()
        {
            if (DbInitialized)
                return;

            // Create a new in-memory database connection
            InMemoryDb = new SQLiteAsyncConnection(":memory:");

            // Create tables for all viewmodel types
            await InMemoryDb.CreateTableAsync<LocationViewModel>();
            await InMemoryDb.CreateTableAsync<SettingViewModel>();
            await InMemoryDb.CreateTableAsync<WeatherViewModel>();
            await InMemoryDb.CreateTableAsync<TipViewModel>();
            await InMemoryDb.CreateTableAsync<TipTypeViewModel>();
            await InMemoryDb.CreateTableAsync<Log>();

            DbInitialized = true;
        }

        /// <summary>
        /// Helper to create a data operation success result
        /// </summary>
        protected DataOperationResult<T> CreateSuccessResult<T>(T data)
        {
            return DataOperationResult<T>.Success(data);
        }

        /// <summary>
        /// Helper to create a data operation failure result
        /// </summary>
        protected DataOperationResult<T> CreateFailureResult<T>(
            ErrorSource source = ErrorSource.Unknown,
            string message = "Test error message",
            Exception exception = null)
        {
            return DataOperationResult<T>.Failure(source, message, exception);
        }

        public void Dispose()
        {
            // Clean up database connection
            if (InMemoryDb != null)
            {
                InMemoryDb.CloseAsync().Wait();
                InMemoryDb = null;
                DbInitialized = false;
            }
        }
    }
}