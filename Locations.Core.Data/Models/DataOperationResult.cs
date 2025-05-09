using System;

namespace Locations.Core.Data.Models
{
    /// <summary>
    /// Generic result object for data operations
    /// </summary>
    /// <typeparam name="T">The type of data contained in the result</typeparam>
    public class DataOperationResult<T>
    {
        /// <summary>
        /// Gets a value indicating whether the operation was successful
        /// </summary>
        public bool IsSuccess { get; }

        /// <summary>
        /// Gets the error message if the operation failed
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Gets the data returned by the operation
        /// </summary>
        public T Data { get; }

        /// <summary>
        /// Gets the source of any error that occurred
        /// </summary>
        public ErrorSource ErrorSource { get; }

        /// <summary>
        /// Gets the exception that caused the operation to fail, if any
        /// </summary>
        public Exception Exception { get; }

        /// <summary>
        /// Creates a successful operation result
        /// </summary>
        /// <param name="data">The data returned by the operation</param>
        private DataOperationResult(T data)
        {
            IsSuccess = true;
            Data = data;
            Message = string.Empty;
            ErrorSource = ErrorSource.Unknown;
            Exception = null;
        }

        /// <summary>
        /// Creates a failed operation result
        /// </summary>
        /// <param name="errorSource">The source of the error</param>
        /// <param name="message">The error message</param>
        /// <param name="exception">The exception that caused the failure, if any</param>
        private DataOperationResult(ErrorSource errorSource, string message, Exception exception)
        {
            IsSuccess = false;
            Data = default;
            Message = message ?? "An unknown error occurred";
            ErrorSource = errorSource;
            Exception = exception;
        }

        /// <summary>
        /// Creates a successful operation result
        /// </summary>
        /// <param name="data">The data returned by the operation</param>
        /// <returns>A successful operation result</returns>
        public static DataOperationResult<T> Success(T data)
        {
            return new DataOperationResult<T>(data);
        }

        /// <summary>
        /// Creates a failed operation result
        /// </summary>
        /// <param name="errorSource">The source of the error</param>
        /// <param name="message">The error message</param>
        /// <param name="exception">The exception that caused the failure, if any</param>
        /// <returns>A failed operation result</returns>
        public static DataOperationResult<T> Failure(ErrorSource errorSource, string message, Exception exception = null)
        {
            return new DataOperationResult<T>(errorSource, message, exception);
        }
    }
}