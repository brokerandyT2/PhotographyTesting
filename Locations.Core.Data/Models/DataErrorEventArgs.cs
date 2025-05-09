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
        Serialization, 
        Repository,
        Service,
        UI
    }

    /// <summary>
    /// Event arguments for data errors
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
        /// The exception that occurred, if any
        /// </summary>
        public Exception Exception { get; }

        /// <summary>
        /// Creates new error event arguments
        /// </summary>
        /// <param name="source">The source of the error</param>
        /// <param name="message">The error message</param>
        /// <param name="exception">The exception that occurred, if any</param>
        public DataErrorEventArgs(ErrorSource source, string message, Exception exception = null)
        {
            Source = source;
            Message = message;
            Exception = exception;
        }
    }

   
}