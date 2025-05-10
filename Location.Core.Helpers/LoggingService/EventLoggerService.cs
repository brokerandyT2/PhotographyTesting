using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Location.Core.Helpers.LoggingService
{
    /// <summary>
    /// An event-based implementation of the logger service that raises events for log entries
    /// This can be used in cases where database access is not required
    /// </summary>
    public class EventLoggerService : ILoggerService
    {
        /// <summary>
        /// Event raised when an information log is created
        /// </summary>
        public event EventHandler<LoggingEventArgs>? InfoLogged;

        /// <summary>
        /// Event raised when a warning log is created
        /// </summary>
        public event EventHandler<LoggingEventArgs>? WarningLogged;

        /// <summary>
        /// Event raised when an error log is created
        /// </summary>
        public event EventHandler<LoggingEventArgs>? ErrorLogged;

        /// <summary>
        /// Event raised when a debug log is created
        /// </summary>
        public event EventHandler<LoggingEventArgs>? DebugLogged;

        /// <summary>
        /// Logs an information message
        /// </summary>
        public void LogInfo(string message)
        {
            if (string.IsNullOrEmpty(message)) return;

            InfoLogged?.Invoke(this, new LoggingEventArgs(LogLevel.Info, message));
        }

        /// <summary>
        /// Logs a warning message
        /// </summary>
        public void LogWarning(string message, Exception? exception = null)
        {
            if (string.IsNullOrEmpty(message)) return;

            WarningLogged?.Invoke(this, new LoggingEventArgs(
                LogLevel.Warning,
                message,
                exception?.ToString() ?? string.Empty));
        }

        /// <summary>
        /// Logs an error message
        /// </summary>
        public void LogError(string message, Exception? exception = null)
        {
            if (string.IsNullOrEmpty(message)) return;

            ErrorLogged?.Invoke(this, new LoggingEventArgs(
                LogLevel.Error,
                message,
                exception?.ToString() ?? string.Empty));
        }

        /// <summary>
        /// Logs a debug message
        /// </summary>
        public void LogDebug(string message, Exception? exception = null)
        {
            if (string.IsNullOrEmpty(message)) return;

            DebugLogged?.Invoke(this, new LoggingEventArgs(
                LogLevel.Debug,
                message,
                exception?.ToString() ?? string.Empty));
        }

        /// <summary>
        /// Gets logs matching the specified criteria
        /// This implementation returns an empty list as it doesn't store logs
        /// </summary>
        public Task<IEnumerable<LogModel>> GetLogsAsync(
            string? level = null,
            DateTime? startDate = null,
            DateTime? endDate = null,
            int? limit = null)
        {
            // This implementation doesn't store logs, so just return an empty list
            return Task.FromResult<IEnumerable<LogModel>>(new List<LogModel>());
        }

        /// <summary>
        /// Clears all logs
        /// This implementation does nothing as it doesn't store logs
        /// </summary>
        public Task ClearLogsAsync()
        {
            // Nothing to clear as this implementation doesn't store logs
            return Task.CompletedTask;
        }

        public void LogInformation(string message)
        {
            LogInfo(message);
        }

        public void LogCritical(string message, Exception? exception = null)
        {
            throw new NotImplementedException();
        }

        public Task<int> PurgeOldErrorLogsAsync()
        {
            throw new NotImplementedException();
        }
    }
}