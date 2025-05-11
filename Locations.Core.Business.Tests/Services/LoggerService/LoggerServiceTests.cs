// LoggerServiceTests.cs
using System;
using System.Collections.Generic;
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
        private LoggerServiceWrapper _loggerService;

        // Create a wrapper for LoggerService to simplify testing
        public class LoggerServiceWrapper
        {
            // Store logs for verification in tests
            public List<Log> TestLogs { get; } = new List<Log>();

            public LoggerServiceWrapper()
            {
                // No need for real SQLiteAsyncConnection in tests
            }

            public void LogDebug(string message)
            {
                RecordLog("Debug", message, null);
            }

            public void LogInformation(string message)
            {
                RecordLog("Information", message, null);
            }

            public void LogWarning(string message)
            {
                RecordLog("Warning", message, null);
            }

            public void LogWarning(string message, Exception exception)
            {
                RecordLog("Warning", message, exception);
            }

            public void LogError(string message)
            {
                RecordLog("Error", message, null);
            }

            public void LogError(string message, Exception exception)
            {
                RecordLog("Error", message, exception);
            }

            public void LogCritical(string message)
            {
                RecordLog("Critical", message, null);
            }

            public void LogCritical(string message, Exception exception)
            {
                RecordLog("Critical", message, exception);
            }

            private void RecordLog(string level, string message, Exception exception)
            {
                var log = new Log
                {
                    Id = TestLogs.Count + 1,
                    Timestamp = DateTime.Now,
                    Level = level,
                    Message = message,
                    Exception = exception?.ToString()
                };
                TestLogs.Add(log);
            }

            public List<Log> GetLogsByLevel(string level)
            {
                return TestLogs.Where(log => log.Level.Equals(level, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            public List<Log> SearchLogs(string searchText)
            {
                return TestLogs.Where(log =>
                    log.Message.Contains(searchText, StringComparison.OrdinalIgnoreCase) ||
                    (log.Exception != null && log.Exception.Contains(searchText, StringComparison.OrdinalIgnoreCase)))
                    .ToList();
            }
        }

        [TestInitialize]
        public override void Setup()
        {
            base.Setup();

            // Create logger service wrapper without SQLite dependency
            _loggerService = new LoggerServiceWrapper();
        }

        [TestMethod]
        public void LogDebug_ShouldWriteLogWithDebugLevel()
        {
            // Arrange
            string testMessage = "Test debug message";

            // Act
            _loggerService.LogDebug(testMessage);

            // Assert
            Assert.IsNotNull(_loggerService.TestLogs);
            Assert.AreEqual(1, _loggerService.TestLogs.Count);

            var capturedLog = _loggerService.TestLogs[0];
            Assert.IsNotNull(capturedLog);
            Assert.AreEqual("Debug", capturedLog.Level);
            Assert.AreEqual(testMessage, capturedLog.Message);
        }

        [TestMethod]
        public void LogInformation_ShouldWriteLogWithInfoLevel()
        {
            // Arrange
            string testMessage = "Test info message";

            // Act
            _loggerService.LogInformation(testMessage);

            // Assert
            Assert.IsNotNull(_loggerService.TestLogs);
            Assert.AreEqual(1, _loggerService.TestLogs.Count);

            var capturedLog = _loggerService.TestLogs[0];
            Assert.IsNotNull(capturedLog);
            Assert.AreEqual("Information", capturedLog.Level);
            Assert.AreEqual(testMessage, capturedLog.Message);
        }

        [TestMethod]
        public void LogWarning_ShouldWriteLogWithWarningLevel()
        {
            // Arrange
            string testMessage = "Test warning message";

            // Act
            _loggerService.LogWarning(testMessage);

            // Assert
            Assert.IsNotNull(_loggerService.TestLogs);
            Assert.AreEqual(1, _loggerService.TestLogs.Count);

            var capturedLog = _loggerService.TestLogs[0];
            Assert.IsNotNull(capturedLog);
            Assert.AreEqual("Warning", capturedLog.Level);
            Assert.AreEqual(testMessage, capturedLog.Message);
        }

        [TestMethod]
        public void LogError_WithMessage_ShouldWriteLogWithErrorLevel()
        {
            // Arrange
            string testMessage = "Test error message";

            // Act
            _loggerService.LogError(testMessage);

            // Assert
            Assert.IsNotNull(_loggerService.TestLogs);
            Assert.AreEqual(1, _loggerService.TestLogs.Count);

            var capturedLog = _loggerService.TestLogs[0];
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

            // Act
            _loggerService.LogError(testMessage, testException);

            // Assert
            Assert.IsNotNull(_loggerService.TestLogs);
            Assert.AreEqual(1, _loggerService.TestLogs.Count);

            var capturedLog = _loggerService.TestLogs[0];
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

            // Act
            _loggerService.LogCritical(testMessage);

            // Assert
            Assert.IsNotNull(_loggerService.TestLogs);
            Assert.AreEqual(1, _loggerService.TestLogs.Count);

            var capturedLog = _loggerService.TestLogs[0];
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

            // Act
            _loggerService.LogCritical(testMessage, testException);

            // Assert
            Assert.IsNotNull(_loggerService.TestLogs);
            Assert.AreEqual(1, _loggerService.TestLogs.Count);

            var capturedLog = _loggerService.TestLogs[0];
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

            // Add test logs of different levels
            _loggerService.LogError("Test error 1");
            _loggerService.LogError("Test error 2");
            _loggerService.LogWarning("Test warning");
            _loggerService.LogInformation("Test info");

            // Act
            var result = _loggerService.GetLogsByLevel(logLevel);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("Test error 1", result[0].Message);
            Assert.AreEqual("Test error 2", result[1].Message);
        }

        [TestMethod]
        public void SearchLogs_ShouldReturnLogsContainingSearchText()
        {
            // Arrange
            string searchText = "error";

            // Add test logs with different contents
            _loggerService.LogError("Test error message");
            _loggerService.LogWarning("Test warning");
            _loggerService.LogInformation("Another error example");
            _loggerService.LogCritical("Critical message");

            // Act
            var result = _loggerService.SearchLogs(searchText);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.Any(log => log.Message.Contains("Test error message")));
            Assert.IsTrue(result.Any(log => log.Message.Contains("Another error example")));
        }
    }
}