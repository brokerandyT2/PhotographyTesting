using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locations.Core.Business.DataAccess
{
    public enum ErrorSource
    {
        Database,
        ModelValidation,
        NetworkConnection,
        Permission,
        Unknown
    }

    public class DataOperationResult<T>
    {
        public bool IsSuccess { get; private set; }
        public T? Data { get; private set; }
        public ErrorSource Source { get; private set; }
        public string? ErrorMessage { get; private set; }
        public Exception? Exception { get; private set; }

        // Success factory
        public static DataOperationResult<T> Success(T data)
        {
            return new DataOperationResult<T> { IsSuccess = true, Data = data };
        }

        // Failure factory
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
}
