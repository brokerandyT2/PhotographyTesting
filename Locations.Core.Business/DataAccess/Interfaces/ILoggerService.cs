using Locations.Core.Shared.ViewModels;
using System;
using System.Collections.Generic;

namespace Locations.Core.Business.DataAccess.Interfaces
{
    /// <summary>
    /// Interface for logging service that provides methods to log messages and retrieve logs
    /// </summary>
    public interface ILoggerService
    {
        /// <summary>
        /// Logs a debug message
        /// </summary>
        /// <param name="message">The message to log</param>
        void LogDebug(string message);

        /// <summary>
        /// Logs an informational message
        /// </summary>
        /// <param name="message">The message to log</param>
        void LogInformation(string message);

        /// <summary>
        /// Logs a warning message
        /// </summary>
        /// <param name="message">The message to log</param>
        void LogWarning(string message);

        /// <summary>
        /// Logs an error message
        /// </summary>
        /// <param name="message">The error message to log</param>
        void LogError(string message);

        /// <summary>
        /// Logs an error message with exception details
        /// </summary>
        /// <param name="message">The error message</param>
        /// <param name="exception">The exception to log</param>
        void LogError(string message, Exception exception);

        /// <summary>
        /// Logs a critical error message
        /// </summary>
        /// <param name="message">The critical error message to log</param>
        void LogCritical(string message);

        /// <summary>
        /// Logs a critical error message with exception details
        /// </summary>
        /// <param name="message">The critical error message</param>
        /// <param name="exception">The exception to log</param>
        void LogCritical(string message, Exception exception);

        /// <summary>
        /// Filters logs by severity level
        /// </summary>
        /// <param name="level">The log level to filter by</param>
        /// <param name="pageSize">Maximum number of logs to return</param>
        /// <param name="pageNumber">Page number for pagination</param>
        /// <returns>Collection of log entries matching the criteria</returns>
        IEnumerable<Log> GetLogsByLevel(string level, int pageSize = 100, int pageNumber = 1);

        /// <summary>
        /// Filters logs by date range
        /// </summary>
        /// <param name="startDate">The start date (inclusive)</param>
        /// <param name="endDate">The end date (inclusive)</param>
        /// <param name="pageSize">Maximum number of logs to return</param>
        /// <param name="pageNumber">Page number for pagination</param>
        /// <returns>Collection of log entries matching the criteria</returns>
        IEnumerable<Log> GetLogsByDateRange(DateTime startDate, DateTime endDate, int pageSize = 100, int pageNumber = 1);

        /// <summary>
        /// Searches logs by message content
        /// </summary>
        /// <param name="searchText">Text to search for in log messages</param>
        /// <param name="pageSize">Maximum number of logs to return</param>
        /// <param name="pageNumber">Page number for pagination</param>
        /// <returns>Collection of log entries matching the criteria</returns>
        IEnumerable<Log> SearchLogs(string searchText, int pageSize = 100, int pageNumber = 1);

        /// <summary>
        /// Retrieves logs with pagination
        /// </summary>
        /// <param name="pageSize">Maximum number of logs to return</param>
        /// <param name="pageNumber">Page number for pagination</param>
        /// <returns>Collection of log entries for the specified page</returns>
        IEnumerable<Log> GetLogs(int pageSize = 100, int pageNumber = 1);

        /// <summary>
        /// Clears logs older than the specified date
        /// </summary>
        /// <param name="olderThan">Date threshold for deletion</param>
        /// <returns>Number of logs deleted</returns>
        int ClearLogsOlderThan(DateTime olderThan);

        /// <summary>
        /// Gets the total count of log entries
        /// </summary>
        /// <returns>Total number of log entries</returns>
        int GetLogCount();

        /// <summary>
        /// Gets the count of log entries with the specified level
        /// </summary>
        /// <param name="level">Log level to count</param>
        /// <returns>Number of log entries with the specified level</returns>
        int GetLogCountByLevel(string level);

        /// <summary>
        /// Event that is raised when an error occurs in the logger service
        /// </summary>
        event EventHandler<OperationErrorEventArgs> ErrorOccurred;
    }
}