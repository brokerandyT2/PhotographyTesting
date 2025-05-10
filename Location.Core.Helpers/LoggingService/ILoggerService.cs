using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Location.Core.Helpers.LoggingService
{
    /// <summary>
    /// Interface for logging service
    /// </summary>
    public interface ILoggerService
    {
        /// <summary>
        /// Logs an information message
        /// </summary>
        /// <param name="message">The message to log</param>
        void LogInformation(string message);
        void LogCritical(string message, Exception? exception = null);
        /// <summary>
        /// Logs a warning message
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <param name="exception">Optional exception related to the warning</param>
        void LogWarning(string message, Exception? exception = null);

        /// <summary>
        /// Logs an error message
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <param name="exception">Optional exception related to the error</param>
        void LogError(string message, Exception? exception = null);

        /// <summary>
        /// Logs a debug message
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <param name="exception">Optional exception related to the debug message</param>
        void LogDebug(string message, Exception? exception = null);

        /// <summary>
        /// Gets logs matching the specified criteria
        /// </summary>
        /// <param name="level">Optional log level filter</param>
        /// <param name="startDate">Optional start date filter</param>
        /// <param name="endDate">Optional end date filter</param>
        /// <param name="limit">Optional limit on the number of logs returned</param>
        /// <returns>Collection of logs matching the criteria</returns>
        Task<IEnumerable<LogModel>> GetLogsAsync(
            string? level = null,
            DateTime? startDate = null,
            DateTime? endDate = null,
            int? limit = null);

        /// <summary>
        /// Clears all logs from the database
        /// </summary>
        Task ClearLogsAsync();
        Task<int> PurgeOldErrorLogsAsync();
    }
}