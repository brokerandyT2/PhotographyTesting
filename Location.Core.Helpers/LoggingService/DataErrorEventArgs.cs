using System;

namespace Location.Core.Helpers.LoggingService
{
    /// <summary>
    /// Enum defining sources of errors in the application
    /// </summary>
    public enum ErrorSource
    {
        /// <summary>
        /// Error occurred in the database
        /// </summary>
        Database,

        /// <summary>
        /// Error occurred during model validation
        /// </summary>
        ModelValidation,

        /// <summary>
        /// Error occurred during network operations
        /// </summary>
        Network,

        /// <summary>
        /// Error with unknown source
        /// </summary>
        Unknown
    }

    /// <summary>
    /// Event arguments for data error events
    /// </summary>
    public class DataErrorEventArgs : EventArgs
    {
        /// <summary>
        /// The source of the error
        /// </summary>
        public ErrorSource Source { get; }

        /// <summary>
        /// The error message
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Optional exception associated with the error
        /// </summary>
        public Exception? Exception { get; }

        /// <summary>
        /// Optional entity related to the error
        /// </summary>
        public object? RelatedEntity { get; }

        /// <summary>
        /// Creates new data error event arguments
        /// </summary>
        /// <param name="source">The source of the error</param>
        /// <param name="message">The error message</param>
        /// <param name="ex">Optional exception related to the error</param>
        /// <param name="entity">Optional entity related to the error</param>
        public DataErrorEventArgs(ErrorSource source, string message, Exception? ex = null, object? entity = null)
        {
            Source = source;
            Message = message;
            Exception = ex;
            RelatedEntity = entity;
        }
    }
}