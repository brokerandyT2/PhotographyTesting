// LoggerServiceTests.cs
using System;
using System.Linq;
using System.Threading.Tasks;
using Location.Core.Helpers.LoggingService;
using Locations.Core.Business.Tests.Base;
using Locations.Core.Shared.ViewModels;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SQLite;

namespace Locations.Core.Business.Tests.Services.LoggerServiceTests
{
    [TestClass]
    [TestCategory("LoggerService")]
    public class LoggerServiceTests : BaseServiceTests
    {
        private Mock<SQLiteAsyncConnection> _mockSqliteConnection;
        private LoggerService _loggerService;

        [TestInitialize]
        public override void Setup()
        {
            base.Setup();

            // Create a mock of SQLiteAsyncConnection
            _mockSqliteConnection = new Mock<SQLiteAsyncConnection>();

            // Setup basic functionality for the mock
            _mockSqliteConnection.Setup(c => c.CreateTableAsync<Log>())
                .ReturnsAsync(CreateInfo.CreateResult.Created);

            // Create LoggerService with the mock
            _loggerService = new LoggerService(_mockSqliteConnection.Object);
        }

        [TestMethod]
        public void LogDebug_ShouldWriteLogWithDebugLevel()
        {
            // Arrange
            string testMessage = "Test debug message";
            Log capturedLog = null;

            _mockSqliteConnection.Setup(c => c.InsertAsync(It.IsAny<Log>()))
                .Callback<Log>(log => capturedLog = log)
                .ReturnsAsync(1);

            // Act
            _loggerService.LogDebug(testMessage);

            // Assert
            Assert.IsNotNull(capturedLog);
            Assert.AreEqual("Debug", capturedLog.Level);
            Assert.AreEqual(testMessage, capturedLog.Message);
        }

        [TestMethod]
        public void LogInformation_ShouldWriteLogWithInfoLevel()
        {
            // Arrange
            string testMessage = "Test info message";
            Log capturedLog = null;

            _mockSqliteConnection.Setup(c => c.InsertAsync(It.IsAny<Log>()))
                .Callback<Log>(log => capturedLog = log)
                .ReturnsAsync(1);

            // Act
            _loggerService.LogInformation(testMessage);

            // Assert
            Assert.IsNotNull(capturedLog);
            Assert.AreEqual("Information", capturedLog.Level);
            Assert.AreEqual(testMessage, capturedLog.Message);
        }

        [TestMethod]
        public void LogWarning_ShouldWriteLogWithWarningLevel()
        {
            // Arrange
            string testMessage = "Test warning message";
            Log capturedLog = null;

            _mockSqliteConnection.Setup(c => c.InsertAsync(It.IsAny<Log>()))
                .Callback<Log>(log => capturedLog = log)
                .ReturnsAsync(1);

            // Act
            _loggerService.LogWarning(testMessage);

            // Assert
            Assert.IsNotNull(capturedLog);
            Assert.AreEqual("Warning", capturedLog.Level);
            Assert.AreEqual(testMessage, capturedLog.Message);
        }

        [TestMethod]
        public void LogError_WithMessage_ShouldWriteLogWithErrorLevel()
        {
            // Arrange
            string testMessage = "Test error message";
            Log capturedLog = null;

            _mockSqliteConnection.Setup(c => c.InsertAsync(It.IsAny<Log>()))
                .Callback<Log>(log => capturedLog = log)
                .ReturnsAsync(1);

            // Act
            _loggerService.LogError(testMessage);

            // Assert
            Assert.IsNotNull(capturedLog);
            Assert.AreEqual("Error", capturedLog.Level);
            Assert.AreEqual(testMessage, capturedLog.Message);
        }

        [TestMethod]
        public void LogError_WithMessageAndException_ShouldWriteLogWithErrorLevelAndException()
        {
            // Arrange
            string testMessage = "Test error message";
            var testException = new Exception("Test exception");
            Log capturedLog = null;

            _mockSqliteConnection.Setup(c => c.InsertAsync(It.IsAny<Log>()))
                .Callback<Log>(log => capturedLog = log)
                .ReturnsAsync(1);

            // Act
            _loggerService.LogError(testMessage, testException);

            // Assert
            Assert.IsNotNull(capturedLog);
            Assert.AreEqual("Error", capturedLog.Level);
            Assert.AreEqual(testMessage, capturedLog.Message);
            Assert.IsTrue(capturedLog.Exception.Contains("Test exception"));
        }

        [TestMethod]
        public void LogCritical_WithMessage_ShouldWriteLogWithCriticalLevel()
        {
            // Arrange
            string testMessage = "Test critical message";
            Log capturedLog = null;

            _mockSqliteConnection.Setup(c => c.InsertAsync(It.IsAny<Log>()))
                .Callback<Log>(log => capturedLog = log)
                .ReturnsAsync(1);

            // Act
            _loggerService.LogCritical(testMessage);

            // Assert
            Assert.IsNotNull(capturedLog);
            Assert.AreEqual("Critical", capturedLog.Level);
            Assert.AreEqual(testMessage, capturedLog.Message);
        }

        [TestMethod]
        public void LogCritical_WithMessageAndException_ShouldWriteLogWithCriticalLevelAndException()
        {
            // Arrange
            string testMessage = "Test critical message";
            var testException = new Exception("Test exception");
            Log capturedLog = null;

            _mockSqliteConnection.Setup(c => c.InsertAsync(It.IsAny<Log>()))
                .Callback<Log>(log => capturedLog = log)
                .ReturnsAsync(1);

            // Act
            _loggerService.LogCritical(testMessage, testException);

            // Assert
            Assert.IsNotNull(capturedLog);
            Assert.AreEqual("Critical", capturedLog.Level);
            Assert.AreEqual(testMessage, capturedLog.Message);
            Assert.IsTrue(capturedLog.Exception.Contains("Test exception"));
        }

        [TestMethod]
        public void GetLogsByLevel_ShouldReturnLogsFilteredByLevel()
        {
            // Arrange
            string logLevel = "Error";
            var testLogs = new List<Log>
            {
                new Log { Id = 1, Level = "Error", Message = "Test error 1" },
                new Log { Id = 2, Level = "Error", Message = "Test error 2" },
                new Log { Id = 3, Level = "Warning", Message = "Test warning" }
            };

            _mockSqliteConnection.Setup(c => c.Table<Log>())
                .Returns(new AsyncTableQuery<Log>(_mockSqliteConnection.Object, () => testLogs.Where(l => l.Level == logLevel)));

            // Act
            var result = _loggerService.GetLogsByLevel(logLevel);

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("Test error 1", result[0].Message);
        }

        [TestMethod]
        public void SearchLogs_ShouldReturnLogsContainingSearchText()
        {
            // Arrange
            string searchText = "error";
            var testLogs = new List<Log>
            {
                new Log { Id = 1, Level = "Error", Message = "Test error message" },
                new Log { Id = 2, Level = "Warning", Message = "Test warning" },
                new Log { Id = 3, Level = "Info", Message = "Another error example" }
            };

            _mockSqliteConnection.Setup(c => c.Table<Log>())
                .Returns(new AsyncTableQuery<Log>(_mockSqliteConnection.Object,
                    () => testLogs.Where(l => l.Message.Contains(searchText, StringComparison.OrdinalIgnoreCase))));

            // Act
            var result = _loggerService.SearchLogs(searchText);

            // Assert
            Assert.AreEqual(2, result.Count);
        }
    }
}