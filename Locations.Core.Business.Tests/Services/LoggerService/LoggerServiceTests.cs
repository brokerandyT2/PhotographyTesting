// LoggerServiceTests.cs
using System;
using System.Linq;
using System.Threading.Tasks;
using Location.Core.Helpers.LoggingService;
using Locations.Core.Business.Tests.Base;
using Locations.Core.Shared.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SQLite;

namespace Locations.Core.Business.Tests.Services.LoggerServiceTests
{
    [TestClass]
    [TestCategory("LoggerService")]
    public class LoggerServiceTests : BaseServiceTests
    {
        private LoggerService _loggerService;
        private string _testDbPath = ":memory:"; // Use in-memory database for testing
        private SQLiteAsyncConnection _sqliteConnection;

        [TestInitialize]
        public override void Setup()
        {
             base.Setup();

            try
            {
                // Create a real SQLite connection for in-memory database
                _sqliteConnection = new SQLiteAsyncConnection(_testDbPath);

                // Initialize the tables needed for logging
                var x = _sqliteConnection.CreateTableAsync<Log>().Result;

                // Create the actual LoggerService with the in-memory database
                _loggerService = new LoggerService(_sqliteConnection);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in test setup: {ex.Message}");
                throw;
            }
        }

        [TestCleanup]
        public async Task Cleanup()
        {
            // Close the connection
            if (_sqliteConnection != null)
            {
                await _sqliteConnection.CloseAsync();
            }
        }

        [TestMethod]
        public async Task LogDebug_ShouldWriteLogWithDebugLevel()
        {
            // Act
            _loggerService.LogDebug("Test debug message");

            // Assert - now we can actually verify the data was written
            var logs = await _sqliteConnection.Table<Log>()
                .Where(l => l.Level == "Debug" && l.Message == "Test debug message")
                .ToListAsync();

            Assert.IsTrue(logs.Count() > 0, "Log entry was not created");
        }

        [TestMethod]
        public async Task LogInformation_ShouldWriteLogWithInfoLevel()
        {
            // Act
            _loggerService.LogInformation("Test info message");

            // Assert
            var logs = await _sqliteConnection.Table<Log>()
                .Where(l => l.Level == "Information" && l.Message == "Test info message")
                .ToListAsync();

            Assert.IsTrue(logs.Count() > 0, "Log entry was not created");
        }

        [TestMethod]
        public async Task LogWarning_ShouldWriteLogWithWarningLevel()
        {
            // Act
            _loggerService.LogWarning("Test warning message");

            // Assert
            var logs = await _sqliteConnection.Table<Log>()
                .Where(l => l.Level == "Warning" && l.Message == "Test warning message")
                .ToListAsync();

            Assert.IsTrue(logs.Count() > 0, "Log entry was not created");
        }

        [TestMethod]
        public async Task LogError_WithMessage_ShouldWriteLogWithErrorLevel()
        {
            // Act
            _loggerService.LogError("Test error message");

            // Assert
            var logs = await _sqliteConnection.Table<Log>()
                .Where(l => l.Level == "Error" && l.Message == "Test error message")
                .ToListAsync();

            Assert.IsTrue(logs.Count() > 0, "Log entry was not created");
        }

        [TestMethod]
        public async Task LogError_WithMessageAndException_ShouldWriteLogWithErrorLevelAndException()
        {
            // Arrange
            var testException = new Exception("Test exception");

            // Act
            _loggerService.LogError("Test error message", testException);

            // Assert
            var logs = await _sqliteConnection.Table<Log>()
                .Where(l => l.Level == "Error" && l.Message == "Test error message")
                .ToListAsync();

            Assert.IsTrue(logs.Count() > 0, "Log entry was not created");
            Assert.IsTrue(logs[0].Exception.Contains("Test exception"),
                "Exception details were not properly stored");
        }

        [TestMethod]
        public async Task LogCritical_WithMessage_ShouldWriteLogWithCriticalLevel()
        {
            // Act
            _loggerService.LogCritical("Test critical message");

            // Assert
            var logs = await _sqliteConnection.Table<Log>()
                .Where(l => l.Level == "Critical" && l.Message == "Test critical message")
                .ToListAsync();

            Assert.IsTrue(logs.Count() > 0, "Log entry was not created");
        }

        [TestMethod]
        public async Task LogCritical_WithMessageAndException_ShouldWriteLogWithCriticalLevelAndException()
        {
            // Arrange
            var testException = new Exception("Test exception");

            // Act
            _loggerService.LogCritical("Test critical message", testException);

            // Assert
            var logs = await _sqliteConnection.Table<Log>()
                .Where(l => l.Level == "Critical" && l.Message == "Test critical message")
                .ToListAsync();

            Assert.IsTrue(logs.Count() > 0, "Log entry was not created");
            Assert.IsTrue(logs[0].Exception.Contains("Test exception"),
                "Exception details were not properly stored");
        }

        [TestMethod]
        public async Task GetLogsByLevel_ShouldReturnLogsFilteredByLevel()
        {
           
        }

        [TestMethod]
        public async Task GetLogsByDateRange_ShouldReturnLogsFilteredByDateRange()
        {
           
        }

        [TestMethod]
        public async Task SearchLogs_ShouldReturnLogsContainingSearchText()
        {
           
        }
    }
}