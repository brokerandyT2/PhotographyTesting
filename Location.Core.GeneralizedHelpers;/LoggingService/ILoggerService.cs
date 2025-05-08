using System;

namespace Location.Core.GeneralizedHelpers.LoggingService
{
    /// <summary>
    /// Interface for logging application events and errors
    /// </summary>
    public interface ILoggerService
    {
        /// <summary>
        /// Logs a debug message
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <param name="args">Optional format arguments for the message</param>
        void LogDebug(string message, params object[] args);

        /// <summary>
        /// Logs an informational message
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <param name="args">Optional format arguments for the message</param>
        void LogInformation(string message, params object[] args);

        /// <summary>
        /// Logs a warning message
        /// </summary>
        /// <param name="message">The message to log</param>
        /// <param name="args">Optional format arguments for the message</param>
        void LogWarning(string message, params object[] args);

        /// <summary>
        /// Logs an error message
        /// </summary>
        /// <param name="message">The error message to log</param>
        /// <param name="args">Optional format arguments for the message</param>
        void LogError(string message, params object[] args);

        /// <summary>
        /// Logs an error message with exception details
        /// </summary>
        /// <param name="exception">The exception to log</param>
        /// <param name="message">Optional additional context message</param>
        /// <param name="args">Optional format arguments for the message</param>
        void LogError(Exception exception, string message = null, params object[] args);

        /// <summary>
        /// Logs a critical error message
        /// </summary>
        /// <param name="message">The critical error message to log</param>
        /// <param name="args">Optional format arguments for the message</param>
        void LogCritical(string message, params object[] args);

        /// <summary>
        /// Logs a critical error message with exception details
        /// </summary>
        /// <param name="exception">The exception to log</param>
        /// <param name="message">Optional additional context message</param>
        /// <param name="args">Optional format arguments for the message</param>
        void LogCritical(Exception exception, string message = null, params object[] args);

        /// <summary>
        /// Begins a logical operation scope that can group related logs
        /// </summary>
        /// <typeparam name="TState">The type of the state object</typeparam>
        /// <param name="state">The state object to associate with the scope</param>
        /// <returns>A disposable object that ends the logical operation scope when disposed</returns>
        IDisposable BeginScope<TState>(TState state);
    }
}