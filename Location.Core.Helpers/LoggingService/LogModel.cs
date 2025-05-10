using SQLite;
using System;

namespace Location.Core.Helpers.LoggingService
{
    /// <summary>
    /// Represents a log entry in the application
    /// </summary>
    public class LogModel
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public DateTime Timestamp { get; set; }

        public string Level { get; set; }

        public string Message { get; set; }

        public string Exception { get; set; }

        /// <summary>
        /// Default constructor
        /// </summary>
        public LogModel()
        {
            Timestamp = DateTime.Now;
            Level = "Info";
            Message = string.Empty;
            Exception = string.Empty;
        }

        /// <summary>
        /// Creates a log entry with specified values
        /// </summary>
        /// <param name="level">The log level</param>
        /// <param name="message">The log message</param>
        /// <param name="exception">Optional exception details</param>
        public LogModel(string level, string message, string exception = "")
        {
            Timestamp = DateTime.Now;
            Level = level;
            Message = message;
            Exception = exception;
        }
    }
}