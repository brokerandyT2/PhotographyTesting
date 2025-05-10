using System;

namespace Locations.Core.Shared.ViewModels
{
    public class OperationResult<T>
    {
        public bool IsSuccess { get;  set; }
        public T? Data { get;  set; }
        public string? ErrorMessage { get;  set; }
        public Exception? Exception { get;  set; }
        public OperationErrorSource Source { get;  set; }
        public string Message { get; set; }
        public OperationResult(
            bool isSuccess,
            T? data = default,
            string? errorMessage = null,
            Exception? exception = null,
            OperationErrorSource source = OperationErrorSource.Unknown)
        {
            IsSuccess = isSuccess;
            Data = data;
            ErrorMessage = errorMessage;
            Exception = exception;
            Source = source;
        }

        public static OperationResult<T> Success(T data)
        {
            return new OperationResult<T>(true, data);
        }

        public static OperationResult<T> Failure(
            string errorMessage,
            Exception? exception = null,
            OperationErrorSource source = OperationErrorSource.Unknown)
        {
            return new OperationResult<T>(false, default, errorMessage, exception, source);
        }
    }
}