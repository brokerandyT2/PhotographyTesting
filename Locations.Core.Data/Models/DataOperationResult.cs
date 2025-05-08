using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locations.Core.Data.Models
{
    /// <summary>
    /// Indicates the source of an error in data operations
    /// </summary>
    public enum ErrorSource
    {
        Database,
        ModelValidation,
        NetworkConnection,
        Permission,
        Unknown
    }

    /// <summary>
    /// Represents the result of a data operation with error handling
    /// </summary>
    /// <typeparam name="T">The type of data returned by the operation</typeparam>
    public class DataOperationResult<T>
    {
        /// <summary>
        /// Whether the operation was successful
        /// </summary>
        public bool IsSuccess { get; private set; }

        /// <summary>
        /// The data returned by the operation (if successful)
        /// </summary>
        public T? Data { get; private set; }

        /// <summary>
        /// The source of the error (if operation failed)
        /// </summary>
        public ErrorSource Source { get; private set; }

        /// <summary>
        /// Descriptive error message (if operation failed)
        /// </summary>
        public string? ErrorMessage { get; private set; }

        /// <summary>
        /// Exception that caused the failure (if applicable)
        /// </summary>
        public Exception? Exception { get; private set; }

        /// <summary>
        /// Creates a successful result with data
        /// </summary>
        public static DataOperationResult<T> Success(T data)
        {
            return new DataOperationResult<T> { IsSuccess = true, Data = data };
        }

        /// <summary>
        /// Creates a failure result with error details
        /// </summary>
        public static DataOperationResult<T> Failure(ErrorSource source, string message, Exception? ex = null)
        {
            return new DataOperationResult<T>
            {
                IsSuccess = false,
                Source = source,
                ErrorMessage = message,
                Exception = ex
            };
        }
    }

    /// <summary>
    /// Event arguments for data operation errors
    /// </summary>
    public class DataErrorEventArgs : EventArgs
    {
        /// <summary>
        /// The source of the error
        /// </summary>
        public ErrorSource Source { get; }

        /// <summary>
        /// Error message
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// The exception that caused the error (if any)
        /// </summary>
        public Exception? Exception { get; }

        /// <summary>
        /// The entity related to the error (if any)
        /// </summary>
        public object? RelatedEntity { get; }

        public DataErrorEventArgs(ErrorSource source, string message, Exception? ex = null, object? entity = null)
        {
            Source = source;
            Message = message;
            Exception = ex;
            RelatedEntity = entity;
        }
    }

    /// <summary>
    /// Delegate for data error events
    /// </summary>
    public delegate void DataErrorEventHandler(object sender, DataErrorEventArgs e);
}