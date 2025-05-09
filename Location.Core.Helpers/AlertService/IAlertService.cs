using System;

namespace Location.Core.Helpers.AlertService
{
    /// <summary>
    /// Service interface for displaying alerts to users
    /// </summary>
    public interface IAlertService
    {
        /// <summary>
        /// Shows an information alert to the user
        /// </summary>
        /// <param name="message">The message to display</param>
        void ShowInfo(string message);

        /// <summary>
        /// Shows a warning alert to the user
        /// </summary>
        /// <param name="message">The message to display</param>
        void ShowWarning(string message);

        /// <summary>
        /// Shows an error alert to the user
        /// </summary>
        /// <param name="message">The message to display</param>
        void ShowError(string message);
    }
}

namespace Location.Core.Helpers.LoggingService
{
    /// <summary>
    /// Service interface for logging messages
    /// </summary>
    public interface ILoggerService
    {
        /// <summary>
        /// Logs an information message
        /// </summary>
        /// <param name="message">The message to log</param>
        void LogInfo(string message);

        /// <summary>
        /// Logs a warning message
        /// </summary>
        /// <param name="message">The message to log</param>
        void LogWarning(string message);

        /// <summary>
        /// Logs an error message
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <param name="exception">The exception to log, if any</param>
        void LogError(string message, Exception exception = null);

        /// <summary>
        /// Logs a debug message
        /// </summary>
        /// <param name="message">The message to log</param>
        void LogDebug(string message);
    }
}