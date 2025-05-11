using System;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Locations.Core.Shared.ViewModelServices
{
    /// <summary>
    /// Indicates the source of an error in operations
    /// </summary>
    public enum OperationErrorSource
    {
        Database,
        ModelValidation,
        NetworkConnection,
        Permission,
        Unknown
    }

    /// <summary>
    /// Represents the result of an operation with error handling
    /// </summary>
    /// <typeparam name="T">The type of data returned by the operation</typeparam>
    public class OperationResult<T>
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
        public OperationErrorSource Source { get; private set; }

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
        public static OperationResult<T> Success(T data)
        {
            return new OperationResult<T> { IsSuccess = true, Data = data };
        }

        /// <summary>
        /// Creates a failure result with error details
        /// </summary>
        public static OperationResult<T> Failure(OperationErrorSource source, string message, Exception? ex = null)
        {
            return new OperationResult<T>
            {
                IsSuccess = false,
                Source = source,
                ErrorMessage = message,
                Exception = ex
            };
        }

      



    }

    /// <summary>
    /// Event arguments for operation errors
    /// </summary>
    public class OperationErrorEventArgs : EventArgs
    {
        /// <summary>
        /// The source of the error
        /// </summary>
        public OperationErrorSource Source { get; }

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

        public OperationErrorEventArgs(OperationErrorSource source, string message, Exception? ex = null, object? entity = null)
        {
            Source = source;
            Message = message;
            Exception = ex;
            RelatedEntity = entity;
        }
    }

    /// <summary>
    /// Delegate for operation error events
    /// </summary>
    public delegate void OperationErrorEventHandler(object sender, OperationErrorEventArgs e);
}