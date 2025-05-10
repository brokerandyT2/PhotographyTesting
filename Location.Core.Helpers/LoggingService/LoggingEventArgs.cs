using System;

namespace Location.Core.Helpers.LoggingService
{
    /// <summary>
    /// Event arguments for logging events
    /// </summary>
    public class LoggingEventArgs : EventArgs
    {
        /// <summary>
        /// The log level
        /// </summary>
        public string Level { get; }

        /// <summary>
        /// The log message
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Optional exception information
        /// </summary>
        public string Exception { get; }

        /// <summary>
        /// The timestamp when the log was created
        /// </summary>
        public DateTime Timestamp { get; }

        /// <summary>
        /// Creates new logging event arguments
        /// </summary>
        /// <param name="level">The log level</param>
        /// <param name="message">The log message</param>
        /// <param name="exception">Optional exception information</param>
        public LoggingEventArgs(string level, string message, string exception = "")
        {
            Level = level;
            Message = message;
            Exception = exception;
            Timestamp = DateTime.Now;
        }
    }
}