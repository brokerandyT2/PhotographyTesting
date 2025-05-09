using System;

namespace Locations.Core.Data.Models
{
    /// <summary>
    /// Delegate for data error events
    /// </summary>
    public delegate void DataErrorEventHandler(object sender, DataErrorEventArgs e);

    /// <summary>
    /// Error source enumeration
    /// </summary>
    public enum ErrorSource
    {
        Database,
        ModelValidation,
        NotSupported,
        Unknown
    }

    /// <summary>
    /// Event arguments for data errors
    /// </summary>
    public class DataErrorEventArgs : EventArgs
    {
        /// <summary>
        /// Source of the error
        /// </summary>
        public ErrorSource Source { get; }

        /// <summary>
        /// Error message
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Original exception, if any
        /// </summary>
        public Exception? Exception { get; }

        /// <summary>
        /// Related data object, if any
        /// </summary>
        public object? Data { get; }

        /// <summary>
        /// Creates a new instance of the DataErrorEventArgs class
        /// </summary>
        public DataErrorEventArgs(ErrorSource source, string message, Exception? exception = null, object? data = null)
        {
            Source = source;
            Message = message;
            Exception = exception;
            Data = data;
        }
    }
}