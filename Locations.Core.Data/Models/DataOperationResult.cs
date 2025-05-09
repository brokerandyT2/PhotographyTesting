using System;

namespace Locations.Core.Data.Models
{
    /// <summary>
    /// Generic result wrapper for data operations
    /// </summary>
    /// <typeparam name="T">Type of the result data</typeparam>
    public class DataOperationResult<T>
    {
        /// <summary>
        /// Gets whether the operation was successful
        /// </summary>
        public bool IsSuccess { get; }

        /// <summary>
        /// Gets the result data (valid only if IsSuccess is true)
        /// </summary>
        public T Data { get; }

        /// <summary>
        /// Gets the error message (valid only if IsSuccess is false)
        /// </summary>
        public string ErrorMessage { get; }

        /// <summary>
        /// Gets the error source (valid only if IsSuccess is false)
        /// </summary>
        public ErrorSource ErrorSource { get; }

        /// <summary>
        /// Gets the exception that caused the error, if any
        /// </summary>
        public Exception? Exception { get; }

        /// <summary>
        /// Creates a new successful result with data
        /// </summary>
        private DataOperationResult(T data)
        {
            IsSuccess = true;
            Data = data;
            ErrorMessage = string.Empty;
            ErrorSource = ErrorSource.Unknown;
            Exception = null;
        }

        /// <summary>
        /// Creates a new failure result with error information
        /// </summary>
        private DataOperationResult(ErrorSource errorSource, string errorMessage, Exception? exception = null)
        {
            IsSuccess = false;
            Data = default!;
            ErrorMessage = errorMessage;
            ErrorSource = errorSource;
            Exception = exception;
        }

        /// <summary>
        /// Creates a new successful result with data
        /// </summary>
        public static DataOperationResult<T> Success(T data)
        {
            return new DataOperationResult<T>(data);
        }

        /// <summary>
        /// Creates a new failure result with error information
        /// </summary>
        public static DataOperationResult<T> Failure(ErrorSource errorSource, string errorMessage, Exception? exception = null)
        {
            return new DataOperationResult<T>(errorSource, errorMessage, exception);
        }
    }
}