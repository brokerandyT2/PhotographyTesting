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
        Unknown,
        Permission,
        Camera,
        AmbientLightSensor,
        LocalStorage,
        SecureStorage,
        Gps,
        FileSystem,
        Network,
        Serialization
    }

    /// <summary>
    /// Event arguments for data errors
    /// </summary>
    public class DataErrorEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the source of the error
        /// </summary>
        public ErrorSource Source { get; }

        /// <summary>
        /// Gets the error message
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Gets the exception that caused the error, if any
        /// </summary>
        public Exception Exception { get; }

        /// <summary>
        /// Creates a new data error event
        /// </summary>
        /// <param name="source">The source of the error</param>
        /// <param name="message">The error message</param>
        /// <param name="exception">The exception that caused the error, if any</param>
        public DataErrorEventArgs(ErrorSource source, string message, Exception exception = null)
        {
            Source = source;
            Message = message ?? "An unknown error occurred";
            Exception = exception;
        }
    }

    /// <summary>
    /// Delegate for data error events
    /// </summary>
    /// <param name="sender">The sender of the event</param>
    /// <param name="e">The event arguments</param>
   // public delegate void DataErrorEventHandler(object sender, DataErrorEventArgs e);
}